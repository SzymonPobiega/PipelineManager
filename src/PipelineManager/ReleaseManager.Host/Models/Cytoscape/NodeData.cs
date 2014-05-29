namespace DeploymentPipelineVisualizer.Models.Cytoscape
{
    public class NodeData
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public string StageName { get; set; }
        public string Color { get; set; }
        public string Shape { get; set; }
        public int Weight { get; set; }
        public string DetailsUrl { get; set; }
    }
}