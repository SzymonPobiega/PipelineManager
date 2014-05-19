using Pipelines;
using Pipelines.Web;

namespace ReleaseManager.Process.TeamCity
{
    public class TeamCityBuildSucceededWebHookController :
        WebHookInputTransformerController<XmlSerializerInputTransformer<TeamCityBuildSucceededNotification>, TeamCityBuildSucceededNotification>
    {
        public TeamCityBuildSucceededWebHookController(IPipelineHost host) : base(host)
        {
        }

        protected override string ExtractPipelineId(TeamCityBuildSucceededNotification data, string correlationId)
        {
            return data.ProjectName + "_" + data.BuildNumber;
        }
    }
}