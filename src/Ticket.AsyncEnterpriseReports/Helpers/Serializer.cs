using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Logging;
using Ticket.AsyncEnterpriseReports.Exceptions;

namespace Ticket.AsyncEnterpriseReports.Helpers
{
    public static class Serializer
    {
        private static ILog _log = Logger.GetLogger("Serializer");

        public static T Deserialize<T>(string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                T result;

                using (TextReader reader = new StringReader(xml))
                {
                    result = (T)serializer.Deserialize(reader);
                }

                return result;
            }
            catch (Exception e)
            {
                var message = "Could not deserialize ReportRequest from Input Message.";
                _log.Error(message, e);
                var detail = Response.AggregateExceptionMessages(e);
                throw new BadPayloadException(message,detail,xml,ComplexTypes.StatusCode._400_Bad_Request);

            }
        }

        public static string RootNodeName(string xml)
        {
            try
            {

                var doc = XElement.Parse(xml);
                var navigator = doc.XPathSelectElement("/");

                return navigator.Name.LocalName;

            }
            catch (Exception e)
            {
                var message = "Could not parse ReportRequest from Input Message.";
                _log.Error(message, e);
                var detail = Response.AggregateExceptionMessages(e);
                throw new BadPayloadException(message, detail, xml, ComplexTypes.StatusCode._400_Bad_Request);
            }

        }


        public static byte[] ToXML<T>(T obj)
        {
            byte[] content = null;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(ms, obj);
                    return ms.ToArray();
                }

                //return content;
            }
            catch (Exception e)
            {


                var message = "Could not serialize to XML";
                var detail = Response.AggregateExceptionMessages(e);


                throw e;

            }
        }

    }



}

