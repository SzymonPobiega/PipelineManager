namespace Pipelines
{
    public class DataContainer
    {
        private object _data;

        public DataContainer(object data)
        {
            _data = data;
        }

        public bool HasData
        {
            get { return _data != null; }
        }

        public object Consume()
        {
            var result = _data;
            _data = null;
            return result;
        }
    }
}