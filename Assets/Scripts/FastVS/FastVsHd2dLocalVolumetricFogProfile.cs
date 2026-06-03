using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dLocalVolumetricFogKind
    {
        InteriorRoom,
        LowValley,
        PortalThreshold
    }

    [CreateAssetMenu(fileName = "FastVsHd2dLocalVolumetricFogProfile", menuName = "Anemora/HD2D/Local Volumetric Fog Profile")]
    public sealed class FastVsHd2dLocalVolumetricFogProfile : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] private float interiorDensity = 0.17f;
        [SerializeField, Range(0f, 1f)] private float lowValleyDensity = 0.12f;
        [SerializeField, Range(0f, 1f)] private float portalClosedDensity = 0.00f;
        [SerializeField, Range(0f, 1f)] private float portalOpenDensity = 0.24f;
        [SerializeField, Range(1f, 2.5f)] private float portalStrongOptionMultiplier = 1.35f;
        [SerializeField, Range(0.05f, 0.95f)] private float edgeFeather = 0.42f;
        [SerializeField, Range(0f, 1f)] private float noiseStrength = 0.18f;
        [SerializeField, Range(0.2f, 12f)] private float noiseScale = 4.2f;
        [SerializeField, Range(0f, 1.5f)] private float heightFade = 0.34f;
        [SerializeField, Range(0f, 1f)] private float portalGlow = 0.20f;
        [SerializeField] private Color interiorColor = new Color(0.66f, 0.76f, 0.84f, 0.30f);
        [SerializeField] private Color lowValleyColor = new Color(0.58f, 0.72f, 0.78f, 0.28f);
        [SerializeField] private Color portalColor = new Color(0.70f, 0.92f, 1.00f, 0.38f);
        [SerializeField] private Vector3 interiorBounds = new Vector3(4.90f, 1.35f, 3.20f);
        [SerializeField] private Vector3 lowValleyBounds = new Vector3(4.40f, 0.58f, 1.45f);
        [SerializeField] private Vector3 portalBounds = new Vector3(2.15f, 1.65f, 0.32f);
        [SerializeField] private bool tiePortalDensityToOpenState = true;
        [SerializeField] private bool renderGraphNativePackageDeferred = true;
        [SerializeField] private bool legitimateVolumetricAssetDecisionRequired = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalLocalFogApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-65 local mist data prep. Tom should decide whether to buy a legitimate Render Graph-native volumetric package or keep this lightweight local-slice fallback, then tune portal/interior density against the approved camera and grade.";

        public float InteriorDensityForReview => interiorDensity;
        public float LowValleyDensityForReview => lowValleyDensity;
        public float PortalClosedDensityForReview => portalClosedDensity;
        public float PortalOpenDensityForReview => portalOpenDensity;
        public float PortalStrongOptionMultiplierForReview => portalStrongOptionMultiplier;
        public float EdgeFeatherForReview => edgeFeather;
        public float NoiseStrengthForReview => noiseStrength;
        public float NoiseScaleForReview => noiseScale;
        public float HeightFadeForReview => heightFade;
        public float PortalGlowForReview => portalGlow;
        public Color InteriorColorForReview => interiorColor;
        public Color LowValleyColorForReview => lowValleyColor;
        public Color PortalColorForReview => portalColor;
        public Vector3 InteriorBoundsForReview => interiorBounds;
        public Vector3 LowValleyBoundsForReview => lowValleyBounds;
        public Vector3 PortalBoundsForReview => portalBounds;
        public bool TiePortalDensityToOpenStateForReview => tiePortalDensityToOpenState;
        public bool RenderGraphNativePackageDeferredForReview => renderGraphNativePackageDeferred;
        public bool LegitimateVolumetricAssetDecisionRequiredForReview => legitimateVolumetricAssetDecisionRequired;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalLocalFogApprovedForReview => finalLocalFogApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            float configuredInteriorDensity,
            float configuredLowValleyDensity,
            float configuredPortalClosedDensity,
            float configuredPortalOpenDensity,
            float configuredPortalStrongOptionMultiplier,
            float configuredEdgeFeather,
            float configuredNoiseStrength,
            float configuredNoiseScale,
            float configuredHeightFade,
            float configuredPortalGlow,
            Color configuredInteriorColor,
            Color configuredLowValleyColor,
            Color configuredPortalColor,
            Vector3 configuredInteriorBounds,
            Vector3 configuredLowValleyBounds,
            Vector3 configuredPortalBounds,
            bool configuredTiePortalDensityToOpenState,
            bool configuredRenderGraphNativePackageDeferred,
            bool configuredLegitimateVolumetricAssetDecisionRequired,
            bool configuredNeedsTomApproval,
            bool configuredFinalLocalFogApproved,
            string configuredRecommendation)
        {
            interiorDensity = Mathf.Clamp01(configuredInteriorDensity);
            lowValleyDensity = Mathf.Clamp01(configuredLowValleyDensity);
            portalClosedDensity = Mathf.Clamp01(configuredPortalClosedDensity);
            portalOpenDensity = Mathf.Clamp01(configuredPortalOpenDensity);
            portalStrongOptionMultiplier = Mathf.Clamp(configuredPortalStrongOptionMultiplier, 1f, 2.5f);
            edgeFeather = Mathf.Clamp(configuredEdgeFeather, 0.05f, 0.95f);
            noiseStrength = Mathf.Clamp01(configuredNoiseStrength);
            noiseScale = Mathf.Clamp(configuredNoiseScale, 0.2f, 12f);
            heightFade = Mathf.Clamp(configuredHeightFade, 0f, 1.5f);
            portalGlow = Mathf.Clamp01(configuredPortalGlow);
            interiorColor = configuredInteriorColor;
            lowValleyColor = configuredLowValleyColor;
            portalColor = configuredPortalColor;
            interiorBounds = ClampBounds(configuredInteriorBounds);
            lowValleyBounds = ClampBounds(configuredLowValleyBounds);
            portalBounds = ClampBounds(configuredPortalBounds);
            tiePortalDensityToOpenState = configuredTiePortalDensityToOpenState;
            renderGraphNativePackageDeferred = configuredRenderGraphNativePackageDeferred;
            legitimateVolumetricAssetDecisionRequired = configuredLegitimateVolumetricAssetDecisionRequired;
            needsTomApproval = configuredNeedsTomApproval;
            finalLocalFogApproved = configuredFinalLocalFogApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }

        public float ResolveDensityForReview(FastVsHd2dLocalVolumetricFogKind kind, bool portalOpen, float alphaMultiplier)
        {
            var density = kind switch
            {
                FastVsHd2dLocalVolumetricFogKind.InteriorRoom => interiorDensity,
                FastVsHd2dLocalVolumetricFogKind.LowValley => lowValleyDensity,
                FastVsHd2dLocalVolumetricFogKind.PortalThreshold => !tiePortalDensityToOpenState || portalOpen ? portalOpenDensity : portalClosedDensity,
                _ => 0f
            };
            return Mathf.Clamp01(density * Mathf.Max(0f, alphaMultiplier));
        }

        public Color ResolveColorForReview(FastVsHd2dLocalVolumetricFogKind kind)
        {
            return kind switch
            {
                FastVsHd2dLocalVolumetricFogKind.InteriorRoom => interiorColor,
                FastVsHd2dLocalVolumetricFogKind.LowValley => lowValleyColor,
                FastVsHd2dLocalVolumetricFogKind.PortalThreshold => portalColor,
                _ => Color.white
            };
        }

        public Vector3 ResolveBoundsForReview(FastVsHd2dLocalVolumetricFogKind kind)
        {
            return kind switch
            {
                FastVsHd2dLocalVolumetricFogKind.InteriorRoom => interiorBounds,
                FastVsHd2dLocalVolumetricFogKind.LowValley => lowValleyBounds,
                FastVsHd2dLocalVolumetricFogKind.PortalThreshold => portalBounds,
                _ => Vector3.one
            };
        }

        private static Vector3 ClampBounds(Vector3 value)
        {
            return new Vector3(
                Mathf.Max(0.05f, value.x),
                Mathf.Max(0.05f, value.y),
                Mathf.Max(0.05f, value.z));
        }
    }
}
