using System.Collections.Generic;
namespace RTSFramework
{
    public class Event : IAddable
    {
        public enum EventType
        {
            /// <summary>
            ///     The event will remain and be processed.
            /// </summary>
            Enabled,
            /// <summary>
            ///     The event will remain but not be processed.
            /// </summary>
            Disabled,
            /// <summary>
            ///     The event will be processed and removed from list afterwards.
            /// </summary>
            Temporary
        }
        public EventType type;
        public List<Request> requests;
        public IEditEvent[][] modifies;


        public void Add(IAddable.IBeAddedable item) { requests.Add( item as Request ); }
    }
}
