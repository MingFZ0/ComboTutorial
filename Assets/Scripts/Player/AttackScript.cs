using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackScript : MonoBehaviour
{
    //Script References
    private ActionScript actionScript;
    private Dictionary<int, MovesetPriorityLevel> movesetPriorityMap;
    private StateScript stateScript;

    //Fields
    public bool IsAttacking;
    public Boolean IsCancelable;
    public Boolean HitBoxCollided { get; private set; }

    void Awake()
    {
        actionScript = GetComponent<ActionScript>();
        stateScript = GetComponent<StateScript>();
    }

    private void Start()
    {
        movesetPriorityMap = actionScript.MovesetPriorityMap;
    }

    private void Update()
    {
        for (int i = 2; i < 6; i++)
        {
            MovesetPriorityLevel movesetPriorityLevel = movesetPriorityMap[i];
            InputActionReference movesetLevelInput = movesetPriorityLevel.LevelInput;

            if (movesetLevelInput.action.WasPressedThisFrame())
            {
                foreach (Move move in movesetPriorityLevel.Moves)
                {
                    //Debug.Log(i + " level was pressed");
                    if (move.DirectionalInput.action.IsPressed() && stateScript.IsGrounded() == move.Grounded) 
                    {
                        Debug.Log(move + " should be ran pressed");
                        if (actionScript.Action(move.MoveName)) 
                        {
                            //IsAttacking = true;
                        }
                        return;
                    }
                }
            }
        }
    }

    public void OnHitBoxCollide() { HitBoxCollided = true; }
    public void OnHitBoxExit() {HitBoxCollided=false; }
}
