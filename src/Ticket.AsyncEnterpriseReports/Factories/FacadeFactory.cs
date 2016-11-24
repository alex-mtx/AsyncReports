using System;
using log4net;
using Ticket.AsyncEnterpriseReports.ComplexTypes;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Exceptions;
using Ticket.AsyncEnterpriseReports.Processing.Activities;
using Ticket.AsyncEnterpriseReports.Processing.Contracts;

namespace Ticket.AsyncEnterpriseReports.Factories
{
    static class FacadeFactory
    {
        private static ILog _log = Logger.GetLogger("FacadeFactory");
        public static IDataRetriever Create(ReportRequest request)
        {
            switch (request.BusinessInformation)
            {
                case BusinessInformation.Order:
                    return new OrderRetriever();
                
                
                
                default:
                
                    var message = String.Format("Facade not implemented: {0}", request.BusinessInformation);
                    _log.Error(message);
                    var response = Helpers.Response.Create(request, null, message, StatusCode._501_Not_Implemented);
                    throw new RequestException(message,response);
            }


        }
    }
}
