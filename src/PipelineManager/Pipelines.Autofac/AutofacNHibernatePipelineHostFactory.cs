using System;
using Autofac;
using NHibernate;
using Pipelines.Infrastructure;

namespace Pipelines.Autofac
{
    public class AutofacNHibernatePipelineHostFactory : INHibernatePipelineHostFactory
    {
        private readonly ILifetimeScope _parentScope;

        public AutofacNHibernatePipelineHostFactory(ILifetimeScope parentScope)
        {
            _parentScope = parentScope;
        }

        public IPipelineHost Create(ISession session)
        {
            var childScope = _parentScope.BeginLifetimeScope();
            var pipelineRepository = new NHibernatePipelineRepository(session, childScope.Resolve<EventDispatcher>());
            var host = new PipelineHost(_parentScope.Resolve<IPipelineTypeResolver>(), pipelineRepository, childScope.Resolve<PipelineFactory>());
            return new AutofacPipelineHost(host, childScope);
        }

        public void Release(IPipelineHost host)
        {
            ((AutofacPipelineHost)host).CleanUp();
        }

        private class AutofacPipelineHost : IPipelineHost
        {
            private readonly IPipelineHost _wrappedHost;
            private readonly ILifetimeScope _scope;

            public AutofacPipelineHost(IPipelineHost wrappedHost, ILifetimeScope scope)
            {
                _wrappedHost = wrappedHost;
                _scope = scope;
            }

            public void Activate(string pipelineId, object data)
            {
                _wrappedHost.Activate(pipelineId, data);   
            }

            public void Trigger(string pipelineId)
            {
                _wrappedHost.Trigger(pipelineId);
            }

            public void CleanUp()
            {
                _scope.Dispose();
            }
        }
    }
}