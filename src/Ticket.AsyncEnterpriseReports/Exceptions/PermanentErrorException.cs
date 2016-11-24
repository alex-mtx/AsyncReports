using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.AsyncEnterpriseReports.Exceptions
{
    public class PermanentErrorException : Exception
    {
        public PermanentErrorException(string message) : base(message){}
        public PermanentErrorException(string message, Exception inner) : base(message, inner) { }
        public PermanentErrorException(){}
    }
}
