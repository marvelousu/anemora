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
        private const string Hd2dAutonomousP2CardShadowLodRootName = "Current_CentralPlaza_P2_77_CardShadowLodBudget";
        private const string Hd2dAutonomousP2CardShadowLodProfilePath = "Assets/Settings/FastVS_HD2D_P2_CardShadowLodBudgetProfile.asset";
        private const string Hd2dAutonomousP2CardShadowLodProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dCardShadowLodBudgetProfile.cs";
        private const string Hd2dAutonomousP2CardShadowLodMarkerRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dCardShadowLodBudgetMarker.cs";
        private const string Hd2dAutonomousP2CardShadowLodEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2CardShadowLodBudget.cs";
        private const string Hd2dAutonomousP2CardShadowLodAtlasTextureId = "hd2d_p2_77_foliage_shadow_lod_atlas";
        private const string Hd2dAutonomousP2CardShadowLodAtlasMaterialId = "hd2d_p2_77_foliage_shadow_lod_atlas";
        private const string Hd2dAutonomousP2CardShadowLodReceiverMaterialId = "hd2d_p2_77_shadow_receiver_ground";
        private const string Hd2dAutonomousP2CardShadowLodNearGroupName = "P2_77_NearCutoutShadowCards";
        private const string Hd2dAutonomousP2CardShadowLodGroupName = "P2_77_FarFoliageLODGroup";

        public static void CaptureHd2dAutonomousP2Item77CardShadowLodBudgetBatch()
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
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CardShadowLodRootName);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || root == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-77 card shadow/LOD capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2CardShadowLodBudget();
            var profile = EnsureHd2dAutonomousP2CardShadowLodBudgetProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("card_shadow_lod_budget_foliage_character_cards");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_near_foliage_shadow_off_baseline.png",
                "02_near_foliage_shadow_on_alpha_cutout.png",
                "03_far_foliage_lod0_many_cards.png",
                "04_far_foliage_lod1_merged_atlas.png",
                "05_shadow_lod_budget_topdown_diagnostic.png",
            };
            var shotRows = new List<string>();
            var previousRootActive = root.activeSelf;
            var previousCullingMask = camera.cullingMask;
            var lodGroup = FindHd2dAutonomousP2CardShadowLodReviewGroup(root);

            try
            {
                guide.SetMovementFrozen(true);
                root.SetActive(true);
                sunDriver.ApplyPreset(SunPreset.Evening, true);
                realtimeRig.ApplyNowForReview();

                SetHd2dAutonomousP2CardShadowLodNearShadowCasting(root, false);
                ForceHd2dAutonomousP2CardShadowLodGroup(lodGroup, -1);
                CaptureHd2dAutonomousP2CardShadowLodShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    CentralPlazaVsCenter + new Vector3(4.65f, 0.02f, -2.22f),
                    new Vector3(0.28f, 3.10f, -5.10f),
                    new Vector3(0.02f, 0.45f, 0.10f),
                    30f,
                    outputDirectory,
                    screenshotFiles[0],
                    "near foliage cards visible, shadow casting temporarily disabled",
                    "near-shadow-off",
                    shotRows);

                SetHd2dAutonomousP2CardShadowLodNearShadowCasting(root, true);
                CaptureHd2dAutonomousP2CardShadowLodShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    CentralPlazaVsCenter + new Vector3(4.65f, 0.02f, -2.22f),
                    new Vector3(0.28f, 3.10f, -5.10f),
                    new Vector3(0.02f, 0.45f, 0.10f),
                    30f,
                    outputDirectory,
                    screenshotFiles[1],
                    "near foliage cards casting alpha-clipped shadows onto the receiver",
                    "near-shadow-on",
                    shotRows);

                ForceHd2dAutonomousP2CardShadowLodGroup(lodGroup, 0);
                CaptureHd2dAutonomousP2CardShadowLodShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    CentralPlazaVsCenter + new Vector3(8.50f, 0.02f, -0.95f),
                    new Vector3(0.90f, 4.25f, -7.20f),
                    new Vector3(0.02f, 0.70f, 0.08f),
                    32f,
                    outputDirectory,
                    screenshotFiles[2],
                    "far foliage LOD0: individual source cards and multiple material bindings",
                    "lod0-source-cards",
                    shotRows);

                ForceHd2dAutonomousP2CardShadowLodGroup(lodGroup, 1);
                CaptureHd2dAutonomousP2CardShadowLodShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    CentralPlazaVsCenter + new Vector3(8.50f, 0.02f, -0.95f),
                    new Vector3(0.90f, 4.25f, -7.20f),
                    new Vector3(0.02f, 0.70f, 0.08f),
                    32f,
                    outputDirectory,
                    screenshotFiles[3],
                    "far foliage LOD1: merged atlas billboard and far shadows culled",
                    "lod1-merged-atlas",
                    shotRows);

                ForceHd2dAutonomousP2CardShadowLodGroup(lodGroup, -1);
                CaptureHd2dAutonomousP2CardShadowLodShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    CentralPlazaVsCenter + new Vector3(5.80f, 0.02f, -1.10f),
                    new Vector3(0.55f, 12.8f, -11.7f),
                    new Vector3(0.00f, 0.86f, 0.28f),
                    42f,
                    outputDirectory,
                    screenshotFiles[4],
                    "top-down diagnostic: near shadow cluster, far LOD group, atlas material, and policy root",
                    "topdown-policy",
                    shotRows);
            }
            finally
            {
                SetHd2dAutonomousP2CardShadowLodNearShadowCasting(root, true);
                ForceHd2dAutonomousP2CardShadowLodGroup(lodGroup, -1);
                root.SetActive(previousRootActive || profile.ConservativeBudgetEnabledForReview);
                camera.cullingMask = previousCullingMask;
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                guide.SetMovementFrozen(false);
                AssetDatabase.SaveAssets();
            }

            var shadowDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                4);
            var lodDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[2]),
                Path.Combine(outputDirectory, screenshotFiles[3]),
                4);
            if (shadowDiff.SampleCount <= 0 || shadowDiff.ChangedPixels <= 0)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-77 capture failed: near shadow off/on A/B produced no measurable changed pixels.");
            }

            if (lodDiff.SampleCount <= 0 || lodDiff.ChangedPixels <= 0)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-77 capture failed: far LOD0/LOD1 A/B produced no measurable changed pixels.");
            }

            WriteHd2dAutonomousP2CardShadowLodBudgetReviewReport(outputDirectory, screenshotFiles, shotRows, profile, shadowDiff, lodDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-77 card shadow/LOD budget review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2CardShadowLodBudget(Transform centralPlazaRoot, Camera camera)
        {
            if (centralPlazaRoot == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP2CardShadowLodBudgetProfile();
            var atlasMaterial = EnsureHd2dAutonomousP2CardShadowLodAtlasMaterial();
            var receiverMaterial = EnsureHd2dAutonomousP2CardShadowLodReceiverMaterial();
            ApplyHd2dAutonomousP2CardShadowLodBudgetToExistingFoliage(profile);
            ApplyHd2dAutonomousP2CardShadowLodBudgetToExistingCharacters(profile);

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CardShadowLodRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2CardShadowLodRootName);
            root.transform.SetParent(centralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;

            CreateHd2dAutonomousP2CardShadowLodReceiver(root.transform, profile, receiverMaterial);
            CreateHd2dAutonomousP2CardShadowLodNearShadowCluster(root.transform, profile, atlasMaterial);
            CreateHd2dAutonomousP2CardShadowLodGroup(root.transform, profile, atlasMaterial);

            ApplyHd2dAutonomousP0StaticFlags(root);
            SetHd2dAutonomousP2CardShadowLodLayerRecursively(root.transform, CurrentSpaceRenderLayer);
            root.SetActive(profile.ConservativeBudgetEnabledForReview);
            EditorUtility.SetDirty(root);
            _ = camera;
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2CardShadowLodBudget()
        {
            var profile = EnsureHd2dAutonomousP2CardShadowLodBudgetProfile();
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CardShadowLodRootName);
            var atlasMaterial = EnsureHd2dAutonomousP2CardShadowLodAtlasMaterial();
            var atlasTexture = EnsureHd2dAutonomousP2CardShadowLodAtlasTexture();
            if (profile == null ||
                root == null ||
                !root.activeSelf ||
                !profile.ConservativeBudgetEnabledForReview ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalCardShadowLodBudgetApprovedForReview ||
                profile.FarShadowCullDistanceMetersForReview <= profile.ShadowFadeStartMetersForReview ||
                profile.Lod1MergeDistanceMetersForReview <= profile.NearShadowDistanceMetersForReview ||
                profile.LodCullDistanceMetersForReview <= profile.Lod1MergeDistanceMetersForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P2-77 needs an active conservative NEEDS-TOM shadow/LOD budget profile with final approval false.");
            }

            if (atlasMaterial == null ||
                atlasTexture == null ||
                !atlasMaterial.enableInstancing ||
                !IsHd2dAutonomousP2CardShadowLodAlphaClipMaterial(atlasMaterial) ||
                atlasMaterial.GetTexture("_BaseMap") != atlasTexture)
            {
                throw new InvalidOperationException("House slice validation failed: P2-77 atlas material must use the generated alpha-clipped foliage atlas with instancing enabled.");
            }

            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dCardShadowLodBudgetMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(marker => marker != null && marker.gameObject.scene.IsValid())
                .ToArray();
            var rootMarkers = root.GetComponentsInChildren<FastVsHd2dCardShadowLodBudgetMarker>(true);
            var nearShadowCasters = markers.Count(marker => marker.NearShadowCasterForReview && marker.TrackedRendererForReview != null && marker.TrackedRendererForReview.shadowCastingMode != ShadowCastingMode.Off);
            var farShadowOff = markers.Count(marker => marker.FarShadowCulledForReview && marker.TrackedRendererForReview != null && marker.TrackedRendererForReview.shadowCastingMode == ShadowCastingMode.Off);
            var atlasCandidates = markers.Count(marker => marker.ParticipatesInAtlasPrototypeForReview);
            var alphaClipReady = markers.Count(marker => marker.AlphaClippedShadowCasterForReview);
            var twoSidedReady = markers.Count(marker => marker.TwoSidedCullOffForReview);
            var receiveOff = markers.Count(marker => marker.ReceiveShadowsDisabledForReview);
            var characterMarkers = markers.Count(marker => marker.SubjectForReview == FastVsHd2dCardShadowLodSubject.CharacterCard && marker.CharacterShadowPolicyDeferredForReview);
            var lodGroups = root.GetComponentsInChildren<LODGroup>(true);
            var validLodGroups = lodGroups.Count(IsHd2dAutonomousP2CardShadowLodGroupValid);
            var rootRendererCount = root.GetComponentsInChildren<Renderer>(true).Length;

            if (markers.Length < profile.MinimumBudgetMarkerCountForReview ||
                rootMarkers.Length < 18 ||
                nearShadowCasters < profile.MinimumNearShadowCasterCountForReview ||
                farShadowOff < profile.MinimumFarShadowOffCountForReview ||
                atlasCandidates < profile.MinimumAtlasCandidateCountForReview ||
                validLodGroups < profile.MinimumLodGroupCountForReview ||
                alphaClipReady < atlasCandidates ||
                twoSidedReady < atlasCandidates ||
                receiveOff < atlasCandidates ||
                characterMarkers < 4 ||
                rootRendererCount < 10)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: P2-77 shadow/LOD metrics are short. markers={markers.Length}, rootMarkers={rootMarkers.Length}, nearCasters={nearShadowCasters}, farOff={farShadowOff}, atlas={atlasCandidates}, lodGroups={validLodGroups}, alphaClip={alphaClipReady}, twoSided={twoSidedReady}, receiveOff={receiveOff}, chars={characterMarkers}, rootRenderers={rootRendererCount}.");
            }

            var stats = AnalyzeHd2dAutonomousP2CardShadowLodBudgetStats(root, profile);
            if (stats.Lod0RendererCount <= stats.Lod1RendererCount ||
                stats.BaselineMaterialBindingCount <= stats.AtlasMaterialBindingCount ||
                stats.EstimatedShadowPassCasterReduction <= 0 ||
                stats.RootFarShadowOffRendererCount < profile.MinimumFarShadowOffCountForReview)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: P2-77 expected fewer LOD1/atlas/shadow casters. lod0={stats.Lod0RendererCount}, lod1={stats.Lod1RendererCount}, baselineMats={stats.BaselineMaterialBindingCount}, atlasMats={stats.AtlasMaterialBindingCount}, reduction={stats.EstimatedShadowPassCasterReduction}, farOff={stats.RootFarShadowOffRendererCount}.");
            }

            var overdrawStats = AnalyzeHd2dAutonomousP0FoliageOverdrawStats();
            if (overdrawStats.CardCount < 12 ||
                overdrawStats.AlphaClipCount != overdrawStats.CardCount ||
                overdrawStats.TightMeshCount != overdrawStats.CardCount)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-77 must preserve P0-16 alpha-clip/tight-card foliage state. cards={overdrawStats.CardCount}, alpha={overdrawStats.AlphaClipCount}, tight={overdrawStats.TightMeshCount}.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2CardShadowLodProfileRuntimePath), "finalCardShadowLodBudgetApproved", Hd2dAutonomousP2CardShadowLodProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2CardShadowLodMarkerRuntimePath), "FastVsHd2dCardShadowLodSubject", Hd2dAutonomousP2CardShadowLodMarkerRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2CardShadowLodEditorPath), "LODGroup", Hd2dAutonomousP2CardShadowLodEditorPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2CardShadowLodEditorPath), Hd2dAutonomousP2CardShadowLodAtlasTextureId, Hd2dAutonomousP2CardShadowLodEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Art/Shaders/FastVS/FastVS_FoliageCardLit.shader"), "clip(alpha - _Cutoff)", "Assets/Art/Shaders/FastVS/FastVS_FoliageCardLit.shader");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2CardShadowLodBudget", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2CardShadowLodBudget", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dCardShadowLodBudgetProfile EnsureHd2dAutonomousP2CardShadowLodBudgetProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dCardShadowLodBudgetProfile>(Hd2dAutonomousP2CardShadowLodProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dCardShadowLodBudgetProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2CardShadowLodProfilePath);
            }

            profile.ConfigureForReview(
                7.5f,
                5.2f,
                10.0f,
                0f,
                13.5f,
                32.0f,
                0.38f,
                0.12f,
                0.02f,
                0.15f,
                28,
                6,
                8,
                1,
                12,
                4,
                1,
                true,
                true,
                true,
                true,
                true,
                false,
                "Keep P2-77 as conservative shadow/LOD/atlas budget data only. Tom should tune final shadow distance/fade, LOD screen heights, atlas layout, and whether character sprite shadow-map casters remain enabled versus contact blobs.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static void CreateHd2dAutonomousP2CardShadowLodReceiver(Transform root, FastVsHd2dCardShadowLodBudgetProfile profile, Material material)
        {
            var receiver = GameObject.CreatePrimitive(PrimitiveType.Cube);
            receiver.name = "P2_77_ShadowReceiverGround";
            receiver.transform.SetParent(root, false);
            receiver.transform.localPosition = CentralPlazaVsCenter + new Vector3(4.65f, 0.035f, -2.22f);
            receiver.transform.localScale = new Vector3(4.20f, 0.05f, 2.40f);
            receiver.transform.localRotation = Quaternion.identity;
            var collider = receiver.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = receiver.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = true;
            ConfigureHd2dAutonomousP2CardShadowLodMarker(
                receiver,
                FastVsHd2dCardShadowLodSubject.ReviewShadowReceiver,
                "review-shadow-receiver",
                renderer,
                null,
                0f,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                1,
                1,
                profile);
        }

        private static void CreateHd2dAutonomousP2CardShadowLodNearShadowCluster(Transform root, FastVsHd2dCardShadowLodBudgetProfile profile, Material atlasMaterial)
        {
            var group = new GameObject(Hd2dAutonomousP2CardShadowLodNearGroupName);
            group.transform.SetParent(root, false);
            group.transform.localPosition = Vector3.zero;
            group.transform.localRotation = Quaternion.identity;
            group.transform.localScale = Vector3.one;

            var baseCenter = CentralPlazaVsCenter + new Vector3(4.35f, 0.02f, -2.70f);
            for (var i = 0; i < 6; i++)
            {
                var card = CreateQuad(
                    $"P2_77_NearShadowCutoutCard_{i:00}",
                    group.transform,
                    baseCenter + new Vector3(i * 0.24f, 0.43f + i * 0.001f, Mathf.Sin(i * 1.37f) * 0.22f),
                    new Vector3(0.44f + PositiveModulo(i, 2) * 0.04f, 0.78f + PositiveModulo(i * 3, 4) * 0.035f, 1f),
                    atlasMaterial);
                card.transform.localRotation = Quaternion.Euler(0f, -32f + i * 13f, i % 2 == 0 ? -3f : 4f);
                ApplyHd2dAutonomousP0FoliageTightMesh(card, atlasMaterial);
                var renderer = card.GetComponent<Renderer>();
                ConfigureHd2dAutonomousP2CardShadowLodRenderer(renderer, atlasMaterial, ShadowCastingMode.On, false, profile.AlphaCutoffForReview);
                ConfigureHd2dAutonomousP2CardShadowLodMarker(
                    card,
                    FastVsHd2dCardShadowLodSubject.FoliageCard,
                    "near-alpha-shadow-review",
                    renderer,
                    null,
                    Mathf.Lerp(1.2f, profile.NearShadowDistanceMetersForReview, i / 5f),
                    true,
                    false,
                    true,
                    true,
                    true,
                    true,
                    false,
                    true,
                    profile.BaselineFoliageMaterialCountForReview,
                    profile.AtlasMaterialCountForReview,
                    profile);
            }
        }

        private static void CreateHd2dAutonomousP2CardShadowLodGroup(Transform root, FastVsHd2dCardShadowLodBudgetProfile profile, Material atlasMaterial)
        {
            var groupObject = new GameObject(Hd2dAutonomousP2CardShadowLodGroupName);
            groupObject.transform.SetParent(root, false);
            groupObject.transform.localPosition = CentralPlazaVsCenter + new Vector3(8.35f, 0.02f, -0.78f);
            groupObject.transform.localRotation = Quaternion.identity;
            groupObject.transform.localScale = Vector3.one;
            var lodGroup = groupObject.AddComponent<LODGroup>();
            lodGroup.fadeMode = LODFadeMode.CrossFade;
            lodGroup.animateCrossFading = true;

            var sourceMaterials = EnsureHd2dAutonomousP2CardShadowLodSourceMaterials();
            var lod0Root = new GameObject("LOD0_SourceIndividualCards");
            lod0Root.transform.SetParent(groupObject.transform, false);
            lod0Root.transform.localPosition = Vector3.zero;
            lod0Root.transform.localRotation = Quaternion.identity;
            lod0Root.transform.localScale = Vector3.one;

            var lod0Renderers = new List<Renderer>();
            for (var i = 0; i < 12; i++)
            {
                var material = sourceMaterials[i % sourceMaterials.Length];
                var card = CreateQuad(
                    $"P2_77_LOD0_SourceCard_{i:00}",
                    lod0Root.transform,
                    new Vector3((i % 6) * 0.28f - 0.70f, 0.34f + (i / 6) * 0.15f, (i / 6) * 0.24f),
                    new Vector3(0.34f + PositiveModulo(i, 3) * 0.03f, 0.56f + PositiveModulo(i * 2, 3) * 0.04f, 1f),
                    material);
                card.transform.localRotation = Quaternion.Euler(0f, -22f + i * 7f, i % 2 == 0 ? -2f : 2f);
                ApplyHd2dAutonomousP0FoliageTightMesh(card, material);
                var renderer = card.GetComponent<Renderer>();
                ConfigureHd2dAutonomousP2CardShadowLodRenderer(renderer, material, ShadowCastingMode.Off, false, profile.AlphaCutoffForReview);
                lod0Renderers.Add(renderer);
                ConfigureHd2dAutonomousP2CardShadowLodMarker(
                    card,
                    FastVsHd2dCardShadowLodSubject.FarFoliageLod0,
                    "far-lod0-source-cards",
                    renderer,
                    lodGroup,
                    profile.Lod1MergeDistanceMetersForReview,
                    false,
                    true,
                    true,
                    true,
                    true,
                    true,
                    false,
                    true,
                    profile.BaselineFoliageMaterialCountForReview,
                    profile.AtlasMaterialCountForReview,
                    profile);
            }

            var lod1Root = new GameObject("LOD1_MergedAtlasBillboards");
            lod1Root.transform.SetParent(groupObject.transform, false);
            lod1Root.transform.localPosition = Vector3.zero;
            lod1Root.transform.localRotation = Quaternion.identity;
            lod1Root.transform.localScale = Vector3.one;
            var lod1Renderers = new List<Renderer>();
            for (var i = 0; i < 2; i++)
            {
                var merged = CreateQuad(
                    $"P2_77_LOD1_MergedAtlasBillboard_{i:00}",
                    lod1Root.transform,
                    new Vector3(i == 0 ? -0.35f : 0.58f, 0.52f, i == 0 ? 0.05f : 0.24f),
                    new Vector3(1.20f, 0.88f, 1f),
                    atlasMaterial);
                merged.transform.localRotation = Quaternion.Euler(0f, i == 0 ? -12f : 10f, 0f);
                ApplyHd2dAutonomousP0FoliageTightMesh(merged, atlasMaterial);
                var renderer = merged.GetComponent<Renderer>();
                ConfigureHd2dAutonomousP2CardShadowLodRenderer(renderer, atlasMaterial, ShadowCastingMode.Off, false, profile.AlphaCutoffForReview);
                lod1Renderers.Add(renderer);
                ConfigureHd2dAutonomousP2CardShadowLodMarker(
                    merged,
                    FastVsHd2dCardShadowLodSubject.FarFoliageMergedLod,
                    "far-lod1-merged-atlas",
                    renderer,
                    lodGroup,
                    profile.LodCullDistanceMetersForReview,
                    false,
                    true,
                    true,
                    true,
                    true,
                    true,
                    false,
                    true,
                    profile.BaselineFoliageMaterialCountForReview,
                    profile.AtlasMaterialCountForReview,
                    profile);
            }

            lodGroup.SetLODs(new[]
            {
                new LOD(profile.Lod0ScreenRelativeHeightForReview, lod0Renderers.Where(renderer => renderer != null).ToArray()),
                new LOD(profile.Lod1ScreenRelativeHeightForReview, lod1Renderers.Where(renderer => renderer != null).ToArray()),
                new LOD(profile.LodCullScreenRelativeHeightForReview, Array.Empty<Renderer>()),
            });
            lodGroup.RecalculateBounds();
            EditorUtility.SetDirty(lodGroup);
            ConfigureHd2dAutonomousP2CardShadowLodMarker(
                groupObject,
                FastVsHd2dCardShadowLodSubject.AtlasPrototype,
                "lod-group-atlas-prototype",
                lod1Renderers.FirstOrDefault(),
                lodGroup,
                profile.Lod1MergeDistanceMetersForReview,
                false,
                true,
                true,
                true,
                true,
                true,
                false,
                true,
                profile.BaselineFoliageMaterialCountForReview,
                profile.AtlasMaterialCountForReview,
                profile);
        }

        private static void ApplyHd2dAutonomousP2CardShadowLodBudgetToExistingFoliage(FastVsHd2dCardShadowLodBudgetProfile profile)
        {
            var states = CaptureHd2dAutonomousP0FoliageCardStates();
            var focus = CentralPlazaVsCenter;
            for (var i = 0; i < states.Count; i++)
            {
                var state = states[i];
                if (state == null || state.Renderer == null || state.MeshFilter == null || state.Renderer.transform.root.name == Hd2dAutonomousP2CardShadowLodRootName)
                {
                    continue;
                }

                var renderer = state.Renderer;
                var material = renderer.sharedMaterial;
                var center = renderer.bounds.center;
                var flatDistance = Vector2.Distance(new Vector2(center.x, center.z), new Vector2(focus.x, focus.z));
                var nearShadow = flatDistance <= profile.NearShadowDistanceMetersForReview;
                var farShadowOff = flatDistance >= profile.FarShadowCullDistanceMetersForReview;
                var shadowMode = nearShadow ? ShadowCastingMode.On : ShadowCastingMode.Off;
                ConfigureHd2dAutonomousP2CardShadowLodRenderer(renderer, material, shadowMode, false, profile.AlphaCutoffForReview);
                var meshArea = GetHd2dAutonomousP0FoliageMeshArea(state.MeshFilter.sharedMesh);
                ConfigureHd2dAutonomousP2CardShadowLodMarker(
                    renderer.gameObject,
                    FastVsHd2dCardShadowLodSubject.FoliageCard,
                    farShadowOff ? "existing-far-shadow-culled" : "existing-near-shadow-budget",
                    renderer,
                    null,
                    flatDistance,
                    nearShadow,
                    farShadowOff || !nearShadow,
                    true,
                    IsHd2dAutonomousP2CardShadowLodCullOff(material),
                    IsHd2dAutonomousP2CardShadowLodAlphaClipMaterial(material),
                    IsHd2dAutonomousP0FoliageTightMesh(state.MeshFilter.sharedMesh, meshArea),
                    false,
                    !renderer.receiveShadows,
                    profile.BaselineFoliageMaterialCountForReview,
                    profile.AtlasMaterialCountForReview,
                    profile);
            }
        }

        private static void ApplyHd2dAutonomousP2CardShadowLodBudgetToExistingCharacters(FastVsHd2dCardShadowLodBudgetProfile profile)
        {
            var renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null || !UsesHd2dAutonomousP0CharacterBillboardShadowFix(renderer.sharedMaterial))
                {
                    continue;
                }

                ConfigureHd2dAutonomousP2CardShadowLodMarker(
                    renderer.gameObject,
                    FastVsHd2dCardShadowLodSubject.CharacterCard,
                    "character-contact-blob-shadow-budget-deferred",
                    renderer,
                    null,
                    0f,
                    renderer.shadowCastingMode != ShadowCastingMode.Off,
                    false,
                    false,
                    IsHd2dAutonomousP2CardShadowLodCullOff(renderer.sharedMaterial),
                    IsHd2dAutonomousP2CardShadowLodAlphaClipMaterial(renderer.sharedMaterial),
                    true,
                    profile.CharacterFullShadowBudgetDeferredForReview,
                    !renderer.receiveShadows,
                    1,
                    1,
                    profile);
            }
        }

        private static Material EnsureHd2dAutonomousP2CardShadowLodAtlasMaterial()
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            AssetDatabase.ImportAsset("Assets/Art/Shaders/FastVS/FastVS_FoliageCardLit.shader", ImportAssetOptions.ForceSynchronousImport);
            var shader = Shader.Find(FoliageCardLitShaderName) ?? Shader.Find(SpriteCardRampLitShaderName) ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-77 foliage atlas shader is missing.");
            }

            var path = $"{MaterialDirectory}/FastVS_House_{Hd2dAutonomousP2CardShadowLodAtlasMaterialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            ConfigureSpriteCardCutoutMaterial(material, SpriteCardCutoutRenderQueue, shader.name);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP2CardShadowLodAtlasTexture(), Vector2.one);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", Color.white);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", Color.white);
            }

            ConfigureHd2dAutonomousP2CardShadowLodMaterialCommon(material, EnsureHd2dAutonomousP2CardShadowLodBudgetProfile().AlphaCutoffForReview);
            SetSharedVegetationMaterialWeights(material, 1f, 0f);
            ApplyStage2EmissionDefaults(material);
            ApplyMaterialRole(material, Hd2dAutonomousP2CardShadowLodAtlasMaterialId, FastVsHd2dMaterialRole.SpriteCard);
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            return material;
        }

        private static Material EnsureHd2dAutonomousP2CardShadowLodReceiverMaterial()
        {
            var material = FlatMaterial(Hd2dAutonomousP2CardShadowLodReceiverMaterialId, new Color(0.58f, 0.62f, 0.52f, 1f), false, FastVsHd2dMaterialRole.SurfaceLit);
            material.enableInstancing = true;
            if (material.HasProperty("_ShadowReceiveStrength"))
            {
                material.SetFloat("_ShadowReceiveStrength", 0.56f);
            }

            if (material.HasProperty("_SurfaceRampStrength"))
            {
                material.SetFloat("_SurfaceRampStrength", 0.18f);
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material[] EnsureHd2dAutonomousP2CardShadowLodSourceMaterials()
        {
            var baseLeaf = FlatMaterial("hd2d_p2_77_source_foliage_leaf_tint", new Color(0.36f, 0.56f, 0.30f, 1f), true, FastVsHd2dMaterialRole.SpriteCard);
            var warmLeaf = FlatMaterial("hd2d_p2_77_source_foliage_warm_tint", new Color(0.54f, 0.66f, 0.32f, 1f), true, FastVsHd2dMaterialRole.SpriteCard);
            var flowerWarm = FlatMaterial("hd2d_p2_77_source_foliage_flower_warm_tint", new Color(0.92f, 0.48f, 0.30f, 1f), true, FastVsHd2dMaterialRole.SpriteCard);
            var flowerCool = FlatMaterial("hd2d_p2_77_source_foliage_flower_cool_tint", new Color(0.42f, 0.54f, 0.92f, 1f), true, FastVsHd2dMaterialRole.SpriteCard);
            return new[]
            {
                EnsureFoliageCardMaterial(FoliageGrassCardATexturePath, "hd2d_p2_77_lod_source_grass_a", baseLeaf, 0.035f),
                EnsureFoliageCardMaterial(FoliageGrassCardBTexturePath, "hd2d_p2_77_lod_source_grass_b", warmLeaf, 0.035f),
                EnsureFoliageCardMaterial(FoliageBushCardTexturePath, "hd2d_p2_77_lod_source_bush", baseLeaf, 0.035f),
                EnsureFoliageCardMaterial(FoliageFlowerCardTexturePath, "hd2d_p2_77_lod_source_flower", flowerWarm, 0.035f),
                EnsureFoliageCardMaterial(FoliageFlowerCardTexturePath, "hd2d_p2_77_lod_source_flower_cool", flowerCool, 0.035f),
            };
        }

        private static Texture2D EnsureHd2dAutonomousP2CardShadowLodAtlasTexture()
        {
            var texture = EnsureGeneratedTexture(
                Hd2dAutonomousP2CardShadowLodAtlasTextureId,
                256,
                256,
                FilterMode.Point,
                SampleHd2dAutonomousP2CardShadowLodAtlasPixel);
            texture.wrapMode = TextureWrapMode.Clamp;
            EditorUtility.SetDirty(texture);
            return texture;
        }

        private static Color SampleHd2dAutonomousP2CardShadowLodAtlasPixel(int x, int y)
        {
            var tileX = Mathf.Clamp(x / 128, 0, 1);
            var tileY = Mathf.Clamp(y / 128, 0, 1);
            var localX = x % 128;
            var localY = y % 128;
            var u = localX / 127f;
            var v = localY / 127f;
            var kind = tileY == 0
                ? (tileX == 0 ? FoliageCardTextureKind.GrassA : FoliageCardTextureKind.GrassB)
                : (tileX == 0 ? FoliageCardTextureKind.Bush : FoliageCardTextureKind.Flower);
            var sample = SampleFoliageCardPixel(kind, Mathf.RoundToInt(u * 63f), Mathf.RoundToInt(v * 63f), 64, 64);
            if (sample.a <= 0.02f)
            {
                return new Color(0f, 0f, 0f, 0f);
            }

            var tint = kind == FoliageCardTextureKind.Flower
                ? new Color(0.94f, 0.58f, 0.40f, 1f)
                : kind == FoliageCardTextureKind.Bush
                    ? new Color(0.40f, 0.62f, 0.30f, 1f)
                    : new Color(0.34f, 0.55f, 0.26f, 1f);
            var atlasGuide = (((x / 4) + (y / 4)) & 1) == 0 ? 1.00f : 0.92f;
            return new Color(tint.r * atlasGuide, tint.g * atlasGuide, tint.b * atlasGuide, sample.a);
        }

        private static void ConfigureHd2dAutonomousP2CardShadowLodRenderer(Renderer renderer, Material material, ShadowCastingMode shadowCastingMode, bool receiveShadows, float alphaCutoff)
        {
            if (renderer == null)
            {
                return;
            }

            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = shadowCastingMode;
            renderer.receiveShadows = receiveShadows;
            renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
            renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            if (material != null)
            {
                ConfigureHd2dAutonomousP2CardShadowLodMaterialCommon(material, alphaCutoff);
            }

            EditorUtility.SetDirty(renderer);
        }

        private static void ConfigureHd2dAutonomousP2CardShadowLodMaterialCommon(Material material, float alphaCutoff)
        {
            if (material == null)
            {
                return;
            }

            if (material.HasProperty("_Cutoff"))
            {
                material.SetFloat("_Cutoff", Mathf.Clamp01(alphaCutoff));
            }

            if (material.HasProperty("_AlphaClip"))
            {
                material.SetFloat("_AlphaClip", 1f);
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", 0f);
            }

            if (material.HasProperty("_ZWrite"))
            {
                material.SetFloat("_ZWrite", 1f);
            }

            material.enableInstancing = true;
            material.doubleSidedGI = true;
            EditorUtility.SetDirty(material);
        }

        private static void ConfigureHd2dAutonomousP2CardShadowLodMarker(
            GameObject target,
            FastVsHd2dCardShadowLodSubject subject,
            string group,
            Renderer renderer,
            LODGroup lodGroup,
            float distanceBand,
            bool nearShadowCaster,
            bool farShadowCulled,
            bool atlasCandidate,
            bool twoSidedCullOff,
            bool alphaClippedShadowCaster,
            bool tightCardMesh,
            bool characterShadowPolicyDeferred,
            bool receiveShadowsDisabled,
            int sourceMaterialBindingCount,
            int optimizedMaterialBindingCount,
            FastVsHd2dCardShadowLodBudgetProfile profile)
        {
            if (target == null)
            {
                return;
            }

            var marker = target.GetComponent<FastVsHd2dCardShadowLodBudgetMarker>();
            if (marker == null)
            {
                marker = target.AddComponent<FastVsHd2dCardShadowLodBudgetMarker>();
            }

            marker.ConfigureForReview(
                subject,
                group,
                renderer,
                lodGroup,
                distanceBand,
                nearShadowCaster,
                farShadowCulled,
                atlasCandidate,
                twoSidedCullOff,
                alphaClippedShadowCaster,
                tightCardMesh,
                characterShadowPolicyDeferred,
                receiveShadowsDisabled,
                sourceMaterialBindingCount,
                optimizedMaterialBindingCount,
                profile != null && profile.NeedsTomApprovalForReview);
            EditorUtility.SetDirty(marker);
            EditorUtility.SetDirty(target);
        }

        private static void SetHd2dAutonomousP2CardShadowLodNearShadowCasting(GameObject root, bool enabled)
        {
            if (root == null)
            {
                return;
            }

            var nearRoot = FindChildByName(root.transform, Hd2dAutonomousP2CardShadowLodNearGroupName);
            if (nearRoot == null)
            {
                return;
            }

            var renderers = nearRoot.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                renderers[i].shadowCastingMode = enabled ? ShadowCastingMode.On : ShadowCastingMode.Off;
                EditorUtility.SetDirty(renderers[i]);
            }
        }

        private static LODGroup FindHd2dAutonomousP2CardShadowLodReviewGroup(GameObject root)
        {
            if (root == null)
            {
                return null;
            }

            var groupObject = FindChildByName(root.transform, Hd2dAutonomousP2CardShadowLodGroupName);
            return groupObject != null ? groupObject.GetComponent<LODGroup>() : null;
        }

        private static void ForceHd2dAutonomousP2CardShadowLodGroup(LODGroup lodGroup, int lod)
        {
            if (lodGroup == null)
            {
                return;
            }

            lodGroup.ForceLOD(lod);
            var lod0Root = FindChildByName(lodGroup.transform, "LOD0_SourceIndividualCards");
            var lod1Root = FindChildByName(lodGroup.transform, "LOD1_MergedAtlasBillboards");
            if (lod == 0)
            {
                SetHd2dAutonomousP2CardShadowLodRootActive(lod0Root, true);
                SetHd2dAutonomousP2CardShadowLodRootActive(lod1Root, false);
            }
            else if (lod == 1)
            {
                SetHd2dAutonomousP2CardShadowLodRootActive(lod0Root, false);
                SetHd2dAutonomousP2CardShadowLodRootActive(lod1Root, true);
            }
            else
            {
                SetHd2dAutonomousP2CardShadowLodRootActive(lod0Root, true);
                SetHd2dAutonomousP2CardShadowLodRootActive(lod1Root, true);
            }

            EditorUtility.SetDirty(lodGroup);
        }

        private static void SetHd2dAutonomousP2CardShadowLodRootActive(Transform root, bool active)
        {
            if (root == null)
            {
                return;
            }

            root.gameObject.SetActive(active);
            EditorUtility.SetDirty(root.gameObject);
        }

        private static Transform FindChildByName(Transform root, string childName)
        {
            if (root == null)
            {
                return null;
            }

            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
            {
                if (string.Equals(transform.name, childName, StringComparison.Ordinal))
                {
                    return transform;
                }
            }

            return null;
        }

        private static void CaptureHd2dAutonomousP2CardShadowLodShot(
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
            string mode,
            ICollection<string> rows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(anchorLocalPosition + new Vector3(0f, 0.02f, -0.84f));
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
            rows?.Add($"| `{fileName}` | {label} | {mode} | {FormatVector3ForReport(anchorLocalPosition)} | {FormatVector3ForReport(cameraOffset)} | {fieldOfView:0.#} |");
        }

        private static void WriteHd2dAutonomousP2CardShadowLodBudgetReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dCardShadowLodBudgetProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics shadowDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics lodDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CardShadowLodRootName);
            var stats = AnalyzeHd2dAutonomousP2CardShadowLodBudgetStats(root, profile);
            var lines = new List<string>
            {
                "# P2-77 Card Shadow / LOD Budget Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for foliage/character card shadow cost, far-card LOD merge, and atlas material binding reduction.",
                "- Final performance/art approval remains false; this pass records a retunable budget profile, marker audit, A/B captures, and runtime-safe review geometry.",
                $"- Recommendation: {profile.RecommendationForReview}",
                string.Empty,
                "| Setting | Value |",
                "|---|---:|",
                $"| Profile | `{Hd2dAutonomousP2CardShadowLodProfilePath}` |",
                $"| Review root | `{Hd2dAutonomousP2CardShadowLodRootName}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalCardShadowLodBudgetApprovedForReview)} |",
                $"| Near shadow / fade start / far shadow cull meters | {profile.NearShadowDistanceMetersForReview:0.###} / {profile.ShadowFadeStartMetersForReview:0.###} / {profile.FarShadowCullDistanceMetersForReview:0.###} |",
                $"| LOD0 / LOD1 / cull distances | {profile.Lod0StartDistanceMetersForReview:0.###} / {profile.Lod1MergeDistanceMetersForReview:0.###} / {profile.LodCullDistanceMetersForReview:0.###} |",
                $"| LOD screen heights | {profile.Lod0ScreenRelativeHeightForReview:0.###} / {profile.Lod1ScreenRelativeHeightForReview:0.###} / {profile.LodCullScreenRelativeHeightForReview:0.###} |",
                $"| Alpha cutoff | {profile.AlphaCutoffForReview:0.###} |",
                string.Empty,
                "| Metric | Value |",
                "|---|---:|",
                $"| Markers total / root | {stats.MarkerCount} / {stats.RootMarkerCount} |",
                $"| Foliage markers / character markers | {stats.FoliageMarkerCount} / {stats.CharacterMarkerCount} |",
                $"| Near shadow casters | {stats.NearShadowCasterCount} |",
                $"| Far shadow-off renderers | {stats.FarShadowOffRendererCount} |",
                $"| Root far shadow-off renderers | {stats.RootFarShadowOffRendererCount} |",
                $"| Atlas prototype candidates | {stats.AtlasCandidateCount} |",
                $"| LOD groups / valid groups | {stats.LodGroupCount} / {stats.ValidLodGroupCount} |",
                $"| LOD0 renderers / LOD1 renderers | {stats.Lod0RendererCount} / {stats.Lod1RendererCount} |",
                $"| Baseline material bindings / atlas bindings | {stats.BaselineMaterialBindingCount} / {stats.AtlasMaterialBindingCount} |",
                $"| Estimated shadow-pass caster reduction | {stats.EstimatedShadowPassCasterReduction} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                shadowDiff.ToReportRow("near foliage shadow off vs alpha-clipped shadow on"),
                lodDiff.ToReportRow("far LOD0 individual cards vs LOD1 merged atlas"),
                string.Empty,
                "| Screenshot | Label | Mode | Anchor | Offset | FOV |",
                "|---|---|---|---|---|---:|",
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Baseline with the review near foliage card shadow casters disabled |",
                $"| `{screenshotFiles[1]}` | Same shot with alpha-clipped foliage shadow casters enabled on the near receiver |",
                $"| `{screenshotFiles[2]}` | Forced LOD0: multiple source cards and several source material bindings |",
                $"| `{screenshotFiles[3]}` | Forced LOD1: merged atlas billboards with far shadows off |",
                $"| `{screenshotFiles[4]}` | Top-down policy diagnostic for shadow receiver, near cluster, far LOD group, and atlas prototype |",
                string.Empty,
                "- Self-review note: this is a conservative budget/proof rig. The atlas is a prototype material/texture and not final packed production UV art.",
                "- Tom decision required: tune shadow distance/fade, LOD thresholds, source atlas packing, far silhouette density, and whether character sprite shadow-map casters should remain enabled or shift fully to contact blobs.",
            });
            File.WriteAllText(Path.Combine(outputDirectory, "card_shadow_lod_budget_foliage_character_cards_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP2CardShadowLodBudgetStats AnalyzeHd2dAutonomousP2CardShadowLodBudgetStats(GameObject root, FastVsHd2dCardShadowLodBudgetProfile profile)
        {
            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dCardShadowLodBudgetMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(marker => marker != null && marker.gameObject.scene.IsValid())
                .ToArray();
            var rootMarkers = root != null ? root.GetComponentsInChildren<FastVsHd2dCardShadowLodBudgetMarker>(true) : Array.Empty<FastVsHd2dCardShadowLodBudgetMarker>();
            var lodGroups = root != null ? root.GetComponentsInChildren<LODGroup>(true) : Array.Empty<LODGroup>();
            var stats = new Hd2dAutonomousP2CardShadowLodBudgetStats
            {
                MarkerCount = markers.Length,
                RootMarkerCount = rootMarkers.Length,
                FoliageMarkerCount = markers.Count(marker => marker.SubjectForReview == FastVsHd2dCardShadowLodSubject.FoliageCard || marker.SubjectForReview == FastVsHd2dCardShadowLodSubject.FarFoliageLod0 || marker.SubjectForReview == FastVsHd2dCardShadowLodSubject.FarFoliageMergedLod),
                CharacterMarkerCount = markers.Count(marker => marker.SubjectForReview == FastVsHd2dCardShadowLodSubject.CharacterCard),
                NearShadowCasterCount = markers.Count(marker => marker.NearShadowCasterForReview && marker.TrackedRendererForReview != null && marker.TrackedRendererForReview.shadowCastingMode != ShadowCastingMode.Off),
                FarShadowOffRendererCount = markers.Count(marker => marker.FarShadowCulledForReview && marker.TrackedRendererForReview != null && marker.TrackedRendererForReview.shadowCastingMode == ShadowCastingMode.Off),
                RootFarShadowOffRendererCount = rootMarkers.Count(marker =>
                    marker.FarShadowCulledForReview &&
                    (marker.SubjectForReview == FastVsHd2dCardShadowLodSubject.FarFoliageLod0 ||
                     marker.SubjectForReview == FastVsHd2dCardShadowLodSubject.FarFoliageMergedLod) &&
                    marker.TrackedRendererForReview != null &&
                    marker.TrackedRendererForReview.shadowCastingMode == ShadowCastingMode.Off),
                AtlasCandidateCount = markers.Count(marker => marker.ParticipatesInAtlasPrototypeForReview),
                LodGroupCount = lodGroups.Length,
                ValidLodGroupCount = lodGroups.Count(IsHd2dAutonomousP2CardShadowLodGroupValid),
            };

            var reviewGroup = root != null ? FindHd2dAutonomousP2CardShadowLodReviewGroup(root) : null;
            if (reviewGroup != null)
            {
                var lods = reviewGroup.GetLODs();
                if (lods.Length >= 2)
                {
                    stats.Lod0RendererCount = lods[0].renderers?.Count(renderer => renderer != null) ?? 0;
                    stats.Lod1RendererCount = lods[1].renderers?.Count(renderer => renderer != null) ?? 0;
                    stats.BaselineMaterialBindingCount = CountDistinctSharedMaterials(lods[0].renderers);
                    stats.AtlasMaterialBindingCount = CountDistinctSharedMaterials(lods[1].renderers);
                    stats.RootFarShadowOffRendererCount = Math.Max(
                        stats.RootFarShadowOffRendererCount,
                        CountShadowOffRenderers(lods[0].renderers) + CountShadowOffRenderers(lods[1].renderers));
                }
            }

            if (stats.BaselineMaterialBindingCount <= 0)
            {
                stats.BaselineMaterialBindingCount = profile.BaselineFoliageMaterialCountForReview;
            }

            if (stats.AtlasMaterialBindingCount <= 0)
            {
                stats.AtlasMaterialBindingCount = profile.AtlasMaterialCountForReview;
            }

            stats.RootFarShadowOffRendererCount = Math.Max(stats.RootFarShadowOffRendererCount, stats.FarShadowOffRendererCount);
            stats.EstimatedShadowPassCasterReduction = Math.Max(0, stats.FarShadowOffRendererCount);
            return stats;
        }

        private static int CountShadowOffRenderers(Renderer[] renderers)
        {
            return renderers?.Count(renderer => renderer != null && renderer.shadowCastingMode == ShadowCastingMode.Off) ?? 0;
        }

        private static bool IsHd2dAutonomousP2CardShadowLodGroupValid(LODGroup lodGroup)
        {
            if (lodGroup == null)
            {
                return false;
            }

            var lods = lodGroup.GetLODs();
            return lods.Length >= 3 &&
                   lods[0].renderers != null &&
                   lods[1].renderers != null &&
                   lods[0].renderers.Count(renderer => renderer != null) > lods[1].renderers.Count(renderer => renderer != null) &&
                   (lods[2].renderers == null || lods[2].renderers.Length == 0);
        }

        private static int CountDistinctSharedMaterials(Renderer[] renderers)
        {
            if (renderers == null)
            {
                return 0;
            }

            return renderers
                .Where(renderer => renderer != null)
                .Select(renderer => renderer.sharedMaterial)
                .Where(material => material != null)
                .Distinct()
                .Count();
        }

        private static bool IsHd2dAutonomousP2CardShadowLodAlphaClipMaterial(Material material)
        {
            return material != null &&
                   material.HasProperty("_Cutoff") &&
                   material.HasProperty("_AlphaClip") &&
                   material.GetFloat("_AlphaClip") > 0.5f &&
                   material.GetFloat("_Cutoff") > 0.01f &&
                   material.GetFloat("_Cutoff") < 0.70f;
        }

        private static bool IsHd2dAutonomousP2CardShadowLodCullOff(Material material)
        {
            return material != null &&
                   material.HasProperty("_Cull") &&
                   material.GetFloat("_Cull") <= 0.01f;
        }

        private static void SetHd2dAutonomousP2CardShadowLodLayerRecursively(Transform root, int layer)
        {
            if (root == null)
            {
                return;
            }

            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
            {
                transform.gameObject.layer = layer;
                EditorUtility.SetDirty(transform.gameObject);
            }
        }

        private struct Hd2dAutonomousP2CardShadowLodBudgetStats
        {
            public int MarkerCount;
            public int RootMarkerCount;
            public int FoliageMarkerCount;
            public int CharacterMarkerCount;
            public int NearShadowCasterCount;
            public int FarShadowOffRendererCount;
            public int RootFarShadowOffRendererCount;
            public int AtlasCandidateCount;
            public int LodGroupCount;
            public int ValidLodGroupCount;
            public int Lod0RendererCount;
            public int Lod1RendererCount;
            public int BaselineMaterialBindingCount;
            public int AtlasMaterialBindingCount;
            public int EstimatedShadowPassCasterReduction;
        }
    }
}
