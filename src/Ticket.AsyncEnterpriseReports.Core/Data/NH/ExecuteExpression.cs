using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticket.AsyncEnterpriseReports.Core.Data.NHibernate
{
    public class ExecuteExpression
    {
        private IStatelessSession _session;

        public ExecuteExpression(string factoryName)
        {
            _session = PersistenceManager.GetSession(factoryName);
        }

        public ParamExpression Execute(string procedureName)
        {
            var dbType = _session.Connection.GetType().Name == "OracleConnection" ? DataBaseType.Oracle : DataBaseType.Mssql;
            return new ParamExpression(_session, procedureName, dbType);
        }
    }
}
