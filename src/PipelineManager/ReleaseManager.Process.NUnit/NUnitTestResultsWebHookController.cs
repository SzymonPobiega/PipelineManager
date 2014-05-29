using Pipelines;
using Pipelines.Web;

namespace ReleaseManager.Process.NUnit
{
    public class NUnitTestResultsWebHookController : WebHookInputTransformerController<NUnitTestResultsTransformer, NUnitTestResults>
    {
        public NUnitTestResultsWebHookController(IPipelineHost host) : base(host)
        {
        }
    }
}