using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateAnimationMap))]
public class StateMapEditorScript : Editor
{
    string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));

    public override void OnInspectorGUI()
    {
        StateAnimationMap stateMap = target as StateAnimationMap;
        int[] inputs = CreateEmptyIntArray(Enum.GetNames(typeof(StateAnimation)).Length);
        int[] priorityLevelInputs = CreateEmptyIntArray(Enum.GetNames(typeof(StateAnimation)).Length);
        if (stateMap.inputs.Length > 0) { inputs = stateMap.inputs; }
        if (stateMap.priorityIndexInputs.Length > 0) { priorityLevelInputs = stateMap.priorityIndexInputs; }

        stateMap.AnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField(stateMap.AnimatorController, typeof(RuntimeAnimatorController), false);

        if (stateMap.AnimatorController != null)
        {
            AnimationClip[] clips = stateMap.AnimatorController.animationClips;
            string[] clipNames = ConvertClipsToName(clips);

            for (int i = 0; i < stateAnimationEnumStrings.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 150;
                priorityLevelInputs[i] = EditorGUILayout.IntField(new GUIContent("Priority Level Index"), priorityLevelInputs[i], GUILayout.Width(180));
                GUILayout.Space(30);
                EditorGUIUtility.labelWidth = 50;
                inputs[i] = EditorGUILayout.Popup(stateAnimationEnumStrings[i], inputs[i], clipNames, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
            }
            stateMap.inputs = inputs;
            stateMap.priorityIndexInputs = priorityLevelInputs;
            serializedObject.ApplyModifiedProperties();
        }
    }

    private string[] ConvertClipsToName(AnimationClip[] clips)
    {
        string[] clipNames = new string[clips.Length];
        for (int i = 0; i < clips.Length; i++) { clipNames[i] = clips[i].name; }
        return clipNames;
    }

    private int[] CreateEmptyIntArray(int size)
    {
        int[] intArray = new int[size];
        for (int i = 0; i < size; i++) 
        {
            intArray[i] = 0;
        }
        return intArray;
    }
}
