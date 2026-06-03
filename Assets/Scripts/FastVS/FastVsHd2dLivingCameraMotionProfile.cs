using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dLivingCameraMotionProfile", menuName = "Anemora/HD2D/Living Camera Motion Profile")]
    public sealed class FastVsHd2dLivingCameraMotionProfile : ScriptableObject
    {
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalLivingCameraMotionApproved;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool directRuntimeCameraAuthorityDisabled = true;
        [SerializeField] private bool activeOnAwake;
        [SerializeField] private bool accessibilityTogglePrepared = true;
        [SerializeField] private bool accessibilityMotionDisabledByDefault = true;
        [SerializeField] private bool cinemachineNoiseConfigured = true;
        [SerializeField] private bool positionComposerDampingConfigured = true;
        [SerializeField, Range(0.5f, 3.0f)] private float targetPixelDrift = 1.2f;
        [SerializeField] private Vector3 positionAmplitudeMeters = new Vector3(0.010f, 0.006f, 0.003f);
        [SerializeField, Range(0.05f, 0.35f)] private float frequencyHz = 0.18f;
        [SerializeField, Range(0f, 0.03f)] private float rotationAmplitudeDegrees;
        [SerializeField, Range(0f, 2f)] private float noiseAmplitudeGain = 1f;
        [SerializeField, Range(0.05f, 0.5f)] private float noiseFrequencyGain = 0.18f;
        [SerializeField] private Vector3 followDampingSeconds = new Vector3(0.45f, 0.58f, 0.72f);
        [SerializeField, Range(0f, 0.45f)] private float lookAheadTime = 0.16f;
        [SerializeField, Range(0f, 20f)] private float lookAheadSmoothing = 7.5f;
        [SerializeField, Range(0.2f, 1.2f)] private float stopEaseSeconds = 0.68f;
        [SerializeField, Range(22f, 40f)] private float fieldOfView = 38f;
        [SerializeField, Min(1f)] private float cameraDistance = 4.55f;
        [SerializeField] private Vector3 basePositionOffset = new Vector3(0f, 2.75f, -4.55f);
        [SerializeField] private Vector3 baseLookOffset = new Vector3(0f, 0.72f, 0.45f);
        [SerializeField] private int inactivePriority = 5;
        [SerializeField] private int plannedLivePriority = 155;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep this as conservative living-camera data only. Tom should tune final drift amplitude, damping, look-ahead, and accessibility defaults before this controls the live gameplay camera.";

        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalLivingCameraMotionApprovedForReview => finalLivingCameraMotionApproved;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool DirectRuntimeCameraAuthorityDisabledForReview => directRuntimeCameraAuthorityDisabled;
        public bool ActiveOnAwakeForReview => activeOnAwake;
        public bool AccessibilityTogglePreparedForReview => accessibilityTogglePrepared;
        public bool AccessibilityMotionDisabledByDefaultForReview => accessibilityMotionDisabledByDefault;
        public bool CinemachineNoiseConfiguredForReview => cinemachineNoiseConfigured;
        public bool PositionComposerDampingConfiguredForReview => positionComposerDampingConfigured;
        public float TargetPixelDriftForReview => Mathf.Clamp(targetPixelDrift, 0.5f, 3.0f);
        public Vector3 PositionAmplitudeMetersForReview => new Vector3(
            Mathf.Clamp(positionAmplitudeMeters.x, 0f, 0.05f),
            Mathf.Clamp(positionAmplitudeMeters.y, 0f, 0.04f),
            Mathf.Clamp(positionAmplitudeMeters.z, 0f, 0.03f));
        public float FrequencyHzForReview => Mathf.Clamp(frequencyHz, 0.05f, 0.35f);
        public float RotationAmplitudeDegreesForReview => Mathf.Clamp(rotationAmplitudeDegrees, 0f, 0.03f);
        public float NoiseAmplitudeGainForReview => Mathf.Clamp(noiseAmplitudeGain, 0f, 2f);
        public float NoiseFrequencyGainForReview => Mathf.Clamp(noiseFrequencyGain, 0.05f, 0.5f);
        public Vector3 FollowDampingSecondsForReview => new Vector3(
            Mathf.Clamp(followDampingSeconds.x, 0.05f, 1.4f),
            Mathf.Clamp(followDampingSeconds.y, 0.05f, 1.4f),
            Mathf.Clamp(followDampingSeconds.z, 0.05f, 1.4f));
        public float LookAheadTimeForReview => Mathf.Clamp(lookAheadTime, 0f, 0.45f);
        public float LookAheadSmoothingForReview => Mathf.Clamp(lookAheadSmoothing, 0f, 20f);
        public float StopEaseSecondsForReview => Mathf.Clamp(stopEaseSeconds, 0.2f, 1.2f);
        public float FieldOfViewForReview => Mathf.Clamp(fieldOfView, 22f, 40f);
        public float CameraDistanceForReview => Mathf.Max(1f, cameraDistance);
        public Vector3 BasePositionOffsetForReview => basePositionOffset;
        public Vector3 BaseLookOffsetForReview => baseLookOffset;
        public int InactivePriorityForReview => inactivePriority;
        public int PlannedLivePriorityForReview => Mathf.Max(plannedLivePriority, inactivePriority + 1);
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            bool configuredNeedsTomApproval,
            bool configuredFinalApproved,
            bool configuredConservativeDataPrep,
            bool configuredDirectRuntimeCameraAuthorityDisabled,
            bool configuredActiveOnAwake,
            bool configuredAccessibilityTogglePrepared,
            bool configuredAccessibilityDisabledByDefault,
            bool configuredCinemachineNoiseConfigured,
            bool configuredPositionComposerDampingConfigured,
            float configuredTargetPixelDrift,
            Vector3 configuredPositionAmplitudeMeters,
            float configuredFrequencyHz,
            float configuredRotationAmplitudeDegrees,
            float configuredNoiseAmplitudeGain,
            float configuredNoiseFrequencyGain,
            Vector3 configuredFollowDampingSeconds,
            float configuredLookAheadTime,
            float configuredLookAheadSmoothing,
            float configuredStopEaseSeconds,
            float configuredFieldOfView,
            float configuredCameraDistance,
            Vector3 configuredBasePositionOffset,
            Vector3 configuredBaseLookOffset,
            int configuredInactivePriority,
            int configuredPlannedLivePriority,
            string configuredRecommendation)
        {
            needsTomApproval = configuredNeedsTomApproval;
            finalLivingCameraMotionApproved = configuredFinalApproved;
            conservativeDataPrep = configuredConservativeDataPrep;
            directRuntimeCameraAuthorityDisabled = configuredDirectRuntimeCameraAuthorityDisabled;
            activeOnAwake = configuredActiveOnAwake;
            accessibilityTogglePrepared = configuredAccessibilityTogglePrepared;
            accessibilityMotionDisabledByDefault = configuredAccessibilityDisabledByDefault;
            cinemachineNoiseConfigured = configuredCinemachineNoiseConfigured;
            positionComposerDampingConfigured = configuredPositionComposerDampingConfigured;
            targetPixelDrift = Mathf.Clamp(configuredTargetPixelDrift, 0.5f, 3.0f);
            positionAmplitudeMeters = new Vector3(
                Mathf.Clamp(configuredPositionAmplitudeMeters.x, 0f, 0.05f),
                Mathf.Clamp(configuredPositionAmplitudeMeters.y, 0f, 0.04f),
                Mathf.Clamp(configuredPositionAmplitudeMeters.z, 0f, 0.03f));
            frequencyHz = Mathf.Clamp(configuredFrequencyHz, 0.05f, 0.35f);
            rotationAmplitudeDegrees = Mathf.Clamp(configuredRotationAmplitudeDegrees, 0f, 0.03f);
            noiseAmplitudeGain = Mathf.Clamp(configuredNoiseAmplitudeGain, 0f, 2f);
            noiseFrequencyGain = Mathf.Clamp(configuredNoiseFrequencyGain, 0.05f, 0.5f);
            followDampingSeconds = new Vector3(
                Mathf.Clamp(configuredFollowDampingSeconds.x, 0.05f, 1.4f),
                Mathf.Clamp(configuredFollowDampingSeconds.y, 0.05f, 1.4f),
                Mathf.Clamp(configuredFollowDampingSeconds.z, 0.05f, 1.4f));
            lookAheadTime = Mathf.Clamp(configuredLookAheadTime, 0f, 0.45f);
            lookAheadSmoothing = Mathf.Clamp(configuredLookAheadSmoothing, 0f, 20f);
            stopEaseSeconds = Mathf.Clamp(configuredStopEaseSeconds, 0.2f, 1.2f);
            fieldOfView = Mathf.Clamp(configuredFieldOfView, 22f, 40f);
            cameraDistance = Mathf.Max(1f, configuredCameraDistance);
            basePositionOffset = configuredBasePositionOffset;
            baseLookOffset = configuredBaseLookOffset;
            inactivePriority = configuredInactivePriority;
            plannedLivePriority = Mathf.Max(configuredPlannedLivePriority, configuredInactivePriority + 1);
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
