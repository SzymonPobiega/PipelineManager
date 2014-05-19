using System;
using JetBrains.Annotations;

namespace Pipelines
{
    public class UniqueStepId
    {
        [NotNull]
        private readonly string _stepId;
        [NotNull]
        private readonly string _activityId;
        [NotNull]
        private readonly string _stageId;
        [NotNull]
        private readonly string _pipelineId;
        [NotNull]
        private readonly string _schemaName;

        public UniqueStepId([NotNull]string stepId, [NotNull]string activityId, [NotNull]string stageId, [NotNull]string pipelineId, [NotNull]string schemaName)
        {
            if (stepId == null) throw new ArgumentNullException("stepId");
            if (activityId == null) throw new ArgumentNullException("activityId");
            if (stageId == null) throw new ArgumentNullException("stageId");
            if (pipelineId == null) throw new ArgumentNullException("pipelineId");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            _stepId = stepId;
            _activityId = activityId;
            _stageId = stageId;
            _pipelineId = pipelineId;
            _schemaName = schemaName;
        }

        [NotNull]
        public string StepId
        {
            get { return _stepId; }
        }

        [NotNull]
        public string ActivityId
        {
            get { return _activityId; }
        }

        [NotNull]
        public string StageId
        {
            get { return _stageId; }
        }

        [NotNull]
        public string PipelineId
        {
            get { return _pipelineId; }
        }

        [NotNull]
        public string SchemaName
        {
            get { return _schemaName; }
        }

        protected bool Equals(UniqueStepId other)
        {
            return string.Equals(_stepId, other._stepId) && string.Equals(_activityId, other._activityId) && string.Equals(_stageId, other._stageId) && string.Equals(_pipelineId, other._pipelineId) && string.Equals(_schemaName, other._schemaName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UniqueStepId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _stepId.GetHashCode();
                hashCode = (hashCode*397) ^ _activityId.GetHashCode();
                hashCode = (hashCode*397) ^ _stageId.GetHashCode();
                hashCode = (hashCode*397) ^ _pipelineId.GetHashCode();
                hashCode = (hashCode*397) ^ _schemaName.GetHashCode();
                return hashCode;
            }
        }
    }
}