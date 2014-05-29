using NHibernate;

namespace Pipelines.Infrastructure
{
    public interface INHibernatePipelineHostFactory
    {
        IPipelineHost Create(ISession session);
        void Release(IPipelineHost host);
    }
}