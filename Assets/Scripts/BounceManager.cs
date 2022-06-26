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
        public static BounceManager Instance;
        [Header("Cooldown")]
        // a duration, in seconds, between two jumps, during which jumps are prevented
        [Tooltip("a duration, in seconds, between two jumps, during which jumps are prevented")]
        public float cooldownDuration = 1f;

        [Header("Bindings")]
        // the animator of the 'feedback' version  
        [Tooltip("the animator of the 'feedback' version")]
        public Animator feedbackAnimator;

        private float _lastJumpStartedAt = -100f;
        private static readonly int JUMP = Animator.StringToHash("Jump");

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        public void GoJump()
        {
            feedbackAnimator.SetTrigger(JUMP);
        }

        /// <summary>
        /// Performs a jump if possible, otherwise plays a denied feedback
        /// </summary>
        protected virtual void Jump()
        {
            if (Time.time - _lastJumpStartedAt < cooldownDuration)
            {
                
            }
            else
            {
                if (feedbackAnimator.isActiveAndEnabled)
                {
                    feedbackAnimator.SetTrigger(JUMP);
                }
                _lastJumpStartedAt = Time.time;
            }            
        }
    }
}