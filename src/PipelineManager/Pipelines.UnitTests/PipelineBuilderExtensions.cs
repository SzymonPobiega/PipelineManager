using Pipelines;
using Pipelines.Schema.Builders;

namespace UnitTests
{
    public static class PipelineBuilderExtensions
    {
        public static Pipeline Build(this IPipelineBuilder builder)
        {
            return new PipelineFactory(new ActivatorStepFactory()).Create("Pipeline", builder.BuildSchema());
        }
    }
}