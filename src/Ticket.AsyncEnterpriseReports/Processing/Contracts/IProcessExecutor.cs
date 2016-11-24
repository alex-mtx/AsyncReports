using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;

namespace Ticket.AsyncEnterpriseReports.Processing.Contracts
{
    interface IProcessExecutor
    {
        void Begin();
        void Run();
        void Interrupt();
        void End();

    }
}
