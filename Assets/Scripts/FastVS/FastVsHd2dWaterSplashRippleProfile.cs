using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dWaterSplashRippleProfile", menuName = "Anemora/HD2D/Water Splash Ripple Profile")]
    public sealed class FastVsHd2dWaterSplashRippleProfile : ScriptableObject
    {
        [SerializeField, Range(4, 18)] private int maxRippleParticles = 10;
        [SerializeField, Range(4, 18)] private int splashBurstParticles = 10;
        [SerializeField, Range(18, 48)] private int maxMistParticles = 36;
        [SerializeField, Range(0.55f, 1.25f)] private float rippleLifetime = 0.96f;
        [SerializeField, Range(0.20f, 0.55f)] private float rippleStartSize = 0.30f;
        [SerializeField, Range(0.55f, 1.35f)] private float rippleEndSize = 1.04f;
        [SerializeField, Range(0.02f, 0.45f)] private float ripplePeakAlpha = 0.30f;
        [SerializeField, Range(0.22f, 0.82f)] private float splashLifetime = 0.56f;
        [SerializeField, Range(0.020f, 0.080f)] private float splashStartSizeMin = 0.034f;
        [SerializeField, Range(0.035f, 0.120f)] private float splashStartSizeMax = 0.074f;
        [SerializeField, Range(0.28f, 1.20f)] private float splashHorizontalVelocity = 0.45f;
        [SerializeField, Range(0.38f, 1.60f)] private float splashUpVelocityMin = 0.62f;
        [SerializeField, Range(0.50f, 1.90f)] private float splashUpVelocityMax = 1.16f;
        [SerializeField, Range(0.20f, 1.60f)] private float splashGravity = 0.84f;
        [SerializeField, Range(4f, 22f)] private float mistEmissionRate = 10.8f;
        [SerializeField, Range(1.20f, 3.60f)] private float mistLifetime = 2.35f;
        [SerializeField, Range(0.08f, 0.52f)] private float mistStartSizeMin = 0.18f;
        [SerializeField, Range(0.16f, 0.82f)] private float mistStartSizeMax = 0.44f;
        [SerializeField, Range(0.04f, 0.45f)] private float mistRiseVelocity = 0.18f;
        [SerializeField, Range(0.00f, 0.36f)] private float mistOutwardVelocity = 0.12f;
        [SerializeField, Range(0.00f, 0.45f)] private float mistNoiseStrength = 0.14f;
        [SerializeField, Range(0.04f, 1.20f)] private float mistNoiseFrequency = 0.32f;
        [SerializeField, Range(0.04f, 0.50f)] private float mistPeakAlpha = 0.22f;
        [SerializeField, Range(0.20f, 2.20f)] private float softParticleFarFade = 0.90f;
        [SerializeField, Range(0.10f, 1.00f)] private float waterRayHeight = 0.48f;
        [SerializeField, Range(0.10f, 1.40f)] private float waterRayDistance = 0.90f;
        [SerializeField, Range(8f, 60f)] private float distanceCullFarMeters = 38f;
        [SerializeField, Range(1f, 2.25f)] private float strongerOptionMultiplier = 1.34f;
        [SerializeField, ColorUsage(false, true)] private Color rippleTint = new Color(0.68f, 0.94f, 1.15f, 0.30f);
        [SerializeField, ColorUsage(false, true)] private Color splashTint = new Color(0.82f, 1.04f, 1.16f, 0.72f);
        [SerializeField, ColorUsage(false, true)] private Color mistTint = new Color(0.70f, 0.86f, 0.94f, 0.22f);
        [SerializeField] private bool subEmitterBirth = true;
        [SerializeField] private bool raycastEntryHookPrepared = true;
        [SerializeField] private bool continuousMistEmitter = true;
        [SerializeField] private bool softParticlesRequired = true;
        [SerializeField] private bool fadeCompleteWithinOneSecond = true;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalWaterSplashRippleApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep this conservative P2-69 water FX data prep. Tom should tune final ripple ring thickness, droplet crown density, mist opacity, and placement against approved water-body art and lighting.";

        public int MaxRippleParticlesForReview => maxRippleParticles;
        public int SplashBurstParticlesForReview => splashBurstParticles;
        public int MaxMistParticlesForReview => maxMistParticles;
        public float RippleLifetimeForReview => rippleLifetime;
        public float RippleStartSizeForReview => Mathf.Min(rippleStartSize, rippleEndSize);
        public float RippleEndSizeForReview => Mathf.Max(rippleStartSize, rippleEndSize);
        public float RipplePeakAlphaForReview => ripplePeakAlpha;
        public float SplashLifetimeForReview => splashLifetime;
        public float SplashStartSizeMinForReview => splashStartSizeMin;
        public float SplashStartSizeMaxForReview => Mathf.Max(splashStartSizeMin, splashStartSizeMax);
        public float SplashHorizontalVelocityForReview => splashHorizontalVelocity;
        public float SplashUpVelocityMinForReview => Mathf.Min(splashUpVelocityMin, splashUpVelocityMax);
        public float SplashUpVelocityMaxForReview => Mathf.Max(splashUpVelocityMin, splashUpVelocityMax);
        public float SplashGravityForReview => splashGravity;
        public float MistEmissionRateForReview => mistEmissionRate;
        public float MistLifetimeForReview => mistLifetime;
        public float MistStartSizeMinForReview => mistStartSizeMin;
        public float MistStartSizeMaxForReview => Mathf.Max(mistStartSizeMin, mistStartSizeMax);
        public float MistRiseVelocityForReview => mistRiseVelocity;
        public float MistOutwardVelocityForReview => mistOutwardVelocity;
        public float MistNoiseStrengthForReview => mistNoiseStrength;
        public float MistNoiseFrequencyForReview => mistNoiseFrequency;
        public float MistPeakAlphaForReview => mistPeakAlpha;
        public float SoftParticleFarFadeForReview => softParticleFarFade;
        public float WaterRayHeightForReview => waterRayHeight;
        public float WaterRayDistanceForReview => waterRayDistance;
        public float DistanceCullFarMetersForReview => distanceCullFarMeters;
        public float StrongerOptionMultiplierForReview => strongerOptionMultiplier;
        public Color RippleTintForReview => rippleTint;
        public Color SplashTintForReview => splashTint;
        public Color MistTintForReview => mistTint;
        public bool SubEmitterBirthForReview => subEmitterBirth;
        public bool RaycastEntryHookPreparedForReview => raycastEntryHookPrepared;
        public bool ContinuousMistEmitterForReview => continuousMistEmitter;
        public bool SoftParticlesRequiredForReview => softParticlesRequired;
        public bool FadeCompleteWithinOneSecondForReview => fadeCompleteWithinOneSecond;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalWaterSplashRippleApprovedForReview => finalWaterSplashRippleApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;
        public int ExpectedMistParticleCountForReview => Mathf.Clamp(Mathf.RoundToInt(MistEmissionRateForReview * MistLifetimeForReview), 0, MaxMistParticlesForReview);

        public void ConfigureForReview(
            int configuredMaxRippleParticles,
            int configuredSplashBurstParticles,
            int configuredMaxMistParticles,
            float configuredRippleLifetime,
            float configuredRippleStartSize,
            float configuredRippleEndSize,
            float configuredRipplePeakAlpha,
            float configuredSplashLifetime,
            float configuredSplashStartSizeMin,
            float configuredSplashStartSizeMax,
            float configuredSplashHorizontalVelocity,
            float configuredSplashUpVelocityMin,
            float configuredSplashUpVelocityMax,
            float configuredSplashGravity,
            float configuredMistEmissionRate,
            float configuredMistLifetime,
            float configuredMistStartSizeMin,
            float configuredMistStartSizeMax,
            float configuredMistRiseVelocity,
            float configuredMistOutwardVelocity,
            float configuredMistNoiseStrength,
            float configuredMistNoiseFrequency,
            float configuredMistPeakAlpha,
            float configuredSoftParticleFarFade,
            float configuredWaterRayHeight,
            float configuredWaterRayDistance,
            float configuredDistanceCullFarMeters,
            float configuredStrongerOptionMultiplier,
            Color configuredRippleTint,
            Color configuredSplashTint,
            Color configuredMistTint,
            bool configuredSubEmitterBirth,
            bool configuredRaycastEntryHookPrepared,
            bool configuredContinuousMistEmitter,
            bool configuredSoftParticlesRequired,
            bool configuredFadeCompleteWithinOneSecond,
            bool configuredConservativeDataPrep,
            bool configuredNeedsTomApproval,
            bool configuredFinalWaterSplashRippleApproved,
            string configuredRecommendation)
        {
            maxRippleParticles = Mathf.Clamp(configuredMaxRippleParticles, 4, 18);
            splashBurstParticles = Mathf.Clamp(configuredSplashBurstParticles, 4, 18);
            maxMistParticles = Mathf.Clamp(configuredMaxMistParticles, 18, 48);
            rippleLifetime = Mathf.Clamp(configuredRippleLifetime, 0.55f, 1.25f);
            rippleStartSize = Mathf.Clamp(configuredRippleStartSize, 0.20f, 0.55f);
            rippleEndSize = Mathf.Clamp(configuredRippleEndSize, 0.55f, 1.35f);
            ripplePeakAlpha = Mathf.Clamp(configuredRipplePeakAlpha, 0.02f, 0.45f);
            splashLifetime = Mathf.Clamp(configuredSplashLifetime, 0.22f, 0.82f);
            splashStartSizeMin = Mathf.Clamp(configuredSplashStartSizeMin, 0.020f, 0.080f);
            splashStartSizeMax = Mathf.Clamp(Mathf.Max(configuredSplashStartSizeMin, configuredSplashStartSizeMax), 0.035f, 0.120f);
            splashHorizontalVelocity = Mathf.Clamp(configuredSplashHorizontalVelocity, 0.28f, 1.20f);
            splashUpVelocityMin = Mathf.Clamp(Mathf.Min(configuredSplashUpVelocityMin, configuredSplashUpVelocityMax), 0.38f, 1.60f);
            splashUpVelocityMax = Mathf.Clamp(Mathf.Max(configuredSplashUpVelocityMin, configuredSplashUpVelocityMax), 0.50f, 1.90f);
            splashGravity = Mathf.Clamp(configuredSplashGravity, 0.20f, 1.60f);
            mistEmissionRate = Mathf.Clamp(configuredMistEmissionRate, 4f, 22f);
            mistLifetime = Mathf.Clamp(configuredMistLifetime, 1.20f, 3.60f);
            mistStartSizeMin = Mathf.Clamp(configuredMistStartSizeMin, 0.08f, 0.52f);
            mistStartSizeMax = Mathf.Clamp(Mathf.Max(configuredMistStartSizeMin, configuredMistStartSizeMax), 0.16f, 0.82f);
            mistRiseVelocity = Mathf.Clamp(configuredMistRiseVelocity, 0.04f, 0.45f);
            mistOutwardVelocity = Mathf.Clamp(configuredMistOutwardVelocity, 0f, 0.36f);
            mistNoiseStrength = Mathf.Clamp(configuredMistNoiseStrength, 0f, 0.45f);
            mistNoiseFrequency = Mathf.Clamp(configuredMistNoiseFrequency, 0.04f, 1.20f);
            mistPeakAlpha = Mathf.Clamp(configuredMistPeakAlpha, 0.04f, 0.50f);
            softParticleFarFade = Mathf.Clamp(configuredSoftParticleFarFade, 0.20f, 2.20f);
            waterRayHeight = Mathf.Clamp(configuredWaterRayHeight, 0.10f, 1.00f);
            waterRayDistance = Mathf.Clamp(configuredWaterRayDistance, 0.10f, 1.40f);
            distanceCullFarMeters = Mathf.Clamp(configuredDistanceCullFarMeters, 8f, 60f);
            strongerOptionMultiplier = Mathf.Clamp(configuredStrongerOptionMultiplier, 1f, 2.25f);
            rippleTint = configuredRippleTint;
            splashTint = configuredSplashTint;
            mistTint = configuredMistTint;
            subEmitterBirth = configuredSubEmitterBirth;
            raycastEntryHookPrepared = configuredRaycastEntryHookPrepared;
            continuousMistEmitter = configuredContinuousMistEmitter;
            softParticlesRequired = configuredSoftParticlesRequired;
            fadeCompleteWithinOneSecond = configuredFadeCompleteWithinOneSecond;
            conservativeDataPrep = configuredConservativeDataPrep;
            needsTomApproval = configuredNeedsTomApproval;
            finalWaterSplashRippleApproved = configuredFinalWaterSplashRippleApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
