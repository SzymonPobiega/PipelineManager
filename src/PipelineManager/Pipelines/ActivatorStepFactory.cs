using System;

namespace Pipelines
{
    public class ActivatorStepFactory : IStepFactory
    {
        public BaseStep CreateInstance(Type stepType, UniqueStepId stepId)
        {
            return (BaseStep)Activator.CreateInstance(stepType, new object[] { stepId });
        }
    }
}