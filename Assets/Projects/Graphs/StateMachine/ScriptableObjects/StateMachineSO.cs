using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Graphs.StateMachine.ScriptableObjects
{
    [CreateAssetMenu( fileName = "StateMachine", menuName = "State Machine", order = 51 )]
    public class StateMachineSO : ScriptableObject
    {
        [SerializeField]
        StateSO m_initialState;

        public StateSO InitialState => m_initialState;

#if UNITY_EDITOR
        static readonly string STATE_POSTFIX = "_States";
        static readonly string ACTION_POSTFIX = "_Actions";
        static readonly string DECISION_POSTFIX = "_Decisions";
        static readonly string DEFAULT_STATE_NAME = "New State";
        static readonly string NODE_MISSING_ERROR = "Reference not found. The element is removed. Do not delete or move files in the generated folder.";

        [SerializeField]
        List<StateNode> m_states = new List<StateNode>();

        [SerializeField]
        List<ActionNode> m_actions = new List<ActionNode>();

        [SerializeField]
        List<DecisionNode> m_decisions = new List<DecisionNode>();

        public IReadOnlyList<StateNode> States => m_states;

        public IReadOnlyList<ActionNode> Actions => m_actions;

        public IReadOnlyList<DecisionNode> Decisions => m_decisions;

        public void CleanFolder()
        {
            foreach (string guid in AssetDatabase.FindAssets( "t:StateSO", new string[] { GetFolderPath( STATE_POSTFIX ) } ))
            {
                string path = AssetDatabase.GUIDToAssetPath( guid );
                StateSO state = AssetDatabase.LoadAssetAtPath<StateSO>( path );
                if (m_states.Find( stateNode => stateNode.StateSO == state ).StateSO == null)
                {
                    AssetDatabase.DeleteAsset( path );
                }
            }

            m_states.RemoveAll( stateNode =>
            {
                if (stateNode.StateSO == null)
                {
                    Debug.LogError( NODE_MISSING_ERROR );
                    return true;
                }
                else
                {
                    return false;
                }
            } );

            foreach (string guid in AssetDatabase.FindAssets( "t:ActionSO", new string[] { GetFolderPath( ACTION_POSTFIX ) } ))
            {
                string path = AssetDatabase.GUIDToAssetPath( guid );
                ActionSO action = AssetDatabase.LoadAssetAtPath<ActionSO>( path );
                if (m_actions.Find( actionNode => actionNode.ActionSO == action ).ActionSO == null)
                {
                    AssetDatabase.DeleteAsset( path );
                }
            }

            m_actions.RemoveAll( actionNode =>
            {
                if (actionNode.ActionSO == null)
                {
                    Debug.LogError( NODE_MISSING_ERROR );
                    return true;
                }
                else
                {
                    return false;
                }
            } );

            foreach (string guid in AssetDatabase.FindAssets( "t:DecisionSO", new string[] { GetFolderPath( DECISION_POSTFIX ) } ))
            {
                string path = AssetDatabase.GUIDToAssetPath( guid );
                DecisionSO decision = AssetDatabase.LoadAssetAtPath<DecisionSO>( path );
                if (m_decisions.Find( decisionNode => decisionNode.DecisionSO == decision ).DecisionSO == null)
                {
                    AssetDatabase.DeleteAsset( path );
                }
            }

            m_decisions.RemoveAll( decisionNode =>
            {
                if (decisionNode.DecisionSO == null)
                {
                    Debug.LogError( NODE_MISSING_ERROR );
                    return true;
                }
                else
                {
                    return false;
                }
            } );
        }

        string GetFolderPath(string postFix)
        {
            string path = AssetDatabase.GetAssetPath( this );
            int split = path.LastIndexOf( '/' );
            string folder = path.Substring( 0, split );
            string nodeFolderName = path.Substring( split + 1 ).Split( '.' )[0] + postFix;
            string nodeFolderPath = folder + '/' + nodeFolderName;

            if (!AssetDatabase.IsValidFolder( nodeFolderPath ))
            {
                AssetDatabase.CreateFolder( folder, nodeFolderName );
            }

            return nodeFolderPath;
        }

        string GetInstancePath(Object instance, string postFix)
        {
            return GetFolderPath( postFix ) + "/" + instance.GetInstanceID() + ".asset";
        }

        public bool RenameState(StateSO state, string newName)
        {
            Undo.RecordObject( this, "Rename state" );
            for (int i = 0; i < m_states.Count; i++)
            {
                if (m_states[i].StateSO == state)
                {
                    m_states[i] = new StateNode( m_states[i].Position, state, newName );
                    EditorUtility.SetDirty( this );
                    return true;
                }
            }
            return false;
        }

        public StateNode CreateState(Vector2 position)
        {
            StateSO state = CreateInstance<StateSO>();
            AssetDatabase.CreateAsset( state, GetInstancePath( state, STATE_POSTFIX ) );
            StateNode ret = new StateNode( position, state, DEFAULT_STATE_NAME );

            Undo.RecordObject( this, "Create new state" );
            if (m_initialState == null)
            {
                m_initialState = state;
            }
            m_states.Add( ret );
            EditorUtility.SetDirty( this );

            return ret;
        }

        public bool MoveState(StateSO state, Vector2 newPosition)
        {
            Undo.RecordObject( this, "Move state" );
            for (int i = 0; i < m_states.Count; i++)
            {
                if (m_states[i].StateSO == state)
                {
                    m_states[i] = new StateNode( newPosition, state, m_states[i].Name );
                    EditorUtility.SetDirty( this );
                    return true;
                }
            }
            return false;
        }

        public bool RemoveState(StateSO state)
        {
            Undo.RecordObject( this, "Remove state" );
            for (int i = 0; i < m_states.Count; i++)
            {
                StateNode stateNode = m_states[i];
                if (stateNode.StateSO == state)
                {
                    if (state == m_initialState)
                    {
                        m_initialState = null;
                    }
                    m_states.RemoveAt( i );
                    EditorUtility.SetDirty( this );
                    return true;
                }
            }
            return false;
        }

        public ActionNode CreateAction(Type type, Vector2 position)
        {
            if (!typeof(ActionSO).IsAssignableFrom( type ))
            {
                Debug.LogError( $"Type {type.Name} is not derived from ActionSO!" );
                return default;
            }

            ActionSO action = CreateInstance( type ) as ActionSO;
            AssetDatabase.CreateAsset( action, GetInstancePath( action, ACTION_POSTFIX ) );
            ActionNode ret = new ActionNode( position, action );

            Undo.RecordObject( this, "Create new action" );
            m_actions.Add( ret );
            EditorUtility.SetDirty( this );

            return ret;
        }

        public bool MoveAction(ActionSO action, Vector2 newPosition)
        {
            Undo.RecordObject( this, "Move action" );
            for (int i = 0; i < m_actions.Count; i++)
            {
                if (m_actions[i].ActionSO == action)
                {
                    m_actions[i] = new ActionNode( newPosition, action );
                    EditorUtility.SetDirty( this );
                    return true;
                }
            }
            return false;
        }

        public bool RemoveAction(ActionSO action)
        {
            Undo.RecordObject( this, "Remove action" );
            for (int i = 0; i < m_actions.Count; i++)
            {
                ActionNode actionNode = m_actions[i];
                if (actionNode.ActionSO == action)
                {
                    m_actions.RemoveAt( i );
                    EditorUtility.SetDirty( this );
                    return true;
                }
            }
            return false;
        }

        public DecisionNode CreateDecision(Type type, Vector2 position)
        {
            if (!typeof(DecisionSO).IsAssignableFrom( type ))
            {
                Debug.LogError( $"Type {type.Name} is not derived from DecisionSO!" );
                return default;
            }

            DecisionSO decision = CreateInstance( type ) as DecisionSO;
            AssetDatabase.CreateAsset( decision, GetInstancePath( decision, DECISION_POSTFIX ) );
            DecisionNode ret = new DecisionNode( position, decision );

            Undo.RecordObject( this, "Create new decision" );
            m_decisions.Add( ret );
            EditorUtility.SetDirty( this );

            return ret;
        }

        public bool MoveDecision(DecisionSO decision, Vector2 newPosition)
        {
            Undo.RecordObject( this, "Move decision" );
            for (int i = 0; i < m_decisions.Count; i++)
            {
                if (m_decisions[i].DecisionSO == decision)
                {
                    m_decisions[i] = new DecisionNode( newPosition, decision );
                    EditorUtility.SetDirty( this );
                    return true;
                }
            }
            return false;
        }

        public bool RemoveDecision(DecisionSO decision)
        {
            Undo.RecordObject( this, "Remove decision" );
            for (int i = 0; i < m_decisions.Count; i++)
            {
                DecisionNode decisionNode = m_decisions[i];
                if (decisionNode.DecisionSO == decision)
                {
                    m_decisions.RemoveAt( i );
                    EditorUtility.SetDirty( this );
                    return true;
                }
            }
            return false;
        }

        public void SetInitialState(StateSO initialState)
        {
            if (initialState == m_initialState)
            {
                return;
            }

            if (m_states.Find( stateNode => stateNode.StateSO == initialState ).StateSO == null)
            {
                Debug.LogError( $"State {initialState} is not in the state list!" );
            }

            Undo.RecordObject( this, "Set initial state" );
            m_initialState = initialState;
            EditorUtility.SetDirty( this );
        }
#endif
    }

#if UNITY_EDITOR
    [Serializable]
    public struct StateNode
    {
        public Vector2 Position;
        public StateSO StateSO;
        public string Name;

        public StateNode(Vector2 position, StateSO stateSO, string name)
        {
            Position = position;
            StateSO = stateSO;
            Name = name;
        }
    }

    [Serializable]
    public struct ActionNode
    {
        public Vector2 Position;
        public ActionSO ActionSO;

        public ActionNode(Vector2 position, ActionSO actionSO)
        {
            Position = position;
            ActionSO = actionSO;
        }
    }

    [Serializable]
    public struct DecisionNode
    {
        public Vector2 Position;
        public DecisionSO DecisionSO;

        public DecisionNode(Vector2 position, DecisionSO decisionSO)
        {
            Position = position;
            DecisionSO = decisionSO;
        }
    }
#endif
}
