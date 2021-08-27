namespace RTSFramework_v01
{
    public abstract class Request : IAddable.IBeAddedable, IInPipelineStage
    {
        protected Request(string subpipeline_name) { pipeline_tag = new PipelineTag( subpipeline_name ); }
        public PipelineTag pipeline_tag { get; }
        public abstract void Process();
    }
}
