using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.AsyncEnterpriseReports.ComplexTypes;

namespace Ticket.AsyncEnterpriseReports.Helpers
{
    static class Response
    {
        public static ReportResponse Create(ReportRequest request, string detail,string message, StatusCode code)
        {

            var responseStatus = new ResponseStatus()
            {
                Detail = detail,
                Message = message,
                StatusCode = code
            };

            var response = new ReportResponse()
            {
                RequestData = request,
                Status = responseStatus
            };


            return response;

        }

        public static string AggregateExceptionMessages(Exception exc)
        {
            string message = exc.Message;

            if (exc.InnerException != null)
                message = String.Concat(message," --> ", exc.InnerException.Message);

            return message;


        }
    }
}
