using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ReleaseManager.Events
{
    public class TestSuiteFinishedEvent
    {
        private readonly int _buildId;
        [NotNull]
        private readonly string _projectName;
        [NotNull]
        private readonly string _versionNumber;
        private readonly TestResult _result;
        private readonly string _suiteType;
        private readonly List<TestOutput> _testOutputs;

        public TestSuiteFinishedEvent(int buildId, [NotNull] string projectName, [NotNull] string versionNumber, TestResult result,
            [NotNull] string suiteType, List<TestOutput> testOutputs)
        {
            if (projectName == null) throw new ArgumentNullException("projectName");
            if (versionNumber == null) throw new ArgumentNullException("versionNumber");
            if (suiteType == null) throw new ArgumentNullException("suiteType");
            _buildId = buildId;
            _projectName = projectName;
            _versionNumber = versionNumber;
            _result = result;
            _suiteType = suiteType;
            _testOutputs = testOutputs;
        }

        public TestResult Result
        {
            get { return _result; }
        }

        public string SuiteType
        {
            get { return _suiteType; }
        }

        public IEnumerable<TestOutput> TestOutputs
        {
            get { return _testOutputs; }
        }

        public int BuildId
        {
            get { return _buildId; }
        }

        public string ProjectName
        {
            get { return _projectName; }
        }

        public string VersionNumber
        {
            get { return _versionNumber; }
        }
    }
}