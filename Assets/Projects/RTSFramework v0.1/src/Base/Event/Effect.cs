using System.Collections.Generic;
namespace RTSFramework_v01
{
    public class Effect : IAddable
    {
        public enum EffectType
        {
            /// <summary>
            ///     The effect will remain and be processed.
            /// </summary>
            Enabled,
            /// <summary>
            ///     The effect will remain but not be processed.
            /// </summary>
            Disabled,
            /// <summary>
            ///     The effect will be processed and removed from list afterwards.
            /// </summary>
            Temporary
        }
        public EffectType type;
        public List<Request> requests;
        public IEditEffect[][] editors_depth_groups;


        public void Add(IAddable.IBeAddedable item) { requests.Add( item as Request ); }
    }
}
