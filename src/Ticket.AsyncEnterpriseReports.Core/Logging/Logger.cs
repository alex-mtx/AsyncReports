using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace Ticket.AsyncEnterpriseReports.Core.Logging
{
    public static class Logger
    {
        private static log4net.ILog Log { get; set; }


        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        public static ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }

   
        public static void Fatal(object msg)
        {
            Log.Fatal(msg);
        }

        public static void Fatal(object msg, Exception ex)
        {
            Log.Fatal(msg, ex);
        }


        public static void Error(object msg)
        {
            Log.Error(msg);
        }

        public static void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }
        public static void Debug(object msg)
        {
            Log.Debug(msg);
        }

        public static void Info(object msg)
        {
            Log.Info(msg);
        }
    }
}
