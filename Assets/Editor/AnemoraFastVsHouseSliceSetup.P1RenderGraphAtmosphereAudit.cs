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
        private const int Hd2dAutonomousP1RenderGraphButoGridPixelSize = 8;
        private const int Hd2dAutonomousP1RenderGraphButoGridSizeZ = 96;
        private const string Hd2dAutonomousP1RenderGraphButoRenderPassPath = "Packages/com.occasoftware.buto/Runtime/Render Passes/ButoRenderPass.cs";
        private const string Hd2dAutonomousP1RenderGraphButoFeaturePath = "Packages/com.occasoftware.buto/Runtime/Renderer Features/ButoRenderFeature.cs";
        private const string Hd2dAutonomousP1RenderGraphButoIsolatedBlitShaderPath = "Packages/com.occasoftware.buto/Shaders/Resources/IsolatedBlit.shader";
        private const string Hd2dAutonomousP1RenderGraphButoMergeShaderPath = "Packages/com.occasoftware.buto/Shaders/Resources/Merge.shader";
        private const string Hd2dAutonomousP1RenderGraphDefaultVolumeProfilePath = "Assets/Settings/DefaultVolumeProfile.asset";
        private const float Hd2dAutonomousP1RenderGraphAtmosphericLocalIntensity = 1f;

        public static void CaptureHd2dAutonomousP1Item51RenderGraphAtmosphereAuditBatch()
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
                throw new InvalidOperationException("Fast VS autonomous P1-51 RenderGraph atmosphere audit capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1RenderGraphAtmosphereAudit();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("render_graph_atmosphere_audit");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_atmosphere_off_ui_guard.png",
                "02_atmosphere_on_ui_guard.png",
                "03_billboard_contact_shadow_halo_guard.png",
                "04_alpha_foliage_shaft_cut_guard.png"
            };

            try
            {
                guide.SetMovementFrozen(true);

                SetHd2dAutonomousP1RenderGraphAtmosphereStackEnabledForReview(false);
                CaptureHd2dAutonomousP1RenderGraphAtmosphereReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.42f, 0.02f, 3.20f),
                    new Vector3(0.60f, 3.05f, -4.95f),
                    new Vector3(0.03f, 0.86f, 0.16f),
                    32f,
                    outputDirectory,
                    screenshotFiles[0],
                    true);

                SetHd2dAutonomousP1RenderGraphAtmosphereStackEnabledForReview(true);
                CaptureHd2dAutonomousP1RenderGraphAtmosphereReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.42f, 0.02f, 3.20f),
                    new Vector3(0.60f, 3.05f, -4.95f),
                    new Vector3(0.03f, 0.86f, 0.16f),
                    32f,
                    outputDirectory,
                    screenshotFiles[1],
                    true);

                CaptureHd2dAutonomousP1RenderGraphAtmosphereReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.42f, 0.02f, 3.20f),
                    new Vector3(0.60f, 3.05f, -4.95f),
                    new Vector3(0.03f, 0.86f, 0.16f),
                    32f,
                    outputDirectory,
                    screenshotFiles[2],
                    false);

                CaptureHd2dAutonomousP1RenderGraphAtmosphereReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.20f, 0.02f, 4.06f),
                    new Vector3(0.74f, 5.15f, -6.75f),
                    new Vector3(0.02f, 0.98f, 0.20f),
                    33f,
                    outputDirectory,
                    screenshotFiles[3],
                    false);

                var stats = AnalyzeHd2dAutonomousP1RenderGraphAtmosphereStats();
                var atmosphereDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                    Path.Combine(outputDirectory, screenshotFiles[0]),
                    Path.Combine(outputDirectory, screenshotFiles[1]),
                    4);
                WriteHd2dAutonomousP1RenderGraphAtmosphereReviewReport(outputDirectory, screenshotFiles, stats, atmosphereDiff);
            }
            finally
            {
                SetHd2dAutonomousP1RenderGraphAtmosphereStackEnabledForReview(true);
            }

            Debug.Log($"Fast VS autonomous P1-51 RenderGraph atmosphere audit review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1RenderGraphAtmosphereAudit()
        {
#if BUTO
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            var butoFeature = FindHd2dAutonomousP1RenderGraphButoFeature(rendererData);
            if (butoFeature != null)
            {
                butoFeature.settings.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
                butoFeature.Create();
                EditorUtility.SetDirty(butoFeature);
            }

            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(Hd2dAutonomousP1RenderGraphDefaultVolumeProfilePath);
            if (volumeProfile != null && volumeProfile.TryGet<OccaSoftware.Buto.Runtime.ButoVolumetricFog>(out var butoFog))
            {
                butoFog.mode.overrideState = true;
                butoFog.mode.value = OccaSoftware.Buto.Runtime.VolumetricFogMode.On;
                butoFog.gridPixelSize.overrideState = true;
                butoFog.gridPixelSize.value = Hd2dAutonomousP1RenderGraphButoGridPixelSize;
                butoFog.gridSizeZ.overrideState = true;
                butoFog.gridSizeZ.value = Hd2dAutonomousP1RenderGraphButoGridSizeZ;
                butoFog.depthRatio.overrideState = true;
                butoFog.depthRatio.value = 2.0f;
                butoFog.temporalAALighting.overrideState = true;
                butoFog.temporalAALighting.value = 0.06f;
                butoFog.temporalAAMedia.overrideState = true;
                butoFog.temporalAAMedia.value = 0.06f;
                EditorUtility.SetDirty(butoFog);
                EditorUtility.SetDirty(volumeProfile);
            }
#endif

            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP1RenderGraphAtmosphereAudit()
        {
            var stats = AnalyzeHd2dAutonomousP1RenderGraphAtmosphereStats();
            var ready = stats.ButoProviderAvailable
                ? IsHd2dAutonomousP1RenderGraphButoAtmosphereAuditReady(stats)
                : IsHd2dAutonomousP1RenderGraphFallbackAtmosphereAuditReady(stats);
            if (!ready)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P1-51 RenderGraph atmosphere ordering/reduced-resolution audit is not ready. " +
                    $"butoProvider={stats.ButoProviderAvailable}, butoDefine={stats.ButoCompileSymbolActive}, renderGraph={stats.RenderGraphSourceReady}, " +
                    $"depthAware={stats.DepthAwareButoUpscaleReady}, merge={stats.MergeAfterIsolatedButoReady}, " +
                    $"depthTex={stats.DepthTextureRequired}, opaqueTex={stats.OpaqueTextureRequired}, butoIndex={stats.ButoFeatureIndex}, fogIndex={stats.AtmosphericFogFeatureIndex}, " +
                    $"postIndex={stats.PostProcessFeatureIndex}, fogInjection={stats.AtmosphericFogInjectionPoint}, fogReqColorDepth={stats.AtmosphericFogRequirementsHasColorDepth}, " +
                    $"grid={stats.ButoGridPixelSize}/{stats.ButoGridSizeZ}, volume720={stats.ButoVolumeWidthFor720p}x{stats.ButoVolumeHeightFor720p}x{stats.ButoVolumeDepthFor720p}.");
            }
        }

        private static bool IsHd2dAutonomousP1RenderGraphButoAtmosphereAuditReady(Hd2dAutonomousP1RenderGraphAtmosphereStats stats)
        {
            return stats.ButoCompileSymbolActive &&
                   stats.RenderGraphSourceReady &&
                   stats.DepthAwareButoUpscaleReady &&
                   stats.MergeAfterIsolatedButoReady &&
                   stats.DepthTextureRequired &&
                   stats.OpaqueTextureRequired &&
                   stats.ButoFeatureIndex >= 0 &&
                   stats.AtmosphericFogFeatureIndex >= 0 &&
                   stats.ButoFeatureIndex < stats.AtmosphericFogFeatureIndex &&
                   stats.AtmosphericFogInjectionPoint == FullScreenPassRendererFeature.InjectionPoint.BeforeRenderingPostProcessing.ToString() &&
                   stats.AtmosphericFogRequirementsHasColorDepth == true &&
                   stats.ButoGridPixelSize <= Hd2dAutonomousP1RenderGraphButoGridPixelSize &&
                   stats.ButoGridSizeZ <= Hd2dAutonomousP1RenderGraphButoGridSizeZ &&
                   stats.ButoVolumeWidthFor720p <= 180 &&
                   stats.ButoVolumeHeightFor720p <= 100 &&
                   stats.PostProcessFeatureIndex > stats.AtmosphericFogFeatureIndex;
        }

        private static bool IsHd2dAutonomousP1RenderGraphFallbackAtmosphereAuditReady(Hd2dAutonomousP1RenderGraphAtmosphereStats stats)
        {
            return stats.DepthTextureRequired &&
                   stats.OpaqueTextureRequired &&
                   stats.AtmosphericFogFeatureIndex >= 0 &&
                   stats.AtmosphericFogInjectionPoint == FullScreenPassRendererFeature.InjectionPoint.BeforeRenderingPostProcessing.ToString() &&
                   stats.AtmosphericFogRequirementsHasColorDepth == true &&
                   stats.PostProcessFeatureIndex > stats.AtmosphericFogFeatureIndex;
        }

        private static Hd2dAutonomousP1RenderGraphAtmosphereStats AnalyzeHd2dAutonomousP1RenderGraphAtmosphereStats()
        {
            var stats = new Hd2dAutonomousP1RenderGraphAtmosphereStats
            {
                FeatureOrder = string.Empty,
                SsaoFeatureIndex = -1,
                ButoFeatureIndex = -1,
                AtmosphericFogFeatureIndex = -1,
                PostProcessFeatureIndex = -1
            };
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            var pipelineAssetText = File.Exists(UniversalRenderPipelineAssetPath) ? File.ReadAllText(UniversalRenderPipelineAssetPath) : string.Empty;
            stats.DepthTextureRequired = pipelineAssetText.Contains("m_RequireDepthTexture: 1", StringComparison.Ordinal);
            stats.OpaqueTextureRequired = pipelineAssetText.Contains("m_RequireOpaqueTexture: 1", StringComparison.Ordinal);
#if BUTO
            stats.ButoCompileSymbolActive = true;
#endif

            if (rendererData != null && rendererData.rendererFeatures != null)
            {
                stats.RendererMode = rendererData.renderingMode.ToString();
                stats.DepthPrimingMode = rendererData.depthPrimingMode.ToString();
                for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
                {
                    var feature = rendererData.rendererFeatures[i];
                    if (feature == null)
                    {
                        continue;
                    }

                    var featureName = feature.name ?? string.Empty;
                    if (stats.FeatureOrder.Length > 0)
                    {
                        stats.FeatureOrder += " -> ";
                    }

                    stats.FeatureOrder += featureName;
                    if (feature is ScreenSpaceAmbientOcclusion)
                    {
                        stats.SsaoFeatureIndex = i;
                    }

                    if (featureName == "Buto Volumetric Fog")
                    {
                        stats.ButoFeatureIndex = i;
                        stats.ButoFeatureActive = IsScriptableRendererFeatureActive(feature);
                    }

                    if (feature is FullScreenPassRendererFeature fullScreen)
                    {
                        if (featureName == "FastVS HD2D P0 Atmospheric Perspective")
                        {
                            stats.AtmosphericFogFeatureIndex = i;
                            stats.AtmosphericFogActive = IsScriptableRendererFeatureActive(feature);
                            stats.AtmosphericFogInjectionPoint = fullScreen.injectionPoint.ToString();
                            stats.AtmosphericFogRequirementsHasColorDepth =
                                (fullScreen.requirements & ScriptableRenderPassInput.Color) != 0 &&
                                (fullScreen.requirements & ScriptableRenderPassInput.Depth) != 0 &&
                                fullScreen.fetchColorBuffer;
                        }
                        else if (featureName == "FastVS HD2D Stage7 TiltShift" || featureName == "FastVS HD2D Stage7 Outline")
                        {
                            stats.PostProcessFeatureIndex = Mathf.Max(stats.PostProcessFeatureIndex, i);
                        }
                    }
                    else if (featureName == "Fronkon Artistic Tilt Shift")
                    {
                        stats.PostProcessFeatureIndex = Mathf.Max(stats.PostProcessFeatureIndex, i);
                    }
                }
            }

            stats.ButoProviderAvailable = IsHd2dAutonomousP1RenderGraphButoProviderAvailable(rendererData);
            stats.RenderGraphSourceReady =
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoRenderPassPath, "RecordRenderGraph") &&
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoRenderPassPath, "AddUnsafePass") &&
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoRenderPassPath, "CreateRenderGraphTexture") &&
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoFeaturePath, "SetupRenderPasses");
            stats.DepthAwareButoUpscaleReady =
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoIsolatedBlitShaderPath, "SampleSceneDepth") &&
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoIsolatedBlitShaderPath, "GetDepthEye") &&
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoIsolatedBlitShaderPath, "IntegratorData.SampleLevel");
            stats.MergeAfterIsolatedButoReady =
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoRenderPassPath, "Blitter.BlitCameraTexture(cmd, source, isolatedBlitTarget") &&
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoRenderPassPath, "Blitter.BlitCameraTexture(cmd, source, mergeTarget") &&
                SourceHasToken(Hd2dAutonomousP1RenderGraphButoMergeShaderPath, "_ButoTexture");

