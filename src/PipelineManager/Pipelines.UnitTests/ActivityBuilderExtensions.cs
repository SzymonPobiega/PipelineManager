using System;
using Pipelines;
using Pipelines.Schema.Builders;
using IStepBuilder = Pipelines.Schema.Builders.IStepBuilder;

namespace UnitTests
{
    public static class ActivityBuilderExtensions
    {
        public static IStepBuilder AddStepWithoutDependencies(this IActivityBuilder builder, Action<UniqueStepId> callback)
        {
            return builder.AddStep<StepWithoutDependencies>(callback);
        }
        
        public static IStepBuilder AddStepWithoutDependencies(this IActivityBuilder builder)
        {
            return builder.AddStep<StepWithoutDependencies>();
        }

        public static IStepBuilder AddStepWithDependencies(this IActivityBuilder builder, Action<UniqueStepId> callback)
        {
            return builder.AddStep<StepWithDependencies>(callback);
        }
        
        public static IStepBuilder AddStepWithDependencies(this IActivityBuilder builder)
        {
            return builder.AddStep<StepWithDependencies>();
        }
    }
}