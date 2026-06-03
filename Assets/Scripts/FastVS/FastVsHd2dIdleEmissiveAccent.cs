using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dIdleEmissiveAccentKind
    {
        None = 0,
        EyeAndLantern = 1,
        MagicCharge = 2,
        WeaponEnchant = 3
    }

    [DefaultExecutionOrder(123)]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Idle Emissive Accent")]
    public sealed class FastVsHd2dIdleEmissiveAccent : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dIdleEmissiveProfile profile;
        [SerializeField] private FastVsHd2dIdleEmissiveAccentKind accentKind;
        [SerializeField] private Renderer spriteRenderer;
        [SerializeField] private Renderer haloRenderer;
        [SerializeField] private Light pointLight;
        [SerializeField] private bool reviewVisible = true;
        [SerializeField, Range(0f, 2f)] private float reviewEmissionMultiplier = 1f;
        [SerializeField, Range(0f, 2f)] private float reviewLightMultiplier = 1f;

        private float reviewTimeOverride = -1f;
        private float appliedPulseMultiplier = 1f;
        private float appliedLightIntensity;

        public FastVsHd2dIdleEmissiveProfile ProfileForReview => profile;
        public FastVsHd2dIdleEmissiveAccentKind AccentKindForReview => accentKind;
        public Renderer SpriteRendererForReview => spriteRenderer;
        public Renderer HaloRendererForReview => haloRenderer;
        public Light PointLightForReview => pointLight;
        public bool ReviewVisibleForReview => reviewVisible;
        public float ReviewEmissionMultiplierForReview => reviewEmissionMultiplier;
        public float ReviewLightMultiplierForReview => reviewLightMultiplier;
        public float AppliedPulseMultiplierForReview => appliedPulseMultiplier;
        public float AppliedLightIntensityForReview => appliedLightIntensity;

        private void OnEnable()
        {
            ApplyForReview();
        }

        private void LateUpdate()
        {
            ApplyForReview();
        }

        public void ConfigureForReview(
            FastVsHd2dIdleEmissiveProfile configuredProfile,
            FastVsHd2dIdleEmissiveAccentKind configuredAccentKind,
            Renderer configuredSpriteRenderer,
            Renderer configuredHaloRenderer,
            Light configuredPointLight)
        {
            profile = configuredProfile;
            accentKind = configuredAccentKind;
            spriteRenderer = configuredSpriteRenderer;
            haloRenderer = configuredHaloRenderer;
            pointLight = configuredPointLight;
            reviewVisible = true;
            reviewEmissionMultiplier = 1f;
            reviewLightMultiplier = 1f;
            reviewTimeOverride = -1f;
            ApplyForReview();
        }

        public void SetReviewVisibleForReview(bool visible)
        {
            reviewVisible = visible;
            ApplyForReview();
        }

        public void SetReviewMultipliersForReview(float emissionMultiplier, float lightMultiplier)
        {
            reviewEmissionMultiplier = Mathf.Clamp(emissionMultiplier, 0f, 2f);
            reviewLightMultiplier = Mathf.Clamp(lightMultiplier, 0f, 2f);
            ApplyForReview();
        }

        public void SetReviewTimeForReview(float seconds)
        {
            reviewTimeOverride = Mathf.Max(0f, seconds);
            ApplyForReview();
        }

        public void ClearReviewTimeForReview()
        {
            reviewTimeOverride = -1f;
            ApplyForReview();
        }

        public void ApplyForReview()
        {
            if (profile == null)
            {
                SetHaloEnabled(false);
                ApplyLight(false, 0f);
                return;
            }

            var active = reviewVisible && accentKind != FastVsHd2dIdleEmissiveAccentKind.None;
            var pulse = ResolvePulseMultiplier();
            appliedPulseMultiplier = active ? pulse * reviewEmissionMultiplier : 0f;
            SetHaloEnabled(active);
            ApplyLight(active, pulse);
        }

        private float ResolvePulseMultiplier()
        {
            if (profile == null)
            {
                return 1f;
            }

            var time = reviewTimeOverride >= 0f ? reviewTimeOverride : (Application.isPlaying ? Time.time : Time.realtimeSinceStartup);
            var wave = Mathf.Sin(time * profile.EmissivePulseFrequencyHzForReview * Mathf.PI * 2f);
            return Mathf.Max(0f, 1f + (wave * profile.EmissivePulseAmplitudeForReview));
        }

        private void SetHaloEnabled(bool active)
        {
            if (haloRenderer != null)
            {
                haloRenderer.enabled = active;
            }
        }

        private void ApplyLight(bool active, float pulse)
        {
            if (pointLight == null || profile == null)
            {
                appliedLightIntensity = 0f;
                return;
            }

            pointLight.enabled = active;
            pointLight.color = profile.EmissiveColorForReview;
            pointLight.range = profile.PointLightRangeMetersForReview;
            pointLight.shadows = LightShadows.None;
            appliedLightIntensity = active
                ? profile.PointLightIntensityForReview * Mathf.Max(0f, pulse) * reviewLightMultiplier
                : 0f;
            pointLight.intensity = appliedLightIntensity;
        }
    }
}
