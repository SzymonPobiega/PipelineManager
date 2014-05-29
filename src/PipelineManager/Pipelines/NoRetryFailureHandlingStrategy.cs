namespace Pipelines
{
    public class NoRetryFailureHandlingStrategy : IFailureHandlingStrategy
    {
        public bool ShouldRetry(BaseStep step, int attempt)
        {
            return false;
        }
    }
}