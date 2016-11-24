using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class InfrastructureConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("logLevel")]
        public String LogLevel
        {
            get
            {
                return (String)this["logLevel"];
            }
            set
            {
                this["logLevel"] = value;
            }
        }
        
        [ConfigurationProperty("modules")]
        public ModuleCollection Modules
        {
            get
            {
                return (ModuleCollection)this["modules"];
            }
        }

        [ConfigurationProperty("sessionFactories")]
        public SessionFactoryCollection SessionFactories
        {
            get
            {
                return (SessionFactoryCollection)this["sessionFactories"];
            }
        }
    }
}
