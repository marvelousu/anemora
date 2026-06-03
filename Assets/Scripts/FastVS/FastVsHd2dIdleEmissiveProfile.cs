using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dIdleEmissiveProfile", menuName = "Anemora/HD2D/Idle Emissive Profile")]
    public sealed class FastVsHd2dIdleEmissiveProfile : ScriptableObject
    {
        [SerializeField, Min(1)] private int loopFrameCount = 4;
        [SerializeField, Range(0.1f, 4f)] private float idleFrequencyHz = 0.72f;
        [SerializeField, Range(0f, 0.04f)] private float verticalBreathMeters = 0.018f;
        [SerializeField, Range(0f, 0.025f)] private float horizontalSwayMeters = 0.006f;
        [SerializeField, Range(0f, 0.035f)] private float squashStretchScale = 0.012f;
        [SerializeField, Range(0f, 6.283f)] private float phaseStepRadians = 0.71f;
        [SerializeField, Range(0.15f, 3f)] private float emissivePulseFrequencyHz = 0.86f;
        [SerializeField, Range(0f, 0.6f)] private float emissivePulseAmplitude = 0.22f;
        [SerializeField, ColorUsage(false, true)] private Color emissiveColor = new Color(1.0f, 0.62f, 0.28f, 1f);
        [SerializeField, Range(0f, 8f)] private float spriteEmissionIntensity = 3.35f;
        [SerializeField, Range(0f, 2f)] private float reviewWindowEmissionStrength = 1.18f;
        [SerializeField, Range(0f, 2f)] private float pointLightIntensity = 0.36f;
        [SerializeField, Range(0.1f, 4f)] private float pointLightRangeMeters = 1.65f;
        [SerializeField, Range(0.1f, 2f)] private float haloWorldSize = 0.46f;
        [SerializeField, Min(1)] private int minimumAnimatedMarkers = 8;
        [SerializeField, Min(1)] private int minimumEmissiveAccentMarkers = 1;
        [SerializeField] private bool phaseOffsetPerInstance = true;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalIdleEmissiveApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep the conservative breathing/sway and Mia lantern/eye glow as A/B data only. Tom should approve final amplitude, blink timing, glow color, bloom strength, and which character accents are canonical.";

        public int LoopFrameCountForReview => loopFrameCount;
        public float IdleFrequencyHzForReview => idleFrequencyHz;
        public float VerticalBreathMetersForReview => verticalBreathMeters;
        public float HorizontalSwayMetersForReview => horizontalSwayMeters;
        public float SquashStretchScaleForReview => squashStretchScale;
        public float PhaseStepRadiansForReview => phaseStepRadians;
        public float EmissivePulseFrequencyHzForReview => emissivePulseFrequencyHz;
        public float EmissivePulseAmplitudeForReview => emissivePulseAmplitude;
        public Color EmissiveColorForReview => emissiveColor;
        public float SpriteEmissionIntensityForReview => spriteEmissionIntensity;
        public float ReviewWindowEmissionStrengthForReview => reviewWindowEmissionStrength;
        public float PointLightIntensityForReview => pointLightIntensity;
        public float PointLightRangeMetersForReview => pointLightRangeMeters;
        public float HaloWorldSizeForReview => haloWorldSize;
        public int MinimumAnimatedMarkersForReview => minimumAnimatedMarkers;
        public int MinimumEmissiveAccentMarkersForReview => minimumEmissiveAccentMarkers;
        public bool PhaseOffsetPerInstanceForReview => phaseOffsetPerInstance;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalIdleEmissiveApprovedForReview => finalIdleEmissiveApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            int configuredLoopFrameCount,
            float configuredIdleFrequencyHz,
            float configuredVerticalBreathMeters,
            float configuredHorizontalSwayMeters,
            float configuredSquashStretchScale,
            float configuredPhaseStepRadians,
            float configuredEmissivePulseFrequencyHz,
            float configuredEmissivePulseAmplitude,
            Color configuredEmissiveColor,
            float configuredSpriteEmissionIntensity,
            float configuredReviewWindowEmissionStrength,
            float configuredPointLightIntensity,
            float configuredPointLightRangeMeters,
            float configuredHaloWorldSize,
            int configuredMinimumAnimatedMarkers,
            int configuredMinimumEmissiveAccentMarkers,
            bool configuredPhaseOffsetPerInstance,
            bool configuredConservativeDataPrep,
            bool configuredNeedsTomApproval,
            bool configuredFinalIdleEmissiveApproved,
            string configuredRecommendation)
        {
            loopFrameCount = Mathf.Max(1, configuredLoopFrameCount);
            idleFrequencyHz = Mathf.Clamp(configuredIdleFrequencyHz, 0.1f, 4f);
            verticalBreathMeters = Mathf.Clamp(configuredVerticalBreathMeters, 0f, 0.04f);
            horizontalSwayMeters = Mathf.Clamp(configuredHorizontalSwayMeters, 0f, 0.025f);
            squashStretchScale = Mathf.Clamp(configuredSquashStretchScale, 0f, 0.035f);
            phaseStepRadians = Mathf.Repeat(configuredPhaseStepRadians, Mathf.PI * 2f);
            emissivePulseFrequencyHz = Mathf.Clamp(configuredEmissivePulseFrequencyHz, 0.15f, 3f);
            emissivePulseAmplitude = Mathf.Clamp01(configuredEmissivePulseAmplitude);
            emissiveColor = configuredEmissiveColor;
            spriteEmissionIntensity = Mathf.Clamp(configuredSpriteEmissionIntensity, 0f, 8f);
            reviewWindowEmissionStrength = Mathf.Clamp(configuredReviewWindowEmissionStrength, 0f, 2f);
            pointLightIntensity = Mathf.Clamp(configuredPointLightIntensity, 0f, 2f);
            pointLightRangeMeters = Mathf.Clamp(configuredPointLightRangeMeters, 0.1f, 4f);
            haloWorldSize = Mathf.Clamp(configuredHaloWorldSize, 0.1f, 2f);
            minimumAnimatedMarkers = Mathf.Max(1, configuredMinimumAnimatedMarkers);
            minimumEmissiveAccentMarkers = Mathf.Max(1, configuredMinimumEmissiveAccentMarkers);
            phaseOffsetPerInstance = configuredPhaseOffsetPerInstance;
            conservativeDataPrep = configuredConservativeDataPrep;
            needsTomApproval = configuredNeedsTomApproval;
            finalIdleEmissiveApproved = configuredFinalIdleEmissiveApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
