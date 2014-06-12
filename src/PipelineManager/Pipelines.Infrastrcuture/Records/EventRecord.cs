using System;
using Newtonsoft.Json;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Pipelines.Infrastructure.Records
{
    public class EventRecord
    {
        public virtual int Id { get; protected set; }
        public virtual string PipelineId { get; protected set; }
        public virtual int Sequence { get; protected set; }
        public virtual DateTime OccurenceDate { get; protected set; }
               
        public virtual string PayloadJson { get; set; }
        public virtual string PayloadTypeName { get; set; }

        public EventRecord(string pipelineId, int sequence, object payload, DateTime occurenceDate)
        {
            PipelineId = pipelineId;
            Sequence = sequence;
            OccurenceDate = occurenceDate;

            PayloadTypeName = payload.GetType().AssemblyQualifiedName;
            PayloadJson = JsonConvert.SerializeObject(payload);
        }

        public virtual object DeserializePayload()
        {
            var payloadType = Type.GetType(PayloadTypeName, true);
            object deserializePayload = JsonConvert.DeserializeObject(PayloadJson, payloadType);
            return deserializePayload;
        }

        protected EventRecord()
        {   
        }

        public class Mapping : ClassMapping<EventRecord>
        {
// ReSharper disable once InconsistentNaming
            private const int MAX = 10000;

            public Mapping()
            {
                Id(x => x.Id, m => m.Generator(Generators.Native));
                Property(x => x.PipelineId, m => m.Length(200));
                Property(x => x.OccurenceDate);
                Property(x => x.Sequence);
                Property(x => x.PayloadJson, m => m.Length(MAX));
                Property(x => x.PayloadTypeName, m => m.Length(200));
            }
        }
    }
}