using System.Linq;
using NHibernate;
using Pipelines;
using Pipelines.Infrastructure;
using ReleaseManager.Extensibility;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class ProjectReadModel : IEventHandler
    {
        public void On(EventEnvelope<PipelineCreatedEvent> evnt)
        {
            var payload = evnt.Payload;
            var existingProject = GetLatestProjectVersion(evnt.Session, payload.PipelineId.SchemaName);

            if (existingProject != null)
            {
                if (existingProject.SchemaVersion < payload.Schema.Revision)
                {
                    var newVersion = existingProject.CreateNewVersion(payload.Schema);
                    evnt.Session.Save(newVersion);
                }
            }
            else
            {
                var newProject = Project.CreateNew(payload.Schema.Name, payload.Schema);
                evnt.Session.Save(newProject);
            }
        }


        public void On(EventEnvelope<StageFinishedEvent> evnt)
        {
            var candidate = evnt.Session.QueryOver<ReleaseCandidate>()
                .Where(x => x.PipelineId == evnt.Payload.StageId.PipelineId)
                .List()
                .Single();

            var project = GetLatestProjectVersion(evnt.Session, candidate.ProjectName);
            project.StageFinished(evnt.Payload.StageId.StageId, candidate, evnt.OccurenceDateUtc);
        }
        
        public void On(EventEnvelope<StageFailedEvent> evnt)
        {
            var candidate = evnt.Session.QueryOver<ReleaseCandidate>()
                .Where(x => x.PipelineId == evnt.Payload.StageId.PipelineId)
                .List()
                .Single();

            var project = GetLatestProjectVersion(evnt.Session, candidate.ProjectName);
            project.StageFailed(evnt.Payload.StageId.StageId, candidate, evnt.OccurenceDateUtc);
        }

        private Project GetLatestProjectVersion(ISession session, string projectName)
        {
            var existingProject = session.QueryOver<Project>()
                .Where(x => x.Name == projectName && x.IsLatestVersion)
                .SingleOrDefault();
            return existingProject;
        }

    }
}