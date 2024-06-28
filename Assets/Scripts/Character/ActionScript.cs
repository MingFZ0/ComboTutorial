using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{

    public class ActionScript : MonoBehaviour
    {
        [SerializeField] private AnimationMapping animationMapping;

        public Dictionary<int, PriorityLevel> MovesetPriorityMap { get; private set; }

        protected PlayerControlsInput playerControlsInput;
        protected AttackScript attackScript;
        protected MovementScript playerMovement;
        public AnimatorScript playerAnimator;

        public AnimationClip CurrentAction { get; protected set; }
        private int currentCancelLevel;

        private List<AnimationClip> usedMoves = new();
        private int currentMovePriorityLevel;


        private void Awake()
        {
            
            attackScript = GetComponent<AttackScript>();
            playerMovement = GetComponent<MovementScript>();
            playerAnimator = GetComponent<AnimatorScript>();


            playerControlsInput = new PlayerControlsInput();
            playerControlsInput.Player.Enable();

            if (animationMapping != null) { MovesetPriorityMap = animationMapping.ActionAnimationMap.ActionPriorityMap; }
        }

        public virtual bool Action(AnimationClip action)
        {
            bool result = true;
            int levelIndex = FindPriorityLevel(action);

            if (levelIndex < currentCancelLevel) { result = false; }
            if (usedMoves.Contains(action)) { result = false; }
            if (levelIndex >= 2 && currentCancelLevel >= 2 && attackScript.HitBoxCollided == false) { result = false; }
            if (result == false) {
                return false; 
            }

            //Debug.Log(action);
            if (playerAnimator.ChangeAnimation(action)) /*{ currentCancelLevel = CheckPriorityLevel(action); }*/
            {
                CurrentAction = action;
                currentCancelLevel = levelIndex;
                usedMoves.Add(action);
                if (levelIndex == 0) { 
                    usedMoves.Clear();
                    currentCancelLevel = 0;
                }
            }
            

            return true;
        }

        public virtual void ResetAction()
        {
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
            Action(action);
        }

        public int FindPriorityLevel(AnimationClip clip)
        {
            foreach (int levelIndex in MovesetPriorityMap.Keys)
            {
                PriorityLevel level = MovesetPriorityMap[levelIndex];
                foreach (Move move in level.Moves)
                {
                    if (move.AnimationClip == clip) { return levelIndex; }
                }
            }

            foreach (AnimationClip animation in animationMapping.StateAnimationMap.AnimationToPriorityIndexMap.Keys)
            {
                if (animation == clip) { return animationMapping.StateAnimationMap.AnimationToPriorityIndexMap[animation]; }
            }
            Debug.Log(clip.name);
            throw new ArgumentOutOfRangeException("Unable to find Priority Level of clip " + clip.name);
        }
    }
}

public class UndefinedMoveName : Exception
{
    public UndefinedMoveName(string message) : base(message) { }
}
