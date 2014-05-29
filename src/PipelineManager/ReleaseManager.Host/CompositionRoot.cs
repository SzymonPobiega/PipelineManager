using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using NHibernate;
using Pipelines;
using Pipelines.Autofac;
using Pipelines.Infrastructure;
using ReleaseManager.Extensibility;

namespace ReleaseManager.Host
{
    class CompositionRoot
    {
        public static IContainer BuildAppContainer()
        {
            var assemblies = new[]
            {
                "Pipelines.Infrastructure", 
                "ReleaseManager.DataAccess", 
                "ReleaseManager.Host", 
                "ReleaseManager.Process.Octopus",
                "ReleaseManager.Process.TeamCity",
                "ReleaseManager.Process.NUnit"
            };
            var extensionAssemblies = assemblies.Select(x => Assembly.Load(new AssemblyName(x))).ToArray();

            IContainer container = null;
            var appContainerBuilder = new ContainerBuilder();
            appContainerBuilder.RegisterAssemblyModules<RuntimeModule>(extensionAssemblies);
            appContainerBuilder.RegisterAssemblyModules<StepModule>(extensionAssemblies);

            appContainerBuilder.RegisterType<QueuedPipelineHost>().AsImplementedInterfaces();
            appContainerBuilder.RegisterApiControllers(typeof (Program).Assembly);
            appContainerBuilder.Register(BuildSessionFactory).SingleInstance();
            appContainerBuilder.RegisterType<StandardProcessTypeResolver>().As<IPipelineTypeResolver>();
            appContainerBuilder.RegisterType<PipelineFactory>();
// ReSharper disable once AccessToModifiedClosure
            appContainerBuilder.Register(context => new AutofacStepFactory(container));
            appContainerBuilder.RegisterType<NoRetryFailureHandlingStrategy>().AsImplementedInterfaces();
            appContainerBuilder.RegisterType<CommandProcessor>();
            appContainerBuilder.Register(BuildEventDispatcher).SingleInstance();

            container = appContainerBuilder.Build();
            return container;
        }

        private static EventDispatcher BuildEventDispatcher(IComponentContext context)
        {
            return new EventDispatcher(context.Resolve<IEnumerable<IEventHandler>>().Select(x => new DynamicEventSink(x)).Cast<IEventSink>().ToArray());
        }

        private static ISessionFactory BuildSessionFactory(IComponentContext context)
        {
            return NHibernateSessionFactoryBuilder.PrepareDatabase(context.Resolve<IEnumerable<IMappingProvider>>());
        }
    }
}