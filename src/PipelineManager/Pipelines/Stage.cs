using System;
using System.Linq;
using Pipelines.Events;

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
                : StageState.OnHold;
        }

        public string StageId
        {
            get { return _stageId.StageId; }
        }

        public StageState Resume(IUnitOfWork eventSink, DataContainer optionalData, DateTime currentTimeUtc)
        {
            if (_state == StageState.OnHold || _state == StageState.Failed)
            {
                return _state;
            }
            foreach (var activity in _activities)
            {
                activity.Resume(eventSink, optionalData, currentTimeUtc);
            }

            if (_activities.All(x => x.State == ActivityState.Finished))
            {
                eventSink.On(new StageFinishedEvent(_stageId));
            }
            else if (_activities.Any(x => x.State == ActivityState.Failed))
            {
                eventSink.On(new StageFailedEvent(_stageId));
            }
            return _state;
        }

        public void Trigger()
        {
            if (_state != StageState.OnHold)
            {
                throw new InvalidOperationException("Stage is in an invalid state for triggering.");
            }
            _state = StageState.NotStarted;
        }

        public void On(StepExecutedEvent evnt)
        {
            var activity = _activities.First(x => x.ActivityId == evnt.StepId.ActivityId);
            activity.On(evnt);

            UpdateState();
        }

        public void On(StepWaitingForExternalDependencyEvent evnt)
        {
            var activity = _activities.First(x => x.ActivityId == evnt.StepId.ActivityId);
            activity.On(evnt);

            UpdateState();
        }

        public void On(StepFailedEvent evnt)
        {
            var activity = _activities.First(x => x.ActivityId == evnt.StepId.ActivityId);
            activity.On(evnt);

            UpdateState();
        } 
        
        public void On(StepAttemptFailedEvent evnt)
        {
            var activity = _activities.First(x => x.ActivityId == evnt.StepId.ActivityId);
            activity.On(evnt);

            UpdateState();
        }

        private void UpdateState()
        {
            _state = _activities.All(x => x.State == ActivityState.Finished)
                ? StageState.Finished
                : _activities.Any(x => x.State == ActivityState.Failed)
                    ? StageState.Failed
                    : _activities.Any(x => x.State == ActivityState.Failing)
                        ? StageState.RequestsRetry
                        : StageState.WaitingForDependency;
        }
    }
}