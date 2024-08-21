using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorHotkeysTracker
{
    static EditorHotkeysTracker()
    {
        SceneView.duringSceneGui += view =>
        {
            var e = Event.current;
            if (e != null && e.keyCode == KeyCode.F)
            {
                
            }
        };
    }
}