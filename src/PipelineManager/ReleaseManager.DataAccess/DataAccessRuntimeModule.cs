using System;
using System.Collections.Generic;
using Autofac;
using ReleaseManager.DataAccess.ReadModels;
using ReleaseManager.Extensibility;
using Environment = ReleaseManager.DataAccess.ReadModels.Environment;

namespace ReleaseManager.DataAccess
{
    public class DataAccessRuntimeModule : RuntimeModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ReleaseCandidateReadModel>().AsImplementedInterfaces();
            builder.RegisterType<ProjectReadModel>().AsImplementedInterfaces();
            builder.RegisterType<EnvironmentReadModel>().AsImplementedInterfaces();
            builder.RegisterType<ThrottledStageTrigger>().AsImplementedInterfaces();
            builder.RegisterType<CoreMappingProvider>().AsImplementedInterfaces();
        }
    }

    public class CoreMappingProvider : IMappingProvider
    {
        public IEnumerable<Type> GetMappings()
        {
            yield return typeof (Environment.Mapping);
            yield return typeof (Project.Mapping);
            yield return typeof (ReleaseCandidate.Mapping);
            yield return typeof (TestSuite.Mapping);
        }
    }
}