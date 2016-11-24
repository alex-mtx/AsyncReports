using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.DTOs;

namespace Ticket.AsyncEnterpriseReports.Pedidos.Domain.Repositories
{
    public interface IUnidadeRepository
    {

        IList<Unidade> PorPedido(long idPedido);
    }
}
