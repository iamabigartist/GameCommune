using RTSFramework_v1.Base.Change;
namespace RTSFramework_v1.Base.Request
{
    public class ChangeRequest : Request
    {
        public ChangeRequest(string subpipeline_name, PrimitiveChange change, PrimitiveData target) : base( subpipeline_name )
        {
            this.target = target;
            this.change = change;
        }
        public PrimitiveData target { get; }
        public PrimitiveChange change { get; }
        public boolean_data enabled = new boolean_data( true );
        public override void Process() { target.ApplyChange( change ); }
    }

}
