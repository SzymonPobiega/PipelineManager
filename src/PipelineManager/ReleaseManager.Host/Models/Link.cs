using System.Web.Http;

namespace ReleaseManager.Host.Models
{
    public class Link
    {
        public string Url { get; set; }
        public string Rel { get; set; }
        public string Type { get; set; }

        public Link(string url, string rel, string type)
        {
            Url = url;
            Rel = rel;
            Type = type;
        }

        public static Link To<TResource>(TResource resource, object id, string rel, string suffix = null)
            where TResource : ApiController
        {
            return To<TResource>(id, rel, suffix);
        }

        public static Link To<TResource>(object id, string rel, string suffix = null)
            where TResource : ApiController
        {
            var trailer = suffix != null 
                ? "/" + suffix 
                : "";
            var controller = typeof(TResource).Name.Replace("Controller","");
            var url = string.Format("{0}/{1}{2}", controller, id ,trailer);
            return new Link(url,rel, 
                ContentTypes.CreateFor(typeof(TResource).BaseType.GetGenericArguments()[0]));
        }
    }

}