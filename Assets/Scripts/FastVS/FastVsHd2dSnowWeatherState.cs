using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Snow Weather State")]
    public sealed class FastVsHd2dSnowWeatherState : MonoBehaviour
    {
        private static readonly int SnowAmountId = Shader.PropertyToID("_AnemoraHd2dSnowAmount");
        private static readonly int SnowColorId = Shader.PropertyToID("_AnemoraHd2dSnowColor");
        private static readonly int SnowTopPowerId = Shader.PropertyToID("_AnemoraHd2dSnowTopPower");
        private static readonly int SnowNoiseScaleId = Shader.PropertyToID("_AnemoraHd2dSnowNoiseScale");
        private static readonly int SnowNoiseStrengthId = Shader.PropertyToID("_AnemoraHd2dSnowNoiseStrength");
        private static readonly int DayHorizonId = Shader.PropertyToID("_DayHorizon");
        private static readonly int DayZenithId = Shader.PropertyToID("_DayZenith");
        private static readonly int CloudTintId = Shader.PropertyToID("_AnemoraHd2dCloudTint");

        [SerializeField] private FastVsHd2dSnowWeatherProfile profile;
        [SerializeField] private FastVsHd2dGradientSkyDriver skyDriver;
        [SerializeField] private ParticleSystem snowParticleSystem;
        [SerializeField] private Transform reviewFlakeProxyRoot;
        [SerializeField] private Light directionalSun;
        [SerializeField] private bool activeOnAwake;
        [SerializeField, Range(0f, 1f)] private float activeSnowAmount;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private int lastEnabledParticleCountForReview;
        [SerializeField] private int reviewFlakeProxyVisibleCount;
        [SerializeField] private int lastEnabledReviewFlakeProxyVisibleCount;
        [SerializeField] private float reviewFlakeTime;

        public bool IsReadyForReview => profile != null && snowParticleSystem != null;
        public FastVsHd2dSnowWeatherProfile ProfileForReview => profile;
        public ParticleSystem SnowParticleSystemForReview => snowParticleSystem;
        public Transform ReviewFlakeProxyRootForReview => reviewFlakeProxyRoot;
        public FastVsHd2dGradientSkyDriver SkyDriverForReview => skyDriver;
        public Light DirectionalSunForReview => directionalSun;
        public bool ActiveOnAwakeForReview => activeOnAwake;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public float ActiveSnowAmountForReview => activeSnowAmount;
        public int LiveParticleCountForReview => snowParticleSystem != null ? snowParticleSystem.particleCount : 0;
        public int LastEnabledParticleCountForReview => lastEnabledParticleCountForReview;
        public int ReviewFlakeProxyVisibleCountForReview => reviewFlakeProxyVisibleCount;
        public int LastEnabledReviewFlakeProxyVisibleCountForReview => lastEnabledReviewFlakeProxyVisibleCount;
        public bool ParticlesEnabledForReview
        {
            get
            {
                if (snowParticleSystem == null)
                {
                    return false;
                }

                var emission = snowParticleSystem.emission;
                return emission.enabled && emission.rateOverTime.constantMax > 0.01f;
            }
        }

        private void OnEnable()
        {
            PublishForReview();
        }

        private void OnValidate()
        {
            activeSnowAmount = Mathf.Clamp01(activeSnowAmount);
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
            FastVsHd2dSnowWeatherProfile snowProfile,
            FastVsHd2dGradientSkyDriver gradientSkyDriver,
            ParticleSystem particleSystem,
            Transform flakeProxyRoot,
            Light sunLight)
        {
            profile = snowProfile;
            skyDriver = gradientSkyDriver;
            snowParticleSystem = particleSystem;
            reviewFlakeProxyRoot = flakeProxyRoot;
            directionalSun = sunLight;
            activeOnAwake = false;
            publishEveryFrame = true;
            activeSnowAmount = profile != null ? profile.DefaultSnowAmountForReview : 0f;
            reviewFlakeTime = 0f;
            ConfigureParticleSystemForReview();
            ApplySnowAmountForReview(activeSnowAmount, activeOnAwake && activeSnowAmount > 0.001f, 0f);
        }

        public void ApplyDefaultReviewStateForReview()
        {
            ApplySnowAmountForReview(profile != null ? profile.DefaultSnowAmountForReview : 0f, false, 0f);
        }

        public void ApplySnowAmountForReview(float snowAmount, bool enableParticles, float simulateSeconds)
        {
            activeSnowAmount = Mathf.Clamp01(snowAmount);
            if (simulateSeconds > 0.001f)
            {
                reviewFlakeTime = simulateSeconds;
            }
            else if (activeSnowAmount <= 0.001f)
            {
                reviewFlakeTime = 0f;
            }

            ConfigureParticleSystemForReview();
            PublishForReview();
            UpdateReviewFlakeProxiesForReview();
            ConfigureParticleEmission(enableParticles && activeSnowAmount > 0.001f);

            if (snowParticleSystem == null)
            {
                return;
            }

            if (enableParticles && activeSnowAmount > 0.001f)
            {
                snowParticleSystem.gameObject.SetActive(true);
                snowParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                snowParticleSystem.Clear(true);
                snowParticleSystem.Play(true);
                if (simulateSeconds > 0.001f)
                {
                    snowParticleSystem.Simulate(simulateSeconds, true, true, true);
                }

                lastEnabledParticleCountForReview = snowParticleSystem.particleCount;
            }
            else
            {
                snowParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                snowParticleSystem.Clear(true);
            }
        }

        public void SimulateForReview(float seconds)
        {
            if (snowParticleSystem == null)
            {
                return;
            }

            if (!snowParticleSystem.isPlaying)
            {
                snowParticleSystem.Play(true);
            }

            var clampedSeconds = Mathf.Max(0.01f, seconds);
            reviewFlakeTime += clampedSeconds;
            snowParticleSystem.Simulate(clampedSeconds, true, false, true);
            lastEnabledParticleCountForReview = snowParticleSystem.particleCount;
            UpdateReviewFlakeProxiesForReview();
        }

        public void PublishForReview()
        {
            if (profile == null)
            {
                Shader.SetGlobalFloat(SnowAmountId, 0f);
                return;
            }

            Shader.SetGlobalFloat(SnowAmountId, Mathf.Clamp01(activeSnowAmount));
            Shader.SetGlobalColor(SnowColorId, profile.SnowColorForReview);
            Shader.SetGlobalFloat(SnowTopPowerId, profile.TopNormalPowerForReview);
            Shader.SetGlobalFloat(SnowNoiseScaleId, profile.AccumulationNoiseScaleForReview);
            Shader.SetGlobalFloat(SnowNoiseStrengthId, profile.AccumulationNoiseStrengthForReview);
            UpdateReviewFlakeProxiesForReview();
            ApplySkyAndFogForReview();
        }

        private void UpdateReviewFlakeProxiesForReview()
        {
            reviewFlakeProxyVisibleCount = 0;
            if (profile == null || reviewFlakeProxyRoot == null)
            {
                return;
            }

            var visible = activeSnowAmount > 0.001f;
            reviewFlakeProxyRoot.gameObject.SetActive(visible);
            if (!visible)
            {
                return;
            }

            var wind = profile.WindDirectionForReview * profile.WindDriftSpeedForReview;
            var fall = Mathf.Max(0.05f, profile.FallSpeedForReview);
            var childCount = reviewFlakeProxyRoot.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var flake = reviewFlakeProxyRoot.GetChild(i);
                var renderer = flake.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                    reviewFlakeProxyVisibleCount++;
                }

                var seed = Mathf.Repeat((i * 0.381966f) + 0.137f, 1f);
                var cycle = Mathf.Repeat((reviewFlakeTime * (0.18f + (seed * 0.11f)) * fall) + seed, 1f);
                var cross = Mathf.Repeat((seed * 2.713f) + (reviewFlakeTime * wind.x * 0.12f), 1f);
                var depth = Mathf.Repeat((seed * 4.219f) + (reviewFlakeTime * wind.z * 0.08f), 1f);
                var sway = Mathf.Sin((reviewFlakeTime * (1.3f + seed)) + (seed * 18.7f)) * profile.TurbulenceForReview * 0.22f;
                flake.localPosition = new Vector3(
                    Mathf.Lerp(-2.25f, 2.25f, cross) + sway,
                    Mathf.Lerp(2.05f, 0.12f, cycle),
                    Mathf.Lerp(-1.25f, 1.10f, depth));
                var size = Mathf.Lerp(profile.FlakeMinSizeForReview, profile.FlakeMaxSizeForReview, Mathf.Repeat(seed * 3.17f, 1f));
                flake.localScale = Vector3.one * size;
            }

            lastEnabledReviewFlakeProxyVisibleCount = reviewFlakeProxyVisibleCount;
        }

        private void ConfigureParticleSystemForReview()
        {
            if (profile == null || snowParticleSystem == null)
            {
                return;
            }

            snowParticleSystem.gameObject.SetActive(true);
            snowParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            snowParticleSystem.Clear(true);

            var main = snowParticleSystem.main;
            main.loop = true;
            main.playOnAwake = false;
            main.duration = Mathf.Max(4f, profile.FlakeLifetimeForReview + 1f);
            main.startLifetime = profile.FlakeLifetimeForReview;
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(profile.FlakeMinSizeForReview, profile.FlakeMaxSizeForReview);
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.94f, 0.97f, 1f, 0.72f), new Color(1f, 1f, 1f, 0.92f));
            main.maxParticles = profile.MaxParticlesForReview;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.stopAction = ParticleSystemStopAction.None;

            var shape = snowParticleSystem.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(5.4f, 2.6f, 4.8f);
            shape.randomDirectionAmount = 0.08f;

            var velocity = snowParticleSystem.velocityOverLifetime;
            var wind = profile.WindDirectionForReview * profile.WindDriftSpeedForReview;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            velocity.x = new ParticleSystem.MinMaxCurve(wind.x * 0.72f, wind.x * 1.28f);
            velocity.y = new ParticleSystem.MinMaxCurve(-profile.FallSpeedForReview * 1.15f, -profile.FallSpeedForReview * 0.68f);
            velocity.z = new ParticleSystem.MinMaxCurve(wind.z * 0.72f, wind.z * 1.28f);

            var noise = snowParticleSystem.noise;
            noise.enabled = true;
            noise.strength = profile.TurbulenceForReview;
            noise.frequency = 0.42f;
            noise.octaveCount = 2;
            noise.quality = ParticleSystemNoiseQuality.Medium;

            var colorOverLifetime = snowParticleSystem.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(new Color(0.90f, 0.96f, 1f, 1f), 0f),
                    new GradientColorKey(Color.white, 0.52f),
                    new GradientColorKey(new Color(0.78f, 0.88f, 1f, 1f), 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0.00f, 0f),
                    new GradientAlphaKey(0.80f, 0.16f),
                    new GradientAlphaKey(0.62f, 0.72f),
                    new GradientAlphaKey(0.00f, 1f)
                });
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

            var renderer = snowParticleSystem.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.renderMode = ParticleSystemRenderMode.Billboard;
                renderer.alignment = ParticleSystemRenderSpace.View;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                renderer.minParticleSize = 0.006f;
                renderer.maxParticleSize = 0.080f;
                renderer.sortingOrder = 4;
            }
        }

        private void ConfigureParticleEmission(bool enabled)
        {
            if (profile == null || snowParticleSystem == null)
            {
                return;
            }

            var emission = snowParticleSystem.emission;
            emission.enabled = enabled;
            emission.rateOverTime = enabled
                ? new ParticleSystem.MinMaxCurve(profile.FlakeEmissionRateForReview * Mathf.Max(0.18f, activeSnowAmount))
                : new ParticleSystem.MinMaxCurve(0f);
        }

        private void ApplySkyAndFogForReview()
        {
            if (profile == null)
            {
                return;
            }

            var snowAmount = Mathf.Clamp01(activeSnowAmount);
            if (skyDriver != null)
            {
                skyDriver.PublishForReview();
            }

            if (snowAmount <= 0.001f)
            {
                return;
            }

            var skybox = skyDriver != null ? skyDriver.SkyboxMaterialForReview : RenderSettings.skybox;
            if (skybox != null)
            {
                if (skybox.HasProperty(DayHorizonId))
                {
                    skybox.SetColor(DayHorizonId, Color.Lerp(skybox.GetColor(DayHorizonId), profile.OvercastHorizonForReview, snowAmount));
                }

                if (skybox.HasProperty(DayZenithId))
                {
                    skybox.SetColor(DayZenithId, Color.Lerp(skybox.GetColor(DayZenithId), profile.OvercastZenithForReview, snowAmount));
                }
            }

            Shader.SetGlobalColor(CloudTintId, Color.Lerp(Color.white, profile.OvercastHorizonForReview, 0.58f * snowAmount));
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, profile.OvercastFogColorForReview, snowAmount);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, profile.OvercastFogDensityForReview, snowAmount);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, profile.OvercastAmbientIntensityForReview, snowAmount);

            if (directionalSun != null)
            {
                directionalSun.color = Color.Lerp(directionalSun.color, new Color(0.80f, 0.88f, 1f, 1f), 0.36f * snowAmount);
                directionalSun.intensity = Mathf.Lerp(directionalSun.intensity, 0.82f, 0.30f * snowAmount);
            }
        }
    }
}
