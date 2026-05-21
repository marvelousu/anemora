using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dSurfaceKind
    {
        Floor,
        Wall,
        Ground,
        Road,
        Furniture,
        Bookshelf,
        Door,
        Window,
        Roof,
        Prop,
        Backdrop,
        Unknown
    }

    /// <summary>
    /// Review-only HD-2D surface metadata marker.
    /// This is for audit/validation only.
    /// </summary>
    public sealed class FastVsHd2dSurfaceProfile : MonoBehaviour
    {
        [SerializeField] private string surfaceId;
        [SerializeField] private FastVsHouseArea areaId;
        [SerializeField] private FastVsHd2dSurfaceKind surfaceKind;
        [SerializeField] private bool currentWorld;
        [SerializeField] private Vector2 targetLuminanceBand;
        [SerializeField] private Vector2 targetContrastBand;
        [SerializeField] private Vector2 textureDensityHint;
        [SerializeField] private bool acceptsOverlayShading;
        [SerializeField] private string intendedMaterialToken;

        public string SurfaceIdForReview => surfaceId;
        public FastVsHouseArea AreaIdForReview => areaId;
        public FastVsHd2dSurfaceKind SurfaceKindForReview => surfaceKind;
        public bool IsCurrentWorldForReview => currentWorld;
        public Vector2 TargetLuminanceBandForReview => targetLuminanceBand;
        public Vector2 TargetContrastBandForReview => targetContrastBand;
        public Vector2 TextureDensityHintForReview => textureDensityHint;
        public bool AcceptsOverlayShadingForReview => acceptsOverlayShading;
        public string IntendedMaterialTokenForReview => intendedMaterialToken;
    }
}
