using Graphs.StateMachine.ScriptableObjects;
using UnityEditor;
using UnityEngine;
namespace Graphs.StateMachine.Editor
{
    [CustomEditor(typeof(StateMachineSO))]
    public class StateMachineInspectorEditor : UnityEditor.Editor
    {
        bool m_isWarning = false;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            StateMachineSO stateMachine = target as StateMachineSO;

            GUILayout.Label($"State count: {stateMachine.States.Count}");
            GUILayout.Label($"Action count: {stateMachine.Actions.Count}");
            GUILayout.Label($"Decision count: {stateMachine.Decisions.Count}");

            if (stateMachine.InitialState == null)
            {
                EditorGUILayout.HelpBox("The state machine does not have a initial state, please set in the editor!", MessageType.Warning);
            }

            if (GUILayout.Button("Open in editor"))
            {
                StateMachineEditor.ShowStateMachine(stateMachine);
            }

            if (GUILayout.Button("Clean folder"))
            {
                m_isWarning = true;
            }

            if (m_isWarning)
            {
                EditorGUILayout.HelpBox("This will make removed nodes until now unrecoverable. \nContinue?", MessageType.Warning);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Yes"))
                {
                    stateMachine.CleanFolder();
                    m_isWarning = false;
                }
                else if (GUILayout.Button("No"))
                {
                    m_isWarning = false;
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
