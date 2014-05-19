using System.Collections.Generic;
using System.Linq;

namespace Pipelines.Schema.Builders
{
    public class StageBuilder : IStageBuilder
    {
        private readonly PipelineBuilder _parent;
        private readonly List<ActivityBuilder> _activityBuilders = new List<ActivityBuilder>();
        private int _counter;
        private readonly string _id;
        private readonly StageTriggerMode _triggerMode;

        public StageBuilder(PipelineBuilder parent, string id, StageTriggerMode triggerMode)
        {
            _parent = parent;
            _id = id;
            _triggerMode = triggerMode;
        }

        public PipelineBuilder Parent
        {
            get { return _parent; }
        }

        public string Id
        {
            get { return _id; }
        }

        public IActivityBuilder AddActivity()
        {
            _counter++;
            var activityBuilder = new ActivityBuilder(this, _counter.ToString());
            _activityBuilders.Add(activityBuilder);
            return activityBuilder;
        }

        public IStageBuilder AddStage(StageTriggerMode stageTriggerMode)
        {
            return _parent.AddStage(stageTriggerMode);
        }

        public IStageBuilder AddStage(string id, StageTriggerMode triggerMode)
        {
            return _parent.AddStage(id, triggerMode);
        }

        public StageSchema BuildStage()
        {
            return new StageSchema
            {
                Name = Id,
                TriggerMode = _triggerMode,
                Activities = _activityBuilders.Select(x => x.BuildActivity()).ToList()
            };
        }

        public PipelineSchema BuildSchema()
        {
            return _parent.BuildSchema();
        }
    }
}