using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Pedidos.Application.Contracts
{
    public interface IPedidoFacade
    {
        ListaPedidosPATDetalhados CompletoPorNomeArquivo(string nomeArquivo);
    }
}
