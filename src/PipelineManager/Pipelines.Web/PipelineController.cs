using System.Web.Http;

namespace Pipelines.Web
{
    public abstract class PipelineController : ApiController
    {
        private readonly IPipelineHost _host;

        protected PipelineController(IPipelineHost host)
        {
            this._host = host;
        }

        protected IPipelineHost Host
        {
            get { return _host; }
        }
    }
}