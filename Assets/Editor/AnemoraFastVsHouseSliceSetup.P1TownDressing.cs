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
        private const string Hd2dAutonomousP1TownDressingRootName = "Current_AriaStreet_P1_TownDressing";
        private const string Hd2dAutonomousP1TownDressingProfilePath = "Assets/Settings/FastVS_HD2D_P1_TownDressingProfile.asset";
        private const string Hd2dAutonomousP1TownDressingWoodMaterialId = "hd2d_p1_town_dressing_warm_wood";
        private const string Hd2dAutonomousP1TownDressingPaintMaterialId = "hd2d_p1_town_dressing_sign_paint";
        private const string Hd2dAutonomousP1TownDressingClothMaterialId = "hd2d_p1_town_dressing_banner_cloth";
        private const string Hd2dAutonomousP1TownDressingRopeMaterialId = "hd2d_p1_town_dressing_rope";
        private const string Hd2dAutonomousP1TownDressingGoodsMaterialId = "hd2d_p1_town_dressing_goods_crate";
        private const string Hd2dAutonomousP1TownDressingBarrelMaterialId = "hd2d_p1_town_dressing_barrel";
        private const string Hd2dAutonomousP1TownDressingLanternFrameMaterialId = "hd2d_p1_town_dressing_lantern_frame";
        private const string Hd2dAutonomousP1TownDressingLanternGlowMaterialId = "hd2d_p1_town_dressing_lantern_glow";
        private const string Hd2dAutonomousP1TownDressingBannerTextureId = "hd2d_p1_town_dressing_banner_cloth";
        private const string Hd2dAutonomousP1TownDressingLanternGlowTextureId = "hd2d_p1_town_dressing_lantern_glow";

        private static readonly string[] Hd2dAutonomousP1TownDressingTypeIds =
        {
            "shop_sign",
            "tavern_bracket",
            "cloth_banner",
            "striped_awning",
            "paper_lantern",
            "rope_line",
            "goods_crate",
            "barrel_stack",
            "market_shelf",
            "flag_pennant"
        };

        public static void CaptureHd2dAutonomousP1Item44TownDressingBatch()
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
            var dressingRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1TownDressingRootName);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || dressingRoot == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P1-44 town dressing capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1TownDressing();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("town_dressing_lanterns");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_dressing_off_baseline.png",
                "02_town_dressing_eight_types_noon.png",
                "03_dusk_lanterns_bloom.png",
                "04_banner_sprite_card_close.png",
                "05_town_dressing_overview.png"
            };
            var shotRows = new List<string>();
            try
            {
                guide.SetMovementFrozen(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.AriaStreet);
                controller.ForcePlayerCurrentLocalForReview(Chapter1AriaStreetMapCenter + new Vector3(-3.5f, 0.04f, 4.7f));
                guide.ApplyActiveTimeIsolationForReview();
                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();
                Physics.SyncTransforms();

                SetHd2dAutonomousP1TownDressingVisible(dressingRoot, false);
                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(-1.0f, 0.02f, 5.4f),
                    new Vector3(0.65f, 10.2f, -12.8f),
                    new Vector3(0.04f, 1.28f, 0.34f),
                    39f,
                    outputDirectory,
                    screenshotFiles[0],
                    "town dressing off baseline",
                    shotRows);

                SetHd2dAutonomousP1TownDressingVisible(dressingRoot, true);
                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(-1.0f, 0.02f, 5.4f),
                    new Vector3(0.65f, 10.2f, -12.8f),
                    new Vector3(0.04f, 1.28f, 0.34f),
                    39f,
                    outputDirectory,
                    screenshotFiles[1],
                    "town dressing on: signs banners lanterns crates barrels ropes",
                    shotRows);

                sunDriver.ApplyPreset(SunPreset.Evening, true);
                realtimeRig.ApplyNowForReview();
                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(-0.6f, 0.02f, 4.9f),
                    new Vector3(-0.10f, 6.9f, -8.6f),
                    new Vector3(0.05f, 1.08f, 0.36f),
                    34f,
                    outputDirectory,
                    screenshotFiles[2],
                    "dusk lanterns with warm emissive bloom and point-light pools",
                    shotRows);

                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(-10.5f, 0.02f, 5.2f),
                    new Vector3(-0.55f, 4.9f, -7.0f),
                    new Vector3(0.04f, 1.02f, 0.34f),
                    32f,
                    outputDirectory,
                    screenshotFiles[3],
                    "cloth banner and flag sprite-card close-up",
                    shotRows);

                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 5.6f),
                    new Vector3(0.50f, 21.8f, -19.8f),
                    new Vector3(0.00f, 1.20f, 0.28f),
                    49f,
                    outputDirectory,
                    screenshotFiles[4],
                    "overview: dressing density and verticality",
                    shotRows);

                WriteHd2dAutonomousP1TownDressingReviewReport(outputDirectory, screenshotFiles, shotRows);
            }
            finally
            {
                SetHd2dAutonomousP1TownDressingVisible(dressingRoot, true);
            }

            Debug.Log($"Fast VS autonomous P1-44 town dressing review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1TownDressing(Transform root, string prefix, bool past, Materials materials)
        {
            if (root == null || past || !string.Equals(prefix, "Current", StringComparison.Ordinal))
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP1TownDressingProfileAsset();
            var dressingMaterials = EnsureHd2dAutonomousP1TownDressingMaterials(profile);
            var dressingRoot = new GameObject(Hd2dAutonomousP1TownDressingRootName);
            dressingRoot.transform.SetParent(root, false);
            dressingRoot.transform.localPosition = Vector3.zero;
            var renderers = new List<Renderer>();
            var c = Chapter1AriaStreetMapCenter;

            AddHd2dAutonomousP1TownDressingSignCluster(dressingRoot.transform, profile, dressingMaterials, c + new Vector3(-16.8f, 0f, 4.7f), Quaternion.Euler(0f, -4f, 0f), renderers);
            AddHd2dAutonomousP1TownDressingTavernBracket(dressingRoot.transform, profile, dressingMaterials, c + new Vector3(-11.6f, 0f, 4.9f), Quaternion.Euler(0f, 2f, 0f), renderers);
            AddHd2dAutonomousP1TownDressingBannerSet(dressingRoot.transform, profile, dressingMaterials, c + new Vector3(-8.4f, 0f, 4.8f), Quaternion.Euler(0f, 0f, 0f), renderers);
            AddHd2dAutonomousP1TownDressingAwning(dressingRoot.transform, profile, dressingMaterials, c + new Vector3(-4.8f, 0f, 4.5f), Quaternion.Euler(0f, -2f, 0f), renderers);
            AddHd2dAutonomousP1TownDressingLanternCluster(dressingRoot.transform, profile, dressingMaterials, c, renderers);
            AddHd2dAutonomousP1TownDressingRopeLine(dressingRoot.transform, profile, dressingMaterials, c + new Vector3(2.5f, 0f, 4.6f), Quaternion.Euler(0f, 0f, 0f), renderers);
            AddHd2dAutonomousP1TownDressingGoodsCluster(dressingRoot.transform, profile, dressingMaterials, c + new Vector3(6.8f, 0f, 4.2f), Quaternion.Euler(0f, 4f, 0f), renderers);
            AddHd2dAutonomousP1TownDressingBarrels(dressingRoot.transform, profile, dressingMaterials, c + new Vector3(10.3f, 0f, 4.4f), Quaternion.Euler(0f, -6f, 0f), renderers);
            AddHd2dAutonomousP1TownDressingMarketShelf(dressingRoot.transform, profile, dressingMaterials, c + new Vector3(13.8f, 0f, 4.8f), Quaternion.Euler(0f, 3f, 0f), renderers);

            ApplyHd2dAutonomousP0StaticFlags(dressingRoot);
            _ = materials;
        }

        private static void ValidateHd2dAutonomousP1TownDressing()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dTownDressingProfile>(Hd2dAutonomousP1TownDressingProfilePath);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1TownDressingRootName);
            if (profile == null || root == null)
            {
                throw new InvalidOperationException("House slice validation failed: P1-44 needs a town dressing profile and current Aria Street dressing root.");
            }

            var markers = root.GetComponentsInChildren<FastVsHd2dTownDressingMarker>(true);
            var distinctKinds = markers.Select(marker => marker.DressingKindForReview).Where(kind => !string.IsNullOrEmpty(kind)).Distinct(StringComparer.Ordinal).Count();
            var lanterns = markers.Count(marker => marker.EmissiveLanternForReview);
            var spriteCards = markers.Count(marker => marker.SpriteCardWindReadyForReview);
            var pointLights = root.GetComponentsInChildren<Light>(true).Count(light => light.type == LightType.Point && light.enabled && light.intensity > 0.2f && light.range >= 1.5f);
            if (markers.Length < 30 ||
                distinctKinds < 8 ||
                lanterns < 5 ||
                spriteCards < 5 ||
                pointLights < 5 ||
                profile.DistinctDressingTypeCountForReview < 8 ||
                profile.LanternEmissionIntensityForReview < 3f ||
                !profile.ConservativeReviewModeForReview ||
                !profile.RequiresTomArtApprovalForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P1-44 needs >=8 dressing types, >=5 lanterns/lights, sprite-card cloth, and a conservative Tom-facing profile.");
            }

            foreach (var materialId in new[]
            {
                Hd2dAutonomousP1TownDressingWoodMaterialId,
                Hd2dAutonomousP1TownDressingPaintMaterialId,
                Hd2dAutonomousP1TownDressingClothMaterialId,
                Hd2dAutonomousP1TownDressingRopeMaterialId,
                Hd2dAutonomousP1TownDressingGoodsMaterialId,
                Hd2dAutonomousP1TownDressingBarrelMaterialId,
                Hd2dAutonomousP1TownDressingLanternFrameMaterialId,
                Hd2dAutonomousP1TownDressingLanternGlowMaterialId
            })
            {
                if (AssetDatabase.LoadAssetAtPath<Material>(GetHd2dAutonomousP1TownDressingMaterialPath(materialId)) == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: P1-44 material missing for {materialId}.");
                }
            }

            if (AssetDatabase.LoadAssetAtPath<Texture2D>(GetHd2dAutonomousP1TownDressingTexturePath(Hd2dAutonomousP1TownDressingBannerTextureId)) == null ||
                AssetDatabase.LoadAssetAtPath<Texture2D>(GetHd2dAutonomousP1TownDressingTexturePath(Hd2dAutonomousP1TownDressingLanternGlowTextureId)) == null)
            {
                throw new InvalidOperationException("House slice validation failed: P1-44 generated banner/lantern textures are missing.");
            }

            var editorSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.P1TownDressing.cs");
            var runtimeProfilePath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dTownDressingProfile.cs");
            var runtimeMarkerPath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dTownDressingMarker.cs");
            var mainSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.cs");
            foreach (var token in new[]
            {
                "CaptureHd2dAutonomousP1Item44TownDressingBatch",
                "town_dressing_lanterns",
                "paper_lantern",
                "ValidateHd2dAutonomousP1TownDressing"
            })
            {
                ValidateSourceToken(File.ReadAllText(editorSourcePath), token, editorSourcePath);
            }

            ValidateSourceToken(File.ReadAllText(runtimeProfilePath), "LanternEmissionIntensityForReview", runtimeProfilePath);
            ValidateSourceToken(File.ReadAllText(runtimeMarkerPath), "SpriteCardWindReadyForReview", runtimeMarkerPath);
            ValidateSourceToken(File.ReadAllText(mainSourcePath), "CreateHd2dAutonomousP1TownDressing", mainSourcePath);
            ValidateSourceToken(File.ReadAllText(mainSourcePath), "ValidateHd2dAutonomousP1TownDressing", mainSourcePath);
        }

        private static FastVsHd2dTownDressingProfile EnsureHd2dAutonomousP1TownDressingProfileAsset()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dTownDressingProfile>(Hd2dAutonomousP1TownDressingProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dTownDressingProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP1TownDressingProfilePath);
            }

            profile.ConfigureForReview(
                Hd2dAutonomousP1TownDressingTypeIds.Length,
                38,
                5,
                6,
                4.6f,
                3.2f,
                1.55f,
                0.18f,
                0.32f,
                new Vector2(0.15f, 4.80f),
                true,
                true,
                false,
                "Procedural CC0-safe review baseline; replace with approved Quaternius/Kenney signs, lanterns, awnings, crates, barrels, and cloth sprites.",
                Hd2dAutonomousP1TownDressingTypeIds);
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Dictionary<string, Material> EnsureHd2dAutonomousP1TownDressingMaterials(FastVsHd2dTownDressingProfile profile)
        {
            return new Dictionary<string, Material>(StringComparer.Ordinal)
            {
                [Hd2dAutonomousP1TownDressingWoodMaterialId] = EnsureHd2dAutonomousP1TownDressingSurfaceMaterial(Hd2dAutonomousP1TownDressingWoodMaterialId, new Color32(91, 57, 38, 255), new Color32(148, 93, 58, 255), new Color32(46, 34, 29, 255), new Vector2(3.0f, 1.8f)),
                [Hd2dAutonomousP1TownDressingPaintMaterialId] = EnsureHd2dAutonomousP1TownDressingSurfaceMaterial(Hd2dAutonomousP1TownDressingPaintMaterialId, new Color32(184, 120, 66, 255), new Color32(220, 169, 96, 255), new Color32(91, 52, 39, 255), new Vector2(2.0f, 1.5f)),
                [Hd2dAutonomousP1TownDressingRopeMaterialId] = EnsureHd2dAutonomousP1TownDressingSurfaceMaterial(Hd2dAutonomousP1TownDressingRopeMaterialId, new Color32(116, 89, 54, 255), new Color32(182, 148, 89, 255), new Color32(61, 47, 35, 255), new Vector2(5.0f, 1.0f)),
                [Hd2dAutonomousP1TownDressingGoodsMaterialId] = EnsureHd2dAutonomousP1TownDressingSurfaceMaterial(Hd2dAutonomousP1TownDressingGoodsMaterialId, new Color32(93, 67, 45, 255), new Color32(151, 111, 69, 255), new Color32(43, 36, 30, 255), new Vector2(2.5f, 2.5f)),
                [Hd2dAutonomousP1TownDressingBarrelMaterialId] = EnsureHd2dAutonomousP1TownDressingSurfaceMaterial(Hd2dAutonomousP1TownDressingBarrelMaterialId, new Color32(83, 49, 35, 255), new Color32(138, 83, 49, 255), new Color32(32, 29, 29, 255), new Vector2(2.8f, 2.0f)),
                [Hd2dAutonomousP1TownDressingLanternFrameMaterialId] = EnsureHd2dAutonomousP1TownDressingSurfaceMaterial(Hd2dAutonomousP1TownDressingLanternFrameMaterialId, new Color32(48, 35, 29, 255), new Color32(108, 77, 48, 255), new Color32(22, 22, 23, 255), new Vector2(2.0f, 2.0f)),
                [Hd2dAutonomousP1TownDressingClothMaterialId] = EnsureHd2dAutonomousP1TownDressingClothMaterial(profile),
                [Hd2dAutonomousP1TownDressingLanternGlowMaterialId] = EnsureHd2dAutonomousP1TownDressingLanternGlowMaterial(profile)
            };
        }

        private static Material EnsureHd2dAutonomousP1TownDressingSurfaceMaterial(string materialId, Color32 a, Color32 b, Color32 c, Vector2 textureScale)
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            var material = FlatMaterial(materialId, Color.white, false, FastVsHd2dMaterialRole.SurfaceLit);
            AssignMaterialTexture(material, EnsureGeneratedRepeatTexture(materialId, 64, 64, (x, y) => SampleHd2dAutonomousP1TownDressingSurfacePixel(materialId, x, y, a, b, c)), textureScale);
            ApplySurfaceRampProfile(material, materialId);
            material.enableInstancing = true;
            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureHd2dAutonomousP1TownDressingClothMaterial(FastVsHd2dTownDressingProfile profile)
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            var material = CreateSpriteCardMaterial(Hd2dAutonomousP1TownDressingClothMaterialId, new Color(0.94f, 0.72f, 0.42f, 1f), 2450);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP1TownDressingBannerTexture(), Vector2.one);
            if (material.HasProperty("_WindStrength"))
            {
                material.SetFloat("_WindStrength", profile.BannerWindSwayStrengthForReview);
            }

            if (material.HasProperty("_WindPhase"))
            {
                material.SetFloat("_WindPhase", profile.BannerWindPhaseRangeForReview.x);
            }

            material.enableInstancing = true;
            ApplyMaterialRole(material, Hd2dAutonomousP1TownDressingClothMaterialId, FastVsHd2dMaterialRole.SpriteCard);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureHd2dAutonomousP1TownDressingLanternGlowMaterial(FastVsHd2dTownDressingProfile profile)
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            var material = FlatMaterial(Hd2dAutonomousP1TownDressingLanternGlowMaterialId, new Color(1.0f, 0.76f, 0.36f, 0.82f), true, FastVsHd2dMaterialRole.OverlayGlow);
            ConfigureTransparentMaterial(material, 3014, URPUnlitShaderName, SpriteCardRampShaderName);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP1TownDressingLanternGlowTexture(), Vector2.one);
            if (material.HasProperty("_EmissionColor"))
            {
                material.SetColor("_EmissionColor", new Color(1.0f, 0.62f, 0.28f, 1f) * profile.LanternEmissionIntensityForReview);
            }

            if (material.HasProperty("_EmissionIntensity"))
            {
                material.SetFloat("_EmissionIntensity", profile.LanternEmissionIntensityForReview);
            }

            material.EnableKeyword("_EMISSION");
            material.enableInstancing = true;
            ApplyMaterialRole(material, Hd2dAutonomousP1TownDressingLanternGlowMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Color SampleHd2dAutonomousP1TownDressingSurfacePixel(string materialId, int x, int y, Color32 a, Color32 b, Color32 c)
        {
            if (materialId.Contains("rope", StringComparison.Ordinal))
            {
                return ((x + y) % 10 < 4) ? a : b;
            }

            if (materialId.Contains("barrel", StringComparison.Ordinal))
            {
                return (x % 13 < 2 || y % 19 < 2) ? c : ((x * 5 + y * 3) % 29 < 4 ? b : a);
            }

            if (materialId.Contains("sign_paint", StringComparison.Ordinal))
            {
                return (x % 21 < 3 || y % 17 < 3) ? c : ((x + y * 2) % 37 < 8 ? b : a);
            }

            return (x % 16 < 2 || y % 23 < 2) ? c : ((x * 7 + y * 11) % 31 < 5 ? b : a);
        }

        private static Texture2D EnsureHd2dAutonomousP1TownDressingBannerTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP1TownDressingBannerTextureId,
                96,
                96,
                FilterMode.Point,
                (x, y) =>
                {
                    var u = x / 95f;
                    var v = y / 95f;
                    var scallop = 0.07f * Mathf.Sin(u * Mathf.PI * 6f);
                    if (v < 0.08f + scallop || u < 0.045f || u > 0.955f)
                    {
                        return new Color(0f, 0f, 0f, 0f);
                    }

                    var stripe = ((x / 12) % 2) == 0;
                    var shade = ((x * 3 + y * 5) % 41) < 6 ? 0.86f : 1f;
                    var baseColor = stripe ? new Color(0.77f, 0.20f, 0.17f, 1f) : new Color(0.93f, 0.68f, 0.34f, 1f);
                    if (y % 18 < 2)
                    {
                        baseColor *= 0.74f;
                        baseColor.a = 1f;
                    }

                    return baseColor * shade;
                });
        }

        private static Texture2D EnsureHd2dAutonomousP1TownDressingLanternGlowTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP1TownDressingLanternGlowTextureId,
                64,
                64,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = (x / 63f - 0.5f) * 2f;
                    var v = (y / 63f - 0.5f) * 2f;
                    var radius = Mathf.Sqrt(u * u + v * v);
                    var core = Mathf.Clamp01(1f - radius * 1.55f);
                    var halo = Mathf.Clamp01(1f - radius * 0.82f);
                    var alpha = Mathf.Clamp01(core * 0.92f + halo * halo * 0.38f);
                    return new Color(1f, 0.66f, 0.30f, alpha);
                });
        }

        private static void AddHd2dAutonomousP1TownDressingSignCluster(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 position, Quaternion rotation, ICollection<Renderer> renderers)
        {
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_ShopSign_Post", "shop_sign", Hd2dAutonomousP1TownDressingWoodMaterialId, position + new Vector3(-0.42f, 0.74f, 0f), new Vector3(0.08f, 1.42f, 0.08f), rotation, materials[Hd2dAutonomousP1TownDressingWoodMaterialId], false, false, false, renderers, profile);
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_ShopSign_Board", "shop_sign", Hd2dAutonomousP1TownDressingPaintMaterialId, position + new Vector3(0.05f, 1.42f, -0.10f), new Vector3(0.86f, 0.42f, 0.08f), rotation, materials[Hd2dAutonomousP1TownDressingPaintMaterialId], false, false, false, renderers, profile);
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_ShopSign_Lintel", "shop_sign", Hd2dAutonomousP1TownDressingWoodMaterialId, position + new Vector3(0.04f, 1.66f, -0.12f), new Vector3(0.96f, 0.07f, 0.10f), rotation, materials[Hd2dAutonomousP1TownDressingWoodMaterialId], false, false, false, renderers, profile);
        }

        private static void AddHd2dAutonomousP1TownDressingTavernBracket(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 position, Quaternion rotation, ICollection<Renderer> renderers)
        {
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_TavernBracket_Post", "tavern_bracket", Hd2dAutonomousP1TownDressingWoodMaterialId, position + new Vector3(-0.40f, 1.45f, 0f), new Vector3(0.09f, 1.60f, 0.09f), rotation, materials[Hd2dAutonomousP1TownDressingWoodMaterialId], false, false, false, renderers, profile);
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_TavernBracket_Arm", "tavern_bracket", Hd2dAutonomousP1TownDressingWoodMaterialId, position + new Vector3(0.10f, 2.13f, -0.08f), new Vector3(0.94f, 0.08f, 0.08f), rotation, materials[Hd2dAutonomousP1TownDressingWoodMaterialId], false, false, false, renderers, profile);
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_TavernBracket_HangingBoard", "tavern_bracket", Hd2dAutonomousP1TownDressingPaintMaterialId, position + new Vector3(0.36f, 1.70f, -0.12f), new Vector3(0.52f, 0.46f, 0.08f), rotation, materials[Hd2dAutonomousP1TownDressingPaintMaterialId], false, false, false, renderers, profile);
        }

        private static void AddHd2dAutonomousP1TownDressingBannerSet(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 position, Quaternion rotation, ICollection<Renderer> renderers)
        {
            CreateHd2dAutonomousP1TownDressingSpriteCard(root, "TownDressing_ClothBanner_Wide", "cloth_banner", position + new Vector3(-0.24f, 2.06f, -0.16f), new Vector3(1.00f, 0.72f, 1f), rotation, materials[Hd2dAutonomousP1TownDressingClothMaterialId], renderers, profile);
            CreateHd2dAutonomousP1TownDressingSpriteCard(root, "TownDressing_FlagPennant_Left", "flag_pennant", position + new Vector3(-0.92f, 1.68f, -0.20f), new Vector3(0.42f, 0.36f, 1f), Quaternion.Euler(0f, 10f, 0f), materials[Hd2dAutonomousP1TownDressingClothMaterialId], renderers, profile);
            CreateHd2dAutonomousP1TownDressingSpriteCard(root, "TownDressing_FlagPennant_Right", "flag_pennant", position + new Vector3(0.54f, 1.72f, -0.21f), new Vector3(0.44f, 0.38f, 1f), Quaternion.Euler(0f, -8f, 0f), materials[Hd2dAutonomousP1TownDressingClothMaterialId], renderers, profile);
        }

        private static void AddHd2dAutonomousP1TownDressingAwning(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 position, Quaternion rotation, ICollection<Renderer> renderers)
        {
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_StripedAwning_Roof", "striped_awning", Hd2dAutonomousP1TownDressingPaintMaterialId, position + new Vector3(0f, 1.54f, -0.22f), new Vector3(1.36f, 0.12f, 0.46f), rotation * Quaternion.Euler(-10f, 0f, 0f), materials[Hd2dAutonomousP1TownDressingPaintMaterialId], false, false, false, renderers, profile);
            CreateHd2dAutonomousP1TownDressingSpriteCard(root, "TownDressing_StripedAwning_Valance", "striped_awning", position + new Vector3(0f, 1.34f, -0.44f), new Vector3(1.38f, 0.28f, 1f), rotation, materials[Hd2dAutonomousP1TownDressingClothMaterialId], renderers, profile);
        }

        private static void AddHd2dAutonomousP1TownDressingLanternCluster(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 c, ICollection<Renderer> renderers)
        {
            var positions = new[]
            {
                c + new Vector3(-12.2f, 1.74f, 4.45f),
                c + new Vector3(-5.4f, 1.56f, 4.22f),
                c + new Vector3(-0.8f, 1.70f, 4.28f),
                c + new Vector3(5.8f, 1.42f, 4.15f),
                c + new Vector3(12.8f, 1.64f, 4.42f)
            };
            for (var index = 0; index < positions.Length; index++)
            {
                var yaw = Quaternion.Euler(0f, -4f + index * 2.5f, 0f);
                CreateHd2dAutonomousP1TownDressingCube(root, $"TownDressing_LanternFrame_{index:00}", "paper_lantern", Hd2dAutonomousP1TownDressingLanternFrameMaterialId, positions[index], new Vector3(0.28f, 0.34f, 0.20f), yaw, materials[Hd2dAutonomousP1TownDressingLanternFrameMaterialId], true, true, false, renderers, profile);
                var glow = CreateHd2dAutonomousP1TownDressingSpriteCard(root, $"TownDressing_LanternGlow_{index:00}", "paper_lantern", positions[index] + new Vector3(0f, 0f, -0.035f), new Vector3(0.72f, 0.72f, 1f), yaw, materials[Hd2dAutonomousP1TownDressingLanternGlowMaterialId], renderers, profile);
                ConfigureHd2dAutonomousP1TownDressingMarker(glow, profile, "paper_lantern", Hd2dAutonomousP1TownDressingLanternGlowMaterialId, true, true, false, true);
            }
        }

        private static void AddHd2dAutonomousP1TownDressingRopeLine(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 position, Quaternion rotation, ICollection<Renderer> renderers)
        {
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_RopeLine_Main", "rope_line", Hd2dAutonomousP1TownDressingRopeMaterialId, position + new Vector3(0f, 1.54f, -0.16f), new Vector3(2.50f, 0.045f, 0.045f), rotation, materials[Hd2dAutonomousP1TownDressingRopeMaterialId], false, false, false, renderers, profile);
            for (var index = 0; index < 3; index++)
            {
                CreateHd2dAutonomousP1TownDressingSpriteCard(root, $"TownDressing_RopePennant_{index:00}", "flag_pennant", position + new Vector3(-0.72f + index * 0.72f, 1.28f, -0.18f), new Vector3(0.34f, 0.30f, 1f), rotation, materials[Hd2dAutonomousP1TownDressingClothMaterialId], renderers, profile);
            }
        }

        private static void AddHd2dAutonomousP1TownDressingGoodsCluster(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 position, Quaternion rotation, ICollection<Renderer> renderers)
        {
            for (var index = 0; index < 5; index++)
            {
                var offset = new Vector3((index % 3) * 0.42f - 0.42f, 0.18f + (index / 3) * 0.26f, -0.06f + (index / 3) * 0.28f);
                CreateHd2dAutonomousP1TownDressingCube(root, $"TownDressing_GoodsCrate_{index:00}", "goods_crate", Hd2dAutonomousP1TownDressingGoodsMaterialId, position + offset, new Vector3(0.36f, 0.30f, 0.34f), rotation, materials[Hd2dAutonomousP1TownDressingGoodsMaterialId], false, false, false, renderers, profile);
            }
        }

        private static void AddHd2dAutonomousP1TownDressingBarrels(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 position, Quaternion rotation, ICollection<Renderer> renderers)
        {
            for (var index = 0; index < 4; index++)
            {
                var offset = new Vector3((index % 2) * 0.42f - 0.20f, 0.22f + (index / 2) * 0.34f, (index / 2) * 0.20f);
                CreateHd2dAutonomousP1TownDressingPrimitive(root, PrimitiveType.Cylinder, $"TownDressing_BarrelStack_{index:00}", "barrel_stack", Hd2dAutonomousP1TownDressingBarrelMaterialId, position + offset, new Vector3(0.30f, 0.36f, 0.30f), rotation * Quaternion.Euler(0f, 0f, 90f), materials[Hd2dAutonomousP1TownDressingBarrelMaterialId], false, false, false, renderers, profile);
            }
        }

        private static void AddHd2dAutonomousP1TownDressingMarketShelf(Transform root, FastVsHd2dTownDressingProfile profile, IReadOnlyDictionary<string, Material> materials, Vector3 position, Quaternion rotation, ICollection<Renderer> renderers)
        {
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_MarketShelf_Frame", "market_shelf", Hd2dAutonomousP1TownDressingWoodMaterialId, position + new Vector3(0f, 0.55f, 0f), new Vector3(1.16f, 0.10f, 0.34f), rotation, materials[Hd2dAutonomousP1TownDressingWoodMaterialId], false, false, false, renderers, profile);
            CreateHd2dAutonomousP1TownDressingCube(root, "TownDressing_MarketShelf_Top", "market_shelf", Hd2dAutonomousP1TownDressingWoodMaterialId, position + new Vector3(0f, 0.88f, -0.02f), new Vector3(1.20f, 0.10f, 0.34f), rotation, materials[Hd2dAutonomousP1TownDressingWoodMaterialId], false, false, false, renderers, profile);
            for (var index = 0; index < 4; index++)
            {
                CreateHd2dAutonomousP1TownDressingCube(root, $"TownDressing_MarketShelf_Goods_{index:00}", "goods_crate", Hd2dAutonomousP1TownDressingGoodsMaterialId, position + new Vector3(-0.42f + index * 0.28f, 1.02f, -0.07f), new Vector3(0.18f, 0.15f, 0.18f), rotation, materials[Hd2dAutonomousP1TownDressingGoodsMaterialId], false, false, false, renderers, profile);
            }
        }

        private static GameObject CreateHd2dAutonomousP1TownDressingCube(Transform root, string objectName, string kind, string materialId, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material, bool emissiveLantern, bool pointLightReady, bool spriteCardWindReady, ICollection<Renderer> renderers, FastVsHd2dTownDressingProfile profile)
        {
            return CreateHd2dAutonomousP1TownDressingPrimitive(root, PrimitiveType.Cube, objectName, kind, materialId, localPosition, localScale, localRotation, material, emissiveLantern, pointLightReady, spriteCardWindReady, renderers, profile);
        }

        private static GameObject CreateHd2dAutonomousP1TownDressingPrimitive(Transform root, PrimitiveType primitiveType, string objectName, string kind, string materialId, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material, bool emissiveLantern, bool pointLightReady, bool spriteCardWindReady, ICollection<Renderer> renderers, FastVsHd2dTownDressingProfile profile)
        {
            var target = GameObject.CreatePrimitive(primitiveType);
            target.name = objectName;
            target.transform.SetParent(root, false);
            target.transform.localPosition = localPosition;
            target.transform.localRotation = localRotation;
            target.transform.localScale = localScale;
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
                renderers?.Add(renderer);
            }

            ConfigureHd2dAutonomousP1TownDressingMarker(target, profile, kind, materialId, emissiveLantern, pointLightReady, spriteCardWindReady, true);
            if (pointLightReady)
            {
                AddHd2dAutonomousP1TownDressingPointLight(target.transform, profile, objectName + "_PointLight");
            }

            return target;
        }

        private static GameObject CreateHd2dAutonomousP1TownDressingSpriteCard(Transform root, string objectName, string kind, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material, ICollection<Renderer> renderers, FastVsHd2dTownDressingProfile profile)
        {
            var card = CreateQuad(objectName, root, localPosition, localScale, material);
            card.transform.localRotation = localRotation;
            var renderer = card.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                renderers?.Add(renderer);
            }

            ConfigureHd2dAutonomousP1TownDressingMarker(card, profile, kind, material != null ? material.name.Replace("FastVS_House_", string.Empty) : string.Empty, false, false, true, true);
            return card;
        }

        private static void AddHd2dAutonomousP1TownDressingPointLight(Transform parent, FastVsHd2dTownDressingProfile profile, string objectName)
        {
            var lightObject = new GameObject(objectName);
            lightObject.transform.SetParent(parent, false);
            lightObject.transform.localPosition = new Vector3(0f, 0.02f, -0.12f);
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1.0f, 0.66f, 0.34f, 1f);
            light.intensity = profile.LanternLightIntensityForReview;
            light.range = profile.LanternLightRangeForReview;
            light.shadows = LightShadows.None;
        }

        private static void ConfigureHd2dAutonomousP1TownDressingMarker(GameObject target, FastVsHd2dTownDressingProfile profile, string kind, string materialId, bool emissiveLantern, bool pointLightReady, bool spriteCardWindReady, bool inAcceptedReviewFrame)
        {
            if (target == null)
            {
                return;
            }

            var marker = target.GetComponent<FastVsHd2dTownDressingMarker>();
            if (marker == null)
            {
                marker = target.AddComponent<FastVsHd2dTownDressingMarker>();
            }

            marker.ConfigureForReview(profile, kind, materialId, emissiveLantern, pointLightReady, spriteCardWindReady, inAcceptedReviewFrame);
            EditorUtility.SetDirty(marker);
        }

        private static void SetHd2dAutonomousP1TownDressingVisible(GameObject root, bool visible)
        {
            if (root == null)
            {
                return;
            }

            foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
            {
                renderer.enabled = visible;
                EditorUtility.SetDirty(renderer);
            }

            foreach (var light in root.GetComponentsInChildren<Light>(true))
            {
                light.enabled = visible;
                EditorUtility.SetDirty(light);
            }
        }

        private static void WriteHd2dAutonomousP1TownDressingReviewReport(string outputDirectory, IReadOnlyList<string> screenshotFiles, IReadOnlyList<string> shotRows)
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dTownDressingProfile>(Hd2dAutonomousP1TownDressingProfilePath);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1TownDressingRootName);
            var markers = root != null ? root.GetComponentsInChildren<FastVsHd2dTownDressingMarker>(true) : Array.Empty<FastVsHd2dTownDressingMarker>();
            var kindCounts = markers
                .Where(marker => !string.IsNullOrEmpty(marker.DressingKindForReview))
                .GroupBy(marker => marker.DressingKindForReview)
                .OrderBy(group => group.Key, StringComparer.Ordinal)
                .Select(group => $"| {group.Key} | {group.Count()} |")
                .ToArray();
            var pointLightCount = root != null ? root.GetComponentsInChildren<Light>(true).Count(light => light.type == LightType.Point) : 0;

            var lines = new List<string>
            {
                "# P1-44 Town Dressing, Banners, and Lanterns Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative lived-in-town dressing baseline for Aria Street, using procedural CC0-safe stand-ins for signs, brackets, banners, awnings, lanterns, ropes, crates, barrels, and shelves.",
                "- Recommendation: keep the profile/marker/material path and replace the generated stand-ins with approved Quaternius/Kenney dressing meshes and hand-authored cloth sprites before final art sign-off.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP1TownDressingProfilePath}` |",
                $"| Distinct types / markers | {markers.Select(marker => marker.DressingKindForReview).Distinct(StringComparer.Ordinal).Count()} / {markers.Length} |",
                $"| Lanterns / point lights | {markers.Count(marker => marker.EmissiveLanternForReview)} / {pointLightCount} |",
                $"| Sprite-card cloth markers | {markers.Count(marker => marker.SpriteCardWindReadyForReview)} |",
                $"| Lantern emission / light intensity / range | {profile?.LanternEmissionIntensityForReview ?? 0f:0.###} / {profile?.LanternLightIntensityForReview ?? 0f:0.###} / {profile?.LanternLightRangeForReview ?? 0f:0.###} |",
                $"| Banner wind strength / phase range | {profile?.BannerWindSwayStrengthForReview ?? 0f:0.###} / {FormatHd2dAutonomousP1TownDressingVector2(profile != null ? profile.BannerWindPhaseRangeForReview : Vector2.zero)} |",
                $"| Conservative review mode | {FormatBool(profile != null && profile.ConservativeReviewModeForReview)} |",
                $"| Requires TOM art approval | {FormatBool(profile != null && profile.RequiresTomArtApprovalForReview)} |",
                $"| Source kit note | {profile?.SourceKitNoteForReview ?? "missing"} |",
                string.Empty,
                "| Dressing kind | Count |",
                "|---|---:|"
            };
            lines.AddRange(kindCounts);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Label | Anchor | Offset | FOV |",
                "|---|---|---|---|---:|"
            });
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Baseline with P1-44 renderers and lantern lights disabled |",
                $"| `{screenshotFiles[1]}` | Same town-street framing with >=8 dressing types enabled |",
                $"| `{screenshotFiles[2]}` | Dusk check for warm lantern emission, bloom, and point-light pools |",
                $"| `{screenshotFiles[3]}` | Cloth banner/flag sprite-card close-up |",
                $"| `{screenshotFiles[4]}` | Wider density/verticality check |"
            });

            File.WriteAllText(Path.Combine(outputDirectory, "town_dressing_lanterns_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static string GetHd2dAutonomousP1TownDressingMaterialPath(string materialId)
        {
            return MaterialDirectory + "/FastVS_House_" + materialId + ".mat";
        }

        private static string GetHd2dAutonomousP1TownDressingTexturePath(string textureId)
        {
            return TextureDirectory + "/FastVS_House_" + textureId + ".asset";
        }

        private static string FormatHd2dAutonomousP1TownDressingVector2(Vector2 value)
        {
            return $"{value.x:0.###},{value.y:0.###}";
        }
    }
}
