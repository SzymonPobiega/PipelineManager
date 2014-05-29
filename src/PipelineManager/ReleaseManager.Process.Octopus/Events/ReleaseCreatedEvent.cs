namespace ReleaseManager.Process.Octopus
{
    public class ReleaseCreatedEvent
    {
        private readonly string _releaseId;

        public ReleaseCreatedEvent(string releaseId)
        {
            _releaseId = releaseId;
        }

        public string ReleaseId
        {
            get { return _releaseId; }
        }
    }
}