using System;
namespace RTSFramework_v1_0.Processor.Pipeline
{
    public interface IInPipelineStage
    {
        GamePipelineTable.Stage stage { get; }
    }
}
