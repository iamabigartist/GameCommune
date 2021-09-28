using MMOBALibrary.BaseComponent;
using MMOBALibrary.Definitions;
namespace MMOBALibrary.Damage
{
    public struct DamageEventArgs
    {
        public HitPoint damaged_HP;
        public DamageType damage_type;
        public EventTimePointType event_time_point_type;
    }
}
