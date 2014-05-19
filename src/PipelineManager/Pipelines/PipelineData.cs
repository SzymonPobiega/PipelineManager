using System.Collections.Generic;

namespace Pipelines
{
    public class PipelineData
    {
        private readonly int _version;
        private readonly List<object> _events;

        public PipelineData(List<object> events, int version)
        {
            _events = events;
            _version = version;
        }

        public IEnumerable<object> Events
        {
            get { return _events; }
        }

        public int Version
        {
            get { return _version; }
        }
    }
}