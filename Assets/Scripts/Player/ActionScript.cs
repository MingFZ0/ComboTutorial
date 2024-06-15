using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{

    public class ActionScript : MonoBehaviour
    {
        [SerializeField] private MovesetMap movesetMap;
        public Dictionary<int, MovesetPriorityLevel> MovesetPriorityMap { get; private set; }

        protected PlayerControlsInput playerControlsInput;
        protected AttackScript attackScript;
        protected MovementScript playerMovement;
        protected AnimatorScript playerAnimator;

        public string CurrentAction { get; protected set; }
        private int currentCancelLevel;

        private List<string> usedMoves;
        private int currentMovePriorityLevel;


        private void Awake()
        {
            MovesetPriorityMap = movesetMap.MovesetPriorityMap;
            attackScript = GetComponent<AttackScript>();
            playerMovement = GetComponent<MovementScript>();
            playerAnimator = GetComponent<AnimatorScript>();


            playerControlsInput = new PlayerControlsInput();
            playerControlsInput.Player.Enable();
        }

        public virtual bool Action(string action)
        {
            bool result = true;
            if (attackScript.IsAttacking) { result = false; }
            //if (playerMovement.IsDashing && Enum.IsDefined(typeof(Attacks), action)) { result = false; }
            //if (attackScript.IsAttacking == true && Enum.IsDefined(typeof(Attacks), action) && Enum.IsDefined(typeof(Attacks), CurrentAction)) { result = false; }
            //if (attackScript.IsAttacking == true && Enum.IsDefined(typeof(Movement), action) == true) { result = false; }
            //if (playerAnimator.IsResettingAnimation) { result = false; }
            if (result == false) { return false; }

            //Debug.Log(action);
            if (playerAnimator.ChangeAnimation(action)) { currentCancelLevel = CheckPriorityLevel(action); }
            CurrentAction = action;

            return true;
        }

        //public virtual bool Action(Move action)
        //{
        //    bool result = true;
        //    if (attackScript.IsAttacking) { result = false; }
        //    //if (playerMovement.IsDashing && Enum.IsDefined(typeof(Attacks), action)) { result = false; }
        //    //if (attackScript.IsAttacking == true && Enum.IsDefined(typeof(Attacks), action) && Enum.IsDefined(typeof(Attacks), CurrentAction)) { result = false; }
        //    //if (attackScript.IsAttacking == true && Enum.IsDefined(typeof(Movement), action) == true) { result = false; }
        //    //if (playerAnimator.IsResettingAnimation) { result = false; }
        //    if (result == false) { return false; }

        //    //Debug.Log(action);
        //    if (playerAnimator.ChangeAnimation(action)) { currentCancelLevel = CheckPriorityLevel(action); }
        //    CurrentAction = action;

        //    return true;
        //}

        private void Update()
        {
            
        }

        private int CheckPriorityLevel(string moveName)
        {
            foreach (int priorityLevel in MovesetPriorityMap.Keys)
            {
                MovesetPriorityLevel level = MovesetPriorityMap[priorityLevel];
                if (level.Contains(moveName)) return priorityLevel;
            }
            throw new UndefinedMoveName(gameObject.name + ": Unable to find the move name of " + moveName + " within the provided movesetMap of " + movesetMap.name);
        }
    }
}

public class UndefinedMoveName : Exception
{
    public UndefinedMoveName(string message) : base(message) { }
}
