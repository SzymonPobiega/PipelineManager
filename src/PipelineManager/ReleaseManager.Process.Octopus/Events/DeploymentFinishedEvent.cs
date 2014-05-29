using ReleaseManager.Process.Octopus.Model;

namespace ReleaseManager.Process.Octopus
{
    public class DeploymentFinishedEvent
    {
        private readonly string _deploymentId;
        private readonly DeploymentResult _result;

        public DeploymentFinishedEvent(string deploymentId, DeploymentResult result)
        {
            _deploymentId = deploymentId;
            _result = result;
        }

        public string DeploymentId
        {
            get { return _deploymentId; }
        }

        public DeploymentResult Result
        {
            get { return _result; }
        }
    }
}