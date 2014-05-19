using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Pipelines.Web
{
    public class PipelineControllerActivator : IHttpControllerActivator
    {
        private readonly IPipelineHost _pipelineHost;

        public PipelineControllerActivator(IPipelineHost pipelineHost)
        {
            _pipelineHost = pipelineHost;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return (IHttpController) Activator.CreateInstance(controllerType, new object[] {_pipelineHost});
        }
    }

}