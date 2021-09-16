using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace Graphs.StateMachine.Editor
{
    public static class StateMachineEditorUtilities
    {
        const BindingFlags BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        static MethodInfo m_getDefaultBackground;

        public static Color GetUnityDefaultBackgroundColor()
        {
            if (m_getDefaultBackground == null)
            {
                m_getDefaultBackground = typeof(EditorGUIUtility).GetMethod("GetDefaultBackgroundColor", BindingFlags.NonPublic | BindingFlags.Static);
            }
            return (Color)m_getDefaultBackground.Invoke(null, null);
        }

        static void GetAllFields(Type type, ref List<FieldInfo> results, Type baseType)
        {
            if (type == baseType)
            {
                return;
            }

            GetAllFields(type.BaseType, ref results, baseType);

            foreach (FieldInfo field in type.GetFields(BINDING_FLAGS))
            {
                results.Add(field);
            }
        }

        public static IMGUIContainer GeneratePropertyContainer(UnityEngine.Object obj, Type baseType, float width)
        {
            SerializedObject serializedObject = new SerializedObject(obj);
            List<FieldInfo> infoArray = new List<FieldInfo>();
            GetAllFields(obj.GetType(), ref infoArray, baseType);
            List<SerializedProperty> properties = new List<SerializedProperty>();
            foreach (FieldInfo info in infoArray)
            {
                if (info.GetCustomAttribute<HideInInspector>() != null)
                {
                    continue;
                }

                SerializedProperty property = serializedObject.FindProperty(info.Name);
                if (property != null)
                {
                    properties.Add(property);
                }
            }

            if (properties.Count == 0)
            {
                return null;
            }

            width -= 16f;
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                float originalLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 0.35f * width;
                foreach (SerializedProperty property in properties)
                {
                    EditorGUILayout.PropertyField(property);
                }
                EditorGUIUtility.labelWidth = originalLabelWidth;

                serializedObject.ApplyModifiedProperties();
            });
            container.style.paddingBottom = container.style.paddingLeft = container.style.paddingRight = container.style.paddingTop = 4f;
            container.style.marginBottom = container.style.marginLeft = container.style.marginRight = container.style.marginTop = 4f;
            container.style.flexGrow = 1f;
            container.style.width = width;

            return container;
        }
    }
}
