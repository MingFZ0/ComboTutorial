using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAction : ActionScript
{
    public int MoveInputInt;
    public RuntimeAnimatorController AnimatorController;
    public AnimationClip SelectedMove;

    private void Update()
    {
        //if (Action(SelectedMove.name) == false && CurrentAction == "Idle")
        //{
        //    Action("Idle");
        //};
        //else { Action("Idle"); }
        Action(SelectedMove.name);
    }
}
