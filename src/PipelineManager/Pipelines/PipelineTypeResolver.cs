using System.Collections.Generic;
using System.Linq;
using Pipelines.Schema;

namespace Pipelines
{
    public class PipelineTypeResolver : IPipelineTypeResolver
    {
        private readonly IPipelineSchemaSelector _schemaSelector;
        private readonly IEnumerable<IPipelineSchemaRepository> _schemaRepositories;

        public PipelineTypeResolver(IPipelineSchemaSelector schemaSelector, IEnumerable<IPipelineSchemaRepository> schemaRepositories)
        {
            _schemaSelector = schemaSelector;
            _schemaRepositories = schemaRepositories;
        }

        public PipelineSchema ResolveType(string pipelineId)
        {
            var schemaName = _schemaSelector.SelectSchema(pipelineId);
            return _schemaRepositories.SelectMany(x => x.EnumerableSchemas()).First(x => x.Name == schemaName);
        }
    }
}