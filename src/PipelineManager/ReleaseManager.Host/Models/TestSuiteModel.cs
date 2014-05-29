using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReleaseManager.Events;

namespace ReleaseManager.Host.Models
{
    public class TestSuiteModel
    {
        public string Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TestResult Result { get; set; }
        public List<TestCaseModel> TestCases { get; set; }
    }
}