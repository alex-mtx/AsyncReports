using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class ProcessExecutorCollection : ConfigurationElementCollection
    {
        public ProcessExecutorCollection()
        {
            AddElementName = "executor";
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProcessExecutorConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var elementKeys = (ProcessExecutorConfig)element;
            return elementKeys.RequestSourceConfigSectionName;
        }

        public ProcessExecutorConfig Get(int index)
        {
            return (ProcessExecutorConfig)BaseGet(index);
        }
    
    }
}
