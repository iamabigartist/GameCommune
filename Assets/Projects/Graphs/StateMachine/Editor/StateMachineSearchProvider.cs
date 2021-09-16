using System;
using System.Collections.Generic;
using System.Reflection;
using Graphs.StateMachine.ScriptableObjects;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
namespace Graphs.StateMachine.Editor
{
    public class StateMachineSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        static readonly string STATE_ICON_NAME = "StateIcon";
        static readonly string ACTION_ICON_NAME = "ActionIcon";
        static readonly string DECISION_ICON_NAME = "DecisionIcon";

        static List<SearchTreeEntry> SEARCH_TREE;

        public Action<Type, Vector2> OnCreateNode;

        void OnEnable()
        {
            Texture stateIcon = Resources.Load<Texture>(STATE_ICON_NAME);
            Texture actionIcon = Resources.Load<Texture>(ACTION_ICON_NAME);
            Texture decisionIcon = Resources.Load<Texture>(DECISION_ICON_NAME);

            SortedList<string, Type> actions = new SortedList<string, Type>();
            SortedList<string, Type> decisions = new SortedList<string, Type>();
            foreach (Type type in TypeCache.GetTypesDerivedFrom<ActionSO>())
            {
                if (type.IsGenericTypeDefinition)
                {
                    continue;
                }

                ActionAttribute attribute = type.GetCustomAttribute<ActionAttribute>();

                actions.Add(attribute == null ? type.Name : attribute.Path, type);
            }
            foreach (Type type in TypeCache.GetTypesDerivedFrom<DecisionSO>())
            {
                if (type.IsGenericTypeDefinition)
                {
                    continue;
                }

                DecisionAttribute attribute = type.GetCustomAttribute<DecisionAttribute>();

                decisions.Add(attribute == null ? type.Name : attribute.Path, type);
            }

            SEARCH_TREE = new List<SearchTreeEntry>();
            SEARCH_TREE.Add(new SearchTreeGroupEntry(new GUIContent("Create"), 0));

            SEARCH_TREE.Add(new SearchTreeEntry(new GUIContent("State", stateIcon)) { userData = typeof(StateSO), level = 1 });

            SEARCH_TREE.Add(new SearchTreeGroupEntry(new GUIContent("Action"), 1));
            HandleGroups(actions, actionIcon);

            SEARCH_TREE.Add(new SearchTreeGroupEntry(new GUIContent("Decision"), 1));
            HandleGroups(decisions, decisionIcon);
        }

        void HandleGroups(SortedList<string, Type> entries, Texture icon)
        {
            int level = 2;
            string group = "";
            foreach (KeyValuePair<string, Type> entryPair in entries)
            {
                string[] path = entryPair.Key.Split('.');
                int newLevel = path.Length + 1;
                if (newLevel != level)
                {
                    level = newLevel;
                    if (level > 2)
                    {
                        group = path[path.Length - 2];
                        SEARCH_TREE.Add(new SearchTreeGroupEntry(new GUIContent(group), level - 1));
                    }
                }
                else
                {
                    string newGroup = path[path.Length - 2];
                    if (newGroup != group)
                    {
                        group = newGroup;
                        SEARCH_TREE.Add(new SearchTreeGroupEntry(new GUIContent(group), level - 1));
                    }
                }
                SEARCH_TREE.Add(new SearchTreeEntry(new GUIContent(path[path.Length - 1], icon)) { userData = entryPair.Value, level = level });
            }
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            return SEARCH_TREE;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            OnCreateNode?.Invoke((Type)SearchTreeEntry.userData, context.screenMousePosition);
            return true;
        }
    }
}
