namespace RTSFramework_v1_0.DataBase
{
    public abstract class CDRequest<T>
    {
        protected CDRequest(ICD_able<T> database, T element)
        {
            this.database = database;
            this.element = element;
        }

        public ICD_able<T> database;
        public T element;
        public abstract void Apply();
    }

    public class CreateRequest<T> : CDRequest<T>
    {
        public CreateRequest(ICD_able<T> database, T element) : base( database, element ) { }
        public override void Apply() { database.Create( element ); }
    }

    public class DeleteRequest<T> : CDRequest<T>
    {
        public DeleteRequest(ICD_able<T> database, T element) : base( database, element ) { }
        public override void Apply() { database.Delete( element ); }
    }
}
