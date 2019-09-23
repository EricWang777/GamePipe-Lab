using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BeatingHealthBar))]
public class BeatingHeartsEditor : IconProgressBarEditor
{
    SerializedProperty minHeartSize, maxHeartSize,
        minBeatingTime, maxBeatingTime,
        yPivot;

    void OnEnable()
    {
        base.OnEnable();

        minHeartSize = serializedObject.FindProperty("minHeartSize");
        maxHeartSize = serializedObject.FindProperty("maxHeartSize");

        minBeatingTime = serializedObject.FindProperty("minBeatingTime");
        maxBeatingTime = serializedObject.FindProperty("maxBeatingTime");

        yPivot = serializedObject.FindProperty("yPivot");
    }

    public override void OnInspectorGUI()
    {
        #region Initialise
        serializedObject.Update();
        current = (BeatingHealthBar)target;
        #endregion

        DrawUI();//draw the base
        current.showFractions = true;//force showing fractions. Doesn't actually do anything but easier to explain to users when viewing the other inspector 

        EditorGUILayout.PropertyField(minHeartSize);
        EditorGUILayout.PropertyField(maxHeartSize);

        EditorGUILayout.PropertyField(minBeatingTime);
        EditorGUILayout.PropertyField(maxBeatingTime);

        #region Pivot
        EditorGUILayout.PropertyField(yPivot);

        ((BeatingHealthBar)current).UpdatePivot();//update the images with any new pivot values
        #endregion

        if(GUI.changed)
            EditorUtility.SetDirty(target);

        serializedObject.ApplyModifiedProperties();// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
    }
}
