using System.Linq;

namespace Pipelines
{
    public class PipelineHost : IPipelineHost
    {
        private readonly IPipelineTypeResolver _typeResolver;
        private readonly PipelineFactory _factory;
        private readonly IPipelineRepository _pipelineRepository;

        public PipelineHost(IPipelineTypeResolver typeResolver, IPipelineRepository pipelineRepository, PipelineFactory factory)
        {
            _typeResolver = typeResolver;
            _factory = factory;
            _pipelineRepository = pipelineRepository;
        }

        public void Activate(string pipelineId, object data)
        {
            IUnitOfWork unitOfWork;
            var pipeline = Load(pipelineId, out unitOfWork) ?? CreateNew(pipelineId, unitOfWork);
            pipeline.Run(unitOfWork, data);
            _pipelineRepository.Store(pipelineId, unitOfWork);
        }
        
        public void Trigger(string pipelineId)
        {
            IUnitOfWork unitOfWork;
            var pipeline = Load(pipelineId, out unitOfWork) ?? CreateNew(pipelineId, unitOfWork);
            pipeline.Trigger();
            pipeline.Run(unitOfWork, null);
            _pipelineRepository.Store(pipelineId, unitOfWork);
        }

        private Pipeline Load(string pipelineId, out IUnitOfWork unitOfWork)
        {
            var data = _pipelineRepository.TryGetById(pipelineId);
            if (data == null)
            {
                unitOfWork = UnitOfWork.CreateForNonExistingPipeline();
                return null;
            }
            var createdEvent = (PipelineCreatedEvent) data.Events.First();
            var instance = _factory.Create(pipelineId, createdEvent.Schema);

            var eventApplicator = new DynamicEventSink(instance);
            foreach (var evnt in data.Events.Skip(1))
            {
                eventApplicator.On(evnt);               
            }
            unitOfWork = UnitOfWork.CreateForExistingPipeline(data.Events, data.Version);
            return instance;
        }

        private Pipeline CreateNew(string pipelineId, IUnitOfWork unitOfWork)
        {
            var schema = _typeResolver.ResolveType(pipelineId);
            var pipeline = _factory.Create(pipelineId, schema);

            unitOfWork.On(new PipelineCreatedEvent(new UniquePipelineId(pipelineId, schema.Name), schema));

            return pipeline;
        }
    }
}