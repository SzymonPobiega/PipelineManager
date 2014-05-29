using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
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

        public static void Enqueue(object command, ISession session)
        {
            var commandRecord = new CommandRecord(command, DateTime.UtcNow);
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

        public IEnumerable<CommandEnvelope> Dequeue(int lastProcessedCommand, int maxCount)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<CommandRecord>()
                    .Where(x => x.Sequence > lastProcessedCommand)
                    .Take(maxCount)
                    .List<CommandRecord>()
                    .Select(x => new CommandEnvelope(x.Sequence, (Command) x.DeserializePayload()))
                    .ToList();
            }
        }
    }
}