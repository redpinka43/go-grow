using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyHippo.Utility
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorTimeOffset : MonoBehaviour
    {
        public string currentAnimatorStateName;
        public float timeOffset;
        public event Action startAnimationEvent;
        public event Action stopAnimationAndReturnToFrame0Event;

        void Start()
        {
           StartAnimationWithOffset();
        }

        public void StartAnimationWithOffset()
        {
            Animator animator = GetComponent<Animator>();
            int stateNumber = animator.GetCurrentAnimatorStateInfo( 0 ).shortNameHash;
            animator.Play(stateNumber, 0, timeOffset);
            GetComponent<Animator>().speed = 1f;

            startAnimationEvent ?.Invoke();
        }

        public void StopAnimationAndReturnToFrame0()
        {
            Animator animator = GetComponent<Animator>();
            int stateNumber = animator.GetCurrentAnimatorStateInfo( 0 ).shortNameHash;
            animator.Play(stateNumber, 0, 0);
            GetComponent<Animator>().speed = 0f;

            stopAnimationAndReturnToFrame0Event ?.Invoke();
        }
    }
}
