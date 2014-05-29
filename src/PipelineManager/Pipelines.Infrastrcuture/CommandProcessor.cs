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
        private readonly IPipelineTypeResolver _typeResolver;
        private readonly PipelineFactory _pipelineFactory;
        private readonly EventDispatcher _eventDispatcher;

        public CommandProcessor(
            [NotNull] ISessionFactory sessionFactory, 
            [NotNull] IPipelineTypeResolver typeResolver, 
            [NotNull] PipelineFactory pipelineFactory, 
            [NotNull] EventDispatcher eventDispatcher)
        {
            if (sessionFactory == null) throw new ArgumentNullException("sessionFactory");
            if (typeResolver == null) throw new ArgumentNullException("typeResolver");
            if (pipelineFactory == null) throw new ArgumentNullException("pipelineFactory");
            if (eventDispatcher == null) throw new ArgumentNullException("eventDispatcher");

            _commandQueue = new CommandQueue(sessionFactory);
            _sessionFactory = sessionFactory;
            _typeResolver = typeResolver;
            _pipelineFactory = pipelineFactory;
            _eventDispatcher = eventDispatcher;
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
        }

        private void Process(CommandEnvelope commandEnvelope)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var host = new PipelineHost(_typeResolver, new NHibernatePipelineRepository(session,_eventDispatcher), _pipelineFactory);

                commandEnvelope.Payload.Execute(host);                    
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
        }
    }
}