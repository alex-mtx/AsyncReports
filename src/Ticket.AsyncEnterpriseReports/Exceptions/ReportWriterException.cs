using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.AsyncEnterpriseReports.Exceptions
{
    public class ReportWriterException : Exception
    {
        public ReportWriterException(string message, Exception e) : base(message, e) { }
        public ReportWriterException(string message) : base(message) { }
    }
}
