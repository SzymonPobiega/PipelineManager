using System;
using System.Collections.Generic;
using System.Linq;
using Pipelines.Schema;

namespace Pipelines
{
    public class PipelineFactory
    {
        private readonly IStepFactory _stepFactory;
        private readonly IStepPropertyValueProvider[] _stepPropertyValueProviders;

        public PipelineFactory(IStepFactory stepFactory, params IStepPropertyValueProvider[] stepPropertyValueProviders)
        {
            _stepFactory = stepFactory;
            _stepPropertyValueProviders = stepPropertyValueProviders;
        }

        public Pipeline Create(string pipelineId, PipelineSchema schema)
        {
            return new Pipeline(pipelineId, BuildStages(schema.Name, pipelineId, schema.Stages));
        }

        private Stage[] BuildStages(string schemaName, string pipelineId, IEnumerable<StageSchema> stageSchemas)
        {
            return stageSchemas.Select(x => BuildStage(schemaName, pipelineId, x)).ToArray();
        }

        private Stage BuildStage(string schemaName, string pipelineId, StageSchema stageSchema)
        {
            var stageId = new UniqueStageId(stageSchema.Name, pipelineId, schemaName);
            return new Stage(stageId, stageSchema.TriggerMode, BuildActivities(stageId, stageSchema.Activities));
        }

        private Activity[] BuildActivities(UniqueStageId stageId, IEnumerable<ActivitySchema> activitySchemas)
        {
            return activitySchemas.Select(x => BuildActivity(stageId, x)).ToArray();
        }

        private Activity BuildActivity(UniqueStageId stageId, ActivitySchema activitySchema)
        {
            var activityId = stageId.MakeActivityId(activitySchema.Name);
            return new Activity(activitySchema.Name, BuildSteps(activityId, activitySchema.Steps));
        }

        private BaseStep[] BuildSteps(UniqueActivityId activityId, IEnumerable<StepSchema> stepSchemas)
        {
            return stepSchemas.Select(x => BuildStep(activityId, x)).ToArray();
        }

        private BaseStep BuildStep(UniqueActivityId activityId, StepSchema stepSchema)
        {
            var stepId = activityId.MakeStepId(stepSchema.Name);
            return CreateStepInstance(stepSchema, stepId);
        }

        private BaseStep CreateStepInstance(StepSchema stepSchema, UniqueStepId stepId)
        {
            var stepType = Type.GetType(stepSchema.Implementation, true);
            var stepInstance = _stepFactory.CreateInstance(stepType, stepId);

            foreach (var publicProperty in stepType.GetProperties())
            {
                object value = null;
                if (AllValueProviders(stepSchema).Any(x => x.TryProvideValue(publicProperty.Name, out value)))
                {
                    publicProperty.SetValue(stepInstance, value);
                }
            }

            return stepInstance;
        }

        private IEnumerable<IStepPropertyValueProvider> AllValueProviders(StepSchema stepSchema)
        {
            yield return new StepSchemaValueProvider(stepSchema);
            foreach (var provider in _stepPropertyValueProviders)
            {
                yield return provider;
            }
        }
    }
}