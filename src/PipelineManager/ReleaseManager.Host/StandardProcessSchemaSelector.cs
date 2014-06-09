using Pipelines;

namespace ReleaseManager.Host
{
    public class StandardProcessSchemaSelector : IPipelineSchemaSelector
    {
        public string SelectSchema(string pipelineId)
        {
            return "TestProject";
        }
    }
}