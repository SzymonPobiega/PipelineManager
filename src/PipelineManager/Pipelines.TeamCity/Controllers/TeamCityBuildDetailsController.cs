using System.Net;
using System.Web.Http;
using NHibernate;
using ReleaseManager.Process.TeamCity.Data;

namespace ReleaseManager.Process.TeamCity.Controllers
{
    public class TeamCityBuildDetailsController : ApiController
    {
        private readonly ISessionFactory _sessionFactory;

        public TeamCityBuildDetailsController(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public TeamCityBuildDetailsModel Get(int id)
        {
            var session = _sessionFactory.OpenSession();
            var data = session.Get<TeamCityBuild>(id);
            if (data == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new TeamCityBuildDetailsModel
            {
                StatusHtml = data.StatusHtml,
                Message = data.Message,
                StatusUrl = data.StatusUrl
            };
        }
    }
}