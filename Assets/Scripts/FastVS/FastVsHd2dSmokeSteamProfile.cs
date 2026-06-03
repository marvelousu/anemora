using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dSmokeSteamKind
    {
        ChimneySmoke,
        CookfireSmoke,
        SteamColumn
    }

    [CreateAssetMenu(fileName = "FastVsHd2dSmokeSteamProfile", menuName = "Anemora/HD2D/Smoke Steam Profile")]
    public sealed class FastVsHd2dSmokeSteamProfile : ScriptableObject
    {
        [SerializeField, Range(12, 40)] private int maxParticlesPerColumn = 36;
        [SerializeField, Range(0.1f, 18f)] private float chimneyEmissionRate = 6.4f;
        [SerializeField, Range(0.1f, 18f)] private float cookfireEmissionRate = 7.8f;
        [SerializeField, Range(0.1f, 18f)] private float steamEmissionRate = 5.8f;
        [SerializeField, Range(1.2f, 7.5f)] private float smokeLifetime = 5.2f;
        [SerializeField, Range(0.8f, 5.5f)] private float steamLifetime = 3.4f;
        [SerializeField, Range(0.04f, 0.80f)] private float smokeStartSize = 0.24f;
        [SerializeField, Range(0.03f, 0.70f)] private float steamStartSize = 0.18f;
        [SerializeField, Range(1.0f, 4.0f)] private float sizeEndMultiplier = 2.35f;
        [SerializeField, Range(0.05f, 1.2f)] private float smokeRiseVelocity = 0.42f;
        [SerializeField, Range(0.05f, 1.4f)] private float steamRiseVelocity = 0.54f;
        [SerializeField] private Vector3 sharedWindDirection = new Vector3(0.86f, 0f, 0.50f);
        [SerializeField, Range(0f, 1.5f)] private float reviewWindStrength = 0.78f;
        [SerializeField, Range(0f, 0.8f)] private float noiseStrength = 0.18f;
        [SerializeField, Range(0.01f, 1.0f)] private float noiseFrequency = 0.17f;
        [SerializeField, Range(8f, 80f)] private float distanceCullFarMeters = 46f;
        [SerializeField, Range(1f, 2.2f)] private float strongerOptionMultiplier = 1.35f;
        [SerializeField] private Color chimneySmokeColor = new Color(0.66f, 0.71f, 0.76f, 0.20f);
        [SerializeField] private Color cookfireSmokeColor = new Color(0.58f, 0.52f, 0.44f, 0.22f);
        [SerializeField] private Color steamColor = new Color(0.78f, 0.88f, 0.96f, 0.16f);
        [SerializeField] private bool persistentLoopingShuriken = true;
        [SerializeField] private bool sharedAmbientVfxWind = true;
        [SerializeField] private bool softParticlesRequired = true;
        [SerializeField] private bool distanceCullFarColumns = true;
        [SerializeField] private bool conservativeDataPrep = true;
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalSmokeSteamApproved;
        [SerializeField, TextArea(2, 4)] private string recommendation =
            "Keep the conservative soft-particle smoke/steam columns as data prep. Tom should tune final density, alpha, height, and roof-edge softness against the approved camera and color grade.";

        public int MaxParticlesPerColumnForReview => maxParticlesPerColumn;
        public float ChimneyEmissionRateForReview => chimneyEmissionRate;
        public float CookfireEmissionRateForReview => cookfireEmissionRate;
        public float SteamEmissionRateForReview => steamEmissionRate;
        public float SmokeLifetimeForReview => smokeLifetime;
        public float SteamLifetimeForReview => steamLifetime;
        public float SmokeStartSizeForReview => smokeStartSize;
        public float SteamStartSizeForReview => steamStartSize;
        public float SizeEndMultiplierForReview => sizeEndMultiplier;
        public float SmokeRiseVelocityForReview => smokeRiseVelocity;
        public float SteamRiseVelocityForReview => steamRiseVelocity;
        public Vector3 SharedWindDirectionForReview => SanitizeDirection(sharedWindDirection);
        public float ReviewWindStrengthForReview => reviewWindStrength;
        public float NoiseStrengthForReview => noiseStrength;
        public float NoiseFrequencyForReview => noiseFrequency;
        public float DistanceCullFarMetersForReview => distanceCullFarMeters;
        public float StrongerOptionMultiplierForReview => strongerOptionMultiplier;
        public Color ChimneySmokeColorForReview => chimneySmokeColor;
        public Color CookfireSmokeColorForReview => cookfireSmokeColor;
        public Color SteamColorForReview => steamColor;
        public bool PersistentLoopingShurikenForReview => persistentLoopingShuriken;
        public bool SharedAmbientVfxWindForReview => sharedAmbientVfxWind;
        public bool SoftParticlesRequiredForReview => softParticlesRequired;
        public bool DistanceCullFarColumnsForReview => distanceCullFarColumns;
        public bool ConservativeDataPrepForReview => conservativeDataPrep;
        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalSmokeSteamApprovedForReview => finalSmokeSteamApproved;
        public string RecommendationForReview => recommendation ?? string.Empty;

        public void ConfigureForReview(
            int configuredMaxParticlesPerColumn,
            float configuredChimneyEmissionRate,
            float configuredCookfireEmissionRate,
            float configuredSteamEmissionRate,
            float configuredSmokeLifetime,
            float configuredSteamLifetime,
            float configuredSmokeStartSize,
            float configuredSteamStartSize,
            float configuredSizeEndMultiplier,
            float configuredSmokeRiseVelocity,
            float configuredSteamRiseVelocity,
            Vector3 configuredSharedWindDirection,
            float configuredReviewWindStrength,
            float configuredNoiseStrength,
            float configuredNoiseFrequency,
            float configuredDistanceCullFarMeters,
            float configuredStrongerOptionMultiplier,
            Color configuredChimneySmokeColor,
            Color configuredCookfireSmokeColor,
            Color configuredSteamColor,
            bool configuredPersistentLoopingShuriken,
            bool configuredSharedAmbientVfxWind,
            bool configuredSoftParticlesRequired,
            bool configuredDistanceCullFarColumns,
            bool configuredConservativeDataPrep,
            bool configuredNeedsTomApproval,
            bool configuredFinalSmokeSteamApproved,
            string configuredRecommendation)
        {
            maxParticlesPerColumn = Mathf.Clamp(configuredMaxParticlesPerColumn, 12, 40);
            chimneyEmissionRate = Mathf.Clamp(configuredChimneyEmissionRate, 0.1f, 18f);
            cookfireEmissionRate = Mathf.Clamp(configuredCookfireEmissionRate, 0.1f, 18f);
            steamEmissionRate = Mathf.Clamp(configuredSteamEmissionRate, 0.1f, 18f);
            smokeLifetime = Mathf.Clamp(configuredSmokeLifetime, 1.2f, 7.5f);
            steamLifetime = Mathf.Clamp(configuredSteamLifetime, 0.8f, 5.5f);
            smokeStartSize = Mathf.Clamp(configuredSmokeStartSize, 0.04f, 0.80f);
            steamStartSize = Mathf.Clamp(configuredSteamStartSize, 0.03f, 0.70f);
            sizeEndMultiplier = Mathf.Clamp(configuredSizeEndMultiplier, 1.0f, 4.0f);
            smokeRiseVelocity = Mathf.Clamp(configuredSmokeRiseVelocity, 0.05f, 1.2f);
            steamRiseVelocity = Mathf.Clamp(configuredSteamRiseVelocity, 0.05f, 1.4f);
            sharedWindDirection = SanitizeDirection(configuredSharedWindDirection);
            reviewWindStrength = Mathf.Clamp(configuredReviewWindStrength, 0f, 1.5f);
            noiseStrength = Mathf.Clamp(configuredNoiseStrength, 0f, 0.8f);
            noiseFrequency = Mathf.Clamp(configuredNoiseFrequency, 0.01f, 1.0f);
            distanceCullFarMeters = Mathf.Clamp(configuredDistanceCullFarMeters, 8f, 80f);
            strongerOptionMultiplier = Mathf.Clamp(configuredStrongerOptionMultiplier, 1f, 2.2f);
            chimneySmokeColor = configuredChimneySmokeColor;
            cookfireSmokeColor = configuredCookfireSmokeColor;
            steamColor = configuredSteamColor;
            persistentLoopingShuriken = configuredPersistentLoopingShuriken;
            sharedAmbientVfxWind = configuredSharedAmbientVfxWind;
            softParticlesRequired = configuredSoftParticlesRequired;
            distanceCullFarColumns = configuredDistanceCullFarColumns;
            conservativeDataPrep = configuredConservativeDataPrep;
            needsTomApproval = configuredNeedsTomApproval;
            finalSmokeSteamApproved = configuredFinalSmokeSteamApproved;
            recommendation = configuredRecommendation ?? string.Empty;
        }

        public float ResolveEmissionRateForReview(FastVsHd2dSmokeSteamKind kind)
        {
            return kind switch
            {
                FastVsHd2dSmokeSteamKind.ChimneySmoke => chimneyEmissionRate,
                FastVsHd2dSmokeSteamKind.CookfireSmoke => cookfireEmissionRate,
                FastVsHd2dSmokeSteamKind.SteamColumn => steamEmissionRate,
                _ => chimneyEmissionRate
            };
        }

        public float ResolveLifetimeForReview(FastVsHd2dSmokeSteamKind kind)
        {
            return kind == FastVsHd2dSmokeSteamKind.SteamColumn ? steamLifetime : smokeLifetime;
        }

        public float ResolveStartSizeForReview(FastVsHd2dSmokeSteamKind kind)
        {
            return kind == FastVsHd2dSmokeSteamKind.SteamColumn ? steamStartSize : smokeStartSize;
        }

        public float ResolveRiseVelocityForReview(FastVsHd2dSmokeSteamKind kind)
        {
            return kind == FastVsHd2dSmokeSteamKind.SteamColumn ? steamRiseVelocity : smokeRiseVelocity;
        }

        public Color ResolveColorForReview(FastVsHd2dSmokeSteamKind kind)
        {
            return kind switch
            {
                FastVsHd2dSmokeSteamKind.CookfireSmoke => cookfireSmokeColor,
                FastVsHd2dSmokeSteamKind.SteamColumn => steamColor,
                _ => chimneySmokeColor
            };
        }

        private static Vector3 SanitizeDirection(Vector3 direction)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = Vector3.right;
            }

            return direction.normalized;
        }
    }
}
