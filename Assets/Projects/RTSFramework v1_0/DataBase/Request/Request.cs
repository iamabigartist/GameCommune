using RTSFramework_v1_0.Processor.Pipeline;
namespace RTSFramework_v1_0.DataBase.Request
{
    /// <summary>
    ///     An apply for the change of data, must be merged before execute
    /// </summary>
    public abstract class Request : IInPipelineStage
    {
        protected Request(string pipeline_name)
        {
            pipeline_tag = new PipelineTag( pipeline_name );
        }
        public PipelineTag pipeline_tag { get; }

        /// <summary>
        ///     Execute this request right now.
        /// </summary>
        public abstract void Process();

    }
}
