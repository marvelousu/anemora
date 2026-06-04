using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        private const string Hd2dAutonomousP1DepthPrimingFoliageShaderPath = "Assets/Art/Shaders/FastVS/FastVS_FoliageCardLit.shader";
        private const string Hd2dAutonomousP1DepthPrimingGpuGrassShaderPath = "Assets/Art/Shaders/FastVS/FastVS_GpuGrassBladeIndirect.shader";
        private const string Hd2dAutonomousP1DepthPrimingSpriteShaderPath = "Assets/Art/Shaders/FastVS/FastVS_SpriteCardRampLit.shader";
        private const string Hd2dAutonomousP1DepthPrimingSurfaceShaderPath = "Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader";

        public static void CaptureHd2dAutonomousP1Item49DepthPrimingBatch()
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
                throw new InvalidOperationException("Fast VS autonomous P1-49 depth priming capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1DepthPriming();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("depth_priming_alpha_clip");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_depth_priming_disabled_beauty.png",
                "02_depth_priming_auto_beauty.png",
                "03_alpha_clip_depth_eligible_close.png",
                "04_depth_texture_portal_guard.png"
            };

            var stats = AnalyzeHd2dAutonomousP1DepthPrimingStats();
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            var originalMode = rendererData != null ? rendererData.depthPrimingMode : DepthPrimingMode.Disabled;
            try
            {
                guide.SetMovementFrozen(true);

                SetHd2dAutonomousP1DepthPrimingModeForReview(DepthPrimingMode.Auto);
                CaptureHd2dAutonomousP1DepthPrimingReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.24f, 0.02f, 3.28f),
                    new Vector3(0.36f, 7.9f, -9.85f),
                    new Vector3(0.02f, 1.18f, 0.12f),
                    36f,
                    outputDirectory,
                    screenshotFiles[1]);

                CaptureHd2dAutonomousP1DepthPrimingReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.MiaHouse,
                    Chapter1MiaHouseMapCenter + new Vector3(-0.20f, 0.02f, -6.40f),
                    new Vector3(0.88f, 1.54f, -3.12f),
                    new Vector3(0.08f, 0.22f, 0.10f),
                    33f,
                    outputDirectory,
                    screenshotFiles[2]);

                CaptureHd2dAutonomousP1DepthPrimingReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(0.02f, 0.02f, 3.46f),
                    new Vector3(1.10f, 7.9f, -10.10f),
                    new Vector3(0.02f, 1.34f, 0.16f),
                    38f,
                    outputDirectory,
                    screenshotFiles[3]);

                SetHd2dAutonomousP1DepthPrimingModeForReview(DepthPrimingMode.Disabled);
                CaptureHd2dAutonomousP1DepthPrimingReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.24f, 0.02f, 3.28f),
                    new Vector3(0.36f, 7.9f, -9.85f),
                    new Vector3(0.02f, 1.18f, 0.12f),
                    36f,
                    outputDirectory,
                    screenshotFiles[0]);

                var beautyDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(Path.Combine(outputDirectory, screenshotFiles[0]), Path.Combine(outputDirectory, screenshotFiles[1]), 4);
                WriteHd2dAutonomousP1DepthPrimingReviewReport(outputDirectory, screenshotFiles, stats, beautyDiff);
                ValidateHd2dAutonomousP1DepthPrimingVisualParity(beautyDiff);
            }
            finally
            {
                // RecoveryV3 (2026-06-03): restore to Disabled (the build truth), not Auto. The old code
                // converted Disabled back to Auto here, re-infecting the renderer asset after review.
                SetHd2dAutonomousP1DepthPrimingModeForReview(DepthPrimingMode.Disabled);
            }

            Debug.Log($"Fast VS autonomous P1-49 depth priming review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1DepthPriming()
        {
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            if (rendererData == null)
            {
                throw new InvalidOperationException($"House slice setup failed: missing renderer data for P1-49 depth priming at {UniversalRenderPipelineRendererAssetPath}.");
            }

            // RecoveryV3 (2026-06-03): set Disabled (was Auto). This is the normal create path that runs
            // on every CreateHouseSliceScene; leaving it Auto re-infected the renderer asset and undid the
            // fix in AnemoraFastVsHd2dRenderAssetSetup. DepthPrimingMode.Auto + ForwardPlus conflicts with
            // PortalStencilFeature, dropping portal-stencil-dependent opaque ground/buildings from the
            // main opaque pass at runtime. The DepthOnly passes below stay enabled (good for
            // alpha-clip/opaque depth); only the priming MODE changes to Disabled.
            rendererData.depthPrimingMode = DepthPrimingMode.Disabled;
            EditorUtility.SetDirty(rendererData);

            foreach (var material in LoadHd2dAutonomousP1DepthPrimingMaterials())
            {
                if (material == null)
                {
                    continue;
                }

                if (IsHd2dAutonomousP1DepthPrimingCutoutOrOpaqueMaterial(material))
                {
                    material.SetShaderPassEnabled("DepthOnly", true);
                    EditorUtility.SetDirty(material);
                }
            }

            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP1DepthPriming()
        {
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            if (rendererData == null || rendererData.depthPrimingMode != DepthPrimingMode.Disabled)
            {
                throw new InvalidOperationException($"House slice validation failed: P1-49 renderer depth priming must be Disabled (Auto+ForwardPlus drops portal-stencil opaque ground/buildings from the main opaque pass), current={rendererData?.depthPrimingMode.ToString() ?? "missing"}.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1DepthPrimingFoliageShaderPath), "Name \"DepthOnly\"", Hd2dAutonomousP1DepthPrimingFoliageShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1DepthPrimingFoliageShaderPath), "clip(alpha - _Cutoff)", Hd2dAutonomousP1DepthPrimingFoliageShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1DepthPrimingGpuGrassShaderPath), "Name \"DepthOnly\"", Hd2dAutonomousP1DepthPrimingGpuGrassShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1DepthPrimingGpuGrassShaderPath), "clip(bladeHalfWidth - abs(input.uv.x - 0.5) - _AlphaCutoff)", Hd2dAutonomousP1DepthPrimingGpuGrassShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1DepthPrimingSpriteShaderPath), "Name \"DepthOnly\"", Hd2dAutonomousP1DepthPrimingSpriteShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1DepthPrimingSpriteShaderPath), "FastVsApplySharedWindControlWithPhase", Hd2dAutonomousP1DepthPrimingSpriteShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1DepthPrimingSurfaceShaderPath), "Name \"DepthOnly\"", Hd2dAutonomousP1DepthPrimingSurfaceShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP1DepthPrimingSurfaceShaderPath), "output.positionCS = TransformWorldToHClip(positionWS)", Hd2dAutonomousP1DepthPrimingSurfaceShaderPath);

            var stats = AnalyzeHd2dAutonomousP1DepthPrimingStats();
            if (stats.CutoutMaterialSlots < 12 ||
                stats.CutoutDepthOnlySlots != stats.CutoutMaterialSlots ||
                stats.CutoutShadowCasterSlots != stats.CutoutMaterialSlots ||
                stats.FoliageCutoutDepthOnlySlots < 4 ||
                stats.OpaqueDepthOnlySlots < 20 ||
                stats.TransparentDepthOnlySlots != 0)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P1-49 depth priming needs alpha-clipped foliage/sprite cards and opaque ground/props to expose DepthOnly while transparent overlays remain excluded. " +
                    $"cutout={stats.CutoutDepthOnlySlots}/{stats.CutoutMaterialSlots}, shadow={stats.CutoutShadowCasterSlots}, foliage={stats.FoliageCutoutDepthOnlySlots}, opaque={stats.OpaqueDepthOnlySlots}, transparentDepth={stats.TransparentDepthOnlySlots}.");
            }
        }

        private static void SetHd2dAutonomousP1DepthPrimingModeForReview(DepthPrimingMode mode)
        {
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            if (rendererData == null)
            {
                return;
            }

            rendererData.depthPrimingMode = mode;
            EditorUtility.SetDirty(rendererData);
            AssetDatabase.SaveAssets();
            RecreateHd2dAutonomousP1DepthPrimingRendererForReview();
        }

        private static void RecreateHd2dAutonomousP1DepthPrimingRendererForReview()
        {
            var pipelineAsset =
                GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset ??
                GraphicsSettings.defaultRenderPipeline as UniversalRenderPipelineAsset ??
                QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            if (pipelineAsset == null)
            {
                return;
            }

            var destroyRenderers = typeof(UniversalRenderPipelineAsset).GetMethod("DestroyRenderers", BindingFlags.Instance | BindingFlags.NonPublic);
            destroyRenderers?.Invoke(pipelineAsset, null);
        }

        private static void CaptureHd2dAutonomousP1DepthPrimingReviewShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            FastVsHouseArea area,
            Vector3 playerLocal,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            string outputDirectory,
            string fileName)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerCurrentLocalForReview(playerLocal);
            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = Hd2dRuntimeCameraNearClipPlane;
            camera.farClipPlane = 170f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(playerLocal), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
        }

        private static Hd2dAutonomousP1DepthPrimingStats AnalyzeHd2dAutonomousP1DepthPrimingStats()
        {
            var stats = new Hd2dAutonomousP1DepthPrimingStats();
            var renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var uniqueMaterials = new HashSet<Material>();
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null || renderer.sharedMaterials == null)
                {
                    continue;
                }

                var materials = renderer.sharedMaterials;
                for (var materialIndex = 0; materialIndex < materials.Length; materialIndex++)
                {
                    var material = materials[materialIndex];
                    if (material == null || material.shader == null)
                    {
                        continue;
                    }

                    uniqueMaterials.Add(material);
                    var isCutout = IsHd2dAutonomousP1DepthPrimingCutoutMaterial(material);
                    var isOpaque = IsHd2dAutonomousP1DepthPrimingOpaqueMaterial(material);
                    var isTransparent = IsHd2dAutonomousP1DepthPrimingTransparentMaterial(material);
                    if (isCutout)
                    {
                        stats.CutoutMaterialSlots++;
                        if (material.GetShaderPassEnabled("DepthOnly"))
                        {
                            stats.CutoutDepthOnlySlots++;
                        }

                        if (material.GetShaderPassEnabled("SHADOWCASTER") || material.GetShaderPassEnabled("ShadowCaster"))
                        {
                            stats.CutoutShadowCasterSlots++;
                        }

                        if (IsHd2dAutonomousP1DepthPrimingFoliageMaterial(material))
                        {
                            stats.FoliageCutoutSlots++;
                            if (material.GetShaderPassEnabled("DepthOnly"))
                            {
                                stats.FoliageCutoutDepthOnlySlots++;
                            }
                        }
                    }
                    else if (isOpaque)
                    {
                        stats.OpaqueMaterialSlots++;
                        if (material.GetShaderPassEnabled("DepthOnly"))
                        {
                            stats.OpaqueDepthOnlySlots++;
                        }
                    }

                    if (isTransparent)
                    {
                        stats.TransparentMaterialSlots++;
                        if (material.GetShaderPassEnabled("DepthOnly"))
                        {
                            stats.TransparentDepthOnlySlots++;
                        }
                    }
                }
            }

            stats.UniqueMaterialCount = uniqueMaterials.Count;
            return stats;
        }

        private static IEnumerable<Material> LoadHd2dAutonomousP1DepthPrimingMaterials()
        {
            var materialGuids = AssetDatabase.FindAssets("t:Material", new[] { MaterialDirectory });
            for (var i = 0; i < materialGuids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(materialGuids[i]);
                var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material != null)
                {
                    yield return material;
                }
            }
        }

        private static bool IsHd2dAutonomousP1DepthPrimingCutoutOrOpaqueMaterial(Material material)
        {
            return IsHd2dAutonomousP1DepthPrimingCutoutMaterial(material) ||
                   IsHd2dAutonomousP1DepthPrimingOpaqueMaterial(material);
        }

        private static bool IsHd2dAutonomousP1DepthPrimingCutoutMaterial(Material material)
        {
            var renderType = material.GetTag("RenderType", false, string.Empty);
            var shaderName = material.shader != null ? material.shader.name : string.Empty;
            return string.Equals(renderType, "TransparentCutout", StringComparison.Ordinal) ||
                   string.Equals(shaderName, SpriteCardRampLitShaderName, StringComparison.Ordinal) ||
                   string.Equals(shaderName, FoliageCardLitShaderName, StringComparison.Ordinal) ||
                   (material.HasProperty("_AlphaClip") && material.GetFloat("_AlphaClip") > 0.5f && material.renderQueue < 2990);
        }

        private static bool IsHd2dAutonomousP1DepthPrimingOpaqueMaterial(Material material)
        {
            var renderType = material.GetTag("RenderType", false, string.Empty);
            return (string.Equals(renderType, "Opaque", StringComparison.Ordinal) || material.renderQueue < 2500) &&
                   !IsHd2dAutonomousP1DepthPrimingCutoutMaterial(material) &&
                   !IsHd2dAutonomousP1DepthPrimingTransparentMaterial(material);
        }

        private static bool IsHd2dAutonomousP1DepthPrimingTransparentMaterial(Material material)
        {
            var renderType = material.GetTag("RenderType", false, string.Empty);
            return string.Equals(renderType, "Transparent", StringComparison.Ordinal) || material.renderQueue >= 2990;
        }

        private static bool IsHd2dAutonomousP1DepthPrimingFoliageMaterial(Material material)
        {
            var name = material.name.ToLowerInvariant();
            return name.Contains("leaf") ||
                   name.Contains("hedge") ||
                   name.Contains("tree") ||
                   name.Contains("grass") ||
                   name.Contains("foliage") ||
                   name.Contains("bush");
        }

        private static void ValidateHd2dAutonomousP1DepthPrimingVisualParity(Hd2dAutonomousP1DepthPrimingDiffMetrics metrics)
        {
            if (metrics.SampleCount <= 0 || metrics.ChangedPercent > 0.10f || metrics.MeanRgbDelta > 0.08f)
            {
                throw new InvalidOperationException($"Fast VS autonomous P1-49 depth priming capture failed: Disabled/Auto beauty parity drifted too far, changed={metrics.ChangedPercent:0.###}%, mean={metrics.MeanRgbDelta:0.###}.");
            }
        }

        private static Hd2dAutonomousP1DepthPrimingDiffMetrics MeasureHd2dAutonomousP1DepthPrimingDiff(string firstPath, string secondPath, int threshold)
        {
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP1DepthPrimingDiffMetrics(0, 0, 0f, 0f);
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

                return new Hd2dAutonomousP1DepthPrimingDiffMetrics(
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

        private static void WriteHd2dAutonomousP1DepthPrimingReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            Hd2dAutonomousP1DepthPrimingStats stats,
            Hd2dAutonomousP1DepthPrimingDiffMetrics beautyDiff)
        {
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            var lines = new List<string>
            {
                "# P1-49 Depth Priming / Alpha Clip Review",
                string.Empty,
                "- Scope: auto-safe URP Depth Priming configuration plus matching DepthOnly passes for alpha-clipped sprite/foliage cards and opaque ground/props.",
                "- Recommendation: keep Depth Priming Mode on Auto; use Frame Debugger/Profiler during Tom's next interactive pass for exact GPU timing, while this batch verifies configuration, eligibility, and visual parity.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Renderer asset | `{UniversalRenderPipelineRendererAssetPath}` |",
                $"| Depth Priming Mode | {rendererData?.depthPrimingMode.ToString() ?? "missing"} |",
                $"| Depth texture required | {FormatBool(UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline != null)} |",
                $"| Unique material count scanned | {stats.UniqueMaterialCount} |",
                $"| Cutout DepthOnly / cutout slots | {stats.CutoutDepthOnlySlots} / {stats.CutoutMaterialSlots} |",
                $"| Cutout ShadowCaster / cutout slots | {stats.CutoutShadowCasterSlots} / {stats.CutoutMaterialSlots} |",
                $"| Foliage cutout DepthOnly / foliage slots | {stats.FoliageCutoutDepthOnlySlots} / {stats.FoliageCutoutSlots} |",
                $"| Opaque DepthOnly / opaque slots | {stats.OpaqueDepthOnlySlots} / {stats.OpaqueMaterialSlots} |",
                $"| Transparent DepthOnly / transparent slots | {stats.TransparentDepthOnlySlots} / {stats.TransparentMaterialSlots} |",
                string.Empty,
                "| Beauty parity | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                beautyDiff.ToReportRow("Depth Priming Disabled vs Auto"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Beauty reference with Depth Priming temporarily disabled |",
                $"| `{screenshotFiles[1]}` | Same shot with Depth Priming Auto restored |",
                $"| `{screenshotFiles[2]}` | Alpha-clipped foliage/card close view for depth-eligible silhouettes |",
                $"| `{screenshotFiles[3]}` | Portal/depth-texture guard shot after Depth Priming Auto |",
                string.Empty,
                "| Shader path | Depth contract |",
                "|---|---|",
                $"| `{Hd2dAutonomousP1DepthPrimingFoliageShaderPath}` | `DepthOnly` pass clips `_BaseMap.a * _BaseColor.a - _Cutoff` and applies shared foliage wind |",
                $"| `{Hd2dAutonomousP1DepthPrimingGpuGrassShaderPath}` | `DepthOnly` pass mirrors indirect blade shaping, pixel snap, bend map, and `_AlphaCutoff` clipping |",
                $"| `{Hd2dAutonomousP1DepthPrimingSpriteShaderPath}` | `DepthOnly` pass clips `_BaseMap.a * _BaseColor.a - _Cutoff` and applies shared sprite-card wind |",
                $"| `{Hd2dAutonomousP1DepthPrimingSurfaceShaderPath}` | Explicit opaque `DepthOnly` pass matches the forward vertex transform for ramp-lit ground/props |"
            };

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllText(Path.Combine(outputDirectory, "depth_priming_alpha_clip_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private struct Hd2dAutonomousP1DepthPrimingStats
        {
            public int UniqueMaterialCount;
            public int CutoutMaterialSlots;
            public int CutoutDepthOnlySlots;
            public int CutoutShadowCasterSlots;
            public int FoliageCutoutSlots;
            public int FoliageCutoutDepthOnlySlots;
            public int OpaqueMaterialSlots;
            public int OpaqueDepthOnlySlots;
            public int TransparentMaterialSlots;
            public int TransparentDepthOnlySlots;
        }

        private readonly struct Hd2dAutonomousP1DepthPrimingDiffMetrics
        {
            public readonly int SampleCount;
            public readonly int ChangedPixels;
            public readonly float ChangedPercent;
            public readonly float MeanRgbDelta;

            public Hd2dAutonomousP1DepthPrimingDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
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
