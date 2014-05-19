namespace Pipelines.Schema.Builders
{
    public interface IStepBuilder : IActivityBuilder, IStageBuilder, IPipelineBuilder
    {
        IStepBuilder WithParameter(string name, object value);
    }
}