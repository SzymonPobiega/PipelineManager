using System;
using Pipelines;

namespace UnitTests
{
    public class SuccessAfterRetryStep : BaseStep
    {
        public bool Retried { get; private set; }

        public SuccessAfterRetryStep(UniqueStepId stepId)
            : base(stepId)
        {
            Retried = false;
        }

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData, TimeSpan retryTime)
        {
            if (!Retried)
            {
                Retried = !Retried;
                return StepExecutionResult.Fail;             
            }
            return StepExecutionResult.Finished;            
        }
    }
}
