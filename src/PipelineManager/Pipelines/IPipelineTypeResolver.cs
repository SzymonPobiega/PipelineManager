using Pipelines.Schema;

namespace Pipelines
{
    public interface IPipelineTypeResolver
    {
        PipelineSchema ResolveType(string pipelineId);
    }
}