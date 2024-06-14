using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateMap))]
public class StateMapEditorScript : Editor
{
    string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));

    public override void OnInspectorGUI()
    {
        StateMap stateMap = target as StateMap;
        int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];
        if (stateMap.inputs != null) { inputs = stateMap.inputs; }
        //inputs = stateMap.inputs;

        stateMap.AnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField(stateMap.AnimatorController, typeof(RuntimeAnimatorController), false);

        if (stateMap.AnimatorController != null)
        {
            AnimationClip[] clips = stateMap.AnimatorController.animationClips;
            string[] clipNames = ConvertClipsToName(clips);

            for (int i = 0; i < stateAnimationEnumStrings.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                inputs[i] = EditorGUILayout.Popup(stateAnimationEnumStrings[i], inputs[i], clipNames);
                stateMap.StateAnimations[i] = clips[i];
                EditorGUILayout.EndHorizontal();

                stateMap.StateAnimationMap[stateAnimationEnumStrings[i]] = clips[inputs[i]];
            }

            //Debug.Log(dictToString);
            stateMap.inputs = inputs;
            string dictToString = "";
            foreach (string key in stateMap.StateAnimationMap.Keys)
            {
                dictToString += key + ": " + stateMap.StateAnimationMap[key].name + "; ";
            }
            Debug.Log(dictToString);
        }
    }

    private string[] ConvertClipsToName(AnimationClip[] clips)
    {
        string[] clipNames = new string[clips.Length];
        for (int i = 0; i < clips.Length; i++) { clipNames[i] = clips[i].name; }
        return clipNames;
    }
}
