using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pipelines.Schema
{
    [DataContract]
    public class PipelineSchema
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Revision { get; set; }

        [DataMember]
        public List<StageSchema> Stages { get; set; }

        public PipelineSchema()
        {
            Stages = new List<StageSchema>();
        }
    }
}