using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StateScript : MonoBehaviour
{
    public int LandingFrames;
    private int currentFrame;
    private bool landed;
    [SerializeField] private StateAnimationMap stateMap;
    
    private MovementScript movementScript;
    private AttackScript attackScript;
    private ActionScript actionScript;

    private bool currentGroundedState;

    private Dictionary<string, AnimationClip> stateAnimationMap = new();
    //private Move idle;
    //private Move falling;
    //private Move landing;

    private void Awake()
    {
        movementScript = GetComponent<MovementScript>();
        actionScript = GetComponent<ActionScript>();
        attackScript = GetComponent<AttackScript>();
        stateAnimationMap = stateMap.StateStringToAnimationMap;
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
            if (IsGrounded() && LandingFrames == 0 && !movementScript.isJumping)
            {
                
                actionScript.Action(stateAnimationMap[StateAnimation.Idle.ToString()]);
            }
            //else if (IsGrounded() && LandingFrames > 0)
            //{
            //    StartLandingRecovery();
            //}
        }

        //else if (actionScript.CurrentAction == stateAnimationMap[StateAnimation.Falling.ToString()] && IsGrounded())
        //{
        //    StartLandingRecovery();
        //}

        //else if (attackScript.IsAttacking && actionScript.CurrentAction.name == StateAnimation.Falling.ToString() && IsGrounded())
        //{
        //    StartLandingRecovery();
        //}
    }

    public void SetLandingFrame(int frame) { this.LandingFrames = frame; }

    private void StartLandingRecovery()
    {
        landed = true;
        actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()]);
    }

    public Boolean IsGrounded() { return movementScript.IsGroundedWithJumping(); }

    private void FixedUpdate()
    {
        //if (landed && LandingFrames > 0)
        //{
        //    if (currentFrame >= LandingFrames)
        //    {
        //        actionScript.ResetAction();
        //        landed = false;
        //        LandingFrames = 0;
        //        currentFrame = 0;
        //        //Debug.Log("Action was reset");
        //    }
        //    else
        //    {
        //        actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()]);
        //        currentFrame++;
        //    }
            
        //}
    }
}
