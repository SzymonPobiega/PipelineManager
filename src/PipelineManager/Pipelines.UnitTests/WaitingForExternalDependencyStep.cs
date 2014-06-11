using System;
using Pipelines;

namespace UnitTests
{
    public class WaitingForExternalDependencyStep : BaseStep
    {
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(1);

        public WaitingForExternalDependencyStep(UniqueStepId stepId)
            : base(stepId)
        {
        }

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData, TimeSpan retryTime)
        {
            return retryTime < _timeout ? StepExecutionResult.WaitingForExternalDependency : StepExecutionResult.Fail;
        }
    }
}