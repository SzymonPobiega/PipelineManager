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

        private readonly IOctopusFacade _octopusFacade;

        public CreateRelease(UniqueStepId stepId, IOctopusFacade octopusFacade) 
            : base(stepId)
        {
            _octopusFacade = octopusFacade;
        }

        protected override bool Resume(IUnitOfWork unitOfWork)
        {
            var releaseCandidate = unitOfWork.LoadSubject<ReleaseCandidate>();
            var octopusRelease = unitOfWork.LoadSubject<OctopusRelease>();

            var projectName = releaseCandidate.ProjectName;
            var versionNumber = releaseCandidate.VersionNumber;

            var existingRelease = _octopusFacade.FindRelease(versionNumber);
            if (existingRelease != null)
            {
                Log.Info("A release with the number " + versionNumber + " already exists.");
                return false;
            }

            var releaseId = CreateReleaseInOctopus(projectName, versionNumber);
            octopusRelease.ReleaseCreated(releaseId);
            return true;
        }

        private string CreateReleaseInOctopus(string projectName, string versionNumber)
        {
            var project = FindProject(projectName);
            var releaseTemplate = GetReleaseTemplate(project);

            Log.Debug("Creating release...");

            var releaseId = _octopusFacade.CreateRelease(new ReleaseResource(versionNumber, project.Id)
            {
                SelectedPackages = releaseTemplate.Packages.Select(x => new SelectedPackage(x.StepName, versionNumber)).ToList()
            });
            Log.Info("Release " + releaseId + " created successfully!");
            return releaseId;
        }

        private ReleaseTemplateResource GetReleaseTemplate(ProjectResource project)
        {
            var releaseTemplate = _octopusFacade.GetDeploymentProcessesTemplate(project.DeploymentProcessId);
            return releaseTemplate;
        }

        private ProjectResource FindProject(string projectName)
        {
            Log.Debug("Finding project: " + projectName);
            var project = _octopusFacade.FindProjectByName(projectName);
            if (project == null)
            {
                throw new InvalidOperationException("Could not find a project named: " + projectName);
            }
            return project;
        }
    }
}