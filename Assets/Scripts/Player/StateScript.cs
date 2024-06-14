using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScript : MonoBehaviour
{
    [SerializeField] private StateMap stateMap;
    
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
        //stateAnimationMap = stateMap.StateAnimationMap;

        //Debug.Log(stateAnimationMap == null);
    }

    private void Start()
    {
        stateAnimationMap = stateMap.StateAnimationMap;
        Debug.Log(stateMap.StateAnimationMap.Keys.Count);

        foreach (string key in stateMap.StateAnimationMap.Keys)
        {
            Debug.Log(key);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (actionScript.MovesetPriorityMap[0].LevelInput.action.IsPressed() == false && attackScript.IsAttacking == false)
        //{
        //    if (IsGrounded())
        //    {
        //        actionScript.Action(stateAnimationMap[StateAnimation.Idle.ToString()].ToString());
        //    }
        //    else
        //    {
        //        //State = CharacterState.Falling;
        //        actionScript.Action(stateAnimationMap[StateAnimation.Falling.ToString()].ToString());
        //    }
        //}

        //if (actionScript.CurrentAction == stateAnimationMap[StateAnimation.Falling.ToString()].ToString() && IsGrounded())
        //{
        //    actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()].ToString());
        //}

        //if (attackScript.IsAttacking && actionScript.CurrentAction == StateAnimation.Falling.ToString() && IsGrounded())
        //{
        //    //State = CharacterState.Landing;
        //    actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()].ToString());
        //}

        foreach (string key in stateMap.StateAnimationMap.Keys)
        {
            Debug.Log(key);
        }
    }

    public Boolean IsGrounded() { return movementScript.IsGrounded(); }
}
