using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private const string Hd2dAutonomousP1BuildingSurfaceDetailProfilePath = "Assets/Settings/FastVS_HD2D_P1_BuildingSurfaceDetailProfile.asset";
        private const string Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId = "hd2d_p1_building_roof_tile_edge";
        private const string Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId = "hd2d_p1_building_wall_timber_frame";
        private const string Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId = "hd2d_p1_building_wall_cavity_accent";
        private const string Hd2dAutonomousP1BuildingSurfaceDetailRoofNormalId = "hd2d_p1_building_roof_tile_detail_normal";
        private const string Hd2dAutonomousP1BuildingSurfaceDetailWallNormalId = "hd2d_p1_building_wall_band_detail_normal";
        private const string Hd2dAutonomousP1BuildingSurfaceDetailRoofHeightId = "hd2d_p1_building_roof_tile_detail_height";
        private const string Hd2dAutonomousP1BuildingSurfaceDetailWallHeightId = "hd2d_p1_building_wall_band_detail_height";

        public static void CaptureHd2dAutonomousP1Item43RoofWallSurfaceDetailingBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            var streetRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1ModularBuildingKitStreetRootName);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || streetRoot == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P1-43 roof/wall surface detail capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1BuildingSurfaceDetailing();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("roof_wall_surface_detailing");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_detail_off_facade_baseline.png",
                "02_detail_on_facade_rows_beams.png",
                "03_roof_tile_rows_micro_shadow.png",
                "04_wall_timber_cavity_banding.png",
                "05_surface_detail_overview.png"
            };
            var shotRows = new List<string>();
            var previousMask = camera.cullingMask;
            try
            {
                guide.SetMovementFrozen(true);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.AriaStreet);
                controller.ForcePlayerCurrentLocalForReview(Chapter1AriaStreetMapCenter + new Vector3(-10.5f, 0.04f, 5.7f));
                guide.ApplyActiveTimeIsolationForReview();
                realtimeRig.ApplyNowForReview();
                Physics.SyncTransforms();

                CaptureHd2dAutonomousP1BuildingSurfaceDetailReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    streetRoot,
                    false,
                    Chapter1AriaStreetMapCenter + new Vector3(-9.0f, 0.02f, 6.1f),
                    new Vector3(-0.65f, 4.85f, -7.40f),
                    new Vector3(0.05f, 1.05f, 0.40f),
                    32f,
                    outputDirectory,
                    screenshotFiles[0],
                    "detail-off wall/facade baseline",
                    shotRows);

                CaptureHd2dAutonomousP1BuildingSurfaceDetailReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    streetRoot,
                    true,
                    Chapter1AriaStreetMapCenter + new Vector3(-9.0f, 0.02f, 6.1f),
                    new Vector3(-0.65f, 4.85f, -7.40f),
                    new Vector3(0.05f, 1.05f, 0.40f),
                    32f,
                    outputDirectory,
                    screenshotFiles[1],
                    "detail-on wall/facade rows and beams",
                    shotRows);

                CaptureHd2dAutonomousP1BuildingSurfaceDetailReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    streetRoot,
                    true,
                    Chapter1AriaStreetMapCenter + new Vector3(3.2f, 0.02f, 6.7f),
                    new Vector3(-2.05f, 5.35f, -8.45f),
                    new Vector3(-0.10f, 1.25f, 0.55f),
                    31f,
                    outputDirectory,
                    screenshotFiles[2],
                    "roof tile rows and micro-shadow",
                    shotRows);

                CaptureHd2dAutonomousP1BuildingSurfaceDetailReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    streetRoot,
                    true,
                    Chapter1AriaStreetMapCenter + new Vector3(-9.0f, 0.02f, 6.1f),
                    new Vector3(-0.65f, 4.85f, -7.40f),
                    new Vector3(0.05f, 1.05f, 0.40f),
                    32f,
                    outputDirectory,
                    screenshotFiles[3],
                    "timber frame and wall banding",
                    shotRows);

                CaptureHd2dAutonomousP1BuildingSurfaceDetailReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    streetRoot,
                    true,
                    Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 6.2f),
                    new Vector3(0.75f, 21.20f, -18.60f),
                    new Vector3(0.20f, 0.72f, 2.75f),
                    48f,
                    outputDirectory,
                    screenshotFiles[4],
                    "surface detail overview",
                    shotRows);

                ValidateHd2dAutonomousP1BuildingSurfaceDetailReviewPairDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1], "detail-off-vs-on");
                WriteHd2dAutonomousP1BuildingSurfaceDetailReviewReport(outputDirectory, screenshotFiles, shotRows);
            }
            finally
            {
                camera.cullingMask = previousMask;
                SetHd2dAutonomousP1BuildingSurfaceDetailRenderersVisible(streetRoot, true);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                guide.SetMovementFrozen(false);
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P1-43 roof/wall surface detail review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void AddHd2dAutonomousP1BuildingSurfaceDetailing(
            Transform root,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            IReadOnlyDictionary<string, Material> baseMaterials,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            if (root == null || recipe == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP1BuildingSurfaceDetailProfileAsset();
            var materials = EnsureHd2dAutonomousP1BuildingSurfaceDetailMaterials(profile);
            var grid = 1f;
            var width = recipe.FootprintForReview.x * grid;
            var depth = recipe.FootprintForReview.y * grid;
            const float storyHeight = 1.14f;
            var topY = recipe.FloorsForReview * storyHeight + 0.12f;

            AddHd2dAutonomousP1BuildingSurfaceRoofTileRows(root, recipe, profile, materials, width, depth, topY, renderers, ref moduleCount);
            AddHd2dAutonomousP1BuildingSurfaceWallFrames(root, recipe, profile, materials, width, depth, storyHeight, renderers, ref moduleCount);
            _ = baseMaterials;
        }

        private static void ValidateHd2dAutonomousP1BuildingSurfaceDetailing()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dBuildingSurfaceDetailProfile>(Hd2dAutonomousP1BuildingSurfaceDetailProfilePath);
            var streetRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1ModularBuildingKitStreetRootName);
            if (profile == null ||
                streetRoot == null ||
                !profile.GeneratedNormalMapsReadyForReview ||
                !profile.ConservativeReviewModeForReview ||
                !profile.RequiresTomArtApprovalForReview ||
                profile.LayerCountForReview < 2)
            {
                throw new InvalidOperationException("House slice validation failed: P1-43 needs a conservative TOM-gated building surface detail profile, street root, and generated normal/height-map prep.");
            }

            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dBuildingSurfaceDetailMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(marker => marker != null && marker.transform.IsChildOf(streetRoot.transform))
                .ToArray();
            var roofRows = markers.Count(marker => string.Equals(marker.DetailKindForReview, "roof_tile_row", StringComparison.Ordinal));
            var timberFrames = markers.Count(marker => string.Equals(marker.DetailKindForReview, "timber_frame", StringComparison.Ordinal));
            var cavityEdges = markers.Count(marker => string.Equals(marker.DetailKindForReview, "cavity_edge", StringComparison.Ordinal));
            if (markers.Length < 80 || roofRows < 45 || timberFrames < 25 || cavityEdges < 8)
            {
                throw new InvalidOperationException($"House slice validation failed: P1-43 detail marker counts are too low (all={markers.Length}, roof={roofRows}, timber={timberFrames}, cavity={cavityEdges}).");
            }

            if (markers.Any(marker => !marker.IsReadyForReview))
            {
                throw new InvalidOperationException("House slice validation failed: P1-43 all detail markers must link to the profile and generated normal-map data.");
            }

            foreach (var materialId in new[]
            {
                Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId,
                Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId,
                Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId
            })
            {
                ValidateHd2dAutonomousP1BuildingSurfaceDetailMaterial(materialId);
            }

            foreach (var textureId in new[]
            {
                Hd2dAutonomousP1BuildingSurfaceDetailRoofNormalId,
                Hd2dAutonomousP1BuildingSurfaceDetailWallNormalId,
                Hd2dAutonomousP1BuildingSurfaceDetailRoofHeightId,
                Hd2dAutonomousP1BuildingSurfaceDetailWallHeightId
            })
            {
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(textureId));
                if (texture == null || texture.width != 64 || texture.height != 64 || texture.wrapMode != TextureWrapMode.Repeat)
                {
                    throw new InvalidOperationException($"House slice validation failed: P1-43 generated normal/height texture is missing or has invalid settings: {textureId}.");
                }
            }

            var runtimeProfilePath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dBuildingSurfaceDetailProfile.cs");
            var runtimeMarkerPath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dBuildingSurfaceDetailMarker.cs");
            var editorSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.P1BuildingSurfaceDetailing.cs");
            var modularSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.P1ModularBuildingKit.cs");
            ValidateSourceToken(File.ReadAllText(runtimeProfilePath), "GeneratedNormalStrengthForReview", runtimeProfilePath);
            ValidateSourceToken(File.ReadAllText(runtimeProfilePath), "UsesGeometryFallbackForReview", runtimeProfilePath);
            ValidateSourceToken(File.ReadAllText(runtimeMarkerPath), "IsReadyForReview", runtimeMarkerPath);
            foreach (var token in new[]
            {
                "CaptureHd2dAutonomousP1Item43RoofWallSurfaceDetailingBatch",
                "roof_wall_surface_detailing",
                "roof_wall_surface_detailing_review.md",
                "AddHd2dAutonomousP1BuildingSurfaceRoofTileRows",
                "Hd2dAutonomousP1BuildingSurfaceDetailRoofHeightId"
            })
            {
                ValidateSourceToken(File.ReadAllText(editorSourcePath), token, editorSourcePath);
            }

            ValidateSourceToken(File.ReadAllText(modularSourcePath), "AddHd2dAutonomousP1BuildingSurfaceDetailing", modularSourcePath);
        }

        private static FastVsHd2dBuildingSurfaceDetailProfile EnsureHd2dAutonomousP1BuildingSurfaceDetailProfileAsset()
        {
            EnsureFolder("Assets/Settings");
            var roofNormal = EnsureHd2dAutonomousP1BuildingSurfaceDetailNormalTexture(Hd2dAutonomousP1BuildingSurfaceDetailRoofNormalId, true);
            var wallNormal = EnsureHd2dAutonomousP1BuildingSurfaceDetailNormalTexture(Hd2dAutonomousP1BuildingSurfaceDetailWallNormalId, false);
            var roofHeight = EnsureHd2dAutonomousP1BuildingSurfaceDetailHeightTexture(Hd2dAutonomousP1BuildingSurfaceDetailRoofHeightId, true);
            var wallHeight = EnsureHd2dAutonomousP1BuildingSurfaceDetailHeightTexture(Hd2dAutonomousP1BuildingSurfaceDetailWallHeightId, false);
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dBuildingSurfaceDetailProfile>(Hd2dAutonomousP1BuildingSurfaceDetailProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dBuildingSurfaceDetailProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP1BuildingSurfaceDetailProfilePath);
            }

            profile.ConfigureForReview(
                0.28f,
                0.024f,
                0.42f,
                0.075f,
                0.36f,
                0.35f,
                0.48f,
                0.34f,
                new Vector2(5.5f, 3.2f),
                new Vector2(4.0f, 2.8f),
                roofNormal != null && wallNormal != null && roofHeight != null && wallHeight != null,
                false,
                false,
                true,
                true,
                GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId),
                GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailRoofNormalId),
                GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailRoofHeightId),
                GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId),
                GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailWallNormalId),
                GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailWallHeightId),
                "Procedural CC0-safe review baseline; replace with approved CC0 roof/wall sources. SurfaceRampLit has no final normal/parallax slot in this branch, so generated normal/height textures are staged as data while geometry rows and edge accents carry review-visible detail.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Dictionary<string, Material> EnsureHd2dAutonomousP1BuildingSurfaceDetailMaterials(FastVsHd2dBuildingSurfaceDetailProfile profile)
        {
            return new Dictionary<string, Material>(StringComparer.Ordinal)
            {
                [Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId] = EnsureHd2dAutonomousP1BuildingSurfaceDetailMaterial(
                    Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId,
                    Hd2dAutonomousP1BuildingSurfaceDetailRoofNormalId,
                    new Color32(76, 42, 38, 255),
                    new Color32(154, 83, 61, 255),
                    new Color32(38, 31, 34, 255),
                    profile.RoofTextureScaleForReview,
                    profile.RoofMicroShadowStrengthForReview),
                [Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId] = EnsureHd2dAutonomousP1BuildingSurfaceDetailMaterial(
                    Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId,
                    Hd2dAutonomousP1BuildingSurfaceDetailWallNormalId,
                    new Color32(72, 47, 32, 255),
                    new Color32(126, 84, 52, 255),
                    new Color32(34, 27, 24, 255),
                    profile.WallTextureScaleForReview,
                    profile.WallBandingStrengthForReview),
                [Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId] = EnsureHd2dAutonomousP1BuildingSurfaceDetailMaterial(
                    Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId,
                    Hd2dAutonomousP1BuildingSurfaceDetailWallNormalId,
                    new Color32(46, 39, 34, 255),
                    new Color32(86, 71, 56, 255),
                    new Color32(21, 19, 18, 255),
                    new Vector2(3.0f, 2.0f),
                    0.52f)
            };
        }

        private static Material EnsureHd2dAutonomousP1BuildingSurfaceDetailMaterial(
            string materialId,
            string normalTextureId,
            Color32 a,
            Color32 b,
            Color32 c,
            Vector2 textureScale,
            float shadowTextureStrength)
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            var material = FlatMaterial(materialId, Color.white, false, FastVsHd2dMaterialRole.SurfaceLit);
            AssignMaterialTexture(
                material,
                EnsureGeneratedRepeatTexture(materialId, 96, 96, (x, y) => SampleHd2dAutonomousP1BuildingSurfaceDetailPixel(materialId, x, y, a, b, c)),
                textureScale);
            ApplySurfaceRampProfile(material, materialId);
            material.enableInstancing = true;
            if (material.HasProperty("_ShadowTextureStrength"))
            {
                material.SetFloat("_ShadowTextureStrength", Mathf.Clamp01(shadowTextureStrength));
            }

            if (material.HasProperty("_SpecularHighlights"))
            {
                material.SetFloat("_SpecularHighlights", materialId.Contains("roof", StringComparison.Ordinal) ? 0.16f : 0.04f);
            }

            if (material.HasProperty("_Smoothness"))
            {
                material.SetFloat("_Smoothness", materialId.Contains("roof", StringComparison.Ordinal) ? 0.18f : 0.12f);
            }

            var normal = AssetDatabase.LoadAssetAtPath<Texture2D>(GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(normalTextureId));
            if (normal != null)
            {
                if (material.HasProperty("_BumpMap"))
                {
                    material.SetTexture("_BumpMap", normal);
                }

                if (material.HasProperty("_NormalMap"))
                {
                    material.SetTexture("_NormalMap", normal);
                }
            }

            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Color SampleHd2dAutonomousP1BuildingSurfaceDetailPixel(string materialId, int x, int y, Color32 a, Color32 b, Color32 c)
        {
            if (materialId.Contains("roof", StringComparison.Ordinal))
            {
                var row = y % 12;
                var stagger = ((y / 12) % 2) * 8;
                var verticalSeam = (x + stagger) % 16 == 0;
                if (row == 0 || row == 1 || verticalSeam)
                {
                    return c;
                }

                return (x * 5 + y * 3) % 19 < 4 ? b : a;
            }

            var beam = x % 18 == 0 || y % 14 == 0;
            var cavity = (x + y * 2) % 23 < 3;
            if (beam)
            {
                return c;
            }

            return cavity ? Color32.Lerp(a, c, 0.34f) : ((x * 7 + y * 11) % 31 < 5 ? b : a);
        }

        private static Texture2D EnsureHd2dAutonomousP1BuildingSurfaceDetailNormalTexture(string textureId, bool roof)
        {
            return EnsureGeneratedRepeatTexture(
                textureId,
                64,
                64,
                (x, y) =>
                {
                    var row = roof ? y % 10 : y % 12;
                    var column = roof ? (x + ((y / 10) % 2) * 5) % 14 : x % 16;
                    var normalX = 0.5f + ((column <= 1 ? -0.13f : column >= 12 ? 0.13f : 0f) * (roof ? 1f : 0.62f));
                    var normalY = 0.5f + (row <= 1 ? -0.18f : (row >= (roof ? 8 : 10) ? 0.12f : 0f));
                    var normalZ = roof ? 0.94f : 0.96f;
                    return new Color(normalX, normalY, normalZ, 1f);
                });
        }

        private static Texture2D EnsureHd2dAutonomousP1BuildingSurfaceDetailHeightTexture(string textureId, bool roof)
        {
            return EnsureGeneratedRepeatTexture(
                textureId,
                64,
                64,
                (x, y) =>
                {
                    if (roof)
                    {
                        var row = y % 10;
                        var staggeredX = (x + ((y / 10) % 2) * 5) % 14;
                        var seam = row <= 1 || staggeredX <= 1;
                        var lip = row >= 8;
                        var value = seam ? 0.20f : (lip ? 0.36f : 0.70f);
                        return new Color(value, value, value, 1f);
                    }

                    var beam = x % 16 <= 1 || y % 12 <= 1;
                    var cavity = (x + y * 2) % 23 < 3;
                    var height = beam ? 0.76f : (cavity ? 0.24f : 0.46f);
                    return new Color(height, height, height, 1f);
                });
        }

        private static void AddHd2dAutonomousP1BuildingSurfaceRoofTileRows(
            Transform root,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            FastVsHd2dBuildingSurfaceDetailProfile profile,
            IReadOnlyDictionary<string, Material> materials,
            float width,
            float depth,
            float topY,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            var rowCount = Mathf.Clamp(Mathf.RoundToInt(depth / Mathf.Max(0.1f, profile.RoofTileRowSpacingForReview)) + 6, 6, 14);
            var rowDepth = Mathf.Max(0.026f, profile.RoofTileRowThicknessForReview * 1.4f);
            var material = materials[Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId];
            var flat = recipe.RoofStyleForReview.Contains("flat", StringComparison.Ordinal);
            var shed = recipe.RoofStyleForReview.Contains("shed", StringComparison.Ordinal);
            var pitch = recipe.RoofStyleForReview.Contains("steep", StringComparison.Ordinal) ? 16f : 10f;
            for (var row = 0; row < rowCount; row++)
            {
                var t = rowCount <= 1 ? 0.5f : row / (rowCount - 1f);
                var z = Mathf.Lerp(-depth * 0.5f - 0.18f, depth * 0.5f + 0.18f, t);
                if (flat || shed)
                {
                    var detail = AddHd2dAutonomousP1ModularBuildingModule(
                        root,
                        "roof_tile_row",
                        new Vector3(0f, topY + 0.13f + row * 0.002f, z),
                        new Vector3(width + 0.46f, profile.RoofTileRowThicknessForReview, rowDepth),
                        shed ? Quaternion.Euler(0f, 0f, -pitch) : Quaternion.identity,
                        material,
                        renderers,
                        ref moduleCount);
                    ConfigureHd2dAutonomousP1BuildingSurfaceDetailMarker(detail, profile, recipe, "roof_tile_row", Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId);
                    continue;
                }

                var left = AddHd2dAutonomousP1ModularBuildingModule(
                    root,
                    "roof_tile_row",
                    new Vector3(-width * 0.25f, topY + 0.22f + row * 0.002f, z),
                    new Vector3(width * 0.58f + 0.34f, profile.RoofTileRowThicknessForReview, rowDepth),
                    Quaternion.Euler(0f, 0f, pitch),
                    material,
                    renderers,
                    ref moduleCount);
                ConfigureHd2dAutonomousP1BuildingSurfaceDetailMarker(left, profile, recipe, "roof_tile_row", Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId);

                var right = AddHd2dAutonomousP1ModularBuildingModule(
                    root,
                    "roof_tile_row",
                    new Vector3(width * 0.25f, topY + 0.22f + row * 0.002f, z),
                    new Vector3(width * 0.58f + 0.34f, profile.RoofTileRowThicknessForReview, rowDepth),
                    Quaternion.Euler(0f, 0f, -pitch),
                    material,
                    renderers,
                    ref moduleCount);
                ConfigureHd2dAutonomousP1BuildingSurfaceDetailMarker(right, profile, recipe, "roof_tile_row", Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId);
            }
        }

        private static void AddHd2dAutonomousP1BuildingSurfaceWallFrames(
            Transform root,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            FastVsHd2dBuildingSurfaceDetailProfile profile,
            IReadOnlyDictionary<string, Material> materials,
            float width,
            float depth,
            float storyHeight,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            var timber = materials[Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId];
            var cavity = materials[Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId];
            var frontZ = -depth * 0.5f - 0.205f;
            var totalHeight = recipe.FloorsForReview * storyHeight;
            var columns = Mathf.Max(3, recipe.FootprintForReview.x + 1);
            for (var column = 0; column < columns; column++)
            {
                var x = Mathf.Lerp(-width * 0.5f, width * 0.5f, columns <= 1 ? 0.5f : column / (columns - 1f));
                var detail = AddHd2dAutonomousP1ModularBuildingModule(
                    root,
                    "timber_frame_vertical",
                    new Vector3(x, totalHeight * 0.5f, frontZ),
                    new Vector3(profile.TimberBeamThicknessForReview, totalHeight + 0.18f, 0.055f),
                    Quaternion.identity,
                    timber,
                    renderers,
                    ref moduleCount);
                ConfigureHd2dAutonomousP1BuildingSurfaceDetailMarker(detail, profile, recipe, "timber_frame", Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId);
            }

            for (var floor = 0; floor <= recipe.FloorsForReview; floor++)
            {
                var y = floor * storyHeight + 0.08f;
                var band = AddHd2dAutonomousP1ModularBuildingModule(
                    root,
                    "timber_frame_horizontal",
                    new Vector3(0f, y, frontZ - 0.012f),
                    new Vector3(width + 0.26f, profile.TimberBeamThicknessForReview * 0.72f, 0.060f),
                    Quaternion.identity,
                    timber,
                    renderers,
                    ref moduleCount);
                ConfigureHd2dAutonomousP1BuildingSurfaceDetailMarker(band, profile, recipe, "timber_frame", Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId);
            }

            var underRoof = AddHd2dAutonomousP1ModularBuildingModule(
                root,
                "wall_cavity_under_eave",
                new Vector3(0f, totalHeight + 0.04f, frontZ - 0.030f),
                new Vector3(width + 0.34f, 0.052f, 0.070f),
                Quaternion.identity,
                cavity,
                renderers,
                ref moduleCount);
            ConfigureHd2dAutonomousP1BuildingSurfaceDetailMarker(underRoof, profile, recipe, "cavity_edge", Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId);

            for (var side = -1; side <= 1; side += 2)
            {
                var edge = AddHd2dAutonomousP1ModularBuildingModule(
                    root,
                    "wall_cavity_side_edge",
                    new Vector3(side * (width * 0.5f + 0.055f), totalHeight * 0.5f, frontZ - 0.025f),
                    new Vector3(0.055f, totalHeight + 0.10f, 0.070f),
                    Quaternion.identity,
                    cavity,
                    renderers,
                    ref moduleCount);
                ConfigureHd2dAutonomousP1BuildingSurfaceDetailMarker(edge, profile, recipe, "cavity_edge", Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId);
            }
        }

        private static void ConfigureHd2dAutonomousP1BuildingSurfaceDetailMarker(
            GameObject detailObject,
            FastVsHd2dBuildingSurfaceDetailProfile profile,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            string detailKind,
            string materialId)
        {
            if (detailObject == null)
            {
                return;
            }

            var marker = detailObject.GetComponent<FastVsHd2dBuildingSurfaceDetailMarker>();
            if (marker == null)
            {
                marker = detailObject.AddComponent<FastVsHd2dBuildingSurfaceDetailMarker>();
            }

            marker.ConfigureForReview(profile, recipe.BuildingIdForReview, detailKind, materialId, true, profile.GeneratedNormalMapsReadyForReview);
            EditorUtility.SetDirty(marker);
        }

        private static void CaptureHd2dAutonomousP1BuildingSurfaceDetailReviewShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            GameObject streetRoot,
            bool detailVisible,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.AriaStreet);
            controller.ForcePlayerCurrentLocalForReview(anchorLocalPosition + new Vector3(0f, 0.02f, -1.2f));
            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            SetHd2dAutonomousP1BuildingSurfaceDetailRenderersVisible(streetRoot, detailVisible);
            Physics.SyncTransforms();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 160f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            WarmUpCameraRender(camera);
            SetHd2dAutonomousP1BuildingSurfaceDetailRenderersVisible(streetRoot, detailVisible);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {FormatVector3ForReport(anchorLocalPosition)} | {FormatVector3ForReport(cameraOffset)} | {fieldOfView:0.#} |");
        }

        private static void SetHd2dAutonomousP1BuildingSurfaceDetailRenderersVisible(GameObject root, bool visible)
        {
            if (root == null)
            {
                return;
            }

            var markers = root.GetComponentsInChildren<FastVsHd2dBuildingSurfaceDetailMarker>(true);
            for (var index = 0; index < markers.Length; index++)
            {
                var markerRenderers = markers[index].GetComponentsInChildren<Renderer>(true);
                for (var rendererIndex = 0; rendererIndex < markerRenderers.Length; rendererIndex++)
                {
                    markerRenderers[rendererIndex].enabled = visible;
                    EditorUtility.SetDirty(markerRenderers[rendererIndex]);
                }
            }
        }

        private static void WriteHd2dAutonomousP1BuildingSurfaceDetailReviewReport(string outputDirectory, IReadOnlyList<string> screenshotFiles, IReadOnlyList<string> shotRows)
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dBuildingSurfaceDetailProfile>(Hd2dAutonomousP1BuildingSurfaceDetailProfilePath);
            var streetRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1ModularBuildingKitStreetRootName);
            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dBuildingSurfaceDetailMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(marker => marker != null && streetRoot != null && marker.transform.IsChildOf(streetRoot.transform))
                .ToArray();
            var roofRows = markers.Count(marker => string.Equals(marker.DetailKindForReview, "roof_tile_row", StringComparison.Ordinal));
            var timberFrames = markers.Count(marker => string.Equals(marker.DetailKindForReview, "timber_frame", StringComparison.Ordinal));
            var cavityEdges = markers.Count(marker => string.Equals(marker.DetailKindForReview, "cavity_edge", StringComparison.Ordinal));

            var lines = new List<string>
            {
                "# P1-43 Roof/Wall Surface Detailing Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative surface-detail baseline layered on the P1-42 modular building variants. This adds generated CC0-safe roof/wall detail textures, prepared normal-map assets, raised roof-tile row strips, timber-frame bands, and dark cavity/eave accents.",
                "- Recommendation: keep the data/profile/material path and geometric detail markers. Tom should replace the generated textures/normals with approved CC0 roof/wall sources and tune row spacing, beam thickness, normal strength, and facade taste before art sign-off.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP1BuildingSurfaceDetailProfilePath}` |",
                $"| Roof row spacing / thickness | {profile?.RoofTileRowSpacingForReview ?? 0f:0.###} / {profile?.RoofTileRowThicknessForReview ?? 0f:0.###} |",
                $"| Roof shadow / wall band / normal strength | {profile?.RoofMicroShadowStrengthForReview ?? 0f:0.###} / {profile?.WallBandingStrengthForReview ?? 0f:0.###} / {profile?.GeneratedNormalStrengthForReview ?? 0f:0.###} |",
                $"| Roof texture scale | {FormatHd2dAutonomousP1BuildingSurfaceDetailVector2(profile != null ? profile.RoofTextureScaleForReview : Vector2.zero)} |",
                $"| Wall texture scale | {FormatHd2dAutonomousP1BuildingSurfaceDetailVector2(profile != null ? profile.WallTextureScaleForReview : Vector2.zero)} |",
                $"| Generated normal maps ready | {FormatBool(profile != null && profile.GeneratedNormalMapsReadyForReview)} |",
                $"| Conservative review mode | {FormatBool(profile != null && profile.ConservativeReviewModeForReview)} |",
                $"| Requires TOM art approval | {FormatBool(profile != null && profile.RequiresTomArtApprovalForReview)} |",
                $"| Shader normal/parallax slots | {FormatBool(profile != null && profile.ShaderHasNormalMapSlotForReview)} / {FormatBool(profile != null && profile.ShaderHasParallaxSlotForReview)} |",
                $"| Geometry fallback used | {FormatBool(profile != null && profile.UsesGeometryFallbackForReview)} |",
                $"| Source texture note | {profile?.SourceTextureNoteForReview ?? "missing"} |",
                string.Empty,
                "| Detail kind | Count |",
                "|---|---:|",
                $"| roof_tile_row | {roofRows} |",
                $"| timber_frame | {timberFrames} |",
                $"| cavity_edge | {cavityEdges} |",
                string.Empty,
                "| Material | Texture |",
                "|---|---|",
                $"| `{Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId}` | `{GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailRoofEdgeMaterialId)}` |",
                $"| `{Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId}` | `{GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailTimberMaterialId)}` |",
                $"| `{Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId}` | `{GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailCavityMaterialId)}` |",
                $"| `{Hd2dAutonomousP1BuildingSurfaceDetailRoofNormalId}` | `{GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailRoofNormalId)}` |",
                $"| `{Hd2dAutonomousP1BuildingSurfaceDetailWallNormalId}` | `{GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailWallNormalId)}` |",
                $"| `{Hd2dAutonomousP1BuildingSurfaceDetailRoofHeightId}` | `{GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailRoofHeightId)}` |",
                $"| `{Hd2dAutonomousP1BuildingSurfaceDetailWallHeightId}` | `{GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(Hd2dAutonomousP1BuildingSurfaceDetailWallHeightId)}` |",
                string.Empty,
                "| Screenshot | Label | Anchor | Offset | FOV |",
                "|---|---|---|---|---:|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Baseline with P1-43 detail markers disabled |",
                $"| `{screenshotFiles[1]}` | Same facade with roof rows, timber bands, and cavity accents enabled |",
                $"| `{screenshotFiles[2]}` | Roof close-up for tile rows and directional micro-shadow readability |",
                $"| `{screenshotFiles[3]}` | Wall close-up for timber-frame/stucco/stone banding readability |",
                $"| `{screenshotFiles[4]}` | Wider check that repeated details do not make adjacent buildings identical |"
            });

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllLines(Path.Combine(outputDirectory, "roof_wall_surface_detailing_review.md"), lines, Encoding.UTF8);
        }

        private static void ValidateHd2dAutonomousP1BuildingSurfaceDetailReviewPairDiff(string outputDirectory, string firstFile, string secondFile, string label)
        {
            var firstPath = Path.Combine(outputDirectory, firstFile);
            var secondPath = Path.Combine(outputDirectory, secondFile);
            var firstBytes = File.ReadAllBytes(firstPath);
            var secondBytes = File.ReadAllBytes(secondPath);
            if (firstBytes.Length != secondBytes.Length)
            {
                return;
            }

            for (var i = 0; i < firstBytes.Length; i++)
            {
                if (firstBytes[i] != secondBytes[i])
                {
                    return;
                }
            }

            throw new InvalidOperationException($"Fast VS autonomous P1-43 roof/wall surface detail capture failed: {label} images are byte-identical.");
        }

        private static void ValidateHd2dAutonomousP1BuildingSurfaceDetailMaterial(string materialId)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(GetHd2dAutonomousP1BuildingSurfaceDetailMaterialPath(materialId));
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(materialId));
            if (material == null ||
                material.shader == null ||
                !string.Equals(material.shader.name, SurfaceRampLitShaderName, StringComparison.Ordinal) ||
                !material.enableInstancing ||
                texture == null ||
                texture.width != 96 ||
                texture.height != 96 ||
                texture.wrapMode != TextureWrapMode.Repeat)
            {
                throw new InvalidOperationException($"House slice validation failed: P1-43 material/texture setup is invalid for {materialId}.");
            }
        }

        private static string GetHd2dAutonomousP1BuildingSurfaceDetailMaterialPath(string materialId)
        {
            return MaterialDirectory + "/FastVS_House_" + materialId + ".mat";
        }

        private static string GetHd2dAutonomousP1BuildingSurfaceDetailTexturePath(string textureId)
        {
            return TextureDirectory + "/FastVS_House_" + textureId + ".asset";
        }

        private static string FormatHd2dAutonomousP1BuildingSurfaceDetailVector2(Vector2 value)
        {
            return $"{value.x:0.###},{value.y:0.###}";
        }
    }
}
