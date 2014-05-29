using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReleaseManager.Events;

namespace ReleaseManager.Host.Models
{
    public class TestCaseModel
    {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TestResult Result { get; set; }
    }
}