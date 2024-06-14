using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateMap))]
public class StateMapEditorScript : Editor
{
    string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));

    public override void OnInspectorGUI()
    {
        StateMap stateMap = target as StateMap;
        Dictionary<string, AnimationClip> stateDictionaryMap = new Dictionary<string, AnimationClip>();
        int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];
        if (stateMap.inputs != null) { inputs = stateMap.inputs; }
        //inputs = stateMap.inputs;

        stateMap.AnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField(stateMap.AnimatorController, typeof(RuntimeAnimatorController), false);

        if (stateMap.AnimatorController != null)
        {
            AnimationClip[] clips = stateMap.AnimatorController.animationClips;
            string[] clipNames = new string[clips.Length];

            for (int i = 0; i < clips.Length; i++)
            {
                clipNames[i] = clips[i].name;
            }

            for (int i = 0; i < stateAnimationEnumStrings.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField(stateAnimationEnumStrings[i]);
                inputs[i] = EditorGUILayout.Popup(stateAnimationEnumStrings[i], inputs[i], clipNames);
                stateMap.StateAnimations[i] = clips[i];
                EditorGUILayout.EndHorizontal();
                stateDictionaryMap.Add(stateAnimationEnumStrings[i], stateMap.StateAnimations[i]);
            }
            stateMap.inputs = inputs;

        }
            
    }
}
