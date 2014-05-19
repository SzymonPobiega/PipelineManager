namespace Pipelines.Schema.Builders
{
    public interface IPipelineBuilder
    {
        IStageBuilder AddStage(StageTriggerMode triggerMode);
        IStageBuilder AddStage(string id, StageTriggerMode triggerMode);
        PipelineSchema BuildSchema();
    }
}