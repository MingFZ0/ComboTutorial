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
        private Dictionary<int, MovesetPriorityLevel> movesetPriorityMap;

        private PlayerAttack playerAttack;
        private PlayerMovement playerMovement;
        private PlayerAnimator playerAnimator;

        private string currentAction;
        private int currentCancelLevel;

        private List<string> usedMoves;
        private int currentMovePriorityLevel;


        private void Awake()
        {
            movesetPriorityMap = movesetMap.MovesetPriorityMap;
            playerAttack = GetComponent<PlayerAttack>();
            playerMovement = GetComponent<PlayerMovement>();
            playerAnimator = GetComponent<PlayerAnimator>(); 
        }

        public bool Action(string action)
        {
            //bool result = true;
            //if (playerAttack.IsAttacking) { result = false; }
            //if (playerMovement.IsDashing && Enum.IsDefined(typeof(Attacks), action)) { result = false; }
            //if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Attacks), action) && Enum.IsDefined(typeof(Attacks), currentAction)) { result = false; }
            //if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Movement), action) == true) { result = false; }
            //if (playerAnimator.IsResettingAnimation) { result = false; }
            //if (result == false) { return false; }

            Debug.Log(action);
            playerAnimator.ChangeAnimation(action);
            currentAction = action;

            return true;
        }

        private int CheckPriorityLevel(string moveName)
        {
            foreach (int priorityLevel in movesetPriorityMap.Keys)
            {
                MovesetPriorityLevel level = movesetPriorityMap[priorityLevel];
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
