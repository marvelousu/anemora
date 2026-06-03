using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Gradient Sky Profile")]
    public sealed class FastVsHd2dGradientSkyProfile : ScriptableObject
    {
        [SerializeField] private Color dayHorizon = new Color(0.58f, 0.72f, 0.90f, 1f);
        [SerializeField] private Color dayZenith = new Color(0.18f, 0.34f, 0.64f, 1f);
        [SerializeField] private Color sunsetHorizon = new Color(1.00f, 0.54f, 0.26f, 1f);
        [SerializeField] private Color sunsetZenith = new Color(0.26f, 0.24f, 0.46f, 1f);
        [SerializeField] private Color nightHorizon = new Color(0.07f, 0.10f, 0.20f, 1f);
        [SerializeField] private Color nightZenith = new Color(0.015f, 0.025f, 0.060f, 1f);
        [SerializeField] private Color sunDiscColor = new Color(1.00f, 0.78f, 0.42f, 1f);
        [SerializeField] private Color sunHaloColor = new Color(1.00f, 0.45f, 0.20f, 1f);
        [SerializeField] private Color moonColor = new Color(0.72f, 0.82f, 1.00f, 1f);
        [SerializeField, Range(0.002f, 0.08f)] private float sunDiscSize = 0.018f;
        [SerializeField, Range(0.02f, 0.35f)] private float sunHaloSize = 0.16f;
        [SerializeField, Range(0.004f, 0.08f)] private float moonSize = 0.028f;
        [SerializeField, Range(-1f, 1f)] private float moonPhase = 0.42f;
        [SerializeField, Range(2f, 32f)] private float bandCount = 11f;
        [SerializeField, Range(0f, 1f)] private float bandStrength = 0.32f;
        [SerializeField, Range(0.25f, 2.5f)] private float gradientExposure = 1.0f;
        [SerializeField, Range(0f, 1f)] private float cloudTintStrength = 0.72f;
        [SerializeField, Range(0f, 1f)] private float cloudAlpha = 0.48f;
        [SerializeField, Range(0f, 2f)] private float ambientIntensity = 0.82f;
        [SerializeField, Range(0f, 0.08f)] private float skyFogDensity = 0.012f;

        public Color DayHorizonForReview => dayHorizon;
        public Color DayZenithForReview => dayZenith;
        public Color SunsetHorizonForReview => sunsetHorizon;
        public Color SunsetZenithForReview => sunsetZenith;
        public Color NightHorizonForReview => nightHorizon;
        public Color NightZenithForReview => nightZenith;
        public Color SunDiscColorForReview => sunDiscColor;
        public Color SunHaloColorForReview => sunHaloColor;
        public Color MoonColorForReview => moonColor;
        public float SunDiscSizeForReview => sunDiscSize;
        public float SunHaloSizeForReview => sunHaloSize;
        public float MoonSizeForReview => moonSize;
        public float MoonPhaseForReview => moonPhase;
        public float BandCountForReview => bandCount;
        public float BandStrengthForReview => bandStrength;
        public float GradientExposureForReview => gradientExposure;
        public float CloudTintStrengthForReview => cloudTintStrength;
        public float CloudAlphaForReview => cloudAlpha;
        public float AmbientIntensityForReview => ambientIntensity;
        public float SkyFogDensityForReview => skyFogDensity;

        public void ConfigureForReview(
            Color configuredDayHorizon,
            Color configuredDayZenith,
            Color configuredSunsetHorizon,
            Color configuredSunsetZenith,
            Color configuredNightHorizon,
            Color configuredNightZenith,
            Color configuredSunDiscColor,
            Color configuredSunHaloColor,
            Color configuredMoonColor,
            float configuredSunDiscSize,
            float configuredSunHaloSize,
            float configuredMoonSize,
            float configuredMoonPhase,
            float configuredBandCount,
            float configuredBandStrength,
            float configuredGradientExposure,
            float configuredCloudTintStrength,
            float configuredCloudAlpha,
            float configuredAmbientIntensity,
            float configuredFogDensity)
        {
            dayHorizon = configuredDayHorizon;
            dayZenith = configuredDayZenith;
            sunsetHorizon = configuredSunsetHorizon;
            sunsetZenith = configuredSunsetZenith;
            nightHorizon = configuredNightHorizon;
            nightZenith = configuredNightZenith;
            sunDiscColor = configuredSunDiscColor;
            sunHaloColor = configuredSunHaloColor;
            moonColor = configuredMoonColor;
            sunDiscSize = Mathf.Clamp(configuredSunDiscSize, 0.002f, 0.08f);
            sunHaloSize = Mathf.Clamp(configuredSunHaloSize, 0.02f, 0.35f);
            moonSize = Mathf.Clamp(configuredMoonSize, 0.004f, 0.08f);
            moonPhase = Mathf.Clamp(configuredMoonPhase, -1f, 1f);
            bandCount = Mathf.Clamp(configuredBandCount, 2f, 32f);
            bandStrength = Mathf.Clamp01(configuredBandStrength);
            gradientExposure = Mathf.Clamp(configuredGradientExposure, 0.25f, 2.5f);
            cloudTintStrength = Mathf.Clamp01(configuredCloudTintStrength);
            cloudAlpha = Mathf.Clamp01(configuredCloudAlpha);
            ambientIntensity = Mathf.Clamp(configuredAmbientIntensity, 0f, 2f);
            skyFogDensity = Mathf.Clamp(configuredFogDensity, 0f, 0.08f);
        }

        public SkyState EvaluateForReview(float sunViewHeight)
        {
            var nightWeight = Mathf.Clamp01((-sunViewHeight * 2.25f) + 0.05f);
            var sunsetWeight = Mathf.Clamp01(1f - Mathf.Abs(sunViewHeight) * 4.2f) * (1f - nightWeight * 0.65f);
            var horizon = Color.Lerp(dayHorizon, sunsetHorizon, sunsetWeight);
            var zenith = Color.Lerp(dayZenith, sunsetZenith, sunsetWeight);
            horizon = Color.Lerp(horizon, nightHorizon, nightWeight);
            zenith = Color.Lerp(zenith, nightZenith, nightWeight);
            var cloudTint = Color.Lerp(Color.white, horizon, cloudTintStrength);
            cloudTint.a = cloudAlpha;
            return new SkyState(horizon, zenith, cloudTint, ambientIntensity, skyFogDensity, sunsetWeight, nightWeight);
        }

        public readonly struct SkyState
        {
            public readonly Color HorizonColor;
            public readonly Color ZenithColor;
            public readonly Color CloudTint;
            public readonly float AmbientIntensity;
            public readonly float FogDensity;
            public readonly float SunsetWeight;
            public readonly float NightWeight;

            public SkyState(
                Color horizonColor,
                Color zenithColor,
                Color cloudTint,
                float ambientIntensity,
                float fogDensity,
                float sunsetWeight,
                float nightWeight)
            {
                HorizonColor = horizonColor;
                ZenithColor = zenithColor;
                CloudTint = cloudTint;
                AmbientIntensity = ambientIntensity;
                FogDensity = fogDensity;
                SunsetWeight = sunsetWeight;
                NightWeight = nightWeight;
            }
        }
    }
}
