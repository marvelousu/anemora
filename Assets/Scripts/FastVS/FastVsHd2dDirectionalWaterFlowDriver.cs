using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Directional Water Flow Driver")]
    public sealed class FastVsHd2dDirectionalWaterFlowDriver : MonoBehaviour
    {
        private static readonly int FlowMapId = Shader.PropertyToID("_FlowMap");
        private static readonly int DirectionalFlowEnabledId = Shader.PropertyToID("_DirectionalFlowEnabled");
        private static readonly int FlowStrengthId = Shader.PropertyToID("_FlowStrength");
        private static readonly int FlowSpeedId = Shader.PropertyToID("_FlowSpeed");
        private static readonly int FlowTimeOffsetId = Shader.PropertyToID("_FlowTimeOffset");
        private static readonly int FlowFoamAdvectionStrengthId = Shader.PropertyToID("_FlowFoamAdvectionStrength");
        private static readonly int FlowSpecularAdvectionStrengthId = Shader.PropertyToID("_FlowSpecularAdvectionStrength");
        private static readonly int FlowPhaseBlendSharpnessId = Shader.PropertyToID("_FlowPhaseBlendSharpness");

        [SerializeField] private FastVsHd2dDirectionalWaterFlowProfile profile;
        [SerializeField] private Material waterMaterial;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool conservativeNeedsTomApproval = true;

        private bool reviewActive = true;
        private bool reviewFlowEnabled = true;
        private float reviewTimeOffset;

        public FastVsHd2dDirectionalWaterFlowProfile ProfileForReview => profile;
        public Material WaterMaterialForReview => waterMaterial;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public bool ConservativeNeedsTomApprovalForReview => conservativeNeedsTomApproval;
        public bool ReviewActiveForReview => reviewActive;
        public bool LastFlowEnabledForReview { get; private set; }
        public float LastFlowStrengthForReview { get; private set; }
        public float LastFlowSpeedForReview { get; private set; }
        public float LastTimeOffsetForReview { get; private set; }
        public float LastFoamAdvectionStrengthForReview { get; private set; }
        public float LastSpecularAdvectionStrengthForReview { get; private set; }
        public float LastPhaseBlendSharpnessForReview { get; private set; }

        public bool IsReadyForReview =>
            profile != null &&
            waterMaterial != null &&
            profile.FlowMapForReview != null &&
            profile.NeedsTomApprovalForReview &&
            !profile.FinalDirectionalFlowApprovedForReview;

        private void OnEnable()
        {
            PublishNowForReview();
        }

        private void OnDisable()
        {
            ClearPublishedFlowForReview();
        }

        private void LateUpdate()
        {
            if (!publishEveryFrame || profile == null || !profile.PublishEveryFrameForReview)
            {
                return;
            }

            PublishNowForReview();
        }

        public void ConfigureForReview(FastVsHd2dDirectionalWaterFlowProfile configuredProfile, Material configuredWaterMaterial)
        {
            profile = configuredProfile;
            waterMaterial = configuredWaterMaterial;
            publishEveryFrame = profile == null || profile.PublishEveryFrameForReview;
            conservativeNeedsTomApproval = profile == null || profile.NeedsTomApprovalForReview;
            reviewActive = true;
            reviewFlowEnabled = true;
            reviewTimeOffset = 0f;
            PublishNowForReview();
        }

        public void ApplyReviewStateForReview(bool flowEnabled, float timeOffset)
        {
            reviewActive = true;
            reviewFlowEnabled = flowEnabled;
            reviewTimeOffset = Mathf.Max(0f, timeOffset);
            PublishNowForReview();
        }

        public void SetReviewActiveForReview(bool active)
        {
            reviewActive = active;
            PublishNowForReview();
        }

        public void PublishNowForReview()
        {
            if (!reviewActive || waterMaterial == null)
            {
                ClearPublishedFlowForReview();
                return;
            }

            if (profile == null)
            {
                return;
            }

            SetTextureIfPresent(waterMaterial, FlowMapId, profile.FlowMapForReview);
            SetFloatIfPresent(waterMaterial, DirectionalFlowEnabledId, reviewFlowEnabled ? 1f : 0f);
            SetFloatIfPresent(waterMaterial, FlowStrengthId, profile.FlowStrengthForReview);
            SetFloatIfPresent(waterMaterial, FlowSpeedId, profile.FlowSpeedForReview);
            SetFloatIfPresent(waterMaterial, FlowTimeOffsetId, reviewTimeOffset);
            SetFloatIfPresent(waterMaterial, FlowFoamAdvectionStrengthId, profile.FoamAdvectionStrengthForReview);
            SetFloatIfPresent(waterMaterial, FlowSpecularAdvectionStrengthId, profile.SpecularAdvectionStrengthForReview);
            SetFloatIfPresent(waterMaterial, FlowPhaseBlendSharpnessId, profile.PhaseBlendSharpnessForReview);

            LastFlowEnabledForReview = reviewFlowEnabled;
            LastFlowStrengthForReview = profile.FlowStrengthForReview;
            LastFlowSpeedForReview = profile.FlowSpeedForReview;
            LastTimeOffsetForReview = reviewTimeOffset;
            LastFoamAdvectionStrengthForReview = profile.FoamAdvectionStrengthForReview;
            LastSpecularAdvectionStrengthForReview = profile.SpecularAdvectionStrengthForReview;
            LastPhaseBlendSharpnessForReview = profile.PhaseBlendSharpnessForReview;
        }

        public void ClearPublishedFlowForReview()
        {
            if (waterMaterial != null)
            {
                SetFloatIfPresent(waterMaterial, DirectionalFlowEnabledId, 0f);
                SetFloatIfPresent(waterMaterial, FlowTimeOffsetId, 0f);
            }

            LastFlowEnabledForReview = false;
            LastTimeOffsetForReview = 0f;
        }

        private static void SetFloatIfPresent(Material material, int propertyId, float value)
        {
            if (material != null && material.HasProperty(propertyId))
            {
                material.SetFloat(propertyId, value);
            }
        }

        private static void SetTextureIfPresent(Material material, int propertyId, Texture texture)
        {
            if (material != null && texture != null && material.HasProperty(propertyId))
            {
                material.SetTexture(propertyId, texture);
            }
        }
    }
}
