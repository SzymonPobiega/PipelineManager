using Pipelines.Infrastructure;
using ReleaseManager.Extensibility;
using IEventHandler = ReleaseManager.Extensibility.IEventHandler;

namespace ReleaseManager.Process.Octopus.DataAccess
{
    public class OctopusDeploymentDetailsView : IEventHandler
    {
        public void On(EventEnvelope<DeploymentRequestedEvent> evnt)
        {
            evnt.Session.Save(new OctopusDeploymentDetails(evnt.Payload.DeploymentId, evnt.Payload.DetailsLink));
        }
    }
}