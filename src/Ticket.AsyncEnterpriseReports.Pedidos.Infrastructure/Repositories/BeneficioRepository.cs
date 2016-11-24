using System;
using Ticket.AsyncEnterpriseReports.Core.Data.NHibernate;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.Repositories;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.DTOs;
using System.Collections.Generic;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Pedidos.Infrastructure.Repositories
{
    public class BeneficioRepository : IBeneficioRepository
    {

        public IList<Beneficio> PorUnidade(long idUnidade)
        {
            throw new NotImplementedException();
        }


        public IList<Beneficio> PorPedido(long idPedido)
        {
            return DbExec.Uses("Oracle")
               .Execute("TKTSVC.TKT_SVC_SGP_PEDIDO_PKG.BENEFICIOS_ID_PEDIDO")
               .WithParam<long>("P_IDPEDIDO", idPedido)
               .GetResult(new InlineTransformer<Beneficio>()
                   .Property<long>(x => x.Id, "IDBENEFICIO")
                   .Property<string>(x => x.CodigoDepartamento, "CDDEPTO")
                   .Property<long>(x => x.PedidoId, "IDPEDIDO")
                   .Property<long>(x => x.UnidadeId, "IDUNIDADE")
                   .Property<string>(x => x.NomeBeneficiario, "NOMEBENEFICIARIO")
                   .Property<string>(x => x.NomeDepartamento, "NOMEDEPTO")
                   .Property<long?>(x => x.CPF, "CPF")
                   .Property<long?>(x => x.IdentificacaoBeneficiario, "NUMIDENTIDADE")
                   .Property<decimal?>(x => x.ValorBeneficio, "VALOR")
                   .HardCodedBooleanProperty<bool>(x => x.CPFSpecified,x=>x.CPF!=null)
                   .HardCodedBooleanProperty<bool>(x => x.IdentificacaoBeneficiarioSpecified, x => x.IdentificacaoBeneficiario != null )
                   .HardCodedProperty<bool>(x => x.IdSpecified, true)
                   .HardCodedProperty<bool>(x => x.PedidoIdSpecified, true)
                   .HardCodedProperty<bool>(x => x.UnidadeIdSpecified, true)
                   .HardCodedBooleanProperty<bool>(x => x.ValorBeneficioSpecified, x => x.ValorBeneficio != null));
                   
        }
    }
}
