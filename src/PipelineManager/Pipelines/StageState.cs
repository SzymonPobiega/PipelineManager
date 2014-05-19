namespace Pipelines
{
    public enum StageState
    {
        Finished,
        NotStarted,
        WaitingForDependency,
        WaitingForManualTrigger
    }
}