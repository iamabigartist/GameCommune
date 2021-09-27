using RTSFramework_v1_0.Processor.Pipeline;
namespace RTSFramework_v1_0.DataBase.Requests
{
    public abstract class CDRequest<T> : Request
        where T : IDataBaseElement
    {
        protected CDRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            bool temporary,
            IDatabase target_database,
            T element) : base( stage, from, temporary )
        {
            this.element = element;
            this.target_database = target_database;
        }

        public IDatabase target_database;
        public override IRequestTo to => target_database;

        /// <summary>
        ///     A constructed but not yet inited element
        /// </summary>
        public T element;
    }

    public abstract class CreateRequest<T> : CDRequest<T>
        where T : IDataBaseElement
    {
        protected CreateRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            bool temporary,
            IDatabase target_database,
            T element) :
            base( stage, from, temporary, target_database, element ) { }
        public override void Process()
        {
            target_database.Create( element );
        }
    }

    public abstract class DeleteRequest<T> : CDRequest<T>
        where T : IDataBaseElement
    {
        protected DeleteRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            bool temporary,
            IDatabase target_database,
            T element) :
            base( stage, from, temporary, target_database, element ) { }
        public override void Process() { target_database.Delete( element ); }
    }
}
