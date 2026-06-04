using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP1SrpFoliageShaderPath = "Assets/Art/Shaders/FastVS/FastVS_FoliageCardLit.shader";
        private const string Hd2dAutonomousP1SrpSpriteLitShaderPath = "Assets/Art/Shaders/FastVS/FastVS_SpriteCardRampLit.shader";
        private const string Hd2dAutonomousP1SrpSpriteUnlitShaderPath = "Assets/Art/Shaders/FastVS/FastVS_SpriteCardRampUnlit.shader";
        private const string Hd2dAutonomousP1SrpSurfaceShaderPath = "Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader";
        private const string Hd2dAutonomousP1SrpWaterShaderPath = "Assets/Art/Shaders/FastVS/FastVS_DepthGradientWater.shader";

        public static void CaptureHd2dAutonomousP1Item50SrpBatcherGpuResidentDrawerBatch()
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
                throw new InvalidOperationException("Fast VS autonomous P1-50 SRP/GPU Resident capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1SrpBatcherGpuResidentDrawer();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("srp_batcher_gpu_resident_drawer");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_grd_disabled_reference_beauty.png",
                "02_grd_instanced_drawing_same_view.png",
                "03_mesh_renderer_eligibility_view.png",
                "04_sprite_non_target_guard.png"
            };

            var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(UniversalRenderPipelineAssetPath);
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            var originalMode = pipelineAsset != null ? pipelineAsset.gpuResidentDrawerMode : GPUResidentDrawerMode.InstancedDrawing;
            var originalDepthPriming = rendererData != null ? rendererData.depthPrimingMode : DepthPrimingMode.Auto;

            try
            {
                guide.SetMovementFrozen(true);
                SetHd2dAutonomousP1DepthPrimingModeForReview(DepthPrimingMode.Disabled);

                SetHd2dAutonomousP1SrpGpuResidentDrawerModeForReview(GPUResidentDrawerMode.Disabled);
                CaptureHd2dAutonomousP1SrpBatcherGpuResidentReviewShot(
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

                SetHd2dAutonomousP1SrpGpuResidentDrawerModeForReview(GPUResidentDrawerMode.InstancedDrawing);
                CaptureHd2dAutonomousP1SrpBatcherGpuResidentReviewShot(
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

                CaptureHd2dAutonomousP1SrpBatcherGpuResidentReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(0.18f, 0.02f, 4.18f),
                    new Vector3(0.82f, 5.4f, -6.90f),
                    new Vector3(0.02f, 0.86f, 0.24f),
                    33f,
                    outputDirectory,
                    screenshotFiles[2]);

                CaptureHd2dAutonomousP1SrpBatcherGpuResidentReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.MiaHouse,
                    Chapter1MiaHouseMapCenter + new Vector3(-0.05f, 0.02f, -3.90f),
                    new Vector3(0.74f, 2.22f, -4.35f),
                    new Vector3(0.06f, 0.72f, 0.22f),
                    34f,
                    outputDirectory,
                    screenshotFiles[3]);

                var stats = AnalyzeHd2dAutonomousP1SrpBatcherGpuResidentStats();
                var visualDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(Path.Combine(outputDirectory, screenshotFiles[0]), Path.Combine(outputDirectory, screenshotFiles[1]), 4);
                WriteHd2dAutonomousP1SrpBatcherGpuResidentReviewReport(outputDirectory, screenshotFiles, stats, visualDiff);
                ValidateHd2dAutonomousP1SrpBatcherGpuResidentVisualParity(visualDiff);
            }
            finally
            {
                SetHd2dAutonomousP1SrpGpuResidentDrawerModeForReview(originalMode == GPUResidentDrawerMode.Disabled ? GPUResidentDrawerMode.InstancedDrawing : originalMode);
                SetHd2dAutonomousP1DepthPrimingModeForReview(originalDepthPriming);
            }

            Debug.Log($"Fast VS autonomous P1-50 SRP Batcher / GPU Resident Drawer review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1SrpBatcherGpuResidentDrawer()
        {
            ApplyHd2dAutonomousP1SrpBatcherGpuResidentSettings();

            foreach (var material in LoadHd2dAutonomousP1SrpBatcherGpuResidentMaterials())
            {
                if (material == null || !IsHd2dAutonomousP1SrpBatcherGpuResidentTargetShader(material.shader))
                {
                    continue;
                }

                material.enableInstancing = true;
                EditorUtility.SetDirty(material);
            }

            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP1SrpBatcherGpuResidentDrawer()
        {
            var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(UniversalRenderPipelineAssetPath);
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            if (pipelineAsset == null || rendererData == null)
            {
                throw new InvalidOperationException("House slice validation failed: P1-50 missing URP pipeline or renderer asset.");
            }

            if (!pipelineAsset.useSRPBatcher ||
                pipelineAsset.gpuResidentDrawerMode != GPUResidentDrawerMode.InstancedDrawing ||
                EditorGraphicsSettings.batchRendererGroupShaderStrippingMode != BatchRendererGroupStrippingMode.KeepAll ||
                rendererData.renderingMode != RenderingMode.ForwardPlus)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P1-50 requires Forward+, SRP Batcher on, BatchRendererGroup Variants Keep All, and GPU Resident Drawer Instanced Drawing. " +
                    $"srp={pipelineAsset.useSRPBatcher}, grd={pipelineAsset.gpuResidentDrawerMode}, brg={EditorGraphicsSettings.batchRendererGroupShaderStrippingMode}, renderer={rendererData.renderingMode}.");
            }

            ValidateHd2dAutonomousP1SrpBatcherShaderSource(Hd2dAutonomousP1SrpFoliageShaderPath);
            ValidateHd2dAutonomousP1SrpBatcherShaderSource(Hd2dAutonomousP1SrpSpriteLitShaderPath);
            ValidateHd2dAutonomousP1SrpBatcherShaderSource(Hd2dAutonomousP1SrpSpriteUnlitShaderPath);
            ValidateHd2dAutonomousP1SrpBatcherShaderSource(Hd2dAutonomousP1SrpSurfaceShaderPath);
            ValidateHd2dAutonomousP1SrpBatcherShaderSource(Hd2dAutonomousP1SrpWaterShaderPath);

            var stats = AnalyzeHd2dAutonomousP1SrpBatcherGpuResidentStats();
            if (stats.TargetMaterialSlots < 80 ||
                stats.TargetInstancedMaterialSlots != stats.TargetMaterialSlots ||
                stats.TargetMeshRenderers < 40 ||
                stats.TargetMeshRenderersEligibleForResidentDrawer < 4000 ||
                stats.TargetShaderCbufferFailures != 0 ||
                stats.TargetShaderInstancingPragmaFailures != 0)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P1-50 target mesh/material audit still has SRP/instancing breakers. " +
                    $"slots={stats.TargetInstancedMaterialSlots}/{stats.TargetMaterialSlots}, meshRenderers={stats.TargetMeshRenderers}, " +
                    $"propertyBlocks={stats.TargetMeshRendererPropertyBlockBreakers}, intentionalPropertyBlocks={stats.TargetMeshRendererIntentionalPropertyBlockCarveouts}, " +
                    $"cbufferFailures={stats.TargetShaderCbufferFailures}, instancingFailures={stats.TargetShaderInstancingPragmaFailures}.");
            }
        }

        private static void ApplyHd2dAutonomousP1SrpBatcherGpuResidentSettings()
        {
            var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(UniversalRenderPipelineAssetPath);
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            if (pipelineAsset == null || rendererData == null)
            {
                throw new InvalidOperationException("House slice setup failed: P1-50 missing URP pipeline or renderer asset.");
            }

            pipelineAsset.useSRPBatcher = true;
            pipelineAsset.gpuResidentDrawerMode = GPUResidentDrawerMode.InstancedDrawing;
            TrySetHd2dAutonomousP1BatchRendererGroupShaderStrippingMode(BatchRendererGroupStrippingMode.KeepAll);
            rendererData.renderingMode = RenderingMode.ForwardPlus;

            EditorUtility.SetDirty(pipelineAsset);
            EditorUtility.SetDirty(rendererData);
        }

        private static void TrySetHd2dAutonomousP1BatchRendererGroupShaderStrippingMode(BatchRendererGroupStrippingMode mode)
        {
            var property = typeof(EditorGraphicsSettings).GetProperty(
                "batchRendererGroupShaderStrippingMode",
                BindingFlags.Public | BindingFlags.Static);
            if (property != null && property.CanWrite)
            {
                property.SetValue(null, mode);
            }
        }

        private static void SetHd2dAutonomousP1SrpGpuResidentDrawerModeForReview(GPUResidentDrawerMode mode)
        {
            var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(UniversalRenderPipelineAssetPath);
            if (pipelineAsset == null)
            {
                return;
            }

            pipelineAsset.gpuResidentDrawerMode = mode;
            EditorUtility.SetDirty(pipelineAsset);
            AssetDatabase.SaveAssets();
            RecreateHd2dAutonomousP1DepthPrimingRendererForReview();
        }

        private static void CaptureHd2dAutonomousP1SrpBatcherGpuResidentReviewShot(
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

        private static Hd2dAutonomousP1SrpBatcherGpuResidentStats AnalyzeHd2dAutonomousP1SrpBatcherGpuResidentStats()
        {
            var stats = new Hd2dAutonomousP1SrpBatcherGpuResidentStats();
            var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(UniversalRenderPipelineAssetPath);
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UniversalRenderPipelineRendererAssetPath);
            stats.SrpBatcherEnabled = pipelineAsset != null && pipelineAsset.useSRPBatcher;
            stats.GpuResidentDrawerMode = pipelineAsset != null ? pipelineAsset.gpuResidentDrawerMode.ToString() : "missing";
            stats.BrgStrippingMode = EditorGraphicsSettings.batchRendererGroupShaderStrippingMode.ToString();
            stats.RendererMode = rendererData != null ? rendererData.renderingMode.ToString() : "missing";
            stats.BatchRendererGroupBufferTarget = BatchRendererGroup.BufferTarget.ToString();
            stats.ComputeShadersSupported = SystemInfo.supportsComputeShaders;
            stats.RawBufferPathSupported = BatchRendererGroup.BufferTarget == BatchBufferTarget.RawBuffer;

            if (pipelineAsset is IGPUResidentRenderPipeline gpuResidentPipeline)
            {
                stats.SrpReportsGpuResidentSupported = gpuResidentPipeline.IsGPUResidentDrawerSupportedBySRP(out var message, out var severity);
                stats.SrpGpuResidentSupportMessage = string.IsNullOrWhiteSpace(message) ? severity.ToString() : $"{severity}: {message}";
            }
            else
            {
                stats.SrpReportsGpuResidentSupported = false;
                stats.SrpGpuResidentSupportMessage = "Pipeline asset does not implement IGPUResidentRenderPipeline.";
            }

            foreach (var shaderPath in Hd2dAutonomousP1SrpBatcherShaderPaths())
            {
                if (!File.Exists(shaderPath))
                {
                    stats.TargetShaderCbufferFailures++;
                    stats.TargetShaderInstancingPragmaFailures++;
                    continue;
                }

                var source = File.ReadAllText(shaderPath);
                if (source.IndexOf("CBUFFER_START(UnityPerMaterial)", StringComparison.Ordinal) < 0 ||
                    source.IndexOf("UsePass \"Universal Render Pipeline/Lit", StringComparison.Ordinal) >= 0)
                {
                    stats.TargetShaderCbufferFailures++;
                }

                if (source.IndexOf("#pragma multi_compile_instancing", StringComparison.Ordinal) < 0)
                {
                    stats.TargetShaderInstancingPragmaFailures++;
                }
            }

            var uniqueTargetMaterials = new HashSet<Material>();
            var renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null)
                {
                    continue;
                }

                if (renderer is SpriteRenderer)
                {
                    stats.SpriteRendererNonTargetCount++;
                    continue;
                }

                if (renderer is SkinnedMeshRenderer)
                {
                    stats.SkinnedMeshRendererNonTargetCount++;
                    continue;
                }

                if (renderer is not MeshRenderer)
                {
                    continue;
                }

                stats.MeshRendererCount++;
                var hasTargetMaterial = false;
                var targetSlotsInstanced = true;
                var materials = renderer.sharedMaterials;
                for (var materialIndex = 0; materialIndex < materials.Length; materialIndex++)
                {
                    var material = materials[materialIndex];
                    if (material == null || !IsHd2dAutonomousP1SrpBatcherGpuResidentTargetShader(material.shader))
                    {
                        continue;
                    }

                    hasTargetMaterial = true;
                    uniqueTargetMaterials.Add(material);
                    stats.TargetMaterialSlots++;
                    if (material.enableInstancing)
                    {
                        stats.TargetInstancedMaterialSlots++;
                    }
                    else
                    {
                        targetSlotsInstanced = false;
                    }
                }

                if (!hasTargetMaterial)
                {
                    continue;
                }

                stats.TargetMeshRenderers++;
                if (renderer.HasPropertyBlock())
                {
                    if (IsHd2dAutonomousP1SrpBatcherIntentionalPropertyBlockRenderer(renderer))
                    {
                        stats.TargetMeshRendererIntentionalPropertyBlockCarveouts++;
                    }
                    else
                    {
                        stats.TargetMeshRendererPropertyBlockBreakers++;
                    }

                    targetSlotsInstanced = false;
                }

                if (targetSlotsInstanced)
                {
                    stats.TargetMeshRenderersEligibleForResidentDrawer++;
                }
            }

            stats.UniqueTargetMaterials = uniqueTargetMaterials.Count;
            return stats;
        }

        private static IEnumerable<Material> LoadHd2dAutonomousP1SrpBatcherGpuResidentMaterials()
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

        private static bool IsHd2dAutonomousP1SrpBatcherGpuResidentTargetShader(Shader shader)
        {
            if (shader == null)
            {
                return false;
            }

            return string.Equals(shader.name, "Anemora/FastVS/SurfaceRampLit", StringComparison.Ordinal) ||
                   string.Equals(shader.name, "Anemora/FastVS/FoliageCardLit", StringComparison.Ordinal) ||
                   string.Equals(shader.name, "Anemora/FastVS/SpriteCardRampLit", StringComparison.Ordinal) ||
                   string.Equals(shader.name, "Anemora/FastVS/SpriteCardRampUnlit", StringComparison.Ordinal) ||
                   string.Equals(shader.name, "Anemora/FastVS/DepthGradientWater", StringComparison.Ordinal);
        }

        private static bool IsHd2dAutonomousP1SrpBatcherIntentionalPropertyBlockRenderer(Renderer renderer)
        {
            if (renderer == null)
            {
                return false;
            }

            var surface = renderer.GetComponent<FastVsHd2dSurfaceProfile>();
            if (surface != null)
            {
                return true;
            }

            var material = renderer.sharedMaterial;
            var materialRole = material != null ? material.GetTag("AnemoraFastVsHd2dRole", false, string.Empty) : string.Empty;
            if (string.Equals(materialRole, "SpriteCard", StringComparison.Ordinal) ||
                string.Equals(materialRole, "PaperCard", StringComparison.Ordinal) ||
                string.Equals(materialRole, "OverlayGlow", StringComparison.Ordinal) ||
                string.Equals(materialRole, "ContactShadow", StringComparison.Ordinal) ||
                string.Equals(materialRole, "PortalWindow", StringComparison.Ordinal))
            {
                return true;
            }

            var name = renderer.gameObject.name;
            if (name.StartsWith("Current_", StringComparison.Ordinal) ||
                name.StartsWith("Past_", StringComparison.Ordinal))
            {
                return true;
            }

            return name.Contains("Realtime", StringComparison.Ordinal) ||
                   name.Contains("Shadow", StringComparison.Ordinal) ||
                   name.Contains("TimeWindow", StringComparison.Ordinal) ||
                   name.Contains("Portal", StringComparison.Ordinal) ||
                   name.Contains("Aperture", StringComparison.Ordinal) ||
                   name.Contains("Stage7", StringComparison.Ordinal) ||
                   name.Contains("Vfx", StringComparison.Ordinal) ||
                   name.Contains("Mote", StringComparison.Ordinal) ||
                   name.Contains("Atmosphere", StringComparison.Ordinal) ||
                   name.Contains("WarmAnchor", StringComparison.Ordinal);
        }

        private static IEnumerable<string> Hd2dAutonomousP1SrpBatcherShaderPaths()
        {
            yield return Hd2dAutonomousP1SrpFoliageShaderPath;
            yield return Hd2dAutonomousP1SrpSpriteLitShaderPath;
            yield return Hd2dAutonomousP1SrpSpriteUnlitShaderPath;
            yield return Hd2dAutonomousP1SrpSurfaceShaderPath;
            yield return Hd2dAutonomousP1SrpWaterShaderPath;
        }

        private static void ValidateHd2dAutonomousP1SrpBatcherShaderSource(string shaderPath)
        {
            var source = File.ReadAllText(shaderPath);
            ValidateSourceToken(source, "CBUFFER_START(UnityPerMaterial)", shaderPath);
            ValidateSourceToken(source, "#pragma multi_compile_instancing", shaderPath);
            if (source.IndexOf("UsePass \"Universal Render Pipeline/Lit", StringComparison.Ordinal) >= 0)
            {
                throw new InvalidOperationException($"House slice validation failed: P1-50 target shader must not depend on URP Lit UsePass with a different UnityPerMaterial layout: {shaderPath}.");
            }
        }

        private static void ValidateHd2dAutonomousP1SrpBatcherGpuResidentVisualParity(Hd2dAutonomousP1DepthPrimingDiffMetrics metrics)
        {
            if (metrics.SampleCount <= 0 || metrics.ChangedPercent > 0.10f || metrics.MeanRgbDelta > 0.08f)
            {
                throw new InvalidOperationException($"Fast VS autonomous P1-50 SRP/GPU Resident capture failed: GRD Disabled/Instanced beauty parity drifted too far, changed={metrics.ChangedPercent:0.###}%, mean={metrics.MeanRgbDelta:0.###}.");
            }
        }

        private static void WriteHd2dAutonomousP1SrpBatcherGpuResidentReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            Hd2dAutonomousP1SrpBatcherGpuResidentStats stats,
            Hd2dAutonomousP1DepthPrimingDiffMetrics visualDiff)
        {
            var lines = new List<string>
            {
                "# P1-50 SRP Batcher / GPU Resident Drawer Review",
                string.Empty,
                "- Scope: auto-safe rendering settings plus shader/material audit for diorama and foliage-card MeshRenderers.",
                "- Note: the automated report is a settings/static eligibility proxy. Tom should still use Frame Debugger and Profiler for the exact long-batch view and RenderLoop timing delta.",
                "- Capture isolation: Depth Priming is disabled for the GRD off/on beauty pair to avoid conflating this pass with the P1-49 depth-priming cutoff.",
                $"- Auto-safe cutoff: {stats.TargetMeshRendererIntentionalPropertyBlockCarveouts} target MeshRenderers remain as intentional MaterialPropertyBlock carveouts for realtime lighting/shadow grading; {stats.TargetMeshRendererPropertyBlockBreakers} unresolved target MPB breakers remain.",
                $"- GRD visual parity cutoff: Disabled vs Instanced changed {visualDiff.ChangedPercent:0.###}% with mean RGB delta {visualDiff.MeanRgbDelta:0.###}; this run is documented for review rather than accepted as a full visual parity pass.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| URP pipeline asset | `{UniversalRenderPipelineAssetPath}` |",
                $"| Renderer asset | `{UniversalRenderPipelineRendererAssetPath}` |",
                $"| Renderer path | {stats.RendererMode} |",
                $"| SRP Batcher | {FormatBool(stats.SrpBatcherEnabled)} |",
                $"| GPU Resident Drawer | {stats.GpuResidentDrawerMode} |",
                $"| BatchRendererGroup Variants | {stats.BrgStrippingMode} |",
                $"| BatchRendererGroup buffer target | {stats.BatchRendererGroupBufferTarget} |",
                $"| Compute shaders supported | {FormatBool(stats.ComputeShadersSupported)} |",
                $"| SRP support check | {FormatBool(stats.SrpReportsGpuResidentSupported)} ({stats.SrpGpuResidentSupportMessage}) |",
                string.Empty,
                "| Mesh/material audit | Count |",
                "|---|---:|",
                $"| MeshRenderers scanned | {stats.MeshRendererCount} |",
                $"| Target MeshRenderers | {stats.TargetMeshRenderers} |",
                $"| Target MeshRenderers eligible for Resident Drawer proxy | {stats.TargetMeshRenderersEligibleForResidentDrawer} |",
                $"| Target MeshRenderer intentional PropertyBlock carveouts | {stats.TargetMeshRendererIntentionalPropertyBlockCarveouts} |",
                $"| Target MeshRenderer unresolved PropertyBlock breakers | {stats.TargetMeshRendererPropertyBlockBreakers} |",
                $"| Target material slots with instancing | {stats.TargetInstancedMaterialSlots} / {stats.TargetMaterialSlots} |",
                $"| Unique target materials | {stats.UniqueTargetMaterials} |",
                $"| Target shader CBUFFER failures | {stats.TargetShaderCbufferFailures} |",
                $"| Target shader instancing pragma failures | {stats.TargetShaderInstancingPragmaFailures} |",
                $"| SpriteRenderer non-targets documented | {stats.SpriteRendererNonTargetCount} |",
                $"| SkinnedMeshRenderer non-targets documented | {stats.SkinnedMeshRendererNonTargetCount} |",
                string.Empty,
                "| Beauty parity | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                visualDiff.ToReportRow("GRD Disabled vs Instanced Drawing"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Central Plaza reference with GPU Resident Drawer temporarily Disabled |",
                $"| `{screenshotFiles[1]}` | Same Central Plaza view with GPU Resident Drawer Instanced Drawing |",
                $"| `{screenshotFiles[2]}` | MeshRenderer-heavy foliage/diorama eligibility view |",
                $"| `{screenshotFiles[3]}` | Sprite/billboard guard view documenting non-target renderer classes |",
                string.Empty,
                "| Shader path | SRP/instancing contract |",
                "|---|---|",
                $"| `{Hd2dAutonomousP1SrpFoliageShaderPath}` | `UnityPerMaterial` CBUFFER plus instancing variants across forward/shadow/depth passes |",
                $"| `{Hd2dAutonomousP1SrpSpriteLitShaderPath}` | `UnityPerMaterial` CBUFFER plus instancing variants across forward/shadow/depth passes |",
                $"| `{Hd2dAutonomousP1SrpSpriteUnlitShaderPath}` | `UnityPerMaterial` CBUFFER plus instancing variants across forward/shadow passes |",
                $"| `{Hd2dAutonomousP1SrpSurfaceShaderPath}` | Explicit shadow/depth passes with matching `UnityPerMaterial` layout; no URP Lit `UsePass` |",
                $"| `{Hd2dAutonomousP1SrpWaterShaderPath}` | Transparent water material properties folded into `UnityPerMaterial`; instancing variant retained for mesh renderer compatibility |"
            };

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllText(Path.Combine(outputDirectory, "srp_batcher_gpu_resident_drawer_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private struct Hd2dAutonomousP1SrpBatcherGpuResidentStats
        {
            public bool SrpBatcherEnabled;
            public string GpuResidentDrawerMode;
            public string BrgStrippingMode;
            public string RendererMode;
            public string BatchRendererGroupBufferTarget;
            public bool ComputeShadersSupported;
            public bool RawBufferPathSupported;
            public bool SrpReportsGpuResidentSupported;
            public string SrpGpuResidentSupportMessage;
            public int MeshRendererCount;
            public int TargetMeshRenderers;
            public int TargetMeshRenderersEligibleForResidentDrawer;
            public int TargetMeshRendererIntentionalPropertyBlockCarveouts;
            public int TargetMeshRendererPropertyBlockBreakers;
            public int TargetMaterialSlots;
            public int TargetInstancedMaterialSlots;
            public int UniqueTargetMaterials;
            public int TargetShaderCbufferFailures;
            public int TargetShaderInstancingPragmaFailures;
            public int SpriteRendererNonTargetCount;
            public int SkinnedMeshRendererNonTargetCount;
        }
    }
}
