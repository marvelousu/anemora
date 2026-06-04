using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Anemora.FastVS
{
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dLivingCameraMotionPreview : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dLivingCameraMotionProfile profile;
        [SerializeField] private Camera reviewCamera;
        [SerializeField] private CinemachineCamera previewCamera;
        [SerializeField] private CinemachineBasicMultiChannelPerlin perlinNoise;
        [SerializeField] private CinemachinePositionComposer positionComposer;
        [SerializeField] private NoiseSettings noiseSettings;
        [SerializeField] private Transform followTarget;
        [SerializeField] private Transform foregroundParallaxMarker;
        [SerializeField] private Transform backgroundParallaxMarker;

        public bool IsReadyForReview =>
            profile != null &&
            reviewCamera != null &&
            previewCamera != null &&
            perlinNoise != null &&
            positionComposer != null &&
            noiseSettings != null &&
            followTarget != null &&
            foregroundParallaxMarker != null &&
            backgroundParallaxMarker != null;

        public FastVsHd2dLivingCameraMotionProfile ProfileForReview => profile;
        public CinemachineCamera PreviewCameraForReview => previewCamera;
        public CinemachineBasicMultiChannelPerlin PerlinNoiseForReview => perlinNoise;
        public CinemachinePositionComposer PositionComposerForReview => positionComposer;
        public NoiseSettings NoiseSettingsForReview => noiseSettings;
        public bool ActiveOnAwakeForReview => profile != null && profile.ActiveOnAwakeForReview;
        public bool DirectRuntimeCameraAuthorityDisabledForReview => profile != null && profile.DirectRuntimeCameraAuthorityDisabledForReview;
        public bool PreviewCameraInactiveForReview => previewCamera != null && profile != null && previewCamera.Priority.Value == profile.InactivePriorityForReview;
        public int PositionNoiseChannelCountForReview => noiseSettings != null && noiseSettings.PositionNoise != null ? noiseSettings.PositionNoise.Length : 0;
        public int OrientationNoiseChannelCountForReview => noiseSettings != null && noiseSettings.OrientationNoise != null ? noiseSettings.OrientationNoise.Length : 0;

        public void ConfigureForReview(
            FastVsHd2dLivingCameraMotionProfile livingProfile,
            Camera camera,
            CinemachineCamera cinemachineCamera,
            CinemachineBasicMultiChannelPerlin noise,
            CinemachinePositionComposer composer,
            NoiseSettings settings,
            Transform target,
            Transform foregroundMarker,
            Transform backgroundMarker)
        {
            profile = livingProfile;
            reviewCamera = camera;
            previewCamera = cinemachineCamera;
            perlinNoise = noise;
            positionComposer = composer;
            noiseSettings = settings;
            followTarget = target;
            foregroundParallaxMarker = foregroundMarker;
            backgroundParallaxMarker = backgroundMarker;
            ApplyInactiveCinemachineContractForReview();
            SetReviewMarkersVisibleForReview(false);
        }

        public void ApplyInactiveCinemachineContractForReview()
        {
            if (profile == null || previewCamera == null)
            {
                return;
            }

            previewCamera.Follow = followTarget;
            previewCamera.LookAt = followTarget;
            previewCamera.Priority.Value = profile.InactivePriorityForReview;
            var lens = previewCamera.Lens;
            lens.FieldOfView = profile.FieldOfViewForReview;
            lens.NearClipPlane = 0.30f;
            lens.FarClipPlane = reviewCamera != null ? reviewCamera.farClipPlane : 140f;
            previewCamera.Lens = lens;

            if (perlinNoise != null)
            {
                perlinNoise.NoiseProfile = noiseSettings;
                perlinNoise.AmplitudeGain = profile.NoiseAmplitudeGainForReview;
                perlinNoise.FrequencyGain = profile.NoiseFrequencyGainForReview;
                perlinNoise.PivotOffset = Vector3.zero;
            }

            if (positionComposer != null)
            {
                positionComposer.CameraDistance = profile.CameraDistanceForReview;
                positionComposer.Damping = profile.FollowDampingSecondsForReview;
                positionComposer.TargetOffset = profile.BaseLookOffsetForReview;
                positionComposer.Lookahead = new LookaheadSettings
                {
                    Enabled = profile.LookAheadTimeForReview > 0.001f,
                    Time = profile.LookAheadTimeForReview,
                    Smoothing = profile.LookAheadSmoothingForReview,
                    IgnoreY = true
                };
            }
        }

        public void SetReviewMarkersVisibleForReview(bool visible)
        {
            if (foregroundParallaxMarker != null)
            {
                foregroundParallaxMarker.gameObject.SetActive(visible);
            }

            if (backgroundParallaxMarker != null)
            {
                backgroundParallaxMarker.gameObject.SetActive(visible);
            }
        }

        public Vector3 CalculateBreathingOffsetForReview(float sampleSeconds, bool motionEnabled, bool accessibilityMotionDisabled)
        {
            if (profile == null || !motionEnabled || accessibilityMotionDisabled)
            {
                return Vector3.zero;
            }

            var amplitude = profile.PositionAmplitudeMetersForReview;
            var phase = sampleSeconds * profile.FrequencyHzForReview * Mathf.PI * 2f;
            return new Vector3(
                Mathf.Sin(phase + 0.35f) * amplitude.x,
                Mathf.Sin(phase * 0.73f + 1.20f) * amplitude.y,
                Mathf.Cos(phase * 0.61f + 0.48f) * amplitude.z);
        }

        public Vector3 EvaluateSoftFollowAnchorForReview(Vector3 previousLocalAnchor, Vector3 targetLocalAnchor, float elapsedSeconds, out float t)
        {
            var seconds = profile != null ? profile.StopEaseSecondsForReview : 0.68f;
            t = 1f - Mathf.Exp(-Mathf.Max(0f, elapsedSeconds) / Mathf.Max(0.001f, seconds));
            t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
            return Vector3.Lerp(previousLocalAnchor, targetLocalAnchor, t);
        }

        public bool ApplyCameraStateForReview(
            Camera camera,
            Transform activeRoot,
            Vector3 anchorLocal,
            float sampleSeconds,
            bool motionEnabled,
            bool accessibilityMotionDisabled,
            out SampleResult result)
        {
            result = default;
            if (profile == null || camera == null)
            {
                return false;
            }

            var anchorWorld = activeRoot != null ? activeRoot.TransformPoint(anchorLocal) : anchorLocal;
            var basePosition = anchorWorld + profile.BasePositionOffsetForReview;
            var lookAt = anchorWorld + profile.BaseLookOffsetForReview;
            var baseRotation = Quaternion.LookRotation(lookAt - basePosition, Vector3.up);
            var motionOffset = CalculateBreathingOffsetForReview(sampleSeconds, motionEnabled, accessibilityMotionDisabled);
            var motionPosition = basePosition + motionOffset;
            var motionRotation = baseRotation;

            var previousPosition = camera.transform.position;
            var previousRotation = camera.transform.rotation;
            var previousFieldOfView = camera.fieldOfView;
            try
            {
                camera.orthographic = false;
                camera.fieldOfView = profile.FieldOfViewForReview;
                camera.transform.SetPositionAndRotation(basePosition, baseRotation);
                var foregroundBase = MeasureMarkerScreenPosition(camera, foregroundParallaxMarker);
                var backgroundBase = MeasureMarkerScreenPosition(camera, backgroundParallaxMarker);

                camera.transform.SetPositionAndRotation(motionPosition, motionRotation);
                var foregroundMotion = MeasureMarkerScreenPosition(camera, foregroundParallaxMarker);
                var backgroundMotion = MeasureMarkerScreenPosition(camera, backgroundParallaxMarker);

                var foregroundPixels = PixelDistance(foregroundBase, foregroundMotion);
                var backgroundPixels = PixelDistance(backgroundBase, backgroundMotion);
                result = new SampleResult(
                    sampleSeconds,
                    motionEnabled,
                    accessibilityMotionDisabled,
                    basePosition,
                    motionPosition,
                    motionOffset,
                    baseRotation.eulerAngles,
                    motionRotation.eulerAngles,
                    foregroundPixels,
                    backgroundPixels,
                    Mathf.Abs(Mathf.DeltaAngle(baseRotation.eulerAngles.x, motionRotation.eulerAngles.x)) +
                    Mathf.Abs(Mathf.DeltaAngle(baseRotation.eulerAngles.y, motionRotation.eulerAngles.y)) +
                    Mathf.Abs(Mathf.DeltaAngle(baseRotation.eulerAngles.z, motionRotation.eulerAngles.z)));
                return true;
            }
            catch (Exception)
            {
                camera.transform.SetPositionAndRotation(previousPosition, previousRotation);
                camera.fieldOfView = previousFieldOfView;
                throw;
            }
        }

        private static Vector2 MeasureMarkerScreenPosition(Camera camera, Transform marker)
        {
            if (camera == null || marker == null)
            {
                return Vector2.zero;
            }

            var screen = camera.WorldToScreenPoint(marker.position);
            return new Vector2(screen.x, screen.y);
        }

        private static float PixelDistance(Vector2 a, Vector2 b)
        {
            return Vector2.Distance(a, b);
        }

        [Serializable]
        public readonly struct SampleResult
        {
            public readonly float SampleSeconds;
            public readonly bool MotionEnabled;
            public readonly bool AccessibilityMotionDisabled;
            public readonly Vector3 BasePosition;
            public readonly Vector3 MotionPosition;
            public readonly Vector3 MotionOffset;
            public readonly Vector3 BaseEuler;
            public readonly Vector3 MotionEuler;
            public readonly float ForegroundPixelShift;
            public readonly float BackgroundPixelShift;
            public readonly float RotationDeltaDegrees;

            public SampleResult(
                float sampleSeconds,
                bool motionEnabled,
                bool accessibilityMotionDisabled,
                Vector3 basePosition,
                Vector3 motionPosition,
                Vector3 motionOffset,
                Vector3 baseEuler,
                Vector3 motionEuler,
                float foregroundPixelShift,
                float backgroundPixelShift,
                float rotationDeltaDegrees)
            {
                SampleSeconds = sampleSeconds;
                MotionEnabled = motionEnabled;
                AccessibilityMotionDisabled = accessibilityMotionDisabled;
                BasePosition = basePosition;
                MotionPosition = motionPosition;
                MotionOffset = motionOffset;
                BaseEuler = baseEuler;
                MotionEuler = motionEuler;
                ForegroundPixelShift = foregroundPixelShift;
                BackgroundPixelShift = backgroundPixelShift;
                RotationDeltaDegrees = rotationDeltaDegrees;
            }
        }
    }
}
