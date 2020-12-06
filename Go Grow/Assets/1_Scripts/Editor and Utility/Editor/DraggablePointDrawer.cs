﻿using System;
using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FluffyHippo
{
    /// <summary>
    /// From https://gist.github.com/ProGM/226204b2a7f99998d84d755ffa1fb39a
    /// </summary>
    #if UNITY_EDITOR
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class DraggablePointDrawer : Editor
    {
        GUIStyle style = new GUIStyle();

        void OnEnable()
        {
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;
        }

        public void OnSceneGUI()
        {
            SerializedProperty property = serializedObject.GetIterator();
            while (property.Next(true))
            {
                if (property.propertyType == SerializedPropertyType.Vector3)
                {
                    handleVectorProperty(property);
                }
                else if (property.isArray)
                {
                    for (int x = 0; x < property.arraySize; x++)
                    {
                        SerializedProperty element = property.GetArrayElementAtIndex(x);
                        if (element.propertyType != SerializedPropertyType.Vector3)
                        {
                            //Break early if we're not an array of Vector3
                            break;
                        }
                        handleVectorPropertyInArray(element, property, x);
                    }
                }
            }
        }

        void handleVectorProperty(SerializedProperty property)
        {
            FieldInfo field = serializedObject.targetObject.GetType().GetField(property.name);
            if (field == null)
            {
                return;
            }
            var draggablePoints = field.GetCustomAttributes(typeof(DraggablePointAttribute), false);
            if (draggablePoints.Length > 0)
            {
                Handles.Label(property.vector3Value + ((MonoBehaviour)target).transform.position, 
                              property.name, 
                              style);
                property.vector3Value = Handles.PositionHandle(property.vector3Value + ((MonoBehaviour)target).transform.position, Quaternion.identity) - ((MonoBehaviour)target).transform.position;
                serializedObject.ApplyModifiedProperties();
            }
        }

        void handleVectorPropertyInArray(SerializedProperty property, SerializedProperty parent, int index)
        {
            FieldInfo parentfield = serializedObject.targetObject.GetType().GetField(parent.name);
            if (parentfield == null)
            {
                return;
            }
            var draggablePoints = parentfield.GetCustomAttributes(typeof(DraggablePointAttribute), false);
            if (draggablePoints.Length > 0)
            {
                Handles.Label(property.vector3Value + ((MonoBehaviour)target).transform.position, 
                              parent.name + "[" + index + "]",
                              style);
                property.vector3Value = Handles.PositionHandle(property.vector3Value + ((MonoBehaviour)target).transform.position, Quaternion.identity) - ((MonoBehaviour)target).transform.position;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
    #endif
}