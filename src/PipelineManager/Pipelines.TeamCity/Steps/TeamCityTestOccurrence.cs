using System.Xml.Serialization;

namespace ReleaseManager.Process.TeamCity.Steps
{
    [XmlType("testOccurrence")]
    public class TeamCityTestOccurrence
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("status")]
        public TestStatus Status { get; set; }
    }
}