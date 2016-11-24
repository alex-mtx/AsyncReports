using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;

namespace Ticket.AsyncEnterpriseReports.Processing.Contracts
{
    public interface IResponseSender
    {
        void Send(ReportResponse responseInfo);
        void Send(BadPayloadMessage responseInfo);
        void Configure(QueueSenderConfig config);
    }
}
