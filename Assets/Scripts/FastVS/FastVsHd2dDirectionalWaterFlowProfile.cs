using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dDirectionalWaterFlowProfile", menuName = "Anemora/HD2D/Directional Water Flow Profile")]
    public sealed class FastVsHd2dDirectionalWaterFlowProfile : ScriptableObject
    {
        [SerializeField] private Texture2D flowMap;
        [SerializeField, Range(0f, 2f)] private float flowStrength = 0.82f;
        [SerializeField, Range(0f, 3f)] private float flowSpeed = 0.95f;
        [SerializeField, Range(0f, 3f)] private float foamAdvectionStrength = 1.25f;
        [SerializeField, Range(0f, 3f)] private float specularAdvectionStrength = 0.72f;
        [SerializeField, Range(0.1f, 4f)] private float phaseBlendSharpness = 1f;
        [SerializeField, Range(0f, 1f)] private float pondStillness = 0.18f;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalDirectionalFlowApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-60 directional water-flow data prep. Tom should tune final river/trough flow speed, bend-painted flow map, foam streak density, and pond stillness after water art sign-off.";

        public Texture2D FlowMapForReview => flowMap;
        public float FlowStrengthForReview => Mathf.Clamp(flowStrength, 0f, 2f);
        public float FlowSpeedForReview => Mathf.Clamp(flowSpeed, 0f, 3f);
        public float FoamAdvectionStrengthForReview => Mathf.Clamp(foamAdvectionStrength, 0f, 3f);
        public float SpecularAdvectionStrengthForReview => Mathf.Clamp(specularAdvectionStrength, 0f, 3f);
        public float PhaseBlendSharpnessForReview => Mathf.Clamp(phaseBlendSharpness, 0.1f, 4f);
        public float PondStillnessForReview => Mathf.Clamp01(pondStillness);
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalDirectionalFlowApprovedForReview => finalDirectionalFlowApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            Texture2D configuredFlowMap,
            float configuredFlowStrength,
            float configuredFlowSpeed,
            float configuredFoamAdvectionStrength,
            float configuredSpecularAdvectionStrength,
            float configuredPhaseBlendSharpness,
            float configuredPondStillness,
            bool configuredPublishEveryFrame,
            bool configuredNeedsTomApproval,
            bool configuredFinalDirectionalFlowApproved,
            string configuredRecommendation)
        {
            flowMap = configuredFlowMap;
            flowStrength = Mathf.Clamp(configuredFlowStrength, 0f, 2f);
            flowSpeed = Mathf.Clamp(configuredFlowSpeed, 0f, 3f);
            foamAdvectionStrength = Mathf.Clamp(configuredFoamAdvectionStrength, 0f, 3f);
            specularAdvectionStrength = Mathf.Clamp(configuredSpecularAdvectionStrength, 0f, 3f);
            phaseBlendSharpness = Mathf.Clamp(configuredPhaseBlendSharpness, 0.1f, 4f);
            pondStillness = Mathf.Clamp01(configuredPondStillness);
            publishEveryFrame = configuredPublishEveryFrame;
            needsTomApproval = configuredNeedsTomApproval;
            finalDirectionalFlowApproved = configuredFinalDirectionalFlowApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
