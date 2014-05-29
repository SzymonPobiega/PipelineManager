using System.Net;
using System.Web.Http;
using NHibernate;
using ReleaseManager.Process.Octopus.DataAccess;

namespace ReleaseManager.Process.Octopus.Controllers
{
    public class OctopusDeploymentDetailsController : ApiController
    {
        private readonly ISessionFactory _sessionFactory;

        public OctopusDeploymentDetailsController(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public OctopusDeploymentDetailsModel Get(string id)
        {
            OctopusDeploymentDetails details;
            using (var session = _sessionFactory.OpenSession())
            {
                details = session.Get<OctopusDeploymentDetails>(id);
            }
            if (details == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new OctopusDeploymentDetailsModel()
            {
                Url = "http://localhost:8070" + details.DetailsUrl.Replace("/api/", "/app#/")
            };
        }
    }
}