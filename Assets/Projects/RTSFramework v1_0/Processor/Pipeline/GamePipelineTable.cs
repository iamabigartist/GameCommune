using System;
namespace RTSFramework_v1_0.Processor.Pipeline
{
    public static class GamePipelineTable
    {
       public enum Stage
        {
            BeforeProcess,
            PreProcess,
            Process,
            Processing,
            PostProcess
        }

        /// <summary>
        ///     The stage amount of the game
        /// </summary>
        public static int StageCount { get; }
        static GamePipelineTable()
        {
            StageCount = Enum.GetNames( typeof(Stage) ).Length;
        }
    }
}
