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

        public override StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData)
        {
            var result = Resume(unitOfWork);
            return result
                ? StepExecutionResult.Finished
                : StepExecutionResult.Fail;
        }

        protected abstract bool Resume(IUnitOfWork unitOfWork);
    }
}