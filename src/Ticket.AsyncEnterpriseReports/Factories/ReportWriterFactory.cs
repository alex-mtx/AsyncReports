using System;
using System.Configuration;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Exceptions;
using Ticket.AsyncEnterpriseReports.Processing.Activities;
using Ticket.AsyncEnterpriseReports.Processing.Contracts;
using Ticket.AsyncEnterpriseReports.Repositories;
using Ticket.AsyncEnterpriseReports.Repositories.Contracts;

namespace Ticket.AsyncEnterpriseReports.Factories
{
    static class ReportWriterFactory
    {
        private static ProcessManagerConfigurationSection _config;

        private static Core.Configuration.RepositoryType _repositoryType;

        static ReportWriterFactory()
        {

            _config = ConfigurationManager.GetSection("ProcessManager") as ProcessManagerConfigurationSection;

            _repositoryType = _config.ReportRepository.RepositoryType;
            

        }
        
        
        public static IReportWriter Create(ReportRequest request)
        {
            var repository = RepositoryFactory.CreateFromProcessManagerConfig();

            switch (request.AcceptedMediaType)
            {
                case MediaType.XML:

                    return new XMLReportWriter(repository);
                default:

                    var message = String.Format("Media Type '{0}' is not supported yet", request.AcceptedMediaType);
                    var response = Helpers.Response.Create(request, null, message, StatusCode._415_Unsupported_Media_Type);
                    throw new RequestException(message,response);
            }


        }
       
    }
}
