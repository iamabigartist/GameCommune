using System.Collections.Generic;
namespace RTSFramework_v02
{
    public class IInPipelineStageLists<TElement> where TElement : IInPipelineStage
    {
        public IInPipelineStageLists()
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
