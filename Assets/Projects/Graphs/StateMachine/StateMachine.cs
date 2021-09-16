using System.Collections.Generic;
using Graphs.StateMachine.ScriptableObjects;
using UnityEngine;
namespace Graphs.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private StateMachineSO m_stateMachine;

        private Dictionary<System.Type, Component> m_cachedComponents = new Dictionary<System.Type, Component>();
        private State m_currentState = null;

        internal Dictionary<StateSO, State> m_createdStates = new Dictionary<StateSO, State>();

        private void Awake()
        {
            if (m_stateMachine.InitialState == null)
            {
                Debug.LogError($"Selected state machine: {m_stateMachine} does not have a initial state!");
            }
            Initialize();
            if (!m_createdStates.ContainsKey(m_stateMachine.InitialState))
            {
                Debug.LogError("Default state not found!");
            }
            m_currentState = m_createdStates[m_stateMachine.InitialState];
            m_currentState.OnStateEnter();
        }

        private void Update()
        {
            m_currentState.OnUpdate();
            if (m_currentState.TryGetTransition(out StateSO stateSO))
            {
                TransitionTo(stateSO);
            }
        }

        private void Initialize()
        {
            Queue<StateSO> Q = new Queue<StateSO>();
            Q.Enqueue(m_stateMachine.InitialState);
            while(Q.Count > 0)
            {
                StateSO stateSO = Q.Dequeue();
                if (m_createdStates.ContainsKey(stateSO))
                {
                    continue;
                }
                stateSO.InitState(this);
                for (int i = 0; i < stateSO.transitions.Length; i++)
                {
                    Q.Enqueue(stateSO.transitions[i].targetState);
                }
            }
        }

        public new bool TryGetComponent<T>(out T component) where T : Component
        {
            Component value;
            if (!m_cachedComponents.TryGetValue(typeof(T), out value))
            {
                if (base.TryGetComponent<T>(out component))
                {
                    m_cachedComponents[typeof(T)] = component;
                    return true;
                }
                return false;
            }
            component = (T)value;
            return true;
        }

        public new T GetComponent<T>() where T : Component
        {
            return TryGetComponent<T>(out T component) ? component : throw new System.InvalidOperationException($"{typeof(T).Name} not found in {name}.");
        }

        public void TransitionTo(StateSO targetState)
        {
            if (!m_createdStates.ContainsKey(targetState))
            {
                Debug.LogError("State " + targetState.name + " not created!");
            }
            m_currentState.OnStateExit();
            m_currentState = m_createdStates[targetState];
            m_currentState.OnStateEnter();
        }
    }
}
