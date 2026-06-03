using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DefaultExecutionOrder(127)]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Water Splash Ripple Emitter")]
    public sealed class FastVsHd2dWaterSplashRippleEmitter : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dWaterSplashRippleProfile profile;
        [SerializeField] private ParticleSystem rippleSystem;
        [SerializeField] private ParticleSystem splashDropletSystem;
        [SerializeField] private ParticleSystem mistSystem;
        [SerializeField] private ParticleSystemRenderer rippleRenderer;
        [SerializeField] private ParticleSystemRenderer splashRenderer;
        [SerializeField] private ParticleSystemRenderer mistRenderer;
        [SerializeField] private LayerMask waterSurfaceMask = ~0;
        [SerializeField] private bool raycastEntryHookPrepared = true;
        [SerializeField] private bool distanceCullEnabled = true;
        [SerializeField, Range(0f, 2.5f)] private float reviewSplashRippleMultiplier = 1f;
        [SerializeField, Range(0f, 2.5f)] private float reviewMistMultiplier = 1f;
        [SerializeField, Range(0f, 2.5f)] private float reviewAlphaMultiplier = 1f;
        [SerializeField] private bool reviewVisible = true;

        private bool lastDistanceCulled;
        private bool lastRaycastHitWater;
        private int totalTriggeredRipples;
        private Vector3 lastTriggerWorldPosition;
        private Vector3 lastResolvedWaterWorldPosition;
        private float appliedMistEmissionRate;

        public FastVsHd2dWaterSplashRippleProfile ProfileForReview => profile;
        public ParticleSystem RippleSystemForReview => rippleSystem;
        public ParticleSystem SplashDropletSystemForReview => splashDropletSystem;
        public ParticleSystem MistSystemForReview => mistSystem;
        public ParticleSystemRenderer RippleRendererForReview => rippleRenderer;
        public ParticleSystemRenderer SplashRendererForReview => splashRenderer;
        public ParticleSystemRenderer MistRendererForReview => mistRenderer;
        public LayerMask WaterSurfaceMaskForReview => waterSurfaceMask;
        public bool RaycastEntryHookPreparedForReview => raycastEntryHookPrepared;
        public bool DistanceCullEnabledForReview => distanceCullEnabled;
        public bool ReviewVisibleForReview => reviewVisible;
        public bool LastDistanceCulledForReview => lastDistanceCulled;
        public bool LastRaycastHitWaterForReview => lastRaycastHitWater;
        public int TotalTriggeredRipplesForReview => totalTriggeredRipples;
        public Vector3 LastTriggerWorldPositionForReview => lastTriggerWorldPosition;
        public Vector3 LastResolvedWaterWorldPositionForReview => lastResolvedWaterWorldPosition;
        public float AppliedMistEmissionRateForReview => appliedMistEmissionRate;
        public int RippleParticleCountForReview => rippleSystem != null ? rippleSystem.particleCount : 0;
        public int SplashParticleCountForReview => splashDropletSystem != null ? splashDropletSystem.particleCount : 0;
        public int MistParticleCountForReview => mistSystem != null ? mistSystem.particleCount : 0;
        public bool SubEmitterBirthConfiguredForReview => HasBirthSubEmitterForReview();

        private void OnEnable()
        {
            ResolveReferences();
            ApplyForReview();
            StartMistIfNeeded();
        }

        private void LateUpdate()
        {
            ApplyDistanceCullForReview();
            ApplyForReview();
        }

        private void OnValidate()
        {
            ApplyForReview();
        }

        public void ConfigureForReview(
            FastVsHd2dWaterSplashRippleProfile configuredProfile,
            ParticleSystem configuredRippleSystem,
            ParticleSystem configuredSplashDropletSystem,
            ParticleSystem configuredMistSystem,
            ParticleSystemRenderer configuredRippleRenderer,
            ParticleSystemRenderer configuredSplashRenderer,
            ParticleSystemRenderer configuredMistRenderer,
            LayerMask configuredWaterSurfaceMask,
            bool configuredRaycastEntryHookPrepared,
            bool configuredDistanceCullEnabled)
        {
            profile = configuredProfile;
            rippleSystem = configuredRippleSystem;
            splashDropletSystem = configuredSplashDropletSystem;
            mistSystem = configuredMistSystem;
            rippleRenderer = configuredRippleRenderer;
            splashRenderer = configuredSplashRenderer;
            mistRenderer = configuredMistRenderer;
            waterSurfaceMask = configuredWaterSurfaceMask;
            raycastEntryHookPrepared = configuredRaycastEntryHookPrepared;
            distanceCullEnabled = configuredDistanceCullEnabled;
            reviewVisible = true;
            reviewSplashRippleMultiplier = 1f;
            reviewMistMultiplier = 1f;
            reviewAlphaMultiplier = 1f;
            ApplyForReview();
            StartMistIfNeeded();
        }

        public void SetReviewVisibleForReview(bool visible)
        {
            reviewVisible = visible;
            ResolveReferences();
            if (!visible)
            {
                StopAndClear(rippleSystem);
                StopAndClear(splashDropletSystem);
                StopAndClear(mistSystem);
            }

            ApplyForReview();
            StartMistIfNeeded();
        }

        public void SetReviewMultipliersForReview(float splashRippleMultiplier, float mistMultiplier, float alphaMultiplier)
        {
            reviewSplashRippleMultiplier = Mathf.Clamp(splashRippleMultiplier, 0f, 2.5f);
            reviewMistMultiplier = Mathf.Clamp(mistMultiplier, 0f, 2.5f);
            reviewAlphaMultiplier = Mathf.Clamp(alphaMultiplier, 0f, 2.5f);
            ApplyForReview();
        }

        public void ClearForReview()
        {
            ResolveReferences();
            StopAndClear(rippleSystem);
            StopAndClear(splashDropletSystem);
            StopAndClear(mistSystem);
            totalTriggeredRipples = 0;
            appliedMistEmissionRate = 0f;
            ApplyForReview();
            StartMistIfNeeded();
        }

        public void ClearTransientEntryForReview()
        {
            ResolveReferences();
            StopAndClear(rippleSystem);
            StopAndClear(splashDropletSystem);
            totalTriggeredRipples = 0;
            ApplyForReview();
        }

        public void TriggerRippleAtForReview(Vector3 worldPosition)
        {
            ResolveReferences();
            if (profile == null || rippleSystem == null || !reviewVisible || lastDistanceCulled)
            {
                return;
            }

            ApplyForReview();
            if (!rippleSystem.isPlaying)
            {
                rippleSystem.Play(true);
            }

            if (splashDropletSystem != null && !splashDropletSystem.isPlaying)
            {
                splashDropletSystem.Play(true);
            }

            var position = worldPosition + Vector3.up * 0.022f;
            var color = profile.RippleTintForReview;
            color.a = Mathf.Clamp01(reviewAlphaMultiplier);
            var emit = new ParticleSystem.EmitParams
            {
                position = position,
                velocity = Vector3.zero,
                startLifetime = profile.RippleLifetimeForReview,
                startSize = profile.RippleEndSizeForReview * Mathf.Clamp(reviewSplashRippleMultiplier, 0.15f, 2.5f),
                startColor = color,
                randomSeed = (uint)(9043 + totalTriggeredRipples * 17)
            };
            rippleSystem.Emit(emit, 1);
            totalTriggeredRipples++;
            lastTriggerWorldPosition = position;
            lastResolvedWaterWorldPosition = position;
        }

        public bool TryTriggerRippleFromRaycastForReview(Vector3 candidateWorldPosition)
        {
            ResolveReferences();
            if (profile == null || !raycastEntryHookPrepared)
            {
                lastRaycastHitWater = false;
                return false;
            }

            var origin = candidateWorldPosition + Vector3.up * profile.WaterRayHeightForReview;
            var distance = profile.WaterRayHeightForReview + profile.WaterRayDistanceForReview;
            if (Physics.Raycast(origin, Vector3.down, out var hit, distance, waterSurfaceMask, QueryTriggerInteraction.Collide) &&
                IsWaterHitForReview(hit.collider))
            {
                lastRaycastHitWater = true;
                lastResolvedWaterWorldPosition = hit.point;
                TriggerRippleAtForReview(hit.point);
                return true;
            }

            lastRaycastHitWater = false;
            return false;
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            ResolveReferences();
            var duration = Mathf.Max(0f, seconds);
            if (restart)
            {
                StopAndClear(rippleSystem);
                StopAndClear(splashDropletSystem);
                StopAndClear(mistSystem);
            }

            ApplyForReview();
            SimulateSystem(rippleSystem, duration, restart);
            SimulateSystem(splashDropletSystem, duration, restart);
            SimulateSystem(mistSystem, duration, restart);
            StartMistIfNeeded();
        }

        public void ApplyForReview()
        {
            ResolveReferences();
            var active = reviewVisible && !lastDistanceCulled && profile != null;
            ConfigureMainColor(rippleSystem, profile != null ? profile.RippleTintForReview : Color.white);
            ConfigureMainColor(splashDropletSystem, profile != null ? profile.SplashTintForReview : Color.white);
            ConfigureMainColor(mistSystem, profile != null ? profile.MistTintForReview : Color.white);

            if (mistSystem != null)
            {
                var emission = mistSystem.emission;
                emission.enabled = active && profile != null && profile.ContinuousMistEmitterForReview;
                appliedMistEmissionRate = emission.enabled ? profile.MistEmissionRateForReview * reviewMistMultiplier : 0f;
                emission.rateOverTime = new ParticleSystem.MinMaxCurve(appliedMistEmissionRate);
            }
            else
            {
                appliedMistEmissionRate = 0f;
            }

            SetRendererEnabled(rippleRenderer, active);
            SetRendererEnabled(splashRenderer, active);
            SetRendererEnabled(mistRenderer, active);
        }

        public void RefreshDistanceCullForReview()
        {
            ResolveReferences();
            ApplyDistanceCullForReview();
            ApplyForReview();
        }

        public void SetDistanceCullEnabledForReview(bool enabled)
        {
            distanceCullEnabled = enabled;
            if (!distanceCullEnabled)
            {
                lastDistanceCulled = false;
            }

            ApplyForReview();
        }

        private void ConfigureMainColor(ParticleSystem system, Color color)
        {
            if (system == null)
            {
                return;
            }

            color.a = Mathf.Clamp01(reviewAlphaMultiplier);
            var main = system.main;
            main.startColor = new ParticleSystem.MinMaxGradient(color);
        }

        private void ApplyDistanceCullForReview()
        {
            if (profile == null || !distanceCullEnabled)
            {
                lastDistanceCulled = false;
                return;
            }

            var camera = Camera.main;
            lastDistanceCulled = camera != null && Vector3.Distance(camera.transform.position, transform.position) > profile.DistanceCullFarMetersForReview;
        }

        private bool HasBirthSubEmitterForReview()
        {
            if (rippleSystem == null || splashDropletSystem == null)
            {
                return false;
            }

            var subEmitters = rippleSystem.subEmitters;
            for (var i = 0; i < subEmitters.subEmittersCount; i++)
            {
                if (subEmitters.GetSubEmitterSystem(i) == splashDropletSystem &&
                    subEmitters.GetSubEmitterType(i) == ParticleSystemSubEmitterType.Birth)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsWaterHitForReview(Collider collider)
        {
            if (collider == null)
            {
                return false;
            }

            var token = collider.name + " " + collider.gameObject.name;
            var renderer = collider.GetComponent<Renderer>();
            if (renderer != null && renderer.sharedMaterial != null)
            {
                token += " " + renderer.sharedMaterial.name;
            }

            return token.IndexOf("water", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   token.IndexOf("ripple", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   token.IndexOf("splash", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   token.IndexOf("fountain", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void ResolveReferences()
        {
            if (rippleSystem == null)
            {
                rippleSystem = transform.Find("RippleSystem")?.GetComponent<ParticleSystem>();
            }

            if (splashDropletSystem == null)
            {
                splashDropletSystem = transform.Find("SplashDropletSubEmitter")?.GetComponent<ParticleSystem>();
            }

            if (mistSystem == null)
            {
                mistSystem = transform.Find("MistSystem")?.GetComponent<ParticleSystem>();
            }

            if (rippleRenderer == null && rippleSystem != null)
            {
                rippleRenderer = rippleSystem.GetComponent<ParticleSystemRenderer>();
            }

            if (splashRenderer == null && splashDropletSystem != null)
            {
                splashRenderer = splashDropletSystem.GetComponent<ParticleSystemRenderer>();
            }

            if (mistRenderer == null && mistSystem != null)
            {
                mistRenderer = mistSystem.GetComponent<ParticleSystemRenderer>();
            }
        }

        private void StartMistIfNeeded()
        {
            if (!reviewVisible || mistSystem == null || profile == null || !profile.ContinuousMistEmitterForReview)
            {
                return;
            }

            if (!mistSystem.isPlaying)
            {
                mistSystem.Play(true);
            }
        }

        private static void SimulateSystem(ParticleSystem system, float seconds, bool restart)
        {
            if (system == null)
            {
                return;
            }

            if (!system.isPlaying)
            {
                system.Play(true);
            }

            system.Simulate(seconds, true, restart, false);
            if (!system.isPlaying)
            {
                system.Play(true);
            }
        }

        private static void StopAndClear(ParticleSystem system)
        {
            if (system == null)
            {
                return;
            }

            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            system.Clear(true);
        }

        private static void SetRendererEnabled(ParticleSystemRenderer renderer, bool enabled)
        {
            if (renderer == null)
            {
                return;
            }

            renderer.enabled = enabled;
            renderer.forceRenderingOff = false;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }
    }
}
