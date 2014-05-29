using System.Linq;
using System.Net;
using System.Net.Http;
using NHibernate;
using ReleaseManager.DataAccess;
using ReleaseManager.DataAccess.ReadModels;
using ReleaseManager.Host.Models;

namespace ReleaseManager.Host.Controllers
{
    public class ProjectsController : RestfulController<ProjectsModel>
    {
        private readonly ISessionFactory _sessionFactory;

        public ProjectsController(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public HttpResponseMessage Get()
        {
            var session = _sessionFactory.OpenSession();
            var projects = session.QueryOver<Project>()
                .Where(x => x.IsLatestVersion)
                .List();


            var projectStates2 = Enumerable.Range(1, 5).Select(x => new ProjectState()
            {
                Name = projects.First().Name + x,
                HealthStatus = ProjectHealth.OK,
                LatestVersion = projects.First().Stages.First().LastSuccessfulVersion.VersionNumber,
                Pipeline = Link.To<PipelineController>(projects.First().Name, "pipeline")
            });

            var projectStates = projects.Select(x => new ProjectState()
            {
                Name = x.Name,
                HealthStatus = ProjectHealth.OK,
                LatestVersion = x.Stages.First().LastSuccessfulVersion.VersionNumber,
                Pipeline =  Link.To<PipelineController>(x.Name, "pipeline")
            });

            var result = new ProjectsModel()
            {
                Projects = projectStates.ToList()
            };

            return Request.CreateResponse(HttpStatusCode.OK, result, result.GetContentType());
        }

    }
}