namespace RTSFramework_v1_0.DataBase.Requests
{
    public interface IDatabase : IRequestTo
    {
        void Create(IDataBaseElement element);
        void Delete(IDataBaseElement element);
    }
    public interface IDataBaseElement
    {
        /// <summary>
        ///     The owner that the element belongs to
        /// </summary>
        /// <remarks>
        ///     One element may have multiple owner referencing to it
        ///     but will have only one owner.
        /// </remarks>
        IDatabase owner { get; set; }
    }
}
