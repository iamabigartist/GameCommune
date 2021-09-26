using System;
using UnityEngine;
namespace Labs.TestFrames
{
    public class EventRelation
    {

    }
    public class TestStructTag : MonoBehaviour
    {
        public static EventRelation to_relation;
        public static EventRelation from_relation;
        public static EventRelation other_relation;
        static TestStructTag()
        {
            to_relation = new EventRelation();
            from_relation = new EventRelation();
            other_relation = new EventRelation();
        }

        void OnGUI()
        {
        }
    }
}
