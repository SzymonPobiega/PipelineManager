using System;
using Pipelines;

namespace UnitTests
{
    public class StepWithoutDependencies : BaseStep
    {
        public StepWithoutDependencies(UniqueStepId stepId)
            : base(stepId)
        {
        }

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData, TimeSpan retryTime)
        {
            return StepExecutionResult.Finished;
        }
    }
}