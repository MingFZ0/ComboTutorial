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
        private string bufferedAction;
        private float bufferedTimer;
        [SerializeField] private float bufferedMemory;

        private void Awake()
        {
            playerAttack = GetComponent<PlayerAttack>();
            playerMovement = GetComponent<PlayerMovement>();
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        private void Update()
        {
            if (bufferedTimer >= 0) { bufferedTimer -= Time.deltaTime; }
        }

        public bool Action(string action)
        {
            bool result = true;
            if (playerAttack.IsAttacking) result = false;
            if (playerMovement.IsDashing && Enum.IsDefined(typeof(Attacks), action)) result = false;
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Attacks), action) && Enum.IsDefined(typeof(Attacks), currentAction)) result = false;
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Movement), action) == true) result = false;

            if (result == false)
            {
                bufferedAction = action;
                bufferedTimer = bufferedMemory;
                return result;
            }

            if (result && action == Movement.Player_Idle.ToString() && bufferedAction != Movement.Player_Idle.ToString() && bufferedTimer >= 0)
            {
                Debug.Log("Replacing Current Action " + action + "With Buffered Action " + bufferedAction);
                Debug.Log(bufferedTimer);
                action = bufferedAction;
                bufferedAction = Movement.Player_Idle.ToString();
                bufferedTimer = -1;
                playerAnimator.ChangeAnimation(action);
                return true;
            }

            playerAnimator.ChangeAnimation(action);
            currentAction = action;
            return result;
        }
    }
}
