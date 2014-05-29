using System;

namespace ReleaseManager.Host.Models
{
    public static class ContentTypes
    {
        public static string CreateFor(Type representationType)
        {
            return "application/vnd+releasemanager." +representationType.Name.Replace("Model", "").ToLowerInvariant() + "+json";
        }

        public static string CreateFor<T>()
        {
            return CreateFor(typeof (T));
        }

        public static string GetContentType<T>(this T result)
        {
            return CreateFor<T>();
        }
    }
}