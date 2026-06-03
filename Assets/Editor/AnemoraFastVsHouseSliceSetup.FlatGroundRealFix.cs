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
        private const string Hd2dFlatGroundCurrentPathMaterialId = "current_path_flat_ground_value";
        private const string Hd2dFlatGroundCurrentGrassMaterialId = "current_grass_flat_ground_value";
        private const string Hd2dFlatGroundPastPathMaterialId = "past_path_flat_ground_value";
        private const string Hd2dFlatGroundPastGrassMaterialId = "past_grass_flat_ground_value";
        private const string Hd2dFlatGroundCurrentPathTextureId = "current_path_flat_ground_value_plate";
        private const string Hd2dFlatGroundCurrentGrassTextureId = "current_grass_flat_ground_value_plate";
        private const string Hd2dFlatGroundPastPathTextureId = "past_path_flat_ground_value_plate";
        private const string Hd2dFlatGroundPastGrassTextureId = "past_grass_flat_ground_value_plate";
        private static readonly string Hd2dFlatGroundCurrentPathMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dFlatGroundCurrentPathMaterialId + ".mat";
        private static readonly string Hd2dFlatGroundCurrentGrassMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dFlatGroundCurrentGrassMaterialId + ".mat";
        private static readonly string Hd2dFlatGroundPastPathMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dFlatGroundPastPathMaterialId + ".mat";
        private static readonly string Hd2dFlatGroundPastGrassMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dFlatGroundPastGrassMaterialId + ".mat";
        private static readonly string Hd2dFlatGroundCurrentPathTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dFlatGroundCurrentPathTextureId + ".asset";
        private static readonly string Hd2dFlatGroundCurrentGrassTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dFlatGroundCurrentGrassTextureId + ".asset";
        private static readonly string Hd2dFlatGroundPastPathTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dFlatGroundPastPathTextureId + ".asset";
        private static readonly string Hd2dFlatGroundPastGrassTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dFlatGroundPastGrassTextureId + ".asset";

        private static readonly Hd2dFlatGroundRealFixTarget[] Hd2dFlatGroundRealFixTargets =
        {
            new Hd2dFlatGroundRealFixTarget("Current_CentralPlaza_Chapter1_D2_PlazaFloor", MaterialDirectory + "/FastVS_House_current_path.mat", Hd2dFlatGroundCurrentPathMaterialPath, "current_path"),
            new Hd2dFlatGroundRealFixTarget("Current_CentralPlaza_Chapter1_D2_D3RoadJoin", MaterialDirectory + "/FastVS_House_current_path.mat", Hd2dFlatGroundCurrentPathMaterialPath, "current_path"),
            new Hd2dFlatGroundRealFixTarget("Current_CentralPlaza_Chapter1_D2_D3DiagonalRoadApron", MaterialDirectory + "/FastVS_House_current_path.mat", Hd2dFlatGroundCurrentPathMaterialPath, "current_path"),
            new Hd2dFlatGroundRealFixTarget("Current_CentralPlaza_Chapter1_D2_D3OuterRoadShoulder", MaterialDirectory + "/FastVS_House_current_grass.mat", Hd2dFlatGroundCurrentGrassMaterialPath, "current_grass"),
            new Hd2dFlatGroundRealFixTarget("Current_CentralPlaza_Chapter1_D3_ExitShoulderUpper", MaterialDirectory + "/FastVS_House_current_grass.mat", Hd2dFlatGroundCurrentGrassMaterialPath, "current_grass"),
            new Hd2dFlatGroundRealFixTarget("Current_CentralPlaza_Chapter1_D3_ExitShoulderLower", MaterialDirectory + "/FastVS_House_current_grass.mat", Hd2dFlatGroundCurrentGrassMaterialPath, "current_grass"),
            new Hd2dFlatGroundRealFixTarget("Current_CentralPlaza_Chapter1_D2_GrassBottomRight", MaterialDirectory + "/FastVS_House_current_grass.mat", Hd2dFlatGroundCurrentGrassMaterialPath, "current_grass"),
            new Hd2dFlatGroundRealFixTarget("Current_CentralPlaza_Chapter1_D2_RoadShoulder", MaterialDirectory + "/FastVS_House_current_grass.mat", Hd2dFlatGroundCurrentGrassMaterialPath, "current_grass"),
            new Hd2dFlatGroundRealFixTarget("Past_CentralPlaza_Chapter1_D2_PlazaFloor", MaterialDirectory + "/FastVS_House_past_path.mat", Hd2dFlatGroundPastPathMaterialPath, "past_path"),
            new Hd2dFlatGroundRealFixTarget("Past_CentralPlaza_Chapter1_D2_D3RoadJoin", MaterialDirectory + "/FastVS_House_past_path.mat", Hd2dFlatGroundPastPathMaterialPath, "past_path"),
            new Hd2dFlatGroundRealFixTarget("Past_CentralPlaza_Chapter1_D2_D3DiagonalRoadApron", MaterialDirectory + "/FastVS_House_past_path.mat", Hd2dFlatGroundPastPathMaterialPath, "past_path"),
            new Hd2dFlatGroundRealFixTarget("Past_CentralPlaza_Chapter1_D2_D3OuterRoadShoulder", MaterialDirectory + "/FastVS_House_past_grass.mat", Hd2dFlatGroundPastGrassMaterialPath, "past_grass"),
            new Hd2dFlatGroundRealFixTarget("Past_CentralPlaza_Chapter1_D3_ExitShoulderUpper", MaterialDirectory + "/FastVS_House_past_grass.mat", Hd2dFlatGroundPastGrassMaterialPath, "past_grass"),
            new Hd2dFlatGroundRealFixTarget("Past_CentralPlaza_Chapter1_D3_ExitShoulderLower", MaterialDirectory + "/FastVS_House_past_grass.mat", Hd2dFlatGroundPastGrassMaterialPath, "past_grass"),
            new Hd2dFlatGroundRealFixTarget("Past_CentralPlaza_Chapter1_D2_GrassBottomRight", MaterialDirectory + "/FastVS_House_past_grass.mat", Hd2dFlatGroundPastGrassMaterialPath, "past_grass"),
            new Hd2dFlatGroundRealFixTarget("Past_CentralPlaza_Chapter1_D2_RoadShoulder", MaterialDirectory + "/FastVS_House_past_grass.mat", Hd2dFlatGroundPastGrassMaterialPath, "past_grass")
        };

        private struct Hd2dFlatGroundRealFixTarget
        {
            public readonly string ObjectName;
            public readonly string BaselineMaterialPath;
            public readonly string FixedMaterialPath;
            public readonly string BaselineToken;

            public Hd2dFlatGroundRealFixTarget(string objectName, string baselineMaterialPath, string fixedMaterialPath, string baselineToken)
            {
                ObjectName = objectName;
                BaselineMaterialPath = baselineMaterialPath;
                FixedMaterialPath = fixedMaterialPath;
                BaselineToken = baselineToken;
            }
        }

        public static void CaptureHd2dAutonomousFlatGroundRealFixBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHouseLightingDirector>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || director == null || realtimeRig == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous flat ground real fix capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousFlatGroundRealFix();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("flat_ground_real_fix");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_current_flat_ground_baseline_materials.png",
                "02_current_flat_ground_value_materials.png",
                "03_past_flat_ground_baseline_materials.png",
                "04_past_flat_ground_value_materials.png"
            };

            var previousCullingMask = camera.cullingMask;
            try
            {
                CaptureHd2dAutonomousFlatGroundRealFixShot(controller, visibility, guide, director, realtimeRig, camera, false, false, previousCullingMask, outputDirectory, screenshotFiles[0]);
                CaptureHd2dAutonomousFlatGroundRealFixShot(controller, visibility, guide, director, realtimeRig, camera, true, false, previousCullingMask, outputDirectory, screenshotFiles[1]);
                CaptureHd2dAutonomousFlatGroundRealFixShot(controller, visibility, guide, director, realtimeRig, camera, false, true, previousCullingMask, outputDirectory, screenshotFiles[2]);
                CaptureHd2dAutonomousFlatGroundRealFixShot(controller, visibility, guide, director, realtimeRig, camera, true, true, previousCullingMask, outputDirectory, screenshotFiles[3]);
            }
            finally
            {
                camera.cullingMask = previousCullingMask;
                ApplyHd2dAutonomousFlatGroundRealFixAssignments(true);
                AssetDatabase.SaveAssets();
            }

            var currentDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                4);
            var pastDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[2]),
                Path.Combine(outputDirectory, screenshotFiles[3]),
                4);
            WriteHd2dAutonomousFlatGroundRealFixReviewReport(outputDirectory, screenshotFiles, currentDiff, pastDiff);

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous flat ground real fix review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousFlatGroundRealFix()
        {
            EnsureHd2dAutonomousFlatGroundRealFixMaterials();
            ApplyHd2dAutonomousFlatGroundRealFixAssignments(true);
            AssetDatabase.SaveAssets();
        }

        private static void EnsureHd2dAutonomousFlatGroundRealFixMaterials()
        {
            ConfigureHd2dFlatGroundMaterial(
                PaintedSurfaceMaterial(Hd2dFlatGroundCurrentPathMaterialId, Hd2dFlatGroundCurrentPathTextureId, 192, 192, SampleHd2dFlatGroundCurrentPathPixel, false, new Vector2(2.2f, 2.2f)),
                true,
                false);
            ConfigureHd2dFlatGroundMaterial(
                PaintedSurfaceMaterial(Hd2dFlatGroundCurrentGrassMaterialId, Hd2dFlatGroundCurrentGrassTextureId, 192, 192, SampleHd2dFlatGroundCurrentGrassPixel, false, new Vector2(3.0f, 3.0f)),
                true,
                true);
            ConfigureHd2dFlatGroundMaterial(
                PaintedSurfaceMaterial(Hd2dFlatGroundPastPathMaterialId, Hd2dFlatGroundPastPathTextureId, 192, 192, SampleHd2dFlatGroundPastPathPixel, false, new Vector2(2.2f, 2.2f)),
                false,
                false);
            ConfigureHd2dFlatGroundMaterial(
                PaintedSurfaceMaterial(Hd2dFlatGroundPastGrassMaterialId, Hd2dFlatGroundPastGrassTextureId, 192, 192, SampleHd2dFlatGroundPastGrassPixel, false, new Vector2(3.0f, 3.0f)),
                false,
                true);
        }

        private static void ConfigureHd2dFlatGroundMaterial(Material material, bool current, bool grass)
        {
            if (material == null)
            {
                return;
            }

            var valueTint = current
                ? (grass ? new Color(0.84f, 1.10f, 0.66f, 1f) : new Color(1.16f, 1.02f, 0.72f, 1f))
                : (grass ? new Color(0.72f, 1.08f, 0.66f, 1f) : new Color(0.88f, 1.08f, 0.72f, 1f));
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", valueTint);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", valueTint);
            }

            if (material.HasProperty("_UseGroundMacroVariation"))
            {
                material.SetFloat("_UseGroundMacroVariation", 1f);
            }

            if (material.HasProperty("_GroundMacroVariationScale"))
            {
                material.SetFloat("_GroundMacroVariationScale", grass ? 0.13f : 0.105f);
            }

            if (material.HasProperty("_GroundMacroVariationStrength"))
            {
                material.SetFloat("_GroundMacroVariationStrength", grass ? 0.34f : 0.30f);
            }

            if (material.HasProperty("_GroundMacroHueStrength"))
            {
                material.SetFloat("_GroundMacroHueStrength", grass ? 0.10f : 0.070f);
            }

            if (material.HasProperty("_UsePainterlyGroundValue"))
            {
                material.SetFloat("_UsePainterlyGroundValue", 1f);
            }

            if (material.HasProperty("_PainterlyGroundCenter"))
            {
                material.SetVector("_PainterlyGroundCenter", new Vector4(Chapter1AriaStreetMapCenter.x, Chapter1AriaStreetMapCenter.z, 0f, 0f));
            }

            if (material.HasProperty("_PainterlyGroundRadius"))
            {
                material.SetFloat("_PainterlyGroundRadius", 34f);
            }

            if (material.HasProperty("_PainterlyGroundValueStrength"))
            {
                material.SetFloat("_PainterlyGroundValueStrength", grass ? 0.24f : 0.28f);
            }

            if (material.HasProperty("_PainterlyGroundAOStrength"))
            {
                material.SetFloat("_PainterlyGroundAOStrength", grass ? 0.28f : 0.32f);
            }

            if (material.HasProperty("_PainterlyGroundWarmTint"))
            {
                material.SetColor("_PainterlyGroundWarmTint", current ? new Color(1.06f, 1.03f, 0.92f, 1f) : new Color(1.00f, 1.05f, 0.90f, 1f));
            }

            if (material.HasProperty("_PainterlyGroundCoolTint"))
            {
                material.SetColor("_PainterlyGroundCoolTint", current ? new Color(0.78f, 0.86f, 0.98f, 1f) : new Color(0.76f, 0.91f, 0.80f, 1f));
            }

            if (material.HasProperty("_ShadowTextureStrength"))
            {
                material.SetFloat("_ShadowTextureStrength", grass ? 0.32f : 0.26f);
            }

            if (material.HasProperty("_Smoothness"))
            {
                material.SetFloat("_Smoothness", 0.12f);
            }

            material.enableInstancing = true;
            EditorUtility.SetDirty(material);
        }

        private static void ApplyHd2dAutonomousFlatGroundRealFixAssignments(bool enabled)
        {
            EnsureHd2dAutonomousFlatGroundRealFixMaterials();
            for (var i = 0; i < Hd2dFlatGroundRealFixTargets.Length; i++)
            {
                var target = Hd2dFlatGroundRealFixTargets[i];
                var obj = FindSceneObjectIncludingInactive(target.ObjectName);
                var renderer = obj != null ? obj.GetComponent<Renderer>() : null;
                if (renderer == null)
                {
                    continue;
                }

                var materialPath = enabled ? target.FixedMaterialPath : target.BaselineMaterialPath;
                var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                if (material == null)
                {
                    throw new InvalidOperationException($"House slice flat ground real fix failed: missing material {materialPath}");
                }

                renderer.sharedMaterial = material;
                EditorUtility.SetDirty(renderer);
            }
        }

        private static void ValidateHd2dAutonomousFlatGroundRealFix()
        {
            EnsureHd2dAutonomousFlatGroundRealFixMaterials();
            ValidateGeneratedRepeatTextureAsset(Hd2dFlatGroundCurrentPathTextureId, 192, 192, 80);
            ValidateGeneratedRepeatTextureAsset(Hd2dFlatGroundCurrentGrassTextureId, 192, 192, 80);
            ValidateGeneratedRepeatTextureAsset(Hd2dFlatGroundPastPathTextureId, 192, 192, 80);
            ValidateGeneratedRepeatTextureAsset(Hd2dFlatGroundPastGrassTextureId, 192, 192, 80);
            ValidateGeneratedSurfaceMaterialTexture(Hd2dFlatGroundCurrentPathMaterialId, Hd2dFlatGroundCurrentPathTextureId);
            ValidateGeneratedSurfaceMaterialTexture(Hd2dFlatGroundCurrentGrassMaterialId, Hd2dFlatGroundCurrentGrassTextureId);
            ValidateGeneratedSurfaceMaterialTexture(Hd2dFlatGroundPastPathMaterialId, Hd2dFlatGroundPastPathTextureId);
            ValidateGeneratedSurfaceMaterialTexture(Hd2dFlatGroundPastGrassMaterialId, Hd2dFlatGroundPastGrassTextureId);

            for (var i = 0; i < Hd2dFlatGroundRealFixTargets.Length; i++)
            {
                var target = Hd2dFlatGroundRealFixTargets[i];
                var obj = FindSceneObjectIncludingInactive(target.ObjectName);
                var renderer = obj != null ? obj.GetComponent<Renderer>() : null;
                if (renderer == null || renderer.sharedMaterial == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: flat ground real fix target is missing renderer/material: {target.ObjectName}");
                }

                var fixedMaterialPath = AssetDatabase.GetAssetPath(renderer.sharedMaterial);
                if (!string.Equals(fixedMaterialPath, target.FixedMaterialPath, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException($"House slice validation failed: flat ground real fix target is not using the fixed material: {target.ObjectName} -> {fixedMaterialPath}");
                }

                if (renderer.sharedMaterial.shader == null || !string.Equals(renderer.sharedMaterial.shader.name, SurfaceRampLitShaderName, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException($"House slice validation failed: flat ground real fix material must use {SurfaceRampLitShaderName}: {target.ObjectName}");
                }

                if (renderer.sharedMaterial.name.IndexOf(target.BaselineToken, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    throw new InvalidOperationException($"House slice validation failed: flat ground real fix material must retain legacy token {target.BaselineToken}: {target.ObjectName}");
                }
            }
        }

        private static void CaptureHd2dAutonomousFlatGroundRealFixShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsHouseLightingDirector director,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            bool enabled,
            bool pastTimeline,
            int previousCullingMask,
            string outputDirectory,
            string fileName)
        {
            ApplyHd2dAutonomousFlatGroundRealFixAssignments(enabled);
            visibility.SetActiveAreaForReview(FastVsHouseArea.AriaStreet);
            director.ApplyAreaForReview(FastVsHouseArea.AriaStreet);
            var playerLocal = Chapter1AriaStreetMapCenter + new Vector3(-1.20f, 0.02f, 0.68f);
            var anchorLocal = Chapter1AriaStreetMapCenter + new Vector3(-2.25f, 0.08f, 2.75f);

            if (pastTimeline)
            {
                controller.ForcePlayerOtherTimeLocalForReview(playerLocal);
                var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
                var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
                var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
                camera.cullingMask = (previousCullingMask & ~currentBit) | otherBit | playerBit;
                PositionChapter1AllMapsCamera(
                    camera,
                    controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocal),
                    new Vector3(0f, 10.80f, -14.40f),
                    new Vector3(0f, 0.16f, 1.65f));
            }
            else
            {
                controller.ForcePlayerCurrentLocalForReview(playerLocal);
                camera.cullingMask = previousCullingMask;
                PositionChapter1AllMapsCamera(
                    camera,
                    controller.CurrentSpaceRootForReview.TransformPoint(anchorLocal),
                    new Vector3(0f, 10.80f, -14.40f),
                    new Vector3(0f, 0.16f, 1.65f));
            }

            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = 36f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 180f;
            ApplyStage7BokehFocusForReview(camera);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
        }

        private static Color SampleHd2dFlatGroundCurrentPathPixel(int x, int y)
        {
            return SampleHd2dFlatGroundValuePixel(x, y, false, false);
        }

        private static Color SampleHd2dFlatGroundCurrentGrassPixel(int x, int y)
        {
            return SampleHd2dFlatGroundValuePixel(x, y, true, false);
        }

        private static Color SampleHd2dFlatGroundPastPathPixel(int x, int y)
        {
            return SampleHd2dFlatGroundValuePixel(x, y, false, true);
        }

        private static Color SampleHd2dFlatGroundPastGrassPixel(int x, int y)
        {
            return SampleHd2dFlatGroundValuePixel(x, y, true, true);
        }

        private static Color SampleHd2dFlatGroundValuePixel(int x, int y, bool grass, bool past)
        {
            const int width = 192;
            const int height = 192;
            var u = x / (float)(width - 1);
            var v = y / (float)(height - 1);
            var broad = SampleSmoothValueNoise2D((x * 0.028f) + (past ? 33.7f : 11.4f), (y * 0.026f) + (grass ? 71.1f : 19.6f), grass ? 733 : 701);
            var mid = SampleSmoothValueNoise2D((x * 0.092f) + (past ? 9.8f : 45.2f), (y * 0.086f) + (grass ? 14.5f : 62.4f), grass ? 751 : 719);
            var fine = SampleSmoothValueNoise2D((x * 0.245f) + 8.35f, (y * 0.232f) + 29.75f, past ? 769 : 743);
            var radial = Mathf.Clamp01(Vector2.Distance(new Vector2(u, v), new Vector2(0.47f, 0.53f)) / 0.72f);
            var edge = Mathf.SmoothStep(0.34f, 1.0f, radial);

            Color low;
            Color midTone;
            Color high;
            if (grass)
            {
                low = past ? new Color(0.15f, 0.18f, 0.10f, 1f) : new Color(0.11f, 0.14f, 0.075f, 1f);
                midTone = past ? new Color(0.25f, 0.31f, 0.16f, 1f) : new Color(0.19f, 0.24f, 0.13f, 1f);
                high = past ? new Color(0.38f, 0.41f, 0.23f, 1f) : new Color(0.32f, 0.36f, 0.20f, 1f);
            }
            else
            {
                low = past ? new Color(0.25f, 0.21f, 0.17f, 1f) : new Color(0.31f, 0.28f, 0.22f, 1f);
                midTone = past ? new Color(0.40f, 0.35f, 0.28f, 1f) : new Color(0.48f, 0.44f, 0.34f, 1f);
                high = past ? new Color(0.56f, 0.51f, 0.38f, 1f) : new Color(0.68f, 0.62f, 0.46f, 1f);
            }

            var tone = LerpColor(low, high, Mathf.Clamp01(0.30f + broad * 0.44f + mid * 0.20f + (1f - edge) * 0.16f));
            tone = LerpColor(tone, midTone, 0.24f);
            var seam = !grass && (x % 24 <= 1 || y % 22 <= 1 || Mathf.Abs((x % 48) - (y % 38)) <= 1);
            var clump = grass && (mid > 0.64f || Hash01(x, y, past ? 787 : 779) > 0.88f);
            var pocket = fine > (grass ? 0.58f : 0.66f);

            if (seam)
            {
                tone = Darken(tone, past ? 0.18f : 0.15f);
            }

            if (clump)
            {
                tone = LerpColor(tone, past ? new Color(0.12f, 0.14f, 0.08f, 1f) : new Color(0.08f, 0.11f, 0.06f, 1f), 0.34f);
            }

            if (pocket)
            {
                tone = Darken(tone, grass ? 0.10f : 0.075f);
            }

            if (fine < 0.18f && !seam)
            {
                tone = Lighten(tone, grass ? 0.045f : 0.035f);
            }

            tone = Darken(tone, edge * (grass ? 0.10f : 0.13f));
            return ShadeSurface(tone, x, y, width, height, grass ? 0.028f : 0.020f, grass ? 0.030f : 0.024f);
        }

        private static void WriteHd2dAutonomousFlatGroundRealFixReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            Hd2dAutonomousP1DepthPrimingDiffMetrics currentDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics pastDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# Flat Ground Real Fix Review",
                string.Empty,
                "- Scope: replace the actually rendered large D2/D3 ground/path renderers with SurfaceRampLit materials that retain the legacy current_path/current_grass/past_path/past_grass tokens but use stronger baked tonal texture, macro variation, and AO-ready painterly settings.",
                "- Baseline A/B note: the off shots temporarily restore the original renderer materials; the on shots restore the fixed materials now assigned by scene setup.",
                string.Empty,
                "| A/B evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                currentDiff.ToReportRow("Current Aria Street flat ground baseline vs fixed materials"),
                pastDiff.ToReportRow("Past Aria Street flat ground baseline vs fixed materials"),
                string.Empty,
                "| Fixed material | Texture |",
                "|---|---|",
                $"| `{Hd2dFlatGroundCurrentPathMaterialPath}` | `{Hd2dFlatGroundCurrentPathTexturePath}` |",
                $"| `{Hd2dFlatGroundCurrentGrassMaterialPath}` | `{Hd2dFlatGroundCurrentGrassTexturePath}` |",
                $"| `{Hd2dFlatGroundPastPathMaterialPath}` | `{Hd2dFlatGroundPastPathTexturePath}` |",
                $"| `{Hd2dFlatGroundPastGrassMaterialPath}` | `{Hd2dFlatGroundPastGrassTexturePath}` |",
                string.Empty,
                "| Rendered target | Baseline material | Fixed material |",
                "|---|---|---|"
            };

            for (var i = 0; i < Hd2dFlatGroundRealFixTargets.Length; i++)
            {
                var target = Hd2dFlatGroundRealFixTargets[i];
                lines.Add($"| `{target.ObjectName}` | `{target.BaselineMaterialPath}` | `{target.FixedMaterialPath}` |");
            }

            lines.Add(string.Empty);
            lines.Add("| Screenshot | Purpose |");
            lines.Add("|---|---|");
            lines.Add($"| `{screenshotFiles[0]}` | Current Aria Street overview with original path/grass materials restored for baseline |");
            lines.Add($"| `{screenshotFiles[1]}` | Same current overview with fixed flat-ground value materials assigned to the actual renderers |");
            lines.Add($"| `{screenshotFiles[2]}` | Past Aria Street overview with original path/grass materials restored for baseline |");
            lines.Add($"| `{screenshotFiles[3]}` | Same past overview with fixed flat-ground value materials assigned to the actual renderers |");
            lines.Add(string.Empty);
            lines.Add("Recommendation: keep the renderer-level material replacement because it proves the visible ground path now reaches the pixels. Tom should still tune the final texture palette, AO mask shape, and whether the broader approach should later move back into a single reusable master ground shader/vertex-paint workflow.");

            File.WriteAllLines(Path.Combine(outputDirectory, "flat_ground_real_fix_review.md"), lines, Encoding.UTF8);
        }
    }
}
