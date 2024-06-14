using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/StateMap")]
public class StateMap : ScriptableObject
{

    [SerializeReference] public Dictionary<string, AnimationClip> StateAnimationMap = new();
    public RuntimeAnimatorController AnimatorController;
    public AnimationClip[] StateAnimations = new AnimationClip[Enum.GetValues(typeof(StateAnimation)).Length];
    
    public readonly string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));
    public int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];
}


public enum StateAnimation
{
    Idle,
    Falling,
    Landing,
}