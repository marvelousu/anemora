using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Underwater Waterline Profile")]
    public sealed class FastVsHd2dUnderwaterWaterlineProfile : ScriptableObject
    {
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalUnderwaterWaterlineApproved;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool onlyRelevantWhenCameraSubmerges = true;
        [SerializeField] private bool runtimeDefaultDisabled = true;
        [SerializeField] private bool fullScreenShaderMaterialPrepared = true;
        [SerializeField] private bool rendererFeatureDeferredUntilApproval = true;
        [SerializeField] private bool heightTriggerPrepared = true;
        [SerializeField, Range(-1f, 1f)] private float referenceWaterPlaneHeight = 0.176f;
        [SerializeField, Range(0f, 0.25f)] private float submergedActivationThreshold = 0.06f;
        [SerializeField, Range(0.04f, 0.50f)] private float transitionBand = 0.18f;
        [SerializeField] private Color underwaterTint = new Color(0.22f, 0.50f, 0.62f, 1f);
        [SerializeField, Range(0.12f, 0.60f)] private float conservativeTintBlend = 0.32f;
        [SerializeField, Range(0.25f, 0.72f)] private float strongerTintBlend = 0.48f;
        [SerializeField, Range(0.10f, 0.70f)] private float conservativeDepthFogDensity = 0.40f;
        [SerializeField, Range(0.25f, 0.88f)] private float strongerDepthFogDensity = 0.56f;
        [SerializeField, Range(0f, 3.50f)] private float conservativeDistortionPixels = 1.45f;
        [SerializeField, Range(0f, 5.00f)] private float strongerDistortionPixels = 2.60f;
        [SerializeField, Range(0f, 0.36f)] private float conservativeFarDesaturation = 0.14f;
        [SerializeField, Range(0f, 0.48f)] private float strongerFarDesaturation = 0.22f;
        [SerializeField, Range(0.35f, 0.78f)] private float normalizedWaterlineY = 0.62f;
        [SerializeField, Range(0.015f, 0.12f)] private float waterlineFeather = 0.055f;
        [SerializeField, Range(0f, 0.42f)] private float surfaceLineStrength = 0.18f;
        [SerializeField, Range(0f, 0.22f)] private float godRayStrength = 0.08f;
        [SerializeField, Range(0f, 0.34f)] private float edgeVignetteStrength = 0.18f;
        [SerializeField, Range(0f, 2f)] private float distortionScrollSpeed = 0.55f;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep as conditional underwater-camera data only. If Tom approves a real submerge camera path, prefer the conservative tint/fog/distortion values and wire the prepared fullscreen shader through a water-region trigger.";

        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalUnderwaterWaterlineApprovedForReview => finalUnderwaterWaterlineApproved;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool OnlyRelevantWhenCameraSubmergesForReview => onlyRelevantWhenCameraSubmerges;
        public bool RuntimeDefaultDisabledForReview => runtimeDefaultDisabled;
        public bool FullScreenShaderMaterialPreparedForReview => fullScreenShaderMaterialPrepared;
        public bool RendererFeatureDeferredUntilApprovalForReview => rendererFeatureDeferredUntilApproval;
        public bool HeightTriggerPreparedForReview => heightTriggerPrepared;
        public float ReferenceWaterPlaneHeightForReview => Mathf.Clamp(referenceWaterPlaneHeight, -1f, 1f);
        public float SubmergedActivationThresholdForReview => Mathf.Clamp(submergedActivationThreshold, 0f, 0.25f);
        public float TransitionBandForReview => Mathf.Clamp(transitionBand, 0.04f, 0.50f);
        public Color UnderwaterTintForReview
        {
            get
            {
                var color = underwaterTint;
                color.a = 1f;
                return color;
            }
        }

        public float ConservativeTintBlendForReview => Mathf.Clamp(conservativeTintBlend, 0.12f, 0.60f);
        public float StrongerTintBlendForReview => Mathf.Clamp(strongerTintBlend, 0.25f, 0.72f);
        public float ConservativeDepthFogDensityForReview => Mathf.Clamp(conservativeDepthFogDensity, 0.10f, 0.70f);
        public float StrongerDepthFogDensityForReview => Mathf.Clamp(strongerDepthFogDensity, 0.25f, 0.88f);
        public float ConservativeDistortionPixelsForReview => Mathf.Clamp(conservativeDistortionPixels, 0f, 3.50f);
        public float StrongerDistortionPixelsForReview => Mathf.Clamp(strongerDistortionPixels, 0f, 5f);
        public float ConservativeFarDesaturationForReview => Mathf.Clamp(conservativeFarDesaturation, 0f, 0.36f);
        public float StrongerFarDesaturationForReview => Mathf.Clamp(strongerFarDesaturation, 0f, 0.48f);
        public float NormalizedWaterlineYForReview => Mathf.Clamp(normalizedWaterlineY, 0.35f, 0.78f);
        public float WaterlineFeatherForReview => Mathf.Clamp(waterlineFeather, 0.015f, 0.12f);
        public float SurfaceLineStrengthForReview => Mathf.Clamp(surfaceLineStrength, 0f, 0.42f);
        public float GodRayStrengthForReview => Mathf.Clamp(godRayStrength, 0f, 0.22f);
        public float EdgeVignetteStrengthForReview => Mathf.Clamp(edgeVignetteStrength, 0f, 0.34f);
        public float DistortionScrollSpeedForReview => Mathf.Clamp(distortionScrollSpeed, 0f, 2f);
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            bool configuredNeedsTomApproval,
            bool configuredFinalApproved,
            bool configuredConservativeDataPrep,
            bool configuredOnlyRelevantWhenCameraSubmerges,
            bool configuredRuntimeDefaultDisabled,
            bool configuredFullScreenShaderMaterialPrepared,
            bool configuredRendererFeatureDeferredUntilApproval,
            bool configuredHeightTriggerPrepared,
            float configuredReferenceWaterPlaneHeight,
            float configuredSubmergedActivationThreshold,
            float configuredTransitionBand,
            Color configuredUnderwaterTint,
            float configuredConservativeTintBlend,
            float configuredStrongerTintBlend,
            float configuredConservativeDepthFogDensity,
            float configuredStrongerDepthFogDensity,
            float configuredConservativeDistortionPixels,
            float configuredStrongerDistortionPixels,
            float configuredConservativeFarDesaturation,
            float configuredStrongerFarDesaturation,
            float configuredNormalizedWaterlineY,
            float configuredWaterlineFeather,
            float configuredSurfaceLineStrength,
            float configuredGodRayStrength,
            float configuredEdgeVignetteStrength,
            float configuredDistortionScrollSpeed,
            string configuredRecommendation)
        {
            needsTomApproval = configuredNeedsTomApproval;
            finalUnderwaterWaterlineApproved = configuredFinalApproved;
            conservativeDataPrep = configuredConservativeDataPrep;
            onlyRelevantWhenCameraSubmerges = configuredOnlyRelevantWhenCameraSubmerges;
            runtimeDefaultDisabled = configuredRuntimeDefaultDisabled;
            fullScreenShaderMaterialPrepared = configuredFullScreenShaderMaterialPrepared;
            rendererFeatureDeferredUntilApproval = configuredRendererFeatureDeferredUntilApproval;
            heightTriggerPrepared = configuredHeightTriggerPrepared;
            referenceWaterPlaneHeight = Mathf.Clamp(configuredReferenceWaterPlaneHeight, -1f, 1f);
            submergedActivationThreshold = Mathf.Clamp(configuredSubmergedActivationThreshold, 0f, 0.25f);
            transitionBand = Mathf.Clamp(configuredTransitionBand, 0.04f, 0.50f);
            underwaterTint = configuredUnderwaterTint;
            underwaterTint.a = 1f;
            conservativeTintBlend = Mathf.Clamp(configuredConservativeTintBlend, 0.12f, 0.60f);
            strongerTintBlend = Mathf.Clamp(configuredStrongerTintBlend, 0.25f, 0.72f);
            conservativeDepthFogDensity = Mathf.Clamp(configuredConservativeDepthFogDensity, 0.10f, 0.70f);
            strongerDepthFogDensity = Mathf.Clamp(configuredStrongerDepthFogDensity, 0.25f, 0.88f);
            conservativeDistortionPixels = Mathf.Clamp(configuredConservativeDistortionPixels, 0f, 3.50f);
            strongerDistortionPixels = Mathf.Clamp(configuredStrongerDistortionPixels, 0f, 5f);
            conservativeFarDesaturation = Mathf.Clamp(configuredConservativeFarDesaturation, 0f, 0.36f);
            strongerFarDesaturation = Mathf.Clamp(configuredStrongerFarDesaturation, 0f, 0.48f);
            normalizedWaterlineY = Mathf.Clamp(configuredNormalizedWaterlineY, 0.35f, 0.78f);
            waterlineFeather = Mathf.Clamp(configuredWaterlineFeather, 0.015f, 0.12f);
            surfaceLineStrength = Mathf.Clamp(configuredSurfaceLineStrength, 0f, 0.42f);
            godRayStrength = Mathf.Clamp(configuredGodRayStrength, 0f, 0.22f);
            edgeVignetteStrength = Mathf.Clamp(configuredEdgeVignetteStrength, 0f, 0.34f);
            distortionScrollSpeed = Mathf.Clamp(configuredDistortionScrollSpeed, 0f, 2f);
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
