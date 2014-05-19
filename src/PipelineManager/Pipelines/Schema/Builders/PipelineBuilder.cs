using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Pipelines.Schema.Builders
{
    public class PipelineBuilder : IPipelineBuilder
    {
        private int _counter;
        private readonly List<StageBuilder> _stageBuilders = new List<StageBuilder>();
        [NotNull]
        private readonly string _schemaName;
        private readonly int _revision;

        public string SchemaName
        {
            get { return _schemaName; }
        }

        public int Revision
        {
            get { return _revision; }
        }

        public PipelineBuilder([NotNull] string schemaName, int revision)
        {
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            _schemaName = schemaName;
            _revision = revision;
        }

        public PipelineBuilder()
            : this("Pipeline", 0)
        {
        }

        public IStageBuilder AddStage(StageTriggerMode triggerMode)
        {
            _counter++;
            return AddStage(_counter.ToString(), triggerMode);
        }

        public IStageBuilder AddStage(string id, StageTriggerMode triggerMode)
        {
            var stageBuilder = new StageBuilder(this, id, triggerMode);
            _stageBuilders.Add(stageBuilder);
            return stageBuilder;
        }

        public PipelineSchema BuildSchema()
        {
            var schema = new PipelineSchema()
            {
                Revision = _revision,
                Stages = _stageBuilders.Select(x => x.BuildStage()).ToList(),
                Name = _schemaName
            };
            return schema;
        }
    }
}