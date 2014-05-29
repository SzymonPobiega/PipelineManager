using System;
using Pipelines;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class Stage
    {
        public virtual string Name { get; protected set; }
        public virtual StageTriggerMode TriggerMode { get; protected set; }
        public virtual ReleaseCandidate LastVersion { get; protected set; }
        public virtual ReleaseCandidate LastSuccessfulVersion { get; protected set; }
        public virtual DateTime LastActivityDateUtc { get; protected set; }
        public virtual bool Busy { get; protected set; }

        public Stage(string stageName, StageTriggerMode triggerMode)
        {
            Name = stageName;
            TriggerMode = triggerMode;
            LastActivityDateUtc = DateTime.UtcNow;
        }

        public virtual void FinishedBy(ReleaseCandidate candidate, DateTime occurenceDateUtc)
        {
            LastActivityDateUtc = occurenceDateUtc;
            LastVersion = candidate;
            LastSuccessfulVersion = candidate;
        }


        public virtual void FailedBy(ReleaseCandidate candidate, DateTime occurenceDateUtc)
        {
            LastActivityDateUtc = occurenceDateUtc;
            LastVersion = candidate;
        }

        public virtual void Lock()
        {
            if (Busy)
            {
                throw new InvalidOperationException("Cannot lock a busy stage.");
            }
            Busy = true;
        }

        public virtual void Release()
        {
            Busy = false;
        }

        protected Stage()
        {
        }

    }
}