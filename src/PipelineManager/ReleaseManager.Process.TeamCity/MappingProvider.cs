using System;
using System.Collections.Generic;
using ReleaseManager.Extensibility;
using ReleaseManager.Process.TeamCity.Data;

namespace ReleaseManager.Process.TeamCity
{
    public class MappingProvider : IMappingProvider
    {
        public IEnumerable<Type> GetMappings()
        {
            yield return typeof (TeamCityBuild.TeamCityBuildMapping);
        }
    }
}