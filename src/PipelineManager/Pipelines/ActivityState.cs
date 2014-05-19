namespace Pipelines
{
    public enum ActivityState
    {
        NotStarted,
        Running,
        Finished,
        WaitingForExternalDependency
    }
}