using System;
using JetBrains.Annotations;

namespace Pipelines.Infrastructure
{
    public class CommandEnvelope
    {
        private readonly int _sequence;
        [NotNull]
        private readonly Command _payload;

        public CommandEnvelope(int sequence, [NotNull] Command payload)
        {
            if (payload == null) throw new ArgumentNullException("payload");
            _sequence = sequence;
            _payload = payload;
        }

        public Command Payload
        {
            get { return _payload; }
        }

        public int Sequence
        {
            get { return _sequence; }
        }
    }
}