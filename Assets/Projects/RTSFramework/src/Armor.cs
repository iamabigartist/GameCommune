using System.Linq;
namespace RTSFramework
{
    public class Armor : IEditEvent
    {
        int_data data;
        public int pipeline_depth => 10;
        float damage_reduction => data.value / (data.value + 100f);

        public ChangeRequestRequest[] Evaluate(in Event e)
        {
            var physical_damages = e.requests.Where( (request) => request is PhysicalDamageRequest );
            var requests = physical_damages.AsParallel().Select( (damage) => new ChangeRequestRequest()
            {
                change = new PrimitiveChange()
                {
                    change_type = PrimitiveChange.ChangeType.Multiply,
                    data = new float_data( 1 - damage_reduction )
                },
                target = damage.change.data
            } ).ToArray();
            return requests;
        }
    }

    public class PhysicalDamageRequest : ChangeRequest
    {
        public PhysicalDamageRequest(float_data HP, PrimitiveChange change)
        {
            target = HP;
            this.change = change;
        }
    }
}
