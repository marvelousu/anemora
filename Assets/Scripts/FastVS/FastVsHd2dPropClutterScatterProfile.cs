using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dPropClutterScatterProfile", menuName = "Anemora/HD2D/Prop Clutter Scatter Profile")]
    public sealed class FastVsHd2dPropClutterScatterProfile : ScriptableObject
    {
        [SerializeField, Range(0, 160)] private int minimumMarkerCount = 72;
        [SerializeField, Range(0, 12)] private int minimumClutterTypeCount = 8;
        [SerializeField, Range(0, 16)] private int minimumBareCornerFillCount = 6;
        [SerializeField, Min(0f)] private float reviewWallBaseSeamMeters = 13.20f;
        [SerializeField, Range(0f, 1f)] private float targetWallBaseSeamCoverage = 0.68f;
        [SerializeField, Min(0f)] private float focalBandNearMeters = 0.0f;
        [SerializeField, Min(0f)] private float focalBandFarMeters = 34.0f;
        [SerializeField, Min(0f)] private float farScatterCullDistanceMeters = 36.0f;
        [SerializeField] private Vector2 scaleJitterRange = new Vector2(0.88f, 1.12f);
        [SerializeField] private Vector2 yawJitterDegrees = new Vector2(-18f, 18f);
        [SerializeField] private bool conservativeScatterEnabled = true;
        [SerializeField] private bool staticGiRequired = true;
        [SerializeField] private bool gpuInstancingRequired = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalPropClutterApproved;
        [SerializeField, TextArea(2, 5)] private string sourceKitNote =
            "Procedural CC0-safe P2-74 review stand-ins; replace with approved Quaternius/Kenney/itch CC0 prop meshes and hand-placed brush scatter after Tom density approval.";
        [SerializeField, TextArea(2, 5)] private string recommendation =
            "Keep the conservative wall-base and corner scatter as data prep only. Tom should tune final density, silhouettes, walkable-path exclusions, approved prop kit meshes, and per-area clutter language.";

        public int MinimumMarkerCountForReview => Mathf.Max(0, minimumMarkerCount);
        public int MinimumClutterTypeCountForReview => Mathf.Max(0, minimumClutterTypeCount);
        public int MinimumBareCornerFillCountForReview => Mathf.Max(0, minimumBareCornerFillCount);
        public float ReviewWallBaseSeamMetersForReview => Mathf.Max(0.01f, reviewWallBaseSeamMeters);
        public float TargetWallBaseSeamCoverageForReview => Mathf.Clamp01(targetWallBaseSeamCoverage);
        public float FocalBandNearMetersForReview => Mathf.Max(0f, focalBandNearMeters);
        public float FocalBandFarMetersForReview => Mathf.Max(FocalBandNearMetersForReview + 1f, focalBandFarMeters);
        public float FarScatterCullDistanceMetersForReview => Mathf.Max(0f, farScatterCullDistanceMeters);
        public Vector2 ScaleJitterRangeForReview => new Vector2(Mathf.Clamp(scaleJitterRange.x, 0.25f, 2f), Mathf.Clamp(scaleJitterRange.y, 0.25f, 2f));
        public Vector2 YawJitterDegreesForReview => yawJitterDegrees;
        public bool ConservativeScatterEnabledForReview => conservativeScatterEnabled;
        public bool StaticGiRequiredForReview => staticGiRequired;
        public bool GpuInstancingRequiredForReview => gpuInstancingRequired;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalPropClutterApprovedForReview => finalPropClutterApproved;
        public string SourceKitNoteForReview => sourceKitNote ?? string.Empty;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            int configuredMinimumMarkerCount,
            int configuredMinimumClutterTypeCount,
            int configuredMinimumBareCornerFillCount,
            float configuredReviewWallBaseSeamMeters,
            float configuredTargetWallBaseSeamCoverage,
            float configuredFocalBandNearMeters,
            float configuredFocalBandFarMeters,
            float configuredFarScatterCullDistanceMeters,
            Vector2 configuredScaleJitterRange,
            Vector2 configuredYawJitterDegrees,
            bool configuredConservativeScatterEnabled,
            bool configuredStaticGiRequired,
            bool configuredGpuInstancingRequired,
            bool configuredNeedsTomApproval,
            bool configuredFinalPropClutterApproved,
            string configuredSourceKitNote,
            string configuredRecommendation)
        {
            minimumMarkerCount = Mathf.Clamp(configuredMinimumMarkerCount, 0, 160);
            minimumClutterTypeCount = Mathf.Clamp(configuredMinimumClutterTypeCount, 0, 12);
            minimumBareCornerFillCount = Mathf.Clamp(configuredMinimumBareCornerFillCount, 0, 16);
            reviewWallBaseSeamMeters = Mathf.Max(0.01f, configuredReviewWallBaseSeamMeters);
            targetWallBaseSeamCoverage = Mathf.Clamp01(configuredTargetWallBaseSeamCoverage);
            focalBandNearMeters = Mathf.Max(0f, configuredFocalBandNearMeters);
            focalBandFarMeters = Mathf.Max(focalBandNearMeters + 1f, configuredFocalBandFarMeters);
            farScatterCullDistanceMeters = Mathf.Max(0f, configuredFarScatterCullDistanceMeters);
            scaleJitterRange = new Vector2(Mathf.Clamp(configuredScaleJitterRange.x, 0.25f, 2f), Mathf.Clamp(configuredScaleJitterRange.y, 0.25f, 2f));
            yawJitterDegrees = configuredYawJitterDegrees;
            conservativeScatterEnabled = configuredConservativeScatterEnabled;
            staticGiRequired = configuredStaticGiRequired;
            gpuInstancingRequired = configuredGpuInstancingRequired;
            needsTomApproval = configuredNeedsTomApproval;
            finalPropClutterApproved = configuredFinalPropClutterApproved;
            sourceKitNote = configuredSourceKitNote ?? string.Empty;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
