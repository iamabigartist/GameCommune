using System;
using UnityEngine;
namespace Labs.TestFrames
{
    public class Framer : MonoBehaviour
    {

        void OnGUI()
        {
            GUILayout.Box($"cur_frame:{Time.frameCount}");
        }

    }
}
