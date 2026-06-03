using System;
using Anemora.FastVS.SunCycle;
using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dAtmospherePresetBlendProfile", menuName = "Anemora/HD2D/Atmosphere Preset Blend Profile")]
    public sealed class FastVsHd2dAtmospherePresetBlendProfile : ScriptableObject
    {
        [SerializeField] private AtmospherePreset[] presets = Array.Empty<AtmospherePreset>();
        [SerializeField, Range(0.05f, 6f)] private float transitionSeconds = 1.8f;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalAtmosphereApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-57 preset blend data prep. Tom should tune final fog colors, shaft strength, and particle density against the approved camera/grade.";

        public int PresetCountForReview => presets != null ? presets.Length : 0;
        public float TransitionSecondsForReview => Mathf.Max(0.05f, transitionSeconds);
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalAtmosphereApprovedForReview => finalAtmosphereApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            AtmospherePreset[] configuredPresets,
            float configuredTransitionSeconds,
            bool configuredPublishEveryFrame,
            bool configuredNeedsTomApproval,
            bool configuredFinalAtmosphereApproved,
            string configuredRecommendation)
        {
            presets = configuredPresets ?? Array.Empty<AtmospherePreset>();
            transitionSeconds = Mathf.Clamp(configuredTransitionSeconds, 0.05f, 6f);
            publishEveryFrame = configuredPublishEveryFrame;
            needsTomApproval = configuredNeedsTomApproval;
            finalAtmosphereApproved = configuredFinalAtmosphereApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }

        public bool TryResolvePresetForReview(SunPreset preset, out AtmospherePreset atmospherePreset)
        {
            if (presets != null)
            {
                for (var i = 0; i < presets.Length; i++)
                {
                    if (presets[i].Preset == preset)
                    {
                        atmospherePreset = presets[i];
                        return true;
                    }
                }
            }

            atmospherePreset = default;
            return false;
        }

        public AtmospherePreset ResolvePresetForReview(SunPreset preset)
        {
            if (TryResolvePresetForReview(preset, out var atmospherePreset))
            {
                return atmospherePreset;
            }

            return AtmospherePreset.Default(preset);
        }

        public AtmospherePreset EvaluateBlendForReview(SunPreset fromPreset, SunPreset toPreset, float normalizedBlend)
        {
            var from = ResolvePresetForReview(fromPreset);
            var to = ResolvePresetForReview(toPreset);
            return AtmospherePreset.Lerp(from, to, normalizedBlend);
        }

        [Serializable]
        public struct AtmospherePreset
        {
            [SerializeField] private SunPreset preset;
            [SerializeField] private float sunElevationDegrees;
            [SerializeField, Range(0f, 0.35f)] private float fogStrength;
            [SerializeField] private Color fogNearColor;
            [SerializeField] private Color fogFarColor;
            [SerializeField] private Texture2D fogGradient;
            [SerializeField, Min(0f)] private float distanceStart;
            [SerializeField, Min(0.1f)] private float distanceEnd;
            [SerializeField] private Vector2 heightBand;
            [SerializeField, Range(0f, 1f)] private float heightStrength;
            [SerializeField, Range(0f, 0.28f)] private float aerialTintStrength;
            [SerializeField] private Vector2 aerialTintDistancePadding;
            [SerializeField, Range(0f, 2.5f)] private float shaftIntensityMultiplier;
            [SerializeField] private Color shaftTint;
            [SerializeField, Range(0f, 3f)] private float ambientDustEmissionMultiplier;
            [SerializeField, Range(0f, 1f)] private float ambientDustTintWarmth;
            [SerializeField, Range(0f, 3f)] private float sunMoteEmissionMultiplier;

            public AtmospherePreset(
                SunPreset preset,
                float sunElevationDegrees,
                float fogStrength,
                Color fogNearColor,
                Color fogFarColor,
                Texture2D fogGradient,
                float distanceStart,
                float distanceEnd,
                Vector2 heightBand,
                float heightStrength,
                float aerialTintStrength,
                Vector2 aerialTintDistancePadding,
                float shaftIntensityMultiplier,
                Color shaftTint,
                float ambientDustEmissionMultiplier,
                float ambientDustTintWarmth,
                float sunMoteEmissionMultiplier)
            {
                this.preset = preset;
                this.sunElevationDegrees = sunElevationDegrees;
                this.fogStrength = Mathf.Clamp(fogStrength, 0f, 0.35f);
                this.fogNearColor = fogNearColor;
                this.fogFarColor = fogFarColor;
                this.fogGradient = fogGradient;
                this.distanceStart = Mathf.Max(0f, distanceStart);
                this.distanceEnd = Mathf.Max(this.distanceStart + 0.25f, distanceEnd);
                this.heightBand = heightBand;
                this.heightStrength = Mathf.Clamp01(heightStrength);
                this.aerialTintStrength = Mathf.Clamp(aerialTintStrength, 0f, 0.28f);
                this.aerialTintDistancePadding = aerialTintDistancePadding;
                this.shaftIntensityMultiplier = Mathf.Clamp(shaftIntensityMultiplier, 0f, 2.5f);
                this.shaftTint = shaftTint;
                this.ambientDustEmissionMultiplier = Mathf.Clamp(ambientDustEmissionMultiplier, 0f, 3f);
                this.ambientDustTintWarmth = Mathf.Clamp01(ambientDustTintWarmth);
                this.sunMoteEmissionMultiplier = Mathf.Clamp(sunMoteEmissionMultiplier, 0f, 3f);
            }

            public SunPreset Preset => preset;
            public float SunElevationDegrees => sunElevationDegrees;
            public float FogStrength => fogStrength;
            public Color FogNearColor => fogNearColor;
            public Color FogFarColor => fogFarColor;
            public Texture2D FogGradient => fogGradient;
            public float DistanceStart => distanceStart;
            public float DistanceEnd => Mathf.Max(distanceStart + 0.25f, distanceEnd);
            public Vector2 HeightBand => heightBand;
            public float HeightStrength => heightStrength;
            public float AerialTintStrength => aerialTintStrength;
            public Vector2 AerialTintDistancePadding => aerialTintDistancePadding;
            public float ShaftIntensityMultiplier => shaftIntensityMultiplier;
            public Color ShaftTint => shaftTint;
            public float AmbientDustEmissionMultiplier => ambientDustEmissionMultiplier;
            public float AmbientDustTintWarmth => ambientDustTintWarmth;
            public float SunMoteEmissionMultiplier => sunMoteEmissionMultiplier;

            public static AtmospherePreset Default(SunPreset preset)
            {
                return new AtmospherePreset(
                    preset,
                    35f,
                    0.12f,
                    new Color(0.88f, 0.78f, 0.62f, 1f),
                    new Color(0.54f, 0.64f, 0.74f, 1f),
                    null,
                    3.5f,
                    12f,
                    new Vector2(-0.4f, 3.1f),
                    0.32f,
                    0.16f,
                    new Vector2(1f, 5f),
                    1f,
                    Color.white,
                    1f,
                    0.38f,
                    1f);
            }

            public static AtmospherePreset Lerp(AtmospherePreset from, AtmospherePreset to, float t)
            {
                var blend = Mathf.Clamp01(t);
                return new AtmospherePreset(
                    to.preset,
                    Mathf.Lerp(from.sunElevationDegrees, to.sunElevationDegrees, blend),
                    Mathf.Lerp(from.fogStrength, to.fogStrength, blend),
                    Color.Lerp(from.fogNearColor, to.fogNearColor, blend),
                    Color.Lerp(from.fogFarColor, to.fogFarColor, blend),
                    blend < 0.5f ? from.fogGradient : to.fogGradient,
                    Mathf.Lerp(from.distanceStart, to.distanceStart, blend),
                    Mathf.Lerp(from.DistanceEnd, to.DistanceEnd, blend),
                    Vector2.Lerp(from.heightBand, to.heightBand, blend),
                    Mathf.Lerp(from.heightStrength, to.heightStrength, blend),
                    Mathf.Lerp(from.aerialTintStrength, to.aerialTintStrength, blend),
                    Vector2.Lerp(from.aerialTintDistancePadding, to.aerialTintDistancePadding, blend),
                    Mathf.Lerp(from.shaftIntensityMultiplier, to.shaftIntensityMultiplier, blend),
                    Color.Lerp(from.shaftTint, to.shaftTint, blend),
                    Mathf.Lerp(from.ambientDustEmissionMultiplier, to.ambientDustEmissionMultiplier, blend),
                    Mathf.Lerp(from.ambientDustTintWarmth, to.ambientDustTintWarmth, blend),
                    Mathf.Lerp(from.sunMoteEmissionMultiplier, to.sunMoteEmissionMultiplier, blend));
            }
        }
    }
}
