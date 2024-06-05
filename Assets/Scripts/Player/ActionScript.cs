using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{

    public class ActionScript : MonoBehaviour
    {
        private PlayerAttack playerAttack;
        private PlayerMovement playerMovement;
        private PlayerAnimator playerAnimator;

        private string currentAction;
        private string bufferedAction;
        private float bufferedTimer;

        private bool isCancelable;

        public bool IsBlocking ;

        public bool IsAttacking;

        public float Health;

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


        public void OnHitBoxCollide()
        {
            Debug.Log("hit");
            isCancelable = true;
        }

        public void OnHitBoxExit()
        {
            isCancelable = false; 
        }

        public bool Action(string action)
        {
            bool result = true;
            if (playerAttack.IsAttacking) result = false;
            if (playerMovement.IsDashing && Enum.IsDefined(typeof(Attacks), action)) result = false;
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Attacks), action) && Enum.IsDefined(typeof(Attacks), currentAction)) result = false;
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Movement), action) == true) result = false;
            if (playerAnimator.IsResettingAnimation) result = false;

            if (playerAttack.IsAttacking && isCancelable == true && Enum.IsDefined(typeof(Attacks), action)) result = true;

            if (result == false) { return false; }

            //if (result == false)
            //{
            //    bufferedAction = action;
            //    bufferedTimer = playerAnimator.CurrentAnimationClip.length;
            //    if (bufferedAction == Dash.Player_DashForward.ToString()) Debug.Log("Buffering Dash");
            //    return result;
            //}

            //if (result && action == Movement.Player_Idle.ToString() && bufferedAction != Movement.Player_Idle.ToString() && bufferedTimer >= 0)
            //{
            //    Debug.Log("Replacing Current Action " + action + " With Buffered Action " + bufferedAction);
            //    if (bufferedAction == action) { playerAnimator.ChangeAnimation(Movement.Player_Idle.ToString()); }
            //    action = bufferedAction;
            //    bufferedAction = Movement.Player_Idle.ToString();
            //    bufferedTimer = -1;
            //}

            bool changedAnimation = playerAnimator.ChangeAnimation(action);
            //if (changedAnimation) Debug.Log("Changed Animation to " + action + " " + changedAnimation);
            IsAttacking = playerAttack.IsAttacking;
            currentAction = action;
            return result;
        }
    }

    public class BufferSystem
    {
        private string bufferedAnimation;
        private float bufferedTimer;

        public BufferSystem(float bufferedTimer)
        {
            this.bufferedTimer = bufferedTimer;
        }

        public void Start(float deltaTime)
        {
            bufferedTimer -= deltaTime;
        }

    }
}
