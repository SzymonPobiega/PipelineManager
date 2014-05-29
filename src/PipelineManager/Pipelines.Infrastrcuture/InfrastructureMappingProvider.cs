using System;
using System.Collections.Generic;
using Pipelines.Infrastructure.Records;
using ReleaseManager.Extensibility;

namespace Pipelines.Infrastructure
{
    public class InfrastructureMappingProvider : IMappingProvider
    {
        public IEnumerable<Type> GetMappings()
        {
            yield return typeof (EventRecord.Mapping);
            yield return typeof (CommandRecord.Mapping);
            yield return typeof (CommandProcessingResultRecord.Mapping);
        }
    }
}