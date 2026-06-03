using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dWaterReflectionProfile", menuName = "Anemora/HD2D/Water Reflection Profile")]
    public sealed class FastVsHd2dWaterReflectionProfile : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] private float reflectionStrength = 0.62f;
        [SerializeField, Range(0.25f, 6f)] private float fresnelPower = 1.65f;
        [SerializeField, Range(0f, 0.5f)] private float fresnelBias = 0.14f;
        [SerializeField, Range(0f, 1f)] private float roughness = 0.12f;
        [SerializeField] private Color reflectionTint = new Color(0.96f, 0.99f, 1.00f, 1f);
        [SerializeField, Range(0f, 0.6f)] private float skyFallback = 0.24f;
        [SerializeField] private Vector3 probeBoxSize = new Vector3(5.8f, 3.2f, 4.4f);
        [SerializeField, Range(16, 256)] private int probeResolution = 128;
        [SerializeField] private bool planarReflectionDeferred = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalWaterReflectionApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-64 boxed-probe reflection baseline using a custom review cubemap. Tom should tune strength, Fresnel bias, roughness, and decide which hero ponds justify planar reflection.";

        public float ReflectionStrengthForReview => Mathf.Clamp01(reflectionStrength);
        public float FresnelPowerForReview => Mathf.Clamp(fresnelPower, 0.25f, 6f);
        public float FresnelBiasForReview => Mathf.Clamp(fresnelBias, 0f, 0.5f);
        public float RoughnessForReview => Mathf.Clamp01(roughness);
        public Color ReflectionTintForReview => reflectionTint;
        public float SkyFallbackForReview => Mathf.Clamp(skyFallback, 0f, 0.6f);
        public Vector3 ProbeBoxSizeForReview => new Vector3(
            Mathf.Max(0.1f, probeBoxSize.x),
            Mathf.Max(0.1f, probeBoxSize.y),
            Mathf.Max(0.1f, probeBoxSize.z));
        public int ProbeResolutionForReview => Mathf.Clamp(probeResolution, 16, 256);
        public bool PlanarReflectionDeferredForReview => planarReflectionDeferred;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalWaterReflectionApprovedForReview => finalWaterReflectionApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            float configuredReflectionStrength,
            float configuredFresnelPower,
            float configuredFresnelBias,
            float configuredRoughness,
            Color configuredReflectionTint,
            float configuredSkyFallback,
            Vector3 configuredProbeBoxSize,
            int configuredProbeResolution,
            bool configuredPlanarReflectionDeferred,
            bool configuredNeedsTomApproval,
            bool configuredFinalWaterReflectionApproved,
            string configuredRecommendation)
        {
            reflectionStrength = Mathf.Clamp01(configuredReflectionStrength);
            fresnelPower = Mathf.Clamp(configuredFresnelPower, 0.25f, 6f);
            fresnelBias = Mathf.Clamp(configuredFresnelBias, 0f, 0.5f);
            roughness = Mathf.Clamp01(configuredRoughness);
            reflectionTint = configuredReflectionTint;
            skyFallback = Mathf.Clamp(configuredSkyFallback, 0f, 0.6f);
            probeBoxSize = new Vector3(
                Mathf.Max(0.1f, configuredProbeBoxSize.x),
                Mathf.Max(0.1f, configuredProbeBoxSize.y),
                Mathf.Max(0.1f, configuredProbeBoxSize.z));
            probeResolution = Mathf.Clamp(configuredProbeResolution, 16, 256);
            planarReflectionDeferred = configuredPlanarReflectionDeferred;
            needsTomApproval = configuredNeedsTomApproval;
            finalWaterReflectionApproved = configuredFinalWaterReflectionApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
