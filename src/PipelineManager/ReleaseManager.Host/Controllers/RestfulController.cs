using System.Web.Http;

namespace ReleaseManager.Host.Controllers
{
    public abstract class RestfulController<TRepresentation> : ApiController
    {
    }
}