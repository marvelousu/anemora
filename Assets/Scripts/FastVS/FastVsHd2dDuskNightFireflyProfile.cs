using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Dusk Night Firefly Profile")]
    public sealed class FastVsHd2dDuskNightFireflyProfile : ScriptableObject
    {
        [SerializeField, Min(20)] private int maxParticles = 44;
        [SerializeField, Min(0f)] private float duration = 8f;
        [SerializeField, Min(0f)] private float lifetime = 6.8f;
        [SerializeField, Min(0f)] private float nightEmissionRate = 7.6f;
        [SerializeField, Range(0f, 1f)] private float morningGate = 0f;
        [SerializeField, Range(0f, 1f)] private float noonGate = 0f;
        [SerializeField, Range(0f, 1f)] private float eveningGate = 0.62f;
        [SerializeField, Range(0f, 1f)] private float nightGate = 1f;
        [SerializeField, Min(0f)] private float startSpeedMin = 0.020f;
        [SerializeField, Min(0f)] private float startSpeedMax = 0.075f;
        [SerializeField, Min(0f)] private float startSizeMin = 0.040f;
        [SerializeField, Min(0f)] private float startSizeMax = 0.092f;
        [SerializeField] private Vector3 centralPlazaLocalCenter = new Vector3(20.8f, 1.42f, 18.7f);
        [SerializeField] private Vector3 boundedVolumeSize = new Vector3(10.0f, 2.8f, 5.8f);
        [SerializeField, Min(0f)] private float noiseStrength = 0.11f;
        [SerializeField, Min(0f)] private float noiseFrequency = 0.22f;
        [SerializeField, Range(0f, 1f)] private float randomDirectionAmount = 0.28f;
        [SerializeField, ColorUsage(false, true)] private Color hdrCoreColor = new Color(1.45f, 1.18f, 0.44f, 1f);
        [SerializeField, ColorUsage(false, true)] private Color hdrBlinkColor = new Color(2.35f, 1.72f, 0.62f, 1f);
        [SerializeField, Range(1f, 6f)] private float hdrIntensity = 2.25f;
        [SerializeField, Range(1f, 3f)] private float reviewCaptureEmissionMultiplier = 1.45f;
        [SerializeField, Range(0f, 1f)] private float blinkLowAlpha = 0.10f;
        [SerializeField, Range(0f, 1f)] private float blinkHighAlpha = 0.82f;
        [SerializeField, Min(0)] private int heroPointLightCount = 2;
        [SerializeField, Min(0f)] private float heroPointLightIntensity = 0.54f;
        [SerializeField, Min(0f)] private float heroPointLightRange = 2.15f;
        [SerializeField] private bool conservativeReviewMode = true;
        [SerializeField] private bool requiresTomArtApproval = true;
        [SerializeField] private string sourceNote = "Procedural CC0-safe glow sprites and CPU Shuriken fireflies; Tom should tune final magical density, blink timing, color, and placement.";

        public int MaxParticlesForReview => maxParticles;
        public float DurationForReview => duration;
        public float LifetimeForReview => lifetime;
        public float NightEmissionRateForReview => nightEmissionRate;
        public float MorningGateForReview => morningGate;
        public float NoonGateForReview => noonGate;
        public float EveningGateForReview => eveningGate;
        public float NightGateForReview => nightGate;
        public float StartSpeedMinForReview => startSpeedMin;
        public float StartSpeedMaxForReview => startSpeedMax;
        public float StartSizeMinForReview => startSizeMin;
        public float StartSizeMaxForReview => startSizeMax;
        public Vector3 CentralPlazaLocalCenterForReview => centralPlazaLocalCenter;
        public Vector3 BoundedVolumeSizeForReview => boundedVolumeSize;
        public float NoiseStrengthForReview => noiseStrength;
        public float NoiseFrequencyForReview => noiseFrequency;
        public float RandomDirectionAmountForReview => randomDirectionAmount;
        public Color HdrCoreColorForReview => hdrCoreColor;
        public Color HdrBlinkColorForReview => hdrBlinkColor;
        public float HdrIntensityForReview => hdrIntensity;
        public float ReviewCaptureEmissionMultiplierForReview => reviewCaptureEmissionMultiplier;
        public float BlinkLowAlphaForReview => blinkLowAlpha;
        public float BlinkHighAlphaForReview => blinkHighAlpha;
        public int HeroPointLightCountForReview => heroPointLightCount;
        public float HeroPointLightIntensityForReview => heroPointLightIntensity;
        public float HeroPointLightRangeForReview => heroPointLightRange;
        public bool ConservativeReviewModeForReview => conservativeReviewMode;
        public bool RequiresTomArtApprovalForReview => requiresTomArtApproval;
        public string SourceNoteForReview => sourceNote;

        public void ConfigureForReview(
            int configuredMaxParticles,
            float configuredDuration,
            float configuredLifetime,
            float configuredNightEmissionRate,
            float configuredMorningGate,
            float configuredNoonGate,
            float configuredEveningGate,
            float configuredNightGate,
            float configuredStartSpeedMin,
            float configuredStartSpeedMax,
            float configuredStartSizeMin,
            float configuredStartSizeMax,
            Vector3 configuredCentralPlazaLocalCenter,
            Vector3 configuredBoundedVolumeSize,
            float configuredNoiseStrength,
            float configuredNoiseFrequency,
            float configuredRandomDirectionAmount,
            Color configuredHdrCoreColor,
            Color configuredHdrBlinkColor,
            float configuredHdrIntensity,
            float configuredReviewCaptureEmissionMultiplier,
            float configuredBlinkLowAlpha,
            float configuredBlinkHighAlpha,
            int configuredHeroPointLightCount,
            float configuredHeroPointLightIntensity,
            float configuredHeroPointLightRange,
            bool configuredConservativeReviewMode,
            bool configuredRequiresTomArtApproval,
            string configuredSourceNote)
        {
            maxParticles = Mathf.Clamp(configuredMaxParticles, 20, 60);
            duration = Mathf.Max(1f, configuredDuration);
            lifetime = Mathf.Max(1f, configuredLifetime);
            nightEmissionRate = Mathf.Max(0f, configuredNightEmissionRate);
            morningGate = Mathf.Clamp01(configuredMorningGate);
            noonGate = Mathf.Clamp01(configuredNoonGate);
            eveningGate = Mathf.Clamp01(configuredEveningGate);
            nightGate = Mathf.Clamp01(configuredNightGate);
            startSpeedMin = Mathf.Max(0f, Mathf.Min(configuredStartSpeedMin, configuredStartSpeedMax));
            startSpeedMax = Mathf.Max(startSpeedMin, configuredStartSpeedMax);
            startSizeMin = Mathf.Max(0.001f, Mathf.Min(configuredStartSizeMin, configuredStartSizeMax));
            startSizeMax = Mathf.Max(startSizeMin, configuredStartSizeMax);
            centralPlazaLocalCenter = configuredCentralPlazaLocalCenter;
            boundedVolumeSize = new Vector3(
                Mathf.Max(0.5f, configuredBoundedVolumeSize.x),
                Mathf.Max(0.5f, configuredBoundedVolumeSize.y),
                Mathf.Max(0.5f, configuredBoundedVolumeSize.z));
            noiseStrength = Mathf.Max(0f, configuredNoiseStrength);
            noiseFrequency = Mathf.Max(0f, configuredNoiseFrequency);
            randomDirectionAmount = Mathf.Clamp01(configuredRandomDirectionAmount);
            hdrCoreColor = configuredHdrCoreColor;
            hdrBlinkColor = configuredHdrBlinkColor;
            hdrIntensity = Mathf.Max(1f, configuredHdrIntensity);
            reviewCaptureEmissionMultiplier = Mathf.Clamp(configuredReviewCaptureEmissionMultiplier, 1f, 3f);
            blinkLowAlpha = Mathf.Clamp01(configuredBlinkLowAlpha);
            blinkHighAlpha = Mathf.Clamp01(Mathf.Max(configuredBlinkLowAlpha, configuredBlinkHighAlpha));
            heroPointLightCount = Mathf.Clamp(configuredHeroPointLightCount, 0, 3);
            heroPointLightIntensity = Mathf.Max(0f, configuredHeroPointLightIntensity);
            heroPointLightRange = Mathf.Max(0f, configuredHeroPointLightRange);
            conservativeReviewMode = configuredConservativeReviewMode;
            requiresTomArtApproval = configuredRequiresTomArtApproval;
            sourceNote = configuredSourceNote ?? string.Empty;
        }
    }
}
