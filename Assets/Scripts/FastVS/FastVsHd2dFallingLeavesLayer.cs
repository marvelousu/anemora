using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [AddComponentMenu("Anemora/FastVS/HD2D Falling Leaves Seasonal Drift")]
    public sealed class FastVsHd2dFallingLeavesLayer : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dFallingLeavesProfile profile;
        [SerializeField] private Camera anchorCamera;
        [SerializeField] private ParticleSystem foregroundSystem;
        [SerializeField] private ParticleSystem midDepthSystem;
        [SerializeField] private ParticleSystemRenderer foregroundRenderer;
        [SerializeField] private ParticleSystemRenderer midDepthRenderer;
        [SerializeField] private bool cameraAttached = true;
        [SerializeField] private bool biomeSwappable = true;
        [SerializeField] private bool foregroundBokehLayer = true;
        [SerializeField] private bool conservativeReviewMode = true;
        [SerializeField] private bool requiresTomArtApproval = true;

        private bool hasReviewOverride;
        private bool reviewEnabled = true;
        private float reviewEmissionMultiplier = 1f;
        private FastVsHd2dFallingLeavesBiome reviewBiome = FastVsHd2dFallingLeavesBiome.GreenLeaf;
        private float appliedEmissionRate;

        public FastVsHd2dFallingLeavesProfile ProfileForReview => profile;
        public int ParticleSystemCountForReview => CountSystems();
        public int RendererCountForReview => CountRenderers();
        public int TotalMaxParticlesForReview => CountMaxParticles();
        public int ForegroundLiveParticlesForReview => foregroundSystem != null ? foregroundSystem.particleCount : 0;
        public int MidDepthLiveParticlesForReview => midDepthSystem != null ? midDepthSystem.particleCount : 0;
        public int TotalLiveParticlesForReview => ForegroundLiveParticlesForReview + MidDepthLiveParticlesForReview;
        public bool CameraAttachedForReview => anchorCamera != null && transform.IsChildOf(anchorCamera.transform) && cameraAttached;
        public bool BiomeSwappableForReview => biomeSwappable;
        public bool ForegroundBokehLayerForReview => foregroundBokehLayer && foregroundSystem != null;
        public bool ConservativeReviewModeForReview => conservativeReviewMode;
        public bool RequiresTomArtApprovalForReview => requiresTomArtApproval;
        public bool UsesWorldSimulationForReview => UsesWorldSimulation();
        public bool BoxEmissionForReview => HasBoxEmission();
        public bool NoiseSwayForReview => HasNoise();
        public bool VelocityFallForReview => HasVelocity();
        public bool RotationBySpeedForReview => HasRotationBySpeed();
        public bool RotationTumbleForReview => HasRotationOverLifetime() && HasRotationBySpeed();
        public bool ShadowlessForReview => RenderersAreShadowless();
        public FastVsHd2dFallingLeavesBiome ActiveBiomeForReview => reviewBiome;
        public string ActiveBiomeIdForReview => profile != null ? profile.ResolveBiomeIdForReview(reviewBiome) : string.Empty;
        public float AppliedEmissionRateForReview => appliedEmissionRate;
        public float ReviewEmissionMultiplierForReview => reviewEmissionMultiplier;

        private void Awake()
        {
            ResolveReferences();
            if (profile != null)
            {
                reviewBiome = profile.DefaultBiomeForReview;
            }

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
            FastVsHd2dFallingLeavesProfile configuredProfile,
            Camera configuredAnchorCamera,
            ParticleSystem configuredForegroundSystem,
            ParticleSystem configuredMidDepthSystem,
            ParticleSystemRenderer configuredForegroundRenderer,
            ParticleSystemRenderer configuredMidDepthRenderer,
            bool configuredCameraAttached,
            bool configuredBiomeSwappable,
            bool configuredForegroundBokehLayer,
            bool configuredConservativeReviewMode,
            bool configuredRequiresTomArtApproval)
        {
            profile = configuredProfile;
            anchorCamera = configuredAnchorCamera;
            foregroundSystem = configuredForegroundSystem;
            midDepthSystem = configuredMidDepthSystem;
            foregroundRenderer = configuredForegroundRenderer;
            midDepthRenderer = configuredMidDepthRenderer;
            cameraAttached = configuredCameraAttached;
            biomeSwappable = configuredBiomeSwappable;
            foregroundBokehLayer = configuredForegroundBokehLayer;
            conservativeReviewMode = configuredConservativeReviewMode;
            requiresTomArtApproval = configuredRequiresTomArtApproval;
            reviewBiome = profile != null ? profile.DefaultBiomeForReview : FastVsHd2dFallingLeavesBiome.GreenLeaf;
            ApplyNowForReview();
        }

        public void SetReviewOverrideForReview(bool enabled, float emissionMultiplier)
        {
            hasReviewOverride = true;
            reviewEnabled = enabled;
            reviewEmissionMultiplier = Mathf.Max(0f, emissionMultiplier);
            ApplyNowForReview();
        }

        public void SetReviewBiomeForReview(FastVsHd2dFallingLeavesBiome biome)
        {
            reviewBiome = biome;
            ApplyNowForReview();
        }

        public void ClearReviewOverrideForReview()
        {
            hasReviewOverride = false;
            reviewEnabled = true;
            reviewEmissionMultiplier = 1f;
            reviewBiome = profile != null ? profile.DefaultBiomeForReview : FastVsHd2dFallingLeavesBiome.GreenLeaf;
            ApplyNowForReview();
        }

        public void SimulateForReview(float seconds)
        {
            SimulateForReview(seconds, true);
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            ResolveReferences();
            ApplyNowForReview();
            var duration = Mathf.Max(0f, seconds);
            SimulateSystemForReview(foregroundSystem, duration, restart);
            SimulateSystemForReview(midDepthSystem, duration, restart);
        }

        public void ApplyNowForReview()
        {
            ResolveReferences();
            if (profile == null)
            {
                return;
            }

            var active = !hasReviewOverride || reviewEnabled;
            appliedEmissionRate = active ? profile.EmissionRateForReview * Mathf.Max(0f, reviewEmissionMultiplier) : 0f;
            ApplySystemForReview(foregroundSystem, 0.42f, active);
            ApplySystemForReview(midDepthSystem, 0.58f, active);
            ApplyRendererForReview(foregroundRenderer, active);
            ApplyRendererForReview(midDepthRenderer, active);
        }

        private void ApplySystemForReview(ParticleSystem system, float emissionWeight, bool active)
        {
            if (system == null || profile == null)
            {
                return;
            }

            var tint = profile.ResolveTintForReview(reviewBiome);
            var main = system.main;
            main.startColor = new ParticleSystem.MinMaxGradient(tint);

            var emission = system.emission;
            emission.enabled = active && appliedEmissionRate > 0.001f;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(appliedEmissionRate * Mathf.Clamp01(emissionWeight));

            if (active && !system.isPlaying)
            {
                system.Play(true);
            }
            else if (!active && system.isPlaying)
            {
                system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        private void ApplyRendererForReview(ParticleSystemRenderer renderer, bool active)
        {
            if (renderer == null || profile == null)
            {
                return;
            }

            var material = profile.ResolveMaterialForReview(reviewBiome);
            if (material != null)
            {
                renderer.sharedMaterial = material;
            }

            renderer.enabled = active;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        private static void SimulateSystemForReview(ParticleSystem system, float seconds, bool restart)
        {
            if (system != null)
            {
                system.Simulate(seconds, true, restart, false);
            }
        }

        private void ResolveReferences()
        {
            if (anchorCamera == null)
            {
                anchorCamera = Camera.main;
            }

            if (foregroundSystem == null)
            {
                var foreground = transform.Find("FastVS_HD2D_P1_FallingLeaves_ForegroundBokeh");
                foregroundSystem = foreground != null ? foreground.GetComponent<ParticleSystem>() : null;
            }

            if (midDepthSystem == null)
            {
                var mid = transform.Find("FastVS_HD2D_P1_FallingLeaves_MidDepthSharp");
                midDepthSystem = mid != null ? mid.GetComponent<ParticleSystem>() : null;
            }

            if (foregroundRenderer == null && foregroundSystem != null)
            {
                foregroundRenderer = foregroundSystem.GetComponent<ParticleSystemRenderer>();
            }

            if (midDepthRenderer == null && midDepthSystem != null)
            {
                midDepthRenderer = midDepthSystem.GetComponent<ParticleSystemRenderer>();
            }
        }

        private int CountSystems()
        {
            var count = 0;
            if (foregroundSystem != null)
            {
                count++;
            }

            if (midDepthSystem != null)
            {
                count++;
            }

            return count;
        }

        private int CountRenderers()
        {
            var count = 0;
            if (foregroundRenderer != null)
            {
                count++;
            }

            if (midDepthRenderer != null)
            {
                count++;
            }

            return count;
        }

        private int CountMaxParticles()
        {
            var total = 0;
            if (foregroundSystem != null)
            {
                total += foregroundSystem.main.maxParticles;
            }

            if (midDepthSystem != null)
            {
                total += midDepthSystem.main.maxParticles;
            }

            return total;
        }

        private bool UsesWorldSimulation()
        {
            return foregroundSystem != null &&
                   midDepthSystem != null &&
                   foregroundSystem.main.simulationSpace == ParticleSystemSimulationSpace.World &&
                   midDepthSystem.main.simulationSpace == ParticleSystemSimulationSpace.World;
        }

        private bool HasBoxEmission()
        {
            return foregroundSystem != null &&
                   midDepthSystem != null &&
                   foregroundSystem.shape.enabled &&
                   midDepthSystem.shape.enabled &&
                   foregroundSystem.shape.shapeType == ParticleSystemShapeType.Box &&
                   midDepthSystem.shape.shapeType == ParticleSystemShapeType.Box;
        }

        private bool HasNoise()
        {
            return foregroundSystem != null &&
                   midDepthSystem != null &&
                   foregroundSystem.noise.enabled &&
                   midDepthSystem.noise.enabled;
        }

        private bool HasVelocity()
        {
            return foregroundSystem != null &&
                   midDepthSystem != null &&
                   foregroundSystem.velocityOverLifetime.enabled &&
                   midDepthSystem.velocityOverLifetime.enabled;
        }

        private bool HasRotationOverLifetime()
        {
            return foregroundSystem != null &&
                   midDepthSystem != null &&
                   foregroundSystem.rotationOverLifetime.enabled &&
                   midDepthSystem.rotationOverLifetime.enabled;
        }

        private bool HasRotationBySpeed()
        {
            return foregroundSystem != null &&
                   midDepthSystem != null &&
                   foregroundSystem.rotationBySpeed.enabled &&
                   midDepthSystem.rotationBySpeed.enabled;
        }

        private bool RenderersAreShadowless()
        {
            return foregroundRenderer != null &&
                   midDepthRenderer != null &&
                   foregroundRenderer.shadowCastingMode == ShadowCastingMode.Off &&
                   midDepthRenderer.shadowCastingMode == ShadowCastingMode.Off &&
                   !foregroundRenderer.receiveShadows &&
                   !midDepthRenderer.receiveShadows;
        }
    }
}
