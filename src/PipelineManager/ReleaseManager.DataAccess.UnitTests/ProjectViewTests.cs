using System;
using System.Linq;
using NUnit.Framework;
using Pipelines;
using Pipelines.Infrastructure;
using Pipelines.Schema;
using Pipelines.Schema.Builders;
using Ploeh.AutoFixture;
using ReleaseManager.DataAccess.ReadModels;

namespace ReleaseManager.DataAccess.UnitTests
{
    [TestFixture]
    public class ProjectViewTests : DataAccessTest
    {
        [Test]
        public void Whren_a_pipeline_is_created_for_the_first_time_it_creates_a_new_project()
        {
            var fixture = new Fixture();
            var pipelineSchemaName = fixture.Create<string>();
            var pipelineId = fixture.Create<string>();
            var occurenceDate = fixture.Create<DateTime>();
            var version = fixture.Create<int>();

            var sut = new ProjectReadModel();

            ProcessCreatedEvent(pipelineId, pipelineSchemaName, version, sut, occurenceDate);

            var assertSession = SessionFactory.OpenSession();
            var projects = assertSession.QueryOver<Project>()
                .Where(x => x.Name == pipelineSchemaName)
                .List();

            Assert.AreEqual(1, projects.Count);
            var project = projects[0];
            Assert.AreEqual(version, project.SchemaVersion);
            Assert.IsNull(project.PreviousSchemaView);
        } 
        
        [Test]
        public void When_a_second_pipeline_using_same_schema_revision_is_started_nothing_happens()
        {
            var fixture = new Fixture();
            var pipelineSchemaName = fixture.Create<string>();
            var firstPipelineId = fixture.Create<string>();
            var secondPipelineId = fixture.Create<string>();
            var firstOccurenceDate = fixture.Create<DateTime>();
            var secondOccurenceDate = fixture.Create<DateTime>();
            var version = fixture.Create<int>();

            var sut = new ProjectReadModel();

            //First pipeline
            ProcessCreatedEvent(firstPipelineId, pipelineSchemaName, version, sut, firstOccurenceDate);

            //Act: Second pipeline
            ProcessCreatedEvent(secondPipelineId, pipelineSchemaName, version, sut, secondOccurenceDate);
            
            var assertSession = SessionFactory.OpenSession();
            var projects = assertSession.QueryOver<Project>()
                .Where(x => x.Name == pipelineSchemaName)
                .List();

            Assert.AreEqual(1, projects.Count);
            var project = projects[0];
            Assert.AreEqual(version, project.SchemaVersion);
            Assert.IsNull(project.PreviousSchemaView);
        } 
        
        [Test]
        public void When_a_second_pipeline_using_newer_schema_revision_is_started_a_new_project_is_created_and_linked_with_the_old_one()
        {
            var fixture = new Fixture();
            fixture.Customize(new NumericSequencePerTypeCustomization());
            var pipelineSchemaName = fixture.Create<string>();
            var firstPipelineId = fixture.Create<string>();
            var secondPipelineId = fixture.Create<string>();
            var firstOccurenceDate = fixture.Create<DateTime>();
            var secondOccurenceDate = fixture.Create<DateTime>();

            var firstVersion = fixture.Create<int>();
            var secondVersion = fixture.Create<int>();

            var sut = new ProjectReadModel();

            //First pipeline
            ProcessCreatedEvent(firstPipelineId, pipelineSchemaName, firstVersion, sut, firstOccurenceDate);
            
            //Ack: Second pipeline
            ProcessCreatedEvent(secondPipelineId, pipelineSchemaName, secondVersion, sut, secondOccurenceDate);

            var assertSession = SessionFactory.OpenSession();
            var projects = assertSession.QueryOver<Project>()
                .Where(x => x.Name == pipelineSchemaName)
                .List();

            Assert.AreEqual(2, projects.Count);
            var newProject = projects.First(x => x.IsLatestVersion);
            var oldProject = projects.First(x => !x.IsLatestVersion);

            Assert.AreSame(oldProject, newProject.PreviousSchemaView);
            Assert.AreEqual(secondVersion, newProject.SchemaVersion);
            Assert.AreEqual(firstVersion, oldProject.SchemaVersion);
        }

        private void ProcessCreatedEvent(string pipelineId, string schemaName, int schemaVersion, ProjectReadModel sut, DateTime occurenceDate)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var evnt = new PipelineCreatedEvent(new UniquePipelineId(pipelineId, schemaName),
                    CreatePipelineSchema(schemaName, schemaVersion));

                sut.On(new EventEnvelope<PipelineCreatedEvent>(pipelineId, evnt, occurenceDate, session));
                transaction.Commit();
            }
        }

        private PipelineSchema CreatePipelineSchema(string schemaName, int revision)
        {
            return new PipelineBuilder(schemaName, revision)
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStep<TestStep>()
                .AddStage(StageTriggerMode.Automatic)
                .AddActivity()
                .AddStep<TestStep>()
                .BuildSchema();
        }

        public class TestStep : Step
        {
            public TestStep(UniqueStepId stepId) : base(stepId)
            {
            }

            protected override bool Resume(IUnitOfWork unitOfWork)
            {
                return true;
            }
        }
    }
}