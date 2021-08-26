namespace RTSFramework
{
    public abstract class AddRequest : Request
    {
        protected AddRequest(string subpipeline_name) : base( subpipeline_name ) { }
        public abstract IAddable target { get; }
        public abstract IAddable.IBeAddedable item { get; }
        public override void Process()
        {
            target.Add( item );
        }
    }
    public interface IAddable
    {
        public abstract class IBeAddedable { }
        void Add(IBeAddedable item);
    }
}
