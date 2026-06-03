using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Town Dressing Profile")]
    public sealed class FastVsHd2dTownDressingProfile : ScriptableObject
    {
        [SerializeField, Min(1)] private int distinctDressingTypeCount = 8;
        [SerializeField, Min(0)] private int markerCount = 32;
        [SerializeField, Min(0)] private int lanternCount = 5;
        [SerializeField, Min(0)] private int spriteCardCount = 6;
        [SerializeField, Min(0f)] private float lanternEmissionIntensity = 4.6f;
        [SerializeField, Min(0f)] private float lanternLightRange = 3.2f;
        [SerializeField, Min(0f)] private float lanternLightIntensity = 1.55f;
        [SerializeField, Range(0f, 1f)] private float lanternFlickerStrength = 0.18f;
        [SerializeField, Range(0f, 1f)] private float bannerWindSwayStrength = 0.32f;
        [SerializeField] private Vector2 bannerWindPhaseRange = new Vector2(0.15f, 4.8f);
        [SerializeField] private bool conservativeReviewMode = true;
        [SerializeField] private bool requiresTomArtApproval = true;
        [SerializeField] private bool cc0SourceReady;
        [SerializeField] private string sourceKitNote = "Procedural CC0-safe review baseline; replace with approved Quaternius/Kenney dressing meshes and cloth sprites.";
        [SerializeField] private string[] dressingTypes = new string[0];

        public int DistinctDressingTypeCountForReview => distinctDressingTypeCount;
        public int MarkerCountForReview => markerCount;
        public int LanternCountForReview => lanternCount;
        public int SpriteCardCountForReview => spriteCardCount;
        public float LanternEmissionIntensityForReview => lanternEmissionIntensity;
        public float LanternLightRangeForReview => lanternLightRange;
        public float LanternLightIntensityForReview => lanternLightIntensity;
        public float LanternFlickerStrengthForReview => lanternFlickerStrength;
        public float BannerWindSwayStrengthForReview => bannerWindSwayStrength;
        public Vector2 BannerWindPhaseRangeForReview => bannerWindPhaseRange;
        public bool ConservativeReviewModeForReview => conservativeReviewMode;
        public bool RequiresTomArtApprovalForReview => requiresTomArtApproval;
        public bool Cc0SourceReadyForReview => cc0SourceReady;
        public string SourceKitNoteForReview => sourceKitNote;
        public int DressingTypeRecordCountForReview => dressingTypes != null ? dressingTypes.Length : 0;

        public string GetDressingTypeForReview(int index)
        {
            if (dressingTypes == null || index < 0 || index >= dressingTypes.Length)
            {
                return string.Empty;
            }

            return dressingTypes[index] ?? string.Empty;
        }

        public void ConfigureForReview(
            int configuredDistinctDressingTypeCount,
            int configuredMarkerCount,
            int configuredLanternCount,
            int configuredSpriteCardCount,
            float configuredLanternEmissionIntensity,
            float configuredLanternLightRange,
            float configuredLanternLightIntensity,
            float configuredLanternFlickerStrength,
            float configuredBannerWindSwayStrength,
            Vector2 configuredBannerWindPhaseRange,
            bool configuredConservativeReviewMode,
            bool configuredRequiresTomArtApproval,
            bool configuredCc0SourceReady,
            string configuredSourceKitNote,
            string[] configuredDressingTypes)
        {
            distinctDressingTypeCount = Mathf.Max(1, configuredDistinctDressingTypeCount);
            markerCount = Mathf.Max(0, configuredMarkerCount);
            lanternCount = Mathf.Max(0, configuredLanternCount);
            spriteCardCount = Mathf.Max(0, configuredSpriteCardCount);
            lanternEmissionIntensity = Mathf.Max(0f, configuredLanternEmissionIntensity);
            lanternLightRange = Mathf.Max(0f, configuredLanternLightRange);
            lanternLightIntensity = Mathf.Max(0f, configuredLanternLightIntensity);
            lanternFlickerStrength = Mathf.Clamp01(configuredLanternFlickerStrength);
            bannerWindSwayStrength = Mathf.Clamp01(configuredBannerWindSwayStrength);
            bannerWindPhaseRange = configuredBannerWindPhaseRange;
            conservativeReviewMode = configuredConservativeReviewMode;
            requiresTomArtApproval = configuredRequiresTomArtApproval;
            cc0SourceReady = configuredCc0SourceReady;
            sourceKitNote = configuredSourceKitNote ?? string.Empty;
            dressingTypes = configuredDressingTypes ?? new string[0];
        }
    }
}
