using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pipelines.Schema;

namespace Pipelines
{
    public class PipelineCreatedEvent
    {
        [NotNull]
        private readonly UniquePipelineId _pipelineId;
        [NotNull]
        private readonly PipelineSchema _schema;

        public PipelineCreatedEvent([NotNull]UniquePipelineId pipelineId, [NotNull]PipelineSchema schema)
        {
            if (pipelineId == null) throw new ArgumentNullException("pipelineId");
            if (schema == null) throw new ArgumentNullException("schema");
            _pipelineId = pipelineId;
            _schema = schema;
        }

        [NotNull]
        public UniquePipelineId PipelineId
        {
            get { return _pipelineId; }
        }

        [NotNull]
        public PipelineSchema Schema
        {
            get { return _schema; }
        }
    }
}