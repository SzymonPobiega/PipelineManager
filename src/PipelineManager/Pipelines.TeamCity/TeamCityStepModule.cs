using Autofac;
using ReleaseManager.Extensibility;
using ReleaseManager.Process.TeamCity.Steps;

namespace ReleaseManager.Process.TeamCity
{
    public class TeamCityStepModule : StepModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<TeamCityBuildFinishedListener>();
            builder.RegisterType<TeamCityTestResultsDownloader>();
        }
    }
}