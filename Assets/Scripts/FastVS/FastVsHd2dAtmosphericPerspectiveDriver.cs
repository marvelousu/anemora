using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [AddComponentMenu("Anemora/HD2D/Atmospheric Perspective Driver")]
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dAtmosphericPerspectiveDriver : MonoBehaviour
    {
        private static readonly int StrengthId = Shader.PropertyToID("_AnemoraHd2dAtmosphericFogStrength");
        private static readonly int NearColorId = Shader.PropertyToID("_AnemoraHd2dAtmosphericFogNearColor");
        private static readonly int FarColorId = Shader.PropertyToID("_AnemoraHd2dAtmosphericFogFarColor");
        private static readonly int DistanceBandId = Shader.PropertyToID("_AnemoraHd2dAtmosphericFogDistance");
        private static readonly int HeightBandId = Shader.PropertyToID("_AnemoraHd2dAtmosphericFogHeight");
        private static readonly int HeightStrengthId = Shader.PropertyToID("_AnemoraHd2dAtmosphericFogHeightStrength");
        private static readonly int GradientTextureId = Shader.PropertyToID("_AnemoraHd2dAtmosphericFogGradientTex");
        private static readonly int GradientBlendId = Shader.PropertyToID("_AnemoraHd2dAtmosphericFogGradientBlend");
        private static readonly int AerialTintId = Shader.PropertyToID("_AerialTint");
        private static readonly int AerialTintDistanceId = Shader.PropertyToID("_AerialTintDistance");
        private static readonly int AerialTintStrengthId = Shader.PropertyToID("_AerialTintStrength");
        private static readonly int AerialRampTintId = Shader.PropertyToID("_AnemoraHd2dAerialRampTint");
        private static readonly int AerialRampTintDistanceId = Shader.PropertyToID("_AnemoraHd2dAerialRampTintDistance");
        private static readonly int AerialRampTintStrengthId = Shader.PropertyToID("_AnemoraHd2dAerialRampTintStrength");

        [SerializeField] private VolumeProfile defaultProfile;
        [SerializeField] private VolumeProfile currentOutdoorProfile;
        [SerializeField] private VolumeProfile pastOutdoorProfile;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool suppressIndoorAreas = true;
        [SerializeField] private bool publishAerialRampTint = true;
        [SerializeField, Range(0f, 0.35f)] private float aerialRampTintStrength = 0.16f;
        [SerializeField] private Vector2 aerialRampTintDistancePadding = new Vector2(1.0f, 5.0f);

        private bool hasAtmospherePresetOverride;
        private float overrideStrength;
        private Color overrideNearColor;
        private Color overrideFarColor;
        private Texture overrideGradientTexture;
        private float overrideDistanceStart;
        private float overrideDistanceEnd;
        private Vector2 overrideHeightBand;
        private float overrideHeightStrength;
        private float overrideAerialRampTintStrength;
        private Vector2 overrideAerialRampTintDistancePadding;

        public bool HasAtmospherePresetOverrideForReview => hasAtmospherePresetOverride;
        public float OverrideStrengthForReview => overrideStrength;
        public Color OverrideFarColorForReview => overrideFarColor;
        public float OverrideAerialRampTintStrengthForReview => overrideAerialRampTintStrength;

        private void OnEnable()
        {
            ResolveReferences();
            PublishCurrentForReview();
        }

        private void LateUpdate()
        {
            if (!publishEveryFrame)
            {
                return;
            }

            PublishCurrentForReview();
        }

        private void OnDisable()
        {
            PublishDisabledForReview();
        }

        public void PublishCurrentForReview()
        {
            if (ShouldSuppressForActiveArea())
            {
                PublishDisabledForReview();
                return;
            }

            PublishProfile(currentOutdoorProfile != null ? currentOutdoorProfile : defaultProfile);
        }

        public void PublishPastForReview()
        {
            if (ShouldSuppressForActiveArea())
            {
                PublishDisabledForReview();
                return;
            }

            PublishProfile(pastOutdoorProfile != null ? pastOutdoorProfile : defaultProfile);
        }

        public void PublishDisabledForReview()
        {
            Shader.SetGlobalFloat(StrengthId, 0f);
            Shader.SetGlobalFloat(GradientBlendId, 0f);
            Shader.SetGlobalFloat(AerialTintStrengthId, 0f);
            Shader.SetGlobalFloat(AerialRampTintStrengthId, 0f);
        }

        public void SetAtmospherePresetOverrideForReview(
            float strength,
            Color nearColor,
            Color farColor,
            Texture gradientTexture,
            float distanceStart,
            float distanceEnd,
            Vector2 heightBand,
            float heightStrength,
            float presetAerialRampTintStrength,
            Vector2 presetAerialRampTintDistancePadding)
        {
            hasAtmospherePresetOverride = true;
            overrideStrength = Mathf.Clamp(strength, 0f, 0.35f);
            overrideNearColor = nearColor;
            overrideFarColor = farColor;
            overrideGradientTexture = gradientTexture;
            overrideDistanceStart = Mathf.Max(0f, distanceStart);
            overrideDistanceEnd = Mathf.Max(overrideDistanceStart + 0.25f, distanceEnd);
            overrideHeightBand = heightBand;
            overrideHeightStrength = Mathf.Clamp01(heightStrength);
            overrideAerialRampTintStrength = Mathf.Clamp(presetAerialRampTintStrength, 0f, 0.28f);
            overrideAerialRampTintDistancePadding = presetAerialRampTintDistancePadding;
            PublishCurrentForReview();
        }

        public void ClearAtmospherePresetOverrideForReview()
        {
            hasAtmospherePresetOverride = false;
            overrideGradientTexture = null;
            PublishCurrentForReview();
        }

        public static bool TryReadSettings(VolumeProfile profile, out FastVsHd2dAtmosphericPerspectiveVolume settings)
        {
            settings = null;
            return profile != null && profile.TryGet(out settings) && settings != null && settings.IsUsable;
        }

        private void PublishProfile(VolumeProfile profile)
        {
            if (hasAtmospherePresetOverride)
            {
                PublishAtmosphereValues(
                    overrideStrength,
                    overrideNearColor,
                    overrideFarColor,
                    overrideGradientTexture,
                    overrideDistanceStart,
                    overrideDistanceEnd,
                    overrideHeightBand,
                    overrideHeightStrength,
                    overrideAerialRampTintStrength,
                    overrideAerialRampTintDistancePadding);
                return;
            }

            if (!TryReadSettings(profile, out var settings))
            {
                Shader.SetGlobalFloat(StrengthId, 0f);
                Shader.SetGlobalFloat(GradientBlendId, 0f);
                Shader.SetGlobalFloat(AerialTintStrengthId, 0f);
                Shader.SetGlobalFloat(AerialRampTintStrengthId, 0f);
                return;
            }

            var distanceStart = Mathf.Max(0f, settings.distanceStart.value);
            var distanceEnd = Mathf.Max(distanceStart + 0.25f, settings.distanceEnd.value);
            var heightMin = Mathf.Min(settings.heightBand.value.x, settings.heightBand.value.y - 0.01f);
            var heightMax = Mathf.Max(settings.heightBand.value.y, heightMin + 0.01f);
            var gradient = settings.colorGradient.value;

            PublishAtmosphereValues(
                settings.strength.value,
                settings.nearColor.value,
                settings.farColor.value,
                gradient,
                distanceStart,
                distanceEnd,
                new Vector2(heightMin, heightMax),
                settings.heightStrength.value,
                aerialRampTintStrength,
                aerialRampTintDistancePadding);
        }

        private void PublishAtmosphereValues(
            float strength,
            Color nearColor,
            Color farColor,
            Texture gradient,
            float distanceStart,
            float distanceEnd,
            Vector2 heightBand,
            float heightStrength,
            float presetAerialRampTintStrength,
            Vector2 presetAerialRampTintDistancePadding)
        {
            var heightMin = Mathf.Min(heightBand.x, heightBand.y - 0.01f);
            var heightMax = Mathf.Max(heightBand.y, heightMin + 0.01f);
            var safeDistanceStart = Mathf.Max(0f, distanceStart);
            var safeDistanceEnd = Mathf.Max(safeDistanceStart + 0.25f, distanceEnd);

            Shader.SetGlobalFloat(StrengthId, Mathf.Clamp(strength, 0f, 0.35f));
            Shader.SetGlobalColor(NearColorId, nearColor);
            Shader.SetGlobalColor(FarColorId, farColor);
            Shader.SetGlobalVector(DistanceBandId, new Vector4(safeDistanceStart, safeDistanceEnd, 0f, 0f));
            Shader.SetGlobalVector(HeightBandId, new Vector4(heightMin, heightMax, 0f, 0f));
            Shader.SetGlobalFloat(HeightStrengthId, Mathf.Clamp01(heightStrength));
            Shader.SetGlobalTexture(GradientTextureId, gradient != null ? gradient : Texture2D.whiteTexture);
            Shader.SetGlobalFloat(GradientBlendId, gradient != null ? 1f : 0f);

            PublishAerialRampTint(farColor, safeDistanceStart, safeDistanceEnd, presetAerialRampTintStrength, presetAerialRampTintDistancePadding);
        }

        private void PublishAerialRampTint(
            Color farColor,
            float distanceStart,
            float distanceEnd,
            float presetAerialRampTintStrength,
            Vector2 presetAerialRampTintDistancePadding)
        {
            if (!publishAerialRampTint || presetAerialRampTintStrength <= 0.001f)
            {
                Shader.SetGlobalFloat(AerialTintStrengthId, 0f);
                Shader.SetGlobalFloat(AerialRampTintStrengthId, 0f);
                return;
            }

            var tintStart = Mathf.Max(0f, distanceStart + Mathf.Min(presetAerialRampTintDistancePadding.x, presetAerialRampTintDistancePadding.y - 0.25f));
            var tintEnd = Mathf.Max(tintStart + 0.25f, distanceEnd + Mathf.Max(presetAerialRampTintDistancePadding.y, 0.25f));
            Shader.SetGlobalColor(AerialTintId, farColor);
            Shader.SetGlobalVector(AerialTintDistanceId, new Vector4(tintStart, tintEnd, 0f, 0f));
            Shader.SetGlobalFloat(AerialTintStrengthId, Mathf.Clamp(presetAerialRampTintStrength, 0f, 0.28f));
            Shader.SetGlobalColor(AerialRampTintId, farColor);
            Shader.SetGlobalVector(AerialRampTintDistanceId, new Vector4(tintStart, tintEnd, 0f, 0f));
            Shader.SetGlobalFloat(AerialRampTintStrengthId, Mathf.Clamp(presetAerialRampTintStrength, 0f, 0.28f));
        }

        private void ResolveReferences()
        {
            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }
        }

        private bool ShouldSuppressForActiveArea()
        {
            if (!suppressIndoorAreas)
            {
                return false;
            }

            ResolveReferences();
            if (areaVisibility == null)
            {
                return false;
            }

            return areaVisibility.ActiveAreaForReview == FastVsHouseArea.Interior ||
                   areaVisibility.ActiveAreaForReview == FastVsHouseArea.Library ||
                   areaVisibility.ActiveAreaForReview == FastVsHouseArea.MiaInterior ||
                   areaVisibility.ActiveAreaForReview == FastVsHouseArea.AriaInterior;
        }
    }
}
