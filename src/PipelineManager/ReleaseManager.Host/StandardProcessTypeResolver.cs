using Pipelines;
using Pipelines.Schema.Builders;
using ReleaseManager.Process.NUnit;
using ReleaseManager.Process.Octopus.Steps;
using ReleaseManager.Process.TeamCity;
using ReleaseManager.Process.TeamCity.Steps;

namespace ReleaseManager.Host
{
    public class StandardProcessTypeResolver : CodeBasedTypeResolver
    {
        public StandardProcessTypeResolver()
        {
            var builder = new PipelineBuilder("TestProject", 1)
                .AddStage("Commit", StageTriggerMode.Automatic)
                .AddActivity()
                .AddStep<TeamCityBuildFinishedListener>()
                .AddStep<TeamCityTestResultsDownloader>().WithParameter("SuiteType", "Commit")
                .AddStep<CreateRelease>()
                .AddStage("Integration", StageTriggerMode.Throttled)
                .AddActivity()
                .AddStep<Deploy>().WithParameter("Environment", "INTEGRATION")
                .AddStep<LoadTestResults>().WithParameter("SuiteType", "Integration")
                .AddStep<WaitForDeploymentFinish>()
                .AddStage("UAT", StageTriggerMode.Throttled)
                .AddActivity()
                .AddStep<Deploy>().WithParameter("Environment", "UAT")
                .AddStep<WaitForDeploymentFinish>()
                .AddStage("Production", StageTriggerMode.Throttled)
                .AddActivity()
                .AddStep<Deploy>().WithParameter("Environment", "PROD")
                .AddStep<WaitForDeploymentFinish>();
            RegisterType(x => true, builder.BuildSchema());
        }
    }
}