using Autofac;
using NUnit.Framework;
using Pipelines;
using Pipelines.Autofac;

namespace UnitTests
{
    [TestFixture]
    public class AutofacStepFactoryTests
    {
        [Test]
        public void It_passes_step_id()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ParameterlessStep>();
            var factory = new AutofacStepFactory(containerBuilder);
            var stepId = new UniqueStepId("1", "1", "1", "1","1");
            
            var instance = factory.CreateInstance(typeof (ParameterlessStep), stepId);

            Assert.AreSame(instance.StepId, stepId);
        }

        [Test]
        public void It_passes_dependencies_via_constructor()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<DependencyImplementation>().AsImplementedInterfaces();
            containerBuilder.RegisterType<ParameterlessStepWithDependency>();
            var factory = new AutofacStepFactory(containerBuilder);
            var stepId = new UniqueStepId("1", "1", "1", "1","1");

            var instance = (ParameterlessStepWithDependency) factory.CreateInstance(typeof(ParameterlessStepWithDependency), stepId);

            Assert.IsNotNull(instance.Dependency);
        }

        public class ParametrizedStep : Step
        {
            public string StringValue { get; set; }

            public ParametrizedStep(UniqueStepId stepId) : base(stepId)
            {
            }

            protected override bool Resume(IUnitOfWork unitOfWork)
            {
                throw new System.NotImplementedException();
            }
        }

        public interface IDependency
        {
        }

        public class DependencyImplementation : IDependency
        {
        }

        public class ParameterlessStepWithDependency : Step
        {
            private readonly IDependency _dependency;

            public ParameterlessStepWithDependency(UniqueStepId stepId, IDependency dependency) : base(stepId)
            {
                _dependency = dependency;
            }

            public IDependency Dependency
            {
                get { return _dependency; }
            }

            protected override bool Resume(IUnitOfWork unitOfWork)
            {
                throw new System.NotImplementedException();
            }
        }

        public class ParameterlessStep : Step
        {
            public ParameterlessStep(UniqueStepId stepId) : base(stepId)
            {
            }

            protected override bool Resume(IUnitOfWork unitOfWork)
            {
                return true;
            }
        }
    }
}