using System;
using System.Collections.Generic;
using Graphs.StateMachine.Editor.Nodes;
using Graphs.StateMachine.ScriptableObjects;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
namespace Graphs.StateMachine.Editor
{
    public class StateMachineView : GraphView
    {
        const string STYLE_SHEET_NAME = "StateMachineView";

        StateMachineEditor m_window;
        StateMachineSO m_target;
        readonly Dictionary<ActionSO, StateMachineActionView> m_actionToView = new Dictionary<ActionSO, StateMachineActionView>();
        readonly Dictionary<DecisionSO, StateMachineDecisionView> m_decisionToView = new Dictionary<DecisionSO, StateMachineDecisionView>();
        readonly List<StateMachineStateView> m_stateViews = new List<StateMachineStateView>();

        public StateMachineSO Target => m_target;

        public StateMachineView(StateMachineEditor window)
        {
            m_window = window;

            //Generate background grid
            StyleSheet styleSheet = Resources.Load<StyleSheet>(STYLE_SHEET_NAME);
            styleSheets.Add(styleSheet);
            GridBackground grid = new GridBackground();
            grid.AddToClassList("GridBackGround");
            Insert(0, grid);
            grid.StretchToParentSize();

            //Generate minimap
            MiniMap miniMap = new MiniMap
            {
                anchored = true
            };
            miniMap.SetPosition(new Rect(5f, 25f, 200f, 200f));
            Add(miniMap);

            //Add manipulators
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            //TODO Setup search window
            StateMachineSearchProvider searchProvider = ScriptableObject.CreateInstance<StateMachineSearchProvider>();
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchProvider);
            searchProvider.OnCreateNode += CreateRequest;

            //Handle graph change
            graphViewChanged += OnGraphViewChanged;
        }

        GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            //Handle movement
            if (change.elementsToRemove != null)
            {
                List<GraphElement> notRemoving = new List<GraphElement>();
                foreach (GraphElement elementToRemove in change.elementsToRemove)
                {
                    switch (elementToRemove)
                    {
                        case StateMachineStateView stateView:
                            if (!m_target.RemoveState(stateView.StateSO))
                            {
                                notRemoving.Add(elementToRemove);
                            }
                            break;

                        case StateMachineActionView actionView:
                            if (!m_target.RemoveAction(actionView.ActionSO))
                            {
                                notRemoving.Add(elementToRemove);
                            }
                            break;

                        case StateMachineDecisionView decisionView:
                            if (!m_target.RemoveDecision(decisionView.DecisionSO))
                            {
                                notRemoving.Add(elementToRemove);
                            }
                            break;

                        case Edge edge:
                            if (edge.input.userData != null)
                            {
                                //Action case
                                SerializedProperty property = edge.input.userData as SerializedProperty;
                                property.objectReferenceValue = null;
                                property.serializedObject.ApplyModifiedProperties();
                            }
                            else if (edge.output.userData != null)
                            {
                                //Decision case
                                SerializedProperty property = edge.output.userData as SerializedProperty;
                                property.objectReferenceValue = null;
                                property.serializedObject.ApplyModifiedProperties();
                            }
                            else
                            {
                                Debug.LogError("Unknown edge removed!");
                            }
                            break;

                        default:
                            Debug.LogWarning($"Removing unknown graph element: {elementToRemove.GetType().Name}");
                            break;
                    }
                }

                foreach (GraphElement element in notRemoving)
                {
                    change.elementsToRemove.Remove(element);
                }
            }

