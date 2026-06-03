using Anemora.FastVS.SunCycle;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [AddComponentMenu("Anemora/FastVS/HD2D Dusk Night Firefly Layer")]
    public sealed class FastVsHd2dDuskNightFireflyLayer : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dDuskNightFireflyProfile profile;
        [SerializeField] private AnemoraSunCycleDriver sunCycleDriver;
        [SerializeField] private ParticleSystem[] fireflySystems;
        [SerializeField] private ParticleSystemRenderer[] fireflyRenderers;
        [SerializeField] private Light[] heroPointLights;
        [SerializeField] private bool todGated = true;
        [SerializeField] private bool conservativeReviewMode = true;
        [SerializeField] private bool requiresTomArtApproval = true;

        private bool hasPresetOverride;
        private SunPreset reviewPresetOverride = SunPreset.Morning;
        private float reviewEmissionMultiplier = 1f;
        private float appliedGate;
        private float appliedEmissionRate;

        public FastVsHd2dDuskNightFireflyProfile ProfileForReview => profile;
        public int ParticleSystemCountForReview => fireflySystems != null ? fireflySystems.Length : 0;
        public int RendererCountForReview => fireflyRenderers != null ? fireflyRenderers.Length : 0;
        public int HeroPointLightCountForReview => heroPointLights != null ? heroPointLights.Length : 0;
        public int TotalMaxParticlesForReview => CountMaxParticles();
        public bool TodGatedForReview => todGated;
        public bool ConservativeReviewModeForReview => conservativeReviewMode;
        public bool RequiresTomArtApprovalForReview => requiresTomArtApproval;
        public bool ColorOverLifetimeBlinkForReview => HasColorOverLifetime();
        public bool NoiseDriftForReview => HasNoise();
        public float AppliedGateForReview => appliedGate;
        public float AppliedEmissionRateForReview => appliedEmissionRate;
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
            FastVsHd2dDuskNightFireflyProfile configuredProfile,
            AnemoraSunCycleDriver configuredSunCycleDriver,
            ParticleSystem[] configuredFireflySystems,
            ParticleSystemRenderer[] configuredFireflyRenderers,
            Light[] configuredHeroPointLights,
            bool configuredTodGated,
            bool configuredConservativeReviewMode,
            bool configuredRequiresTomArtApproval)
        {
            profile = configuredProfile;
            sunCycleDriver = configuredSunCycleDriver;
            fireflySystems = configuredFireflySystems ?? new ParticleSystem[0];
            fireflyRenderers = configuredFireflyRenderers ?? new ParticleSystemRenderer[0];
            heroPointLights = configuredHeroPointLights ?? new Light[0];
            todGated = configuredTodGated;
            conservativeReviewMode = configuredConservativeReviewMode;
            requiresTomArtApproval = configuredRequiresTomArtApproval;
            ApplyNowForReview();
        }

        public void SetReviewPresetForReview(SunPreset preset, float emissionMultiplier)
        {
            hasPresetOverride = true;
            reviewPresetOverride = preset;
            reviewEmissionMultiplier = Mathf.Max(0f, emissionMultiplier);
            ApplyNowForReview();
        }

        public void ClearReviewPresetForReview()
        {
            hasPresetOverride = false;
            reviewEmissionMultiplier = 1f;
            ApplyNowForReview();
        }

        public float ResolveGateForReview(SunPreset preset)
        {
            if (profile == null)
            {
                return 0f;
            }

            switch (preset)
            {
                case SunPreset.Evening:
                    return profile.EveningGateForReview;
                case SunPreset.Night:
                    return profile.NightGateForReview;
                case SunPreset.Noon:
                    return profile.NoonGateForReview;
                case SunPreset.Morning:
                default:
                    return profile.MorningGateForReview;
            }
        }

        public void SimulateForReview(float seconds)
        {
            SimulateForReview(seconds, true);
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            ResolveReferences();
            ApplyNowForReview();
            if (fireflySystems == null)
            {
                return;
            }

            var duration = Mathf.Max(0f, seconds);
            for (var i = 0; i < fireflySystems.Length; i++)
            {
                if (fireflySystems[i] != null)
                {
                    fireflySystems[i].Simulate(duration, true, restart, false);
                }
            }
        }

        public void ApplyNowForReview()
        {
            ResolveReferences();
            if (profile == null)
            {
                return;
            }

            var preset = hasPresetOverride
                ? reviewPresetOverride
                : (sunCycleDriver != null ? sunCycleDriver.CurrentPreset : SunPreset.Morning);
            appliedGate = todGated ? ResolveGateForReview(preset) : 1f;
            appliedEmissionRate = profile.NightEmissionRateForReview * appliedGate * reviewEmissionMultiplier;
            var active = appliedEmissionRate > 0.01f;

            if (fireflySystems != null)
            {
                for (var i = 0; i < fireflySystems.Length; i++)
                {
                    var system = fireflySystems[i];
                    if (system == null)
                    {
                        continue;
                    }

                    var emission = system.emission;
                    emission.enabled = active;
                    emission.rateOverTime = new ParticleSystem.MinMaxCurve(appliedEmissionRate);

                    if (active && !system.isPlaying)
                    {
                        system.Play(true);
                    }
                    else if (!active && system.isPlaying)
                    {
                        system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }
                }
            }

            if (fireflyRenderers != null)
            {
                for (var i = 0; i < fireflyRenderers.Length; i++)
                {
                    if (fireflyRenderers[i] != null)
                    {
                        fireflyRenderers[i].enabled = active;
                        fireflyRenderers[i].shadowCastingMode = ShadowCastingMode.Off;
                        fireflyRenderers[i].receiveShadows = false;
                    }
                }
            }

            if (heroPointLights != null)
            {
                for (var i = 0; i < heroPointLights.Length; i++)
                {
                    var hero = heroPointLights[i];
                    if (hero == null)
                    {
                        continue;
                    }

                    hero.enabled = active;
                    hero.intensity = profile.HeroPointLightIntensityForReview * appliedGate;
                    hero.range = profile.HeroPointLightRangeForReview;
                }
            }
        }

        private void ResolveReferences()
        {
            if (sunCycleDriver == null)
            {
                sunCycleDriver = AnemoraSunCycleDriver.Instance != null ? AnemoraSunCycleDriver.Instance : FindFirstObjectByType<AnemoraSunCycleDriver>();
            }

            if (fireflySystems == null || fireflySystems.Length == 0)
            {
                fireflySystems = GetComponentsInChildren<ParticleSystem>(true);
            }

            if (fireflyRenderers == null || fireflyRenderers.Length == 0)
            {
                fireflyRenderers = GetComponentsInChildren<ParticleSystemRenderer>(true);
            }

            if (heroPointLights == null || heroPointLights.Length == 0)
            {
                heroPointLights = GetComponentsInChildren<Light>(true);
            }
        }

        private int CountMaxParticles()
        {
            var total = 0;
            if (fireflySystems == null)
            {
                return total;
            }

            for (var i = 0; i < fireflySystems.Length; i++)
            {
                if (fireflySystems[i] != null)
                {
                    total += fireflySystems[i].main.maxParticles;
                }
            }

            return total;
        }

        private bool HasColorOverLifetime()
        {
            if (fireflySystems == null)
            {
                return false;
            }

            for (var i = 0; i < fireflySystems.Length; i++)
            {
                if (fireflySystems[i] != null && fireflySystems[i].colorOverLifetime.enabled)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasNoise()
        {
            if (fireflySystems == null)
            {
                return false;
            }

            for (var i = 0; i < fireflySystems.Length; i++)
            {
                if (fireflySystems[i] != null && fireflySystems[i].noise.enabled)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
