using System.Collections;
using System.Collections.Generic;

namespace Pipelines
{
    public interface IPipelineRepository
    {
        PipelineData TryGetById(string pipelineId);
        void Store(string pipelineId, IUnitOfWork unitOfWork);
    }
}