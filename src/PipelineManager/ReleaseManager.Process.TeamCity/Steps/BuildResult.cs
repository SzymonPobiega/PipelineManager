using System.Xml.Serialization;

namespace ReleaseManager.Process.TeamCity.Steps
{
    public enum BuildResult
    {
        [XmlEnum("success")]
        Success,
        [XmlEnum("failure")]
        Failure
    }
}