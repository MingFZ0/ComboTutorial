using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAction : ActionScript
{
    private AnimatorScript _animatorScript;

    public int MoveInputInt;
    public RuntimeAnimatorController AnimatorController;
    public AnimationClip SelectedMove;
    public AnimationMap dummyAnimationMapping;

    //private void Awake()
    //{
    //    _animatorScript = GetComponent<AnimatorScript>();
    //}

    private void Start()
    {
        
    }

    private void Update()
    {
        //if (Action(SelectedMove.name) == false && CurrentAction == "Idle")
        //{
        //    Action("Idle");
        //};
        //else { Action("Idle"); }
        this.Action(SelectedMove);
    }

    public override bool Action(AnimationClip action)
    {
        bool result = base.playerAnimator.ChangeAnimation(action);
        //base.CurrentAction = action;

        return result;
    }

    public override void ResetAction()
    {
        StateMove idle = dummyAnimationMapping.StateAnimationMap.StateStringToAnimationMap[StateAnimation.Idle.ToString()];
        StateMove falling = dummyAnimationMapping.StateAnimationMap.StateStringToAnimationMap[StateAnimation.Falling.ToString()];

        Action(idle);
    }
}
