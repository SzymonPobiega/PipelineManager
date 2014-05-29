using Pipelines;
using Pipelines.Schema.Builders;

namespace UnitTests
{
    public static class PipelineBuilderExtensions
    {
        public static Pipeline Build(this IPipelineBuilder builder)
        {
            return builder.Build(new NoRetryFailureHandlingStrategy());
        }

        public static Pipeline Build(this IPipelineBuilder builder, IFailureHandlingStrategy failureHandlingStrategy)
        {
            return new PipelineFactory(new ActivatorStepFactory(), failureHandlingStrategy).Create("Pipeline", builder.BuildSchema());
        }
    }
}