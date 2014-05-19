using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pipelines;

namespace UnitTests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [Test]
        public void Unit_of_work_implements_identity_map()
        {
            var uow = UnitOfWork.CreateForNonExistingPipeline();

            var subject = uow.LoadSubject<Subject1>();
            var subjectRetrievedAgain = uow.LoadSubject<Subject1>();

            Assert.AreSame(subject, subjectRetrievedAgain);
        }

        [Test]
        public void Subject_is_reconstructed_using_both_comitted_and_uncommitted_events()
        {
            var uow = UnitOfWork.CreateForExistingPipeline(new Object[] {new CommittedEvent()},1);
            uow.On(new UncommittedEvent());

            var subject = uow.LoadSubject<Subject1>();

            Assert.IsTrue(subject.CommittedEventProcessed);
            Assert.IsTrue(subject.UncommittedEventProcessed);
        }

        [Test]
        public void Loaded_subjects_are_notified_when_new_events_are_published()
        {
            var uow = UnitOfWork.CreateForExistingPipeline(new Object[] { new CommittedEvent() },1);
            var subject = uow.LoadSubject<Subject1>();

            uow.On(new UncommittedEvent());

            Assert.IsTrue(subject.UncommittedEventProcessed);
        }

        public class Subject1 : PipelineSubject
        {
            public bool CommittedEventProcessed;
            public bool UncommittedEventProcessed;

            public void On(CommittedEvent evnt)
            {
                CommittedEventProcessed = true;
            }

            public void On(UncommittedEvent evnt)
            {
                UncommittedEventProcessed = true;
            }
        }

        public class CommittedEvent
        {
        }

        public class UncommittedEvent
        {
        }
    }
}