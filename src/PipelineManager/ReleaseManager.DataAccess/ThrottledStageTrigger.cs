using System;
using System.Linq;
using NHibernate;
using Pipelines;
using Pipelines.Infrastructure;
using ReleaseManager.DataAccess.ReadModels;
using ReleaseManager.Extensibility;
using IEventHandler = ReleaseManager.Extensibility.IEventHandler;

namespace ReleaseManager.DataAccess
{
    public class ThrottledStageTrigger : IEventHandler
    {
        public void On(EventEnvelope<StageFinishedEvent> evnt)
        {
            var project = GetLatestProjectVersion(evnt.Session, evnt.Payload.StageId.SchemaName);
            var stage = project.Stages.First(x => x.Name == evnt.Payload.StageId.StageId);

            if (stage.TriggerMode == StageTriggerMode.Throttled)
            {
                stage.Release();
            }

            var stageIndex = project.Stages.IndexOf(stage);
            if (stageIndex < project.Stages.Count - 1) //Not a last stage
            {
                var nextStage = project.Stages[stageIndex + 1];

                if (nextStage.TriggerMode == StageTriggerMode.Throttled && !nextStage.Busy)
                {
                    nextStage.Lock();
                    evnt.EnqueueCommand(new TriggerCommand(evnt.PipelineId), DateTime.UtcNow);
                }
            }
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