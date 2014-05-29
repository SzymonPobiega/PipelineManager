using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Owin.Hosting;
using Pipelines.Infrastructure;

namespace ReleaseManager.Host
{
    class Program
    {
// ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:9000/";

            var tokenSource = new CancellationTokenSource();

            var container = CompositionRoot.BuildAppContainer();
            var commandProcessor = container.Resolve<CommandProcessor>();
            Task.Factory.StartNew(() => commandProcessor.BeginProcessing(tokenSource.Token), tokenSource.Token);

            using (WebApp.Start(baseAddress, x => WebServerSetup.ConfigureWebServer(x, container)))
            {
                System.Diagnostics.Process.Start(baseAddress);
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine(); 
            }
            tokenSource.Cancel();
        }
    }
}
