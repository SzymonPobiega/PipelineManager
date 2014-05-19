using System.Collections.Generic;
using System.Xml.Serialization;

namespace ReleaseManager.Process.TeamCity
{
    [XmlRoot("testOccurrences")]
    public class TeamCityTestOccurrences
    {
        [XmlElement("testOccurrence")]
        public List<TeamCityTestOccurrence> Occurrences { get; set; } 
    }
}