using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/FastVS/HD2D Light Cookie Gobo Profile")]
    public sealed class FastVsHd2dLightCookieGoboProfile : ScriptableObject
    {
        [Header("Directional Canopy Dapple")]
        public Texture2D directionalCanopyCookie;
        [Range(1f, 30f)] public float directionalCookieSize = 8.5f;
        [Range(0f, 1f)] public float directionalCookieContrast = 0.32f;
        [Range(0f, 0.5f)] public float directionalCookieDriftPreview = 0.04f;

        [Header("Window / Local Light Gobo")]
        public Texture2D windowGoboCookie;
        [Range(0f, 8f)] public float windowGoboIntensity = 2.4f;
        [Range(1f, 20f)] public float windowGoboRange = 8.5f;
        [Range(1f, 90f)] public float windowGoboSpotAngle = 46f;

        [Header("Review State")]
        public bool needsTomApproval = true;
        public bool runtimePresetIntegrationApproved;
        [TextArea(2, 4)] public string reviewNotes =
            "Conservative P2-53 data staging: generated dapple and window-gobo cookies are review baselines only. Tom should approve final cookie art, intensity, scale, and runtime preset integration.";
    }
}
