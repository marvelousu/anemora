using System;
using Anemora.FastVS.SunCycle;
using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dVegetationTintProfile", menuName = "Anemora/HD2D/Vegetation Tint Profile")]
    public sealed class FastVsHd2dVegetationTintProfile : ScriptableObject
    {
        [SerializeField] private VegetationTimeOfDayTint[] timeOfDayTints = Array.Empty<VegetationTimeOfDayTint>();
        [SerializeField] private Color lushSeasonTint = Color.white;
        [SerializeField] private Color witheredSeasonTint = new Color(0.86f, 0.72f, 0.46f, 1f);
        [SerializeField, Range(0f, 1f)] private float defaultWitheredness = 0.08f;
        [SerializeField, Range(0.05f, 6f)] private float transitionSeconds = 1.8f;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalVegetationTintApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Conservative P2-58 vegetation tint data prep. Tom should tune final lush/withered hue, night coolness, and area/season mapping after lighting and ground sign-off.";

        public int TimeOfDayTintCountForReview => timeOfDayTints != null ? timeOfDayTints.Length : 0;
        public Color LushSeasonTintForReview => SanitizeTint(lushSeasonTint);
        public Color WitheredSeasonTintForReview => SanitizeTint(witheredSeasonTint);
        public float DefaultWitherednessForReview => Mathf.Clamp01(defaultWitheredness);
        public float TransitionSecondsForReview => Mathf.Max(0.05f, transitionSeconds);
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalVegetationTintApprovedForReview => finalVegetationTintApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            VegetationTimeOfDayTint[] configuredTimeOfDayTints,
            Color configuredLushSeasonTint,
            Color configuredWitheredSeasonTint,
            float configuredDefaultWitheredness,
            float configuredTransitionSeconds,
            bool configuredPublishEveryFrame,
            bool configuredNeedsTomApproval,
            bool configuredFinalVegetationTintApproved,
            string configuredRecommendation)
        {
            timeOfDayTints = configuredTimeOfDayTints ?? Array.Empty<VegetationTimeOfDayTint>();
            lushSeasonTint = SanitizeTint(configuredLushSeasonTint);
            witheredSeasonTint = SanitizeTint(configuredWitheredSeasonTint);
            defaultWitheredness = Mathf.Clamp01(configuredDefaultWitheredness);
            transitionSeconds = Mathf.Clamp(configuredTransitionSeconds, 0.05f, 6f);
            publishEveryFrame = configuredPublishEveryFrame;
            needsTomApproval = configuredNeedsTomApproval;
            finalVegetationTintApproved = configuredFinalVegetationTintApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }

        public Color ResolveSeasonTintForReview(float witheredness)
        {
            return Color.Lerp(LushSeasonTintForReview, WitheredSeasonTintForReview, Mathf.Clamp01(witheredness));
        }

        public bool TryResolveTimeOfDayTintForReview(SunPreset preset, out VegetationTimeOfDayTint tint)
        {
            if (timeOfDayTints != null)
            {
                for (var i = 0; i < timeOfDayTints.Length; i++)
                {
                    if (timeOfDayTints[i].Preset == preset)
                    {
                        tint = timeOfDayTints[i];
                        return true;
                    }
                }
            }

            tint = VegetationTimeOfDayTint.Default(preset);
            return false;
        }

        public VegetationTimeOfDayTint ResolveTimeOfDayTintForReview(SunPreset preset)
        {
            return TryResolveTimeOfDayTintForReview(preset, out var tint) ? tint : VegetationTimeOfDayTint.Default(preset);
        }

        public VegetationTimeOfDayTint EvaluateTimeOfDayBlendForReview(SunPreset fromPreset, SunPreset toPreset, float normalizedBlend)
        {
            return VegetationTimeOfDayTint.Lerp(
                ResolveTimeOfDayTintForReview(fromPreset),
                ResolveTimeOfDayTintForReview(toPreset),
                normalizedBlend);
        }

        private static Color SanitizeTint(Color color)
        {
            return new Color(
                Mathf.Max(0.001f, color.r),
                Mathf.Max(0.001f, color.g),
                Mathf.Max(0.001f, color.b),
                Mathf.Max(0.001f, color.a));
        }

        [Serializable]
        public struct VegetationTimeOfDayTint
        {
            [SerializeField] private SunPreset preset;
            [SerializeField] private Color timeOfDayTint;
            [SerializeField, Range(0f, 0.35f)] private float witherednessBias;

            public VegetationTimeOfDayTint(SunPreset preset, Color timeOfDayTint, float witherednessBias)
            {
                this.preset = preset;
                this.timeOfDayTint = SanitizeTint(timeOfDayTint);
                this.witherednessBias = Mathf.Clamp(witherednessBias, 0f, 0.35f);
            }

            public SunPreset Preset => preset;
            public Color TimeOfDayTint => SanitizeTint(timeOfDayTint);
            public float WitherednessBias => Mathf.Clamp(witherednessBias, 0f, 0.35f);

            public static VegetationTimeOfDayTint Default(SunPreset preset)
            {
                return new VegetationTimeOfDayTint(preset, Color.white, 0f);
            }

            public static VegetationTimeOfDayTint Lerp(VegetationTimeOfDayTint from, VegetationTimeOfDayTint to, float t)
            {
                var blend = Mathf.Clamp01(t);
                return new VegetationTimeOfDayTint(
                    to.preset,
                    Color.Lerp(from.TimeOfDayTint, to.TimeOfDayTint, blend),
                    Mathf.Lerp(from.WitherednessBias, to.WitherednessBias, blend));
            }
        }
    }
}
