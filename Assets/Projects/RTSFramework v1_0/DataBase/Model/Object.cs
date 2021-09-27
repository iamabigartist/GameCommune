using System;
using System.Collections.Generic;
using RTSFramework_v1_0.DataBase.Requests;
using RTSFramework_v1_0.Processor.Pipeline;
using RTSFramework_v1_0.Processor.Reference;
namespace RTSFramework_v1_0.DataBase.Model
{
    public class Object : IRequestFrom, IDataBaseElement
    {

        public GamePipelineTable.Stage stage { get; }
        public Dictionary<Type,
            RefList<RefOf<
                Func<Request, Request[]>>>> from_request_events { get; set; }
        public IDatabase owner { get; set; }
    }
}
