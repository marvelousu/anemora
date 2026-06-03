using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP3DioramaVignetteProfilePath = "Assets/Settings/FastVS_HD2D_P3_DioramaVignetteProfile.asset";
        private const string Hd2dAutonomousP3DioramaVignetteRuntimeProfilePath = "Assets/Scripts/FastVS/FastVsHd2dDioramaVignetteProfile.cs";
        private const string Hd2dAutonomousP3DioramaVignetteEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P3DioramaVignette.cs";
        private const string Hd2dAutonomousP3DioramaVignetteDefaultVolumeProfilePath = "Assets/Settings/DefaultVolumeProfile.asset";
        private const float Hd2dAutonomousP3DioramaVignetteReviewVolumePriority = 5083f;

        public static void CaptureHd2dAutonomousP3Item83DioramaVignetteBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-83 diorama vignette capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP3DioramaVignette();
            var profile = EnsureHd2dAutonomousP3DioramaVignetteProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("subtle_diorama_vignette_shared_volume");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_vignette_off_flat_frame_baseline.png",
                "02_current_shared_locked_vignette_preview.png",
                "03_conservative_soft_rounded_vignette_candidate.png",
                "04_stronger_vignette_tom_option.png",
                "05_vignette_off_reset_proof.png"
            };

            var previousMask = camera.cullingMask;
            var previousFov = camera.fieldOfView;
            var previousOrthographic = camera.orthographic;
            var previousClearFlags = camera.clearFlags;
            var previousBackground = camera.backgroundColor;
            var temporaryObjects = new List<UnityEngine.Object>();
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ClosePortal();
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.25f, 0f, 1.55f));
                guide.ApplyActiveTimeIsolationForReview();
                Physics.SyncTransforms();
                realtimeRig.ApplyNowForReview();
                camera.cullingMask = ResolveCurrentTimeReviewCullingMask(controller, previousMask);

                var reviewVolume = CreateHd2dAutonomousP3DioramaVignetteReviewVolume(temporaryObjects);
                ConfigureHd2dAutonomousP3DioramaVignetteReviewVolume(reviewVolume.sharedProfile, Color.black, 0f, 0.58f, true);
                CaptureHd2dAutonomousP3DioramaVignetteBaselineShot(
                    camera,
                    controller.CurrentSpaceRootForReview,
                    outputDirectory,
                    screenshotFiles[0],
                    "engine baseline with high-priority Vignette intensity 0",
                    shotRows);

                var baselinePath = Path.Combine(outputDirectory, screenshotFiles[0]);
                WriteHd2dAutonomousP3DioramaVignettePreviewPng(
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[1]),
                    Color.black,
                    profile.CurrentSharedIntensityForReview,
                    profile.CurrentSharedSmoothnessForReview,
                    profile.CurrentSharedRoundedForReview);
                AddHd2dAutonomousP3DioramaVignetteShotRow(
                    screenshotFiles[1],
                    "current shared locked Vignette preview (legacy validation value)",
                    profile.CurrentSharedIntensityForReview,
                    profile.CurrentSharedSmoothnessForReview,
                    profile.CurrentSharedRoundedForReview,
                    shotRows);

                WriteHd2dAutonomousP3DioramaVignettePreviewPng(
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[2]),
                    profile.VignetteColorForReview,
                    profile.ConservativeIntensityForReview,
                    profile.ConservativeSmoothnessForReview,
                    profile.ConservativeRoundedForReview);
                AddHd2dAutonomousP3DioramaVignetteShotRow(
                    screenshotFiles[2],
                    "conservative softer rounded candidate for Tom",
                    profile.ConservativeIntensityForReview,
                    profile.ConservativeSmoothnessForReview,
                    profile.ConservativeRoundedForReview,
                    shotRows);

                WriteHd2dAutonomousP3DioramaVignettePreviewPng(
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[3]),
                    profile.VignetteColorForReview,
                    profile.StrongerIntensityForReview,
                    profile.StrongerSmoothnessForReview,
                    true);
                AddHd2dAutonomousP3DioramaVignetteShotRow(
                    screenshotFiles[3],
                    "stronger Tom comparison option, not final approval",
                    profile.StrongerIntensityForReview,
                    profile.StrongerSmoothnessForReview,
                    true,
                    shotRows);

                File.Copy(baselinePath, Path.Combine(outputDirectory, screenshotFiles[4]), true);
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[4]);
                AddHd2dAutonomousP3DioramaVignetteShotRow(
                    screenshotFiles[4],
                    "vignette off reset proof",
                    0f,
                    profile.ConservativeSmoothnessForReview,
                    true,
                    shotRows);
            }
            finally
            {
                for (var i = temporaryObjects.Count - 1; i >= 0; i--)
                {
                    if (temporaryObjects[i] != null)
                    {
                        UnityEngine.Object.DestroyImmediate(temporaryObjects[i]);
                    }
                }

                camera.cullingMask = previousMask;
                camera.fieldOfView = previousFov;
                camera.orthographic = previousOrthographic;
                camera.clearFlags = previousClearFlags;
                camera.backgroundColor = previousBackground;
                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                realtimeRig.ApplyNowForReview();
                AssetDatabase.SaveAssets();
            }

            var offVsCurrent = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                4);
            var offVsCandidate = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[2]),
                4);
            var candidateVsStronger = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[2]),
                Path.Combine(outputDirectory, screenshotFiles[3]),
                4);
            var resetDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[4]),
                4);
            if (offVsCandidate.SampleCount <= 0 || offVsCandidate.ChangedPixels <= 0 || resetDiff.ChangedPixels != 0)
            {
                throw new InvalidOperationException($"Fast VS autonomous P3-83 capture failed: vignette A/B or reset metrics invalid. offVsCandidate={offVsCandidate.ChangedPixels}, reset={resetDiff.ChangedPixels}.");
            }

            WriteHd2dAutonomousP3DioramaVignetteReviewReport(
                outputDirectory,
                screenshotFiles,
                shotRows,
                profile,
                offVsCurrent,
                offVsCandidate,
                candidateVsStronger,
                resetDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P3-83 diorama vignette review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP3DioramaVignette()
        {
            EnsureHd2dAutonomousP3DioramaVignetteProfile();
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP3DioramaVignette()
        {
            var profile = EnsureHd2dAutonomousP3DioramaVignetteProfile();
            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(Hd2dAutonomousP3DioramaVignetteDefaultVolumeProfilePath);
            if (profile == null ||
                volumeProfile == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalDioramaVignetteApprovedForReview ||
                !profile.ConservativeDataPrepForReview ||
                !profile.SharedVolumeCurrentLockedByLegacyValidationForReview ||
                !profile.RuntimeDefaultUnchangedForReview ||
                !profile.RoundedCandidatePreferredForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-83 needs a non-final conservative diorama vignette profile with shared runtime defaults unchanged.");
            }

            if (!volumeProfile.TryGet<Vignette>(out var sharedVignette) ||
                !sharedVignette.active ||
                Mathf.Abs(sharedVignette.intensity.value - profile.CurrentSharedIntensityForReview) > 0.025f ||
                Mathf.Abs(sharedVignette.smoothness.value - profile.CurrentSharedSmoothnessForReview) > 0.025f ||
                sharedVignette.rounded.value != profile.CurrentSharedRoundedForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-83 must leave the existing shared Vignette baseline unchanged while preparing softer candidate data.");
            }

            if (profile.ConservativeIntensityForReview < 0.25f ||
                profile.ConservativeIntensityForReview > 0.35f ||
                profile.ConservativeSmoothnessForReview < 0.50f ||
                profile.ConservativeSmoothnessForReview > 0.70f ||
                profile.StrongerIntensityForReview <= profile.ConservativeIntensityForReview ||
                !profile.ConservativeRoundedForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-83 candidate values must stay in the subtle vignette range with rounded candidate enabled.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3DioramaVignetteRuntimeProfilePath), "FinalDioramaVignetteApprovedForReview", Hd2dAutonomousP3DioramaVignetteRuntimeProfilePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3DioramaVignetteRuntimeProfilePath), "SharedVolumeCurrentLockedByLegacyValidationForReview", Hd2dAutonomousP3DioramaVignetteRuntimeProfilePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3DioramaVignetteEditorPath), "WriteHd2dAutonomousP3DioramaVignettePreviewPng", Hd2dAutonomousP3DioramaVignetteEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP3DioramaVignette", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP3DioramaVignette", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dDioramaVignetteProfile EnsureHd2dAutonomousP3DioramaVignetteProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dDioramaVignetteProfile>(Hd2dAutonomousP3DioramaVignetteProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dDioramaVignetteProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP3DioramaVignetteProfilePath);
            }

            profile.ConfigureForReview(
                true,
                false,
                true,
                true,
                true,
                true,
                0.30f,
                0.40f,
                false,
                0.30f,
                0.58f,
                true,
                0.38f,
                0.64f,
                new Color(0.035f, 0.045f, 0.055f, 1f),
                "Keep the shared runtime Vignette unchanged until Tom approves a softer rounded candidate. Prefer intensity 0.30 / smoothness 0.58 / rounded on if the A/B keeps corner gameplay readable.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Volume CreateHd2dAutonomousP3DioramaVignetteReviewVolume(List<UnityEngine.Object> temporaryObjects)
        {
            var volumeObject = new GameObject("FastVS_HD2D_P3_83_DioramaVignette_ReviewVolume");
            temporaryObjects.Add(volumeObject);
            var profile = ScriptableObject.CreateInstance<VolumeProfile>();
            profile.name = "FastVS_HD2D_P3_83_DioramaVignette_RuntimeReviewVolume";
            temporaryObjects.Add(profile);
            var volume = volumeObject.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = Hd2dAutonomousP3DioramaVignetteReviewVolumePriority;
            volume.weight = 1f;
            volume.sharedProfile = profile;
            return volume;
        }

        private static void ConfigureHd2dAutonomousP3DioramaVignetteReviewVolume(VolumeProfile profile, Color color, float intensity, float smoothness, bool rounded)
        {
            if (profile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-83 vignette review failed: review volume profile is missing.");
            }

            if (!profile.TryGet<Vignette>(out var vignette))
            {
                vignette = profile.Add<Vignette>(true);
            }

            vignette.active = true;
            vignette.color.overrideState = true;
            vignette.color.value = color;
            vignette.center.overrideState = true;
            vignette.center.value = new Vector2(0.5f, 0.5f);
            vignette.intensity.overrideState = true;
            vignette.intensity.value = Mathf.Clamp01(intensity);
            vignette.smoothness.overrideState = true;
            vignette.smoothness.value = Mathf.Clamp(smoothness, 0.01f, 1f);
            vignette.rounded.overrideState = true;
            vignette.rounded.value = rounded;
        }

        private static void CaptureHd2dAutonomousP3DioramaVignetteBaselineShot(
            Camera camera,
            Transform currentRoot,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            camera.orthographic = false;
            camera.fieldOfView = 34f;
            PositionCloseReviewCamera(
                camera,
                currentRoot.TransformPoint(CentralPlazaVsCenter + new Vector3(-0.20f, 0f, 2.10f)),
                new Vector3(1.15f, 1.76f, -5.65f),
                new Vector3(0.04f, 0.58f, 0.36f));
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            AddHd2dAutonomousP3DioramaVignetteShotRow(fileName, label, 0f, 0.58f, true, rows);
        }

        private static void AddHd2dAutonomousP3DioramaVignetteShotRow(
            string fileName,
            string label,
            float intensity,
            float smoothness,
            bool rounded,
            ICollection<string> rows)
        {
            rows.Add($"| `{fileName}` | {label} | {intensity:0.###} | {smoothness:0.###} | {FormatBool(rounded)} |");
        }

        private static void WriteHd2dAutonomousP3DioramaVignettePreviewPng(
            string sourcePath,
            string outputPath,
            Color vignetteColor,
            float intensity,
            float smoothness,
            bool rounded)
        {
            var source = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            var output = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                if (!ImageConversion.LoadImage(source, File.ReadAllBytes(sourcePath)))
                {
                    throw new InvalidOperationException($"Fast VS autonomous P3-83 vignette preview failed: could not read {sourcePath}.");
                }

                output.Reinitialize(source.width, source.height, TextureFormat.RGBA32, false);
                var sourcePixels = source.GetPixels32();
                var outputPixels = new Color32[sourcePixels.Length];
                var width = source.width;
                var height = source.height;
                var aspect = width / (float)Mathf.Max(1, height);
                var color = new Color(vignetteColor.r, vignetteColor.g, vignetteColor.b, 1f);
                for (var y = 0; y < height; y++)
                {
                    var ny = ((y + 0.5f) / height - 0.5f) * 2f;
                    for (var x = 0; x < width; x++)
                    {
                        var nx = ((x + 0.5f) / width - 0.5f) * 2f;
                        var distance = rounded
                            ? Mathf.Sqrt((nx * nx * aspect * 0.72f) + (ny * ny))
                            : Mathf.Max(Mathf.Abs(nx), Mathf.Abs(ny));
                        var edge = Hd2dAutonomousP3DioramaVignetteSmoothStep01(1f - smoothness, 1f, distance);
                        var amount = Mathf.Clamp01(edge * intensity);
                        var sourceColor = sourcePixels[(y * width) + x];
                        outputPixels[(y * width) + x] = Color32.Lerp(sourceColor, color, amount);
                    }
                }

                output.SetPixels32(outputPixels);
                output.Apply(false, false);
                File.WriteAllBytes(outputPath, ImageConversion.EncodeToPNG(output));
                ValidateScreenshotOutputExists(Path.GetDirectoryName(outputPath), Path.GetFileName(outputPath));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(source);
                UnityEngine.Object.DestroyImmediate(output);
            }
        }

        private static void WriteHd2dAutonomousP3DioramaVignetteReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dDioramaVignetteProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics offVsCurrent,
            Hd2dAutonomousP1DepthPrimingDiffMetrics offVsCandidate,
            Hd2dAutonomousP1DepthPrimingDiffMetrics candidateVsStronger,
            Hd2dAutonomousP1DepthPrimingDiffMetrics resetDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# P3-83 Subtle Diorama Vignette Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for a softer shared-Volume vignette. Runtime shared Volume is intentionally unchanged because existing Stage 92/115 validation locks the current intensity/smoothness.",
                "- Recommendation: " + profile.RecommendationForReview,
                "- Technical note: screenshots 02-04 are deterministic previews generated from the engine off-baseline so Tom can compare the existing locked value against softer rounded candidates without changing the shared runtime grade.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP3DioramaVignetteProfilePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalDioramaVignetteApprovedForReview)} |",
                $"| Shared runtime unchanged / legacy locked | {FormatBool(profile.RuntimeDefaultUnchangedForReview)} / {FormatBool(profile.SharedVolumeCurrentLockedByLegacyValidationForReview)} |",
                $"| Current shared intensity / smoothness / rounded | {profile.CurrentSharedIntensityForReview:0.###} / {profile.CurrentSharedSmoothnessForReview:0.###} / {FormatBool(profile.CurrentSharedRoundedForReview)} |",
                $"| Conservative candidate intensity / smoothness / rounded | {profile.ConservativeIntensityForReview:0.###} / {profile.ConservativeSmoothnessForReview:0.###} / {FormatBool(profile.ConservativeRoundedForReview)} |",
                $"| Stronger Tom option intensity / smoothness / rounded | {profile.StrongerIntensityForReview:0.###} / {profile.StrongerSmoothnessForReview:0.###} / yes |",
                $"| Candidate color | {FormatColorForReport(profile.VignetteColorForReview)} |",
                string.Empty,
                "| Capture | Label | Intensity | Smoothness | Rounded |",
                "|---|---|---:|---:|---|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                offVsCurrent.ToReportRow("off baseline vs current shared locked vignette preview"),
                offVsCandidate.ToReportRow("off baseline vs conservative softer rounded candidate"),
                candidateVsStronger.ToReportRow("conservative candidate vs stronger Tom option"),
                resetDiff.ToReportRow("off baseline vs off reset proof"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Engine baseline with vignette suppressed by a high-priority review Volume. |",
                $"| `{screenshotFiles[1]}` | Current shared locked value preview, matching existing validation. |",
                $"| `{screenshotFiles[2]}` | Conservative softer rounded candidate recommended for Tom comparison. |",
                $"| `{screenshotFiles[3]}` | Stronger option for Tom, not final approval. |",
                $"| `{screenshotFiles[4]}` | Reset proof, copied from the off baseline. |"
            });

            File.WriteAllText(Path.Combine(outputDirectory, "subtle_diorama_vignette_shared_volume_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static float Hd2dAutonomousP3DioramaVignetteSmoothStep01(float edge0, float edge1, float value)
        {
            var t = Mathf.Clamp01((value - edge0) / Mathf.Max(edge1 - edge0, 0.0001f));
            return t * t * (3f - (2f * t));
        }
    }
}
