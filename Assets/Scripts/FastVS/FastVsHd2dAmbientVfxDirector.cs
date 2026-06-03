using Anemora.FastVS.SunCycle;
using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/FastVS/HD2D Ambient VFX Director")]
    public sealed class FastVsHd2dAmbientVfxDirector : MonoBehaviour
    {
        private static readonly int AmbientWindId = Shader.PropertyToID("_AnemoraHd2dAmbientVfxWind");
        private static readonly int AmbientDayNightId = Shader.PropertyToID("_AnemoraHd2dAmbientVfxDayNight");
        private static readonly int AmbientZoneParamsId = Shader.PropertyToID("_AnemoraHd2dAmbientVfxZoneParams");
        private static readonly int AmbientCloudDriftId = Shader.PropertyToID("_AnemoraHd2dAmbientVfxCloudDrift");
        private static readonly int AmbientParticleBudgetId = Shader.PropertyToID("_AnemoraHd2dAmbientVfxParticleBudget");

        [SerializeField] private FastVsHd2dAmbientVfxDirectorProfile profile;
        [SerializeField] private FastVsHd2dVegetationWindManager windManager;
        [SerializeField] private AnemoraSunCycleDriver sunCycleDriver;
        [SerializeField] private FastVsHd2dAmbientDustPollenLayer dustLayer;
        [SerializeField] private FastVsHd2dDuskNightFireflyLayer fireflyLayer;
        [SerializeField] private FastVsHd2dFallingLeavesLayer fallingLeavesLayer;
        [SerializeField] private FastVsHd2dGradientSkyDriver skyDriver;
        [SerializeField] private ParticleSystem[] smokeSystems = System.Array.Empty<ParticleSystem>();
        [SerializeField] private string activeZoneId = "central_plaza_green";
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool applyToLayers = true;

        private bool hasReviewState;
        private string reviewZoneId = "central_plaza_green";
        private SunPreset reviewPreset = SunPreset.Noon;
        private Vector3 reviewWindDirection = Vector3.right;
        private float reviewWindStrength = 0.44f;
        private float reviewCloudDriftOffset;
        private float reviewGustPhase;

        public FastVsHd2dAmbientVfxDirectorProfile ProfileForReview => profile;
        public FastVsHd2dVegetationWindManager WindManagerForReview => windManager;
        public AnemoraSunCycleDriver SunCycleDriverForReview => sunCycleDriver;
        public FastVsHd2dAmbientDustPollenLayer DustLayerForReview => dustLayer;
        public FastVsHd2dDuskNightFireflyLayer FireflyLayerForReview => fireflyLayer;
        public FastVsHd2dFallingLeavesLayer FallingLeavesLayerForReview => fallingLeavesLayer;
        public FastVsHd2dGradientSkyDriver SkyDriverForReview => skyDriver;
        public int SmokeSystemCountForReview => smokeSystems != null ? smokeSystems.Length : 0;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public bool ApplyToLayersForReview => applyToLayers;
        public string ActiveZoneIdForReview { get; private set; } = string.Empty;
        public SunPreset ActivePresetForReview { get; private set; } = SunPreset.Noon;
        public Vector3 LastWindVectorForReview { get; private set; }
        public float LastDayNightForReview { get; private set; }
        public float LastDustMultiplierForReview { get; private set; }
        public float LastFireflyMultiplierForReview { get; private set; }
        public float LastLeafMultiplierForReview { get; private set; }
        public float LastSmokeMultiplierForReview { get; private set; }
        public float LastCloudDriftOffsetForReview { get; private set; }
        public int LastParticleBudgetForReview { get; private set; }
        public FastVsHd2dFallingLeavesBiome LastLeafBiomeForReview { get; private set; } = FastVsHd2dFallingLeavesBiome.GreenLeaf;

        private void OnEnable()
        {
            PublishNowForReview();
        }

        private void LateUpdate()
        {
            if (publishEveryFrame)
            {
                PublishNowForReview();
            }
        }

        public void ConfigureForReview(
            FastVsHd2dAmbientVfxDirectorProfile configuredProfile,
            FastVsHd2dVegetationWindManager configuredWindManager,
            AnemoraSunCycleDriver configuredSunCycleDriver,
            FastVsHd2dAmbientDustPollenLayer configuredDustLayer,
            FastVsHd2dDuskNightFireflyLayer configuredFireflyLayer,
            FastVsHd2dFallingLeavesLayer configuredFallingLeavesLayer,
            FastVsHd2dGradientSkyDriver configuredSkyDriver,
            ParticleSystem[] configuredSmokeSystems)
        {
            profile = configuredProfile;
            windManager = configuredWindManager;
            sunCycleDriver = configuredSunCycleDriver;
            dustLayer = configuredDustLayer;
            fireflyLayer = configuredFireflyLayer;
            fallingLeavesLayer = configuredFallingLeavesLayer;
            skyDriver = configuredSkyDriver;
            smokeSystems = configuredSmokeSystems ?? System.Array.Empty<ParticleSystem>();
            PublishNowForReview();
        }

        public void ApplyReviewStateForReview(
            string zoneId,
            SunPreset preset,
            Vector3 windDirection,
            float windStrength,
            float cloudDriftOffset,
            float gustPhase)
        {
            hasReviewState = true;
            reviewZoneId = string.IsNullOrWhiteSpace(zoneId) ? activeZoneId : zoneId;
            reviewPreset = preset;
            reviewWindDirection = SanitizeDirection(windDirection);
            reviewWindStrength = Mathf.Max(0f, windStrength);
            reviewCloudDriftOffset = cloudDriftOffset;
            reviewGustPhase = gustPhase;
            PublishNowForReview();
        }

        public void ClearReviewStateForReview()
        {
            hasReviewState = false;
            PublishNowForReview();
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            PublishNowForReview();
            var duration = Mathf.Max(0f, seconds);
            dustLayer?.SimulateForReview(duration, restart);
            fireflyLayer?.SimulateForReview(duration, restart);
            fallingLeavesLayer?.SimulateForReview(duration, restart);
            if (smokeSystems == null)
            {
                return;
            }

            for (var i = 0; i < smokeSystems.Length; i++)
            {
                if (smokeSystems[i] != null)
                {
                    smokeSystems[i].Simulate(duration, true, restart, false);
                }
            }
        }

        public void PublishNowForReview()
        {
            ResolveReferences();
            if (profile == null)
            {
                return;
            }

            var preset = ResolvePreset();
            var zone = profile.ResolveZoneForReview(hasReviewState ? reviewZoneId : activeZoneId);
            var dayNight = ResolveDayNight(preset);
            var wind = ResolveWindVector();
            var dustMultiplier = Mathf.Lerp(zone.DustDayMultiplier, zone.DustNightMultiplier, dayNight);
            var fireflyMultiplier = Mathf.Lerp(zone.FireflyDayMultiplier, zone.FireflyNightMultiplier, dayNight);
            var leafMultiplier = zone.LeafEmissionMultiplier;
            var smokeMultiplier = zone.SmokeEmissionMultiplier;
            var cloudDrift = (hasReviewState ? reviewCloudDriftOffset : 0f) + (wind.x + wind.z) * 0.035f * zone.CloudDriftMultiplier;

            ActiveZoneIdForReview = zone.ZoneId;
            ActivePresetForReview = preset;
            LastWindVectorForReview = wind;
            LastDayNightForReview = dayNight;
            LastDustMultiplierForReview = dustMultiplier;
            LastFireflyMultiplierForReview = fireflyMultiplier;
            LastLeafMultiplierForReview = leafMultiplier;
            LastSmokeMultiplierForReview = smokeMultiplier;
            LastCloudDriftOffsetForReview = cloudDrift;
            LastParticleBudgetForReview = zone.MaxParticleBudget;
            LastLeafBiomeForReview = zone.LeafBiome;

            if (profile.PublishShaderGlobalsForReview)
            {
                Shader.SetGlobalVector(AmbientWindId, new Vector4(wind.x, wind.y, wind.z, wind.magnitude));
                Shader.SetGlobalFloat(AmbientDayNightId, dayNight);
                Shader.SetGlobalVector(AmbientZoneParamsId, new Vector4(dustMultiplier, fireflyMultiplier, leafMultiplier, smokeMultiplier));
                Shader.SetGlobalFloat(AmbientCloudDriftId, cloudDrift);
                Shader.SetGlobalFloat(AmbientParticleBudgetId, zone.MaxParticleBudget);
            }

            if (!applyToLayers)
            {
                return;
            }

            ApplyLayerState(zone, preset, wind, dustMultiplier, fireflyMultiplier, leafMultiplier, smokeMultiplier, cloudDrift, dayNight);
        }

        private void ApplyLayerState(
            FastVsHd2dAmbientVfxDirectorProfile.ZoneConfig zone,
            SunPreset preset,
            Vector3 wind,
            float dustMultiplier,
            float fireflyMultiplier,
            float leafMultiplier,
            float smokeMultiplier,
            float cloudDrift,
            float dayNight)
        {
            if (profile.DrivesDustForReview && dustLayer != null)
            {
                dustLayer.SetReviewTintWarmthForReview(Mathf.Lerp(0.42f, 0.18f, dayNight));
                dustLayer.SetReviewOverrideForReview(true, dustMultiplier);
                ApplyWindToParticleSystems(dustLayer.GetComponentsInChildren<ParticleSystem>(true), wind * profile.DustWindScaleForReview, false, 1f);
            }

            if (profile.DrivesFirefliesForReview && fireflyLayer != null)
            {
                fireflyLayer.SetReviewPresetForReview(preset, fireflyMultiplier);
                ApplyWindToParticleSystems(fireflyLayer.GetComponentsInChildren<ParticleSystem>(true), wind * profile.FireflyWindScaleForReview, true, 0.35f);
            }

            if (profile.DrivesFallingLeavesForReview && fallingLeavesLayer != null)
            {
                fallingLeavesLayer.SetReviewBiomeForReview(zone.LeafBiome);
                fallingLeavesLayer.SetReviewOverrideForReview(true, leafMultiplier);
                ApplyWindToParticleSystems(fallingLeavesLayer.GetComponentsInChildren<ParticleSystem>(true), wind * profile.LeafWindScaleForReview, true, 1f);
            }

            if (profile.DrivesSmokeForReview && smokeSystems != null)
            {
                ApplySmokeEmission(smokeMultiplier);
                ApplyWindToParticleSystems(smokeSystems, wind * profile.SmokeWindScaleForReview, true, 0.8f);
            }

            if (profile.DrivesCloudDriftForReview && skyDriver != null)
            {
                skyDriver.ApplyAmbientVfxCloudDriftForReview(cloudDrift);
            }
        }

        private void ApplySmokeEmission(float smokeMultiplier)
        {
            if (smokeSystems == null)
            {
                return;
            }

            for (var i = 0; i < smokeSystems.Length; i++)
            {
                var system = smokeSystems[i];
                if (system == null)
                {
                    continue;
                }

                var emission = system.emission;
                emission.enabled = smokeMultiplier > 0.001f;
                emission.rateOverTimeMultiplier = Mathf.Max(0f, smokeMultiplier);
            }
        }

        private static void ApplyWindToParticleSystems(ParticleSystem[] systems, Vector3 windVelocity, bool preserveY, float horizontalScale)
        {
            if (systems == null)
            {
                return;
            }

            for (var i = 0; i < systems.Length; i++)
            {
                var system = systems[i];
                if (system == null)
                {
                    continue;
                }

                var velocity = system.velocityOverLifetime;
                velocity.enabled = true;
                velocity.space = ParticleSystemSimulationSpace.World;
                var yCurve = velocity.y;
                velocity.x = MatchParticleVelocityCurveModeForReview(windVelocity.x * horizontalScale, yCurve, preserveY);
                if (!preserveY)
                {
                    velocity.y = new ParticleSystem.MinMaxCurve(windVelocity.y);
                }

                velocity.z = MatchParticleVelocityCurveModeForReview(windVelocity.z * horizontalScale, yCurve, preserveY);
            }
        }

        private static ParticleSystem.MinMaxCurve MatchParticleVelocityCurveModeForReview(
            float constantValue,
            ParticleSystem.MinMaxCurve yCurve,
            bool preserveY)
        {
            if (preserveY && yCurve.mode == ParticleSystemCurveMode.TwoConstants)
            {
                return new ParticleSystem.MinMaxCurve(constantValue, constantValue);
            }

            return new ParticleSystem.MinMaxCurve(constantValue);
        }

        private void ResolveReferences()
        {
            if (windManager == null)
            {
                windManager = FindFirstObjectByType<FastVsHd2dVegetationWindManager>();
            }

            if (sunCycleDriver == null)
            {
                sunCycleDriver = AnemoraSunCycleDriver.Instance != null ? AnemoraSunCycleDriver.Instance : FindFirstObjectByType<AnemoraSunCycleDriver>();
            }

            if (dustLayer == null)
            {
                dustLayer = FindFirstObjectByType<FastVsHd2dAmbientDustPollenLayer>(FindObjectsInactive.Include);
            }

            if (fireflyLayer == null)
            {
                fireflyLayer = FindFirstObjectByType<FastVsHd2dDuskNightFireflyLayer>(FindObjectsInactive.Include);
            }

            if (fallingLeavesLayer == null)
            {
                fallingLeavesLayer = FindFirstObjectByType<FastVsHd2dFallingLeavesLayer>(FindObjectsInactive.Include);
            }

            if (skyDriver == null)
            {
                skyDriver = FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include);
            }

            if (smokeSystems == null || smokeSystems.Length == 0)
            {
                smokeSystems = ResolveSmokeSystems();
            }
        }

        private static ParticleSystem[] ResolveSmokeSystems()
        {
            var allSystems = FindObjectsByType<ParticleSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var matches = new System.Collections.Generic.List<ParticleSystem>();
            for (var i = 0; i < allSystems.Length; i++)
            {
                if (allSystems[i] != null && allSystems[i].name.IndexOf("Smoke", System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matches.Add(allSystems[i]);
                }
            }

            return matches.ToArray();
        }

        private SunPreset ResolvePreset()
        {
            if (hasReviewState)
            {
                return reviewPreset;
            }

            return sunCycleDriver != null ? sunCycleDriver.CurrentPreset : SunPreset.Noon;
        }

        private Vector3 ResolveWindVector()
        {
            var direction = hasReviewState
                ? reviewWindDirection
                : (windManager != null ? windManager.WindDirectionForReview : profile.DefaultWindDirectionForReview);
            var strength = hasReviewState
                ? reviewWindStrength
                : (windManager != null ? windManager.WindMainForReview : profile.BaseWindMetersPerSecondForReview);
            var phase = hasReviewState ? reviewGustPhase : (Application.isPlaying ? Time.time : 0f);
            var gust = 1f + (Mathf.Sin(phase * profile.GustFrequencyForReview * Mathf.PI * 2f) * profile.GustAmplitudeForReview);
            return SanitizeDirection(direction) * Mathf.Max(0f, strength) * Mathf.Max(0f, gust);
        }

        private static float ResolveDayNight(SunPreset preset)
        {
            switch (preset)
            {
                case SunPreset.Night:
                    return 1f;
                case SunPreset.Evening:
                    return 0.65f;
                case SunPreset.Morning:
                    return 0.18f;
                case SunPreset.Noon:
                default:
                    return 0f;
            }
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
