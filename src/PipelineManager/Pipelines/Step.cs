using System;

namespace Pipelines
{
    public abstract class Step<T> : BaseStep
        where T : class 
    {
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(1);

        protected Step(UniqueStepId stepId) : base(stepId)
        {
        }

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData, TimeSpan retryTime)
        {            
            if (!optionalData.HasData)
            {                
                return StepExecutionResult.WaitingForExternalDependency;
            }
            if (retryTime > _timeout)
            {
                return StepExecutionResult.Fail;
            }
            var typedData = (T) optionalData.Consume();
            var result = Resume(unitOfWork, typedData);
            return result
                ? StepExecutionResult.Finished
                : StepExecutionResult.Fail;
        }

        protected abstract bool Resume(IUnitOfWork unitOfWork, T data);
    }

    public abstract class Step : BaseStep
    {
        protected Step(UniqueStepId stepId) : base(stepId)
        {
        }

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData, TimeSpan retryTime)
        {
            var result = Resume(unitOfWork);
            return result
                ? StepExecutionResult.Finished
                : StepExecutionResult.Fail;
        }

        protected abstract bool Resume(IUnitOfWork unitOfWork);
    }
}