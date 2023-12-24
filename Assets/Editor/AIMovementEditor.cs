using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIMovement))]
public class AIMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AIMovement aiMovement = target as AIMovement;
        EditorGUILayout.LabelField(
            "Target Position", aiMovement.TargetPosition.ToString());
        EditorGUILayout.LabelField(
            "Next Node", aiMovement.NextNode.ToString());
    }
}
