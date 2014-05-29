using Octopus.Client;
using Pipelines;
using ReleaseManager.Model;
using ReleaseManager.Process.Octopus.Model;

namespace ReleaseManager.Process.Octopus.Steps
{
    public class WaitForDeploymentFinish : Step
    {
        private readonly IOctopusRepository _octopusRepository;

        public WaitForDeploymentFinish(UniqueStepId stepId, IOctopusRepository octopusRepository)
            : base(stepId)
        {
            _octopusRepository = octopusRepository;
        }

        protected override bool Resume(IUnitOfWork unitOfWork)
        {
            var candidate = unitOfWork.LoadSubject<ReleaseCandidate>();
            var release = unitOfWork.LoadSubject<OctopusRelease>();

            var deployment = release.WaitForDeploymentToFinish(StepId.UniqueActivityId.ToString(), _octopusRepository);
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