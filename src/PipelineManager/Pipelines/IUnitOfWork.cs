using System.Collections;
using System.Collections.Generic;

namespace Pipelines
{
    public interface IUnitOfWork : IEventSink
    {
        T LoadSubject<T>()
            where T : PipelineSubject, new();
        IEnumerable<object> UncommittedEvents { get; }
        int Version { get; }
    }
}