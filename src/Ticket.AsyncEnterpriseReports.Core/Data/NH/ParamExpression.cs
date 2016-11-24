using System.Data;
using NHibernate;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Text;

namespace Ticket.AsyncEnterpriseReports.Core.Data.NHibernate
{
    public class ParamExpression
    {
        private readonly string _procedureName;
        private const string OraProcedureBody = "{{ call {0}({1})}}";
        private const string MssqlProcedureBody = "EXECUTE {0} {1}";
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private readonly DataBaseType _dbType;
        private readonly IStatelessSession _session;

        public ParamExpression(IStatelessSession session, string procedureName, DataBaseType dbType)
        {
            _session = session;
            _procedureName = procedureName;
            _dbType = dbType;
        }

        public ParamExpression WithParam<T>(string paramName, T paramValue)
        {
            _parameters.Add(paramName, paramValue);
            return this;
        }

        public IList<T> GetResult<T>()
        {
            var query = GetQuery<T>(new AliasToBeanResultTransformer(typeof(T)));
            return query.List<T>();
        }

        public IList<T> GetResult<T>(InlineTransformer<T> resultTransformer)
        {
            var query = GetQuery<T>(resultTransformer);
            return query.List<T>();
        }

        public T GetUniqueResult<T>()
        {
            var query = GetQuery<T>(new AliasToBeanResultTransformer(typeof(T)));
            return query.UniqueResult<T>();
        }

        public T GetUniqueResult<T>(InlineTransformer<T> resultTransformer)
        {
            var query = GetQuery<T>(resultTransformer);
            return query.UniqueResult<T>();
        }

        public void ExecuteNonQuery()
        {
            var query = PrepareQuery();

            query.ExecuteUpdate();
        }

        private IQuery GetQuery<T>(IResultTransformer resultTransformer)
        {
            var query = PrepareQuery();

            if (resultTransformer != null)
                query.SetResultTransformer(resultTransformer);

            return query;
        }

        public T GetTypeResult<T>()
        {
            var query = GetQuery<T>(null);
            var result = query.UniqueResult();
            return (T)System.Convert.ChangeType(result, typeof(T));
        }

        private IQuery PrepareQuery()
        {
            var sb = new StringBuilder();
            foreach (var param in _parameters)
            {
                sb.AppendFormat(":{0} ,", param.Key);
            }

            var paramlist = sb.ToString();

            if (paramlist.Length > 0)
                paramlist = paramlist.Substring(0, paramlist.Length - 1);


            IQuery query =
                _session.CreateSQLQuery(string.Format(_dbType == DataBaseType.Oracle ? OraProcedureBody : MssqlProcedureBody,
                    _procedureName, paramlist));

            foreach (var param in _parameters)
            {
                if (param.Value != null)
                {
                    query.SetParameter(param.Key, param.Value);
                }
                else
                {
                    query.SetParameter(param.Key, param.Value, NHibernateUtil.Int32);
                }
            }
            return query;
        }
    }
}

