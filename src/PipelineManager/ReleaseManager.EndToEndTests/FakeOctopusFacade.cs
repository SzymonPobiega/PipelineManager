using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using Octopus.Platform.Model;
using ReleaseManager.Process.Octopus;

namespace ReleaseManager.EndToEndTests
{
    public class FakeOctopusFacade : IOctopusFacade
    {
        private readonly List<ReleaseResource> _releases = new List<ReleaseResource>(); 
        private readonly List<ProjectResource> _projects = new List<ProjectResource>(); 
        private readonly List<EnvironmentResource> _environments = new List<EnvironmentResource>(); 
        private readonly List<ExpectedDeployment> _expectedDeploymentResults = new List<ExpectedDeployment>();


        public FakeOctopusFacade WithEnvironment(string name)
        {
            _environments.Add(new EnvironmentResource()
            {
                Id = name,
                Name = name
            });
            return this;
        }

        public FakeOctopusFacade WithProject(string name)
        {
            _projects.Add(new ProjectResource()
            {
                Id = name,
                Name = name,
            });
            return this;
        }

        public FakeOctopusFacade WithExistingRelease(string versionNumber, string projectName)
        {
            _releases.Add(new ReleaseResource(versionNumber, projectName));
            return this;
        }

        public FakeOctopusFacade ExtpectDeploymentTimeoutFor(string environment, string projectName)
        {
            _expectedDeploymentResults.Add(new ExpectedDeployment(projectName, environment, Guid.NewGuid().ToString(), ExpectedDeploymentResult.Timeout));
            return this;
        }
        
        public FakeOctopusFacade ExtpectDeploymentFailureFor(string environment, string projectName)
        {
            _expectedDeploymentResults.Add(new ExpectedDeployment(projectName, environment, Guid.NewGuid().ToString(), ExpectedDeploymentResult.Failure));
            return this;
        }
        
        public FakeOctopusFacade ExtpectDeploymentSuccessFor(string environment, string projectName)
        {
            _expectedDeploymentResults.Add(new ExpectedDeployment(projectName, environment, Guid.NewGuid().ToString(), ExpectedDeploymentResult.Success));
            return this;
        }

        public ReleaseResource FindRelease(string versionNumber)
        {
            return _releases.FirstOrDefault(x => x.Version == versionNumber);
        }

        public ReleaseTemplateResource GetDeploymentProcessesTemplate(string deploymentProcessId)
        {
            return new ReleaseTemplateResource()
            {
                Packages = new ReleaseTemplatePackage[0]
            };
        }

        public ProjectResource FindProjectByName(string projectName)
        {
            return _projects.FirstOrDefault(x => x.Name == projectName);
        }

        public EnvironmentResource FindEnvironmentByName(string environmentName)
        {
            return _environments.FirstOrDefault(x => x.Name == environmentName);
        }

        public DeploymentResource CreateDeployment(string environmentId, string releaseId)
        {
            var release = _releases.FirstOrDefault(x => x.Id == releaseId);
            if (release  == null)
            {
                throw new ArgumentException("releaseId");
            }
            if (_environments.All(x => x.Id != environmentId))
            {
                throw new ArgumentException("environmentId");
            }
            var expectedResult = _expectedDeploymentResults.FirstOrDefault(x => x.Environment == environmentId && x.Project == release.ProjectId);


            var taskId = expectedResult != null ? expectedResult.TaskId : Guid.NewGuid().ToString();
            return new DeploymentResource()
            {
                Id = Guid.NewGuid().ToString(),
                TaskId = taskId
            };
        }

        public TaskResource WaitForTaskCompletion(string taskId)
        {
            var expectedResult = _expectedDeploymentResults.FirstOrDefault(x => x.TaskId == taskId);
            if (expectedResult != null)
            {
                if (expectedResult.Result == ExpectedDeploymentResult.Success)
                {
                    return new TaskResource
                    {
                        State = TaskState.Success
                    };
                }
                if (expectedResult.Result == ExpectedDeploymentResult.Failure)
                {
                    return new TaskResource
                    {
                        State = TaskState.Failed
                    };
                }
                throw new TimeoutException();
            }
            return new TaskResource
            {
                State = TaskState.Success
            };
        }

        public string CreateRelease(ReleaseResource release)
        {
            release.Id = Guid.NewGuid().ToString();
            _releases.Add(release);
            return release.Id;
        }

        private class ExpectedDeployment
        {
            private readonly string _project;
            private readonly string _environment;
            private readonly string _taskId;
            private readonly ExpectedDeploymentResult _result;

            public ExpectedDeployment(string project, string environment, string taskId, ExpectedDeploymentResult result)
            {
                _taskId = taskId;
                _result = result;
                _project = project;
                _environment = environment;
            }

            public string TaskId
            {
                get { return _taskId; }
            }

            public ExpectedDeploymentResult Result
            {
                get { return _result; }
            }

            public string Project
            {
                get { return _project; }
            }

            public string Environment
            {
                get { return _environment; }
            }
        }

        private enum ExpectedDeploymentResult
        {
            Success,
            Failure,
            Timeout
        }
    }
}