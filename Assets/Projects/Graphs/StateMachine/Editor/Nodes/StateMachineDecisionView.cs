using System.Reflection;
using Graphs.StateMachine.ScriptableObjects;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
namespace Graphs.StateMachine.Editor.Nodes
{
    public class StateMachineDecisionView : Node
    {
        static readonly Color TITLE_BACKGROUND_COLOR = new Color(0.1f, 0.1f, 0.5f, 0.8f);

        readonly DecisionSO m_decision;
        readonly Port m_port;

        public DecisionSO DecisionSO => m_decision;
        public Port Port => m_port;

        public StateMachineDecisionView(DecisionNode decisionNode)
        {
            m_decision = decisionNode.DecisionSO;

            //Get attribute
            DecisionAttribute attribute = m_decision.GetType().GetCustomAttribute<DecisionAttribute>();

            //Title
            title = attribute == null ? m_decision.GetType().Name : attribute.Path;
            titleButtonContainer.RemoveFromHierarchy();
            titleContainer.style.backgroundColor = TITLE_BACKGROUND_COLOR;
            titleContainer.style.flexDirection = FlexDirection.RowReverse;
            foreach (VisualElement child in titleContainer.Children())
            {
                child.style.marginRight = 5f;
                child.style.flexGrow = 1f;
                child.style.unityTextAlign = TextAnchor.MiddleRight;
            }

            //Port
            m_port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Port));
            m_port.portName = "";
            m_port.style.alignSelf = Align.Center;
            titleContainer.Add(Port);

            //Info
            if (attribute != null && !string.IsNullOrEmpty(attribute.Info))
            {
                IMGUIContainer infoContainer = new IMGUIContainer(() =>
                {
                    EditorGUILayout.HelpBox(attribute.Info, MessageType.Info);
                });
                infoContainer.style.marginBottom = infoContainer.style.marginTop = infoContainer.style.marginLeft = infoContainer.style.marginRight = 4f;
                extensionContainer.Add(infoContainer);
            }

            //Properties
            IMGUIContainer properties = StateMachineEditorUtilities.GeneratePropertyContainer(m_decision, typeof(DecisionSO), 300f);
            if (properties != null)
            {
                extensionContainer.Add(properties);
            }

            contentContainer.style.width = 300f;
            extensionContainer.style.backgroundColor = StateMachineEditorUtilities.GetUnityDefaultBackgroundColor();
            RefreshExpandedState();

            SetPosition(new Rect(decisionNode.Position, GetPosition().size));
        }

        public override bool IsResizable()
        {
            return false;
        }
    }
}
