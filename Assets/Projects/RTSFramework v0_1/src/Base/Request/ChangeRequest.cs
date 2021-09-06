using RTSFramework_v0_1.src.Base.Change;
namespace RTSFramework_v0_1.src.Base.Request
{
    public class ChangeRequest : Request
    {
        public ChangeRequest(string pipeline_name, PrimitiveChange change, PrimitiveData target) : base( pipeline_name )
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
