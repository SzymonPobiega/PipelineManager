using System.Linq;

namespace Pipelines
{
    public class Activity
    {
        private int _currentStep;
        private readonly BaseStep[] _steps;
        private readonly string _activityId;
        private int _failures;
        private ActivityState _state = ActivityState.NotStarted;
        private readonly IFailureHandlingStrategy _failureHandlingStrategy;

        public Activity(string activityId, IFailureHandlingStrategy failureHandlingStrategy, params BaseStep[] steps)
        {
            _activityId = activityId;
            _failureHandlingStrategy = failureHandlingStrategy;
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
                if (result == StepExecutionResult.Fail)
                {
                    eventSink.On(new StepAttemptFailedEvent(step.StepId));
                    if (!_failureHandlingStrategy.ShouldRetry(step, _failures))
                    {
                        eventSink.On(new StepFailedEvent(step.StepId));
                    }
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

        public void On(StepAttemptFailedEvent evnt)
        {
            _state = ActivityState.Failing;
            _failures++;
        }
        
        public void On(StepFailedEvent evnt)
        {
            _state = ActivityState.Failed;
            _failures++;
        }
    }
}