namespace Pipelines
{
    public enum ActivityState
    {
        NotStarted,
        Running,
        Finished,
        Failing,
        Failed,
        WaitingForExternalDependency
    }
}