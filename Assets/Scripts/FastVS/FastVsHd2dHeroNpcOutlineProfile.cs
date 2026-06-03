using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Hero NPC Outline Profile")]
    public sealed class FastVsHd2dHeroNpcOutlineProfile : ScriptableObject
    {
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalHeroNpcOutlineApproved;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool spriteSpaceOutlinePreferred = true;
        [SerializeField] private bool urpRenderGraphEdgeDetectDeferred = true;
        [SerializeField] private bool activeOnAwake;
        [SerializeField] private bool perCharacterOptInOnly = true;
        [SerializeField] private bool washedSceneFallbackPrepared = true;
        [SerializeField, Range(0.25f, 2.0f)] private float conservativeOutlineWidthTexels = 0.55f;
        [SerializeField, Range(0.75f, 3.0f)] private float strongerOutlineWidthTexels = 1.15f;
        [SerializeField, Range(0f, 1f)] private float outlineAlpha = 0.64f;
        [SerializeField, Range(0.01f, 0.55f)] private float alphaCutoff = 0.16f;
        [SerializeField, Range(0.15f, 1f)] private float washedSpriteAlpha = 0.68f;
        [SerializeField] private Color outlineColor = new Color(0.08f, 0.10f, 0.14f, 1f);
        [SerializeField] private Color washedBackdropTint = new Color(0.76f, 0.80f, 0.82f, 0.72f);
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep this as conservative sprite-space outline data only. Tom should approve final opt-in characters, outline color, 1px/2px width, and any scene-wide Render Graph edge-detect fallback.";

        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalHeroNpcOutlineApprovedForReview => finalHeroNpcOutlineApproved;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool SpriteSpaceOutlinePreferredForReview => spriteSpaceOutlinePreferred;
        public bool UrpRenderGraphEdgeDetectDeferredForReview => urpRenderGraphEdgeDetectDeferred;
        public bool ActiveOnAwakeForReview => activeOnAwake;
        public bool PerCharacterOptInOnlyForReview => perCharacterOptInOnly;
        public bool WashedSceneFallbackPreparedForReview => washedSceneFallbackPrepared;
        public float ConservativeOutlineWidthTexelsForReview => Mathf.Clamp(conservativeOutlineWidthTexels, 0.25f, 2.0f);
        public float StrongerOutlineWidthTexelsForReview => Mathf.Clamp(strongerOutlineWidthTexels, 0.75f, 3.0f);
        public float OutlineAlphaForReview => Mathf.Clamp01(outlineAlpha);
        public float AlphaCutoffForReview => Mathf.Clamp(alphaCutoff, 0.01f, 0.55f);
        public float WashedSpriteAlphaForReview => Mathf.Clamp(washedSpriteAlpha, 0.15f, 1f);
        public Color OutlineColorForReview
        {
            get
            {
                var color = outlineColor;
                color.a = 1f;
                return color;
            }
        }

        public Color WashedBackdropTintForReview => washedBackdropTint;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            bool configuredNeedsTomApproval,
            bool configuredFinalApproved,
            bool configuredConservativeDataPrep,
            bool configuredSpriteSpaceOutlinePreferred,
            bool configuredRenderGraphEdgeDetectDeferred,
            bool configuredActiveOnAwake,
            bool configuredPerCharacterOptInOnly,
            bool configuredWashedSceneFallbackPrepared,
            float configuredConservativeOutlineWidthTexels,
            float configuredStrongerOutlineWidthTexels,
            float configuredOutlineAlpha,
            float configuredAlphaCutoff,
            float configuredWashedSpriteAlpha,
            Color configuredOutlineColor,
            Color configuredWashedBackdropTint,
            string configuredRecommendation)
        {
            needsTomApproval = configuredNeedsTomApproval;
            finalHeroNpcOutlineApproved = configuredFinalApproved;
            conservativeDataPrep = configuredConservativeDataPrep;
            spriteSpaceOutlinePreferred = configuredSpriteSpaceOutlinePreferred;
            urpRenderGraphEdgeDetectDeferred = configuredRenderGraphEdgeDetectDeferred;
            activeOnAwake = configuredActiveOnAwake;
            perCharacterOptInOnly = configuredPerCharacterOptInOnly;
            washedSceneFallbackPrepared = configuredWashedSceneFallbackPrepared;
            conservativeOutlineWidthTexels = Mathf.Clamp(configuredConservativeOutlineWidthTexels, 0.25f, 2.0f);
            strongerOutlineWidthTexels = Mathf.Clamp(configuredStrongerOutlineWidthTexels, 0.75f, 3.0f);
            outlineAlpha = Mathf.Clamp01(configuredOutlineAlpha);
            alphaCutoff = Mathf.Clamp(configuredAlphaCutoff, 0.01f, 0.55f);
            washedSpriteAlpha = Mathf.Clamp(configuredWashedSpriteAlpha, 0.15f, 1f);
            outlineColor = configuredOutlineColor;
            outlineColor.a = 1f;
            washedBackdropTint = configuredWashedBackdropTint;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
