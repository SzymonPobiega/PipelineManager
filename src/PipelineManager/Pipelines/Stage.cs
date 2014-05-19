using System;
using System.Linq;

namespace Pipelines
{
    public class Stage
    {
        private readonly UniqueStageId _stageId;
        private readonly Activity[] _activities;
        private StageState _state;

        public Stage(UniqueStageId stageId, StageTriggerMode triggerMode, params Activity[] activities)
        {
            _stageId = stageId;
            _activities = activities;
            _state = triggerMode == StageTriggerMode.Automatic
                ? StageState.NotStarted
                : StageState.WaitingForManualTrigger;
        }

        public string StageId
        {
            get { return _stageId.StageId; }
        }

        public StageState Resume(IUnitOfWork eventSink, DataContainer optionalData)
        {
            if (_state == StageState.WaitingForManualTrigger)
            {
                return _state;
            }
            foreach (var activity in _activities)
            {
                activity.Resume(eventSink, optionalData);
            }

            if (_activities.All(x => x.State == ActivityState.Finished))
            {
                eventSink.On(new StageFinishedEvent(_stageId));
            }
            return _state;
        }

        public void Trigger()
        {
            if (_state != StageState.WaitingForManualTrigger)
            {
                throw new InvalidOperationException("Stage is in an invalid state for triggering.");
            }
            _state = StageState.NotStarted;
        }

        public void On(StepExecutedEvent evnt)
        {
            var activity = _activities.First(x => x.ActivityId == evnt.StepId.ActivityId);
            activity.On(evnt);

            _state = _activities.All(x => x.State == ActivityState.Finished)
                ? StageState.Finished
                : StageState.WaitingForDependency;
        }
    }
}