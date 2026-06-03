using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dFoliageVarietyProfile", menuName = "Anemora/HD2D/Foliage Variety Profile")]
    public sealed class FastVsHd2dFoliageVarietyProfile : ScriptableObject
    {
        [SerializeField, Range(0, 16)] private int flowerPatchCount = 5;
        [SerializeField, Range(0, 32)] private int fallenLeafCount = 14;
        [SerializeField, Range(0, 16)] private int vineStripCount = 5;
        [SerializeField, Range(0, 16)] private int mossSeamStripCount = 6;
        [SerializeField] private Color flowerWarmTint = new Color(1.00f, 0.42f, 0.32f, 1f);
        [SerializeField] private Color flowerCoolTint = new Color(0.42f, 0.58f, 1.00f, 1f);
        [SerializeField] private Color fallenLeafTint = new Color(0.92f, 0.54f, 0.20f, 1f);
        [SerializeField] private Color vineTint = new Color(0.32f, 0.62f, 0.30f, 1f);
        [SerializeField] private Color mossTint = new Color(0.24f, 0.44f, 0.22f, 1f);
        [SerializeField, Range(0f, 0.25f)] private float accentWindStrength = 0.045f;
        [SerializeField, Range(0f, 1f)] private float seamSofteningStrength = 0.62f;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalFoliageVarietyApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-59 foliage variety data prep. Tom should tune final accent density, flower color, vine placement, moss seam strength, and whether these accents belong in each biome.";

        public int FlowerPatchCountForReview => Mathf.Max(0, flowerPatchCount);
        public int FallenLeafCountForReview => Mathf.Max(0, fallenLeafCount);
        public int VineStripCountForReview => Mathf.Max(0, vineStripCount);
        public int MossSeamStripCountForReview => Mathf.Max(0, mossSeamStripCount);
        public Color FlowerWarmTintForReview => ResolveVisibleTint(flowerWarmTint);
        public Color FlowerCoolTintForReview => ResolveVisibleTint(flowerCoolTint);
        public Color FallenLeafTintForReview => ResolveVisibleTint(fallenLeafTint);
        public Color VineTintForReview => ResolveVisibleTint(vineTint);
        public Color MossTintForReview => ResolveVisibleTint(mossTint);
        public float AccentWindStrengthForReview => Mathf.Clamp(accentWindStrength, 0f, 0.25f);
        public float SeamSofteningStrengthForReview => Mathf.Clamp01(seamSofteningStrength);
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalFoliageVarietyApprovedForReview => finalFoliageVarietyApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            int configuredFlowerPatchCount,
            int configuredFallenLeafCount,
            int configuredVineStripCount,
            int configuredMossSeamStripCount,
            Color configuredFlowerWarmTint,
            Color configuredFlowerCoolTint,
            Color configuredFallenLeafTint,
            Color configuredVineTint,
            Color configuredMossTint,
            float configuredAccentWindStrength,
            float configuredSeamSofteningStrength,
            bool configuredNeedsTomApproval,
            bool configuredFinalFoliageVarietyApproved,
            string configuredRecommendation)
        {
            flowerPatchCount = Mathf.Clamp(configuredFlowerPatchCount, 0, 16);
            fallenLeafCount = Mathf.Clamp(configuredFallenLeafCount, 0, 32);
            vineStripCount = Mathf.Clamp(configuredVineStripCount, 0, 16);
            mossSeamStripCount = Mathf.Clamp(configuredMossSeamStripCount, 0, 16);
            flowerWarmTint = ResolveVisibleTint(configuredFlowerWarmTint);
            flowerCoolTint = ResolveVisibleTint(configuredFlowerCoolTint);
            fallenLeafTint = ResolveVisibleTint(configuredFallenLeafTint);
            vineTint = ResolveVisibleTint(configuredVineTint);
            mossTint = ResolveVisibleTint(configuredMossTint);
            accentWindStrength = Mathf.Clamp(configuredAccentWindStrength, 0f, 0.25f);
            seamSofteningStrength = Mathf.Clamp01(configuredSeamSofteningStrength);
            needsTomApproval = configuredNeedsTomApproval;
            finalFoliageVarietyApproved = configuredFinalFoliageVarietyApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }

        private static Color ResolveVisibleTint(Color color)
        {
            return new Color(
                Mathf.Clamp(color.r, 0.02f, 1.25f),
                Mathf.Clamp(color.g, 0.02f, 1.25f),
                Mathf.Clamp(color.b, 0.02f, 1.25f),
                Mathf.Clamp(color.a, 0.20f, 1f));
        }
    }
}
