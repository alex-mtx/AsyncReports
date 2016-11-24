using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class ModuleCollection : ConfigurationElementCollection
    {
        public ModuleCollection()
        {
            AddElementName = "module";
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var elementKeys = (ModuleConfigurationElement)element;
            return elementKeys.AssemblyName;
        }

        public ModuleConfigurationElement Get(int index)
        {
            return (ModuleConfigurationElement)BaseGet(index);
        }
    }
}
