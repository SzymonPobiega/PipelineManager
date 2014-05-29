using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using NHibernate;
using ReleaseManager.DataAccess;
using ReleaseManager.DataAccess.ReadModels;
using ReleaseManager.Host.Models;

namespace ReleaseManager.Host.Controllers
{
    public class PipelineController : RestfulController<PipelineModel>
    {
        private readonly ISessionFactory _sessionFactory;

        public PipelineController(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }


        public HttpResponseMessage Get(string id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var project = session.QueryOver<Project>().Where(x => x.Name == id && x.IsLatestVersion).SingleOrDefault();
                var result = project == null
                    ? new PipelineModel { Name = id }
                    : new PipelineModel
                    {
                        Name = project.Name,
                        Stages = project.Stages.Select(MapStage).ToList()
                    };
                return Request.CreateResponse(HttpStatusCode.OK, result, result.GetContentType());
            }
        }

        private StageModel MapStage(Stage source)
        {
            return new StageModel
            {
                Name = source.Name,
                LatestVersion = source.LastVersion != null ? source.LastVersion.VersionNumber : "",
                LatestSuccessfulVersion = source.LastSuccessfulVersion != null ? source.LastSuccessfulVersion.VersionNumber : "",
                LatestBuild = source.LastVersion != null
                    ? Link.To<ReleaseCandidateController>(source.LastVersion.BuildId.ToString(), "latest-build")
                    : null,
                LatestSuccessfulBuild = source.LastSuccessfulVersion != null
                    ? Link.To<ReleaseCandidateController>(source.LastSuccessfulVersion.BuildId.ToString(),
                        "latest-successful-build")
                    : null,
                Activities = new List<ActivityModel>
                {
                    new ActivityModel
                    {
                        Name = source.Name,
                        Busy = source.Busy
                    }
                }
            };
        }
    }
}
