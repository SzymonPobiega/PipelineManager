namespace Pipelines
{
    public interface IPipelineHost
    {
        void Activate(string pipelineId, object data);
        void Trigger(string pipelineId);
    }
}