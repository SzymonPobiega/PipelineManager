using System;
using JetBrains.Annotations;

namespace Pipelines
{
    public class UniquePipelineId
    {
        [NotNull]
        private readonly string _pipelineId;
        [NotNull]
        private readonly string _schemaName;

        public UniquePipelineId([NotNull] string pipelineId, [NotNull] string schemaName)
        {
            if (pipelineId == null) throw new ArgumentNullException("pipelineId");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            _pipelineId = pipelineId;
            _schemaName = schemaName;
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

        protected bool Equals(UniquePipelineId other)
        {
            return string.Equals(_pipelineId, other._pipelineId) && string.Equals(_schemaName, other._schemaName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UniquePipelineId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_pipelineId.GetHashCode()*397) ^ _schemaName.GetHashCode();
            }
        }
    }
}