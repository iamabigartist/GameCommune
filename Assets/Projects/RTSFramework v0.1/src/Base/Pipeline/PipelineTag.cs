namespace RTSFramework_v01
{
    /// <summary>
    ///     A tag will record the pipeline name and its stage for a object that want to be tagged by pipeline name
    /// </summary>
    public readonly struct PipelineTag
    {
        public string pipeline_name { get; }
        public int value { get; }

        public PipelineTag(string pipeline_name)
        {
            this.pipeline_name = pipeline_name;
            value = GamePipelineTable.GetPipelineStage( this.pipeline_name );
        }
    }

}
