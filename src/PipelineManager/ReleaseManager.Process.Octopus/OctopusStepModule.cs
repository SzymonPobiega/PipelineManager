using Autofac;
using Octopus.Client;
using ReleaseManager.Extensibility;
using ReleaseManager.Process.Octopus.Steps;

namespace ReleaseManager.Process.Octopus
{
    public class OctopusStepModule : StepModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<CreateRelease>();
            builder.RegisterType<Deploy>();
            builder.RegisterType<WaitForDeploymentFinish>();
            var repo = new OctopusRepository(new OctopusServerEndpoint("http://localhost:8070", "API-9WCGTKESXNXNBFHDZB2DROC28G"));
            builder.RegisterInstance(repo).AsImplementedInterfaces();
            builder.RegisterType<OctopusFacade>().AsImplementedInterfaces();
        }
    }
}