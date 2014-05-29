using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReleaseManager.Host.Models
{
    public class ProjectState
    {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectHealth HealthStatus { get; set; }
        public string LatestVersion { get; set; }
        public Link Pipeline { get; set; }
    }
}