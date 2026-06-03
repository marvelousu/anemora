using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dDioramaEdgeTreatmentType
    {
        CliffLip = 0,
        FoliageSkirt = 1,
        ValueDropOff = 2,
        EdgeAoBand = 3,
        RockBreakup = 4,
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Diorama Edge Treatment Marker")]
    public sealed class FastVsHd2dDioramaEdgeTreatmentMarker : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dDioramaEdgeTreatmentProfile profile;
        [SerializeField] private FastVsHd2dDioramaEdgeTreatmentType treatmentType;
        [SerializeField] private string mapToken = string.Empty;
        [SerializeField] private string edgeSide = string.Empty;
        [SerializeField] private bool currentWorld = true;
        [SerializeField, Min(0f)] private float coverageMeters;
        [SerializeField] private bool hidesFlatSlabEdge = true;
        [SerializeField] private bool alphaClipFoliageCard;
        [SerializeField] private bool staticBatchingReady;
        [SerializeField] private bool gpuInstancingReady;
        [SerializeField] private bool conservativeNeedsTomApproval = true;

        public FastVsHd2dDioramaEdgeTreatmentProfile ProfileForReview => profile;
        public FastVsHd2dDioramaEdgeTreatmentType TreatmentTypeForReview => treatmentType;
        public string MapTokenForReview => mapToken ?? string.Empty;
        public string EdgeSideForReview => edgeSide ?? string.Empty;
        public bool CurrentWorldForReview => currentWorld;
        public float CoverageMetersForReview => Mathf.Max(0f, coverageMeters);
        public bool HidesFlatSlabEdgeForReview => hidesFlatSlabEdge;
        public bool AlphaClipFoliageCardForReview => alphaClipFoliageCard;
        public bool StaticBatchingReadyForReview => staticBatchingReady;
        public bool GpuInstancingReadyForReview => gpuInstancingReady;
        public bool ConservativeNeedsTomApprovalForReview => conservativeNeedsTomApproval;

        public void ConfigureForReview(
            FastVsHd2dDioramaEdgeTreatmentProfile configuredProfile,
            FastVsHd2dDioramaEdgeTreatmentType configuredTreatmentType,
            string configuredMapToken,
            string configuredEdgeSide,
            bool configuredCurrentWorld,
            float configuredCoverageMeters,
            bool configuredHidesFlatSlabEdge,
            bool configuredAlphaClipFoliageCard,
            bool configuredStaticBatchingReady,
            bool configuredGpuInstancingReady,
            bool configuredConservativeNeedsTomApproval)
        {
            profile = configuredProfile;
            treatmentType = configuredTreatmentType;
            mapToken = configuredMapToken ?? string.Empty;
            edgeSide = configuredEdgeSide ?? string.Empty;
            currentWorld = configuredCurrentWorld;
            coverageMeters = Mathf.Max(0f, configuredCoverageMeters);
            hidesFlatSlabEdge = configuredHidesFlatSlabEdge;
            alphaClipFoliageCard = configuredAlphaClipFoliageCard;
            staticBatchingReady = configuredStaticBatchingReady;
            gpuInstancingReady = configuredGpuInstancingReady;
            conservativeNeedsTomApproval = configuredConservativeNeedsTomApproval;
        }
    }
}
