using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class RepositoryConfig : ConfigurationElement
    {

       [ConfigurationProperty("repositoryType", IsRequired = true)]
        public RepositoryType RepositoryType
        {
            get
            {
                return (RepositoryType)this["repositoryType"];
            }
            set
            {
                this["repositoryType"] = value;
            }
        }



    }
}
