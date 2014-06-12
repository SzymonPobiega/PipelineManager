using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Pipelines.Infrastructure.Records;

namespace Pipelines.Infrastructure
{
    public class CommandQueue
    {
        private const int NoCommandsProcessedYet = -1;

        private readonly ISessionFactory _sessionFactory;

        public CommandQueue(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public static void Enqueue(object command, ISession session, DateTime executeAfter)
        {
            var commandRecord = new CommandRecord(command, DateTime.UtcNow, executeAfter);
            session.Save(commandRecord);
        }
        
        public static void MarkProcessed(int sequenceNumber, ISession session)
        {
            var record = new CommandProcessingResultRecord(sequenceNumber, DateTime.UtcNow);
            session.Save(record);
        }

        public int GetLastProcessed()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var record = session.QueryOver<CommandProcessingResultRecord>()
                    .OrderBy(x => x.Sequence).Desc
                    .Take(1)
                    .SingleOrDefault<CommandProcessingResultRecord>();
                return record != null
                    ? record.Sequence
                    : NoCommandsProcessedYet;
            }
        }

        public IEnumerable<CommandEnvelope> Dequeue(int maxCount)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                CommandRecord aliasCommandRecord = null;
                CommandProcessingResultRecord aliasCommandProcessing = null;
                var processedCommands =
                    QueryOver.Of(() => aliasCommandProcessing)
                        .Where(() => aliasCommandProcessing.Sequence == aliasCommandRecord.Sequence)
                        .Select(x => x.Sequence)
                        .DetachedCriteria;

                return session.QueryOver(() => aliasCommandRecord)
                    .Where(Subqueries.NotExists(processedCommands))
                    .Take(maxCount)
                    .List<CommandRecord>()
                    .Select(x => new CommandEnvelope(x.Sequence, (Command) x.DeserializePayload()))
                    .ToList();
            }
        }
    }
}