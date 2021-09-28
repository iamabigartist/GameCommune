using System;
using UnityEngine;
namespace Labs.TestFrames
{
    public class TestComponent:MonoBehaviour
    {
        int a = 1;
        public virtual void OnGUI()
        {
            GUILayout.Box($"{GetType().BaseType}");
        }
    }
}
