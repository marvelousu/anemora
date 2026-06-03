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
        private const string Hd2dAutonomousP2VegetationTintRootName = "FastVS_HD2D_P2_VegetationTintDriver";
        private const string Hd2dAutonomousP2VegetationTintProfilePath = "Assets/Settings/FastVS_HD2D_P2_VegetationTintProfile.asset";
        private const string Hd2dAutonomousP2VegetationTintRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dVegetationTintDriver.cs";
        private const string Hd2dAutonomousP2VegetationTintProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dVegetationTintProfile.cs";

        public static void CaptureHd2dAutonomousP2Item58VegetationTintBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            var windManager = FindHd2dAutonomousP0VegetationWindManager();
            var tintDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dVegetationTintDriver>(FindObjectsInactive.Include);
            var currentGrass = FindSceneObjectIncludingInactive(Hd2dAutonomousP0GpuGrassCurrentObjectName)?.GetComponent<FastVsHd2dGpuGrassCarpet>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || realtimeRig == null ||
                sunDriver == null || windManager == null || tintDriver == null || currentGrass == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-58 vegetation tint capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2VegetationTint();
            var profile = EnsureHd2dAutonomousP2VegetationTintProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("vegetation_tint_control_plane");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_noon_neutral_control.png",
                "02_noon_lush_control_plane.png",
                "03_noon_withered_sweep.png",
                "04_evening_tod_tint.png",
                "05_night_cool_tod_bias.png",
                "06_noon_to_evening_half_blend.png"
            };
            var shotRows = new List<string>();
            var focus = GetHd2dAutonomousP0GpuGrassLocalPosition(false) + new Vector3(-0.46f, 0.18f, -0.38f);
            var player = focus + new Vector3(-0.46f, -0.14f, -0.54f);
            var cameraOffset = new Vector3(0.46f, 1.28f, -2.34f);
            var lookOffset = new Vector3(0.00f, 0.10f, 0.12f);

            try
            {
                guide.SetMovementFrozen(true);
                SetHd2dAutonomousP0GpuGrassCarpetActive(currentGrass, true);
                CaptureHd2dAutonomousP2VegetationTintShot(
                    controller, visibility, guide, realtimeRig, sunDriver, windManager, tintDriver, camera,
                    SunPreset.Noon, false, false, 0f, 0f, focus, player, cameraOffset, lookOffset,
                    outputDirectory, screenshotFiles[0], "Noon neutral control-plane tint off", shotRows);
                CaptureHd2dAutonomousP2VegetationTintShot(
                    controller, visibility, guide, realtimeRig, sunDriver, windManager, tintDriver, camera,
                    SunPreset.Noon, true, false, 0f, 0.08f, focus, player, cameraOffset, lookOffset,
                    outputDirectory, screenshotFiles[1], "Noon lush conservative tint baseline", shotRows);
                CaptureHd2dAutonomousP2VegetationTintShot(
                    controller, visibility, guide, realtimeRig, sunDriver, windManager, tintDriver, camera,
                    SunPreset.Noon, true, false, 0f, 0.82f, focus, player, cameraOffset, lookOffset,
                    outputDirectory, screenshotFiles[2], "Noon witheredness sweep toward desaturated brown", shotRows);
                CaptureHd2dAutonomousP2VegetationTintShot(
                    controller, visibility, guide, realtimeRig, sunDriver, windManager, tintDriver, camera,
                    SunPreset.Evening, true, false, 0f, 0.16f, focus, player, cameraOffset, lookOffset,
                    outputDirectory, screenshotFiles[3], "Evening warm ToD vegetation tint", shotRows);
                CaptureHd2dAutonomousP2VegetationTintShot(
                    controller, visibility, guide, realtimeRig, sunDriver, windManager, tintDriver, camera,
                    SunPreset.Night, true, false, 0f, 0.18f, focus, player, cameraOffset, lookOffset,
                    outputDirectory, screenshotFiles[4], "Night cool ToD vegetation tint with witheredness bias", shotRows);
                CaptureHd2dAutonomousP2VegetationTintShot(
                    controller, visibility, guide, realtimeRig, sunDriver, windManager, tintDriver, camera,
                    SunPreset.Evening, true, true, 0.5f, 0.12f, focus, player, cameraOffset, lookOffset,
                    outputDirectory, screenshotFiles[5], "Noon-to-evening half blend diagnostic", shotRows);
            }
            finally
            {
                tintDriver.ClearReviewStateForReview();
                windManager.ApplyDefaultReviewStateForReview();
                SetHd2dAutonomousP0GpuGrassCarpetActive(currentGrass, true);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                AssetDatabase.SaveAssets();
            }

            var lushDiff = MeasureHd2dAutonomousP2VegetationTintDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var witheredDiff = MeasureHd2dAutonomousP2VegetationTintDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var eveningDiff = MeasureHd2dAutonomousP2VegetationTintDiff(outputDirectory, screenshotFiles[1], screenshotFiles[3]);
            var nightDiff = MeasureHd2dAutonomousP2VegetationTintDiff(outputDirectory, screenshotFiles[1], screenshotFiles[4]);
            var blendDiff = MeasureHd2dAutonomousP2VegetationTintDiff(outputDirectory, screenshotFiles[1], screenshotFiles[5]);
            WriteHd2dAutonomousP2VegetationTintReviewReport(
                outputDirectory,
                screenshotFiles,
                shotRows,
                profile,
                lushDiff,
                witheredDiff,
                eveningDiff,
                nightDiff,
                blendDiff);

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-58 vegetation tint review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2VegetationTint()
        {
            var profile = EnsureHd2dAutonomousP2VegetationTintProfile();
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2VegetationTintRootName);
            if (root == null)
            {
                root = new GameObject(Hd2dAutonomousP2VegetationTintRootName, typeof(FastVsHd2dVegetationTintDriver));
            }
            else if (root.GetComponent<FastVsHd2dVegetationTintDriver>() == null)
            {
                root.AddComponent<FastVsHd2dVegetationTintDriver>();
            }

            root.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            root.transform.localScale = Vector3.one;
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var tintDriver = root.GetComponent<FastVsHd2dVegetationTintDriver>();
            tintDriver.ConfigureForReview(
                profile,
                FindHd2dAutonomousP0VegetationWindManager(),
                sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : UnityEngine.Object.FindFirstObjectByType<AnemoraSunCycleDriver>(FindObjectsInactive.Include));
            tintDriver.SetReviewActiveForReview(true);

            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(tintDriver);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2VegetationTint()
        {
            var profile = EnsureHd2dAutonomousP2VegetationTintProfile();
            var tintDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dVegetationTintDriver>(FindObjectsInactive.Include);
            var windManager = FindHd2dAutonomousP0VegetationWindManager();
            if (profile == null ||
                tintDriver == null ||
                tintDriver.ProfileForReview != profile ||
                tintDriver.WindManagerForReview == null ||
                tintDriver.SunCycleDriverForReview == null ||
                windManager == null ||
                !tintDriver.PublishEveryFrameForReview ||
                !tintDriver.ConservativeNeedsTomApprovalForReview ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalVegetationTintApprovedForReview ||
                profile.TimeOfDayTintCountForReview != 4)
            {
                throw new InvalidOperationException("House slice validation failed: P2-58 needs one conservative NEEDS-TOM vegetation tint driver wired to the existing sun cycle and vegetation control plane.");
            }

            foreach (SunPreset preset in Enum.GetValues(typeof(SunPreset)))
            {
                if (!profile.TryResolveTimeOfDayTintForReview(preset, out var tint) ||
                    tint.TimeOfDayTint.r <= 0.35f ||
                    tint.TimeOfDayTint.g <= 0.35f ||
                    tint.TimeOfDayTint.b <= 0.35f ||
                    tint.WitherednessBias > 0.24f)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-58 time-of-day vegetation tint is missing or outside conservative bounds: {preset}.");
                }
            }

            if (ColorDistance(profile.LushSeasonTintForReview, profile.WitheredSeasonTintForReview) < 0.22f ||
                profile.DefaultWitherednessForReview > 0.18f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-58 lush/withered tint data must be distinct while the default remains conservative.");
            }

            var noon = profile.ResolveTimeOfDayTintForReview(SunPreset.Noon);
            var evening = profile.ResolveTimeOfDayTintForReview(SunPreset.Evening);
            var night = profile.ResolveTimeOfDayTintForReview(SunPreset.Night);
            if (ColorDistance(noon.TimeOfDayTint, evening.TimeOfDayTint) < 0.14f ||
                ColorDistance(noon.TimeOfDayTint, night.TimeOfDayTint) < 0.22f ||
                night.WitherednessBias <= evening.WitherednessBias)
            {
                throw new InvalidOperationException("House slice validation failed: P2-58 noon/evening/night vegetation tints must be distinct.");
            }

            tintDriver.ApplyPresetForReview(SunPreset.Night, 0.18f);
            if (ColorDistance(windManager.TimeOfDayTintForReview, night.TimeOfDayTint) > 0.005f ||
                Mathf.Abs(windManager.WitherednessForReview - (0.18f + night.WitherednessBias)) > 0.01f ||
                ColorDistance(tintDriver.LastSeasonTintForReview, profile.ResolveSeasonTintForReview(windManager.WitherednessForReview)) > 0.005f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-58 driver did not publish night tint and witheredness through the vegetation wind manager.");
            }

            tintDriver.ApplyBlendForReview(SunPreset.Noon, SunPreset.Evening, 0.5f, 0.12f);
            var half = profile.EvaluateTimeOfDayBlendForReview(SunPreset.Noon, SunPreset.Evening, 0.5f);
            if (ColorDistance(tintDriver.LastTimeOfDayTintForReview, half.TimeOfDayTint) > 0.01f ||
                Mathf.Abs(tintDriver.LastWitherednessForReview - Mathf.Clamp01(0.12f + half.WitherednessBias)) > 0.01f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-58 half-blend diagnostic did not interpolate ToD tint data.");
            }

            tintDriver.ClearReviewStateForReview();

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2VegetationTintRuntimePath), "ApplyPresetForReview", Hd2dAutonomousP2VegetationTintRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2VegetationTintRuntimePath), "ApplyVegetationTintReviewStateForReview", Hd2dAutonomousP2VegetationTintRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2VegetationTintProfileRuntimePath), "VegetationTimeOfDayTint", Hd2dAutonomousP2VegetationTintProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP0VegetationWindManagerPath), "ApplyVegetationTintReviewStateForReview", Hd2dAutonomousP0VegetationWindManagerPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP0VegetationWindSharedHlslPath), "FastVsApplySharedVegetationTint", Hd2dAutonomousP0VegetationWindSharedHlslPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP0GpuGrassShaderPath), "_Witheredness", Hd2dAutonomousP0GpuGrassShaderPath);
            ValidateSourceToken(File.ReadAllText("Assets/Art/Shaders/FastVS/FastVS_FoliageCardLit.shader"), "FastVsApplySharedVegetationTint", "Assets/Art/Shaders/FastVS/FastVS_FoliageCardLit.shader");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2VegetationTint", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2VegetationTint", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dVegetationTintProfile EnsureHd2dAutonomousP2VegetationTintProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dVegetationTintProfile>(Hd2dAutonomousP2VegetationTintProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dVegetationTintProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2VegetationTintProfilePath);
            }

            profile.ConfigureForReview(
                new[]
                {
                    new FastVsHd2dVegetationTintProfile.VegetationTimeOfDayTint(
                        SunPreset.Morning,
                        new Color(1.00f, 0.92f, 0.76f, 1f),
                        0.04f),
                    new FastVsHd2dVegetationTintProfile.VegetationTimeOfDayTint(
                        SunPreset.Noon,
                        new Color(0.94f, 1.00f, 0.88f, 1f),
                        0.00f),
                    new FastVsHd2dVegetationTintProfile.VegetationTimeOfDayTint(
                        SunPreset.Evening,
                        new Color(1.00f, 0.78f, 0.58f, 1f),
                        0.08f),
                    new FastVsHd2dVegetationTintProfile.VegetationTimeOfDayTint(
                        SunPreset.Night,
                        new Color(0.58f, 0.68f, 0.90f, 1f),
                        0.16f)
                },
                new Color(0.96f, 1.00f, 0.90f, 1f),
                new Color(0.82f, 0.68f, 0.42f, 1f),
                0.08f,
                1.8f,
                true,
                true,
                false,
                "Keep P2-58 as conservative vegetation control-plane data prep. Recommendation: use the current lush/withered and ToD tint deltas as a starting point, but Tom should tune final hue, season mapping, and night readability after lighting/ground sign-off.");

            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static void CaptureHd2dAutonomousP2VegetationTintShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            AnemoraSunCycleDriver sunDriver,
            FastVsHd2dVegetationWindManager windManager,
            FastVsHd2dVegetationTintDriver tintDriver,
            Camera camera,
            SunPreset preset,
            bool tintEnabled,
            bool halfBlendDiagnostic,
            float halfBlendT,
            float witheredness,
            Vector3 focusLocal,
            Vector3 playerLocal,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            sunDriver.ApplyPreset(preset, true);
            realtimeRig.ApplyNowForReview();
            windManager.ApplyReviewStateForReview(new Vector3(0.86f, 0f, 0.50f), 0.44f, 0.22f, 0.95f, Color.white, Color.white, 0f);

            if (tintEnabled)
            {
                tintDriver.SetReviewActiveForReview(true);
                if (halfBlendDiagnostic)
                {
                    tintDriver.ApplyBlendForReview(SunPreset.Noon, SunPreset.Evening, halfBlendT, witheredness);
                }
                else
                {
                    tintDriver.ApplyPresetForReview(preset, witheredness);
                }
            }
            else
            {
                tintDriver.SetReviewActiveForReview(false);
            }

            CaptureCloseReviewScreenshotWithoutPlayer(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                playerLocal,
                focusLocal,
                cameraOffset,
                lookOffset,
                outputDirectory,
                fileName);

            var seasonTint = tintEnabled ? tintDriver.LastSeasonTintForReview : Color.white;
            var todTint = tintEnabled ? tintDriver.LastTimeOfDayTintForReview : Color.white;
            var appliedWitheredness = tintEnabled ? tintDriver.LastWitherednessForReview : 0f;
            rows?.Add(
                $"| `{fileName}` | {label} | {preset} | {FormatBool(tintEnabled)} | {FormatBool(halfBlendDiagnostic)} | {FormatColor(seasonTint)} | {FormatColor(todTint)} | {appliedWitheredness:0.###} | {tintDriver.LastBlendTForReview:0.###} |");
        }

        private static void WriteHd2dAutonomousP2VegetationTintReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dVegetationTintProfile profile,
            Hd2dAutonomousP2VegetationTintDiffMetrics lushDiff,
            Hd2dAutonomousP2VegetationTintDiffMetrics witheredDiff,
            Hd2dAutonomousP2VegetationTintDiffMetrics eveningDiff,
            Hd2dAutonomousP2VegetationTintDiffMetrics nightDiff,
            Hd2dAutonomousP2VegetationTintDiffMetrics blendDiff)
        {
            var lines = new List<string>
            {
                "# P2-58 Seasonal / ToD Vegetation Tint Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for lush/withered and time-of-day vegetation tinting through the existing global vegetation control plane.",
                $"- Recommendation: {profile.RecommendationForReview}",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2VegetationTintProfilePath}` |",
                $"| Runtime manager | `{Hd2dAutonomousP2VegetationTintRootName}` |",
                $"| Publish every frame / transition seconds | {FormatBool(profile.PublishEveryFrameForReview)} / {profile.TransitionSecondsForReview:0.###} |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalVegetationTintApprovedForReview)} |",
                $"| Lush tint / withered tint / default witheredness | {FormatColor(profile.LushSeasonTintForReview)} / {FormatColor(profile.WitheredSeasonTintForReview)} / {profile.DefaultWitherednessForReview:0.###} |",
                string.Empty,
                "| Preset | Time-of-day tint | Witheredness bias |",
                "|---|---|---:|"
            };

            foreach (SunPreset preset in Enum.GetValues(typeof(SunPreset)))
            {
                var tint = profile.ResolveTimeOfDayTintForReview(preset);
                lines.Add($"| {preset} | {FormatColor(tint.TimeOfDayTint)} | {tint.WitherednessBias:0.###} |");
            }

            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                lushDiff.ToReportRow("Neutral control vs noon lush tint"),
                witheredDiff.ToReportRow("Noon lush vs noon witheredness sweep"),
                eveningDiff.ToReportRow("Noon lush vs evening ToD tint"),
                nightDiff.ToReportRow("Noon lush vs night cool ToD tint"),
                blendDiff.ToReportRow("Noon lush vs noon-to-evening half blend"),
                string.Empty,
                "| Screenshot | Label | Preset | Tint enabled | Half blend | Season tint | ToD tint | Witheredness | Blend t |",
                "|---|---|---|---|---|---|---|---:|---:|"
            });
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|"
            });

            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[i]);
                lines.Add($"| `{screenshotFiles[i]}` | P2-58 review capture {i + 1} |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "vegetation_tint_control_plane_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP2VegetationTintDiffMetrics MeasureHd2dAutonomousP2VegetationTintDiff(string outputDirectory, string firstFile, string secondFile)
        {
            var firstPath = Path.Combine(outputDirectory, firstFile);
            var secondPath = Path.Combine(outputDirectory, secondFile);
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP2VegetationTintDiffMetrics(0, 0, 0f, 0f);
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
                    if (delta > 4)
                    {
                        changedPixels++;
                    }
                }

                return new Hd2dAutonomousP2VegetationTintDiffMetrics(
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

        private readonly struct Hd2dAutonomousP2VegetationTintDiffMetrics
        {
            public readonly int SampleCount;
            public readonly int ChangedPixels;
            public readonly float ChangedPercent;
            public readonly float MeanRgbDelta;

            public Hd2dAutonomousP2VegetationTintDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
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