            //Handle create edge
            if (change.edgesToCreate != null)
            {
                foreach (Edge edgeToCreate in change.edgesToCreate)
                {
                    if (edgeToCreate.input.userData != null)
                    {
                        //Action case
                        SerializedProperty property = edgeToCreate.input.userData as SerializedProperty;
                        property.objectReferenceValue = (edgeToCreate.output.node as StateMachineActionView).ActionSO;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    else if (edgeToCreate.output.userData != null)
                    {
                        //Decision case
                        SerializedProperty property = edgeToCreate.output.userData as SerializedProperty;
                        property.objectReferenceValue = (edgeToCreate.input.node as StateMachineDecisionView).DecisionSO;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    else
                    {
                        Debug.LogError("Unknown edge added!");
                    }
                }
            }

            //Handle moved elements
            if (change.movedElements != null)
            {
                foreach (GraphElement movedElement in change.movedElements)
                {
                    switch (movedElement)
                    {
                        case StateMachineStateView stateView:
                            m_target.MoveState(stateView.StateSO, stateView.GetPosition().position);
                            break;

                        case StateMachineActionView actionView:
                            m_target.MoveAction(actionView.ActionSO, actionView.GetPosition().position);
                            break;

                        case StateMachineDecisionView decisionView:
                            m_target.MoveDecision(decisionView.DecisionSO, decisionView.GetPosition().position);
                            break;

                        default:
                            break;
                    }
                }
            }

            return change;
        }

        public void SetStateMachine(StateMachineSO stateMachine)
        {
            m_target = stateMachine;

            graphElements.ForEach(element => element.RemoveFromHierarchy());
            m_actionToView.Clear();
            m_decisionToView.Clear();
            m_stateViews.Clear();

            if (m_target == null)
            {
                return;
            }

            foreach (ActionNode actionNode in m_target.Actions)
            {
                StateMachineActionView actionView = new StateMachineActionView(actionNode);
                m_actionToView.Add(actionNode.ActionSO, actionView);
                AddElement(actionView);
            }

            foreach (DecisionNode decisionNode in m_target.Decisions)
            {
                StateMachineDecisionView decisionView = new StateMachineDecisionView(decisionNode);
                m_decisionToView.Add(decisionNode.DecisionSO, decisionView);
                AddElement(decisionView);
            }

            StateSO initialState = m_target.InitialState;
            foreach (StateNode stateNode in m_target.States)
            {
                StateMachineStateView stateView = new StateMachineStateView(stateNode, this, stateNode.StateSO == initialState);
                m_stateViews.Add(stateView);
                AddElement(stateView);

                stateView.ConnectPorts();
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> ret = new List<Port>();

            if (startPort.node is StateMachineStateView)
            {
                ports.ForEach(port =>
                {
                    if (!(port.node is StateMachineStateView) && port.direction != startPort.direction)
                    {
                        ret.Add(port);
                    }
                });
            }
            else
            {
                ports.ForEach(port =>
                {
                    if ((port.node is StateMachineStateView) && port.direction != startPort.direction)
                    {
                        ret.Add(port);
                    }
                });
            }

            return ret;
        }

        public void TryConnectByData(Port from, ActionSO action)
        {
            if (action == null)
            {
                return;
            }

            if (m_actionToView.TryGetValue(action, out StateMachineActionView actionView))
            {
                AddElement(from.ConnectTo(actionView.Port));
            }
        }

        public void TryConnectByData(Port from, DecisionSO decision)
        {
            if (decision == null)
            {
                return;
            }

            if (m_decisionToView.TryGetValue(decision, out StateMachineDecisionView decisionView))
            {
                AddElement(from.ConnectTo(decisionView.Port));
            }
        }

        public void SetInitialState(StateMachineStateView initialStateView)
        {
            foreach (StateMachineStateView stateView in m_stateViews)
            {
                if (stateView != initialStateView)
                {
                    stateView.IsInitialState = false;
                }
            }
            m_target.SetInitialState(initialStateView.StateSO);
        }

        void CreateRequest(Type type, Vector2 screenMousePosition)
        {
            if (m_target == null)
            {
                return;
            }

            Vector2 pos = contentViewContainer.WorldToLocal(screenMousePosition - m_window.position.position);
            if (type == typeof(StateSO))
            {
                StateNode newState = m_target.CreateState(pos);
                StateMachineStateView stateView = new StateMachineStateView(newState, this, newState.StateSO == m_target.InitialState);
                m_stateViews.Add(stateView);
                AddElement(stateView);
            }
            else if (typeof(ActionSO).IsAssignableFrom(type))
            {
                ActionNode newAction = m_target.CreateAction(type, pos);
                StateMachineActionView actionView = new StateMachineActionView(newAction);
                m_actionToView.Add(newAction.ActionSO, actionView);
                AddElement(actionView);
            }
            else if (typeof(DecisionSO).IsAssignableFrom(type))
            {
                DecisionNode newDecision = m_target.CreateDecision(type, pos);
                StateMachineDecisionView decisionView = new StateMachineDecisionView(newDecision);
                m_decisionToView.Add(newDecision.DecisionSO, decisionView);
                AddElement(decisionView);
            }
            else
            {
                Debug.LogError($"Unknown type: {type.Name}");
            }
        }
    }
}
