using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;
using JetBrains.Annotations;

[Serializable]
public class AttackAnimationMap
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
    public Dictionary<string, StateMove> StateStringToAnimationMap;
    public Dictionary<StateMove, int> AnimationToPriorityIndexMap;
    //private RuntimeAnimatorController animatorController;
    public StateMove[] StateMoves = new StateMove[Enum.GetValues(typeof(StateAnimation)).Length];

    public readonly string[] stateAnimationEnumStrings = Enum.GetNames(typeof(StateAnimation));
    public int[] inputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];
    public int[] priorityIndexInputs = new int[Enum.GetNames(typeof(StateAnimation)).Length];

    //public void UpdateAnimatorController(RuntimeAnimatorController animatorController) { this.animatorController = animatorController; }

    public void UpdateDictMap()
    {
        this.StateStringToAnimationMap = new();
        this.AnimationToPriorityIndexMap = new();
        for (int i = 0; i < inputs.Length; i++)
        {
            this.StateStringToAnimationMap.Add(stateAnimationEnumStrings[i], StateMoves[i]);
            this.AnimationToPriorityIndexMap.Add(StateMoves[i], priorityIndexInputs[i]);
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
public class StateMove : Move
{
    public StateMove(int priorityLevelIndex) : base (priorityLevelIndex) { }

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
    public int PriorityLevelIndex;
    public int AnimationClipIndexInput;

    public Move(int priorityLevelIndex)
    {
        PriorityLevelIndex = priorityLevelIndex;
    }

    public Move(InputActionReference directionalInput, AnimationClip animationClip, bool grounded, int priorityLevelIndex)
    {
        DirectionalInput = directionalInput;
        AnimationClip = animationClip;
        Grounded = grounded;
        this.PriorityLevelIndex = priorityLevelIndex;
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