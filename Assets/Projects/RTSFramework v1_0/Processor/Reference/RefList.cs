using System.Collections.Generic;
namespace RTSFramework_v1_0.Processor.Reference
{
    public abstract class RefList
    {
        public static List<RefList> ref_lists;
        static RefList()
        {
            ref_lists = new List<RefList>();
        }
        public static void RemoveAllNull_AllList()
        {
            foreach (RefList ref_list in ref_lists)
            {
                ref_list.RemoveAllNull();
            }
        }

        protected RefList()
        {
            ref_lists.Add( this );
        }

        protected abstract void RemoveAllNull();
    }

    public class RefList<TData> : RefList
        where TData : IReferable
    {
        public List<TData> refs;
        public RefList() : base()
        {
            refs = new List<TData>();
        }
        protected override void RemoveAllNull()
        {
            refs.RemoveAll( (@ref) => @ref.will_be_null );
        }
    }


}
