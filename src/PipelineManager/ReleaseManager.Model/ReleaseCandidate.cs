using System.Collections.Generic;
using System.Linq;
using Pipelines;
using ReleaseManager.Events;

namespace ReleaseManager.Model
{
    public class ReleaseCandidate : PipelineSubject
    {
        private int _buildId;
        private string _projectName;
        private string _versionNumber;

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

        public void Create(string buildNumber, string projectName, int buildId)
        {
            Apply(new ReleaseCandidateCreatedEvent(buildId, projectName, buildNumber));
        }

        public void ProcessTestSuiteResults(TestResult aggregateResult, string suiteType, List<TestOutput> outputs)
        {
            Apply(new TestSuiteFinishedEvent(_buildId, _projectName, _versionNumber, aggregateResult, suiteType, outputs));
        }

        public void Deployed(string environment, string uniqueDeploymentId)
        {
            Apply(new ReleaseCandidateDeployedEvent(uniqueDeploymentId, _buildId, _projectName, _versionNumber, environment));
        }

        public void DeploymentFailed(string environment, string uniqueDeploymentId)
        {
            Apply(new ReleaseCandidateDeploymentFailedEvent(uniqueDeploymentId, _buildId, _projectName, _versionNumber, environment));
        }

        public void On(ReleaseCandidateCreatedEvent evnt)
        {
            _projectName = evnt.ProjectName;
            _buildId = evnt.BuildId;
            _versionNumber = evnt.VersionNumber;
        }

    }
}
