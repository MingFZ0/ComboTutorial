using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;
using JetBrains.Annotations;

[Serializable]
public class ActionAnimationMap
{
    public static readonly int StartingPriorityLevelIndex = 2;
    [SerializeField] public PriorityLevel<AttackMove>[] PriorityLevels =
    {
        new PriorityLevel<AttackMove>(2),
        new PriorityLevel<AttackMove>(3),
        new PriorityLevel<AttackMove>(4),
        new PriorityLevel<AttackMove>(5),
    };
}

[Serializable]
public class MovementAnimationMap
{
    public static readonly int StartingPriorityLevelIndex = 0;
    public static readonly int EndingPriorityLevelIndex = 1;
    public PriorityLevel<Move> MovementMoveLevel = new(0);
    public PriorityLevel<DashMove> DashMoveLevel = new(1);
}

[Serializable]
public class StateAnimationMap
{
    public Dictionary<string, AnimationClip> StateStringToAnimationMap;
    public Dictionary<AnimationClip, int> AnimationToPriorityIndexMap;
    public RuntimeAnimatorController AnimatorController;
    public AnimationClip[] StateAnimations = new AnimationClip[Enum.GetValues(typeof(StateAnimation)).Length];

    public readonly string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));
    public int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];
    public int[] priorityIndexInputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];


    public void UpdateDictMap()
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


[Serializable]
public class PriorityLevel<MoveType>
{
    [SerializeField] public InputActionReference LevelInput;
    public readonly int PriorityLevelIndex;
    [SerializeField] public List<MoveType> Moves = new();

    public bool Fold;

    public PriorityLevel(int PriorityLevelIndex) { this.PriorityLevelIndex = PriorityLevelIndex; }
}

[Serializable]
public class AttackMove : Move
{
    [SerializeField] public Motion MovementCurve = new();
    [SerializeField] public Motion HitstunCurve = new();

    public AttackMove(int priorityLevelIndex) : base(priorityLevelIndex) { }
    public AttackMove(Move move) : base(move.PriorityLevelIndex) 
    {
        this.DirectionalInput = move.DirectionalInput;
        this.AnimationClip = move.AnimationClip;
        this.Grounded = move.Grounded;
        this.AnimationClipIndexInput = move.AnimationClipIndexInput;

    }
}

[Serializable]
public class DashMove : Move
{
    [SerializeField] public Motion MovementCurve = new();
    public DashMove(int priorityLevelIndex) : base(priorityLevelIndex) { }
}

[Serializable]
public class Move
{
    [SerializeField] public InputActionReference DirectionalInput;
    [SerializeField] public AnimationClip AnimationClip;
    [SerializeField] public bool Grounded;
    public readonly int PriorityLevelIndex;
    public int AnimationClipIndexInput;

    public Move(int priorityLevelIndex)
    {
        PriorityLevelIndex = priorityLevelIndex;
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
    [SerializeField] public AnimationCurve VerticalAccerationCurve = new();
    [SerializeField] public AnimationCurve HorizontalAccerationCurve = new();
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

public enum StateAnimation
{
    Idle,
    Falling,
    Landing,
}