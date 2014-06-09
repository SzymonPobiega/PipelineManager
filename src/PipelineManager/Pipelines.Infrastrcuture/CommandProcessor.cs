using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using NHibernate;

namespace Pipelines.Infrastructure
{
    public class CommandProcessor
    {
        private readonly CommandQueue _commandQueue;
        private readonly ISessionFactory _sessionFactory;
        private readonly INHibernatePipelineHostFactory _hostFactory;

        public CommandProcessor(
            [NotNull] ISessionFactory sessionFactory, [NotNull] INHibernatePipelineHostFactory hostFactory)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");
            if (hostFactory == null) throw new ArgumentNullException("hostFactory");

            _commandQueue = new CommandQueue(sessionFactory);
            _sessionFactory = sessionFactory;
            _hostFactory = hostFactory;
        }

        public void BeginProcessing(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    var lastProcessedCommand = _commandQueue.GetLastProcessed();
                    var commandStream = DequeueCommands(lastProcessedCommand)
                        .TakeWhile(_ => !token.IsCancellationRequested);

                    foreach (var command in commandStream)
                    {
                        Process(command);
                    }
                }
                catch (Exception ex)
                {
                    Trace.Write(ex);
                }
            }
// ReSharper disable once FunctionNeverReturns
        }

        private void Process(CommandEnvelope commandEnvelope)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var host = _hostFactory.Create(session);
                commandEnvelope.Payload.Execute(host);
                _hostFactory.Release(host);
                CommandQueue.MarkProcessed(commandEnvelope.Sequence, session);
                tx.Commit();
            }
        }

        private IEnumerable<CommandEnvelope> DequeueCommands(int lastProcessedCommand)
        {
            while (true)
            {
                var unprocessedCommands = _commandQueue.Dequeue(lastProcessedCommand, 10);
                foreach (var command in unprocessedCommands)
                {
                    yield return command;
                    lastProcessedCommand = command.Sequence;
                }
            }
// ReSharper disable once FunctionNeverReturns
        }
    }
}