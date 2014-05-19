using System;
using JetBrains.Annotations;

namespace Pipelines
{
    public class UniqueStageId
    {
        [NotNull]
        private readonly string _stageId;
        [NotNull]
        private readonly string _pipelineId;
        [NotNull]
        private readonly string _schemaName;

        public UniqueStageId([NotNull]string stageId, [NotNull]string pipelineId, [NotNull] string schemaName)
        {
            if (stageId == null) throw new ArgumentNullException("stageId");
            if (pipelineId == null) throw new ArgumentNullException("pipelineId");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            _stageId = stageId;
            _pipelineId = pipelineId;
            _schemaName = schemaName;
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

        [NotNull]
        public UniqueActivityId MakeActivityId(string localId)
        {
            return new UniqueActivityId(localId, _stageId, _pipelineId, SchemaName);
        }

        protected bool Equals(UniqueStageId other)
        {
            return string.Equals(_stageId, other._stageId) && string.Equals(_pipelineId, other._pipelineId) && string.Equals(SchemaName, other.SchemaName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UniqueStageId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _stageId.GetHashCode();
                hashCode = (hashCode*397) ^ _pipelineId.GetHashCode();
                hashCode = (hashCode*397) ^ SchemaName.GetHashCode();
                return hashCode;
            }
        }
    }
}