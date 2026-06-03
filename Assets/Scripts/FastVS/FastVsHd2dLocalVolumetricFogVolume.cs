using Anemora.TimeManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [AddComponentMenu("Anemora/HD2D/Local Volumetric Fog Volume")]
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dLocalVolumetricFogVolume : MonoBehaviour
    {
        private static readonly int FogColorId = Shader.PropertyToID("_FogColor");
        private static readonly int FogDensityId = Shader.PropertyToID("_FogDensity");
        private static readonly int EdgeFeatherId = Shader.PropertyToID("_EdgeFeather");
        private static readonly int NoiseStrengthId = Shader.PropertyToID("_NoiseStrength");
        private static readonly int NoiseScaleId = Shader.PropertyToID("_NoiseScale");
        private static readonly int HeightFadeId = Shader.PropertyToID("_HeightFade");
        private static readonly int PortalGlowId = Shader.PropertyToID("_PortalGlow");
        private static readonly int TimeOffsetId = Shader.PropertyToID("_TimeOffset");

        [SerializeField] private FastVsHd2dLocalVolumetricFogProfile profile;
        [SerializeField] private FastVsHd2dLocalVolumetricFogKind fogKind;
        [SerializeField] private Renderer[] fogRenderers = System.Array.Empty<Renderer>();
        [SerializeField] private TimeWindowPairedSpacePortalController portalController;
        [SerializeField] private bool reactToPortalOpen;
        [SerializeField] private bool syncToPortalPlane;
        [SerializeField] private Vector3 fallbackLocalCenter;
        [SerializeField] private Vector3 portalLocalOffset;
        [SerializeField, Range(0f, 2.5f)] private float reviewAlphaMultiplier = 1f;
        [SerializeField] private bool reviewVisible = true;
        [SerializeField, Range(0f, 8f)] private float reviewTimeOffset;

        private MaterialPropertyBlock propertyBlock;
        private float lastAppliedDensity;
        private Color lastAppliedColor = Color.clear;
        private bool lastPortalOpen;

        public FastVsHd2dLocalVolumetricFogProfile ProfileForReview => profile;
        public FastVsHd2dLocalVolumetricFogKind FogKindForReview => fogKind;
        public int RendererCountForReview => fogRenderers != null ? fogRenderers.Length : 0;
        public bool ReactsToPortalOpenForReview => reactToPortalOpen;
        public bool SyncsToPortalPlaneForReview => syncToPortalPlane;
        public Vector3 FallbackLocalCenterForReview => fallbackLocalCenter;
        public Vector3 PortalLocalOffsetForReview => portalLocalOffset;
        public float ReviewAlphaMultiplierForReview => reviewAlphaMultiplier;
        public bool ReviewVisibleForReview => reviewVisible;
        public float LastAppliedDensityForReview => lastAppliedDensity;
        public Color LastAppliedColorForReview => lastAppliedColor;
        public bool LastPortalOpenForReview => lastPortalOpen;

        private void OnEnable()
        {
            ResolveReferences();
            ApplyNowForReview();
        }

        private void LateUpdate()
        {
            ApplyNowForReview();
        }

        private void OnValidate()
        {
            ApplyNowForReview();
        }

        public void ConfigureForReview(
            FastVsHd2dLocalVolumetricFogProfile configuredProfile,
            FastVsHd2dLocalVolumetricFogKind configuredKind,
            Renderer[] configuredRenderers,
            bool configuredReactToPortalOpen,
            bool configuredSyncToPortalPlane,
            Vector3 configuredFallbackLocalCenter,
            Vector3 configuredPortalLocalOffset,
            float configuredAlphaMultiplier)
        {
            profile = configuredProfile;
            fogKind = configuredKind;
            fogRenderers = configuredRenderers ?? System.Array.Empty<Renderer>();
            reactToPortalOpen = configuredReactToPortalOpen;
            syncToPortalPlane = configuredSyncToPortalPlane;
            fallbackLocalCenter = configuredFallbackLocalCenter;
            portalLocalOffset = configuredPortalLocalOffset;
            reviewAlphaMultiplier = Mathf.Clamp(configuredAlphaMultiplier, 0f, 2.5f);
            reviewVisible = true;
            ResolveReferences();
            ApplyNowForReview();
        }

        public void SetReviewVisibleForReview(bool visible)
        {
            reviewVisible = visible;
            ApplyNowForReview();
        }

        public void SetReviewAlphaMultiplierForReview(float multiplier)
        {
            reviewAlphaMultiplier = Mathf.Clamp(multiplier, 0f, 2.5f);
            ApplyNowForReview();
        }

        public void SetReviewTimeOffsetForReview(float offset)
        {
            reviewTimeOffset = Mathf.Clamp(offset, 0f, 8f);
            ApplyNowForReview();
        }

        public void ApplyNowForReview()
        {
            if (profile == null)
            {
                SetRenderersEnabled(false);
                lastAppliedDensity = 0f;
                return;
            }

            ResolveReferences();
            SyncToPortalPlaneIfNeeded();
            lastPortalOpen = reactToPortalOpen && portalController != null && portalController.HasPortalPair;
            lastAppliedDensity = reviewVisible
                ? profile.ResolveDensityForReview(fogKind, lastPortalOpen, reviewAlphaMultiplier)
                : 0f;
            lastAppliedColor = profile.ResolveColorForReview(fogKind);

            if (propertyBlock == null)
            {
                propertyBlock = new MaterialPropertyBlock();
            }

            propertyBlock.Clear();
            propertyBlock.SetColor(FogColorId, lastAppliedColor);
            propertyBlock.SetFloat(FogDensityId, lastAppliedDensity);
            propertyBlock.SetFloat(EdgeFeatherId, profile.EdgeFeatherForReview);
            propertyBlock.SetFloat(NoiseStrengthId, profile.NoiseStrengthForReview);
            propertyBlock.SetFloat(NoiseScaleId, profile.NoiseScaleForReview);
            propertyBlock.SetFloat(HeightFadeId, profile.HeightFadeForReview);
            propertyBlock.SetFloat(PortalGlowId, fogKind == FastVsHd2dLocalVolumetricFogKind.PortalThreshold ? profile.PortalGlowForReview : 0f);
            propertyBlock.SetFloat(TimeOffsetId, reviewTimeOffset);

            var enableRenderer = lastAppliedDensity > 0.001f;
            if (fogRenderers == null)
            {
                return;
            }

            for (var i = 0; i < fogRenderers.Length; i++)
            {
                var fogRenderer = fogRenderers[i];
                if (fogRenderer == null)
                {
                    continue;
                }

                fogRenderer.enabled = enableRenderer;
                fogRenderer.shadowCastingMode = ShadowCastingMode.Off;
                fogRenderer.receiveShadows = false;
                fogRenderer.lightProbeUsage = LightProbeUsage.Off;
                fogRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
                fogRenderer.SetPropertyBlock(propertyBlock);
            }
        }

        private void ResolveReferences()
        {
            if (portalController == null && (reactToPortalOpen || syncToPortalPlane))
            {
                portalController = FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            }
        }

        private void SyncToPortalPlaneIfNeeded()
        {
            if (!syncToPortalPlane)
            {
                return;
            }

            var localCenter = fallbackLocalCenter;
            if (portalController != null && portalController.HasPortalPair)
            {
                localCenter = portalController.PortalLocalCenterForReview;
            }

            transform.localPosition = localCenter + portalLocalOffset;
        }

        private void SetRenderersEnabled(bool enabled)
        {
            if (fogRenderers == null)
            {
                return;
            }

            for (var i = 0; i < fogRenderers.Length; i++)
            {
                if (fogRenderers[i] != null)
                {
                    fogRenderers[i].enabled = enabled;
                }
            }
        }
    }
}
