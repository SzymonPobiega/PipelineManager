namespace ReleaseManager.Host.Models
{
    public class DeploymentModel
    {
        public string UniqueId { get; set; }
        public string Environment { get; set; }
        public string DeploymentDate { get; set; }
        public string DetailsUrl { get; set; }
        public bool Success { get; set; }
    }
}