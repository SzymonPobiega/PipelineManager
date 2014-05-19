using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pipelines.Schema
{
    [DataContract]
    public class StepSchema
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Implementation { get; set; }

        [DataMember]
        public List<StepParameter> Parameters { get; set; }

        public StepSchema()
        {
            Parameters = new List<StepParameter>();
        }
    }
}