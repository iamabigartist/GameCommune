using System;
using System.Diagnostics;
using System.Threading;
using UnityEditor;
using UnityEngine;
namespace MUtility.test.Editor
{
    public static class TestUtility
    {
        public class FunctionTester
        {
            public long finish_times { get; private set; }
            public double tick_per_run
            {
                get
                {
                    if (finish_times == 0)
                    {
                        return double.NaN;
                    }
                    return (double)((decimal)watch.ElapsedTicks / (decimal)finish_times);
                }
            }
            public double second_per_run => tick_per_run / 1e7;
            Stopwatch watch;

            public string function_name { get; private set; }
            Action tested_function;
            Thread t;

            public bool play { get; private set; }
            bool quit { get; set; }


            public FunctionTester(Action tested_function, string function_name)
            {
                this.function_name = function_name;
                this.tested_function = tested_function;
                finish_times = 0;
                watch = new Stopwatch();
                t = new Thread( Run );
                play = false;
                quit = false;
                t.Start();
            }

            public void Start()
            {
                play = true;
            }

            public void Stop()
            {
                play = false;
            }

            void Run()
            {
                while (!quit)
                {
                    if (play)
                    {
                        watch.Start();
                        tested_function.Invoke();
                        watch.Stop();
                        finish_times++;
                    }

                }
            }

            ~FunctionTester()
            {
                quit = true;
                t.Join();
            }
        }

        /// <returns>return true if there is no exception</returns>
        public static bool OnTesterGUI(FunctionTester tester, GUIStyle tester_style)
        {

            try
            {
                using (var v = new EditorGUILayout.VerticalScope( tester_style ))
                {
                    using (var h1 = new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.SelectableLabel( tester.function_name );
                        EditorGUILayout.SelectableLabel( $"Average Time: {tester.tick_per_run}" );
                    }

                    using (var h2 = new EditorGUILayout.HorizontalScope())
                    {
                        if (tester.play)
                        {
                            if (GUILayout.Button( "Stop" ))
                            {
                                tester.Stop();
                            }
                        }
                        else
                        {
                            if (GUILayout.Button( "Start" ))
                            {
                                tester.Start();
                            }
                        }

                        if (tester.play)
                        {
                            EditorGUILayout.LabelField( "Running" );
                        }

                        EditorGUILayout.SelectableLabel( $"Run Times: {tester.finish_times}" );

                    }
                }
            }

            catch (Exception)
            {
                return false;
            }

            return true;

        }
    }
}
