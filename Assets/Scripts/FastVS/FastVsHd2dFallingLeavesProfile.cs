using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dFallingLeavesBiome
    {
        GreenLeaf = 0,
        PinkPetal = 1
    }

    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Falling Leaves Profile")]
    public sealed class FastVsHd2dFallingLeavesProfile : ScriptableObject
    {
        [SerializeField, Range(5, 15)] private int totalMaxParticles = 15;
        [SerializeField, Range(2, 6)] private int foregroundMaxParticles = 5;
        [SerializeField, Range(3, 10)] private int midDepthMaxParticles = 10;
        [SerializeField, Min(0f)] private float duration = 9.5f;
        [SerializeField, Min(0f)] private float lifetime = 7.8f;
        [SerializeField, Min(0f)] private float emissionRate = 1.75f;
        [SerializeField, Range(1f, 3f)] private float reviewCaptureEmissionMultiplier = 1.65f;
        [SerializeField] private Vector3 cameraLocalOffset = Vector3.zero;
        [SerializeField] private Vector3 foregroundLocalCenter = new Vector3(0f, 1.12f, 2.75f);
        [SerializeField] private Vector3 midDepthLocalCenter = new Vector3(0f, 1.62f, 6.85f);
        [SerializeField] private Vector3 foregroundBoxSize = new Vector3(7.4f, 1.7f, 1.4f);
        [SerializeField] private Vector3 midDepthBoxSize = new Vector3(10.8f, 2.4f, 3.0f);
        [SerializeField] private Vector3 localFallVelocity = new Vector3(0.10f, -0.46f, -0.03f);
        [SerializeField, Min(0f)] private float gravityModifier = 0.018f;
        [SerializeField, Min(0f)] private float noiseStrength = 0.28f;
        [SerializeField, Min(0f)] private float noiseFrequency = 0.22f;
        [SerializeField, Range(0f, 1f)] private float randomDirectionAmount = 0.20f;
        [SerializeField, Min(0f)] private float rotationOverLifetimeRadians = 2.8f;
        [SerializeField, Min(0f)] private float rotationBySpeedRadians = 1.45f;
        [SerializeField] private Color greenLeafTint = new Color(0.84f, 1.00f, 0.46f, 0.88f);
        [SerializeField] private Color pinkPetalTint = new Color(1.00f, 0.56f, 0.82f, 0.86f);
        [SerializeField] private string greenLeafBiomeId = "green_leaf";
        [SerializeField] private string pinkPetalBiomeId = "pink_petal";
        [SerializeField] private Material greenLeafMaterial;
        [SerializeField] private Material pinkPetalMaterial;
        [SerializeField] private Texture2D greenLeafTexture;
        [SerializeField] private Texture2D pinkPetalTexture;
        [SerializeField] private FastVsHd2dFallingLeavesBiome defaultBiome = FastVsHd2dFallingLeavesBiome.GreenLeaf;
        [SerializeField] private bool cameraAttached = true;
        [SerializeField] private bool biomeSwappable = true;
        [SerializeField] private bool foregroundBokehLayer = true;
        [SerializeField] private bool cpuShuriken = true;
        [SerializeField] private bool simulationSpaceWorld = true;
        [SerializeField] private bool conservativeReviewMode = true;
        [SerializeField] private bool requiresTomArtApproval = true;
        [SerializeField] private string sourceNote = "Procedural CC0-safe leaf/petal sprites and CPU Shuriken camera overlay; Tom should tune final sprite art, density, wind, tilt-shift blur, and biome mapping.";

        public int TotalMaxParticlesForReview => totalMaxParticles;
        public int ForegroundMaxParticlesForReview => foregroundMaxParticles;
        public int MidDepthMaxParticlesForReview => midDepthMaxParticles;
        public float DurationForReview => duration;
        public float LifetimeForReview => lifetime;
        public float EmissionRateForReview => emissionRate;
        public float ReviewCaptureEmissionMultiplierForReview => reviewCaptureEmissionMultiplier;
        public Vector3 CameraLocalOffsetForReview => cameraLocalOffset;
        public Vector3 ForegroundLocalCenterForReview => foregroundLocalCenter;
        public Vector3 MidDepthLocalCenterForReview => midDepthLocalCenter;
        public Vector3 ForegroundBoxSizeForReview => foregroundBoxSize;
        public Vector3 MidDepthBoxSizeForReview => midDepthBoxSize;
        public Vector3 LocalFallVelocityForReview => localFallVelocity;
        public float GravityModifierForReview => gravityModifier;
        public float NoiseStrengthForReview => noiseStrength;
        public float NoiseFrequencyForReview => noiseFrequency;
        public float RandomDirectionAmountForReview => randomDirectionAmount;
        public float RotationOverLifetimeRadiansForReview => rotationOverLifetimeRadians;
        public float RotationBySpeedRadiansForReview => rotationBySpeedRadians;
        public Color GreenLeafTintForReview => greenLeafTint;
        public Color PinkPetalTintForReview => pinkPetalTint;
        public string GreenLeafBiomeIdForReview => greenLeafBiomeId;
        public string PinkPetalBiomeIdForReview => pinkPetalBiomeId;
        public Material GreenLeafMaterialForReview => greenLeafMaterial;
        public Material PinkPetalMaterialForReview => pinkPetalMaterial;
        public Texture2D GreenLeafTextureForReview => greenLeafTexture;
        public Texture2D PinkPetalTextureForReview => pinkPetalTexture;
        public FastVsHd2dFallingLeavesBiome DefaultBiomeForReview => defaultBiome;
        public int BiomeRecordCountForReview => CountBiomeRecords();
        public bool HasBothBiomeSpriteRecordsForReview => greenLeafMaterial != null && pinkPetalMaterial != null && greenLeafTexture != null && pinkPetalTexture != null;
        public bool CameraAttachedForReview => cameraAttached;
        public bool BiomeSwappableForReview => biomeSwappable;
        public bool ForegroundBokehLayerForReview => foregroundBokehLayer;
        public bool CpuShurikenForReview => cpuShuriken;
        public bool SimulationSpaceWorldForReview => simulationSpaceWorld;
        public bool ConservativeReviewModeForReview => conservativeReviewMode;
        public bool RequiresTomArtApprovalForReview => requiresTomArtApproval;
        public string SourceNoteForReview => sourceNote;

        public Color ResolveTintForReview(FastVsHd2dFallingLeavesBiome biome)
        {
            return biome == FastVsHd2dFallingLeavesBiome.PinkPetal ? pinkPetalTint : greenLeafTint;
        }

        public Material ResolveMaterialForReview(FastVsHd2dFallingLeavesBiome biome)
        {
            return biome == FastVsHd2dFallingLeavesBiome.PinkPetal ? pinkPetalMaterial : greenLeafMaterial;
        }

        public Texture2D ResolveTextureForReview(FastVsHd2dFallingLeavesBiome biome)
        {
            return biome == FastVsHd2dFallingLeavesBiome.PinkPetal ? pinkPetalTexture : greenLeafTexture;
        }

        public string ResolveBiomeIdForReview(FastVsHd2dFallingLeavesBiome biome)
        {
            return biome == FastVsHd2dFallingLeavesBiome.PinkPetal ? pinkPetalBiomeId : greenLeafBiomeId;
        }

        public void ConfigureForReview(
            int configuredTotalMaxParticles,
            int configuredForegroundMaxParticles,
            int configuredMidDepthMaxParticles,
            float configuredDuration,
            float configuredLifetime,
            float configuredEmissionRate,
            float configuredReviewCaptureEmissionMultiplier,
            Vector3 configuredCameraLocalOffset,
            Vector3 configuredForegroundLocalCenter,
            Vector3 configuredMidDepthLocalCenter,
            Vector3 configuredForegroundBoxSize,
            Vector3 configuredMidDepthBoxSize,
            Vector3 configuredLocalFallVelocity,
            float configuredGravityModifier,
            float configuredNoiseStrength,
            float configuredNoiseFrequency,
            float configuredRandomDirectionAmount,
            float configuredRotationOverLifetimeRadians,
            float configuredRotationBySpeedRadians,
            Color configuredGreenLeafTint,
            Color configuredPinkPetalTint,
            string configuredGreenLeafBiomeId,
            string configuredPinkPetalBiomeId,
            Material configuredGreenLeafMaterial,
            Material configuredPinkPetalMaterial,
            Texture2D configuredGreenLeafTexture,
            Texture2D configuredPinkPetalTexture,
            FastVsHd2dFallingLeavesBiome configuredDefaultBiome,
            bool configuredCameraAttached,
            bool configuredBiomeSwappable,
            bool configuredForegroundBokehLayer,
            bool configuredCpuShuriken,
            bool configuredSimulationSpaceWorld,
            bool configuredConservativeReviewMode,
            bool configuredRequiresTomArtApproval,
            string configuredSourceNote)
        {
            totalMaxParticles = Mathf.Clamp(configuredTotalMaxParticles, 5, 15);
            foregroundMaxParticles = Mathf.Clamp(configuredForegroundMaxParticles, 2, 6);
            midDepthMaxParticles = Mathf.Clamp(configuredMidDepthMaxParticles, 3, 10);
            if (foregroundMaxParticles + midDepthMaxParticles != totalMaxParticles)
            {
                midDepthMaxParticles = Mathf.Clamp(totalMaxParticles - foregroundMaxParticles, 3, 10);
                foregroundMaxParticles = Mathf.Clamp(totalMaxParticles - midDepthMaxParticles, 2, 6);
            }

            duration = Mathf.Max(1f, configuredDuration);
            lifetime = Mathf.Max(1f, configuredLifetime);
            emissionRate = Mathf.Max(0f, configuredEmissionRate);
            reviewCaptureEmissionMultiplier = Mathf.Clamp(configuredReviewCaptureEmissionMultiplier, 1f, 3f);
            cameraLocalOffset = configuredCameraLocalOffset;
            foregroundLocalCenter = configuredForegroundLocalCenter;
            midDepthLocalCenter = configuredMidDepthLocalCenter;
            foregroundBoxSize = SanitizeBox(configuredForegroundBoxSize);
            midDepthBoxSize = SanitizeBox(configuredMidDepthBoxSize);
            localFallVelocity = configuredLocalFallVelocity;
            gravityModifier = Mathf.Max(0f, configuredGravityModifier);
            noiseStrength = Mathf.Max(0f, configuredNoiseStrength);
            noiseFrequency = Mathf.Max(0f, configuredNoiseFrequency);
            randomDirectionAmount = Mathf.Clamp01(configuredRandomDirectionAmount);
            rotationOverLifetimeRadians = Mathf.Max(0f, configuredRotationOverLifetimeRadians);
            rotationBySpeedRadians = Mathf.Max(0f, configuredRotationBySpeedRadians);
            greenLeafTint = ClampParticleTint(configuredGreenLeafTint);
            pinkPetalTint = ClampParticleTint(configuredPinkPetalTint);
            greenLeafBiomeId = string.IsNullOrWhiteSpace(configuredGreenLeafBiomeId) ? "green_leaf" : configuredGreenLeafBiomeId;
            pinkPetalBiomeId = string.IsNullOrWhiteSpace(configuredPinkPetalBiomeId) ? "pink_petal" : configuredPinkPetalBiomeId;
            greenLeafMaterial = configuredGreenLeafMaterial;
            pinkPetalMaterial = configuredPinkPetalMaterial;
            greenLeafTexture = configuredGreenLeafTexture;
            pinkPetalTexture = configuredPinkPetalTexture;
            defaultBiome = configuredDefaultBiome;
            cameraAttached = configuredCameraAttached;
            biomeSwappable = configuredBiomeSwappable;
            foregroundBokehLayer = configuredForegroundBokehLayer;
            cpuShuriken = configuredCpuShuriken;
            simulationSpaceWorld = configuredSimulationSpaceWorld;
            conservativeReviewMode = configuredConservativeReviewMode;
            requiresTomArtApproval = configuredRequiresTomArtApproval;
            sourceNote = configuredSourceNote ?? string.Empty;
        }

        private int CountBiomeRecords()
        {
            var count = 0;
            if (greenLeafMaterial != null && greenLeafTexture != null)
            {
                count++;
            }

            if (pinkPetalMaterial != null && pinkPetalTexture != null)
            {
                count++;
            }

            return count;
        }

        private static Vector3 SanitizeBox(Vector3 box)
        {
            return new Vector3(Mathf.Max(0.5f, box.x), Mathf.Max(0.5f, box.y), Mathf.Max(0.5f, box.z));
        }

        private static Color ClampParticleTint(Color color)
        {
            color.a = Mathf.Clamp(color.a, 0.25f, 0.9f);
            return color;
        }
    }
}
