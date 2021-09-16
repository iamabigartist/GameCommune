namespace RTSFramework_v1_0.DataBase
{
    public abstract class CDRequest<T> : Request
    {
        protected CDRequest(string pipeline_name, ICD_able<T> database) : base( pipeline_name )
        {
            this.database = database;
        }

        public ICD_able<T> database;

        /// <summary>
        ///     A constructor that create a T element
        /// </summary>
        protected abstract T element { get; }
    }

    public abstract class CreateRequest<T> : CDRequest<T>
    {
        protected CreateRequest(string pipeline_name, ICD_able<T> database) :
            base( pipeline_name, database ) { }
        public override void Process() { database.Create( element ); }
    }

    public abstract class DeleteRequest<T> : CDRequest<T>
    {
        public T _element;
        protected override T element => _element;
        protected DeleteRequest(string pipeline_name, ICD_able<T> database, T _element) :
            base( pipeline_name, database )
        {
            this._element = _element;
        }
        public override void Process() { database.Delete( element ); }
    }
}
