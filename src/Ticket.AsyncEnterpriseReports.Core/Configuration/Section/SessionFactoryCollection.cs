using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class SessionFactoryCollection : ConfigurationElementCollection
    {
        public SessionFactoryCollection()
        {
            AddElementName = "sessionFactory";
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SessionFactoryConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var elementKeys = (SessionFactoryConfigurationElement)element;
            return elementKeys.Name;
        }

        public SessionFactoryConfigurationElement Get(int index)
        {
            return (SessionFactoryConfigurationElement)BaseGet(index);
        }
    }
}
