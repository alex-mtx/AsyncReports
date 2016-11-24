using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Processing.Contracts;

namespace Ticket.AsyncEnterpriseReports.Processing
{
    public class ProcessManager : IProcessManager
    {
        private static ILog _log = Logger.GetLogger("ProcessManager");

        public event Action OnRunning;

        public event Action OnStopping;

        public event Action OnSuspending;

        private ProcessManagerConfigurationSection _config;


        public ProcessManager(ProcessManagerConfigurationSection config)
        {
            _config = config;
            InitializeExecutors();
        }

        /// <summary>
        /// Automatically loads a ConfigurarionSection named "ProcessManager" from config file.
        /// </summary>
        public ProcessManager()
        {
            _config = ConfigurationManager.GetSection("ProcessManager") as ProcessManagerConfigurationSection;
            InitializeExecutors();
        }

        private void InitializeExecutors()
        {
            _log.Debug("Initializing ProcessExecutors");

            foreach(ProcessExecutorConfig config in _config.Executors)
            {
                Parallel.For(0, config.ParallelismDegree, x =>
                {
                    
                    var processExec = new ProcessExecutor(config,this);
                    _log.DebugFormat("Instance {0} Created", x);

                });
            }
        }

        public void StopExecutors()
        {
            _log.Info("Stopping Executors");
            
            if (OnStopping != null)
                foreach (Action handler in OnStopping.GetInvocationList())
                {
                    handler();
                }

            _log.Info("Stopped");
        }


        public void RunExecutors()
        {
            _log.Info("RunExecutors");

            if (OnRunning != null)
                foreach (Action handler in OnRunning.GetInvocationList())
                {
                    handler();
                }
        }

        public void SuspendExecutors()
        {
            _log.Info("SuspendExecutors");
            if (OnSuspending != null)
                foreach (Action handler in OnSuspending.GetInvocationList())
                {
                    handler();
                }

        }


        public void Prepare(string configSectionName)
        {
            _config = ConfigurationManager.GetSection(configSectionName) as ProcessManagerConfigurationSection;
        }

        public void Prepare(ProcessManagerConfigurationSection config)
        {
            _config = config;
        }
    }
}
