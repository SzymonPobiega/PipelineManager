using Autofac;
using Autofac.Integration.WebApi;
using ReleaseManager.Extensibility;

namespace ReleaseManager.Process.NUnit
{
    public class NUnitRuntimeModule : RuntimeModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterApiControllers(GetType().Assembly);
        }
    }
}