using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Pipelines.Infrastructure.UnitTests
{
    [TestFixture]
    public class NHibernatePipelineRepositoryTests : DataAccessTest
    {
        [Test]
        public void It_returns_null_if_pipeline_does_not_exist()
        {
            var fixture = new Fixture();
            var nonExistingPipelineId = fixture.Create<string>();

            using (var storeSession = SessionFactory.OpenSession())
            using (var tx = storeSession.BeginTransaction())
            {
                var sut = new NHibernatePipelineRepository(storeSession, new EventDispatcher());
                var pipelineData = sut.TryGetById(nonExistingPipelineId);

                Assert.IsNull(pipelineData);
            }
        }

        [Test]
        public void It_can_store_and_load_events()
        {
            var fixture = new Fixture();
            var pipelineId = fixture.Create<string>();

            var payLoad1 = fixture.Create<int>();
            var payload2 = fixture.Create<int>();
            var payload3 = fixture.Create<int>();
            var payload4 = fixture.Create<int>();
            var payload5 = fixture.Create<int>();

            using (var storeSession = SessionFactory.OpenSession())
            using (var tx = storeSession.BeginTransaction())
            {
                var sut = new NHibernatePipelineRepository(storeSession, new EventDispatcher());
                var unitOfWork = UnitOfWork.CreateForNonExistingPipeline();
                unitOfWork.On(new TestEvent(payLoad1));
                unitOfWork.On(new TestEvent(payload2));
                unitOfWork.On(new TestEvent(payload3));
                sut.Store(pipelineId, unitOfWork);
                tx.Commit();
            }

            PipelineData pipelineData;
            using (var loadSession = SessionFactory.OpenSession())
            {
                var sut = new NHibernatePipelineRepository(loadSession, new EventDispatcher());
                pipelineData = sut.TryGetById(pipelineId);

                Assert.AreEqual(3, pipelineData.Version);
                CollectionAssert.AreEqual(new[]
                {
                    new TestEvent(payLoad1), 
                    new TestEvent(payload2), 
                    new TestEvent(payload3),
                }, pipelineData.Events);
            }

            using (var storeSession = SessionFactory.OpenSession())
            using (var tx = storeSession.BeginTransaction())
            {
                var sut = new NHibernatePipelineRepository(storeSession, new EventDispatcher());
                var unitOfWork = UnitOfWork.CreateForExistingPipeline(pipelineData.Events, pipelineData.Version);
                unitOfWork.On(new TestEvent(payload4));
                unitOfWork.On(new TestEvent(payload5));
                sut.Store(pipelineId, unitOfWork);
                tx.Commit();
            }

            using (var loadSession = SessionFactory.OpenSession())
            {
                var sut = new NHibernatePipelineRepository(loadSession, new EventDispatcher());
                pipelineData = sut.TryGetById(pipelineId);

                Assert.AreEqual(5, pipelineData.Version);
                CollectionAssert.AreEqual(new[]
                {
                    new TestEvent(payLoad1),
                    new TestEvent(payload2),
                    new TestEvent(payload3),
                    new TestEvent(payload4),
                    new TestEvent(payload5),
                }, pipelineData.Events);
            }
        }

        public class TestEvent
        {
            private readonly int _data;

            public TestEvent(int data)
            {
                _data = data;
            }

            public int Data
            {
                get { return _data; }
            }

            protected bool Equals(TestEvent other)
            {
                return _data == other._data;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((TestEvent)obj);
            }

            public override int GetHashCode()
            {
                return _data;
            }
        }
    }
}