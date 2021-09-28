using System;
using System.Collections.Generic;
using UnityEngine;
namespace Labs.TestFrames
{
    [List(new []{1,2,3})]
    public class Framer : MonoBehaviour
    {
        SortedSet<int> sorted_list;
        void Start()
        {
            sorted_list = new SortedSet<int>();
        }



    }

    public class ListAttribute : Attribute
    {
        public ListAttribute(int[] depth)
        {

        }
    }
}
