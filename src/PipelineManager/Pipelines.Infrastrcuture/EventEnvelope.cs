using System;
using JetBrains.Annotations;
using NHibernate;

namespace Pipelines.Infrastructure
{
    public class EventEnvelope<T>
        where T : class 
    {
        private readonly T _payload;
        private readonly DateTime _occurenceDateUtc;
        private readonly string _pipelineId;
        private readonly ISession _session;

        public EventEnvelope([NotNull] string pipelineId, [NotNull] T payload, DateTime occurenceDateUtc,
            [NotNull] ISession session)
        {
            if (pipelineId == null) throw new ArgumentNullException("pipelineId");
            if (payload == null) throw new ArgumentNullException("payload");
            if (session == null) throw new ArgumentNullException("session");
            _pipelineId = pipelineId;
            _payload = payload;
            _occurenceDateUtc = occurenceDateUtc;
            _session = session;
        }

        public T Payload
        {
            get { return _payload; }
        }

        public string PipelineId
        {
            get { return _pipelineId; }
        }

        public DateTime OccurenceDateUtc
        {
            get { return _occurenceDateUtc; }
        }

        public ISession Session
        {
            get { return _session; }
        }

        public void EnqueueCommand(object command, DateTime executeAfter)
        {
            CommandQueue.Enqueue(command, _session, executeAfter);
        }
    }
}