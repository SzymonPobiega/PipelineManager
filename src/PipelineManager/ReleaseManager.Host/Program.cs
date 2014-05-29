using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Hosting;
using NHibernate;
using Pipelines;
using Pipelines.Autofac;
using Pipelines.Infrastructure;
using ReleaseManager.Extensibility;

namespace ReleaseManager.Host
{
    class Program
    {
// ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
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

            const string baseAddress = "http://localhost:9000/";

            var pipelineFactory = BuildPipelineFactory(extensionAssemblies);

            var appContainerBuilder = new ContainerBuilder();
            appContainerBuilder.RegisterAssemblyModules<RuntimeModule>(extensionAssemblies);
            appContainerBuilder.RegisterType<QueuedPipelineHost>().AsImplementedInterfaces();
            appContainerBuilder.RegisterApiControllers(typeof(Program).Assembly);
            appContainerBuilder.Register(BuildSessionFactory).SingleInstance();
            appContainerBuilder.RegisterType<StandardProcessTypeResolver>().As<IPipelineTypeResolver>();
            appContainerBuilder.RegisterInstance(pipelineFactory);
            appContainerBuilder.RegisterType<CommandProcessor>();
            appContainerBuilder.Register(BuildEventDispatcher).SingleInstance();
            
            var container = appContainerBuilder.Build();
            var tokenSource = new CancellationTokenSource();

            var commandProcessor = container.Resolve<CommandProcessor>();
            Task.Factory.StartNew(() => commandProcessor.BeginProcessing(tokenSource.Token), tokenSource.Token);

            using (WebApp.Start(baseAddress, x => WebServerSetup.ConfigureWebServer(x, container)))
            {
                System.Diagnostics.Process.Start("http://localhost:9000");
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine(); 
            }
            tokenSource.Cancel();
        }

        private static PipelineFactory BuildPipelineFactory(Assembly[] extensionAssemblies)
        {
            var configValues = new DictionaryStepPropertyValueProvider(new Dictionary<string, object>
            {
                {"TeamCityUrl", "http://localhost:8090"},
            });

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyModules<StepModule>(extensionAssemblies);

            var pipelineFactory = new PipelineFactory(new AutofacStepFactory(containerBuilder), new NoRetryFailureHandlingStrategy(), configValues);
            return pipelineFactory;
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
