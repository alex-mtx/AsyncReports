using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class ModuleConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("assemblyName")]
        public String AssemblyName
        {
            get
            {
                return (String)this["assemblyName"];
            }
            set
            {
                this["assemblyName"] = value;
            }
        }

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
    }
}
