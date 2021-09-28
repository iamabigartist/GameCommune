using UnityEngine;
namespace Labs.TestFrames
{
    public class TestChildComponent : TestComponent
    {
        public override void OnGUI()
        {
            base.OnGUI();
            GUILayout.Box( $"{GetType()}" );
        }
    }
}
