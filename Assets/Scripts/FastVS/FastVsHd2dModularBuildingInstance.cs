using System;
using UnityEngine;

namespace Anemora.FastVS
{
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dModularBuildingInstance : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dModularBuildingKitProfile profile;
        [SerializeField] private string buildingId = string.Empty;
        [SerializeField] private string facadeSignature = string.Empty;
        [SerializeField] private Vector2Int footprint;
        [SerializeField] private int floors;
        [SerializeField] private int moduleCount;
        [SerializeField] private int rendererCount;
        [SerializeField] private int variantIndex;
        [SerializeField] private bool snappedToGrid;
        [SerializeField] private bool prefabBacked;
        [SerializeField] private bool staticBatchingMarked;
        [SerializeField] private Renderer[] renderers = Array.Empty<Renderer>();

        public FastVsHd2dModularBuildingKitProfile ProfileForReview => profile;
        public string BuildingIdForReview => buildingId;
        public string FacadeSignatureForReview => facadeSignature;
        public Vector2Int FootprintForReview => footprint;
        public int FloorsForReview => floors;
        public int ModuleCountForReview => moduleCount;
        public int RendererCountForReview => rendererCount;
        public int VariantIndexForReview => variantIndex;
        public bool SnappedToGridForReview => snappedToGrid;
        public bool PrefabBackedForReview => prefabBacked;
        public bool StaticBatchingMarkedForReview => staticBatchingMarked;
        public bool IsReadyForReview => profile != null && !string.IsNullOrEmpty(buildingId) && moduleCount > 0 && rendererCount > 0 && snappedToGrid && prefabBacked;

        public Renderer GetRendererForReview(int index)
        {
            if (renderers == null || index < 0 || index >= renderers.Length)
            {
                return null;
            }

            return renderers[index];
        }

        public void ConfigureForReview(
            FastVsHd2dModularBuildingKitProfile configuredProfile,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            int configuredModuleCount,
            Renderer[] configuredRenderers,
            int configuredVariantIndex,
            bool configuredSnappedToGrid,
            bool configuredPrefabBacked,
            bool configuredStaticBatchingMarked)
        {
            profile = configuredProfile;
            buildingId = recipe != null ? recipe.BuildingIdForReview : string.Empty;
            facadeSignature = recipe != null ? recipe.SignatureForReview : string.Empty;
            footprint = recipe != null ? recipe.FootprintForReview : Vector2Int.zero;
            floors = recipe != null ? recipe.FloorsForReview : 0;
            moduleCount = Mathf.Max(0, configuredModuleCount);
            renderers = configuredRenderers ?? Array.Empty<Renderer>();
            rendererCount = renderers.Length;
            variantIndex = configuredVariantIndex;
            snappedToGrid = configuredSnappedToGrid;
            prefabBacked = configuredPrefabBacked;
            staticBatchingMarked = configuredStaticBatchingMarked;
        }
    }
}
