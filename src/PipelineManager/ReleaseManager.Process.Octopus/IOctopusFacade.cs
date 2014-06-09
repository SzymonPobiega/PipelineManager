using Octopus.Client.Model;

namespace ReleaseManager.Process.Octopus
{
    public interface IOctopusFacade
    {
        ReleaseResource FindRelease(string versionNumber);
        string CreateRelease(ReleaseResource release);
        ReleaseTemplateResource GetDeploymentProcessesTemplate(string deploymentProcessId);
        ProjectResource FindProjectByName(string projectName);
        EnvironmentResource FindEnvironmentByName(string environmentName);
        DeploymentResource CreateDeployment(string environmentId, string releaseId);
        TaskResource WaitForTaskCompletion(string taskId);
    }
}