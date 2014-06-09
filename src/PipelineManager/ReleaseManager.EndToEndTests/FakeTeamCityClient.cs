using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReleaseManager.Process.TeamCity.Steps;

namespace ReleaseManager.EndToEndTests
{
    public class FakeTeamCityClient : ITeamCityClient
    {
        public TeamCityTestOccurrences GetTestResults(int buildId)
        {
            throw new NotImplementedException();
        }
    }
}
