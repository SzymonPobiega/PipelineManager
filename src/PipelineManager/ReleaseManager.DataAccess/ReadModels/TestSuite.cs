using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using ReleaseManager.Events;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class TestSuite
    {
        public virtual int Id { get; protected set; }
        public virtual string Type { get; protected set; }
        public virtual TestResult Result { get; protected set; }
        public virtual IList<TestCase> TestCases { get; set; }

        public TestSuite(string type, TestResult result, IEnumerable<TestCase> testCases)
        {
            Type = type;
            Result = result;
            TestCases = testCases.ToList();
        }

        protected TestSuite()
        {
        }

        public class Mapping : ClassMapping<TestSuite>
        {
            public Mapping()
            {
                Id(i => i.Id, m => m.Generator(Generators.Native));
                Property(p => p.Type, m => m.Length(200));
                Property(p => p.Result);

                Bag(x => x.TestCases,
                    bag => bag.Cascade(Cascade.All),
                    r => r.Component(c =>
                    {
                        c.Property(p => p.Name);
                        c.Property(p => p.Result);
                    }));
            }
        }
    }
}