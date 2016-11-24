namespace Ticket.AsyncEnterpriseReports.Core.Data.NHibernate
{
    public static class DbExec
    {
        public static ExecuteExpression Uses (string factoryName){
            return new ExecuteExpression(factoryName);
        }
    }
}
