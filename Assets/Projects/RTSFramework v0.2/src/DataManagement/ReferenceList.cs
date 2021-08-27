using System;
using System.Collections.Generic;
namespace RTSFramework_v02
{
    public class ReferenceList<TData> where TData : class, INullNoticeable
    {
        public ReferenceList()
        {
            remove_delegate_dict = new Dictionary<TData, EventHandler>();
            listened_ref_list = new List<TData>();
            temporary_ref_list = new List<TData>();
        }


    #region Listened

        Dictionary<TData, EventHandler> remove_delegate_dict;
        public List<TData> listened_ref_list;

        public void RemoveListened(TData data)
        {
            data.GoNull -= remove_delegate_dict[data];
            remove_delegate_dict.Remove( data );
            listened_ref_list.Remove( data );
        }

        public void AddListened(TData data)
        {
            var remove = new EventHandler( RemoveListenedHandler );
            data.GoNull += remove;
            remove_delegate_dict[data] = remove;
            listened_ref_list.Add( data );
        }

        void RemoveListenedHandler(object data, EventArgs args) { RemoveListened( data as TData ); }

    #endregion

    #region Temporary

        public List<TData> temporary_ref_list;
        public void ClearTemporary() { temporary_ref_list.Clear(); }

    #endregion

    }

    /// <summary>
    ///     <para> 一个数据，在其变为空之前可以通知引用列表将这个引用删除。</para>
    ///     <para>A Data that can trigger the <see cref="GoNull" /> event before it becomes null;</para>
    /// </summary>
    public interface INullNoticeable
    {
        event EventHandler GoNull;
    }
}
