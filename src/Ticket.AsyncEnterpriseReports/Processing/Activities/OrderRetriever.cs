using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.ServiceLocation;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Exceptions;
using Ticket.AsyncEnterpriseReports.Pedidos.Application;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Processing.Activities
{
    public class OrderRetriever : Contracts.IDataRetriever
    {
        private static ILog _log = Logger.GetLogger("OrderRetriever");
        private PedidoFacade _pedidoFacade;

        public OrderRetriever()
        {
            _log.Debug("Instantiating PedidoFacade");
            _pedidoFacade = ServiceLocator.Current.GetInstance<PedidoFacade>();
            _log.Debug("ok");
        }

        public object GetIt(ComplexTypes.ReportRequest requestInfo)
        {
            if (requestInfo is PedidoPATDetalhadoPorNomeArquivo)
            {
                try
                {

                    _log.Debug("Obtain data for PedidoPATDetalhadoPorNomeArquivo");


                    var param = (requestInfo as PedidoPATDetalhadoPorNomeArquivo).NomeArquivo;
                    _log.DebugFormat("Param: {0}", param);


                    ListaPedidosPATDetalhados lista = _pedidoFacade.CompletoPorNomeArquivo(param);
                    _log.DebugFormat("{0} records returned from pedidoFacade.CompletoPorNomeArquivo", lista.PedidoPATDetalhado.Length);

                    if (lista.PedidoPATDetalhado.Length > 0)
                        return lista;

                    return null;

                }
                catch (Exception e)
                {
                    var message = "Error executing pedidoFacade.CompletoPorNomeArquivo";
                    _log.Error(message, e);
                    throw new PermanentErrorException(message, e);
                }
            }


            //if it got so far, requested report is not implemented yet

            ///TODO: Lançar mensagem na fila de retorno
            var error = String.Format("Requested Report is not implemented in OrderRetriever: {0}", requestInfo.GetType().FullName);

            _log.Error(error);

            throw new NotImplementedException(error);
        
        
        }
    }
}
