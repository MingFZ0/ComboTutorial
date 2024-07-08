using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;

[CustomEditor(typeof(AnimationMap))]
public class AnimationMapEditorScript : Editor
{

    private string[] tabs = { "Movement Map", "Attack Map", "State Map" };
    private int tabIndex = 0;
    private AnimationMap animationMap;

    public override void OnInspectorGUI()
    {
        animationMap = (AnimationMap)target;
        
        EditorGUILayout.Space();
        animationMap.AnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField("Animator Controller", animationMap.AnimatorController, typeof(RuntimeAnimatorController), false);
        
        if (animationMap.AnimatorController == null)
        {
            EditorGUILayout.HelpBox("Animator Controller is needed to assign animations to moves", MessageType.Warning);
            return;
        }
        
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        tabIndex = GUILayout.Toolbar(tabIndex, tabs);
        EditorGUILayout.EndHorizontal();
        
        
        EditorGUILayout.Space();

        switch (tabIndex)
        {
            case 0:
                MovementMap();
                break;
            case 1:
                AttackMap(); 
                break;
            case 2:
                StateMap(); 
                break;
            default:
                MovementMap();
                break;
        }

        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    private void MovementMap()
    {
        EditorGUILayout.TextField(tabs[tabIndex].ToString());
    }

    private void AttackMap()
    {
        EditorGUI.indentLevel++;

        //EditorGUILayout.TextField(tabs[tabIndex].ToString());
        string[] levelNames = Enum.GetNames(typeof(ActionEnum));

        if (animationMap.ActionAnimationMap.PriorityLevels == null || animationMap.ActionAnimationMap.PriorityLevels.Length == 0)
        {
            animationMap.ActionAnimationMap.PriorityLevels = new PriorityLevel[levelNames.Length];
            for (int i = ActionAnimationMap.StartingPriorityLevelIndex; i < levelNames.Length; i++) { animationMap.ActionAnimationMap.PriorityLevels[i] = new PriorityLevel(); }
            Debug.Log(animationMap.ActionAnimationMap.PriorityLevels.Length);
        }

        for (int i = ActionAnimationMap.StartingPriorityLevelIndex; i < levelNames.Length; i++)
        {
            PriorityLevel priorityLevel = animationMap.ActionAnimationMap.PriorityLevels[i];

            //priorityLevel.Fold = EditorGUILayout.Foldout(priorityLevel.Fold, levelNames[i]);
            priorityLevel.Fold = EditorGUILayout.BeginFoldoutHeaderGroup(priorityLevel.Fold, levelNames[i]);

            if (priorityLevel.Fold)
            {
                GUILayout.Box("", GUILayout.MaxWidth(float.MaxValue), GUILayout.Height(5));
                EditorGUILayout.LabelField("Priority Index [" + i + "]");
                priorityLevel.LevelInput = (InputActionReference)EditorGUILayout.ObjectField("Level Input", priorityLevel.LevelInput, typeof(InputActionReference), false);
                GUILayout.Box("", GUILayout.MaxWidth(float.MaxValue), GUILayout.Height(5));

                EditorGUI.indentLevel++;

                AnimationClip[] allClips = animationMap.AnimatorController.animationClips;
                string[] allClipNames = new string[allClips.Length];
                for (int clipIndex = 0; clipIndex < allClips.Length; clipIndex++) { allClipNames[clipIndex] = allClips[clipIndex].name; }

                foreach (Move move in priorityLevel.Moves)
                {
                    EditorGUILayout.Space();
                    move.DirectionalInput = (InputActionReference)EditorGUILayout.ObjectField("Directional Input", move.DirectionalInput, typeof(InputActionReference), false);
                    move.AnimationClipIndexInput = EditorGUILayout.Popup("AnimationClip", move.AnimationClipIndexInput, allClipNames);
                    move.Grounded = EditorGUILayout.Toggle("Grounded", move.Grounded);
                    move.AnimationClip = allClips[move.AnimationClipIndexInput];

                    EditorGUI.indentLevel++;
                    Motion movementAcceration;
                    if (move.MovementAcceration != null) { movementAcceration = move.MovementAcceration; }
                    else
                    {
                        move.MovementAcceration = new Motion();
                        movementAcceration = new();
                    }

                    AnimationCurve verticalMovementCurve;
                    AnimationCurve horizontalMovementCurve;
                    if (movementAcceration.VerticalAccerationCurve != null) { verticalMovementCurve = movementAcceration.VerticalAccerationCurve; }
                    else { verticalMovementCurve = new(); }
                    if (movementAcceration.HorizontalAccerationCurve != null) { horizontalMovementCurve = movementAcceration.HorizontalAccerationCurve; }
                    else { horizontalMovementCurve = new(); }

                    movementAcceration.VerticalAccerationCurve = EditorGUILayout.CurveField("Vertical Acceration", verticalMovementCurve);
                    movementAcceration.HorizontalAccerationCurve = EditorGUILayout.CurveField("Horizontal Acceration", horizontalMovementCurve);
                    move.MovementAcceration = movementAcceration;
                    EditorGUI.indentLevel--;
                    EditorGUILayout.HelpBox("", MessageType.None);

                }
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                if (EditorGUILayout.DropdownButton(new GUIContent("Add Move"), FocusType.Passive))
                {
                    priorityLevel.Moves.Add(new Move());
                }
                if (EditorGUILayout.DropdownButton(new GUIContent("Remove Move"), FocusType.Passive))
                {
                    priorityLevel.Moves.RemoveAt(priorityLevel.Moves.Count - 1);
                }
                EditorGUILayout.EndHorizontal();

                //Move move = new Move(actionMap.AnimatorController);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }

        EditorGUI.indentLevel--;
    }

    private void StateMap()
    {
        EditorGUILayout.TextField(tabs[tabIndex].ToString());
    }
}
