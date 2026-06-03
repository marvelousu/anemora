using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Character Sprite Scale Profile")]
    public sealed class FastVsHd2dCharacterSpriteScaleProfile : ScriptableObject
    {
        [SerializeField, Min(0.01f)] private float worldUnitMeters = 1f;
        [SerializeField, Min(1)] private int framePixelWidth = 64;
        [SerializeField, Min(1)] private int framePixelHeight = 96;
        [SerializeField, Min(1)] private int loopFrameCount = 4;
        [SerializeField, Min(0.01f)] private float standardAdultCardWorldHeight = 1.18f;
        [SerializeField, Min(0.01f)] private float smallAdultMinimumWorldHeight = 0.96f;
        [SerializeField, Min(0.01f)] private float smallAdultMaximumWorldHeight = 1.12f;
        [SerializeField, Min(0.01f)] private float childCardWorldHeight = 0.78f;
        [SerializeField, Min(0f)] private float transparentFootPixels = 2f;
        [SerializeField, Min(0f)] private float heightTolerance = 0.035f;
        [SerializeField, Min(0f)] private float widthTolerance = 0.030f;
        [SerializeField, Min(0f)] private float visualFootPivotTolerance = 0.018f;
        [SerializeField, Min(0f)] private float texelsPerWorldUnitTolerance = 0.75f;
        [SerializeField, Min(0.01f)] private float doorwayReferenceWorldHeight = 1.80f;
        [SerializeField, Min(3)] private int minimumReviewLineupCount = 4;
        [SerializeField, Min(1f)] private float fixedReviewCameraFieldOfView = 30f;
        [SerializeField] private bool pointFilteringRequired = true;
        [SerializeField] private bool mipMapsDisabledRequired = true;
        [SerializeField] private string reviewContract =
            "World unit is one meter. Character cards use bottom-center visual-foot pivot after the transparent 2px foot pad, 64x96 authored frames, Point filtering, no mips, and fixed review camera distance so texel density remains comparable.";

        public float WorldUnitMetersForReview => worldUnitMeters;
        public int FramePixelWidthForReview => framePixelWidth;
        public int FramePixelHeightForReview => framePixelHeight;
        public int LoopFrameCountForReview => loopFrameCount;
        public float StandardAdultCardWorldHeightForReview => standardAdultCardWorldHeight;
        public float StandardAdultCardWorldWidthForReview => standardAdultCardWorldHeight * FrameAspectForReview;
        public float SmallAdultMinimumWorldHeightForReview => smallAdultMinimumWorldHeight;
        public float SmallAdultMaximumWorldHeightForReview => smallAdultMaximumWorldHeight;
        public float ChildCardWorldHeightForReview => childCardWorldHeight;
        public float TransparentFootPixelsForReview => transparentFootPixels;
        public float HeightToleranceForReview => heightTolerance;
        public float WidthToleranceForReview => widthTolerance;
        public float VisualFootPivotToleranceForReview => visualFootPivotTolerance;
        public float TexelsPerWorldUnitToleranceForReview => texelsPerWorldUnitTolerance;
        public float DoorwayReferenceWorldHeightForReview => doorwayReferenceWorldHeight;
        public int MinimumReviewLineupCountForReview => minimumReviewLineupCount;
        public float FixedReviewCameraFieldOfViewForReview => fixedReviewCameraFieldOfView;
        public bool PointFilteringRequiredForReview => pointFilteringRequired;
        public bool MipMapsDisabledRequiredForReview => mipMapsDisabledRequired;
        public string ReviewContractForReview => reviewContract ?? string.Empty;
        public float FrameAspectForReview => framePixelHeight <= 0 ? 1f : framePixelWidth / (float)framePixelHeight;
        public float StandardTexelsPerWorldUnitForReview => standardAdultCardWorldHeight <= 0.001f ? 0f : framePixelHeight / standardAdultCardWorldHeight;

        public void ConfigureForReview(
            float configuredWorldUnitMeters,
            int configuredFramePixelWidth,
            int configuredFramePixelHeight,
            int configuredLoopFrameCount,
            float configuredStandardAdultCardWorldHeight,
            float configuredSmallAdultMinimumWorldHeight,
            float configuredSmallAdultMaximumWorldHeight,
            float configuredChildCardWorldHeight,
            float configuredTransparentFootPixels,
            float configuredHeightTolerance,
            float configuredWidthTolerance,
            float configuredVisualFootPivotTolerance,
            float configuredTexelsPerWorldUnitTolerance,
            float configuredDoorwayReferenceWorldHeight,
            int configuredMinimumReviewLineupCount,
            float configuredFixedReviewCameraFieldOfView,
            bool configuredPointFilteringRequired,
            bool configuredMipMapsDisabledRequired,
            string configuredReviewContract)
        {
            worldUnitMeters = Mathf.Max(0.01f, configuredWorldUnitMeters);
            framePixelWidth = Mathf.Max(1, configuredFramePixelWidth);
            framePixelHeight = Mathf.Max(1, configuredFramePixelHeight);
            loopFrameCount = Mathf.Max(1, configuredLoopFrameCount);
            standardAdultCardWorldHeight = Mathf.Max(0.01f, configuredStandardAdultCardWorldHeight);
            smallAdultMinimumWorldHeight = Mathf.Max(0.01f, configuredSmallAdultMinimumWorldHeight);
            smallAdultMaximumWorldHeight = Mathf.Max(smallAdultMinimumWorldHeight, configuredSmallAdultMaximumWorldHeight);
            childCardWorldHeight = Mathf.Max(0.01f, configuredChildCardWorldHeight);
            transparentFootPixels = Mathf.Max(0f, configuredTransparentFootPixels);
            heightTolerance = Mathf.Max(0f, configuredHeightTolerance);
            widthTolerance = Mathf.Max(0f, configuredWidthTolerance);
            visualFootPivotTolerance = Mathf.Max(0f, configuredVisualFootPivotTolerance);
            texelsPerWorldUnitTolerance = Mathf.Max(0f, configuredTexelsPerWorldUnitTolerance);
            doorwayReferenceWorldHeight = Mathf.Max(0.01f, configuredDoorwayReferenceWorldHeight);
            minimumReviewLineupCount = Mathf.Max(3, configuredMinimumReviewLineupCount);
            fixedReviewCameraFieldOfView = Mathf.Max(1f, configuredFixedReviewCameraFieldOfView);
            pointFilteringRequired = configuredPointFilteringRequired;
            mipMapsDisabledRequired = configuredMipMapsDisabledRequired;
            reviewContract = string.IsNullOrWhiteSpace(configuredReviewContract) ? reviewContract : configuredReviewContract;
        }
    }
}
