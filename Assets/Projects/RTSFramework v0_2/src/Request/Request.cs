using RTSFramework_v0_2.src.Pipeline;
namespace RTSFramework_v0_2.src.Request
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

        /// <summary>
        ///     Judge whether <see cref="another_request" /> is able to be merged with this requests
        /// </summary>
        public abstract bool AbleToReduce(Request another_request);

        // /// <summary>
        // ///     Merge this request with a group of requests that <see cref="AbleToReduce{TRequest}" /> with this request.
        // /// </summary>
        // /// <returns>A new request that is produced by the reduction of previous requests</returns>
        // public virtual Request ReduceRequests(Request[] requests) { return this; }
    }
}
