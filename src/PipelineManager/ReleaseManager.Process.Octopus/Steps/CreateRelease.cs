using System;
using System.Linq;
using log4net;
using Octopus.Client;
using Octopus.Client.Model;
using Pipelines;
using ReleaseManager.Model;
using ReleaseManager.Process.Octopus.Model;

namespace ReleaseManager.Process.Octopus.Steps
{
    public class CreateRelease : Step
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (CreateRelease));

        private readonly IOctopusRepository _octopusRepository;

        public CreateRelease(UniqueStepId stepId, IOctopusRepository octopusRepository) 
            : base(stepId)
        {
            _octopusRepository = octopusRepository;
        }

        protected override bool Resume(IUnitOfWork unitOfWork)
        {
            var releaseCandidate = unitOfWork.LoadSubject<ReleaseCandidate>();
            var octopusRelease = unitOfWork.LoadSubject<OctopusRelease>();

            var projectName = releaseCandidate.ProjectName;
            var versionNumber = releaseCandidate.VersionNumber;

            var existingRelease = _octopusRepository.Releases.FindOne(r => r.Version == versionNumber);
            if (existingRelease != null)
            {
                Log.Info("A release with the number " + versionNumber + " already exists.");
                return false;
            }

            var release = CreateReleaseInOctopus(projectName, versionNumber);
            octopusRelease.ReleaseCreated(release.Id);
            return true;
        }

        private ReleaseResource CreateReleaseInOctopus(string projectName, string versionNumber)
        {
            var project = FindProject(projectName);
            var releaseTemplate = GetReleaseTemplate(projectName, project);

            Log.Debug("Creating release...");

            var release = _octopusRepository.Releases.Create(new ReleaseResource(versionNumber, project.Id)
            {
                SelectedPackages = releaseTemplate.Packages.Select(x => new SelectedPackage(x.StepName, versionNumber)).ToList()
            });
            Log.Info("Release " + release.Version + " created successfully!");
            return release;
        }

        private ReleaseTemplateResource GetReleaseTemplate(string projectName, ProjectResource project)
        {
            Log.Debug("Finding deployment process for project: " + projectName);
            var deploymentProcess = _octopusRepository.DeploymentProcesses.Get(project.DeploymentProcessId);

            Log.Debug("Finding release template...");
            var releaseTemplate = _octopusRepository.DeploymentProcesses.GetTemplate(deploymentProcess);
            return releaseTemplate;
        }

        private ProjectResource FindProject(string projectName)
        {
            Log.Debug("Finding project: " + projectName);
            var project = _octopusRepository.Projects.FindByName(projectName);
            if (project == null)
            {
                throw new InvalidOperationException("Could not find a project named: " + projectName);
            }
            return project;
        }
    }
}