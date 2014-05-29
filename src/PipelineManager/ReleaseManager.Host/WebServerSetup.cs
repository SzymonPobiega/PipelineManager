using System.Net.Http.Headers;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using ReleaseManager.Host.Models;

namespace ReleaseManager.Host
{
    class WebServerSetup
    {
        public static void ConfigureWebServer(IAppBuilder appBuilder, IContainer container)
        {
            ConfigureApi(appBuilder, container);
            ConfigureStaticFiles(appBuilder);
        }

        private static void ConfigureStaticFiles(IAppBuilder x)
        {
#if DEBUG
            var baseContentFolder = "../../";
#else
            var baseContentFolder = ".";
#endif
            x.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = new PathString("/static"),
                FileSystem = new PhysicalFileSystem(baseContentFolder)
            });
        }

        private static void ConfigureApi(IAppBuilder appBuilder, ILifetimeScope container)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Hooks", "hooks/{controller}/{correlationId}", new { correlationId = RouteParameter.Optional });
            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { controller = "Root", id = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();
            ConfigureJsonFormatter(config);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseWebApi(config);
        }

        private static void ConfigureJsonFormatter(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.JsonFormatter;

            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentTypes.CreateFor<PipelineModel>()));
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentTypes.CreateFor<ProjectsModel>()));
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentTypes.CreateFor<ReleaseCandidateModel>()));

            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}