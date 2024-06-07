using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Animations;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Characters/CancelLevels")]
public class MoveSet : ScriptableObject
{
    [SerializeField] public AnimatorController controller;

    [SerializeField] private MoveSetLevel Movement = new MoveSetLevel(0);
    [SerializeField] private MoveSetLevel Dash = new MoveSetLevel(1);
    [SerializeField] private MoveSetLevel Light = new MoveSetLevel(2);
    [SerializeField] private MoveSetLevel Medium = new MoveSetLevel(3);
    [SerializeField] private MoveSetLevel HeavyAndUnique = new MoveSetLevel(4);
    public Dictionary<int, List<string>> Moves;

    private void OnValidate()
    {
        Moves = ToDictionary();
        AnimationClip[] clips = controller.animationClips;
        foreach (AnimationClip clip in clips)
        {
            Debug.Log(clip.name);
        }
    }

    public Dictionary<int, List<string>> ToDictionary()
    {
        Dictionary<int, List<string>> dict = new Dictionary<int, List<string>>();
        MoveSetLevel[] moveSet = { Movement, Dash, Light, Medium };

        if (moveSet == null) { return null; }
        foreach (MoveSetLevel moveSetLevel in moveSet)
        {
            dict.Add(moveSetLevel.Level, moveSetLevel.Moves);
        }
        return dict;
    }

}

[Serializable]
public class MoveSetLevel
{
     public MoveSetLevel(int level) { this.Level = level; }

    [HideInInspector] public int Level { get; private set; }
    [SerializeField] public List<string> Moves;
}


