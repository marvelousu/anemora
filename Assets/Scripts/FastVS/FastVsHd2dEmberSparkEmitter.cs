using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DefaultExecutionOrder(126)]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Ember Spark Emitter")]
    public sealed class FastVsHd2dEmberSparkEmitter : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dEmberSparkProfile profile;
        [SerializeField] private FastVsHd2dEmberSparkSourceKind sourceKind;
        [SerializeField, FormerlySerializedAs("particleSystem")] private ParticleSystem emberParticleSystem;
        [SerializeField] private ParticleSystemRenderer particleRenderer;
        [SerializeField] private Light flickerPointLight;
        [SerializeField] private bool flickerLightEnabled = true;
        [SerializeField] private bool distanceCullEnabled = true;
        [SerializeField, Range(0f, 2.5f)] private float reviewRateMultiplier = 1f;
        [SerializeField, Range(0f, 2.5f)] private float reviewLightMultiplier = 1f;
        [SerializeField] private bool reviewVisible = true;

        private bool lastDistanceCulled;
        private float reviewTimeOverride = -1f;
        private float appliedEmissionRate;
        private float appliedLightIntensity;

        public FastVsHd2dEmberSparkProfile ProfileForReview => profile;
        public FastVsHd2dEmberSparkSourceKind SourceKindForReview => sourceKind;
        public ParticleSystem ParticleSystemForReview => emberParticleSystem;
        public ParticleSystemRenderer ParticleRendererForReview => particleRenderer;
        public Light FlickerPointLightForReview => flickerPointLight;
        public bool FlickerLightEnabledForReview => flickerLightEnabled;
        public bool DistanceCullEnabledForReview => distanceCullEnabled;
        public float ReviewRateMultiplierForReview => reviewRateMultiplier;
        public float ReviewLightMultiplierForReview => reviewLightMultiplier;
        public bool ReviewVisibleForReview => reviewVisible;
        public bool LastDistanceCulledForReview => lastDistanceCulled;
        public float AppliedEmissionRateForReview => appliedEmissionRate;
        public float AppliedLightIntensityForReview => appliedLightIntensity;
        public int LiveParticleCountForReview => emberParticleSystem != null ? emberParticleSystem.particleCount : 0;

        private void OnEnable()
        {
            ResolveReferences();
            ApplyForReview();
            if (reviewVisible && emberParticleSystem != null && !emberParticleSystem.isPlaying)
            {
                emberParticleSystem.Play(true);
            }
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
            FastVsHd2dEmberSparkProfile configuredProfile,
            FastVsHd2dEmberSparkSourceKind configuredSourceKind,
            ParticleSystem configuredParticleSystem,
            ParticleSystemRenderer configuredRenderer,
            Light configuredFlickerPointLight,
            bool configuredFlickerLightEnabled,
            bool configuredDistanceCullEnabled)
        {
            profile = configuredProfile;
            sourceKind = configuredSourceKind;
            emberParticleSystem = configuredParticleSystem;
            particleRenderer = configuredRenderer;
            flickerPointLight = configuredFlickerPointLight;
            flickerLightEnabled = configuredFlickerLightEnabled;
            distanceCullEnabled = configuredDistanceCullEnabled;
            reviewVisible = true;
            reviewRateMultiplier = 1f;
            reviewLightMultiplier = 1f;
            reviewTimeOverride = -1f;
            ApplyForReview();
        }

        public void SetReviewVisibleForReview(bool visible)
        {
            reviewVisible = visible;
            ResolveReferences();
            if (emberParticleSystem != null && !visible)
            {
                emberParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            ApplyForReview();
            if (emberParticleSystem != null && visible && !emberParticleSystem.isPlaying)
            {
                emberParticleSystem.Play(true);
            }
        }

        public void SetReviewRateMultiplierForReview(float multiplier)
        {
            reviewRateMultiplier = Mathf.Clamp(multiplier, 0f, 2.5f);
            ApplyForReview();
        }

        public void SetReviewLightMultiplierForReview(float multiplier)
        {
            reviewLightMultiplier = Mathf.Clamp(multiplier, 0f, 2.5f);
            ApplyForReview();
        }

        public void SetReviewTimeForReview(float seconds)
        {
            reviewTimeOverride = Mathf.Max(0f, seconds);
            ApplyForReview();
        }

        public void ClearForReview()
        {
            ResolveReferences();
            emberParticleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            appliedEmissionRate = 0f;
            appliedLightIntensity = 0f;
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            ResolveReferences();
            if (emberParticleSystem == null || !reviewVisible)
            {
                ApplyForReview();
                return;
            }

            var duration = Mathf.Max(0f, seconds);
            if (restart)
            {
                emberParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                reviewTimeOverride = duration;
                ApplyForReview();
                emberParticleSystem.Simulate(duration, true, true, false);
            }
            else
            {
                reviewTimeOverride = ResolveReviewTimeForReview() + duration;
                ApplyForReview();
                emberParticleSystem.Simulate(duration, true, false, false);
            }

            if (!emberParticleSystem.isPlaying)
            {
                emberParticleSystem.Play(true);
            }
        }

        public void ApplyForReview()
        {
            ResolveReferences();
            if (emberParticleSystem == null || profile == null)
            {
                appliedEmissionRate = 0f;
                appliedLightIntensity = 0f;
                SetRendererEnabled(false);
                ApplyLight(false);
                return;
            }

            var active = reviewVisible && !lastDistanceCulled;
            var main = emberParticleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(Color.white);

            var emission = emberParticleSystem.emission;
            emission.enabled = active;
            appliedEmissionRate = active ? ResolveFlickeredEmissionRate() : 0f;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(appliedEmissionRate);

            SetRendererEnabled(active);
            ApplyLight(active);
        }

        private void ApplyDistanceCullForReview()
        {
            if (profile == null || !distanceCullEnabled)
            {
                lastDistanceCulled = false;
                return;
            }

            var camera = Camera.main;
            var shouldCull = false;
            if (camera != null)
            {
                shouldCull = Vector3.Distance(camera.transform.position, transform.position) > profile.DistanceCullFarMetersForReview;
            }

            lastDistanceCulled = shouldCull;
        }

        private float ResolveFlickeredEmissionRate()
        {
            var baseRate = profile.ResolveEmissionRateForReview(sourceKind) * reviewRateMultiplier;
            var phase = profile.ResolveFlickerPhaseForReview(sourceKind);
            var time = ResolveReviewTimeForReview();
            var wave = Mathf.Sin((time * profile.FlickerFrequencyForReview * Mathf.PI * 2f) + phase);
            var multiplier = 1f + (wave * profile.FlickerAmplitudeForReview);
            return Mathf.Max(0f, baseRate * multiplier);
        }

        private float ResolveReviewTimeForReview()
        {
            if (reviewTimeOverride >= 0f)
            {
                return reviewTimeOverride;
            }

            return Application.isPlaying ? Time.time : Time.realtimeSinceStartup;
        }

        private void ApplyLight(bool active)
        {
            if (flickerPointLight == null)
            {
                appliedLightIntensity = 0f;
                return;
            }

            var lightActive = active && flickerLightEnabled && profile != null && profile.FlickerPointLightsEnabledForReview;
            flickerPointLight.enabled = lightActive;
            if (!lightActive)
            {
                flickerPointLight.intensity = 0f;
                appliedLightIntensity = 0f;
                return;
            }

            var phase = profile.ResolveFlickerPhaseForReview(sourceKind) + 0.67f;
            var time = ResolveReviewTimeForReview();
            var wave = Mathf.Sin((time * profile.FlickerFrequencyForReview * Mathf.PI * 2f) + phase);
            var multiplier = 1f + (wave * profile.FlickerAmplitudeForReview * 0.55f);
            appliedLightIntensity = Mathf.Max(0f, profile.PointLightIntensityForReview * reviewLightMultiplier * multiplier);
            flickerPointLight.color = profile.PointLightColorForReview;
            flickerPointLight.intensity = appliedLightIntensity;
            flickerPointLight.range = profile.PointLightRangeForReview;
            flickerPointLight.shadows = LightShadows.None;
            flickerPointLight.renderMode = LightRenderMode.ForcePixel;
        }

        private void ResolveReferences()
        {
            if (emberParticleSystem == null)
            {
                emberParticleSystem = GetComponent<ParticleSystem>();
            }

            if (particleRenderer == null)
            {
                particleRenderer = GetComponent<ParticleSystemRenderer>();
            }

            if (flickerPointLight == null)
            {
                flickerPointLight = GetComponent<Light>();
            }
        }

        private void SetRendererEnabled(bool enabled)
        {
            if (particleRenderer == null)
            {
                return;
            }

            particleRenderer.enabled = enabled;
            particleRenderer.forceRenderingOff = false;
            particleRenderer.shadowCastingMode = ShadowCastingMode.Off;
            particleRenderer.receiveShadows = false;
        }
    }
}
