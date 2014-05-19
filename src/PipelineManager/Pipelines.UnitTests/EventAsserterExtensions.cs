using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public static class EventAsserterExtensions
    {
        public static void ExpectNothing(this IEnumerable<object> events)
        {
            var asserter = new EventAsserter(events.ToList());
            asserter.AndNothingElse();
        }

        public static EventAsserter Expect(this IEnumerable<object> events, object firstExpectedEvent)
        {
            var asserter = new EventAsserter(events.ToList());
            asserter.ThenExpect(firstExpectedEvent);
            return asserter;
        }

        public static EventAsserter Expect<T>(this IEnumerable<object> events, Func<T, bool> firstExpectedEventSpec)
        {
            var asserter = new EventAsserter(events.ToList());
            asserter.ThenExpect(firstExpectedEventSpec);
            return asserter;
        }

        public static EventAsserter ExpectAny<T>(this IEnumerable<object> events)
        {
            return Expect(events, (T e) => true);
        }

    }
}