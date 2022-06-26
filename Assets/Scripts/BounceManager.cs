using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Feel
{
    /// <summary>
    /// An example class part of the Feel demos
    /// This class acts as a character controller for the Duck in the FeelDuck demo scene
    /// It looks for input, and jumps when instructed to
    /// </summary>
    public class BounceManager : MonoBehaviour
    {
        public static BounceManager instance;
        [Header("Cooldown")]
        /// a duration, in seconds, between two jumps, during which jumps are prevented
        [Tooltip("a duration, in seconds, between two jumps, during which jumps are prevented")]
        public float CooldownDuration = 1f;

        [Header("Bindings")]
        /// the animator of the 'feedback' version  
        [Tooltip("the animator of the 'feedback' version")]
        public Animator FeedbackAnimator;

        protected float _lastJumpStartedAt = -100f;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }
        public void GoJump()
        {
            FeedbackAnimator.SetTrigger("Jump");
        }

        /// <summary>
        /// Performs a jump if possible, otherwise plays a denied feedback
        /// </summary>
        protected virtual void Jump()
        {
            if (Time.time - _lastJumpStartedAt < CooldownDuration)
            {
                
            }
            else
            {
                if (FeedbackAnimator.isActiveAndEnabled)
                {
                    FeedbackAnimator.SetTrigger("Jump");
                }
                _lastJumpStartedAt = Time.time;
            }            
        }
    }
}