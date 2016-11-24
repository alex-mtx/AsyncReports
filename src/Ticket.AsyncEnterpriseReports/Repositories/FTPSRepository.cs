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
using AlexPilotti.FTPS.Client;
using AlexPilotti.FTPS.Common;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Exceptions;
using Ticket.AsyncEnterpriseReports.Repositories.Contracts;

namespace Ticket.AsyncEnterpriseReports.Repositories
{
    public class FTPSRepository : IFileRepository
    {
        private static ILog _log = Logger.GetLogger("FTPSRepository");
        private static NetworkCredential _credential;
        private static Uri _server;
        private static FTPRepositoryConfig _config;

        public  FTPSRepository()
        {
            var managerConfig = ConfigurationManager.GetSection("ProcessManager") as ProcessManagerConfigurationSection;

            _config = managerConfig.FTPRepository;


            _server = _config.ServerUri;
            _credential = new NetworkCredential(_config.User, _config.Pwd);

            _log.Debug("FTPSRepository created");
            _log.DebugFormat("Server: '{0}' User: '{1}'", _config.ServerUri.ToString(), _config.User);

        }


        public void Write(string fullFileName)
        {
            FileInfo info = new FileInfo(fullFileName);

            StreamReader sourceStream = new StreamReader(fullFileName);
            byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            sourceStream.Close();

            Write(fileContents, info.Name);

        }



        public void Write(byte[] fileContents, string filename)
        {
            _log.DebugFormat("Trying to write '{0}' to '{1}'", filename, _server.Authority);

            var uri = new Uri(_server, filename);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Credentials = _credential;
            request.EnableSsl = true;

            ServicePointManager.ServerCertificateValidationCallback =
                CertificateValidationCallback;

            request.UseBinary = false;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UsePassive = false;

            Stream requestStream = null;

            try
            {
                using (requestStream = request.GetRequestStream())
                {

                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    _log.DebugFormat("UploadFile status: {0}", response.StatusDescription);

                    response.Close();

                }
            }
            catch (Exception e)
            {

                var message = Helpers.Response.AggregateExceptionMessages(e);
                _log.Error(message);
                throw new ReportWriterException(message, e);

            }

            _log.DebugFormat("Wrote '{0}' on {1}", filename, _server.Authority);


        }

        public static bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //I'm considering any certificate valid, since we're in the LAN.
            return true;

        }



        public void Delete(string fullFileName)
        {
            throw new NotImplementedException();
        }


        public void DeleteAllUpUntil(DateTime finalDate)
        {
            int deletedFiles = 0;

            _log.DebugFormat("Trying to delete all files created up until '{0}'", finalDate);

            using (FTPSClient client = new FTPSClient())
            {
                // Connect to the server, with mandatory SSL/TLS 
                // encryption during authentication and 
                // optional encryption on the data channel 
                // (directory lists, file transfers)
                client.Connect(_server.Authority, _credential,

                               ESSLSupportMode.All, CertificateValidationCallback);


                var items = client.GetDirectoryList();

                //adding root dir to guarantee old files deletion in it.
                items.Add(new DirectoryListItem() { IsDirectory = true, Name = "/" });

                var dirs = items.Where<DirectoryListItem>(x => x.IsDirectory);


                

                foreach (var dir in dirs)
                {
                    client.SetCurrentDirectory(dir.Name);

                    var files = client.GetDirectoryList().Where<DirectoryListItem>(x => x.IsDirectory == false);
                    foreach (var file in files)
                    {
                        try
                        {
                            if (!file.IsDirectory && file.CreationTime < finalDate)
                            {
                                client.DeleteFile(file.Name);
                                deletedFiles++;
                            }

                        }
                        catch (Exception e)
                        {

                            _log.ErrorFormat("'{0}' while deleting '{1}/{2}'", e.Message, dir.Name, file.Name);


                        }
                    }
                    client.SetCurrentDirectory("/");
                }

            }

            _log.DebugFormat("{0} files deleted",deletedFiles);
        }

        private  FtpWebRequest OpenConnection(Uri path,string ftpAction)
        {
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(_server,"tep"));
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
            request.Credentials = _credential;

            request.EnableSsl = true;

            ServicePointManager.ServerCertificateValidationCallback =
                CertificateValidationCallback;

            request.UseBinary = false;
            request.Method = ftpAction;
            request.UsePassive = false;
            return request;
        }
    }
}
