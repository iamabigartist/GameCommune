namespace RTSFramework_v1_0.Pipeline
{
    /// <summary>
    ///     A tag will record the pipeline name and its stage for a object that want to be tagged by pipeline name
    /// </summary>
    public readonly struct PipelineTag
    {
        public int stage { get; }

        public PipelineTag(string pipeline_name)
        {
            stage = GamePipelineTable.GetPipelineStage( pipeline_name );
        }
    }

}
