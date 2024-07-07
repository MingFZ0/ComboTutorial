using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(AnimationMap))]
public class AnimationMapEditorScript : Editor
{

    private string[] tabs = { "Movement Map", "Attack Map", "State Map" };
    private int tabIndex = 0;

    public override void OnInspectorGUI()
    {
        AnimationMap animationMap = (AnimationMap)target;
        
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
        
        EditorGUILayout.Separator();
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
    }

    private void MovementMap()
    {
        EditorGUILayout.TextField(tabs[tabIndex].ToString());
    }

    private void AttackMap()
    {
        EditorGUILayout.TextField(tabs[tabIndex].ToString());
    }

    private void StateMap()
    {
        EditorGUILayout.TextField(tabs[tabIndex].ToString());
    }
}
