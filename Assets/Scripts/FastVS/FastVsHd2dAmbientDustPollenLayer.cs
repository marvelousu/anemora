using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [AddComponentMenu("Anemora/FastVS/HD2D Ambient Dust Pollen Layer")]
    public sealed class FastVsHd2dAmbientDustPollenLayer : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dAmbientDustPollenProfile profile;
        [SerializeField] private Camera anchorCamera;
        [SerializeField] private ParticleSystem dustSystem;
        [SerializeField] private ParticleSystemRenderer dustRenderer;
        [SerializeField] private bool cameraAttached = true;
        [SerializeField] private bool independentOfSunShafts = true;
        [SerializeField] private bool conservativeReviewMode = true;
        [SerializeField] private bool requiresTomArtApproval = true;

        private bool hasReviewOverride;
        private bool reviewEnabled = true;
        private float reviewEmissionMultiplier = 1f;
        private float reviewTintWarmth = -1f;
        private float appliedEmissionRate;
        private Color appliedTint;

        public FastVsHd2dAmbientDustPollenProfile ProfileForReview => profile;
        public int ParticleSystemCountForReview => dustSystem != null ? 1 : 0;
        public int TotalMaxParticlesForReview => dustSystem != null ? dustSystem.main.maxParticles : 0;
        public bool UsesCpuShurikenForReview => true;
        public bool SimulationSpaceWorldForReview => dustSystem != null && dustSystem.main.simulationSpace == ParticleSystemSimulationSpace.World;
        public bool CameraAttachedForReview => anchorCamera != null && transform.IsChildOf(anchorCamera.transform) && cameraAttached;
        public bool IndependentOfSunShaftsForReview => independentOfSunShafts;
        public bool ConservativeReviewModeForReview => conservativeReviewMode;
        public bool RequiresTomArtApprovalForReview => requiresTomArtApproval;
        public bool RendererVisibleForReview => dustRenderer != null && dustRenderer.enabled;
        public bool NoiseEnabledForReview => dustSystem != null && dustSystem.noise.enabled;
        public bool BoxEmissionForReview => dustSystem != null && dustSystem.shape.enabled && dustSystem.shape.shapeType == ParticleSystemShapeType.Box;
        public Vector3 BoxSizeForReview => dustSystem != null ? dustSystem.shape.scale : Vector3.zero;
        public Vector3 WorldWindVelocityForReview => profile != null ? profile.WorldWindVelocityForReview : Vector3.zero;
        public float AppliedEmissionRateForReview => appliedEmissionRate;
        public float AppliedTintAlphaForReview => appliedTint.a;
        public float AppliedTintWarmthForReview => ResolveTintWarmth();
        public float AlphaCeilingForReview => profile != null ? profile.AlphaCeilingForReview : 0f;
        public float ReviewEmissionMultiplierForReview => reviewEmissionMultiplier;

        private void Awake()
        {
            ResolveReferences();
            ApplyNowForReview();
        }

        private void OnEnable()
        {
            ResolveReferences();
            ApplyNowForReview();
        }

        private void LateUpdate()
        {
            ApplyNowForReview();
        }

        public void ConfigureForReview(
            FastVsHd2dAmbientDustPollenProfile configuredProfile,
            Camera configuredAnchorCamera,
            ParticleSystem configuredDustSystem,
            ParticleSystemRenderer configuredDustRenderer,
            bool configuredCameraAttached,
            bool configuredIndependentOfSunShafts,
            bool configuredConservativeReviewMode,
            bool configuredRequiresTomArtApproval)
        {
            profile = configuredProfile;
            anchorCamera = configuredAnchorCamera;
            dustSystem = configuredDustSystem;
            dustRenderer = configuredDustRenderer;
            cameraAttached = configuredCameraAttached;
            independentOfSunShafts = configuredIndependentOfSunShafts;
            conservativeReviewMode = configuredConservativeReviewMode;
            requiresTomArtApproval = configuredRequiresTomArtApproval;
            reviewTintWarmth = profile != null ? profile.DefaultTintWarmthForReview : -1f;
            ApplyNowForReview();
        }

        public void SetReviewOverrideForReview(bool enabled, float emissionMultiplier)
        {
            hasReviewOverride = true;
            reviewEnabled = enabled;
            reviewEmissionMultiplier = Mathf.Max(0f, emissionMultiplier);
            ApplyNowForReview();
        }

        public void ClearReviewOverrideForReview()
        {
            hasReviewOverride = false;
            reviewEnabled = true;
            reviewEmissionMultiplier = 1f;
            reviewTintWarmth = profile != null ? profile.DefaultTintWarmthForReview : -1f;
            ApplyNowForReview();
        }

        public void SetReviewTintWarmthForReview(float warmth)
        {
            reviewTintWarmth = Mathf.Clamp01(warmth);
            ApplyNowForReview();
        }

        public void SimulateForReview(float seconds)
        {
            SimulateForReview(seconds, true);
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            ResolveReferences();
            if (dustSystem == null)
            {
                return;
            }

            ApplyNowForReview();
            dustSystem.Simulate(Mathf.Max(0f, seconds), true, restart, false);
        }

        public void ApplyNowForReview()
        {
            ResolveReferences();
            if (dustSystem == null || profile == null)
            {
                return;
            }

            var active = !hasReviewOverride || reviewEnabled;
            var emissionMultiplier = active ? Mathf.Max(0f, reviewEmissionMultiplier) : 0f;
            appliedEmissionRate = profile.EmissionRateForReview * emissionMultiplier;
            appliedTint = ResolveTint();

            var main = dustSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(appliedTint);

            var emission = dustSystem.emission;
            emission.enabled = active && appliedEmissionRate > 0.001f;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(appliedEmissionRate);

            if (dustRenderer != null)
            {
                dustRenderer.enabled = active && appliedEmissionRate > 0.001f;
                dustRenderer.shadowCastingMode = ShadowCastingMode.Off;
                dustRenderer.receiveShadows = false;
            }

            if (active && !dustSystem.isPlaying)
            {
                dustSystem.Play(true);
            }
            else if (!active && dustSystem.isPlaying)
            {
                dustSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        private void ResolveReferences()
        {
            if (anchorCamera == null)
            {
                anchorCamera = Camera.main;
            }

            if (dustSystem == null)
            {
                dustSystem = GetComponentInChildren<ParticleSystem>(true);
            }

            if (dustRenderer == null && dustSystem != null)
            {
                dustRenderer = dustSystem.GetComponent<ParticleSystemRenderer>();
            }
        }

        private Color ResolveTint()
        {
            if (profile == null)
            {
                return new Color(0.82f, 0.82f, 0.76f, 0.08f);
            }

            var color = Color.Lerp(profile.CoolShadeTintForReview, profile.WarmPollenTintForReview, ResolveTintWarmth());
            color.a = Mathf.Min(color.a, profile.AlphaCeilingForReview);
            return color;
        }

        private float ResolveTintWarmth()
        {
            if (reviewTintWarmth >= 0f)
            {
                return Mathf.Clamp01(reviewTintWarmth);
            }

            return profile != null ? profile.DefaultTintWarmthForReview : 0.5f;
        }
    }
}
