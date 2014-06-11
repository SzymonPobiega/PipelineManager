using System;
using System.Linq;
using Pipelines.Events;

namespace Pipelines
{
    public class Activity
    {
        private int _currentStep;
        private readonly BaseStep[] _steps;
        private readonly string _activityId;
        private int _failures;
        private DateTime? _waitingForExternalDependenciesBeginTime;
        private ActivityState _state = ActivityState.NotStarted;
        private readonly IFailureHandlingStrategy _failureHandlingStrategy;

        public Activity(string activityId, IFailureHandlingStrategy failureHandlingStrategy, params BaseStep[] steps)
        {
            _activityId = activityId;
            _failureHandlingStrategy = failureHandlingStrategy;
            _steps = steps;
            _waitingForExternalDependenciesBeginTime = null;
        }

        public string ActivityId
        {
            get { return _activityId; }
        }

        public ActivityState State
        {
            get { return _state; }
        }

        public void Resume(IUnitOfWork eventSink, DataContainer optionalData, DateTime currentTimeUtc)
        {
            var currentStep = _currentStep;
            foreach (var step in _steps.Skip(currentStep))
            {
                var retryTime = currentTimeUtc - _waitingForExternalDependenciesBeginTime ?? TimeSpan.FromSeconds(0);
                var result = step.Resume(eventSink, optionalData, retryTime);
                if (result == StepExecutionResult.WaitingForExternalDependency)
                {
                    eventSink.On(new StepWaitingForExternalDependencyEvent(step.StepId, currentTimeUtc));
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
            _failures = 0;
            _state = _currentStep == _steps.Length
                ? ActivityState.Finished
                : ActivityState.WaitingForExternalDependency;
        }

        public void On(StepWaitingForExternalDependencyEvent evnt)
        {
            _waitingForExternalDependenciesBeginTime = _waitingForExternalDependenciesBeginTime ?? evnt.ResumeTimeUtc;
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