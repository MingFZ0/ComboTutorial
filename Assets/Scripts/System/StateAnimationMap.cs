using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/StateMap")]
public class StateAnimationMap : ScriptableObject
{
    public Dictionary<string, AnimationClip> AnimationMap;
    public RuntimeAnimatorController AnimatorController;
    public AnimationClip[] StateAnimations = new AnimationClip[Enum.GetValues(typeof(StateAnimation)).Length];
    
    public readonly string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));
    public int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];


    private void OnValidate()
    {
        if (AnimatorController != null)
        {
            AnimationMap = new();
            for (int i = 0; i < inputs.Length; i++)
            {
                AnimationMap.Add(stateAnimationEnumStrings[i], AnimatorController.animationClips[inputs[i]]);
            }
        }
        

        //Debug.Log("Updated Dictionary");

        //foreach (string key in StateAnimationMap.Keys)
        //{
        //    Debug.Log(key + ": " + StateAnimationMap[key].name);
        //}

        //Debug.Log(inputs[0] + ", " + inputs[1] + ", " + inputs[2]);
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