using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerAttack playerAttack;
        private PlayerMovement playerMovement;

        private Animator animator;

        public string CurrentAnimation { get; private set; }


        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerAttack = GetComponent<PlayerAttack>();
            animator = GetComponent<Animator>();
        }

        public void Jumping()
        {
            Debug.Log("Jumping!");
        }

        public void Moving(string dir)
        {
            //Debug.Log(dir);
            if (dir == "Forward") ChangeAnimation(Movement.Player_WalkForward.ToString());
            else if (dir == "Backward") ChangeAnimation(Movement.Player_WalkForward.ToString());
            else ChangeAnimation(Movement.Player_Idle.ToString());
        }

        public void Attacking(Attacks move)
        {
            if (move == Attacks.Player_None) ChangeAnimation(Movement.Player_Idle.ToString());
            else { ChangeAnimation(move.ToString()); }
        }

        public void Dashing(Dash dash)
        {
            ChangeAnimation(dash.ToString());
        }

        private void ChangeAnimation(string targetAnimation)
        {
            if (CurrentAnimation == targetAnimation) return;

            if (playerMovement.IsDashing && Enum.IsDefined(typeof(Attacks), targetAnimation)) return;
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Attacks), targetAnimation) && Enum.IsDefined(typeof(Attacks), CurrentAnimation)) { return; }
            if (playerAttack.IsAttacking == true && Enum.IsDefined(typeof(Movement), targetAnimation) == true) { return; }

            animator.Play(targetAnimation);
            Debug.Log("playing " + targetAnimation);
            CurrentAnimation = targetAnimation;
        }
    }
}
