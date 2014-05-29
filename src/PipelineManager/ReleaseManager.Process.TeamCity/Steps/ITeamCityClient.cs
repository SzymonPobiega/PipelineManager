namespace ReleaseManager.Process.TeamCity.Steps
{
    public interface ITeamCityClient
    {
        TeamCityTestOccurrences GetTestResults(int buildId);
    }
}