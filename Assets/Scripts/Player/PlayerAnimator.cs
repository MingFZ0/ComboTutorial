using System;
using UnityEngine;

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

        //public void Moving(string dir)
        //{
        //    //Debug.Log(dir);
        //    if (dir == "Forward") ChangeAnimation(Movement.Player_WalkForward.ToString());
        //    else if (dir == "Backward") ChangeAnimation(Movement.Player_WalkForward.ToString());
        //    else ChangeAnimation(Movement.Player_Idle.ToString());
        //}

        //public void Attacking(Attacks move)
        //{
        //    if (move == Attacks.Player_None) ChangeAnimation(Movement.Player_Idle.ToString());
        //    else { ChangeAnimation(move.ToString()); }
        //}

        //public void Dashing(Dash dash)
        //{
        //    ChangeAnimation(dash.ToString());
        //}

        public void ChangeAnimation(string targetAnimation)
        {
            if (CurrentAnimation == targetAnimation) return;

            animator.Play(targetAnimation);
            Debug.Log("playing " + targetAnimation);
            CurrentAnimation = targetAnimation;
        }
    }
}
