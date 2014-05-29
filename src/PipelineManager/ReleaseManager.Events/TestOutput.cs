namespace ReleaseManager.Events
{
    public class TestOutput
    {
        private readonly string _name;
        private readonly TestResult _result;

        public TestOutput(string name, TestResult result)
        {
            _name = name;
            _result = result;
        }

        public string Name
        {
            get { return _name; }
        }

        public TestResult Result
        {
            get { return _result; }
        }
    }
}