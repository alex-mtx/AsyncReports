using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Processing.Activities;
using Ticket.AsyncEnterpriseReports.Processing.Contracts;

namespace Ticket.AsyncEnterpriseReports.Factories
{
    public static class ResponseSenderFactory
    {
        private static ProcessManagerConfigurationSection _config;
        private static QueueSenderConfig _queueConfig;
        private static ILog _log = Logger.GetLogger("ResponseSenderFactory");

        static ResponseSenderFactory()
        {
            _config = ConfigurationManager.GetSection("ProcessManager") as ProcessManagerConfigurationSection;
            _queueConfig = _config.QueueResponseSender;

        }
        public static IResponseSender Create(ReportRequest requestInfo)
        {

            return Create(requestInfo.Provider);


        }

        public static IResponseSender Create(ResponseProvider provider)
        {

            switch (provider)
            {
                case ResponseProvider.MessageQueue:
                    return QueueResponseSender.GetInstance(_queueConfig);
                default:
                    ///
                    ///TODO: Log!!!!
                    ///TODO: Lançar mensagem na fila de retorno
                    var message = String.Format("ResponseProvider '{0}' not supported", provider);

                    _log.Error(message);
                    
                    throw new NotImplementedException(message);

            }


        }
    }
}
