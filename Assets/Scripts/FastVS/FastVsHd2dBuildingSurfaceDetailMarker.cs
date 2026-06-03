using UnityEngine;

namespace Anemora.FastVS
{
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dBuildingSurfaceDetailMarker : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dBuildingSurfaceDetailProfile profile;
        [SerializeField] private string buildingId = string.Empty;
        [SerializeField] private string detailKind = string.Empty;
        [SerializeField] private string materialId = string.Empty;
        [SerializeField] private bool geometryAccent;
        [SerializeField] private bool generatedNormalReady;

        public FastVsHd2dBuildingSurfaceDetailProfile ProfileForReview => profile;
        public string BuildingIdForReview => buildingId;
        public string DetailKindForReview => detailKind;
        public string MaterialIdForReview => materialId;
        public bool GeometryAccentForReview => geometryAccent;
        public bool GeneratedNormalReadyForReview => generatedNormalReady;
        public bool IsReadyForReview =>
            profile != null &&
            !string.IsNullOrWhiteSpace(buildingId) &&
            !string.IsNullOrWhiteSpace(detailKind) &&
            !string.IsNullOrWhiteSpace(materialId) &&
            generatedNormalReady;

        public void ConfigureForReview(
            FastVsHd2dBuildingSurfaceDetailProfile configuredProfile,
            string configuredBuildingId,
            string configuredDetailKind,
            string configuredMaterialId,
            bool configuredGeometryAccent,
            bool configuredGeneratedNormalReady)
        {
            profile = configuredProfile;
            buildingId = configuredBuildingId ?? string.Empty;
            detailKind = configuredDetailKind ?? string.Empty;
            materialId = configuredMaterialId ?? string.Empty;
            geometryAccent = configuredGeometryAccent;
            generatedNormalReady = configuredGeneratedNormalReady;
        }
    }
}
