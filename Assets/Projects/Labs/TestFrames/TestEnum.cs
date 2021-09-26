using RTSFramework_v1_0.Processor.Pipeline;
using UnityEngine;
namespace Labs.TestFrames
{
    public class TestEnum : MonoBehaviour
    {

        void OnGUI()
        {
            GUILayout.Box( $"{(int)GamePipelineTable.Stage.Process}" );
        }
    }
}
