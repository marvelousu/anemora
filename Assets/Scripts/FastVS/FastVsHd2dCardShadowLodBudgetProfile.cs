using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dCardShadowLodBudgetProfile", menuName = "Anemora/HD2D/Card Shadow LOD Budget Profile")]
    public sealed class FastVsHd2dCardShadowLodBudgetProfile : ScriptableObject
    {
        [SerializeField, Range(0f, 30f)] private float nearShadowDistanceMeters = 7.5f;
        [SerializeField, Range(0f, 30f)] private float shadowFadeStartMeters = 5.2f;
        [SerializeField, Range(0f, 50f)] private float farShadowCullDistanceMeters = 10.0f;
        [SerializeField, Range(0f, 80f)] private float lod0StartDistanceMeters = 0f;
        [SerializeField, Range(0f, 80f)] private float lod1MergeDistanceMeters = 13.5f;
        [SerializeField, Range(0f, 120f)] private float lodCullDistanceMeters = 32.0f;
        [SerializeField, Range(0.01f, 1f)] private float lod0ScreenRelativeHeight = 0.38f;
        [SerializeField, Range(0.01f, 1f)] private float lod1ScreenRelativeHeight = 0.12f;
        [SerializeField, Range(0f, 1f)] private float lodCullScreenRelativeHeight = 0.02f;
        [SerializeField, Range(0f, 1f)] private float alphaCutoff = 0.15f;
        [SerializeField, Range(1, 128)] private int minimumBudgetMarkerCount = 28;
        [SerializeField, Range(1, 64)] private int minimumNearShadowCasterCount = 6;
        [SerializeField, Range(1, 64)] private int minimumFarShadowOffCount = 8;
        [SerializeField, Range(1, 16)] private int minimumLodGroupCount = 1;
        [SerializeField, Range(1, 64)] private int minimumAtlasCandidateCount = 12;
        [SerializeField, Range(1, 16)] private int baselineFoliageMaterialCount = 4;
        [SerializeField, Range(1, 16)] private int atlasMaterialCount = 1;
        [SerializeField] private bool conservativeBudgetEnabled = true;
        [SerializeField] private bool twoSidedCullOffRequired = true;
        [SerializeField] private bool alphaClipShadowCasterRequired = true;
        [SerializeField] private bool characterFullShadowBudgetDeferred = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalCardShadowLodBudgetApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-77 card shadow/LOD/atlas budget data. Tom should tune final shadow distance, fade/cull thresholds, LOD screen heights, atlas layout, and whether character sprite shadow-map casters remain enabled.";

        public float NearShadowDistanceMetersForReview => Mathf.Clamp(nearShadowDistanceMeters, 0f, 30f);
        public float ShadowFadeStartMetersForReview => Mathf.Clamp(shadowFadeStartMeters, 0f, 30f);
        public float FarShadowCullDistanceMetersForReview => Mathf.Clamp(farShadowCullDistanceMeters, 0f, 50f);
        public float Lod0StartDistanceMetersForReview => Mathf.Clamp(lod0StartDistanceMeters, 0f, 80f);
        public float Lod1MergeDistanceMetersForReview => Mathf.Clamp(lod1MergeDistanceMeters, 0f, 80f);
        public float LodCullDistanceMetersForReview => Mathf.Clamp(lodCullDistanceMeters, 0f, 120f);
        public float Lod0ScreenRelativeHeightForReview => Mathf.Clamp(lod0ScreenRelativeHeight, 0.01f, 1f);
        public float Lod1ScreenRelativeHeightForReview => Mathf.Clamp(lod1ScreenRelativeHeight, 0.01f, 1f);
        public float LodCullScreenRelativeHeightForReview => Mathf.Clamp01(lodCullScreenRelativeHeight);
        public float AlphaCutoffForReview => Mathf.Clamp01(alphaCutoff);
        public int MinimumBudgetMarkerCountForReview => Mathf.Max(1, minimumBudgetMarkerCount);
        public int MinimumNearShadowCasterCountForReview => Mathf.Max(1, minimumNearShadowCasterCount);
        public int MinimumFarShadowOffCountForReview => Mathf.Max(1, minimumFarShadowOffCount);
        public int MinimumLodGroupCountForReview => Mathf.Max(1, minimumLodGroupCount);
        public int MinimumAtlasCandidateCountForReview => Mathf.Max(1, minimumAtlasCandidateCount);
        public int BaselineFoliageMaterialCountForReview => Mathf.Max(1, baselineFoliageMaterialCount);
        public int AtlasMaterialCountForReview => Mathf.Max(1, atlasMaterialCount);
        public bool ConservativeBudgetEnabledForReview => conservativeBudgetEnabled;
        public bool TwoSidedCullOffRequiredForReview => twoSidedCullOffRequired;
        public bool AlphaClipShadowCasterRequiredForReview => alphaClipShadowCasterRequired;
        public bool CharacterFullShadowBudgetDeferredForReview => characterFullShadowBudgetDeferred;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalCardShadowLodBudgetApprovedForReview => finalCardShadowLodBudgetApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            float configuredNearShadowDistanceMeters,
            float configuredShadowFadeStartMeters,
            float configuredFarShadowCullDistanceMeters,
            float configuredLod0StartDistanceMeters,
            float configuredLod1MergeDistanceMeters,
            float configuredLodCullDistanceMeters,
            float configuredLod0ScreenRelativeHeight,
            float configuredLod1ScreenRelativeHeight,
            float configuredLodCullScreenRelativeHeight,
            float configuredAlphaCutoff,
            int configuredMinimumBudgetMarkerCount,
            int configuredMinimumNearShadowCasterCount,
            int configuredMinimumFarShadowOffCount,
            int configuredMinimumLodGroupCount,
            int configuredMinimumAtlasCandidateCount,
            int configuredBaselineFoliageMaterialCount,
            int configuredAtlasMaterialCount,
            bool configuredConservativeBudgetEnabled,
            bool configuredTwoSidedCullOffRequired,
            bool configuredAlphaClipShadowCasterRequired,
            bool configuredCharacterFullShadowBudgetDeferred,
            bool configuredNeedsTomApproval,
            bool configuredFinalCardShadowLodBudgetApproved,
            string configuredRecommendation)
        {
            nearShadowDistanceMeters = Mathf.Clamp(configuredNearShadowDistanceMeters, 0f, 30f);
            shadowFadeStartMeters = Mathf.Clamp(configuredShadowFadeStartMeters, 0f, 30f);
            farShadowCullDistanceMeters = Mathf.Clamp(configuredFarShadowCullDistanceMeters, 0f, 50f);
            lod0StartDistanceMeters = Mathf.Clamp(configuredLod0StartDistanceMeters, 0f, 80f);
            lod1MergeDistanceMeters = Mathf.Clamp(configuredLod1MergeDistanceMeters, 0f, 80f);
            lodCullDistanceMeters = Mathf.Clamp(configuredLodCullDistanceMeters, 0f, 120f);
            lod0ScreenRelativeHeight = Mathf.Clamp(configuredLod0ScreenRelativeHeight, 0.01f, 1f);
            lod1ScreenRelativeHeight = Mathf.Clamp(configuredLod1ScreenRelativeHeight, 0.01f, 1f);
            lodCullScreenRelativeHeight = Mathf.Clamp01(configuredLodCullScreenRelativeHeight);
            alphaCutoff = Mathf.Clamp01(configuredAlphaCutoff);
            minimumBudgetMarkerCount = Mathf.Clamp(configuredMinimumBudgetMarkerCount, 1, 128);
            minimumNearShadowCasterCount = Mathf.Clamp(configuredMinimumNearShadowCasterCount, 1, 64);
            minimumFarShadowOffCount = Mathf.Clamp(configuredMinimumFarShadowOffCount, 1, 64);
            minimumLodGroupCount = Mathf.Clamp(configuredMinimumLodGroupCount, 1, 16);
            minimumAtlasCandidateCount = Mathf.Clamp(configuredMinimumAtlasCandidateCount, 1, 64);
            baselineFoliageMaterialCount = Mathf.Clamp(configuredBaselineFoliageMaterialCount, 1, 16);
            atlasMaterialCount = Mathf.Clamp(configuredAtlasMaterialCount, 1, 16);
            conservativeBudgetEnabled = configuredConservativeBudgetEnabled;
            twoSidedCullOffRequired = configuredTwoSidedCullOffRequired;
            alphaClipShadowCasterRequired = configuredAlphaClipShadowCasterRequired;
            characterFullShadowBudgetDeferred = configuredCharacterFullShadowBudgetDeferred;
            needsTomApproval = configuredNeedsTomApproval;
            finalCardShadowLodBudgetApproved = configuredFinalCardShadowLodBudgetApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
