using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dFoliageVarietyAccentType
    {
        Flower = 0,
        FallenLeaf = 1,
        Vine = 2,
        Moss = 3,
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Foliage Variety Marker")]
    public sealed class FastVsHd2dFoliageVarietyMarker : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dFoliageVarietyAccentType accentType;
        [SerializeField] private string placementGroup = "central_plaza";
        [SerializeField] private bool seamSoftener;
        [SerializeField] private bool conservativeNeedsTomApproval = true;

        public FastVsHd2dFoliageVarietyAccentType AccentTypeForReview => accentType;
        public string PlacementGroupForReview => placementGroup ?? string.Empty;
        public bool SeamSoftenerForReview => seamSoftener;
        public bool ConservativeNeedsTomApprovalForReview => conservativeNeedsTomApproval;

        public void ConfigureForReview(
            FastVsHd2dFoliageVarietyAccentType configuredAccentType,
            string configuredPlacementGroup,
            bool configuredSeamSoftener,
            bool configuredConservativeNeedsTomApproval)
        {
            accentType = configuredAccentType;
            placementGroup = configuredPlacementGroup ?? string.Empty;
            seamSoftener = configuredSeamSoftener;
            conservativeNeedsTomApproval = configuredConservativeNeedsTomApproval;
        }
    }
}
