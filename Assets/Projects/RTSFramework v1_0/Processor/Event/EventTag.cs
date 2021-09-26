using System;
namespace RTSFramework_v1_0.Processor.Event
{
    public struct EventTag
    {
        public enum Relation
        {
            from,
            to
        }
        public Type request_type;
        public Relation relation;

        public EventTag(Type request_type, Relation relation)
        {
            this.request_type = request_type;
            this.relation = relation;
        }
    }
}
