using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class PlayerScript : MonoBehaviour
    {
        private PlayerAttack playerAttack;
        private PlayerMovement playerMovement;
        private PlayerAnimator playerAnimator;

        private string currentAction;

        private void Awake()
        {
            playerAttack = GetComponent<PlayerAttack>();
            playerMovement = GetComponent<PlayerMovement>();
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        public bool Action(string action)
        {
            if (playerAttack.IsAttacking) { return false; }
            if (playerMovement.IsDashing && Enum.IsDefined(typeof(Attacks), action)) return false;
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Attacks), action) && Enum.IsDefined(typeof(Attacks), currentAction)) return false;
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Movement), action) == true) return false;

            playerAnimator.ChangeAnimation(action);
            currentAction = action;
            return true;
        }
    }
}
