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
        private const string Hd2dAutonomousP2FoliageVarietyRootName = "Current_CentralPlaza_P2_FoliageVarietyAccents";
        private const string Hd2dAutonomousP2FoliageVarietyProfilePath = "Assets/Settings/FastVS_HD2D_P2_FoliageVarietyProfile.asset";
        private const string Hd2dAutonomousP2FoliageVarietyProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dFoliageVarietyProfile.cs";
        private const string Hd2dAutonomousP2FoliageVarietyMarkerRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dFoliageVarietyMarker.cs";
        private const string Hd2dAutonomousP2FoliageVarietyFallenLeafTextureId = "hd2d_p2_variety_fallen_leaf_card";
        private const string Hd2dAutonomousP2FoliageVarietyVineTextureId = "hd2d_p2_variety_vine_card";
        private const string Hd2dAutonomousP2FoliageVarietyMossTextureId = "hd2d_p2_variety_moss_seam_card";

        public static void CaptureHd2dAutonomousP2Item59FoliageVarietyBatch()
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
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2FoliageVarietyRootName);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || root == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-59 foliage variety capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2FoliageVariety();
            var profile = EnsureHd2dAutonomousP2FoliageVarietyProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("foliage_variety_accents");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_variety_off_baseline.png",
                "02_flowers_leaves_vines_moss_on.png",
                "03_vines_moss_seam_close.png",
                "04_accent_types_topdown.png",
                "05_evening_color_pop_check.png"
            };
            var shotRows = new List<string>();
            var overviewFocus = CentralPlazaVsCenter + new Vector3(-1.72f, 0.22f, -1.38f);
            var overviewPlayer = overviewFocus + new Vector3(-0.72f, -0.18f, -0.76f);
            var seamFocus = CentralPlazaVsCenter + new Vector3(-2.86f, 0.66f, -1.02f);
            var seamPlayer = seamFocus + new Vector3(-0.52f, -0.36f, -0.58f);
            var topDownFocus = CentralPlazaVsCenter + new Vector3(-1.48f, 0.08f, -1.18f);
            var topDownPlayer = topDownFocus + new Vector3(-0.24f, -0.10f, -0.36f);

            try
            {
                guide.SetMovementFrozen(true);
                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();

                root.SetActive(false);
                CaptureHd2dAutonomousP2FoliageVarietyShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    overviewPlayer,
                    overviewFocus,
                    new Vector3(0.86f, 1.48f, -2.92f),
                    new Vector3(0.00f, 0.12f, 0.12f),
                    root,
                    false,
                    outputDirectory,
                    screenshotFiles[0],
                    "accent root disabled baseline",
                    shotRows);

                root.SetActive(true);
                CaptureHd2dAutonomousP2FoliageVarietyShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    overviewPlayer,
                    overviewFocus,
                    new Vector3(0.86f, 1.48f, -2.92f),
                    new Vector3(0.00f, 0.12f, 0.12f),
                    root,
                    true,
                    outputDirectory,
                    screenshotFiles[1],
                    "flower, fallen leaf, vine, and moss accents enabled",
                    shotRows);

                CaptureHd2dAutonomousP2FoliageVarietyShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    seamPlayer,
                    seamFocus,
                    new Vector3(0.48f, 1.00f, -2.05f),
                    new Vector3(0.00f, 0.08f, 0.10f),
                    root,
                    true,
                    outputDirectory,
                    screenshotFiles[2],
                    "close seam softened by vine and moss cards",
                    shotRows);

                CaptureHd2dAutonomousP2FoliageVarietyShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    topDownPlayer,
                    topDownFocus,
                    new Vector3(0.12f, 5.40f, -0.58f),
                    new Vector3(0.00f, 0.00f, 0.04f),
                    root,
                    true,
                    outputDirectory,
                    screenshotFiles[3],
                    "top-down accent-type placement diagnostic",
                    shotRows);

                sunDriver.ApplyPreset(SunPreset.Evening, true);
                realtimeRig.ApplyNowForReview();
                CaptureHd2dAutonomousP2FoliageVarietyShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    overviewPlayer,
                    overviewFocus,
                    new Vector3(0.86f, 1.48f, -2.92f),
                    new Vector3(0.00f, 0.12f, 0.12f),
                    root,
                    true,
                    outputDirectory,
                    screenshotFiles[4],
                    "evening warm/cool accent color-pop check",
                    shotRows);
            }
            finally
            {
                root.SetActive(true);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                AssetDatabase.SaveAssets();
            }

            var overviewDiff = MeasureHd2dAutonomousP2FoliageVarietyDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var eveningDiff = MeasureHd2dAutonomousP2FoliageVarietyDiff(outputDirectory, screenshotFiles[1], screenshotFiles[4]);
            WriteHd2dAutonomousP2FoliageVarietyReviewReport(outputDirectory, screenshotFiles, shotRows, profile, overviewDiff, eveningDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-59 foliage variety review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2FoliageVarietyPass(Transform centralPlazaRoot)
        {
            if (centralPlazaRoot == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP2FoliageVarietyProfile();
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2FoliageVarietyRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2FoliageVarietyRootName);
            root.transform.SetParent(centralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            var flowerLeafMaterial = EnsureHd2dAutonomousP2FoliageVarietyCardMaterial(
                "hd2d_p2_variety_flower_leaf",
                EnsureFoliageCardTexture(FoliageBushCardTexturePath),
                profile.VineTintForReview,
                profile.AccentWindStrengthForReview,
                1f);
            var flowerWarmMaterial = EnsureHd2dAutonomousP2FoliageVarietyCardMaterial(
                "hd2d_p2_variety_flower_warm",
                EnsureFoliageCardTexture(FoliageFlowerCardTexturePath),
                profile.FlowerWarmTintForReview,
                profile.AccentWindStrengthForReview,
                1f);
            var flowerCoolMaterial = EnsureHd2dAutonomousP2FoliageVarietyCardMaterial(
                "hd2d_p2_variety_flower_cool",
                EnsureFoliageCardTexture(FoliageFlowerCardTexturePath),
                profile.FlowerCoolTintForReview,
                profile.AccentWindStrengthForReview,
                1f);
            var fallenLeafMaterial = EnsureHd2dAutonomousP2FoliageVarietyCardMaterial(
                "hd2d_p2_variety_fallen_leaf",
                EnsureHd2dAutonomousP2FoliageVarietyFallenLeafTexture(),
                profile.FallenLeafTintForReview,
                profile.AccentWindStrengthForReview * 0.15f,
                1f);
            var vineMaterial = EnsureHd2dAutonomousP2FoliageVarietyCardMaterial(
                "hd2d_p2_variety_vine",
                EnsureHd2dAutonomousP2FoliageVarietyVineTexture(),
                profile.VineTintForReview,
                profile.AccentWindStrengthForReview,
                1f);
            var mossMaterial = EnsureHd2dAutonomousP2FoliageVarietyCardMaterial(
                "hd2d_p2_variety_moss_seam",
                EnsureHd2dAutonomousP2FoliageVarietyMossTexture(),
                profile.MossTintForReview,
                profile.AccentWindStrengthForReview * 0.20f,
                1f);

            CreateHd2dAutonomousP2FoliageVarietyFlowers(root.transform, profile, flowerLeafMaterial, flowerWarmMaterial, flowerCoolMaterial);
            CreateHd2dAutonomousP2FoliageVarietyFallenLeaves(root.transform, profile, fallenLeafMaterial);
            CreateHd2dAutonomousP2FoliageVarietyVines(root.transform, profile, vineMaterial);
            CreateHd2dAutonomousP2FoliageVarietyMoss(root.transform, profile, mossMaterial);
            CreateHd2dAutonomousP2FoliageVarietyVisibleReviewFallback(root.transform, profile);

            EditorUtility.SetDirty(root);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2FoliageVariety()
        {
            var profile = EnsureHd2dAutonomousP2FoliageVarietyProfile();
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2FoliageVarietyRootName);
            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dFoliageVarietyMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            CountHd2dAutonomousP2FoliageVarietyMarkers(markers, out var flower, out var fallenLeaf, out var vine, out var moss, out var seam);
            if (profile == null ||
                root == null ||
                profile.FinalFoliageVarietyApprovedForReview ||
                !profile.NeedsTomApprovalForReview ||
                flower < profile.FlowerPatchCountForReview * 3 ||
                fallenLeaf < profile.FallenLeafCountForReview ||
                vine < profile.VineStripCountForReview ||
                moss < profile.MossSeamStripCountForReview ||
                seam < 4)
            {
                throw new InvalidOperationException("House slice validation failed: P2-59 needs conservative flower, fallen-leaf, vine, and moss accent markers, with final approval left to Tom.");
            }

            if (ColorDistance(profile.FlowerWarmTintForReview, profile.FlowerCoolTintForReview) < 0.35f ||
                ColorDistance(profile.VineTintForReview, profile.MossTintForReview) < 0.08f ||
                profile.SeamSofteningStrengthForReview < 0.35f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-59 accent colors and seam-softening strength must be distinct enough for review.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2FoliageVarietyProfileRuntimePath), "finalFoliageVarietyApproved", Hd2dAutonomousP2FoliageVarietyProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2FoliageVarietyMarkerRuntimePath), "FastVsHd2dFoliageVarietyAccentType", Hd2dAutonomousP2FoliageVarietyMarkerRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP0VegetationWindSharedHlslPath), "FastVsApplySharedVegetationTint", Hd2dAutonomousP0VegetationWindSharedHlslPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2FoliageVarietyPass", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2FoliageVariety", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dFoliageVarietyProfile EnsureHd2dAutonomousP2FoliageVarietyProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dFoliageVarietyProfile>(Hd2dAutonomousP2FoliageVarietyProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dFoliageVarietyProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2FoliageVarietyProfilePath);
            }

            profile.ConfigureForReview(
                5,
                14,
                5,
                6,
                new Color(1.00f, 0.42f, 0.32f, 1f),
                new Color(0.42f, 0.58f, 1.00f, 1f),
                new Color(0.92f, 0.54f, 0.20f, 1f),
                new Color(0.32f, 0.62f, 0.30f, 1f),
                new Color(0.24f, 0.44f, 0.22f, 1f),
                0.045f,
                0.62f,
                true,
                false,
                "Keep P2-59 as conservative accent variety data prep. Recommendation: Tom should tune flower color count, fallen-leaf density, vine placement, moss seam strength, and per-biome usage after ground/lighting sign-off.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static void CreateHd2dAutonomousP2FoliageVarietyFlowers(
            Transform root,
            FastVsHd2dFoliageVarietyProfile profile,
            Material leafMaterial,
            Material warmMaterial,
            Material coolMaterial)
        {
            var centers = new[]
            {
                CentralPlazaVsCenter + new Vector3(-2.56f, 0.065f, -1.84f),
                CentralPlazaVsCenter + new Vector3(-1.78f, 0.065f, -1.36f),
                CentralPlazaVsCenter + new Vector3(-0.98f, 0.065f, -1.68f),
                CentralPlazaVsCenter + new Vector3(-2.22f, 0.065f, -0.72f),
                CentralPlazaVsCenter + new Vector3(-1.18f, 0.065f, -0.76f)
            };

            for (var i = 0; i < Mathf.Min(profile.FlowerPatchCountForReview, centers.Length); i++)
            {
                var center = centers[i];
                CreateHd2dAutonomousP2MarkedCardCluster(
                    root,
                    $"P2_59_FlowerPatch_{i:00}_Leaf",
                    center,
                    new Vector2(0.32f, 0.34f),
                    leafMaterial,
                    i * 19 + 3,
                    FastVsHd2dFoliageVarietyAccentType.Flower,
                    "ground_flower_leaf_cluster",
                    false,
                    2,
                    true);
                CreateHd2dAutonomousP2MarkedCardCluster(
                    root,
                    $"P2_59_FlowerPatch_{i:00}_WarmBloom",
                    center + new Vector3(-0.08f, 0.01f, 0.03f),
                    new Vector2(0.18f, 0.24f),
                    warmMaterial,
                    i * 23 + 7,
                    FastVsHd2dFoliageVarietyAccentType.Flower,
                    "ground_flower_warm_bloom",
                    false,
                    2,
                    true);
                CreateHd2dAutonomousP2MarkedCardCluster(
                    root,
                    $"P2_59_FlowerPatch_{i:00}_CoolBloom",
                    center + new Vector3(0.11f, 0.01f, -0.03f),
                    new Vector2(0.18f, 0.24f),
                    coolMaterial,
                    i * 29 + 11,
                    FastVsHd2dFoliageVarietyAccentType.Flower,
                    "ground_flower_cool_bloom",
                    false,
                    2,
                    true);
            }
        }

        private static void CreateHd2dAutonomousP2FoliageVarietyFallenLeaves(
            Transform root,
            FastVsHd2dFoliageVarietyProfile profile,
            Material material)
        {
            for (var i = 0; i < profile.FallenLeafCountForReview; i++)
            {
                var t = i / Mathf.Max(1f, profile.FallenLeafCountForReview - 1f);
                var x = Mathf.Lerp(-2.72f, -0.58f, t) + (PositiveModulo(i * 17, 7) - 3) * 0.045f;
                var z = -1.98f + Mathf.Sin(i * 1.73f) * 0.62f;
                var yaw = 18f + i * 31f;
                var leaf = CreateQuad(
                    $"P2_59_FallenLeaf_{i:00}",
                    root,
                    CentralPlazaVsCenter + new Vector3(x, 0.082f + i * 0.0008f, z),
                    new Vector3(0.22f + PositiveModulo(i, 3) * 0.025f, 0.105f, 1f),
                    material);
                leaf.transform.localRotation = Quaternion.Euler(90f, yaw, 0f);
                AddHd2dAutonomousP2FoliageVarietyMarker(leaf, FastVsHd2dFoliageVarietyAccentType.FallenLeaf, "settled_fallen_leaf_ground_accent", false);
            }
        }

        private static void CreateHd2dAutonomousP2FoliageVarietyVines(
            Transform root,
            FastVsHd2dFoliageVarietyProfile profile,
            Material material)
        {
            var bases = new[]
            {
                CentralPlazaVsCenter + new Vector3(-3.08f, 0.62f, -1.24f),
                CentralPlazaVsCenter + new Vector3(-2.92f, 0.82f, -1.06f),
                CentralPlazaVsCenter + new Vector3(-2.72f, 0.70f, -0.86f),
                CentralPlazaVsCenter + new Vector3(-2.44f, 0.74f, -0.66f),
                CentralPlazaVsCenter + new Vector3(-2.24f, 0.58f, -0.50f)
            };

            for (var i = 0; i < Mathf.Min(profile.VineStripCountForReview, bases.Length); i++)
            {
                var vine = CreateQuad(
                    $"P2_59_VineStrip_{i:00}",
                    root,
                    bases[i],
                    new Vector3(0.24f + PositiveModulo(i, 2) * 0.05f, 0.82f + PositiveModulo(i, 3) * 0.10f, 1f),
                    material);
                vine.transform.localRotation = Quaternion.Euler(0f, 8f + i * 4f, i % 2 == 0 ? -5f : 6f);
                AddHd2dAutonomousP2FoliageVarietyMarker(vine, FastVsHd2dFoliageVarietyAccentType.Vine, "vertical_wall_vine_seam_softener", true);
            }
        }

        private static void CreateHd2dAutonomousP2FoliageVarietyMoss(
            Transform root,
            FastVsHd2dFoliageVarietyProfile profile,
            Material material)
        {
            var centers = new[]
            {
                new Vector4(-3.04f, -1.70f, 0.72f, -12f),
                new Vector4(-2.42f, -1.36f, 0.64f, 8f),
                new Vector4(-1.84f, -0.88f, 0.58f, -18f),
                new Vector4(-1.08f, -0.74f, 0.50f, 16f),
                new Vector4(-2.70f, -0.28f, 0.54f, 4f),
                new Vector4(-1.50f, -0.22f, 0.48f, -10f)
            };

            for (var i = 0; i < Mathf.Min(profile.MossSeamStripCountForReview, centers.Length); i++)
            {
                var center = centers[i];
                var moss = CreateQuad(
                    $"P2_59_MossSeamStrip_{i:00}",
                    root,
                    CentralPlazaVsCenter + new Vector3(center.x, 0.076f + i * 0.0006f, center.y),
                    new Vector3(center.z, 0.145f, 1f),
                    material);
                moss.transform.localRotation = Quaternion.Euler(90f, center.w, 0f);
                AddHd2dAutonomousP2FoliageVarietyMarker(moss, FastVsHd2dFoliageVarietyAccentType.Moss, "ground_stone_moss_seam_softener", true);
            }
        }

        private static void CreateHd2dAutonomousP2FoliageVarietyVisibleReviewFallback(
            Transform root,
            FastVsHd2dFoliageVarietyProfile profile)
        {
            if (root == null || profile == null)
            {
                return;
            }

            var flowerWarmMaterial = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial(
                "hd2d_p2_variety_visible_flower_warm_review",
                BoostHd2dAutonomousP2FoliageVarietyReviewColor(profile.FlowerWarmTintForReview, 0.12f, 1.18f));
            var flowerCoolMaterial = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial(
                "hd2d_p2_variety_visible_flower_cool_review",
                BoostHd2dAutonomousP2FoliageVarietyReviewColor(profile.FlowerCoolTintForReview, 0.10f, 1.12f));
            var fallenLeafMaterial = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial(
                "hd2d_p2_variety_visible_fallen_leaf_review",
                BoostHd2dAutonomousP2FoliageVarietyReviewColor(profile.FallenLeafTintForReview, 0.10f, 1.16f));
            var vineMaterial = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial(
                "hd2d_p2_variety_visible_vine_review",
                BoostHd2dAutonomousP2FoliageVarietyReviewColor(profile.VineTintForReview, 0.08f, 1.10f));
            var mossMaterial = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial(
                "hd2d_p2_variety_visible_moss_review",
                BoostHd2dAutonomousP2FoliageVarietyReviewColor(profile.MossTintForReview, 0.07f, 1.10f));

            var flowerCenters = new[]
            {
                CentralPlazaVsCenter + new Vector3(-2.36f, 0.36f, -1.82f),
                CentralPlazaVsCenter + new Vector3(-1.92f, 0.38f, -1.34f),
                CentralPlazaVsCenter + new Vector3(-1.36f, 0.36f, -1.70f),
                CentralPlazaVsCenter + new Vector3(-2.18f, 0.37f, -0.96f),
                CentralPlazaVsCenter + new Vector3(-1.14f, 0.36f, -0.92f)
            };

            for (var i = 0; i < Mathf.Min(profile.FlowerPatchCountForReview, flowerCenters.Length); i++)
            {
                var center = flowerCenters[i];
                CreateHd2dAutonomousP2FoliageVarietyVisibleAccentSlab(
                    root,
                    $"P2_59_VisibleReviewFlowerWarm_{i:00}",
                    center + new Vector3(-0.055f, 0.015f, 0.025f),
                    new Vector3(0.13f, 0.07f, 0.13f),
                    Quaternion.Euler(0f, 12f + i * 17f, 0f),
                    flowerWarmMaterial,
                    FastVsHd2dFoliageVarietyAccentType.Flower,
                    "visible_review_ground_flower_warm_pop",
                    false);
                CreateHd2dAutonomousP2FoliageVarietyVisibleAccentSlab(
                    root,
                    $"P2_59_VisibleReviewFlowerCool_{i:00}",
                    center + new Vector3(0.070f, 0.020f, -0.030f),
                    new Vector3(0.11f, 0.065f, 0.11f),
                    Quaternion.Euler(0f, -9f - i * 13f, 0f),
                    flowerCoolMaterial,
                    FastVsHd2dFoliageVarietyAccentType.Flower,
                    "visible_review_ground_flower_cool_pop",
                    false);
            }

            for (var i = 0; i < Mathf.Min(profile.FallenLeafCountForReview, 10); i++)
            {
                var t = i / 9f;
                var x = Mathf.Lerp(-2.58f, -0.84f, t) + (PositiveModulo(i * 11, 5) - 2) * 0.055f;
                var z = -1.86f + Mathf.Sin(i * 1.27f) * 0.54f;
                CreateHd2dAutonomousP2FoliageVarietyVisibleAccentSlab(
                    root,
                    $"P2_59_VisibleReviewFallenLeaf_{i:00}",
                    CentralPlazaVsCenter + new Vector3(x, 0.245f + i * 0.0025f, z),
                    new Vector3(0.38f + PositiveModulo(i, 3) * 0.035f, 0.045f, 0.145f),
                    Quaternion.Euler(0f, 19f + i * 31f, 0f),
                    fallenLeafMaterial,
                    FastVsHd2dFoliageVarietyAccentType.FallenLeaf,
                    "visible_review_settled_fallen_leaf_pop",
                    false);
            }

            var vineCenters = new[]
            {
                CentralPlazaVsCenter + new Vector3(-3.08f, 0.82f, -1.23f),
                CentralPlazaVsCenter + new Vector3(-2.88f, 0.92f, -1.05f),
                CentralPlazaVsCenter + new Vector3(-2.60f, 0.82f, -0.82f)
            };

            for (var i = 0; i < vineCenters.Length; i++)
            {
                CreateHd2dAutonomousP2FoliageVarietyVisibleAccentSlab(
                    root,
                    $"P2_59_VisibleReviewVineStrip_{i:00}",
                    vineCenters[i],
                    new Vector3(0.085f, 0.78f + i * 0.08f, 0.060f),
                    Quaternion.Euler(0f, 8f + i * 5f, i % 2 == 0 ? -4f : 5f),
                    vineMaterial,
                    FastVsHd2dFoliageVarietyAccentType.Vine,
                    "visible_review_vertical_wall_vine_seam",
                    true);
            }

            var mossCenters = new[]
            {
                new Vector4(-3.02f, -1.66f, 0.82f, -12f),
                new Vector4(-2.36f, -1.26f, 0.72f, 8f),
                new Vector4(-1.70f, -0.80f, 0.66f, -15f),
                new Vector4(-2.72f, -0.34f, 0.68f, 5f)
            };

            for (var i = 0; i < mossCenters.Length; i++)
            {
                var center = mossCenters[i];
                CreateHd2dAutonomousP2FoliageVarietyVisibleAccentSlab(
                    root,
                    $"P2_59_VisibleReviewMossSeam_{i:00}",
                    CentralPlazaVsCenter + new Vector3(center.x, 0.235f + i * 0.002f, center.y),
                    new Vector3(center.z, 0.050f, 0.165f),
                    Quaternion.Euler(0f, center.w, 0f),
                    mossMaterial,
                    FastVsHd2dFoliageVarietyAccentType.Moss,
                    "visible_review_moss_seam_softener",
                    true);
            }
        }

        private static GameObject CreateHd2dAutonomousP2FoliageVarietyVisibleAccentSlab(
            Transform root,
            string objectName,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation,
            Material material,
            FastVsHd2dFoliageVarietyAccentType type,
            string placementGroup,
            bool seamSoftener)
        {
            var slab = CreateLandmarkCube(
                objectName,
                root,
                localPosition,
                localScale,
                localRotation,
                material,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"current.central_plaza.p2_59.visible_review.{placementGroup}.{objectName}");
            AddHd2dAutonomousP2FoliageVarietyMarker(slab, type, placementGroup, seamSoftener);
            return slab;
        }

        private static void CreateHd2dAutonomousP2MarkedCardCluster(
            Transform root,
            string objectName,
            Vector3 baseCenter,
            Vector2 cardSize,
            Material cardMaterial,
            int variantSeed,
            FastVsHd2dFoliageVarietyAccentType type,
            string placementGroup,
            bool seamSoftener,
            int planeCount,
            bool tightMesh)
        {
            var planes = Mathf.Clamp(planeCount, 1, 3);
            var baseYaw = PositiveModulo(variantSeed * 37, 180);
            for (var i = 0; i < planes; i++)
            {
                var yaw = baseYaw + i * (180f / planes);
                var scaleJitter = 0.90f + PositiveModulo(variantSeed + i, 4) * 0.045f;
                var quad = CreateQuad(
                    $"{objectName}_Card{i}",
                    root,
                    baseCenter + new Vector3(0f, cardSize.y * scaleJitter * 0.5f, 0f),
                    new Vector3(cardSize.x * scaleJitter, cardSize.y * scaleJitter, 1f),
                    cardMaterial);
                quad.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
                if (tightMesh)
                {
                    ApplyHd2dAutonomousP0FoliageTightMesh(quad, cardMaterial);
                }

                AddHd2dAutonomousP2FoliageVarietyMarker(quad, type, placementGroup, seamSoftener);
            }
        }

        private static void AddHd2dAutonomousP2FoliageVarietyMarker(
            GameObject target,
            FastVsHd2dFoliageVarietyAccentType type,
            string placementGroup,
            bool seamSoftener)
        {
            var marker = target.GetComponent<FastVsHd2dFoliageVarietyMarker>();
            if (marker == null)
            {
                marker = target.AddComponent<FastVsHd2dFoliageVarietyMarker>();
            }

            marker.ConfigureForReview(type, placementGroup, seamSoftener, true);
            var landmark = target.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                landmark = target.AddComponent<TimeWindowPairedSpaceLandmark>();
            }

            SerializedSet(landmark, "landmarkId", $"current.central_plaza.p2_59.{placementGroup}.{target.name}");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);
            EditorUtility.SetDirty(marker);
            EditorUtility.SetDirty(landmark);
            EditorUtility.SetDirty(target);
        }

        private static Material EnsureHd2dAutonomousP2FoliageVarietyCardMaterial(
            string materialId,
            Texture2D texture,
            Color tint,
            float windStrength,
            float vegetationControlWeight)
        {
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var shader = Shader.Find(FoliageCardLitShaderName) ?? Shader.Find(SpriteCardRampLitShaderName) ?? Shader.Find(SpriteCardRampShaderName) ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Required foliage variety shader not found for {materialId}.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, materialPath);
            }

            if (string.Equals(shader.name, FoliageCardLitShaderName, StringComparison.Ordinal) ||
                string.Equals(shader.name, SpriteCardRampLitShaderName, StringComparison.Ordinal))
            {
                ConfigureSpriteCardCutoutMaterial(material, SpriteCardCutoutRenderQueue, shader.name);
            }
            else
            {
                ConfigureTransparentMaterial(material, SpriteCardCutoutRenderQueue, SpriteCardRampShaderName, URPUnlitShaderName);
            }

            AssignMaterialTexture(material, texture, Vector2.one);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", tint);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", tint);
            }

            ApplySpriteCardRampProfile(material);
            if (material.HasProperty("_WindStrength"))
            {
                material.SetFloat("_WindStrength", windStrength);
            }

            if (material.HasProperty("_WindSpeed"))
            {
                material.SetFloat("_WindSpeed", 0.90f + Mathf.Clamp01(windStrength * 5f));
            }

            SetSharedVegetationMaterialWeights(material, vegetationControlWeight, 0f);
            ApplyStage2EmissionDefaults(material);
            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.SpriteCard);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureHd2dAutonomousP2FoliageVarietySolidMaterial(string materialId, Color color)
        {
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var shader = Shader.Find(SurfaceRampLitShaderName) ?? Shader.Find(URPLitShaderName) ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Required foliage variety solid shader not found for {materialId}.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, materialPath);
            }
            else if (material.shader == null || !string.Equals(material.shader.name, shader.name, StringComparison.Ordinal))
            {
                material.shader = shader;
            }

            material.enableInstancing = true;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            if (string.Equals(shader.name, SurfaceRampLitShaderName, StringComparison.Ordinal))
            {
                ApplySurfaceRampProfile(material, materialId);
            }
            else
            {
                if (material.HasProperty("_Metallic"))
                {
                    material.SetFloat("_Metallic", 0f);
                }

                if (material.HasProperty("_Smoothness"))
                {
                    material.SetFloat("_Smoothness", 0.12f);
                }
            }

            if (material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 0.08f);
            }

            ApplyStage2EmissionDefaults(material);
            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Color BoostHd2dAutonomousP2FoliageVarietyReviewColor(Color color, float lift, float gain)
        {
            return new Color(
                Mathf.Clamp01(color.r * gain + lift),
                Mathf.Clamp01(color.g * gain + lift),
                Mathf.Clamp01(color.b * gain + lift),
                color.a);
        }

        private static Texture2D EnsureHd2dAutonomousP2FoliageVarietyFallenLeafTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2FoliageVarietyFallenLeafTextureId,
                64,
                64,
                FilterMode.Point,
                (x, y) =>
                {
                    var u = x / 63f;
                    var v = y / 63f;
                    var alpha = SampleRotatedEllipseAlpha(u, v, 0.50f, 0.50f, 0.34f, 0.13f, -0.42f);
                    alpha = Mathf.Max(alpha, SampleRotatedEllipseAlpha(u, v, 0.39f, 0.52f, 0.15f, 0.055f, 0.72f) * 0.82f);
                    alpha = Mathf.Max(alpha, SampleRotatedEllipseAlpha(u, v, 0.61f, 0.48f, 0.15f, 0.055f, 0.72f) * 0.82f);
                    if (alpha <= 0.025f)
                    {
                        return new Color(0f, 0f, 0f, 0f);
                    }

                    var vein = Mathf.Abs(v - (0.50f + (u - 0.50f) * -0.18f)) < 0.018f ? 0.12f : 0f;
                    var value = Mathf.Clamp01(0.76f + vein + ((x + y) % 5 == 0 ? 0.06f : 0f));
                    return new Color(value, value, value, Mathf.Clamp01(alpha));
                });
        }

        private static Texture2D EnsureHd2dAutonomousP2FoliageVarietyVineTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2FoliageVarietyVineTextureId,
                64,
                64,
                FilterMode.Point,
                (x, y) =>
                {
                    var u = x / 63f;
                    var v = y / 63f;
                    var stem = Mathf.Abs(u - (0.50f + Mathf.Sin(v * 9.0f) * 0.055f)) < 0.030f ? 0.90f : 0f;
                    var alpha = stem;
                    alpha = Mathf.Max(alpha, SampleRotatedEllipseAlpha(u, v, 0.34f, 0.22f, 0.17f, 0.075f, -0.62f));
                    alpha = Mathf.Max(alpha, SampleRotatedEllipseAlpha(u, v, 0.66f, 0.34f, 0.17f, 0.075f, 0.62f));
                    alpha = Mathf.Max(alpha, SampleRotatedEllipseAlpha(u, v, 0.33f, 0.52f, 0.18f, 0.075f, -0.58f));
                    alpha = Mathf.Max(alpha, SampleRotatedEllipseAlpha(u, v, 0.67f, 0.72f, 0.18f, 0.075f, 0.58f));
                    if (alpha <= 0.025f)
                    {
                        return new Color(0f, 0f, 0f, 0f);
                    }

                    var value = Mathf.Clamp01(0.70f + v * 0.18f + (((x * 3 + y) & 7) == 0 ? 0.08f : 0f));
                    return new Color(value, value, value, Mathf.Clamp01(alpha));
                });
        }

        private static Texture2D EnsureHd2dAutonomousP2FoliageVarietyMossTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2FoliageVarietyMossTextureId,
                64,
                64,
                FilterMode.Point,
                (x, y) =>
                {
                    var u = x / 63f;
                    var v = y / 63f;
                    var band = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.10f, 0.32f, v)) *
                        (1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.72f, 0.96f, v)));
                    var edgeNoise = (((x * 17 + y * 11) & 15) / 15f) * 0.18f;
                    var scallop = 0.78f + Mathf.Sin(u * 19.0f + y * 0.19f) * 0.12f;
                    var alpha = Mathf.Clamp01(band * scallop + edgeNoise);
                    if (alpha <= 0.08f)
                    {
                        return new Color(0f, 0f, 0f, 0f);
                    }

                    var value = Mathf.Clamp01(0.58f + edgeNoise + (v * 0.10f));
                    return new Color(value, value, value, Mathf.Clamp01(alpha));
                });
        }

        private static float SampleRotatedEllipseAlpha(float u, float v, float centerX, float centerY, float radiusX, float radiusY, float rotation)
        {
            var dx = u - centerX;
            var dy = v - centerY;
            var cos = Mathf.Cos(rotation);
            var sin = Mathf.Sin(rotation);
            var rx = (dx * cos) - (dy * sin);
            var ry = (dx * sin) + (dy * cos);
            var distance = (rx * rx) / Mathf.Max(0.0001f, radiusX * radiusX) + (ry * ry) / Mathf.Max(0.0001f, radiusY * radiusY);
            return Mathf.Clamp01(1f - Mathf.SmoothStep(0.72f, 1.0f, distance));
        }

        private static void CaptureHd2dAutonomousP2FoliageVarietyShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHouseArea area,
            Vector3 playerLocal,
            Vector3 focusLocal,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            GameObject root,
            bool rootActive,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            root.SetActive(rootActive);
            CaptureCloseReviewScreenshotWithoutPlayer(
                controller,
                visibility,
                guide,
                camera,
                area,
                playerLocal,
                focusLocal,
                cameraOffset,
                lookOffset,
                outputDirectory,
                fileName);
            rows?.Add($"| `{fileName}` | {label} | {FormatBool(rootActive)} |");
        }

        private static void WriteHd2dAutonomousP2FoliageVarietyReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dFoliageVarietyProfile profile,
            Hd2dAutonomousP2FoliageVarietyDiffMetrics overviewDiff,
            Hd2dAutonomousP2FoliageVarietyDiffMetrics eveningDiff)
        {
            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dFoliageVarietyMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            CountHd2dAutonomousP2FoliageVarietyMarkers(markers, out var flower, out var fallenLeaf, out var vine, out var moss, out var seam);
            var lines = new List<string>
            {
                "# P2-59 Foliage Variety Accent Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for ground flowers, settled fallen leaves, wall vines, and moss seam strips.",
                "- Pass2 visibility support: raised solid review slabs are generated under the same root because the pass1 alpha-card A/B measured no visible changed pixels in capture; these remain conservative review aids, not final art approval.",
                $"- Recommendation: {profile.RecommendationForReview}",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2FoliageVarietyProfilePath}` |",
                $"| Root | `{Hd2dAutonomousP2FoliageVarietyRootName}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalFoliageVarietyApprovedForReview)} |",
                $"| Flower warm / cool | {FormatColor(profile.FlowerWarmTintForReview)} / {FormatColor(profile.FlowerCoolTintForReview)} |",
                $"| Fallen leaf / vine / moss | {FormatColor(profile.FallenLeafTintForReview)} / {FormatColor(profile.VineTintForReview)} / {FormatColor(profile.MossTintForReview)} |",
                $"| Accent wind / seam softening | {profile.AccentWindStrengthForReview:0.###} / {profile.SeamSofteningStrengthForReview:0.###} |",
                string.Empty,
                "| Marker Count | Value |",
                "|---|---:|",
                $"| Flower cards | {flower} |",
                $"| Fallen leaves | {fallenLeaf} |",
                $"| Vine strips | {vine} |",
                $"| Moss seam strips | {moss} |",
                $"| Seam softeners | {seam} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                overviewDiff.ToReportRow("Accent root off vs on"),
                eveningDiff.ToReportRow("Noon accent pass vs evening color-pop check"),
                string.Empty,
                "| Screenshot | Label | Accent root active |",
                "|---|---|---|"
            };
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
                lines.Add($"| `{screenshotFiles[i]}` | P2-59 review capture {i + 1} |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "foliage_variety_accents_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static void CountHd2dAutonomousP2FoliageVarietyMarkers(
            IReadOnlyList<FastVsHd2dFoliageVarietyMarker> markers,
            out int flower,
            out int fallenLeaf,
            out int vine,
            out int moss,
            out int seam)
        {
            flower = 0;
            fallenLeaf = 0;
            vine = 0;
            moss = 0;
            seam = 0;
            for (var i = 0; i < markers.Count; i++)
            {
                var marker = markers[i];
                if (marker == null || !marker.gameObject.scene.IsValid())
                {
                    continue;
                }

                if (marker.SeamSoftenerForReview)
                {
                    seam++;
                }

                switch (marker.AccentTypeForReview)
                {
                    case FastVsHd2dFoliageVarietyAccentType.Flower:
                        flower++;
                        break;
                    case FastVsHd2dFoliageVarietyAccentType.FallenLeaf:
                        fallenLeaf++;
                        break;
                    case FastVsHd2dFoliageVarietyAccentType.Vine:
                        vine++;
                        break;
                    case FastVsHd2dFoliageVarietyAccentType.Moss:
                        moss++;
                        break;
                }
            }
        }

        private static Hd2dAutonomousP2FoliageVarietyDiffMetrics MeasureHd2dAutonomousP2FoliageVarietyDiff(string outputDirectory, string firstFile, string secondFile)
        {
            var firstPath = Path.Combine(outputDirectory, firstFile);
            var secondPath = Path.Combine(outputDirectory, secondFile);
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP2FoliageVarietyDiffMetrics(0, 0, 0f, 0f);
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

                return new Hd2dAutonomousP2FoliageVarietyDiffMetrics(
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

        private readonly struct Hd2dAutonomousP2FoliageVarietyDiffMetrics
        {
            private readonly int sampleCount;
            private readonly int changedPixels;
            private readonly float changedPercent;
            private readonly float meanRgbDelta;

            public Hd2dAutonomousP2FoliageVarietyDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
            {
                this.sampleCount = sampleCount;
                this.changedPixels = changedPixels;
                this.changedPercent = changedPercent;
                this.meanRgbDelta = meanRgbDelta;
            }

            public string ToReportRow(string label)
            {
                return $"| {label} | {sampleCount} | {changedPixels} | {changedPercent:0.###} | {meanRgbDelta:0.###} |";
            }
        }
    }
}
