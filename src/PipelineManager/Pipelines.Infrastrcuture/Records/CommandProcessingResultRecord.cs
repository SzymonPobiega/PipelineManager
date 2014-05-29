using System;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Pipelines.Infrastructure.Records
{
    public class CommandProcessingResultRecord
    {
        public virtual int Sequence { get; protected set; }
        public virtual DateTime ProcessingDate { get; protected set; }

        public CommandProcessingResultRecord(int sequence, DateTime processingDate)
        {
            Sequence = sequence;
            ProcessingDate = processingDate;
        }

        protected CommandProcessingResultRecord()
        {
        }

        public class Mapping : ClassMapping<CommandProcessingResultRecord>
        {
            public Mapping()
            {
                Id(x => x.Sequence, m => m.Generator(Generators.Assigned));
                Property(x => x.ProcessingDate);
            }
        }
    }
}