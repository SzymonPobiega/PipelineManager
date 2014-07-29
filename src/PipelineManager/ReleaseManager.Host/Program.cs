using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            const string localhostAddress = "http://localhost:9000/";

            var tokenSource = new CancellationTokenSource();

            var container = CompositionRoot.BuildAppContainer();
            var commandProcessor = container.Resolve<CommandProcessor>();
            Task.Factory.StartNew(() => commandProcessor.BeginProcessing(tokenSource.Token), tokenSource.Token);

            var options = new StartOptions();
            options.Urls.Add(localhostAddress);

            var firstIPAddress =
                Dns.GetHostEntry(Dns.GetHostName())
                    .AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                    .ToString();

            options.Urls.Add(localhostAddress.Replace("localhost",firstIPAddress));

            using (WebApp.Start(options, x => WebServerSetup.ConfigureWebServer(x, container)))
            {
                System.Diagnostics.Process.Start(localhostAddress);
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine(); 
            }
            tokenSource.Cancel();
        }
    }
}
