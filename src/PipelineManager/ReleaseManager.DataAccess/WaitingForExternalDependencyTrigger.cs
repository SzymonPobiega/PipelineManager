using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipelines.Events;
using Pipelines.Infrastructure;
using Pipelines.Infrastructure.Records;
using ReleaseManager.Extensibility;

namespace ReleaseManager.DataAccess
{
    public class WaitingForExternalDependencyTrigger : IEventHandler
    {
        public void On(EventEnvelope<StepWaitingForExternalDependencyEvent> evnt)
        {            
            evnt.EnqueueCommand(new ActivateCommand(evnt.PipelineId, evnt.Payload), DateTime.UtcNow + TimeSpan.FromSeconds(30));
        }
    }
}
