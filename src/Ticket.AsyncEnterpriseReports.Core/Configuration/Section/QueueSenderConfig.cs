using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class QueueSenderConfig : ConfigurationElement
    {

       [ConfigurationProperty("sectionName", IsRequired = true)]
        public string sectionName
        {
            get
            {
                return (string)this["sectionName"];
            }
            set
            {
                this["sectionName"] = value;
            }
        }



    }
}
