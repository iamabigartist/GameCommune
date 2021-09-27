namespace RTSFramework_v1_0.Processor.Reference
{

    /// <summary>
    ///     A referable object
    /// </summary>
    public interface IReferable
    {
        bool will_be_null { get; }
    }

    /// <summary>
    ///     The reference of a object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RefOf<T> : IReferable
        where T : class
    {
        /// <summary>
        ///     A ref can have a source to determine whether it's null
        /// </summary>
        IReferable source;
        /// <summary>
        ///     Whether the ref is temporary and will be deleted right at next event
        /// </summary>
        bool temporary;
        public bool will_be_null => (source?.will_be_null ?? false) || temporary;
        T data;

        public RefOf(IReferable source, bool temporary, T data)
        {
            this.source = source;
            this.temporary = temporary;
            this.data = data;
        }

    }
}
