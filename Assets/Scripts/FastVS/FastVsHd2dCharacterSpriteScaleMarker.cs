using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dCharacterSpriteScaleClass
    {
        StandardAdult = 0,
        SmallAdult = 1,
        Child = 2,
        SeatedOrDesk = 3,
        ReviewStandard = 4,
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Character Sprite Scale Marker")]
    public sealed class FastVsHd2dCharacterSpriteScaleMarker : MonoBehaviour
    {
        [SerializeField] private string characterId = string.Empty;
        [SerializeField] private FastVsHd2dCharacterSpriteScaleClass scaleClass;
        [SerializeField] private int expectedFramePixelWidth = 64;
        [SerializeField] private int expectedFramePixelHeight = 96;
        [SerializeField] private int expectedFrameCount = 1;
        [SerializeField] private float expectedWorldHeight = 1.18f;
        [SerializeField] private float expectedWorldWidth = 0.787f;
        [SerializeField] private float transparentFootPixels = 2f;
        [SerializeField] private bool bottomCenterVisualFootPivot = true;
        [SerializeField] private bool reviewLineupCard;

        public string CharacterIdForReview => characterId ?? string.Empty;
        public FastVsHd2dCharacterSpriteScaleClass ScaleClassForReview => scaleClass;
        public int ExpectedFramePixelWidthForReview => expectedFramePixelWidth;
        public int ExpectedFramePixelHeightForReview => expectedFramePixelHeight;
        public int ExpectedFrameCountForReview => expectedFrameCount;
        public float ExpectedWorldHeightForReview => expectedWorldHeight;
        public float ExpectedWorldWidthForReview => expectedWorldWidth;
        public float TransparentFootPixelsForReview => transparentFootPixels;
        public bool BottomCenterVisualFootPivotForReview => bottomCenterVisualFootPivot;
        public bool ReviewLineupCardForReview => reviewLineupCard;

        public Renderer RendererForReview => GetComponent<Renderer>();

        public Texture2D TextureForReview
        {
            get
            {
                var renderer = RendererForReview;
                return renderer == null || renderer.sharedMaterial == null
                    ? null
                    : renderer.sharedMaterial.mainTexture as Texture2D;
            }
        }

        public float AuthoredWorldHeightForReview => Mathf.Abs(transform.localScale.y);
        public float AuthoredWorldWidthForReview => Mathf.Abs(transform.localScale.x);

        public float TransparentFootWorldPaddingForReview
        {
            get
            {
                return expectedFramePixelHeight <= 0
                    ? 0f
                    : AuthoredWorldHeightForReview * transparentFootPixels / expectedFramePixelHeight;
            }
        }

        public float VisualFootLocalYForReview
        {
            get
            {
                return transform.localPosition.y - (AuthoredWorldHeightForReview * 0.5f) + TransparentFootWorldPaddingForReview;
            }
        }

        public float CenterLocalXForReview => transform.localPosition.x;

        public float ActualTexelsPerWorldUnitForReview
        {
            get
            {
                return AuthoredWorldHeightForReview <= 0.001f
                    ? 0f
                    : expectedFramePixelHeight / AuthoredWorldHeightForReview;
            }
        }

        public int TextureFrameCountForReview
        {
            get
            {
                var texture = TextureForReview;
                return texture == null || expectedFramePixelWidth <= 0 ? 0 : Mathf.Max(1, texture.width / expectedFramePixelWidth);
            }
        }

        public bool TextureUsesPointFilteringForReview
        {
            get
            {
                var texture = TextureForReview;
                return texture != null && texture.filterMode == FilterMode.Point;
            }
        }

        public void ConfigureForReview(
            string configuredCharacterId,
            FastVsHd2dCharacterSpriteScaleClass configuredScaleClass,
            int configuredFramePixelWidth,
            int configuredFramePixelHeight,
            int configuredFrameCount,
            float configuredWorldHeight,
            float configuredTransparentFootPixels,
            bool configuredBottomCenterVisualFootPivot,
            bool configuredReviewLineupCard)
        {
            characterId = configuredCharacterId ?? string.Empty;
            scaleClass = configuredScaleClass;
            expectedFramePixelWidth = Mathf.Max(1, configuredFramePixelWidth);
            expectedFramePixelHeight = Mathf.Max(1, configuredFramePixelHeight);
            expectedFrameCount = Mathf.Max(1, configuredFrameCount);
            expectedWorldHeight = Mathf.Max(0.01f, configuredWorldHeight);
            expectedWorldWidth = expectedWorldHeight * expectedFramePixelWidth / expectedFramePixelHeight;
            transparentFootPixels = Mathf.Max(0f, configuredTransparentFootPixels);
            bottomCenterVisualFootPivot = configuredBottomCenterVisualFootPivot;
            reviewLineupCard = configuredReviewLineupCard;
        }
    }
}
