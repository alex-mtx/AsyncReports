using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;

namespace Ticket.AsyncEnterpriseReports.Processing.Contracts
{
    public interface IProcessManager
    {
        void Prepare(string configSectionName);
        void Prepare(ProcessManagerConfigurationSection config);
        event Action  OnRunning;
        event Action OnStopping;
        event Action OnSuspending;
        void StopExecutors();
        void RunExecutors();
        void SuspendExecutors();
    }
}
