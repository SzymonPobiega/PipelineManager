using System;
using JetBrains.Annotations;

namespace ReleaseManager.Events
{
    public class ReleaseCandidateCreatedEvent
    {
        private readonly int _buildId;
        [NotNull]
        private readonly string _versionNumber;
        [NotNull]
        private readonly string _projectName;

        public ReleaseCandidateCreatedEvent(int buildId, [NotNull] string projectName, [NotNull] string versionNumber)
        {
            if (projectName == null) throw new ArgumentNullException("projectName");
            if (versionNumber == null) throw new ArgumentNullException("versionNumber");
            _versionNumber = versionNumber;
            _projectName = projectName;
            _buildId = buildId;
        }

        [NotNull]
        public string VersionNumber
        {
            get { return _versionNumber; }
        }

        [NotNull]
        public string ProjectName
        {
            get { return _projectName; }
        }

        [NotNull]
        public int BuildId
        {
            get { return _buildId; }
        }
    }
}