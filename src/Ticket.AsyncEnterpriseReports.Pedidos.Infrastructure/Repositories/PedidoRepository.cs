using System;
using Ticket.AsyncEnterpriseReports.Core.Data.NHibernate;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.Repositories;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.DTOs;
using System.Collections.Generic;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Pedidos.Infrasctructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        public IList<PedidoPATDetalhado> PorNomeArquivo(string nomeArquivo)
        {

            IList<PedidoPATDetalhado> pedidos = new List<PedidoPATDetalhado>();
            
            var dtos = DbExec.Uses("Oracle")
                .Execute("TKTSVC.TKT_SVC_SGP_PEDIDO_PKG.PEDIDO_NOME_ARQUIVO")
                .WithParam<string>("P_NOMEARQUIVO", nomeArquivo)
                .GetResult(new InlineTransformer<PedidoArquivoDto>()
                    .Property<long>(x => x.Id, "IDPEDIDO")
                    .Property<long>(x => x.IdCanalEntrada, "IDCANALENTRADA")
                    .Property<DateTime?>(x => x.DataPedido, "DATAPEDIDO")
                    .Property<DateTime?>(x => x.DataEntrega, "DATAENTREGA")
                    .Property<string>(x => x.NumeroPedidoCliente, "NUMPEDIDOCLIENTE")
                    .Property<string>(x => x.NumeroPedidoOrigem, "NUMPEDIDOORIGEM")
                    .Property<string>(x => x.NumeroCarrinhoCompra, "NUMCARRINHOCOMPRA")
                    .Property<string>(x => x.NumeroEnvio, "NUMENVIO")
                    .Property<decimal?>(x => x.ValorTotal,"VALORTOTAL")

                    //Dados do Arquivo
                    .Property<long>(x => x.ArquivoId, "IDARQUIVO")
                    .Property<string>(x => x.ArquivoNome, "NOMEARQUIVO")
                    .Property<string>(x => x.ArquivoSistemaOrigem, "SISTEMAORIGEM")

                    //Dados de Produto
                    .Property<long>(x => x.ProdutoId, "IDPRODUTO")
                    .Property<string>(x => x.ProdutoDescricao, "DESCPRODUTO")
                    .Property<string>(x => x.ProdutoCodigoERP, "CODPRODUTOERP")


                    //Dados de Contrato
                    .Property<long>(x => x.ContratoId, "IDCONTRATO")
                    .Property<string>(x => x.ContratoNumero, "NUMCONTRATO")

                    //Dados de Cliente
                    .Property<long>(x => x.ClienteId, "IDCLIENTE")
                    .Property<string>(x => x.RazaoSocial, "RAZAOSOCIAL")
                    .Property<string>(x => x.NomeFantasia, "NOMEFANTASIA")
                    .Property<string>(x => x.Cnpj, "CNPJ")

                    //Dados Status Pedido
                    .Property<string>(x => x.DescricaoStatus, "STATUS")
                    .Property<long>(x => x.StatusId, "IDSTATUS")
                    );


            foreach (var dto in dtos)
            {
                PedidoPATDetalhado pedido = AutoMapper.Mapper.Map<PedidoPATDetalhado>(dto);

                pedido.Arquivo = AutoMapper.Mapper.Map<Arquivo>(dto);
                pedido.Produto = AutoMapper.Mapper.Map<ProdutoVO>(dto);
                pedido.Cliente = AutoMapper.Mapper.Map<ClienteVO>(dto);
                pedido.Contrato = AutoMapper.Mapper.Map<ContratoClienteVO>(dto);
                pedido.Status = AutoMapper.Mapper.Map<StatusPedido>(dto);

                pedido.NumeroPedido = NumeroPedido(dto);

                pedidos.Add(pedido);
            }

            return pedidos;
        }

        private string NumeroPedido(PedidoArquivoDto dto)
        {
       

            //regra tirada do SGP
            //" WHEN IDCANALENTRADA = 1 THEN  NUMCARRINHOCOMPRA" - EXPRESS
            //" WHEN IDCANALENTRADA = 11 THEN  NUMCARRINHOCOMPRA" + 	// TEP

            //" WHEN IDCANALENTRADA = 5 THEN  NUMPEDIDOORIGEM" + 		// eTICKET

            //" WHEN IDCANALENTRADA = 3 THEN  NUMENVIO" + 			// TCP
            //" WHEN IDCANALENTRADA = 12 THEN  NUMENVIO" +            // BONUS
            //" WHEN IDCANALENTRADA = 4 THEN  NUMENVIO" + 			// MAGNETICO
            //" WHEN IDCANALENTRADA = 9 THEN  NUMENVIO" + 			//UPLOAD

            if (dto.IdCanalEntrada.Equals(1) || dto.IdCanalEntrada.Equals(11))
                return dto.NumeroCarrinhoCompra;

            if (dto.IdCanalEntrada == 5)
                return dto.NumeroPedidoCliente;

            return dto.NumeroEnvio;
        }


    }
}

