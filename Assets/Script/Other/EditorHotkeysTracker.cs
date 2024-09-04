using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EditorHotkeysTracker
{
    public static CollideAndDamage collideAndDamage;
    static EditorHotkeysTracker()
    {
        SceneView.duringSceneGui += view =>
        {
            var e = Event.current;
            if (e != null && e.keyCode == KeyCode.K)
            {
                CollideAndDamage.test = true;
            }
        };
    }
}