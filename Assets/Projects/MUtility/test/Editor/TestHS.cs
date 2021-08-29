using UnityEditor;
using UnityEngine;
namespace MUtility.test.Editor
{
    public class TestHeapVsStack_float : EditorWindow
    {
        [MenuItem( "MUtility/test/HeapVsStack_float" )]
        static void ShowWindow()
        {
            var window = GetWindow<TestHeapVsStack_float>();
            window.titleContent = new GUIContent( "TestersWindow" );
            window.Show();
        }

        class float_data
        {
            public float value;
        }

        GUIStyle tester_style;

        void Awake()
        {
            stack_data = 1;
            heap_data = new float_data() { value = 1 };
            tester_heap_float = new TestUtility.FunctionTester( () =>
            {
                // heap_data.value += 1;
                // heap_data.value -= 1;
                heap_data.value *= 3.1415926f;
                heap_data.value /= 3.141526f;
            }, "float in heap" );
            tester_stack_float = new TestUtility.FunctionTester( () =>
            {
                // stack_data += 1;
                // stack_data -= 1;
                stack_data *= 3.1415926f;
                stack_data /= 3.141526f;
            }, "float in stack" );
        }

        TestUtility.FunctionTester tester_heap_float;
        TestUtility.FunctionTester tester_stack_float;

        float stack_data;
        float_data heap_data;



        void OnInspectorUpdate()
        {
            Repaint();
        }


        void InitStyles()
        {
            tester_style = GUI.skin.window;
            tester_style.margin = new RectOffset( 5, 5, 5, 5 );
        }

        void OnGUI()
        {
            if (tester_style == null) { InitStyles(); }

            using (var h = new EditorGUILayout.HorizontalScope())
            {

                if (GUILayout.Button( "Run All" ))
                {
                    tester_heap_float.Start();
                    tester_stack_float.Start();
                }
                if (GUILayout.Button( "Stop All" ))
                {
                    tester_heap_float.Stop();
                    tester_stack_float.Stop();
                }
            }

            var e1 = TestUtility.OnTesterGUI( tester_stack_float, tester_style );
            var e2 = TestUtility.OnTesterGUI( tester_heap_float, tester_style );
            if (!(e1 && e2)) { Close(); }
        }

        // void OnDestroy()
        // {
        //     tester_stack_float.
        // }
    }
}
