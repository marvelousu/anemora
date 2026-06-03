using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dCausticsProfile", menuName = "Anemora/HD2D/Caustics Profile")]
    public sealed class FastVsHd2dCausticsProfile : ScriptableObject
    {
        [SerializeField] private Color highlightColor = new Color(0.78f, 1.00f, 0.88f, 0.55f);
        [SerializeField] private Color shadowColor = new Color(0.03f, 0.18f, 0.24f, 0.25f);
        [SerializeField, Range(0f, 2f)] private float intensity = 0.62f;
        [SerializeField, Range(1f, 64f)] private float scaleA = 18f;
        [SerializeField, Range(1f, 64f)] private float scaleB = 33f;
        [SerializeField, Range(0f, 4f)] private float speedA = 0.34f;
        [SerializeField, Range(0f, 4f)] private float speedB = 0.21f;
        [SerializeField, Range(0f, 1f)] private float cutoff = 0.58f;
        [SerializeField, Range(0.01f, 0.5f)] private float edgeFeather = 0.18f;
        [SerializeField, Range(0f, 1f)] private float depthFade = 0.74f;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalCausticsApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-63 caustics baseline. Tom should tune final dapple density, brightness, and footprint feather per approved water-body geometry.";

        public Color HighlightColorForReview => highlightColor;
        public Color ShadowColorForReview => shadowColor;
        public float IntensityForReview => Mathf.Clamp(intensity, 0f, 2f);
        public float ScaleAForReview => Mathf.Clamp(scaleA, 1f, 64f);
        public float ScaleBForReview => Mathf.Clamp(scaleB, 1f, 64f);
        public float SpeedAForReview => Mathf.Clamp(speedA, 0f, 4f);
        public float SpeedBForReview => Mathf.Clamp(speedB, 0f, 4f);
        public float CutoffForReview => Mathf.Clamp01(cutoff);
        public float EdgeFeatherForReview => Mathf.Clamp(edgeFeather, 0.01f, 0.5f);
        public float DepthFadeForReview => Mathf.Clamp01(depthFade);
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalCausticsApprovedForReview => finalCausticsApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            Color configuredHighlightColor,
            Color configuredShadowColor,
            float configuredIntensity,
            float configuredScaleA,
            float configuredScaleB,
            float configuredSpeedA,
            float configuredSpeedB,
            float configuredCutoff,
            float configuredEdgeFeather,
            float configuredDepthFade,
            bool configuredNeedsTomApproval,
            bool configuredFinalCausticsApproved,
            string configuredRecommendation)
        {
            highlightColor = configuredHighlightColor;
            shadowColor = configuredShadowColor;
            intensity = Mathf.Clamp(configuredIntensity, 0f, 2f);
            scaleA = Mathf.Clamp(configuredScaleA, 1f, 64f);
            scaleB = Mathf.Clamp(configuredScaleB, 1f, 64f);
            speedA = Mathf.Clamp(configuredSpeedA, 0f, 4f);
            speedB = Mathf.Clamp(configuredSpeedB, 0f, 4f);
            cutoff = Mathf.Clamp01(configuredCutoff);
            edgeFeather = Mathf.Clamp(configuredEdgeFeather, 0.01f, 0.5f);
            depthFade = Mathf.Clamp01(configuredDepthFade);
            needsTomApproval = configuredNeedsTomApproval;
            finalCausticsApproved = configuredFinalCausticsApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
