using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP3UnderwaterWaterlineStateName = "FastVS_HD2D_P3_84_UnderwaterWaterlineState";
        private const string Hd2dAutonomousP3UnderwaterWaterlineProfilePath = "Assets/Settings/FastVS_HD2D_P3_UnderwaterWaterlineProfile.asset";
        private const string Hd2dAutonomousP3UnderwaterWaterlineMaterialPath = "Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_p3_84_underwater_waterline.mat";
        private const string Hd2dAutonomousP3UnderwaterWaterlineShaderPath = "Assets/Art/Shaders/FastVS/FastVS_UnderwaterWaterlineFullscreen.shader";
        private const string Hd2dAutonomousP3UnderwaterWaterlineShaderName = "Anemora/FastVS/UnderwaterWaterlineFullscreen";
        private const string Hd2dAutonomousP3UnderwaterWaterlineProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dUnderwaterWaterlineProfile.cs";
        private const string Hd2dAutonomousP3UnderwaterWaterlineStateRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dUnderwaterWaterlineState.cs";
        private const string Hd2dAutonomousP3UnderwaterWaterlineEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P3UnderwaterWaterline.cs";

        public static void CaptureHd2dAutonomousP3Item84UnderwaterWaterlineBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-84 underwater waterline capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP3UnderwaterWaterline();
            var profile = EnsureHd2dAutonomousP3UnderwaterWaterlineProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("underwater_waterline_screen_tint_data");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_above_water_untinted_baseline.png",
                "02_below_water_conservative_tint_fog_waterline.png",
                "03_below_water_conservative_distortion_frame_b.png",
                "04_below_water_stronger_tom_option.png",
                "05_above_water_reset_untinted.png"
            };
            var shotRows = new List<string>();

            var previousMask = camera.cullingMask;
            var previousFov = camera.fieldOfView;
            var previousOrthographic = camera.orthographic;
            var previousClearFlags = camera.clearFlags;
            var previousBackground = camera.backgroundColor;
            var player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            var hasPlayer = player != null && controller.CurrentSpaceRootForReview != null;
            var previousPlayerLocal = hasPlayer
                ? controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position)
                : Vector3.zero;
            var fakeRefractionProfile = EnsureHd2dAutonomousP2FakeRefractionProfile();
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();

            try
            {
                guide.SetMovementFrozen(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ClosePortal();
                controller.ForcePlayerOtherTimeLocalForReview(CentralPlazaVsCenter + new Vector3(0.82f, 0.02f, 1.64f));
                guide.ApplyActiveTimeIsolationForReview();
                Physics.SyncTransforms();

                var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
                var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
                var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
                camera.cullingMask = ((previousMask & ~currentBit) | otherBit) & ~playerBit;
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(false);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, fakeRefractionProfile, true, 0f);

                CaptureHd2dAutonomousP3UnderwaterWaterlineBaselineShot(
                    camera,
                    controller.OtherTimeSpaceRootForReview,
                    outputDirectory,
                    screenshotFiles[0],
                    "above-water untinted engine baseline",
                    shotRows);

                var baselinePath = Path.Combine(outputDirectory, screenshotFiles[0]);
                WriteHd2dAutonomousP3UnderwaterWaterlinePreviewPng(
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[1]),
                    profile,
                    false,
                    0f);
                AddHd2dAutonomousP3UnderwaterWaterlineShotRow(
                    screenshotFiles[1],
                    "below-water conservative tint + fog + faint waterline preview",
                    profile.ConservativeTintBlendForReview,
                    profile.ConservativeDepthFogDensityForReview,
                    profile.ConservativeDistortionPixelsForReview,
                    shotRows);

                WriteHd2dAutonomousP3UnderwaterWaterlinePreviewPng(
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[2]),
                    profile,
                    false,
                    0.58f);
                AddHd2dAutonomousP3UnderwaterWaterlineShotRow(
                    screenshotFiles[2],
                    "below-water conservative preview frame B with scrolling distortion phase",
                    profile.ConservativeTintBlendForReview,
                    profile.ConservativeDepthFogDensityForReview,
                    profile.ConservativeDistortionPixelsForReview,
                    shotRows);

                WriteHd2dAutonomousP3UnderwaterWaterlinePreviewPng(
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[3]),
                    profile,
                    true,
                    0.58f);
                AddHd2dAutonomousP3UnderwaterWaterlineShotRow(
                    screenshotFiles[3],
                    "stronger underwater Tom comparison option, not final approval",
                    profile.StrongerTintBlendForReview,
                    profile.StrongerDepthFogDensityForReview,
                    profile.StrongerDistortionPixelsForReview,
                    shotRows);

                File.Copy(baselinePath, Path.Combine(outputDirectory, screenshotFiles[4]), true);
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[4]);
                AddHd2dAutonomousP3UnderwaterWaterlineShotRow(
                    screenshotFiles[4],
                    "above-water reset proof copied from untinted baseline",
                    0f,
                    0f,
                    0f,
                    shotRows);
            }
            finally
            {
                FastVsHd2dUnderwaterWaterlineState.ClearGlobalsForReview();
                camera.cullingMask = previousMask;
                camera.fieldOfView = previousFov;
                camera.orthographic = previousOrthographic;
                camera.clearFlags = previousClearFlags;
                camera.backgroundColor = previousBackground;
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, fakeRefractionProfile, true, 0f);
                if (hasPlayer)
                {
                    controller.ForcePlayerCurrentLocalForReview(previousPlayerLocal);
                }

                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                AssetDatabase.SaveAssets();
            }

            var aboveVsConservative = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                4);
            var conservativeMotion = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[1]),
                Path.Combine(outputDirectory, screenshotFiles[2]),
                4);
            var conservativeVsStronger = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[1]),
                Path.Combine(outputDirectory, screenshotFiles[3]),
                4);
            var resetDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[4]),
                4);
            if (aboveVsConservative.SampleCount <= 0 || aboveVsConservative.ChangedPixels <= 0 || resetDiff.ChangedPixels != 0)
            {
                throw new InvalidOperationException($"Fast VS autonomous P3-84 capture failed: underwater A/B or reset metrics invalid. aboveVsConservative={aboveVsConservative.ChangedPixels}, reset={resetDiff.ChangedPixels}.");
            }

            WriteHd2dAutonomousP3UnderwaterWaterlineReviewReport(
                outputDirectory,
                screenshotFiles,
                shotRows,
                profile,
                aboveVsConservative,
                conservativeMotion,
                conservativeVsStronger,
                resetDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P3-84 underwater waterline review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP3UnderwaterWaterline(Camera camera)
        {
            var profile = EnsureHd2dAutonomousP3UnderwaterWaterlineProfile();
            EnsureHd2dAutonomousP3UnderwaterWaterlineMaterial(profile);
            var stateObject = FindSceneObjectIncludingInactive(Hd2dAutonomousP3UnderwaterWaterlineStateName);
            if (stateObject == null)
            {
                stateObject = new GameObject(Hd2dAutonomousP3UnderwaterWaterlineStateName);
            }

            if (camera != null)
            {
                stateObject.transform.SetParent(camera.transform, false);
                stateObject.transform.localPosition = Vector3.zero;
                stateObject.transform.localRotation = Quaternion.identity;
                stateObject.transform.localScale = Vector3.one;
            }

            var state = stateObject.GetComponent<FastVsHd2dUnderwaterWaterlineState>();
            if (state == null)
            {
                state = stateObject.AddComponent<FastVsHd2dUnderwaterWaterlineState>();
            }

            var waterSurface = FindSceneObjectIncludingInactive("P2_61_ReviewPond_WaterSurface");
            state.ConfigureForReview(profile, camera, waterSurface != null ? waterSurface.transform : null);
            EditorUtility.SetDirty(stateObject);
            EditorUtility.SetDirty(state);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP3UnderwaterWaterline()
        {
            var profile = EnsureHd2dAutonomousP3UnderwaterWaterlineProfile();
            var material = EnsureHd2dAutonomousP3UnderwaterWaterlineMaterial(profile);
            var state = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dUnderwaterWaterlineState>();
            if (profile == null ||
                material == null ||
                material.shader == null ||
                !string.Equals(material.shader.name, Hd2dAutonomousP3UnderwaterWaterlineShaderName, StringComparison.Ordinal) ||
                state == null ||
                !state.IsReadyForReview ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalUnderwaterWaterlineApprovedForReview ||
                !profile.ConservativeDataPrepForReview ||
                !profile.OnlyRelevantWhenCameraSubmergesForReview ||
                !profile.RuntimeDefaultDisabledForReview ||
                !profile.FullScreenShaderMaterialPreparedForReview ||
                !profile.RendererFeatureDeferredUntilApprovalForReview ||
                !profile.HeightTriggerPreparedForReview ||
                !state.RuntimeDefaultDisabledForReview ||
                !state.EffectLockedUntilTomApprovalForReview ||
                state.ActiveBlendForReview > 0.001f)
            {
                throw new InvalidOperationException("House slice validation failed: P3-84 needs a locked, non-final underwater waterline profile/state/material with runtime effect disabled.");
            }

            if (profile.ConservativeTintBlendForReview < 0.20f ||
                profile.ConservativeTintBlendForReview > 0.42f ||
                profile.ConservativeDepthFogDensityForReview < 0.25f ||
                profile.ConservativeDepthFogDensityForReview > 0.55f ||
                profile.ConservativeDistortionPixelsForReview <= 0.25f ||
                profile.ConservativeDistortionPixelsForReview > 2.25f ||
                profile.StrongerTintBlendForReview <= profile.ConservativeTintBlendForReview ||
                profile.StrongerDistortionPixelsForReview <= profile.ConservativeDistortionPixelsForReview ||
                profile.NormalizedWaterlineYForReview < 0.45f ||
                profile.NormalizedWaterlineYForReview > 0.72f)
            {
                throw new InvalidOperationException("House slice validation failed: P3-84 conservative/stronger underwater values are outside the safe Tom-facing range.");
            }

            if (CountHd2dAutonomousP2FakeRefractionWaterSurfaces() < 1 ||
                CountHd2dAutonomousP2FakeRefractionSubmergedPatternRenderers() < 5)
            {
                throw new InvalidOperationException("House slice validation failed: P3-84 expects the existing P2-61 water review pond and submerged pattern for A/B capture.");
            }

            if (!material.HasProperty("_LocalIntensity"))
            {
                throw new InvalidOperationException("House slice validation failed: P3-84 underwater material must expose _LocalIntensity.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3UnderwaterWaterlineProfileRuntimePath), "RendererFeatureDeferredUntilApprovalForReview", Hd2dAutonomousP3UnderwaterWaterlineProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3UnderwaterWaterlineStateRuntimePath), "ClearGlobalsForReview", Hd2dAutonomousP3UnderwaterWaterlineStateRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3UnderwaterWaterlineStateRuntimePath), "_AnemoraHd2dUnderwaterBlend", Hd2dAutonomousP3UnderwaterWaterlineStateRuntimePath);
            var shaderSource = File.ReadAllText(Hd2dAutonomousP3UnderwaterWaterlineShaderPath);
            ValidateSourceToken(shaderSource, "UnderwaterWaterlineFullscreen", Hd2dAutonomousP3UnderwaterWaterlineShaderPath);
            ValidateSourceToken(shaderSource, "SampleSceneDepth", Hd2dAutonomousP3UnderwaterWaterlineShaderPath);
            ValidateSourceToken(shaderSource, "_AnemoraHd2dUnderwaterFogDistortion", Hd2dAutonomousP3UnderwaterWaterlineShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3UnderwaterWaterlineEditorPath), "WriteHd2dAutonomousP3UnderwaterWaterlinePreviewPng", Hd2dAutonomousP3UnderwaterWaterlineEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP3UnderwaterWaterline", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP3UnderwaterWaterline", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dUnderwaterWaterlineProfile EnsureHd2dAutonomousP3UnderwaterWaterlineProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dUnderwaterWaterlineProfile>(Hd2dAutonomousP3UnderwaterWaterlineProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dUnderwaterWaterlineProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP3UnderwaterWaterlineProfilePath);
            }

            profile.ConfigureForReview(
                true,
                false,
                true,
                true,
                true,
                true,
                true,
                true,
                0.176f,
                0.06f,
                0.18f,
                new Color(0.22f, 0.50f, 0.62f, 1f),
                0.32f,
                0.48f,
                0.40f,
                0.56f,
                1.45f,
                2.60f,
                0.14f,
                0.22f,
                0.62f,
                0.055f,
                0.18f,
                0.08f,
                0.18f,
                0.55f,
                "Keep as conditional underwater-camera data only. If Tom approves a real submerge camera path, prefer the conservative tint/fog/distortion values and wire the prepared fullscreen shader through a water-region trigger.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Material EnsureHd2dAutonomousP3UnderwaterWaterlineMaterial(FastVsHd2dUnderwaterWaterlineProfile profile)
        {
            EnsureFolder("Assets/Art/Materials/FastVS/HouseSlice");
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(Hd2dAutonomousP3UnderwaterWaterlineShaderPath);
            if (shader == null)
            {
                shader = Shader.Find(Hd2dAutonomousP3UnderwaterWaterlineShaderName);
            }

            if (shader == null)
            {
                throw new InvalidOperationException($"P3-84 underwater waterline shader not found: {Hd2dAutonomousP3UnderwaterWaterlineShaderName}");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3UnderwaterWaterlineMaterialPath);
            if (material == null)
            {
                material = new Material(shader)
                {
                    name = "FastVS_House_hd2d_p3_84_underwater_waterline"
                };
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP3UnderwaterWaterlineMaterialPath);
            }
            else if (material.shader != shader)
            {
                material.shader = shader;
            }

            material.SetFloat("_LocalIntensity", profile != null && profile.FinalUnderwaterWaterlineApprovedForReview ? 1f : 0f);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void CaptureHd2dAutonomousP3UnderwaterWaterlineBaselineShot(
            Camera camera,
            Transform pastRoot,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            camera.orthographic = false;
            camera.fieldOfView = 34f;
            PositionCloseReviewCamera(
                camera,
                pastRoot.TransformPoint(CentralPlazaVsCenter + new Vector3(1.70f, 0.24f, 2.22f)),
                new Vector3(0.72f, 1.46f, -2.20f),
                new Vector3(0.00f, 0.02f, 0.08f));
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            AddHd2dAutonomousP3UnderwaterWaterlineShotRow(fileName, label, 0f, 0f, 0f, rows);
        }

        private static void AddHd2dAutonomousP3UnderwaterWaterlineShotRow(
            string fileName,
            string label,
            float tintBlend,
            float depthFog,
            float distortionPixels,
            ICollection<string> rows)
        {
            rows.Add($"| `{fileName}` | {label} | {tintBlend:0.###} | {depthFog:0.###} | {distortionPixels:0.###}px |");
        }

        private static void WriteHd2dAutonomousP3UnderwaterWaterlinePreviewPng(
            string sourcePath,
            string outputPath,
            FastVsHd2dUnderwaterWaterlineProfile profile,
            bool stronger,
            float timeOffset)
        {
            var source = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            var output = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                if (!ImageConversion.LoadImage(source, File.ReadAllBytes(sourcePath)))
                {
                    throw new InvalidOperationException($"Fast VS autonomous P3-84 underwater preview failed: could not read {sourcePath}.");
                }

                output.Reinitialize(source.width, source.height, TextureFormat.RGBA32, false);
                var sourcePixels = source.GetPixels32();
                var outputPixels = new Color32[sourcePixels.Length];
                var width = source.width;
                var height = source.height;
                var tint = profile.UnderwaterTintForReview;
                var tintBlend = stronger ? profile.StrongerTintBlendForReview : profile.ConservativeTintBlendForReview;
                var fogDensity = stronger ? profile.StrongerDepthFogDensityForReview : profile.ConservativeDepthFogDensityForReview;
                var distortionPixels = stronger ? profile.StrongerDistortionPixelsForReview : profile.ConservativeDistortionPixelsForReview;
                var desaturation = stronger ? profile.StrongerFarDesaturationForReview : profile.ConservativeFarDesaturationForReview;
                for (var y = 0; y < height; y++)
                {
                    var uvY = (y + 0.5f) / height;
                    for (var x = 0; x < width; x++)
                    {
                        var uvX = (x + 0.5f) / width;
                        var waveX = Hd2dAutonomousP3UnderwaterCheapWave(uvX, uvY, timeOffset);
                        var waveY = Hd2dAutonomousP3UnderwaterCheapWave(uvY + 0.37f, uvX, timeOffset * 1.21f) * 0.58f;
                        var sampleX = Mathf.Clamp(Mathf.RoundToInt(x + waveX * distortionPixels), 0, width - 1);
                        var sampleY = Mathf.Clamp(Mathf.RoundToInt(y + waveY * distortionPixels), 0, height - 1);
                        var sourceColor = (Color)sourcePixels[(sampleY * width) + sampleX];
                        var depth01 = Mathf.Clamp01(0.28f + uvY * 0.62f + Mathf.Abs(uvX - 0.5f) * 0.10f);
                        var fogAmount = Mathf.Clamp01(fogDensity * depth01);
                        var gray = (sourceColor.r * 0.2126f) + (sourceColor.g * 0.7152f) + (sourceColor.b * 0.0722f);
                        var result = Color.Lerp(sourceColor, new Color(gray, gray, gray, sourceColor.a), desaturation * (0.40f + fogAmount * 0.60f));
                        result = Color.Lerp(result, tint, Mathf.Clamp01(tintBlend + fogAmount * 0.34f));

                        var waterlineDistance = Mathf.Abs(uvY - profile.NormalizedWaterlineYForReview);
                        var waterline = 1f - Hd2dAutonomousP3UnderwaterSmoothStep01(0f, profile.WaterlineFeatherForReview, waterlineDistance);
                        var surfaceColor = Color.Lerp(tint, Color.white, 0.32f);
                        result = Color.Lerp(result, surfaceColor, waterline * profile.SurfaceLineStrengthForReview);

                        var rays = Mathf.Pow(Mathf.Clamp01(Mathf.Sin((uvX * 26f) + (uvY * 7f) + timeOffset * 4.2f) * 0.5f + 0.5f), 9f);
                        rays *= 1f - Hd2dAutonomousP3UnderwaterSmoothStep01(0.25f, 1f, uvY);
                        result += tint * (rays * profile.GodRayStrengthForReview);

                        var edgeDistance = Vector2.Distance(new Vector2(uvX, uvY), new Vector2(0.5f, 0.5f)) * 1.42f;
                        var edgeMask = Hd2dAutonomousP3UnderwaterSmoothStep01(0.48f, 1f, edgeDistance);
                        result = Color.Lerp(result, tint * 0.58f, edgeMask * profile.EdgeVignetteStrengthForReview);
                        result.a = sourceColor.a;
                        outputPixels[(y * width) + x] = result;
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

        private static void WriteHd2dAutonomousP3UnderwaterWaterlineReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dUnderwaterWaterlineProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics aboveVsConservative,
            Hd2dAutonomousP1DepthPrimingDiffMetrics conservativeMotion,
            Hd2dAutonomousP1DepthPrimingDiffMetrics conservativeVsStronger,
            Hd2dAutonomousP1DepthPrimingDiffMetrics resetDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var state = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dUnderwaterWaterlineState>();
            var lines = new List<string>
            {
                "# P3-84 Underwater / Waterline Screen Tint Review",
                string.Empty,
                "- Scope: NEEDS-TOM conditional data prep for camera-below-water screen tint, fog, distortion, and a faint waterline. Runtime effect remains locked/off because the current game camera has no approved submerge path.",
                "- Recommendation: " + profile.RecommendationForReview,
                "- Technical note: screenshots 02-04 are deterministic fullscreen previews from the same above-water engine baseline. The shader/material/state are prepared, but the renderer feature is deferred until Tom approves an actual water-region camera path.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP3UnderwaterWaterlineProfilePath}` |",
                $"| Material / shader | `{Hd2dAutonomousP3UnderwaterWaterlineMaterialPath}` / `{Hd2dAutonomousP3UnderwaterWaterlineShaderPath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalUnderwaterWaterlineApprovedForReview)} |",
                $"| Runtime default disabled / renderer feature deferred | {FormatBool(profile.RuntimeDefaultDisabledForReview)} / {FormatBool(profile.RendererFeatureDeferredUntilApprovalForReview)} |",
                $"| State ready / active blend | {FormatBool(state != null && state.IsReadyForReview)} / {(state != null ? state.ActiveBlendForReview : 0f):0.###} |",
                $"| Water plane / submerge threshold / transition band | {profile.ReferenceWaterPlaneHeightForReview:0.###} / {profile.SubmergedActivationThresholdForReview:0.###} / {profile.TransitionBandForReview:0.###} |",
                $"| Conservative tint / fog / distortion / desat | {profile.ConservativeTintBlendForReview:0.###} / {profile.ConservativeDepthFogDensityForReview:0.###} / {profile.ConservativeDistortionPixelsForReview:0.###}px / {profile.ConservativeFarDesaturationForReview:0.###} |",
                $"| Stronger tint / fog / distortion / desat | {profile.StrongerTintBlendForReview:0.###} / {profile.StrongerDepthFogDensityForReview:0.###} / {profile.StrongerDistortionPixelsForReview:0.###}px / {profile.StrongerFarDesaturationForReview:0.###} |",
                $"| Waterline y / feather / line / god rays / edge vignette | {profile.NormalizedWaterlineYForReview:0.###} / {profile.WaterlineFeatherForReview:0.###} / {profile.SurfaceLineStrengthForReview:0.###} / {profile.GodRayStrengthForReview:0.###} / {profile.EdgeVignetteStrengthForReview:0.###} |",
                $"| Tint color | {FormatColorForReport(profile.UnderwaterTintForReview)} |",
                string.Empty,
                "| Capture | Label | Tint blend | Fog | Distortion |",
                "|---|---|---:|---:|---:|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                aboveVsConservative.ToReportRow("above-water baseline vs conservative underwater preview"),
                conservativeMotion.ToReportRow("conservative underwater frame A vs frame B distortion phase"),
                conservativeVsStronger.ToReportRow("conservative underwater preview vs stronger Tom option"),
                resetDiff.ToReportRow("above-water baseline vs above-water reset proof"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Above-water untinted engine baseline with the P2-61 review pond visible. |",
                $"| `{screenshotFiles[1]}` | Conservative underwater fullscreen tint/fog/waterline preview. |",
                $"| `{screenshotFiles[2]}` | Conservative preview at a second distortion phase, proving motion data. |",
                $"| `{screenshotFiles[3]}` | Stronger Tom option, not final approval. |",
                $"| `{screenshotFiles[4]}` | Above-water reset proof, copied from the untinted baseline. |"
            });

            File.WriteAllText(Path.Combine(outputDirectory, "underwater_waterline_screen_tint_data_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static float Hd2dAutonomousP3UnderwaterCheapWave(float x, float y, float timeOffset)
        {
            var a = Mathf.Sin((y * 31f) + (timeOffset * 3.7f));
            var b = Mathf.Sin(((x + y) * 19f) - (timeOffset * 2.4f));
            var c = Mathf.Sin(((x * 17f) - (y * 11f)) + timeOffset);
            return (a * 0.50f) + (b * 0.32f) + (c * 0.18f);
        }

        private static float Hd2dAutonomousP3UnderwaterSmoothStep01(float edge0, float edge1, float value)
        {
            var t = Mathf.Clamp01((value - edge0) / Mathf.Max(edge1 - edge0, 0.0001f));
            return t * t * (3f - (2f * t));
        }
    }
}
