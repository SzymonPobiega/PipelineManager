using System;

namespace Pipelines
{
    public interface IStepFactory
    {
        BaseStep CreateInstance(Type stepType, UniqueStepId stepId);
    }
}