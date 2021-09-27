using System;
using UnityEngine;
namespace Labs.TestFrames
{
    public class AA
    {

    }
    public class BB:AA
    {

    }
    public class TestStructTag : MonoBehaviour
    {
        public BB bb;
        void Start()
        {
            bb = new BB();
        }
        void OnGUI()
        {
        }
    }
}
