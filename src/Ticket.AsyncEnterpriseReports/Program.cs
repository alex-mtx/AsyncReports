using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Configuration;
using Ticket.AsyncEnterpriseReports.Core.Initialization;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Processing;
using Ticket.AsyncEnterpriseReports.Repositories;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Ticket.AsyncEnterpriseReports
{
    static class Program
    {
        private static ILog _log;

 

        static void Main()
        {

            _log = Logger.GetLogger("WindowsService");
            _log.DebugFormat(Environment.NewLine,"Main Method");
#if DEBUG
            Bootstrap.GetInstance();
            ////70K benef pedido: 401300 10328367-14489910.XML
            ////7 benef pedido:406702  10358969-14531794.XML 
            //AutomapperConfig.Configure();
            //_log.Debug("Automapper Configured");
            //var manager = new ProcessManager();
            //manager.RunExecutors();

            var ftp = new FTPSRepository();
            var corte = new DateTime(2014,12,31);
            //ftp.Test();
            ftp.DeleteAllUpUntil(corte);
            Thread.Sleep(1500000);
  
#else

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Services.AsyncEntReportsService() 
            };

            ServiceBase.Run(ServicesToRun);

#endif
        }

        //Dados para teste de recebimento de solicitação
        //private static string XML()
        //{

        //    return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +

        //            "<PedidoPATDetalhadoPorNomeArquivo xmlns=\"http://ticket.corporativo/params/reports\">" +

        //            " <SourceSystem>TEP</SourceSystem>" +

        //            "<ReplyTo>tep</ReplyTo>" +

        //            " <CorrelationID>CorrelationID1</CorrelationID> " +

        //            "  <AcceptedMediaType>XML</AcceptedMediaType>" +

        //            " <NomeArquivo>10358969-14531794.XML</NomeArquivo>" +

        //            "</PedidoPATDetalhadoPorNomeArquivo>";
        //}

    }


}
