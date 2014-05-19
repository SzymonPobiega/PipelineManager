namespace Pipelines.Commands
{
    public class CommandHandler
    {
        private readonly IPipelineHost _pipelineHost;

        public CommandHandler(IPipelineHost pipelineHost)
        {
            _pipelineHost = pipelineHost;
        }

        public void On()
    }

    public class ActivateCommand
    {
        private readonly string _pipelineId;
        private readonly byte[] _data;

        public ActivateCommand(string pipelineId, byte[] data)
        {
            _pipelineId = pipelineId;
            _data = data;
        }

        public string PipelineId
        {
            get { return _pipelineId; }
        }

        public byte[] Data
        {
            get { return _data; }
        }
    }
}