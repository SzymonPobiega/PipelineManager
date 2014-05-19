namespace Pipelines.Schema.Builders
{
    public interface IStageBuilder : IPipelineBuilder
    {
        IActivityBuilder AddActivity();
    }
}