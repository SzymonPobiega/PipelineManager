using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Pipelines;
using Pipelines.Schema;
using Pipelines.Schema.Builders;
using Ploeh.AutoFixture;

namespace UnitTests
{
    [TestFixture]
    public class PipelineHostTests : EventDriventTest
    {
        private const PipelineData Nothing = null;

        [Test]
        public void If_pipeline_does_not_exist_it_is_created_and_then_activated()
        {
            var fixture = new Fixture();
            var pipelineId = fixture.Create<string>();

            var typeResolverMock = new Mock<IPipelineTypeResolver>();
            typeResolverMock
                .Setup(x => x.ResolveType(It.IsAny<string>()))
                .Returns((string id) => CreatePipelineSchema(id));

            var repositoryMock = new Mock<IPipelineRepository>();
            repositoryMock.Setup(x => x.TryGetById(It.IsAny<string>())).Returns(Nothing);
            ForwardEvents(repositoryMock);
            var host = new PipelineHost(typeResolverMock.Object, repositoryMock.Object, CreatePipelineFactory());

            host.Activate(pipelineId, null);

            Expect((PipelineCreatedEvent e) => e.PipelineId.PipelineId == pipelineId)
                .ThenExpectAny<StepExecutedEvent>()
                .ThenExpectAny<StageFinishedEvent>();
        }

        [Test]
        public void If_pipeline_already_exists_it_is_activated()
        {
            var fixture = new Fixture();
            var pipelineId = fixture.Create<string>();
            var schemaName = fixture.Create<string>();

            var typeResolverMock = new Mock<IPipelineTypeResolver>();

            var repositoryMock = new Mock<IPipelineRepository>();
            repositoryMock.Setup(x => x.TryGetById(It.IsAny<string>()))
                .Returns((string id) => LoadPipelineDataForJustCreatedPipeline(id, schemaName));
            ForwardEvents(repositoryMock);

            var host = new PipelineHost(typeResolverMock.Object, repositoryMock.Object, CreatePipelineFactory());

            host.Activate(pipelineId, null);

            ExpectAny<StepExecutedEvent>()
                .ThenExpectAny<StageFinishedEvent>();
        }

        [Test]
        public void If_pipeline_already_exists_its_state_is_reconstructed_from_events()
        {
            var fixture = new Fixture();
            var pipelineId = fixture.Create<string>();
            var schemaName = fixture.Create<string>();

            var typeResolverMock = new Mock<IPipelineTypeResolver>();

            var repositoryMock = new Mock<IPipelineRepository>();
            repositoryMock.Setup(x => x.TryGetById(It.IsAny<string>()))
                .Returns((string id) => LoadPipelineDataForPipelineWaitingForDependency(id, schemaName));
            ForwardEvents(repositoryMock);

            var host = new PipelineHost(typeResolverMock.Object, repositoryMock.Object, CreatePipelineFactory());

            host.Activate(pipelineId, null);

            Expect(new StepExecutedEvent(new UniqueStepId("1", "1", "2", pipelineId, schemaName)))
                .ThenExpect(new StageFinishedEvent(new UniqueStageId("2", pipelineId, schemaName)));
        }

        private void ForwardEvents(Mock<IPipelineRepository> repositoryMock)
        {
            repositoryMock.Setup(x => x.Store(It.IsAny<string>(), It.IsAny<IUnitOfWork>()))
                .Callback((string p, IUnitOfWork uow) =>
                {
                    foreach (var evnt in uow.UncommittedEvents)
                    {
                        EventSink.On(evnt);
                    }
                });
        }

        private static PipelineFactory CreatePipelineFactory()
        {
            return new PipelineFactory(new ActivatorStepFactory());
        }
        
        private PipelineData LoadPipelineDataForJustCreatedPipeline(string pipelineId, string schemaName)
        {
            return new PipelineData(new List<object>
            {
                new PipelineCreatedEvent(new UniquePipelineId(pipelineId, schemaName), CreatePipelineSchema(schemaName))
            },1);
        }
        
        private PipelineData LoadPipelineDataForPipelineWaitingForDependency(string pipelineId, string schemaName)
        {
            return new PipelineData(new List<object>
            {
                new PipelineCreatedEvent(new UniquePipelineId(pipelineId, schemaName), CreatePipelineSchema(schemaName)),
                new StepExecutedEvent(new UniqueStepId("1","1","1",pipelineId, schemaName)),
                new StageFinishedEvent(new UniqueStageId("1",pipelineId, schemaName))
            },3);
        }

        private PipelineSchema CreatePipelineSchema(string schemaName)
        {
            return new PipelineBuilder(schemaName, 0)
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStepWithoutDependencies()
                .BuildSchema();
        }
    }
}