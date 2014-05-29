using System.Xml.Serialization;

namespace ReleaseManager.Process.TeamCity.Steps
{
    [XmlRoot("build")]
    public class TeamCityBuildFinishedNotification
    {
        [XmlElement("buildId")]
        public int BuildId { get; set; }
        [XmlElement("buildTypeId")]
        public string BuildTypeId { get; set; }

        [XmlElement("buildResult")]
        public BuildResult BuildResult { get; set; }

        [XmlElement("projectName")]
        public string ProjectName { get; set; }
        [XmlElement("projectId")]
        public string ProjectId { get; set; }

        [XmlElement("buildNumber")]
        public string BuildNumber { get; set; }

        [XmlElement("buildStatusUrl")]
        public string BuildStatusUrl { get; set; }
        
        [XmlElement("buildStatusHtml")]
        public string BuildStatusHtml { get; set; }
        
        [XmlElement("message")]
        public string Message { get; set; }
    }
}