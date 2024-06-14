using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScript : MonoBehaviour
{
    private MovementScript movementScript;
    private AttackScript attackScript;
    private ActionScript actionScript;

    public CharacterState State { get; private set; }

    private Move idle;
    private Move falling;
    private Move landing;

    public enum CharacterState
    {
        Falling,
        Landing
    }

    private void Awake()
    {
        movementScript = GetComponent<MovementScript>();
        actionScript = GetComponent<ActionScript>();
        attackScript = GetComponent<AttackScript>();
    }

    private void Start()
    {
        foreach (Move move in actionScript.MovesetPriorityMap[-1].Moves)
        {
            if (move.MoveName == "Idle") { idle = move; }
            if (move.MoveName == "Falling") { falling = move; }
            if (move.MoveName == "Landing") { landing = move; }
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        if (actionScript.MovesetPriorityMap[0].LevelInput.action.IsPressed() == false && attackScript.IsAttacking == false)
        {
            if (IsGrounded()) 
            { 
                actionScript.Action(idle.ToString()); 
            }
            else 
            {  
                State = CharacterState.Falling;
                actionScript.Action(falling.ToString()); 
            }
        }

        if (attackScript.IsAttacking && State == CharacterState.Falling && IsGrounded())
        {
            State = CharacterState.Landing;
            actionScript.Action(landing.ToString());
        }
    }

    public Boolean IsGrounded() { return movementScript.IsGrounded(); }
}
