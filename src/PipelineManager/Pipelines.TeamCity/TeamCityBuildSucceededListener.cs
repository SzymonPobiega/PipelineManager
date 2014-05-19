using Pipelines;
using ReleaseManager.Model;

namespace ReleaseManager.Process.TeamCity
{
    public class TeamCityBuildSucceededListener : Step<TeamCityBuildSucceededNotification>
    {
        public TeamCityBuildSucceededListener(UniqueStepId stepId)
            : base(stepId)
        {
        }

        protected override void Resume(IUnitOfWork unitOfWork, TeamCityBuildSucceededNotification data)
        {
            var newCandidate = unitOfWork.LoadSubject<ReleaseCandidate>();
            newCandidate.Create(data.BuildNumber, data.ProjectName, data.BuildId);
        }
    }
}
