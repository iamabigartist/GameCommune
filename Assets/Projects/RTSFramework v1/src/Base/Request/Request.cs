using RTSFramework_v1.Base.Pipeline;
namespace RTSFramework_v1.Base.Request
{
    public abstract class Request : IAddable.IBeAddedable, IInPipelineStage
    {
        protected Request(string subpipeline_name) { pipeline_tag = new PipelineTag( subpipeline_name ); }
        public PipelineTag pipeline_tag { get; }
        public abstract void Process();
    }
}
