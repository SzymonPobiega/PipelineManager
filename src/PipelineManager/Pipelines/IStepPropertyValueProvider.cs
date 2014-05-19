using System;

namespace Pipelines
{
    public interface IStepPropertyValueProvider
    {
        bool TryProvideValue(string propertyName, out object value);
    }
}