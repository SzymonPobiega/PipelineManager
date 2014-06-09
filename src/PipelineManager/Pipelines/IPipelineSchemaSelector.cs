namespace Pipelines
{
    public interface IPipelineSchemaSelector
    {
        string SelectSchema(string pipelineId);
    }
}