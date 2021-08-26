namespace RTSFramework
{
    /// <summary>
    ///     A tag will record the subpipeline name and its stage for a object that want to be tagged by subpipeline name
    /// </summary>
    public readonly struct SubPipelineTag
    {
        public string subpipeline_name { get; }
        public int value { get; }

        public SubPipelineTag(string subpipeline_name)
        {
            this.subpipeline_name = subpipeline_name;
            value = GamePipelineTable.GetSubPipelineStage( this.subpipeline_name );
        }
    }

}
