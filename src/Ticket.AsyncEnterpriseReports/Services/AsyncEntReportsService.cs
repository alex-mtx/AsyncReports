using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Initialization;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Processing;

namespace Ticket.AsyncEnterpriseReports.Services
{
    //followed this: http://msdn.microsoft.com/en-us/library/zt39148a%28v=vs.110%29.aspx
    partial class AsyncEntReportsService : ServiceBase
    {
        private ProcessManager _process;
        private static ILog _log = Logger.GetLogger("AsyncEntReportsService");
        private static string _eventSource = "AsyncEntReportsService";
        private static string _eventLog = "Application";

        public AsyncEntReportsService()
        {
            try
            {


                InitializeComponent();

                SetupLog();
                
                _log.Debug("Creating AsyncEntReportsService");

                Bootstrap.GetInstance();

                _log.Debug("Bootstrapped");

                AutomapperConfig.Configure();
                _log.Debug("Automapper Configured");


                this.AutoLog = true;

                _log.Debug("Creating ProcessManager");
                _process = new ProcessManager();
            }
            catch (Exception e)
            {

                EventLog.WriteEntry(_eventSource, e.Message,EventLogEntryType.Error);
                throw;

            }
        }

        protected override void OnStart(string[] args)
        {   
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 15000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            base.OnStart(args);

            _process.RunExecutors();
            

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnContinue()
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_CONTINUE_PENDING;
            serviceStatus.dwWaitHint = 15000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            base.OnContinue();

            _process.RunExecutors();
            

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }


        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 2000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            base.OnStop();

            _process.StopExecutors();

            // Update the service state to Stoped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnPause()
        {
            // Update the service state to Pause Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSE_PENDING;
            serviceStatus.dwWaitHint = 2000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            base.OnPause();

            _process.SuspendExecutors();

            

            // Update the service state to Stoped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_PAUSED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private void SetupLog()
        {
         
            if (!EventLog.SourceExists(_eventLog))
                EventLog.CreateEventSource(_eventSource, _eventLog);

        }
    }
}
