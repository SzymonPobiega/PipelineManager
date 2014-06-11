using System;
using System.Activities.Tracking;
using System.Threading;
using NUnit.Framework;
using Pipelines;
using Pipelines.Events;
using Pipelines.Schema.Builders;

namespace UnitTests
{
    [TestFixture]
    public class PipelineTests : EventDriventTest
    {

        [Test]
        public void Pipeline_without_external_dependencies_runs_all_steps_till_completion()
        {
            UniqueStepId firstStepId = null;
            UniqueStepId secondStepId = null;

            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(x => firstStepId = x)
                .AddStepWithoutDependencies(x => secondStepId = x)
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.Finished, result);

            Expect(new StepExecutedEvent(firstStepId))
                .ThenExpect(new StepExecutedEvent(secondStepId))
                .ThenExpectAny<StageFinishedEvent>()
                .AndNothingElse();
        }

        [Test]
        public void Pipeline_execution_stops_on_first_step_with_external_dependency()
        {
            UniqueStepId firstStepId = null;

            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(x => firstStepId = x)
                .AddStepWithDependencies()
                .AddStepWithoutDependencies()
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.WaitingForDependency, result);

            Expect(new StepExecutedEvent(firstStepId))
                 .AndNothingElse();
        }

        [Test]
        public void Pipeline_execution_stops_on_first_failed_step()
        {
            UniqueStepId firstStepId = null;
            UniqueStepId failedStepId = null;

            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(id => firstStepId = id)
                .AddStep<FailingStep>(id => failedStepId = id)
                .AddStepWithoutDependencies()
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.Failed, result);

            Expect(new StepExecutedEvent(firstStepId))
                .ThenExpect(new StepAttemptFailedEvent(failedStepId))
                .ThenExpect(new StepFailedEvent(failedStepId))
                .ThenExpectAny<StageFailedEvent>()
                .AndNothingElse();
        }

        [Test]
        public void Pipeline_requests_retry_when_allowed()
        {
            UniqueStepId firstStepId = null;
            UniqueStepId failedStepId = null;

            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(id => firstStepId = id)
                .AddStep<FailingStep>(id => failedStepId = id)
                .AddStepWithoutDependencies()
                .Build(new RetryOnceFailureHandlingStrategy());

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.RequestsRetry, result);

            Expect(new StepExecutedEvent(firstStepId))
                .ThenExpect(new StepAttemptFailedEvent(failedStepId))
                .AndNothingElse();
        }

        [Test]
        public void Two_steps_should_execute_correctly_after_failing_once_each()
        {            
            UniqueStepId firstRetryStepId = null;
            UniqueStepId secondRetryStepId = null;

            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()                
                .AddStep<SuccessAfterRetryStep>(id => firstRetryStepId = id)
                .AddStep<SuccessAfterRetryStep>(id => secondRetryStepId = id)                
                .Build(new RetryOnceFailureHandlingStrategy());

            pipeline.Run(EventSink, null, DateTime.UtcNow);
            pipeline.Run(EventSink, null, DateTime.UtcNow);
            pipeline.Run(EventSink, null, DateTime.UtcNow);

            Expect((new StepAttemptFailedEvent(firstRetryStepId)))
                .ThenExpect(new StepExecutedEvent(firstRetryStepId))
                .ThenExpect(new StepAttemptFailedEvent(secondRetryStepId))
                .ThenExpect(new StepExecutedEvent(secondRetryStepId))
                .ThenExpectAny<StageFinishedEvent>()
                .AndNothingElse();
        }

        [Test]
        public void Step_should_timeout_after_1_seconds()
        {
            UniqueStepId firstRetryStepId = null;

            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStep<WaitingForExternalDependencyStep>(id => firstRetryStepId = id)
                .Build(new NoRetryFailureHandlingStrategy());

            var startTime = DateTime.UtcNow;
            var retryTime = startTime + TimeSpan.FromSeconds(5);

            pipeline.Run(EventSink, null, startTime);            
            pipeline.Run(EventSink, null, retryTime);

            Expect((new StepWaitingForExternalDependencyEvent(firstRetryStepId, startTime)))
                .ThenExpect(new StepAttemptFailedEvent(firstRetryStepId))
                .ThenExpect(new StepFailedEvent(firstRetryStepId))
                .ThenExpectAny<StageFailedEvent>()
                .AndNothingElse();
        }

        [Test]
        public void Resuming_execution_of_a_failed_pipeline_fails()
        {
            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies()
                .AddStep<FailingStep>()
                .AddStepWithoutDependencies()
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.Failed, result);
            EventSink.Events.Clear();

            pipeline.Run(EventSink, null, DateTime.UtcNow);

            ExpectNothing();
        }

        [Test]
        public void Running_multi_stage_multi_activity_pipeline_that_waits_for_external_dependency_does_nothing()
        {
            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies()
                .AddStepWithDependencies()
                .AddStepWithoutDependencies()
                .AddActivity()
                .AddStepWithoutDependencies()
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.WaitingForDependency, result);
            EventSink.Events.Clear();

            pipeline.Run(EventSink, null, DateTime.UtcNow);

            ExpectNothing();
        }

        [Test]
        public void Execution_stops_before_stage_with_manual_trigger()
        {
            UniqueStepId firstStepId = null;
            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(x => firstStepId = x)
                .AddStage(StageTriggerMode.Manual)
                .AddActivity()
                .AddStepWithoutDependencies()
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.OnHold, result);

            Expect(new StepExecutedEvent(firstStepId))
                .ThenExpectAny<StageFinishedEvent>()
                .AndNothingElse();
        }

        [Test]
        public void Execution_stops_before_stage_with_throttled_trigger()
        {
            UniqueStepId firstStepId = null;
            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(x => firstStepId = x)
                .AddStage(StageTriggerMode.Throttled)
                .AddActivity()
                .AddStepWithoutDependencies()
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.OnHold, result);

            Expect(new StepExecutedEvent(firstStepId))
                .ThenExpectAny<StageFinishedEvent>()
                .AndNothingElse();
        }

        [Test]
        public void Manual_trigger_resumes_execution_of_pipeline()
        {
            UniqueStepId manualStepId = null;
            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies()
                .AddStage(StageTriggerMode.Manual)
                .AddActivity()
                .AddStepWithoutDependencies(x => manualStepId = x)
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.OnHold, result);
            EventSink.Events.Clear();

            pipeline.Trigger();
            pipeline.Run(EventSink, null, DateTime.UtcNow);

            Expect(new StepExecutedEvent(manualStepId))
                .ThenExpectAny<StageFinishedEvent>()
                .AndNothingElse();
        }

        [Test]
        public void Execution_continues_to_next_stage_with_automatic_trigger()
        {
            UniqueStepId firstStepId = null;
            UniqueStepId secondStepId = null;
            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(x => firstStepId = x)
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(x => secondStepId = x)
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.Finished, result);

            Expect(new StepExecutedEvent(firstStepId))
                .ThenExpectAny<StageFinishedEvent>()
                .ThenExpect(new StepExecutedEvent(secondStepId))
                .ThenExpectAny<StageFinishedEvent>()
                .AndNothingElse();
        }

        [Test]
        public void Multiple_activities_are_executed_concurrently()
        {
            UniqueStepId firstStepId = null;
            UniqueStepId secondStepId = null;
            var pipeline = new PipelineBuilder()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies(x => firstStepId = x)
                .AddStepWithDependencies()
                .AddActivity()
                .AddStepWithoutDependencies(x => secondStepId = x)
                .AddStepWithDependencies()
                .Build();

            var result = pipeline.Run(EventSink, null, DateTime.UtcNow);

            Assert.AreEqual(StageState.WaitingForDependency, result);

            Expect(new StepExecutedEvent(firstStepId))
                .ThenExpect(new StepExecutedEvent(secondStepId))
                .AndNothingElse();
        }

       
    }
}
