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
    private PriorityLevel<AttackMove>[] priorityLevels;
    private StateScript stateScript;

    //Fields
    public bool IsAttacking;
    public bool HitBoxCollided;

    void Awake()
    {
        actionScript = GetComponent<ActionScript>();
        stateScript = GetComponent<StateScript>();
    }

    private void Start()
    {
        priorityLevels = actionScript.AttackAnimationMap.PriorityLevels;
    }

    private void Update()
    {
        for (int i = 0; i < priorityLevels.Length; i++)
        {
            PriorityLevel<AttackMove> attackPriorityLevel = priorityLevels[i];
            InputActionReference movesetLevelInput = attackPriorityLevel.LevelInput;

            if (movesetLevelInput == null) { throw new MissingReferenceException("Unsure what attack button to look for within the current attack priority level of " + attackPriorityLevel.PriorityLevelIndex); }
            else if (movesetLevelInput.action.WasPressedThisFrame())
            {
                foreach (AttackMove move in attackPriorityLevel.Moves)
                {
                    //Debug.Log(i + " level was pressed");
                    if ((move.DirectionalInput == null  || move.DirectionalInput.action.IsPressed()) && stateScript.IsGrounded() == move.Grounded) 
                    {
                        Debug.Log(move + " should be ran pressed");
                        if (actionScript.Action(move))
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
    public void OnHitBoxExit() { HitBoxCollided=false; }
}
