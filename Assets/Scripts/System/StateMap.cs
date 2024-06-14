using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/StateMap")]
public class StateMap : ScriptableObject
{
    public Dictionary<string, AnimationClip> StateAnimationMap { get; private set; }
    public RuntimeAnimatorController AnimatorController;
    public AnimationClip[] StateAnimations;

    private void OnValidate()
    {
        StateAnimationMap = new Dictionary<string, AnimationClip>();
    }

}


public enum StateAnimation
{
    Idle,
    Falling,
    Landing,
}