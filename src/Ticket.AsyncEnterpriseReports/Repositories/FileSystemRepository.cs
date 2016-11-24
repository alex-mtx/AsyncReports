using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Exceptions;
using Ticket.AsyncEnterpriseReports.Repositories.Contracts;

namespace Ticket.AsyncEnterpriseReports.Repositories
{
    public class FileSystemRepository : IFileRepository
    {
        private static ILog _log = Logger.GetLogger("FileSystemRepository");
        private static string _path;
        private static FileSystemRepositoryConfig _config;

        public FileSystemRepository()
        {
            var managerConfig = ConfigurationManager.GetSection("ProcessManager") as ProcessManagerConfigurationSection;

            _config = managerConfig.FileSystemRepository;


            _path = _config.AbsoluteFileLocation;

            _log.Debug("FileSystemRepository created");
            _log.DebugFormat("Path: '{0}'", _config.AbsoluteFileLocation);

        }


        public void Write(string fullFileName)
        {
            FileInfo info = new FileInfo(fullFileName);

            StreamReader sourceStream = new StreamReader(fullFileName);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();

            Write(fileContents, info.Name);

        }



        public void Write(byte[] fileContents, string fileName)
        {
            
            var fullFileName = Path.Combine(_path,fileName);
            _log.DebugFormat("Trying to write '{0}'", fullFileName);

            try
            {
                File.WriteAllBytes(fullFileName,fileContents);
            }
            catch (Exception e)
            {

                var message = Helpers.Response.AggregateExceptionMessages(e);
                _log.Error(message);
                throw new ReportWriterException(message, e);

            }

            _log.DebugFormat("Wrote '{0}' ", fullFileName);


        }

     

        public void Delete(string fileName)
        {
            var fullFileName = Path.Combine(_path, fileName);
            try
            {

                File.Delete(fullFileName);

            }
            catch (Exception e)
            {
                var message = Helpers.Response.AggregateExceptionMessages(e);
                _log.Error("Could not delete file",e);
                throw new ReportWriterException(message);

            }
        }


        public void DeleteAllUpUntil(DateTime finalDate)
        {
            throw new NotImplementedException();
        }
    }
}
