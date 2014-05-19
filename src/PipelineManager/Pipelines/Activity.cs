using System.Linq;

namespace Pipelines
{
    public class Activity
    {
        private int _currentStep;
        private readonly BaseStep[] _steps;
        private readonly string _activityId;
        private ActivityState _state = ActivityState.NotStarted;

        public Activity(string activityId, params BaseStep[] steps)
        {
            _activityId = activityId;
            _steps = steps;
        }

        public string ActivityId
        {
            get { return _activityId; }
        }

        public ActivityState State
        {
            get { return _state; }
        }

        public void Resume(IUnitOfWork eventSink, DataContainer optionalData)
        {
            var currentStep = _currentStep;

            foreach (var step in _steps.Skip(currentStep))
            {
                var result = step.Resume(eventSink, optionalData);
                if (result == StepExecutionResult.WaitingForExternalDependency)
                {
                    break;
                }
                eventSink.On(new StepExecutedEvent(step.StepId));
            }  
        }

        public void On(StepExecutedEvent evnt)
        {
            _currentStep++;

            _state = _currentStep == _steps.Length
                ? ActivityState.Finished
                : ActivityState.WaitingForExternalDependency;
        }

    }
}