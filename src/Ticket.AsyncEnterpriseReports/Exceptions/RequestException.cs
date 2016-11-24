using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Exceptions
{
    public class RequestException : Exception
    {
        private string _message;
        public ReportResponse ResponseData { get; private set; }

        public RequestException(string message,ReportResponse responseData) : base(message)
        {

            this._message = message;
            ResponseData = responseData;

        }

     
    }
}
