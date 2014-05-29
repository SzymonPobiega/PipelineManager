using System;
using System.Collections.Generic;

namespace ReleaseManager.Extensibility
{
    public interface IMappingProvider
    {
        IEnumerable<Type> GetMappings();
    }
}