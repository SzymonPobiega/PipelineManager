using Autofac;
using ReleaseManager.Extensibility;

namespace Pipelines.Infrastructure
{
    public class InfrastructureRuntimeModule : RuntimeModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<InfrastructureMappingProvider>();
        }
    }
}