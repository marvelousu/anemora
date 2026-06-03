using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP3TwinklingStarFieldProfilePath = "Assets/Settings/FastVS_HD2D_P3_TwinklingStarFieldProfile.asset";
        private const string Hd2dAutonomousP3TwinklingStarFieldRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dTwinklingStarFieldProfile.cs";
        private const string Hd2dAutonomousP3TwinklingStarFieldDriverRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dGradientSkyDriver.cs";
        private const string Hd2dAutonomousP3TwinklingStarFieldEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P3TwinklingStarField.cs";

        public static void CaptureHd2dAutonomousP3Item78TwinklingStarFieldBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var skyDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include);
            var camera = Camera.main;
            if (skyDriver == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-78 star field capture failed: sky driver or camera missing.");
            }

            ValidateHd2dAutonomousP3TwinklingStarField();
            var profile = EnsureHd2dAutonomousP3TwinklingStarFieldProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("procedural_twinkling_star_field_night_only");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_day_no_stars.png",
                "02_night_stars_time0.png",
                "03_night_stars_time3p5.png",
                "04_night_horizon_mask.png",
            };

            var rows = new List<string>();
            var previousMask = camera.cullingMask;
            var previousClearFlags = camera.clearFlags;
            var previousBackground = camera.backgroundColor;
            try
            {
                camera.cullingMask = 0;
                camera.clearFlags = CameraClearFlags.Skybox;
                SetHd2dAutonomousP1CloudBandRenderersVisible(false);
                CaptureHd2dAutonomousP3StarFieldShot(skyDriver, camera, 62f, 180f, 0f, true, outputDirectory, screenshotFiles[0], "daytime: star layer should be invisible", rows);
                CaptureHd2dAutonomousP3StarFieldShot(skyDriver, camera, -12f, 0f, 0f, true, outputDirectory, screenshotFiles[1], "night: crisp overhead star field at review time 0.0s", rows);
                CaptureHd2dAutonomousP3StarFieldShot(skyDriver, camera, -12f, 0f, 3.5f, true, outputDirectory, screenshotFiles[2], "night: same view at review time 3.5s for twinkle A/B", rows);
                CaptureHd2dAutonomousP3StarFieldShot(skyDriver, camera, -12f, 0f, 1.75f, false, outputDirectory, screenshotFiles[3], "night horizon diagnostic: stars fade before lower hemisphere", rows);
            }
            finally
            {
                camera.cullingMask = previousMask;
                camera.clearFlags = previousClearFlags;
                camera.backgroundColor = previousBackground;
                SetHd2dAutonomousP1CloudBandRenderersVisible(true);
                skyDriver.ApplyReviewStarTimeForReview(0f);
                skyDriver.ApplyReviewSunDirectionForReview(62f, 180f, 0f);
                AssetDatabase.SaveAssets();
            }

            var dayNightDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                4);
            var twinkleDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[1]),
                Path.Combine(outputDirectory, screenshotFiles[2]),
                4);
            if (dayNightDiff.SampleCount <= 0 || dayNightDiff.ChangedPixels <= 0)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-78 capture failed: day/night star field A/B produced no measurable changed pixels.");
            }

            if (twinkleDiff.SampleCount <= 0 || twinkleDiff.ChangedPixels <= 0)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-78 capture failed: night twinkle A/B produced no measurable changed pixels.");
            }

            WriteHd2dAutonomousP3TwinklingStarFieldReviewReport(outputDirectory, screenshotFiles, rows, profile, dayNightDiff, twinkleDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P3-78 twinkling star field review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP3TwinklingStarField()
        {
            var profile = EnsureHd2dAutonomousP3TwinklingStarFieldProfile();
            var driver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include);
            if (driver == null)
            {
                return;
            }

            driver.ConfigureStarFieldForReview(profile);
            EditorUtility.SetDirty(driver);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP3TwinklingStarField()
        {
            var profile = EnsureHd2dAutonomousP3TwinklingStarFieldProfile();
            var driver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include);
            var skyboxMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP1GradientSkyboxMaterialPath);
            if (profile == null ||
                driver == null ||
                skyboxMaterial == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalStarFieldApprovedForReview ||
                profile.EstimatedNightStarCellCountForReview < 250 ||
                profile.HorizonFadeEndForReview <= profile.HorizonFadeStartForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-78 needs a conservative NEEDS-TOM star profile with final approval false and hundreds of estimated stars.");
            }

            driver.ConfigureStarFieldForReview(profile);
            driver.ApplyReviewSunDirectionForReview(62f, 180f, 0f);
            var dayVisibility = driver.LastStarNightVisibilityForReview;
            driver.ApplyReviewSunDirectionForReview(-12f, 0f, 0f);
            var nightVisibility = driver.LastStarNightVisibilityForReview;
            if (dayVisibility > 0.01f ||
                nightVisibility < 0.35f ||
                skyboxMaterial.GetFloat("_StarIntensity") <= 0f ||
                !skyboxMaterial.HasProperty("_StarDensity") ||
                !skyboxMaterial.HasProperty("_StarReviewTime"))
            {
                throw new InvalidOperationException($"House slice validation failed: P3-78 day/night star visibility or material properties are invalid. day={dayVisibility:0.###}, night={nightVisibility:0.###}.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1GradientSkyboxShaderPath), "ProceduralStarField", Hd2dAutonomousP1GradientSkyboxShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1GradientSkyboxShaderPath), "Hash21", Hd2dAutonomousP1GradientSkyboxShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1GradientSkyboxShaderPath), "_StarHorizonFadeEnd", Hd2dAutonomousP1GradientSkyboxShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3TwinklingStarFieldRuntimePath), "FinalStarFieldApprovedForReview", Hd2dAutonomousP3TwinklingStarFieldRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3TwinklingStarFieldDriverRuntimePath), "ConfigureStarFieldForReview", Hd2dAutonomousP3TwinklingStarFieldDriverRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3TwinklingStarFieldDriverRuntimePath), "ApplyReviewStarTimeForReview", Hd2dAutonomousP3TwinklingStarFieldDriverRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP3TwinklingStarField", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP3TwinklingStarField", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dTwinklingStarFieldProfile EnsureHd2dAutonomousP3TwinklingStarFieldProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dTwinklingStarFieldProfile>(Hd2dAutonomousP3TwinklingStarFieldProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dTwinklingStarFieldProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP3TwinklingStarFieldProfilePath);
            }

            profile.ConfigureForReview(
                new Color(0.74f, 0.84f, 1.0f, 1f),
                158f,
                0.974f,
                0.080f,
                1.35f,
                0.58f,
                0.72f,
                0.025f,
                0.30f,
                0.92f,
                0.045f,
                true,
                false);
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static void CaptureHd2dAutonomousP3StarFieldShot(
            FastVsHd2dGradientSkyDriver skyDriver,
            Camera camera,
            float elevationDegrees,
            float azimuthDegrees,
            float starTimeSeconds,
            bool overhead,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            skyDriver.ApplyReviewSunDirectionForReview(elevationDegrees, azimuthDegrees, 0f);
            skyDriver.ApplyReviewStarTimeForReview(starTimeSeconds);
            PositionHd2dAutonomousP3StarFieldReviewCamera(camera, overhead);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {elevationDegrees:0.#} | {skyDriver.LastSunViewHeightForReview:0.###} | {skyDriver.LastStarNightVisibilityForReview:0.###} | {starTimeSeconds:0.###} |");
        }

        private static void PositionHd2dAutonomousP3StarFieldReviewCamera(Camera camera, bool overhead)
        {
            camera.orthographic = false;
            camera.fieldOfView = overhead ? 72f : 60f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 160f;
            camera.transform.position = CentralPlazaVsCenter + new Vector3(0f, 5.0f, -12.0f);
            var lookDirection = overhead
                ? new Vector3(0f, 0.88f, 0.48f).normalized
                : new Vector3(0f, 0.10f, 0.995f).normalized;
            camera.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }

        private static void WriteHd2dAutonomousP3TwinklingStarFieldReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> rows,
            FastVsHd2dTwinklingStarFieldProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics dayNightDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics twinkleDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# P3-78 Procedural Twinkling Star Field Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative procedural star layer in the existing HD2D gradient skybox shader.",
                "- Final sky taste approval remains false; this pass records night-only visibility, twinkle A/B, horizon masking, and retunable profile values.",
                string.Empty,
                "| Setting | Value |",
                "|---|---:|",
                $"| Profile | `{Hd2dAutonomousP3TwinklingStarFieldProfilePath}` |",
                $"| Estimated night star cells | {profile.EstimatedNightStarCellCountForReview} |",
                $"| Density / threshold / point size | {profile.StarDensityForReview:0.#} / {profile.StarThresholdForReview:0.###} / {profile.StarPointSizeForReview:0.###} |",
                $"| Intensity / twinkle strength / speed | {profile.StarIntensityForReview:0.###} / {profile.TwinkleStrengthForReview:0.###} / {profile.TwinkleSpeedForReview:0.###} |",
                $"| Horizon fade start / end | {profile.HorizonFadeStartForReview:0.###} / {profile.HorizonFadeEndForReview:0.###} |",
                $"| Max night opacity / Milky Way intensity | {profile.MaxNightOpacityForReview:0.###} / {profile.MilkyWayIntensityForReview:0.###} |",
                $"| finalStarFieldApproved | {profile.FinalStarFieldApprovedForReview} |",
                string.Empty,
                "| Capture | Purpose | Sun elevation | Sun view height | Star visibility | Star time |",
                "|---|---|---:|---:|---:|---:|",
            };
            lines.AddRange(rows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Metric | Changed Pixels | Changed % | Mean RGB Delta |",
                "|---|---:|---:|---:|",
                $"| Day vs night | {dayNightDiff.ChangedPixels} / {dayNightDiff.SampleCount} | {dayNightDiff.ChangedPercent:0.###}% | {dayNightDiff.MeanRgbDelta:0.###} |",
                $"| Night twinkle time 0.0s vs 3.5s | {twinkleDiff.ChangedPixels} / {twinkleDiff.SampleCount} | {twinkleDiff.ChangedPercent:0.###}% | {twinkleDiff.MeanRgbDelta:0.###} |",
                string.Empty,
                "- Self-review note: this is a single-pass skybox shader layer with no extra draw calls or scene star meshes.",
                "- Tom decision required: final star density, brightness, color, Milky Way band strength, horizon cutoff softness, and twinkle cadence.",
            });
            File.WriteAllText(Path.Combine(outputDirectory, "procedural_twinkling_star_field_night_only_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }
    }
}
