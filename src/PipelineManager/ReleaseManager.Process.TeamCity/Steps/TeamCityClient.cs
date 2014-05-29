using System.Net.Http;
using System.Xml.Serialization;

namespace ReleaseManager.Process.TeamCity.Steps
{
    public class TeamCityClient : ITeamCityClient
    {
        private readonly string _baseTeamCityUrl;

        public TeamCityClient(string baseTeamCityUrl)
        {
            _baseTeamCityUrl = baseTeamCityUrl;
        }

        public TeamCityTestOccurrences GetTestResults(int buildId)
        {
            var requestUrl = _baseTeamCityUrl + string.Format(@"/guestAuth/app/rest/testOccurrences?locator=build:{0}", buildId);
            var httpClient = new HttpClient();
            using (var resultStream = httpClient.GetStreamAsync(requestUrl).Result)
            {
                var serializer = new XmlSerializer(typeof(TeamCityTestOccurrences));
                var testOccurences = (TeamCityTestOccurrences)serializer.Deserialize(resultStream);
                return testOccurences;
            }
        }
    }
}