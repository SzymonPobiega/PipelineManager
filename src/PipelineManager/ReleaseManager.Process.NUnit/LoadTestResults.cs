using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pipelines;
using ReleaseManager.Events;
using ReleaseManager.Model;

namespace ReleaseManager.Process.NUnit
{
    public class LoadTestResults : Step<NUnitTestResults>
    {
        public string SuiteType { get; set; }

        public LoadTestResults(UniqueStepId stepId) : base(stepId)
        {
        }

        protected override bool Resume(IUnitOfWork unitOfWork, NUnitTestResults data)
        {
            var testCases = data.TestCases;

            var succeeded = testCases
                .Where(Succeeded)
                .Select(x => new TestOutput(x.Name, TestResult.Success))
                .ToList();

            var failed = testCases
                .Where(Failed)
                .Select(x => new TestOutput(x.Name, TestResult.Failed))
                .ToList();

            var inconclusive = testCases
                .Where(x => !Succeeded(x) && !Failed(x))
                .Select(x => new TestOutput(x.Name, TestResult.Inconclusive))
                .ToList();

            var result = failed.Any() ? TestResult.Failed : TestResult.Success;
            var outputs = succeeded.Concat(failed).Concat(inconclusive).ToList();

            var candidate = unitOfWork.LoadSubject<ReleaseCandidate>();
            candidate.ProcessTestSuiteResults(result, SuiteType, outputs);

            return result != TestResult.Failed;
        }

        private static bool Failed(TestCase testCase)
        {
            return testCase.Result == "Error" || testCase.Result == "Failure";
        }

        private static bool Succeeded(TestCase testCase)
        {
            return testCase.Result == "Success";
        }
    }
}
