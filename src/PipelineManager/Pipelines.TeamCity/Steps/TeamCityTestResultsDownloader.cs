using System;
using System.Linq;
using System.Net.Http;
using System.Xml.Serialization;
using Pipelines;
using ReleaseManager.Events;
using ReleaseManager.Model;

namespace ReleaseManager.Process.TeamCity.Steps
{
    public class TeamCityTestResultsDownloader : Step
    {
        public string TeamCityUrl { get; set; }
        public string SuiteType { get; set; }

        public TeamCityTestResultsDownloader(UniqueStepId stepId)
            : base(stepId)
        {
        }

        protected override bool Resume(IUnitOfWork unitOfWork)
        {
            var candidate = unitOfWork.LoadSubject<ReleaseCandidate>();

            TeamCityTestOccurrences testOccurences;
            var requestUrl = TeamCityUrl + string.Format(@"/guestAuth/app/rest/testOccurrences?locator=build:{0}", candidate.BuildId);
            var httpClient = new HttpClient();
            using (var resultStream = httpClient.GetStreamAsync(requestUrl).Result)
            {
                var serializer = new XmlSerializer(typeof(TeamCityTestOccurrences));
                testOccurences = (TeamCityTestOccurrences)serializer.Deserialize(resultStream);
            }
            var result = testOccurences.Occurrences.Any(x => x.Status == TestStatus.FAILURE)
                    ? TestResult.Failed
                    : TestResult.Success;

            var outputs = testOccurences.Occurrences.Select(x => new TestOutput(x.Name, MapStatus(x.Status))).ToList();

            candidate.ProcessTestSuiteResults(result, SuiteType, outputs);
            return result != TestResult.Failed;
        }

        private TestResult MapStatus(TestStatus status)
        {
            switch (status)
            {
                case TestStatus.FAILURE:
                    return TestResult.Failed;
                case TestStatus.SUCCESS:
                    return TestResult.Success;
                case TestStatus.UNKNOWN:
                    return TestResult.Inconclusive;
                default:
                    throw new NotSupportedException("Not supported test status: " + status);
            }
        }
    }
}