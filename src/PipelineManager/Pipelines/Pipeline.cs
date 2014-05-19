using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pipelines
{
    public class Pipeline
    {
        private readonly string _pipelineId;
        private readonly Stage[] _stages;
        private int _currentStage;

        public Pipeline(string pipelineId, params Stage[] stages)
        {
            _pipelineId = pipelineId;
            _stages = stages;
        }

        public string Id
        {
            get { return _pipelineId; }
        }

        public StageState Run(IUnitOfWork unitOfWork, object optionalData)
        {
            var dataContainer = new DataContainer(optionalData);
            var publisherWrapper = new CompositeEventSink(unitOfWork, new DynamicEventSink(this));
            return _stages
                .Skip(_currentStage)
                .Select(step => step.Resume(publisherWrapper, dataContainer))
                .FirstOrDefault(result => result != StageState.Finished);
        }

        public void Trigger()
        {
            _stages[_currentStage].Trigger();
        }

        public void On(StepExecutedEvent evnt)
        {
            var stage = _stages.First(x => x.StageId == evnt.StepId.StageId);
            stage.On(evnt);
        }

        public void On(StageFinishedEvent evnt)
        {
            _currentStage++;
        }
    }
}