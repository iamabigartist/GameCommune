using Graphs.StateMachine.ScriptableObjects;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
namespace Graphs.StateMachine.Editor
{
    public class StateMachineEditor : EditorWindow
    {
        static Color EDIT_BLOCKER_COLOR = new Color(0f, 0f, 0f, 0.5f);
        static StateMachineEditor WINDOW = null;

        StateMachineView m_view;
        Toolbar m_toolbar;
        VisualElement m_editBlocker;
        ObjectField m_selector;
        Label m_blockerInfo;

        StateMachineSO m_target;

        [MenuItem("Tools/State Machine Editor")]
        public static void ShowWindow()
        {
            if (WINDOW == null)
            {
                WINDOW = GetWindow<StateMachineEditor>();
            }

            if (WINDOW == null)
            {
                WINDOW = CreateWindow<StateMachineEditor>();
                WINDOW.titleContent = new GUIContent("State Machine Editor");
                WINDOW.Show();
                WINDOW.SetTarget(null);
            }
            else
            {
                WINDOW.Focus();
            }
        }

        public static void ShowStateMachine(StateMachineSO target)
        {
            ShowWindow();
            WINDOW.SetTarget(target);
        }

        void OnEnable()
        {
            Init();
            Undo.undoRedoPerformed += OnUndo;
        }

        void OnDisable()
        {
            Disable();
            Undo.undoRedoPerformed -= OnUndo;
        }

        void Init()
        {
            m_view = CreateView();
            m_editBlocker = CreateEditBlocker();
            m_toolbar = CreateToolbar();
            rootVisualElement.Add(m_view);
            rootVisualElement.Add(m_editBlocker);
            rootVisualElement.Add(m_toolbar);
        }

        void Disable()
        {
            rootVisualElement.Remove(m_view);
            rootVisualElement.Remove(m_editBlocker);
            rootVisualElement.Remove(m_toolbar);
        }

        void OnUndo()
        {
            SetTarget(m_target);
        }

        Toolbar CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();

            //Graph selector
            m_selector = new ObjectField();
            m_selector.objectType = typeof(StateMachineSO);
            m_selector.RegisterCallback<ChangeEvent<Object>>(evt =>
            {
                SetTarget(evt.newValue as StateMachineSO);
            });
            toolbar.Add(m_selector);

            toolbar.StretchToParentSize();
            return toolbar;
        }

        StateMachineView CreateView()
        {
            StateMachineView view = new StateMachineView(this);
            view.StretchToParentSize();
            return view;
        }

        VisualElement CreateEditBlocker()
        {
            VisualElement blocker = new VisualElement();
            blocker.style.position = Position.Absolute;
            blocker.style.top = blocker.style.bottom = blocker.style.left = blocker.style.right = 0f;
            blocker.style.backgroundColor = EDIT_BLOCKER_COLOR;

            m_blockerInfo = new Label();
            m_blockerInfo.style.fontSize = 50f;
            m_blockerInfo.style.unityTextAlign = TextAnchor.MiddleCenter;
            m_blockerInfo.StretchToParentSize();
            blocker.Add(m_blockerInfo);

            blocker.StretchToParentSize();
            return blocker;
        }

        void SetTarget(StateMachineSO target)
        {
            m_target = target;
            m_selector.SetValueWithoutNotify(target);

            m_view.SetStateMachine(target);

            if (m_target == null)
            {
                m_editBlocker.visible = true;
                m_blockerInfo.text = "Select a state machine";
            }
            else
            {
                m_editBlocker.visible = false;
            }
        }
    }
}
