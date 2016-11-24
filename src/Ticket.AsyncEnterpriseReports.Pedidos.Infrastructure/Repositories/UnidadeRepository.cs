using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.Core.Data.NHibernate;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.Repositories;

namespace Ticket.AsyncEnterpriseReports.Pedidos.Infrastructure.Repositories
{
    public class UnidadeRepository : IUnidadeRepository
    {

        public IList<Unidade> PorPedido(long idPedido)
        {
              return DbExec.Uses("Oracle")
                .Execute("TKTSVC.TKT_SVC_SGP_PEDIDO_PKG.UNIDADES_ID_PEDIDO")
                .WithParam<long>("P_IDPEDIDO", idPedido)
                .GetResult(new InlineTransformer<Unidade>()
                    .Property<long>(x => x.Id, "IDUNIDADE")
                    .Property<long>(x => x.PedidoId, "IDPEDIDO")
                    .Property<string>(x => x.NomeUnidade, "NOME")
                    .Property<string>(x => x.CodigoUnidadeEntrega, "CDUNIDADE")
                    .Property<DateTime?>(x => x.LiberacaoCredito, "DATALIBERACAOCREDITO"));
        }
    }
}
