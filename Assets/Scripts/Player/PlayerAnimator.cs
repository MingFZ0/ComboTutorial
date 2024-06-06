using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator animator;
        public bool IsResettingAnimation;

        public string CurrentAnimation { get; private set; }
        public AnimationClip CurrentAnimationClip { get; private set; }


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Jumping()
        {
            Debug.Log("Jumping!");
        }

        public void ResetAnimation()
        {
            if (ChangeAnimation(Movement.Player_Idle.ToString()))
            {
                IsResettingAnimation = true;
                //Debug.Log("Reset Animation to Idle");
                //actionScript.Action(Movement.Player_Idle.ToString());
                
            };
            
        }

        public bool ChangeAnimation(string targetAnimation)
        {
            if (CurrentAnimation == targetAnimation) return false;

            animator.Play(targetAnimation);
            CurrentAnimation = targetAnimation;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                //O(n) Operation
                if (clip.name == CurrentAnimation)
                {
                    CurrentAnimationClip = clip;
                    return true;
                }
            }
            return true;
        }
    }
}
