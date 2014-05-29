using System.IO;
using System.Linq;
using System.Xml.Linq;
using Pipelines;

namespace ReleaseManager.Process.NUnit
{
    public class NUnitTestResultsTransformer : IInputTransformer<NUnitTestResults>
    {
        public NUnitTestResults Transform(Stream data)
        {
            var root = XElement.Load(data);

            var testCases = root.Descendants("test-case");

            return new NUnitTestResults()
            {
                TestCases = testCases.Select(x => new TestCase(x.Attribute("name").Value, x.Attribute("result").Value)).ToList()
            };
        }
    }
}