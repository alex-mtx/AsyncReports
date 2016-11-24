using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.DTOs;

namespace Ticket.AsyncEnterpriseReports
{
    internal static class AutomapperConfig
    {
        public static void Configure()
        {

            Mapper.CreateMap<PedidoArquivoDto, PedidoPATDetalhado>()
                .ForMember(d => d.Arquivo, opts => opts.Ignore())
                .ForMember(d => d.Cliente, opts => opts.Ignore())
                .ForMember(d => d.Contrato, opts => opts.Ignore())
                .ForMember(d => d.DataEntregaSpecified, opts => opts.MapFrom(src => src.DataEntrega == null ? false : true))
                .ForMember(d => d.DataPedidoSpecified, opts => opts.MapFrom(src => src.DataPedido == null ? false : true))
                .ForMember(d => d.IdSpecified, opts => opts.MapFrom(src => true))
                .ForMember(d => d.ValorTotalSpecified, opts => opts.MapFrom(src => src.ValorTotal != null))
                .ForMember(d => d.ValorTotal, opts => opts.MapFrom(src => src.ValorTotal));
                    


            Mapper.CreateMap<PedidoArquivoDto, Arquivo>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.ArquivoId))
                .ForMember(d => d.Nome, opts => opts.MapFrom(s => s.ArquivoNome))
                .ForMember(d => d.SistemaOrigem, opts => opts.MapFrom(s => s.ArquivoSistemaOrigem))
                .ForMember(d => d.IdSpecified, o=>o.UseValue(true));


            Mapper.CreateMap<PedidoArquivoDto, ProdutoVO>()
                .ForMember(d => d.CodigoERP, opts => opts.MapFrom(s => s.ProdutoCodigoERP))
                .ForMember(d => d.Descricao, opts => opts.MapFrom(s => s.ProdutoDescricao));

            Mapper.CreateMap<PedidoArquivoDto, ContratoClienteVO>()
                .ForMember(d => d.NumContrato, opts => opts.MapFrom(s => s.ContratoId));

            Mapper.CreateMap<PedidoArquivoDto, ClienteVO>()
                .ForMember(d => d.Cnpj, opts => opts.MapFrom(s => s.Cnpj))
                .ForMember(d => d.RazaoSocial, opts => opts.MapFrom(s => s.RazaoSocial));


            Mapper.CreateMap<PedidoArquivoDto, StatusPedido>()
              .ForMember(d => d.Descricao, opts => opts.MapFrom(s => s.DescricaoStatus))
              .ForMember(d => d.Id, opts => opts.MapFrom(s => s.StatusId))
              .ForMember(d => d.IdSpecified, opts => opts.MapFrom(s => true));
        }
    }
}
