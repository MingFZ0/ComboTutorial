using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScript : MonoBehaviour
{
    [SerializeField] private StateAnimationMap stateMap;
    
    private MovementScript movementScript;
    private AttackScript attackScript;
    private ActionScript actionScript;

    private Dictionary<string, AnimationClip> stateAnimationMap = new();
    //private Move idle;
    //private Move falling;
    //private Move landing;

    private void Awake()
    {
        movementScript = GetComponent<MovementScript>();
        actionScript = GetComponent<ActionScript>();
        attackScript = GetComponent<AttackScript>();
        stateAnimationMap = stateMap.AnimationMap;
    }

    private void Start()
    {
        //foreach (string key in stateMap.StateAnimationMap.Keys)
        //{
        //    Debug.Log(key + ": " + stateMap.StateAnimationMap[key].name);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (actionScript.MovesetPriorityMap[0].LevelInput.action.IsPressed() == false && attackScript.IsAttacking == false)
        {
            if (IsGrounded())
            {
              
                actionScript.Action(stateAnimationMap[StateAnimation.Idle.ToString()]);
            }
            else
            {
                //State = CharacterState.Falling;
                actionScript.Action(stateAnimationMap[StateAnimation.Falling.ToString()]);
            }
        }

        if (actionScript.CurrentAction == stateAnimationMap[StateAnimation.Falling.ToString()] && IsGrounded())
        {
            actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()]);
        }

        if (attackScript.IsAttacking && actionScript.CurrentAction.name == StateAnimation.Falling.ToString() && IsGrounded())
        {
            //State = CharacterState.Landing;
            actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()]);
        }
    }

    public Boolean IsGrounded() { return movementScript.IsGrounded(); }
}
