using System;
using System.Collections.Generic;
using RTSFramework_v1_0.DataBase.Requests;
using RTSFramework_v1_0.Processor.Reference;
namespace RTSFramework_v1_0.DataBase.Model
{
    public class PrimitiveField<T> : IRequestTo where T : struct
    {
        public PrimitiveField(T value)
        {
            this.value = value;
            to_request_events = new Dictionary<Type,
                RefList<RefOf<
                    Func<Request, Request[]>>>>();
        }

        public T value;
        public Dictionary<Type,
            RefList<RefOf<
                Func<Request, Request[]>>>> to_request_events { get; set; }
    }

    ///Find a way to store the relation between field and gameobject/owner
    /// and a way to express the meaning of the field itself
}
