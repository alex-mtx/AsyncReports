using System;
using System.Reflection;
using System.Text;

namespace Ticket.AsyncEnterpriseReports.Core.Initialization
{
    internal static class AssemblyManager
    {

        private const string Implementation = ".Infrastructure";

        internal static Assembly GetMainAssembly(string moduleName)
        {
            return GetAssembly(".Domain", moduleName);
        }

        internal static Assembly GetImplementationAssembly(string businessModuleName)
        {
            return GetAssembly(Implementation, businessModuleName);
        }

        private static Assembly GetAssembly(string assemblyType, string businessModuleName)
        {
            var assemblyName = businessModuleName + assemblyType;

            var ret = AppDomain.CurrentDomain.Load(assemblyName);

            //Log.To(typeof(AssemblyManager)).Message("Found [{0}] assembly.", ret).Debug();

            return ret;
        }
    }
}
