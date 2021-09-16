using System.Reflection;
using Graphs.StateMachine.ScriptableObjects;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
namespace Graphs.StateMachine.Editor.Nodes
{
    public class StateMachineActionView : Node
    {
        static readonly Color TITLE_BACKGROUND_COLOR = new Color(0.1f, 0.5f, 0.1f, 0.8f);

        readonly ActionSO m_action;
        readonly Port m_port;

        public ActionSO ActionSO => m_action;
        public Port Port => m_port;

        public StateMachineActionView(ActionNode actionNode)
        {
            m_action = actionNode.ActionSO;

            //Get attribute
            ActionAttribute attribute = m_action.GetType().GetCustomAttribute<ActionAttribute>();

            //Title
            title = attribute == null ? m_action.GetType().Name : attribute.Path;
            titleButtonContainer.RemoveFromHierarchy();
            titleContainer.style.backgroundColor = TITLE_BACKGROUND_COLOR;
            foreach (VisualElement child in titleContainer.Children())
            {
                child.style.marginLeft = 5f;
                child.style.flexGrow = 1f;
                child.style.unityTextAlign = TextAnchor.MiddleLeft;
            }

            //Port
            m_port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
            m_port.portName = "";
            m_port.style.alignSelf = Align.Center;
            titleContainer.Add(m_port);

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
            IMGUIContainer properties = StateMachineEditorUtilities.GeneratePropertyContainer(m_action, typeof(ActionSO), 300f);
            if (properties != null)
            {
                extensionContainer.Add(properties);
            }

            contentContainer.style.width = 300f;
            extensionContainer.style.backgroundColor = StateMachineEditorUtilities.GetUnityDefaultBackgroundColor();
            RefreshExpandedState();

            SetPosition(new Rect(actionNode.Position, GetPosition().size));
        }

        public override bool IsResizable()
        {
            return false;
        }
    }
}
