using System;
using Newtonsoft.Json;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Pipelines.Infrastructure.Records
{
    public class CommandRecord
    {
        public virtual int Sequence { get; protected set; }
        public virtual DateTime EnqueueDate { get; protected set; }

        public virtual string PayloadJson { get; set; }
        public virtual string PayloadTypeName { get; set; }

        public CommandRecord(object payload, DateTime enqueueDate)
        {
            EnqueueDate = enqueueDate;

            PayloadTypeName = payload.GetType().AssemblyQualifiedName;
            PayloadJson = JsonConvert.SerializeObject(payload);
        }

        public virtual object DeserializePayload()
        {
            var payloadType = Type.GetType(PayloadTypeName, true);
            return JsonConvert.DeserializeObject(PayloadJson, payloadType);
        }

        protected CommandRecord()
        {
        }

        public class Mapping : ClassMapping<CommandRecord>
        {
// ReSharper disable once InconsistentNaming
            private const int MAX = 10000;

            public Mapping()
            {
                Id(x => x.Sequence, m => m.Generator(Generators.Native));
                Property(x => x.EnqueueDate);
                Property(x => x.PayloadJson, m => m.Length(MAX));
                Property(x => x.PayloadTypeName, m => m.Length(200));
            }
        }
    }
}