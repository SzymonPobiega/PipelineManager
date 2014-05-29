using System.Linq;
using Pipelines.Infrastructure;
using ReleaseManager.Events;
using ReleaseManager.Extensibility;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class ReleaseCandidateReadModel : IEventHandler
    {
        public void On(EventEnvelope<ReleaseCandidateCreatedEvent> evnt)
        {
            var rc = new ReleaseCandidate(evnt.Payload.BuildId, evnt.Payload.ProjectName, evnt.Payload.VersionNumber, evnt.PipelineId);
            evnt.Session.Save(rc);
        }

        public void On(EventEnvelope<TestSuiteFinishedEvent> evnt)
        {
            var rc = evnt.Session.Load<ReleaseCandidate>(evnt.Payload.BuildId);

            var testCases = evnt.Payload.TestOutputs.Select(x => new TestCase(x.Name, x.Result));
            rc.TestSuites.Add(new TestSuite(evnt.Payload.SuiteType, evnt.Payload.Result, testCases));
        }

        public void On(EventEnvelope<ReleaseCandidateDeployedEvent> evnt)
        {
            var rc = evnt.Session.Load<ReleaseCandidate>(evnt.Payload.BuildId);
            
            rc.Deployments.Add(new Deployment()
            {
                Date = evnt.OccurenceDateUtc,
                UniqueId = evnt.Payload.UniqueDeploymentId,
                Environment = evnt.Payload.Environment,
                Success = true
            });
        }
        
        public void On(EventEnvelope<ReleaseCandidateDeploymentFailedEvent> evnt)
        {
            var rc = evnt.Session.Load<ReleaseCandidate>(evnt.Payload.BuildId);

            rc.Deployments.Add(new Deployment()
            {
                Date = evnt.OccurenceDateUtc,
                UniqueId = evnt.Payload.UniqueDeploymentId,
                Environment = evnt.Payload.Environment,
                Success = false
            });
        }
    }
}