namespace Pipelines
{
    public abstract class Step<T> : BaseStep
        where T : class 
    {
        protected Step(UniqueStepId stepId) : base(stepId)
        {
        }

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData)
        {
            if (!optionalData.HasData)
            {
                return StepExecutionResult.WaitingForExternalDependency;
            }
            var typedData = (T) optionalData.Consume();
            Resume(unitOfWork, typedData);
            return StepExecutionResult.Finished;
        }

        protected abstract void Resume(IUnitOfWork unitOfWork, T data);
    }

    public abstract class Step : BaseStep
    {
        protected Step(UniqueStepId stepId) : base(stepId)
        {
        }

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData)
        {
            Resume(unitOfWork);
            return StepExecutionResult.Finished;
        }

        protected abstract void Resume(IUnitOfWork unitOfWork);
    }
}