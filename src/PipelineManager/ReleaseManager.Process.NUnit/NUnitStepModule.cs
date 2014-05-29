using Autofac;
using Pipelines;
using ReleaseManager.Extensibility;

namespace ReleaseManager.Process.NUnit
{
    public class NUnitStepModule : StepModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<LoadTestResults>();
        }
    }
}