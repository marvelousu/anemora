using UnityEngine;

namespace Anemora.FastVS
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Idle Emissive Marker")]
    public sealed class FastVsHd2dIdleEmissiveMarker : MonoBehaviour
    {
        [SerializeField] private string characterId = string.Empty;
        [SerializeField] private FastVsHd2dIdleEmissiveProfile profile;
        [SerializeField] private Renderer spriteRenderer;
        [SerializeField] private FastVsSpriteStripLoopAnimator stripAnimator;
        [SerializeField] private FastVsHd2dIdleSecondaryMotion secondaryMotion;
        [SerializeField] private FastVsHd2dIdleEmissiveAccent emissiveAccent;
        [SerializeField] private FastVsHd2dIdleEmissiveAccentKind accentKind;
        [SerializeField] private bool currentWorld;
        [SerializeField] private int areaIndex;
        [SerializeField] private bool primaryReviewAccent;

        public string CharacterIdForReview => characterId ?? string.Empty;
        public FastVsHd2dIdleEmissiveProfile ProfileForReview => profile;
        public Renderer SpriteRendererForReview => spriteRenderer;
        public FastVsSpriteStripLoopAnimator StripAnimatorForReview => stripAnimator;
        public FastVsHd2dIdleSecondaryMotion SecondaryMotionForReview => secondaryMotion;
        public FastVsHd2dIdleEmissiveAccent EmissiveAccentForReview => emissiveAccent;
        public FastVsHd2dIdleEmissiveAccentKind AccentKindForReview => accentKind;
        public bool CurrentWorldForReview => currentWorld;
        public int AreaIndexForReview => areaIndex;
        public bool PrimaryReviewAccentForReview => primaryReviewAccent;
        public bool HasFourFrameIdleLoopForReview => stripAnimator != null && stripAnimator.FrameCountForReview >= 4;
        public bool HasSecondaryMotionForReview => secondaryMotion != null && secondaryMotion.ProfileForReview == profile;
        public bool HasEmissiveAccentForReview => emissiveAccent != null && emissiveAccent.PointLightForReview != null && emissiveAccent.HaloRendererForReview != null;

        public void ConfigureForReview(
            string configuredCharacterId,
            FastVsHd2dIdleEmissiveProfile configuredProfile,
            Renderer configuredSpriteRenderer,
            FastVsSpriteStripLoopAnimator configuredStripAnimator,
            FastVsHd2dIdleSecondaryMotion configuredSecondaryMotion,
            FastVsHd2dIdleEmissiveAccent configuredEmissiveAccent,
            FastVsHd2dIdleEmissiveAccentKind configuredAccentKind,
            bool configuredCurrentWorld,
            int configuredAreaIndex,
            bool configuredPrimaryReviewAccent)
        {
            characterId = configuredCharacterId ?? string.Empty;
            profile = configuredProfile;
            spriteRenderer = configuredSpriteRenderer;
            stripAnimator = configuredStripAnimator;
            secondaryMotion = configuredSecondaryMotion;
            emissiveAccent = configuredEmissiveAccent;
            accentKind = configuredAccentKind;
            currentWorld = configuredCurrentWorld;
            areaIndex = configuredAreaIndex;
            primaryReviewAccent = configuredPrimaryReviewAccent;
        }
    }
}
