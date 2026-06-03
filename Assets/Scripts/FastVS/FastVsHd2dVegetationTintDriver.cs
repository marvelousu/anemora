using Anemora.FastVS.SunCycle;
using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Vegetation Tint Driver")]
    public sealed class FastVsHd2dVegetationTintDriver : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dVegetationTintProfile profile;
        [SerializeField] private FastVsHd2dVegetationWindManager windManager;
        [SerializeField] private AnemoraSunCycleDriver sunCycleDriver;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool conservativeNeedsTomApproval = true;

        private bool reviewActive = true;
        private bool hasReviewPreset;
        private bool hasReviewBlend;
        private SunPreset reviewPreset = SunPreset.Noon;
        private SunPreset reviewBlendFrom = SunPreset.Noon;
        private SunPreset reviewBlendTo = SunPreset.Evening;
        private float reviewBlendT;
        private float reviewWitheredness = 0.08f;

        public FastVsHd2dVegetationTintProfile ProfileForReview => profile;
        public FastVsHd2dVegetationWindManager WindManagerForReview => windManager;
        public AnemoraSunCycleDriver SunCycleDriverForReview => sunCycleDriver;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public bool ConservativeNeedsTomApprovalForReview => conservativeNeedsTomApproval;
        public bool ReviewActiveForReview => reviewActive;
        public SunPreset LastPresetForReview { get; private set; } = SunPreset.Noon;
        public float LastBlendTForReview { get; private set; }
        public Color LastSeasonTintForReview { get; private set; } = Color.white;
        public Color LastTimeOfDayTintForReview { get; private set; } = Color.white;
        public float LastWitherednessForReview { get; private set; }

        private void OnEnable()
        {
            ResolveReferences();
            SubscribeToSunCycle();
            PublishNowForReview();
        }

        private void OnDisable()
        {
            UnsubscribeFromSunCycle();
            ClearPublishedTintForReview();
        }

        private void LateUpdate()
        {
            if (!publishEveryFrame || profile == null || !profile.PublishEveryFrameForReview)
            {
                return;
            }

            PublishNowForReview();
        }

        public void ConfigureForReview(
            FastVsHd2dVegetationTintProfile configuredProfile,
            FastVsHd2dVegetationWindManager configuredWindManager,
            AnemoraSunCycleDriver configuredSunCycleDriver)
        {
            UnsubscribeFromSunCycle();
            profile = configuredProfile;
            windManager = configuredWindManager;
            sunCycleDriver = configuredSunCycleDriver;
            publishEveryFrame = profile == null || profile.PublishEveryFrameForReview;
            conservativeNeedsTomApproval = profile == null || profile.NeedsTomApprovalForReview;
            reviewWitheredness = profile != null ? profile.DefaultWitherednessForReview : 0f;
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
                ClearPublishedTintForReview();
            }
        }

        public void ApplyPresetForReview(SunPreset preset, float witheredness)
        {
            hasReviewPreset = true;
            hasReviewBlend = false;
            reviewPreset = preset;
            reviewWitheredness = Mathf.Clamp01(witheredness);
            PublishNowForReview();
        }

        public void ApplyBlendForReview(SunPreset fromPreset, SunPreset toPreset, float normalizedBlend, float witheredness)
        {
            hasReviewPreset = false;
            hasReviewBlend = true;
            reviewBlendFrom = fromPreset;
            reviewBlendTo = toPreset;
            reviewBlendT = Mathf.Clamp01(normalizedBlend);
            reviewWitheredness = Mathf.Clamp01(witheredness);
            PublishNowForReview();
        }

        public void ClearReviewStateForReview()
        {
            hasReviewPreset = false;
            hasReviewBlend = false;
            reviewBlendT = 0f;
            reviewWitheredness = profile != null ? profile.DefaultWitherednessForReview : 0f;
            reviewActive = true;
            PublishNowForReview();
        }

        public void PublishNowForReview()
        {
            ResolveReferences();
            if (!reviewActive)
            {
                ClearPublishedTintForReview();
                return;
            }

            if (profile == null || windManager == null)
            {
                return;
            }

            var tint = ResolveCurrentTimeOfDayTint();
            var baseWitheredness = hasReviewPreset || hasReviewBlend
                ? reviewWitheredness
                : profile.DefaultWitherednessForReview;
            var witheredness = Mathf.Clamp01(baseWitheredness + tint.WitherednessBias);
            var seasonTint = profile.ResolveSeasonTintForReview(witheredness);
            windManager.ApplyVegetationTintReviewStateForReview(seasonTint, tint.TimeOfDayTint, witheredness);

            LastSeasonTintForReview = seasonTint;
            LastTimeOfDayTintForReview = tint.TimeOfDayTint;
            LastWitherednessForReview = witheredness;
        }

        public void ClearPublishedTintForReview()
        {
            if (windManager != null)
            {
                windManager.ApplyVegetationTintReviewStateForReview(Color.white, Color.white, 0f);
            }

            LastSeasonTintForReview = Color.white;
            LastTimeOfDayTintForReview = Color.white;
            LastWitherednessForReview = 0f;
            LastBlendTForReview = 0f;
        }

        private FastVsHd2dVegetationTintProfile.VegetationTimeOfDayTint ResolveCurrentTimeOfDayTint()
        {
            if (hasReviewBlend)
            {
                LastPresetForReview = reviewBlendTo;
                LastBlendTForReview = reviewBlendT;
                return profile.EvaluateTimeOfDayBlendForReview(reviewBlendFrom, reviewBlendTo, reviewBlendT);
            }

            var preset = hasReviewPreset
                ? reviewPreset
                : (sunCycleDriver != null ? sunCycleDriver.CurrentPreset : SunPreset.Noon);
            LastPresetForReview = preset;
            LastBlendTForReview = hasReviewPreset ? 1f : 0f;
            return profile.ResolveTimeOfDayTintForReview(preset);
        }

        private void ResolveReferences()
        {
            if (windManager == null)
            {
                windManager = FindFirstObjectByType<FastVsHd2dVegetationWindManager>();
            }

            if (sunCycleDriver == null)
            {
                sunCycleDriver = FindFirstObjectByType<AnemoraSunCycleDriver>();
            }
        }

        private void SubscribeToSunCycle()
        {
            if (sunCycleDriver != null)
            {
                sunCycleDriver.OnPresetChanged -= HandleSunPresetChanged;
                sunCycleDriver.OnPresetTransitionStart -= HandleSunPresetTransitionStart;
                sunCycleDriver.OnPresetChanged += HandleSunPresetChanged;
                sunCycleDriver.OnPresetTransitionStart += HandleSunPresetTransitionStart;
            }
        }

        private void UnsubscribeFromSunCycle()
        {
            if (sunCycleDriver != null)
            {
                sunCycleDriver.OnPresetChanged -= HandleSunPresetChanged;
                sunCycleDriver.OnPresetTransitionStart -= HandleSunPresetTransitionStart;
            }
        }

        private void HandleSunPresetChanged(SunPreset previous, SunPreset current)
        {
            if (!hasReviewPreset && !hasReviewBlend)
            {
                PublishNowForReview();
            }
        }

        private void HandleSunPresetTransitionStart(SunPreset from, SunPreset to)
        {
            if (!hasReviewPreset && !hasReviewBlend)
            {
                PublishNowForReview();
            }
        }
    }
}
