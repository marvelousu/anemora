using UnityEngine;

namespace Anemora.FastVS.SunCycle
{
    [CreateAssetMenu(fileName = "SunPreset", menuName = "Anemora/HD2D/Sun Preset Data")]
    public sealed class SunPresetData : ScriptableObject
    {
        public SunPreset preset;

        [Header("Directional Light")]
        public Vector3 directionEuler;
        [ColorUsage(false, true)] public Color lightColor = Color.white;
        [Range(0f, 4f)] public float lightIntensity = 1f;
        public bool useColorTemperature = true;
        [Range(1500f, 20000f)] public float colorTemperatureKelvin = 5600f;

        [Header("Cookie")]
        public Texture2D cookieTexture;
        [ColorUsage(false, false)] public Color cookieTint = Color.white;
        [Range(1f, 30f)] public float cookieSize = 9.5f;

        [Header("Sky")]
        [ColorUsage(false, false)] public Color skyTint = Color.white;
        [Range(0f, 0.2f)] public float skySunSize = 0.04f;
        [Range(1f, 20f)] public float skySunSizeConvergence = 5f;

        [Header("Fog")]
        [ColorUsage(false, false)] public Color fogColor = Color.gray;
        [Range(0f, 0.2f)] public float fogDensity = 0.012f;

        [Header("Volumetric Fog")]
        public bool volumetricFogEnabled = true;
        [Range(-1f, 1f)] public float volumetricAnisotropy = 0.6f;
        [Range(0f, 500f)] public float volumetricMeanFreePath = 100f;
        public float volumetricBaseHeight;
        [Range(0f, 500f)] public float volumetricMaximumHeight = 30f;

        [Header("Bloom")]
        [ColorUsage(false, true)] public Color bloomTint = Color.white;

        [Header("Ambient")]
        [ColorUsage(false, false)] public Color ambientLightColor = Color.gray;

        [Header("Readability Floor")]
        [Range(0f, 0.12f)] public float readabilityFloor = 0.05f;
        [ColorUsage(false, false)] public Color readabilityFloorTint = new Color(0.20f, 0.22f, 0.28f, 1f);
        [Range(0f, 1f)] public float readabilityFloorStrength = 0.3f;

        [Header("Window Emission")]
        [Range(0f, 1.5f)] public float windowEmissionStrength = 0.25f;

        [Header("Screen Space Lens Flare")]
        [Range(0f, 4f)] public float screenSpaceLensFlareIntensity = 0.4f;

        [Header("Sun Lens Flare")]
        [Range(0f, 4f)] public float sunLensFlareIntensity = 0.5f;

        [Header("Color Lookup")]
        public Texture2D colorLookup;
        [Range(0f, 1f)] public float lutContribution = 0.6f;

        [Header("White Balance")]
        [Range(-100f, 100f)] public float volumeTemperature;
        [Range(-100f, 100f)] public float volumeTint;
    }
}
