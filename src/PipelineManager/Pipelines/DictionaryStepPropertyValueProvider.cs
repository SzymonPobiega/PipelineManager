using System.Collections.Generic;

namespace Pipelines
{
    public class DictionaryStepPropertyValueProvider : IStepPropertyValueProvider
    {
        private readonly Dictionary<string, object> _values;

        public DictionaryStepPropertyValueProvider(Dictionary<string, object> values)
        {
            _values = values;
        }

        public bool TryProvideValue(string propertyName, out object value)
        {
            return _values.TryGetValue(propertyName, out value);
        }
    }
}