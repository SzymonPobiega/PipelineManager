using System.Runtime.Serialization;

namespace Pipelines.Schema
{
    [DataContract]
    [KnownType(typeof(string))]
    [KnownType(typeof(int))]
    [KnownType(typeof(bool))]
    public class StepParameter
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public object Value { get; set; }
    }
}