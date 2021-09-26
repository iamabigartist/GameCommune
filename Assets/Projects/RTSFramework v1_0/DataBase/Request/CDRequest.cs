using RTSFramework_v1_0.Processor.Pipeline;
namespace RTSFramework_v1_0.DataBase.Request
{
    public abstract class CDRequest<T> : Request
    {
        protected CDRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            ICD_able<T> database,
            T element) : base( stage, from )
        {
            this.database = database;
            this.element = element;
        }

        public ICD_able<T> database;

        /// <summary>
        ///     A constructor that create a T element
        /// </summary>
        public T element;
    }

    /// <remarks>Create Request use a factory method to construct the element.</remarks>
    public abstract class CreateRequest<T> : CDRequest<T>
    {
        protected CreateRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            ICD_able<T> database,T element) :
            base( stage, from, database ,element) { }
        public override void Process() { database.Create( element ); }
    }

    public abstract class DeleteRequest<T> : CDRequest<T>
    {
        protected DeleteRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            ICD_able<T> database,
            T element) :
            base( stage, from, database, element ) { }
        public override void Process() { database.Delete( element ); }
    }
}
