using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DummyAction))]
public class DummyActionEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DummyAction dummyAction = (DummyAction)target;
        dummyAction.AnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField(dummyAction.AnimatorController, typeof(RuntimeAnimatorController), false);
    
        if (dummyAction.AnimatorController != null)
        {
            AnimationClip[] clips = dummyAction.AnimatorController.animationClips;
            string[] clipNames = ConvertClipsToName(clips);

            dummyAction.MoveInputInt = EditorGUILayout.Popup("Selected Move", dummyAction.MoveInputInt, clipNames);
            dummyAction.SelectedMove = clips[dummyAction.MoveInputInt];
            serializedObject.Update();
        }
    }

    private string[] ConvertClipsToName(AnimationClip[] clips)
    {
        string[] clipNames = new string[clips.Length];
        for (int i = 0; i < clips.Length; i++) { clipNames[i] = clips[i].name; }
        return clipNames;
    }
}
