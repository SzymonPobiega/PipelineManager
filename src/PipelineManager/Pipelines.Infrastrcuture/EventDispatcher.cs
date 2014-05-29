using System;
using JetBrains.Annotations;
using NHibernate;

namespace Pipelines.Infrastructure
{
    public class EventDispatcher
    {
        private readonly IEventSink[] _eventSinks;

        public EventDispatcher([NotNull] params IEventSink[] eventSinks)
        {
            if (eventSinks == null) throw new ArgumentNullException("eventSinks");
            _eventSinks = eventSinks;
        }

        public void Dispatch(ISession session, string pipelineId, object evnt, DateTime occurenceDate)
        {
            var envelopeType = typeof(EventEnvelope<>).MakeGenericType(evnt.GetType());
            var envelope = Activator.CreateInstance(envelopeType, new[] { pipelineId, evnt, occurenceDate, session });

            foreach (var sink in _eventSinks)
            {
                sink.On(envelope);    
            }
        }
    }
}