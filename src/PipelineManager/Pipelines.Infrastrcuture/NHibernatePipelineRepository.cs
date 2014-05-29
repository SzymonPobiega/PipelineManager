using System;
using System.Linq;
using JetBrains.Annotations;
using NHibernate;
using Pipelines.Infrastructure.Records;

namespace Pipelines.Infrastructure
{
    public class NHibernatePipelineRepository : IPipelineRepository
    {
        private readonly ISession _session;
        private readonly EventDispatcher _dispatcher;

        public NHibernatePipelineRepository([NotNull] ISession session, [NotNull] EventDispatcher dispatcher)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (dispatcher == null) throw new ArgumentNullException("dispatcher");
            _session = session;
            _dispatcher = dispatcher;
        }

        public PipelineData TryGetById(string pipelineId)
        {
            var events = _session.QueryOver<EventRecord>()
                .Where(x => x.PipelineId == pipelineId)
                .List()
                .OrderBy(x => x.Sequence)
                .ToList();

            if (events.Count == 0)
            {
                return null;
            }
            var version = events.Last().Sequence;
            var payloads = events.Select(x => x.DeserializePayload()).ToList();
            return new PipelineData(payloads, version+1);
        }

        public void Store(string pipelineId, IUnitOfWork unitOfWork)
        {
            var occurenceDate = DateTime.UtcNow;
            foreach (var uncommittedEvent in unitOfWork.UncommittedEvents)
            {
                _dispatcher.Dispatch(_session, pipelineId, uncommittedEvent, occurenceDate);
            }
            var pipelineEvents = unitOfWork.UncommittedEvents.Select((x, i) => new EventRecord(pipelineId, unitOfWork.Version + i, x, occurenceDate));
            foreach (var pipelineEvent in pipelineEvents)
            {
                _session.Save(pipelineEvent);
            }
        }
    }
}
