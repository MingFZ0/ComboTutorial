using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/StateMap")]
public class StateAnimationMap : ScriptableObject
{
    public Dictionary<string, AnimationClip> StateStringToAnimationMap;
    public Dictionary<AnimationClip, int> AnimationToPriorityIndexMap;
    public RuntimeAnimatorController AnimatorController;
    public AnimationClip[] StateAnimations = new AnimationClip[Enum.GetValues(typeof(StateAnimation)).Length];
    
    public readonly string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));
    public int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];
    public int[] priorityIndexInputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];


    private void OnValidate()
    {
        if (AnimatorController != null)
        {
            StateStringToAnimationMap = new();
            AnimationToPriorityIndexMap = new();
            for (int i = 0; i < inputs.Length; i++)
            {
                StateStringToAnimationMap.Add(stateAnimationEnumStrings[i], AnimatorController.animationClips[inputs[i]]);
                AnimationToPriorityIndexMap.Add(AnimatorController.animationClips[inputs[i]], priorityIndexInputs[i]);
            }
        }
    }
}

[Serializable]
public class StateAnimationKeyPair
{
    [SerializeField] public string Key;
    [SerializeField] public AnimationClip Value;

    public StateAnimationKeyPair(string key, AnimationClip value)
    {
        this.Key = key;
        this.Value = value;
    }
}


public enum StateAnimation
{
    Idle,
    Falling,
    Landing,
}