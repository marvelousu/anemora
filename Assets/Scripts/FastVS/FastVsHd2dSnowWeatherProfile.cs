using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Snow Weather Profile")]
    public sealed class FastVsHd2dSnowWeatherProfile : ScriptableObject
    {
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalSnowWeatherApproved;
        [SerializeField] private bool particleSystemFallback = true;
        [SerializeField] private bool vfxGraphDeferredForTom = true;
        [SerializeField, Range(0f, 1f)] private float defaultSnowAmount;
        [SerializeField, Range(0f, 1f)] private float conservativeSnowAmount = 0.28f;
        [SerializeField, Range(0f, 1f)] private float strongerSnowAmount = 0.52f;
        [SerializeField] private Color snowColor = new Color(0.92f, 0.96f, 1.0f, 1f);
        [SerializeField, Range(1f, 12f)] private float topNormalPower = 4.2f;
        [SerializeField, Range(0.05f, 8f)] private float accumulationNoiseScale = 1.8f;
        [SerializeField, Range(0f, 0.5f)] private float accumulationNoiseStrength = 0.18f;
        [SerializeField] private Color overcastHorizon = new Color(0.76f, 0.84f, 0.92f, 1f);
        [SerializeField] private Color overcastZenith = new Color(0.48f, 0.58f, 0.70f, 1f);
        [SerializeField] private Color overcastFogColor = new Color(0.72f, 0.82f, 0.92f, 1f);
        [SerializeField, Range(0f, 0.08f)] private float overcastFogDensity = 0.018f;
        [SerializeField, Range(0f, 2f)] private float overcastAmbientIntensity = 1.08f;
        [SerializeField] private Vector3 windDirection = new Vector3(0.78f, 0f, 0.62f);
        [SerializeField, Range(0f, 2f)] private float windDriftSpeed = 0.36f;
        [SerializeField, Range(0.05f, 2f)] private float fallSpeed = 0.62f;
        [SerializeField, Range(0f, 2f)] private float turbulence = 0.24f;
        [SerializeField, Range(1f, 120f)] private float flakeEmissionRate = 38f;
        [SerializeField, Range(8, 512)] private int maxParticles = 180;
        [SerializeField, Range(0.006f, 0.12f)] private float flakeMinSize = 0.028f;
        [SerializeField, Range(0.006f, 0.16f)] private float flakeMaxSize = 0.060f;
        [SerializeField, Range(0.5f, 12f)] private float flakeLifetime = 5.2f;
        [SerializeField] private string recommendation = "Keep this as conservative snow-weather data only. Tom should approve final snow amount, sky brightness, flake density, drift speed, and whether to replace the ParticleSystem fallback with VFX Graph turbulence.";

        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalSnowWeatherApprovedForReview => finalSnowWeatherApproved;
        public bool ParticleSystemFallbackForReview => particleSystemFallback;
        public bool VfxGraphDeferredForTomForReview => vfxGraphDeferredForTom;
        public float DefaultSnowAmountForReview => defaultSnowAmount;
        public float ConservativeSnowAmountForReview => conservativeSnowAmount;
        public float StrongerSnowAmountForReview => strongerSnowAmount;
        public Color SnowColorForReview => snowColor;
        public float TopNormalPowerForReview => topNormalPower;
        public float AccumulationNoiseScaleForReview => accumulationNoiseScale;
        public float AccumulationNoiseStrengthForReview => accumulationNoiseStrength;
        public Color OvercastHorizonForReview => overcastHorizon;
        public Color OvercastZenithForReview => overcastZenith;
        public Color OvercastFogColorForReview => overcastFogColor;
        public float OvercastFogDensityForReview => overcastFogDensity;
        public float OvercastAmbientIntensityForReview => overcastAmbientIntensity;
        public Vector3 WindDirectionForReview => SanitizeDirection(windDirection);
        public float WindDriftSpeedForReview => windDriftSpeed;
        public float FallSpeedForReview => fallSpeed;
        public float TurbulenceForReview => turbulence;
        public float FlakeEmissionRateForReview => flakeEmissionRate;
        public int MaxParticlesForReview => maxParticles;
        public float FlakeMinSizeForReview => flakeMinSize;
        public float FlakeMaxSizeForReview => Mathf.Max(flakeMinSize, flakeMaxSize);
        public float FlakeLifetimeForReview => flakeLifetime;
        public string RecommendationForReview => recommendation;

        public void ConfigureForReview(
            float configuredDefaultSnowAmount,
            float configuredConservativeSnowAmount,
            float configuredStrongerSnowAmount,
            Color configuredSnowColor,
            float configuredTopNormalPower,
            float configuredAccumulationNoiseScale,
            float configuredAccumulationNoiseStrength,
            Color configuredOvercastHorizon,
            Color configuredOvercastZenith,
            Color configuredOvercastFogColor,
            float configuredOvercastFogDensity,
            float configuredOvercastAmbientIntensity,
            Vector3 configuredWindDirection,
            float configuredWindDriftSpeed,
            float configuredFallSpeed,
            float configuredTurbulence,
            float configuredFlakeEmissionRate,
            int configuredMaxParticles,
            float configuredFlakeMinSize,
            float configuredFlakeMaxSize,
            float configuredFlakeLifetime,
            string configuredRecommendation)
        {
            needsTomApproval = true;
            finalSnowWeatherApproved = false;
            particleSystemFallback = true;
            vfxGraphDeferredForTom = true;
            defaultSnowAmount = Mathf.Clamp01(configuredDefaultSnowAmount);
            conservativeSnowAmount = Mathf.Clamp01(configuredConservativeSnowAmount);
            strongerSnowAmount = Mathf.Clamp01(configuredStrongerSnowAmount);
            snowColor = configuredSnowColor;
            snowColor.a = 1f;
            topNormalPower = Mathf.Clamp(configuredTopNormalPower, 1f, 12f);
            accumulationNoiseScale = Mathf.Clamp(configuredAccumulationNoiseScale, 0.05f, 8f);
            accumulationNoiseStrength = Mathf.Clamp(configuredAccumulationNoiseStrength, 0f, 0.5f);
            overcastHorizon = configuredOvercastHorizon;
            overcastHorizon.a = 1f;
            overcastZenith = configuredOvercastZenith;
            overcastZenith.a = 1f;
            overcastFogColor = configuredOvercastFogColor;
            overcastFogColor.a = 1f;
            overcastFogDensity = Mathf.Clamp(configuredOvercastFogDensity, 0f, 0.08f);
            overcastAmbientIntensity = Mathf.Clamp(configuredOvercastAmbientIntensity, 0f, 2f);
            windDirection = SanitizeDirection(configuredWindDirection);
            windDriftSpeed = Mathf.Clamp(configuredWindDriftSpeed, 0f, 2f);
            fallSpeed = Mathf.Clamp(configuredFallSpeed, 0.05f, 2f);
            turbulence = Mathf.Clamp(configuredTurbulence, 0f, 2f);
            flakeEmissionRate = Mathf.Clamp(configuredFlakeEmissionRate, 1f, 120f);
            maxParticles = Mathf.Clamp(configuredMaxParticles, 8, 512);
            flakeMinSize = Mathf.Clamp(configuredFlakeMinSize, 0.006f, 0.12f);
            flakeMaxSize = Mathf.Clamp(Mathf.Max(configuredFlakeMinSize, configuredFlakeMaxSize), 0.006f, 0.16f);
            flakeLifetime = Mathf.Clamp(configuredFlakeLifetime, 0.5f, 12f);
            recommendation = string.IsNullOrWhiteSpace(configuredRecommendation) ? recommendation : configuredRecommendation;
        }

        private static Vector3 SanitizeDirection(Vector3 direction)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = new Vector3(1f, 0f, 0.25f);
            }

            return direction.normalized;
        }
    }
}
