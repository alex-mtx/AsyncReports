using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Processing.Contracts
{
    public interface IReportWriter
    {
        void Write(object data, ReportRequest info);
        event Action<ReportResponse> ReportCreated;
    }
}
