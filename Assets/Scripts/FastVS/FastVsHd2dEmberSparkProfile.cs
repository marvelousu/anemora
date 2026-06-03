using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dEmberSparkSourceKind
    {
        LibraryTorch,
        Cookfire,
        PorchTorch
    }

    [CreateAssetMenu(fileName = "FastVsHd2dEmberSparkProfile", menuName = "Anemora/HD2D/Ember Spark Profile")]
    public sealed class FastVsHd2dEmberSparkProfile : ScriptableObject
    {
        [SerializeField, Range(10, 30)] private int maxParticlesPerEmitter = 28;
        [SerializeField, Range(1f, 30f)] private float libraryTorchEmissionRate = 13.5f;
        [SerializeField, Range(1f, 30f)] private float cookfireEmissionRate = 18.0f;
        [SerializeField, Range(1f, 30f)] private float porchTorchEmissionRate = 12.0f;
        [SerializeField, Range(0.55f, 2.20f)] private float emberLifetime = 1.35f;
        [SerializeField, Range(1.0f, 3.5f)] private float systemDuration = 2.25f;
        [SerializeField, Range(0.010f, 0.090f)] private float startSizeMin = 0.024f;
        [SerializeField, Range(0.014f, 0.120f)] private float startSizeMax = 0.058f;
        [SerializeField, Range(0.15f, 1.60f)] private float riseVelocityMin = 0.58f;
        [SerializeField, Range(0.20f, 1.80f)] private float riseVelocityMax = 0.86f;
        [SerializeField, Range(0f, 0.60f)] private float lateralDriftVelocity = 0.16f;
        [SerializeField, Range(0f, 0.45f)] private float noiseStrength = 0.105f;
        [SerializeField, Range(0.02f, 1.00f)] private float noiseFrequency = 0.34f;
        [SerializeField, Range(0f, 0.75f)] private float buoyancyEaseOutDamping = 0.18f;
        [SerializeField, Range(0.2f, 8f)] private float flickerFrequency = 2.35f;
        [SerializeField, Range(0f, 0.80f)] private float flickerAmplitude = 0.34f;
        [SerializeField, Range(8f, 60f)] private float distanceCullFarMeters = 34f;
        [SerializeField, Range(1f, 2.5f)] private float strongerOptionMultiplier = 1.42f;
        [SerializeField, ColorUsage(false, true)] private Color hotCoreColor = new Color(2.15f, 1.14f, 0.34f, 1f);
        [SerializeField, ColorUsage(false, true)] private Color hotFlickerColor = new Color(3.15f, 1.68f, 0.48f, 1f);
        [SerializeField, ColorUsage(false, true)] private Color darkRedFadeColor = new Color(0.44f, 0.075f, 0.018f, 1f);
        [SerializeField, ColorUsage(false, true)] private Color pointLightColor = new Color(1.00f, 0.58f, 0.24f, 1f);
        [SerializeField, Range(1f, 6f)] private float hdrIntensity = 2.65f;
        [SerializeField, Range(0f, 1.0f)] private float pointLightIntensity = 0.11f;
        [SerializeField, Range(0.20f, 3.0f)] private float pointLightRange = 1.24f;
        [SerializeField] private bool loopingShuriken = true;
        [SerializeField] private bool additiveHdrMaterial = true;
        [SerializeField] private bool softParticlesRequired = true;
        [SerializeField] private bool upwardFadeBeforeHeight = true;
        [SerializeField] private bool flickerPointLightsEnabled = true;
        [SerializeField] private bool distanceCullFarEmitters = true;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalEmberSparkApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep the conservative HDR additive ember baseline as data prep. Tom should tune final density, bloom intensity, spark size, and point-light flicker after checking it against the approved night grade.";

        public int MaxParticlesPerEmitterForReview => maxParticlesPerEmitter;
        public float LibraryTorchEmissionRateForReview => libraryTorchEmissionRate;
        public float CookfireEmissionRateForReview => cookfireEmissionRate;
        public float PorchTorchEmissionRateForReview => porchTorchEmissionRate;
        public float EmberLifetimeForReview => emberLifetime;
        public float SystemDurationForReview => systemDuration;
        public float StartSizeMinForReview => startSizeMin;
        public float StartSizeMaxForReview => Mathf.Max(startSizeMin, startSizeMax);
        public float RiseVelocityMinForReview => Mathf.Min(riseVelocityMin, riseVelocityMax);
        public float RiseVelocityMaxForReview => Mathf.Max(riseVelocityMin, riseVelocityMax);
        public float LateralDriftVelocityForReview => lateralDriftVelocity;
        public float NoiseStrengthForReview => noiseStrength;
        public float NoiseFrequencyForReview => noiseFrequency;
        public float BuoyancyEaseOutDampingForReview => buoyancyEaseOutDamping;
        public float FlickerFrequencyForReview => flickerFrequency;
        public float FlickerAmplitudeForReview => flickerAmplitude;
        public float DistanceCullFarMetersForReview => distanceCullFarMeters;
        public float StrongerOptionMultiplierForReview => strongerOptionMultiplier;
        public Color HotCoreColorForReview => hotCoreColor;
        public Color HotFlickerColorForReview => hotFlickerColor;
        public Color DarkRedFadeColorForReview => darkRedFadeColor;
        public Color PointLightColorForReview => pointLightColor;
        public float HdrIntensityForReview => hdrIntensity;
        public float PointLightIntensityForReview => pointLightIntensity;
        public float PointLightRangeForReview => pointLightRange;
        public bool LoopingShurikenForReview => loopingShuriken;
        public bool AdditiveHdrMaterialForReview => additiveHdrMaterial;
        public bool SoftParticlesRequiredForReview => softParticlesRequired;
        public bool UpwardFadeBeforeHeightForReview => upwardFadeBeforeHeight;
        public bool FlickerPointLightsEnabledForReview => flickerPointLightsEnabled;
        public bool DistanceCullFarEmittersForReview => distanceCullFarEmitters;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalEmberSparkApprovedForReview => finalEmberSparkApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;
        public float ExpectedUpperFadeHeightMetersForReview => EmberLifetimeForReview * RiseVelocityMaxForReview;

        public void ConfigureForReview(
            int configuredMaxParticlesPerEmitter,
            float configuredLibraryTorchEmissionRate,
            float configuredCookfireEmissionRate,
            float configuredPorchTorchEmissionRate,
            float configuredEmberLifetime,
            float configuredSystemDuration,
            float configuredStartSizeMin,
            float configuredStartSizeMax,
            float configuredRiseVelocityMin,
            float configuredRiseVelocityMax,
            float configuredLateralDriftVelocity,
            float configuredNoiseStrength,
            float configuredNoiseFrequency,
            float configuredBuoyancyEaseOutDamping,
            float configuredFlickerFrequency,
            float configuredFlickerAmplitude,
            float configuredDistanceCullFarMeters,
            float configuredStrongerOptionMultiplier,
            Color configuredHotCoreColor,
            Color configuredHotFlickerColor,
            Color configuredDarkRedFadeColor,
            Color configuredPointLightColor,
            float configuredHdrIntensity,
            float configuredPointLightIntensity,
            float configuredPointLightRange,
            bool configuredLoopingShuriken,
            bool configuredAdditiveHdrMaterial,
            bool configuredSoftParticlesRequired,
            bool configuredUpwardFadeBeforeHeight,
            bool configuredFlickerPointLightsEnabled,
            bool configuredDistanceCullFarEmitters,
            bool configuredConservativeDataPrep,
            bool configuredNeedsTomApproval,
            bool configuredFinalEmberSparkApproved,
            string configuredRecommendation)
        {
            maxParticlesPerEmitter = Mathf.Clamp(configuredMaxParticlesPerEmitter, 10, 30);
            libraryTorchEmissionRate = Mathf.Clamp(configuredLibraryTorchEmissionRate, 1f, 30f);
            cookfireEmissionRate = Mathf.Clamp(configuredCookfireEmissionRate, 1f, 30f);
            porchTorchEmissionRate = Mathf.Clamp(configuredPorchTorchEmissionRate, 1f, 30f);
            emberLifetime = Mathf.Clamp(configuredEmberLifetime, 0.55f, 2.20f);
            systemDuration = Mathf.Clamp(configuredSystemDuration, 1.0f, 3.5f);
            startSizeMin = Mathf.Clamp(configuredStartSizeMin, 0.010f, 0.090f);
            startSizeMax = Mathf.Clamp(Mathf.Max(configuredStartSizeMin, configuredStartSizeMax), 0.014f, 0.120f);
            riseVelocityMin = Mathf.Clamp(Mathf.Min(configuredRiseVelocityMin, configuredRiseVelocityMax), 0.15f, 1.60f);
            riseVelocityMax = Mathf.Clamp(Mathf.Max(configuredRiseVelocityMin, configuredRiseVelocityMax), 0.20f, 1.80f);
            lateralDriftVelocity = Mathf.Clamp(configuredLateralDriftVelocity, 0f, 0.60f);
            noiseStrength = Mathf.Clamp(configuredNoiseStrength, 0f, 0.45f);
            noiseFrequency = Mathf.Clamp(configuredNoiseFrequency, 0.02f, 1.00f);
            buoyancyEaseOutDamping = Mathf.Clamp01(configuredBuoyancyEaseOutDamping);
            flickerFrequency = Mathf.Clamp(configuredFlickerFrequency, 0.2f, 8f);
            flickerAmplitude = Mathf.Clamp(configuredFlickerAmplitude, 0f, 0.80f);
            distanceCullFarMeters = Mathf.Clamp(configuredDistanceCullFarMeters, 8f, 60f);
            strongerOptionMultiplier = Mathf.Clamp(configuredStrongerOptionMultiplier, 1f, 2.5f);
            hotCoreColor = configuredHotCoreColor;
            hotFlickerColor = configuredHotFlickerColor;
            darkRedFadeColor = configuredDarkRedFadeColor;
            pointLightColor = configuredPointLightColor;
            hdrIntensity = Mathf.Clamp(configuredHdrIntensity, 1f, 6f);
            pointLightIntensity = Mathf.Clamp(configuredPointLightIntensity, 0f, 1.0f);
            pointLightRange = Mathf.Clamp(configuredPointLightRange, 0.20f, 3.0f);
            loopingShuriken = configuredLoopingShuriken;
            additiveHdrMaterial = configuredAdditiveHdrMaterial;
            softParticlesRequired = configuredSoftParticlesRequired;
            upwardFadeBeforeHeight = configuredUpwardFadeBeforeHeight;
            flickerPointLightsEnabled = configuredFlickerPointLightsEnabled;
            distanceCullFarEmitters = configuredDistanceCullFarEmitters;
            conservativeDataPrep = configuredConservativeDataPrep;
            needsTomApproval = configuredNeedsTomApproval;
            finalEmberSparkApproved = configuredFinalEmberSparkApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }

        public float ResolveEmissionRateForReview(FastVsHd2dEmberSparkSourceKind sourceKind)
        {
            return sourceKind switch
            {
                FastVsHd2dEmberSparkSourceKind.Cookfire => cookfireEmissionRate,
                FastVsHd2dEmberSparkSourceKind.PorchTorch => porchTorchEmissionRate,
                _ => libraryTorchEmissionRate
            };
        }

        public float ResolveFlickerPhaseForReview(FastVsHd2dEmberSparkSourceKind sourceKind)
        {
            return sourceKind switch
            {
                FastVsHd2dEmberSparkSourceKind.Cookfire => 1.73f,
                FastVsHd2dEmberSparkSourceKind.PorchTorch => 3.41f,
                _ => 0.42f
            };
        }

        public int ResolveExpectedParticleCountForReview(FastVsHd2dEmberSparkSourceKind sourceKind)
        {
            return Mathf.Clamp(Mathf.RoundToInt(ResolveEmissionRateForReview(sourceKind) * EmberLifetimeForReview), 0, maxParticlesPerEmitter);
        }
    }
}
