using System.Collections.Generic;
using Graphs.StateMachine.ScriptableObjects;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
namespace Graphs.StateMachine.Editor.Nodes
{
    public class StateMachineStateView : Node
    {
        static readonly string EXECUTIONS_PROPERTY_NAME = "executions";
        static readonly string TRANSITIONS_PROPERTY_NAME = "transitions";
        static readonly string TARGET_STATE_PROPERTY_NAME = "targetState";
        static readonly string CONDITIONS_PROPERTY_NAME = "conditions";
        static readonly string ACTION_PROPERTY_NAME = "action";
        static readonly string EXECUTE_POINT_PROPERTY_NAME = "executePoint";
        static readonly Color DEFAULT_COLOR = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        static readonly Color INITIAL_COLOR = new Color(1.0f, 0.5f, 0.1f, 0.8f);
        static readonly Color EVEN_BACKGROUND_COLOR = new Color(0.6f, 0.6f, 0.6f, 0.8f);
        static readonly Color ODD_BACKGROUND_COLOR = new Color(0.5f, 0.5f, 0.5f, 0.8f);


        readonly StateSO m_state;
        readonly StateMachineView m_view;
        readonly SerializedProperty m_executionsProperty;
        readonly SerializedProperty m_transitionsProperty;
        readonly SerializedObject m_target;
        readonly List<ExecutionPort> m_executionPorts = new List<ExecutionPort>();
        readonly List<TransitionPort> m_transitionPorts = new List<TransitionPort>();

        bool m_isInitialState;

        public bool IsInitialState
        {
            get => m_isInitialState;
            set
            {
                m_isInitialState = value;
                titleContainer.style.backgroundColor = value ? INITIAL_COLOR : DEFAULT_COLOR;
            }
        }

        public StateSO StateSO => m_state;

        public StateMachineStateView(StateNode stateNode, StateMachineView view, bool isInitialState)
        {
            m_state = stateNode.StateSO;
            m_view = view;
            m_target = new SerializedObject(m_state);
            m_executionsProperty = m_target.FindProperty(EXECUTIONS_PROPERTY_NAME);
            m_transitionsProperty = m_target.FindProperty(TRANSITIONS_PROPERTY_NAME);

            //Clear title
            List<VisualElement> children = new List<VisualElement>(titleContainer.Children());
            children.ForEach(child => child.RemoveFromHierarchy());
            titleContainer.style.alignItems = Align.Center;
            //Setup name text
            TextField nameEditor = new TextField();
            nameEditor.SetValueWithoutNotify(stateNode.Name);
            nameEditor.RegisterValueChangedCallback(evt =>
            {
                m_view.Target.RenameState(m_state, evt.newValue);
            });
            nameEditor.style.width = 100f;
            nameEditor.style.height = 20f;
            titleContainer.Add(nameEditor);

            //Setup initial button
            Button initialButton = new Button(() =>
            {
                IsInitialState = true;
                m_view.SetInitialState(this);
            });
            initialButton.text = "Set To Initial State";
            IsInitialState = isInitialState;
            titleContainer.Add(initialButton);

            //Setup port views
            CreateExecutionPorts();
            CreateTransitionPorts();

            SetPosition(new Rect(stateNode.Position, GetPosition().size));
        }

        public void ConnectPorts()
        {
            ConnectActionPorts();
            ConnectTransitionPorts();
        }

        public override bool IsResizable()
        {
            return false;
        }

        void CreateExecutionPorts()
        {
            m_executionPorts.ForEach(port => port.Disconnect());
            m_executionPorts.Clear();
            List<VisualElement> children = new List<VisualElement>(inputContainer.Children());
            foreach (VisualElement child in children)
            {
                child.RemoveFromHierarchy();
            }

            Label label = new Label("Actions");
            label.style.alignSelf = Align.Center;
            label.style.paddingBottom = 4f;
            inputContainer.Add(label);

            for (int i = 0; i < m_executionsProperty.arraySize; i++)
            {
                VisualElement container = new VisualElement();

                ExecutionPort executionPort = new ExecutionPort(m_view, this, m_executionsProperty.GetArrayElementAtIndex(i));
                executionPort.style.flexGrow = 1f;
                m_executionPorts.Add(executionPort);

                int index = i;
                Button removeButton = new Button(() => RemoveExecution(index));
                removeButton.text = "-";

                container.style.flexDirection = FlexDirection.Row;
                container.style.backgroundColor = i % 2 == 0 ? EVEN_BACKGROUND_COLOR : ODD_BACKGROUND_COLOR;
                container.Add(executionPort);
                container.Add(removeButton);
                inputContainer.Add(container);
            }

            Button addButton = new Button(AddExecution);
            addButton.text = "Add Execution";
            inputContainer.Add(addButton);
            inputContainer.style.width = 160f;
            RefreshPorts();
        }

        void ConnectActionPorts()
        {
            m_executionPorts.ForEach(port => port.ConnectPort());
        }

