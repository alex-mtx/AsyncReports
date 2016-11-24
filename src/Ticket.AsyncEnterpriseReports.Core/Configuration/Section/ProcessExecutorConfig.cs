using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class ProcessExecutorConfig : ConfigurationElement
    {
        //[ConfigurationProperty("logLevel")]
        //public String LogLevel
        //{
        //    get
        //    {
        //        return (String)this["logLevel"];
        //    }
        //    set
        //    {
        //        this["logLevel"] = value;
        //    }
        //}

        [ConfigurationProperty("requestSourceConfigSectionName",IsRequired=true)]
        public String RequestSourceConfigSectionName
        {
            get
            {
                return (String)this["requestSourceConfigSectionName"];
            }
            set
            {
                this["requestSourceConfigSectionName"] = value;
            }
        }



        //[ConfigurationProperty("responseConfigSectionName", IsRequired = true)]
        //public String ResponseConfigSectionName
        //{
        //    get
        //    {
        //        return (String)this["responseConfigSectionName"];
        //    }
        //    set
        //    {
        //        this["responseConfigSectionName"] = value;
        //    }
        //}


        [ConfigurationProperty("parallelismDegree",IsRequired=false,DefaultValue=1)]
        public int ParallelismDegree
        {
            get
            {
                return (int)this["parallelismDegree"];
            }
            set
            {
                this["parallelismDegree"] = value;
            }
        }
    }
}
