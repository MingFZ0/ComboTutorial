using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class AnimatorScript : MonoBehaviour
    {
        private Animator animator;
        public bool IsResettingAnimation;

        public string CurrentAnimation { get; private set; }
        public AnimationClip CurrentAnimationClip { get; private set; }


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (CurrentAnimation != animator.GetCurrentAnimatorClipInfo(0)[0].clip.name)
            {
                CurrentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            }
        }

        public void Jumping()
        {
            Debug.Log("Jumping!");
        }

        public bool ChangeAnimation(AnimationClip targetAnimation)
        {
            if (CurrentAnimation == targetAnimation.name) return false;
            Debug.Log("changed Animation to " + targetAnimation.name);
            animator.Play(targetAnimation.name);
            CurrentAnimation = targetAnimation.name;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            CurrentAnimationClip = targetAnimation;
            return true;
        }
    }
}
