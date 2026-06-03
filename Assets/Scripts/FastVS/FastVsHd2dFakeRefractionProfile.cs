using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dFakeRefractionProfile", menuName = "Anemora/HD2D/Fake Refraction Profile")]
    public sealed class FastVsHd2dFakeRefractionProfile : ScriptableObject
    {
        [SerializeField, Range(0f, 8f)] private float strengthPixels = 2.4f;
        [SerializeField, Range(1f, 64f)] private float noiseScale = 18f;
        [SerializeField, Range(0f, 3f)] private float scrollSpeed = 0.82f;
        [SerializeField, Range(0.01f, 1f)] private float depthFade = 0.28f;
        [SerializeField, Range(0f, 1f)] private float sceneBlend = 0.34f;
        [SerializeField, Range(0f, 1f)] private float edgeGuard = 0.78f;
        [SerializeField] private bool autoSafeAccepted;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Auto-safe P2-61 fake refraction baseline. Keep the effect subtle, clamp to a few pixels, and tune strength only after approved shallow-water art is present.";

        public float StrengthPixelsForReview => Mathf.Clamp(strengthPixels, 0f, 8f);
        public float NoiseScaleForReview => Mathf.Clamp(noiseScale, 1f, 64f);
        public float ScrollSpeedForReview => Mathf.Clamp(scrollSpeed, 0f, 3f);
        public float DepthFadeForReview => Mathf.Clamp(depthFade, 0.01f, 1f);
        public float SceneBlendForReview => Mathf.Clamp01(sceneBlend);
        public float EdgeGuardForReview => Mathf.Clamp01(edgeGuard);
        public bool AutoSafeAcceptedForReview => autoSafeAccepted;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            float configuredStrengthPixels,
            float configuredNoiseScale,
            float configuredScrollSpeed,
            float configuredDepthFade,
            float configuredSceneBlend,
            float configuredEdgeGuard,
            bool configuredAutoSafeAccepted,
            string configuredRecommendation)
        {
            strengthPixels = Mathf.Clamp(configuredStrengthPixels, 0f, 8f);
            noiseScale = Mathf.Clamp(configuredNoiseScale, 1f, 64f);
            scrollSpeed = Mathf.Clamp(configuredScrollSpeed, 0f, 3f);
            depthFade = Mathf.Clamp(configuredDepthFade, 0.01f, 1f);
            sceneBlend = Mathf.Clamp01(configuredSceneBlend);
            edgeGuard = Mathf.Clamp01(configuredEdgeGuard);
            autoSafeAccepted = configuredAutoSafeAccepted;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
