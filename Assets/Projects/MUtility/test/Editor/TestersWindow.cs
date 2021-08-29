using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace MUtility.test.Editor
{
    public class TestersWindow
    {

        GUIStyle tester_style;

        List<TestUtility.FunctionTester> testers;

        void InitStyles()
        {
            tester_style = GUI.skin.window;
            tester_style.margin = new RectOffset( 5, 5, 5, 5 );
        }

        /// <summary>
        ///     return true if there is no exceptions
        /// </summary>
        /// <returns></returns>
        bool OnGUI()
        {
            if (tester_style == null) { InitStyles(); }

            using (var h = new EditorGUILayout.HorizontalScope())
            {

                if (GUILayout.Button( "Run All" ))
                {
                    foreach (var tester in testers)
                    {
                        tester.Start();
                    }
                }
                if (GUILayout.Button( "Stop All" ))
                {
                    foreach (var tester in testers)
                    {
                        tester.Stop();
                    }
                }
            }

            var exceptions = testers.Select( (tester) => TestUtility.OnTesterGUI( tester, tester_style ) );

            return !exceptions.Contains( false );
        }

    }
}
