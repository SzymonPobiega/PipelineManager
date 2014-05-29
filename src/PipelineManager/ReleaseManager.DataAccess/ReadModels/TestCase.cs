using ReleaseManager.Events;

namespace ReleaseManager.DataAccess.ReadModels
{
    public class TestCase
    {
        public virtual string Name { get; protected set; }
        public virtual TestResult Result { get; protected set; }

        public TestCase(string name, TestResult result)
        {
            Name = name;
            Result = result;
        }

        protected TestCase()
        {
        }

        
    }
}