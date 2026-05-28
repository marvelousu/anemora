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

        [Header("Bloom")]
        [ColorUsage(false, true)] public Color bloomTint = Color.white;

        [Header("Ambient")]
        [ColorUsage(false, false)] public Color ambientLightColor = Color.gray;

        [Header("Color Lookup")]
        public Texture2D colorLookup;
        [Range(0f, 1f)] public float lutContribution = 0.6f;

        [Header("White Balance")]
        [Range(-100f, 100f)] public float volumeTemperature;
        [Range(-100f, 100f)] public float volumeTint;
    }
}
