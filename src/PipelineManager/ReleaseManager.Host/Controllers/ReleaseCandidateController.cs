using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using NHibernate;
using Pipelines;
using Pipelines.Infrastructure;
using Pipelines.Infrastructure.Records;
using ReleaseManager.DataAccess;
using ReleaseManager.DataAccess.ReadModels;
using ReleaseManager.Host.Models;
using Environment = ReleaseManager.DataAccess.ReadModels.Environment;

namespace ReleaseManager.Host.Controllers
{
    public class ReleaseCandidateController : RestfulController<ReleaseCandidateModel>
    {
        private readonly ISessionFactory _sessionFactory;

        public ReleaseCandidateController(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public ReleaseCandidateModel Get(int id)
        {
            ReleaseCandidate candidate;
            IList<Environment> environments;
            using (var session = _sessionFactory.OpenSession())
            {
                candidate = session.Get<ReleaseCandidate>(id);
                if (candidate == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                environments = session.QueryOver<Environment>().Where(x => x.LastDeployedRelease == candidate).List();
            }

            return new ReleaseCandidateModel()
            {
                Id = id,
                ProjectName = candidate.ProjectName,
                Version = candidate.VersionNumber,
                DeployedTo = environments.Select(x => x.Name).ToArray(),
                TestResults = Link.To(this, id, "tests", "tests"),
                History = Link.To(this, id, "history","history"),
                Deployments = Link.To(this, id, "deployments","deployments")
            };
        }

        [Route("releasecandidate/{id}/history")]
        public ReleaseCandidateHistoryModel GetHistory(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var candidate = session.Get<ReleaseCandidate>(id);
                if (candidate == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                var events = session.QueryOver<EventRecord>()
                    .Where(x => x.PipelineId == candidate.PipelineId)
                    .List()
                    .OrderBy(x => x.Sequence)
                    .ToList();

                var datesAndPayloads = events.Select(x => Tuple.Create(x.OccurenceDate, x.DeserializePayload())).ToList();
                var model = new ReleaseCandidateHistoryModel();
                var dispatcher = new EventDispatcher(new DynamicEventSink(model));
                foreach (var tuple in datesAndPayloads)
                {
                    dispatcher.Dispatch(session, candidate.PipelineId, tuple.Item2, tuple.Item1);
                }
                return model;
            }
        }

        [Route("releasecandidate/{id}/tests")]
        public ReleaseCandidateTestingModel GetTestResults(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var candidate = session.Get<ReleaseCandidate>(id);
                if (candidate == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return new ReleaseCandidateTestingModel
                {
                    Suites = candidate.TestSuites.Select(ts => new TestSuiteModel
                    {
                        Result = ts.Result,
                        Type = ts.Type,
                        TestCases = ts.TestCases.Select(tc => new TestCaseModel
                        {
                            Name = tc.Name,
                            Result = tc.Result
                        }).ToList()
                    }).ToList()
                };
            }
        }

        [Route("releasecandidate/{id}/deployments")]
        public List<DeploymentModel> GetDeployments(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var candidate = session.Get<ReleaseCandidate>(id);
                if (candidate == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return candidate.Deployments.Select(x => new DeploymentModel
                {
                    UniqueId = x.UniqueId,
                    Environment = x.Environment,
                    DeploymentDate = x.Date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss", CultureInfo.InvariantCulture),
                    Success = x.Success,
                }).ToList();
            }
        }
    }
}