using System.Net;
using System.Net.Http;

namespace Pipelines.Web
{
    public abstract class WebHookInputTransformerController<TTransformer, TData> : PipelineController
        where TTransformer : IInputTransformer<TData>, new()
    {
        protected WebHookInputTransformerController(IPipelineHost host) : base(host)
        {
        }

        protected virtual string ExtractPipelineId(TData data, string correlationId)
        {
            return correlationId;
        }

        public HttpResponseMessage Post(string correlationId = null)
        {
            var transformer = new TTransformer();
            var data = transformer.Transform(Request.Content.ReadAsStreamAsync().Result);
            var pipelineId = ExtractPipelineId(data, correlationId);
            Host.Activate(pipelineId, data);

            var response = Request.CreateResponse(HttpStatusCode.Created);
            return response;
        }
    }
}
