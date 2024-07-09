using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Linq;

[Serializable]
public class ActionAnimationMap
{
    public static readonly int StartingPriorityLevelIndex = 2;
    public List<ActionMapInput> ActionMapInput;
    public PriorityLevel<AttackMove>[] PriorityLevels;
    public Dictionary<int, PriorityLevel<AttackMove>> ActionPriorityMap = new();

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
    [SerializeField] public PriorityLevel<AttackMove> PriorityLevel;
    [SerializeField] public int PriorityIndex;
}

[Serializable]
public class PriorityLevel<MoveType>
{
    [SerializeField] public InputActionReference LevelInput;
    [SerializeField] public List<MoveType> Moves = new();
    public bool Fold;

    public PriorityLevel() { }
}

[Serializable]
public class AttackMove : Move
{
    [SerializeField] public Motion MovementCurve;
    [SerializeField] public Motion HitstunCurve;
}

[Serializable]
public class Move
{
    [SerializeField] public InputActionReference DirectionalInput;
    [SerializeField] public AnimationClip AnimationClip;
    [SerializeField] public bool Grounded;
    public int AnimationClipIndexInput;

    public Move()
    {

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
