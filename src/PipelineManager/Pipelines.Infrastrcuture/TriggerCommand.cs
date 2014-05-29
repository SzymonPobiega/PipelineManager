using System;
using JetBrains.Annotations;

namespace Pipelines.Infrastructure
{
    public class TriggerCommand : Command
    {
        private readonly string _pipelineId;

        public TriggerCommand([NotNull] string pipelineId)
        {
            if (pipelineId == null) throw new ArgumentNullException("pipelineId");
            _pipelineId = pipelineId;
        }

        public string PipelineId
        {
            get { return _pipelineId; }
        }

        public override void Execute(IPipelineHost pipelineHost)
        {
            pipelineHost.Trigger(PipelineId);
        }
    }
}