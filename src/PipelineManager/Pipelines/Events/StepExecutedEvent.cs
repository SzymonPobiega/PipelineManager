using System;

namespace Pipelines
{
    public class StepExecutedEvent
    {
        private readonly UniqueStepId _stepId;

        public StepExecutedEvent(UniqueStepId stepId)
        {
            if (stepId == null) throw new ArgumentNullException("stepId");
            _stepId = stepId;
        }

        public UniqueStepId StepId
        {
            get { return _stepId; }
        }

        protected bool Equals(StepExecutedEvent other)
        {
            return Equals(_stepId, other._stepId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StepExecutedEvent) obj);
        }

        public override int GetHashCode()
        {
            return _stepId.GetHashCode();
        }
    }
}