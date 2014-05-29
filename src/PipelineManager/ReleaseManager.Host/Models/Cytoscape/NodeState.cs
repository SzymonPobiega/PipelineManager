namespace DeploymentPipelineVisualizer.Models.Cytoscape
{
    public class NodeState
    {
        public string NodeId { get; set; }
        public string CurrentVersion { get; set; }
        public bool IsActive { get; set; }
        public bool IsFailing { get; set; }
    }
}