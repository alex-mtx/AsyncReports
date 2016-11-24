using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.Core.Configuration;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Repositories;
using Ticket.AsyncEnterpriseReports.Repositories.Contracts;

namespace Ticket.AsyncEnterpriseReports.Factories
{
    public static class RepositoryFactory
    {
        public static IFileRepository CreateFromProcessManagerConfig()
        {

            var config = ConfigurationManager.GetSection("ProcessManager") as ProcessManagerConfigurationSection;

           var repositoryType = config.ReportRepository.RepositoryType;

           return Create(repositoryType);


        }


        private static IFileRepository Create(RepositoryType repositoryType)
        {
            switch (repositoryType)
            {
                case Core.Configuration.RepositoryType.FTP:
                    return new FTPSRepository();

                case Core.Configuration.RepositoryType.FileSystem:
                    return new FileSystemRepository();

                default:
                    throw new ConfigurationErrorsException(String.Format("The Repository type '{0}' is not supported yet", repositoryType));

            }

        }
    }
}
