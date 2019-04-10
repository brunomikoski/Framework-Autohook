using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Framework.AutoHook
{
    [CustomPropertyDrawer(typeof(AutohookAttribute))]
    public sealed class AutohookPropertyDrawer : PropertyDrawer
    {
        private const BindingFlags BINDIN_FLAGS = BindingFlags.IgnoreCase
                                                  | BindingFlags.Public
                                                  | BindingFlags.Instance
                                                  | BindingFlags.NonPublic;

        private AutohookAttribute AutoHookAttribute { get { return (AutohookAttribute)attribute; } }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Component component = FindAutohookTarget(property);
            if (component != null)
            {
                if (property.objectReferenceValue == null)
                    property.objectReferenceValue = component;

                if (AutoHookAttribute.Visibility == Visibility.Hidden)
                    return;
            }

            bool guiEnabled = GUI.enabled;
            if (AutoHookAttribute.Visibility == Visibility.Disabled)
                GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = guiEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Component component = FindAutohookTarget(property);
            if (component != null && AutoHookAttribute.Visibility == Visibility.Hidden)
                return 0;

            return base.GetPropertyHeight(property, label);
        }

        private Component FindAutohookTarget(SerializedProperty property)
        {
            SerializedObject root = property.serializedObject;

            if (root.targetObject is Component)
            {
                Type type = GetTypeFromProperty(property);

                Component component = (Component)root.targetObject;
                switch (AutoHookAttribute.Context)
                {
                    case Context.Self:
                        return component.GetComponent(type);
                    case Context.Child:
                        return component.GetComponentInChildren(type);
                    case Context.Parent:
                        return component.GetComponentInParent(type);
                }
            }
            else
            {
                throw new Exception(root.targetObject + "is not a component");
            }

            return null;
        }

        private static Type GetTypeFromProperty(SerializedProperty property)
        {
            Type parentComponentType = property.serializedObject.targetObject.GetType();
            FieldInfo fieldInfo = parentComponentType.GetField(property.propertyPath, BINDIN_FLAGS);
            return fieldInfo.FieldType;
        }
    }
}
