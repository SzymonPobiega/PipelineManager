namespace Pipelines.Infrastructure
{
    public interface IInterceptor
    {
        void OnProcessing(CommandEnvelope command);
        void OnProcessed();
    }
}