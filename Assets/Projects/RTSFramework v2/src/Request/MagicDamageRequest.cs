using System;
using RTSFramework_v2.Data1;
namespace RTSFramework_v2.Request
{
    public class MagicDamageRequest : Request
    {

        PrimitiveChange<int> damage;
        IHP target;
        public MagicDamageRequest(string pipeline_name, int damage) : base( pipeline_name )
        {
            this.damage = new PrimitiveChange<int>( PrimitiveChange<int>.Type.Add, -damage );
        }
        public override void Process()
        {
            var temp = target.HP;
            temp.ApplyChange( damage );
        }
        public override bool AbleToReduce(Request another_request) { throw new NotImplementedException(); }
    }

    interface IHP
    {
        PrimitiveData<int> HP { get; set; }
    }
}
