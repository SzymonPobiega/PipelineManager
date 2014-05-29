using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using ReleaseManager.Extensibility;
using ReleaseManager.Process.Octopus.Controllers;
using ReleaseManager.Process.Octopus.DataAccess;

namespace ReleaseManager.Process.Octopus
{
    public class OctopusRuntimeModule : RuntimeModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<OctopusDeploymentDetailsView>().As<IEventHandler>();
            builder.RegisterType<MappingProvider>().As<IMappingProvider>();
            builder.RegisterApiControllers(this.GetType().Assembly);
                
            //Manifest
            builder.RegisterInstance(new ExtensionDescriptor(new UIExtension(UIExtensionType.DeploymentDetails, "releaseCandidateDeploymentOctopus")));
        }
    }
}