using Anemora.FastVS.SunCycle;
using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/FastVS/HD2D Atmosphere Preset Blender")]
    public sealed class FastVsHd2dAtmospherePresetBlender : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dAtmospherePresetBlendProfile profile;
        [SerializeField] private AnemoraSunCycleDriver sunCycleDriver;
        [SerializeField] private FastVsHd2dAtmosphericPerspectiveDriver atmosphericDriver;
        [SerializeField] private FastVsDynamicSunShaftField shaftField;
        [SerializeField] private FastVsHd2dSunShaftDustMoteField sunShaftDustMoteField;
        [SerializeField] private FastVsHd2dAmbientDustPollenLayer ambientDustLayer;
        [SerializeField] private FastVsHd2dAmbientVfxDirector ambientVfxDirector;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool applyToParticles = true;
        [SerializeField] private bool conservativeNeedsTomApproval = true;

        private bool reviewActive = true;
        private bool hasReviewPreset;
        private bool hasReviewBlend;
        private SunPreset reviewPreset = SunPreset.Morning;
        private SunPreset reviewBlendFrom = SunPreset.Morning;
        private SunPreset reviewBlendTo = SunPreset.Noon;
        private float reviewBlendT;
        private bool transitionBlending;
        private SunPreset transitionFrom = SunPreset.Morning;
        private SunPreset transitionTo = SunPreset.Morning;
        private float transitionElapsed;

        public FastVsHd2dAtmospherePresetBlendProfile ProfileForReview => profile;
        public AnemoraSunCycleDriver SunCycleDriverForReview => sunCycleDriver;
        public FastVsHd2dAtmosphericPerspectiveDriver AtmosphericDriverForReview => atmosphericDriver;
        public FastVsDynamicSunShaftField ShaftFieldForReview => shaftField;
        public FastVsHd2dSunShaftDustMoteField SunShaftDustMoteFieldForReview => sunShaftDustMoteField;
        public FastVsHd2dAmbientDustPollenLayer AmbientDustLayerForReview => ambientDustLayer;
        public FastVsHd2dAmbientVfxDirector AmbientVfxDirectorForReview => ambientVfxDirector;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public bool ApplyToParticlesForReview => applyToParticles;
        public bool ConservativeNeedsTomApprovalForReview => conservativeNeedsTomApproval;
        public bool ReviewActiveForReview => reviewActive;
        public SunPreset LastPresetForReview { get; private set; } = SunPreset.Morning;
        public float LastBlendTForReview { get; private set; }
        public float LastFogStrengthForReview { get; private set; }
        public Color LastFogFarColorForReview { get; private set; } = Color.white;
        public float LastAerialTintStrengthForReview { get; private set; }
        public float LastShaftMultiplierForReview { get; private set; } = 1f;
        public float LastAmbientDustMultiplierForReview { get; private set; } = 1f;
        public float LastSunMoteMultiplierForReview { get; private set; } = 1f;

        private void OnEnable()
        {
            ResolveReferences();
            SubscribeToSunCycle();
            PublishNowForReview();
        }

        private void OnDisable()
        {
            UnsubscribeFromSunCycle();
            ClearPublishedOverridesForReview();
        }

        private void LateUpdate()
        {
            if (!publishEveryFrame || profile == null || !profile.PublishEveryFrameForReview)
            {
                return;
            }

            if (transitionBlending)
            {
                transitionElapsed += Application.isPlaying ? Time.deltaTime : 0f;
            }

            PublishNowForReview();
        }

        public void ConfigureForReview(
            FastVsHd2dAtmospherePresetBlendProfile configuredProfile,
            AnemoraSunCycleDriver configuredSunCycleDriver,
            FastVsHd2dAtmosphericPerspectiveDriver configuredAtmosphericDriver,
            FastVsDynamicSunShaftField configuredShaftField,
            FastVsHd2dSunShaftDustMoteField configuredSunShaftDustMoteField,
            FastVsHd2dAmbientDustPollenLayer configuredAmbientDustLayer,
            FastVsHd2dAmbientVfxDirector configuredAmbientVfxDirector)
        {
            UnsubscribeFromSunCycle();
            profile = configuredProfile;
            sunCycleDriver = configuredSunCycleDriver;
            atmosphericDriver = configuredAtmosphericDriver;
            shaftField = configuredShaftField;
            sunShaftDustMoteField = configuredSunShaftDustMoteField;
            ambientDustLayer = configuredAmbientDustLayer;
            ambientVfxDirector = configuredAmbientVfxDirector;
            publishEveryFrame = profile == null || profile.PublishEveryFrameForReview;
            conservativeNeedsTomApproval = profile == null || profile.NeedsTomApprovalForReview;
            SubscribeToSunCycle();
            PublishNowForReview();
        }

        public void SetReviewActiveForReview(bool active)
        {
            reviewActive = active;
            if (reviewActive)
            {
                PublishNowForReview();
            }
            else
            {
                ClearPublishedOverridesForReview();
            }
        }

        public void ApplyPresetForReview(SunPreset preset)
        {
            hasReviewPreset = true;
            hasReviewBlend = false;
            reviewPreset = preset;
            PublishNowForReview();
        }

        public void ApplyBlendForReview(SunPreset fromPreset, SunPreset toPreset, float normalizedBlend)
        {
            hasReviewPreset = false;
            hasReviewBlend = true;
            reviewBlendFrom = fromPreset;
            reviewBlendTo = toPreset;
            reviewBlendT = Mathf.Clamp01(normalizedBlend);
            PublishNowForReview();
        }

        public void ClearReviewStateForReview()
        {
            hasReviewPreset = false;
            hasReviewBlend = false;
            reviewBlendT = 0f;
            reviewActive = true;
            PublishNowForReview();
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            PublishNowForReview();
            var duration = Mathf.Max(0f, seconds);
            if (ambientDustLayer != null)
            {
                ambientDustLayer.SimulateForReview(duration, restart);
            }

            if (sunShaftDustMoteField != null)
            {
                sunShaftDustMoteField.SimulateForReview(duration, restart);
            }

            if (ambientVfxDirector != null)
            {
                ambientVfxDirector.SimulateForReview(duration, restart);
            }
        }

        public void PublishNowForReview()
        {
            ResolveReferences();
            if (!reviewActive)
            {
                ClearPublishedOverridesForReview();
                return;
            }

            if (profile == null)
            {
                return;
            }

            var values = ResolveCurrentPresetValues();
            LastPresetForReview = values.Preset;
            LastFogStrengthForReview = values.FogStrength;
            LastFogFarColorForReview = values.FogFarColor;
            LastAerialTintStrengthForReview = values.AerialTintStrength;
            LastShaftMultiplierForReview = values.ShaftIntensityMultiplier;
            LastAmbientDustMultiplierForReview = values.AmbientDustEmissionMultiplier;
            LastSunMoteMultiplierForReview = values.SunMoteEmissionMultiplier;

            if (atmosphericDriver != null)
            {
                atmosphericDriver.SetAtmospherePresetOverrideForReview(
                    values.FogStrength,
                    values.FogNearColor,
                    values.FogFarColor,
                    values.FogGradient,
                    values.DistanceStart,
                    values.DistanceEnd,
                    values.HeightBand,
                    values.HeightStrength,
                    values.AerialTintStrength,
                    values.AerialTintDistancePadding);
            }

            if (shaftField != null)
            {
                shaftField.SetAtmospherePresetOverrideForReview(values.ShaftIntensityMultiplier, values.ShaftTint);
            }

            if (!applyToParticles)
            {
                return;
            }

            if (ambientDustLayer != null)
            {
                ambientDustLayer.SetReviewTintWarmthForReview(values.AmbientDustTintWarmth);
                ambientDustLayer.SetReviewOverrideForReview(values.AmbientDustEmissionMultiplier > 0.001f, values.AmbientDustEmissionMultiplier);
            }

            if (sunShaftDustMoteField != null)
            {
                sunShaftDustMoteField.SetReviewOverrideForReview(values.SunMoteEmissionMultiplier > 0.001f, values.SunMoteEmissionMultiplier);
            }
        }

        public void ClearPublishedOverridesForReview()
        {
            if (atmosphericDriver != null)
            {
                atmosphericDriver.ClearAtmospherePresetOverrideForReview();
            }

            if (shaftField != null)
            {
                shaftField.ClearAtmospherePresetOverrideForReview();
            }

            if (ambientDustLayer != null)
            {
                ambientDustLayer.ClearReviewOverrideForReview();
            }

            if (sunShaftDustMoteField != null)
            {
                sunShaftDustMoteField.ClearReviewOverrideForReview();
            }
        }

        private FastVsHd2dAtmospherePresetBlendProfile.AtmospherePreset ResolveCurrentPresetValues()
        {
            if (hasReviewBlend)
            {
                LastBlendTForReview = reviewBlendT;
                return profile.EvaluateBlendForReview(reviewBlendFrom, reviewBlendTo, reviewBlendT);
            }

            if (hasReviewPreset)
            {
                LastBlendTForReview = 1f;
                return profile.ResolvePresetForReview(reviewPreset);
            }

            if (transitionBlending && sunCycleDriver != null && sunCycleDriver.IsTransitioning)
            {
                var duration = profile.TransitionSecondsForReview;
                var t = duration > 0.001f ? Mathf.Clamp01(transitionElapsed / duration) : 1f;
                LastBlendTForReview = t;
                return profile.EvaluateBlendForReview(transitionFrom, transitionTo, t);
            }

            transitionBlending = false;
            LastBlendTForReview = 1f;
            return profile.ResolvePresetForReview(sunCycleDriver != null ? sunCycleDriver.CurrentPreset : SunPreset.Morning);
        }

        private void ResolveReferences()
        {
            if (sunCycleDriver == null)
            {
                sunCycleDriver = AnemoraSunCycleDriver.Instance != null
                    ? AnemoraSunCycleDriver.Instance
                    : FindFirstObjectByType<AnemoraSunCycleDriver>(FindObjectsInactive.Include);
            }

            if (atmosphericDriver == null)
            {
                atmosphericDriver = FindFirstObjectByType<FastVsHd2dAtmosphericPerspectiveDriver>(FindObjectsInactive.Include);
            }

            if (shaftField == null)
            {
                shaftField = FindFirstObjectByType<FastVsDynamicSunShaftField>(FindObjectsInactive.Include);
            }

            if (sunShaftDustMoteField == null)
            {
                sunShaftDustMoteField = FindFirstObjectByType<FastVsHd2dSunShaftDustMoteField>(FindObjectsInactive.Include);
            }

            if (ambientDustLayer == null)
            {
                ambientDustLayer = FindFirstObjectByType<FastVsHd2dAmbientDustPollenLayer>(FindObjectsInactive.Include);
            }

            if (ambientVfxDirector == null)
            {
                ambientVfxDirector = FindFirstObjectByType<FastVsHd2dAmbientVfxDirector>(FindObjectsInactive.Include);
            }
        }

        private void SubscribeToSunCycle()
        {
            if (sunCycleDriver == null)
            {
                return;
            }

            sunCycleDriver.OnPresetTransitionStart -= HandlePresetTransitionStart;
            sunCycleDriver.OnPresetChanged -= HandlePresetChanged;
            sunCycleDriver.OnPresetTransitionStart += HandlePresetTransitionStart;
            sunCycleDriver.OnPresetChanged += HandlePresetChanged;
        }

        private void UnsubscribeFromSunCycle()
        {
            if (sunCycleDriver == null)
            {
                return;
            }

            sunCycleDriver.OnPresetTransitionStart -= HandlePresetTransitionStart;
            sunCycleDriver.OnPresetChanged -= HandlePresetChanged;
        }

        private void HandlePresetTransitionStart(SunPreset fromPreset, SunPreset toPreset)
        {
            transitionFrom = fromPreset;
            transitionTo = toPreset;
            transitionElapsed = 0f;
            transitionBlending = true;
        }

        private void HandlePresetChanged(SunPreset fromPreset, SunPreset toPreset)
        {
            _ = fromPreset;
            transitionFrom = toPreset;
            transitionTo = toPreset;
            transitionElapsed = profile != null ? profile.TransitionSecondsForReview : 0f;
            transitionBlending = false;
            PublishNowForReview();
        }
    }
}
