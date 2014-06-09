using System.Collections.Generic;
using Pipelines;
using Pipelines.Schema;
using Pipelines.Schema.Builders;
using ReleaseManager.Process.NUnit;
using ReleaseManager.Process.Octopus.Steps;
using ReleaseManager.Process.TeamCity.Steps;

namespace ReleaseManager.StandardProcesses
{
    public class StandardProcessSchemaRepository : IPipelineSchemaRepository
    {
        public IEnumerable<PipelineSchema> EnumerableSchemas()
        {
            yield return CIUP();
        }

// ReSharper disable once InconsistentNaming
        private static PipelineSchema CIUP()
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
            return builder.BuildSchema();
        }
    }
}