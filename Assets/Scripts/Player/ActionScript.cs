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

        private PlayerActionScriptBehavior playerActionScriptBehavior;

        private void Awake()
        {
            playerAttack = GetComponent<PlayerAttack>();
            playerMovement = GetComponent<PlayerMovement>();
            playerAnimator = GetComponent<PlayerAnimator>();
            playerActionScriptBehavior = new PlayerActionScriptBehavior(playerAttack, playerMovement, playerAnimator);
        }

        //private void Update()
        //{
        //    //if (bufferedTimer >= 0) { bufferedTimer -= Time.deltaTime; }
        //}


        public void OnHitBoxCollide()
        {
            Debug.Log("hit");
            //isCancelable = true;
        }

        public void OnHitBoxExit()
        {
            //isCancelable = false; 
        }

        public bool Action(string action)
        {
            return playerActionScriptBehavior.Action(action);
        }
    }

    public class PlayerActionScriptBehavior: ActionScriptBehavior
    {
        private readonly PlayerAttack playerAttack;
        private readonly PlayerMovement playerMovement;
        private readonly PlayerAnimator playerAnimator;

        private string currentAction;
        private int currentCancelLevel;
        private Boolean HitBoxCollided;

        public Dictionary<int, string[]> CancelLevels = new Dictionary<int, string[]>();
        private HashSet<string> UsedMoves = new HashSet<string>();


        public PlayerActionScriptBehavior(PlayerAttack playerAttack, PlayerMovement playerMovement, PlayerAnimator playerAnimator)
        {
            this.playerAttack = playerAttack;
            this.playerMovement = playerMovement;
            this.playerAnimator = playerAnimator;

            string[] lights = { Attacks.Player_2L.ToString(), Attacks.Player_5L.ToString(), Attacks.Player_4L.ToString() };
            CancelLevels.Add(1, lights);

            string[] mediums = { Attacks.Player_5M.ToString(), Attacks.Player_2M.ToString() };
            CancelLevels.Add(2, mediums);

            string[] heavies = { Attacks.Player_5H.ToString(), Attacks.Player_2H.ToString() };
            CancelLevels.Add(3, heavies);
        }

        public override bool Action(string action)
        {
            bool result = true;
            if (playerAttack.IsAttacking) { result = false; }
            if (playerMovement.IsDashing && Enum.IsDefined(typeof(Attacks), action)) { result = false; }
            //if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Attacks), action) && Enum.IsDefined(typeof(Attacks), currentAction)) { result = false; }
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Movement), action) == true) { result = false; }
            if (playerAnimator.IsResettingAnimation) { result = false; }

            //Check for Button Canceling
            if (playerAttack.IsAttacking == true && HitBoxCollided && Enum.IsDefined(typeof(Attacks), currentAction) && playerAttack.IsCancelable) 
            {
                if (CheckCancelLevel(action) < currentCancelLevel) { result = false; }
                else if (UsedMoves.Contains(action)) { result = false; }
                else
                {
                    result = true;
                }
            }

            //if (playerAttack.IsAttacking && Enum.IsDefined(typeof(Attacks), action)) { result = true; }

            if (result == false) { return false; }

                #region Unused BufferSystem Code
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
                #endregion

            playerAnimator.ChangeAnimation(action);
            //bool changedAnimation = playerAnimator.ChangeAnimation(action);
            //if (changedAnimation) Debug.Log("Changed Animation to " + action + " " + changedAnimation);
            //IsAttacking = playerAttack.IsAttacking;
            
            if (currentAction != null && Enum.IsDefined(typeof(Attacks), currentAction)) { currentCancelLevel = CheckCancelLevel(currentAction); }
            else 
            {
                currentCancelLevel = 0;
                UsedMoves.Clear();
            }
            currentAction = action;
            
            return result;
        }

        private int CheckCancelLevel(string action)
        {
            if (Enum.IsDefined(typeof(Lights), action)) { return 1; }
            if (Enum.IsDefined(typeof(Mediums), action)) { return 2; }
            else { return 3; }
        }
    }

    public abstract class ActionScriptBehavior
    {
        public abstract bool Action(string action);
    }
}
