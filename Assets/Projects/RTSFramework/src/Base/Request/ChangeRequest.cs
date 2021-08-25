namespace RTSFramework
{
    public abstract class ChangeRequest : Request
    {
        protected ChangeRequest(int pipeline_depth, PrimitiveChange change, PrimitiveData target) : base( pipeline_depth )
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
