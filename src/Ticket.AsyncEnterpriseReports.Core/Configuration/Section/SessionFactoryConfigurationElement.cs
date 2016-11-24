using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class SessionFactoryConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("connectionString")]
        public String ConnectionString
        {
            get
            {
                return (String)this["connectionString"];
            }
            set
            {
                this["connectionString"] = value;
            }
        }

        [ConfigurationProperty("factory")]
        public String Factory
        {
            get
            {
                return (String)this["factory"];
            }
            set
            {
                this["factory"] = value;
            }
        }

        [ConfigurationProperty("debug")]
        public bool Debug
        {
            get
            {
                return (bool)this["debug"];
            }
            set
            {
                this["debug"] = value;
            }
        }
    }
}
