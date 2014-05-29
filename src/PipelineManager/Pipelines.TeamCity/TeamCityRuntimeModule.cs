using Autofac;
using Autofac.Integration.WebApi;
using ReleaseManager.Extensibility;
using ReleaseManager.Process.TeamCity.Data;

namespace ReleaseManager.Process.TeamCity
{
    public class TeamCityRuntimeModule : RuntimeModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<TeamCityBuildView>().AsImplementedInterfaces();
            builder.RegisterType<MappingProvider>().AsImplementedInterfaces();
            builder.RegisterApiControllers(GetType().Assembly);
                
            //Manifest
            builder.RegisterInstance(new ExtensionDescriptor(new UIExtension(UIExtensionType.ReleaseCandidate, "releaseCandidateTeamCity")));
        }
    }
}