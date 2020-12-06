using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FluffyHippo
{
    [CustomEditor(typeof(CatmullRomSpline))]
    public class CatmullRomSplineEditor : Editor
    {
        protected void OnSceneGUI()
        {
            CatmullRomSpline spline = target as CatmullRomSpline;

            Transform handleTransform = spline.transform;
            Vector3 position = handleTransform.TransformPoint(spline.position);
            Quaternion handleRotation = handleTransform.rotation;
            // Vector3 p0 = handleTransform.TransformPoint(spline.p0);
            // Vector3 p1 = handleTransform.TransformPoint(spline.p1);

            Handles.color = Color.white;
            // Handles.DoPositionHandle(position, handleRotation);


            EditorGUI.BeginChangeCheck();
            position = Handles.DoPositionHandle(position, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.position = handleTransform.InverseTransformPoint(position);
            }
            // EditorGUI.BeginChangeCheck();
            // p1 = Handles.DoPositionHandle(p1, handleRotation);
            // if (EditorGUI.EndChangeCheck())
            // {
            //     Undo.RecordObject(line, "Move Point");
            //     EditorUtility.SetDirty(line);
            //     line.p1 = handleTransform.InverseTransformPoint(p1);
            // }
            // Handles.DrawLine(p0, p1);
            // Handles.DoPositionHandle(p0, handleRotation);
            // Handles.DoPositionHandle(p1, handleRotation);

            // EditorGUI.BeginChangeCheck();
            // Vector3 newTargetPosition = Handles.PositionHandle(spline.position, Quaternion.identity);
            // if (EditorGUI.EndChangeCheck())
            // {
            //     Undo.RecordObject(spline, "Change Position");
            //     spline.position = newTargetPosition;
            //     // spline.Update();
            // }
        }
    }
}
