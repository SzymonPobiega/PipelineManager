using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Pipelines.Infrastructure
{
    public class ActivateCommand : Command
    {
        private readonly string _pipelineId;
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        private readonly object _data;

        public ActivateCommand([NotNull] string pipelineId, [NotNull] object data)
        {
            if (pipelineId == null) throw new ArgumentNullException("pipelineId");
            if (data == null) throw new ArgumentNullException("data");
            _pipelineId = pipelineId;
            _data = data;
        }

        public string PipelineId
        {
            get { return _pipelineId; }
        }

        public object Data
        {
            get { return _data; }
        }

        public override void Execute(IPipelineHost pipelineHost)
        {
            pipelineHost.Activate(PipelineId, Data);
        }
    }
}