using System;
using System.Collections.Generic;

namespace Pipelines.Schema.Builders
{
    public class ActivityBuilder : IActivityBuilder, IStageBuilder, IPipelineBuilder
    {
        private readonly string _id;
        private readonly StageBuilder _parent;
        private readonly List<StepSchema> _steps = new List<StepSchema>();
        private int _counter;

        public ActivityBuilder(StageBuilder parent, string id)
        {
            _parent = parent;
            _id = id;
        }

        public string Id
        {
            get { return _id; }
        }

        public List<StepSchema> Steps
        {
            get { return _steps; }
        }

        public IStepBuilder AddStep<T>(Action<UniqueStepId> callback)
        {
            _counter ++;
            var uniqueId = new UniqueStepId(_counter.ToString(), Id, _parent.Id, _parent.Parent.SchemaName, _parent.Parent.SchemaName);
            var stepSchema = new StepSchema()
            {
                Name = _counter.ToString(),
                Implementation = typeof(T).AssemblyQualifiedName
            };
            Steps.Add(stepSchema);
            callback(uniqueId);
            return new StepBuilder(this, stepSchema);
        }

        public IStepBuilder AddStep<T>()
        {
            return AddStep<T>(x => { });
        }

        public IActivityBuilder AddActivity()
        {
            return _parent.AddActivity();
        }

        public IStageBuilder AddStage(StageTriggerMode triggerMode)
        {
            return _parent.AddStage(triggerMode);
        }

        public IStageBuilder AddStage(string id, StageTriggerMode triggerMode)
        {
            return _parent.AddStage(id, triggerMode);
        }

        public PipelineSchema BuildSchema()
        {
            return _parent.BuildSchema();
        }

        public ActivitySchema BuildActivity()
        {
            return new ActivitySchema()
            {
                Name = Id,
                Steps = _steps
            };
        }
    }
}