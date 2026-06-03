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
        private const string Hd2dAutonomousP1ModularBuildingKitProfilePath = "Assets/Settings/FastVS_HD2D_P1_ModularBuildingKitProfile.asset";
        private const string Hd2dAutonomousP1ModularBuildingKitPrefabRoot = "Assets/Prefabs/Zone1/Hd2dAutonomousP1ModularBuildingKit";
        private const string Hd2dAutonomousP1ModularBuildingKitModulePrefabRoot = Hd2dAutonomousP1ModularBuildingKitPrefabRoot + "/Modules";
        private const string Hd2dAutonomousP1ModularBuildingKitVariantPrefabRoot = Hd2dAutonomousP1ModularBuildingKitPrefabRoot + "/Variants";
        private const string Hd2dAutonomousP1ModularBuildingKitStreetRootName = "Current_AriaStreet_P1_ModularBuildingKitStreet";
        private const string Hd2dAutonomousP1ModularBuildingKitObjectPrefix = "FastVS_HD2D_P1_ModularBuildingKit_";
        private const string Hd2dAutonomousP1ModularBuildingWallMaterialId = "hd2d_p1_modular_building_wall";
        private const string Hd2dAutonomousP1ModularBuildingRoofMaterialId = "hd2d_p1_modular_building_roof";
        private const string Hd2dAutonomousP1ModularBuildingTrimMaterialId = "hd2d_p1_modular_building_trim";
        private const string Hd2dAutonomousP1ModularBuildingWindowMaterialId = "hd2d_p1_modular_building_lit_window";
        private const string Hd2dAutonomousP1ModularBuildingFloorMaterialId = "hd2d_p1_modular_building_floor";

        public static void CaptureHd2dAutonomousP1Item42ModularBuildingKitStreetBatch()
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
                throw new InvalidOperationException("Fast VS autonomous P1-42 modular building kit capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1ModularBuildingKitStreet();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("modular_building_kit_street");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_before_legacy_aria_street.png",
                "02_after_five_modular_variants.png",
                "03_grid_snap_facade_close.png",
                "04_prefab_variant_roof_window_combo.png",
                "05_profile_prefab_overview.png"
            };
            var shotRows = new List<string>();

            var previousMask = camera.cullingMask;
            try
            {
                guide.SetMovementFrozen(true);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.AriaStreet);
                controller.ForcePlayerCurrentLocalForReview(Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 0.15f));
                guide.ApplyActiveTimeIsolationForReview();
                realtimeRig.ApplyNowForReview();
                Physics.SyncTransforms();

                SetHd2dAutonomousP1ModularBuildingKitRenderersVisible(streetRoot, false);
                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 4.8f),
                    new Vector3(0f, 18.65f, -25.70f),
                    new Vector3(0.70f, 0.58f, 4.75f),
                    43f,
                    outputDirectory,
                    screenshotFiles[0],
                    "legacy Aria street baseline",
                    shotRows);

                SetHd2dAutonomousP1ModularBuildingKitRenderersVisible(streetRoot, true);
                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 4.8f),
                    new Vector3(0f, 18.65f, -25.70f),
                    new Vector3(0.70f, 0.58f, 4.75f),
                    43f,
                    outputDirectory,
                    screenshotFiles[1],
                    "five modular variants enabled",
                    shotRows);

                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(-13.4f, 0.02f, 7.0f),
                    new Vector3(2.25f, 4.60f, -7.20f),
                    new Vector3(0.20f, 0.92f, 0.30f),
                    35f,
                    outputDirectory,
                    screenshotFiles[2],
                    "grid-snap facade close",
                    shotRows);

                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(3.2f, 0.02f, 6.7f),
                    new Vector3(-2.05f, 5.35f, -8.45f),
                    new Vector3(-0.10f, 1.25f, 0.55f),
                    33f,
                    outputDirectory,
                    screenshotFiles[3],
                    "prefab roof and lit-window combo",
                    shotRows);

                CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 6.2f),
                    new Vector3(0.75f, 21.20f, -18.60f),
                    new Vector3(0.20f, 0.72f, 2.75f),
                    48f,
                    outputDirectory,
                    screenshotFiles[4],
                    "profile and prefab overview",
                    shotRows);

                ValidateHd2dAutonomousP1ModularBuildingKitReviewPairDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1], "legacy-vs-modular-street");
                WriteHd2dAutonomousP1ModularBuildingKitReviewReport(outputDirectory, screenshotFiles, shotRows);
            }
            finally
            {
                camera.cullingMask = previousMask;
                SetHd2dAutonomousP1ModularBuildingKitRenderersVisible(streetRoot, true);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                guide.SetMovementFrozen(false);
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P1-42 modular building kit review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1ModularBuildingKitStreet(Transform root, string prefix, bool past, Materials materials)
        {
            if (root == null || past || !string.Equals(prefix, "Current", StringComparison.Ordinal))
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP1ModularBuildingKitProfileAsset();
            EnsureHd2dAutonomousP1ModularBuildingKitPrefabs(profile);

            var streetRoot = new GameObject(Hd2dAutonomousP1ModularBuildingKitStreetRootName);
            streetRoot.transform.SetParent(root, false);
            streetRoot.transform.localPosition = Vector3.zero;

            var recipes = GetHd2dAutonomousP1ModularBuildingRecipes();
            var positions = GetHd2dAutonomousP1ModularBuildingStreetPositions(profile.GridUnitForReview);
            for (var index = 0; index < recipes.Length; index++)
            {
                var recipe = recipes[index];
                var prefabPath = GetHd2dAutonomousP1ModularBuildingVariantPrefabPath(recipe.BuildingIdForReview);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    throw new InvalidOperationException($"Fast VS P1-42 modular building prefab is missing: {prefabPath}");
                }

                var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (instance == null)
                {
                    instance = UnityEngine.Object.Instantiate(prefab);
                }

                instance.name = Hd2dAutonomousP1ModularBuildingKitObjectPrefix + recipe.BuildingIdForReview;
                instance.transform.SetParent(streetRoot.transform, false);
                instance.transform.localPosition = positions[index];
                instance.transform.localRotation = Quaternion.identity;
                instance.transform.localScale = Vector3.one;

                var renderers = instance.GetComponentsInChildren<Renderer>(true);
                var reviewInstance = instance.GetComponent<FastVsHd2dModularBuildingInstance>();
                if (reviewInstance == null)
                {
                    reviewInstance = instance.AddComponent<FastVsHd2dModularBuildingInstance>();
                }

                reviewInstance.ConfigureForReview(profile, recipe, CountHd2dAutonomousP1ModularBuildingModules(instance.transform), renderers, index, true, true, true);
                ApplyHd2dAutonomousP0StaticFlags(instance);
            }

            ApplyHd2dAutonomousP0StaticFlags(streetRoot);
            _ = materials;
        }

        private static void ValidateHd2dAutonomousP1ModularBuildingKitStreet()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dModularBuildingKitProfile>(Hd2dAutonomousP1ModularBuildingKitProfilePath);
            var streetRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1ModularBuildingKitStreetRootName);
            if (profile == null ||
                streetRoot == null ||
                profile.GridUnitForReview <= 0f ||
                profile.ModuleCountForReview < 7 ||
                profile.BuildingRecipeCountForReview < 5 ||
                !profile.HasInteriorIncludedWallForReview ||
                !profile.PrefabVariantsEnabledForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P1-42 needs a modular kit profile, street root, interior-included wall module, and at least 7 modules / 5 recipes.");
            }

            if (!AssetDatabase.IsValidFolder(Hd2dAutonomousP1ModularBuildingKitPrefabRoot) ||
                !AssetDatabase.IsValidFolder(Hd2dAutonomousP1ModularBuildingKitModulePrefabRoot) ||
                !AssetDatabase.IsValidFolder(Hd2dAutonomousP1ModularBuildingKitVariantPrefabRoot))
            {
                throw new InvalidOperationException("House slice validation failed: P1-42 prefab folders are missing.");
            }

            for (var index = 0; index < profile.ModuleCountForReview; index++)
            {
                var module = profile.GetModuleForReview(index);
                if (module == null || AssetDatabase.LoadAssetAtPath<GameObject>(module.PrefabPathForReview) == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: P1-42 module prefab is missing for module index {index}.");
                }
            }

            var expectedMaterials = new[]
            {
                Hd2dAutonomousP1ModularBuildingWallMaterialId,
                Hd2dAutonomousP1ModularBuildingRoofMaterialId,
                Hd2dAutonomousP1ModularBuildingTrimMaterialId,
                Hd2dAutonomousP1ModularBuildingWindowMaterialId,
                Hd2dAutonomousP1ModularBuildingFloorMaterialId
            };
            for (var index = 0; index < expectedMaterials.Length; index++)
            {
                var materialId = expectedMaterials[index];
                var material = AssetDatabase.LoadAssetAtPath<Material>(GetHd2dAutonomousP1ModularBuildingMaterialPath(materialId));
                if (material == null || !material.enableInstancing)
                {
                    throw new InvalidOperationException($"House slice validation failed: P1-42 shared material `{materialId}` must exist and have GPU instancing enabled.");
                }
            }

            var instances = UnityEngine.Object.FindObjectsByType<FastVsHd2dModularBuildingInstance>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(instance => instance != null && instance.transform.IsChildOf(streetRoot.transform))
                .OrderBy(instance => instance.transform.localPosition.x)
                .ToArray();
            if (instances.Length < 5)
            {
                throw new InvalidOperationException("House slice validation failed: P1-42 needs at least five scene building instances.");
            }

            var signatures = new HashSet<string>(StringComparer.Ordinal);
            var previousSignature = string.Empty;
            for (var index = 0; index < instances.Length; index++)
            {
                var instance = instances[index];
                if (!instance.IsReadyForReview ||
                    instance.ModuleCountForReview < 16 ||
                    instance.RendererCountForReview < 16 ||
                    !instance.SnappedToGridForReview ||
                    !instance.PrefabBackedForReview ||
                    PrefabUtility.GetPrefabInstanceStatus(instance.gameObject) == PrefabInstanceStatus.NotAPrefab ||
                    !IsHd2dAutonomousP1ModularBuildingGridSnapped(instance.transform.localPosition, profile.GridUnitForReview))
                {
                    throw new InvalidOperationException($"House slice validation failed: P1-42 building `{instance.BuildingIdForReview}` is not ready, prefab-backed, renderer-rich, and grid-snapped.");
                }

                var flags = GameObjectUtility.GetStaticEditorFlags(instance.gameObject);
                if ((flags & StaticEditorFlags.BatchingStatic) == 0 ||
                    (flags & StaticEditorFlags.ContributeGI) == 0)
                {
                    throw new InvalidOperationException($"House slice validation failed: P1-42 building `{instance.BuildingIdForReview}` must be marked for static batching and GI.");
                }

                if (string.Equals(previousSignature, instance.FacadeSignatureForReview, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException("House slice validation failed: P1-42 adjacent modular buildings must not share identical signatures.");
                }

                previousSignature = instance.FacadeSignatureForReview;
                signatures.Add(instance.FacadeSignatureForReview);
            }

            if (signatures.Count < 5)
            {
                throw new InvalidOperationException("House slice validation failed: P1-42 needs five distinct modular building configurations.");
            }

            var runtimeProfilePath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dModularBuildingKitProfile.cs");
            var runtimeProfileSource = File.ReadAllText(runtimeProfilePath);
            foreach (var token in new[] { "FastVsHd2dModularBuildingKitProfile", "ModuleDefinition", "BuildingRecipe", "HasInteriorIncludedWallForReview" })
            {
                ValidateSourceToken(runtimeProfileSource, token, runtimeProfilePath);
            }

            var runtimeInstancePath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dModularBuildingInstance.cs");
            var runtimeInstanceSource = File.ReadAllText(runtimeInstancePath);
            foreach (var token in new[] { "FastVsHd2dModularBuildingInstance", "SnappedToGridForReview", "PrefabBackedForReview", "StaticBatchingMarkedForReview" })
            {
                ValidateSourceToken(runtimeInstanceSource, token, runtimeInstancePath);
            }

            var editorSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.P1ModularBuildingKit.cs");
            var editorSource = File.ReadAllText(editorSourcePath);
            foreach (var token in new[]
            {
                "CaptureHd2dAutonomousP1Item42ModularBuildingKitStreetBatch",
                "modular_building_kit_street",
                "modular_building_kit_street_review.md",
                "CreateHd2dAutonomousP1ModularBuildingKitStreet",
                "PrefabUtility.SaveAsPrefabAsset"
            })
            {
                ValidateSourceToken(editorSource, token, editorSourcePath);
            }
        }

        private static FastVsHd2dModularBuildingKitProfile EnsureHd2dAutonomousP1ModularBuildingKitProfileAsset()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dModularBuildingKitProfile>(Hd2dAutonomousP1ModularBuildingKitProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dModularBuildingKitProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP1ModularBuildingKitProfilePath);
            }

            profile.ConfigureForReview(
                1f,
                "Quaternius Medieval Village MegaKit",
                Hd2dAutonomousP0Cc0MedievalVillageRoot,
                true,
                HasHd2dAutonomousP1QuaterniusBuildingFbxSource(),
                GetHd2dAutonomousP1ModularBuildingModules(),
                GetHd2dAutonomousP1ModularBuildingRecipes());
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static void EnsureHd2dAutonomousP1ModularBuildingKitPrefabs(FastVsHd2dModularBuildingKitProfile profile)
        {
            if (profile == null)
            {
                throw new InvalidOperationException("Fast VS P1-42 modular building kit prefab generation needs a profile.");
            }

            EnsureFolder(Hd2dAutonomousP1ModularBuildingKitPrefabRoot);
            EnsureFolder(Hd2dAutonomousP1ModularBuildingKitModulePrefabRoot);
            EnsureFolder(Hd2dAutonomousP1ModularBuildingKitVariantPrefabRoot);
            var materials = EnsureHd2dAutonomousP1ModularBuildingMaterials();

            for (var index = 0; index < profile.ModuleCountForReview; index++)
            {
                var module = profile.GetModuleForReview(index);
                if (module != null)
                {
                    EnsureHd2dAutonomousP1ModularBuildingModulePrefab(module, materials);
                }
            }

            for (var index = 0; index < profile.BuildingRecipeCountForReview; index++)
            {
                var recipe = profile.GetRecipeForReview(index);
                if (recipe != null)
                {
                    EnsureHd2dAutonomousP1ModularBuildingVariantPrefab(profile, recipe, materials, index);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static Dictionary<string, Material> EnsureHd2dAutonomousP1ModularBuildingMaterials()
        {
            var materials = new Dictionary<string, Material>(StringComparer.Ordinal)
            {
                [Hd2dAutonomousP1ModularBuildingWallMaterialId] = EnsureHd2dAutonomousP1ModularBuildingMaterial(
                    Hd2dAutonomousP1ModularBuildingWallMaterialId,
                    new Color32(148, 132, 103, 255),
                    new Color32(196, 176, 134, 255),
                    new Color32(91, 80, 64, 255),
                    PixelPattern.Bricks,
                    false,
                    FastVsHd2dMaterialRole.SurfaceLit,
                    new Vector2(3.0f, 2.0f)),
                [Hd2dAutonomousP1ModularBuildingRoofMaterialId] = EnsureHd2dAutonomousP1ModularBuildingMaterial(
                    Hd2dAutonomousP1ModularBuildingRoofMaterialId,
                    new Color32(92, 62, 55, 255),
                    new Color32(151, 87, 70, 255),
                    new Color32(55, 47, 48, 255),
                    PixelPattern.Roof,
                    false,
                    FastVsHd2dMaterialRole.SurfaceLit,
                    new Vector2(3.5f, 2.0f)),
                [Hd2dAutonomousP1ModularBuildingTrimMaterialId] = EnsureHd2dAutonomousP1ModularBuildingMaterial(
                    Hd2dAutonomousP1ModularBuildingTrimMaterialId,
                    new Color32(86, 60, 42, 255),
                    new Color32(132, 92, 60, 255),
                    new Color32(47, 38, 32, 255),
                    PixelPattern.Planks,
                    false,
                    FastVsHd2dMaterialRole.SurfaceLit,
                    new Vector2(2.0f, 2.0f)),
                [Hd2dAutonomousP1ModularBuildingWindowMaterialId] = EnsureHd2dAutonomousP1ModularBuildingMaterial(
                    Hd2dAutonomousP1ModularBuildingWindowMaterialId,
                    new Color32(246, 181, 80, 255),
                    new Color32(255, 231, 144, 255),
                    new Color32(154, 89, 49, 255),
                    PixelPattern.Checker,
                    true,
                    FastVsHd2dMaterialRole.PortalWindow,
                    new Vector2(1.0f, 1.0f)),
                [Hd2dAutonomousP1ModularBuildingFloorMaterialId] = EnsureHd2dAutonomousP1ModularBuildingMaterial(
                    Hd2dAutonomousP1ModularBuildingFloorMaterialId,
                    new Color32(117, 89, 61, 255),
                    new Color32(168, 124, 77, 255),
                    new Color32(67, 52, 41, 255),
                    PixelPattern.Planks,
                    false,
                    FastVsHd2dMaterialRole.SurfaceLit,
                    new Vector2(2.5f, 2.0f))
            };

            return materials;
        }

        private static Material EnsureHd2dAutonomousP1ModularBuildingMaterial(
            string materialId,
            Color32 a,
            Color32 b,
            Color32 c,
            PixelPattern pattern,
            bool unlit,
            FastVsHd2dMaterialRole role,
            Vector2 textureScale)
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            var material = FlatMaterial(materialId, Color.white, unlit, role);
            AssignMaterialTexture(material, EnsurePixelTexture(materialId, a, b, c, pattern), textureScale);
            material.enableInstancing = true;
            if (!unlit)
            {
                ApplySurfaceRampProfile(material, materialId);
            }

            ApplyMaterialRole(material, materialId, role);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void EnsureHd2dAutonomousP1ModularBuildingModulePrefab(
            FastVsHd2dModularBuildingKitProfile.ModuleDefinition module,
            IReadOnlyDictionary<string, Material> materials)
        {
            var prefabPath = module.PrefabPathForReview;
            var root = new GameObject("FastVS_HD2D_P1_Module_" + module.ModuleIdForReview);
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = module.ModuleIdForReview + "_Mesh";
            cube.transform.SetParent(root.transform, false);
            cube.transform.localPosition = Vector3.zero;
            cube.transform.localRotation = Quaternion.identity;
            cube.transform.localScale = Vector3.one;
            if (cube.TryGetComponent<Collider>(out var collider))
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = cube.GetComponent<Renderer>();
            renderer.sharedMaterial = ResolveHd2dAutonomousP1ModularBuildingModuleMaterial(module.ModuleIdForReview, materials);
            renderer.shadowCastingMode = ShadowCastingMode.On;
            renderer.receiveShadows = true;
            ApplyHd2dAutonomousP0StaticFlags(root);
            PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static void EnsureHd2dAutonomousP1ModularBuildingVariantPrefab(
            FastVsHd2dModularBuildingKitProfile profile,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            IReadOnlyDictionary<string, Material> materials,
            int variantIndex)
        {
            var root = new GameObject("FastVS_HD2D_P1_ModularBuilding_" + recipe.BuildingIdForReview);
            var renderers = new List<Renderer>();
            var moduleCount = 0;
            BuildHd2dAutonomousP1ModularBuildingModules(root.transform, profile, recipe, materials, renderers, ref moduleCount);
            AddHd2dAutonomousP1BuildingSurfaceDetailing(root.transform, recipe, materials, renderers, ref moduleCount);
            var instance = root.AddComponent<FastVsHd2dModularBuildingInstance>();
            instance.ConfigureForReview(profile, recipe, moduleCount, renderers.ToArray(), variantIndex, true, true, true);
            ApplyHd2dAutonomousP0StaticFlags(root);
            PrefabUtility.SaveAsPrefabAsset(root, GetHd2dAutonomousP1ModularBuildingVariantPrefabPath(recipe.BuildingIdForReview));
            UnityEngine.Object.DestroyImmediate(root);
        }

        private static void BuildHd2dAutonomousP1ModularBuildingModules(
            Transform root,
            FastVsHd2dModularBuildingKitProfile profile,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            IReadOnlyDictionary<string, Material> materials,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            var grid = profile.GridUnitForReview;
            var width = recipe.FootprintForReview.x * grid;
            var depth = recipe.FootprintForReview.y * grid;
            const float storyHeight = 1.14f;
            for (var floor = 0; floor < recipe.FloorsForReview; floor++)
            {
                var y = floor * storyHeight;
                AddHd2dAutonomousP1ModularBuildingModule(root, "floor_slab", new Vector3(0f, y + 0.04f, 0f), new Vector3(width + 0.16f, 0.08f, depth + 0.16f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingFloorMaterialId], renderers, ref moduleCount);
                AddHd2dAutonomousP1ModularBuildingModule(root, "wall_interior", new Vector3(0f, y + 0.58f, -depth * 0.5f), new Vector3(width, 0.96f, 0.16f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingWallMaterialId], renderers, ref moduleCount);
                AddHd2dAutonomousP1ModularBuildingModule(root, "wall_interior", new Vector3(0f, y + 0.58f, depth * 0.5f), new Vector3(width, 0.96f, 0.16f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingWallMaterialId], renderers, ref moduleCount);
                AddHd2dAutonomousP1ModularBuildingModule(root, "wall_interior", new Vector3(-width * 0.5f, y + 0.58f, 0f), new Vector3(0.16f, 0.96f, depth), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingWallMaterialId], renderers, ref moduleCount);
                AddHd2dAutonomousP1ModularBuildingModule(root, "wall_interior", new Vector3(width * 0.5f, y + 0.58f, 0f), new Vector3(0.16f, 0.96f, depth), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingWallMaterialId], renderers, ref moduleCount);

                if (recipe.InteriorWallsForReview)
                {
                    AddHd2dAutonomousP1ModularBuildingModule(root, "wall_interior", new Vector3(0f, y + 0.58f, 0f), new Vector3(0.12f, 0.82f, Mathf.Max(0.8f, depth - 0.48f)), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingWallMaterialId], renderers, ref moduleCount);
                }

                AddHd2dAutonomousP1ModularBuildingCornerPosts(root, width, depth, y, materials, renderers, ref moduleCount);
                AddHd2dAutonomousP1ModularBuildingWindows(root, recipe, width, depth, y, floor, materials, renderers, ref moduleCount);
            }

            if (recipe.DoorCountForReview > 0)
            {
                AddHd2dAutonomousP1ModularBuildingModule(root, "door_single", new Vector3(0f, 0.50f, -depth * 0.5f - 0.11f), new Vector3(0.62f, 0.88f, 0.08f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId], renderers, ref moduleCount);
                AddHd2dAutonomousP1ModularBuildingModule(root, "door_single", new Vector3(0f, 0.56f, -depth * 0.5f - 0.16f), new Vector3(0.42f, 0.54f, 0.05f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingWindowMaterialId], renderers, ref moduleCount);
            }

            AddHd2dAutonomousP1ModularBuildingRoof(root, recipe, width, depth, recipe.FloorsForReview * storyHeight + 0.12f, materials, renderers, ref moduleCount);
            if (recipe.BuildingIdForReview.Contains("stair", StringComparison.Ordinal) || recipe.FloorsForReview >= 3)
            {
                AddHd2dAutonomousP1ModularBuildingStairs(root, width, depth, materials, renderers, ref moduleCount);
            }
        }

        private static void AddHd2dAutonomousP1ModularBuildingCornerPosts(
            Transform root,
            float width,
            float depth,
            float y,
            IReadOnlyDictionary<string, Material> materials,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            var x = width * 0.5f + 0.03f;
            var z = depth * 0.5f + 0.03f;
            AddHd2dAutonomousP1ModularBuildingModule(root, "corner_post", new Vector3(-x, y + 0.58f, -z), new Vector3(0.18f, 1.08f, 0.18f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId], renderers, ref moduleCount);
            AddHd2dAutonomousP1ModularBuildingModule(root, "corner_post", new Vector3(x, y + 0.58f, -z), new Vector3(0.18f, 1.08f, 0.18f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId], renderers, ref moduleCount);
            AddHd2dAutonomousP1ModularBuildingModule(root, "corner_post", new Vector3(-x, y + 0.58f, z), new Vector3(0.18f, 1.08f, 0.18f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId], renderers, ref moduleCount);
            AddHd2dAutonomousP1ModularBuildingModule(root, "corner_post", new Vector3(x, y + 0.58f, z), new Vector3(0.18f, 1.08f, 0.18f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId], renderers, ref moduleCount);
        }

        private static void AddHd2dAutonomousP1ModularBuildingWindows(
            Transform root,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            float width,
            float depth,
            float y,
            int floor,
            IReadOnlyDictionary<string, Material> materials,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            var windowsThisFloor = Mathf.Max(1, Mathf.CeilToInt(recipe.WindowCountForReview / Mathf.Max(1f, recipe.FloorsForReview)));
            var spacing = width / (windowsThisFloor + 1);
            for (var index = 0; index < windowsThisFloor; index++)
            {
                var x = -width * 0.5f + spacing * (index + 1);
                if (floor == 0 && Mathf.Abs(x) < 0.38f && recipe.DoorCountForReview > 0)
                {
                    x += 0.58f;
                }

                AddHd2dAutonomousP1ModularBuildingModule(root, "window_small", new Vector3(x, y + 0.66f, -depth * 0.5f - 0.13f), new Vector3(0.42f, 0.34f, 0.06f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingWindowMaterialId], renderers, ref moduleCount);
                AddHd2dAutonomousP1ModularBuildingModule(root, "corner_post", new Vector3(x, y + 0.66f, -depth * 0.5f - 0.17f), new Vector3(0.52f, 0.42f, 0.04f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId], renderers, ref moduleCount);
            }
        }

        private static void AddHd2dAutonomousP1ModularBuildingRoof(
            Transform root,
            FastVsHd2dModularBuildingKitProfile.BuildingRecipe recipe,
            float width,
            float depth,
            float topY,
            IReadOnlyDictionary<string, Material> materials,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            if (recipe.RoofStyleForReview.Contains("flat", StringComparison.Ordinal))
            {
                AddHd2dAutonomousP1ModularBuildingModule(root, "roof_gable", new Vector3(0f, topY, 0f), new Vector3(width + 0.38f, 0.18f, depth + 0.38f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingRoofMaterialId], renderers, ref moduleCount);
                AddHd2dAutonomousP1ModularBuildingModule(root, "corner_post", new Vector3(0f, topY + 0.20f, -depth * 0.5f), new Vector3(width + 0.28f, 0.20f, 0.13f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId], renderers, ref moduleCount);
                return;
            }

            var pitch = recipe.RoofStyleForReview.Contains("steep", StringComparison.Ordinal) ? 16f : 10f;
            if (recipe.RoofStyleForReview.Contains("shed", StringComparison.Ordinal))
            {
                AddHd2dAutonomousP1ModularBuildingModule(root, "roof_gable", new Vector3(0f, topY + 0.08f, 0f), new Vector3(width + 0.40f, 0.20f, depth + 0.48f), Quaternion.Euler(0f, 0f, -pitch), materials[Hd2dAutonomousP1ModularBuildingRoofMaterialId], renderers, ref moduleCount);
                return;
            }

            AddHd2dAutonomousP1ModularBuildingModule(root, "roof_gable", new Vector3(-width * 0.25f, topY + 0.10f, 0f), new Vector3(width * 0.58f + 0.32f, 0.18f, depth + 0.48f), Quaternion.Euler(0f, 0f, pitch), materials[Hd2dAutonomousP1ModularBuildingRoofMaterialId], renderers, ref moduleCount);
            AddHd2dAutonomousP1ModularBuildingModule(root, "roof_gable", new Vector3(width * 0.25f, topY + 0.10f, 0f), new Vector3(width * 0.58f + 0.32f, 0.18f, depth + 0.48f), Quaternion.Euler(0f, 0f, -pitch), materials[Hd2dAutonomousP1ModularBuildingRoofMaterialId], renderers, ref moduleCount);
            AddHd2dAutonomousP1ModularBuildingModule(root, "corner_post", new Vector3(0f, topY + 0.26f, 0f), new Vector3(0.16f, 0.16f, depth + 0.60f), Quaternion.identity, materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId], renderers, ref moduleCount);
            if (recipe.RoofStyleForReview.Contains("cross", StringComparison.Ordinal))
            {
                AddHd2dAutonomousP1ModularBuildingModule(root, "roof_gable", new Vector3(0f, topY + 0.38f, 0f), new Vector3(width * 0.42f, 0.16f, depth * 0.72f), Quaternion.Euler(0f, 90f, 12f), materials[Hd2dAutonomousP1ModularBuildingRoofMaterialId], renderers, ref moduleCount);
            }
        }

        private static void AddHd2dAutonomousP1ModularBuildingStairs(
            Transform root,
            float width,
            float depth,
            IReadOnlyDictionary<string, Material> materials,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            for (var index = 0; index < 4; index++)
            {
                AddHd2dAutonomousP1ModularBuildingModule(
                    root,
                    "stair_step",
                    new Vector3(-width * 0.5f - 0.24f + index * 0.18f, 0.10f + index * 0.08f, -depth * 0.5f - 0.48f - index * 0.16f),
                    new Vector3(0.58f, 0.10f, 0.22f),
                    Quaternion.identity,
                    materials[Hd2dAutonomousP1ModularBuildingFloorMaterialId],
                    renderers,
                    ref moduleCount);
            }
        }

        private static GameObject AddHd2dAutonomousP1ModularBuildingModule(
            Transform root,
            string moduleId,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation,
            Material material,
            ICollection<Renderer> renderers,
            ref int moduleCount)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(GetHd2dAutonomousP1ModularBuildingModulePrefabPath(moduleId));
            var module = prefab != null ? PrefabUtility.InstantiatePrefab(prefab) as GameObject : null;
            if (module == null)
            {
                module = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (module.TryGetComponent<Collider>(out var collider))
                {
                    UnityEngine.Object.DestroyImmediate(collider);
                }
            }

            module.name = moduleId + "_" + moduleCount.ToString("00");
            module.transform.SetParent(root, false);
            module.transform.localPosition = localPosition;
            module.transform.localRotation = localRotation;
            module.transform.localScale = localScale;
            ConfigureHd2dAutonomousP1ModularBuildingRenderers(module, material, renderers);
            moduleCount++;
            return module;
        }

        private static void ConfigureHd2dAutonomousP1ModularBuildingRenderers(GameObject target, Material material, ICollection<Renderer> renderers)
        {
            var targetRenderers = target.GetComponentsInChildren<Renderer>(true);
            for (var index = 0; index < targetRenderers.Length; index++)
            {
                var renderer = targetRenderers[index];
                var slots = renderer.sharedMaterials;
                if (slots == null || slots.Length == 0)
                {
                    slots = new Material[1];
                }

                for (var slot = 0; slot < slots.Length; slot++)
                {
                    slots[slot] = material;
                }

                renderer.sharedMaterials = slots;
                renderer.shadowCastingMode = ShadowCastingMode.On;
                renderer.receiveShadows = true;
                renderers?.Add(renderer);
            }
        }

        private static Material ResolveHd2dAutonomousP1ModularBuildingModuleMaterial(string moduleId, IReadOnlyDictionary<string, Material> materials)
        {
            if (moduleId.Contains("roof", StringComparison.Ordinal))
            {
                return materials[Hd2dAutonomousP1ModularBuildingRoofMaterialId];
            }

            if (moduleId.Contains("window", StringComparison.Ordinal))
            {
                return materials[Hd2dAutonomousP1ModularBuildingWindowMaterialId];
            }

            if (moduleId.Contains("floor", StringComparison.Ordinal) || moduleId.Contains("stair", StringComparison.Ordinal))
            {
                return materials[Hd2dAutonomousP1ModularBuildingFloorMaterialId];
            }

            if (moduleId.Contains("door", StringComparison.Ordinal) || moduleId.Contains("corner", StringComparison.Ordinal))
            {
                return materials[Hd2dAutonomousP1ModularBuildingTrimMaterialId];
            }

            return materials[Hd2dAutonomousP1ModularBuildingWallMaterialId];
        }

        private static FastVsHd2dModularBuildingKitProfile.ModuleDefinition[] GetHd2dAutonomousP1ModularBuildingModules()
        {
            return new[]
            {
                new FastVsHd2dModularBuildingKitProfile.ModuleDefinition("wall_interior", "wall", new Vector3(1f, 1f, 0.16f), true, GetHd2dAutonomousP1ModularBuildingModulePrefabPath("wall_interior")),
                new FastVsHd2dModularBuildingKitProfile.ModuleDefinition("corner_post", "corner", new Vector3(0.18f, 1f, 0.18f), false, GetHd2dAutonomousP1ModularBuildingModulePrefabPath("corner_post")),
                new FastVsHd2dModularBuildingKitProfile.ModuleDefinition("floor_slab", "floor", new Vector3(1f, 0.08f, 1f), true, GetHd2dAutonomousP1ModularBuildingModulePrefabPath("floor_slab")),
                new FastVsHd2dModularBuildingKitProfile.ModuleDefinition("roof_gable", "roof", new Vector3(1f, 0.18f, 1f), false, GetHd2dAutonomousP1ModularBuildingModulePrefabPath("roof_gable")),
                new FastVsHd2dModularBuildingKitProfile.ModuleDefinition("door_single", "door", new Vector3(0.62f, 0.88f, 0.08f), true, GetHd2dAutonomousP1ModularBuildingModulePrefabPath("door_single")),
                new FastVsHd2dModularBuildingKitProfile.ModuleDefinition("window_small", "window", new Vector3(0.42f, 0.34f, 0.06f), true, GetHd2dAutonomousP1ModularBuildingModulePrefabPath("window_small")),
                new FastVsHd2dModularBuildingKitProfile.ModuleDefinition("stair_step", "stair", new Vector3(0.58f, 0.10f, 0.22f), false, GetHd2dAutonomousP1ModularBuildingModulePrefabPath("stair_step"))
            };
        }

        private static FastVsHd2dModularBuildingKitProfile.BuildingRecipe[] GetHd2dAutonomousP1ModularBuildingRecipes()
        {
            return new[]
            {
                new FastVsHd2dModularBuildingKitProfile.BuildingRecipe("narrow_shop", new Vector2Int(3, 2), 2, "gable", 1, 4, "plaster_a", true),
                new FastVsHd2dModularBuildingKitProfile.BuildingRecipe("corner_tower", new Vector2Int(2, 2), 3, "steep_gable", 1, 5, "stone_trim", true),
                new FastVsHd2dModularBuildingKitProfile.BuildingRecipe("wide_inn", new Vector2Int(4, 2), 2, "cross_gable", 1, 6, "warm_plaster", true),
                new FastVsHd2dModularBuildingKitProfile.BuildingRecipe("workshop_shed", new Vector2Int(3, 3), 1, "shed", 1, 3, "timber_low", true),
                new FastVsHd2dModularBuildingKitProfile.BuildingRecipe("stair_loft", new Vector2Int(2, 3), 2, "flat_parapet", 1, 4, "loft_stair", true)
            };
        }

        private static Vector3[] GetHd2dAutonomousP1ModularBuildingStreetPositions(float gridUnit)
        {
            var c = Chapter1AriaStreetMapCenter;
            return new[]
            {
                SnapHd2dAutonomousP1ModularBuildingPosition(c + new Vector3(-17.0f, 0f, 6.8f), gridUnit),
                SnapHd2dAutonomousP1ModularBuildingPosition(c + new Vector3(-9.0f, 0f, 6.1f), gridUnit),
                SnapHd2dAutonomousP1ModularBuildingPosition(c + new Vector3(-1.0f, 0f, 6.9f), gridUnit),
                SnapHd2dAutonomousP1ModularBuildingPosition(c + new Vector3(7.0f, 0f, 6.0f), gridUnit),
                SnapHd2dAutonomousP1ModularBuildingPosition(c + new Vector3(15.0f, 0f, 6.8f), gridUnit)
            };
        }

        private static Vector3 SnapHd2dAutonomousP1ModularBuildingPosition(Vector3 position, float gridUnit)
        {
            var grid = Mathf.Max(0.25f, gridUnit);
            return new Vector3(Mathf.Round(position.x / grid) * grid, position.y, Mathf.Round(position.z / grid) * grid);
        }

        private static bool IsHd2dAutonomousP1ModularBuildingGridSnapped(Vector3 position, float gridUnit)
        {
            var snapped = SnapHd2dAutonomousP1ModularBuildingPosition(position, gridUnit);
            return Mathf.Abs(snapped.x - position.x) <= 0.01f && Mathf.Abs(snapped.z - position.z) <= 0.01f;
        }

        private static string GetHd2dAutonomousP1ModularBuildingModulePrefabPath(string moduleId)
        {
            return Hd2dAutonomousP1ModularBuildingKitModulePrefabRoot + "/FastVS_HD2D_P1_Module_" + moduleId + ".prefab";
        }

        private static string GetHd2dAutonomousP1ModularBuildingVariantPrefabPath(string buildingId)
        {
            return Hd2dAutonomousP1ModularBuildingKitVariantPrefabRoot + "/FastVS_HD2D_P1_ModularBuilding_" + buildingId + ".prefab";
        }

        private static string GetHd2dAutonomousP1ModularBuildingMaterialPath(string materialId)
        {
            return MaterialDirectory + "/FastVS_House_" + materialId + ".mat";
        }

        private static bool HasHd2dAutonomousP1QuaterniusBuildingFbxSource()
        {
            if (!Directory.Exists(Hd2dAutonomousP0Cc0MedievalVillageRoot))
            {
                return false;
            }

            return Directory.GetFiles(Hd2dAutonomousP0Cc0MedievalVillageRoot, "*.fbx", SearchOption.AllDirectories)
                .Any(path =>
                {
                    var fileName = Path.GetFileNameWithoutExtension(path);
                    return fileName.IndexOf("house", StringComparison.OrdinalIgnoreCase) >= 0 ||
                           fileName.IndexOf("building", StringComparison.OrdinalIgnoreCase) >= 0 ||
                           fileName.IndexOf("wall", StringComparison.OrdinalIgnoreCase) >= 0 ||
                           fileName.IndexOf("roof", StringComparison.OrdinalIgnoreCase) >= 0;
                });
        }

        private static int CountHd2dAutonomousP1ModularBuildingModules(Transform root)
        {
            if (root == null)
            {
                return 0;
            }

            var count = 0;
            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
            {
                if (transform != root && transform.name.IndexOf("_", StringComparison.Ordinal) >= 0)
                {
                    count++;
                }
            }

            return count;
        }

        private static void SetHd2dAutonomousP1ModularBuildingKitRenderersVisible(GameObject root, bool visible)
        {
            if (root == null)
            {
                return;
            }

            var renderers = root.GetComponentsInChildren<Renderer>(true);
            for (var index = 0; index < renderers.Length; index++)
            {
                renderers[index].enabled = visible;
                EditorUtility.SetDirty(renderers[index]);
            }
        }

        private static void CaptureHd2dAutonomousP1ModularBuildingKitReviewShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
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
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 160f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {FormatVector3ForReport(anchorLocalPosition)} | {FormatVector3ForReport(cameraOffset)} | {fieldOfView:0.#} |");
        }

        private static void WriteHd2dAutonomousP1ModularBuildingKitReviewReport(string outputDirectory, IReadOnlyList<string> screenshotFiles, IReadOnlyList<string> shotRows)
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dModularBuildingKitProfile>(Hd2dAutonomousP1ModularBuildingKitProfilePath);
            var streetRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1ModularBuildingKitStreetRootName);
            var instances = UnityEngine.Object.FindObjectsByType<FastVsHd2dModularBuildingInstance>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(instance => instance != null && streetRoot != null && instance.transform.IsChildOf(streetRoot.transform))
                .OrderBy(instance => instance.transform.localPosition.x)
                .ToArray();

            var lines = new List<string>
            {
                "# P1-42 Modular Building Kit + Grid-Snapping Workflow Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative modular-building workflow baseline. The Quaternius Medieval Village source folder is tracked, but this workspace currently lacks real house/wall/roof FBX modules; the cycle therefore creates prefab-backed grid modules and five nested-prefab building variants as a data/workflow stand-in.",
                "- Recommendation: keep the profile, grid unit, module/recipe data, shared materials, prefab variants, instancing, static batching, and lit-window-ready interior wall contract. Tom should import/approve the final CC0 modular meshes and replace these placeholder cubes before art sign-off.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP1ModularBuildingKitProfilePath}` |",
                $"| Prefab root | `{Hd2dAutonomousP1ModularBuildingKitPrefabRoot}` |",
                $"| Source kit | `{profile?.SourceKitNameForReview ?? "missing"}` |",
                $"| Source root | `{profile?.SourceKitRootForReview ?? "missing"}` |",
                $"| Source building FBX present | {FormatBool(profile != null && profile.SourceBuildingFbxPresentForReview)} |",
                $"| Grid unit | {profile?.GridUnitForReview ?? 0f:0.###} |",
                $"| Modules / recipes | {profile?.ModuleCountForReview ?? 0} / {profile?.BuildingRecipeCountForReview ?? 0} |",
                string.Empty,
                "| Module | Kind | Grid size | Interior included | Prefab |",
                "|---|---|---|---|---|"
            };

            if (profile != null)
            {
                for (var index = 0; index < profile.ModuleCountForReview; index++)
                {
                    var module = profile.GetModuleForReview(index);
                    if (module != null)
                    {
                        lines.Add($"| `{module.ModuleIdForReview}` | {module.KindForReview} | {FormatVector3ForReport(module.GridSizeForReview)} | {FormatBool(module.InteriorIncludedForReview)} | `{module.PrefabPathForReview}` |");
                    }
                }
            }

            lines.AddRange(new[]
            {
                string.Empty,
                "| Building | Signature | Position | Footprint | Floors | Modules | Renderers | Snapped | Prefab-backed | Static/GI |",
                "|---|---|---|---|---:|---:|---:|---|---|---|"
            });
            for (var index = 0; index < instances.Length; index++)
            {
                var instance = instances[index];
                lines.Add($"| `{instance.BuildingIdForReview}` | `{instance.FacadeSignatureForReview}` | {FormatVector3ForReport(instance.transform.localPosition)} | {instance.FootprintForReview.x}x{instance.FootprintForReview.y} | {instance.FloorsForReview} | {instance.ModuleCountForReview} | {instance.RendererCountForReview} | {FormatBool(instance.SnappedToGridForReview)} | {FormatBool(instance.PrefabBackedForReview)} | {FormatBool(instance.StaticBatchingMarkedForReview)} |");
            }

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
                $"| `{screenshotFiles[0]}` | Legacy Aria street baseline with the P1-42 street root disabled |",
                $"| `{screenshotFiles[1]}` | Same camera after enabling five modular prefab-backed building variants |",
                $"| `{screenshotFiles[2]}` | Close facade pass showing grid-snapped wall, door, window, and corner modules |",
                $"| `{screenshotFiles[3]}` | Roof/window variant check for nested module composition and lit-window readiness |",
                $"| `{screenshotFiles[4]}` | Overview for spacing, no-adjacent-identical signatures, and shared material style |"
            });

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllLines(Path.Combine(outputDirectory, "modular_building_kit_street_review.md"), lines, Encoding.UTF8);
        }

        private static void ValidateHd2dAutonomousP1ModularBuildingKitReviewPairDiff(string outputDirectory, string firstFile, string secondFile, string label)
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

            throw new InvalidOperationException($"Fast VS autonomous P1-42 modular building capture failed: {label} images are byte-identical.");
        }
    }
}
