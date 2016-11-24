using NHibernate;
using NHibernate.Cfg;

namespace Ticket.AsyncEnterpriseReports.Core.Data.NHibernate.SessionFactory
{
    public class OracleSessionFactoryConfig : ISessionFactoryConfig
    {
        public ISessionFactory GetSessionFactory(string connectionString, bool isDebug)
        {
            var configure = new global::NHibernate.Cfg.Configuration();
            configure.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.Driver<Driver.OracleDataClientDriver>();
                db.Dialect<global::NHibernate.Dialect.Oracle10gDialect>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.LogFormattedSql = isDebug;
                db.LogSqlInConsole = isDebug;
                db.AutoCommentSql = false;
            });
            return configure.BuildSessionFactory();
        }
    }
}
