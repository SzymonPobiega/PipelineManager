using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class Environment
    {
        public virtual string Name { get; protected set; }
        public virtual ReleaseCandidate LastDeployedRelease { get; set; }

        public Environment(string name)
        {
            Name = name;
        }

        public virtual void CandidateDeployed(ReleaseCandidate candidate)
        {
            LastDeployedRelease = candidate;
        }

        protected Environment()
        {
        }

        public class Mapping : ClassMapping<Environment>
        {
            public Mapping()
            {
                Id(i => i.Name, m => m.Generator(Generators.Assigned));
                ManyToOne(p => p.LastDeployedRelease, m =>
                {
                    m.Cascade(Cascade.Persist);
                    m.Lazy(LazyRelation.Proxy);
                });
            }
        }
    }
}