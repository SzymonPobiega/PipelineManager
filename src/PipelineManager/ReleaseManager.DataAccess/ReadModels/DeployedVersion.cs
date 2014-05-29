using System;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class DeployedVersion
    {
        public string Environment { get; set; }
        public string Version { get; set; }
        public DateTime DeploymentDate { get; set; }
    }
}