using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dGroupTargetFramingProfile", menuName = "Anemora/HD2D/Group Target Framing Profile")]
    public sealed class FastVsHd2dGroupTargetFramingProfile : ScriptableObject
    {
        [SerializeField, Range(22f, 32f)] private float baseFieldOfView = 28f;
        [SerializeField, Range(22f, 34f)] private float maxFieldOfView = 32f;
        [SerializeField, Range(28f, 35f)] private float pitchDegrees = 29f;
        [SerializeField, Range(3.5f, 8f)] private float baseDistance = 5.45f;
        [SerializeField, Range(6f, 16f)] private float maxDistance = 9.65f;
        [SerializeField, Range(0.55f, 0.88f)] private float groupFramingSize = 0.72f;
        [SerializeField, Range(0.04f, 0.16f)] private float viewportSafeMargin = 0.08f;
        [SerializeField, Range(0.35f, 0.85f)] private float actorRadius = 0.54f;
        [SerializeField, Range(0.90f, 1.80f)] private float actorVisualHeight = 1.18f;
        [SerializeField, Range(0.0f, 1.0f)] private float depthFitWeight = 0.46f;
        [SerializeField, Range(0.0f, 1.2f)] private float distancePadding = 0.42f;
        [SerializeField, Range(0.6f, 2.2f)] private float allyWeight = 1.0f;
        [SerializeField, Range(0.6f, 2.2f)] private float enemyWeight = 1.0f;
        [SerializeField, Range(1.0f, 3.0f)] private float speakerWeight = 1.85f;
        [SerializeField, Range(0.4f, 1.5f)] private float listenerWeight = 0.85f;
        [SerializeField, Range(0.0f, 0.45f)] private float dialogueScreenOffset = 0.25f;
        [SerializeField, Range(0.0f, 0.60f)] private float dialogueHeadroom = 0.24f;
        [SerializeField, Range(0f, 1.0f)] private float groupDampingSeconds = 0f;
        [SerializeField] private int inactivePriority = 6;
        [SerializeField] private int previewPriority = 170;
        [SerializeField] private bool targetGroupConfigured = true;
        [SerializeField] private bool groupFramingConfigured = true;
        [SerializeField] private bool recomposerConfigured = true;
        [SerializeField] private bool directRuntimeCameraAuthorityDisabled = true;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalGroupFramingApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep this as P2-70 camera data prep only. Tom should tune combat padding, dialogue thirds/headroom, and blend timing before this controls the live gameplay camera.";

        public float BaseFieldOfViewForReview => Mathf.Clamp(baseFieldOfView, 22f, 32f);
        public float MaxFieldOfViewForReview => Mathf.Clamp(Mathf.Max(baseFieldOfView, maxFieldOfView), 22f, 34f);
        public float PitchDegreesForReview => Mathf.Clamp(pitchDegrees, 28f, 35f);
        public float BaseDistanceForReview => Mathf.Clamp(baseDistance, 3.5f, 8f);
        public float MaxDistanceForReview => Mathf.Clamp(Mathf.Max(baseDistance, maxDistance), 6f, 16f);
        public float GroupFramingSizeForReview => Mathf.Clamp(groupFramingSize, 0.55f, 0.88f);
        public float ViewportSafeMarginForReview => Mathf.Clamp(viewportSafeMargin, 0.04f, 0.16f);
        public float ActorRadiusForReview => Mathf.Clamp(actorRadius, 0.35f, 0.85f);
        public float ActorVisualHeightForReview => Mathf.Clamp(actorVisualHeight, 0.90f, 1.80f);
        public float DepthFitWeightForReview => Mathf.Clamp01(depthFitWeight);
        public float DistancePaddingForReview => Mathf.Clamp(distancePadding, 0f, 1.2f);
        public float AllyWeightForReview => Mathf.Clamp(allyWeight, 0.6f, 2.2f);
        public float EnemyWeightForReview => Mathf.Clamp(enemyWeight, 0.6f, 2.2f);
        public float SpeakerWeightForReview => Mathf.Clamp(speakerWeight, 1f, 3f);
        public float ListenerWeightForReview => Mathf.Clamp(listenerWeight, 0.4f, 1.5f);
        public float DialogueScreenOffsetForReview => Mathf.Clamp(dialogueScreenOffset, 0f, 0.45f);
        public float DialogueHeadroomForReview => Mathf.Clamp(dialogueHeadroom, 0f, 0.60f);
        public float GroupDampingSecondsForReview => Mathf.Clamp(groupDampingSeconds, 0f, 1f);
        public int InactivePriorityForReview => inactivePriority;
        public int PreviewPriorityForReview => Mathf.Max(previewPriority, inactivePriority + 1);
        public bool TargetGroupConfiguredForReview => targetGroupConfigured;
        public bool GroupFramingConfiguredForReview => groupFramingConfigured;
        public bool RecomposerConfiguredForReview => recomposerConfigured;
        public bool DirectRuntimeCameraAuthorityDisabledForReview => directRuntimeCameraAuthorityDisabled;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalGroupFramingApprovedForReview => finalGroupFramingApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            float configuredBaseFieldOfView,
            float configuredMaxFieldOfView,
            float configuredPitchDegrees,
            float configuredBaseDistance,
            float configuredMaxDistance,
            float configuredGroupFramingSize,
            float configuredViewportSafeMargin,
            float configuredActorRadius,
            float configuredActorVisualHeight,
            float configuredDepthFitWeight,
            float configuredDistancePadding,
            float configuredAllyWeight,
            float configuredEnemyWeight,
            float configuredSpeakerWeight,
            float configuredListenerWeight,
            float configuredDialogueScreenOffset,
            float configuredDialogueHeadroom,
            float configuredGroupDampingSeconds,
            int configuredInactivePriority,
            int configuredPreviewPriority,
            bool configuredTargetGroupConfigured,
            bool configuredGroupFramingConfigured,
            bool configuredRecomposerConfigured,
            bool configuredDirectRuntimeCameraAuthorityDisabled,
            bool configuredConservativeDataPrep,
            bool configuredNeedsTomApproval,
            bool configuredFinalGroupFramingApproved,
            string configuredRecommendation)
        {
            baseFieldOfView = Mathf.Clamp(configuredBaseFieldOfView, 22f, 32f);
            maxFieldOfView = Mathf.Clamp(Mathf.Max(configuredBaseFieldOfView, configuredMaxFieldOfView), 22f, 34f);
            pitchDegrees = Mathf.Clamp(configuredPitchDegrees, 28f, 35f);
            baseDistance = Mathf.Clamp(configuredBaseDistance, 3.5f, 8f);
            maxDistance = Mathf.Clamp(Mathf.Max(configuredBaseDistance, configuredMaxDistance), 6f, 16f);
            groupFramingSize = Mathf.Clamp(configuredGroupFramingSize, 0.55f, 0.88f);
            viewportSafeMargin = Mathf.Clamp(configuredViewportSafeMargin, 0.04f, 0.16f);
            actorRadius = Mathf.Clamp(configuredActorRadius, 0.35f, 0.85f);
            actorVisualHeight = Mathf.Clamp(configuredActorVisualHeight, 0.90f, 1.80f);
            depthFitWeight = Mathf.Clamp01(configuredDepthFitWeight);
            distancePadding = Mathf.Clamp(configuredDistancePadding, 0f, 1.2f);
            allyWeight = Mathf.Clamp(configuredAllyWeight, 0.6f, 2.2f);
            enemyWeight = Mathf.Clamp(configuredEnemyWeight, 0.6f, 2.2f);
            speakerWeight = Mathf.Clamp(configuredSpeakerWeight, 1f, 3f);
            listenerWeight = Mathf.Clamp(configuredListenerWeight, 0.4f, 1.5f);
            dialogueScreenOffset = Mathf.Clamp(configuredDialogueScreenOffset, 0f, 0.45f);
            dialogueHeadroom = Mathf.Clamp(configuredDialogueHeadroom, 0f, 0.60f);
            groupDampingSeconds = Mathf.Clamp(configuredGroupDampingSeconds, 0f, 1f);
            inactivePriority = configuredInactivePriority;
            previewPriority = Mathf.Max(configuredPreviewPriority, configuredInactivePriority + 1);
            targetGroupConfigured = configuredTargetGroupConfigured;
            groupFramingConfigured = configuredGroupFramingConfigured;
            recomposerConfigured = configuredRecomposerConfigured;
            directRuntimeCameraAuthorityDisabled = configuredDirectRuntimeCameraAuthorityDisabled;
            conservativeDataPrep = configuredConservativeDataPrep;
            needsTomApproval = configuredNeedsTomApproval;
            finalGroupFramingApproved = configuredFinalGroupFramingApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
