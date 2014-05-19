using System;

namespace Pipelines
{
    public interface IEventSink
    {
        void On(object evnt);
    }
}