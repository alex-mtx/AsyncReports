using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class FileSystemRepositoryConfig : ConfigurationElement
    {

        [ConfigurationProperty("absoluteFileLocation",IsRequired=true)]
        public String AbsoluteFileLocation
        {
            get
            {
                return (String)this["absoluteFileLocation"];
            }
            set
            {
                this["absoluteFileLocation"] = value;
            }
        }

    }
}
