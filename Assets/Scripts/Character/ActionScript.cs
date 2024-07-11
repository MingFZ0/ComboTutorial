using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{

    public class ActionScript : MonoBehaviour
    {
        [SerializeField] private AnimationMap animationMapping;

        public ActionAnimationMap ActionAnimationMap { get; private set; }
        public MovementAnimationMap MovementAnimationMap { get; private set; }

        protected PlayerControlsInput playerControlsInput;
        protected AttackScript attackScript;
        protected MovementScript playerMovement;
        public AnimatorScript playerAnimator;

        public AnimationClip CurrentAction { get; protected set; }
        private int currentCancelLevel;

        private List<AnimationClip> usedMoves = new();
        private int currentMovePriorityLevel;

        private Motion currentActionMotion;
        private float _maxcurrentVerticalMotion;
        private float _maxcurrentHorizontalMotion;
        private float _currentActionMotionTimer;


        private void Awake()
        {
            
            attackScript = GetComponent<AttackScript>();
            playerMovement = GetComponent<MovementScript>();
            playerAnimator = GetComponent<AnimatorScript>();


            playerControlsInput = new PlayerControlsInput();
            playerControlsInput.Player.Enable();

        }

        private void Update()
        {
            if (_currentActionMotionTimer >= _maxcurrentHorizontalMotion && _currentActionMotionTimer >= _maxcurrentVerticalMotion) { _currentActionMotionTimer = -1; }

            if (_currentActionMotionTimer >= 0)
            {
                _currentActionMotionTimer += Time.deltaTime;

                RunCurrentActionMotion();

            }
        }

        public virtual bool Action(AnimationClip clip) 
        {
            throw new NotImplementedException("This method is only used for DummyAction");
        }

        public bool Action(AttackMove action)
        {
            if (CheckMoveForAction(action))
            {
                currentActionMotion = action.MovementCurve;

                if (currentActionMotion.HorizontalAccerationCurve.length == 0) { _maxcurrentHorizontalMotion = 0; }
                else { _maxcurrentHorizontalMotion = currentActionMotion.HorizontalAccerationCurve[currentActionMotion.HorizontalAccerationCurve.length - 1].time; }

                if (currentActionMotion.VerticalAccerationCurve.length == 0) { _maxcurrentVerticalMotion = 0; }
                else { _maxcurrentVerticalMotion = currentActionMotion.VerticalAccerationCurve[currentActionMotion.VerticalAccerationCurve.length - 1].time; }
                _currentActionMotionTimer = 0;

                return true;
            }
            else return false;
        }

        public bool CheckMoveForAction(Move action)
        {
            bool result = true;
            int levelIndex = action.PriorityLevelIndex;

            if (levelIndex < currentCancelLevel) { result = false; }
            if (usedMoves.Contains(action.AnimationClip)) { result = false; }
            if (levelIndex >= 2 && currentCancelLevel >= 2 && attackScript.HitBoxCollided == false) { result = false; }
            if (result == false)
            {
                return false;
            }

            //Debug.Log(action);
            if (playerAnimator.ChangeAnimation(action.AnimationClip)) /*{ currentCancelLevel = CheckPriorityLevel(action); }*/
            {
                CurrentAction = action.AnimationClip;
                currentCancelLevel = levelIndex;
                usedMoves.Add(action.AnimationClip);
                if (levelIndex == 0)
                {
                    usedMoves.Clear();
                    currentCancelLevel = 0;
                }
            }
            return true;
        }

        private void RunCurrentActionMotion()
        {
            float horizontalForce;
            float verticalForce; 
            
            if (_maxcurrentHorizontalMotion == 0) { horizontalForce = 0; }
            else if (_currentActionMotionTimer >= _maxcurrentHorizontalMotion) { horizontalForce = currentActionMotion.HorizontalAccerationCurve.Evaluate(_maxcurrentHorizontalMotion); }
            else { horizontalForce = currentActionMotion.HorizontalAccerationCurve.Evaluate(_currentActionMotionTimer); }

            if (_maxcurrentVerticalMotion == 0) { verticalForce = 0; }
            else if (_currentActionMotionTimer >= _maxcurrentVerticalMotion) { verticalForce = currentActionMotion.HorizontalAccerationCurve.Evaluate(_maxcurrentVerticalMotion); }
            else { verticalForce = currentActionMotion.VerticalAccerationCurve.Evaluate(_currentActionMotionTimer); }

            playerMovement.MoveCharacter(horizontalForce, verticalForce);
        }

        public virtual void ResetAction()
        {
            Debug.Log("Reset Action");
            AnimationClip idle = animationMapping.StateAnimationMap.StateStringToAnimationMap[StateAnimation.Idle.ToString()];
            AnimationClip falling = animationMapping.StateAnimationMap.StateStringToAnimationMap[StateAnimation.Falling.ToString()];
            AnimationClip action;

            if (playerMovement.IsGroundedWithJumping() == false ) 
            {
                action = falling;
            }
            else { 
                action = idle;
                playerMovement.SetJumpingFalse();
            }

            Debug.Log("Switching from " + CurrentAction + " to " + action);
            CurrentAction = action;
            currentCancelLevel = 0;
            //Action(action);
        }

        public object[] GetPriorityLevelsRaw()
        {
            return animationMapping.PriorityMap;
        }
    }
}

public class UndefinedMoveName : Exception
{
    public UndefinedMoveName(string message) : base(message) { }
}