#if BUTO
            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(Hd2dAutonomousP1RenderGraphDefaultVolumeProfilePath);
            if (volumeProfile != null && volumeProfile.TryGet<OccaSoftware.Buto.Runtime.ButoVolumetricFog>(out var butoFog))
            {
                stats.ButoVolumeActive = butoFog.active && butoFog.mode.value == OccaSoftware.Buto.Runtime.VolumetricFogMode.On;
                stats.ButoGridPixelSize = butoFog.gridPixelSize.value;
                stats.ButoGridSizeZ = butoFog.gridSizeZ.value;
                stats.ButoDepthRatio = butoFog.depthRatio.value;
                stats.ButoFogDensity = butoFog.fogDensity.value;
                var volumeSize = butoFog.GetVolumeSize(new Vector2Int(1280, 720));
                stats.ButoVolumeWidthFor720p = volumeSize.x;
                stats.ButoVolumeHeightFor720p = volumeSize.y;
                stats.ButoVolumeDepthFor720p = volumeSize.z;
                stats.ButoEstimatedVramMb = butoFog.EstimatedVram;
            }

            var butoFeature = FindHd2dAutonomousP1RenderGraphButoFeature(rendererData);
            if (butoFeature != null)
            {
                stats.ButoRenderPassEvent = butoFeature.settings.renderPassEvent.ToString();
            }
