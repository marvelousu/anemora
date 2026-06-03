using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Ambient Dust Pollen Profile")]
    public sealed class FastVsHd2dAmbientDustPollenProfile : ScriptableObject
    {
        [SerializeField, Min(80)] private int maxParticles = 144;
        [SerializeField, Min(0f)] private float duration = 12f;
        [SerializeField, Min(0f)] private float lifetime = 10.5f;
        [SerializeField, Min(0f)] private float emissionRate = 13.5f;
        [SerializeField, Min(0f)] private float startSpeedMin = 0.012f;
        [SerializeField, Min(0f)] private float startSpeedMax = 0.042f;
        [SerializeField, Min(0f)] private float startSizeMin = 0.024f;
        [SerializeField, Min(0f)] private float startSizeMax = 0.062f;
        [SerializeField] private Vector3 cameraLocalOffset = new Vector3(0f, 0.18f, 6.0f);
        [SerializeField] private Vector3 boxSize = new Vector3(15.5f, 4.2f, 18.0f);
        [SerializeField] private Vector3 worldWindVelocity = new Vector3(0.018f, 0.014f, -0.010f);
        [SerializeField, Range(0f, 1f)] private float randomDirectionAmount = 0.16f;
        [SerializeField, Min(0f)] private float noiseStrength = 0.034f;
        [SerializeField, Min(0f)] private float noiseFrequency = 0.18f;
        [SerializeField, Range(0f, 1f)] private float defaultTintWarmth = 0.38f;
        [SerializeField] private Color coolShadeTint = new Color(0.76f, 0.84f, 0.90f, 0.082f);
        [SerializeField] private Color warmPollenTint = new Color(1.00f, 0.82f, 0.54f, 0.104f);
        [SerializeField, Range(1f, 4f)] private float reviewCaptureEmissionMultiplier = 2.15f;
        [SerializeField] private bool cameraAttached = true;
        [SerializeField] private bool simulationSpaceWorld = true;
        [SerializeField] private bool cpuShuriken = true;
        [SerializeField] private bool independentOfSunShafts = true;
        [SerializeField] private bool conservativeReviewMode = true;
        [SerializeField] private bool requiresTomArtApproval = true;
        [SerializeField] private string sourceNote = "Procedural CPU Shuriken camera-attached ambient dust/pollen review layer; Tom should tune final density, tint, and grade.";

        public int MaxParticlesForReview => maxParticles;
        public float DurationForReview => duration;
        public float LifetimeForReview => lifetime;
        public float EmissionRateForReview => emissionRate;
        public float StartSpeedMinForReview => startSpeedMin;
        public float StartSpeedMaxForReview => startSpeedMax;
        public float StartSizeMinForReview => startSizeMin;
        public float StartSizeMaxForReview => startSizeMax;
        public Vector3 CameraLocalOffsetForReview => cameraLocalOffset;
        public Vector3 BoxSizeForReview => boxSize;
        public Vector3 WorldWindVelocityForReview => worldWindVelocity;
        public float RandomDirectionAmountForReview => randomDirectionAmount;
        public float NoiseStrengthForReview => noiseStrength;
        public float NoiseFrequencyForReview => noiseFrequency;
        public float DefaultTintWarmthForReview => defaultTintWarmth;
        public Color CoolShadeTintForReview => coolShadeTint;
        public Color WarmPollenTintForReview => warmPollenTint;
        public float AlphaCeilingForReview => Mathf.Max(coolShadeTint.a, warmPollenTint.a);
        public float ReviewCaptureEmissionMultiplierForReview => reviewCaptureEmissionMultiplier;
        public bool CameraAttachedForReview => cameraAttached;
        public bool SimulationSpaceWorldForReview => simulationSpaceWorld;
        public bool CpuShurikenForReview => cpuShuriken;
        public bool IndependentOfSunShaftsForReview => independentOfSunShafts;
        public bool ConservativeReviewModeForReview => conservativeReviewMode;
        public bool RequiresTomArtApprovalForReview => requiresTomArtApproval;
        public string SourceNoteForReview => sourceNote;

        public void ConfigureForReview(
            int configuredMaxParticles,
            float configuredDuration,
            float configuredLifetime,
            float configuredEmissionRate,
            float configuredStartSpeedMin,
            float configuredStartSpeedMax,
            float configuredStartSizeMin,
            float configuredStartSizeMax,
            Vector3 configuredCameraLocalOffset,
            Vector3 configuredBoxSize,
            Vector3 configuredWorldWindVelocity,
            float configuredRandomDirectionAmount,
            float configuredNoiseStrength,
            float configuredNoiseFrequency,
            float configuredDefaultTintWarmth,
            Color configuredCoolShadeTint,
            Color configuredWarmPollenTint,
            float configuredReviewCaptureEmissionMultiplier,
            bool configuredCameraAttached,
            bool configuredSimulationSpaceWorld,
            bool configuredCpuShuriken,
            bool configuredIndependentOfSunShafts,
            bool configuredConservativeReviewMode,
            bool configuredRequiresTomArtApproval,
            string configuredSourceNote)
        {
            maxParticles = Mathf.Clamp(configuredMaxParticles, 80, 200);
            duration = Mathf.Max(1f, configuredDuration);
            lifetime = Mathf.Max(1f, configuredLifetime);
            emissionRate = Mathf.Max(0f, configuredEmissionRate);
            startSpeedMin = Mathf.Max(0f, Mathf.Min(configuredStartSpeedMin, configuredStartSpeedMax));
            startSpeedMax = Mathf.Max(startSpeedMin, configuredStartSpeedMax);
            startSizeMin = Mathf.Max(0.001f, Mathf.Min(configuredStartSizeMin, configuredStartSizeMax));
            startSizeMax = Mathf.Max(startSizeMin, configuredStartSizeMax);
            cameraLocalOffset = configuredCameraLocalOffset;
            boxSize = new Vector3(
                Mathf.Max(1f, configuredBoxSize.x),
                Mathf.Max(1f, configuredBoxSize.y),
                Mathf.Max(1f, configuredBoxSize.z));
            worldWindVelocity = configuredWorldWindVelocity;
            randomDirectionAmount = Mathf.Clamp01(configuredRandomDirectionAmount);
            noiseStrength = Mathf.Max(0f, configuredNoiseStrength);
            noiseFrequency = Mathf.Max(0f, configuredNoiseFrequency);
            defaultTintWarmth = Mathf.Clamp01(configuredDefaultTintWarmth);
            coolShadeTint = ClampReviewAlpha(configuredCoolShadeTint);
            warmPollenTint = ClampReviewAlpha(configuredWarmPollenTint);
            reviewCaptureEmissionMultiplier = Mathf.Clamp(configuredReviewCaptureEmissionMultiplier, 1f, 4f);
            cameraAttached = configuredCameraAttached;
            simulationSpaceWorld = configuredSimulationSpaceWorld;
            cpuShuriken = configuredCpuShuriken;
            independentOfSunShafts = configuredIndependentOfSunShafts;
            conservativeReviewMode = configuredConservativeReviewMode;
            requiresTomArtApproval = configuredRequiresTomArtApproval;
            sourceNote = configuredSourceNote ?? string.Empty;
        }

        private static Color ClampReviewAlpha(Color color)
        {
            color.a = Mathf.Clamp(color.a, 0.01f, 0.12f);
            return color;
        }
    }
}
