using Octopus.Client;
using Octopus.Client.Model;

namespace ReleaseManager.Process.Octopus
{
    public class OctopusFacade : IOctopusFacade
    {
        private readonly IOctopusRepository _octopusRepository;

        public OctopusFacade(IOctopusRepository octopusRepository)
        {
            _octopusRepository = octopusRepository;
        }

        public ReleaseResource FindRelease(string versionNumber)
        {
            return _octopusRepository.Releases.FindOne(r => r.Version == versionNumber);
        }

        public string CreateRelease(ReleaseResource release)
        {
            return _octopusRepository.Releases.Create(release).Id;
        }

        public ReleaseTemplateResource GetDeploymentProcessesTemplate(string deploymentProcessId)
        {
            var deploymentProcess = _octopusRepository.DeploymentProcesses.Get(deploymentProcessId);

            var releaseTemplate = _octopusRepository.DeploymentProcesses.GetTemplate(deploymentProcess);

            return releaseTemplate;
        }

        public ProjectResource FindProjectByName(string projectName)
        {
            return _octopusRepository.Projects.FindByName(projectName);
        }

        public EnvironmentResource FindEnvironmentByName(string environmentName)
        {
            return _octopusRepository.Environments.FindByName(environmentName);
        }

        public DeploymentResource CreateDeployment(string environmentId, string releaseId)
        {
            return _octopusRepository.Deployments.Create(new DeploymentResource()
            {
                EnvironmentId = environmentId,
                ReleaseId = releaseId
            });
        }

        public TaskResource WaitForTaskCompletion(string taskId)
        {
            var task = _octopusRepository.Tasks.Get(taskId);
            _octopusRepository.Tasks.WaitForCompletion(task);
            var updated = _octopusRepository.Tasks.Get(taskId);
            return updated;
        }
    }
}