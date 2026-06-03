using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    public enum FastVsHd2dCardShadowLodSubject
    {
        FoliageCard = 0,
        CharacterCard = 1,
        FarFoliageLod0 = 2,
        FarFoliageMergedLod = 3,
        AtlasPrototype = 4,
        ReviewShadowReceiver = 5,
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Card Shadow LOD Budget Marker")]
    public sealed class FastVsHd2dCardShadowLodBudgetMarker : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dCardShadowLodSubject subject;
        [SerializeField] private string budgetGroup = "p2_77";
        [SerializeField] private Renderer trackedRenderer;
        [SerializeField] private LODGroup lodGroup;
        [SerializeField] private float distanceBandMeters;
        [SerializeField] private bool nearShadowCaster;
        [SerializeField] private bool farShadowCulled;
        [SerializeField] private bool participatesInAtlasPrototype;
        [SerializeField] private bool twoSidedCullOff;
        [SerializeField] private bool alphaClippedShadowCaster;
        [SerializeField] private bool tightCardMesh;
        [SerializeField] private bool characterShadowPolicyDeferred;
        [SerializeField] private bool receiveShadowsDisabled;
        [SerializeField] private int sourceMaterialBindingCount = 1;
        [SerializeField] private int optimizedMaterialBindingCount = 1;
        [SerializeField] private bool needsTomApproval = true;

        public FastVsHd2dCardShadowLodSubject SubjectForReview => subject;
        public string BudgetGroupForReview => budgetGroup ?? string.Empty;
        public Renderer TrackedRendererForReview => trackedRenderer;
        public LODGroup LodGroupForReview => lodGroup;
        public float DistanceBandMetersForReview => Mathf.Max(0f, distanceBandMeters);
        public bool NearShadowCasterForReview => nearShadowCaster;
        public bool FarShadowCulledForReview => farShadowCulled;
        public bool ParticipatesInAtlasPrototypeForReview => participatesInAtlasPrototype;
        public bool TwoSidedCullOffForReview => twoSidedCullOff;
        public bool AlphaClippedShadowCasterForReview => alphaClippedShadowCaster;
        public bool TightCardMeshForReview => tightCardMesh;
        public bool CharacterShadowPolicyDeferredForReview => characterShadowPolicyDeferred;
        public bool ReceiveShadowsDisabledForReview => receiveShadowsDisabled;
        public int SourceMaterialBindingCountForReview => Mathf.Max(1, sourceMaterialBindingCount);
        public int OptimizedMaterialBindingCountForReview => Mathf.Max(1, optimizedMaterialBindingCount);
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public ShadowCastingMode ShadowCastingModeForReview => trackedRenderer != null ? trackedRenderer.shadowCastingMode : ShadowCastingMode.Off;

        public void ConfigureForReview(
            FastVsHd2dCardShadowLodSubject configuredSubject,
            string configuredBudgetGroup,
            Renderer configuredRenderer,
            LODGroup configuredLodGroup,
            float configuredDistanceBandMeters,
            bool configuredNearShadowCaster,
            bool configuredFarShadowCulled,
            bool configuredParticipatesInAtlasPrototype,
            bool configuredTwoSidedCullOff,
            bool configuredAlphaClippedShadowCaster,
            bool configuredTightCardMesh,
            bool configuredCharacterShadowPolicyDeferred,
            bool configuredReceiveShadowsDisabled,
            int configuredSourceMaterialBindingCount,
            int configuredOptimizedMaterialBindingCount,
            bool configuredNeedsTomApproval)
        {
            subject = configuredSubject;
            budgetGroup = configuredBudgetGroup ?? string.Empty;
            trackedRenderer = configuredRenderer;
            lodGroup = configuredLodGroup;
            distanceBandMeters = Mathf.Max(0f, configuredDistanceBandMeters);
            nearShadowCaster = configuredNearShadowCaster;
            farShadowCulled = configuredFarShadowCulled;
            participatesInAtlasPrototype = configuredParticipatesInAtlasPrototype;
            twoSidedCullOff = configuredTwoSidedCullOff;
            alphaClippedShadowCaster = configuredAlphaClippedShadowCaster;
            tightCardMesh = configuredTightCardMesh;
            characterShadowPolicyDeferred = configuredCharacterShadowPolicyDeferred;
            receiveShadowsDisabled = configuredReceiveShadowsDisabled;
            sourceMaterialBindingCount = Mathf.Max(1, configuredSourceMaterialBindingCount);
            optimizedMaterialBindingCount = Mathf.Max(1, configuredOptimizedMaterialBindingCount);
            needsTomApproval = configuredNeedsTomApproval;
        }
    }
}
