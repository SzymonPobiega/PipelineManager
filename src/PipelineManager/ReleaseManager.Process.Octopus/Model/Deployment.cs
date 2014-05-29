namespace ReleaseManager.Process.Octopus.Model
{
    public class Deployment
    {
        private readonly string _id;
        private readonly string _environment;
        private readonly string _correlationId;
        private readonly string _taskId;
        private DeploymentResult _result;

        public Deployment(string id, string environment, string correlationId, string taskId)
        {
            _id = id;
            _correlationId = correlationId;
            _taskId = taskId;
            _environment = environment;
        }

        public string Id
        {
            get { return _id; }
        }

        public string CorrelationId
        {
            get { return _correlationId; }
        }

        public string TaskId
        {
            get { return _taskId; }
        }

        public DeploymentResult Result
        {
            get { return _result; }
        }

        public string Environment
        {
            get { return _environment; }
        }

        public void Finished(DeploymentResult result)
        {
            _result = result;
        }
    }
}