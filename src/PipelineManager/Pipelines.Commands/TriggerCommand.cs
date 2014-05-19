namespace Pipelines.Commands
{
    public class TriggerCommand
    {
        private readonly string _pipelineId;

        public TriggerCommand(string pipelineId)
        {
            _pipelineId = pipelineId;
        }

        public string PipelineId
        {
            get { return _pipelineId; }
        }
    }
}
