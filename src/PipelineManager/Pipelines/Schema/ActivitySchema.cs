using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pipelines.Schema
{
    [DataContract]
    public class ActivitySchema
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<StepSchema> Steps { get; set; }

        public ActivitySchema()
        {
            Steps = new List<StepSchema>();
        }
    }
}