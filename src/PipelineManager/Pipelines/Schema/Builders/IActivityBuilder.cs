using System;

namespace Pipelines.Schema.Builders
{
    public interface IActivityBuilder
    {
        IStepBuilder AddStep<T>(Action<UniqueStepId> callback);
        IStepBuilder AddStep<T>();
    }
}