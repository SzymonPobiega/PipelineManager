namespace Pipelines
{
    public abstract class BaseStep
    {
        private readonly UniqueStepId _stepId;

        protected BaseStep(UniqueStepId stepId)
        {
            _stepId = stepId;
        }

        public UniqueStepId StepId
        {
            get { return _stepId; }
        }

        public abstract StepExecutionResult Resume(IUnitOfWork unitOfWork, DataContainer optionalData);
    }
}
