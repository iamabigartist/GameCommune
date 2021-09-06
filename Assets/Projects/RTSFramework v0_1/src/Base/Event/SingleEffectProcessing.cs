using System;
using System.Linq;
using System.Threading.Tasks;
using RTSFramework_v0_1.src.Base.Change;
using RTSFramework_v0_1.src.Base.Request;
namespace RTSFramework_v0_1.src.Base.Event
{
    public static class SingleEffectProcessing
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
                        return new PrimitiveChange( change_type, new float_data( sum ) );
                    }
                case PrimitiveChange.ChangeType.Multiply:
                    {
                        float product = primitive_changes.AsParallel().Aggregate( 1f, (s, change) => s * change.data.value );
                        return new PrimitiveChange( change_type, new float_data( product ) );
                    }
                case PrimitiveChange.ChangeType.Flip:
                    {
                        bool flip = primitive_changes.AsParallel().Aggregate( false, (f, change) => !f );
                        return new PrimitiveChange( change_type, new float_data( flip ? 1 : 0 ) );
                    }
                case PrimitiveChange.ChangeType.TurnOff:
                case PrimitiveChange.ChangeType.TurnOn:
                    {
                        return new PrimitiveChange( change_type, null );
                    }
                default: throw new ArgumentOutOfRangeException( nameof(change_type), change_type, null );
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="original_requests">any change request requests in one stage</param>
        /// <returns> reduced_requests that are all unique</returns>
        static ChangeRequest[] ReduceChangeRequests(ChangeRequest[] original_requests, string subpipeline_name)
        {
            return original_requests.GroupBy(
                (request) => request.target,
                (request) => request.change,
                (requests_index, changes) =>
                {
                    var changes_array = changes.ToArray();
                    return new ChangeRequest( subpipeline_name,
                        ReducePrimitiveChanges( changes_array, changes_array[0].change_type ),
                        requests_index );
                }
            ).ToArray();
        }

        /// <summary>
        ///     Apply a bunch of unique requests in one pipeline stage
        /// </summary>
        static void ProcessRequestsSingleDepth(in ChangeRequest[] changes, in AddRequest[] adds)
        {
            Parallel.ForEach( changes, (change) => { change.Process(); } );
            foreach (var add in adds) { add.Process(); }
        }

        /// <summary>
        ///     Process normal requests will need to sort the stage first
        /// </summary>
        static void ProcessRequestsDepthGroups(Request.Request[] origin_requests)
        {

            //首先是分深度
            //然后reduce 改变请求
            //然后一个一个深度的处理请求

            var requests_groups =
                origin_requests.AsParallel().GroupBy(
                    (request) => request.pipeline_tag,
                    (depth, request) => request.ToArray() ).ToArray();
            foreach (var requests in requests_groups)
            {
                var changes_adds = requests.GroupBy( (request) => request is ChangeRequest ).ToArray();
                var adds = changes_adds.FirstOrDefault( group => !group.Key )?.Select( (request) => request as AddRequest ).ToArray();
                var changes = changes_adds.FirstOrDefault( group => group.Key )?.Select( (request) => request as ChangeRequest ).ToArray();
                if (changes != null) { changes = ReduceChangeRequests( changes, changes[0].pipeline_tag.pipeline_name ); }
                ProcessRequestsSingleDepth( changes, adds );

            }
        }

        // static void ApplyRequestsSingleDepth(in Request[] requests)
        // {
        //     foreach (var request in requests) { request.Process(); }
        // }
        //
        // static void ApplyRequests(in Request[] requests)
        // {
        //     //首先是分深度
        //     //然后reduce请求
        //     //然后一个一个深度的处理请求
        //     (ChangeRequest[], AddRequest[])[] requests_depth_groups =
        // }

        static void BuildEffect(Effect e)
        {
            foreach (IEditEffect[] editors in e.editors_depth_groups)
            {
                var event_edit_requests =
                    editors.AsParallel().Select( modify => modify.Edit( e ) ).ToArray();
                var change_request_requests =
                    event_edit_requests.SelectMany( (tuple) => tuple.changes ).
                        Select( (change) => change as ChangeRequest ).ToArray();
                var add_request_requests =
                    event_edit_requests.SelectMany( (tuple) => tuple.adds ).
                        Select( (change) => change as AddRequest ).ToArray();
                ProcessRequestsSingleDepth( change_request_requests, add_request_requests );
            }
        }

        public static void ProcessEffect(Effect e)
        {
            BuildEffect( e );
            ProcessRequestsDepthGroups( e.requests.ToArray() );
        }
    }
}
