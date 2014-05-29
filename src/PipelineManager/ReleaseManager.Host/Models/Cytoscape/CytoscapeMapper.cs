using System.Collections.Generic;
using System.Linq;

namespace DeploymentPipelineVisualizer.Models.Cytoscape
{
    public class CytoscapeMapper
    {
        public Elements Map(Pipeline pipeline)
        {
            var nodes = pipeline.Stages.Select(MapStageToNode);
            var edges = pipeline.Stages.SelectMany(GenerateEdgesFromDependencies);

            return new Elements
                {
                    Nodes = nodes.ToArray(),
                    Edges = edges.ToArray()
                };
        }

        public IEnumerable<NodeState> MapState(Pipeline pipeline)
        {
            return pipeline.Stages.Select(x => new NodeState
                {
                    NodeId = x.Name.Replace(" ","_"),
                    CurrentVersion = x.State.CurrentVersion,
                    IsActive = x.State.Active,
                    IsFailing = !x.State.Success
                });
        }

        private static IEnumerable<Edge> GenerateEdgesFromDependencies(PipelineStage stage)
        {
            return stage.Dependencies.Select(x => new Edge
                {
                    Data = new EdgeData
                        {
                            Color = "black",
                            Strength = 100,
                            Source = x.Replace(" ", "_"),
                            Target = stage.Name.Replace(" ", "_")
                        }
                });
        }

        private static Node MapStageToNode(PipelineStage stage)
        {
            return new Node
                {
                    Classes = stage.State.Active ? "active" : "",
                    Data = new NodeData
                        {
                            Id = stage.Name.Replace(" ", "_"),
                            Caption = stage.Name,
                            StageName = stage.Name,
                            Weight = 100,
                            Color = stage.State.Success ? "green" : "red",
                            Shape = "rectangle",
                        }
                };
        }
    }
}