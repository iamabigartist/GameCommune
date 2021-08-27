using System.Collections.Generic;
using System.Linq;
namespace RTSFramework_v01
{
    public static class GamePipelineTable
    {
        static Dictionary<string, int> Get_CurrentGame_PipelineDictionary()
        {
            return new Dictionary<string, int>()
            {
                { "PreProcess", 0 },
                { "Process", 50 },
                { "Processing", 50 },
                { "BeforeProcess", 40 },
                { "SufProcess", 100 }
            };
        }

        /// <summary>
        ///     The dictionary that contains all the subpipeline depths and their names
        /// </summary>
        static Dictionary<string, int> subpipeline_dict;

        /// <summary>
        ///     Sorted distinct depths
        /// </summary>
        static SortedSet<int> depths;

        static Dictionary<int, int> depth_to_stage;

        /// <summary>
        ///     Provide stage number according to a depth name.
        /// </summary>
        static Dictionary<string, int> name_to_stage;

        static GamePipelineTable()
        {
            //Get current game pipeline arrangement, can differ from different game
            subpipeline_dict = Get_CurrentGame_PipelineDictionary();

            //Construct Depths
            depths = new SortedSet<int>();
            foreach (KeyValuePair<string, int> keyValuePair in subpipeline_dict)
            {
                depths.Add( keyValuePair.Value );
            }

            //Construct depth to stage
            depth_to_stage = depths.
                Select( (depth, index) => (depth, index) ).
                ToDictionary( (pair) => pair.depth, (pair) => pair.index );

            //Construct name to stage
            name_to_stage = subpipeline_dict.ToDictionary( (pair) => pair.Key, (pair) => depth_to_stage[pair.Value] );
        }

        public static int GetSubPipelineStage(string subpipeline_name)
        {
            return name_to_stage[subpipeline_name];
        }

        /// <summary>
        /// The stage amount of the game
        /// </summary>
        public static int StageCount => depth_to_stage.Count;

    }
}
