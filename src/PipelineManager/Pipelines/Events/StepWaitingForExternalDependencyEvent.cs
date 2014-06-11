using System;

namespace Pipelines.Events
{
    public class StepWaitingForExternalDependencyEvent
    {
        private readonly UniqueStepId _stepId;
        private DateTime? _resumeTimeUtc;

        public StepWaitingForExternalDependencyEvent(UniqueStepId stepId, DateTime currentTimeUtc)
        {
            if (stepId == null) throw new ArgumentNullException("stepId");
            _stepId = stepId;
            _resumeTimeUtc = currentTimeUtc;
        }

        public UniqueStepId StepId
        {
            get { return _stepId; }
        }

        public DateTime? ResumeTimeUtc {
            get { return _resumeTimeUtc; }
        }

        protected bool Equals(StepWaitingForExternalDependencyEvent other)
        {
            return Equals(_stepId, other._stepId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StepWaitingForExternalDependencyEvent)obj);
        }

        public override int GetHashCode()
        {
            return _stepId.GetHashCode();
        }
    }
}