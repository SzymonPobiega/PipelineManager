using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pipelines.Schema
{
    [DataContract]
    public class StageSchema
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public StageTriggerMode TriggerMode { get; set; }

        [DataMember]
        public List<ActivitySchema> Activities { get; set; }

        public StageSchema()
        {
            Activities = new List<ActivitySchema>();
        }
    }
}