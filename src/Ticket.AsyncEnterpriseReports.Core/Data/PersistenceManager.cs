using System.Collections.Generic;

using NHibernate;
using Ticket.AsyncEnterpriseReports.Core.Data.NHibernate.SessionFactory;
using System;
using System.Reflection;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Logging;

namespace Ticket.AsyncEnterpriseReports.Core.Data
{
    public static class PersistenceManager
    {
        private static readonly Dictionary<string, ISessionFactory> SessionFactories = new Dictionary<string, ISessionFactory>();
        private static readonly object PersistenceLock = new object();
        private static ILog _log = Logger.GetLogger("PersistenceManager");

        internal static void Configure(string name, string connectionString, string factoryTypeName, bool isDebug)
        {
            var type = Assembly.GetExecutingAssembly().GetType(factoryTypeName);
            _log.DebugFormat("Exported Factory: {0}", type.FullName);
            
            var factoryConfig = (ISessionFactoryConfig)Activator.CreateInstance(type);
            _log.DebugFormat("Intantiated Factory: {0}", factoryConfig.GetType().FullName);

            
            _log.Debug("GetSessionFactory");
            
            var sessionFactory = factoryConfig.GetSessionFactory(connectionString, isDebug);
            
            _log.Debug("GetSessionFactory created");
            
            SessionFactories.Add(name, sessionFactory);

           // Log.To(typeof(PersistenceManager)).Message("Created session factory [{0}] for [{1}] with [{2}]", name, factoryTypeName, connectionString).Debug();
        }

        internal static IStatelessSession GetSession(string moduleName)
        {
            lock (PersistenceLock)
            {
                var session = GetSessionFactory(moduleName).OpenStatelessSession();

                //Log.To((typeof (PersistenceManager)))
                //    .Message("Get session [{0}] for [{1}]", session.GetHashCode(), moduleName)
                //    .Debug();
                _log.DebugFormat("Opened Stateless session [{0}] for [{1}]", session.GetHashCode(), moduleName);
                return session;
            }
        }

        private static ISessionFactory GetSessionFactory(string moduleName)
        {
            lock (PersistenceLock)
            {
                //Log.To((typeof(PersistenceManager))).Message("Get Session Factory for [{0}]", moduleName).Debug();

                return SessionFactories.ContainsKey(moduleName) ? SessionFactories[moduleName] : null;
            }
        }
    }
}
