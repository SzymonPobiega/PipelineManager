using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Pipelines.Schema;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class Project
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual int SchemaVersion { get; protected set; }
        public virtual Project PreviousSchemaView { get; protected set; }
        public virtual bool IsLatestVersion { get; protected set; }

        public virtual IList<Stage> Stages { get; set; }
        public virtual IList<DeployedVersion> DeployedVersions { get; set; }

        public virtual Project CreateNewVersion(PipelineSchema schema)
        {
            IsLatestVersion = false;
            return new Project(Name, schema, this);
        }

        public static Project CreateNew(string name, PipelineSchema schema)
        {
            return new Project(name, schema, null);
        }

        private Project(string name, PipelineSchema schema, Project previousSchemaView)
        {
            Name = name;
            SchemaVersion = schema.Revision;
            PreviousSchemaView = previousSchemaView;
            IsLatestVersion = true;
            Stages = new List<Stage>();
            DeployedVersions = new List<DeployedVersion>();

            foreach (var stageSchema in schema.Stages)
            {
                var stage = new Stage(stageSchema.Name, stageSchema.TriggerMode);
                Stages.Add(stage);
            }
        }

        public virtual void StageFinished(string stageName, ReleaseCandidate candidate, DateTime occurenceDateUtc)
        {
            var stage = Stages.Single(x => x.Name == stageName);
            stage.FinishedBy(candidate, occurenceDateUtc);
        }


        public virtual void StageFailed(string stageName, ReleaseCandidate candidate, DateTime occurenceDateUtc)
        {
            var stage = Stages.Single(x => x.Name == stageName);
            stage.FailedBy(candidate, occurenceDateUtc);
        }

        protected Project()
        {
        }

        public class Mapping : ClassMapping<Project>
        {
            public Mapping()
            {
                Id(i => i.Id, m => m.Generator(Generators.Identity));
                Property(p => p.Name, m => m.Length(200));
                Property(p => p.SchemaVersion);
                Property(p => p.IsLatestVersion);

                ManyToOne(p => p.PreviousSchemaView, m =>
                {
                    m.Cascade(Cascade.Persist);
                    m.Lazy(LazyRelation.Proxy);
                });

                List(x => x.Stages,
                    list =>
                    {
                        list.Index(i => i.Column("StageIndex"));
                        list.Cascade(Cascade.All);
                    },
                    r => r.Component(c =>
                    {
                        c.Property(p => p.Name, m => m.Length(200));
                        c.Property(p => p.TriggerMode);
                        c.Property(p => p.Busy);
                        c.Property(p => p.LastVersion);
                        c.Property(p => p.LastActivityDateUtc);
                        c.ManyToOne(p => p.LastVersion, m =>
                        {
                            m.Cascade(Cascade.Persist);
                            m.Lazy(LazyRelation.NoLazy);
                        });
                        c.ManyToOne(p => p.LastSuccessfulVersion, m =>
                        {
                            m.Cascade(Cascade.Persist);
                            m.Lazy(LazyRelation.NoLazy);
                        });
                    }));

                Bag(x => x.DeployedVersions,
                    bag => bag.Cascade(Cascade.All),
                    r => r.Component(c =>
                    {
                        c.Property(p => p.Environment);
                        c.Property(p => p.Version);
                        c.Property(p => p.DeploymentDate);
                    }));
            }
        }
    }
}