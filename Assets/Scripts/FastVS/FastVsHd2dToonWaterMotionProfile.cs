using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dToonWaterMotionProfile", menuName = "Anemora/HD2D/Toon Water Motion Profile")]
    public sealed class FastVsHd2dToonWaterMotionProfile : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] private float toonSpecularIntensity = 0.52f;
        [SerializeField, Range(1f, 4f)] private float toonSpecularSteps = 2f;
        [SerializeField, Range(0f, 1f)] private float toonSpecularCutoff = 0.42f;
        [SerializeField, Range(0f, 0.12f)] private float vertexRippleAmplitude = 0.032f;
        [SerializeField, Range(0.1f, 8f)] private float vertexRippleFrequency = 3.2f;
        [SerializeField, Range(0f, 4f)] private float vertexRippleSpeed = 0.72f;
        [SerializeField, Range(0f, 1f)] private float vertexRippleNormalStrength = 0.55f;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalToonWaterMotionApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-62 toon-water motion baseline. Tom should tune glint band size/intensity and ripple amplitude per approved pond/river geometry after water art sign-off.";

        public float ToonSpecularIntensityForReview => Mathf.Clamp01(toonSpecularIntensity);
        public float ToonSpecularStepsForReview => Mathf.Clamp(toonSpecularSteps, 1f, 4f);
        public float ToonSpecularCutoffForReview => Mathf.Clamp01(toonSpecularCutoff);
        public float VertexRippleAmplitudeForReview => Mathf.Clamp(vertexRippleAmplitude, 0f, 0.12f);
        public float VertexRippleFrequencyForReview => Mathf.Clamp(vertexRippleFrequency, 0.1f, 8f);
        public float VertexRippleSpeedForReview => Mathf.Clamp(vertexRippleSpeed, 0f, 4f);
        public float VertexRippleNormalStrengthForReview => Mathf.Clamp01(vertexRippleNormalStrength);
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalToonWaterMotionApprovedForReview => finalToonWaterMotionApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            float configuredToonSpecularIntensity,
            float configuredToonSpecularSteps,
            float configuredToonSpecularCutoff,
            float configuredVertexRippleAmplitude,
            float configuredVertexRippleFrequency,
            float configuredVertexRippleSpeed,
            float configuredVertexRippleNormalStrength,
            bool configuredNeedsTomApproval,
            bool configuredFinalToonWaterMotionApproved,
            string configuredRecommendation)
        {
            toonSpecularIntensity = Mathf.Clamp01(configuredToonSpecularIntensity);
            toonSpecularSteps = Mathf.Clamp(configuredToonSpecularSteps, 1f, 4f);
            toonSpecularCutoff = Mathf.Clamp01(configuredToonSpecularCutoff);
            vertexRippleAmplitude = Mathf.Clamp(configuredVertexRippleAmplitude, 0f, 0.12f);
            vertexRippleFrequency = Mathf.Clamp(configuredVertexRippleFrequency, 0.1f, 8f);
            vertexRippleSpeed = Mathf.Clamp(configuredVertexRippleSpeed, 0f, 4f);
            vertexRippleNormalStrength = Mathf.Clamp01(configuredVertexRippleNormalStrength);
            needsTomApproval = configuredNeedsTomApproval;
            finalToonWaterMotionApproved = configuredFinalToonWaterMotionApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
