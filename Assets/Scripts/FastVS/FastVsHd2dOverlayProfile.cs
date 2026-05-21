using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dOverlayKind
    {
        CharacterContactShadow,
        CharacterFootContact,
        CharacterDirectionalCastShadow,
        StaticDirectionalCastShadow,
        SurfaceDirectionalShade,
        LightPool,
        Atmosphere,
        DepthFrame
    }

    /// <summary>
    /// Review-only HD-2D overlay metadata marker.
    /// This component exists for authoring, audit, and validation only.
    /// </summary>
    public sealed class FastVsHd2dOverlayProfile : MonoBehaviour
    {
        [SerializeField] private string overlayId;
        [SerializeField] private FastVsHouseArea areaId;
        [SerializeField] private FastVsHd2dOverlayKind overlayKind;
        [SerializeField] private bool currentWorld;
        [SerializeField] private bool dynamicSubject;
        [SerializeField] private Vector2 opacityBand;
        [SerializeField] private Vector2 footprintWorldSize;
        [SerializeField] private Color intendedTint = Color.black;

        public string OverlayIdForReview => overlayId;
        public FastVsHouseArea AreaIdForReview => areaId;
        public FastVsHd2dOverlayKind OverlayKindForReview => overlayKind;
        public bool IsCurrentWorldForReview => currentWorld;
        public bool IsDynamicSubjectForReview => dynamicSubject;
        public Vector2 OpacityBandForReview => opacityBand;
        public Vector2 FootprintWorldSizeForReview => footprintWorldSize;
        public Color IntendedTintForReview => intendedTint;
    }
}
