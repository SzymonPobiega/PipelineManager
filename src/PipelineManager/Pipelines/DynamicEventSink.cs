using System.Reflection;

namespace Pipelines
{
    public class DynamicEventSink : IEventSink
    {
        private readonly object _target;

        public DynamicEventSink(object target)
        {
            _target = target;
        }

        public void On(object evnt)
        {
            var method = _target.GetType()
                .GetMethod("On", BindingFlags.Instance | BindingFlags.Public, null, new[] {evnt.GetType()}, null);
            if (method != null)
            {
                method.Invoke(_target, new[] {evnt});
            }
        }
    }
}