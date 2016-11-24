using System;
using System.Configuration;
using System.Net.Mime;
using log4net;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Processing.Contracts;
using Ticket.Messaging;
using Ticket.Messaging.Contracts;

namespace Ticket.AsyncEnterpriseReports.Processing.Activities
{
    public sealed class QueueResponseSender : IResponseSender
    {
        private static readonly object SyncRoot = new object();
        private static QueueResponseSender _instance;
        private static IProducer _producer;
        private QueueSection _queueConfig;
        private static ILog _log = Logger.GetLogger("QueueResponseSender");
        private static readonly string _appId = "AsyncEnterpriseReports";


        private static readonly ContentType _contentType = new ContentType("xml/application;charset=utf-8");
        private  QueueSenderConfig _senderConfig;

        public static QueueResponseSender GetInstance(QueueSenderConfig config)
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new QueueResponseSender(config);

                    }
                }
                
            }

            return _instance;
        }

        private QueueResponseSender(QueueSenderConfig config)
        {

            this._senderConfig = config;
            _producer = Helper.Instance<IProducer>(config.sectionName);
        }

        public void Send(ReportResponse responseInfo)
        {
            var message = Prepare(responseInfo);
            _producer.TxPublish(message);

        }
        public void Send(Message message)
        {

            _producer.TxPublish(message);

        }


        public void Configure(QueueSenderConfig config)
        {
            var configSectionName = config.sectionName;
            _queueConfig = Helper.QueueConfig(configSectionName);
        }

        private void InstantiateProducer()
        {
            _log.Debug("Creating Message Producer");

            if (_producer == null)
            {
                _producer = Helper.Instance<IProducer>(_queueConfig);
                _log.Debug("Static Producer created");
            }
            else
            {
                _log.Debug("Static Producer already created.");
            }

        }

        private Message Prepare(ReportResponse responseInfo)
        {

            var content = Helpers.Serializer.ToXML<ReportResponse>(responseInfo);
            var message = new Message(content, _contentType);
            message.CorrelationId = responseInfo.RequestData.CorrelationID;
            message.DeliveryType = DeliveryType.Persistent;
            message.ProducerId = _appId;
            message.RoutingKey = responseInfo.RequestData.ReplyTo;
            return message;

        }


        private Message Prepare(BadPayloadMessage badNewsMessage)
        {
            var content = Helpers.Serializer.ToXML<BadPayloadMessage>(badNewsMessage);
            var message = new Message(content, _contentType);
            message.CorrelationId = Guid.NewGuid().ToString("N");
            message.DeliveryType = DeliveryType.Persistent;
            message.ProducerId = _appId;
            message.RoutingKey = "badpayload";
            return message;

        }

        public void Send(BadPayloadMessage responseInfo)
        {
            var message = Prepare(responseInfo);
            _producer.TxPublish(message);
        }
    }
}
