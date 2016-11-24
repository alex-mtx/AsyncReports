using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplexTypes = Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Pedidos.Infrastructure.Repositories.DTOs
{
    public class PedidoArquivoDto : ComplexTypes.Pedido
    {
        //public virtual long Id { get; set; }
        //public virtual DateTime DataPedido { get; set; }
        //public virtual DateTime DataEntrega { get; set; }
        //public virtual string NumeroPedido { get; set; }
        //public virtual string NumeroPedidoCliente { get; set; }
        //public virtual string NumeroPedidoOrigem { get; set; }
        //public virtual string NumeroCarrinhoCompra { get; set; }
        //public virtual string NumeroEnvio { get; set; }

        //Dados Arquivo
        public virtual long ArquivoId { get; set; }
        public virtual string ArquivoNome { get;set; }
        public virtual string ArquivoOrigemaSistema { get;set; }

        //Dados Produto
        public virtual long ProdutoId { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string CodigoERP { get; set; }
        
    }
}
