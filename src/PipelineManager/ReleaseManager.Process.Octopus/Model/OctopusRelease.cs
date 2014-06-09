using System.Collections.Generic;
using System.Linq;
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

        public void RequestDeploymentTo(string environment, string correlationId, IOctopusFacade octopusFacade)
        {
            var environmentResource = octopusFacade.FindEnvironmentByName(environment);

            var deployment = octopusFacade.CreateDeployment(environmentResource.Id, _releaseId);
            Apply(new DeploymentRequestedEvent(deployment.Id, environment, correlationId, deployment.TaskId, deployment.Links["Self"].AsString()));
        }


        public Deployment WaitForDeploymentToFinish(string correlationId, IOctopusFacade octopusFacade)
        {
            var deployment = _deployments.First(x => x.CorrelationId == correlationId);

            var task = octopusFacade.WaitForTaskCompletion(deployment.TaskId);

            var result = task.FinishedSuccessfully ? DeploymentResult.Succeeded : DeploymentResult.Failed;

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