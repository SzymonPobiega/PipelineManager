using System.Collections.Generic;
using Pipelines.Schema;

namespace Pipelines
{
    public interface IPipelineSchemaRepository
    {
        IEnumerable<PipelineSchema> EnumerableSchemas();
    }
}