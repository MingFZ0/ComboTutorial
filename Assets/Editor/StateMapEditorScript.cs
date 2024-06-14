using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateMap))]
public class StateMapEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        StateMap stateMap = target as StateMap;
        stateMap.StateAnimations = new AnimationClip[Enum.GetValues(typeof(StateAnimation)).Length];
        string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));

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
                EditorGUILayout.LabelField(stateAnimationEnumStrings[i]);
                stateMap.StateAnimations[i] = clips[EditorGUILayout.Popup(0, clipNames)];
                EditorGUILayout.EndHorizontal();
            }
        }
            
    }
}
