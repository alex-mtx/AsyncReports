using System;
using log4net;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Exceptions;
using Ticket.AsyncEnterpriseReports.Helpers;

namespace Ticket.AsyncEnterpriseReports.Processing.Activities
{
    public static class RequestHandler
    {
        private static ILog _log = Logger.GetLogger("RequestHandler");

        /// <summary>
        /// Returns an instance of specific Type that represents the XML payload. it is based on the root xml node.
        /// </summary>
        /// <param name="reportRequestPayload"></param>
        /// <returns></returns>
        public static object Deserialize(string reportRequestPayload)
        {
            var node = Serializer.RootNodeName(reportRequestPayload);

            switch (node)
            {
                case "PedidoPATDetalhadoPorNomeArquivo":
                    return Serializer.Deserialize<PedidoPATDetalhadoPorNomeArquivo>(reportRequestPayload);

                default:
                    ///TODO: Log!!!!
                    ///TODO: Lançar mensagem na fila de retorno
                    var message = String.Format("Report '{0}' not supported", node);

                    _log.Error(message);
                    _log.Error(reportRequestPayload);

                    throw new BadPayloadException(message,null, reportRequestPayload,StatusCode._501_Not_Implemented);


            }
        }



    }
}
