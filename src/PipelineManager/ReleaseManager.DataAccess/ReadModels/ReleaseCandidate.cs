using System.Collections.Generic;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class ReleaseCandidate
    {
        public virtual int BuildId { get; protected set; }
        public virtual string ProjectName { get; protected set; }
        public virtual string VersionNumber { get; protected set; }
        public virtual string PipelineId { get; protected set; }

        public virtual IList<Deployment> Deployments { get; set; }
        public virtual IList<TestSuite> TestSuites { get; set; }

        public ReleaseCandidate(int buildId, string projectName, string versionNumber, string pipelineId)
        {
            BuildId = buildId;
            ProjectName = projectName;
            VersionNumber = versionNumber;
            PipelineId = pipelineId;

            Deployments = new List<Deployment>();
            TestSuites = new List<TestSuite>();
        }

        protected ReleaseCandidate()
        {            
        }

        public class Mapping : ClassMapping<ReleaseCandidate>
        {
            public Mapping()
            {
                Id(i => i.BuildId, m => m.Generator(Generators.Assigned));
                Property(p => p.ProjectName, m => m.Length(200));
                Property(p => p.VersionNumber, m => m.Length(30));
                Property(p => p.PipelineId, m => m.Length(200));

                Bag(x => x.TestSuites,
                    bag => bag.Cascade(Cascade.All),
                    r => r.OneToMany(o =>
                    {
                    }));

                Bag(x => x.Deployments,
                    bag => bag.Cascade(Cascade.All),
                    r => r.Component(c =>
                    {
                        c.Property(p => p.UniqueId, m => m.Length(200));
                        c.Property(p => p.Environment, m => m.Length(200));
                        c.Property(p => p.Date);
                        c.Property(p => p.Success);
                    }));
            }
        }
    }
}