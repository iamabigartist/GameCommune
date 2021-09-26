using RTSFramework_v1_0.Processor.Pipeline;
namespace RTSFramework_v1_0.DataBase.Request
{
    /// <summary>
    ///     An apply for the change of data, must be merged before execute
    /// </summary>
    public abstract class Request : IInPipelineStage
    {
        protected Request(GamePipelineTable.Stage stage, IRequestFrom from)
        {
            this.stage = stage;
            this.from = from;
        }
        public GamePipelineTable.Stage stage { get; }
        public IRequestFrom from { get; }
        public abstract IRequestTo to { get; }


        /// <summary>
        ///     Execute this request right now.
        /// </summary>
        public abstract void Process();

    }

    public interface IRequestFrom { }
    public interface IRequestTo { }
}
