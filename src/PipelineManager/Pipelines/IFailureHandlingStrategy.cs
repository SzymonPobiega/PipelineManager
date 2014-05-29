namespace Pipelines
{
    public interface IFailureHandlingStrategy
    {
        bool ShouldRetry(BaseStep step, int attempt);
    }
}