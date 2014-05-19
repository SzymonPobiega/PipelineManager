using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pipelines;

namespace UnitTests
{
    public abstract class EventDriventTest
    {
        protected SimpleEventSink EventSink;

        [SetUp]
        public void Setup()
        {
            EventSink = new SimpleEventSink();
        }

        protected void ExpectNothing()
        {
            EventSink.Events.ExpectNothing();
        }

        protected EventAsserter Expect(object firstExpectedEvent)
        {
            var asserter = new EventAsserter(EventSink.Events);
            asserter.ThenExpect(firstExpectedEvent);
            return asserter;
        }

        protected EventAsserter Expect<T>(Func<T, bool> firstExpectedEventSpec)
        {
            var asserter = new EventAsserter(EventSink.Events);
            asserter.ThenExpect(firstExpectedEventSpec);
            return asserter;
        }
        
        protected EventAsserter ExpectAny<T>()
        {
            return Expect((T e) => true);
        }

        protected class SimpleEventSink : IUnitOfWork
        {
            public readonly List<object> Events = new List<object>();

            public void On(object evnt)
            {
                Events.Add(evnt);
            }

            public T LoadSubject<T>() where T : PipelineSubject, new()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> UncommittedEvents { get { return Events; }}
            public int Version { get; private set; }
        }
    }
}