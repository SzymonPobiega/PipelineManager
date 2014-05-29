namespace Pipelines.Infrastructure
{
    public abstract class Command
    {
        public abstract void Execute(IPipelineHost pipelineHost);
    }
}