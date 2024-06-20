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
        protected AnimatorScript playerAnimator;

        public AnimationClip CurrentAction { get; protected set; }
        private int currentCancelLevel;

        private List<string> usedMoves;
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
            if (attackScript.IsAttacking) { result = false; }

            if (result == false) { return false; }

            //Debug.Log(action);
            if (playerAnimator.ChangeAnimation(action)) /*{ currentCancelLevel = CheckPriorityLevel(action); }*/
            CurrentAction = action;

            return true;
        }

        public virtual void ResetAction()
        {
            AnimationClip idle = animationMapping.StateAnimationMap.AnimationMap[StateAnimation.Idle.ToString()];
            AnimationClip falling = animationMapping.StateAnimationMap.AnimationMap[StateAnimation.Falling.ToString()];

            if (playerMovement.IsGrounded() == false) { 

                bool result = Action(falling);
                Debug.Log("Falling " + result);
            }
            else { Action(idle); }
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
            throw new ArgumentOutOfRangeException("Unable to find Priority Level of clip " + clip.name);
        }
    }
}

public class UndefinedMoveName : Exception
{
    public UndefinedMoveName(string message) : base(message) { }
}
