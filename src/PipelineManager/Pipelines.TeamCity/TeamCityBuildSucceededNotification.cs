using System.Xml.Serialization;

namespace ReleaseManager.Process.TeamCity
{
    [XmlRoot("build")]
    public class TeamCityBuildSucceededNotification
    {
        [XmlElement("buildId")]
        public int BuildId { get; set; }
        [XmlElement("buildTypeId")]
        public string BuildTypeId { get; set; }

        [XmlElement("projectName")]
        public string ProjectName { get; set; }
        [XmlElement("projectId")]
        public string ProjectId { get; set; }

        [XmlElement("buildNumber")]
        public string BuildNumber { get; set; }
    }
}