using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class FTPRepositoryConfig : ConfigurationElement
    {

       [ConfigurationProperty("server", IsRequired = true)]
        public Uri ServerUri
        {
            get
            {
                return (Uri)this["server"];
            }
            set
            {
                this["server"] = value;
            }
        }


       [ConfigurationProperty("user", IsRequired = true)]
       public string User
       {
           get
           {
               return (string)this["user"];
           }
           set
           {
               this["user"] = value;
           }
       }



       [ConfigurationProperty("pwd", IsRequired = true)]
       public string Pwd
       {
           get
           {
               return (string)this["pwd"];
           }
           set
           {
               this["pwd"] = value;
           }
       }


    }
}
