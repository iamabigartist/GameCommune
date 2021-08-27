namespace RTSFramework_v01
{
    public abstract class Request : IAddable.IBeAddedable, IInPipelineStage
    {
        protected Request(string subpipeline_name) { subpipeline_tag = new SubPipelineTag( subpipeline_name ); }
        public SubPipelineTag subpipeline_tag { get; }
        public abstract void Process();
    }
}
