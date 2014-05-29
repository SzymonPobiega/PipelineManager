namespace DeploymentPipelineVisualizer.Models.Cytoscape
{
    public class Node
    {
        public string Classes { get; set; }
        public bool Selectable { get; set; }
        public bool Grabbable { get; set; }
        public NodeData Data { get; set; }
    }
}