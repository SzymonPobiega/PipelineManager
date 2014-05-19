using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace UnitTests
{
    public class EventAsserter
    {
        private readonly List<object> _actualEvents;
        private int _index;

        public EventAsserter(List<object> actualEvents)
        {
            _actualEvents = actualEvents;
        }

        public EventAsserter ThenExpect(object expectedEvent)
        {
            Assert.IsTrue(_actualEvents.Count > _index, string.Format("Expected at least {0} events but found only {1}", _index + 1, _actualEvents.Count));
            Assert.AreEqual(expectedEvent, _actualEvents[_index]);
            _index++;
            return this;
        }

        public EventAsserter ThenExpect<T>(Func<T, bool> expectedEventSpec)
        {
            Assert.IsTrue(_actualEvents.Count > _index, string.Format("Expected at least {0} events but found only {1}", _index + 1, _actualEvents.Count));
            var actualEvent = _actualEvents[_index];
            Assert.IsInstanceOf<T>(actualEvent);
            Assert.IsTrue(expectedEventSpec((T)actualEvent));
            _index++;
            return this;
        }

        public EventAsserter ThenExpectAny<T>()
        {
            return ThenExpect((T e) => true);
        }

        public void AndNothingElse()
        {
            Assert.AreEqual(_index, _actualEvents.Count, "Expected at most {0} events but found {1}", _index, _actualEvents.Count);
        }
    }
}