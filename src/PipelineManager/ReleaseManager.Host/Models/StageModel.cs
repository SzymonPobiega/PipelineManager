using System.Collections.Generic;

namespace ReleaseManager.Host.Models
{
    public class StageModel
    {
        public string Name { get; set; }
        public string LatestVersion { get; set; }
        public string LatestSuccessfulVersion { get; set; }
        public Link LatestBuild { get; set; }
        public Link LatestSuccessfulBuild { get; set; }
        public List<ActivityModel> Activities { get; set; } 
    }
}