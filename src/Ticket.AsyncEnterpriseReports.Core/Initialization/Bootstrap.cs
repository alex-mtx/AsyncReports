using System;
using System.Diagnostics;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Configuration;
using Ticket.AsyncEnterpriseReports.Core.Data;
using Ticket.AsyncEnterpriseReports.Core.Logging;


namespace Ticket.AsyncEnterpriseReports.Core.Initialization
{
    public sealed class Bootstrap
    {
        private static readonly object SyncRoot = new object();
        private static volatile Bootstrap _instance;
        private static ILog _log = Logger.GetLogger("Bootstrap");

        //http://msdn.microsoft.com/en-us/library/ff650316.aspx
        public static Bootstrap GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new Bootstrap();
                }
            }
            return _instance;
        }


        private Bootstrap()
        {
            var sw = new Stopwatch();
            sw.Start();
            _log.Debug("New Bootstrap");

            try
            {
                _log.Debug("EnvironmentChecker");
                EnvironmentChecker.RunCheckers();

 
                var configurations = ConfigurationManager.GetModulesConfiguration();

                foreach (var configuration in configurations)
                {

                    var mainAssembly = AssemblyManager.GetMainAssembly(configuration.AssemblyName);
                    var implementationAssembly = AssemblyManager.GetImplementationAssembly(configuration.AssemblyName);

                   // LogManager.Configure(configuration.AssemblyName, configuration.LogLevel);
                    UnityContainerInitializer.Configure();
                    UnityContainerInitializer.AutoRegister(mainAssembly, implementationAssembly);
                }


                var sessionFactories = ConfigurationManager.GetSessionConfiguration();

                foreach (var sessionFactory in sessionFactories)
                {
                  //  Log.To(typeof(Bootstrap)).Message("Prepare to configure [{0}] session", sessionFactory.Name).Debug();

                    PersistenceManager.Configure(sessionFactory.Name, sessionFactory.ConnectionString, sessionFactory.Factory, sessionFactory.IsDebug);
                }

              //  Log.To(typeof(Bootstrap)).Message("Finalizing Bootstrap at {0}ms", sw.ElapsedMilliseconds).Debug();
               // LogManager.FinalizeFrameworkLogger();
            }
            catch (Exception exception)
            {
                _log.Fatal("Failed to run bootstrap", exception);
                throw;
            }
        }

        public void Register<T, U>() where U : T
        {
            UnityContainerInitializer.Register<T, U>();
        }

        
    }
}