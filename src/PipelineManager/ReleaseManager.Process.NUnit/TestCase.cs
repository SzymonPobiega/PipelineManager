namespace ReleaseManager.Process.NUnit
{
    public class TestCase
    {
        private readonly string _name;
        private readonly string _result;

        public TestCase(string name, string result)
        {
            _name = name;
            _result = result;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Result
        {
            get { return _result; }
        }
    }
}