#endif

            return stats;
        }

        private static bool SourceHasToken(string path, string token)
        {
            return File.Exists(path) && File.ReadAllText(path).Contains(token, StringComparison.Ordinal);
        }

        private static bool IsHd2dAutonomousP1RenderGraphButoProviderAvailable(UniversalRendererData rendererData)
        {
#if BUTO
            return true;
#else
            if (File.Exists(Hd2dAutonomousP1RenderGraphButoRenderPassPath) ||
                File.Exists(Hd2dAutonomousP1RenderGraphButoFeaturePath))
            {
                return true;
            }

            if (rendererData == null || rendererData.rendererFeatures == null)
            {
                return false;
            }

            foreach (var feature in rendererData.rendererFeatures)
            {
                if (feature != null && string.Equals(feature.name, "Buto Volumetric Fog", StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
#endif
        }

#if BUTO
        private static OccaSoftware.Buto.Runtime.ButoRenderFeature FindHd2dAutonomousP1RenderGraphButoFeature(UniversalRendererData rendererData)
        {
            if (rendererData == null)
            {
                return null;
            }

            for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
            {
                if (rendererData.rendererFeatures[i] is OccaSoftware.Buto.Runtime.ButoRenderFeature feature)
                {
                    return feature;
                }
            }

            return null;
        }
#endif

        private static void SetHd2dAutonomousP1RenderGraphAtmosphereStackEnabledForReview(bool enabled)
        {
#if BUTO
            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(Hd2dAutonomousP1RenderGraphDefaultVolumeProfilePath);
            if (volumeProfile != null && volumeProfile.TryGet<OccaSoftware.Buto.Runtime.ButoVolumetricFog>(out var butoFog))
            {
                butoFog.mode.overrideState = true;
                butoFog.mode.value = enabled ? OccaSoftware.Buto.Runtime.VolumetricFogMode.On : OccaSoftware.Buto.Runtime.VolumetricFogMode.Off;
                EditorUtility.SetDirty(butoFog);
            }
#endif

            var atmosphericMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP0AtmosphericPerspectiveMaterialPath);
            if (atmosphericMaterial != null && atmosphericMaterial.HasProperty("_LocalIntensity"))
            {
                atmosphericMaterial.SetFloat("_LocalIntensity", enabled ? Hd2dAutonomousP1RenderGraphAtmosphericLocalIntensity : 0f);
                EditorUtility.SetDirty(atmosphericMaterial);
            }

            foreach (var layer in UnityEngine.Object.FindObjectsByType<FastVsHd2dAmbientDustPollenLayer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (enabled)
                {
                    layer.ClearReviewOverrideForReview();
                    layer.SimulateForReview(1.0f, true);
                }
                else
                {
                    layer.SetReviewOverrideForReview(false, 0f);
                }
            }

            foreach (var moteField in UnityEngine.Object.FindObjectsByType<FastVsHd2dSunShaftDustMoteField>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (enabled)
                {
                    moteField.ClearReviewOverrideForReview();
                    moteField.SimulateForReview(1.0f, true);
                }
                else
                {
                    moteField.SetReviewOverrideForReview(false, 0f);
                }
            }

            foreach (var shaftField in UnityEngine.Object.FindObjectsByType<FastVsDynamicSunShaftField>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                shaftField.enabled = enabled;
                foreach (var renderer in shaftField.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = enabled;
                }
            }

            AssetDatabase.SaveAssets();
        }

        private static void CaptureHd2dAutonomousP1RenderGraphAtmosphereReviewShot(
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
            string fileName,
            bool compositeUi)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerCurrentLocalForReview(playerLocal);
            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(playerLocal), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            WarmUpCameraRender(camera);
            var path = Path.Combine(outputDirectory, fileName);
            SaveCameraPng(camera, path);
            if (compositeUi)
            {
                CompositeHd2dAutonomousP0OrnateDialogueHud(
                    path,
                    1280,
                    720,
                    1,
                    "Reto",
                    "Render Graph pass order: volumetric, fog, sprites, focus, UI.",
                    "v");
            }

            ValidateCloseReviewOutputExists(outputDirectory, fileName);
        }

        private static void WriteHd2dAutonomousP1RenderGraphAtmosphereReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            Hd2dAutonomousP1RenderGraphAtmosphereStats stats,
            Hd2dAutonomousP1DepthPrimingDiffMetrics atmosphereDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# P1-51 RenderGraph Atmosphere Ordering / Reduced-Resolution Audit",
                string.Empty,
                "- Scope: static RenderGraph/pass-order audit plus review captures for fog/volumetric/DoF/UI ordering hazards.",
                "- Note: this batch records automated evidence only. Render Graph Viewer, Frame Debugger, and GPU timing still need an interactive confirmation for final acceptance.",
                string.Empty,
                "| Render contract | Value |",
                "|---|---|",
                $"| Feature order | {stats.FeatureOrder} |",
                $"| Renderer mode / Depth Priming | {stats.RendererMode} / {stats.DepthPrimingMode} |",
                $"| Depth texture / Opaque texture | {FormatBool(stats.DepthTextureRequired)} / {FormatBool(stats.OpaqueTextureRequired)} |",
                $"| Buto provider available / compile symbol | {FormatBool(stats.ButoProviderAvailable)} / {FormatBool(stats.ButoCompileSymbolActive)} |",
                $"| Buto feature index / active / event | {stats.ButoFeatureIndex} / {FormatBool(stats.ButoFeatureActive)} / {stats.ButoRenderPassEvent} |",
                $"| Atmospheric fog index / active / injection | {stats.AtmosphericFogFeatureIndex} / {FormatBool(stats.AtmosphericFogActive)} / {stats.AtmosphericFogInjectionPoint} |",
                $"| Fog feature fetches Color+Depth | {FormatBool(stats.AtmosphericFogRequirementsHasColorDepth)} |",
                $"| Post-process feature max index | {stats.PostProcessFeatureIndex} |",
                string.Empty,
                "| Reduced-resolution volumetric audit | Value |",
                "|---|---:|",
                $"| Buto volume active | {FormatBool(stats.ButoVolumeActive)} |",
                $"| Buto grid pixel size | {stats.ButoGridPixelSize} |",
                $"| Buto grid Z | {stats.ButoGridSizeZ} |",
                $"| Buto depth ratio | {stats.ButoDepthRatio:0.###} |",
                $"| Buto fog density | {stats.ButoFogDensity:0.###} |",
                $"| Buto 1280x720 volume dimensions | {stats.ButoVolumeWidthFor720p} x {stats.ButoVolumeHeightFor720p} x {stats.ButoVolumeDepthFor720p} |",
                $"| Buto estimated VRAM at reference dimensions | {stats.ButoEstimatedVramMb:0.0} MB |",
                string.Empty,
                "| RenderGraph/source audit | Value |",
                "|---|---|",
                $"| Buto RenderGraph source path ready | {FormatBool(stats.RenderGraphSourceReady)} |",
                $"| Buto depth-aware isolated blit/upscale ready | {FormatBool(stats.DepthAwareButoUpscaleReady)} |",
                $"| Buto isolated texture merged back after volume integration | {FormatBool(stats.MergeAfterIsolatedButoReady)} |",
                string.Empty,
                "| A/B evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                atmosphereDiff.ToReportRow("Atmosphere off UI guard vs atmosphere on UI guard"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Central Plaza dialogue/UI guard with Buto/fog/dust/shafts disabled for baseline |",
                $"| `{screenshotFiles[1]}` | Same Central Plaza UI guard with atmosphere stack enabled; UI is composited after camera capture to document no in-scene fog pass touches HUD art |",
                $"| `{screenshotFiles[2]}` | Billboard/contact-shadow halo guard in Central Plaza with atmosphere stack enabled |",
                $"| `{screenshotFiles[3]}` | Alpha foliage and sun-shaft cut guard with DepthOnly-enabled foliage cards and depth-aware Buto isolated blit |"
            };

            File.WriteAllText(Path.Combine(outputDirectory, "render_graph_atmosphere_audit_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private struct Hd2dAutonomousP1RenderGraphAtmosphereStats
        {
            public string FeatureOrder;
            public string RendererMode;
            public string DepthPrimingMode;
            public bool DepthTextureRequired;
            public bool OpaqueTextureRequired;
            public int SsaoFeatureIndex;
            public bool ButoProviderAvailable;
            public bool ButoCompileSymbolActive;
            public int ButoFeatureIndex;
            public bool ButoFeatureActive;
            public string ButoRenderPassEvent;
            public int AtmosphericFogFeatureIndex;
            public bool AtmosphericFogActive;
            public string AtmosphericFogInjectionPoint;
            public bool AtmosphericFogRequirementsHasColorDepth;
            public int PostProcessFeatureIndex;
            public bool ButoVolumeActive;
            public int ButoGridPixelSize;
            public int ButoGridSizeZ;
            public float ButoDepthRatio;
            public float ButoFogDensity;
            public int ButoVolumeWidthFor720p;
            public int ButoVolumeHeightFor720p;
            public int ButoVolumeDepthFor720p;
            public float ButoEstimatedVramMb;
            public bool RenderGraphSourceReady;
            public bool DepthAwareButoUpscaleReady;
            public bool MergeAfterIsolatedButoReady;
        }
    }
}
