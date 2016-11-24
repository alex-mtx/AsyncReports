using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Pedidos.Domain.Repositories;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using log4net;

namespace Ticket.AsyncEnterpriseReports.Pedidos.Application
{
    public class PedidoFacade : Contracts.IPedidoFacade
    {
        private readonly ILog _log = Logger.GetLogger("PedidoFacade");
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUnidadeRepository _unidadeRepository;
        private IBeneficioRepository _beneficiarioRepository;


        public PedidoFacade(IPedidoRepository pedido, IUnidadeRepository unidade, IBeneficioRepository beneficiario)
        {
            _pedidoRepository = pedido;
            _unidadeRepository = unidade;
            _beneficiarioRepository = beneficiario;
        }
        
        
        public ListaPedidosPATDetalhados CompletoPorNomeArquivo(string nomeArquivo)
        {

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            var pedidos = _pedidoRepository.PorNomeArquivo(nomeArquivo);


            _log.DebugFormat("NomeArquivo: {0} Tempo carga Pedidos: {1} ms", nomeArquivo,timer.Elapsed.Milliseconds);
            timer.Reset();

            AgregarUnidadesBeneficios(pedidos);


            var listaPedidos = new ListaPedidosPATDetalhados();
            listaPedidos.PedidoPATDetalhado = pedidos.ToArray<PedidoPATDetalhado>();
            return listaPedidos;
        }

        private void AgregarUnidadesBeneficios(IList<PedidoPATDetalhado> pedidos)
        {
            var timer = new System.Diagnostics.Stopwatch();
            //pedido: 377098
            //10328367-14489910.XML
            foreach (var pedido in pedidos)
            {

                timer.Start();

                var unidades = _unidadeRepository.PorPedido(pedido.Id).ToArray();
                Console.WriteLine("Unidades: {0} s", timer.Elapsed.Seconds);

                timer.Restart();

                List<Beneficio> beneficios = _beneficiarioRepository.PorPedido(pedido.Id) as List<Beneficio>;
                Console.WriteLine("{1} Beneficios: {0} s", timer.Elapsed.Seconds, beneficios.Count);

                timer.Restart();

                var mapUnidades = new Dictionary<long, List<Beneficio>>();

                beneficios.ForEach(
                    delegate(Beneficio beneficio)
                    {
                        AgregarBeneficioNaUnidade(mapUnidades, beneficio);

                    }
                );
                Console.WriteLine("AgregarBeneficioNaUnidade: {0} s", timer.Elapsed.Seconds);
                timer.Restart();

                foreach (var unidade in unidades)
                {
                    if (mapUnidades.ContainsKey(unidade.Id))
                        unidade.Beneficios = mapUnidades[unidade.Id].ToArray();

                }

                Console.WriteLine("Beneficios para cada unidade: {0} s", timer.Elapsed.Seconds);
                timer.Restart();
                pedido.Unidades = unidades.ToArray<Unidade>();
                Console.WriteLine("Unidades para Array: {0} s", timer.Elapsed.Seconds);
            }
        }

        private void AgregarBeneficioNaUnidade(Dictionary<long, List<Beneficio>> map, Beneficio beneficio)
        {
            if (!map.ContainsKey(beneficio.UnidadeId))
                map.Add(beneficio.UnidadeId,new List<Beneficio>());
            
            map[beneficio.UnidadeId].Add(beneficio);
            
        }
    
    
    
    }
}
