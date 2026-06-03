using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dDioramaEdgeTreatmentProfile", menuName = "Anemora/HD2D/Diorama Edge Treatment Profile")]
    public sealed class FastVsHd2dDioramaEdgeTreatmentProfile : ScriptableObject
    {
        [SerializeField, Range(1, 20)] private int minimumDioramaRootCount = 10;
        [SerializeField, Range(0, 2000)] private int minimumMarkerCount = 260;
        [SerializeField, Range(0, 12)] private int minimumMapCount = 5;
        [SerializeField, Range(0, 8)] private int minimumTreatmentTypeCount = 4;
        [SerializeField, Min(0f)] private float targetVisibleEdgeCoverageMeters = 520f;
        [SerializeField, Min(0f)] private float targetFoliageSkirtCoverageMeters = 190f;
        [SerializeField, Min(0f)] private float cliffLipDropMeters = 0.78f;
        [SerializeField, Min(0f)] private float valueDropBandMeters = 1.18f;
        [SerializeField, Min(0f)] private float edgeAoBandWidthMeters = 0.42f;
        [SerializeField, Min(0.25f)] private float foliageSkirtSpacingMeters = 3.45f;
        [SerializeField] private bool conservativeTreatmentEnabled = true;
        [SerializeField] private bool staticGiRequired = true;
        [SerializeField] private bool gpuInstancingRequired = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalDioramaEdgeTreatmentApproved;
        [SerializeField, TextArea(2, 5)] private string sourceNote =
            "P2-75 composes existing static geometry, ramp-lit pixel materials, alpha-clip foliage cards, and dark value bands; no new render pipeline feature is introduced.";
        [SerializeField, TextArea(2, 5)] private string recommendation =
            "Keep the conservative cliff lip, foliage skirt, and value-drop data only. Tom should tune final edge silhouettes, fog value, foliage density, and per-map rim language.";

        public int MinimumDioramaRootCountForReview => Mathf.Max(1, minimumDioramaRootCount);
        public int MinimumMarkerCountForReview => Mathf.Max(0, minimumMarkerCount);
        public int MinimumMapCountForReview => Mathf.Max(1, minimumMapCount);
        public int MinimumTreatmentTypeCountForReview => Mathf.Max(1, minimumTreatmentTypeCount);
        public float TargetVisibleEdgeCoverageMetersForReview => Mathf.Max(0f, targetVisibleEdgeCoverageMeters);
        public float TargetFoliageSkirtCoverageMetersForReview => Mathf.Max(0f, targetFoliageSkirtCoverageMeters);
        public float CliffLipDropMetersForReview => Mathf.Max(0f, cliffLipDropMeters);
        public float ValueDropBandMetersForReview => Mathf.Max(0f, valueDropBandMeters);
        public float EdgeAoBandWidthMetersForReview => Mathf.Max(0f, edgeAoBandWidthMeters);
        public float FoliageSkirtSpacingMetersForReview => Mathf.Max(0.25f, foliageSkirtSpacingMeters);
        public bool ConservativeTreatmentEnabledForReview => conservativeTreatmentEnabled;
        public bool StaticGiRequiredForReview => staticGiRequired;
        public bool GpuInstancingRequiredForReview => gpuInstancingRequired;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalDioramaEdgeTreatmentApprovedForReview => finalDioramaEdgeTreatmentApproved;
        public string SourceNoteForReview => sourceNote ?? string.Empty;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            int configuredMinimumDioramaRootCount,
            int configuredMinimumMarkerCount,
            int configuredMinimumMapCount,
            int configuredMinimumTreatmentTypeCount,
            float configuredTargetVisibleEdgeCoverageMeters,
            float configuredTargetFoliageSkirtCoverageMeters,
            float configuredCliffLipDropMeters,
            float configuredValueDropBandMeters,
            float configuredEdgeAoBandWidthMeters,
            float configuredFoliageSkirtSpacingMeters,
            bool configuredConservativeTreatmentEnabled,
            bool configuredStaticGiRequired,
            bool configuredGpuInstancingRequired,
            bool configuredNeedsTomApproval,
            bool configuredFinalDioramaEdgeTreatmentApproved,
            string configuredSourceNote,
            string configuredRecommendation)
        {
            minimumDioramaRootCount = Mathf.Clamp(configuredMinimumDioramaRootCount, 1, 20);
            minimumMarkerCount = Mathf.Clamp(configuredMinimumMarkerCount, 0, 2000);
            minimumMapCount = Mathf.Clamp(configuredMinimumMapCount, 1, 12);
            minimumTreatmentTypeCount = Mathf.Clamp(configuredMinimumTreatmentTypeCount, 1, 8);
            targetVisibleEdgeCoverageMeters = Mathf.Max(0f, configuredTargetVisibleEdgeCoverageMeters);
            targetFoliageSkirtCoverageMeters = Mathf.Max(0f, configuredTargetFoliageSkirtCoverageMeters);
            cliffLipDropMeters = Mathf.Max(0f, configuredCliffLipDropMeters);
            valueDropBandMeters = Mathf.Max(0f, configuredValueDropBandMeters);
            edgeAoBandWidthMeters = Mathf.Max(0f, configuredEdgeAoBandWidthMeters);
            foliageSkirtSpacingMeters = Mathf.Max(0.25f, configuredFoliageSkirtSpacingMeters);
            conservativeTreatmentEnabled = configuredConservativeTreatmentEnabled;
            staticGiRequired = configuredStaticGiRequired;
            gpuInstancingRequired = configuredGpuInstancingRequired;
            needsTomApproval = configuredNeedsTomApproval;
            finalDioramaEdgeTreatmentApproved = configuredFinalDioramaEdgeTreatmentApproved;
            sourceNote = configuredSourceNote ?? string.Empty;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
