using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Diorama Vignette Profile")]
    public sealed class FastVsHd2dDioramaVignetteProfile : ScriptableObject
    {
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalDioramaVignetteApproved;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool sharedVolumeCurrentLockedByLegacyValidation = true;
        [SerializeField] private bool runtimeDefaultUnchanged = true;
        [SerializeField] private bool roundedCandidatePreferred = true;
        [SerializeField, Range(0.20f, 0.45f)] private float currentSharedIntensity = 0.30f;
        [SerializeField, Range(0.20f, 0.80f)] private float currentSharedSmoothness = 0.40f;
        [SerializeField] private bool currentSharedRounded;
        [SerializeField, Range(0.20f, 0.45f)] private float conservativeIntensity = 0.30f;
        [SerializeField, Range(0.50f, 0.70f)] private float conservativeSmoothness = 0.58f;
        [SerializeField] private bool conservativeRounded = true;
        [SerializeField, Range(0.25f, 0.45f)] private float strongerIntensity = 0.38f;
        [SerializeField, Range(0.50f, 0.75f)] private float strongerSmoothness = 0.64f;
        [SerializeField] private Color vignetteColor = new Color(0.035f, 0.045f, 0.055f, 1f);
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep the shared runtime Vignette unchanged until Tom approves a softer rounded candidate. Prefer intensity 0.30 / smoothness 0.58 / rounded on if the A/B keeps corner gameplay readable.";

        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalDioramaVignetteApprovedForReview => finalDioramaVignetteApproved;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool SharedVolumeCurrentLockedByLegacyValidationForReview => sharedVolumeCurrentLockedByLegacyValidation;
        public bool RuntimeDefaultUnchangedForReview => runtimeDefaultUnchanged;
        public bool RoundedCandidatePreferredForReview => roundedCandidatePreferred;
        public float CurrentSharedIntensityForReview => Mathf.Clamp(currentSharedIntensity, 0.20f, 0.45f);
        public float CurrentSharedSmoothnessForReview => Mathf.Clamp(currentSharedSmoothness, 0.20f, 0.80f);
        public bool CurrentSharedRoundedForReview => currentSharedRounded;
        public float ConservativeIntensityForReview => Mathf.Clamp(conservativeIntensity, 0.20f, 0.45f);
        public float ConservativeSmoothnessForReview => Mathf.Clamp(conservativeSmoothness, 0.50f, 0.70f);
        public bool ConservativeRoundedForReview => conservativeRounded;
        public float StrongerIntensityForReview => Mathf.Clamp(strongerIntensity, 0.25f, 0.45f);
        public float StrongerSmoothnessForReview => Mathf.Clamp(strongerSmoothness, 0.50f, 0.75f);
        public Color VignetteColorForReview
        {
            get
            {
                var color = vignetteColor;
                color.a = 1f;
                return color;
            }
        }

        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            bool configuredNeedsTomApproval,
            bool configuredFinalApproved,
            bool configuredConservativeDataPrep,
            bool configuredSharedVolumeCurrentLocked,
            bool configuredRuntimeDefaultUnchanged,
            bool configuredRoundedCandidatePreferred,
            float configuredCurrentSharedIntensity,
            float configuredCurrentSharedSmoothness,
            bool configuredCurrentSharedRounded,
            float configuredConservativeIntensity,
            float configuredConservativeSmoothness,
            bool configuredConservativeRounded,
            float configuredStrongerIntensity,
            float configuredStrongerSmoothness,
            Color configuredVignetteColor,
            string configuredRecommendation)
        {
            needsTomApproval = configuredNeedsTomApproval;
            finalDioramaVignetteApproved = configuredFinalApproved;
            conservativeDataPrep = configuredConservativeDataPrep;
            sharedVolumeCurrentLockedByLegacyValidation = configuredSharedVolumeCurrentLocked;
            runtimeDefaultUnchanged = configuredRuntimeDefaultUnchanged;
            roundedCandidatePreferred = configuredRoundedCandidatePreferred;
            currentSharedIntensity = Mathf.Clamp(configuredCurrentSharedIntensity, 0.20f, 0.45f);
            currentSharedSmoothness = Mathf.Clamp(configuredCurrentSharedSmoothness, 0.20f, 0.80f);
            currentSharedRounded = configuredCurrentSharedRounded;
            conservativeIntensity = Mathf.Clamp(configuredConservativeIntensity, 0.20f, 0.45f);
            conservativeSmoothness = Mathf.Clamp(configuredConservativeSmoothness, 0.50f, 0.70f);
            conservativeRounded = configuredConservativeRounded;
            strongerIntensity = Mathf.Clamp(configuredStrongerIntensity, 0.25f, 0.45f);
            strongerSmoothness = Mathf.Clamp(configuredStrongerSmoothness, 0.50f, 0.75f);
            vignetteColor = configuredVignetteColor;
            vignetteColor.a = 1f;
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
