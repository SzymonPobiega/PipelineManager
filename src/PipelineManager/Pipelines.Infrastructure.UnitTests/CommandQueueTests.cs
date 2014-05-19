using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Pipelines.Infrastructure.UnitTests
{
    [TestFixture]
    public class CommandQueueTests : DataAccessTest
    {
        [Test]
        public void When_no_commands_were_processed_it_returns_the_first_command()
        {
            var fixture = new Fixture();

            var unique = fixture.Create<int>();
            var sut = new CommandQueue(SessionFactory);

            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                CommandQueue.Enqueue(new TestCommand(unique), session);
                tx.Commit();
            }

            var lastProcessed = sut.GetLastProcessed();
            var command = (TestCommand)sut.Dequeue(lastProcessed, 1).First().Payload;

            Assert.AreEqual(unique, command.SomeUniqueProperty);
        }

        [Test]
        public void If_multiple_commands_were_enqueued_it_returns_them_in_FIFO_order()
        {
            var fixture = new Fixture();
            fixture.Customize(new NumericSequencePerTypeCustomization());

            var firstUnique = fixture.Create<int>();
            var secondUnique = fixture.Create<int>();

            var sut = new CommandQueue(SessionFactory);

            Enqueue(firstUnique);
            Enqueue(secondUnique);

            var lastProcessed = sut.GetLastProcessed();
            var uniques = sut.Dequeue(lastProcessed, 2).Select(x => x.Payload).Cast<TestCommand>().Select(x => x.SomeUniqueProperty).ToList();

            CollectionAssert.AreEqual(new[]{firstUnique, secondUnique}, uniques);
        }

        private void Enqueue(int unique)
        {
            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                CommandQueue.Enqueue(new TestCommand(unique), session);
                tx.Commit();
            }
        }

        [Test]
        public void Processing_state_can_be_stored_and_loadad()
        {
            var fixture = new Fixture();
            var sequence = fixture.Create<int>();
            var sut = new CommandQueue(SessionFactory);

            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                CommandQueue.MarkProcessed(sequence, session);
                tx.Commit();
            }

            var loadedSequence = sut.GetLastProcessed();

            Assert.AreEqual(sequence, loadedSequence);
        }

        [Test]
        public void If_there_are_no_unprocessed_commands_dequeue_returns_empty_collection()
        {
            const int lastProcessedCommand = 15;
            var sut = new CommandQueue(SessionFactory);

            var outstandingCommands = sut.Dequeue(lastProcessedCommand, 1);

            Assert.IsFalse(outstandingCommands.Any());
        }

        public class TestCommand : Command
        {
            private readonly int _someUniqueProperty;

            public TestCommand(int someUniqueProperty)
            {
                _someUniqueProperty = someUniqueProperty;
            }

            public int SomeUniqueProperty
            {
                get { return _someUniqueProperty; }
            }

            public override void Execute(IPipelineHost pipelineHost)
            {
            }
        }
    }
}
