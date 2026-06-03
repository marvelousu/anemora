using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Twinkling Star Field Profile")]
    public sealed class FastVsHd2dTwinklingStarFieldProfile : ScriptableObject
    {
        [SerializeField] private Color starColor = new Color(0.74f, 0.84f, 1.0f, 1f);
        [SerializeField, Range(24f, 220f)] private float starDensity = 132f;
        [SerializeField, Range(0.90f, 0.998f)] private float starThreshold = 0.974f;
        [SerializeField, Range(0.015f, 0.22f)] private float starPointSize = 0.075f;
        [SerializeField, Range(0f, 3f)] private float starIntensity = 1.28f;
        [SerializeField, Range(0f, 1f)] private float twinkleStrength = 0.42f;
        [SerializeField, Range(0.05f, 3f)] private float twinkleSpeed = 0.68f;
        [SerializeField, Range(0f, 0.2f)] private float horizonFadeStart = 0.025f;
        [SerializeField, Range(0.05f, 0.7f)] private float horizonFadeEnd = 0.28f;
        [SerializeField, Range(0f, 1f)] private float maxNightOpacity = 0.92f;
        [SerializeField, Range(0f, 0.5f)] private float milkyWayIntensity = 0.055f;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalStarFieldApproved;

        public Color StarColorForReview => starColor;
        public float StarDensityForReview => starDensity;
        public float StarThresholdForReview => starThreshold;
        public float StarPointSizeForReview => starPointSize;
        public float StarIntensityForReview => starIntensity;
        public float TwinkleStrengthForReview => twinkleStrength;
        public float TwinkleSpeedForReview => twinkleSpeed;
        public float HorizonFadeStartForReview => horizonFadeStart;
        public float HorizonFadeEndForReview => horizonFadeEnd;
        public float MaxNightOpacityForReview => maxNightOpacity;
        public float MilkyWayIntensityForReview => milkyWayIntensity;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalStarFieldApprovedForReview => finalStarFieldApproved;
        public int EstimatedNightStarCellCountForReview
        {
            get
            {
                var cells = Mathf.RoundToInt(starDensity) * Mathf.RoundToInt(starDensity * 0.5f);
                return Mathf.Max(0, Mathf.RoundToInt(cells * Mathf.Clamp01(1f - starThreshold)));
            }
        }

        public void ConfigureForReview(
            Color configuredStarColor,
            float configuredStarDensity,
            float configuredStarThreshold,
            float configuredStarPointSize,
            float configuredStarIntensity,
            float configuredTwinkleStrength,
            float configuredTwinkleSpeed,
            float configuredHorizonFadeStart,
            float configuredHorizonFadeEnd,
            float configuredMaxNightOpacity,
            float configuredMilkyWayIntensity,
            bool configuredNeedsTomApproval,
            bool configuredFinalStarFieldApproved)
        {
            starColor = configuredStarColor;
            starDensity = Mathf.Clamp(configuredStarDensity, 24f, 220f);
            starThreshold = Mathf.Clamp(configuredStarThreshold, 0.90f, 0.998f);
            starPointSize = Mathf.Clamp(configuredStarPointSize, 0.015f, 0.22f);
            starIntensity = Mathf.Clamp(configuredStarIntensity, 0f, 3f);
            twinkleStrength = Mathf.Clamp01(configuredTwinkleStrength);
            twinkleSpeed = Mathf.Clamp(configuredTwinkleSpeed, 0.05f, 3f);
            horizonFadeStart = Mathf.Clamp(configuredHorizonFadeStart, 0f, 0.2f);
            horizonFadeEnd = Mathf.Clamp(configuredHorizonFadeEnd, 0.05f, 0.7f);
            if (horizonFadeEnd <= horizonFadeStart + 0.02f)
            {
                horizonFadeEnd = Mathf.Min(0.7f, horizonFadeStart + 0.02f);
            }

            maxNightOpacity = Mathf.Clamp01(configuredMaxNightOpacity);
            milkyWayIntensity = Mathf.Clamp(configuredMilkyWayIntensity, 0f, 0.5f);
            needsTomApproval = configuredNeedsTomApproval;
            finalStarFieldApproved = configuredFinalStarFieldApproved;
        }

        public float EvaluateNightVisibilityForReview(float sunViewHeight)
        {
            return Mathf.Clamp01((-sunViewHeight * 2.25f) + 0.05f) * maxNightOpacity;
        }
    }
}
