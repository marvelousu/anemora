using UnityEngine;

namespace Anemora.FastVS
{
    [DefaultExecutionOrder(122)]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Idle Secondary Motion")]
    public sealed class FastVsHd2dIdleSecondaryMotion : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dIdleEmissiveProfile profile;
        [SerializeField] private Transform motionRoot;
        [SerializeField, Range(0f, 6.283f)] private float phaseRadians;
        [SerializeField] private bool motionEnabled = true;

        private Vector3 baseLocalPosition;
        private Vector3 baseLocalScale = Vector3.one;
        private bool basePoseCached;
        private float reviewTimeOverride = -1f;
        private float lastAppliedVerticalMeters;
        private float lastAppliedHorizontalMeters;
        private float lastAppliedSquashScale;

        public FastVsHd2dIdleEmissiveProfile ProfileForReview => profile;
        public Transform MotionRootForReview => motionRoot;
        public float PhaseRadiansForReview => phaseRadians;
        public bool MotionEnabledForReview => motionEnabled;
        public float LastAppliedVerticalMetersForReview => lastAppliedVerticalMeters;
        public float LastAppliedHorizontalMetersForReview => lastAppliedHorizontalMeters;
        public float LastAppliedSquashScaleForReview => lastAppliedSquashScale;

        private void Awake()
        {
            CacheBasePoseIfNeeded();
        }

        private void OnEnable()
        {
            CacheBasePoseIfNeeded();
            if (Application.isPlaying)
            {
                ApplyForReview();
            }
            else
            {
                ResetPoseForReview();
            }
        }

        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            ApplyForReview();
        }

        public void ConfigureForReview(FastVsHd2dIdleEmissiveProfile configuredProfile, Transform configuredMotionRoot, float configuredPhaseRadians)
        {
            profile = configuredProfile;
            motionRoot = configuredMotionRoot != null ? configuredMotionRoot : transform;
            phaseRadians = Mathf.Repeat(configuredPhaseRadians, Mathf.PI * 2f);
            motionEnabled = true;
            basePoseCached = false;
            reviewTimeOverride = -1f;
            CacheBasePoseIfNeeded();
            if (Application.isPlaying)
            {
                ApplyForReview();
            }
            else
            {
                ResetPoseForReview();
            }
        }

        public void SetMotionEnabledForReview(bool enabled)
        {
            motionEnabled = enabled;
            ApplyForReview();
        }

        public void SampleForReview(float seconds)
        {
            reviewTimeOverride = Mathf.Max(0f, seconds);
            ApplyForReview();
        }

        public void ClearReviewTimeForReview()
        {
            reviewTimeOverride = -1f;
            if (Application.isPlaying)
            {
                ApplyForReview();
            }
            else
            {
                ResetPoseForReview();
            }
        }

        public void ResetPoseForReview()
        {
            CacheBasePoseIfNeeded();
            if (motionRoot == null)
            {
                return;
            }

            motionRoot.localPosition = baseLocalPosition;
            motionRoot.localScale = baseLocalScale;
            lastAppliedVerticalMeters = 0f;
            lastAppliedHorizontalMeters = 0f;
            lastAppliedSquashScale = 0f;
        }

        public Vector3 EvaluateLocalPositionForReview(float seconds)
        {
            CacheBasePoseIfNeeded();
            var motion = ResolveMotion(seconds);
            return baseLocalPosition + new Vector3(motion.x, motion.y, 0f);
        }

        private void ApplyForReview()
        {
            CacheBasePoseIfNeeded();
            if (motionRoot == null || profile == null)
            {
                return;
            }

            if (!Application.isPlaying && reviewTimeOverride < 0f)
            {
                ResetPoseForReview();
                return;
            }

            if (!motionEnabled)
            {
                ResetPoseForReview();
                return;
            }

            var time = reviewTimeOverride >= 0f ? reviewTimeOverride : Time.time;
            var motion = ResolveMotion(time);
            var squash = ResolveSquash(time);
            motionRoot.localPosition = baseLocalPosition + new Vector3(motion.x, motion.y, 0f);
            motionRoot.localScale = new Vector3(
                baseLocalScale.x * Mathf.Max(0.01f, 1f - squash * 0.35f),
                baseLocalScale.y * Mathf.Max(0.01f, 1f + squash),
                baseLocalScale.z);

            lastAppliedHorizontalMeters = motion.x;
            lastAppliedVerticalMeters = motion.y;
            lastAppliedSquashScale = squash;
        }

        private Vector2 ResolveMotion(float seconds)
        {
            if (profile == null)
            {
                return Vector2.zero;
            }

            var cycle = seconds * profile.IdleFrequencyHzForReview * Mathf.PI * 2f;
            var vertical = Mathf.Sin(cycle + phaseRadians) * profile.VerticalBreathMetersForReview;
            var sway = Mathf.Sin((cycle * 0.63f) + (phaseRadians * 1.37f)) * profile.HorizontalSwayMetersForReview;
            return new Vector2(sway, vertical);
        }

        private float ResolveSquash(float seconds)
        {
            if (profile == null)
            {
                return 0f;
            }

            var cycle = seconds * profile.IdleFrequencyHzForReview * Mathf.PI * 2f;
            return Mathf.Cos(cycle + phaseRadians) * profile.SquashStretchScaleForReview;
        }

        private void CacheBasePoseIfNeeded()
        {
            if (basePoseCached)
            {
                return;
            }

            if (motionRoot == null)
            {
                motionRoot = transform;
            }

            baseLocalPosition = motionRoot.localPosition;
            baseLocalScale = motionRoot.localScale;
            basePoseCached = true;
        }
    }
}
