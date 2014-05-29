using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ReleaseManager.Process.Octopus.DataAccess
{
    public class OctopusDeploymentDetails
    {
        public virtual string Id { get; protected set; }
        public virtual string DetailsUrl { get; protected set; }

        public OctopusDeploymentDetails(string id, string detailsUrl)
        {
            Id = id;
            DetailsUrl = detailsUrl;
        }

        protected OctopusDeploymentDetails()
        {
        }

        public class OctopusDeploymentDetailsMapping : ClassMapping<OctopusDeploymentDetails>
        {
            public OctopusDeploymentDetailsMapping()
            {
                Id(i => i.Id, m =>
                {
                    m.Generator(Generators.Assigned);
                    m.Length(200);
                });
                Property(p => p.DetailsUrl, m => m.Length(2000));
            }
        }
    }
}