        void CreateTransitionPorts()
        {
            m_transitionPorts.ForEach(port => port.Disconnect());
            m_transitionPorts.Clear();
            List<VisualElement> children = new List<VisualElement>(outputContainer.Children());
            foreach (VisualElement child in children)
            {
                child.RemoveFromHierarchy();
            }

            Label label = new Label("Transitions");
            label.style.alignSelf = Align.Center;
            label.style.paddingBottom = 4f;
            outputContainer.Add(label);

            for (int i = 0; i < m_transitionsProperty.arraySize; i++)
            {
                VisualElement container = new VisualElement();

                TransitionPort transitionPort = new TransitionPort(m_view, this, m_transitionsProperty.GetArrayElementAtIndex(i));
                transitionPort.style.flexGrow = 1f;
                m_transitionPorts.Add(transitionPort);

                int index = i;
                Button removeButton = new Button(() => RemoveTransition(index));
                removeButton.text = "-";

                container.style.flexDirection = FlexDirection.RowReverse;
                container.style.backgroundColor = i % 2 == 0 ? EVEN_BACKGROUND_COLOR : ODD_BACKGROUND_COLOR;
                container.Add(transitionPort);
                container.Add(removeButton);
                outputContainer.Add(container);
            }

            Button addButton = new Button(AddTransition);
            addButton.text = "Add Transition";
            outputContainer.Add(addButton);
            outputContainer.style.width = 160f;
            RefreshPorts();
        }

        void ConnectTransitionPorts()
        {
            m_transitionPorts.ForEach(port => port.ConnectPorts());
        }

        static void DisconnectPort(Port port)
        {
            List<Edge> connections = new List<Edge>(port.connections);
            foreach (Edge edge in connections)
            {
                switch (port.direction)
                {
                    case Direction.Input:
                        edge.output.Disconnect(edge);
                        break;
                    case Direction.Output:
                        edge.input.Disconnect(edge);
                        break;
                    default:
                        break;
                }
                edge.RemoveFromHierarchy();
            }
        }

        void AddExecution()
        {
            int index = m_executionsProperty.arraySize;
            m_executionsProperty.InsertArrayElementAtIndex(index);
            SerializedProperty executionProperty = m_executionsProperty.GetArrayElementAtIndex(index);
            executionProperty.FindPropertyRelative(ACTION_PROPERTY_NAME).objectReferenceValue = null;
            executionProperty.FindPropertyRelative(EXECUTE_POINT_PROPERTY_NAME).intValue = 0;
            m_executionsProperty.serializedObject.ApplyModifiedProperties();
            CreateExecutionPorts();
            ConnectActionPorts();
        }

        void RemoveExecution(int index)
        {
            m_executionsProperty.DeleteArrayElementAtIndex(index);
            m_executionsProperty.serializedObject.ApplyModifiedProperties();
            CreateExecutionPorts();
            ConnectActionPorts();
        }

        void AddTransition()
        {
            int index = m_transitionsProperty.arraySize;
            m_transitionsProperty.InsertArrayElementAtIndex(index);
            SerializedProperty transitionProperty = m_transitionsProperty.GetArrayElementAtIndex(index);
            transitionProperty.FindPropertyRelative(TARGET_STATE_PROPERTY_NAME).objectReferenceValue = null;
            transitionProperty.FindPropertyRelative(CONDITIONS_PROPERTY_NAME).ClearArray();
            m_transitionsProperty.serializedObject.ApplyModifiedProperties();
            CreateTransitionPorts();
            ConnectTransitionPorts();
        }

        void RemoveTransition(int index)
        {
            m_transitionsProperty.DeleteArrayElementAtIndex(index);
            m_transitionsProperty.serializedObject.ApplyModifiedProperties();
            CreateTransitionPorts();
            ConnectTransitionPorts();
        }

        class ExecutionPort : VisualElement
        {
            readonly StateMachineView m_view;
            readonly Port m_port;

            public ExecutionPort(StateMachineView view, StateMachineStateView stateView, SerializedProperty executionProperty) : base()
            {
                m_view = view;

                m_port = stateView.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
                m_port.portName = "";
                m_port.userData = executionProperty.FindPropertyRelative(ACTION_PROPERTY_NAME);
                m_port.style.flexShrink = 0f;

                SerializedProperty executePointProperty = executionProperty.FindPropertyRelative(EXECUTE_POINT_PROPERTY_NAME);
                IMGUIContainer executePoint = new IMGUIContainer(() =>
                {
                    EditorGUILayout.PropertyField(executePointProperty, GUIContent.none);
                    executePointProperty.serializedObject.ApplyModifiedProperties();
                });
                executePoint.style.alignSelf = Align.Center;

                style.flexDirection = FlexDirection.Row;
                Add(m_port);
                Add(executePoint);
            }

            public void Disconnect()
            {
                if (m_port != null)
                {
                    DisconnectPort(m_port);
                }
            }

            public void ConnectPort()
            {
                m_view.TryConnectByData(m_port, (m_port.userData as SerializedProperty).objectReferenceValue as ActionSO);
            }
        }

        class TransitionPort : VisualElement
        {
            static readonly string DECISION_PROPERTY_NAME = "decision";
            static readonly string RESULT_PROPERTY_NAME = "expectedResult";
            static readonly Color CONDITION_EVEN_BACKGROUND_COLOR = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            static readonly Color CONDITION_ODD_BACKGROUND_COLOR = new Color(0.3f, 0.3f, 0.3f, 0.8f);

