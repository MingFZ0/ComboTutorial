using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(ActionAnimationMap))]
public class ActionMapEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        ActionAnimationMap actionMap = (ActionAnimationMap)target;
        actionMap.AnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField(actionMap.AnimatorController, typeof(RuntimeAnimatorController), false);

        string[] levelNames = Enum.GetNames(typeof(ActionEnum));

        if (actionMap.AnimatorController != null) {

            if (actionMap.PriorityLevels == null || actionMap.PriorityLevels.Length == 0)
            {
                actionMap.PriorityLevels = new PriorityLevel[levelNames.Length];
                for (int i = 0; i < levelNames.Length; i++) { actionMap.PriorityLevels[i] = new PriorityLevel(); }
                Debug.Log(actionMap.PriorityLevels.Length);
            }

            for (int i = 0; i < levelNames.Length; i++)
            {
                PriorityLevel priorityLevel = actionMap.PriorityLevels[i];

                priorityLevel.Fold = EditorGUILayout.Foldout(priorityLevel.Fold, levelNames[i]);

                if (priorityLevel.Fold)
                {
                    EditorGUILayout.LabelField("Priority Index [" + i + "]");
                    priorityLevel.LevelInput = (InputActionReference)EditorGUILayout.ObjectField("Level Input", priorityLevel.LevelInput, typeof(InputActionReference), false);

                    EditorGUI.indentLevel++;
                    AnimationClip[] allClips = actionMap.AnimatorController.animationClips;
                    string[] allClipNames = new string[allClips.Length];
                    for (int clipIndex = 0; clipIndex < allClips.Length; clipIndex++) { allClipNames[clipIndex] = allClips[clipIndex].name; }

                    foreach (Move move in priorityLevel.Moves)
                    {
                        EditorGUILayout.Space();
                        move.DirectionalInput = (InputActionReference)EditorGUILayout.ObjectField("Directional Input", move.DirectionalInput, typeof(InputActionReference), false);
                        move.AnimationClipIndexInput = EditorGUILayout.Popup("AnimationClip", move.AnimationClipIndexInput, allClipNames);
                        move.Grounded = EditorGUILayout.Toggle("Grounded", move.Grounded);
                        move.AnimationClip = allClips[move.AnimationClipIndexInput];
                    }
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    if (EditorGUILayout.DropdownButton(new GUIContent("Add Move"), FocusType.Passive))
                    {
                        priorityLevel.Moves.Add(new Move(actionMap.AnimatorController));
                    }
                    if (EditorGUILayout.DropdownButton(new GUIContent("Remove Move"), FocusType.Passive))
                    {
                        priorityLevel.Moves.RemoveAt(priorityLevel.Moves.Count - 1);
                    }
                    EditorGUILayout.EndHorizontal();
                    //Move move = new Move(actionMap.AnimatorController);

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}
