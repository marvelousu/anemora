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
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2PropClutterScatterRootName = "Current_AriaStreet_P2_74_PropClutterScatter";
        private const string Hd2dAutonomousP2PropClutterScatterProfilePath = "Assets/Settings/FastVS_HD2D_P2_PropClutterScatterProfile.asset";
        private const string Hd2dAutonomousP2PropClutterScatterProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dPropClutterScatterProfile.cs";
        private const string Hd2dAutonomousP2PropClutterScatterMarkerRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dPropClutterScatterMarker.cs";
        private const string Hd2dAutonomousP2PropClutterScatterEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2PropClutterScatter.cs";

        public static void CaptureHd2dAutonomousP2Item74PropClutterScatterBatch()
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
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2PropClutterScatterRootName);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || root == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-74 prop clutter scatter capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2PropClutterScatter();
            var profile = EnsureHd2dAutonomousP2PropClutterScatterProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("prop_clutter_scatter_lived_in");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_before_same_street_clutter_off.png",
                "02_after_conservative_clutter_on.png",
                "03_wall_base_weeds_seam_close.png",
                "04_bare_corner_props_close.png",
                "05_topdown_scatter_density_diagnostic.png",
            };
            var shotRows = new List<string>();
            var streetAnchor = Chapter1AriaStreetMapCenter + new Vector3(-1.0f, 0.02f, 5.4f);
            var seamAnchor = Chapter1AriaStreetMapCenter + new Vector3(-10.5f, 0.02f, 5.2f);
            var cornerAnchor = Chapter1AriaStreetMapCenter + new Vector3(6.8f, 0.02f, 4.2f);
            var topDownAnchor = Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 5.6f);
            var previousRootActive = root.activeSelf;

            try
            {
                guide.SetMovementFrozen(true);
                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();

                CaptureHd2dAutonomousP2PropClutterScatterShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    streetAnchor,
                    new Vector3(0.65f, 10.2f, -12.8f),
                    new Vector3(0.04f, 1.28f, 0.34f),
                    39f,
                    root,
                    false,
                    outputDirectory,
                    screenshotFiles[0],
                    "same Aria Street framing with P2-74 clutter root disabled",
                    shotRows);

                CaptureHd2dAutonomousP2PropClutterScatterShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    streetAnchor,
                    new Vector3(0.65f, 10.2f, -12.8f),
                    new Vector3(0.04f, 1.28f, 0.34f),
                    39f,
                    root,
                    true,
                    outputDirectory,
                    screenshotFiles[1],
                    "same Aria Street framing with conservative scatter enabled",
                    shotRows);

                CaptureHd2dAutonomousP2PropClutterScatterShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    seamAnchor,
                    new Vector3(-0.55f, 4.9f, -7.0f),
                    new Vector3(0.04f, 1.02f, 0.34f),
                    32f,
                    root,
                    true,
                    outputDirectory,
                    screenshotFiles[2],
                    "wall-base weeds and low scatter seam close-up",
                    shotRows);

                CaptureHd2dAutonomousP2PropClutterScatterShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    cornerAnchor,
                    new Vector3(-0.10f, 5.4f, -7.6f),
                    new Vector3(0.04f, 1.05f, 0.34f),
                    32f,
                    root,
                    true,
                    outputDirectory,
                    screenshotFiles[3],
                    "bare-corner pots, sacks, firewood, and crates",
                    shotRows);

                CaptureHd2dAutonomousP2PropClutterScatterShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    topDownAnchor,
                    new Vector3(0.50f, 21.8f, -19.8f),
                    new Vector3(0.00f, 1.20f, 0.28f),
                    49f,
                    root,
                    true,
                    outputDirectory,
                    screenshotFiles[4],
                    "top-down diagnostic: scatter stays in the sharp street band and leaves the travel lane readable",
                    shotRows);
            }
            finally
            {
                root.SetActive(previousRootActive || profile.ConservativeScatterEnabledForReview);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                guide.SetMovementFrozen(false);
                AssetDatabase.SaveAssets();
            }

            var beforeAfterDiff = MeasureHd2dAutonomousP2PropClutterScatterDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var seamAfterDiff = MeasureHd2dAutonomousP2PropClutterScatterDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            WriteHd2dAutonomousP2PropClutterScatterReviewReport(outputDirectory, screenshotFiles, shotRows, profile, beforeAfterDiff, seamAfterDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-74 prop clutter scatter review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2PropClutterScatter(Transform currentRoot, string prefix, bool past, Materials materials)
        {
            if (currentRoot == null || past || !string.Equals(prefix, "Current", StringComparison.Ordinal))
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP2PropClutterScatterProfile();
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2PropClutterScatterRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2PropClutterScatterRootName);
            root.transform.SetParent(currentRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;

            var materialSet = EnsureHd2dAutonomousP2PropClutterScatterMaterials(materials);
            CreateHd2dAutonomousP2PropClutterScatterWallBaseWeeds(root.transform, profile, materialSet.Weed, materialSet.WeedCard);
            CreateHd2dAutonomousP2PropClutterScatterPots(root.transform, profile, materialSet.Pottery);
            CreateHd2dAutonomousP2PropClutterScatterSacksAndCrates(root.transform, profile, materialSet.Sack, materialSet.Crate, materialSet.Barrel);
            CreateHd2dAutonomousP2PropClutterScatterFirewood(root.transform, profile, materialSet.Firewood);
            CreateHd2dAutonomousP2PropClutterScatterLaundry(root.transform, profile, materialSet.LaundryWarm, materialSet.LaundryCool, materialSet.Rope);
            CreateHd2dAutonomousP2PropClutterScatterGroundAccents(root.transform, profile, materialSet.Puddle, materialSet.FallenLeaf);
            CreateHd2dAutonomousP2PropClutterScatterFocalBandReadabilityStrip(root.transform, profile, materialSet.Crate, materialSet.Sack, materialSet.Weed, materialSet.FallenLeaf);

            ApplyHd2dAutonomousP0StaticFlags(root);
            SetHd2dAutonomousP2PropClutterScatterLayerRecursively(root.transform, CurrentSpaceRenderLayer);
            root.SetActive(profile.ConservativeScatterEnabledForReview);
            EditorUtility.SetDirty(root);
            _ = materials;
        }

        private static void ValidateHd2dAutonomousP2PropClutterScatter()
        {
            var profile = EnsureHd2dAutonomousP2PropClutterScatterProfile();
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2PropClutterScatterRootName);
            if (profile == null ||
                root == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalPropClutterApprovedForReview ||
                !profile.ConservativeScatterEnabledForReview ||
                !root.activeSelf)
            {
                throw new InvalidOperationException("House slice validation failed: P2-74 needs an active conservative prop clutter scatter root with final approval left to Tom.");
            }

            var markers = root.GetComponentsInChildren<FastVsHd2dPropClutterScatterMarker>(true);
            var renderers = root.GetComponentsInChildren<Renderer>(true);
            var colliderCount = root.GetComponentsInChildren<Collider>(true).Length;
            var typeCount = markers.Select(marker => marker.ClutterTypeForReview).Distinct().Count();
            var bareCornerCount = markers.Count(marker => marker.BareCornerFillForReview);
            var focalBandCount = markers.Count(marker => marker.InFocalBandForReview);
            var seamMeters = markers.Sum(marker => marker.SeamBreakMetersForReview);
            var seamCoverage = seamMeters / profile.ReviewWallBaseSeamMetersForReview;
            var instancedRenderers = renderers.Count(renderer => renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.enableInstancing);
            var staticReady = markers.Count(marker =>
            {
                var flags = GameObjectUtility.GetStaticEditorFlags(marker.gameObject);
                return (flags & StaticEditorFlags.BatchingStatic) != 0 && (flags & StaticEditorFlags.ContributeGI) != 0;
            });

            if (markers.Length < profile.MinimumMarkerCountForReview ||
                typeCount < profile.MinimumClutterTypeCountForReview ||
                bareCornerCount < profile.MinimumBareCornerFillCountForReview ||
                focalBandCount < markers.Length ||
                seamCoverage < 0.60f ||
                seamCoverage < profile.TargetWallBaseSeamCoverageForReview - 0.04f ||
                colliderCount != 0 ||
                renderers.Length == 0 ||
                instancedRenderers < renderers.Length ||
                staticReady < markers.Length)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: P2-74 clutter scatter metrics are short. markers={markers.Length}, types={typeCount}, bareCorners={bareCornerCount}, seamCoverage={seamCoverage:0.###}, colliders={colliderCount}, instanced={instancedRenderers}/{renderers.Length}, static={staticReady}/{markers.Length}.");
            }

            if (profile.ScaleJitterRangeForReview.x >= profile.ScaleJitterRangeForReview.y ||
                Mathf.Abs(profile.YawJitterDegreesForReview.x) < 4f ||
                Mathf.Abs(profile.YawJitterDegreesForReview.y) < 4f ||
                profile.FarScatterCullDistanceMetersForReview < 20f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-74 profile must record conservative scale/yaw variation and a far scatter cull distance.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2PropClutterScatterProfileRuntimePath), "finalPropClutterApproved", Hd2dAutonomousP2PropClutterScatterProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2PropClutterScatterMarkerRuntimePath), "FastVsHd2dPropClutterScatterType", Hd2dAutonomousP2PropClutterScatterMarkerRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2PropClutterScatterEditorPath), "ReviewWallBaseSeamMetersForReview", Hd2dAutonomousP2PropClutterScatterEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2PropClutterScatter", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2PropClutterScatter", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dPropClutterScatterProfile EnsureHd2dAutonomousP2PropClutterScatterProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dPropClutterScatterProfile>(Hd2dAutonomousP2PropClutterScatterProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dPropClutterScatterProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2PropClutterScatterProfilePath);
            }

            profile.ConfigureForReview(
                72,
                8,
                6,
                13.20f,
                0.68f,
                0f,
                34f,
                36f,
                new Vector2(0.88f, 1.12f),
                new Vector2(-18f, 18f),
                true,
                true,
                true,
                true,
                false,
                "Procedural CC0-safe P2-74 review stand-ins; replace with approved Quaternius/Kenney/itch CC0 prop meshes and hand-placed brush scatter after Tom density approval.",
                "Keep the conservative wall-base and corner scatter as data prep only. Tom should tune final density, silhouettes, walkable-path exclusions, approved prop kit meshes, and per-area clutter language.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Hd2dAutonomousP2PropClutterScatterMaterials EnsureHd2dAutonomousP2PropClutterScatterMaterials(Materials materials)
        {
            var pottery = EnsureHd2dAutonomousP2PropClutterScatterPixelMaterial("hd2d_p2_74_clutter_pottery", new Color32(78, 54, 42, 255), new Color32(126, 83, 56, 255), new Color32(44, 36, 34, 255), PixelPattern.Stone, new Vector2(1.2f, 1.2f));
            var sack = EnsureHd2dAutonomousP2PropClutterScatterPixelMaterial("hd2d_p2_74_clutter_sack", new Color32(116, 96, 72, 255), new Color32(158, 132, 92, 255), new Color32(74, 62, 50, 255), PixelPattern.Noise, new Vector2(1.1f, 1.1f));
            var firewood = EnsureHd2dAutonomousP2PropClutterScatterPixelMaterial("hd2d_p2_74_clutter_firewood", new Color32(70, 44, 30, 255), new Color32(122, 76, 42, 255), new Color32(36, 28, 22, 255), PixelPattern.Planks, new Vector2(1.4f, 1.0f));
            var crate = EnsureHd2dAutonomousP2PropClutterScatterPixelMaterial("hd2d_p2_74_clutter_corner_crate", new Color32(76, 52, 36, 255), new Color32(128, 88, 48, 255), new Color32(40, 32, 26, 255), PixelPattern.Planks, new Vector2(1.6f, 1.2f));
            var barrel = EnsureHd2dAutonomousP2PropClutterScatterPixelMaterial("hd2d_p2_74_clutter_barrel", new Color32(82, 55, 36, 255), new Color32(132, 88, 48, 255), new Color32(42, 32, 26, 255), PixelPattern.Planks, new Vector2(1.2f, 1.2f));
            var weed = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial("hd2d_p2_74_clutter_visible_weeds", new Color(0.25f, 0.48f, 0.24f, 1f));
            var weedCard = EnsureHd2dAutonomousP2FoliageVarietyCardMaterial("hd2d_p2_74_clutter_alpha_clip_weeds", EnsureFoliageCardTexture(FoliageBushCardTexturePath), new Color(0.31f, 0.56f, 0.28f, 1f), 0.035f, 1f);
            var puddle = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial("hd2d_p2_74_clutter_puddle_review", new Color(0.16f, 0.23f, 0.27f, 1f));
            var fallenLeaf = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial("hd2d_p2_74_clutter_fallen_leaf", new Color(0.72f, 0.38f, 0.14f, 1f));
            var laundryWarm = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial("hd2d_p2_74_clutter_laundry_warm", new Color(0.86f, 0.68f, 0.48f, 1f));
            var laundryCool = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial("hd2d_p2_74_clutter_laundry_cool", new Color(0.46f, 0.58f, 0.82f, 1f));
            var rope = EnsureHd2dAutonomousP2FoliageVarietySolidMaterial("hd2d_p2_74_clutter_laundry_rope", new Color(0.48f, 0.35f, 0.20f, 1f));

            foreach (var material in new[] { pottery, sack, firewood, crate, barrel, weed, weedCard, puddle, fallenLeaf, laundryWarm, laundryCool, rope })
            {
                if (material == null)
                {
                    continue;
                }

                material.enableInstancing = true;
                EditorUtility.SetDirty(material);
            }

            _ = materials;
            return new Hd2dAutonomousP2PropClutterScatterMaterials(pottery, sack, firewood, crate, barrel, weed, weedCard, puddle, fallenLeaf, laundryWarm, laundryCool, rope);
        }

        private static Material EnsureHd2dAutonomousP2PropClutterScatterPixelMaterial(string materialId, Color32 dark, Color32 mid, Color32 high, PixelPattern pattern, Vector2 tiling)
        {
            var material = PixelMaterial(materialId, mid, high, dark, pattern, false, tiling);
            material.enableInstancing = true;
            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void CreateHd2dAutonomousP2PropClutterScatterWallBaseWeeds(Transform root, FastVsHd2dPropClutterScatterProfile profile, Material weedMaterial, Material weedCardMaterial)
        {
            var c = Chapter1AriaStreetMapCenter;
            for (var i = 0; i < 18; i++)
            {
                var t = i / 17f;
                var x = Mathf.Lerp(-17.4f, 13.6f, t) + Mathf.Sin(i * 1.87f) * 0.16f;
                var z = 4.04f + Mathf.Sin(i * 1.23f) * 0.20f;
                var scale = Mathf.Lerp(profile.ScaleJitterRangeForReview.x, profile.ScaleJitterRangeForReview.y, PositiveModulo(i * 7, 11) / 10f);
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_WallBaseWeedSlab_{i:00}",
                    FastVsHd2dPropClutterScatterType.Weed,
                    "aria_street_wall_base_weed_slab",
                    c + new Vector3(x, 0.145f + i * 0.0008f, z),
                    new Vector3(0.42f * scale, 0.075f, 0.16f * scale),
                    Quaternion.Euler(0f, -12f + i * 7f, 0f),
                    weedMaterial,
                    0.34f,
                    true,
                    i % 6 == 0,
                    profile);
            }

            for (var i = 0; i < 10; i++)
            {
                var x = Mathf.Lerp(-16.2f, 12.8f, i / 9f) + Mathf.Cos(i * 1.31f) * 0.22f;
                var z = 4.18f + Mathf.Sin(i * 0.91f) * 0.14f;
                var card = CreateQuad(
                    $"P2_74_WallBaseAlphaWeedCard_{i:00}",
                    root,
                    c + new Vector3(x, 0.42f + i * 0.001f, z),
                    new Vector3(0.36f + PositiveModulo(i, 3) * 0.06f, 0.58f + PositiveModulo(i * 2, 3) * 0.05f, 1f),
                    weedCardMaterial);
                card.transform.localRotation = Quaternion.Euler(0f, -2f + i * 3f, i % 2 == 0 ? -3f : 3f);
                ApplyHd2dAutonomousP0FoliageTightMesh(card, weedCardMaterial);
                ConfigureHd2dAutonomousP2PropClutterScatterMarker(card, FastVsHd2dPropClutterScatterType.Weed, "aria_street_wall_base_alpha_clip_weed_card", 0.20f, true, i % 5 == 0, profile);
            }
        }

        private static void CreateHd2dAutonomousP2PropClutterScatterPots(Transform root, FastVsHd2dPropClutterScatterProfile profile, Material pottery)
        {
            var c = Chapter1AriaStreetMapCenter;
            var placements = new[]
            {
                new Vector4(-16.6f, 3.72f, 0.34f, -9f),
                new Vector4(-13.2f, 4.24f, 0.28f, 11f),
                new Vector4(-6.1f, 3.82f, 0.30f, -14f),
                new Vector4(1.8f, 4.12f, 0.32f, 8f),
                new Vector4(7.3f, 3.84f, 0.29f, -6f),
                new Vector4(12.7f, 4.18f, 0.31f, 16f),
            };

            for (var i = 0; i < placements.Length; i++)
            {
                var p = placements[i];
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cylinder,
                    $"P2_74_PotteryClusterPot_{i:00}",
                    FastVsHd2dPropClutterScatterType.Pot,
                    "aria_street_wall_base_pottery",
                    c + new Vector3(p.x, 0.25f, p.y),
                    new Vector3(p.z, 0.42f + PositiveModulo(i, 2) * 0.06f, p.z),
                    Quaternion.Euler(0f, p.w, 0f),
                    pottery,
                    0.16f,
                    true,
                    i == 0 || i == placements.Length - 1,
                    profile);
            }
        }

        private static void CreateHd2dAutonomousP2PropClutterScatterSacksAndCrates(Transform root, FastVsHd2dPropClutterScatterProfile profile, Material sack, Material crate, Material barrel)
        {
            var c = Chapter1AriaStreetMapCenter;
            for (var i = 0; i < 6; i++)
            {
                var x = -4.8f + i * 0.42f + Mathf.Sin(i * 1.4f) * 0.06f;
                var z = 3.78f + PositiveModulo(i, 3) * 0.18f;
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_SackStack_{i:00}",
                    FastVsHd2dPropClutterScatterType.Sack,
                    "aria_street_corner_sack_stack",
                    c + new Vector3(x, 0.20f + (i / 3) * 0.10f, z),
                    new Vector3(0.36f, 0.24f, 0.30f),
                    Quaternion.Euler(0f, -12f + i * 9f, i % 2 == 0 ? 3f : -4f),
                    sack,
                    0.18f,
                    true,
                    true,
                    profile);
            }

            for (var i = 0; i < 6; i++)
            {
                var x = 6.82f + PositiveModulo(i, 3) * 0.38f;
                var z = 3.72f + (i / 3) * 0.34f;
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_CornerCrate_{i:00}",
                    FastVsHd2dPropClutterScatterType.CornerCrate,
                    "aria_street_bare_corner_crate_fill",
                    c + new Vector3(x, 0.25f + (i / 3) * 0.08f, z),
                    new Vector3(0.38f, 0.34f, 0.34f),
                    Quaternion.Euler(0f, 8f + i * 11f, 0f),
                    crate,
                    0.12f,
                    true,
                    true,
                    profile);
            }

            for (var i = 0; i < 4; i++)
            {
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cylinder,
                    $"P2_74_PathsideBarrel_{i:00}",
                    FastVsHd2dPropClutterScatterType.Barrel,
                    "aria_street_pathside_barrel_corner_fill",
                    c + new Vector3(12.1f + i * 0.44f, 0.26f, 3.64f + Mathf.Sin(i * 1.6f) * 0.16f),
                    new Vector3(0.32f, 0.48f, 0.32f),
                    Quaternion.Euler(0f, 4f + i * 17f, 90f),
                    barrel,
                    0.12f,
                    true,
                    i < 2,
                    profile);
            }
        }

        private static void CreateHd2dAutonomousP2PropClutterScatterFirewood(Transform root, FastVsHd2dPropClutterScatterProfile profile, Material firewood)
        {
            var c = Chapter1AriaStreetMapCenter;
            var bases = new[]
            {
                c + new Vector3(-10.4f, 0.18f, 3.84f),
                c + new Vector3(9.25f, 0.18f, 4.06f),
            };

            for (var cluster = 0; cluster < bases.Length; cluster++)
            {
                for (var i = 0; i < 6; i++)
                {
                    CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                        root,
                        PrimitiveType.Cube,
                        $"P2_74_Firewood_{cluster:00}_{i:00}",
                        FastVsHd2dPropClutterScatterType.Firewood,
                        "aria_street_firewood_stack",
                        bases[cluster] + new Vector3((i % 3) * 0.20f, (i / 3) * 0.10f, (i / 3) * 0.08f),
                        new Vector3(0.42f, 0.075f, 0.105f),
                        Quaternion.Euler(0f, cluster == 0 ? -11f : 14f, i % 2 == 0 ? 4f : -5f),
                        firewood,
                        i < 2 ? 0.08f : 0f,
                        true,
                        cluster == 1 && i < 2,
                        profile);
                }
            }
        }

        private static void CreateHd2dAutonomousP2PropClutterScatterLaundry(Transform root, FastVsHd2dPropClutterScatterProfile profile, Material laundryWarm, Material laundryCool, Material rope)
        {
            var c = Chapter1AriaStreetMapCenter;
            CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                root,
                PrimitiveType.Cube,
                "P2_74_HangingLaundryRope",
                FastVsHd2dPropClutterScatterType.HangingLaundry,
                "aria_street_hanging_laundry_rope",
                c + new Vector3(-1.8f, 1.56f, 4.34f),
                new Vector3(3.10f, 0.035f, 0.035f),
                Quaternion.Euler(0f, -2f, 0f),
                rope,
                0f,
                true,
                false,
                profile);

            for (var i = 0; i < 5; i++)
            {
                var material = i % 2 == 0 ? laundryWarm : laundryCool;
                var cloth = CreateQuad(
                    $"P2_74_HangingLaundryCloth_{i:00}",
                    root,
                    c + new Vector3(-3.05f + i * 0.58f, 1.30f - PositiveModulo(i, 2) * 0.04f, 4.28f + Mathf.Sin(i) * 0.04f),
                    new Vector3(0.36f + PositiveModulo(i, 3) * 0.04f, 0.46f + PositiveModulo(i * 2, 3) * 0.04f, 1f),
                    material);
                cloth.transform.localRotation = Quaternion.Euler(0f, -2f + i * 2f, i % 2 == 0 ? -2f : 3f);
                ConfigureHd2dAutonomousP2PropClutterScatterMarker(cloth, FastVsHd2dPropClutterScatterType.HangingLaundry, "aria_street_hanging_laundry_cloth", 0f, true, false, profile);
            }
        }

        private static void CreateHd2dAutonomousP2PropClutterScatterGroundAccents(Transform root, FastVsHd2dPropClutterScatterProfile profile, Material puddle, Material fallenLeaf)
        {
            var c = Chapter1AriaStreetMapCenter;
            for (var i = 0; i < 4; i++)
            {
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_ShallowPuddle_{i:00}",
                    FastVsHd2dPropClutterScatterType.Puddle,
                    "aria_street_path_puddle",
                    c + new Vector3(-12.2f + i * 6.1f, 0.118f + i * 0.001f, 2.60f + Mathf.Sin(i * 1.2f) * 0.30f),
                    new Vector3(0.82f + PositiveModulo(i, 2) * 0.18f, 0.022f, 0.32f),
                    Quaternion.Euler(0f, -18f + i * 13f, 0f),
                    puddle,
                    0f,
                    true,
                    false,
                    profile);
            }

            for (var i = 0; i < 14; i++)
            {
                var t = i / 13f;
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_FallenLeafScatter_{i:00}",
                    FastVsHd2dPropClutterScatterType.FallenLeaf,
                    "aria_street_settled_fallen_leaf_scatter",
                    c + new Vector3(Mathf.Lerp(-15.8f, 10.4f, t) + Mathf.Sin(i * 1.9f) * 0.28f, 0.142f + i * 0.001f, 3.02f + Mathf.Cos(i * 1.33f) * 0.42f),
                    new Vector3(0.28f + PositiveModulo(i, 3) * 0.035f, 0.026f, 0.105f),
                    Quaternion.Euler(0f, -24f + i * 19f, 0f),
                    fallenLeaf,
                    0f,
                    true,
                    false,
                    profile);
            }
        }

        private static void CreateHd2dAutonomousP2PropClutterScatterFocalBandReadabilityStrip(Transform root, FastVsHd2dPropClutterScatterProfile profile, Material crate, Material sack, Material weed, Material fallenLeaf)
        {
            var c = Chapter1AriaStreetMapCenter;
            for (var i = 0; i < 4; i++)
            {
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_FocalBandPathEdgeCrate_{i:00}",
                    FastVsHd2dPropClutterScatterType.CornerCrate,
                    "aria_street_focal_band_path_edge_crate",
                    c + new Vector3(-5.2f + i * 1.15f, 0.30f, 2.72f + Mathf.Sin(i * 1.1f) * 0.12f),
                    new Vector3(0.44f, 0.38f, 0.34f),
                    Quaternion.Euler(0f, -14f + i * 12f, 0f),
                    crate,
                    0.10f,
                    true,
                    true,
                    profile);
            }

            for (var i = 0; i < 4; i++)
            {
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_FocalBandPathEdgeSack_{i:00}",
                    FastVsHd2dPropClutterScatterType.Sack,
                    "aria_street_focal_band_path_edge_sack",
                    c + new Vector3(0.85f + i * 0.62f, 0.25f + (i % 2) * 0.035f, 2.88f + Mathf.Cos(i * 0.9f) * 0.12f),
                    new Vector3(0.40f, 0.28f, 0.32f),
                    Quaternion.Euler(0f, 8f + i * 9f, i % 2 == 0 ? -3f : 4f),
                    sack,
                    0.10f,
                    true,
                    true,
                    profile);
            }

            for (var i = 0; i < 5; i++)
            {
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_FocalBandPathEdgeWeed_{i:00}",
                    FastVsHd2dPropClutterScatterType.Weed,
                    "aria_street_focal_band_path_edge_weed",
                    c + new Vector3(-4.72f + i * 1.42f, 0.162f + i * 0.001f, 3.18f + Mathf.Sin(i * 1.7f) * 0.11f),
                    new Vector3(0.50f, 0.085f, 0.18f),
                    Quaternion.Euler(0f, -20f + i * 14f, 0f),
                    weed,
                    0.18f,
                    true,
                    false,
                    profile);
            }

            for (var i = 0; i < 6; i++)
            {
                CreateHd2dAutonomousP2PropClutterScatterPrimitive(
                    root,
                    PrimitiveType.Cube,
                    $"P2_74_FocalBandReadableLeaf_{i:00}",
                    FastVsHd2dPropClutterScatterType.FallenLeaf,
                    "aria_street_focal_band_readable_leaf",
                    c + new Vector3(-4.95f + i * 1.25f, 0.154f + i * 0.001f, 2.36f + Mathf.Cos(i * 1.4f) * 0.16f),
                    new Vector3(0.36f, 0.030f, 0.12f),
                    Quaternion.Euler(0f, -24f + i * 21f, 0f),
                    fallenLeaf,
                    0f,
                    true,
                    false,
                    profile);
            }
        }

        private static GameObject CreateHd2dAutonomousP2PropClutterScatterPrimitive(
            Transform root,
            PrimitiveType primitiveType,
            string objectName,
            FastVsHd2dPropClutterScatterType type,
            string placementGroup,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation,
            Material material,
            float seamBreakMeters,
            bool inFocalBand,
            bool bareCornerFill,
            FastVsHd2dPropClutterScatterProfile profile)
        {
            var target = GameObject.CreatePrimitive(primitiveType);
            target.name = objectName;
            target.transform.SetParent(root, false);
            target.transform.localPosition = localPosition;
            target.transform.localScale = localScale;
            target.transform.localRotation = localRotation;
            if (target.TryGetComponent<Collider>(out var collider))
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.On;
                renderer.receiveShadows = true;
            }

            ConfigureHd2dAutonomousP2PropClutterScatterMarker(target, type, placementGroup, seamBreakMeters, inFocalBand, bareCornerFill, profile);
            return target;
        }

        private static void ConfigureHd2dAutonomousP2PropClutterScatterMarker(
            GameObject target,
            FastVsHd2dPropClutterScatterType type,
            string placementGroup,
            float seamBreakMeters,
            bool inFocalBand,
            bool bareCornerFill,
            FastVsHd2dPropClutterScatterProfile profile)
        {
            var marker = target.GetComponent<FastVsHd2dPropClutterScatterMarker>();
            if (marker == null)
            {
                marker = target.AddComponent<FastVsHd2dPropClutterScatterMarker>();
            }

            var renderer = target.GetComponent<Renderer>();
            var gpuInstancingReady = renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.enableInstancing;
            marker.ConfigureForReview(profile, type, placementGroup, seamBreakMeters, inFocalBand, bareCornerFill, true, gpuInstancingReady, profile.FarScatterCullDistanceMetersForReview, true);

            var landmark = target.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                landmark = target.AddComponent<TimeWindowPairedSpaceLandmark>();
            }

            SerializedSet(landmark, "landmarkId", $"current.aria_street.p2_74.{placementGroup}.{target.name}");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);
            EditorUtility.SetDirty(marker);
            EditorUtility.SetDirty(landmark);
            EditorUtility.SetDirty(target);
        }

        private static void CaptureHd2dAutonomousP2PropClutterScatterShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            Vector3 anchorLocal,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            GameObject root,
            bool rootActive,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            root.SetActive(rootActive);
            CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                controller,
                visibility,
                guide,
                realtimeRig,
                camera,
                anchorLocal,
                cameraOffset,
                lookOffset,
                fieldOfView,
                outputDirectory,
                fileName,
                label,
                null);
            rows?.Add($"| `{fileName}` | {label} | {FormatBool(rootActive)} |");
        }

        private static void WriteHd2dAutonomousP2PropClutterScatterReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dPropClutterScatterProfile profile,
            Hd2dAutonomousP2PropClutterScatterDiffMetrics beforeAfterDiff,
            Hd2dAutonomousP2PropClutterScatterDiffMetrics seamCloseDiff)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2PropClutterScatterRootName);
            var markers = root != null ? root.GetComponentsInChildren<FastVsHd2dPropClutterScatterMarker>(true) : Array.Empty<FastVsHd2dPropClutterScatterMarker>();
            var typeRows = markers
                .GroupBy(marker => marker.ClutterTypeForReview)
                .OrderBy(group => group.Key.ToString(), StringComparer.Ordinal)
                .Select(group => $"| {group.Key} | {group.Count()} | {group.Sum(marker => marker.SeamBreakMetersForReview):0.###} | {group.Count(marker => marker.BareCornerFillForReview)} |")
                .ToArray();
            var seamMeters = markers.Sum(marker => marker.SeamBreakMetersForReview);
            var seamCoverage = seamMeters / profile.ReviewWallBaseSeamMetersForReview;
            var rendererCount = root != null ? root.GetComponentsInChildren<Renderer>(true).Length : 0;
            var lines = new List<string>
            {
                "# P2-74 Prop Clutter Scatter Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for lived-in street clutter: pots, sacks, firewood, hanging laundry, weeds, puddles, fallen leaves, crates, and barrels.",
                "- Final art approval remains false; this pass records scatter density, wall-base seam coverage, and A/B readability evidence for Tom.",
                $"- Recommendation: {profile.RecommendationForReview}",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2PropClutterScatterProfilePath}` |",
                $"| Root | `{Hd2dAutonomousP2PropClutterScatterRootName}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalPropClutterApprovedForReview)} |",
                $"| Conservative scatter enabled | {FormatBool(profile.ConservativeScatterEnabledForReview)} |",
                $"| Review seam meters / covered meters / coverage | {profile.ReviewWallBaseSeamMetersForReview:0.###} / {seamMeters:0.###} / {seamCoverage:0.###} |",
                $"| Target seam coverage | {profile.TargetWallBaseSeamCoverageForReview:0.###} |",
                $"| Focal band near/far | {profile.FocalBandNearMetersForReview:0.###} / {profile.FocalBandFarMetersForReview:0.###} |",
                $"| Far scatter cull distance | {profile.FarScatterCullDistanceMetersForReview:0.###} |",
                $"| Scale jitter / yaw jitter | {FormatVector2(profile.ScaleJitterRangeForReview)} / {FormatVector2(profile.YawJitterDegreesForReview)} |",
                $"| Source note | {profile.SourceKitNoteForReview} |",
                string.Empty,
                "| Metric | Value |",
                "|---|---:|",
                $"| Markers | {markers.Length} |",
                $"| Renderers | {rendererCount} |",
                $"| Distinct clutter types | {markers.Select(marker => marker.ClutterTypeForReview).Distinct().Count()} |",
                $"| Bare-corner fill markers | {markers.Count(marker => marker.BareCornerFillForReview)} |",
                $"| Focal-band markers | {markers.Count(marker => marker.InFocalBandForReview)} |",
                string.Empty,
                "| Clutter Type | Count | Seam Meters | Bare-Corner Fills |",
                "|---|---:|---:|---:|"
            };
            lines.AddRange(typeRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                beforeAfterDiff.ToReportRow("same street clutter off vs conservative on"),
                seamCloseDiff.ToReportRow("overview after vs seam close-up framing"),
                string.Empty,
                "| Screenshot | Label | Scatter root active |",
                "|---|---|---|"
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
                lines.Add($"| `{screenshotFiles[i]}` | P2-74 review capture {i + 1} |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "prop_clutter_scatter_lived_in_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP2PropClutterScatterDiffMetrics MeasureHd2dAutonomousP2PropClutterScatterDiff(string outputDirectory, string firstFile, string secondFile)
        {
            var firstPath = Path.Combine(outputDirectory, firstFile);
            var secondPath = Path.Combine(outputDirectory, secondFile);
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP2PropClutterScatterDiffMetrics(0, 0, 0f, 0f);
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

                return new Hd2dAutonomousP2PropClutterScatterDiffMetrics(
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

        private static void SetHd2dAutonomousP2PropClutterScatterLayerRecursively(Transform root, int layer)
        {
            if (root == null)
            {
                return;
            }

            foreach (var child in root.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = layer;
            }
        }

        private readonly struct Hd2dAutonomousP2PropClutterScatterMaterials
        {
            public readonly Material Pottery;
            public readonly Material Sack;
            public readonly Material Firewood;
            public readonly Material Crate;
            public readonly Material Barrel;
            public readonly Material Weed;
            public readonly Material WeedCard;
            public readonly Material Puddle;
            public readonly Material FallenLeaf;
            public readonly Material LaundryWarm;
            public readonly Material LaundryCool;
            public readonly Material Rope;

            public Hd2dAutonomousP2PropClutterScatterMaterials(
                Material pottery,
                Material sack,
                Material firewood,
                Material crate,
                Material barrel,
                Material weed,
                Material weedCard,
                Material puddle,
                Material fallenLeaf,
                Material laundryWarm,
                Material laundryCool,
                Material rope)
            {
                Pottery = pottery;
                Sack = sack;
                Firewood = firewood;
                Crate = crate;
                Barrel = barrel;
                Weed = weed;
                WeedCard = weedCard;
                Puddle = puddle;
                FallenLeaf = fallenLeaf;
                LaundryWarm = laundryWarm;
                LaundryCool = laundryCool;
                Rope = rope;
            }
        }

        private readonly struct Hd2dAutonomousP2PropClutterScatterDiffMetrics
        {
            private readonly int sampleCount;
            private readonly int changedPixels;
            private readonly float changedPercent;
            private readonly float meanRgbDelta;

            public Hd2dAutonomousP2PropClutterScatterDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
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
