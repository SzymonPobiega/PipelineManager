using System;
using Pipelines;

namespace ReleaseManager.Host
{
    public class LoggingEventSink : IEventSink
    {
        public void On(object evnt)
        {
            Console.WriteLine(evnt.GetType());
        }
    }
}