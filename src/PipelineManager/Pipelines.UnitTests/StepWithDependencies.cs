using System;
using Pipelines;

namespace UnitTests
{
    public class StepWithDependencies : BaseStep
    {
        public StepWithDependencies(UniqueStepId stepId)
            : base(stepId)
        {
        }

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData, TimeSpan retryTime)
        {
            return StepExecutionResult.WaitingForExternalDependency;
        }
    }
}