namespace RTSFramework
{
    public abstract class AddRequest : Request
    {
        protected AddRequest(int pipeline_depth) : base( pipeline_depth ) { }
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
