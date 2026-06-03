using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Rain Weather State")]
    public sealed class FastVsHd2dRainWeatherState : MonoBehaviour
    {
        private static readonly int RainWetnessId = Shader.PropertyToID("_AnemoraHd2dRainWetness");
        private static readonly int RainWetDarkenId = Shader.PropertyToID("_AnemoraHd2dRainWetDarken");
        private static readonly int RainSpecBoostId = Shader.PropertyToID("_AnemoraHd2dRainSpecBoost");
        private static readonly int DayHorizonId = Shader.PropertyToID("_DayHorizon");
        private static readonly int DayZenithId = Shader.PropertyToID("_DayZenith");
        private static readonly int SunsetHorizonId = Shader.PropertyToID("_SunsetHorizon");
        private static readonly int SunsetZenithId = Shader.PropertyToID("_SunsetZenith");
        private static readonly int NightHorizonId = Shader.PropertyToID("_NightHorizon");
        private static readonly int NightZenithId = Shader.PropertyToID("_NightZenith");
        private static readonly int GradientExposureId = Shader.PropertyToID("_GradientExposure");
        private static readonly int CloudTintId = Shader.PropertyToID("_AnemoraHd2dCloudTint");

        [SerializeField] private FastVsHd2dRainWeatherProfile profile;
        [SerializeField] private FastVsHd2dGradientSkyDriver skyDriver;
        [SerializeField] private ParticleSystem rainParticleSystem;
        [SerializeField] private Transform reviewRainProxyRoot;
        [SerializeField] private Transform reviewWetProbeRoot;
        [SerializeField] private Light directionalSun;
        [SerializeField] private bool activeOnAwake;
        [SerializeField, Range(0f, 1f)] private float activeRainIntensity;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private float reviewRainTime;
        [SerializeField] private float lastLightningAmount;
        [SerializeField] private float baseDirectionalSunIntensity = 1f;
        [SerializeField] private int lastEnabledParticleCountForReview;
        [SerializeField] private int reviewRainProxyVisibleCount;
        [SerializeField] private int lastEnabledReviewRainProxyVisibleCount;

        public bool IsReadyForReview => profile != null && rainParticleSystem != null && reviewRainProxyRoot != null;
        public FastVsHd2dRainWeatherProfile ProfileForReview => profile;
        public ParticleSystem RainParticleSystemForReview => rainParticleSystem;
        public Transform ReviewRainProxyRootForReview => reviewRainProxyRoot;
        public Transform ReviewWetProbeRootForReview => reviewWetProbeRoot;
        public FastVsHd2dGradientSkyDriver SkyDriverForReview => skyDriver;
        public Light DirectionalSunForReview => directionalSun;
        public bool ActiveOnAwakeForReview => activeOnAwake;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public float ActiveRainIntensityForReview => activeRainIntensity;
        public float LastLightningAmountForReview => lastLightningAmount;
        public float CurrentWetnessForReview => profile != null ? Mathf.Clamp01(activeRainIntensity * profile.WetnessScaleForReview) : 0f;
        public int LiveParticleCountForReview => rainParticleSystem != null ? rainParticleSystem.particleCount : 0;
        public int LastEnabledParticleCountForReview => lastEnabledParticleCountForReview;
        public int ReviewRainProxyVisibleCountForReview => reviewRainProxyVisibleCount;
        public int LastEnabledReviewRainProxyVisibleCountForReview => lastEnabledReviewRainProxyVisibleCount;
        public bool ReviewWetProbeVisibleForReview => reviewWetProbeRoot != null && reviewWetProbeRoot.gameObject.activeSelf;
        public bool ParticlesEnabledForReview
        {
            get
            {
                if (rainParticleSystem == null)
                {
                    return false;
                }

                var emission = rainParticleSystem.emission;
                return emission.enabled && emission.rateOverTime.constantMax > 0.01f;
            }
        }

        private void OnEnable()
        {
            PublishForReview();
        }

        private void OnValidate()
        {
            activeRainIntensity = Mathf.Clamp01(activeRainIntensity);
            PublishForReview();
        }

        private void LateUpdate()
        {
            if (publishEveryFrame)
            {
                PublishForReview();
            }
        }

        public void ConfigureForReview(
            FastVsHd2dRainWeatherProfile rainProfile,
            FastVsHd2dGradientSkyDriver gradientSkyDriver,
            ParticleSystem particleSystem,
            Transform rainProxyRoot,
            Transform wetProbeRoot,
            Light sunLight)
        {
            profile = rainProfile;
            skyDriver = gradientSkyDriver;
            rainParticleSystem = particleSystem;
            reviewRainProxyRoot = rainProxyRoot;
            reviewWetProbeRoot = wetProbeRoot;
            directionalSun = sunLight;
            activeOnAwake = false;
            publishEveryFrame = true;
            activeRainIntensity = profile != null ? profile.DefaultRainIntensityForReview : 0f;
            reviewRainTime = 0f;
            lastLightningAmount = 0f;
            baseDirectionalSunIntensity = directionalSun != null ? Mathf.Max(0.001f, directionalSun.intensity) : 1f;
            ConfigureParticleSystemForReview();
            SetReviewWetProbeVisibleForReview(false);
            ApplyRainAmountForReview(activeRainIntensity, activeOnAwake && activeRainIntensity > 0.001f, 0f, 0f);
        }

        public void SetReviewWetProbeVisibleForReview(bool visible)
        {
            if (reviewWetProbeRoot != null)
            {
                reviewWetProbeRoot.gameObject.SetActive(visible);
            }
        }

        public void ApplyDefaultReviewStateForReview()
        {
            ApplyRainAmountForReview(profile != null ? profile.DefaultRainIntensityForReview : 0f, false, 0f, 0f);
        }

        public void ApplyRainAmountForReview(float rainIntensity, bool enableParticles, float simulateSeconds, float lightningAmount)
        {
            activeRainIntensity = Mathf.Clamp01(rainIntensity);
            lastLightningAmount = Mathf.Clamp01(lightningAmount);
            if (simulateSeconds > 0.001f)
            {
                reviewRainTime = simulateSeconds;
            }
            else if (activeRainIntensity <= 0.001f)
            {
                reviewRainTime = 0f;
            }

            ConfigureParticleSystemForReview();
            PublishForReview();
            UpdateReviewRainProxiesForReview();
            ConfigureRainEmission(enableParticles && activeRainIntensity > 0.001f);

            if (rainParticleSystem == null)
            {
                return;
            }

            if (enableParticles && activeRainIntensity > 0.001f)
            {
                rainParticleSystem.gameObject.SetActive(true);
                rainParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                rainParticleSystem.Clear(true);
                rainParticleSystem.Play(true);
                if (simulateSeconds > 0.001f)
                {
                    rainParticleSystem.Simulate(simulateSeconds, true, true, true);
                }

                lastEnabledParticleCountForReview = rainParticleSystem.particleCount;
            }
            else
            {
                rainParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                rainParticleSystem.Clear(true);
            }
        }

        public void SimulateForReview(float seconds)
        {
            if (rainParticleSystem == null)
            {
                return;
            }

            if (!rainParticleSystem.isPlaying)
            {
                rainParticleSystem.Play(true);
            }

            var clampedSeconds = Mathf.Max(0.01f, seconds);
            reviewRainTime += clampedSeconds;
            rainParticleSystem.Simulate(clampedSeconds, true, false, true);
            lastEnabledParticleCountForReview = rainParticleSystem.particleCount;
            UpdateReviewRainProxiesForReview();
        }

        public void PublishForReview()
        {
            if (profile == null)
            {
                Shader.SetGlobalFloat(RainWetnessId, 0f);
                Shader.SetGlobalFloat(RainWetDarkenId, 0f);
                Shader.SetGlobalFloat(RainSpecBoostId, 0f);
                return;
            }

            Shader.SetGlobalFloat(RainWetnessId, CurrentWetnessForReview);
            Shader.SetGlobalFloat(RainWetDarkenId, profile.WetDarkenForReview);
            Shader.SetGlobalFloat(RainSpecBoostId, profile.WetSpecularBoostForReview);
            UpdateReviewRainProxiesForReview();
            ApplySkyFogAndLightForReview();
        }

        private void UpdateReviewRainProxiesForReview()
        {
            reviewRainProxyVisibleCount = 0;
            if (profile == null || reviewRainProxyRoot == null)
            {
                return;
            }

            var visible = activeRainIntensity > 0.001f;
            reviewRainProxyRoot.gameObject.SetActive(visible);
            if (!visible)
            {
                return;
            }

            var wind = profile.WindDirectionForReview * profile.WindDriftSpeedForReview;
            var fall = Mathf.Max(1f, profile.FallSpeedForReview);
            var childCount = reviewRainProxyRoot.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var streak = reviewRainProxyRoot.GetChild(i);
                var renderer = streak.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                    reviewRainProxyVisibleCount++;
                }

                var seed = Mathf.Repeat((i * 0.618034f) + 0.173f, 1f);
                var fallCycle = Mathf.Repeat((reviewRainTime * (1.20f + seed * 0.42f) * fall * 0.22f) + seed, 1f);
                var cross = Mathf.Repeat((seed * 5.129f) + (reviewRainTime * wind.x * 0.10f), 1f);
                var depth = Mathf.Repeat((seed * 2.731f) + (reviewRainTime * wind.z * 0.07f), 1f);
                var sway = Mathf.Sin((reviewRainTime * (2.7f + seed)) + (seed * 15.1f)) * profile.TurbulenceForReview * 0.10f;
                streak.localPosition = new Vector3(
                    Mathf.Lerp(-2.65f, 2.65f, cross) + sway,
                    Mathf.Lerp(2.30f, -0.65f, fallCycle),
                    Mathf.Lerp(-1.60f, 1.42f, depth));
                streak.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-10f, -16f, seed));
                streak.localScale = new Vector3(
                    profile.StreakWidthForReview * Mathf.Lerp(0.80f, 1.35f, seed),
                    profile.StreakLengthForReview * Mathf.Lerp(0.72f, 1.18f, Mathf.Repeat(seed * 3.1f, 1f)),
                    1f);
            }

            lastEnabledReviewRainProxyVisibleCount = reviewRainProxyVisibleCount;
        }

        private void ConfigureParticleSystemForReview()
        {
            if (profile == null || rainParticleSystem == null)
            {
                return;
            }

            rainParticleSystem.gameObject.SetActive(true);
            rainParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            rainParticleSystem.Clear(true);

            var main = rainParticleSystem.main;
            main.loop = true;
            main.playOnAwake = false;
            main.duration = Mathf.Max(2f, profile.StreakLifetimeForReview + 0.5f);
            main.startLifetime = profile.StreakLifetimeForReview;
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(profile.StreakWidthForReview * 0.72f, profile.StreakWidthForReview * 1.22f);
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.64f, 0.74f, 0.88f, 0.42f), new Color(0.86f, 0.94f, 1f, 0.62f));
            main.maxParticles = profile.MaxParticlesForReview;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.stopAction = ParticleSystemStopAction.None;

            var shape = rainParticleSystem.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(8.4f, 4.5f, 5.8f);
            shape.randomDirectionAmount = 0.02f;

            var velocity = rainParticleSystem.velocityOverLifetime;
            var wind = profile.WindDirectionForReview * profile.WindDriftSpeedForReview;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.Local;
            velocity.x = new ParticleSystem.MinMaxCurve(wind.x * 0.35f, wind.x * 0.85f);
            velocity.y = new ParticleSystem.MinMaxCurve(-profile.FallSpeedForReview * 1.10f, -profile.FallSpeedForReview * 0.82f);
            velocity.z = new ParticleSystem.MinMaxCurve(wind.z * 0.18f, wind.z * 0.52f);

            var noise = rainParticleSystem.noise;
            noise.enabled = true;
            noise.strength = profile.TurbulenceForReview;
            noise.frequency = 0.92f;
            noise.octaveCount = 1;
            noise.quality = ParticleSystemNoiseQuality.Low;

            var colorOverLifetime = rainParticleSystem.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(new Color(0.68f, 0.80f, 1f, 1f), 0f),
                    new GradientColorKey(new Color(0.86f, 0.92f, 1f, 1f), 0.45f),
                    new GradientColorKey(new Color(0.45f, 0.56f, 0.72f, 1f), 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0.00f, 0f),
                    new GradientAlphaKey(0.58f, 0.12f),
                    new GradientAlphaKey(0.44f, 0.78f),
                    new GradientAlphaKey(0.00f, 1f)
                });
            colorOverLifetime.color = gradient;
        }

        private void ConfigureRainEmission(bool enabled)
        {
            if (profile == null || rainParticleSystem == null)
            {
                return;
            }

            var emission = rainParticleSystem.emission;
            emission.enabled = enabled;
            emission.rateOverTime = enabled ? profile.RainEmissionRateForReview * Mathf.Max(0.10f, activeRainIntensity) : 0f;
        }

        private void ApplySkyFogAndLightForReview()
        {
            if (skyDriver != null)
            {
                skyDriver.PublishForReview();
            }

            var amount = Mathf.Clamp01(activeRainIntensity);
            if (amount <= 0.001f)
            {
                if (directionalSun != null)
                {
                    directionalSun.intensity = baseDirectionalSunIntensity;
                }

                return;
            }

            var lightning = Mathf.Clamp01(lastLightningAmount);
            var rainyHorizon = Color.Lerp(profile.RainyHorizonForReview, Color.white, lightning * profile.LightningSkyFlashForReview);
            var rainyZenith = Color.Lerp(profile.RainyZenithForReview, Color.white, lightning * profile.LightningSkyFlashForReview * 0.80f);
            var rainyFog = Color.Lerp(profile.RainyFogColorForReview, Color.white, lightning * profile.LightningSkyFlashForReview * 0.62f);
            if (skyDriver != null && skyDriver.SkyboxMaterialForReview != null && skyDriver.ProfileForReview != null)
            {
                var skybox = skyDriver.SkyboxMaterialForReview;
                var skyProfile = skyDriver.ProfileForReview;
                skybox.SetColor(DayHorizonId, Color.Lerp(skyProfile.DayHorizonForReview, rainyHorizon, amount));
                skybox.SetColor(DayZenithId, Color.Lerp(skyProfile.DayZenithForReview, rainyZenith, amount));
                skybox.SetColor(SunsetHorizonId, Color.Lerp(skyProfile.SunsetHorizonForReview, rainyHorizon, amount));
                skybox.SetColor(SunsetZenithId, Color.Lerp(skyProfile.SunsetZenithForReview, rainyZenith, amount));
                skybox.SetColor(NightHorizonId, Color.Lerp(skyProfile.NightHorizonForReview, rainyHorizon * 0.55f, amount));
                skybox.SetColor(NightZenithId, Color.Lerp(skyProfile.NightZenithForReview, rainyZenith * 0.62f, amount));
                skybox.SetFloat(GradientExposureId, Mathf.Lerp(skyProfile.GradientExposureForReview, 0.82f, amount));
                Shader.SetGlobalColor(CloudTintId, Color.Lerp(skyDriver.LastCloudTintForReview, rainyHorizon, amount));
                RenderSettings.skybox = skybox;
            }

            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, rainyFog, amount);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, profile.RainyFogDensityForReview, amount);
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, profile.RainyAmbientIntensityForReview, amount);
            if (directionalSun != null)
            {
                var rainyIntensity = baseDirectionalSunIntensity * Mathf.Lerp(1f, profile.DirectionalLightRainMultiplierForReview, amount);
                directionalSun.intensity = rainyIntensity + (lightning * profile.LightningDirectionalBoostForReview);
            }

            DynamicGI.UpdateEnvironment();
        }
    }
}
