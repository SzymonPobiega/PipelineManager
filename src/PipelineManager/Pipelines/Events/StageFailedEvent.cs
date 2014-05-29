using System;

namespace Pipelines
{
    public class StageFailedEvent
    {
        private readonly UniqueStageId _stageId;

        public StageFailedEvent(UniqueStageId stageId)
        {
            if (stageId == null) throw new ArgumentNullException("stageId");
            _stageId = stageId;
        }

        public UniqueStageId StageId
        {
            get { return _stageId; }
        }

        protected bool Equals(StageFailedEvent other)
        {
            return Equals(_stageId, other._stageId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StageFailedEvent)obj);
        }

        public override int GetHashCode()
        {
            return _stageId.GetHashCode();
        }
    }
}