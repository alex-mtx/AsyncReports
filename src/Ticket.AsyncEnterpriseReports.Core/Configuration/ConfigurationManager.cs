using System;
using System.Collections.Generic;
using log4net;
using Ticket.AsyncEnterpriseReports.Core.Configuration.Section;
using Ticket.AsyncEnterpriseReports.Core.Logging;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration
{
    internal static class ConfigurationManager
    {
        private static readonly List<ModuleInfo> ModuleInfos = new List<ModuleInfo>();
        private static readonly List<SessionInfo> SessionInfos = new List<SessionInfo>();
        private static ILog _log = Logger.GetLogger("ConfigurationManager");

        internal static IEnumerable<ModuleInfo> GetModulesConfiguration()
        {
            _log.Debug("GetModulesConfiguration");

            ModuleInfos.Clear();

            var infrastructureSection = GetConfigSection();

            for (var count = 0; count < infrastructureSection.Modules.Count; count++)
            {
                var moduleConfiguration = infrastructureSection.Modules.Get(count);

                var moduleInfo = new ModuleInfo
                              {
                                  AssemblyName = moduleConfiguration.AssemblyName,
                                  LogLevel = moduleConfiguration.LogLevel
                              };

                ModuleInfos.Add(moduleInfo);

                _log.DebugFormat("Found configuration for [{0}] module", moduleConfiguration.AssemblyName);
            }

            return ModuleInfos;
        }

        internal static IEnumerable<SessionInfo> GetSessionConfiguration()
        {
            SessionInfos.Clear();

            var infrastructureSection = GetConfigSection();

            for (var count = 0; count < infrastructureSection.SessionFactories.Count; count++)
            {
                var sessionFactoryConfiguration = infrastructureSection.SessionFactories.Get(count);

                var sessionInfo = new SessionInfo
                {
                    Name = sessionFactoryConfiguration.Name,
                    ConnectionString = sessionFactoryConfiguration.ConnectionString,
                    Factory = sessionFactoryConfiguration.Factory,
                    IsDebug = sessionFactoryConfiguration.Debug,
                };

                SessionInfos.Add(sessionInfo);

                _log.DebugFormat("Found configuration for [{0}] sessionFactoryConfiguration", sessionFactoryConfiguration.Name);
            }

            return SessionInfos;
        }

        private static InfrastructureConfigurationSection GetConfigSection()
        {
            _log.Debug("GetConfigSection");

            var infrastructureConfiguration = System.Configuration.ConfigurationManager.GetSection("infrastructureConfiguration") as InfrastructureConfigurationSection;

            if (infrastructureConfiguration == null)
                throw new Exception("infrastructureConfiguration section not found!");

            _log.Debug("infrastructureConfiguration loaded");
            return infrastructureConfiguration;
        }

        internal static string GetLogLevel()
        {
            var infrastructureConfiguration = GetConfigSection();

            return infrastructureConfiguration.LogLevel;
        }
    
    }
}
