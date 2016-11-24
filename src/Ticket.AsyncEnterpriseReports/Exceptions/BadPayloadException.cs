using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Exceptions
{
    public class BadPayloadException : Exception
    {
        public BadPayloadMessage BadPayloadMessage { get; private set; }

        public BadPayloadException(string message, string detail, string payload, StatusCode statusCode)
            : base(message) 
        {
            var statusData = new ComplexTypes.ResponseStatus();
            statusData.Detail = detail;
            statusData.Message = message;
            statusData.StatusCode = statusCode;

            BadPayloadMessage = new BadPayloadMessage() { RawRequestData = payload,
                                                        Status = statusData };
        
        }
    }
}
