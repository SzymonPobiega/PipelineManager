using Pipelines;
using ReleaseManager.Model;
using ReleaseManager.Process.TeamCity.Events;

namespace ReleaseManager.Process.TeamCity.Steps
{
    public class TeamCityBuildFinishedListener : Step<TeamCityBuildFinishedNotification>
    {
        public TeamCityBuildFinishedListener(UniqueStepId stepId)
            : base(stepId)
        {
        }

        protected override bool Resume(IUnitOfWork unitOfWork, TeamCityBuildFinishedNotification data)
        {
            var newCandidate = unitOfWork.LoadSubject<ReleaseCandidate>();
            newCandidate.Create(data.BuildNumber, data.ProjectName, data.BuildId);
            unitOfWork.On(new TeamCityBuildFinishedEvent(data.BuildId, data.BuildResult == BuildResult.Success, data.BuildStatusUrl, data.BuildStatusHtml, data.Message));

            return data.BuildResult == BuildResult.Success;
        }
    }
}
