using System.Linq;
using Pipelines.Schema;

namespace Pipelines
{
    public class StepSchemaValueProvider : IStepPropertyValueProvider
    {
        private readonly StepSchema _schema;

        public StepSchemaValueProvider(StepSchema schema)
        {
            _schema = schema;
        }

        public bool TryProvideValue(string propertyName, out object value)
        {
            var result = _schema.Parameters.FirstOrDefault(x => x.Name == propertyName);
            if (result != null)
            {
                value = result.Value;
                return true;
            }
            value = null;
            return false;
        }
    }
}