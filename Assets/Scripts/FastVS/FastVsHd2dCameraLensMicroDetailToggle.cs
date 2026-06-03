using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Camera Lens Micro Detail Toggle")]
    public sealed class FastVsHd2dCameraLensMicroDetailToggle : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dCameraLensMicroDetailProfile profile;
        [SerializeField] private Volume targetVolume;
        [SerializeField] private bool cinematicToggleRequested;
        [SerializeField] private bool strongerTomOptionRequested;
        [SerializeField] private bool promoScreenshotSuppressed;
        [SerializeField] private float lastFilmGrainIntensity;
        [SerializeField] private float lastChromaticAberrationIntensity;

        public bool IsReadyForReview =>
            profile != null &&
            targetVolume != null &&
            targetVolume.sharedProfile != null &&
            targetVolume.sharedProfile.TryGet<FilmGrain>(out _) &&
            targetVolume.sharedProfile.TryGet<ChromaticAberration>(out _);

        public FastVsHd2dCameraLensMicroDetailProfile ProfileForReview => profile;
        public Volume TargetVolumeForReview => targetVolume;
        public bool CinematicToggleRequestedForReview => cinematicToggleRequested;
        public bool StrongerTomOptionRequestedForReview => strongerTomOptionRequested;
        public bool PromoScreenshotSuppressedForReview => promoScreenshotSuppressed;
        public float LastFilmGrainIntensityForReview => lastFilmGrainIntensity;
        public float LastChromaticAberrationIntensityForReview => lastChromaticAberrationIntensity;
        public bool RuntimeToggleLockedByTomForReview =>
            profile != null &&
            profile.RuntimeActivationLockedUntilTomApprovalForReview &&
            profile.NeedsTomApprovalForReview &&
            !profile.FinalLensMicroDetailApprovedForReview;

        private void OnEnable()
        {
            ApplyDefaultStateForReview();
        }

        private void OnDisable()
        {
            ApplyOffStateForReview();
        }

        public void ConfigureForReview(FastVsHd2dCameraLensMicroDetailProfile configuredProfile, Volume configuredTargetVolume)
        {
            profile = configuredProfile;
            targetVolume = configuredTargetVolume;
            ApplyDefaultStateForReview();
        }

        public void ApplyDefaultStateForReview()
        {
            var suppressPromo = profile == null || profile.DisabledForPromoScreenshotsForReview;
            ApplyCinematicToggleForReview(profile != null && profile.CinematicToggleDefaultEnabledForReview, false, suppressPromo);
        }

        public void ApplyOffStateForReview()
        {
            cinematicToggleRequested = false;
            strongerTomOptionRequested = false;
            promoScreenshotSuppressed = false;
            ApplyVolumeSettings(0f, 0f);
        }

        public bool ApplyCinematicToggleForReview(bool enabled, bool strongerTomOption, bool promoScreenshot)
        {
            cinematicToggleRequested = enabled;
            strongerTomOptionRequested = strongerTomOption;
            promoScreenshotSuppressed = promoScreenshot && profile != null && profile.DisabledForPromoScreenshotsForReview;

            var allowed = enabled && !promoScreenshotSuppressed && !RuntimeToggleLockedByTomForReview && profile != null;
            var filmGrainIntensity = allowed
                ? strongerTomOption ? profile.StrongerFilmGrainIntensityForReview : profile.ConservativeFilmGrainIntensityForReview
                : 0f;
            var chromaticAberrationIntensity = allowed
                ? strongerTomOption ? profile.StrongerChromaticAberrationIntensityForReview : profile.ConservativeChromaticAberrationIntensityForReview
                : 0f;

            ApplyVolumeSettings(filmGrainIntensity, chromaticAberrationIntensity);
            return allowed;
        }

        private void ApplyVolumeSettings(float filmGrainIntensity, float chromaticAberrationIntensity)
        {
            lastFilmGrainIntensity = Mathf.Clamp01(filmGrainIntensity);
            lastChromaticAberrationIntensity = Mathf.Clamp01(chromaticAberrationIntensity);
            if (targetVolume == null || targetVolume.sharedProfile == null)
            {
                return;
            }

            if (targetVolume.sharedProfile.TryGet<FilmGrain>(out var filmGrain))
            {
                filmGrain.active = lastFilmGrainIntensity > 0f;
                filmGrain.type.overrideState = true;
                filmGrain.type.value = profile != null ? profile.FilmGrainTypeForReview : FilmGrainLookup.Thin1;
                filmGrain.intensity.overrideState = true;
                filmGrain.intensity.value = lastFilmGrainIntensity;
                filmGrain.response.overrideState = true;
                filmGrain.response.value = profile != null ? profile.FilmGrainResponseForReview : 0.76f;
                filmGrain.texture.overrideState = true;
                filmGrain.texture.value = null;
            }

            if (targetVolume.sharedProfile.TryGet<ChromaticAberration>(out var chromaticAberration))
            {
                chromaticAberration.active = lastChromaticAberrationIntensity > 0f;
                chromaticAberration.intensity.overrideState = true;
                chromaticAberration.intensity.value = lastChromaticAberrationIntensity;
            }
        }
    }
}
