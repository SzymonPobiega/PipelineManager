using System;
using JetBrains.Annotations;

namespace ReleaseManager.Process.Octopus
{
    public class DeploymentRequestedEvent
    {
        private readonly string _environment;
        private readonly string _deploymentId;
        private readonly string _correlationId;
        private readonly string _taskId;
        private readonly string _detailsLink;

        public DeploymentRequestedEvent([NotNull] string deploymentId, [NotNull] string environment,
            [NotNull] string correlationId, [NotNull] string taskId, [NotNull] string detailsLink)
        {
            if (deploymentId == null) throw new ArgumentNullException("deploymentId");
            if (environment == null) throw new ArgumentNullException("environment");
            if (correlationId == null) throw new ArgumentNullException("correlationId");
            if (taskId == null) throw new ArgumentNullException("taskId");
            _environment = environment;
            _deploymentId = deploymentId;
            _correlationId = correlationId;
            _taskId = taskId;
            _detailsLink = detailsLink;
        }

        public string Environment
        {
            get { return _environment; }
        }

        public string DeploymentId
        {
            get { return _deploymentId; }
        }

        public string CorrelationId
        {
            get { return _correlationId; }
        }

        public string TaskId
        {
            get { return _taskId; }
        }

        public string DetailsLink
        {
            get { return _detailsLink; }
        }
    }
}