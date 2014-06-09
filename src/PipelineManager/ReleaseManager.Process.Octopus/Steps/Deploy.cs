using Octopus.Client;
using Pipelines;
using ReleaseManager.Process.Octopus.Model;

namespace ReleaseManager.Process.Octopus.Steps
{
    public class Deploy : Step
    {
        private readonly IOctopusFacade _octopusFacade;

        public string Environment { get; set; }

        public Deploy(UniqueStepId stepId, IOctopusFacade octopusFacade) 
            : base(stepId)
        {
            _octopusFacade = octopusFacade;
        }

        protected override bool Resume(IUnitOfWork unitOfWork)
        {
            var release = unitOfWork.LoadSubject<OctopusRelease>();

            release.RequestDeploymentTo(Environment, StepId.UniqueActivityId.ToString(), _octopusFacade);
            return true;
        }
    }
}
