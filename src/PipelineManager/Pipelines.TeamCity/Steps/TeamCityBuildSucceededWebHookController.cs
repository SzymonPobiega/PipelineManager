using Pipelines;
using Pipelines.Web;

namespace ReleaseManager.Process.TeamCity.Steps
{
    public class TeamCityBuildSucceededWebHookController :
        WebHookInputTransformerController<XmlSerializerInputTransformer<TeamCityBuildFinishedNotification>, TeamCityBuildFinishedNotification>
    {
        public TeamCityBuildSucceededWebHookController(IPipelineHost host) : base(host)
        {
        }

        protected override string ExtractPipelineId(TeamCityBuildFinishedNotification data, string correlationId)
        {
            return data.ProjectName + "_" + data.BuildNumber;
        }
    }
}