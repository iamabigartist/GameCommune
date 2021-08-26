using System.Collections.Generic;
namespace RTSFramework
{
    public class Effect : IAddable
    {
        public enum EffectType
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
        public EffectType type;
        public List<Request> requests;
        public IEditEffect[][] editors_depth_groups;


        public void Add(IAddable.IBeAddedable item) { requests.Add( item as Request ); }
    }
}
