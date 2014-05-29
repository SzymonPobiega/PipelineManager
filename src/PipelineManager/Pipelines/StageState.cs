namespace Pipelines
{
    public enum StageState
    {
        Finished,
        Failed,
        NotStarted,
        RequestsRetry,
        WaitingForDependency,
        OnHold
    }
}