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

        EditorUtility.SetDirty(animationMap);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    private void MovementMap()
    {
        EditorGUILayout.TextField(tabs[tabIndex].ToString());
        string[] levelNames = Enum.GetNames(typeof(ActionEnum));

        PriorityLevel<Move> MovementPriorityLevel = animationMap.MovementAnimationMap.MovementMoveLevel;
        PriorityLevel<DashMove> DashPriorityLevel = animationMap.MovementAnimationMap.DashMoveLevel;

        MovementPriorityLevel.Fold = EditorGUILayout.BeginFoldoutHeaderGroup(MovementPriorityLevel.Fold, levelNames[0]);
        if (MovementPriorityLevel.Fold)
        {
            GUILayout.Box("", GUILayout.MaxWidth(float.MaxValue), GUILayout.Height(5));
            EditorGUILayout.LabelField("Priority Index [" + MovementPriorityLevel.PriorityLevelIndex + "]");
            MovementPriorityLevel.LevelInput = (InputActionReference)EditorGUILayout.ObjectField("Level Input", MovementPriorityLevel.LevelInput, typeof(InputActionReference), false);
            GUILayout.Box("", GUILayout.MaxWidth(float.MaxValue), GUILayout.Height(5));

            EditorGUI.indentLevel++;

            AnimationClip[] allClips = animationMap.AnimatorController.animationClips;
            string[] allClipNames = new string[allClips.Length];
            for (int clipIndex = 0; clipIndex < allClips.Length; clipIndex++) { allClipNames[clipIndex] = allClips[clipIndex].name; }

            foreach (Move move in MovementPriorityLevel.Moves)
            {
                EditorGUILayout.Space();
                move.DirectionalInput = (InputActionReference)EditorGUILayout.ObjectField("Directional Input", move.DirectionalInput, typeof(InputActionReference), false);
                move.AnimationClipIndexInput = EditorGUILayout.Popup("AnimationClip", move.AnimationClipIndexInput, allClipNames);
                move.Grounded = EditorGUILayout.Toggle("Grounded", move.Grounded);
                move.AnimationClip = allClips[move.AnimationClipIndexInput];

                
                //EditorGUI.indentLevel--;
                EditorGUILayout.HelpBox("", MessageType.None);

            }
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.DropdownButton(new GUIContent("Add Move"), FocusType.Passive))
            {
                MovementPriorityLevel.Moves.Add(new Move(MovementPriorityLevel.PriorityLevelIndex));
            }
            if (EditorGUILayout.DropdownButton(new GUIContent("Remove Move"), FocusType.Passive))
            {
                MovementPriorityLevel.Moves.RemoveAt(MovementPriorityLevel.Moves.Count - 1);
            }
            EditorGUILayout.EndHorizontal();
            animationMap.MovementAnimationMap.MovementMoveLevel = MovementPriorityLevel;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.Space();
        EditorGUI.indentLevel--;

        DashPriorityLevel.Fold = EditorGUILayout.BeginFoldoutHeaderGroup(DashPriorityLevel.Fold, levelNames[1]);
        if (DashPriorityLevel.Fold) 
        {
            GUILayout.Box("", GUILayout.MaxWidth(float.MaxValue), GUILayout.Height(5));
            EditorGUILayout.LabelField("Priority Index [" + DashPriorityLevel.PriorityLevelIndex + "]");
            DashPriorityLevel.LevelInput = (InputActionReference)EditorGUILayout.ObjectField("Level Input", DashPriorityLevel.LevelInput, typeof(InputActionReference), false);
            GUILayout.Box("", GUILayout.MaxWidth(float.MaxValue), GUILayout.Height(5));

            EditorGUI.indentLevel++;

            AnimationClip[] allClips = animationMap.AnimatorController.animationClips;
            string[] allClipNames = new string[allClips.Length];
            for (int clipIndex = 0; clipIndex < allClips.Length; clipIndex++) { allClipNames[clipIndex] = allClips[clipIndex].name; }

            foreach (DashMove move in DashPriorityLevel.Moves)
            {
                EditorGUILayout.Space();
                move.DirectionalInput = (InputActionReference)EditorGUILayout.ObjectField("Directional Input", move.DirectionalInput, typeof(InputActionReference), false);
                move.AnimationClipIndexInput = EditorGUILayout.Popup("AnimationClip", move.AnimationClipIndexInput, allClipNames);
                move.Grounded = EditorGUILayout.Toggle("Grounded", move.Grounded);
                move.AnimationClip = allClips[move.AnimationClipIndexInput];

                Motion movementAcceration = move.MovementCurve;
                AnimationCurve verticalMovementCurve = movementAcceration.VerticalAccerationCurve;
                AnimationCurve horizontalMovementCurve = movementAcceration.HorizontalAccerationCurve;

                EditorGUI.indentLevel++;
                movementAcceration.VerticalAccerationCurve = EditorGUILayout.CurveField("Vertical Movement", verticalMovementCurve);
                movementAcceration.HorizontalAccerationCurve = EditorGUILayout.CurveField("Horizontal Movement", horizontalMovementCurve);

                move.MovementCurve = movementAcceration;

                EditorGUI.indentLevel--;
                EditorGUILayout.HelpBox("", MessageType.None);

            }
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.DropdownButton(new GUIContent("Add Move"), FocusType.Passive))
            {
                DashPriorityLevel.Moves.Add(new DashMove(DashPriorityLevel.PriorityLevelIndex));
            }
            if (EditorGUILayout.DropdownButton(new GUIContent("Remove Move"), FocusType.Passive))
            {
                DashPriorityLevel.Moves.RemoveAt(DashPriorityLevel.Moves.Count - 1);
            }
            EditorGUILayout.EndHorizontal();
            animationMap.MovementAnimationMap.DashMoveLevel = DashPriorityLevel;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.indentLevel--;

        EditorGUI.indentLevel--;
    }

    private void AttackMap()
    {
        EditorGUI.indentLevel++;

        //EditorGUILayout.TextField(tabs[tabIndex].ToString());
        string[] levelNames = Enum.GetNames(typeof(ActionEnum));

        for (int i = ActionAnimationMap.StartingPriorityLevelIndex; i < animationMap.PriorityMap.Length; i++)
        {
            PriorityLevel<AttackMove> priorityLevel = (PriorityLevel<AttackMove>) animationMap.PriorityMap[i];

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

                foreach (AttackMove move in priorityLevel.Moves)
                {
                    EditorGUILayout.Space();
                    move.DirectionalInput = (InputActionReference)EditorGUILayout.ObjectField("Directional Input", move.DirectionalInput, typeof(InputActionReference), false);
                    move.AnimationClipIndexInput = EditorGUILayout.Popup("AnimationClip", move.AnimationClipIndexInput, allClipNames);
                    move.Grounded = EditorGUILayout.Toggle("Grounded", move.Grounded);
                    move.AnimationClip = allClips[move.AnimationClipIndexInput];

                    EditorGUI.indentLevel++;
                    Motion movementAcceration = move.MovementCurve;
                    Motion hitstunAcceration = move.HitstunCurve;

                    AnimationCurve verticalMovementCurve = movementAcceration.VerticalAccerationCurve;
                    AnimationCurve horizontalMovementCurve = movementAcceration.HorizontalAccerationCurve;
                    AnimationCurve verticalHitstunCurve = hitstunAcceration.VerticalAccerationCurve;
                    AnimationCurve horizontalHitstunCurve = hitstunAcceration.HorizontalAccerationCurve;


                    movementAcceration.VerticalAccerationCurve = EditorGUILayout.CurveField("Vertical Movement", verticalMovementCurve);
                    movementAcceration.HorizontalAccerationCurve = EditorGUILayout.CurveField("Horizontal Movement", horizontalMovementCurve);
                    hitstunAcceration.VerticalAccerationCurve = EditorGUILayout.CurveField("Vertical Hitstun", verticalHitstunCurve);
                    hitstunAcceration.HorizontalAccerationCurve = EditorGUILayout.CurveField("Horizontal Hitstun", horizontalHitstunCurve);

                    move.MovementCurve = movementAcceration;
                    move.HitstunCurve = hitstunAcceration;
                    EditorGUI.indentLevel--;
                    EditorGUILayout.HelpBox("", MessageType.None);

                }
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                if (EditorGUILayout.DropdownButton(new GUIContent("Add Move"), FocusType.Passive))
                {
                    priorityLevel.Moves.Add(new AttackMove(i));
                }
                if (EditorGUILayout.DropdownButton(new GUIContent("Remove Move"), FocusType.Passive))
                {
                    priorityLevel.Moves.RemoveAt(priorityLevel.Moves.Count - 1);
                }
                EditorGUILayout.EndHorizontal();

                //Move move = new Move(actionMap.AnimatorController);

                EditorGUI.indentLevel--;
            }

            animationMap.ActionAnimationMap.PriorityLevels[i - ActionAnimationMap.StartingPriorityLevelIndex] = priorityLevel;

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
        }
        
        EditorGUI.indentLevel--;
    }

    private void StateMap()
    {
        //EditorGUILayout.TextField(tabs[tabIndex].ToString());
        StateAnimationMap stateAnimationMap = animationMap.StateAnimationMap;
        int[] inputs = MyAnimationClipEditorDisplayUtil.CreateEmptyIntArray(Enum.GetNames(typeof(StateAnimation)).Length);
        int[] priorityLevelInputs = MyAnimationClipEditorDisplayUtil.CreateEmptyIntArray(Enum.GetNames(typeof(StateAnimation)).Length);
        if (stateAnimationMap.inputs.Length > 0) { inputs = stateAnimationMap.inputs; }
        if (stateAnimationMap.priorityIndexInputs.Length > 0) { priorityLevelInputs = stateAnimationMap.priorityIndexInputs; }


        AnimationClip[] clips = animationMap.AnimatorController.animationClips;
        string[] clipNames = MyAnimationClipEditorDisplayUtil.ConvertClipsToName(clips);

        for (int i = 0; i < animationMap.StateAnimationMap.stateAnimationEnumStrings.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 150;
            priorityLevelInputs[i] = EditorGUILayout.IntField(new GUIContent("Priority Level Index"), priorityLevelInputs[i], GUILayout.Width(180));
            GUILayout.Space(30);
            EditorGUIUtility.labelWidth = 50;
            inputs[i] = EditorGUILayout.Popup(animationMap.StateAnimationMap.stateAnimationEnumStrings[i], inputs[i], clipNames, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
        }
        stateAnimationMap.inputs = inputs;
        stateAnimationMap.priorityIndexInputs = priorityLevelInputs;
        //serializedObject.ApplyModifiedProperties();
    }
}

public class MyAnimationClipEditorDisplayUtil
{
    public static string[] ConvertClipsToName(AnimationClip[] clips)
    {
        string[] clipNames = new string[clips.Length];
        for (int i = 0; i < clips.Length; i++) { clipNames[i] = clips[i].name; }
        return clipNames;
    }

    public static int[] CreateEmptyIntArray(int size)
    {
        int[] intArray = new int[size];
        for (int i = 0; i < size; i++)
        {
            intArray[i] = 0;
        }
        return intArray;
    }
}
