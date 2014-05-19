namespace Pipelines
{
    public abstract class PipelineSubject
    {
        internal IEventSink Sink { get; set; }

        protected void Apply(object evnt)
        {
            Sink.On(evnt);
        }

        internal void UpdateState(object evnt)
        {
            new DynamicEventSink(this).On(evnt);
        }
    }
}