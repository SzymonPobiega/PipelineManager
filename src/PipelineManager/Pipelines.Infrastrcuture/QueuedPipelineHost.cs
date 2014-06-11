using System;
using NHibernate;

namespace Pipelines.Infrastructure
{
    public class QueuedPipelineHost : IPipelineHost
    {
        private readonly ISessionFactory _sessionFactory;

        public QueuedPipelineHost(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Activate(string pipelineId, object data)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var command = new ActivateCommand(pipelineId, data);
                CommandQueue.Enqueue(command, session);
                tx.Commit();
            }
        }

        public void Trigger(string pipelineId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                CommandQueue.Enqueue(new TriggerCommand(pipelineId), session);
                tx.Commit();
            }
        }
    }
}