using System;

namespace Pipelines.Schema.Builders
{
    public class StepBuilder : IStepBuilder, IActivityBuilder, IStageBuilder, IPipelineBuilder
    {
        private readonly ActivityBuilder _parent;
        private readonly StepSchema _stepSchema;

        public StepBuilder(ActivityBuilder activityBuilder, StepSchema stepSchema)
        {
            _parent = activityBuilder;
            _stepSchema = stepSchema;
        }

        public IStepBuilder AddStep<T>(Action<UniqueStepId> callback)
        {
            return _parent.AddStep<T>(callback);
        }

        public IStepBuilder AddStep<T>()
        {
            return _parent.AddStep<T>();
        }

        public IStepBuilder WithParameter(string name, object value)
        {
            _stepSchema.Parameters.Add(new StepParameter(){Name = name, Value = value});
            return this;
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
            return _parent.BuildActivity();
        }
    }
}