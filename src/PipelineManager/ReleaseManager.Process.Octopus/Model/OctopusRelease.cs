using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using Pipelines;

namespace ReleaseManager.Process.Octopus.Model
{
    public class OctopusRelease : PipelineSubject
    {
        private string _releaseId;
        private readonly List<Deployment> _deployments = new List<Deployment>();

        public void ReleaseCreated(string releaseId)
        {
            Apply(new ReleaseCreatedEvent(releaseId));
        }

        public void RequestDeploymentTo(string environment, string correlationId, IOctopusRepository octopusRepository)
        {
            var environmentResource = octopusRepository.Environments.FindByName(environment);

            var deployment = octopusRepository.Deployments.Create(new DeploymentResource
            {
                EnvironmentId = environmentResource.Id,
                ReleaseId = _releaseId,
            });
            Apply(new DeploymentRequestedEvent(deployment.Id, environment, correlationId, deployment.TaskId, deployment.Links["Self"].AsString()));
        }


        public Deployment WaitForDeploymentToFinish(string correlationId, IOctopusRepository octopusRepository)
        {
            var deployment = _deployments.First(x => x.CorrelationId == correlationId);
            var task = octopusRepository.Tasks.Get(deployment.TaskId);

            octopusRepository.Tasks.WaitForCompletion(task);
            var updated = octopusRepository.Tasks.Get(deployment.TaskId);

            var result = updated.FinishedSuccessfully ? DeploymentResult.Succeeded : DeploymentResult.Failed;

            Apply(new DeploymentFinishedEvent(deployment.Id, result));
            return deployment;
        }

        public void On(ReleaseCreatedEvent evnt)
        {
            _releaseId = evnt.ReleaseId;
        }

        public void On(DeploymentRequestedEvent evnt)
        {
            _deployments.Add(new Deployment(evnt.DeploymentId, evnt.Environment,  evnt.CorrelationId, evnt.TaskId));
        }

        public void On(DeploymentFinishedEvent evnt)
        {
            _deployments.First(x => x.Id == evnt.DeploymentId).Finished(evnt.Result);
        }
    }
}