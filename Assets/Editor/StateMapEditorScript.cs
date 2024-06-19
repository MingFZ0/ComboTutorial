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
        int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];
        if (stateMap.inputs != null) { inputs = stateMap.inputs; }

        stateMap.AnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField(stateMap.AnimatorController, typeof(RuntimeAnimatorController), false);

        if (stateMap.AnimatorController != null)
        {
            AnimationClip[] clips = stateMap.AnimatorController.animationClips;
            string[] clipNames = ConvertClipsToName(clips);

            for (int i = 0; i < stateAnimationEnumStrings.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                inputs[i] = EditorGUILayout.Popup(stateAnimationEnumStrings[i], inputs[i], clipNames);
                EditorGUILayout.EndHorizontal();
            }
            stateMap.inputs = inputs;
        }
    }

    private string[] ConvertClipsToName(AnimationClip[] clips)
    {
        string[] clipNames = new string[clips.Length];
        for (int i = 0; i < clips.Length; i++) { clipNames[i] = clips[i].name; }
        return clipNames;
    }
}
