using System.Web.Http;

namespace ReleaseManager.Host.Models
{
    public abstract class Representation<TResource>
        where TResource : ApiController
    {
    }
}