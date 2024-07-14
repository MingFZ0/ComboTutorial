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
    
    private MovementScript movementScript;
    private AttackScript attackScript;
    private ActionScript actionScript;

    private bool currentGroundedState;

    private Dictionary<string, StateMove> stateAnimationMap = new();


    private WaitForSeconds waitForAFrame = new WaitForSeconds(0.0133f);
    private Coroutine _landingRecoveryCoroutine;

    private void Awake()
    {
        movementScript = GetComponent<MovementScript>();
        actionScript = GetComponent<ActionScript>();
        attackScript = GetComponent<AttackScript>();
        stateAnimationMap = actionScript.StateAnimationMap.StateStringToAnimationMap;
    }

    private void Start()
    {
        Debug.Log(stateAnimationMap.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (actionScript.MovementAnimationMap.MovementMoveLevel.LevelInput.action.IsPressed() == false 
            && attackScript.IsAttacking == false 
            && !movementScript.isJumping
            && LandingFrames == 0
            && IsGrounded())
        {
            actionScript.Action(stateAnimationMap[StateAnimation.Idle.ToString()]);
        }
        else if (IsGrounded() && LandingFrames > 0 && landed == false)
        {
            StartLandingRecovery();
        }
    }

    public void SetLandingFrame(int frame) { this.LandingFrames = frame; }

    private void StartLandingRecovery()
    {
        if (_landingRecoveryCoroutine == null) { _landingRecoveryCoroutine = StartCoroutine(LandingRecoveryCalculation()); }
        else 
        { 
            StopCoroutine(_landingRecoveryCoroutine);
            _landingRecoveryCoroutine = StartCoroutine(LandingRecoveryCalculation());
        }
    }

    public Boolean IsGrounded() { return movementScript.IsGroundedWithJumping(); }

    private IEnumerator LandingRecoveryCalculation()
    {

        if (currentFrame >= LandingFrames) 
        {
            actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()]);
            actionScript.ResetAction();
            yield break; 
        }

        else
        {
            landed = true;
            actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()]);
            currentFrame++;
            yield return waitForAFrame;

            while (currentFrame < LandingFrames)
            {
                currentFrame++;
                yield return waitForAFrame;
            }

            actionScript.ResetAction();
            landed = false;
            LandingFrames = 0;
            currentFrame = 0;
            yield break;
        }
    }

    //private void FixedUpdate()
    //{
    //    if (landed && LandingFrames > 0)
    //    {
    //        if (currentFrame >= LandingFrames)
    //        {
    //            actionScript.ResetAction();
    //            landed = false;
    //            LandingFrames = 0;
    //            currentFrame = 0;
    //            //Debug.Log("Action was reset");
    //        }
    //        else
    //        {
    //            actionScript.Action(stateAnimationMap[StateAnimation.Landing.ToString()]);
    //            currentFrame++;
    //        }

    //    }
    //}
}
