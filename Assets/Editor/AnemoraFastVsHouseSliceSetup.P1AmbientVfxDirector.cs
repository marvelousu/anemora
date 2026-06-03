using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP1AmbientVfxDirectorName = "FastVS_HD2D_P1_AmbientVFXDirector";
        private const string Hd2dAutonomousP1AmbientVfxDirectorProfilePath = "Assets/Settings/FastVS_HD2D_P1_AmbientVFXDirectorProfile.asset";
        private const string Hd2dAutonomousP1AmbientVfxRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dAmbientVfxDirector.cs";
        private const string Hd2dAutonomousP1AmbientVfxProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dAmbientVfxDirectorProfile.cs";
        private const string Hd2dAutonomousP1AmbientVfxCentralZoneId = "central_plaza_green";
        private const string Hd2dAutonomousP1AmbientVfxPetalZoneId = "library_pink_petals";

        public static void CaptureHd2dAutonomousP1Item48AmbientVfxDirectorBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            var camera = Camera.main;
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientVfxDirector>(FindObjectsInactive.Include);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || sunDriver == null || camera == null || director == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P1-48 ambient VFX director capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1AmbientVfxDirector();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("ambient_vfx_director");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_day_west_wind_green_zone.png",
                "02_day_east_wind_green_zone.png",
                "03_night_firefly_density.png",
                "04_pink_petal_zone_swap.png",
                "05_cloud_weather_drift_offset.png"
            };
            var shotRows = new List<string>();
            try
            {
                guide.SetMovementFrozen(true);

                CaptureHd2dAutonomousP1AmbientVfxDirectorReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    sunDriver,
                    camera,
                    director,
                    Hd2dAutonomousP1AmbientVfxCentralZoneId,
                    SunPreset.Noon,
                    Vector3.right,
                    0.78f,
                    0.03f,
                    0.10f,
                    CentralPlazaVsCenter + new Vector3(0.08f, 0.02f, 3.2f),
                    new Vector3(0.20f, 7.8f, -9.7f),
                    new Vector3(0.00f, 1.18f, 0.15f),
                    36f,
                    8.6f,
                    true,
                    outputDirectory,
                    screenshotFiles[0],
                    "day green zone with eastward shared wind",
                    shotRows);

                CaptureHd2dAutonomousP1AmbientVfxDirectorReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    sunDriver,
                    camera,
                    director,
                    Hd2dAutonomousP1AmbientVfxCentralZoneId,
                    SunPreset.Noon,
                    Vector3.left,
                    0.78f,
                    0.34f,
                    0.62f,
                    CentralPlazaVsCenter + new Vector3(0.08f, 0.02f, 3.2f),
                    new Vector3(0.20f, 7.8f, -9.7f),
                    new Vector3(0.00f, 1.18f, 0.15f),
                    36f,
                    8.6f,
                    true,
                    outputDirectory,
                    screenshotFiles[1],
                    "same zone with westward shared wind redirect",
                    shotRows);

                CaptureHd2dAutonomousP1AmbientVfxDirectorReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    sunDriver,
                    camera,
                    director,
                    Hd2dAutonomousP1AmbientVfxCentralZoneId,
                    SunPreset.Night,
                    new Vector3(0.68f, 0f, 0.36f),
                    0.64f,
                    0.18f,
                    0.35f,
                    CentralPlazaVsCenter + new Vector3(0.08f, 0.02f, 3.2f),
                    new Vector3(0.20f, 7.8f, -9.7f),
                    new Vector3(0.00f, 1.18f, 0.15f),
                    36f,
                    8.6f,
                    true,
                    outputDirectory,
                    screenshotFiles[2],
                    "night raises firefly density and lowers dust",
                    shotRows);

                CaptureHd2dAutonomousP1AmbientVfxDirectorReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    sunDriver,
                    camera,
                    director,
                    Hd2dAutonomousP1AmbientVfxPetalZoneId,
                    SunPreset.Evening,
                    Vector3.left,
                    0.58f,
                    0.24f,
                    0.82f,
                    CentralPlazaVsCenter + new Vector3(-0.16f, 0.02f, 2.55f),
                    new Vector3(0.12f, 5.9f, -7.6f),
                    new Vector3(0.00f, 1.10f, 0.08f),
                    32f,
                    8.6f,
                    true,
                    outputDirectory,
                    screenshotFiles[3],
                    "zone swap changes density and leaf sprite set to pink petals",
                    shotRows);

                CaptureHd2dAutonomousP1AmbientVfxDirectorReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    sunDriver,
                    camera,
                    director,
                    Hd2dAutonomousP1AmbientVfxCentralZoneId,
                    SunPreset.Evening,
                    new Vector3(0.96f, 0f, 0.24f),
                    0.70f,
                    0.52f,
                    0.55f,
                    CentralPlazaVsCenter + new Vector3(0.00f, 0.02f, 1.7f),
                    new Vector3(0.10f, 10.2f, -13.0f),
                    new Vector3(0.00f, 2.25f, 0.25f),
                    42f,
                    6.0f,
                    true,
                    outputDirectory,
                    screenshotFiles[4],
                    "cloud/weather drift offset driven from the same director",
                    shotRows);

                var windMetrics = ValidateHd2dAutonomousP1AmbientVfxDirectorPairDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1], "east-wind-vs-west-wind");
                var todMetrics = ValidateHd2dAutonomousP1AmbientVfxDirectorPairDiff(outputDirectory, screenshotFiles[0], screenshotFiles[2], "day-vs-night-density");
                var zoneMetrics = ValidateHd2dAutonomousP1AmbientVfxDirectorPairDiff(outputDirectory, screenshotFiles[1], screenshotFiles[3], "green-zone-vs-pink-petal-zone");
                WriteHd2dAutonomousP1AmbientVfxDirectorReviewReport(outputDirectory, screenshotFiles, shotRows, windMetrics, todMetrics, zoneMetrics, director);
            }
            finally
            {
                director.ClearReviewStateForReview();
            }

            Debug.Log($"Fast VS autonomous P1-48 ambient VFX director review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1AmbientVfxDirector()
        {
            var profile = EnsureHd2dAutonomousP1AmbientVfxDirectorProfileAsset();
            var directorObject = FindSceneObjectIncludingInactive(Hd2dAutonomousP1AmbientVfxDirectorName);
            if (directorObject == null)
            {
                directorObject = new GameObject(Hd2dAutonomousP1AmbientVfxDirectorName, typeof(FastVsHd2dAmbientVfxDirector));
            }
            else if (directorObject.GetComponent<FastVsHd2dAmbientVfxDirector>() == null)
            {
                directorObject.AddComponent<FastVsHd2dAmbientVfxDirector>();
            }

            directorObject.transform.position = Vector3.zero;
            directorObject.transform.rotation = Quaternion.identity;
            directorObject.transform.localScale = Vector3.one;

            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var director = directorObject.GetComponent<FastVsHd2dAmbientVfxDirector>();
            SerializedSet(director, "activeZoneId", Hd2dAutonomousP1AmbientVfxCentralZoneId);
            SerializedSet(director, "publishEveryFrame", true);
            SerializedSet(director, "applyToLayers", true);
            director.ConfigureForReview(
                profile,
                FindHd2dAutonomousP0VegetationWindManager(),
                sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : UnityEngine.Object.FindFirstObjectByType<AnemoraSunCycleDriver>(FindObjectsInactive.Include),
                UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientDustPollenLayer>(FindObjectsInactive.Include),
                UnityEngine.Object.FindFirstObjectByType<FastVsHd2dDuskNightFireflyLayer>(FindObjectsInactive.Include),
                UnityEngine.Object.FindFirstObjectByType<FastVsHd2dFallingLeavesLayer>(FindObjectsInactive.Include),
                UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include),
                ResolveHd2dAutonomousP1AmbientVfxSmokeSystems());

            EditorUtility.SetDirty(directorObject);
            EditorUtility.SetDirty(director);
        }

        private static void ValidateHd2dAutonomousP1AmbientVfxDirector()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAmbientVfxDirectorProfile>(Hd2dAutonomousP1AmbientVfxDirectorProfilePath);
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientVfxDirector>(FindObjectsInactive.Include);
            if (profile == null ||
                director == null ||
                director.WindManagerForReview == null ||
                director.SunCycleDriverForReview == null ||
                director.DustLayerForReview == null ||
                director.FireflyLayerForReview == null ||
                director.FallingLeavesLayerForReview == null ||
                director.SkyDriverForReview == null ||
                director.SmokeSystemCountForReview <= 0 ||
                !director.PublishEveryFrameForReview ||
                !director.ApplyToLayersForReview ||
                !profile.PublishShaderGlobalsForReview ||
                !profile.DrivesDustForReview ||
                !profile.DrivesSmokeForReview ||
                !profile.DrivesFirefliesForReview ||
                !profile.DrivesFallingLeavesForReview ||
                !profile.DrivesCloudDriftForReview ||
                !profile.ConservativeAutoSafeForReview ||
                profile.ZoneCountForReview < 2)
            {
                throw new InvalidOperationException("House slice validation failed: P1-48 needs one ambient VFX director wired to wind, ToD, dust, smoke, fireflies, leaves, cloud drift, and at least two zone configs.");
            }

            director.ApplyReviewStateForReview(Hd2dAutonomousP1AmbientVfxCentralZoneId, SunPreset.Noon, Vector3.right, 0.78f, 0.03f, 0.10f);
            director.SimulateForReview(0.15f, true);
            var eastWind = Shader.GetGlobalVector("_AnemoraHd2dAmbientVfxWind");
            var eastDust = director.LastDustMultiplierForReview;
            var eastFireflies = director.LastFireflyMultiplierForReview;
            var eastLeafBiome = director.LastLeafBiomeForReview;
            var eastCloud = director.LastCloudDriftOffsetForReview;
            var eastLeafWindX = ReadHd2dAutonomousP1AmbientVfxParticleWindX(director.FallingLeavesLayerForReview.GetComponentInChildren<ParticleSystem>(true));
            var eastDustWindX = ReadHd2dAutonomousP1AmbientVfxParticleWindX(director.DustLayerForReview.GetComponentInChildren<ParticleSystem>(true));
            var eastSmokeWindX = ReadHd2dAutonomousP1AmbientVfxParticleWindX(ResolveHd2dAutonomousP1AmbientVfxSmokeSystems()[0]);

            director.ApplyReviewStateForReview(Hd2dAutonomousP1AmbientVfxPetalZoneId, SunPreset.Night, Vector3.left, 0.84f, 0.34f, 0.62f);
            director.SimulateForReview(0.15f, true);
            var westWind = Shader.GetGlobalVector("_AnemoraHd2dAmbientVfxWind");
            var nightDust = director.LastDustMultiplierForReview;
            var nightFireflies = director.LastFireflyMultiplierForReview;
            var nightLeafBiome = director.LastLeafBiomeForReview;
            var nightCloud = director.LastCloudDriftOffsetForReview;
            var westLeafWindX = ReadHd2dAutonomousP1AmbientVfxParticleWindX(director.FallingLeavesLayerForReview.GetComponentInChildren<ParticleSystem>(true));
            var westDustWindX = ReadHd2dAutonomousP1AmbientVfxParticleWindX(director.DustLayerForReview.GetComponentInChildren<ParticleSystem>(true));
            var westSmokeWindX = ReadHd2dAutonomousP1AmbientVfxParticleWindX(ResolveHd2dAutonomousP1AmbientVfxSmokeSystems()[0]);

            if (eastWind.x < 0.60f ||
                westWind.x > -0.60f ||
                eastLeafWindX <= 0f ||
                eastDustWindX <= 0f ||
                eastSmokeWindX <= 0f ||
                westLeafWindX >= 0f ||
                westDustWindX >= 0f ||
                westSmokeWindX >= 0f ||
                nightDust >= eastDust ||
                nightFireflies <= eastFireflies ||
                eastLeafBiome != FastVsHd2dFallingLeavesBiome.GreenLeaf ||
                nightLeafBiome != FastVsHd2dFallingLeavesBiome.PinkPetal ||
                Mathf.Abs(nightCloud - eastCloud) < 0.05f ||
                director.LastParticleBudgetForReview <= 0)
            {
                throw new InvalidOperationException($"House slice validation failed: P1-48 director must redirect dust/smoke/leaves wind and drive ToD/zone density, eastWind={eastWind}, westWind={westWind}, dust={eastDust:0.###}->{nightDust:0.###}, fireflies={eastFireflies:0.###}->{nightFireflies:0.###}, leaf={eastLeafBiome}->{nightLeafBiome}.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1AmbientVfxRuntimePath), "_AnemoraHd2dAmbientVfxWind", Hd2dAutonomousP1AmbientVfxRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1AmbientVfxRuntimePath), "ApplyReviewStateForReview", Hd2dAutonomousP1AmbientVfxRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1AmbientVfxRuntimePath), "FastVsHd2dAmbientDustPollenLayer", Hd2dAutonomousP1AmbientVfxRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1AmbientVfxProfileRuntimePath), "ZoneConfig", Hd2dAutonomousP1AmbientVfxProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Scripts/FastVS/FastVsHd2dGradientSkyDriver.cs"), "ApplyAmbientVfxCloudDriftForReview", "Assets/Scripts/FastVS/FastVsHd2dGradientSkyDriver.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP1AmbientVfxDirector", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP1AmbientVfxDirector", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dAmbientVfxDirectorProfile EnsureHd2dAutonomousP1AmbientVfxDirectorProfileAsset()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAmbientVfxDirectorProfile>(Hd2dAutonomousP1AmbientVfxDirectorProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dAmbientVfxDirectorProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP1AmbientVfxDirectorProfilePath);
            }

            profile.ConfigureForReview(
                new Vector3(0.86f, 0f, 0.50f),
                0.44f,
                0.18f,
                0.18f,
                0.34f,
                0.42f,
                0.24f,
                0.12f,
                new[]
                {
                    new FastVsHd2dAmbientVfxDirectorProfile.ZoneConfig(
                        Hd2dAutonomousP1AmbientVfxCentralZoneId,
                        "Central Plaza green leaves",
                        1.00f,
                        0.42f,
                        0.00f,
                        1.00f,
                        1.00f,
                        0.86f,
                        1.00f,
                        260,
                        FastVsHd2dFallingLeavesBiome.GreenLeaf),
                    new FastVsHd2dAmbientVfxDirectorProfile.ZoneConfig(
                        Hd2dAutonomousP1AmbientVfxPetalZoneId,
                        "Library pink petal review zone",
                        0.62f,
                        0.30f,
                        0.00f,
                        0.46f,
                        0.68f,
                        0.55f,
                        0.58f,
                        150,
                        FastVsHd2dFallingLeavesBiome.PinkPetal)
                },
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                "Auto-safe P1-48 profile: shared wind, ToD density, zone budgets, and biome sprite data drive dust, smoke, fireflies, leaves, and cloud/weather drift.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static ParticleSystem[] ResolveHd2dAutonomousP1AmbientVfxSmokeSystems()
        {
            var smoke = FindSceneObjectIncludingInactive("FastVS_HD2D_PhaseCAlpha_Smoke");
            if (smoke != null)
            {
                var system = smoke.GetComponent<ParticleSystem>();
                if (system != null)
                {
                    return new[] { system };
                }
            }

            var allSystems = UnityEngine.Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var matches = new List<ParticleSystem>();
            for (var i = 0; i < allSystems.Length; i++)
            {
                if (allSystems[i] != null && allSystems[i].name.IndexOf("Smoke", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matches.Add(allSystems[i]);
                }
            }

            return matches.ToArray();
        }

        private static float ReadHd2dAutonomousP1AmbientVfxParticleWindX(ParticleSystem system)
        {
            if (system == null)
            {
                return 0f;
            }

            return system.velocityOverLifetime.x.constant;
        }

        private static void CaptureHd2dAutonomousP1AmbientVfxDirectorReviewShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            AnemoraSunCycleDriver sunDriver,
            Camera camera,
            FastVsHd2dAmbientVfxDirector director,
            string zoneId,
            SunPreset preset,
            Vector3 windDirection,
            float windStrength,
            float cloudDriftOffset,
            float gustPhase,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            float simulateSeconds,
            bool restart,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(anchorLocalPosition + new Vector3(0f, 0.02f, -0.8f));
            guide.ApplyActiveTimeIsolationForReview();
            sunDriver.ApplyPreset(preset, true);
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            director.ApplyReviewStateForReview(zoneId, preset, windDirection, windStrength, cloudDriftOffset, gustPhase);
            director.SimulateForReview(simulateSeconds, restart);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {zoneId} | {preset} | {FormatVector3ForReport(director.LastWindVectorForReview)} | {director.LastDayNightForReview:0.###} | {director.LastDustMultiplierForReview:0.###} | {director.LastFireflyMultiplierForReview:0.###} | {director.LastLeafMultiplierForReview:0.###} | {director.LastSmokeMultiplierForReview:0.###} | {director.LastLeafBiomeForReview} | {director.LastCloudDriftOffsetForReview:0.###} |");
        }

        private static void WriteHd2dAutonomousP1AmbientVfxDirectorReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics windMetrics,
            Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics todMetrics,
            Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics zoneMetrics,
            FastVsHd2dAmbientVfxDirector director)
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAmbientVfxDirectorProfile>(Hd2dAutonomousP1AmbientVfxDirectorProfilePath);
            var lines = new List<string>
            {
                "# P1-48 Ambient VFX Director Review",
                string.Empty,
                "- Scope: auto-safe shared wind, ToD density, zone budget, biome sprite, and cloud/weather drift director for ambient VFX.",
                "- Recommendation: keep the director/profile as the single lightweight data source over existing ambient VFX layers; future VFX should consume this contract rather than hand-editing per-system wind and density.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP1AmbientVfxDirectorProfilePath}` |",
                $"| Root | `{Hd2dAutonomousP1AmbientVfxDirectorName}` |",
                $"| Zone count | {profile?.ZoneCountForReview ?? 0} |",
                $"| Wind manager linked | {FormatBool(director != null && director.WindManagerForReview != null)} |",
                $"| Dust / smoke / fireflies / leaves / clouds | {FormatBool(director != null && director.DustLayerForReview != null)} / {director?.SmokeSystemCountForReview ?? 0} / {FormatBool(director != null && director.FireflyLayerForReview != null)} / {FormatBool(director != null && director.FallingLeavesLayerForReview != null)} / {FormatBool(director != null && director.SkyDriverForReview != null)} |",
                $"| Shader globals | `_AnemoraHd2dAmbientVfxWind`, `_AnemoraHd2dAmbientVfxDayNight`, `_AnemoraHd2dAmbientVfxZoneParams`, `_AnemoraHd2dAmbientVfxCloudDrift`, `_AnemoraHd2dAmbientVfxParticleBudget` |",
                $"| Source note | {profile?.SourceNoteForReview ?? "missing"} |",
                string.Empty,
                "| Comparison | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                windMetrics.ToReportRow("east shared wind vs west shared wind"),
                todMetrics.ToReportRow("day density vs night density"),
                zoneMetrics.ToReportRow("green zone vs pink petal zone"),
                string.Empty,
                "| Screenshot | Label | Zone | Preset | Wind | DayNight | Dust x | Firefly x | Leaf x | Smoke x | Leaf biome | Cloud drift |",
                "|---|---|---|---|---|---:|---:|---:|---:|---:|---|---:|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Day/green zone with one wind direction |",
                $"| `{screenshotFiles[1]}` | Same zone with wind direction flipped for dust/smoke/leaves |",
                $"| `{screenshotFiles[2]}` | Night raises firefly density and lowers dust multiplier from the same ToD signal |",
                $"| `{screenshotFiles[3]}` | Zone swap changes density and leaf sprite set to pink petals |",
                $"| `{screenshotFiles[4]}` | Cloud/weather drift offset uses the same director wind path |"
            });

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllText(Path.Combine(outputDirectory, "ambient_vfx_director_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics ValidateHd2dAutonomousP1AmbientVfxDirectorPairDiff(string outputDirectory, string firstFile, string secondFile, string label)
        {
            var metrics = MeasureHd2dAutonomousP1AmbientVfxDirectorDiff(Path.Combine(outputDirectory, firstFile), Path.Combine(outputDirectory, secondFile), 4);
            if (metrics.SampleCount <= 0 || metrics.ChangedPixels <= 0)
            {
                throw new InvalidOperationException($"Fast VS autonomous P1-48 ambient VFX director capture failed: {label} images have no measurable difference.");
            }

            return metrics;
        }

        private static Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics MeasureHd2dAutonomousP1AmbientVfxDirectorDiff(string firstPath, string secondPath, int threshold)
        {
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics(0, 0, 0f, 0f);
                }

                var firstPixels = firstTexture.GetPixels32();
                var secondPixels = secondTexture.GetPixels32();
                var sampleCount = Mathf.Min(firstPixels.Length, secondPixels.Length);
                var changedPixels = 0;
                var totalDelta = 0f;
                for (var i = 0; i < sampleCount; i++)
                {
                    var delta =
                        Mathf.Abs(firstPixels[i].r - secondPixels[i].r) +
                        Mathf.Abs(firstPixels[i].g - secondPixels[i].g) +
                        Mathf.Abs(firstPixels[i].b - secondPixels[i].b);
                    totalDelta += delta / 3f;
                    if (delta > threshold)
                    {
                        changedPixels++;
                    }
                }

                return new Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics(
                    sampleCount,
                    changedPixels,
                    sampleCount > 0 ? changedPixels * 100f / sampleCount : 0f,
                    sampleCount > 0 ? totalDelta / sampleCount : 0f);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(firstTexture);
                UnityEngine.Object.DestroyImmediate(secondTexture);
            }
        }

        private readonly struct Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics
        {
            public readonly int SampleCount;
            public readonly int ChangedPixels;
            public readonly float ChangedPercent;
            public readonly float MeanRgbDelta;

            public Hd2dAutonomousP1AmbientVfxDirectorDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
            {
                SampleCount = sampleCount;
                ChangedPixels = changedPixels;
                ChangedPercent = changedPercent;
                MeanRgbDelta = meanRgbDelta;
            }

            public string ToReportRow(string label)
            {
                return $"| {label} | {SampleCount} | {ChangedPixels} | {ChangedPercent:0.###} | {MeanRgbDelta:0.###} |";
            }
        }
    }
}
