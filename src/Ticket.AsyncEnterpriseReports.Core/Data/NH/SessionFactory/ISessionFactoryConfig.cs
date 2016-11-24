using NHibernate;

namespace Ticket.AsyncEnterpriseReports.Core.Data.NHibernate.SessionFactory
{
    public interface ISessionFactoryConfig
    {
        ISessionFactory GetSessionFactory(string assemblyName, bool isDebug);
    }
}