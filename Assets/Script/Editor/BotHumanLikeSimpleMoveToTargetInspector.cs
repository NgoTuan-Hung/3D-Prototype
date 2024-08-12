using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BotHumanLikeSimpleMoveToTarget))]
public class BotHumanLikeSimpleMoveToTargetInspector : Editor
{
    // public VisualTreeAsset visualTreeAsset;
    // public override VisualElement CreateInspectorGUI()
    // {
    //     VisualElement myInspector = new VisualElement();

    //     myInspector.Add(new Label("This is a custom Inspector"));
    //     // visualTreeAsset.CloneTree(myInspector);
    //     return myInspector;
    // }

    private BotHumanLikeSimpleMoveToTarget.Intelligence selectIntelligence;
    public override void OnInspectorGUI()
    {
        BotHumanLikeSimpleMoveToTarget botHumanLikeSimpleMoveToTarget = (BotHumanLikeSimpleMoveToTarget)target;
        selectIntelligence = (BotHumanLikeSimpleMoveToTarget.Intelligence)EditorGUILayout.EnumPopup("Intelligence Mode", botHumanLikeSimpleMoveToTarget.IntelligenceMode);
        if (selectIntelligence != botHumanLikeSimpleMoveToTarget.IntelligenceMode)
        {
            botHumanLikeSimpleMoveToTarget.IntelligenceMode = selectIntelligence;
        }
    }
}
