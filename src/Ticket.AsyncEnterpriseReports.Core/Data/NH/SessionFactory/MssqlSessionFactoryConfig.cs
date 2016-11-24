using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace Ticket.AsyncEnterpriseReports.Core.Data.NHibernate.SessionFactory
{
    public class MssqlSessionFactoryConfig : ISessionFactoryConfig
    {
        public ISessionFactory GetSessionFactory(string connectionString, bool isDebug)
        {
            var configure = new global::NHibernate.Cfg.Configuration();
            configure.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.Driver<Sql2008ClientDriver>();
                db.Dialect<MsSql2008Dialect>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.LogFormattedSql = isDebug;
                db.LogSqlInConsole = isDebug;
                db.AutoCommentSql = false;
            });

            return configure.BuildSessionFactory();
        }
    }
}
