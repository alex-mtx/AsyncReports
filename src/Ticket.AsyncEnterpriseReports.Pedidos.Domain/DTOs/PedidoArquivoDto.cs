using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplexTypes = Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Pedidos.Domain.DTOs
{
    public class PedidoArquivoDto
    {
        public virtual long Id { get; set; }
        public virtual long IdCanalEntrada { get; set; }
        public virtual DateTime? DataPedido { get; set; }
        public virtual DateTime? DataEntrega { get; set; }
        public virtual string NumeroPedidoCliente { get; set; }
        public virtual string NumeroPedidoOrigem { get; set; }
        public virtual string NumeroCarrinhoCompra { get; set; }
        public virtual string NumeroEnvio { get; set; }
        public virtual decimal? ValorTotal { get; set; }

        ///Dados Arquivo
        public virtual long ArquivoId { get; set; }
        public virtual string ArquivoNome { get;set; }
        public virtual string ArquivoSistemaOrigem { get;set; }

        // Produto
        public virtual long ProdutoId { get; set; }
        public virtual string ProdutoDescricao { get; set; }
        public virtual string ProdutoCodigoERP { get; set; }

        //Contrato
        public virtual long ContratoId { get; set; }
        public virtual string ContratoNumero { get; set; }

        //cliente
        public virtual long ClienteId { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string Cnpj { get; set; }

        //status pedido
        public virtual string DescricaoStatus { get; set; }
        public virtual long StatusId { get; set; }

    
    
    }
}
