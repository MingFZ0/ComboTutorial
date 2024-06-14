using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/StateMap")]
public class StateMap : ScriptableObject
{
    public Dictionary<string, AnimationClip> StateAnimationMap { get; private set; }
    public RuntimeAnimatorController AnimatorController;
    public AnimationClip[] StateAnimations = new AnimationClip[Enum.GetValues(typeof(StateAnimation)).Length];
    
    public readonly string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));
    public int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];

    public void SetStateAnimationMap(Dictionary<string, AnimationClip> dict)
    {
        this.StateAnimationMap = dict;
    }

    private void OnValidate()
    {
        //Debug.Log(StateAnimationMap.Values.Count);
    }

}


public enum StateAnimation
{
    Idle,
    Falling,
    Landing,
}