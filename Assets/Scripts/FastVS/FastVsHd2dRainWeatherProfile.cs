using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Rain Weather Profile")]
    public sealed class FastVsHd2dRainWeatherProfile : ScriptableObject
    {
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalRainWeatherApproved;
        [SerializeField] private bool particleSystemFallback = true;
        [SerializeField] private bool vfxGraphDeferredForTom = true;
        [SerializeField, Range(0f, 1f)] private float defaultRainIntensity;
        [SerializeField, Range(0f, 1f)] private float conservativeRainIntensity = 0.42f;
        [SerializeField, Range(0f, 1f)] private float strongerRainIntensity = 0.62f;
        [SerializeField] private Color rainyHorizon = new Color(0.42f, 0.48f, 0.56f, 1f);
        [SerializeField] private Color rainyZenith = new Color(0.22f, 0.28f, 0.36f, 1f);
        [SerializeField] private Color rainyFogColor = new Color(0.45f, 0.52f, 0.59f, 1f);
        [SerializeField, Range(0f, 0.08f)] private float rainyFogDensity = 0.024f;
        [SerializeField, Range(0f, 2f)] private float rainyAmbientIntensity = 0.74f;
        [SerializeField, Range(0f, 1.5f)] private float directionalLightRainMultiplier = 0.66f;
        [SerializeField, Range(0f, 1f)] private float wetnessScale = 0.54f;
        [SerializeField, Range(0f, 1f)] private float wetDarken = 0.34f;
        [SerializeField, Range(0f, 1.5f)] private float wetSpecularBoost = 0.48f;
        [SerializeField] private Vector3 windDirection = new Vector3(0.62f, 0f, 0.78f);
        [SerializeField, Range(0f, 4f)] private float windDriftSpeed = 0.46f;
        [SerializeField, Range(1f, 18f)] private float fallSpeed = 8.4f;
        [SerializeField, Range(0f, 3f)] private float turbulence = 0.34f;
        [SerializeField, Range(10f, 900f)] private float rainEmissionRate = 520f;
        [SerializeField, Range(32, 1800)] private int maxParticles = 1200;
        [SerializeField, Range(0.005f, 0.08f)] private float streakWidth = 0.042f;
        [SerializeField, Range(0.10f, 1.50f)] private float streakLength = 1.05f;
        [SerializeField, Range(0.10f, 2.50f)] private float streakLifetime = 0.58f;
        [SerializeField, Range(12, 96)] private int reviewProxyStreakCount = 76;
        [SerializeField, Range(0f, 4f)] private float lightningDirectionalBoost = 1.65f;
        [SerializeField, Range(0f, 1f)] private float lightningSkyFlash = 0.36f;
        [SerializeField] private string recommendation = "Keep this as conservative rain-weather data only. Tom should approve final VFX Graph rain density, sky grade, fog density, wetness/specular strength, and lightning cadence.";

        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalRainWeatherApprovedForReview => finalRainWeatherApproved;
        public bool ParticleSystemFallbackForReview => particleSystemFallback;
        public bool VfxGraphDeferredForTomForReview => vfxGraphDeferredForTom;
        public float DefaultRainIntensityForReview => defaultRainIntensity;
        public float ConservativeRainIntensityForReview => conservativeRainIntensity;
        public float StrongerRainIntensityForReview => strongerRainIntensity;
        public Color RainyHorizonForReview => rainyHorizon;
        public Color RainyZenithForReview => rainyZenith;
        public Color RainyFogColorForReview => rainyFogColor;
        public float RainyFogDensityForReview => rainyFogDensity;
        public float RainyAmbientIntensityForReview => rainyAmbientIntensity;
        public float DirectionalLightRainMultiplierForReview => directionalLightRainMultiplier;
        public float WetnessScaleForReview => wetnessScale;
        public float WetDarkenForReview => wetDarken;
        public float WetSpecularBoostForReview => wetSpecularBoost;
        public Vector3 WindDirectionForReview => SanitizeDirection(windDirection);
        public float WindDriftSpeedForReview => windDriftSpeed;
        public float FallSpeedForReview => fallSpeed;
        public float TurbulenceForReview => turbulence;
        public float RainEmissionRateForReview => rainEmissionRate;
        public int MaxParticlesForReview => maxParticles;
        public float StreakWidthForReview => streakWidth;
        public float StreakLengthForReview => streakLength;
        public float StreakLifetimeForReview => streakLifetime;
        public int ReviewProxyStreakCountForReview => reviewProxyStreakCount;
        public float LightningDirectionalBoostForReview => lightningDirectionalBoost;
        public float LightningSkyFlashForReview => lightningSkyFlash;
        public string RecommendationForReview => recommendation;

        public void ConfigureForReview(
            float configuredDefaultRainIntensity,
            float configuredConservativeRainIntensity,
            float configuredStrongerRainIntensity,
            Color configuredRainyHorizon,
            Color configuredRainyZenith,
            Color configuredRainyFogColor,
            float configuredRainyFogDensity,
            float configuredRainyAmbientIntensity,
            float configuredDirectionalLightRainMultiplier,
            float configuredWetnessScale,
            float configuredWetDarken,
            float configuredWetSpecularBoost,
            Vector3 configuredWindDirection,
            float configuredWindDriftSpeed,
            float configuredFallSpeed,
            float configuredTurbulence,
            float configuredRainEmissionRate,
            int configuredMaxParticles,
            float configuredStreakWidth,
            float configuredStreakLength,
            float configuredStreakLifetime,
            int configuredReviewProxyStreakCount,
            float configuredLightningDirectionalBoost,
            float configuredLightningSkyFlash,
            string configuredRecommendation)
        {
            needsTomApproval = true;
            finalRainWeatherApproved = false;
            particleSystemFallback = true;
            vfxGraphDeferredForTom = true;
            defaultRainIntensity = Mathf.Clamp01(configuredDefaultRainIntensity);
            conservativeRainIntensity = Mathf.Clamp01(configuredConservativeRainIntensity);
            strongerRainIntensity = Mathf.Clamp01(configuredStrongerRainIntensity);
            rainyHorizon = configuredRainyHorizon;
            rainyHorizon.a = 1f;
            rainyZenith = configuredRainyZenith;
            rainyZenith.a = 1f;
            rainyFogColor = configuredRainyFogColor;
            rainyFogColor.a = 1f;
            rainyFogDensity = Mathf.Clamp(configuredRainyFogDensity, 0f, 0.08f);
            rainyAmbientIntensity = Mathf.Clamp(configuredRainyAmbientIntensity, 0f, 2f);
            directionalLightRainMultiplier = Mathf.Clamp(configuredDirectionalLightRainMultiplier, 0f, 1.5f);
            wetnessScale = Mathf.Clamp01(configuredWetnessScale);
            wetDarken = Mathf.Clamp01(configuredWetDarken);
            wetSpecularBoost = Mathf.Clamp(configuredWetSpecularBoost, 0f, 1.5f);
            windDirection = SanitizeDirection(configuredWindDirection);
            windDriftSpeed = Mathf.Clamp(configuredWindDriftSpeed, 0f, 4f);
            fallSpeed = Mathf.Clamp(configuredFallSpeed, 1f, 18f);
            turbulence = Mathf.Clamp(configuredTurbulence, 0f, 3f);
            rainEmissionRate = Mathf.Clamp(configuredRainEmissionRate, 10f, 900f);
            maxParticles = Mathf.Clamp(configuredMaxParticles, 32, 1800);
            streakWidth = Mathf.Clamp(configuredStreakWidth, 0.005f, 0.08f);
            streakLength = Mathf.Clamp(configuredStreakLength, 0.10f, 1.50f);
            streakLifetime = Mathf.Clamp(configuredStreakLifetime, 0.10f, 2.50f);
            reviewProxyStreakCount = Mathf.Clamp(configuredReviewProxyStreakCount, 12, 96);
            lightningDirectionalBoost = Mathf.Clamp(configuredLightningDirectionalBoost, 0f, 4f);
            lightningSkyFlash = Mathf.Clamp01(configuredLightningSkyFlash);
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
