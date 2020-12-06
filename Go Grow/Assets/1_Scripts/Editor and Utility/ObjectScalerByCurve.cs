using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyHippo.Utility
{
    public class ObjectScalerByCurve : MonoBehaviour
    {
        public AnimationCurve curve;
        public float animationDuration;
        public float animationTimeOffset;
        public float minimumScale;
        public AnimatorTimeOffset animatorTimeOffset;

        private float _timeAnimationStarted;
        private bool _isPlaying = true;

        void OnEnable()
        {
            // Subscribe events
            if (animatorTimeOffset != null)
            {
                animatorTimeOffset.startAnimationEvent += StartAnimation;
                animatorTimeOffset.stopAnimationAndReturnToFrame0Event += StopAnimationAndReturnToFrame0;
            }
        }

        void OnDisable()
        {
            // Unsubscribe events
            if (animatorTimeOffset != null)
            {
                animatorTimeOffset.startAnimationEvent -= StartAnimation;
                animatorTimeOffset.stopAnimationAndReturnToFrame0Event -= StopAnimationAndReturnToFrame0;
            }
        }

        void Start()
        {
            if (animatorTimeOffset != null)
            {
                animationTimeOffset += animatorTimeOffset.timeOffset;
            }
            StartAnimation();
        }

        void Update()
        {
            if (_isPlaying)
            {
                float scale = GetScale( ((Time.time - _timeAnimationStarted) + animationTimeOffset) % animationDuration );
                transform.localScale = new Vector3(scale, scale, transform.localScale.z);
            }
        }

        /// <summary>
        /// Get the scale that the object should have, given a time (float).
        /// </summary>
        float GetScale(float time)
        {
            float scale = curve.Evaluate(time);
            return minimumScale + (1 - minimumScale) * scale;
        }

        void StartAnimation()
        {
            _isPlaying = true;
            _timeAnimationStarted = Time.time;
        }

        void StopAnimationAndReturnToFrame0()
        {
            _isPlaying = false;
            animationTimeOffset = 0;
            float scale = GetScale(0);
            transform.localScale = new Vector3(scale, scale, transform.localScale.z);
        }
    }
}
