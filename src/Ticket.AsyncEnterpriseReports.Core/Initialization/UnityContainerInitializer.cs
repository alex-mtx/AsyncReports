using System;
using System.Linq;
using System.Reflection;
using Ticket.AsyncEnterpriseReports.Core.Data;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Ticket.AsyncEnterpriseReports.Core.Initialization
{
    internal static class UnityContainerInitializer
    {
        private static IUnityContainer Container { get; set; }

        internal static void Configure()
        {
            if (Container != null) return;

            Container = new UnityContainer();
            Container.AddNewExtension<Interception>();

            InitializeServiceLocator();

           // ConfigurePoliceInterceptor();

            //Log.To(typeof(UnityContainerInitializer)).Message("Container initialized").Debug();
        }

        internal static void AutoRegister(Assembly interfaceAssembly, Assembly implementationAssembly)
        {
            ContainerConfig(interfaceAssembly, implementationAssembly);

        }

        private static void InitializeServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(Container));

            //Log.To(typeof(UnityContainerInitializer)).Message("Service Locator Configured").Debug();
        }

        private static void ContainerConfig(Assembly mainAssembly, Assembly implementationAssembly)
        {
            var implementationTypes = implementationAssembly.GetExportedTypes();
            var interfaceTypes = mainAssembly.GetExportedTypes().Where(t=>t.IsInterface);

           // Log.To(typeof(UnityContainerInitializer)).Message("Mapping interfaces from [{0}] to [{1}]", mainAssembly.FullName, implementationAssembly.FullName).Debug();

            foreach (var exportedType in interfaceTypes)
            {
                var type = exportedType;
                var implementationType = Array.Find(implementationTypes, t => type.IsAssignableFrom(t) && !type.Equals(t));

                if (implementationType == null)
                {
                    //Log.To(typeof(UnityContainerInitializer)).Message("Not found implementation for [{0}]", exportedType.Name).Debug();
                    continue;
                }

                Container.RegisterType(exportedType, implementationType, null, null);

                //Log.To(typeof(UnityContainerInitializer)).Message("Mapped [{0}] to [{1}]", exportedType.Name, implementationType.Name).Debug();
            }
        }

    
        public static void Register<T,U>() where U : T
        {
            var type = typeof(T);
            if (!type.IsInterface)
                throw new Exception("T must be an interface");

    
            throw new Exception("T must a facade interface, a domain service interface or a domain repository interface");
            
        }

        //private static void ConfigurePoliceInterceptor()
        //{
        //    Container.Configure<Interception>()
        //        .AddPolicy("Context")
        //        .AddMatchingRule<FacadeMatchingRule>(
        //            new InjectionConstructor(
        //                new InjectionParameter(true)))
        //        .AddCallHandler(typeof(ContextHandler));
        //    Container.Configure<Interception>()
        //        .AddPolicy("Loggining")
        //        .AddMatchingRule<FacadeMatchingRule>(
        //            new InjectionConstructor(
        //                new InjectionParameter(true)))
        //        .AddCallHandler(typeof(LoggingHandler));
        //    Container.Configure<Interception>();
        //}
    }
}
