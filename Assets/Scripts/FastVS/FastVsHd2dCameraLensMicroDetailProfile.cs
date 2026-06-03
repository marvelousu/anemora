using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Camera Lens Micro Detail Profile")]
    public sealed class FastVsHd2dCameraLensMicroDetailProfile : ScriptableObject
    {
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalLensMicroDetailApproved;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool userCinematicTogglePrepared = true;
        [SerializeField] private bool cinematicToggleDefaultEnabled;
        [SerializeField] private bool disabledForPromoScreenshots = true;
        [SerializeField] private bool runtimeActivationLockedUntilTomApproval = true;
        [SerializeField] private bool capturePreviewOnly = true;
        [SerializeField] private bool centralSpriteSharpnessPriority = true;
        [SerializeField] private FilmGrainLookup filmGrainType = FilmGrainLookup.Thin1;
        [SerializeField, Range(0.10f, 0.25f)] private float conservativeFilmGrainIntensity = 0.14f;
        [SerializeField, Range(0.10f, 0.30f)] private float strongerFilmGrainIntensity = 0.22f;
        [SerializeField, Range(0.70f, 0.80f)] private float filmGrainResponse = 0.76f;
        [SerializeField, Range(0.05f, 0.15f)] private float conservativeChromaticAberrationIntensity = 0.06f;
        [SerializeField, Range(0.05f, 0.18f)] private float strongerChromaticAberrationIntensity = 0.11f;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep the cinematic lens micro-detail toggle disabled by default and locked until Tom approves the look. The conservative option is preferred if the flat sky/fog grain reads as texture without softening central pixel sprites.";

        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalLensMicroDetailApprovedForReview => finalLensMicroDetailApproved;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool UserCinematicTogglePreparedForReview => userCinematicTogglePrepared;
        public bool CinematicToggleDefaultEnabledForReview => cinematicToggleDefaultEnabled;
        public bool DisabledForPromoScreenshotsForReview => disabledForPromoScreenshots;
        public bool RuntimeActivationLockedUntilTomApprovalForReview => runtimeActivationLockedUntilTomApproval;
        public bool CapturePreviewOnlyForReview => capturePreviewOnly;
        public bool CentralSpriteSharpnessPriorityForReview => centralSpriteSharpnessPriority;
        public FilmGrainLookup FilmGrainTypeForReview => filmGrainType;
        public float ConservativeFilmGrainIntensityForReview => Mathf.Clamp(conservativeFilmGrainIntensity, 0.10f, 0.25f);
        public float StrongerFilmGrainIntensityForReview => Mathf.Clamp(strongerFilmGrainIntensity, 0.10f, 0.30f);
        public float FilmGrainResponseForReview => Mathf.Clamp(filmGrainResponse, 0.70f, 0.80f);
        public float ConservativeChromaticAberrationIntensityForReview => Mathf.Clamp(conservativeChromaticAberrationIntensity, 0.05f, 0.15f);
        public float StrongerChromaticAberrationIntensityForReview => Mathf.Clamp(strongerChromaticAberrationIntensity, 0.05f, 0.18f);
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            bool configuredNeedsTomApproval,
            bool configuredFinalApproved,
            bool configuredConservativeDataPrep,
            bool configuredUserCinematicTogglePrepared,
            bool configuredCinematicToggleDefaultEnabled,
            bool configuredDisabledForPromoScreenshots,
            bool configuredRuntimeActivationLockedUntilTomApproval,
            bool configuredCapturePreviewOnly,
            bool configuredCentralSpriteSharpnessPriority,
            FilmGrainLookup configuredFilmGrainType,
            float configuredConservativeFilmGrainIntensity,
            float configuredStrongerFilmGrainIntensity,
            float configuredFilmGrainResponse,
            float configuredConservativeChromaticAberrationIntensity,
            float configuredStrongerChromaticAberrationIntensity,
            string configuredRecommendation)
        {
            needsTomApproval = configuredNeedsTomApproval;
            finalLensMicroDetailApproved = configuredFinalApproved;
            conservativeDataPrep = configuredConservativeDataPrep;
            userCinematicTogglePrepared = configuredUserCinematicTogglePrepared;
            cinematicToggleDefaultEnabled = configuredCinematicToggleDefaultEnabled;
            disabledForPromoScreenshots = configuredDisabledForPromoScreenshots;
            runtimeActivationLockedUntilTomApproval = configuredRuntimeActivationLockedUntilTomApproval;
            capturePreviewOnly = configuredCapturePreviewOnly;
            centralSpriteSharpnessPriority = configuredCentralSpriteSharpnessPriority;
            filmGrainType = configuredFilmGrainType;
            conservativeFilmGrainIntensity = Mathf.Clamp(configuredConservativeFilmGrainIntensity, 0.10f, 0.25f);
            strongerFilmGrainIntensity = Mathf.Clamp(configuredStrongerFilmGrainIntensity, 0.10f, 0.30f);
            filmGrainResponse = Mathf.Clamp(configuredFilmGrainResponse, 0.70f, 0.80f);
            conservativeChromaticAberrationIntensity = Mathf.Clamp(configuredConservativeChromaticAberrationIntensity, 0.05f, 0.15f);
            strongerChromaticAberrationIntensity = Mathf.Clamp(configuredStrongerChromaticAberrationIntensity, 0.05f, 0.18f);
            recommendation = configuredRecommendation ?? string.Empty;
        }
    }
}
