using Pipelines.Infrastructure;
using ReleaseManager.Events;
using ReleaseManager.Extensibility;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class EnvironmentReadModel : IEventHandler
    {
        public void On(EventEnvelope<ReleaseCandidateDeployedEvent> evnt)
        {
            var environmentName = evnt.Payload.Environment;
            var environment = evnt.Session.Get<Environment>(environmentName);
            if (environment == null)
            {
                environment = new Environment(environmentName);
                evnt.Session.Save(environment);
            }
            var candidate = evnt.Session.Load<ReleaseCandidate>(evnt.Payload.BuildId);
            environment.CandidateDeployed(candidate);
        }
    }
}