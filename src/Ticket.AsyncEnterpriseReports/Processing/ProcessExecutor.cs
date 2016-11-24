using System;
using System.Threading.Tasks;
using log4net;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Exceptions;
using Ticket.AsyncEnterpriseReports.Factories;
using Ticket.AsyncEnterpriseReports.Processing.Contracts;
using Ticket.Messaging;
using Ticket.Messaging.Contracts;

namespace Ticket.AsyncEnterpriseReports.Processing
{
    public class ProcessExecutor : IProcessExecutor
    {
        private string _consumerSectionName;
        private IConsumer _consumer;
        private int? _processId;
        private static ILog _log;

        public ProcessExecutor(ProcessExecutorConfig config, IProcessManager manager)
        {
            _consumerSectionName = config.RequestSourceConfigSectionName;
            _processId = Task.CurrentId;
            _log = Logger.GetLogger(SetLoggerName());

            CreateConsumer();

            _log.DebugFormat("ProcessExecutor created on Task {0}", Task.CurrentId);

            SubscribeToManagerEvents(manager);
        }

        private string SetLoggerName()
        {
            return string.Format("{0}_ProcessExecutor", Task.CurrentId);

        }
        private void SubscribeToManagerEvents(IProcessManager manager)
        {
            manager.OnRunning += Run;
            manager.OnStopping += End;
            manager.OnSuspending += Interrupt;
            
        }

     

        public void Begin()
        {
            _consumer.Start();
            _log.DebugFormat("{0} Begin",_processId);

        }

        public void Run()
        {
            _consumer.Start();
            _log.DebugFormat("{0} Run", _processId);
        }

        public void Interrupt()
        {
            
            _consumer.Stop();
            _log.DebugFormat("{0} Interrupt", _processId);
        }

        public void End()
        {
           
            _consumer.Stop();
            _log.DebugFormat("{0} End", _processId);
        }


        private void CreateConsumer()
        {
            _log.DebugFormat("{0} Creating Message Consumer for {1}", _processId, _consumerSectionName);
            var requestConfig = Helper.QueueConfig(_consumerSectionName);
            _consumer = Helper.Instance<IConsumer>(requestConfig);

            _log.DebugFormat("{0} Consumer created", _processId);
            _consumer.MessageReceivedEvent +=MessageReceivedEvent;
        }

        
        private void MessageReceivedEvent(object sender, EventArgs e)
        {
            IResponseSender responseSender = null;
            ReceivedMessage message = null;
            string rawMessageContent = String.Empty;
            
            try
            {
                message = (e as MessageEventArgs).Message;

                _log.DebugFormat("{0} Message Received: {1}", _processId, message);

                rawMessageContent = Helper.ConvertToTextBasedOnEncoding(message.Content, message.MessageContentType);

                _log.DebugFormat("{0} ReportRequest converted to text: {1}", _processId, rawMessageContent);

                object reportRequest = Activities.RequestHandler.Deserialize(rawMessageContent);

                ValidadeRequestData(reportRequest as ReportRequest, rawMessageContent);

                _log.DebugFormat("{0} ReportRequest deserialized", _processId);

                var facade = FacadeFactory.Create(reportRequest as ReportRequest);

                var data = facade.GetIt(reportRequest as ReportRequest);

                responseSender = ResponseSenderFactory.Create(reportRequest as ReportRequest);

                if (data == null)
                {
                    // send 204 no content
                    var response = CreateResponseForNoContentReport(reportRequest as ReportRequest);
                    responseSender.Send(response);
                    return;

                }
              
                
                var writer = ReportWriterFactory.Create(reportRequest as ReportRequest);

                //any time the writer finish creating the report, we automatically will send the status to the client
                writer.ReportCreated += responseSender.Send;

                writer.Write(data, reportRequest as ReportRequest);

                _log.DebugFormat("{0} MessageEventArgs", _processId);
            }
            catch (BadPayloadException ex)
            {
                //in this case, it is not possible to reply to the sender, since the payload has problems
                //so we will send a message to the async reports exception queue using an 
                //appropriate routing key
                _log.Error(ex.BadPayloadMessage.Status.Message);
                _log.Error(ex.BadPayloadMessage.Status.Detail);
                _log.Error(ex.BadPayloadMessage.RawRequestData);


                var queueSender = ResponseSenderFactory.Create(ResponseProvider.MessageQueue);
                    queueSender.Send(ex.BadPayloadMessage);
              
            }
  
            catch (RequestException ex)
            {
                _log.Error(ex.ResponseData.Status.Detail, ex);
                if (responseSender != null)
                    responseSender.Send(ex.ResponseData);



            }
            catch (Exception ex)
            {

                _log.Error("Unknown error", ex);
                var queueSender = ResponseSenderFactory.Create(ResponseProvider.MessageQueue);
               
                var statusData = new ComplexTypes.ResponseStatus();
                statusData.Detail = Helpers.Response.AggregateExceptionMessages(ex);
                statusData.Message = "Unknown Error";
                statusData.StatusCode = StatusCode._500_Internal_Server_Error;

                var badPayloadMessage = new BadPayloadMessage()
                {
                    RawRequestData = rawMessageContent,
                    Status = statusData
                };
                
                
                queueSender.Send(badPayloadMessage);

                throw ex;

            }
        }

        private ReportResponse CreateResponseForNoContentReport(ReportRequest request)
        {
            var response = new ReportResponse();
            response.RequestData = request;
            response.Status = new ResponseStatus() { StatusCode = StatusCode._204_No_Content };

            return response;

        }

        private void ValidadeRequestData(ReportRequest request, string rawMessageContent)
        {
            
            if(String.IsNullOrEmpty(request.ReplyTo))
            {
                var message = "ReplyTo not informed";
                _log.Warn(message);
                throw new BadPayloadException(message, null, rawMessageContent, StatusCode._412_Pre_Condition_Failed);

            }



        }

      
    }
}
