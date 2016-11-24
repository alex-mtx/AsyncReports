using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.Messaging;


namespace Ticket.AsyncEnterpriseReports.Core.Initialization
{
   public static class EnvironmentChecker
    {
       private static ILog _log = Logger.GetLogger("Initialization");
       private static string PROCESS_MANAGER_SECTION_NAME = "ProcessManager";
       private static string DIRECTORY_WRITE_ACCESS_ERROR = "Either the '{0}' directory does not exist or the user '{1}' has no write permission on it.";

         private static void FileSystemRepositoryConfigCheck()
        {
             FileSystemRepositoryConfig config = null;
            
             try
            {
                var managerConfig = GetProcessManagerConfig();

                if (managerConfig.ReportRepository.RepositoryType != Configuration.RepositoryType.FileSystem)
                    return;


                config = managerConfig.FileSystemRepository;

                if (!Path.IsPathRooted(config.AbsoluteFileLocation))
                    throw new ConfigurationErrorsException("Config Section 'fileSystemRepository' requires an Absolute path");

                VerifyDirectoryAndRights(config.AbsoluteFileLocation);

            }
            catch (ConfigurationErrorsException e)
            {

                _log.Fatal(e);
                throw;

            }

             _log.Debug("'FileRepository' section is OK");
           
        }

         private static void QueueResponseSenderConfigCheck()
         {
             QueueSenderConfig config = null;

             try
             {
                 var managerConfig = GetProcessManagerConfig();
                 config = managerConfig.QueueResponseSender;


                 if (String.IsNullOrEmpty(config.sectionName))
                     throw new ConfigurationErrorsException("Config Section 'queueResponseSender' requires the 'configSectionName' attribute");

             }
             catch (ConfigurationErrorsException e)
             {

                 _log.Fatal(e);
                 throw;

             }

             _log.Debug("'queueResponseSender' section is OK");

         }

         private static void VerifySectionExistence<T>(string sectionToVerify)
         {
  
             try
             {
                 T config = (T)ConfigurationManager.GetSection(sectionToVerify);
                 if (config == null)
                     throw new ConfigurationErrorsException(string.Format("Section '{0}' must be declared", sectionToVerify));
                
             }
             catch (ConfigurationErrorsException e)
             {
                 _log.FatalFormat("Could not load '{0} section. Exception: {1}",sectionToVerify, e);
                 throw e;
             }
             _log.DebugFormat("Section '{0}' existis", sectionToVerify);
         }

         internal static void RunCheckers()
         {
             VerifySectionExistence<ProcessManagerConfigurationSection>(PROCESS_MANAGER_SECTION_NAME);

             FileSystemRepositoryConfigCheck();
             QueueResponseSenderConfigCheck();
             CheckFTPRepositoryConfig();
         }

         private static void CheckProcessManagerConfig()
         {
             GetProcessManagerConfig();

         }


         private static ProcessManagerConfigurationSection GetProcessManagerConfig()
         {

                 var managerConfig = ConfigurationManager.GetSection(PROCESS_MANAGER_SECTION_NAME) as ProcessManagerConfigurationSection;

                 return managerConfig;

         }

         private static void VerifyDirectoryAndRights(string absolutePath)
         {


             try
             {


                 var testFile = Guid.NewGuid().ToString("N") + ".tmp";
                 testFile = Path.Combine(absolutePath, testFile);

                 using (FileStream fstream = new FileStream(testFile, FileMode.Create))
                    using (TextWriter writer = new StreamWriter(fstream))
                    {

                        writer.WriteLine("AsyncReport write access test");

                    }

                 File.Delete(testFile);
             }
             catch (Exception e)
             {
                 var error = String.Format(DIRECTORY_WRITE_ACCESS_ERROR, absolutePath, Environment.UserName);
                 _log.Fatal(error,e);
                 throw e;

             }

             _log.DebugFormat("Write access is granted for '{0}' directory", absolutePath);

         }

         private static void CheckFTPRepositoryConfig()
         {
             try
             {

                 var managerConfig = ConfigurationManager.GetSection("ProcessManager") as ProcessManagerConfigurationSection;

                 var repoType = managerConfig.ReportRepository.RepositoryType;

                 if (repoType != Configuration.RepositoryType.FTP)
                     return;

                 var config = managerConfig.FTPRepository;

                 if (config.ServerUri == null)
                     LogAndThrow("Missing 'ftpRepository.server' configuration.");

                 if (String.IsNullOrEmpty(config.User))
                     LogAndThrow("ftpRepository.user cannot be empty");

                 if (String.IsNullOrEmpty(config.Pwd))
                     LogAndThrow("ftpRepository.pwd cannot be empty");



             }
             catch (Exception e)
             {

                 LogAndThrow(String.Concat("Error while checking FTPRepositoryConfig: ",e.Message));


             }



         }

         private static void LogAndThrow(string message)
         {
             _log.Fatal(message);
             throw new ConfigurationErrorsException(message);


         }
    }
}
