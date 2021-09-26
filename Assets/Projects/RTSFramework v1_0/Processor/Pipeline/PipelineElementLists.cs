using System.Collections.Generic;
namespace RTSFramework_v1_0.Processor.Pipeline
{
    /// <summary>
    ///     A group of lists that store <see cref="TStageElement" />s of different pipeline stage.
    /// </summary>
    public class PipelineElementLists<TStageElement> where TStageElement : IInPipelineStage
    {
        public PipelineElementLists()
        {
            lists = new List<TStageElement>[GamePipelineTable.StageCount];
            for (int i = 0; i < lists.Length; i++) { lists[i] = new List<TStageElement>(); }
        }

        List<TStageElement>[] lists;

    #region Indexer

        public List<TStageElement> this[GamePipelineTable.Stage stage] => lists[(int)stage];
        public List<TStageElement> this[int stage] => lists[stage];
        public List<TStageElement> this[TStageElement element]
        {
            get
            {
                var list = this[(int)element.stage];
                return list.Contains( element ) ? list : null;
            }
        }

    #endregion

    #region ListBehaviour

        public void Add(TStageElement element)
        {
            this[(int)element.stage].Add( element );

        }
        public bool Remove(TStageElement element)
        {
            return this[(int)element.stage].Remove( element );
        }

    #endregion

    }
}
