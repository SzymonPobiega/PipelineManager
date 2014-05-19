using NUnit.Framework;
using Pipelines;
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

            var result = pipeline.Run(EventSink, null);

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

            var result = pipeline.Run(EventSink, null);

            Assert.AreEqual(StageState.WaitingForDependency, result);

            Expect(new StepExecutedEvent(firstStepId))
                 .AndNothingElse();
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

            var result = pipeline.Run(EventSink, null);

            Assert.AreEqual(StageState.WaitingForDependency, result);
            EventSink.Events.Clear();

            pipeline.Run(EventSink, null);

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

            var result = pipeline.Run(EventSink, null);

            Assert.AreEqual(StageState.WaitingForManualTrigger, result);

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

            var result = pipeline.Run(EventSink, null);

            Assert.AreEqual(StageState.WaitingForManualTrigger, result);

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

            var result = pipeline.Run(EventSink, null);

            Assert.AreEqual(StageState.WaitingForManualTrigger, result);
            EventSink.Events.Clear();

            pipeline.Trigger();
            pipeline.Run(EventSink, null);

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

            var result = pipeline.Run(EventSink, null);

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

            var result = pipeline.Run(EventSink, null);

            Assert.AreEqual(StageState.WaitingForDependency, result);

            Expect(new StepExecutedEvent(firstStepId))
                .ThenExpect(new StepExecutedEvent(secondStepId))
                .AndNothingElse();
        }

       
    }
}