            readonly StateMachineView m_view;
            readonly StateMachineStateView m_stateView;
            readonly SerializedProperty m_targetStateProperty;
            readonly SerializedProperty m_conditionsProperty;
            readonly List<Port> m_decisionPorts = new List<Port>();

            public TransitionPort(StateMachineView view, StateMachineStateView stateView, SerializedProperty transitionProperty) : base()
            {
                m_view = view;
                m_stateView = stateView;
                m_targetStateProperty = transitionProperty.FindPropertyRelative(TARGET_STATE_PROPERTY_NAME);
                m_conditionsProperty = transitionProperty.FindPropertyRelative(CONDITIONS_PROPERTY_NAME);

                CreatePorts();
            }

            public void Disconnect()
            {
                m_decisionPorts.ForEach(DisconnectPort);
            }

            void CreatePorts()
            {
                m_decisionPorts.ForEach(DisconnectPort);
                m_decisionPorts.Clear();
                List<VisualElement> children = new List<VisualElement>(Children());
                foreach (VisualElement child in children)
                {
                    child.RemoveFromHierarchy();
                }

                IMGUIContainer targetState = new IMGUIContainer(() =>
                {
                    int currentSelection = -1;
                    List<string> options = new List<string>();
                    List<StateSO> targetStates = new List<StateSO>();
                    options.Add("(None)");
                    targetStates.Add(null);
                    foreach (StateNode stateNode in m_view.Target.States)
                    {
                        if (stateNode.StateSO == m_targetStateProperty.objectReferenceValue)
                        {
                            currentSelection = targetStates.Count;
                        }
                        options.Add(stateNode.Name);
                        targetStates.Add(stateNode.StateSO);
                    }

                    if (currentSelection == -1)
                    {
                        m_targetStateProperty.objectReferenceValue = null;
                        m_targetStateProperty.serializedObject.ApplyModifiedProperties();
                        currentSelection = 0;
                    }

                    int newSelection = EditorGUILayout.Popup(currentSelection, options.ToArray());

                    if (newSelection != currentSelection)
                    {
                        m_targetStateProperty.objectReferenceValue = targetStates[newSelection];
                        m_targetStateProperty.serializedObject.ApplyModifiedProperties();
                    }
                });
                targetState.style.marginTop = targetState.style.marginBottom = targetState.style.marginLeft = targetState.style.marginRight = 4f;
                Add(targetState);

                for (int i = 0; i < m_conditionsProperty.arraySize; i++)
                {
                    SerializedProperty conditionProperty = m_conditionsProperty.GetArrayElementAtIndex(i);
                    VisualElement container = new VisualElement();

                    int index = i;
                    Button removeButton = new Button(() => RemoveCondition(index));
                    removeButton.text = "-";

                    SerializedProperty resultProperty = conditionProperty.FindPropertyRelative(RESULT_PROPERTY_NAME);
                    IMGUIContainer result = new IMGUIContainer(() =>
                    {
                        EditorGUILayout.PropertyField(resultProperty, GUIContent.none);
                        resultProperty.serializedObject.ApplyModifiedProperties();
                    });
                    result.style.marginTop = 3f;

                    Port port = m_stateView.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
                    port.portName = "";
                    port.userData = conditionProperty.FindPropertyRelative(DECISION_PROPERTY_NAME);
                    port.style.flexShrink = 0f;

                    container.style.flexDirection = FlexDirection.RowReverse;
                    container.style.backgroundColor = i % 2 == 0 ? CONDITION_EVEN_BACKGROUND_COLOR : CONDITION_ODD_BACKGROUND_COLOR;
                    container.Add(port);
                    container.Add(result);
                    container.Add(removeButton);
                    m_decisionPorts.Add(port);

                    Add(container);
                }

                Button addButton = new Button(AddCondition);
                addButton.text = "Add Condition";
                Add(addButton);
            }

            public void ConnectPorts()
            {
                foreach (Port port in m_decisionPorts)
                {
                    m_view.TryConnectByData(port, (port.userData as SerializedProperty).objectReferenceValue as DecisionSO);
                }
            }

            void RemoveCondition(int index)
            {
                m_conditionsProperty.DeleteArrayElementAtIndex(index);
                m_targetStateProperty.serializedObject.ApplyModifiedProperties();
                CreatePorts();
                ConnectPorts();
            }

            void AddCondition()
            {
                int index = m_conditionsProperty.arraySize;
                m_conditionsProperty.InsertArrayElementAtIndex(index);
                SerializedProperty conditionProperty = m_conditionsProperty.GetArrayElementAtIndex(index);
                conditionProperty.FindPropertyRelative(RESULT_PROPERTY_NAME).intValue = 0;
                conditionProperty.FindPropertyRelative(DECISION_PROPERTY_NAME).objectReferenceValue = null;
                m_conditionsProperty.serializedObject.ApplyModifiedProperties();
                CreatePorts();
                ConnectPorts();
            }
        }
    }
}
