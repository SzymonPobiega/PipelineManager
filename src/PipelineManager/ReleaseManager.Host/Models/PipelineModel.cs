using System.Collections.Generic;
using Pipelines.Web;

namespace ReleaseManager.Host.Models
{
    public class PipelineModel : Representation<PipelineController>
    {
        public string Name { get; set; }
        public List<StageModel> Stages { get; set; }
    }
}