namespace ReleaseManager.Host.Models
{
    public class ReleaseCandidateModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Version { get; set; }
        public string[] DeployedTo { get; set; }

        public Link TestResults { get; set; }
        public Link History { get; set; }
        public Link Deployments { get; set; }
    }
}