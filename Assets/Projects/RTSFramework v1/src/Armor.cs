using System.Linq;
using System.Linq;
using System.Linq;
using System.Linq;
using RTSFramework_v1.Base.Change;
using RTSFramework_v1.Base.Event;
using RTSFramework_v1.Base.Pipeline;
using RTSFramework_v1.Base.Request;
namespace RTSFramework_v1
{
    public class Armor : IEditEffect
    {
        public PipelineTag pipeline_tag { get; }
        public Armor()
        {
            pipeline_tag = new PipelineTag( "Process" );
        }

        int_data data;
        float damage_reduction => data.value / (data.value + 100f);

        /// <summary>
        ///     Generate Edit Requests for the event
        /// </summary>
        public
            (ChangeRequestRequest[] changes,
            AddRequestRequest[] adds)
            Edit(in Effect e)
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