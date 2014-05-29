using System;
using JetBrains.Annotations;

namespace ReleaseManager.Events
{
    public class ReleaseCandidateDeployedEvent
    {
        [NotNull]
        private readonly string _uniqueDeploymentId;
        private readonly int _buildId;
        [NotNull]
        private readonly string _projectName;
        [NotNull]
        private readonly string _versionNumber;
        [NotNull]
        private readonly string _environment;

        public ReleaseCandidateDeployedEvent([NotNull] string uniqueDeploymentId, int buildId, [NotNull] string projectName, [NotNull] string versionNumber, [NotNull] string environment)
        {
            if (environment == null) throw new ArgumentNullException("environment");
            if (projectName == null) throw new ArgumentNullException("projectName");
            if (versionNumber == null) throw new ArgumentNullException("versionNumber");
            _environment = environment;
            _uniqueDeploymentId = uniqueDeploymentId;
            _buildId = buildId;
            _projectName = projectName;
            _versionNumber = versionNumber;
        }

        public string Environment
        {
            get { return _environment; }
        }

        public int BuildId
        {
            get { return _buildId; }
        }

        public string ProjectName
        {
            get { return _projectName; }
        }

        public string VersionNumber
        {
            get { return _versionNumber; }
        }

        public string UniqueDeploymentId
        {
            get { return _uniqueDeploymentId; }
        }
    }
}