using Pipelines;

namespace UnitTests
{
    public class RetryOnceFailureHandlingStrategy : IFailureHandlingStrategy
    {
        public bool ShouldRetry(BaseStep step, int attempt)
        {
            return attempt == 1;
        }
    }
}