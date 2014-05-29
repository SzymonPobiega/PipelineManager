using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ReleaseManager.Process.TeamCity.Data
{
    public class TeamCityBuild
    {
        public virtual int Id { get; set; }
        public virtual string StatusUrl { get; set; }
        public virtual string StatusHtml { get; set; }
        public virtual string Message { get; set; }

        public TeamCityBuild(int id, string statusUrl, string statusHtml, string message)
        {
            Id = id;
            StatusUrl = statusUrl;
            StatusHtml = statusHtml;
            Message = message;
        }

        protected TeamCityBuild()
        {
        }

        public class TeamCityBuildMapping : ClassMapping<TeamCityBuild>
        {
            private const int MAX = 8001;

            public TeamCityBuildMapping()
            {
                Id(id => id.Id, m => m.Generator(Generators.Assigned));
                Property(p => p.StatusUrl, m => m.Length(4000));
                Property(p => p.StatusHtml, m => m.Length(MAX));
                Property(p => p.Message, m => m.Length(MAX));
            }
        }
    }
}