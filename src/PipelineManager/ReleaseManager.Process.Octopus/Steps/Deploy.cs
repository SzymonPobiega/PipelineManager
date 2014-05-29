using Octopus.Client;
using Pipelines;
using ReleaseManager.Process.Octopus.Model;

namespace ReleaseManager.Process.Octopus.Steps
{
    public class Deploy : Step
    {
        private readonly IOctopusRepository _octopusRepository;

        public string Environment { get; set; }

        public Deploy(UniqueStepId stepId, IOctopusRepository octopusRepository) 
            : base(stepId)
        {
            _octopusRepository = octopusRepository;
        }

        protected override bool Resume(IUnitOfWork unitOfWork)
        {
            var release = unitOfWork.LoadSubject<OctopusRelease>();

            release.RequestDeploymentTo(Environment, StepId.UniqueActivityId.ToString(), _octopusRepository);
            return true;
        }
    }
}
