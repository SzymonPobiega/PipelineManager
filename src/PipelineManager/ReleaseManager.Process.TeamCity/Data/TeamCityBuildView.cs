using Pipelines.Infrastructure;
using ReleaseManager.Extensibility;
using ReleaseManager.Process.TeamCity.Events;

namespace ReleaseManager.Process.TeamCity.Data
{
    public class TeamCityBuildView : IEventHandler
    {
        public void On(EventEnvelope<TeamCityBuildFinishedEvent> evnt)
        {
            evnt.Session.Save(new TeamCityBuild(evnt.Payload.BuildId, evnt.Payload.StatusUrl, evnt.Payload.StatusHtml,
                evnt.Payload.Message));
        }
    }
}