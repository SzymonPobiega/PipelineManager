using System.Collections.Generic;

namespace Pipelines
{
    public class CompositeEventSink : IUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventSink[] _sinks;

        public CompositeEventSink(IUnitOfWork unitOfWork, params IEventSink[] sinks)
        {
            _unitOfWork = unitOfWork;
            _sinks = sinks;
        }

        public void On(object evnt)
        {
            _unitOfWork.On(evnt);
            foreach (var sink in _sinks)
            {
                sink.On(evnt);
            }
        }

        public T LoadSubject<T>() where T : PipelineSubject,  new()
        {
            return _unitOfWork.LoadSubject<T>();
        }

        public IEnumerable<object> UncommittedEvents
        {
            get { return _unitOfWork.UncommittedEvents; }
        }

        public int Version
        {
            get { return _unitOfWork.Version; }
        }
    }
}