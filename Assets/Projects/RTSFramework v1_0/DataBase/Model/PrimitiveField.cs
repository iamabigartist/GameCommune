using System;
using System.Collections.Generic;
using RTSFramework_v1_0.DataBase.Requests;
namespace RTSFramework_v1_0.DataBase.Model
{
    public class PrimitiveField<T> : IRequestTo where T : struct
    {
        public PrimitiveField(T value)
        {
            this.value = value;
            to_event_lists = new Dictionary<Type, List<Func<Request, Request[]>>>();
        }

        public T value;
        public Dictionary<Type, List<Func<Request, Request[]>>> to_event_lists { get; }
    }

    ///Find a way to store the relation between field and gameobject/database
    /// and a way to express the meaning of the field itself
}
