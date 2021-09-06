using System.Collections.Generic;
namespace RTSFramework_v0_2.src.Pipeline
{
    /// <summary>
    ///     A group of lists that store <see cref="TElement" />s of different pipeline stage.
    /// </summary>
    /// <remarks>
    ///     The pipeline stages number must be fixed by the <see cref="GamePipelineTable" /> before this
    ///     <see cref="PipelineElementLists{TElement}" /> construct,
    ///     and it can't be changed at runtime.
    /// </remarks>
    public class PipelineElementLists<TElement> where TElement : IInPipelineStage
    {
        public PipelineElementLists()
        {
            lists = new List<TElement>[GamePipelineTable.StageCount];
            for (int i = 0; i < lists.Length; i++) { lists[i] = new List<TElement>(); }
        }

        List<TElement>[] lists;
        public List<TElement> this[int stage] => lists[stage];
        // public List<TElement> this[TElement element]
        // {
        //     get
        //     {
        //         var list = this[element.GetStage()];
        //         return list.Contains( element ) ? list : null;
        //     }
        // }

        public void Add(TElement element)
        {
            this[element.pipeline_tag.value].Add( element );

        }
        public bool Remove(TElement element)
        {
            return this[element.pipeline_tag.value].Remove( element );
        }
    }
}
