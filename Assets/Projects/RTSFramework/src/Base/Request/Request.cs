namespace RTSFramework
{
    public abstract class Request : IAddable.IBeAddedable
    {
        protected Request(int pipeline_depth) { this.pipeline_depth = pipeline_depth; }
        public int pipeline_depth { get; }
        public abstract void Process();
    }
}
