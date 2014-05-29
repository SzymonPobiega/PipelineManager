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
        private readonly ITeamCityClient _teamCityClient;
        public string SuiteType { get; set; }

        public TeamCityTestResultsDownloader(UniqueStepId stepId, ITeamCityClient teamCityClient)
            : base(stepId)
        {
            _teamCityClient = teamCityClient;
        }

        protected override bool Resume(IUnitOfWork unitOfWork)
        {
            var candidate = unitOfWork.LoadSubject<ReleaseCandidate>();

            var testOccurences = _teamCityClient.GetTestResults(candidate.BuildId);
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