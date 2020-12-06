using UnityEngine;
using UnityEditor;

namespace FluffyHippo
{
    // Display a zoom indicator overlay on the 2D scene view.  Click it to reset to 100% zoom
    [InitializeOnLoad]
    public static class EditorZoomSlider
    {
        static GUIStyle buttonStyle = new GUIStyle();
        static float sliderPos;
        static bool sliderHasBeenSetOnStart = false;
        static float defaultSliderPos = 3f;
        static float zoomMultiplier = 1f;

        static EditorZoomSlider()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            buttonStyle.normal.textColor = Color.white;
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if (!sceneView.in2DMode)
                return;

            // Calculate current zoom level
            // float zoom = GetSceneViewHeight(sceneView) / (sceneView.camera.orthographicSize * 2f);
            float zoom = GetSceneViewHeight(sceneView) / (sceneView.camera.orthographicSize * 2f);

            if (!sliderHasBeenSetOnStart)
            {

                sliderHasBeenSetOnStart = true;
                sliderPos = defaultSliderPos;
                SetSceneViewZoom(sceneView, sliderPos);
            }

            // Display zoom indicator in top left corner; clicking the indicator resets to 100%
            Handles.BeginGUI();

            float newSliderPos = GUI.HorizontalSlider(new Rect(5, 5, 100, buttonStyle.lineHeight),
                             sliderPos, .5f, 15f);

            // If slider was moved
            if (newSliderPos != sliderPos)
            {
                sliderPos = newSliderPos;
                SetSceneViewZoom(sceneView, sliderPos);
            }

            // Reset button / display zoom level
            if (GUI.Button(new Rect(110, 5, 50, buttonStyle.lineHeight), $"{zoom * 100:N0}%", buttonStyle))
            {
                sliderPos = defaultSliderPos;
                SetSceneViewZoom(sceneView, sliderPos);
            }
            Handles.EndGUI();
        }

        static float GetSceneViewHeight(SceneView sceneView)
        {
            // Don't use sceneView.position.height, as it does not account for the space taken up by
            // toolbars.
            return sceneView.position.width / sceneView.camera.aspect;
        }

        static void SetSceneViewZoom(SceneView sceneView, float zoom)
        {
            zoom *= zoomMultiplier;
            float orthoHeight = GetSceneViewHeight(sceneView) / 2f / zoom;

            // We can't set camera.orthographicSize directly, because SceneView overrides it
            // every frame based on SceneView.size, so set SceneView.size instead.
            //
            // See SceneView.GetVerticalOrthoSize for the source of these sqrts.
            // sceneView.size = orthoHeight * Mathf.Sqrt(2f) * Mathf.Sqrt(sceneView.camera.aspect);
            float size = orthoHeight * Mathf.Sqrt(2f) * Mathf.Sqrt(sceneView.camera.aspect);
            sceneView.LookAt(sceneView.pivot, sceneView.rotation, size);
        }
    }
}