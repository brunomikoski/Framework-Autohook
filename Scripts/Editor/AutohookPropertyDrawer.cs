using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BrunoMikoski.Framework.AutoHook
{
   [CustomPropertyDrawer(typeof(AutohookAttribute), true)]
    public sealed class AutohookPropertyDrawer : PropertyDrawer
    {
        private Visibility visibility;

        private const BindingFlags BINDING_FLAGS = BindingFlags.IgnoreCase
                                                  | BindingFlags.Public
                                                  | BindingFlags.Instance
                                                  | BindingFlags.NonPublic;

        private AutohookAttribute AutoHookAttribute { get { return (AutohookAttribute)attribute; } }
        private List<Component> componentsList = new List<Component>(100);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Component component = FindAutohookTargets(property).First();
            if (component != null)
            {
                if (property.objectReferenceValue == null)
                    property.objectReferenceValue = component;

                if (visibility == Visibility.Hidden)
                    return;
            }

            bool guiEnabled = GUI.enabled;
            if (visibility == Visibility.Disabled && component != null)
                GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = guiEnabled;
        }

        private void UpdateVisibility()
        {
            visibility = AutoHookAttribute.Visibility;
            if (visibility == Visibility.Default)
            {
                visibility = (Visibility)EditorPrefs.GetInt(AutoHookEditorSettings.AUTO_HOOK_VISIBILITY_KEY,
                    0);

            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            UpdateVisibility();

            Component component = FindAutohookTargets(property).First();
            if (component != null && visibility == Visibility.Hidden)
                return 0;

            return base.GetPropertyHeight(property, label);
        }

        private List<Component> FindAutohookTargets(SerializedProperty property)
        {
            SerializedObject root = property.serializedObject;
            componentsList.Clear();

            if (root.targetObject is Component)
            {
                Type type = GetTypeFromProperty(property);

                Component component = (Component)root.targetObject;
                switch (AutoHookAttribute.Context)
                {
                    case Context.Self:
                    {
                        component.GetComponents(type, componentsList);
                        return componentsList;
                    }

                    case Context.Child:
                    {
                        componentsList.Clear();
                        componentsList.AddRange(component.GetComponentsInChildren(type, true));

                        if (AutoHookAttribute.IgnoreSelf)
                        {
                            for (int i = 0; i < componentsList.Count; i++)
                            {
                                Component foundComponent = componentsList[i];
                                if (foundComponent.gameObject != component.gameObject)
                                    continue;

                                componentsList.RemoveAt(i);
                                break;
                            }
                        }
                        return componentsList;
                    }
                    case Context.Parent:
                    {
                        componentsList.Clear();
                        componentsList.AddRange(component.GetComponentsInParent(type, true));

                        if (AutoHookAttribute.IgnoreSelf)
                        {
                            for (int i = 0; i < componentsList.Count; i++)
                            {
                                Component foundComponent = componentsList[i];
                                if (foundComponent.gameObject != component.gameObject)
                                    continue;

                                componentsList.RemoveAt(i);
                                break;

                            }
                        }
                        return componentsList;
                    }
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
            FieldInfo fieldInfo = parentComponentType.GetField(property.propertyPath, BINDING_FLAGS);
            return fieldInfo.FieldType;
        }
    }
}
