using System;
using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Ambient VFX Director Profile")]
    public sealed class FastVsHd2dAmbientVfxDirectorProfile : ScriptableObject
    {
        [SerializeField] private Vector3 defaultWindDirection = new Vector3(0.86f, 0f, 0.50f);
        [SerializeField, Min(0f)] private float baseWindMetersPerSecond = 0.44f;
        [SerializeField, Range(0f, 1f)] private float gustAmplitude = 0.18f;
        [SerializeField, Min(0.01f)] private float gustFrequency = 0.18f;
        [SerializeField, Min(0f)] private float dustWindScale = 0.34f;
        [SerializeField, Min(0f)] private float leafWindScale = 0.42f;
        [SerializeField, Min(0f)] private float smokeWindScale = 0.24f;
        [SerializeField, Min(0f)] private float fireflyWindScale = 0.12f;
        [SerializeField] private ZoneConfig[] zones = Array.Empty<ZoneConfig>();
        [SerializeField] private bool publishShaderGlobals = true;
        [SerializeField] private bool drivesDust = true;
        [SerializeField] private bool drivesSmoke = true;
        [SerializeField] private bool drivesFireflies = true;
        [SerializeField] private bool drivesFallingLeaves = true;
        [SerializeField] private bool drivesCloudDrift = true;
        [SerializeField] private bool conservativeAutoSafe = true;
        [SerializeField] private string sourceNote = "Auto-safe P1-48 director profile: one wind/ToD/zone data source drives ambient VFX density and drift.";

        public Vector3 DefaultWindDirectionForReview => SanitizeDirection(defaultWindDirection);
        public float BaseWindMetersPerSecondForReview => baseWindMetersPerSecond;
        public float GustAmplitudeForReview => gustAmplitude;
        public float GustFrequencyForReview => gustFrequency;
        public float DustWindScaleForReview => dustWindScale;
        public float LeafWindScaleForReview => leafWindScale;
        public float SmokeWindScaleForReview => smokeWindScale;
        public float FireflyWindScaleForReview => fireflyWindScale;
        public int ZoneCountForReview => zones != null ? zones.Length : 0;
        public bool PublishShaderGlobalsForReview => publishShaderGlobals;
        public bool DrivesDustForReview => drivesDust;
        public bool DrivesSmokeForReview => drivesSmoke;
        public bool DrivesFirefliesForReview => drivesFireflies;
        public bool DrivesFallingLeavesForReview => drivesFallingLeaves;
        public bool DrivesCloudDriftForReview => drivesCloudDrift;
        public bool ConservativeAutoSafeForReview => conservativeAutoSafe;
        public string SourceNoteForReview => sourceNote;

        public ZoneConfig ResolveZoneForReview(string zoneId)
        {
            if (zones != null)
            {
                for (var i = 0; i < zones.Length; i++)
                {
                    if (string.Equals(zones[i].ZoneId, zoneId, StringComparison.Ordinal))
                    {
                        return zones[i];
                    }
                }

                if (zones.Length > 0)
                {
                    return zones[0];
                }
            }

            return ZoneConfig.Default;
        }

        public void ConfigureForReview(
            Vector3 configuredDefaultWindDirection,
            float configuredBaseWindMetersPerSecond,
            float configuredGustAmplitude,
            float configuredGustFrequency,
            float configuredDustWindScale,
            float configuredLeafWindScale,
            float configuredSmokeWindScale,
            float configuredFireflyWindScale,
            ZoneConfig[] configuredZones,
            bool configuredPublishShaderGlobals,
            bool configuredDrivesDust,
            bool configuredDrivesSmoke,
            bool configuredDrivesFireflies,
            bool configuredDrivesFallingLeaves,
            bool configuredDrivesCloudDrift,
            bool configuredConservativeAutoSafe,
            string configuredSourceNote)
        {
            defaultWindDirection = SanitizeDirection(configuredDefaultWindDirection);
            baseWindMetersPerSecond = Mathf.Max(0f, configuredBaseWindMetersPerSecond);
            gustAmplitude = Mathf.Clamp01(configuredGustAmplitude);
            gustFrequency = Mathf.Max(0.01f, configuredGustFrequency);
            dustWindScale = Mathf.Max(0f, configuredDustWindScale);
            leafWindScale = Mathf.Max(0f, configuredLeafWindScale);
            smokeWindScale = Mathf.Max(0f, configuredSmokeWindScale);
            fireflyWindScale = Mathf.Max(0f, configuredFireflyWindScale);
            zones = configuredZones ?? Array.Empty<ZoneConfig>();
            publishShaderGlobals = configuredPublishShaderGlobals;
            drivesDust = configuredDrivesDust;
            drivesSmoke = configuredDrivesSmoke;
            drivesFireflies = configuredDrivesFireflies;
            drivesFallingLeaves = configuredDrivesFallingLeaves;
            drivesCloudDrift = configuredDrivesCloudDrift;
            conservativeAutoSafe = configuredConservativeAutoSafe;
            sourceNote = configuredSourceNote ?? string.Empty;
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

        [Serializable]
        public struct ZoneConfig
        {
            [SerializeField] private string zoneId;
            [SerializeField] private string label;
            [SerializeField, Min(0f)] private float dustDayMultiplier;
            [SerializeField, Min(0f)] private float dustNightMultiplier;
            [SerializeField, Min(0f)] private float fireflyDayMultiplier;
            [SerializeField, Min(0f)] private float fireflyNightMultiplier;
            [SerializeField, Min(0f)] private float leafEmissionMultiplier;
            [SerializeField, Min(0f)] private float smokeEmissionMultiplier;
            [SerializeField, Min(0f)] private float cloudDriftMultiplier;
            [SerializeField, Min(0)] private int maxParticleBudget;
            [SerializeField] private FastVsHd2dFallingLeavesBiome leafBiome;

            public ZoneConfig(
                string zoneId,
                string label,
                float dustDayMultiplier,
                float dustNightMultiplier,
                float fireflyDayMultiplier,
                float fireflyNightMultiplier,
                float leafEmissionMultiplier,
                float smokeEmissionMultiplier,
                float cloudDriftMultiplier,
                int maxParticleBudget,
                FastVsHd2dFallingLeavesBiome leafBiome)
            {
                this.zoneId = string.IsNullOrWhiteSpace(zoneId) ? "default" : zoneId;
                this.label = label ?? string.Empty;
                this.dustDayMultiplier = Mathf.Max(0f, dustDayMultiplier);
                this.dustNightMultiplier = Mathf.Max(0f, dustNightMultiplier);
                this.fireflyDayMultiplier = Mathf.Max(0f, fireflyDayMultiplier);
                this.fireflyNightMultiplier = Mathf.Max(0f, fireflyNightMultiplier);
                this.leafEmissionMultiplier = Mathf.Max(0f, leafEmissionMultiplier);
                this.smokeEmissionMultiplier = Mathf.Max(0f, smokeEmissionMultiplier);
                this.cloudDriftMultiplier = Mathf.Max(0f, cloudDriftMultiplier);
                this.maxParticleBudget = Mathf.Max(0, maxParticleBudget);
                this.leafBiome = leafBiome;
            }

            public static ZoneConfig Default => new ZoneConfig(
                "default",
                "Default",
                1f,
                0.45f,
                0f,
                1f,
                1f,
                1f,
                1f,
                220,
                FastVsHd2dFallingLeavesBiome.GreenLeaf);

            public string ZoneId => string.IsNullOrWhiteSpace(zoneId) ? "default" : zoneId;
            public string Label => label ?? string.Empty;
            public float DustDayMultiplier => dustDayMultiplier;
            public float DustNightMultiplier => dustNightMultiplier;
            public float FireflyDayMultiplier => fireflyDayMultiplier;
            public float FireflyNightMultiplier => fireflyNightMultiplier;
            public float LeafEmissionMultiplier => leafEmissionMultiplier;
            public float SmokeEmissionMultiplier => smokeEmissionMultiplier;
            public float CloudDriftMultiplier => cloudDriftMultiplier;
            public int MaxParticleBudget => maxParticleBudget;
            public FastVsHd2dFallingLeavesBiome LeafBiome => leafBiome;
        }
    }
}
