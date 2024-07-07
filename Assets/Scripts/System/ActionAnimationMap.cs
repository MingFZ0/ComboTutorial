using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Linq;

[Serializable]
public class ActionAnimationMap
{
    public RuntimeAnimatorController AnimatorController;
    public List<ActionMapInput> ActionMapInput;
    public PriorityLevel[] PriorityLevels;
    public Dictionary<int, PriorityLevel> ActionPriorityMap = new();

    public void UpdatePriorityMap()
    {
        ActionPriorityMap.Clear();

        for (int i = 0; i < PriorityLevels.Length; i++)
        {
            ActionPriorityMap[i] = PriorityLevels[i];
        }
    }
}

public struct ActionMapInput 
{
    [SerializeField] public PriorityLevel PriorityLevel;
    [SerializeField] public int PriorityIndex;
}

[Serializable]
public class PriorityLevel
{
    [SerializeField] public InputActionReference LevelInput;
    [SerializeField] public List<Move> Moves = new();
    public bool Fold;

    public PriorityLevel() { }
}

[Serializable]
public class Move
{
    public RuntimeAnimatorController AnimatorController;
    [SerializeField] public InputActionReference DirectionalInput;
    [SerializeField] public AnimationClip AnimationClip;
    [SerializeField] public bool Grounded;
    [SerializeField] public Motion MovementAcceration;
    public int AnimationClipIndexInput;

    public Move(RuntimeAnimatorController _animatorController) { 
        this.AnimatorController = _animatorController;
    }

    public Move(InputActionReference directionalInput, AnimationClip animationClip, bool grounded)
    {
        DirectionalInput = directionalInput;
        AnimationClip = animationClip;
        Grounded = grounded;
    }

    public override string ToString()
    {
        return AnimationClip.name;
    }
}

[Serializable]
public class Motion
{
    [SerializeField] public AnimationCurve VerticalAccerationCurve;
    [SerializeField] public AnimationCurve HorizontalAccerationCurve;
}

public enum ActionEnum
{
    Movement,
    Dash,
    Light,
    Medium,
    Heavy,
    Unique
}
