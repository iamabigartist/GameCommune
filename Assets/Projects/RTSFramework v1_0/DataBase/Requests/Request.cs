using System;
using System.Collections.Generic;
using RTSFramework_v1_0.Processor.Pipeline;
namespace RTSFramework_v1_0.DataBase.Requests
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

    /// <summary>
    ///     Other objects' functions may subscribe to a type of request with a relation of this object.
    /// </summary>
    /// <remarks>Has a bunch of evaluate functions of other objects which can run in parallel.</remarks>
    public interface IRequestRelation { }

    public interface IRequestFrom : IRequestRelation
    {

        Dictionary<Type,
            List<Func<Request, Request[]>>> from_event_lists { get; }
    }
    public interface IRequestTo : IRequestRelation
    {
        Dictionary<Type,
            List<Func<Request, Request[]>>> to_event_lists { get; }
    }
}
