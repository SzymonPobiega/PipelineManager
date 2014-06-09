using Octopus.Client;
using Pipelines;
using ReleaseManager.Model;
using ReleaseManager.Process.Octopus.Model;

namespace ReleaseManager.Process.Octopus.Steps
{
    public class WaitForDeploymentFinish : Step
    {
        private readonly IOctopusFacade _octopusFacade;

        public WaitForDeploymentFinish(UniqueStepId stepId, IOctopusFacade octopusFacade)
            : base(stepId)
        {
            _octopusFacade = octopusFacade;
        }

        protected override bool Resume(IUnitOfWork unitOfWork)
        {
            var candidate = unitOfWork.LoadSubject<ReleaseCandidate>();
            var release = unitOfWork.LoadSubject<OctopusRelease>();

            var deployment = release.WaitForDeploymentToFinish(StepId.UniqueActivityId.ToString(), _octopusFacade);
            if (deployment.Result == DeploymentResult.Succeeded)
            {
                candidate.Deployed(deployment.Environment, deployment.Id);
                return true;
            }
            candidate.DeploymentFailed(deployment.Environment, deployment.Id);
            return false;
        }
    }
}