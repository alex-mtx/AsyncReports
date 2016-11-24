using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Processing.Contracts;
using Ticket.AsyncEnterpriseReports.Repositories.Contracts;

namespace Ticket.AsyncEnterpriseReports.Processing.Activities
{
    public class XMLReportWriter : IReportWriter
    {
        private static ILog _log = Logger.GetLogger("XMLReportWriter");


        public XMLReportWriter(IFileRepository repository)
        {

            this._repository = repository;
        }

        
        public void Write(object data,ReportRequest info)
        {
            var fileName = ConfigureFileName(info);

            _log.DebugFormat("Writing file '{0}'", fileName);

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();



            var ser = new XmlSerializer(data.GetType());

            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream))
            {
                ser.Serialize(writer, data);

            }
            _log.Debug("Sending file");
            _repository.Write(stream.ToArray(),fileName);

            _log.InfoFormat("Wrote '{0}' in {1}ms", fileName, timer.ElapsedMilliseconds);

            timer.Stop();

            _log.Debug("Notifying listeners");
            
            timer.Restart();
            
            NotifyListeners(info, fileName);

            _log.DebugFormat("Listeners notified in {1}ms", timer.ElapsedMilliseconds);
            _log.Debug("End Write operation");


        }

        private void NotifyListeners(ReportRequest info, string fileName)
        {


            var response = Helpers.Response.Create(info, null, "Report Created", StatusCode._201_Created);

            response.ReportFilePath = fileName;

            OnReportCreated(response);
        }


     

        public event Action<ReportResponse> ReportCreated;
        private IFileRepository _repository;

        private void OnReportCreated(ReportResponse responseData)
        {
            if (ReportCreated != null)
            {
                foreach (Action<ReportResponse> handler in ReportCreated.GetInvocationList())
                {
                    handler(responseData);
                }

            }
            else
            {
                //throw ERROR: no subscribers
            }
        }

        private string ConfigureFileName(ReportRequest info)
        {
            var file = String.Concat(info.CorrelationID,".xml");

            if (String.IsNullOrEmpty(info.RelativePathDestination))
                return file;

            var path = Path.Combine(info.RelativePathDestination,file);

            return path;
        }
    }
}
