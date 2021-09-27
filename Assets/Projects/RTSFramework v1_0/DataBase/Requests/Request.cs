using System;
using System.Collections.Generic;
using RTSFramework_v1_0.Processor.Pipeline;
using RTSFramework_v1_0.Processor.Reference;
namespace RTSFramework_v1_0.DataBase.Requests
{
    /// <summary>
    ///     An apply for the change of data, must be merged before execute
    /// </summary>
    public abstract class Request : IInPipelineStage, IReferable
    {
        protected Request(GamePipelineTable.Stage stage, IRequestFrom from, bool temporary)
        {
            this.stage = stage;
            this.from = from;
            will_be_null = temporary;
        }
        public GamePipelineTable.Stage stage { get; }
        public IRequestFrom from { get; }
        public abstract IRequestTo to { get; }


        /// <summary>
        ///     Execute this request right now.
        /// </summary>
        public abstract void Process();
        public bool will_be_null { get; set; }
    }

    /// <summary>
    ///     Other objects' functions may subscribe to a type of request with a relation of this object.
    /// </summary>
    /// <remarks>Has a bunch of evaluate functions of other objects which can run in parallel.</remarks>
    public interface IRequestRelation { }

    public interface IRequestFrom : IRequestRelation, IInPipelineStage
    {
        Dictionary<Type,
            RefList<RefOf<
                Func<Request, Request[]>>>> from_request_events { get; set; }

    }
    public interface IRequestTo : IRequestRelation
    {
        Dictionary<Type,
            RefList<RefOf<
                Func<Request, Request[]>>>> to_request_events { get; set; }
    }
}
