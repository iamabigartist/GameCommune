using System;
using System.Linq;
using System.Threading.Tasks;
namespace RTSFramework
{
    public static class EventProcessing
    {
        static PrimitiveChange ReducePrimitiveChanges(PrimitiveChange[] primitive_changes, PrimitiveChange.ChangeType change_type)
        {
            if (primitive_changes.AsParallel().Any( change => change.change_type != change_type ))
            {
                throw new ArgumentException( "Different kind primitive changes!" );
            }
            switch (change_type)
            {
                case PrimitiveChange.ChangeType.Add:
                    {
                        float sum = primitive_changes.AsParallel().Aggregate( 0f, (s, change) => s + change.data.value );
                        return new PrimitiveChange() { change_type = change_type, data = new float_data( sum ) };
                    }
                case PrimitiveChange.ChangeType.Multiply:
                    {
                        float product = primitive_changes.AsParallel().Aggregate( 1f, (s, change) => s * change.data.value );
                        return new PrimitiveChange() { change_type = change_type, data = new float_data( product ) };
                    }
                case PrimitiveChange.ChangeType.Flip:
                    {
                        bool flip = primitive_changes.AsParallel().Aggregate( false, (f, change) => !f );
                        return new PrimitiveChange() { change_type = change_type, data = new float_data( flip ? 1 : 0 ) };
                    }
                case PrimitiveChange.ChangeType.TurnOff:
                case PrimitiveChange.ChangeType.TurnOn:
                    {
                        return new PrimitiveChange() { change_type = change_type };
                    }
                default: throw new ArgumentOutOfRangeException( nameof(change_type), change_type, null );
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="original_requests">any change request requests in one pipeline stage</param>
        /// <returns> reduced_requests that are all unique</returns>
        static ChangeRequestRequest[] ReduceChangeRequests(ChangeRequestRequest[] original_requests)
        {
            return original_requests.GroupBy(
                (request) => (request.target,request.pipeline_depth),
                (request) => request.change,
                (target, changes) =>
                {
                    var changes_array = changes.ToArray();
                    return new ChangeRequestRequest()
                    {
                        change = ReducePrimitiveChanges( changes_array, changes_array[0].change_type ),
                        target = target
                    };
                }
            ).ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="changes"></param>
        static void ApplyUniqueRequestsOnEvent(ChangeRequestRequest[] changes, AddRequestRequest<Request>[] adds)
        {
            Parallel.ForEach( changes, (request) => { request.Process(); } );
            foreach (var add in adds) { add.Process(); }
        }

        static void ModifyEvent(Event e)
        {
            foreach (IEditEvent[] stage_modifies in e.modifies)
            {
                var event_requests =
                    stage_modifies.AsParallel().Select( modify => modify.Edit( e ) ).ToArray();
                var change_request_requests =
                    event_requests.SelectMany( (tuple) => tuple.changes ).ToArray();
                var add_request_requests =
                    event_requests.SelectMany( (tuple) => tuple.adds ).ToArray();
                change_request_requests = ReduceChangeRequests( change_request_requests );
                ApplyUniqueRequestsOnEvent( change_request_requests, add_request_requests );
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="e">the event being processed</param>
        public static void ProcessEvent(Event e)
        {
            ModifyEvent(e);
            for
        }
    }
}
