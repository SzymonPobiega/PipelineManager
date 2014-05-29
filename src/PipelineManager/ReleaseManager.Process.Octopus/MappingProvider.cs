using System;
using System.Collections.Generic;
using ReleaseManager.Extensibility;
using ReleaseManager.Process.Octopus.DataAccess;

namespace ReleaseManager.Process.Octopus
{
    public class MappingProvider : IMappingProvider
    {
        public IEnumerable<Type> GetMappings()
        {
            yield return typeof (OctopusDeploymentDetails.OctopusDeploymentDetailsMapping);
        }
    }
}