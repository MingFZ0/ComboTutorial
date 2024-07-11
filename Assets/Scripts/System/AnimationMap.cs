using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/AnimationMap")]
public class AnimationMap : ScriptableObject
{
    public RuntimeAnimatorController AnimatorController;
    public ActionAnimationMap ActionAnimationMap = new();
    public StateAnimationMap StateAnimationMap = new();
    public MovementAnimationMap MovementAnimationMap = new();

    public object[] PriorityMap = {
        new PriorityLevel<Move>(0),         //Movement
        new PriorityLevel<DashMove>(1),     //Dash
        new PriorityLevel<AttackMove>(2),   //Light
        new PriorityLevel<AttackMove>(3),   //Medium
        new PriorityLevel<AttackMove>(4),   //Heavy
        new PriorityLevel<AttackMove>(5),   //Unique
    };
    //public object[] PriorityMap = new object[Enum.GetValues(typeof(ActionEnum)).Length];
}


