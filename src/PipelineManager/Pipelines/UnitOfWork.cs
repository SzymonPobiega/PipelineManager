using System.Collections.Generic;
using System.Linq;

namespace Pipelines
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly int _version;
        private readonly List<object> _committedEvents;
        private readonly List<object> _uncommittedEvents = new List<object>();
        private readonly List<PipelineSubject> _loadedSubjects = new List<PipelineSubject>(); 

        public IEnumerable<object> UncommittedEvents
        {
            get { return _uncommittedEvents; }
        }

        public int Version
        {
            get { return _version; }
        }

        public static IUnitOfWork CreateForNonExistingPipeline()
        {
            return new UnitOfWork(new List<object>(),0);
        }

        public static IUnitOfWork CreateForExistingPipeline(IEnumerable<object> events, int version)
        {
            return new UnitOfWork(events.ToList(), version);
        }

        private UnitOfWork(List<object> committedEvents, int version)
        {
            this._committedEvents = committedEvents;
            _version = version;
        }

        public T LoadSubject<T>()
            where T : PipelineSubject, new()
        {
            var subject = _loadedSubjects.OfType<T>().FirstOrDefault();
            if (subject != null)
            {
                return subject;
            }
            
            subject = new T {Sink = this};
            var eventApplicator = new DynamicEventSink(subject);
            foreach (var evnt in _committedEvents.Concat(_uncommittedEvents))
            {
                eventApplicator.On(evnt);
            }
            _loadedSubjects.Add(subject);
            return subject;
        }

        public void On(object evnt)
        {
            _uncommittedEvents.Add(evnt);
            foreach (var subject in _loadedSubjects)
            {
                subject.UpdateState(evnt);
            }
        }
    }
}