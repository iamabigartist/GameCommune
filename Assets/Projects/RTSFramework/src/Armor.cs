using System.Linq;
namespace RTSFramework
{
    public class Armor : IEditEvent
    {
        int_data data;

        public SubPipelineTag subpipeline_tag
        {
            get;
        }

        float damage_reduction => data.value / (data.value + 100f);

        /// <summary>
        ///     Generate Edit Requests for the event
        /// </summary>
        public
            (ChangeRequestRequest[] changes,
            AddRequestRequest[] adds)
            Edit(in Event e)
        {
            var physical_damages =
                e.requests.Select( (request) => request as PhysicalDamageRequest ).
                    Where( (request) => request != null );
            var changes = physical_damages.AsParallel().Select(
                (damage) =>
                    new ChangeRequestRequest( "Process",
                        new PrimitiveChange(
                            PrimitiveChange.ChangeType.Multiply,
                            new float_data( 1 - damage_reduction ) ),
                        damage.change.data ) ).ToArray();
            return (changes, null);
        }


    }

    public class PhysicalDamageRequest : ChangeRequest
    {
        public PhysicalDamageRequest(string subpipeline_name, float damage, float_data target_HP) :
            base( subpipeline_name,
                new PrimitiveChange(
                    PrimitiveChange.ChangeType.Add,
                    new float_data( -damage ) ),
                target_HP ) { }
    }
}
