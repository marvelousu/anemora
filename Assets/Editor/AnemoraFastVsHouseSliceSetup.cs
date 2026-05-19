using System;
using System.Collections.Generic;
using System.IO;
using Anemora.FastVS;
using Anemora.TimeManagement;
using TMPro;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHouseSliceSetup
    {
        public const string ScenePath = "Assets/Scenes/Anemora_FastVS_HouseSlice.unity";
        public const string BuildDirectory = "Builds/FastVS_HouseSlice";
        public const string BuildExePath = BuildDirectory + "/Anemora_FastVS_HouseSlice.exe";

        private const string MaterialDirectory = "Assets/Art/Materials/FastVS/HouseSlice";
        private const string TextureDirectory = "Assets/Art/Textures/FastVS/HouseSlice";
        private const string TimewriterBrushIconTexturePath = TextureDirectory + "/FastVS_House_timewriter_brush_icon_v01.png";
        private const string MusicClipPath = "Assets/Audio/Music/Zone1_Ambient.ogg";
        private const string WindClipPath = "Assets/Audio/SFX/env/sfx_env_wind_loop_01.ogg";
        private const string BirdsClipPath = "Assets/Audio/SFX/env/sfx_env_birds_01.ogg";
        private const string FastVsDialogueFontAssetPath = "Assets/UI/Localization/Fonts/Anemora_JP.asset";
        private const string NiroFrontSpritePath = "Assets/Art/Characters/FastVS/Niro/hero_niro_front_v12_64x96_review_only.png";
        private const string NiroBackSpritePath = "Assets/Art/Characters/FastVS/Niro/hero_niro_back_v12_64x96_review_only.png";
        private const string NiroLeftSpritePath = "Assets/Art/Characters/FastVS/Niro/hero_niro_left_v12_64x96_review_only.png";
        private const string NiroRightSpritePath = "Assets/Art/Characters/FastVS/Niro/hero_niro_right_v12_64x96_review_only.png";
        private const string NiroFrontStripPath = "Assets/Art/Characters/FastVS/Niro/hero_niro_idle_front_v45_review_only.png";
        private const string NiroBackStripPath = "Assets/Art/Characters/FastVS/Niro/hero_niro_idle_back_v45_review_only.png";
        private const string NiroLeftStripPath = "Assets/Art/Characters/FastVS/Niro/hero_niro_idle_left_v45_review_only.png";
        private const string NiroRightStripPath = "Assets/Art/Characters/FastVS/Niro/hero_niro_idle_right_v45_review_only.png";
        private const string NiroWalkFrontStripPath = "Assets/Art/Characters/FastVS/Niro/hero_niro_walk_front_v45_review_only.png";
        private const string NiroWalkBackStripPath = "Assets/Art/Characters/FastVS/Niro/hero_niro_walk_back_v45_review_only.png";
        private const string NiroWalkLeftStripPath = "Assets/Art/Characters/FastVS/Niro/hero_niro_walk_left_v45_review_only.png";
        private const string NiroWalkRightStripPath = "Assets/Art/Characters/FastVS/Niro/hero_niro_walk_right_v45_review_only.png";
        private const string RetoWritingLoopStripPath = "Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png";
        private const string RetoLowerArmsStripPath = "Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png";
        private const string RetoTalkLoopStripPath = "Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png";
        private const string RetoRaiseArmsStripPath = "Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png";
        private const string AriaNormalLoopStripPath = "Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png";
        private const int RetoExpectedFrameWidth = 64;
        private const int RetoExpectedTextureHeight = 96;
        private const int RetoTransitionFrameCount = 6;
        private const int RetoTalkFrameCount = 4;
        private const int NiroExpectedTextureWidth = 64;
        private const int NiroExpectedTextureHeight = 96;
        private const int NiroAnimatedFrameCount = 4;
        private const int NiroExpectedStripWidth = NiroExpectedTextureWidth * NiroAnimatedFrameCount;
        private const float NiroTransparentFootPixels = 2f;
        private const int CurrentSpaceRenderLayer = 27;
        private const int OtherTimeSpaceRenderLayer = 28;
        private const int PortalFrameRenderLayer = 26;
        private const int PlayerVisibleRenderLayer = 0;
        private static readonly Vector3 HouseInteriorCenter = new Vector3(-8.35f, 0f, -8.35f);
        private static readonly Vector3 HouseInteriorPlayerStart = HouseInteriorCenter + new Vector3(-2.42f, 0.12f, 0.96f);
        private static readonly Vector3 HouseExteriorCenter = new Vector3(8.20f, 0f, 8.20f);
        private static readonly Vector3 CentralPlazaVsCenter = new Vector3(20.80f, 0f, 15.80f);
        private static readonly Vector3 LibraryVsCenter = new Vector3(31.00f, 0f, 20.00f);
        private static readonly Vector3 InteriorDoorStoryTriggerCenter = HouseInteriorCenter + new Vector3(0.82f, 0.70f, -1.86f);
        private static readonly Vector3 InteriorDoorStoryTriggerSize = new Vector3(0.86f, 1.72f, 0.28f);
        private static readonly Vector3 InteriorDoorTriggerCenter = HouseInteriorCenter + new Vector3(0.82f, 0.70f, -2.32f);
        private static readonly Vector3 InteriorDoorExitTarget = HouseInteriorCenter + new Vector3(0.82f, 0.02f, -1.52f);
        private static readonly Vector3 ExteriorDoorTriggerCenter = HouseExteriorCenter + new Vector3(-1.05f, 0.70f, -2.22f);
        private static readonly Vector3 ExteriorDoorExitTarget = HouseExteriorCenter + new Vector3(-1.05f, 0.02f, -3.18f);
        private static readonly Vector3 ExteriorToPlazaTriggerCenter = HouseExteriorCenter + new Vector3(10.95f, 0.70f, 5.90f);
        private static readonly Vector3 PlazaFromExteriorTarget = CentralPlazaVsCenter + new Vector3(-3.15f, 0.02f, -2.15f);
        private static readonly Vector3 PlazaToExteriorTriggerCenter = CentralPlazaVsCenter + new Vector3(-5.25f, 0.70f, -3.65f);
        private static readonly Vector3 ExteriorFromPlazaTarget = HouseExteriorCenter + new Vector3(8.95f, 0.02f, 4.55f);
        private static readonly Vector3 PlazaToLibraryTriggerCenter = CentralPlazaVsCenter + new Vector3(0.00f, 0.70f, 7.30f);
        private static readonly Vector3 LibraryFromPlazaTarget = LibraryVsCenter + new Vector3(0.00f, 0.02f, -4.75f);
        private static readonly Vector3 LibraryToPlazaTriggerCenter = LibraryVsCenter + new Vector3(0.00f, 0.70f, -6.35f);
        private static readonly Vector3 PlazaFromLibraryTarget = CentralPlazaVsCenter + new Vector3(0.00f, 0.02f, 5.20f);
        private static readonly Vector3 RetoLibraryDeskLocalPosition = LibraryVsCenter + new Vector3(1.35f, 0.02f, 0.82f);
        private static readonly Vector3 DoorTriggerSize = new Vector3(0.86f, 1.72f, 0.62f);
        private static readonly Vector3 RouteTriggerSize = new Vector3(0.82f, 1.72f, 0.62f);
        private static readonly Vector3 PastLibraryBookCueLocalPosition = LibraryVsCenter + new Vector3(0.00f, 0.405f, -0.92f);
        private static readonly Vector3 CurrentLibraryRetoDeskBookInitialLocalPosition = LibraryVsCenter + new Vector3(1.36f, 0.405f, 0.42f);
        private static readonly Vector3 CurrentLibraryReturnedBookLocalPosition = LibraryVsCenter + new Vector3(1.08f, 0.405f, 0.06f);
        private static readonly Vector3 PastLibraryPersonCueLocalPosition = LibraryVsCenter + new Vector3(-2.98f, 0.06f, 1.42f);
        private const float LibrarySideShelfRunLength = 4.96f;
        private const float LibrarySideShelfPostThickness = 0.14f;
        private const float LibrarySideShelfCapThickness = 0.14f;
        private const float LibrarySideShelfBoardThickness = 0.08f;
        private const float LibrarySideShelfBoardDepth = 0.88f;
        private const float LibrarySideShelfBackPanelCenterY = 0.90f;
        private const float LibrarySideShelfBackPanelHeight = 1.64f;
        private const float LibrarySideShelfPostCenterY = 0.88f;
        private const float LibrarySideShelfPostHeight = 1.72f;
        private const float LibrarySideShelfTopCapCenterY = 1.74f;
        private const float LibrarySideShelfTexturePanelCenterY = 0.86f;
        private const float LibrarySideShelfTexturePanelHeight = 1.42f;
        private const float LibrarySideShelfBoardFirstY = 0.38f;
        private const float LibrarySideShelfBoardStepY = 0.40f;
        private static readonly Vector3 CurrentLibraryBookCueGlowScale = new Vector3(0.62f, 0.018f, 0.62f);
        private static readonly Vector3 CurrentLibraryAriaCueGlowScale = new Vector3(0.68f, 0.018f, 0.68f);
        private const float CurrentLibraryCueFloorY = 0.082f;
        private static readonly Vector3 PastLibraryTargetBookMarkerScale = new Vector3(0.16f, 0.16f, 0.16f);
        private static readonly Vector3 PastLibraryReadingTableCleanScale = new Vector3(1.90f, 0.18f, 0.54f);
        private static readonly Vector3 PastLibraryReadingTableCleanColliderSize = new Vector3(2.10f, 1.14f, 0.70f);
        private static readonly Vector3 CurrentLibraryRuinBookPileScale = new Vector3(0.50f, 0.16f, 0.28f);
        private static readonly Vector2 RegionSize = new Vector2(78f, 58f);
        private static readonly Vector2 DragStart = new Vector2(380f, 215f);
        private static readonly Vector2 DragEnd = new Vector2(850f, 600f);
        private static readonly string[] ForbiddenReferenceTokens =
        {
            "GfxPolish",
            "Meshy",
            "TimeWindow_Diorama",
            "V32",
            "BroadInteriorExteriorRoute"
        };

        [MenuItem("Anemora/Fast VS/Create House Slice")]
        public static void CreateHouseSliceScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EnsureFolder("Assets/Scenes");
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            EnsureExternalCharacterAssets();
            var materials = EnsureMaterials();

            var currentRoot = new GameObject("FastVS_Current_NiroHouseInteriorExterior").transform;
            currentRoot.position = Vector3.zero;
            var pastRoot = new GameObject("FastVS_Past_NiroHouseInteriorExterior").transform;
            pastRoot.position = Vector3.zero;

            var currentAreas = CreateHouseMap(currentRoot, false, materials);
            var pastAreas = CreateHouseMap(pastRoot, true, materials);
            var areaVisibility = CreateHouseAreaVisibility(currentAreas, pastAreas);

            var camera = CreateCamera(currentRoot);
            CreateLighting();
            CreateHd2dGlobalVolume();
            CreateHd2dAtmosphere(currentRoot, pastRoot);
            CreateAudio(currentRoot, areaVisibility);
            var player = CreateNiroPlayer(currentRoot, camera, materials);
            var controller = CreateController(camera, currentRoot, pastRoot, player, materials);
            var guide = CreateGuide(camera, controller, player, areaVisibility);
            var story = CreateStoryFlow(camera, controller, player, areaVisibility, guide);
            CreateHouseDoorTransitions(controller, player, areaVisibility, story);
            ApplyInitialReviewLayers(currentRoot, pastRoot, player.transform, camera);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS house slice scene created: {ScenePath}");
        }

        public static void ValidateHouseSliceBatch()
        {
            if (!File.Exists(ScenePath))
            {
                CreateHouseSliceScene();
            }

            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            if (controller == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing V24 TimeWindow controller.");
            }

            if (GameObject.Find("FastVS_Current_NiroHouseInteriorExterior") == null ||
                GameObject.Find("FastVS_Past_NiroHouseInteriorExterior") == null ||
                GameObject.Find("FastVS_Player_NiroHouseSlice") == null)
            {
                throw new InvalidOperationException("House slice validation failed: required roots or player are missing.");
            }

            ValidateNoForbiddenSceneReferences();
            ValidateFastVsDialogueTmpFontAsset();
            ValidateTimewriterBrushIconTexture();
            ValidateHouseYardBirdsAudio();
            ValidateNiroSpriteAsset();
            ValidateAriaSpriteAsset();
            ValidateRetoStateflowAssets();
            ValidateHouseMapSeparationAndDoorTransitions(controller);
            ValidateDirectionalSpriteAnimator();
            ValidatePlayerSpritePresentation();
            ValidateFastVsHd2dFirstCycleVisuals();
            ValidateFastVsHd2dSecondCycleAtmosphere();
            ValidateFastVsHd2dThirdCycleSurfaceTextures();
            ValidateFastVsStoryFlow();
            ValidateCameraStaysOnSameCoordinateRoot(controller);

            controller.ClosePortal();
            ValidateDoorWarp(controller);
            if (!controller.TryOpenPortalForTests(DragStart, DragEnd))
            {
                throw new InvalidOperationException("House slice validation failed: TimeWindow drag-open was rejected.");
            }

            if (!controller.HasPortalPair || !controller.HasLiveApertureViewForReview)
            {
                throw new InvalidOperationException("House slice validation failed: live aperture portal was not created.");
            }

            ValidateApertureBottomSitsAboveFloor(controller);
            ValidateApertureIntersectingObjectSuppression(controller);
            ValidateCurrentBackSideEdgeBlock(controller);
            ValidateAperturePlayerLayerCulling(controller, false);

            var portalLocal = controller.PortalLocalCenterForReview;
            controller.TransferCurrentToOtherForReview(new Vector3(portalLocal.x, 0.72f, portalLocal.z + 0.18f));
            ValidateAperturePlayerLayerCulling(controller, true);
            if (!controller.PlayerInOtherTime)
            {
                throw new InvalidOperationException("House slice validation failed: transfer to past did not occur.");
            }

            controller.ClosePortal();
            if (!controller.PlayerInOtherTime ||
                !controller.HasPortalPair ||
                !controller.CloseRejectedBecausePlayerInOtherTimeForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Time Window can be closed while Niro is still in past-side space.");
            }

            controller.TransferOtherToCurrentForReview(new Vector3(portalLocal.x, 0.72f, portalLocal.z - 0.18f));
            ValidateAperturePlayerLayerCulling(controller, false);
            if (controller.PlayerInOtherTime)
            {
                throw new InvalidOperationException("House slice validation failed: return to current did not occur.");
            }

            Debug.Log("Fast VS house slice validation passed.");
        }

        [MenuItem("Anemora/Fast VS/Build House Slice")]
        public static void BuildHouseSlicePlayer()
        {
            if (!File.Exists(ScenePath))
            {
                CreateHouseSliceScene();
            }

            Directory.CreateDirectory(BuildDirectory);
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = BuildExePath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.CleanBuildCache
            });

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException($"Fast VS house slice build failed: {report.summary.result}");
            }

            Debug.Log($"Fast VS house slice player built: {Path.GetFullPath(BuildExePath)}");
        }

        public static void BuildAndValidateBatch()
        {
            CreateHouseSliceScene();
            ValidateHouseSliceBatch();
            CreateHouseSliceScene();
            BuildHouseSlicePlayer();
        }

        public static void CaptureReviewScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518");
        }

        public static void CaptureHd2dLocalShapeScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_local_shape_20260519");
        }

        public static void CaptureHd2dFirstCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_first_cycle_20260520");
        }

        public static void CaptureHd2dSecondCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_second_cycle_20260520");
        }

        public static void CaptureHd2dThirdCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_surface_textures_20260520");
        }

        private static void CaptureReviewScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var story = UnityEngine.Object.FindFirstObjectByType<FastVsStoryFlowController>();
            if (controller == null || visibility == null || guide == null || camera == null || story == null)
            {
                throw new InvalidOperationException("Fast VS screenshot capture failed: scene review components are missing.");
            }

            CaptureReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.Interior, HouseInteriorPlayerStart, $"{outputDirectory}/01_interior_niro_shadow.png");
            CaptureReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.Exterior, ExteriorDoorExitTarget, $"{outputDirectory}/02_exterior_niro_shadow.png");
            CaptureReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.CentralPlaza, CentralPlazaVsCenter + new Vector3(0f, 0.02f, -1.10f), $"{outputDirectory}/07_plaza_library_facade_current.png");
            CaptureOtherTimeReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.CentralPlaza, CentralPlazaVsCenter + new Vector3(0f, 0.02f, -1.10f), $"{outputDirectory}/08_plaza_library_facade_past.png");
            CaptureReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.Library, RetoLibraryDeskLocalPosition + new Vector3(-1.15f, 0f, -1.35f), $"{outputDirectory}/03_library_reto_desk.png");
            var retoAnimator = UnityEngine.Object.FindFirstObjectByType<FastVsRetoWritingAnimator>(FindObjectsInactive.Include);
            if (retoAnimator != null)
            {
                retoAnimator.SetDialogueImmediateForReview();
                CaptureReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.Library, RetoLibraryDeskLocalPosition + new Vector3(-1.15f, 0f, -1.35f), $"{outputDirectory}/04_library_reto_talk_loop.png");
                retoAnimator.SetWritingImmediateForReview();
            }
            CaptureOtherTimeReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.Library, LibraryVsCenter + new Vector3(0f, 0.02f, -0.40f), $"{outputDirectory}/05_library_past_no_temp_people.png");
            CaptureReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.Library, RetoLibraryDeskLocalPosition + new Vector3(-1.15f, 0f, -1.35f), $"{outputDirectory}/06_library_dialogue_tmp_font.png");
            story.TriggerRetoEventForReview();
            story.RefreshPresentationForReview();
            story.CompleteRuntimeHudTypingForReview();
            SaveCameraPng(camera, $"{outputDirectory}/06_library_dialogue_tmp_font.png");
            var guard = 0;
            while (story.CurrentBeatIdForReview != "scene1.reto.1d.timewriter_activation.pocket_glow_pause" && guard++ < 20)
            {
                story.AdvanceStoryForReview();
                story.RefreshPresentationForReview();
                story.CompleteRuntimeHudTypingForReview();
            }

            if (!story.TimewriterPocketGlowVisibleForReview)
            {
                throw new InvalidOperationException("Fast VS screenshot capture failed: Timewriter pocket glow did not become visible for the review frame.");
            }

            SaveCameraPng(camera, $"{outputDirectory}/09_library_timewriter_pocket_glow.png");
            guard = 0;
            while (!story.WaitingForPastObservationForReview && guard++ < 20)
            {
                story.AdvanceStoryForReview();
                story.RefreshPresentationForReview();
                story.CompleteRuntimeHudTypingForReview();
            }

            if (!story.CurrentTimeWindowBookCueVisibleForReview || !story.CurrentTimeWindowAriaCueVisibleForReview)
            {
                throw new InvalidOperationException("Fast VS screenshot capture failed: yellow current-side Time Window floor cues did not become visible.");
            }

            CaptureReviewScreenshot(controller, visibility, guide, camera, FastVsHouseArea.Library, LibraryVsCenter + new Vector3(0f, 0.02f, -0.40f), $"{outputDirectory}/10_library_current_yellow_timewindow_cues.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS review screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureReviewScreenshot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHouseArea area,
            Vector3 playerLocalPosition,
            string outputPath)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerCurrentLocalForReview(playerLocalPosition);
            guide.ApplyActiveTimeIsolationForReview();
            PositionReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(playerLocalPosition));
            SaveCameraPng(camera, outputPath);
        }

        private static void CaptureOtherTimeReviewScreenshot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHouseArea area,
            Vector3 playerLocalPosition,
            string outputPath)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerOtherTimeLocalForReview(playerLocalPosition);
            guide.ApplyActiveTimeIsolationForReview();
            var previousMask = camera.cullingMask;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            camera.cullingMask = (previousMask & ~currentBit) | otherBit | playerBit;
            PositionReviewCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(playerLocalPosition));
            SaveCameraPng(camera, outputPath);
            camera.cullingMask = previousMask;
            controller.ForcePlayerCurrentLocalForReview(playerLocalPosition);
            guide.ApplyActiveTimeIsolationForReview();
        }

        private static void PositionReviewCamera(Camera camera, Vector3 anchor)
        {
            var position = anchor + new Vector3(0f, 2.75f, -4.55f);
            var lookAt = anchor + new Vector3(0f, 0.72f, 0.45f);
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
        }

        private static void SaveCameraPng(Camera camera, string outputPath)
        {
            var previousTarget = camera.targetTexture;
            var previousActive = RenderTexture.active;
            var renderTexture = new RenderTexture(1280, 720, 24, RenderTextureFormat.ARGB32);
            var texture = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
            try
            {
                camera.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;
                Canvas.ForceUpdateCanvases();
                camera.Render();
                texture.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply(false, false);
                File.WriteAllBytes(outputPath, texture.EncodeToPNG());
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                renderTexture.Release();
                UnityEngine.Object.DestroyImmediate(renderTexture);
                UnityEngine.Object.DestroyImmediate(texture);
            }
        }

        private static HouseMapAreas CreateHouseMap(Transform root, bool past, Materials materials)
        {
            var prefix = past ? "Past" : "Current";
            var interiorRoot = CreateMapSetRoot(root, $"{prefix}_HouseInteriorMap_SeparateSpace");
            var exteriorRoot = CreateMapSetRoot(root, $"{prefix}_HouseExteriorMap_SeparateSpace");
            var plazaRoot = CreateMapSetRoot(root, $"{prefix}_CentralPlazaMap_SeparateSpace");
            var libraryRoot = CreateMapSetRoot(root, $"{prefix}_LibraryMap_SeparateSpace");
            CreateInterior(interiorRoot, prefix, past, materials);
            CreateExterior(exteriorRoot, prefix, past, materials);
            CreateCentralPlaza(plazaRoot, prefix, past, materials);
            CreateLibrary(libraryRoot, prefix, past, materials);
            CreateHouseDoorMarkers(interiorRoot, exteriorRoot, prefix, past, materials);
            CreateRouteMoveMarkers(exteriorRoot, plazaRoot, libraryRoot, prefix, past, materials);

            return new HouseMapAreas(interiorRoot.gameObject, exteriorRoot.gameObject, plazaRoot.gameObject, libraryRoot.gameObject);
        }

        private static Transform CreateMapSetRoot(Transform root, string name)
        {
            var mapRoot = new GameObject(name).transform;
            mapRoot.SetParent(root, false);
            mapRoot.localPosition = Vector3.zero;
            mapRoot.localRotation = Quaternion.identity;
            mapRoot.localScale = Vector3.one;
            return mapRoot;
        }

        private static FastVsHouseAreaVisibility CreateHouseAreaVisibility(HouseMapAreas currentAreas, HouseMapAreas pastAreas)
        {
            var areaObject = new GameObject("FastVS_HouseAreaVisibility_SeparateInteriorExterior");
            var visibility = areaObject.AddComponent<FastVsHouseAreaVisibility>();
            SerializedSet(visibility, "currentInteriorMap", currentAreas.Interior);
            SerializedSet(visibility, "pastInteriorMap", pastAreas.Interior);
            SerializedSet(visibility, "currentExteriorMap", currentAreas.Exterior);
            SerializedSet(visibility, "pastExteriorMap", pastAreas.Exterior);
            SerializedSet(visibility, "currentCentralPlazaMap", currentAreas.CentralPlaza);
            SerializedSet(visibility, "pastCentralPlazaMap", pastAreas.CentralPlaza);
            SerializedSet(visibility, "currentLibraryMap", currentAreas.Library);
            SerializedSet(visibility, "pastLibraryMap", pastAreas.Library);
            SerializedSet(visibility, "activeArea", FastVsHouseArea.Interior);
            visibility.SetActiveAreaForReview(FastVsHouseArea.Interior);
            return visibility;
        }

        private static void CreateWideBase(Transform root, string prefix, bool past, Materials materials)
        {
            CreateLandmarkCube($"{prefix}_FullPairedGround_42mCoordinateField", root, Vector3.zero + new Vector3(0f, -0.08f, 0f), new Vector3(RegionSize.x, 0.10f, RegionSize.y), Quaternion.identity, past ? materials.PastGrass : materials.CurrentGround, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.wide.ground");
            CreateLandmarkCube($"{prefix}_NorthCoordinateFence", root, new Vector3(0f, 0.45f, 20.85f), new Vector3(41.8f, 0.90f, 0.20f), Quaternion.identity, past ? materials.PastFence : materials.CurrentFence, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.bounds.north");
            CreateLandmarkCube($"{prefix}_SouthCoordinateFence", root, new Vector3(0f, 0.45f, -20.85f), new Vector3(41.8f, 0.90f, 0.20f), Quaternion.identity, past ? materials.PastFence : materials.CurrentFence, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.bounds.south");
            CreateLandmarkCube($"{prefix}_WestCoordinateFence", root, new Vector3(-20.85f, 0.45f, 0f), new Vector3(0.20f, 0.90f, 41.8f), Quaternion.identity, past ? materials.PastFence : materials.CurrentFence, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.bounds.west");
            CreateLandmarkCube($"{prefix}_EastCoordinateFence", root, new Vector3(20.85f, 0.45f, 0f), new Vector3(0.20f, 0.90f, 41.8f), Quaternion.identity, past ? materials.PastFence : materials.CurrentFence, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.bounds.east");
        }

        private static void CreateInterior(Transform root, string prefix, bool past, Materials materials)
        {
            var c = HouseInteriorCenter;
            var floor = past ? materials.PastWoodFloor : materials.CurrentInteriorFloor;
            var wall = past ? materials.PastInteriorWall : materials.CurrentInteriorWall;
            var wood = past ? materials.PastFurniture : materials.CurrentFurniture;
            var trim = past ? materials.PastFence : materials.CurrentFence;

            CreateLandmarkCube($"{prefix}_HouseInterior_PixelFloor", root, c + new Vector3(0f, 0f, 0f), new Vector3(7.2f, 0.12f, 5.8f), Quaternion.identity, floor, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_interior.floor");
            CreateLandmarkCube($"{prefix}_HouseInterior_BackWall", root, c + new Vector3(0f, 1.05f, 2.82f), new Vector3(7.35f, 2.10f, 0.18f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_interior.back_wall");
            CreateLandmarkCube($"{prefix}_HouseInterior_LeftWall", root, c + new Vector3(-3.60f, 0.95f, 0.05f), new Vector3(0.18f, 1.90f, 5.70f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_interior.left_wall");
            CreateLandmarkCube($"{prefix}_HouseInterior_RightWall", root, c + new Vector3(3.60f, 0.95f, 0.05f), new Vector3(0.18f, 1.90f, 5.70f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_interior.right_wall");
            CreateLandmarkCube($"{prefix}_HouseInterior_BackWallTopTrim", root, c + new Vector3(0f, 1.92f, 2.74f), new Vector3(6.95f, 0.06f, 0.06f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.back_wall_top_trim");

            var bedMaterial = past ? materials.PastBed : materials.CurrentBed;
            CreateLandmarkCube($"{prefix}_NiroBed_PaperPixelBed", root, c + new Vector3(-1.25f, 0.26f, 0.96f), new Vector3(1.92f, 0.24f, 1.06f), Quaternion.Euler(0f, 0f, past ? 0f : -3f), bedMaterial, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.bed");
            CreateLandmarkCube($"{prefix}_NiroBed_PaperPixelBed_Blanket", root, c + new Vector3(-1.24f, 0.47f, 0.96f), new Vector3(1.72f, 0.12f, 0.92f), Quaternion.Euler(0f, 0f, past ? -2f : -5f), bedMaterial, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.bed.blanket");
            CreateLandmarkCube($"{prefix}_NiroBed_PaperPixelBed_TopFold", root, c + new Vector3(-1.32f, 0.57f, 0.54f), new Vector3(1.48f, 0.04f, 0.16f), Quaternion.Euler(0f, past ? -1f : -4f, 0f), trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.bed.top_fold");
            CreateLandmarkCube($"{prefix}_NiroBed_PaperPixelBed_UnderShadow", root, c + new Vector3(-1.24f, 0.29f, 0.96f), new Vector3(1.64f, 0.05f, 0.86f), Quaternion.Euler(0f, past ? 0f : -3f, 0f), materials.Shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.bed.under_shadow");
            CreateLandmarkCube($"{prefix}_NiroBed_PaperPixelBed_Headboard", root, c + new Vector3(-2.16f, 0.54f, 0.96f), new Vector3(0.12f, 0.60f, 1.02f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.bed.headboard");
            CreateLandmarkCube($"{prefix}_NiroBed_PaperPixelBed_Footboard", root, c + new Vector3(-0.38f, 0.30f, 0.96f), new Vector3(0.10f, 0.28f, 1.00f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.bed.footboard");
            CreateLandmarkCube($"{prefix}_NiroBed_PaperPixelBed_LeftRail", root, c + new Vector3(-1.27f, 0.35f, 0.42f), new Vector3(1.88f, 0.06f, 0.08f), Quaternion.Euler(0f, -3f, 0f), wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.bed.left_rail");
            CreateLandmarkCube($"{prefix}_NiroBed_PaperPixelBed_RightRail", root, c + new Vector3(-1.23f, 0.35f, 1.50f), new Vector3(1.88f, 0.06f, 0.08f), Quaternion.Euler(0f, -3f, 0f), wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.bed.right_rail");
            CreateLandmarkCube($"{prefix}_NiroBed_PillowPixel", root, c + new Vector3(-1.90f, 0.57f, 0.96f), new Vector3(0.50f, 0.18f, 0.80f), Quaternion.identity, materials.Pillow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.pillow");
            CreateLandmarkCube($"{prefix}_SmallTable_PixelTop", root, c + new Vector3(1.04f, 0.48f, -0.84f), new Vector3(0.95f, 0.16f, 0.72f), Quaternion.Euler(0f, past ? 0f : -8f, 0f), wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.table");
            CreateLandmarkCube($"{prefix}_SmallTable_PixelTop_EdgeBand", root, c + new Vector3(1.04f, 0.57f, -0.84f), new Vector3(0.78f, 0.03f, 0.56f), Quaternion.Euler(0f, past ? 0f : -8f, 0f), trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.table.edge_band");
            CreateLandmarkCube($"{prefix}_SmallTable_PixelTop_UnderShadow", root, c + new Vector3(1.04f, 0.39f, -0.84f), new Vector3(0.84f, 0.04f, 0.60f), Quaternion.Euler(0f, past ? 0f : -8f, 0f), materials.Shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.table.under_shadow");
            CreateLandmarkCube($"{prefix}_SmallTable_LegFL", root, c + new Vector3(0.66f, 0.24f, -1.12f), new Vector3(0.10f, 0.42f, 0.10f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.table.leg_fl");
            CreateLandmarkCube($"{prefix}_SmallTable_LegFR", root, c + new Vector3(1.42f, 0.24f, -1.12f), new Vector3(0.10f, 0.42f, 0.10f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.table.leg_fr");
            CreateLandmarkCube($"{prefix}_SmallTable_LegBL", root, c + new Vector3(0.66f, 0.24f, -0.56f), new Vector3(0.10f, 0.42f, 0.10f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.table.leg_bl");
            CreateLandmarkCube($"{prefix}_SmallTable_LegBR", root, c + new Vector3(1.42f, 0.24f, -0.56f), new Vector3(0.10f, 0.42f, 0.10f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.table.leg_br");
            CreateLandmarkCube($"{prefix}_ShelfOrBrokenStack", root, c + new Vector3(2.35f, past ? 0.85f : 0.54f, 1.42f), new Vector3(0.58f, past ? 1.70f : 1.08f, 1.35f), Quaternion.Euler(0f, 0f, past ? 0f : 6f), wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.shelf");
            CreateLandmarkCube($"{prefix}_HearthPixelBlock", root, c + new Vector3(-2.48f, 0.42f, -1.25f), new Vector3(0.90f, 0.84f, 0.32f), Quaternion.identity, past ? materials.PastStone : materials.CurrentStone, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_interior.hearth");

            if (past)
            {
                CreateLandmarkCube("Past_HouseInterior_WindowLightPatch", root, c + new Vector3(1.78f, 0.015f, 0.45f), new Vector3(1.10f, 0.04f, 0.72f), Quaternion.Euler(0f, -8f, 0f), materials.Lamp, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.house_interior.window_light");
                CreateReadableBookProp(root, "Past_HouseInterior_BookOnTable", c + new Vector3(1.04f, 0.59f, -0.84f), Quaternion.Euler(0f, 18f, 0f), new Vector3(0.42f, 0.05f, 0.30f), materials.Book, materials.SignPaint, materials.PastFence, true, "Past.house_interior.book");
            }
            else
            {
                CreateLandmarkCube("Current_HouseInterior_DustPatch_PixelNoise", root, c + new Vector3(1.55f, 0.015f, 0.42f), new Vector3(1.35f, 0.04f, 0.88f), Quaternion.Euler(0f, -8f, 0f), materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.house_interior.dust");
                CreateReadableBookProp(root, "Current_HouseInterior_TimewriterBookCue", c + new Vector3(1.04f, 0.61f, -0.84f), Quaternion.Euler(0f, -12f, 0f), new Vector3(0.50f, 0.05f, 0.34f), materials.Book, materials.SignPaint, materials.CurrentFence, true, "Current.house_interior.timewriter_book_cue");
            }

            CreateInvisibleColliderBox(
                $"{prefix}_HouseInterior_InvisibleFrontDropGuard",
                root,
                c + new Vector3(0f, 0.75f, -2.99f),
                new Vector3(7.45f, 1.50f, 0.22f),
                $"{prefix}.house_interior.front_drop_guard");
        }

        private static void CreateHouseDoorMarkers(Transform interiorRoot, Transform exteriorRoot, string prefix, bool past, Materials materials)
        {
            var wood = past ? materials.PastFurniture : materials.CurrentFurniture;
            var trim = past ? materials.PastFence : materials.CurrentFence;
            var glow = FlatMaterial(
                past ? "past_map_move_floor_glow" : "current_map_move_floor_glow",
                past ? new Color(0.42f, 0.95f, 1.00f, 1f) : new Color(1.00f, 0.56f, 0.20f, 1f),
                true);

            CreateGlowDisc($"{prefix}_HouseInterior_MapMoveGlowPad", interiorRoot, InteriorDoorTriggerCenter + new Vector3(0f, -0.58f, -0.05f), new Vector3(0.68f, 0.018f, 0.45f), glow, true);
            CreateLandmarkCube($"{prefix}_HouseExterior_ReturnDoorHandleCue", exteriorRoot, HouseExteriorCenter + new Vector3(-0.72f, 0.94f, -1.29f), new Vector3(0.10f, 0.10f, 0.08f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.return_door_handle_cue");
            CreateGlowDisc($"{prefix}_HouseExterior_MapMoveGlowPad", exteriorRoot, ExteriorDoorTriggerCenter + new Vector3(0f, -0.58f, 0.02f), new Vector3(0.46f, 0.018f, 0.30f), glow, true);
            CreateGlowDisc($"{prefix}_HouseExterior_DoorEntrySmallGlow", exteriorRoot, ExteriorDoorTriggerCenter + new Vector3(0f, -0.50f, 0.02f), new Vector3(0.30f, 0.015f, 0.20f), glow, true);
            CreateLandmarkCube($"{prefix}_HouseExterior_DoorClosedPanel", exteriorRoot, HouseExteriorCenter + new Vector3(-1.05f, 0.83f, -1.48f), new Vector3(0.74f, 1.38f, 0.07f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.closed_door_panel");
        }

        private static void CreateRouteMoveMarkers(Transform exteriorRoot, Transform plazaRoot, Transform libraryRoot, string prefix, bool past, Materials materials)
        {
            var glow = FlatMaterial(
                past ? "past_route_move_floor_glow" : "current_route_move_floor_glow",
                past ? new Color(0.42f, 0.95f, 1.00f, 1f) : new Color(1.00f, 0.56f, 0.20f, 1f),
                true);

            CreateRouteGlowPad(exteriorRoot, $"{prefix}_HouseExterior_ToPlaza_MapMoveGlowPad", ExteriorToPlazaTriggerCenter, glow, $"{prefix}.house_exterior.to_plaza.pad");
            CreateRouteGlowPad(plazaRoot, $"{prefix}_CentralPlaza_ToHouseExterior_MapMoveGlowPad", PlazaToExteriorTriggerCenter, glow, $"{prefix}.central_plaza.to_house.pad");
            CreateRouteGlowPad(plazaRoot, $"{prefix}_CentralPlaza_ToLibrary_MapMoveGlowPad", PlazaToLibraryTriggerCenter, glow, $"{prefix}.central_plaza.to_library.pad");
            CreateRouteGlowPad(libraryRoot, $"{prefix}_Library_ToCentralPlaza_MapMoveGlowPad", LibraryToPlazaTriggerCenter, glow, $"{prefix}.library.to_plaza.pad");
        }

        private static void CreateRouteGlowPad(Transform root, string objectName, Vector3 triggerCenter, Material glow, string landmarkId)
        {
            CreateGlowDisc(objectName, root, triggerCenter + new Vector3(0f, -0.48f, 0f), new Vector3(0.80f, 0.018f, 0.56f), glow, true);
        }

        private static GameObject CreateGlowDisc(string objectName, Transform root, Vector3 localPosition, Vector3 localScale, Material glow, bool pulse)
        {
            var disc = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            disc.name = objectName;
            disc.transform.SetParent(root, false);
            disc.transform.localPosition = localPosition;
            disc.transform.localRotation = Quaternion.identity;
            disc.transform.localScale = localScale;
            disc.GetComponent<Renderer>().sharedMaterial = glow;
            var collider = disc.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            if (pulse)
            {
                disc.AddComponent<FastVsMapMoveGlowPulse>();
            }

            return disc;
        }

        private static void CreateExterior(Transform root, string prefix, bool past, Materials materials)
        {
            var c = HouseExteriorCenter;
            var wall = past ? materials.PastExteriorWall : materials.CurrentExteriorWall;
            var roof = past ? materials.PastRoof : materials.CurrentRoof;
            var yard = past ? materials.PastGrass : materials.CurrentGrass;
            var path = past ? materials.PastPath : materials.CurrentPath;
            var wood = past ? materials.PastFurniture : materials.CurrentFurniture;

            CreateLandmarkCube($"{prefix}_HouseExterior_YardPixelGround", root, c + new Vector3(0f, 0.005f, 0f), new Vector3(13.6f, 0.08f, 10.4f), Quaternion.identity, yard, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_exterior.yard");
            CreateNorthEastRoad(root, prefix, past, materials);
            CreateLandmarkCube($"{prefix}_HouseExterior_PathToInterior", root, c + new Vector3(-0.78f, 0.035f, -1.82f), new Vector3(2.15f, 0.06f, 3.20f), Quaternion.Euler(0f, -5f, 0f), path, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_exterior.path");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeWallLeftPanel", root, c + new Vector3(-2.65f, 1.05f, -1.62f), new Vector3(1.25f, 2.10f, 0.34f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.facade.left");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeWallRightPanel", root, c + new Vector3(0.55f, 1.05f, -1.62f), new Vector3(1.25f, 2.10f, 0.34f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.facade.right");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeWallLintel", root, c + new Vector3(-1.05f, 1.92f, -1.62f), new Vector3(4.45f, 0.38f, 0.34f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.facade.lintel");
            CreateLandmarkCube($"{prefix}_HouseExterior_RoofWidePixelPlane", root, c + new Vector3(-1.05f, 2.25f, -1.50f), new Vector3(5.15f, 0.35f, 1.90f), Quaternion.Euler(8f, 0f, 0f), roof, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.roof");
            CreateLandmarkCube($"{prefix}_HouseExterior_DoorInteriorDarkGap", root, c + new Vector3(-1.05f, 0.80f, -1.37f), new Vector3(0.86f, 1.46f, 0.05f), Quaternion.identity, materials.DoorwayDark, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.door_gap");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowLeft", root, c + new Vector3(-2.38f, 1.12f, -1.38f), new Vector3(0.62f, 0.54f, 0.08f), Quaternion.identity, past ? materials.WindowLight : materials.EmptyWindow, false, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.window.left");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowRight", root, c + new Vector3(0.22f, 1.12f, -1.38f), new Vector3(0.62f, 0.54f, 0.08f), Quaternion.identity, past ? materials.WindowLight : materials.EmptyWindow, false, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.window.right");
            CreateExteriorHouseArchitecture(root, prefix, past, materials);

            CreateLandmarkCube($"{prefix}_FenceWest_BroadYardBoundary", root, c + new Vector3(-6.78f, 0.42f, 0f), new Vector3(0.18f, 0.84f, 9.8f), Quaternion.identity, past ? materials.PastFence : materials.CurrentFence, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.fence.west");
            CreateLandmarkCube($"{prefix}_FenceNorth_BroadYardBoundary", root, c + new Vector3(-1.25f, 0.42f, 5.12f), new Vector3(10.8f, 0.84f, 0.18f), Quaternion.identity, past ? materials.PastFence : materials.CurrentFence, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.fence.north");
            CreateLandmarkCube($"{prefix}_TreeBillboardLikeTrunk", root, c + new Vector3(3.35f, 0.72f, 2.85f), new Vector3(0.34f, 1.44f, 0.34f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.tree.trunk");
            CreateLandmarkCube($"{prefix}_TreePixelCrown", root, c + new Vector3(3.35f, 1.78f, 2.85f), new Vector3(1.45f, 1.05f, 1.45f), Quaternion.identity, past ? materials.Leaf : materials.CurrentLeaf, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.tree.crown");
            CreateExteriorDetails(root, prefix, past, materials);

            if (!past)
            {
                CreateLandmarkCube("Current_HouseExterior_CollapsedFencePile", root, c + new Vector3(2.45f, 0.22f, -1.85f), new Vector3(1.95f, 0.26f, 0.42f), Quaternion.Euler(0f, -21f, 0f), materials.CurrentFence, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.house_exterior.collapsed_fence");
                CreateLandmarkCube("Current_HouseExterior_SplinteredRoofPatch", root, c + new Vector3(1.30f, 2.46f, -1.33f), new Vector3(1.10f, 0.08f, 0.42f), Quaternion.Euler(8f, 0f, 9f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.house_exterior.roof_patch");
                CreateLandmarkCube("Current_HouseExterior_LooseDoorPlank", root, c + new Vector3(-0.72f, 1.02f, -1.33f), new Vector3(0.12f, 1.30f, 0.09f), Quaternion.Euler(0f, 0f, -8f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.house_exterior.loose_door_plank");
            }
            else
            {
                CreateLandmarkCube("Past_HouseExterior_HangingLanternGlow", root, c + new Vector3(-1.05f, 1.72f, -1.22f), new Vector3(0.24f, 0.34f, 0.08f), Quaternion.identity, materials.Lamp, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.house_exterior.lantern");
                CreateLandmarkCube("Past_HouseExterior_RepairedFenceGate", root, c + new Vector3(1.85f, 0.50f, -2.36f), new Vector3(1.25f, 0.60f, 0.14f), Quaternion.Euler(0f, -17f, 0f), materials.PastFence, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, "Past.house_exterior.repaired_gate");
            }

        }

        private static void CreateCentralPlaza(Transform root, string prefix, bool past, Materials materials)
        {
            var c = CentralPlazaVsCenter;
            var ground = past ? materials.PastGrass : materials.CurrentGrass;
            var path = past ? materials.PastPath : materials.CurrentPath;
            var stone = past ? materials.PastStone : materials.CurrentStone;
            var wood = past ? materials.PastFurniture : materials.CurrentFurniture;
            var trim = past ? materials.PastFence : materials.CurrentFence;
            var wall = past ? materials.PastExteriorWall : materials.CurrentExteriorWall;

            CreateLandmarkCube($"{prefix}_CentralPlaza_PixelGround", root, c + new Vector3(0f, 0f, 2.35f), new Vector3(17.8f, 0.08f, 21.8f), Quaternion.identity, ground, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.central_plaza.ground");
            CreateLandmarkCube($"{prefix}_CentralPlaza_StoneSquare", root, c + new Vector3(0f, 0.035f, 2.25f), new Vector3(12.8f, 0.06f, 12.2f), Quaternion.identity, path, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.central_plaza.square");
            CreatePathBetween(root, c + new Vector3(-1.15f, 0.06f, -1.18f), c + new Vector3(-5.25f, 0.06f, -3.65f), 1.50f, path, $"{prefix}_CentralPlaza_RoadToHouseExterior", true);
            CreatePathBetween(root, c + new Vector3(1.15f, 0.06f, -1.18f), c + new Vector3(5.20f, 0.06f, -3.55f), 1.44f, path, $"{prefix}_CentralPlaza_RoadToSouthEastQuarter", true);
            CreatePathBetween(root, c + new Vector3(0f, 0.06f, 0.60f), c + new Vector3(0f, 0.06f, 7.22f), 1.34f, path, $"{prefix}_CentralPlaza_PathToLibraryEntrance", true);
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryNorthFacade", root, c + new Vector3(0f, 1.55f, 8.10f), new Vector3(9.65f, 3.10f, 0.38f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.central_plaza.library_facade");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWestWing", root, c + new Vector3(-4.58f, 1.30f, 6.95f), new Vector3(0.36f, 2.60f, 2.35f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.central_plaza.library_west_wing");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryEastWing", root, c + new Vector3(4.58f, 1.30f, 6.95f), new Vector3(0.36f, 2.60f, 2.35f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.central_plaza.library_east_wing");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryRoofBlock", root, c + new Vector3(0f, 3.08f, 7.94f), new Vector3(10.05f, 0.48f, 1.36f), Quaternion.identity, past ? materials.PastRoof : materials.CurrentRoof, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.central_plaza.library_roof");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeBaseCourse", root, c + new Vector3(0f, 0.20f, 8.02f), new Vector3(9.95f, 0.10f, 0.12f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade.base_course");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeHeaderTrim", root, c + new Vector3(0f, 2.84f, 8.00f), new Vector3(9.95f, 0.08f, 0.10f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade.header_trim");
            CreateCentralPlazaLibraryFacadeDoor(root, prefix, c + new Vector3(0f, 1.02f, 7.78f), past ? materials.PastFence : materials.CurrentFence, past ? materials.PastFurniture : materials.CurrentFurniture, past ? materials.PastFence : materials.CurrentFence);
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryEntranceStep", root, c + new Vector3(0f, 0.10f, 7.30f), new Vector3(1.70f, 0.12f, 0.78f), Quaternion.identity, stone, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.central_plaza.library_entrance_step");
            CreateCentralPlazaLibraryFacadeWindow(root, prefix, "Left", c + new Vector3(-2.35f, 1.45f, 7.82f), past ? materials.PastFence : materials.CurrentFence, past ? materials.WindowLight : materials.EmptyWindow);
            CreateCentralPlazaLibraryFacadeWindow(root, prefix, "Right", c + new Vector3(2.35f, 1.45f, 7.82f), past ? materials.PastFence : materials.CurrentFence, past ? materials.WindowLight : materials.EmptyWindow);
            CreateLandmarkCube($"{prefix}_CentralPlaza_StoneSquareNorthBorder", root, c + new Vector3(0f, 0.066f, 8.14f), new Vector3(12.55f, 0.03f, 0.12f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.central_plaza.square.border_north");
            CreateLandmarkCube($"{prefix}_CentralPlaza_StoneSquareSouthBorder", root, c + new Vector3(0f, 0.066f, -3.64f), new Vector3(12.55f, 0.03f, 0.12f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.central_plaza.square.border_south");
            CreateLandmarkCube($"{prefix}_CentralPlaza_StoneSquareWestBorder", root, c + new Vector3(-6.34f, 0.066f, 2.25f), new Vector3(0.12f, 0.03f, 11.96f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.central_plaza.square.border_west");
            CreateLandmarkCube($"{prefix}_CentralPlaza_StoneSquareEastBorder", root, c + new Vector3(6.34f, 0.066f, 2.25f), new Vector3(0.12f, 0.03f, 11.96f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.central_plaza.square.border_east");
            CreateLandmarkCube($"{prefix}_CentralPlaza_FountainBase", root, c + new Vector3(0f, 0.22f, 2.25f), new Vector3(1.85f, 0.38f, 1.85f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.fountain_base");
            CreateLandmarkCube($"{prefix}_CentralPlaza_FountainWater", root, c + new Vector3(0f, 0.46f, 2.25f), new Vector3(1.30f, 0.06f, 1.30f), Quaternion.identity, past ? materials.Water : materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.fountain_water");
            if (!past)
            {
                CreateLandmarkCube("Current_CentralPlaza_DryFountainWoodPlankA", root, c + new Vector3(-0.24f, 0.52f, 2.25f), new Vector3(0.92f, 0.07f, 0.16f), Quaternion.Euler(0f, 18f, 2f), materials.CurrentFurniture, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.dry_fountain.wood_plank_a");
                CreateLandmarkCube("Current_CentralPlaza_DryFountainWoodPlankB", root, c + new Vector3(0.35f, 0.54f, 2.08f), new Vector3(0.58f, 0.06f, 0.14f), Quaternion.Euler(0f, -24f, -3f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.dry_fountain.wood_plank_b");
            }
            CreateInvisibleColliderBox($"{prefix}_CentralPlaza_FountainNoStepCollider", root, c + new Vector3(0f, 0.80f, 2.25f), new Vector3(2.25f, 1.60f, 2.25f), $"{prefix}.central_plaza.fountain_no_step");
            CreateLandmarkCube($"{prefix}_CentralPlaza_NoticeBasePlank", root, c + new Vector3(-3.10f, 0.16f, 1.86f), new Vector3(1.08f, 0.10f, 0.22f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.notice_base");
            CreateLandmarkCube($"{prefix}_CentralPlaza_NoticePost", root, c + new Vector3(-3.18f, 0.78f, 1.92f), new Vector3(0.18f, 1.36f, 0.18f), Quaternion.identity, trim, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.notice_post");
            CreateLandmarkCube($"{prefix}_CentralPlaza_NoticeBoard", root, c + new Vector3(-3.18f, 1.23f, 1.99f), new Vector3(1.12f, 0.52f, 0.08f), Quaternion.identity, materials.SignPaint, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.notice_board");

            if (past)
            {
                CreateLandmarkCube("Past_CentralPlaza_MarketStallCounter", root, c + new Vector3(2.85f, 0.42f, -1.25f), new Vector3(1.72f, 0.18f, 0.62f), Quaternion.Euler(0f, -8f, 0f), wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.market_counter");
                CreateLandmarkCube("Past_CentralPlaza_MarketStallCloth", root, c + new Vector3(2.85f, 1.14f, -1.25f), new Vector3(1.82f, 0.12f, 1.02f), Quaternion.Euler(0f, -8f, 0f), materials.LaundryAccent, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.market_awning");
                CreateLandmarkCube("Past_CentralPlaza_MarketStallPostBackLeft", root, c + new Vector3(2.08f, 0.78f, -0.88f), new Vector3(0.10f, 1.24f, 0.10f), Quaternion.identity, trim, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.market_awning.post.back_left");
                CreateLandmarkCube("Past_CentralPlaza_MarketStallPostBackRight", root, c + new Vector3(3.62f, 0.78f, -0.88f), new Vector3(0.10f, 1.24f, 0.10f), Quaternion.identity, trim, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.market_awning.post.back_right");
                CreateLandmarkCube("Past_CentralPlaza_MarketStallPostFrontLeft", root, c + new Vector3(2.00f, 0.72f, -1.64f), new Vector3(0.10f, 1.08f, 0.10f), Quaternion.identity, trim, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.market_awning.post.front_left");
                CreateLandmarkCube("Past_CentralPlaza_MarketStallPostFrontRight", root, c + new Vector3(3.70f, 0.72f, -1.64f), new Vector3(0.10f, 1.08f, 0.10f), Quaternion.identity, trim, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.market_awning.post.front_right");
            }
            else
            {
                CreateLandmarkCube("Current_CentralPlaza_BrokenMarketPlanks", root, c + new Vector3(2.65f, 0.24f, -1.20f), new Vector3(1.95f, 0.24f, 0.42f), Quaternion.Euler(0f, -18f, 0f), wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.broken_market");
                CreateLandmarkCube("Current_CentralPlaza_DryFountainCrack", root, c + new Vector3(0.18f, 0.515f, 2.27f), new Vector3(0.72f, 0.035f, 0.10f), Quaternion.Euler(0f, 22f, 0f), materials.DoorwayDark, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.dry_fountain_crack");
            }

            CreateInvisibleColliderBox($"{prefix}_CentralPlaza_InvisibleFrontDropGuard", root, c + new Vector3(0f, 0.75f, -7.45f), new Vector3(17.80f, 1.50f, 0.24f), $"{prefix}.central_plaza.front_drop_guard");
            CreateInvisibleColliderBox($"{prefix}_CentralPlaza_InvisibleBackBoundary", root, c + new Vector3(0f, 0.75f, 13.35f), new Vector3(17.80f, 1.50f, 0.24f), $"{prefix}.central_plaza.back_boundary");
            CreateInvisibleColliderBox($"{prefix}_CentralPlaza_InvisibleLeftBoundary", root, c + new Vector3(-8.95f, 0.75f, 2.95f), new Vector3(0.24f, 1.50f, 20.90f), $"{prefix}.central_plaza.left_boundary");
            CreateInvisibleColliderBox($"{prefix}_CentralPlaza_InvisibleRightBoundary", root, c + new Vector3(8.95f, 0.75f, 2.95f), new Vector3(0.24f, 1.50f, 20.90f), $"{prefix}.central_plaza.right_boundary");
        }

        private static void CreateLibrary(Transform root, string prefix, bool past, Materials materials)
        {
            var c = LibraryVsCenter;
            var floor = past ? materials.PastWoodFloor : materials.CurrentInteriorFloor;
            var wall = past ? materials.PastInteriorWall : materials.CurrentInteriorWall;
            var wood = past ? materials.PastFurniture : materials.CurrentFurniture;
            var trim = past ? materials.PastFence : materials.CurrentFence;

            CreateLandmarkCube($"{prefix}_Library_PixelFloor", root, c, new Vector3(11.6f, 0.10f, 15.4f), Quaternion.identity, floor, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.library.floor");
            CreateLandmarkCube($"{prefix}_Library_BackWall", root, c + new Vector3(0f, 1.40f, 7.55f), new Vector3(11.8f, 2.80f, 0.24f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.library.back_wall");
            CreateLandmarkCube($"{prefix}_Library_BackWallShelfWide", root, c + new Vector3(0f, 1.24f, 7.28f), new Vector3(9.95f, 1.92f, 0.18f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.back_wall_shelf");
            if (past)
            {
                CreateLandmarkCube("Past_Library_BackWallShelfBoardUpper", root, c + new Vector3(0f, 1.72f, 7.06f), new Vector3(9.45f, 0.08f, 0.22f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.library.back_wall_shelf.board.upper");
                CreateLandmarkCube("Past_Library_BackWallShelfBoardMiddle", root, c + new Vector3(0f, 1.20f, 7.06f), new Vector3(9.45f, 0.08f, 0.22f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.library.back_wall_shelf.board.middle");
                CreateLandmarkCube("Past_Library_BackWallShelfBoardLower", root, c + new Vector3(0f, 0.68f, 7.06f), new Vector3(9.45f, 0.08f, 0.22f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.library.back_wall_shelf.board.lower");
                CreateBookRowTexturePanel("Past_Library_BackWallBookshelfFrontTexturePanel", root, c + new Vector3(0f, 1.34f, 6.99f), new Vector3(9.04f, 1.62f, 0.055f), Quaternion.identity, materials.Book, "Past.library.back_wall_books.front_texture");
            }
            else
            {
                CreateLandmarkCube($"{prefix}_Library_BackWallBooksUpper", root, c + new Vector3(0f, 1.80f, 7.14f), new Vector3(9.35f, 0.20f, 0.10f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.back_wall_books_upper");
                CreateLandmarkCube($"{prefix}_Library_BackWallBooksMiddle", root, c + new Vector3(0f, 1.30f, 7.14f), new Vector3(9.15f, 0.18f, 0.10f), Quaternion.identity, materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.back_wall_books_middle");
                CreateLandmarkCube($"{prefix}_Library_BackWallBooksLower", root, c + new Vector3(0f, 0.82f, 7.14f), new Vector3(8.85f, 0.16f, 0.10f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.back_wall_books_lower");
            }

            CreateLandmarkCube($"{prefix}_Library_LeftWallHint", root, c + new Vector3(-5.65f, 1.35f, 0.05f), new Vector3(0.24f, 2.70f, 14.8f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.library.left_wall");
            CreateLandmarkCube($"{prefix}_Library_RightWallHint", root, c + new Vector3(5.65f, 1.35f, 0.05f), new Vector3(0.24f, 2.70f, 14.8f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.library.right_wall");
            var windowPanelMaterial = past ? materials.WindowLight : materials.EmptyWindow;
            CreateLandmarkCube($"{prefix}_Library_WindowTexture_Left", root, c + new Vector3(-5.50f, 1.48f, -2.54f), new Vector3(0.08f, 0.68f, 1.04f), Quaternion.identity, windowPanelMaterial, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.window.left_texture");
            CreateLandmarkCube($"{prefix}_Library_WindowTexture_Right", root, c + new Vector3(5.50f, 1.48f, -2.54f), new Vector3(0.08f, 0.68f, 1.04f), Quaternion.identity, windowPanelMaterial, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.window.right_texture");
            if (past)
            {
                CreatePastLibrarySideBookshelf(root, "Left", c + new Vector3(-4.78f, 0.18f, 0.60f), Quaternion.Euler(0f, 90f, 0f), wood, materials.Book, materials.Lamp, materials.RedLight);
                CreatePastLibrarySideBookshelf(root, "Right", c + new Vector3(4.78f, 0.18f, 0.60f), Quaternion.Euler(0f, -90f, 0f), wood, materials.Book, materials.Lamp, materials.RedLight);
            }
            CreateLandmarkCube($"{prefix}_Library_ServiceDesk", root, c + new Vector3(-2.45f, 0.34f, -3.20f), new Vector3(1.55f, 0.38f, 0.54f), Quaternion.Euler(0f, -4f, 0f), wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.service_desk");
            if (!past)
            {
                CreateLibraryReadingTableAssembly($"{prefix}_Library_ReadingTableLong", root, c + new Vector3(1.08f, 0.32f, 0.12f), Quaternion.Euler(0f, 4f, 0f), new Vector3(2.28f, 0.18f, 0.72f), wood, trim, materials.Shadow, $"{prefix}.library.reading_table");
                CreateLibraryReadingTableAssembly($"{prefix}_Library_ReadingTableSideA", root, c + new Vector3(-1.72f, 0.32f, 1.42f), Quaternion.Euler(0f, -5f, 0f), new Vector3(1.80f, 0.18f, 0.62f), wood, trim, materials.Shadow, $"{prefix}.library.reading_table_side_a");
                CreateLibraryReadingTableAssembly($"{prefix}_Library_ReadingTableSideB", root, c + new Vector3(2.98f, 0.32f, -1.48f), Quaternion.Euler(0f, 8f, 0f), new Vector3(1.65f, 0.18f, 0.58f), wood, trim, materials.Shadow, $"{prefix}.library.reading_table_side_b");
                CreateInvisibleColliderBox($"{prefix}_Library_ReadingTableLong_NoStepCollider", root, c + new Vector3(1.08f, 0.88f, 0.12f), new Vector3(2.42f, 1.32f, 0.88f), $"{prefix}.library.reading_table.long.no_step");
                CreateInvisibleColliderBox($"{prefix}_Library_ReadingTableSideA_NoStepCollider", root, c + new Vector3(-1.72f, 0.88f, 1.42f), new Vector3(1.96f, 1.32f, 0.78f), $"{prefix}.library.reading_table.side_a.no_step");
                CreateInvisibleColliderBox($"{prefix}_Library_ReadingTableSideB_NoStepCollider", root, c + new Vector3(2.98f, 0.88f, -1.48f), new Vector3(1.82f, 1.32f, 0.74f), $"{prefix}.library.reading_table.side_b.no_step");
                CreateReadableBookProp(root, $"{prefix}_Library_TableOpenBook", c + new Vector3(-1.72f, 0.405f, 1.42f), Quaternion.Euler(0f, -13f, 0f), new Vector3(0.34f, 0.04f, 0.22f), materials.Book, materials.SignPaint, materials.CurrentFence, true, $"{prefix}.library.table_open_book");
            }
            CreateLandmarkCube($"{prefix}_Library_EntryThreshold", root, c + new Vector3(0f, 0.035f, -6.35f), new Vector3(1.18f, 0.035f, 0.26f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.library.entry_threshold");
            CreateLandmarkCube($"{prefix}_Library_SecondFloor_LeftBalcony", root, c + new Vector3(-4.18f, 2.05f, 0.10f), new Vector3(1.55f, 0.10f, 10.40f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.library.second_floor.left_balcony");
            CreateLandmarkCube($"{prefix}_Library_SecondFloor_RightBalcony", root, c + new Vector3(4.18f, 2.05f, 0.10f), new Vector3(1.55f, 0.10f, 10.40f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.library.second_floor.right_balcony");
            CreateLandmarkCube($"{prefix}_Library_SecondFloor_BackGallery", root, c + new Vector3(0f, 2.05f, 6.45f), new Vector3(9.20f, 0.10f, 1.38f), Quaternion.identity, wood, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.library.second_floor.back_gallery");
            CreateLandmarkCube($"{prefix}_Library_SecondFloor_Railing_Left", root, c + new Vector3(-4.92f, 2.44f, 0.10f), new Vector3(0.10f, 0.58f, 10.40f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.second_floor.railing.left");
            CreateLandmarkCube($"{prefix}_Library_SecondFloor_Railing_Right", root, c + new Vector3(4.92f, 2.44f, 0.10f), new Vector3(0.10f, 0.58f, 10.40f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.second_floor.railing.right");
            CreateLandmarkCube($"{prefix}_Library_SecondFloor_Railing_Back", root, c + new Vector3(0f, 2.44f, 7.08f), new Vector3(9.20f, 0.58f, 0.10f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.second_floor.railing.back");
            CreateLandmarkCube($"{prefix}_Library_SecondFloor_LadderHint", root, c + new Vector3(-4.95f, 0.96f, 5.90f), new Vector3(0.32f, 1.90f, 0.18f), Quaternion.Euler(0f, 0f, -12f), wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.second_floor.ladder_hint");

            if (past)
            {
                CreateLandmarkCube("Past_Library_OrderlyIndexCards", root, c + new Vector3(-2.45f, 0.54f, -3.18f), new Vector3(0.32f, 0.05f, 0.20f), Quaternion.identity, materials.SignPaint, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.library.index_cards");
                CreateReadableBookProp(root, "Past_Library_TargetBook_ForPickup", PastLibraryBookCueLocalPosition, Quaternion.Euler(0f, 12f, 0f), new Vector3(0.46f, 0.05f, 0.30f), materials.Book, materials.SignPaint, materials.PastFence, true, "Past.library.target_book");
                CreateRedCubeMarkerWithOutline("Past_Library_TargetBook_RedCubeMarker", root, PastLibraryBookCueLocalPosition + new Vector3(0f, 0.30f, 0f), PastLibraryTargetBookMarkerScale, Quaternion.Euler(12f, 18f, 0f), materials.RedMarker, materials.DoorwayDark, "Past.library.target_book_marker");
                CreatePastLibraryCleanReadingTable(root, "LeftFront", c + new Vector3(-2.88f, 0.32f, -0.92f), Quaternion.identity, wood, materials.Book, materials.SignPaint, trim, materials.Shadow);
                CreatePastLibraryCleanReadingTable(root, "CenterFront", c + new Vector3(0.00f, 0.32f, -0.92f), Quaternion.identity, wood, materials.Book, materials.SignPaint, trim, materials.Shadow);
                CreatePastLibraryCleanReadingTable(root, "RightFront", c + new Vector3(2.88f, 0.32f, -0.92f), Quaternion.identity, wood, materials.Book, materials.SignPaint, trim, materials.Shadow);
                CreatePastLibraryCleanReadingTable(root, "LeftRear", c + new Vector3(-2.88f, 0.32f, 1.96f), Quaternion.identity, wood, materials.Book, materials.SignPaint, trim, materials.Shadow);
                CreatePastLibraryCleanReadingTable(root, "CenterRear", c + new Vector3(0.00f, 0.32f, 1.96f), Quaternion.identity, wood, materials.Book, materials.SignPaint, trim, materials.Shadow);
                CreatePastLibraryCleanReadingTable(root, "RightRear", c + new Vector3(2.88f, 0.32f, 1.96f), Quaternion.identity, wood, materials.Book, materials.SignPaint, trim, materials.Shadow);

                var aria = new GameObject("Past_Library_AriaIdleAtTable");
                aria.transform.SetParent(root, false);
                aria.transform.localPosition = PastLibraryPersonCueLocalPosition;
                aria.AddComponent<FastVsPaperBillboard>();
                var ariaRenderer = CreateSpriteCardParts(
                    aria.transform,
                    "Aria",
                    1.18f,
                    SpriteStripMaterial("aria_v46_normal_loop_breath_sprite", AriaNormalLoopStripPath, Color.white, 4),
                    false,
                    materials.Label);
                var ariaAnimator = aria.AddComponent<FastVsSpriteStripLoopAnimator>();
                SerializedSet(ariaAnimator, "spriteRenderer", ariaRenderer);
                SerializedSet(ariaAnimator, "frameCount", 4);
                SerializedSet(ariaAnimator, "framesPerSecond", 2.2f);
                CreateRedCubeMarkerWithOutline("Past_Library_Aria_RedCubeMarker", root, PastLibraryPersonCueLocalPosition + new Vector3(0f, 1.32f, 0f), PastLibraryTargetBookMarkerScale, Quaternion.Euler(10f, -14f, 0f), materials.RedMarker, materials.DoorwayDark, "Past.library.aria_marker");
            }
            else
            {
                CreateCurrentLibrarySideBookshelfSilhouette(root, "Left", c + new Vector3(-4.78f, 0.18f, 0.60f), Quaternion.Euler(0f, 90f, 0f), wood);
                CreateCurrentLibrarySideBookshelfSilhouette(root, "Right", c + new Vector3(4.78f, 0.18f, 0.60f), Quaternion.Euler(0f, -90f, 0f), wood);
                CreateLandmarkCube("Current_Library_Ruin_ScatteredBoardPile", root, c + new Vector3(0.90f, 0.13f, -1.52f), new Vector3(0.86f, 0.08f, 0.22f), Quaternion.Euler(0f, -14f, 7f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.scattered_board_pile");
                CreateLandmarkCube("Current_Library_Ruin_ScatteredBoardPile_PlankA", root, c + new Vector3(0.52f, 0.15f, -1.78f), new Vector3(0.62f, 0.07f, 0.16f), Quaternion.Euler(0f, 22f, -5f), materials.CurrentFurniture, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.scattered_board_pile.plank_a");
                CreateLandmarkCube("Current_Library_Ruin_ScatteredBoardPile_PlankB", root, c + new Vector3(1.32f, 0.14f, -1.30f), new Vector3(0.74f, 0.07f, 0.14f), Quaternion.Euler(0f, -38f, 4f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.scattered_board_pile.plank_b");
                CreateLandmarkCube("Current_Library_Ruin_ScatteredBoardPile_DustPatch", root, c + new Vector3(0.96f, 0.045f, -1.55f), new Vector3(0.72f, 0.035f, 0.40f), Quaternion.Euler(0f, -12f, 0f), materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.scattered_board_pile.dust_patch");
                CreateLandmarkCube("Current_Library_Ruin_ToppledBookStack", root, c + new Vector3(3.12f, 0.16f, 0.52f), CurrentLibraryRuinBookPileScale, Quaternion.Euler(0f, 28f, -11f), materials.Book, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.toppled_book_stack");
                CreateLandmarkCube("Current_Library_Ruin_DustSheetNearEntry", root, c + new Vector3(-4.05f, 0.05f, -3.88f), new Vector3(0.82f, 0.045f, 0.34f), Quaternion.Euler(0f, -18f, 0f), materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.dust_sheet_near_entry");
                CreateLandmarkCube("Current_Library_Ruin_BrokenBackShelfBoard", root, c + new Vector3(-1.80f, 1.26f, 7.02f), new Vector3(2.10f, 0.10f, 0.16f), Quaternion.Euler(0f, 0f, -7f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.broken_back_shelf_board");
                CreateLandmarkCube("Current_Library_Ruin_FallenBookSpines", root, c + new Vector3(3.50f, 0.16f, -2.70f), new Vector3(0.96f, 0.16f, 0.32f), Quaternion.Euler(0f, 24f, 0f), materials.Book, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.fallen_book_spines");
                var windowCue = CreateFloorGlowCue("Current_Library_TimeWindowOpenCue_Book", root, new Vector3(PastLibraryBookCueLocalPosition.x, CurrentLibraryCueFloorY, PastLibraryBookCueLocalPosition.z), CurrentLibraryBookCueGlowScale, materials.RedLight, "Current.library.timewindow_book_cue");
                windowCue.SetActive(false);
                var ariaCue = CreateFloorGlowCue("Current_Library_TimeWindowOpenCue_Aria", root, new Vector3(PastLibraryPersonCueLocalPosition.x, CurrentLibraryCueFloorY, PastLibraryPersonCueLocalPosition.z), CurrentLibraryAriaCueGlowScale, materials.RedLight, "Current.library.timewindow_aria_cue");
                ariaCue.SetActive(false);
                CreateReadableBookProp(root, "Current_Library_RetoDeskBook_Initial", CurrentLibraryRetoDeskBookInitialLocalPosition, FaceTargetOnPlane(CurrentLibraryRetoDeskBookInitialLocalPosition, RetoLibraryDeskLocalPosition), new Vector3(0.42f, 0.05f, 0.26f), materials.Book, materials.SignPaint, materials.CurrentFence, true, "Current.library.reto_desk_book");
                var returnedBook = CreateReadableBookProp(root, "Current_Library_ReturnedBookOnDesk", CurrentLibraryReturnedBookLocalPosition, FaceTargetOnPlane(CurrentLibraryReturnedBookLocalPosition, RetoLibraryDeskLocalPosition), new Vector3(0.48f, 0.05f, 0.28f), materials.Book, materials.SignPaint, materials.RedLight, true, "Current.library.returned_book_on_desk");
                returnedBook.SetActive(false);
                CreateRetoAtLibraryDesk(root, materials);
            }

            CreateInvisibleColliderBox($"{prefix}_Library_InvisibleFrontDropGuard", root, c + new Vector3(0f, 0.75f, -7.85f), new Vector3(12.25f, 1.50f, 0.24f), $"{prefix}.library.front_drop_guard");
            CreateInvisibleColliderBox($"{prefix}_Library_InvisibleBackBoundary", root, c + new Vector3(0f, 0.75f, 7.85f), new Vector3(12.25f, 1.50f, 0.24f), $"{prefix}.library.back_boundary");
            CreateInvisibleColliderBox($"{prefix}_Library_InvisibleLeftBoundary", root, c + new Vector3(-5.95f, 0.75f, 0f), new Vector3(0.24f, 1.50f, 15.80f), $"{prefix}.library.left_boundary");
            CreateInvisibleColliderBox($"{prefix}_Library_InvisibleRightBoundary", root, c + new Vector3(5.95f, 0.75f, 0f), new Vector3(0.24f, 1.50f, 15.80f), $"{prefix}.library.right_boundary");
        }

        private static void CreatePastLibraryCleanReadingTable(Transform root, string id, Vector3 localPosition, Quaternion rotation, Material wood, Material book, Material page, Material trim, Material shadow)
        {
            CreateLibraryReadingTableAssembly($"Past_Library_ReadingTableClean_{id}", root, localPosition, rotation, PastLibraryReadingTableCleanScale, wood, trim, shadow, $"Past.library.reading_table_clean.{id}");
            CreateInvisibleColliderBox($"Past_Library_ReadingTableClean_{id}_NoStepCollider", root, localPosition + new Vector3(0f, 0.56f, 0f), PastLibraryReadingTableCleanColliderSize, $"Past.library.reading_table_clean.{id}.no_step");
            CreateReadableBookProp(root, $"Past_Library_ReadingTableClean_{id}_BookA", localPosition + new Vector3(-0.55f, 0.085f, -0.06f), Quaternion.Euler(0f, 9f, 0f), new Vector3(0.28f, 0.04f, 0.18f), book, page, wood, true, $"Past.library.reading_table_clean.{id}.book_a");
            CreateReadableBookProp(root, $"Past_Library_ReadingTableClean_{id}_BookB", localPosition + new Vector3(0.55f, 0.085f, 0.06f), Quaternion.Euler(0f, -12f, 0f), new Vector3(0.24f, 0.04f, 0.16f), book, page, wood, true, $"Past.library.reading_table_clean.{id}.book_b");
            CreateReadableBookProp(root, $"Past_Library_ReadingTableClean_{id}_BookC", localPosition + new Vector3(0.03f, 0.085f, 0.17f), Quaternion.Euler(0f, 4f, 0f), new Vector3(0.20f, 0.035f, 0.14f), book, page, wood, false, $"Past.library.reading_table_clean.{id}.book_c");
        }

        private static GameObject CreateLibraryReadingTableAssembly(string objectName, Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Material wood, Material trim, Material shadow, string landmarkId)
        {
            var tableRoot = new GameObject(objectName);
            tableRoot.transform.SetParent(parent, false);
            tableRoot.transform.localPosition = localPosition;
            tableRoot.transform.localRotation = localRotation;
            tableRoot.transform.localScale = localScale;

            var collider = tableRoot.AddComponent<BoxCollider>();
            collider.size = Vector3.one;

            var landmark = tableRoot.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", landmarkId);
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", true);

            CreateLibraryReadingTablePart(
                $"{objectName}_TopSlab",
                tableRoot.transform,
                localScale,
                new Vector3(0f, localScale.y * 0.33f, 0f),
                new Vector3(localScale.x - 0.12f, Mathf.Max(0.04f, localScale.y * 0.28f), localScale.z - 0.08f),
                wood);

            CreateLibraryReadingTablePart(
                $"{objectName}_FrontShadowLine",
                tableRoot.transform,
                localScale,
                new Vector3(0f, localScale.y * 0.12f, -localScale.z * 0.5f + 0.02f),
                new Vector3(localScale.x - 0.18f, Mathf.Max(0.018f, localScale.y * 0.12f), 0.03f),
                shadow);

            CreateLibraryReadingTablePart(
                $"{objectName}_RearDepthLine",
                tableRoot.transform,
                localScale,
                new Vector3(0f, localScale.y * 0.16f, localScale.z * 0.5f - 0.02f),
                new Vector3(localScale.x - 0.20f, Mathf.Max(0.016f, localScale.y * 0.10f), 0.03f),
                trim);

            var legHeight = 0.30f;
            var legThickness = Mathf.Max(0.045f, Mathf.Min(localScale.x, localScale.z) * 0.085f);
            var legInsetX = Mathf.Max(0.14f, localScale.x * 0.18f);
            var legInsetZ = Mathf.Max(0.10f, localScale.z * 0.16f);
            var legY = -0.27f + legHeight * 0.5f;

            CreateLibraryReadingTablePart($"{objectName}_LegFrontLeft", tableRoot.transform, localScale, new Vector3(-localScale.x * 0.5f + legInsetX, legY, -localScale.z * 0.5f + legInsetZ), new Vector3(legThickness, legHeight, legThickness), wood);
            CreateLibraryReadingTablePart($"{objectName}_LegFrontRight", tableRoot.transform, localScale, new Vector3(localScale.x * 0.5f - legInsetX, legY, -localScale.z * 0.5f + legInsetZ), new Vector3(legThickness, legHeight, legThickness), wood);
            CreateLibraryReadingTablePart($"{objectName}_LegBackLeft", tableRoot.transform, localScale, new Vector3(-localScale.x * 0.5f + legInsetX, legY, localScale.z * 0.5f - legInsetZ), new Vector3(legThickness, legHeight, legThickness), wood);
            CreateLibraryReadingTablePart($"{objectName}_LegBackRight", tableRoot.transform, localScale, new Vector3(localScale.x * 0.5f - legInsetX, legY, localScale.z * 0.5f - legInsetZ), new Vector3(legThickness, legHeight, legThickness), wood);

            return tableRoot;
        }

        private static GameObject CreateLibraryReadingTablePart(string objectName, Transform parent, Vector3 parentScale, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.name = objectName;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = new Vector3(
                localPosition.x / parentScale.x,
                localPosition.y / parentScale.y,
                localPosition.z / parentScale.z);
            part.transform.localScale = new Vector3(
                localScale.x / parentScale.x,
                localScale.y / parentScale.y,
                localScale.z / parentScale.z);
            part.GetComponent<Renderer>().sharedMaterial = material;
            var partCollider = part.GetComponent<Collider>();
            if (partCollider != null)
            {
                UnityEngine.Object.DestroyImmediate(partCollider);
            }

            return part;
        }

        private static GameObject CreateFloorGlowCue(string objectName, Transform root, Vector3 localPosition, Vector3 localScale, Material glow, string landmarkId)
        {
            var cue = CreateGlowDisc(objectName, root, localPosition, localScale, glow, true);
            var landmark = cue.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", landmarkId);
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", true);
            return cue;
        }

        private static GameObject CreateBookRowTexturePanel(string objectName, Transform root, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material bookRows, string landmarkId)
        {
            var textureScale = new Vector2(Mathf.Max(1f, localScale.x / 1.14f), Mathf.Max(1f, localScale.y / 0.88f));
            return CreateLandmarkCube(objectName, root, localPosition, localScale, localRotation, BookshelfFrontMaterial(objectName, textureScale), false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, landmarkId);
        }

        private static GameObject CreateRedCubeMarkerWithOutline(string objectName, Transform root, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material redFill, Material outline, string landmarkId)
        {
            var marker = new GameObject(objectName);
            marker.transform.SetParent(root, false);
            marker.transform.localPosition = localPosition;
            marker.transform.localRotation = localRotation;
            marker.transform.localScale = Vector3.one;
            marker.AddComponent<FastVsMapMoveGlowPulse>();

            var landmark = marker.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", landmarkId);
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", true);

            CreateLandmarkCube($"{objectName}_Fill", marker.transform, Vector3.zero, localScale, Quaternion.identity, redFill, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.fill");

            var halfX = localScale.x * 0.5f;
            var halfY = localScale.y * 0.5f;
            var halfZ = localScale.z * 0.5f;
            var edge = Mathf.Max(0.005f, Mathf.Min(localScale.x, Mathf.Min(localScale.y, localScale.z)) * 0.035f);
            var xLength = localScale.x + edge * 2f;
            var yLength = localScale.y + edge * 2f;
            var zLength = localScale.z + edge * 2f;
            var xEdgeScale = new Vector3(xLength, edge, edge);
            var yEdgeScale = new Vector3(edge, yLength, edge);
            var zEdgeScale = new Vector3(edge, edge, zLength);

            CreateMarkerEdge(objectName, "EdgeX_TopFront", marker.transform, new Vector3(0f, halfY + edge * 0.5f, -halfZ - edge * 0.5f), xEdgeScale, outline, $"{landmarkId}.edge.x.top_front");
            CreateMarkerEdge(objectName, "EdgeX_TopBack", marker.transform, new Vector3(0f, halfY + edge * 0.5f, halfZ + edge * 0.5f), xEdgeScale, outline, $"{landmarkId}.edge.x.top_back");
            CreateMarkerEdge(objectName, "EdgeX_BottomFront", marker.transform, new Vector3(0f, -halfY - edge * 0.5f, -halfZ - edge * 0.5f), xEdgeScale, outline, $"{landmarkId}.edge.x.bottom_front");
            CreateMarkerEdge(objectName, "EdgeX_BottomBack", marker.transform, new Vector3(0f, -halfY - edge * 0.5f, halfZ + edge * 0.5f), xEdgeScale, outline, $"{landmarkId}.edge.x.bottom_back");
            CreateMarkerEdge(objectName, "EdgeY_LeftFront", marker.transform, new Vector3(-halfX - edge * 0.5f, 0f, -halfZ - edge * 0.5f), yEdgeScale, outline, $"{landmarkId}.edge.y.left_front");
            CreateMarkerEdge(objectName, "EdgeY_LeftBack", marker.transform, new Vector3(-halfX - edge * 0.5f, 0f, halfZ + edge * 0.5f), yEdgeScale, outline, $"{landmarkId}.edge.y.left_back");
            CreateMarkerEdge(objectName, "EdgeY_RightFront", marker.transform, new Vector3(halfX + edge * 0.5f, 0f, -halfZ - edge * 0.5f), yEdgeScale, outline, $"{landmarkId}.edge.y.right_front");
            CreateMarkerEdge(objectName, "EdgeY_RightBack", marker.transform, new Vector3(halfX + edge * 0.5f, 0f, halfZ + edge * 0.5f), yEdgeScale, outline, $"{landmarkId}.edge.y.right_back");
            CreateMarkerEdge(objectName, "EdgeZ_LeftTop", marker.transform, new Vector3(-halfX - edge * 0.5f, halfY + edge * 0.5f, 0f), zEdgeScale, outline, $"{landmarkId}.edge.z.left_top");
            CreateMarkerEdge(objectName, "EdgeZ_LeftBottom", marker.transform, new Vector3(-halfX - edge * 0.5f, -halfY - edge * 0.5f, 0f), zEdgeScale, outline, $"{landmarkId}.edge.z.left_bottom");
            CreateMarkerEdge(objectName, "EdgeZ_RightTop", marker.transform, new Vector3(halfX + edge * 0.5f, halfY + edge * 0.5f, 0f), zEdgeScale, outline, $"{landmarkId}.edge.z.right_top");
            CreateMarkerEdge(objectName, "EdgeZ_RightBottom", marker.transform, new Vector3(halfX + edge * 0.5f, -halfY - edge * 0.5f, 0f), zEdgeScale, outline, $"{landmarkId}.edge.z.right_bottom");
            CreateMarkerBangGlyph(objectName, marker.transform, halfY, halfZ, edge, outline, landmarkId);
            return marker;
        }

        private static GameObject CreateMarkerEdge(string objectName, string edgeName, Transform parent, Vector3 localPosition, Vector3 localScale, Material outline, string landmarkId)
        {
            return CreateLandmarkCube($"{objectName}_{edgeName}", parent, localPosition, localScale, Quaternion.identity, outline, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, landmarkId);
        }

        private static void CreateMarkerBangGlyph(string objectName, Transform parent, float halfY, float halfZ, float edge, Material outline, string landmarkId)
        {
            var z = -halfZ - edge * 2.6f;
            var width = Mathf.Max(0.015f, edge * 2.6f);
            var depth = Mathf.Max(0.006f, edge * 1.1f);
            CreateMarkerEdge(
                objectName,
                "BangFrontStem",
                parent,
                new Vector3(0f, halfY * 0.10f, z),
                new Vector3(width, halfY * 1.05f, depth),
                outline,
                $"{landmarkId}.bang.stem");
            CreateMarkerEdge(
                objectName,
                "BangFrontDot",
                parent,
                new Vector3(0f, -halfY * 0.62f, z),
                new Vector3(width * 1.25f, width * 1.25f, depth),
                outline,
                $"{landmarkId}.bang.dot");
        }

        private static void CreateCurrentLibrarySideBookshelfSilhouette(Transform root, string side, Vector3 localPosition, Quaternion localRotation, Material frame)
        {
            var shelfRoot = new GameObject($"Current_Library_{side}SideBookshelf");
            shelfRoot.transform.SetParent(root, false);
            shelfRoot.transform.localPosition = localPosition;
            shelfRoot.transform.localRotation = localRotation;

            CreateLibrarySideBookshelfFrame(shelfRoot.transform, "Current", side, frame);
        }

        private static GameObject CreateReadableBookProp(Transform root, string objectName, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Material cover, Material pages, Material spine, bool openPages, string landmarkId)
        {
            var book = new GameObject(objectName);
            book.transform.SetParent(root, false);
            book.transform.localPosition = localPosition;
            book.transform.localRotation = localRotation;
            book.transform.localScale = Vector3.one;

            CreateLandmarkCube($"{objectName}_BackCover", book.transform, new Vector3(-localScale.x * 0.02f, 0.014f, 0f), new Vector3(localScale.x * 0.94f, 0.020f, localScale.z * 0.92f), Quaternion.identity, cover, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.back_cover");
            CreateLandmarkCube($"{objectName}_Cover", book.transform, new Vector3(0f, 0.018f, 0f), new Vector3(localScale.x, 0.028f, localScale.z), Quaternion.identity, cover, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.cover");
            CreateLandmarkCube($"{objectName}_Spine", book.transform, new Vector3(-localScale.x * 0.32f, 0.025f, 0f), new Vector3(localScale.x * 0.18f, 0.040f, localScale.z * 0.94f), Quaternion.identity, spine, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.spine");
            CreateLandmarkCube($"{objectName}_PageEdge", book.transform, new Vector3(localScale.x * 0.27f, 0.027f, 0f), new Vector3(localScale.x * 0.12f, 0.036f, localScale.z * 0.90f), Quaternion.identity, pages, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.page_edge");
            CreateLandmarkCube($"{objectName}_PageBlock", book.transform, new Vector3(localScale.x * 0.03f, 0.031f, 0f), new Vector3(localScale.x * 0.42f, 0.018f, localScale.z * 0.78f), Quaternion.identity, pages, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.page_block");
            CreateLandmarkCube($"{objectName}_TopEdge", book.transform, new Vector3(0f, 0.045f, -localScale.z * 0.02f), new Vector3(localScale.x * 0.54f, 0.008f, localScale.z * 0.16f), Quaternion.identity, pages, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.top_edge");

            if (openPages)
            {
                CreateLandmarkCube($"{objectName}_OpenPageLeft", book.transform, new Vector3(-localScale.x * 0.04f, 0.039f, -localScale.z * 0.08f), new Vector3(localScale.x * 0.30f, 0.012f, localScale.z * 0.66f), Quaternion.Euler(0f, 0f, -10f), pages, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.open_page_left");
                CreateLandmarkCube($"{objectName}_OpenPageRight", book.transform, new Vector3(localScale.x * 0.18f, 0.041f, localScale.z * 0.02f), new Vector3(localScale.x * 0.26f, 0.012f, localScale.z * 0.62f), Quaternion.Euler(0f, 0f, 10f), pages, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.open_page_right");
            }

            return book;
        }

        private static void CreateReadableShelfBookRun(Transform root, string id, Vector3 center, int bookCount, float bookSpan, float bookWidth, float bookDepth, Material frame, Material cover, Material pages, Material spine, float baseHeight, float heightVariance, float topLip)
        {
            var startX = center.x - ((bookCount - 1) * bookSpan * 0.5f);
            for (var i = 0; i < bookCount; i++)
            {
                var x = startX + i * bookSpan;
                var height = baseHeight + ((i % 3) * heightVariance * 0.5f);
                var thickness = bookWidth + ((i % 2) * 0.012f);
                var depth = bookDepth + ((i % 4) * 0.01f);
                var book = new GameObject($"{id}_Book{i}");
                book.transform.SetParent(root, false);
                book.transform.localPosition = new Vector3(x, center.y, center.z);
                book.transform.localRotation = Quaternion.Euler(0f, (i % 2 == 0 ? -3f : 4f), 0f);
                book.transform.localScale = Vector3.one;
                CreateLandmarkCube($"{id}_Book{i}_BackCover", book.transform, new Vector3(-thickness * 0.02f, height * 0.48f, 0f), new Vector3(thickness * 0.92f, height * 0.92f, depth * 0.90f), Quaternion.identity, cover, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{id}.book.{i}.back_cover");
                CreateLandmarkCube($"{id}_Book{i}_Cover", book.transform, new Vector3(0f, height * 0.50f, 0f), new Vector3(thickness, height, depth), Quaternion.identity, cover, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{id}.book.{i}.cover");
                CreateLandmarkCube($"{id}_Book{i}_Spine", book.transform, new Vector3(-thickness * 0.34f, height * 0.50f, 0f), new Vector3(thickness * 0.18f, height * 0.96f, depth * 0.94f), Quaternion.identity, spine, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{id}.book.{i}.spine");
                CreateLandmarkCube($"{id}_Book{i}_EdgeBand", book.transform, new Vector3(thickness * 0.18f, height * 0.50f, 0f), new Vector3(thickness * 0.10f, height * 0.90f, depth * 0.84f), Quaternion.identity, spine, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{id}.book.{i}.edge_band");
                CreateLandmarkCube($"{id}_Book{i}_TopLip", book.transform, new Vector3(0f, height + topLip, 0f), new Vector3(thickness * 0.30f, 0.018f, depth * 0.84f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{id}.book.{i}.top_lip");
            }
        }

        private static void CreateLibrarySideBookshelfFrame(Transform shelfRoot, string prefix, string side, Material frame)
        {
            var sideToken = side.ToLowerInvariant();
            var halfRun = LibrarySideShelfRunLength * 0.5f;
            var postX = halfRun - 0.07f;
            CreateLandmarkCube($"{shelfRoot.name}_BackPanel", shelfRoot, new Vector3(0f, LibrarySideShelfBackPanelCenterY, 0.08f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfBackPanelHeight, 1.16f), Quaternion.identity, frame, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.back_panel");
            CreateLandmarkCube($"{shelfRoot.name}_LeftPost", shelfRoot, new Vector3(-postX, LibrarySideShelfPostCenterY, 0.02f), new Vector3(LibrarySideShelfPostThickness, LibrarySideShelfPostHeight, LibrarySideShelfPostThickness), Quaternion.identity, frame, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.left_post");
            CreateLandmarkCube($"{shelfRoot.name}_RightPost", shelfRoot, new Vector3(postX, LibrarySideShelfPostCenterY, 0.02f), new Vector3(LibrarySideShelfPostThickness, LibrarySideShelfPostHeight, LibrarySideShelfPostThickness), Quaternion.identity, frame, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.right_post");
            CreateLandmarkCube($"{shelfRoot.name}_TopCap", shelfRoot, new Vector3(0f, LibrarySideShelfTopCapCenterY, 0.02f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfCapThickness, 0.16f), Quaternion.identity, frame, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.top_cap");
            CreateLandmarkCube($"{shelfRoot.name}_BottomBase", shelfRoot, new Vector3(0f, 0.06f, 0.02f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfCapThickness, 0.16f), Quaternion.identity, frame, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.bottom_base");

            for (var row = 0; row < 3; row++)
            {
                var rowY = LibrarySideShelfBoardFirstY + row * LibrarySideShelfBoardStepY;
                CreateLandmarkCube($"{shelfRoot.name}_ShelfBoard_{row}", shelfRoot, new Vector3(0f, rowY, 0.00f), new Vector3(LibrarySideShelfRunLength - 0.10f, LibrarySideShelfBoardThickness, LibrarySideShelfBoardDepth), Quaternion.identity, frame, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.board.{row}");
            }
        }

        private static void CreatePastLibrarySideBookshelf(Transform root, string side, Vector3 localPosition, Quaternion localRotation, Material frame, Material cover, Material pages, Material spine)
        {
            var shelfRoot = new GameObject($"Past_Library_{side}SideBookshelf");
            shelfRoot.transform.SetParent(root, false);
            shelfRoot.transform.localPosition = localPosition;
            shelfRoot.transform.localRotation = localRotation;

            CreateLibrarySideBookshelfFrame(shelfRoot.transform, "Past", side, frame);

            CreateBookRowTexturePanel(
                $"{shelfRoot.name}_BookshelfFrontTexturePanel",
                shelfRoot.transform,
                new Vector3(0f, LibrarySideShelfTexturePanelCenterY, 0.70f),
                new Vector3(LibrarySideShelfRunLength - 0.38f, LibrarySideShelfTexturePanelHeight, 0.040f),
                Quaternion.identity,
                cover,
                $"Past.library.{side.ToLowerInvariant()}.shelf.front_texture");
        }

        private static void CreateCentralPlazaLibraryFacadeDoor(Transform root, string prefix, Vector3 center, Material frame, Material panel, Material handle)
        {
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorFrameTop", root, center + new Vector3(0f, 0.64f, 0f), new Vector3(1.42f, 0.10f, 0.12f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.frame_top");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorFrameLeft", root, center + new Vector3(-0.70f, -0.05f, 0f), new Vector3(0.12f, 1.48f, 0.12f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.frame_left");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorFrameRight", root, center + new Vector3(0.70f, -0.05f, 0f), new Vector3(0.12f, 1.48f, 0.12f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.frame_right");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorPanelsLeft", root, center + new Vector3(-0.24f, -0.05f, 0.01f), new Vector3(0.44f, 1.34f, 0.08f), Quaternion.identity, panel, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.panel_left");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorPanelsRight", root, center + new Vector3(0.24f, -0.05f, 0.01f), new Vector3(0.44f, 1.34f, 0.08f), Quaternion.identity, panel, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.panel_right");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorTopLip", root, center + new Vector3(0f, 0.74f, 0.012f), new Vector3(1.46f, 0.04f, 0.06f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.top_lip");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorCenterPlank", root, center + new Vector3(0f, -0.02f, 0.015f), new Vector3(0.12f, 1.40f, 0.09f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.center_plank");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorHandle", root, center + new Vector3(0.44f, 0.05f, 0.04f), new Vector3(0.08f, 0.16f, 0.05f), Quaternion.identity, handle, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.handle");
        }

        private static void CreateCentralPlazaLibraryFacadeWindow(Transform root, string prefix, string side, Vector3 center, Material frame, Material pane)
        {
            var sideToken = side.ToLowerInvariant();
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}FrameTop", root, center + new Vector3(0f, 0.35f, 0f), new Vector3(0.86f, 0.08f, 0.10f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.frame_top");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}FrameBottom", root, center + new Vector3(0f, -0.35f, 0f), new Vector3(0.90f, 0.08f, 0.10f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.frame_bottom");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}FrameLeft", root, center + new Vector3(-0.43f, 0f, 0f), new Vector3(0.08f, 0.68f, 0.10f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.frame_left");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}FrameRight", root, center + new Vector3(0.43f, 0f, 0f), new Vector3(0.08f, 0.68f, 0.10f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.frame_right");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}TopLip", root, center + new Vector3(0f, 0.43f, 0.012f), new Vector3(0.90f, 0.04f, 0.06f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.top_lip");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}BottomLip", root, center + new Vector3(0f, -0.43f, 0.012f), new Vector3(0.92f, 0.04f, 0.06f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.bottom_lip");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}MullionVertical", root, center + new Vector3(0f, 0f, 0.01f), new Vector3(0.08f, 0.62f, 0.08f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.mullion_vertical");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}MullionHorizontal", root, center + new Vector3(0f, -0.01f, 0.01f), new Vector3(0.76f, 0.08f, 0.08f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.mullion_horizontal");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}PaneUpperLeft", root, center + new Vector3(-0.19f, 0.17f, 0.02f), new Vector3(0.30f, 0.24f, 0.05f), Quaternion.identity, pane, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.pane_upper_left");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}PaneUpperRight", root, center + new Vector3(0.19f, 0.17f, 0.02f), new Vector3(0.30f, 0.24f, 0.05f), Quaternion.identity, pane, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.pane_upper_right");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}PaneLowerLeft", root, center + new Vector3(-0.19f, -0.17f, 0.02f), new Vector3(0.30f, 0.24f, 0.05f), Quaternion.identity, pane, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.pane_lower_left");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryWindow{side}PaneLowerRight", root, center + new Vector3(0.19f, -0.17f, 0.02f), new Vector3(0.30f, 0.24f, 0.05f), Quaternion.identity, pane, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_window.{sideToken}.pane_lower_right");
        }

        private static void CreateNorthEastRoad(Transform root, string prefix, bool past, Materials materials)
        {
            var c = HouseExteriorCenter;
            var path = past ? materials.PastPath : materials.CurrentPath;
            var grass = past ? materials.PastGrass : materials.CurrentGrass;
            CreateLandmarkCube($"{prefix}_HouseExterior_NorthEastRoadShoulderA", root, c + new Vector3(4.10f, 0.0f, 1.15f), new Vector3(7.2f, 0.06f, 4.8f), Quaternion.Euler(0f, -39f, 0f), grass, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_exterior.ne_road.shoulder_a");
            CreateLandmarkCube($"{prefix}_HouseExterior_NorthEastRoadShoulderB", root, c + new Vector3(8.25f, 0.0f, 3.95f), new Vector3(7.0f, 0.06f, 4.8f), Quaternion.Euler(0f, -43f, 0f), grass, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_exterior.ne_road.shoulder_b");
            CreateLandmarkCube($"{prefix}_HouseExterior_NorthEastRoadShoulderC_PlazaApproach", root, c + new Vector3(10.85f, 0.0f, 5.95f), new Vector3(3.8f, 0.06f, 3.2f), Quaternion.Euler(0f, -45f, 0f), grass, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_exterior.ne_road.shoulder_c");
            CreatePathBetween(root, c + new Vector3(1.70f, 0.06f, -2.05f), c + new Vector3(5.65f, 0.06f, 1.72f), 1.55f, path, $"{prefix}_HouseExterior_NorthEastRoad_FromYard", true);
            CreatePathBetween(root, c + new Vector3(5.65f, 0.06f, 1.72f), c + new Vector3(8.55f, 0.06f, 3.78f), 1.54f, path, $"{prefix}_HouseExterior_NorthEastRoad_ToPlazaRoute", true);
            CreatePathBetween(root, c + new Vector3(8.55f, 0.06f, 3.78f), c + new Vector3(10.95f, 0.06f, 5.90f), 1.48f, path, $"{prefix}_HouseExterior_NorthEastRoad_PlazaApproach", true);
            CreateLandmarkCube($"{prefix}_HouseExterior_NorthEastRouteMarker", root, c + new Vector3(10.95f, 0.58f, 5.90f), new Vector3(0.20f, 1.05f, 0.20f), Quaternion.identity, past ? materials.PastFence : materials.CurrentFence, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.ne_road.marker_post");
            CreateLandmarkCube($"{prefix}_HouseExterior_NorthEastRouteMarkerBoard", root, c + new Vector3(11.20f, 1.10f, 6.08f), new Vector3(0.92f, 0.34f, 0.08f), Quaternion.Euler(0f, -42f, 0f), materials.SignPaint, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.ne_road.marker_board");
        }

        private static void CreateExteriorHouseArchitecture(Transform root, string prefix, bool past, Materials materials)
        {
            var c = HouseExteriorCenter;
            var wall = past ? materials.PastExteriorWall : materials.CurrentExteriorWall;
            var roof = past ? materials.PastRoof : materials.CurrentRoof;
            var wood = past ? materials.PastFurniture : materials.CurrentFurniture;
            var stone = past ? materials.PastStone : materials.CurrentStone;
            var trim = past ? materials.PastFence : materials.CurrentFence;

            CreateLandmarkCube($"{prefix}_HouseExterior_FoundationStoneLine", root, c + new Vector3(-1.05f, 0.23f, -1.58f), new Vector3(5.00f, 0.24f, 0.44f), Quaternion.identity, stone, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.foundation.front");
            CreateLandmarkCube($"{prefix}_HouseExterior_LeftSideWallDepth", root, c + new Vector3(-3.18f, 1.03f, 0.00f), new Vector3(0.32f, 2.02f, 3.10f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.side.left_depth");
            CreateLandmarkCube($"{prefix}_HouseExterior_RightSideWallDepth", root, c + new Vector3(1.08f, 1.03f, 0.00f), new Vector3(0.32f, 2.02f, 3.10f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.side.right_depth");
            CreateLandmarkCube($"{prefix}_HouseExterior_BackWallHint", root, c + new Vector3(-1.05f, 1.00f, 1.04f), new Vector3(4.55f, 1.84f, 0.26f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.back_wall_hint");

            CreateLandmarkCube($"{prefix}_HouseExterior_RoofLeftSlope", root, c + new Vector3(-2.20f, 2.43f, 0.45f), new Vector3(3.45f, 0.34f, 3.15f), Quaternion.Euler(0f, 0f, -11f), roof, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.roof.left_slope");
            CreateLandmarkCube($"{prefix}_HouseExterior_RoofRightSlope", root, c + new Vector3(0.10f, 2.43f, 0.45f), new Vector3(3.45f, 0.34f, 3.15f), Quaternion.Euler(0f, 0f, 11f), roof, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.roof.right_slope");
            CreateLandmarkCube($"{prefix}_HouseExterior_RoofFrontEave", root, c + new Vector3(-1.05f, 2.15f, -1.90f), new Vector3(5.42f, 0.22f, 0.32f), Quaternion.identity, roof, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.roof.front_eave");
            CreateLandmarkCube($"{prefix}_HouseExterior_RoofBackEave", root, c + new Vector3(-1.05f, 2.09f, 1.20f), new Vector3(5.08f, 0.20f, 0.30f), Quaternion.identity, roof, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.house_exterior.roof.back_eave");
            CreateLandmarkCube($"{prefix}_HouseExterior_RoofRidgeHighlightBand", root, c + new Vector3(-1.05f, 2.58f, -1.34f), new Vector3(4.92f, 0.06f, 0.10f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.roof.ridge_highlight");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeTopTrimBand", root, c + new Vector3(-1.05f, 1.94f, -1.42f), new Vector3(4.70f, 0.06f, 0.08f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade.top_trim");
            CreateLandmarkCube($"{prefix}_HouseExterior_Chimney", root, c + new Vector3(0.62f, 3.03f, 0.10f), new Vector3(0.42f, 0.90f, 0.42f), Quaternion.identity, stone, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.chimney");
            CreateLandmarkCube($"{prefix}_HouseExterior_ChimneyCap", root, c + new Vector3(0.62f, 3.53f, 0.10f), new Vector3(0.62f, 0.16f, 0.56f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.chimney_cap");

            CreateLandmarkCube($"{prefix}_HouseExterior_DoorFrameTop", root, c + new Vector3(-1.05f, 1.56f, -1.31f), new Vector3(1.14f, 0.16f, 0.12f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.door_frame.top");
            CreateLandmarkCube($"{prefix}_HouseExterior_DoorFrameLeft", root, c + new Vector3(-1.55f, 0.86f, -1.30f), new Vector3(0.14f, 1.40f, 0.12f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.door_frame.left");
            CreateLandmarkCube($"{prefix}_HouseExterior_DoorFrameRight", root, c + new Vector3(-0.55f, 0.86f, -1.30f), new Vector3(0.14f, 1.40f, 0.12f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.door_frame.right");
            CreateLandmarkCube($"{prefix}_HouseExterior_PorchDeck", root, c + new Vector3(-1.05f, 0.20f, -1.86f), new Vector3(2.12f, 0.16f, 1.18f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_exterior.porch_deck");
            CreateLandmarkCube($"{prefix}_HouseExterior_PorchStepLower", root, c + new Vector3(-1.05f, 0.10f, -2.50f), new Vector3(2.42f, 0.12f, 0.42f), Quaternion.identity, stone, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_exterior.porch_step_lower");
        }

        private static void CreateExteriorDetails(Transform root, string prefix, bool past, Materials materials)
        {
            var c = HouseExteriorCenter;
            var wood = past ? materials.PastFurniture : materials.CurrentFurniture;
            var fence = past ? materials.PastFence : materials.CurrentFence;
            var plant = past ? materials.Leaf : materials.CurrentLeaf;

            CreateLandmarkCube($"{prefix}_HouseExterior_UnderEaveShadowBand", root, c + new Vector3(-1.05f, 1.98f, -1.20f), new Vector3(4.82f, 0.12f, 0.16f), Quaternion.identity, materials.Shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.eave_shadow");
            CreateLandmarkCube($"{prefix}_HouseExterior_RoofRidgePixelCap", root, c + new Vector3(-1.05f, 2.52f, -1.36f), new Vector3(5.38f, 0.12f, 0.18f), Quaternion.Euler(8f, 0f, 0f), past ? materials.PastRoof : materials.CurrentRoof, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.roof_ridge");
            CreateLandmarkCube($"{prefix}_HouseExterior_DoorStepStone", root, c + new Vector3(-1.05f, 0.13f, -2.22f), new Vector3(1.42f, 0.18f, 0.58f), Quaternion.identity, past ? materials.PastStone : materials.CurrentStone, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.house_exterior.door_step");
            CreateLandmarkCube($"{prefix}_HouseExterior_PorchLeftPost", root, c + new Vector3(-1.85f, 0.86f, -1.78f), new Vector3(0.14f, 1.52f, 0.14f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.porch_left");
            CreateLandmarkCube($"{prefix}_HouseExterior_PorchRightPost", root, c + new Vector3(-0.25f, 0.86f, -1.78f), new Vector3(0.14f, 1.52f, 0.14f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.porch_right");
            CreateLandmarkCube($"{prefix}_HouseExterior_PorchDeckFrontLip", root, c + new Vector3(-1.05f, 0.14f, -1.30f), new Vector3(2.04f, 0.05f, 0.08f), Quaternion.identity, materials.Shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.porch_front_lip");
            CreateLandmarkCube($"{prefix}_HouseExterior_PorchStepUpperLip", root, c + new Vector3(-1.05f, 0.21f, -2.03f), new Vector3(2.18f, 0.05f, 0.08f), Quaternion.identity, fence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.porch_step_upper_lip");
            CreateLandmarkCube($"{prefix}_HouseExterior_HouseCastShadow", root, c + new Vector3(0.25f, 0.014f, -0.52f), new Vector3(5.85f, 0.03f, 1.70f), Quaternion.Euler(0f, -5f, 0f), materials.Shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.cast_shadow");

            CreateWindowTrim(root, prefix, "left", c + new Vector3(-2.38f, 1.12f, -1.315f), wood);
            CreateWindowTrim(root, prefix, "right", c + new Vector3(0.22f, 1.12f, -1.315f), wood);

            CreateLandmarkCube($"{prefix}_HouseExterior_FlowerBedBox", root, c + new Vector3(0.92f, 0.15f, 0.78f), new Vector3(1.60f, 0.22f, 0.44f), Quaternion.Euler(0f, -5f, 0f), fence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.flower_bed_box");
            CreateFlowerPatch(root, prefix, c + new Vector3(0.48f, 0.34f, 0.77f), plant, materials.FlowerRed, materials.FlowerYellow);
            CreateFlowerPatch(root, prefix, c + new Vector3(1.28f, 0.34f, 0.79f), plant, materials.FlowerBlue, materials.FlowerYellow);

            CreateLandmarkCube($"{prefix}_HouseExterior_WaterBarrel", root, c + new Vector3(-3.35f, 0.43f, -0.36f), new Vector3(0.72f, 0.86f, 0.72f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.water_barrel");
            CreateLandmarkCube($"{prefix}_HouseExterior_BarrelWaterTop", root, c + new Vector3(-3.35f, 0.88f, -0.36f), new Vector3(0.62f, 0.04f, 0.62f), Quaternion.identity, materials.Water, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.water_top");
            CreateLandmarkCube($"{prefix}_HouseExterior_WoodPileBase", root, c + new Vector3(2.02f, 0.30f, 0.35f), new Vector3(1.35f, 0.28f, 0.36f), Quaternion.Euler(0f, 11f, 0f), wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.woodpile_base");
            CreateLandmarkCube($"{prefix}_HouseExterior_WoodPileUpper", root, c + new Vector3(2.08f, 0.58f, 0.30f), new Vector3(0.95f, 0.22f, 0.34f), Quaternion.Euler(0f, -7f, 0f), wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.woodpile_upper");

            CreateLandmarkCube($"{prefix}_HouseExterior_ClotheslinePostLeft", root, c + new Vector3(-3.95f, 0.88f, 2.62f), new Vector3(0.12f, 1.55f, 0.12f), Quaternion.identity, fence, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.clothesline_left");
            CreateLandmarkCube($"{prefix}_HouseExterior_ClotheslinePostRight", root, c + new Vector3(-2.30f, 0.88f, 2.78f), new Vector3(0.12f, 1.55f, 0.12f), Quaternion.identity, fence, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.clothesline_right");
            CreateLandmarkCube($"{prefix}_HouseExterior_ClotheslineRope", root, c + new Vector3(-3.12f, 1.54f, 2.70f), new Vector3(1.70f, 0.04f, 0.05f), Quaternion.Euler(0f, -5f, 0f), materials.Rope, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.clothesline_rope");
            CreateLandmarkCube($"{prefix}_HouseExterior_ClothSheetA", root, c + new Vector3(-3.55f, 1.23f, 2.65f), new Vector3(0.42f, 0.58f, 0.04f), Quaternion.Euler(0f, -5f, 0f), past ? materials.LaundryBright : materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.cloth_a");
            CreateLandmarkCube($"{prefix}_HouseExterior_ClothSheetB", root, c + new Vector3(-2.80f, 1.19f, 2.72f), new Vector3(0.36f, 0.48f, 0.04f), Quaternion.Euler(0f, -5f, 0f), past ? materials.LaundryAccent : materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.cloth_b");

            CreateLandmarkCube($"{prefix}_HouseExterior_WaySignPost", root, c + new Vector3(2.72f, 0.70f, -2.36f), new Vector3(0.12f, 1.18f, 0.12f), Quaternion.identity, fence, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.sign_post");
            CreateLandmarkCube($"{prefix}_HouseExterior_WaySignBoard", root, c + new Vector3(2.72f, 1.25f, -2.40f), new Vector3(0.82f, 0.32f, 0.08f), Quaternion.Euler(0f, -18f, 0f), materials.SignPaint, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.sign_board");

            CreateGrassTuft(root, prefix, c + new Vector3(-4.30f, 0.20f, -2.65f), plant, 0);
            CreateGrassTuft(root, prefix, c + new Vector3(-3.72f, 0.20f, 3.55f), plant, 1);
            CreateGrassTuft(root, prefix, c + new Vector3(4.18f, 0.20f, 1.55f), plant, 2);
            CreateGrassTuft(root, prefix, c + new Vector3(3.70f, 0.20f, -2.20f), plant, 3);
            CreateInvisibleColliderBox(
                $"{prefix}_HouseExterior_InvisibleFrontDropGuard",
                root,
                c + new Vector3(0f, 0.75f, -5.34f),
                new Vector3(13.85f, 1.50f, 0.24f),
                $"{prefix}.house_exterior.front_drop_guard");
        }

        private static void CreateWindowTrim(Transform root, string prefix, string side, Vector3 center, Material material)
        {
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowTrim_{side}_Top", root, center + new Vector3(0f, 0.34f, 0f), new Vector3(0.78f, 0.08f, 0.08f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.trim_top");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowTrim_{side}_Bottom", root, center + new Vector3(0f, -0.34f, 0f), new Vector3(0.86f, 0.08f, 0.10f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.trim_bottom");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowTrim_{side}_Left", root, center + new Vector3(-0.39f, 0f, 0f), new Vector3(0.08f, 0.66f, 0.08f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.trim_left");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowTrim_{side}_Right", root, center + new Vector3(0.39f, 0f, 0f), new Vector3(0.08f, 0.66f, 0.08f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.trim_right");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowShadow_{side}", root, center + new Vector3(0.02f, -0.48f, 0.05f), new Vector3(0.92f, 0.10f, 0.06f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.shadow_lip");
        }

        private static void CreateFlowerPatch(Transform root, string prefix, Vector3 center, Material leaf, Material flowerA, Material flowerB)
        {
            CreateLandmarkCube($"{prefix}_FlowerPatch_LeafA_{center.x:0.0}_{center.z:0.0}", root, center + new Vector3(-0.15f, -0.04f, 0.02f), new Vector3(0.20f, 0.16f, 0.20f), Quaternion.identity, leaf, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.flower.leaf.a");
            CreateLandmarkCube($"{prefix}_FlowerPatch_LeafB_{center.x:0.0}_{center.z:0.0}", root, center + new Vector3(0.10f, -0.03f, -0.04f), new Vector3(0.22f, 0.16f, 0.22f), Quaternion.identity, leaf, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.flower.leaf.b");
            CreateLandmarkCube($"{prefix}_FlowerPatch_BloomA_{center.x:0.0}_{center.z:0.0}", root, center + new Vector3(-0.16f, 0.08f, 0.04f), new Vector3(0.12f, 0.12f, 0.12f), Quaternion.identity, flowerA, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.flower.bloom.a");
            CreateLandmarkCube($"{prefix}_FlowerPatch_BloomB_{center.x:0.0}_{center.z:0.0}", root, center + new Vector3(0.12f, 0.09f, -0.05f), new Vector3(0.12f, 0.12f, 0.12f), Quaternion.identity, flowerB, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.flower.bloom.b");
        }

        private static void CreateGrassTuft(Transform root, string prefix, Vector3 center, Material material, int index)
        {
            CreateLandmarkCube($"{prefix}_HouseExterior_GrassTuft{index}_A", root, center, new Vector3(0.12f, 0.40f, 0.12f), Quaternion.Euler(0f, 0f, -10f), material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.grass_tuft.{index}.a");
            CreateLandmarkCube($"{prefix}_HouseExterior_GrassTuft{index}_B", root, center + new Vector3(0.12f, 0.02f, 0.04f), new Vector3(0.12f, 0.34f, 0.12f), Quaternion.Euler(0f, 0f, 12f), material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.grass_tuft.{index}.b");
            CreateLandmarkCube($"{prefix}_HouseExterior_GrassTuft{index}_C", root, center + new Vector3(-0.10f, -0.02f, -0.02f), new Vector3(0.12f, 0.30f, 0.12f), Quaternion.Euler(0f, 0f, 4f), material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.grass_tuft.{index}.c");
        }

        private static CharacterController CreateNiroPlayer(Transform currentRoot, Camera camera, Materials materials)
        {
            var player = new GameObject("FastVS_Player_NiroHouseSlice");
            player.tag = "Player";
            player.transform.position = currentRoot.TransformPoint(HouseInteriorPlayerStart);

            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.34f;
            controller.radius = 0.22f;
            controller.center = new Vector3(0f, 0.67f, 0f);

            var visual = new GameObject("FastVS_PlayerVisual_NiroPaper");
            visual.transform.SetParent(player.transform, false);
            var billboard = visual.AddComponent<FastVsPaperBillboard>();
            SerializedSet(billboard, "targetCamera", camera);
            var spriteRenderer = CreateSpriteCardParts(visual.transform, "Niro", 1.18f, materials.NiroSprite, false, materials.Label);
            var pocketGlow = CreateQuad(
                "FastVS_PlayerPocketTimewriterGlow_Niro",
                visual.transform,
                new Vector3(0.13f, 0.47f, -0.055f),
                new Vector3(0.28f, 0.28f, 1f),
                EnsureTimewriterPocketGlowMaterial());
            pocketGlow.AddComponent<FastVsMapMoveGlowPulse>();
            pocketGlow.SetActive(false);
            var contactShadow = CreateQuad(
                "FastVS_PlayerContactShadow_Niro",
                player.transform,
                new Vector3(0f, 0.025f, -0.02f),
                new Vector3(0.74f, 0.34f, 1f),
                EnsureNiroContactShadowMaterial());
            contactShadow.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            var directional = visual.AddComponent<FastVsDirectionalSpriteAnimator>();
            SerializedSet(directional, "player", player.transform);
            SerializedSet(directional, "spriteRenderer", spriteRenderer);
            SerializedSet(directional, "currentFrontMaterial", materials.NiroSprite);
            SerializedSet(directional, "currentLeftMaterial", SpriteMaterial("niro_left_sprite", NiroRightStripPath, Color.white, true));
            SerializedSet(directional, "currentRightMaterial", SpriteMaterial("niro_right_sprite", NiroLeftStripPath, Color.white, true));
            SerializedSet(directional, "otherFrontMaterial", materials.NiroPastSprite);
            SerializedSet(directional, "currentBackMaterial", SpriteMaterial("niro_back_sprite", NiroBackStripPath, Color.white, true));
            SerializedSet(directional, "otherBackMaterial", SpriteMaterial("niro_past_back_sprite", NiroBackStripPath, new Color(0.72f, 0.88f, 1.0f, 0.92f), true));
            SerializedSet(directional, "otherLeftMaterial", SpriteMaterial("niro_past_left_sprite", NiroRightStripPath, new Color(0.72f, 0.88f, 1.0f, 0.92f), true));
            SerializedSet(directional, "otherRightMaterial", SpriteMaterial("niro_past_right_sprite", NiroLeftStripPath, new Color(0.72f, 0.88f, 1.0f, 0.92f), true));
            SerializedSet(directional, "currentWalkFrontMaterial", SpriteMaterial("niro_walk_front_sprite", NiroWalkFrontStripPath, Color.white, true));
            SerializedSet(directional, "currentWalkBackMaterial", SpriteMaterial("niro_walk_back_sprite", NiroWalkBackStripPath, Color.white, true));
            SerializedSet(directional, "currentWalkLeftMaterial", SpriteMaterial("niro_walk_left_sprite", NiroWalkRightStripPath, Color.white, true));
            SerializedSet(directional, "currentWalkRightMaterial", SpriteMaterial("niro_walk_right_sprite", NiroWalkLeftStripPath, Color.white, true));
            SerializedSet(directional, "otherWalkFrontMaterial", SpriteMaterial("niro_past_walk_front_sprite", NiroWalkFrontStripPath, new Color(0.72f, 0.88f, 1.0f, 0.92f), true));
            SerializedSet(directional, "otherWalkBackMaterial", SpriteMaterial("niro_past_walk_back_sprite", NiroWalkBackStripPath, new Color(0.72f, 0.88f, 1.0f, 0.92f), true));
            SerializedSet(directional, "otherWalkLeftMaterial", SpriteMaterial("niro_past_walk_left_sprite", NiroWalkRightStripPath, new Color(0.72f, 0.88f, 1.0f, 0.92f), true));
            SerializedSet(directional, "otherWalkRightMaterial", SpriteMaterial("niro_past_walk_right_sprite", NiroWalkLeftStripPath, new Color(0.72f, 0.88f, 1.0f, 0.92f), true));
            SerializedSet(directional, "animationFrameCount", NiroAnimatedFrameCount);
            SerializedSet(directional, "framePixelWidth", NiroExpectedTextureWidth);
            SerializedSet(directional, "walkFramesPerSecond", 8f);
            SerializedSet(directional, "idleFramesPerSecond", 2f);
            return controller;
        }

        private static TimeWindowPairedSpacePortalController CreateController(Camera camera, Transform currentRoot, Transform pastRoot, CharacterController player, Materials materials)
        {
            var controllerObject = new GameObject("FastVS_HouseSlice_V24_PairedSpacePortalController");
            var controller = controllerObject.AddComponent<TimeWindowPairedSpacePortalController>();
            SerializedSet(controller, "currentSpaceRoot", currentRoot);
            SerializedSet(controller, "otherTimeSpaceRoot", pastRoot);
            SerializedSet(controller, "regionSize", RegionSize);
            SerializedSet(controller, "portalLocalZ", HouseInteriorCenter.z - 0.65f);
            SerializedSet(controller, "placePortalFromGroundProjection", true);
            SerializedSet(controller, "anchorPortalBottomToGround", true);
            SerializedSet(controller, "groundClearance", 0.065f);
            SerializedSet(controller, "playerController", player);
            SerializedSet(controller, "player", player.transform);
            SerializedSet(controller, "currentPlayerMaterial", materials.NiroSprite);
            SerializedSet(controller, "otherTimePlayerMaterial", materials.NiroPastSprite);
            SerializedSet(controller, "sceneCamera", camera);
            SerializedSet(controller, "runtimeInputEnabled", false);
            SerializedSet(controller, "currentFrameMaterial", materials.CurrentFrame);
            SerializedSet(controller, "otherTimeFrameMaterial", materials.PastFrame);
            SerializedSet(controller, "previewFrameMaterial", materials.PreviewFrame);
            SerializedSet(controller, "thresholdMaterial", materials.Threshold);
            SerializedSet(controller, "enablePortalApertureView", true);
            SerializedSet(controller, "apertureTextureSize", 1024);
            SerializedSet(controller, "currentSpaceRenderLayer", CurrentSpaceRenderLayer);
            SerializedSet(controller, "otherTimeSpaceRenderLayer", OtherTimeSpaceRenderLayer);
            SerializedSet(controller, "portalFrameRenderLayer", PortalFrameRenderLayer);
            SerializedSet(controller, "playerVisibleRenderLayer", PlayerVisibleRenderLayer);
            SerializedSet(controller, "aperturePlaneOffset", 0.001f);
            SerializedSet(controller, "apertureObjectSuppressionDepth", 0.30f);
            SerializedSet(controller, "portalApertureMaterial", materials.Aperture);
            SerializedSet(controller, "enableBackSideBlocking", true);
            SerializedSet(controller, "enableGeneratedOtherTimeWallVolume", true);
            SerializedSet(controller, "wallVolumeDepth", 8.0f);
            SerializedSet(controller, "wallVolumeSideMargin", 0.22f);
            SerializedSet(controller, "wallVolumeThickness", 0.14f);
            SerializedSet(controller, "wallVolumeNearGapDepth", 0.42f);
            SerializedSet(controller, "farBackWallDepthMultiplier", 1.12f);
            SerializedSet(controller, "farBackWallDepthPadding", 0.28f);
            SerializedSet(controller, "farBackWallMinimumDepth", 1.15f);
            SerializedSet(controller, "currentBackSideBlockDepth", 0.38f);
            return controller;
        }

        private static void CreateHouseDoorTransitions(TimeWindowPairedSpacePortalController controller, CharacterController player, FastVsHouseAreaVisibility areaVisibility, FastVsStoryFlowController storyFlow)
        {
            CreateAreaDoorTransition(
                "FastVS_DoorTransition_Interior_To_Exterior",
                controller,
                player,
                areaVisibility,
                FastVsHouseArea.Interior,
                FastVsHouseArea.Exterior,
                InteriorDoorTriggerCenter,
                DoorTriggerSize,
                ExteriorDoorExitTarget,
                "Door: house interior to exterior local",
                storyFlow);
            CreateAreaDoorTransition(
                "FastVS_DoorTransition_Exterior_To_Interior",
                controller,
                player,
                areaVisibility,
                FastVsHouseArea.Exterior,
                FastVsHouseArea.Interior,
                ExteriorDoorTriggerCenter,
                DoorTriggerSize,
                InteriorDoorExitTarget,
                "Door: house exterior to interior local",
                storyFlow);
            CreateAreaDoorTransition(
                "FastVS_DoorTransition_Exterior_To_CentralPlaza",
                controller,
                player,
                areaVisibility,
                FastVsHouseArea.Exterior,
                FastVsHouseArea.CentralPlaza,
                ExteriorToPlazaTriggerCenter,
                RouteTriggerSize,
                PlazaFromExteriorTarget,
                "Route: house exterior to central plaza local",
                storyFlow);
            CreateAreaDoorTransition(
                "FastVS_DoorTransition_CentralPlaza_To_Exterior",
                controller,
                player,
                areaVisibility,
                FastVsHouseArea.CentralPlaza,
                FastVsHouseArea.Exterior,
                PlazaToExteriorTriggerCenter,
                RouteTriggerSize,
                ExteriorFromPlazaTarget,
                "Route: central plaza to house exterior local",
                storyFlow);
            CreateAreaDoorTransition(
                "FastVS_DoorTransition_CentralPlaza_To_Library",
                controller,
                player,
                areaVisibility,
                FastVsHouseArea.CentralPlaza,
                FastVsHouseArea.Library,
                PlazaToLibraryTriggerCenter,
                RouteTriggerSize,
                LibraryFromPlazaTarget,
                "Route: central plaza to library local",
                storyFlow);
            CreateAreaDoorTransition(
                "FastVS_DoorTransition_Library_To_CentralPlaza",
                controller,
                player,
                areaVisibility,
                FastVsHouseArea.Library,
                FastVsHouseArea.CentralPlaza,
                LibraryToPlazaTriggerCenter,
                RouteTriggerSize,
                PlazaFromLibraryTarget,
                "Route: library to central plaza local",
                storyFlow);
        }

        private static void CreateAreaDoorTransition(
            string name,
            TimeWindowPairedSpacePortalController controller,
            CharacterController player,
            FastVsHouseAreaVisibility areaVisibility,
            FastVsHouseArea sourceArea,
            FastVsHouseArea targetArea,
            Vector3 triggerLocalCenter,
            Vector3 triggerLocalSize,
            Vector3 targetLocalPosition,
            string transitionLabel,
            FastVsStoryFlowController storyFlow)
        {
            var doorObject = new GameObject(name);
            if (controller != null && controller.CurrentSpaceRootForReview != null)
            {
                doorObject.transform.position = controller.CurrentSpaceRootForReview.TransformPoint(triggerLocalCenter);
            }

            var transition = doorObject.AddComponent<FastVsAreaDoorTransition>();
            SerializedSet(transition, "portalController", controller);
            SerializedSet(transition, "playerController", player);
            SerializedSet(transition, "player", player != null ? player.transform : null);
            SerializedSet(transition, "areaVisibility", areaVisibility);
            SerializedSet(transition, "storyFlow", storyFlow);
            SerializedSet(transition, "sourceArea", sourceArea);
            SerializedSet(transition, "targetArea", targetArea);
            SerializedSet(transition, "triggerLocalCenter", triggerLocalCenter);
            SerializedSet(transition, "triggerLocalSize", triggerLocalSize);
            SerializedSet(transition, "targetLocalPosition", targetLocalPosition);
            SerializedSet(transition, "transitionLabel", transitionLabel);
            SerializedSet(transition, "cooldownSeconds", 0.55f);
            SerializedSet(transition, "transitionFadeSeconds", 0.26f);
            SerializedSet(transition, "transitionHoldSeconds", 0.08f);
        }

        private static FastVsVisualDirectionGuide CreateGuide(Camera camera, TimeWindowPairedSpacePortalController controller, CharacterController player, FastVsHouseAreaVisibility areaVisibility)
        {
            var guideObject = new GameObject("FastVS_HouseSlice_Guide");
            var guide = guideObject.AddComponent<FastVsVisualDirectionGuide>();
            SerializedSet(guide, "portalController", controller);
            SerializedSet(guide, "playerController", player);
            SerializedSet(guide, "player", player.transform);
            SerializedSet(guide, "reviewCamera", camera);
            SerializedSet(guide, "areaVisibility", areaVisibility);
            SerializedSet(guide, "title", "Anemora Fast VS Chapter 1 route - house / plaza / library");
            SerializedSet(guide, "currentStateLabel", "CURRENT / Chapter 1 route");
            SerializedSet(guide, "otherStateLabel", "PAST / Chapter 1 route");
            SerializedSet(guide, "controlHint", "Walk into glowing floor pads to switch maps along Interior > House Exterior > Central Plaza > Library. Left-drag creates the V24 Time Window; close it after returning to current time.");
            SerializedSet(guide, "showDebugOverlay", false);
            return guide;
        }

        private static FastVsStoryFlowController CreateStoryFlow(Camera camera, TimeWindowPairedSpacePortalController controller, CharacterController player, FastVsHouseAreaVisibility areaVisibility, FastVsVisualDirectionGuide guide)
        {
            var brushIcon = EnsureTimewriterBrushIconTexture();
            var presenterObject = new GameObject("FastVS_StoryDialoguePresenter_TMP");
            var presenter = presenterObject.AddComponent<FastVsStoryDialoguePresenter>();
            SerializedSet(presenter, "fontAsset", EnsureFastVsDialogueTmpFontAsset());
            SerializedSet(presenter, "targetCamera", camera);
            SerializedSet(presenter, "brushIconTexture", brushIcon);
            SerializedSet(presenter, "useTmpPresenter", false);

            var hudObject = new GameObject("FastVS_StoryRuntimeHud");
            var hud = hudObject.AddComponent<FastVsStoryRuntimeHud>();
            SerializedSet(hud, "targetCamera", camera);
            SerializedSet(hud, "fontAsset", EnsureFastVsDialogueTmpFontAsset());
            SerializedSet(hud, "brushIconTexture", brushIcon);
            SerializedSet(hud, "charactersPerSecond", 26f);

            var storyObject = new GameObject("FastVS_Chapter1_StoryFlow_RetoClear");
            var story = storyObject.AddComponent<FastVsStoryFlowController>();
            SerializedSet(story, "portalController", controller);
            SerializedSet(story, "areaVisibility", areaVisibility);
            SerializedSet(story, "movementGuide", guide);
            SerializedSet(story, "playerController", player);
            SerializedSet(story, "player", player != null ? player.transform : null);
            SerializedSet(story, "storyCamera", camera);
            SerializedSet(story, "retoAnimator", UnityEngine.Object.FindFirstObjectByType<FastVsRetoWritingAnimator>(FindObjectsInactive.Include));
            SerializedSet(story, "dialoguePresenter", presenter);
            SerializedSet(story, "runtimeHud", hud);
            SerializedSet(story, "currentDeskBookObject", FindSceneObjectIncludingInactive("Current_Library_ReturnedBookOnDesk"));
            SerializedSet(story, "currentTimeWindowBookCueObject", FindSceneObjectIncludingInactive("Current_Library_TimeWindowOpenCue_Book"));
            SerializedSet(story, "currentTimeWindowAriaCueObject", FindSceneObjectIncludingInactive("Current_Library_TimeWindowOpenCue_Aria"));
            SerializedSet(story, "timewriterPocketGlowObject", FindSceneObjectIncludingInactive("FastVS_PlayerPocketTimewriterGlow_Niro"));
            SerializedSet(story, "pastTargetBookObject", FindSceneObjectIncludingInactive("Past_Library_TargetBook_ForPickup"));
            SerializedSet(story, "pastTargetBookMarkerObject", FindSceneObjectIncludingInactive("Past_Library_TargetBook_RedCubeMarker"));
            SerializedSet(story, "pastAriaMarkerObject", FindSceneObjectIncludingInactive("Past_Library_Aria_RedCubeMarker"));
            SerializedSet(story, "retoLocalPosition", RetoLibraryDeskLocalPosition);
            SerializedSet(story, "retoInteractionRadius", 2.05f);
            SerializedSet(story, "pastLibraryBookLocalPosition", new Vector3(PastLibraryBookCueLocalPosition.x, 0.02f, PastLibraryBookCueLocalPosition.z));
            SerializedSet(story, "pastBookInteractionRadius", 1.00f);
            SerializedSet(story, "pastLibraryAriaLocalPosition", new Vector3(PastLibraryPersonCueLocalPosition.x, 0.02f, PastLibraryPersonCueLocalPosition.z));
            SerializedSet(story, "pastAriaInteractionRadius", 1.05f);
            SerializedSet(story, "doorBrushBeatTriggerLocalCenter", InteriorDoorStoryTriggerCenter);
            SerializedSet(story, "doorBrushBeatTriggerLocalSize", InteriorDoorStoryTriggerSize);
            SerializedSet(story, "showOpeningHint", true);
            return story;
        }

        private static TMP_FontAsset EnsureFastVsDialogueTmpFontAsset()
        {
            var existing = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FastVsDialogueFontAssetPath);
            if (existing != null)
            {
                return existing;
            }

            throw new InvalidOperationException($"Fast VS dialogue TMP font asset is missing: {FastVsDialogueFontAssetPath}");
        }

        private static Camera CreateCamera(Transform currentRoot)
        {
            var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            var additionalData = cameraObject.GetComponent<UniversalAdditionalCameraData>();
            if (additionalData == null)
            {
                additionalData = cameraObject.AddComponent<UniversalAdditionalCameraData>();
            }

            additionalData.renderPostProcessing = true;
            additionalData.requiresDepthTexture = true;
            var camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.075f, 0.078f, 0.084f, 1f);
            camera.fieldOfView = 38f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 140f;
            var position = currentRoot.TransformPoint(HouseInteriorCenter + new Vector3(-0.90f, 2.78f, -5.15f));
            var lookAt = currentRoot.TransformPoint(HouseInteriorCenter + new Vector3(-0.20f, 0.72f, 0.20f));
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
            return camera;
        }

        private static void ApplyInitialReviewLayers(Transform currentRoot, Transform pastRoot, Transform player, Camera camera)
        {
            SetLayerRecursive(currentRoot, CurrentSpaceRenderLayer);
            SetLayerRecursive(pastRoot, OtherTimeSpaceRenderLayer);
            SetLayerRecursive(player, PlayerVisibleRenderLayer);

            var currentBit = 1 << CurrentSpaceRenderLayer;
            var pastBit = 1 << OtherTimeSpaceRenderLayer;
            var portalBit = 1 << PortalFrameRenderLayer;
            var playerBit = 1 << PlayerVisibleRenderLayer;
            camera.cullingMask = (camera.cullingMask | currentBit | portalBit | playerBit) & ~pastBit;
        }

        private static void SetLayerRecursive(Transform root, int layer)
        {
            if (root == null)
            {
                return;
            }

            var clamped = Mathf.Clamp(layer, 0, 31);
            foreach (var target in root.GetComponentsInChildren<Transform>(true))
            {
                target.gameObject.layer = clamped;
            }
        }

        private static void CreateLighting()
        {
            var lightObject = new GameObject("Directional Light", typeof(Light));
            var light = lightObject.GetComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.15f;
            light.shadows = LightShadows.Soft;
            light.color = new Color(1.00f, 0.96f, 0.88f, 1f);
            lightObject.transform.rotation = Quaternion.Euler(52f, -35f, 0f);
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.22f, 0.23f, 0.27f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = new Color(0.13f, 0.15f, 0.18f);
            RenderSettings.fogStartDistance = 18f;
            RenderSettings.fogEndDistance = 75f;
        }

        private static void CreateHd2dGlobalVolume()
        {
            var volumeObject = new GameObject("FastVS_HD2D_GlobalVolume");
            var volume = volumeObject.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = 0f;
            volume.weight = 1f;

            const string profilePath = "Assets/Settings/DefaultVolumeProfile.asset";
            var profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(profilePath);
            if (profile == null)
            {
                throw new InvalidOperationException($"Fast VS HD-2D global volume profile is missing: {profilePath}");
            }

            volume.sharedProfile = profile;
        }

        private static void CreateHd2dAtmosphere(Transform currentRoot, Transform pastRoot)
        {
            var material = EnsureHd2dAtmosphereParticleMaterial();

            CreateAtmosphereParticleSystem(
                currentRoot,
                "FastVS_HD2D_CurrentInterior_DustMotes",
                HouseInteriorCenter + new Vector3(0f, 1.25f, 0.20f),
                new Vector3(4.4f, 0.85f, 2.6f),
                new Color(0.90f, 0.86f, 0.78f, 0.16f),
                60,
                12f,
                10.5f,
                6f,
                0.03f,
                0.045f,
                ParticleSystemSimulationSpace.World,
                material);

            CreateAtmosphereParticleSystem(
                currentRoot,
                "FastVS_HD2D_CurrentLibrary_DustMotes",
                LibraryVsCenter + new Vector3(0f, 1.55f, 2.75f),
                new Vector3(6.4f, 1.10f, 4.2f),
                new Color(0.90f, 0.88f, 0.82f, 0.15f),
                64,
                12f,
                11f,
                5.5f,
                0.035f,
                0.042f,
                ParticleSystemSimulationSpace.World,
                material);

            CreateAtmosphereParticleSystem(
                currentRoot,
                "FastVS_HD2D_CurrentExterior_DustDrift",
                HouseExteriorCenter + new Vector3(0f, 1.20f, 0.55f),
                new Vector3(5.8f, 1.30f, 4.8f),
                new Color(0.86f, 0.82f, 0.74f, 0.13f),
                48,
                10f,
                8.5f,
                4.5f,
                0.08f,
                0.060f,
                ParticleSystemSimulationSpace.World,
                material);

            CreateAtmosphereParticleSystem(
                currentRoot,
                "FastVS_HD2D_CurrentPlaza_DustDrift",
                CentralPlazaVsCenter + new Vector3(0f, 1.40f, 3.35f),
                new Vector3(7.2f, 1.50f, 5.0f),
                new Color(0.88f, 0.84f, 0.76f, 0.12f),
                52,
                10f,
                9f,
                3.5f,
                0.09f,
                0.070f,
                ParticleSystemSimulationSpace.World,
                material);

            CreateAtmosphereParticleSystem(
                pastRoot,
                "FastVS_HD2D_PastInterior_WarmMotes",
                HouseInteriorCenter + new Vector3(0f, 1.25f, 0.38f),
                new Vector3(4.4f, 0.85f, 2.6f),
                new Color(0.96f, 0.82f, 0.62f, 0.18f),
                56,
                12f,
                10f,
                6f,
                0.04f,
                0.050f,
                ParticleSystemSimulationSpace.World,
                material);

            CreateAtmosphereParticleSystem(
                pastRoot,
                "FastVS_HD2D_PastLibrary_WarmMotes",
                LibraryVsCenter + new Vector3(0f, 1.55f, 3.10f),
                new Vector3(6.4f, 1.10f, 4.2f),
                new Color(0.96f, 0.84f, 0.58f, 0.17f),
                60,
                12f,
                11f,
                5f,
                0.04f,
                0.050f,
                ParticleSystemSimulationSpace.World,
                material);

            CreateAtmosphereParticleSystem(
                pastRoot,
                "FastVS_HD2D_PastExterior_MemoryDrift",
                HouseExteriorCenter + new Vector3(0f, 1.20f, 0.45f),
                new Vector3(5.8f, 1.30f, 4.8f),
                new Color(0.95f, 0.72f, 0.52f, 0.14f),
                44,
                10f,
                8f,
                4f,
                0.08f,
                0.065f,
                ParticleSystemSimulationSpace.World,
                material);

            CreateAtmosphereParticleSystem(
                pastRoot,
                "FastVS_HD2D_PastPlaza_MemoryDrift",
                CentralPlazaVsCenter + new Vector3(0f, 1.40f, 3.95f),
                new Vector3(7.2f, 1.50f, 5.0f),
                new Color(0.92f, 0.76f, 0.58f, 0.13f),
                46,
                10f,
                9f,
                3.5f,
                0.09f,
                0.075f,
                ParticleSystemSimulationSpace.World,
                material);
        }

        private static void CreateAtmosphereParticleSystem(
            Transform parent,
            string name,
            Vector3 localPosition,
            Vector3 boxSize,
            Color startColor,
            int maxParticles,
            float duration,
            float lifetime,
            float emissionRate,
            float startSpeed,
            float startSize,
            ParticleSystemSimulationSpace simulationSpace,
            Material material)
        {
            var particleObject = new GameObject(name, typeof(ParticleSystem));
            particleObject.transform.SetParent(parent, false);
            particleObject.transform.localPosition = localPosition;
            particleObject.transform.localRotation = Quaternion.identity;
            particleObject.transform.localScale = Vector3.one;

            var system = particleObject.GetComponent<ParticleSystem>();
            var main = system.main;
            main.loop = true;
            main.prewarm = true;
            main.playOnAwake = true;
            main.duration = duration;
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime);
            main.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed);
            main.startSize = new ParticleSystem.MinMaxCurve(startSize);
            main.startColor = new ParticleSystem.MinMaxGradient(startColor);
            main.maxParticles = maxParticles;
            main.gravityModifier = 0f;
            main.simulationSpace = simulationSpace;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);

            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = boxSize;
            shape.position = Vector3.zero;
            shape.rotation = Vector3.zero;
            shape.randomDirectionAmount = 0.30f;

            var renderer = particleObject.GetComponent<ParticleSystemRenderer>();
            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.sortingOrder = 0;
        }

        private static void CreateAudio(Transform currentRoot, FastVsHouseAreaVisibility areaVisibility)
        {
            CreateLoopingAudioSource(
                "FastVS_Audio_Zone1AmbientMusic",
                MusicClipPath,
                currentRoot.TransformPoint(HouseExteriorCenter + new Vector3(0f, 1.2f, 0f)),
                0.34f,
                0f);
            CreateLoopingAudioSource(
                "FastVS_Audio_HouseYardWind",
                WindClipPath,
                currentRoot.TransformPoint(HouseExteriorCenter + new Vector3(0f, 1.0f, 0f)),
                0.22f,
                0.35f);
            CreateOneShotAreaAudioCue(
                "FastVS_Audio_HouseYardBirds_OneShotOnExterior",
                BirdsClipPath,
                currentRoot.TransformPoint(HouseExteriorCenter + new Vector3(2.6f, 2.2f, 2.8f)),
                0.16f,
                0.65f,
                areaVisibility,
                FastVsHouseArea.Exterior);
        }

        private static void CreateLoopingAudioSource(string name, string clipPath, Vector3 position, float volume, float spatialBlend)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(clipPath);
            if (clip == null)
            {
                Debug.LogWarning($"Fast VS audio clip missing: {clipPath}");
                return;
            }

            var audioObject = new GameObject(name);
            audioObject.transform.position = position;
            var source = audioObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = true;
            source.playOnAwake = true;
            source.volume = volume;
            source.spatialBlend = spatialBlend;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2f;
            source.maxDistance = 18f;
        }

        private static void CreateOneShotAreaAudioCue(
            string name,
            string clipPath,
            Vector3 position,
            float volume,
            float spatialBlend,
            FastVsHouseAreaVisibility areaVisibility,
            FastVsHouseArea triggerArea)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(clipPath);
            if (clip == null)
            {
                Debug.LogWarning($"Fast VS audio clip missing: {clipPath}");
                return;
            }

            var audioObject = new GameObject(name);
            audioObject.transform.position = position;
            var source = audioObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = false;
            source.playOnAwake = false;
            source.volume = volume;
            source.spatialBlend = spatialBlend;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2f;
            source.maxDistance = 18f;

            var cue = audioObject.AddComponent<FastVsOneShotAreaAudioCue>();
            SerializedSet(cue, "areaVisibility", areaVisibility);
            SerializedSet(cue, "triggerArea", triggerArea);
            SerializedSet(cue, "audioSource", source);
        }

        private static void ValidateNoForbiddenSceneReferences()
        {
            var sceneText = File.ReadAllText(ScenePath);
            foreach (var token in ForbiddenReferenceTokens)
            {
                if (sceneText.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    throw new InvalidOperationException($"House slice validation failed: forbidden old asset reference found: {token}");
                }
            }
        }

        private static void ValidateFastVsDialogueTmpFontAsset()
        {
            var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FastVsDialogueFontAssetPath);
            if (font == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing Fast VS dialogue font asset: {FastVsDialogueFontAssetPath}");
            }

            if (font.faceInfo.familyName.IndexOf("DotGothic16", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: Anemora_JP must be regenerated from DotGothic16, but familyName was '{font.faceInfo.familyName}'.");
            }
        }

        private static void ValidateHouseYardBirdsAudio()
        {
            if (FindSceneObjectIncludingInactive("FastVS_Audio_HouseYardBirds") != null)
            {
                throw new InvalidOperationException("House slice validation failed: legacy looping birds audio object still exists.");
            }

            var birds = FindSceneObjectIncludingInactive("FastVS_Audio_HouseYardBirds_OneShotOnExterior");
            if (birds == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing one-shot exterior birds audio object.");
            }

            var source = birds.GetComponent<AudioSource>();
            if (source == null)
            {
                throw new InvalidOperationException("House slice validation failed: birds audio object is missing its AudioSource.");
            }

            if (source.loop || source.playOnAwake)
            {
                throw new InvalidOperationException("House slice validation failed: birds audio source must be one-shot only.");
            }

            if (birds.GetComponent<FastVsOneShotAreaAudioCue>() == null)
            {
                throw new InvalidOperationException("House slice validation failed: birds audio object is missing FastVsOneShotAreaAudioCue.");
            }
        }

        private static void ValidateNiroSpriteAsset()
        {
            ValidateNiroSingleFrameAsset("front source", NiroFrontSpritePath);
            ValidateNiroSingleFrameAsset("back source", NiroBackSpritePath);
            ValidateNiroSingleFrameAsset("left source", NiroLeftSpritePath);
            ValidateNiroSingleFrameAsset("right source", NiroRightSpritePath);
            ValidateNiroStripAsset("front animated strip", NiroFrontStripPath);
            ValidateNiroStripAsset("back animated strip", NiroBackStripPath);
            ValidateNiroStripAsset("left animated strip", NiroLeftStripPath);
            ValidateNiroStripAsset("right animated strip", NiroRightStripPath);
            ValidateNiroWalkStripAsset("front walk strip", NiroWalkFrontStripPath);
            ValidateNiroWalkStripAsset("back walk strip", NiroWalkBackStripPath);
            ValidateNiroWalkStripAsset("left walk strip", NiroWalkLeftStripPath);
            ValidateNiroWalkStripAsset("right walk strip", NiroWalkRightStripPath);
        }

        private static void ValidateRetoStateflowAssets()
        {
            ValidateRetoStripAsset("writing normal loop", RetoWritingLoopStripPath, RetoTransitionFrameCount);
            ValidateRetoStripAsset("normal to talk lower arms", RetoLowerArmsStripPath, RetoTransitionFrameCount);
            ValidateRetoStripAsset("talk loop face raised", RetoTalkLoopStripPath, RetoTalkFrameCount);
            ValidateRetoStripAsset("talk to normal raise arms", RetoRaiseArmsStripPath, RetoTransitionFrameCount);
        }

        private static void ValidateAriaSpriteAsset()
        {
            EnsureExternalCharacterAssets();
            if (AriaNormalLoopStripPath.IndexOf("v47", StringComparison.OrdinalIgnoreCase) >= 0 ||
                AriaNormalLoopStripPath.IndexOf("reject", StringComparison.OrdinalIgnoreCase) >= 0 ||
                AriaNormalLoopStripPath.IndexOf("hold", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new InvalidOperationException($"House slice validation failed: Aria must not use rejected or hold assets: {AriaNormalLoopStripPath}");
            }

            EnsureTextureImporter(AriaNormalLoopStripPath);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AriaNormalLoopStripPath);
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: Aria idle-breath texture missing: {AriaNormalLoopStripPath}");
            }

            var expectedWidth = NiroExpectedTextureWidth * NiroAnimatedFrameCount;
            if (texture.width != expectedWidth || texture.height != NiroExpectedTextureHeight)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: Aria idle-breath strip must be {expectedWidth}x{NiroExpectedTextureHeight}, but was {texture.width}x{texture.height}: {AriaNormalLoopStripPath}");
            }
        }

        private static void ValidateRetoStripAsset(string label, string path, int expectedFrameCount)
        {
            if (path.IndexOf("Resident_B", StringComparison.OrdinalIgnoreCase) >= 0 ||
                path.IndexOf("resident_b_idle", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new InvalidOperationException($"House slice validation failed: Reto {label} points at the discarded old Resident_B asset path: {path}");
            }

            EnsureTextureImporter(path);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: Reto {label} sprite texture missing: {path}");
            }

            var expectedWidth = RetoExpectedFrameWidth * expectedFrameCount;
            if (texture.width != expectedWidth || texture.height != RetoExpectedTextureHeight)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: Reto {label} must be {expectedWidth}x{RetoExpectedTextureHeight}, but was {texture.width}x{texture.height}: {path}");
            }
        }

        private static void ValidateNiroSingleFrameAsset(string direction, string path)
        {
            if (path.IndexOf("v45", StringComparison.OrdinalIgnoreCase) >= 0 ||
                path.IndexOf("idle_", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new InvalidOperationException($"House slice validation failed: Niro {direction} sprite still points at a multi-frame review strip: {path}");
            }

            EnsureTextureImporter(path);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: Niro {direction} sprite texture missing: {path}");
            }

            if (texture.width != NiroExpectedTextureWidth || texture.height != NiroExpectedTextureHeight)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: Niro {direction} sprite must be {NiroExpectedTextureWidth}x{NiroExpectedTextureHeight}, but was {texture.width}x{texture.height}: {path}");
            }
        }

        private static void ValidateNiroStripAsset(string direction, string path)
        {
            EnsureTextureImporter(path);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: Niro {direction} texture missing: {path}");
            }

            if (texture.width != NiroExpectedStripWidth || texture.height != NiroExpectedTextureHeight)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: Niro {direction} must be {NiroExpectedStripWidth}x{NiroExpectedTextureHeight}, but was {texture.width}x{texture.height}: {path}");
            }
        }

        private static void ValidateNiroWalkStripAsset(string direction, string path)
        {
            EnsureTextureImporter(path);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: Niro {direction} texture missing: {path}");
            }

            if (texture.height != NiroExpectedTextureHeight ||
                texture.width % NiroExpectedTextureWidth != 0 ||
                texture.width < NiroExpectedStripWidth)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: Niro {direction} must be a 64x96-frame walk strip with at least {NiroAnimatedFrameCount} frames, but was {texture.width}x{texture.height}: {path}");
            }
        }

        private static void ValidateHouseMapSeparationAndDoorTransitions(TimeWindowPairedSpacePortalController controller)
        {
            if (controller == null || controller.CurrentSpaceRootForReview == null || controller.OtherTimeSpaceRootForReview == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing paired map roots.");
            }

            if (Vector3.Distance(controller.CurrentSpaceRootForReview.position, controller.OtherTimeSpaceRootForReview.position) > 0.01f)
            {
                throw new InvalidOperationException("House slice validation failed: current/past roots must share the same world coordinate for V24 same-coordinate camera behavior.");
            }

            if (Mathf.Abs(LibraryVsCenter.x) + 5.2f > RegionSize.x * 0.5f ||
                Mathf.Abs(LibraryVsCenter.z) + 3.8f > RegionSize.y * 0.5f)
            {
                throw new InvalidOperationException("House slice validation failed: Time Window region no longer contains the Chapter 1 library route map.");
            }

            if (GameObject.Find("Current_BroadInteriorExteriorRoute") != null ||
                GameObject.Find("Past_BroadInteriorExteriorRoute") != null ||
                FindSceneObjectIncludingInactive("Current_FullPairedGround_42mCoordinateField") != null ||
                FindSceneObjectIncludingInactive("Past_FullPairedGround_42mCoordinateField") != null)
            {
                throw new InvalidOperationException("House slice validation failed: interior/exterior still look like one continuous physical map.");
            }

            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>(FindObjectsInactive.Include);
            if (visibility == null || !visibility.HasAllMapSetsForReview)
            {
                throw new InvalidOperationException("House slice validation failed: separate interior/exterior map visibility controller is missing or incomplete.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.Interior);
            if (!visibility.InteriorActiveForReview || visibility.ExteriorActiveForReview || visibility.CentralPlazaActiveForReview || visibility.LibraryActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: interior map set is not isolated from the other route map sets.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.Exterior);
            if (!visibility.ExteriorActiveForReview || visibility.InteriorActiveForReview || visibility.CentralPlazaActiveForReview || visibility.LibraryActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: exterior map set is not isolated from the other route map sets.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            if (!visibility.CentralPlazaActiveForReview || visibility.InteriorActiveForReview || visibility.ExteriorActiveForReview || visibility.LibraryActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: central plaza map set is not isolated from the other route map sets.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.Library);
            if (!visibility.LibraryActiveForReview || visibility.InteriorActiveForReview || visibility.ExteriorActiveForReview || visibility.CentralPlazaActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: library map set is not isolated from the other route map sets.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.Interior);

            if (FindSceneObjectIncludingInactive("Current_HouseInterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_DoorEntrySmallGlow") == null ||
                FindSceneObjectIncludingInactive("Past_HouseInterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_DoorEntrySmallGlow") == null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_ToPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_ToPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_ToHouseExterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_ToHouseExterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_Library_ToCentralPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_Library_ToCentralPlaza_MapMoveGlowPad") == null)
            {
                throw new InvalidOperationException("House slice validation failed: separate-map route floor glow pads are missing.");
            }

            ValidateInvisibleDropGuard("Current_HouseInterior_InvisibleFrontDropGuard");
            ValidateInvisibleDropGuard("Past_HouseInterior_InvisibleFrontDropGuard");
            ValidateInvisibleDropGuard("Current_HouseExterior_InvisibleFrontDropGuard");
            ValidateInvisibleDropGuard("Past_HouseExterior_InvisibleFrontDropGuard");
            ValidateInvisibleDropGuard("Current_CentralPlaza_InvisibleFrontDropGuard");
            ValidateInvisibleDropGuard("Past_CentralPlaza_InvisibleFrontDropGuard");
            ValidateInvisibleDropGuard("Current_CentralPlaza_InvisibleBackBoundary");
            ValidateInvisibleDropGuard("Past_CentralPlaza_InvisibleBackBoundary");
            ValidateInvisibleDropGuard("Current_CentralPlaza_InvisibleLeftBoundary");
            ValidateInvisibleDropGuard("Past_CentralPlaza_InvisibleLeftBoundary");
            ValidateInvisibleDropGuard("Current_CentralPlaza_InvisibleRightBoundary");
            ValidateInvisibleDropGuard("Past_CentralPlaza_InvisibleRightBoundary");
            ValidateInvisibleDropGuard("Current_CentralPlaza_FountainNoStepCollider");
            ValidateInvisibleDropGuard("Past_CentralPlaza_FountainNoStepCollider");
            ValidateFountainNoStepCollider("Current_CentralPlaza_FountainNoStepCollider");
            ValidateFountainNoStepCollider("Past_CentralPlaza_FountainNoStepCollider");
            ValidateCentralPlazaStoneSquareAndFountainLayout("Current_CentralPlaza_StoneSquare", "Current_CentralPlaza_FountainNoStepCollider");
            ValidateCentralPlazaStoneSquareAndFountainLayout("Past_CentralPlaza_StoneSquare", "Past_CentralPlaza_FountainNoStepCollider");
            ValidateInvisibleDropGuard("Current_Library_InvisibleFrontDropGuard");
            ValidateInvisibleDropGuard("Past_Library_InvisibleFrontDropGuard");
            ValidateInvisibleDropGuard("Current_Library_InvisibleBackBoundary");
            ValidateInvisibleDropGuard("Past_Library_InvisibleBackBoundary");
            ValidateInvisibleDropGuard("Current_Library_InvisibleLeftBoundary");
            ValidateInvisibleDropGuard("Past_Library_InvisibleLeftBoundary");
            ValidateInvisibleDropGuard("Current_Library_InvisibleRightBoundary");
            ValidateInvisibleDropGuard("Past_Library_InvisibleRightBoundary");
            ValidateInvisibleDropGuard("Current_Library_ReadingTableLong_NoStepCollider");
            ValidateInvisibleDropGuard("Current_Library_ReadingTableSideA_NoStepCollider");
            ValidateInvisibleDropGuard("Current_Library_ReadingTableSideB_NoStepCollider");
            ValidateLibraryMapLayout("Current_Library_PixelFloor", "Current_Library_BackWall", "Current_Library_SecondFloor_LeftBalcony", "Current_Library_SecondFloor_RightBalcony", "Current_Library_SecondFloor_BackGallery", "Current_Library_SecondFloor_Railing_Left");
            ValidateLibraryMapLayout("Past_Library_PixelFloor", "Past_Library_BackWall", "Past_Library_SecondFloor_LeftBalcony", "Past_Library_SecondFloor_RightBalcony", "Past_Library_SecondFloor_BackGallery", "Past_Library_SecondFloor_Railing_Left");
            ValidateLibrarySideBookshelves();
            ValidateLibraryEntryAlignment("Current_Library_EntryThreshold", "Past_Library_EntryThreshold");
            ValidateLibraryEntryDoorPanelsRemoved();
            ValidateCentralPlazaLibraryFacadeDetails("Current");
            ValidateCentralPlazaLibraryFacadeDetails("Past");
            ValidateCentralPlazaFountainWaterPresentation("Current_CentralPlaza_FountainWater", "Dust");
            ValidateCentralPlazaFountainWaterPresentation("Past_CentralPlaza_FountainWater", "Water");
            ValidateCurrentCentralPlazaDryFountainDetails();
            ValidateCentralPlazaMarketStallPresentation();
            ValidatePastLibraryBackWallBookRuns();
            ValidateNoDebugWorldLabel("CURRENT HOUSE INTERIOR");
            ValidateNoDebugWorldLabel("PAST HOUSE INTERIOR");
            ValidateNoDebugWorldLabel("CURRENT HOUSE EXTERIOR");
            ValidateNoDebugWorldLabel("PAST HOUSE EXTERIOR");
            ValidateNoDebugWorldLabel("CURRENT: Niro house exterior / road toward");
            ValidateNoDebugWorldLabel("PAST: lived-in house / bright yard / memor");
            ValidateNoDebugWorldLabel("CURRENT: central plaza / route toward libr");
            ValidateNoDebugWorldLabel("PAST: central plaza / market day memory");
            ValidateNoDebugWorldLabel("CURRENT: closed library / Chapter 1 VS rou");
            ValidateNoDebugWorldLabel("PAST: open library / restored archive");

            if (FindSceneObjectIncludingInactive("Current_HouseInterior_FrontLipLeft") != null ||
                FindSceneObjectIncludingInactive("Current_HouseInterior_FrontLipRight") != null ||
                FindSceneObjectIncludingInactive("Current_HouseInterior_DoorTravelGlow") != null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_DoorTravelGlow") != null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_OpenDoorCard") != null ||
                FindSceneObjectIncludingInactive("Current_HouseInterior_TimewriterRedCue") != null ||
                FindSceneObjectIncludingInactive("Current_Library_RedRouteCue") != null ||
                FindSceneObjectIncludingInactive("Current_Library_ArchiveDoorHint") != null ||
                FindSceneObjectIncludingInactive("Past_Library_OpenBookGlow") != null ||
                FindSceneObjectIncludingInactive("Past_Library_TargetBookCueSpark") != null ||
                FindSceneObjectIncludingInactive("Past_Library_AriaCueSpark") != null ||
                FindSceneObjectIncludingInactive("Past_Library_TargetBookInteractionGlow") != null ||
                FindSceneObjectIncludingInactive("Past_Library_AriaInteractionGlow") != null)
            {
                throw new InvalidOperationException("House slice validation failed: removed front-wall, warp-door, mosaic cue, or past-side floating guide visual still exists.");
            }

            if (FindSceneObjectIncludingInactive("Past Niro") != null)
            {
                throw new InvalidOperationException("House slice validation failed: Past Niro review sprite must not be present in the house slice.");
            }

            if (FindSceneObjectIncludingInactive("FastVS_PaperCharacter_ArchivistMemory") != null ||
                FindSceneObjectIncludingInactive("Archivist Memory_NameLabel") != null ||
                FindSceneObjectIncludingInactive("Past Clerk_NameLabel") != null)
            {
                throw new InvalidOperationException("House slice validation failed: temporary past-library paper people or white name labels must not remain.");
            }

            if (FindSceneObjectIncludingInactive("FastVS_PaperCharacter_PlazaVendor") != null ||
                FindSceneObjectIncludingInactive("Plaza Vendor_NameLabel") != null)
            {
                throw new InvalidOperationException("House slice validation failed: past plaza vendor/person must not remain in the fast VS slice.");
            }

            if (FindSceneObjectIncludingInactive("Current_SmallTable_LegFL") == null ||
                FindSceneObjectIncludingInactive("Current_SmallTable_LegBR") == null ||
                FindSceneObjectIncludingInactive("Past_SmallTable_LegFL") == null ||
                FindSceneObjectIncludingInactive("Past_SmallTable_LegBR") == null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_NorthEastRoad_PlazaApproach") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_NorthEastRoad_PlazaApproach") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_PixelGround") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_PixelGround") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_RoadToSouthEastQuarter") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_RoadToSouthEastQuarter") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_LibraryNorthFacade") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_LibraryNorthFacade") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_FountainNoStepCollider") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_FountainNoStepCollider") == null ||
                FindSceneObjectIncludingInactive("Current_Library_PixelFloor") == null ||
                FindSceneObjectIncludingInactive("Past_Library_PixelFloor") == null ||
                FindSceneObjectIncludingInactive("Current_Library_ReadingTableLong") == null ||
                FindSceneObjectIncludingInactive("Current_Library_TableOpenBook") == null ||
                FindSceneObjectIncludingInactive("Current_Library_LeftSideBookshelf") == null ||
                FindSceneObjectIncludingInactive("Current_Library_RightSideBookshelf") == null ||
                FindSceneObjectIncludingInactive("Current_Library_RetoDeskBook_Initial") == null ||
                FindSceneObjectIncludingInactive("Current_Library_ReturnedBookOnDesk") == null ||
                FindSceneObjectIncludingInactive("Current_Library_TimeWindowOpenCue_Book") == null ||
                FindSceneObjectIncludingInactive("Current_Library_TimeWindowOpenCue_Aria") == null ||
                FindSceneObjectIncludingInactive("Past_Library_TargetBook_ForPickup") == null ||
                FindSceneObjectIncludingInactive("Past_Library_TargetBook_RedCubeMarker") == null ||
                FindSceneObjectIncludingInactive("Past_Library_LeftSideBookshelf") == null ||
                FindSceneObjectIncludingInactive("Past_Library_RightSideBookshelf") == null ||
                FindSceneObjectIncludingInactive("Past_Library_ReadingTableClean_LeftFront") == null ||
                FindSceneObjectIncludingInactive("Past_Library_ReadingTableClean_CenterFront") == null ||
                FindSceneObjectIncludingInactive("Past_Library_ReadingTableClean_RightFront") == null ||
                FindSceneObjectIncludingInactive("Past_Library_ReadingTableClean_LeftRear") == null ||
                FindSceneObjectIncludingInactive("Past_Library_ReadingTableClean_CenterRear") == null ||
                FindSceneObjectIncludingInactive("Past_Library_ReadingTableClean_RightRear") == null ||
                FindSceneObjectIncludingInactive("Past_Library_AriaIdleAtTable") == null ||
                FindSceneObjectIncludingInactive("Current_Library_ReadingTableLong_NoStepCollider") == null)
            {
                throw new InvalidOperationException("House slice validation failed: Chapter 1 route maps or approach roads are missing.");
            }

            ValidateInvisibleDropGuard("Current_Library_ReadingTableLong_NoStepCollider");
            ValidateInvisibleDropGuard("Current_Library_ReadingTableSideA_NoStepCollider");
            ValidateInvisibleDropGuard("Current_Library_ReadingTableSideB_NoStepCollider");
            ValidateInvisibleDropGuard("Past_Library_ReadingTableClean_LeftFront_NoStepCollider");
            ValidateInvisibleDropGuard("Past_Library_ReadingTableClean_CenterFront_NoStepCollider");
            ValidateInvisibleDropGuard("Past_Library_ReadingTableClean_RightFront_NoStepCollider");
            ValidateInvisibleDropGuard("Past_Library_ReadingTableClean_LeftRear_NoStepCollider");
            ValidateInvisibleDropGuard("Past_Library_ReadingTableClean_CenterRear_NoStepCollider");
            ValidateInvisibleDropGuard("Past_Library_ReadingTableClean_RightRear_NoStepCollider");

            if (FindSceneObjectIncludingInactive("Past_Library_ReadingTableLong") != null ||
                FindSceneObjectIncludingInactive("Past_Library_ReadingTableSideA") != null ||
                FindSceneObjectIncludingInactive("Past_Library_ReadingTableSideB") != null ||
                FindSceneObjectIncludingInactive("Past_Library_TableOpenBook") != null)
            {
                throw new InvalidOperationException("House slice validation failed: past library must use the orderly clean table set, not the current-side table blockout.");
            }

            if (FindSceneObjectIncludingInactive("Current_Library_ArchiveShelfHint") != null ||
                FindSceneObjectIncludingInactive("Past_Library_ArchiveShelfHint") != null ||
                FindSceneObjectIncludingInactive("Current_Library_ArchiveShelfBooksUpper") != null ||
                FindSceneObjectIncludingInactive("Past_Library_ArchiveShelfBooksUpper") != null ||
                FindSceneObjectIncludingInactive("Current_Library_ArchiveShelfBooksLower") != null ||
                FindSceneObjectIncludingInactive("Past_Library_ArchiveShelfBooksLower") != null ||
                FindSceneObjectIncludingInactive("Current_Library_ArchiveShelfSign") != null ||
                FindSceneObjectIncludingInactive("Past_Library_ArchiveShelfSign") != null)
            {
                throw new InvalidOperationException("House slice validation failed: temporary central archive-shelf blockout remains on the library back wall.");
            }

            if (FindSceneObjectIncludingInactive("FastVS_SpriteCharacter_PastNiro") != null)
            {
                throw new InvalidOperationException("House slice validation failed: past-side Niro NPC should not be visible through exterior Time Windows.");
            }

            ValidateInitialCurrentOnlyCulling(controller);

            if (ExteriorDoorTriggerCenter.z >= HouseExteriorCenter.z ||
                ExteriorDoorExitTarget.z >= ExteriorDoorTriggerCenter.z)
            {
                throw new InvalidOperationException("House slice validation failed: exterior door must face the road-facing south side of Niro's house.");
            }

            var interiorToExterior = GameObject.Find("FastVS_DoorTransition_Interior_To_Exterior")?.GetComponent<FastVsAreaDoorTransition>();
            var exteriorToInterior = GameObject.Find("FastVS_DoorTransition_Exterior_To_Interior")?.GetComponent<FastVsAreaDoorTransition>();
            var exteriorToPlaza = GameObject.Find("FastVS_DoorTransition_Exterior_To_CentralPlaza")?.GetComponent<FastVsAreaDoorTransition>();
            var plazaToExterior = GameObject.Find("FastVS_DoorTransition_CentralPlaza_To_Exterior")?.GetComponent<FastVsAreaDoorTransition>();
            var plazaToLibrary = GameObject.Find("FastVS_DoorTransition_CentralPlaza_To_Library")?.GetComponent<FastVsAreaDoorTransition>();
            var libraryToPlaza = GameObject.Find("FastVS_DoorTransition_Library_To_CentralPlaza")?.GetComponent<FastVsAreaDoorTransition>();
            if (interiorToExterior == null || exteriorToInterior == null || exteriorToPlaza == null || plazaToExterior == null || plazaToLibrary == null || libraryToPlaza == null)
            {
                throw new InvalidOperationException("House slice validation failed: actual route transition components are missing.");
            }

            if (!interiorToExterior.StoryFlowWiredForReview)
            {
                throw new InvalidOperationException("House slice validation failed: interior door transition must be wired to the story flow so the pre-exit brush beat cannot be skipped.");
            }

            if (interiorToExterior.TransitionFadeSecondsForReview < 0.18f ||
                exteriorToInterior.TransitionFadeSecondsForReview < 0.18f ||
                exteriorToPlaza.TransitionFadeSecondsForReview < 0.18f ||
                plazaToExterior.TransitionFadeSecondsForReview < 0.18f ||
                plazaToLibrary.TransitionFadeSecondsForReview < 0.18f ||
                libraryToPlaza.TransitionFadeSecondsForReview < 0.18f ||
                interiorToExterior.TransitionHoldSecondsForReview < 0.04f ||
                exteriorToInterior.TransitionHoldSecondsForReview < 0.04f ||
                exteriorToPlaza.TransitionHoldSecondsForReview < 0.04f ||
                plazaToExterior.TransitionHoldSecondsForReview < 0.04f ||
                plazaToLibrary.TransitionHoldSecondsForReview < 0.04f ||
                libraryToPlaza.TransitionHoldSecondsForReview < 0.04f)
            {
                throw new InvalidOperationException("House slice validation failed: route map transitions need a visible fade instead of an instant cut.");
            }

            ValidateVectorNear("interior door trigger", interiorToExterior.TriggerLocalCenterForReview, InteriorDoorTriggerCenter);
            ValidateVectorNear("interior door target", interiorToExterior.TargetLocalPositionForReview, ExteriorDoorExitTarget);
            if (interiorToExterior.SourceAreaForReview != FastVsHouseArea.Interior ||
                interiorToExterior.TargetAreaForReview != FastVsHouseArea.Exterior)
            {
                throw new InvalidOperationException("House slice validation failed: interior door does not switch to exterior map set.");
            }

            ValidateVectorNear("exterior door trigger", exteriorToInterior.TriggerLocalCenterForReview, ExteriorDoorTriggerCenter);
            ValidateVectorNear("exterior door target", exteriorToInterior.TargetLocalPositionForReview, InteriorDoorExitTarget);
            if (exteriorToInterior.SourceAreaForReview != FastVsHouseArea.Exterior ||
                exteriorToInterior.TargetAreaForReview != FastVsHouseArea.Interior)
            {
                throw new InvalidOperationException("House slice validation failed: exterior door does not switch to interior map set.");
            }

            ValidateRouteTransition("exterior to plaza", exteriorToPlaza, ExteriorToPlazaTriggerCenter, PlazaFromExteriorTarget, FastVsHouseArea.Exterior, FastVsHouseArea.CentralPlaza);
            ValidateRouteTransition("plaza to exterior", plazaToExterior, PlazaToExteriorTriggerCenter, ExteriorFromPlazaTarget, FastVsHouseArea.CentralPlaza, FastVsHouseArea.Exterior);
            ValidateRouteTransition("plaza to library", plazaToLibrary, PlazaToLibraryTriggerCenter, LibraryFromPlazaTarget, FastVsHouseArea.CentralPlaza, FastVsHouseArea.Library);
            ValidateRouteTransition("library to plaza", libraryToPlaza, LibraryToPlazaTriggerCenter, PlazaFromLibraryTarget, FastVsHouseArea.Library, FastVsHouseArea.CentralPlaza);
            ValidateVectorNear("exterior to plaza route trigger size", exteriorToPlaza.TriggerLocalSizeForReview, RouteTriggerSize);
            ValidateVectorNear("plaza to exterior route trigger size", plazaToExterior.TriggerLocalSizeForReview, RouteTriggerSize);
            ValidateVectorNear("plaza to library route trigger size", plazaToLibrary.TriggerLocalSizeForReview, RouteTriggerSize);
            ValidateVectorNear("library to plaza route trigger size", libraryToPlaza.TriggerLocalSizeForReview, RouteTriggerSize);
            ValidateRouteSpawnOffset("exterior to plaza return clearance", PlazaFromExteriorTarget, PlazaToExteriorTriggerCenter);
            ValidateRouteSpawnOffset("plaza to exterior return clearance", ExteriorFromPlazaTarget, ExteriorToPlazaTriggerCenter);
            ValidateRouteSpawnOffset("plaza to library return clearance", LibraryFromPlazaTarget, LibraryToPlazaTriggerCenter);
            ValidateRouteSpawnOffset("library to plaza return clearance", PlazaFromLibraryTarget, PlazaToLibraryTriggerCenter);
            ValidateRouteTriggerSourceAreaIsolation(controller, visibility, exteriorToPlaza, FastVsHouseArea.CentralPlaza, ExteriorToPlazaTriggerCenter, "exterior to plaza");
            ValidateRouteTriggerSourceAreaIsolation(controller, visibility, plazaToLibrary, FastVsHouseArea.Exterior, PlazaToLibraryTriggerCenter, "plaza to library");

            ValidateMapTransitionClosesCurrentTimePortal(controller, visibility, exteriorToPlaza);
            ValidateDoorTriggerReachability(controller, visibility, interiorToExterior, exteriorToInterior, exteriorToPlaza, plazaToExterior, plazaToLibrary, libraryToPlaza);
            ValidateDoorTransitionExecution(controller, visibility, interiorToExterior, exteriorToInterior, exteriorToPlaza, plazaToExterior, plazaToLibrary, libraryToPlaza);
        }

        private static void ValidateDoorWarp(TimeWindowPairedSpacePortalController controller)
        {
            var player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            if (player == null)
            {
                throw new InvalidOperationException("House slice validation failed: player controller missing for door warp validation.");
            }

            controller.ForcePlayerCurrentLocalForReview(HouseInteriorPlayerStart);
            controller.WarpPlayerToLocalForReview(ExteriorDoorExitTarget, "Validation: interior door to exterior");
            var currentLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("current-space door warp", currentLocal, ExteriorDoorExitTarget);

            controller.ForcePlayerOtherTimeLocalForReview(InteriorDoorExitTarget);
            controller.WarpPlayerToLocalForReview(ExteriorDoorExitTarget, "Validation: past interior door to past exterior");
            if (!controller.PlayerInOtherTime)
            {
                throw new InvalidOperationException("House slice validation failed: door warp changed the active time state.");
            }

            var otherLocal = controller.OtherTimeSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("past-space door warp", otherLocal, ExteriorDoorExitTarget);
            controller.ForcePlayerCurrentLocalForReview(HouseInteriorPlayerStart);
        }

        private static void ValidateRouteTransition(string label, FastVsAreaDoorTransition transition, Vector3 expectedTrigger, Vector3 expectedTarget, FastVsHouseArea expectedSourceArea, FastVsHouseArea expectedTargetArea)
        {
            ValidateVectorNear($"{label} trigger", transition.TriggerLocalCenterForReview, expectedTrigger);
            ValidateVectorNear($"{label} trigger size", transition.TriggerLocalSizeForReview, RouteTriggerSize);
            ValidateVectorNear($"{label} target", transition.TargetLocalPositionForReview, expectedTarget);
            if (transition.SourceAreaForReview != expectedSourceArea ||
                transition.TargetAreaForReview != expectedTargetArea)
            {
                throw new InvalidOperationException($"House slice validation failed: {label} does not switch from {expectedSourceArea} to {expectedTargetArea} map set.");
            }
        }

        private static void ValidateMapTransitionClosesCurrentTimePortal(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsAreaDoorTransition exteriorToPlaza)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.Exterior);
            controller.ForcePlayerCurrentLocalForReview(ExteriorToPlazaTriggerCenter);
            controller.ClosePortal();
            if (!controller.TryOpenPortalForTests(DragStart, DragEnd) || !controller.HasPortalPair)
            {
                throw new InvalidOperationException("House slice validation failed: setup could not open a Time Window before route-transition close validation.");
            }

            if (!exteriorToPlaza.TryEvaluateCurrentPlayerForReview())
            {
                throw new InvalidOperationException("House slice validation failed: route transition did not execute while a current-time Time Window was open.");
            }

            if (controller.HasPortalPair || controller.HasPreviewPortal)
            {
                throw new InvalidOperationException("House slice validation failed: route transition must close an open current-time Time Window instead of leaving a stray window at the screen edge.");
            }

            if (visibility.ActiveAreaForReview != FastVsHouseArea.CentralPlaza)
            {
                throw new InvalidOperationException("House slice validation failed: closing the Time Window before route transition prevented the destination map from activating.");
            }

            var player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            var local = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("open-window route transition target", local, PlazaFromExteriorTarget);
        }

        private static void ValidateRouteTriggerSourceAreaIsolation(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsAreaDoorTransition transition,
            FastVsHouseArea wrongArea,
            Vector3 triggerCenter,
            string label)
        {
            visibility.SetActiveAreaForReview(wrongArea);
            controller.ForcePlayerCurrentLocalForReview(triggerCenter);
            if (transition.TryEvaluateCurrentPlayerForReview())
            {
                throw new InvalidOperationException($"House slice validation failed: inactive {label} trigger fires while {wrongArea} map is active.");
            }
        }

        private static void ValidateRouteSpawnOffset(string label, Vector3 spawn, Vector3 returnTrigger)
        {
            var spawnXZ = new Vector2(spawn.x, spawn.z);
            var triggerXZ = new Vector2(returnTrigger.x, returnTrigger.z);
            if (Vector2.Distance(spawnXZ, triggerXZ) < 1.45f)
            {
                throw new InvalidOperationException($"House slice validation failed: {label} is too close and may immediately bounce the player back.");
            }
        }

        private static void ValidateDoorTransitionExecution(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsAreaDoorTransition interiorToExterior,
            FastVsAreaDoorTransition exteriorToInterior,
            FastVsAreaDoorTransition exteriorToPlaza,
            FastVsAreaDoorTransition plazaToExterior,
            FastVsAreaDoorTransition plazaToLibrary,
            FastVsAreaDoorTransition libraryToPlaza)
        {
            var player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            if (player == null)
            {
                throw new InvalidOperationException("House slice validation failed: player controller missing for door transition validation.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.Interior);
            controller.ForcePlayerCurrentLocalForReview(InteriorDoorExitTarget);
            interiorToExterior.TriggerForReview();
            if (visibility.ActiveAreaForReview != FastVsHouseArea.Exterior)
            {
                throw new InvalidOperationException("House slice validation failed: interior door did not activate exterior map set.");
            }

            var exteriorLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("interior door execution target", exteriorLocal, ExteriorDoorExitTarget);

            exteriorToInterior.TriggerForReview();
            if (visibility.ActiveAreaForReview != FastVsHouseArea.Interior)
            {
                throw new InvalidOperationException("House slice validation failed: exterior door did not activate interior map set.");
            }

            var interiorLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("exterior door execution target", interiorLocal, InteriorDoorExitTarget);

            visibility.SetActiveAreaForReview(FastVsHouseArea.Exterior);
            controller.ForcePlayerCurrentLocalForReview(ExteriorToPlazaTriggerCenter);
            exteriorToPlaza.TriggerForReview();
            if (visibility.ActiveAreaForReview != FastVsHouseArea.CentralPlaza)
            {
                throw new InvalidOperationException("House slice validation failed: exterior route pad did not activate central plaza map set.");
            }

            var plazaLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("exterior to plaza execution target", plazaLocal, PlazaFromExteriorTarget);

            plazaToExterior.TriggerForReview();
            if (visibility.ActiveAreaForReview != FastVsHouseArea.Exterior)
            {
                throw new InvalidOperationException("House slice validation failed: plaza return pad did not activate exterior map set.");
            }

            var exteriorReturnLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("plaza to exterior execution target", exteriorReturnLocal, ExteriorFromPlazaTarget);

            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(PlazaToLibraryTriggerCenter);
            plazaToLibrary.TriggerForReview();
            if (visibility.ActiveAreaForReview != FastVsHouseArea.Library)
            {
                throw new InvalidOperationException("House slice validation failed: plaza library pad did not activate library map set.");
            }

            var libraryLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("plaza to library execution target", libraryLocal, LibraryFromPlazaTarget);

            libraryToPlaza.TriggerForReview();
            if (visibility.ActiveAreaForReview != FastVsHouseArea.CentralPlaza)
            {
                throw new InvalidOperationException("House slice validation failed: library return pad did not activate central plaza map set.");
            }

            var plazaReturnLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            ValidateVectorNear("library to plaza execution target", plazaReturnLocal, PlazaFromLibraryTarget);
        }

        private static void ValidateDoorTriggerReachability(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsAreaDoorTransition interiorToExterior,
            FastVsAreaDoorTransition exteriorToInterior,
            FastVsAreaDoorTransition exteriorToPlaza,
            FastVsAreaDoorTransition plazaToExterior,
            FastVsAreaDoorTransition plazaToLibrary,
            FastVsAreaDoorTransition libraryToPlaza)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.Interior);
            controller.ForcePlayerCurrentLocalForReview(InteriorDoorTriggerCenter);
            if (!interiorToExterior.TryEvaluateCurrentPlayerForReview())
            {
                throw new InvalidOperationException("House slice validation failed: standing on the interior door pad does not trigger exterior travel.");
            }

            if (visibility.ActiveAreaForReview != FastVsHouseArea.Exterior)
            {
                throw new InvalidOperationException("House slice validation failed: reachable interior door pad did not activate exterior map set.");
            }

            var exteriorLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(UnityEngine.Object.FindFirstObjectByType<CharacterController>().transform.position);
            ValidateVectorNear("reachable interior door target", exteriorLocal, ExteriorDoorExitTarget);

            if (PointInsideBox(ExteriorDoorExitTarget, ExteriorDoorTriggerCenter, DoorTriggerSize))
            {
                throw new InvalidOperationException("House slice validation failed: exterior spawn target is still inside the return trigger and may bounce back indoors.");
            }

            controller.ForcePlayerCurrentLocalForReview(ExteriorDoorTriggerCenter);
            if (!exteriorToInterior.TryEvaluateCurrentPlayerForReview())
            {
                throw new InvalidOperationException("House slice validation failed: standing on the exterior door pad does not trigger interior travel.");
            }

            if (visibility.ActiveAreaForReview != FastVsHouseArea.Interior)
            {
                throw new InvalidOperationException("House slice validation failed: reachable exterior door pad did not activate interior map set.");
            }

            var interiorLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(UnityEngine.Object.FindFirstObjectByType<CharacterController>().transform.position);
            ValidateVectorNear("reachable exterior door target", interiorLocal, InteriorDoorExitTarget);

            if (PointInsideBox(InteriorDoorExitTarget, InteriorDoorTriggerCenter, DoorTriggerSize))
            {
                throw new InvalidOperationException("House slice validation failed: interior spawn target is still inside the exit trigger and may bounce back outside.");
            }

            ValidateReachableRoutePad(controller, visibility, FastVsHouseArea.Exterior, exteriorToPlaza, ExteriorToPlazaTriggerCenter, PlazaFromExteriorTarget, FastVsHouseArea.CentralPlaza, "exterior to plaza");
            ValidateReachableRoutePad(controller, visibility, FastVsHouseArea.CentralPlaza, plazaToExterior, PlazaToExteriorTriggerCenter, ExteriorFromPlazaTarget, FastVsHouseArea.Exterior, "plaza to exterior");
            ValidateReachableRoutePad(controller, visibility, FastVsHouseArea.CentralPlaza, plazaToLibrary, PlazaToLibraryTriggerCenter, LibraryFromPlazaTarget, FastVsHouseArea.Library, "plaza to library");
            ValidateReachableRoutePad(controller, visibility, FastVsHouseArea.Library, libraryToPlaza, LibraryToPlazaTriggerCenter, PlazaFromLibraryTarget, FastVsHouseArea.CentralPlaza, "library to plaza");
        }

        private static void ValidateReachableRoutePad(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsHouseArea sourceArea,
            FastVsAreaDoorTransition transition,
            Vector3 triggerCenter,
            Vector3 target,
            FastVsHouseArea expectedArea,
            string label)
        {
            visibility.SetActiveAreaForReview(sourceArea);
            controller.ForcePlayerCurrentLocalForReview(triggerCenter);
            if (!transition.TryEvaluateCurrentPlayerForReview())
            {
                throw new InvalidOperationException($"House slice validation failed: standing on the {label} pad does not trigger travel.");
            }

            if (visibility.ActiveAreaForReview != expectedArea)
            {
                throw new InvalidOperationException($"House slice validation failed: reachable {label} pad did not activate {expectedArea} map set.");
            }

            var local = controller.CurrentSpaceRootForReview.InverseTransformPoint(UnityEngine.Object.FindFirstObjectByType<CharacterController>().transform.position);
            ValidateVectorNear($"reachable {label} target", local, target);
            if (PointInsideBox(target, triggerCenter, transition.TriggerLocalSizeForReview))
            {
                throw new InvalidOperationException($"House slice validation failed: {label} spawn target is still inside the source trigger and may bounce back.");
            }
        }

        private static void ValidateCameraStaysOnSameCoordinateRoot(TimeWindowPairedSpacePortalController controller)
        {
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            if (guide == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing visual direction guide.");
            }

            controller.ForcePlayerOtherTimeLocalForReview(HouseInteriorPlayerStart);
            guide.ApplyActiveTimeIsolationForReview();
            var expectedSameCoordinateAnchor = controller.CurrentSpaceRootForReview.TransformPoint(HouseInteriorPlayerStart);
            ValidateWorldVectorNear("same-coordinate past camera anchor", guide.ResolveActiveCameraAnchorForReview(), expectedSameCoordinateAnchor, 0.05f);
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing main camera for active-time culling validation.");
            }

            var currentBit = 1 << controller.CurrentSpaceRenderLayerForReview;
            var otherBit = 1 << controller.OtherTimeSpaceRenderLayerForReview;
            var portalBit = 1 << controller.PortalFrameRenderLayerForReview;
            var playerBit = 1 << controller.PlayerVisibleRenderLayerForReview;
            if (controller.CurrentSpaceRootForReview == null ||
                controller.OtherTimeSpaceRootForReview == null ||
                controller.CurrentSpaceRootForReview.gameObject.layer != controller.CurrentSpaceRenderLayerForReview ||
                controller.OtherTimeSpaceRootForReview.gameObject.layer != controller.OtherTimeSpaceRenderLayerForReview ||
                controller.PlayerRenderLayerForReview != controller.PlayerVisibleRenderLayerForReview)
            {
                throw new InvalidOperationException("House slice validation failed: startup render layers should isolate current, past, and player before any Time Window is created.");
            }

            if ((camera.cullingMask & currentBit) == 0 ||
                (camera.cullingMask & portalBit) == 0 ||
                (camera.cullingMask & playerBit) != 0 ||
                (camera.cullingMask & otherBit) != 0)
            {
                throw new InvalidOperationException("House slice validation failed: other-time player must be hidden from the main camera and restricted to the Time Window aperture.");
            }

            if (controller.PlayerRenderLayerForReview != controller.PlayerVisibleRenderLayerForReview)
            {
                throw new InvalidOperationException("House slice validation failed: player becomes hidden by Time Window culling after entering the Time Window.");
            }

            controller.ForcePlayerCurrentLocalForReview(HouseInteriorPlayerStart);
            guide.ApplyActiveTimeIsolationForReview();
            var expectedCurrentAnchor = controller.CurrentSpaceRootForReview.TransformPoint(HouseInteriorPlayerStart);
            ValidateWorldVectorNear("current camera anchor", guide.ResolveActiveCameraAnchorForReview(), expectedCurrentAnchor, 0.05f);
            if ((camera.cullingMask & currentBit) == 0 ||
                (camera.cullingMask & portalBit) == 0 ||
                (camera.cullingMask & playerBit) == 0 ||
                (camera.cullingMask & otherBit) != 0)
            {
                throw new InvalidOperationException("House slice validation failed: current camera culling no longer keeps past outside the window hidden.");
            }
        }

        private static void ValidateInitialCurrentOnlyCulling(TimeWindowPairedSpacePortalController controller)
        {
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing main camera for initial culling validation.");
            }

            var currentBit = 1 << controller.CurrentSpaceRenderLayerForReview;
            var otherBit = 1 << controller.OtherTimeSpaceRenderLayerForReview;
            var portalBit = 1 << controller.PortalFrameRenderLayerForReview;
            var playerBit = 1 << controller.PlayerVisibleRenderLayerForReview;
            if ((camera.cullingMask & currentBit) == 0 ||
                (camera.cullingMask & portalBit) == 0 ||
                (camera.cullingMask & playerBit) == 0 ||
                (camera.cullingMask & otherBit) != 0)
            {
                throw new InvalidOperationException("House slice validation failed: startup camera culling should show current/player/portal layers while hiding past.");
            }
        }

        private static void ValidateInvisibleDropGuard(string objectName)
        {
            var guard = FindSceneObjectIncludingInactive(objectName);
            if (guard == null ||
                guard.GetComponent<BoxCollider>() == null ||
                guard.GetComponent<Renderer>() != null)
            {
                throw new InvalidOperationException($"House slice validation failed: invisible drop guard is missing or visible: {objectName}");
            }
        }

        private static void ValidateNoDebugWorldLabel(string objectName)
        {
            if (FindSceneObjectIncludingInactive(objectName) != null)
            {
                throw new InvalidOperationException($"House slice validation failed: debug world label must not remain visible in the prototype: {objectName}");
            }
        }

        private static void ValidateLibraryMapLayout(string floorName, string backWallName, string leftBalconyName, string rightBalconyName, string backGalleryName, string railingName)
        {
            var floor = FindSceneObjectIncludingInactive(floorName);
            if (floor == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing library floor: {floorName}");
            }

            if (floor.transform.localScale.x < 11.2f || floor.transform.localScale.x > 12.4f)
            {
                throw new InvalidOperationException($"House slice validation failed: library floor width is outside the intended narrow range: {floorName}");
            }

            if (floor.transform.localScale.z < 14.5f)
            {
                throw new InvalidOperationException($"House slice validation failed: library floor is not deep enough: {floorName}");
            }

            if (FindSceneObjectIncludingInactive(backWallName) == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing library back wall: {backWallName}");
            }

            if (FindSceneObjectIncludingInactive(leftBalconyName) == null ||
                FindSceneObjectIncludingInactive(rightBalconyName) == null ||
                FindSceneObjectIncludingInactive(backGalleryName) == null ||
                FindSceneObjectIncludingInactive(railingName) == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing library second-floor blockout pieces for {floorName}");
            }
        }

        private static void ValidateLibraryEntryAlignment(string currentThresholdName, string pastThresholdName)
        {
            ValidateLibraryEntryAlignmentForThreshold(currentThresholdName);
            ValidateLibraryEntryAlignmentForThreshold(pastThresholdName);
        }

        private static void ValidateLibraryEntryDoorPanelsRemoved()
        {
            var removed = new[]
            {
                "Current_Library_EntryDoorTexturePanel",
                "Current_Library_EntryDoorTextureTopTrim",
                "Past_Library_EntryDoorTexturePanel",
                "Past_Library_EntryDoorTextureTopTrim"
            };

            foreach (var objectName in removed)
            {
                if (FindSceneObjectIncludingInactive(objectName) != null)
                {
                    throw new InvalidOperationException($"House slice validation failed: obsolete interior library entry panel must not exist: {objectName}");
                }
            }
        }

        private static void ValidateCentralPlazaMarketStallPresentation()
        {
            var awning = FindSceneObjectIncludingInactive("Past_CentralPlaza_MarketStallCloth");
            if (awning == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing past central plaza market awning.");
            }

            var renderer = awning.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.name.IndexOf("laundry_accent", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("House slice validation failed: past market awning must use a colored material, not the white laundry material.");
            }

            var requiredSupports = new[]
            {
                "Past_CentralPlaza_MarketStallCounter",
                "Past_CentralPlaza_MarketStallPostBackLeft",
                "Past_CentralPlaza_MarketStallPostBackRight",
                "Past_CentralPlaza_MarketStallPostFrontLeft",
                "Past_CentralPlaza_MarketStallPostFrontRight"
            };

            foreach (var objectName in requiredSupports)
            {
                if (FindSceneObjectIncludingInactive(objectName) == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: market awning must not look like a floating box; missing support: {objectName}");
                }
            }
        }

        private static void ValidatePastLibraryBackWallBookRuns()
        {
            var panel = FindSceneObjectIncludingInactive("Past_Library_BackWallBookshelfFrontTexturePanel");
            var renderer = panel != null ? panel.GetComponent<Renderer>() : null;
            if (renderer == null ||
                renderer.sharedMaterial == null ||
                renderer.sharedMaterial.name.IndexOf("painted_hd2d", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("House slice validation failed: past back-wall bookshelf must use the painted_hd2d front-facing bookshelf texture panel.");
            }
        }

        private static void ValidateCentralPlazaLibraryFacadeDetails(string prefix)
        {
            var required = new[]
            {
                $"{prefix}_CentralPlaza_LibraryDoorFrameTop",
                $"{prefix}_CentralPlaza_LibraryDoorFrameLeft",
                $"{prefix}_CentralPlaza_LibraryDoorFrameRight",
                $"{prefix}_CentralPlaza_LibraryDoorPanelsLeft",
                $"{prefix}_CentralPlaza_LibraryDoorPanelsRight",
                $"{prefix}_CentralPlaza_LibraryDoorCenterPlank",
                $"{prefix}_CentralPlaza_LibraryDoorHandle",
                $"{prefix}_CentralPlaza_LibraryWindowLeftFrameTop",
                $"{prefix}_CentralPlaza_LibraryWindowLeftFrameBottom",
                $"{prefix}_CentralPlaza_LibraryWindowLeftFrameLeft",
                $"{prefix}_CentralPlaza_LibraryWindowLeftFrameRight",
                $"{prefix}_CentralPlaza_LibraryWindowLeftMullionVertical",
                $"{prefix}_CentralPlaza_LibraryWindowLeftMullionHorizontal",
                $"{prefix}_CentralPlaza_LibraryWindowLeftPaneUpperLeft",
                $"{prefix}_CentralPlaza_LibraryWindowLeftPaneUpperRight",
                $"{prefix}_CentralPlaza_LibraryWindowLeftPaneLowerLeft",
                $"{prefix}_CentralPlaza_LibraryWindowLeftPaneLowerRight",
                $"{prefix}_CentralPlaza_LibraryWindowRightFrameTop",
                $"{prefix}_CentralPlaza_LibraryWindowRightFrameBottom",
                $"{prefix}_CentralPlaza_LibraryWindowRightFrameLeft",
                $"{prefix}_CentralPlaza_LibraryWindowRightFrameRight",
                $"{prefix}_CentralPlaza_LibraryWindowRightMullionVertical",
                $"{prefix}_CentralPlaza_LibraryWindowRightMullionHorizontal",
                $"{prefix}_CentralPlaza_LibraryWindowRightPaneUpperLeft",
                $"{prefix}_CentralPlaza_LibraryWindowRightPaneUpperRight",
                $"{prefix}_CentralPlaza_LibraryWindowRightPaneLowerLeft",
                $"{prefix}_CentralPlaza_LibraryWindowRightPaneLowerRight"
            };

            foreach (var objectName in required)
            {
                if (FindSceneObjectIncludingInactive(objectName) == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: missing central plaza library facade child piece: {objectName}");
                }
            }

            var obsoleteFlatParts = new[]
            {
                $"{prefix}_CentralPlaza_LibraryEntranceDoor",
                $"{prefix}_CentralPlaza_LibraryWindowLeft",
                $"{prefix}_CentralPlaza_LibraryWindowRight"
            };

            foreach (var objectName in obsoleteFlatParts)
            {
                if (FindSceneObjectIncludingInactive(objectName) != null)
                {
                    throw new InvalidOperationException($"House slice validation failed: obsolete central plaza library facade blockout must not remain: {objectName}");
                }
            }

            var doorPanel = FindSceneObjectIncludingInactive($"{prefix}_CentralPlaza_LibraryDoorPanelsLeft");
            var doorRenderer = doorPanel != null ? doorPanel.GetComponent<Renderer>() : null;
            if (doorRenderer == null || doorRenderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing central plaza library door panel material: {prefix}");
            }

            if (prefix == "Current" &&
                doorRenderer.sharedMaterial.name.IndexOf("doorway_dark", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new InvalidOperationException("House slice validation failed: current central plaza library door must not be the black doorway material.");
            }

            ValidateCentralPlazaLibraryWindowPaneMaterials(prefix, prefix == "Past" ? "window_light" : "empty_window");
        }

        private static void ValidateCentralPlazaLibraryWindowPaneMaterials(string prefix, string expectedMaterialToken)
        {
            var paneObjects = new[]
            {
                $"{prefix}_CentralPlaza_LibraryWindowLeftPaneUpperLeft",
                $"{prefix}_CentralPlaza_LibraryWindowLeftPaneUpperRight",
                $"{prefix}_CentralPlaza_LibraryWindowLeftPaneLowerLeft",
                $"{prefix}_CentralPlaza_LibraryWindowLeftPaneLowerRight",
                $"{prefix}_CentralPlaza_LibraryWindowRightPaneUpperLeft",
                $"{prefix}_CentralPlaza_LibraryWindowRightPaneUpperRight",
                $"{prefix}_CentralPlaza_LibraryWindowRightPaneLowerLeft",
                $"{prefix}_CentralPlaza_LibraryWindowRightPaneLowerRight"
            };

            foreach (var objectName in paneObjects)
            {
                var pane = FindSceneObjectIncludingInactive(objectName);
                var paneRenderer = pane != null ? pane.GetComponent<Renderer>() : null;
                if (paneRenderer == null ||
                    paneRenderer.sharedMaterial == null ||
                    paneRenderer.sharedMaterial.name.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    throw new InvalidOperationException($"House slice validation failed: central plaza library window panes must use {expectedMaterialToken} instead of frame/tile material: {objectName}");
                }
            }
        }

        private static void ValidateCentralPlazaFountainWaterPresentation(string objectName, string expectedMaterialToken)
        {
            var water = FindSceneObjectIncludingInactive(objectName);
            if (water == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing central plaza fountain water object: {objectName}");
            }

            var renderer = water.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.name.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a {expectedMaterialToken} material instead of a white box.");
            }
        }

        private static void ValidateCurrentCentralPlazaDryFountainDetails()
        {
            var requiredWood = new[]
            {
                "Current_CentralPlaza_DryFountainWoodPlankA",
                "Current_CentralPlaza_DryFountainWoodPlankB"
            };

            foreach (var objectName in requiredWood)
            {
                var plank = FindSceneObjectIncludingInactive(objectName);
                var renderer = plank != null ? plank.GetComponent<Renderer>() : null;
                if (renderer == null ||
                    renderer.sharedMaterial == null ||
                    (renderer.sharedMaterial.name.IndexOf("current_furniture", StringComparison.OrdinalIgnoreCase) < 0 &&
                     renderer.sharedMaterial.name.IndexOf("current_fence", StringComparison.OrdinalIgnoreCase) < 0))
                {
                    throw new InvalidOperationException($"House slice validation failed: dry fountain top should use visible wood debris, not a black bar: {objectName}");
                }
            }

            var crack = FindSceneObjectIncludingInactive("Current_CentralPlaza_DryFountainCrack");
            if (crack == null || crack.transform.localScale.x > 0.82f || crack.transform.localScale.y > 0.05f)
            {
                throw new InvalidOperationException("House slice validation failed: dry fountain crack must remain a small detail, not a large black bar.");
            }
        }

        private static void ValidateLibrarySideBookshelves()
        {
            ValidateLibrarySideBookshelfPair("Left");
            ValidateLibrarySideBookshelfPair("Right");
        }

        private static void ValidateLibrarySideBookshelfPair(string side)
        {
            var currentRoot = FindSceneObjectIncludingInactive($"Current_Library_{side}SideBookshelf");
            var pastRoot = FindSceneObjectIncludingInactive($"Past_Library_{side}SideBookshelf");
            if (currentRoot == null || pastRoot == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing current/past library side bookshelf roots: {side}");
            }

            ValidateVectorNear($"{side} library side bookshelf root position", currentRoot.transform.localPosition, pastRoot.transform.localPosition);
            if (Quaternion.Angle(currentRoot.transform.localRotation, pastRoot.transform.localRotation) > 0.02f)
            {
                throw new InvalidOperationException($"House slice validation failed: current/past {side.ToLowerInvariant()} library side bookshelf roots no longer share the same rotation.");
            }

            ValidateLibrarySideBookshelfFrame(currentRoot.transform, "Current", side);
            ValidateLibrarySideBookshelfFrame(pastRoot.transform, "Past", side);
            ValidateLibrarySideBookshelfFrameParity(currentRoot.transform, pastRoot.transform, side);

            if (currentRoot.transform.childCount != 8)
            {
                throw new InvalidOperationException($"House slice validation failed: current {side.ToLowerInvariant()} library side bookshelf should stay empty and only expose the frame boards.");
            }

            if (pastRoot.transform.childCount < 9)
            {
                throw new InvalidOperationException($"House slice validation failed: past {side.ToLowerInvariant()} library side bookshelf should keep the frame plus front-facing bookshelf texture panels.");
            }

            if (HasBookDescendant(currentRoot.transform))
            {
                throw new InvalidOperationException($"House slice validation failed: current {side.ToLowerInvariant()} library side bookshelf must not contain book children.");
            }

            if (!HasBookDescendant(pastRoot.transform))
            {
                throw new InvalidOperationException($"House slice validation failed: past {side.ToLowerInvariant()} library side bookshelf must contain book children.");
            }

            var texturePanel = pastRoot.transform.Find($"{pastRoot.name}_BookshelfFrontTexturePanel");
            var textureRenderer = texturePanel != null ? texturePanel.GetComponent<Renderer>() : null;
            if (textureRenderer == null ||
                textureRenderer.sharedMaterial == null ||
                textureRenderer.sharedMaterial.name.IndexOf("painted_hd2d", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: past {side.ToLowerInvariant()} library side bookshelf must use the painted_hd2d front-facing bookshelf texture panel.");
            }

            ValidateVectorNear($"{side} past side bookshelf texture panel position", texturePanel.localPosition, new Vector3(0f, LibrarySideShelfTexturePanelCenterY, 0.70f));
            ValidateVectorNear($"{side} past side bookshelf texture panel scale", texturePanel.localScale, new Vector3(LibrarySideShelfRunLength - 0.38f, LibrarySideShelfTexturePanelHeight, 0.040f));

            for (var row = 0; row < 3; row++)
            {
                if (currentRoot.transform.Find($"{currentRoot.name}_Row{row}") != null)
                {
                    throw new InvalidOperationException($"House slice validation failed: current {side.ToLowerInvariant()} library side bookshelf must remain empty.");
                }
            }
        }

        private static void ValidateLibrarySideBookshelfFrame(Transform shelfRoot, string prefix, string side)
        {
            var sideToken = side.ToLowerInvariant();
            var halfRun = LibrarySideShelfRunLength * 0.5f;
            var postX = halfRun - 0.07f;
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_BackPanel", new Vector3(0f, LibrarySideShelfBackPanelCenterY, 0.08f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfBackPanelHeight, 1.16f), $"{prefix}.library.{sideToken}.shelf.back_panel");
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_LeftPost", new Vector3(-postX, LibrarySideShelfPostCenterY, 0.02f), new Vector3(LibrarySideShelfPostThickness, LibrarySideShelfPostHeight, LibrarySideShelfPostThickness), $"{prefix}.library.{sideToken}.shelf.left_post");
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_RightPost", new Vector3(postX, LibrarySideShelfPostCenterY, 0.02f), new Vector3(LibrarySideShelfPostThickness, LibrarySideShelfPostHeight, LibrarySideShelfPostThickness), $"{prefix}.library.{sideToken}.shelf.right_post");
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_TopCap", new Vector3(0f, LibrarySideShelfTopCapCenterY, 0.02f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfCapThickness, 0.16f), $"{prefix}.library.{sideToken}.shelf.top_cap");
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_BottomBase", new Vector3(0f, 0.06f, 0.02f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfCapThickness, 0.16f), $"{prefix}.library.{sideToken}.shelf.bottom_base");

            for (var row = 0; row < 3; row++)
            {
                var rowY = LibrarySideShelfBoardFirstY + row * LibrarySideShelfBoardStepY;
                ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_ShelfBoard_{row}", new Vector3(0f, rowY, 0.00f), new Vector3(LibrarySideShelfRunLength - 0.10f, LibrarySideShelfBoardThickness, LibrarySideShelfBoardDepth), $"{prefix}.library.{sideToken}.shelf.board.{row}");
            }

            var topCap = shelfRoot.Find($"{shelfRoot.name}_TopCap");
            if (topCap == null ||
                shelfRoot.localPosition.y + topCap.localPosition.y + topCap.localScale.y * 0.5f > 2.02f)
            {
                throw new InvalidOperationException($"House slice validation failed: {prefix.ToLowerInvariant()} {side.ToLowerInvariant()} library side bookshelf penetrates the second-floor balcony.");
            }
        }

        private static void ValidateLibrarySideBookshelfFrameParity(Transform currentRoot, Transform pastRoot, string side)
        {
            var childNames = new[]
            {
                "BackPanel",
                "LeftPost",
                "RightPost",
                "TopCap",
                "BottomBase",
                "ShelfBoard_0",
                "ShelfBoard_1",
                "ShelfBoard_2"
            };

            foreach (var suffix in childNames)
            {
                var currentChild = currentRoot.Find($"{currentRoot.name}_{suffix}");
                var pastChild = pastRoot.Find($"{pastRoot.name}_{suffix}");
                if (currentChild == null || pastChild == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: library side bookshelf frame child missing for {side.ToLowerInvariant()}: {suffix}");
                }

                ValidateVectorNear($"{side} bookshelf {suffix} local position", currentChild.localPosition, pastChild.localPosition);
                ValidateVectorNear($"{side} bookshelf {suffix} local scale", currentChild.localScale, pastChild.localScale);
                if (Quaternion.Angle(currentChild.localRotation, pastChild.localRotation) > 0.02f)
                {
                    throw new InvalidOperationException($"House slice validation failed: library side bookshelf frame child rotation drifted for {side.ToLowerInvariant()}: {suffix}");
                }
            }
        }

        private static void ValidateLibraryShelfChild(Transform shelfRoot, string childName, Vector3 expectedLocalPosition, Vector3 expectedLocalScale, string landmarkId)
        {
            var child = shelfRoot.Find(childName);
            if (child == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing library side bookshelf child: {landmarkId}");
            }

            ValidateVectorNear($"{landmarkId} position", child.localPosition, expectedLocalPosition);
            ValidateVectorNear($"{landmarkId} scale", child.localScale, expectedLocalScale);
        }

        private static bool HasBookDescendant(Transform root)
        {
            foreach (Transform target in root.GetComponentsInChildren<Transform>(true))
            {
                if (target != root && target.name.IndexOf("_Book", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static void ValidatePastLibraryReadingTableBooks()
        {
            var ids = new[] { "LeftFront", "CenterFront", "RightFront", "LeftRear", "CenterRear", "RightRear" };
            foreach (var id in ids)
            {
                ValidatePastLibraryReadingTableSize($"Past_Library_ReadingTableClean_{id}");
                ValidateTabletopBookHeight($"Past_Library_ReadingTableClean_{id}_BookA");
                ValidateTabletopBookHeight($"Past_Library_ReadingTableClean_{id}_BookB");
            }
        }

        private static void ValidatePastLibraryReadingTableSize(string objectName)
        {
            var table = FindSceneObjectIncludingInactive(objectName);
            if (table == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing past clean reading table: {objectName}");
            }

            ValidateVectorNear($"{objectName} scale", table.transform.localScale, PastLibraryReadingTableCleanScale);
        }

        private static void ValidateTabletopBookHeight(string objectName)
        {
            var book = FindSceneObjectIncludingInactive(objectName);
            if (book == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing readable book prop: {objectName}");
            }

            ValidateTabletopBookHeight(objectName, book.transform.localPosition.y);
        }

        private static void ValidateTabletopBookHeight(string objectName, float actualY)
        {
            const float minLocalY = 0.385f;
            const float maxLocalY = 0.430f;
            if (actualY < minLocalY || actualY > maxLocalY)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must rest on a table surface (y={actualY:0.000}, expected {minLocalY:0.000}-{maxLocalY:0.000}).");
            }
        }

        private static void ValidateFloorGlowCuePresentation(string objectName, GameObject cue, Vector3 expectedLocalPosition, Vector3 expectedLocalScale)
        {
            ValidateVectorNear($"{objectName} placement", cue.transform.localPosition, expectedLocalPosition);
            ValidateVectorNear($"{objectName} scale", cue.transform.localScale, expectedLocalScale);

            if (cue.GetComponent<FastVsMapMoveGlowPulse>() == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must move like the map-transition floor glow.");
            }

            if (cue.GetComponent<TimeWindowPairedSpaceLandmark>() == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay a paired-space landmark.");
            }

            if (cue.GetComponent<Collider>() != null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay a floor light, not a solid cube.");
            }

            var meshFilter = cue.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null || meshFilter.sharedMesh.name.IndexOf("Cylinder", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use the round floor-light primitive.");
            }

            var renderer = cue.GetComponent<Renderer>();
            var material = renderer != null ? renderer.sharedMaterial : null;
            if (material == null ||
                material.name.IndexOf("timewindow_cue_yellow_light", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use the yellow Time Window cue material, not the orange map-transition light.");
            }
        }

        private static void ValidatePastLibraryTargetBookMarkerMotion()
        {
            var marker = FindSceneObjectIncludingInactive("Past_Library_TargetBook_RedCubeMarker");
            if (marker == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing past target book red cube marker.");
            }

            if (marker.GetComponent<FastVsMapMoveGlowPulse>() == null)
            {
                throw new InvalidOperationException("House slice validation failed: past target book red cube marker must keep subtle motion.");
            }

            ValidateRedCubeMarkerWithOutline(marker, "Past_Library_TargetBook_RedCubeMarker");
            ValidateRedCubeMarkerWithOutline(FindSceneObjectIncludingInactive("Past_Library_Aria_RedCubeMarker"), "Past_Library_Aria_RedCubeMarker");
        }

        private static void ValidateRedCubeMarkerWithOutline(GameObject marker, string objectName)
        {
            if (marker == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing red cube marker: {objectName}.");
            }

            if (marker.GetComponent<FastVsMapMoveGlowPulse>() == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep subtle motion.");
            }

            var fill = marker.transform.Find($"{objectName}_Fill");
            if (fill == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a yellow fill child.");
            }

            ValidateVectorNear($"{objectName} fill scale", fill.localScale, PastLibraryTargetBookMarkerScale);

            var meshFilter = fill.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null || meshFilter.sharedMesh.name.IndexOf("Cube", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} yellow fill must remain a cube.");
            }

            var renderer = fill.GetComponent<Renderer>();
            var markerMaterial = renderer != null ? renderer.sharedMaterial : null;
            var hasBaseMap = markerMaterial != null && markerMaterial.HasProperty("_BaseMap") && markerMaterial.GetTexture("_BaseMap") != null;
            var hasMainTex = markerMaterial != null && markerMaterial.HasProperty("_MainTex") && markerMaterial.GetTexture("_MainTex") != null;
            if (markerMaterial == null ||
                markerMaterial.name.IndexOf("timewindow_marker_yellow", StringComparison.OrdinalIgnoreCase) < 0 ||
                hasBaseMap ||
                hasMainTex)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a flat yellow marker material without mosaic texture.");
            }

            var bangStem = marker.transform.Find($"{objectName}_BangFrontStem");
            var bangDot = marker.transform.Find($"{objectName}_BangFrontDot");
            if (bangStem == null ||
                bangDot == null ||
                bangStem.localPosition.z >= -PastLibraryTargetBookMarkerScale.z * 0.48f ||
                bangDot.localPosition.z >= -PastLibraryTargetBookMarkerScale.z * 0.48f)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must include a front-side exclamation mark glyph.");
            }

            var outlineNames = new[]
            {
                "EdgeX_TopFront",
                "EdgeX_TopBack",
                "EdgeX_BottomFront",
                "EdgeX_BottomBack",
                "EdgeY_LeftFront",
                "EdgeY_LeftBack",
                "EdgeY_RightFront",
                "EdgeY_RightBack",
                "EdgeZ_LeftTop",
                "EdgeZ_LeftBottom",
                "EdgeZ_RightTop",
                "EdgeZ_RightBottom"
            };

            foreach (var outlineName in outlineNames)
            {
                var edge = marker.transform.Find($"{objectName}_{outlineName}");
                if (edge == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: {objectName} is missing black frame piece: {outlineName}.");
                }

                var smallestAxis = Mathf.Min(edge.localScale.x, Mathf.Min(edge.localScale.y, edge.localScale.z));
                if (smallestAxis > 0.0085f)
                {
                    throw new InvalidOperationException($"House slice validation failed: {objectName} frame piece is too thick: {outlineName}.");
                }
            }
        }

        private static void ValidateCurrentLibraryRuinObjects()
        {
            var currentObjects = new[]
            {
                "Current_Library_Ruin_ScatteredBoardPile",
                "Current_Library_Ruin_ToppledBookStack",
                "Current_Library_Ruin_DustSheetNearEntry",
                "Current_Library_Ruin_BrokenBackShelfBoard",
                "Current_Library_Ruin_FallenBookSpines"
            };

            var pastObjects = new[]
            {
                "Past_Library_Ruin_CollapsedShelfPile",
                "Past_Library_Ruin_ScatteredBoardPile",
                "Past_Library_Ruin_ToppledBookStack",
                "Past_Library_Ruin_DustSheetNearEntry",
                "Past_Library_Ruin_BrokenBackShelfBoard",
                "Past_Library_Ruin_FallenBookSpines"
            };

            foreach (var objectName in currentObjects)
            {
                if (FindSceneObjectIncludingInactive(objectName) == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: missing current-side library ruin object: {objectName}");
                }
            }

            if (FindSceneObjectIncludingInactive("Current_Library_Ruin_CollapsedShelfPile") != null)
            {
                throw new InvalidOperationException("House slice validation failed: current library collapsed shelf pile must not overlap the left side bookshelf.");
            }

            var dustSheet = FindSceneObjectIncludingInactive("Current_Library_Ruin_DustSheetNearEntry");
            if (dustSheet != null &&
                (Mathf.Abs(dustSheet.transform.localPosition.x - LibraryVsCenter.x) < 2.4f ||
                 dustSheet.transform.localScale.x > 0.95f ||
                 dustSheet.transform.localScale.z > 0.45f))
            {
                throw new InvalidOperationException("House slice validation failed: current library entry dust sheet must stay small and off the entrance centerline.");
            }

            var boardPile = FindSceneObjectIncludingInactive("Current_Library_Ruin_ScatteredBoardPile");
            if (boardPile != null &&
                (boardPile.transform.localScale.x > 0.98f ||
                 boardPile.transform.localScale.y > 0.12f ||
                 boardPile.transform.localScale.z > 0.30f ||
                 FindSceneObjectIncludingInactive("Current_Library_Ruin_ScatteredBoardPile_PlankA") == null ||
                 FindSceneObjectIncludingInactive("Current_Library_Ruin_ScatteredBoardPile_PlankB") == null ||
                 FindSceneObjectIncludingInactive("Current_Library_Ruin_ScatteredBoardPile_DustPatch") == null))
            {
                throw new InvalidOperationException("House slice validation failed: current library scattered boards must be small separate debris, not one large flat slab.");
            }

            var toppledBookStack = FindSceneObjectIncludingInactive("Current_Library_Ruin_ToppledBookStack");
            if (toppledBookStack != null &&
                (toppledBookStack.transform.localPosition.y > 0.22f ||
                 toppledBookStack.transform.localPosition.z > LibraryVsCenter.z + 0.90f ||
                 toppledBookStack.transform.localScale.y > 0.20f))
            {
                throw new InvalidOperationException("House slice validation failed: current library toppled book stack must stay low and away from the right-back bookshelf silhouette.");
            }

            foreach (var objectName in pastObjects)
            {
                if (FindSceneObjectIncludingInactive(objectName) != null)
                {
                    throw new InvalidOperationException($"House slice validation failed: past-side library ruin object should not exist: {objectName}");
                }
            }
        }

        private static void ValidateLibraryEntryAlignmentForThreshold(string thresholdName)
        {
            var threshold = FindSceneObjectIncludingInactive(thresholdName);
            if (threshold == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing library entry threshold: {thresholdName}");
            }

            if (Mathf.Abs(threshold.transform.localPosition.z - LibraryToPlazaTriggerCenter.z) > 0.25f)
            {
                throw new InvalidOperationException($"House slice validation failed: library entry threshold drifted away from the route trigger: {thresholdName}");
            }

            if (threshold.transform.localScale.x > 1.35f ||
                threshold.transform.localScale.y > 0.06f ||
                threshold.transform.localScale.z > 0.34f ||
                threshold.GetComponent<Collider>() != null)
            {
                throw new InvalidOperationException($"House slice validation failed: library entry threshold must remain a small flat floor strip, not a wall/slab: {thresholdName}");
            }

            if (LibraryFromPlazaTarget.z < LibraryToPlazaTriggerCenter.z + 0.85f ||
                LibraryFromPlazaTarget.z > LibraryToPlazaTriggerCenter.z + 1.70f)
            {
                throw new InvalidOperationException("House slice validation failed: library return target is no longer landing just inside the entry threshold.");
            }
        }

        private static void ValidateFountainNoStepCollider(string objectName)
        {
            var guard = FindSceneObjectIncludingInactive(objectName);
            var collider = guard != null ? guard.GetComponent<BoxCollider>() : null;
            if (guard == null || collider == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing fountain no-step collider: {objectName}");
            }

            if (collider.size.x < 2.05f || collider.size.z < 2.05f || collider.size.y < 1.20f)
            {
                throw new InvalidOperationException($"House slice validation failed: fountain no-step collider is too small: {objectName}");
            }

            if (guard.transform.localPosition.y < 0.65f)
            {
                throw new InvalidOperationException($"House slice validation failed: fountain no-step collider must block the player body, not only the floor: {objectName}");
            }
        }

        private static void ValidateCentralPlazaStoneSquareAndFountainLayout(string stoneSquareName, string fountainNoStepColliderName)
        {
            var stoneSquare = FindSceneObjectIncludingInactive(stoneSquareName);
            if (stoneSquare == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing central plaza stone square: {stoneSquareName}");
            }

            if (stoneSquare.transform.localScale.x < 12.0f || stoneSquare.transform.localScale.z < 11.5f)
            {
                throw new InvalidOperationException($"House slice validation failed: central plaza stone square is too small: {stoneSquareName}");
            }

            var fountainNoStepCollider = FindSceneObjectIncludingInactive(fountainNoStepColliderName);
            var collider = fountainNoStepCollider != null ? fountainNoStepCollider.GetComponent<BoxCollider>() : null;
            if (fountainNoStepCollider == null || collider == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing fountain no-step collider for central plaza layout validation: {fountainNoStepColliderName}");
            }

            if (Mathf.Abs(fountainNoStepCollider.transform.localPosition.z - stoneSquare.transform.localPosition.z) > 0.35f)
            {
                throw new InvalidOperationException($"House slice validation failed: fountain no-step collider drifted away from the stone square center: {fountainNoStepColliderName}");
            }

            if (collider.size.x < 2.05f || collider.size.z < 2.05f || collider.size.y < 1.20f)
            {
                throw new InvalidOperationException($"House slice validation failed: fountain no-step collider size regressed: {fountainNoStepColliderName}");
            }

            if (fountainNoStepCollider.transform.localPosition.y < 0.65f)
            {
                throw new InvalidOperationException($"House slice validation failed: fountain no-step collider must remain tall enough to block the player body: {fountainNoStepColliderName}");
            }
        }

        private static void ValidateApertureIntersectingObjectSuppression(TimeWindowPairedSpacePortalController controller)
        {
            var portalLocal = controller.PortalLocalCenterForReview;
            controller.ClosePortal();

            GameObject currentProbe = null;
            GameObject pastProbe = null;
            try
            {
                var probeMaterial = FlatMaterial("aperture_overlap_probe_validation", new Color(1f, 0.1f, 0.8f, 1f), true);
                currentProbe = CreateLandmarkCube(
                    "Current_ApertureOverlapProbe_Validation",
                    controller.CurrentSpaceRootForReview,
                    portalLocal,
                    new Vector3(0.44f, 0.60f, 0.20f),
                    Quaternion.identity,
                    probeMaterial,
                    false,
                    TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                    "Current.validation.aperture_overlap_probe");
                pastProbe = CreateLandmarkCube(
                    "Past_ApertureOverlapProbe_Validation",
                    controller.OtherTimeSpaceRootForReview,
                    portalLocal,
                    new Vector3(0.44f, 0.60f, 0.20f),
                    Quaternion.identity,
                    probeMaterial,
                    false,
                    TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                    "Past.validation.aperture_overlap_probe");

                if (!controller.TryOpenPortalForTests(DragStart, DragEnd))
                {
                    throw new InvalidOperationException("House slice validation failed: TimeWindow drag-open failed during aperture suppression validation.");
                }

                var currentRenderer = currentProbe.GetComponent<Renderer>();
                var pastRenderer = pastProbe.GetComponent<Renderer>();
                if (currentRenderer == null ||
                    pastRenderer == null ||
                    currentRenderer.enabled ||
                    pastRenderer.enabled ||
                    controller.ApertureSuppressedRendererCountForReview < 2)
                {
                    throw new InvalidOperationException("House slice validation failed: objects intersecting the Time Window aperture were not hidden.");
                }
            }
            finally
            {
                if (currentProbe != null)
                {
                    UnityEngine.Object.DestroyImmediate(currentProbe);
                }

                if (pastProbe != null)
                {
                    UnityEngine.Object.DestroyImmediate(pastProbe);
                }

                controller.ClosePortal();
                if (!controller.TryOpenPortalForTests(DragStart, DragEnd))
                {
                    throw new InvalidOperationException("House slice validation failed: TimeWindow drag-open failed after aperture suppression validation.");
                }
            }
        }

        private static void ValidateApertureBottomSitsAboveFloor(TimeWindowPairedSpacePortalController controller)
        {
            if (controller.PortalBottomLocalYForReview < 0.055f)
            {
                throw new InvalidOperationException("House slice validation failed: TimeWindow aperture bottom is embedded below the visible floor top.");
            }
        }

        private static void ValidateAperturePlayerLayerCulling(TimeWindowPairedSpacePortalController controller, bool playerInOtherTime)
        {
            if (controller.PlayerInOtherTime != playerInOtherTime)
            {
                throw new InvalidOperationException("House slice validation failed: requested player culling state does not match Time Window player time side.");
            }

            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            if (guide != null)
            {
                guide.ApplyActiveTimeIsolationForReview();
            }

            controller.RenderPortalAperturesForReview();
            var currentBit = 1 << controller.CurrentSpaceRenderLayerForReview;
            var otherBit = 1 << controller.OtherTimeSpaceRenderLayerForReview;
            var portalBit = 1 << controller.PortalFrameRenderLayerForReview;
            var playerBit = 1 << controller.PlayerVisibleRenderLayerForReview;
            var currentToOtherMask = controller.CurrentToOtherPortalCameraCullingMaskForReview;
            var otherToCurrentMask = controller.OtherToCurrentPortalCameraCullingMaskForReview;
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing main camera for player culling validation.");
            }

            if ((currentToOtherMask & otherBit) == 0 ||
                (currentToOtherMask & currentBit) != 0 ||
                (otherToCurrentMask & currentBit) == 0 ||
                (otherToCurrentMask & otherBit) != 0)
            {
                throw new InvalidOperationException("House slice validation failed: aperture cameras should render only their target time root, not both paired roots.");
            }

            var currentToOtherShouldIncludePlayer = playerInOtherTime;
            var otherToCurrentShouldIncludePlayer = !playerInOtherTime;
            var mainCameraShouldIncludePlayer = !playerInOtherTime;
            if ((mainCamera.cullingMask & currentBit) == 0 ||
                (mainCamera.cullingMask & portalBit) == 0 ||
                (mainCamera.cullingMask & otherBit) != 0 ||
                ((mainCamera.cullingMask & playerBit) != 0) != mainCameraShouldIncludePlayer)
            {
                throw new InvalidOperationException("House slice validation failed: main camera player culling should show Niro only while he is in current time.");
            }

            if (((currentToOtherMask & playerBit) != 0) != currentToOtherShouldIncludePlayer ||
                ((otherToCurrentMask & playerBit) != 0) != otherToCurrentShouldIncludePlayer ||
                controller.CurrentToOtherApertureIncludesPlayerForReview != currentToOtherShouldIncludePlayer ||
                controller.OtherToCurrentApertureIncludesPlayerForReview != otherToCurrentShouldIncludePlayer)
            {
                throw new InvalidOperationException("House slice validation failed: Time Window aperture camera player culling does not match the active player time side.");
            }

            if (!playerInOtherTime && controller.EnabledOtherTimeWallVolumeColliderCountForReview != 0)
            {
                throw new InvalidOperationException("House slice validation failed: other-time Time Window wall colliders are trapping current-side Niro.");
            }

            if (playerInOtherTime && controller.HasGeneratedOtherTimeWallVolumeForReview && controller.EnabledOtherTimeWallVolumeColliderCountForReview == 0)
            {
                throw new InvalidOperationException("House slice validation failed: other-time Time Window wall colliders are disabled after Niro enters past space.");
            }
        }

        private static void ValidateCurrentBackSideEdgeBlock(TimeWindowPairedSpacePortalController controller)
        {
            var player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            if (player == null)
            {
                throw new InvalidOperationException("House slice validation failed: player controller missing for Time Window edge blocker validation.");
            }

            var portalLocal = controller.PortalLocalCenterForReview;
            var portalSize = controller.PortalSizeForReview;
            var blockDepth = Mathf.Max(controller.CurrentBackSideBlockDepthForReview, 0.18f);
            var startLocal = new Vector3(
                portalLocal.x + portalSize.x * 0.5f + 0.08f,
                0.72f,
                portalLocal.z + blockDepth + 0.18f);

            controller.ForcePlayerCurrentLocalForReview(startLocal);
            controller.MovePlayerWorldForReview(controller.CurrentSpaceRootForReview.TransformVector(new Vector3(0f, 0f, -blockDepth - 0.42f)));

            if (!controller.BackSideCrossingRejected)
            {
                throw new InvalidOperationException("House slice validation failed: Time Window back-side edge crossing was not rejected.");
            }

            var currentLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            if (currentLocal.z < portalLocal.z + blockDepth - 0.05f)
            {
                throw new InvalidOperationException("House slice validation failed: player pierced through the Time Window back-side edge blocker.");
            }
        }

        private static void ValidateDirectionalSpriteAnimator()
        {
            var animator = UnityEngine.Object.FindFirstObjectByType<FastVsDirectionalSpriteAnimator>();
            if (animator == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing directional sprite animator on Niro.");
            }

            if (!animator.HasAllDirectionMaterialsForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Niro directional sprite animator is missing one or more direction materials.");
            }

            if (animator.AnimationFrameCountForReview != NiroAnimatedFrameCount)
            {
                throw new InvalidOperationException("House slice validation failed: Niro sprite animation frame count is not configured.");
            }

            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Front, false, false, "niro_front_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Back, false, false, "niro_back_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Left, false, false, "niro_left_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Right, false, false, "niro_right_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Front, true, false, "niro_past_front_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Back, true, false, "niro_past_back_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Left, true, false, "niro_past_left_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Right, true, false, "niro_past_right_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Front, false, true, "niro_walk_front_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Back, false, true, "niro_walk_back_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Left, false, true, "niro_walk_left_sprite");
            ValidateDirectionMaterialSwitch(animator, FastVsCharacterDirection.Right, false, true, "niro_walk_right_sprite");
        }

        private static void ValidateDirectionMaterialSwitch(FastVsDirectionalSpriteAnimator animator, FastVsCharacterDirection direction, bool otherTime, bool moving, string materialId)
        {
            animator.SetPoseForReview(direction, otherTime, moving);
            var material = animator.ActiveMaterialForReview;
            if (material == null || material.name.IndexOf(materialId, StringComparison.OrdinalIgnoreCase) < 0)
            {
                var actual = material != null ? material.name : "<null>";
                throw new InvalidOperationException($"House slice validation failed: Niro direction {direction} otherTime={otherTime} moving={moving} expected material containing {materialId}, got {actual}.");
            }

            if (moving && animator.ActiveFrameCountForReview < NiroAnimatedFrameCount)
            {
                throw new InvalidOperationException($"House slice validation failed: Niro walk material {material.name} has too few frames.");
            }
        }

        private static void ValidatePlayerSpritePresentation()
        {
            var player = GameObject.Find("FastVS_Player_NiroHouseSlice");
            if (player == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing Niro player object.");
            }

            var labels = player.GetComponentsInChildren<TextMesh>(true);
            if (labels.Length > 0)
            {
                throw new InvalidOperationException("House slice validation failed: player still has overhead TextMesh labels.");
            }

            var visual = GameObject.Find("FastVS_PlayerVisual_NiroPaper");
            if (visual == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing Niro visual object.");
            }

            var sprite = FindSceneObjectIncludingInactive("Niro_Sprite64x96");
            if (sprite == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing Niro sprite quad.");
            }

            if (sprite.GetComponent<Renderer>() == null || sprite.GetComponent<Collider>() != null)
            {
                throw new InvalidOperationException("House slice validation failed: Niro sprite quad must keep a renderer and no collider.");
            }

            if (FindSceneObjectIncludingInactive("Niro_SpriteGroundShadow") != null)
            {
                throw new InvalidOperationException("House slice validation failed: player still has a black ground-shadow strip at the feet.");
            }

            if (FindSceneObjectIncludingInactive("FastVS_PlayerSpriteShadingOverlay_Niro") != null)
            {
                throw new InvalidOperationException("House slice validation failed: Niro sprite shading overlay should not exist because it reads as a dark rectangle in review.");
            }

            var pocketGlow = FindSceneObjectIncludingInactive("FastVS_PlayerPocketTimewriterGlow_Niro");
            var pocketRenderer = pocketGlow != null ? pocketGlow.GetComponent<Renderer>() : null;
            var pocketMaterial = pocketRenderer != null ? pocketRenderer.sharedMaterial : null;
            if (pocketGlow == null ||
                pocketGlow.transform.parent != visual.transform ||
                pocketGlow.GetComponent<FastVsMapMoveGlowPulse>() == null ||
                pocketGlow.GetComponent<Collider>() != null ||
                pocketMaterial == null ||
                pocketMaterial.name.IndexOf("timewriter_pocket_yellow_glow", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("House slice validation failed: Niro must have an inactive yellow Timewriter pocket glow attached to the paper visual.");
            }

            var contactShadow = FindSceneObjectIncludingInactive("FastVS_PlayerContactShadow_Niro");
            if (contactShadow == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing Niro contact shadow.");
            }

            if (contactShadow.name == "Niro_SpriteGroundShadow")
            {
                throw new InvalidOperationException("House slice validation failed: the old Niro sprite ground shadow strip is still present.");
            }

            if (contactShadow.transform.parent != player.transform)
            {
                throw new InvalidOperationException("House slice validation failed: Niro contact shadow is not parented under the player root.");
            }

            if (Quaternion.Angle(contactShadow.transform.localRotation, Quaternion.Euler(90f, 0f, 0f)) > 0.5f)
            {
                throw new InvalidOperationException("House slice validation failed: Niro contact shadow is not lying on the ground plane.");
            }

            if (contactShadow.GetComponent<Renderer>() == null || contactShadow.GetComponent<Collider>() != null)
            {
                throw new InvalidOperationException("House slice validation failed: Niro contact shadow must keep a renderer and no collider.");
            }

            if (contactShadow.GetComponent<TextMesh>() != null || contactShadow.GetComponentsInChildren<TextMesh>(true).Length > 0)
            {
                throw new InvalidOperationException("House slice validation failed: Niro contact shadow must not include TextMesh content.");
            }

            var visibleBottom = sprite.transform.localPosition.y -
                                sprite.transform.localScale.y * 0.5f +
                                sprite.transform.localScale.y * (NiroTransparentFootPixels / NiroExpectedTextureHeight);
            if (Mathf.Abs(visibleBottom) > 0.012f)
            {
                throw new InvalidOperationException($"House slice validation failed: Niro sprite feet are not grounded. visibleBottom={visibleBottom:0.000}");
            }

            var firstRenderer = player.GetComponentInChildren<Renderer>();
            if (firstRenderer == null || firstRenderer.gameObject != sprite)
            {
                throw new InvalidOperationException("House slice validation failed: Time Window player material lookup would no longer target the Niro sprite renderer.");
            }

            var spriteTexture = ResolveMaterialTexture(firstRenderer.sharedMaterial);
            if (spriteTexture == null ||
                spriteTexture.name.IndexOf("shaded", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("House slice validation failed: Niro body shading must be baked into the sprite texture, not a full-card overlay.");
            }
        }

        private static void ValidateFastVsHd2dFirstCycleVisuals()
        {
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("House slice validation failed: main camera is missing.");
            }

            var additionalData = camera.GetComponent<UniversalAdditionalCameraData>();
            if (additionalData == null ||
                !additionalData.renderPostProcessing ||
                !additionalData.requiresDepthTexture)
            {
                throw new InvalidOperationException("House slice validation failed: main camera must enable URP post-processing and depth texture rendering.");
            }

            var volumeObject = FindSceneObjectIncludingInactive("FastVS_HD2D_GlobalVolume");
            var volume = volumeObject != null ? volumeObject.GetComponent<Volume>() : null;
            const string profilePath = "Assets/Settings/DefaultVolumeProfile.asset";
            if (volumeObject == null ||
                volume == null ||
                !volume.isGlobal ||
                !Mathf.Approximately(volume.priority, 0f) ||
                !Mathf.Approximately(volume.weight, 1f) ||
                volume.sharedProfile == null ||
                AssetDatabase.GetAssetPath(volume.sharedProfile) != profilePath)
            {
                throw new InvalidOperationException("House slice validation failed: FastVS_HD2D_GlobalVolume must be global, weighted at 1, and backed by Assets/Settings/DefaultVolumeProfile.asset.");
            }

            if (!RenderSettings.fog)
            {
                throw new InvalidOperationException("House slice validation failed: render fog must be enabled.");
            }

            var ambient = RenderSettings.ambientLight;
            var targetAmbient = new Color(0.22f, 0.23f, 0.27f);
            if (Mathf.Abs(ambient.r - targetAmbient.r) > 0.02f ||
                Mathf.Abs(ambient.g - targetAmbient.g) > 0.02f ||
                Mathf.Abs(ambient.b - targetAmbient.b) > 0.02f)
            {
                throw new InvalidOperationException("House slice validation failed: ambient light must remain near the HD-2D target tint.");
            }

            if (Mathf.Abs(ambient.r - 0.30f) < 0.001f &&
                Mathf.Abs(ambient.g - 0.30f) < 0.001f &&
                Mathf.Abs(ambient.b - 0.34f) < 0.001f)
            {
                throw new InvalidOperationException("House slice validation failed: ambient light still matches the old flat preset.");
            }

            var activeScene = SceneManager.GetActiveScene();
            Light directionalLight = null;
            var directionalCount = 0;
            foreach (var light in Resources.FindObjectsOfTypeAll<Light>())
            {
                if (light == null ||
                    !light.gameObject.scene.IsValid() ||
                    light.gameObject.scene != activeScene ||
                    light.type != LightType.Directional)
                {
                    continue;
                }

                directionalCount++;
                directionalLight = light;
            }

            if (directionalCount != 1 || directionalLight == null)
            {
                throw new InvalidOperationException($"House slice validation failed: expected exactly one directional light in the house slice scene, found {directionalCount}.");
            }

            if (directionalLight.shadows != LightShadows.Soft)
            {
                throw new InvalidOperationException("House slice validation failed: directional light must use soft shadows.");
            }

            if (Mathf.Abs(directionalLight.color.r - 1f) < 0.001f &&
                Mathf.Abs(directionalLight.color.g - 1f) < 0.001f &&
                Mathf.Abs(directionalLight.color.b - 1f) < 0.001f)
            {
                throw new InvalidOperationException("House slice validation failed: directional light color must not be pure white.");
            }

            var contactShadow = FindSceneObjectIncludingInactive("FastVS_PlayerContactShadow_Niro");
            if (contactShadow == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing FastVS_PlayerContactShadow_Niro.");
            }

            if (FindSceneObjectIncludingInactive("FastVS_PlayerSpriteShadingOverlay_Niro") != null)
            {
                throw new InvalidOperationException("House slice validation failed: FastVS_PlayerSpriteShadingOverlay_Niro must not exist.");
            }

            ValidateMaterialSmoothness("Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_interior_floor.mat");
            ValidateMaterialSmoothness("Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_wood_floor.mat");
        }

        private static void ValidateFastVsHd2dSecondCycleAtmosphere()
        {
            var atmosphereMaterial = EnsureHd2dAtmosphereParticleMaterial();
            ValidateAtmosphereParticleSystem("FastVS_HD2D_CurrentInterior_DustMotes", CurrentSpaceRenderLayer, atmosphereMaterial);
            ValidateAtmosphereParticleSystem("FastVS_HD2D_CurrentLibrary_DustMotes", CurrentSpaceRenderLayer, atmosphereMaterial);
            ValidateAtmosphereParticleSystem("FastVS_HD2D_CurrentExterior_DustDrift", CurrentSpaceRenderLayer, atmosphereMaterial);
            ValidateAtmosphereParticleSystem("FastVS_HD2D_CurrentPlaza_DustDrift", CurrentSpaceRenderLayer, atmosphereMaterial);
            ValidateAtmosphereParticleSystem("FastVS_HD2D_PastInterior_WarmMotes", OtherTimeSpaceRenderLayer, atmosphereMaterial);
            ValidateAtmosphereParticleSystem("FastVS_HD2D_PastLibrary_WarmMotes", OtherTimeSpaceRenderLayer, atmosphereMaterial);
            ValidateAtmosphereParticleSystem("FastVS_HD2D_PastExterior_MemoryDrift", OtherTimeSpaceRenderLayer, atmosphereMaterial);
            ValidateAtmosphereParticleSystem("FastVS_HD2D_PastPlaza_MemoryDrift", OtherTimeSpaceRenderLayer, atmosphereMaterial);
        }

        private static void ValidateFastVsHd2dThirdCycleSurfaceTextures()
        {
            ValidateGeneratedRepeatTextureAsset("current_interior_floor_hd2d_plate", 128, 128, 18);
            ValidateGeneratedRepeatTextureAsset("past_wood_floor_hd2d_plate", 128, 128, 18);
            ValidateGeneratedRepeatTextureAsset("current_interior_wall_hd2d_plate", 128, 128, 18);
            ValidateGeneratedRepeatTextureAsset("past_interior_wall_hd2d_plate", 128, 128, 18);
            ValidateGeneratedRepeatTextureAsset("current_furniture_hd2d_plate", 128, 128, 18);
            ValidateGeneratedRepeatTextureAsset("past_furniture_hd2d_plate", 128, 128, 18);
            ValidateGeneratedRepeatTextureAsset("book_spines_hd2d_plate", 128, 64, 30);
            ValidateGeneratedRepeatTextureAsset("bookshelf_front_painted_hd2d", 256, 128, 30);

            ValidateGeneratedSurfaceMaterialTexture("current_interior_floor", "current_interior_floor_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_wood_floor", "past_wood_floor_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("current_interior_wall", "current_interior_wall_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_interior_wall", "past_interior_wall_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("current_furniture", "current_furniture_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_furniture", "past_furniture_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("book", "book_spines_hd2d_plate");
        }

        private static void ValidateAtmosphereParticleSystem(string objectName, int expectedLayer, Material expectedMaterial)
        {
            var atmosphere = FindSceneObjectIncludingInactive(objectName);
            if (atmosphere == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing atmosphere object {objectName}.");
            }

            var system = atmosphere.GetComponent<ParticleSystem>();
            var renderer = atmosphere.GetComponent<ParticleSystemRenderer>();
            if (system == null || renderer == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have ParticleSystem and ParticleSystemRenderer components.");
            }

            if (atmosphere.layer != expectedLayer)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must be on layer {expectedLayer}, but was {atmosphere.layer}.");
            }

            var main = system.main;
            var emission = system.emission;
            if (main.maxParticles > 80 ||
                !main.loop ||
                !main.prewarm ||
                main.duration <= 0f ||
                main.startLifetime.constant <= 0f ||
                main.startSpeed.constant <= 0f ||
                main.startSize.constant <= 0f ||
                emission.rateOverTime.constant <= 0f)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must be a looping prewarmed atmosphere emitter with nonzero timing values and maxParticles <= 80.");
            }

            if (renderer.sharedMaterial != expectedMaterial ||
                renderer.shadowCastingMode != ShadowCastingMode.Off ||
                renderer.receiveShadows)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use the generated atmosphere material with shadows disabled.");
            }
        }

        private static void ValidateGeneratedRepeatTextureAsset(string textureId, int minWidth, int minHeight, int minimumDistinctColors)
        {
            var path = $"{TextureDirectory}/FastVS_House_{textureId}.asset";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing generated texture asset {path}.");
            }

            if (texture.width < minWidth || texture.height < minHeight)
            {
                throw new InvalidOperationException($"House slice validation failed: {textureId} must be at least {minWidth}x{minHeight}, but was {texture.width}x{texture.height}.");
            }

            if (texture.filterMode != FilterMode.Point || texture.wrapMode != TextureWrapMode.Repeat)
            {
                throw new InvalidOperationException($"House slice validation failed: {textureId} must use point filtering and repeat wrap.");
            }

            if (CountDistinctOpaqueColors(texture) < minimumDistinctColors)
            {
                throw new InvalidOperationException($"House slice validation failed: {textureId} must contain enough opaque color variation.");
            }
        }

        private static void ValidateGeneratedSurfaceMaterialTexture(string materialId, string textureId)
        {
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var texturePath = $"{TextureDirectory}/FastVS_House_{textureId}.asset";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            if (material == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing material asset {materialPath}.");
            }

            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing texture asset {texturePath}.");
            }

            if (!MaterialUsesTexture(material, texture))
            {
                throw new InvalidOperationException($"House slice validation failed: {materialId} must reference {textureId}.");
            }
        }

        private static bool MaterialUsesTexture(Material material, Texture2D texture)
        {
            var matched = false;

            if (material.HasProperty("_BaseMap"))
            {
                var baseMap = material.GetTexture("_BaseMap") as Texture2D;
                if (baseMap == texture)
                {
                    matched = true;
                }
            }

            if (material.HasProperty("_MainTex"))
            {
                var mainTex = material.GetTexture("_MainTex") as Texture2D;
                if (mainTex == texture)
                {
                    matched = true;
                }
            }

            return matched;
        }

        private static int CountDistinctOpaqueColors(Texture2D texture)
        {
            var distinct = new HashSet<int>();
            var pixels = texture.GetPixels32();
            for (var index = 0; index < pixels.Length; index++)
            {
                var pixel = pixels[index];
                if (pixel.a < 255)
                {
                    continue;
                }

                distinct.Add((pixel.r << 24) | (pixel.g << 16) | (pixel.b << 8) | pixel.a);
            }

            return distinct.Count;
        }

        private static void ValidateMaterialSmoothness(string materialPath)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing material asset {materialPath}.");
            }

            if (material.HasProperty("_Smoothness") && material.GetFloat("_Smoothness") > 0.20f)
            {
                throw new InvalidOperationException($"House slice validation failed: {materialPath} must remain matte with _Smoothness <= 0.20.");
            }
        }

        private static void ValidateFastVsStoryFlow()
        {
            var story = UnityEngine.Object.FindFirstObjectByType<FastVsStoryFlowController>(FindObjectsInactive.Include);
            if (story == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing Chapter 1 Fast VS story flow controller.");
            }

            var reto = FindSceneObjectIncludingInactive("FastVS_Reto_WritingAtDesk");
            var retoAnimator = UnityEngine.Object.FindFirstObjectByType<FastVsRetoWritingAnimator>(FindObjectsInactive.Include);
            if (reto == null || retoAnimator == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing Reto writing character or animator.");
            }

            if (FindSceneObjectIncludingInactive("FastVS_Reto_LeftWritingArm") != null ||
                FindSceneObjectIncludingInactive("FastVS_Reto_RightWritingArm") != null ||
                FindSceneObjectIncludingInactive("FastVS_Reto_PenOnRaisedHand") != null)
            {
                throw new InvalidOperationException("House slice validation failed: old kitbashed Reto arm/pen overlay parts must be discarded.");
            }

            if (!retoAnimator.HasFinalStateflowMaterialsForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Reto must use the accepted v02 seated stateflow sprite pack.");
            }

            retoAnimator.SetWritingImmediateForReview();
            if (retoAnimator.CurrentStateForReview != FastVsRetoWritingState.WritingRaised ||
                retoAnimator.ActiveMaterialForReview == null ||
                retoAnimator.ActiveMaterialForReview.name.IndexOf("writing_loop", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("House slice validation failed: Reto normal writing state must settle on the writing loop, not the raise-arms transition.");
            }

            retoAnimator.SetDialogueImmediateForReview();
            if (retoAnimator.CurrentStateForReview != FastVsRetoWritingState.DialogueIdle ||
                retoAnimator.ActiveMaterialForReview == null ||
                retoAnimator.ActiveMaterialForReview.name.IndexOf("talk_loop", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("House slice validation failed: Reto dialogue idle must use the accepted talk-loop material, not the lower-arms transition.");
            }

            retoAnimator.SetWritingImmediateForReview();
            ValidateVectorNear("Reto library desk placement", reto.transform.localPosition, RetoLibraryDeskLocalPosition);

            var tableOpenBook = FindSceneObjectIncludingInactive("Current_Library_TableOpenBook");
            var initialDeskBook = FindSceneObjectIncludingInactive("Current_Library_RetoDeskBook_Initial");
            var returnedDeskBook = FindSceneObjectIncludingInactive("Current_Library_ReturnedBookOnDesk");
            if (tableOpenBook == null || initialDeskBook == null || returnedDeskBook == null)
            {
                throw new InvalidOperationException("House slice validation failed: current-side Reto desk books are missing.");
            }

            ValidateTabletopBookHeight("Current_Library_TableOpenBook", tableOpenBook.transform.localPosition.y);
            ValidateVectorNear("Reto initial desk book placement", initialDeskBook.transform.localPosition, CurrentLibraryRetoDeskBookInitialLocalPosition);
            ValidateVectorNear("Reto returned desk book placement", returnedDeskBook.transform.localPosition, CurrentLibraryReturnedBookLocalPosition);
            ValidateTabletopBookHeight("Current_Library_RetoDeskBook_Initial", initialDeskBook.transform.localPosition.y);
            ValidateTabletopBookHeight("Current_Library_ReturnedBookOnDesk", returnedDeskBook.transform.localPosition.y);
            ValidateSeparatedOnGroundPlane("Reto desk books", initialDeskBook.transform.localPosition, returnedDeskBook.transform.localPosition, 0.28f);

            var currentBookCue = FindSceneObjectIncludingInactive("Current_Library_TimeWindowOpenCue_Book");
            var currentAriaCue = FindSceneObjectIncludingInactive("Current_Library_TimeWindowOpenCue_Aria");
            var pastTargetBook = FindSceneObjectIncludingInactive("Past_Library_TargetBook_ForPickup");
            if (currentBookCue == null || currentAriaCue == null || pastTargetBook == null)
            {
                throw new InvalidOperationException("House slice validation failed: time-window book cue or past target book is missing.");
            }

            ValidateVectorNear("past target book placement", pastTargetBook.transform.localPosition, PastLibraryBookCueLocalPosition);
            ValidateFloorGlowCuePresentation("Current_Library_TimeWindowOpenCue_Book", currentBookCue, new Vector3(PastLibraryBookCueLocalPosition.x, CurrentLibraryCueFloorY, PastLibraryBookCueLocalPosition.z), CurrentLibraryBookCueGlowScale);
            ValidateFloorGlowCuePresentation("Current_Library_TimeWindowOpenCue_Aria", currentAriaCue, new Vector3(PastLibraryPersonCueLocalPosition.x, CurrentLibraryCueFloorY, PastLibraryPersonCueLocalPosition.z), CurrentLibraryAriaCueGlowScale);
            ValidateCurrentLibraryRuinObjects();
            ValidatePastLibraryTargetBookMarkerMotion();
            ValidateTabletopBookHeight("Past_Library_TargetBook_ForPickup", pastTargetBook.transform.localPosition.y);
            ValidatePastLibraryReadingTableBooks();

            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>(FindObjectsInactive.Include);
            if (guide == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing visual direction guide for story movement lock.");
            }

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>(FindObjectsInactive.Include);
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>(FindObjectsInactive.Include);
            if (controller == null || visibility == null)
            {
                throw new InvalidOperationException("House slice validation failed: story validation needs the portal controller and area visibility controller.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.Interior);
            controller.ForcePlayerCurrentLocalForReview(HouseInteriorPlayerStart);

            void ValidateVisibleObjective(string expected, string context)
            {
                ValidateVisibleObjectiveAny(context, expected);
            }

            void ValidateVisibleObjectiveAny(string context, params string[] expected)
            {
                story.RefreshPresentationForReview();
                var matched = false;
                foreach (var candidate in expected)
                {
                    if (story.RuntimeHudObjectiveTextForReview == candidate)
                    {
                        matched = true;
                        break;
                    }
                }

                if (!matched || !story.RuntimeHudObjectivePanelActiveForReview)
                {
                    throw new InvalidOperationException(
                        $"House slice validation failed: lower-left HUD objective mismatch at {context}. expected='{string.Join("' or '", expected)}', actual='{story.RuntimeHudObjectiveTextForReview}', active={story.RuntimeHudObjectivePanelActiveForReview}, activeText='{story.RuntimeHudActiveTextForReview}', question={story.RuntimeHudQuestionActiveForReview}, brush={story.RuntimeHudBrushActiveForReview}");
                }
            }

            if (story.UsesTmpDialoguePresenterForReview &&
                story.DialoguePresenterFontNameForReview.IndexOf("Anemora_JP", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("House slice validation failed: TMP story dialogue presenter must use the bundled Japanese font when enabled.");
            }

            if (!story.UsesRuntimeHudForReview ||
                story.RuntimeHudFontNameForReview.IndexOf("Anemora_JP", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("House slice validation failed: story flow must use the runtime TMP HUD with the bundled Japanese font instead of the legacy OnGUI panel.");
            }

            if (controller.RuntimeInputEnabledForReview ||
                story.PortalInputUnlockedForReview ||
                story.CurrentTimeWindowBookCueVisibleForReview ||
                story.CurrentTimeWindowAriaCueVisibleForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Time Window input and current-side cues must be locked before the Reto event unlocks them.");
            }

            ValidateVisibleObjectiveAny("start route objective", "ベッドから起きる。", "外へ出る。", "ベッドから起きた。外へ出る。");

            story.TriggerOpeningWakeForReview();
            story.RefreshPresentationForReview();
            if (!story.OpeningWakeCompleteForReview ||
                story.CurrentBeatIdForReview != "opening.house_interior" ||
                guide.MovementFrozenForReview ||
                story.CurrentLineTextForReview != string.Empty ||
                story.RuntimeHudActiveTextForReview != string.Empty ||
                (story.RuntimeHudObjectiveTextForReview != "外へ出る。" &&
                 story.RuntimeHudObjectiveTextForReview != "ベッドから起きた。外へ出る。") ||
                !story.RuntimeHudObjectivePanelActiveForReview)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: VS branch must skip the opening wake dialogue and start playable in the house interior. opening={story.OpeningWakeCompleteForReview}, beat='{story.CurrentBeatIdForReview}', frozen={guide.MovementFrozenForReview}, line='{story.CurrentLineTextForReview}', hudActive='{story.RuntimeHudActiveTextForReview}', objective='{story.RuntimeHudObjectiveTextForReview}', objectiveActive={story.RuntimeHudObjectivePanelActiveForReview}");
            }

            ValidateVisibleObjectiveAny("house interior route", "外へ出る。", "ベッドから起きた。外へ出る。");
            visibility.SetActiveAreaForReview(FastVsHouseArea.Exterior);
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0.10f, 0.02f, -0.90f));
            ValidateVisibleObjective("北東の道を進む。", "house exterior route");
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-1.80f, 0.02f, -1.20f));
            ValidateVisibleObjective("図書館へ向かう。", "central plaza route");
            visibility.SetActiveAreaForReview(FastVsHouseArea.Library);
            controller.ForcePlayerCurrentLocalForReview(LibraryVsCenter + new Vector3(-2.80f, 0.02f, -2.40f));
            ValidateVisibleObjective("レトの机へ向かう。", "library approach route");

            visibility.SetActiveAreaForReview(FastVsHouseArea.Interior);
            controller.ForcePlayerCurrentLocalForReview(InteriorDoorTriggerCenter);
            if (!story.TryBlockHouseExitForDoorBrushBeat(FastVsHouseArea.Interior, FastVsHouseArea.Exterior, true) ||
                visibility.ActiveAreaForReview != FastVsHouseArea.Interior)
            {
                throw new InvalidOperationException("House slice validation failed: door Timewriter beat must intercept the actual interior exit trigger before changing to the exterior map.");
            }

            if (!guide.MovementFrozenForReview ||
                story.DoorBrushBeatCompleteForReview ||
                story.CurrentBeatIdForReview != "opening.timewriter_pocket_beat")
            {
                throw new InvalidOperationException("House slice validation failed: door Timewriter beat did not freeze movement or expose the expected beat id.");
            }

            story.RefreshPresentationForReview();
            if (story.DoorBrushBeatPageForReview != 0 ||
                story.RuntimeHudActiveTextForReview != string.Empty ||
                !story.RuntimeHudQuestionActiveForReview ||
                story.RuntimeHudBrushActiveForReview ||
                story.RuntimeHudObjectivePanelActiveForReview ||
                story.RuntimeHudQuestionHeadWorldOffsetForReview < 1.38f ||
                story.RuntimeHudQuestionHeadWorldOffsetForReview > 1.52f)
            {
                throw new InvalidOperationException("House slice validation failed: door Timewriter beat must start with a question-only pause placed close to Niro's head before showing the brush.");
            }

            story.AdvanceStoryForReview();
            story.RefreshPresentationForReview();
            if (story.DoorBrushBeatPageForReview != 1 ||
                story.RuntimeHudActiveTextForReview != "(ポケットに、何か...)" ||
                story.RuntimeHudQuestionActiveForReview ||
                !story.RuntimeHudBrushActiveForReview ||
                story.RuntimeHudObjectivePanelActiveForReview ||
                story.RuntimeHudBrushIconTextureNameForReview.IndexOf("timewriter_brush_icon", StringComparison.OrdinalIgnoreCase) < 0 ||
                story.RuntimeHudBrushAnchoredPositionForReview.magnitude > 0.01f)
            {
                throw new InvalidOperationException("House slice validation failed: door Timewriter beat must clear the question and show the brush picture at screen center with the revised pocket line.");
            }

            story.AdvanceStoryForReview();
            story.RefreshPresentationForReview();
            if (story.DoorBrushBeatPageForReview != 2 ||
                story.RuntimeHudActiveTextForReview != "(...筆?)" ||
                story.RuntimeHudQuestionActiveForReview ||
                !story.RuntimeHudBrushActiveForReview ||
                story.RuntimeHudObjectivePanelActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: door Timewriter beat must keep the brush reveal before Niro notices the brush.");
            }

            story.AdvanceStoryForReview();
            if (!story.DoorBrushBeatCompleteForReview || guide.MovementFrozenForReview)
            {
                throw new InvalidOperationException("House slice validation failed: door Timewriter beat did not complete and release movement after advancing.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.Library);
            controller.ForcePlayerCurrentLocalForReview(RetoLibraryDeskLocalPosition + new Vector3(-1.15f, 0.02f, -1.35f));
            story.RefreshPresentationForReview();
            if (!story.RetoInteractionReadyForReview ||
                story.RuntimeHudObjectiveTextForReview != "E: レトと話す" ||
                !story.RuntimeHudObjectivePanelActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Reto must be talkable from the reachable front-of-desk player position with a lower-left interaction prompt.");
            }

            story.TriggerRetoEventForReview();
            story.RefreshPresentationForReview();
            story.CompleteRuntimeHudTypingForReview();
            if (!guide.MovementFrozenForReview ||
                story.RetoBeatIndexForReview != 0 ||
                story.CurrentBeatIdForReview != "scene1.reto.1b.initial" ||
                story.CurrentLineSpeakerForReview != "レト" ||
                story.CurrentLineTextForReview != "...見ない顔ですね。" ||
                story.RuntimeHudVisibleTextForReview != "...見ない顔ですね。" ||
                story.RuntimeHudObjectivePanelActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Reto event did not begin at the initial encounter beat.");
            }

            var guard = 0;
            var sawResolveToRecord = false;
            var sawResolvePause = false;
            var sawPocketGlowPause = false;
            while (!story.WaitingForPastObservationForReview && guard++ < 30)
            {
                if (story.CurrentLineTextForReview.IndexOf("からっぽ", StringComparison.Ordinal) >= 0 ||
                    story.CurrentLineTextForReview.IndexOf("なんとなく", StringComparison.Ordinal) >= 0)
                {
                    throw new InvalidOperationException("House slice validation failed: removed opening/library thought is still present.");
                }

                if (story.CurrentBeatIdForReview == "scene1.reto.1c.library_history.pause_before_resolve_to_record")
                {
                    sawResolvePause = true;
                    if (story.CurrentLineTextForReview != string.Empty ||
                        story.TimewriterPocketGlowVisibleForReview)
                    {
                        throw new InvalidOperationException("House slice validation failed: Reto must hold a silent pause before resolving that the previous line was only a monologue.");
                    }
                }

                if (story.CurrentBeatIdForReview == "scene1.reto.1c.library_history.resolve_to_record")
                {
                    sawResolveToRecord = true;
                    if (!sawResolvePause ||
                        story.CurrentLineTextForReview != "いえ。今のは、ただの独り言です。")
                    {
                        throw new InvalidOperationException("House slice validation failed: Reto resolve-to-record line changed unexpectedly.");
                    }
                }

                if (story.CurrentBeatIdForReview == "scene1.reto.1d.timewriter_activation.pocket_glow_pause")
                {
                    sawPocketGlowPause = true;
                    if (!sawResolveToRecord || !story.TimewriterPocketGlowVisibleForReview)
                    {
                        throw new InvalidOperationException("House slice validation failed: Niro pocket glow must appear during the long pause after Reto resolves to keep records.");
                    }
                }

                if (story.CurrentLineTextForReview == "(筆が...!)" && !story.TimewriterPocketGlowVisibleForReview)
                {
                    throw new InvalidOperationException("House slice validation failed: Niro pocket glow must remain visible when Niro notices the brush.");
                }

                if (story.CurrentLineTextForReview == "...本物だ")
                {
                    throw new InvalidOperationException("House slice validation failed: Reto says the book is real before Niro takes the past book.");
                }

                story.AdvanceStoryForReview();
            }

            if (!story.RetoOpeningCompleteForReview ||
                !story.WaitingForPastObservationForReview ||
                story.VsClearForReview ||
                story.CurrentBeatIdForReview != "scene1.reto.1e.await_past_library_observation" ||
                guide.MovementFrozenForReview ||
                !story.PortalInputUnlockedForReview ||
                !story.CurrentTimeWindowBookCueVisibleForReview ||
                !story.CurrentTimeWindowAriaCueVisibleForReview ||
                !sawResolveToRecord ||
                !sawResolvePause ||
                !sawPocketGlowPause ||
                story.TimewriterPocketGlowVisibleForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Reto [1.B]-[1.D] did not wait for player-controlled past observation or unlock both current-side Time Window cues.");
            }

            ValidateVisibleObjective("黄色い光の近くに、左ドラッグで時の窓を開く。", "time window unlock");

            visibility.SetActiveAreaForReview(FastVsHouseArea.Library);
            controller.ForcePlayerOtherTimeLocalForReview(new Vector3(PastLibraryPersonCueLocalPosition.x, 0.02f, PastLibraryPersonCueLocalPosition.z));
            ValidateVisibleObjective("E: 過去の人影を見る", "past person cue");

            if (!story.AriaInteractionReadyForReview)
            {
                throw new InvalidOperationException("House slice validation failed: past Aria monologue interaction is not available near Aria.");
            }

            story.TriggerAriaObservationForReview();
            story.RefreshPresentationForReview();
            story.CompleteRuntimeHudTypingForReview();
            if (!guide.MovementFrozenForReview ||
                story.CurrentLineSpeakerForReview != "ニロ" ||
                story.CurrentLineTextForReview != "(...人)" ||
                story.RuntimeHudObjectivePanelActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Aria interaction must start with Niro noticing a person, not the book event.");
            }

            story.AdvanceStoryForReview();
            story.AdvanceStoryForReview();
            if (!guide.MovementFrozenForReview ||
                story.CurrentLineSpeakerForReview != "ニロ" ||
                story.CurrentLineTextForReview != "(...本を読んでいる)")
            {
                throw new InvalidOperationException("House slice validation failed: Aria interaction must skip the removed return-record line and continue with Niro's observation.");
            }

            guard = 0;
            while (story.CurrentBeatIdForReview != "scene1.reto.1e.await_past_library_observation" && guard++ < 8)
            {
                story.AdvanceStoryForReview();
            }

            if (!story.WaitingForPastObservationForReview ||
                guide.MovementFrozenForReview ||
                story.CurrentTimeWindowAriaCueVisibleForReview == true)
            {
                throw new InvalidOperationException("House slice validation failed: Aria monologue must return to the player-controlled past-observation state and clear the Aria cue.");
            }

            controller.ForcePlayerOtherTimeLocalForReview(new Vector3(PastLibraryBookCueLocalPosition.x, 0.02f, PastLibraryBookCueLocalPosition.z));
            ValidateVisibleObjective("E: 光っている本を調べる", "past book cue");

            if (!story.PastBookInteractionReadyForReview)
            {
                throw new InvalidOperationException("House slice validation failed: past book event must require the player to stand on the past-side book guide and press E/Space.");
            }

            story.TriggerPastObservationForReview();
            story.RefreshPresentationForReview();
            story.CompleteRuntimeHudTypingForReview();
            if (!guide.MovementFrozenForReview ||
                story.CurrentBeatIdForReview != "scene1.reto.1e.past_library_observation.book_location" ||
                story.CurrentLineTextForReview != "(...ここに、本が)" ||
                story.CurrentTimeWindowBookCueVisibleForReview ||
                story.RuntimeHudObjectivePanelActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Reto [1.E] past observation did not start with the canonical Niro thought.");
            }

            guard = 0;
            while (!story.WaitingForRetoBookShowForReview && guard++ < 12)
            {
                if (story.CurrentLineTextForReview.IndexOf("あの子", StringComparison.Ordinal) >= 0)
                {
                    throw new InvalidOperationException("House slice validation failed: old Aria thought '(…あの子)' is still present.");
                }

                if (story.CurrentLineTextForReview == "(...人)")
                {
                    throw new InvalidOperationException("House slice validation failed: the person notice beat must belong to Aria, not the book interaction.");
                }

                story.AdvanceStoryForReview();
            }

            if (!story.PastObservationCompleteForReview ||
                !story.BookTakenForReview ||
                !story.WaitingForRetoBookShowForReview ||
                story.VsClearForReview ||
                story.CurrentBeatIdForReview != "scene1.reto.1f.await_reto_book_show" ||
                story.CurrentDeskBookVisibleForReview ||
                guide.MovementFrozenForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Reto [1.E] must wait for a second interaction and must not place the returned book before Reto receives it.");
            }

            if (FindSceneObjectIncludingInactive("Past_Library_TargetBook_ForPickup").activeSelf ||
                FindSceneObjectIncludingInactive("Past_Library_TargetBook_RedCubeMarker").activeSelf)
            {
                throw new InvalidOperationException("House slice validation failed: past-side target book and red marker must disappear after Niro takes the book.");
            }

            visibility.SetActiveAreaForReview(FastVsHouseArea.Library);
            controller.ForcePlayerCurrentLocalForReview(LibraryVsCenter + new Vector3(-2.20f, 0.02f, -1.85f));
            ValidateVisibleObjective("レトの机へ戻る。", "return to Reto desk after past observations");

            controller.ForcePlayerCurrentLocalForReview(RetoLibraryDeskLocalPosition + new Vector3(-1.15f, 0.02f, -1.35f));
            ValidateVisibleObjective("E: レトに本を見せる", "Reto book-show prompt");

            if (!story.RetoBookShowReadyForReview)
            {
                throw new InvalidOperationException("House slice validation failed: returning to current time must wait for the player to talk to Reto before showing the book.");
            }

            story.TriggerRetoBookReturnForReview();
            story.RefreshPresentationForReview();
            story.CompleteRuntimeHudTypingForReview();
            if (!guide.MovementFrozenForReview ||
                story.CurrentBeatIdForReview != "scene1.reto.1f.return_present.show_book" ||
                story.CurrentLineTextForReview != "(...本を、レトに見せる)" ||
                !story.BookShownToRetoForReview ||
                story.RuntimeHudObjectivePanelActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Reto [1.F] did not start with Niro showing the book.");
            }

            var sawQuestion = false;
            var sawBookReal = false;
            var sawFaceDropMotion = false;
            var sawFaceLiftMotion = false;
            var sawAcceptance = false;
            var sawV4MiaHint = false;
            guard = 0;
            while (!story.VsClearForReview && guard++ < 20)
            {
                var currentText = story.CurrentLineTextForReview;
                if (story.CurrentBeatIdForReview == "scene1.reto.1f.return_present.face_drop_motion")
                {
                    sawFaceDropMotion = true;
                    if (!sawBookReal ||
                        retoAnimator.CurrentStateForReview != FastVsRetoWritingState.Lowering)
                    {
                        throw new InvalidOperationException("House slice validation failed: Reto must visibly lower his arms after saying the book is real.");
                    }
                }

                if (story.CurrentBeatIdForReview == "scene1.reto.1f.return_present.face_lift_motion")
                {
                    sawFaceLiftMotion = true;
                    if (!sawBookReal ||
                        !sawFaceDropMotion ||
                        retoAnimator.CurrentStateForReview != FastVsRetoWritingState.Raising)
                    {
                        throw new InvalidOperationException("House slice validation failed: Reto must perform a visible down/up motion beat after saying the book is real.");
                    }
                }

                if (currentText == "...?")
                {
                    sawQuestion = true;
                }

                if (currentText == "...本物だ")
                {
                    sawBookReal = true;
                    if (!sawQuestion)
                    {
                        throw new InvalidOperationException("House slice validation failed: Reto says the book is real before the question beat.");
                    }
                }

                if (currentText == "...そうですか。")
                {
                    sawAcceptance = true;
                    if (!sawBookReal || !sawFaceDropMotion || !sawFaceLiftMotion)
                    {
                        throw new InvalidOperationException("House slice validation failed: Reto acceptance beat arrived before the book-real and down/up motion beats.");
                    }
                }

                if (currentText == "もし手があるなら、少し、助けてやってください。")
                {
                    throw new InvalidOperationException("House slice validation failed: old v3 Mia hint line is still present.");
                }

                if (currentText == "あなたなら、力になれるかもしれません。")
                {
                    sawV4MiaHint = true;
                }

                story.AdvanceStoryForReview();
            }

            if (!story.RetoEventCompleteForReview ||
                !story.VsClearForReview ||
                story.CurrentBeatIdForReview != "vs.clear" ||
                !story.CurrentDeskBookVisibleForReview ||
                FindSceneObjectIncludingInactive("Current_Library_ReturnedBookOnDesk").activeSelf == false ||
                !sawQuestion ||
                !sawBookReal ||
                !sawFaceDropMotion ||
                !sawFaceLiftMotion ||
                !sawAcceptance ||
                !sawV4MiaHint ||
                guide.MovementFrozenForReview)
            {
                throw new InvalidOperationException("House slice validation failed: Reto v4 book-show story event did not reach VS clear and release movement.");
            }

            story.RefreshPresentationForReview();
            if (story.RuntimeHudObjectiveTextForReview != "レトの話を聞いた。" ||
                !story.RuntimeHudObjectivePanelActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: lower-left HUD must remain visible after VS clear with the completion objective.");
            }

            retoAnimator.SetWritingImmediateForReview();
            if (retoAnimator.CurrentStateForReview != FastVsRetoWritingState.WritingRaised)
            {
                throw new InvalidOperationException("House slice validation failed: Reto does not return to raised writing pose after the event.");
            }
        }

        private static void ValidateVectorNear(string label, Vector3 actual, Vector3 expected)
        {
            if (Vector3.Distance(actual, expected) > 0.035f)
            {
                throw new InvalidOperationException($"House slice validation failed: {label} expected {expected}, but got {actual}.");
            }
        }

        private static Quaternion FaceTargetOnPlane(Vector3 source, Vector3 target)
        {
            var delta = target - source;
            delta.y = 0f;
            if (delta.sqrMagnitude < 0.0001f)
            {
                return Quaternion.identity;
            }

            return Quaternion.LookRotation(delta, Vector3.up);
        }

        private static bool PointInsideBox(Vector3 point, Vector3 center, Vector3 size)
        {
            var half = size * 0.5f;
            return Mathf.Abs(point.x - center.x) <= half.x &&
                   Mathf.Abs(point.y - center.y) <= half.y &&
                   Mathf.Abs(point.z - center.z) <= half.z;
        }

        private static Texture ResolveMaterialTexture(Material material)
        {
            if (material == null)
            {
                return null;
            }

            if (material.HasProperty("_BaseMap"))
            {
                var texture = material.GetTexture("_BaseMap");
                if (texture != null)
                {
                    return texture;
                }
            }

            return material.HasProperty("_MainTex") ? material.GetTexture("_MainTex") : null;
        }

        private static void ValidateWorldVectorNear(string label, Vector3 actual, Vector3 expected, float tolerance)
        {
            if (Vector3.Distance(actual, expected) > tolerance)
            {
                throw new InvalidOperationException($"House slice validation failed: {label} expected {expected}, but got {actual}.");
            }
        }

        private static GameObject FindSceneObjectIncludingInactive(string objectName)
        {
            foreach (var candidate in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (candidate.name == objectName && candidate.scene.IsValid())
                {
                    return candidate;
                }
            }

            return null;
        }

        private static void ValidateSeparatedOnGroundPlane(string label, Vector3 first, Vector3 second, float minimumDistance)
        {
            var delta = new Vector2(first.x - second.x, first.z - second.z);
            if (delta.magnitude < minimumDistance)
            {
                throw new InvalidOperationException($"House slice validation failed: {label} objects are too close together and may overlap.");
            }
        }

        private static void CreatePathBetween(Transform root, Vector3 start, Vector3 end, float width, Material material, string name, bool keepCollider = false)
        {
            var midpoint = (start + end) * 0.5f;
            var delta = end - start;
            var length = new Vector2(delta.x, delta.z).magnitude;
            var yaw = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
            CreateLandmarkCube(name, root, midpoint, new Vector3(width, 0.08f, length), Quaternion.Euler(0f, yaw, 0f), material, keepCollider, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, name);
        }

        private static void CreatePaperCharacter(Transform root, string displayName, Vector3 localPosition, float height, Material body, Material accent, Material face, Material label)
        {
            var character = new GameObject($"FastVS_PaperCharacter_{displayName.Replace(" ", string.Empty)}");
            character.transform.SetParent(root, false);
            character.transform.localPosition = localPosition;
            character.AddComponent<FastVsPaperBillboard>();
            CreatePaperCardParts(character.transform, displayName, height, body, accent, face, label);
        }

        private static void CreateSpriteCharacter(Transform root, string displayName, Vector3 localPosition, float height, Material spriteMaterial, Material label)
        {
            var character = new GameObject($"FastVS_SpriteCharacter_{displayName.Replace(" ", string.Empty)}");
            character.transform.SetParent(root, false);
            character.transform.localPosition = localPosition;
            character.AddComponent<FastVsPaperBillboard>();
            CreateSpriteCardParts(character.transform, displayName, height, spriteMaterial, false, label);
        }

        private static void CreateRetoAtLibraryDesk(Transform root, Materials materials)
        {
            var reto = new GameObject("FastVS_Reto_WritingAtDesk");
            reto.transform.SetParent(root, false);
            reto.transform.localPosition = RetoLibraryDeskLocalPosition;
            reto.AddComponent<FastVsPaperBillboard>();

            var renderer = CreateSpriteCardParts(
                reto.transform,
                "Reto",
                1.08f,
                SpriteStripMaterial("reto_v02_writing_loop_sprite", RetoWritingLoopStripPath, Color.white, RetoTransitionFrameCount),
                false,
                materials.Label);

            var animator = reto.AddComponent<FastVsRetoWritingAnimator>();
            SerializedSet(animator, "spriteRenderer", renderer);
            SerializedSet(animator, "writingLoopMaterial", SpriteStripMaterial("reto_v02_writing_loop_sprite", RetoWritingLoopStripPath, Color.white, RetoTransitionFrameCount));
            SerializedSet(animator, "lowerArmsMaterial", SpriteStripMaterial("reto_v02_lower_arms_sprite", RetoLowerArmsStripPath, Color.white, RetoTransitionFrameCount));
            SerializedSet(animator, "talkLoopMaterial", SpriteStripMaterial("reto_v02_talk_loop_sprite", RetoTalkLoopStripPath, Color.white, RetoTalkFrameCount));
            SerializedSet(animator, "raiseArmsMaterial", SpriteStripMaterial("reto_v02_raise_arms_sprite", RetoRaiseArmsStripPath, Color.white, RetoTransitionFrameCount));
            SerializedSet(animator, "framePixelWidth", RetoExpectedFrameWidth);
            SerializedSet(animator, "writingLoopFrameCount", RetoTransitionFrameCount);
            SerializedSet(animator, "lowerArmsFrameCount", RetoTransitionFrameCount);
            SerializedSet(animator, "talkLoopFrameCount", RetoTalkFrameCount);
            SerializedSet(animator, "raiseArmsFrameCount", RetoTransitionFrameCount);
            SerializedSet(animator, "loopFramesPerSecond", 4f);
            SerializedSet(animator, "transitionFramesPerSecond", 9f);
            animator.SetWritingImmediateForReview();
        }

        private static void CreatePaperCardParts(Transform parent, string displayName, float height, Material body, Material accent, Material face, Material label)
        {
            CreateQuad($"{displayName}_PaperBody", parent, new Vector3(0f, height * 0.52f, 0f), new Vector3(height * 0.42f, height * 0.88f, 1f), body);
            CreateQuad($"{displayName}_PaperHead", parent, new Vector3(0f, height * 1.02f, -0.012f), new Vector3(height * 0.33f, height * 0.30f, 1f), face);
            CreateQuad($"{displayName}_PaperHatOrHair", parent, new Vector3(0f, height * 1.17f, -0.024f), new Vector3(height * 0.42f, height * 0.18f, 1f), accent);
            CreateQuad($"{displayName}_PaperFootLine", parent, new Vector3(0f, height * 0.08f, -0.026f), new Vector3(height * 0.48f, height * 0.08f, 1f), accent);
            CreateNameLabel(parent, displayName, new Vector3(0f, height * 1.45f, -0.035f), label);
        }

        private static Renderer CreateSpriteCardParts(Transform parent, string displayName, float height, Material spriteMaterial, bool showLabel, Material label)
        {
            var footPadding = height * (NiroTransparentFootPixels / NiroExpectedTextureHeight);
            var sprite = CreateQuad($"{displayName}_Sprite64x96", parent, new Vector3(0f, height * 0.50f - footPadding, 0f), new Vector3(height * 0.667f, height, 1f), spriteMaterial);
            if (showLabel)
            {
                CreateNameLabel(parent, displayName, new Vector3(0f, height * 1.08f, -0.035f), label);
            }

            return sprite.GetComponent<Renderer>();
        }

        private static GameObject CreateQuad(string name, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = name;
            quad.transform.SetParent(parent, false);
            quad.transform.localPosition = localPosition;
            quad.transform.localScale = localScale;
            quad.GetComponent<Renderer>().sharedMaterial = material;
            var collider = quad.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            return quad;
        }

        private static void ValidateTimewriterBrushIconTexture()
        {
            var texture = EnsureTimewriterBrushIconTexture();
            if (texture == null || texture.width != 256 || texture.height != 256)
            {
                throw new InvalidOperationException($"House slice validation failed: Timewriter brush icon texture must be generated at 256x256: {TimewriterBrushIconTexturePath}");
            }

            var importer = AssetImporter.GetAtPath(TimewriterBrushIconTexturePath) as TextureImporter;
            if (importer == null ||
                !importer.alphaIsTransparency ||
                importer.mipmapEnabled ||
                importer.filterMode != FilterMode.Point)
            {
                throw new InvalidOperationException($"House slice validation failed: Timewriter brush icon texture importer must use point-filtered alpha UI settings: {TimewriterBrushIconTexturePath}");
            }
        }

        private static Material EnsureNiroContactShadowMaterial()
        {
            var material = FlatMaterial("niro_contact_shadow", Color.white, true);
            ConfigureTransparentUnlitMaterial(material, 2995);
            var texture = EnsureNiroContactShadowTexture();
            AssignMaterialTexture(material, texture, Vector2.one);
            return material;
        }

        private static Material EnsureHd2dAtmosphereParticleMaterial()
        {
            var path = $"{MaterialDirectory}/FastVS_House_hd2d_atmosphere_particle.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Unlit");
            }

            if (shader == null)
            {
                throw new InvalidOperationException("Atmosphere particle shader not found.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            material.doubleSidedGI = true;
            ConfigureTransparentParticleMaterial(material, 3000);
            AssignMaterialTexture(material, EnsureHd2dAtmosphereParticleTexture(), Vector2.one);

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(1f, 1f, 1f, 0.92f));
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", new Color(1f, 1f, 1f, 0.92f));
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureTimewriterPocketGlowMaterial()
        {
            var material = FlatMaterial("timewriter_pocket_yellow_glow", Color.white, true);
            ConfigureTransparentUnlitMaterial(material, 3035);
            var texture = EnsureTimewriterPocketGlowTexture();
            AssignMaterialTexture(material, texture, Vector2.one);
            return material;
        }

        private static Material EnsureNiroShadingOverlayMaterial()
        {
            var material = FlatMaterial("niro_shading_overlay", Color.white, true);
            ConfigureTransparentUnlitMaterial(material, 3010);
            var texture = EnsureNiroShadingOverlayTexture();
            AssignMaterialTexture(material, texture, Vector2.one);
            return material;
        }

        private static void ConfigureTransparentUnlitMaterial(Material material, int renderQueue)
        {
            ConfigureTransparentMaterial(material, renderQueue, "Universal Render Pipeline/Unlit");
        }

        private static void ConfigureTransparentParticleMaterial(Material material, int renderQueue)
        {
            ConfigureTransparentMaterial(material, renderQueue, "Universal Render Pipeline/Particles/Unlit", "Universal Render Pipeline/Unlit");
        }

        private static void ConfigureTransparentMaterial(Material material, int renderQueue, params string[] shaderNames)
        {
            Shader shader = null;
            foreach (var shaderName in shaderNames)
            {
                shader = Shader.Find(shaderName);
                if (shader != null)
                {
                    break;
                }
            }

            if (shader == null)
            {
                throw new InvalidOperationException($"Required shader not found: {string.Join(", ", shaderNames)}");
            }

            material.shader = shader;
            material.doubleSidedGI = true;

            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 1f);
            }

            if (material.HasProperty("_Blend"))
            {
                material.SetFloat("_Blend", 0f);
            }

            if (material.HasProperty("_SrcBlend"))
            {
                material.SetFloat("_SrcBlend", 5f);
            }

            if (material.HasProperty("_DstBlend"))
            {
                material.SetFloat("_DstBlend", 10f);
            }

            if (material.HasProperty("_ZWrite"))
            {
                material.SetFloat("_ZWrite", 0f);
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", 0f);
            }

            if (material.HasProperty("_AlphaClip"))
            {
                material.SetFloat("_AlphaClip", 0f);
            }

            if (material.HasProperty("_QueueControl"))
            {
                material.SetFloat("_QueueControl", 0f);
            }

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = renderQueue;
            EditorUtility.SetDirty(material);
        }

        private static Texture2D EnsureHd2dAtmosphereParticleTexture()
        {
            return EnsureGeneratedTexture(
                "hd2d_atmosphere_particle_soft",
                64,
                64,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var centeredX = ((x + 0.5f) / 64f) * 2f - 1f;
                    var centeredY = ((y + 0.5f) / 64f) * 2f - 1f;
                    var distance = Mathf.Sqrt((centeredX * centeredX) + (centeredY * centeredY));
                    var core = Mathf.Clamp01(1f - distance);
                    var alpha = core * core * 0.95f;
                    var tone = 0.96f + (core * 0.04f);
                    return new Color(tone, tone, tone, alpha);
                });
        }

        private static void AssignMaterialTexture(Material material, Texture2D texture, Vector2 scale)
        {
            if (material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", texture);
                material.SetTextureScale("_BaseMap", scale);
                material.SetTextureOffset("_BaseMap", Vector2.zero);
            }

            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", texture);
                material.SetTextureScale("_MainTex", scale);
                material.SetTextureOffset("_MainTex", Vector2.zero);
            }

            EditorUtility.SetDirty(material);
        }

        private static Texture2D EnsureNiroContactShadowTexture()
        {
            return EnsureGeneratedTexture(
                "niro_contact_shadow",
                64,
                32,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = x / 63f;
                    var v = y / 31f;
                    var dx = (u - 0.5f) / 0.5f;
                    var dy = (v - 0.5f) / 0.5f;
                    var ellipse = Mathf.Sqrt((dx * dx * 0.92f) + (dy * dy * 2.45f));
                    var alpha = Mathf.Clamp01(1f - ellipse);
                    alpha = alpha * alpha * 0.46f;
                    return new Color(0.02f, 0.03f, 0.05f, alpha);
                });
        }

        private static Texture2D EnsureTimewriterPocketGlowTexture()
        {
            return EnsureGeneratedTexture(
                "timewriter_pocket_yellow_glow",
                64,
                64,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = x / 63f;
                    var v = y / 63f;
                    var dx = (u - 0.5f) / 0.5f;
                    var dy = (v - 0.5f) / 0.5f;
                    var d = Mathf.Sqrt(dx * dx + dy * dy);
                    var alpha = Mathf.Clamp01(1f - d);
                    alpha = alpha * alpha * 0.78f;
                    var core = Mathf.Clamp01((0.36f - d) / 0.36f);
                    return new Color(
                        1f,
                        0.78f + core * 0.20f,
                        0.12f + core * 0.38f,
                        alpha);
                });
        }

        private static Texture2D EnsureTimewriterBrushIconTexture()
        {
            EnsureFolder(TextureDirectory);
            var texture = new Texture2D(256, 256, TextureFormat.RGBA32, false)
            {
                name = "FastVS_House_timewriter_brush_icon_v01",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var dx = (x - 128f) / 112f;
                    var dy = (y - 128f) / 112f;
                    var d = Mathf.Sqrt(dx * dx + dy * dy);
                    var glow = Mathf.Clamp01(1f - d);
                    glow = glow * glow * 0.22f;
                    texture.SetPixel(x, y, new Color(1f, 0.76f, 0.18f, glow));
                }
            }

            DrawPixelLine(texture, new Vector2(82f, 50f), new Vector2(145f, 150f), 24f, new Color32(52, 29, 17, 255));
            DrawPixelLine(texture, new Vector2(82f, 50f), new Vector2(145f, 150f), 16f, new Color32(124, 70, 35, 255));
            DrawPixelLine(texture, new Vector2(94f, 62f), new Vector2(137f, 133f), 5f, new Color32(183, 120, 61, 255));
            DrawPixelLine(texture, new Vector2(142f, 145f), new Vector2(177f, 184f), 30f, new Color32(65, 42, 28, 255));
            DrawPixelLine(texture, new Vector2(146f, 148f), new Vector2(176f, 181f), 21f, new Color32(232, 178, 70, 255));
            DrawPixelLine(texture, new Vector2(155f, 154f), new Vector2(170f, 171f), 5f, new Color32(255, 230, 122, 255));
            DrawPixelLine(texture, new Vector2(176f, 181f), new Vector2(207f, 215f), 30f, new Color32(33, 20, 16, 255));
            DrawPixelLine(texture, new Vector2(178f, 182f), new Vector2(199f, 207f), 19f, new Color32(74, 42, 30, 255));
            DrawPixelLine(texture, new Vector2(195f, 205f), new Vector2(217f, 226f), 13f, new Color32(22, 15, 13, 255));
            DrawPixelLine(texture, new Vector2(62f, 36f), new Vector2(94f, 68f), 7f, new Color32(255, 216, 85, 180));

            texture.Apply(false, false);
            File.WriteAllBytes(TimewriterBrushIconTexturePath, texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(TimewriterBrushIconTexturePath, ImportAssetOptions.ForceSynchronousImport);

            var importer = AssetImporter.GetAtPath(TimewriterBrushIconTexturePath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Default;
                importer.alphaIsTransparency = true;
                importer.isReadable = true;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.filterMode = FilterMode.Point;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }

            var imported = AssetDatabase.LoadAssetAtPath<Texture2D>(TimewriterBrushIconTexturePath);
            if (imported == null)
            {
                throw new InvalidOperationException($"Fast VS Timewriter brush icon texture generation failed: {TimewriterBrushIconTexturePath}");
            }

            return imported;
        }

        private static void DrawPixelLine(Texture2D texture, Vector2 start, Vector2 end, float width, Color color)
        {
            var minX = Mathf.Max(0, Mathf.FloorToInt(Mathf.Min(start.x, end.x) - width));
            var maxX = Mathf.Min(texture.width - 1, Mathf.CeilToInt(Mathf.Max(start.x, end.x) + width));
            var minY = Mathf.Max(0, Mathf.FloorToInt(Mathf.Min(start.y, end.y) - width));
            var maxY = Mathf.Min(texture.height - 1, Mathf.CeilToInt(Mathf.Max(start.y, end.y) + width));
            var segment = end - start;
            var lengthSquared = Mathf.Max(0.001f, Vector2.Dot(segment, segment));
            var half = width * 0.5f;

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var point = new Vector2(x + 0.5f, y + 0.5f);
                    var t = Mathf.Clamp01(Vector2.Dot(point - start, segment) / lengthSquared);
                    var closest = start + segment * t;
                    if (Vector2.Distance(point, closest) > half)
                    {
                        continue;
                    }

                    var existing = texture.GetPixel(x, y);
                    texture.SetPixel(x, y, AlphaBlend(existing, color));
                }
            }
        }

        private static Color AlphaBlend(Color under, Color over)
        {
            var alpha = over.a + under.a * (1f - over.a);
            if (alpha <= 0.001f)
            {
                return Color.clear;
            }

            return new Color(
                (over.r * over.a + under.r * under.a * (1f - over.a)) / alpha,
                (over.g * over.a + under.g * under.a * (1f - over.a)) / alpha,
                (over.b * over.a + under.b * under.a * (1f - over.a)) / alpha,
                alpha);
        }

        private static Texture2D EnsureNiroShadingOverlayTexture()
        {
            return EnsureGeneratedTexture(
                "niro_shading_overlay",
                64,
                96,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = x / 63f;
                    var v = y / 95f;
                    var ramp = Mathf.Clamp01((u * 0.48f) + ((1f - v) * 0.88f) - 0.18f);
                    var alpha = ramp * 0.22f;
                    return new Color(0.02f, 0.025f, 0.04f, alpha);
                });
        }

        private static Texture2D EnsureGeneratedTexture(string id, int width, int height, FilterMode filterMode, Func<int, int, Color> sample)
        {
            var path = $"{TextureDirectory}/FastVS_House_{id}.asset";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                AssetDatabase.CreateAsset(texture, path);
            }

            texture.name = $"FastVS_House_{id}";
            texture.filterMode = filterMode;
            texture.wrapMode = TextureWrapMode.Clamp;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, sample(x, y));
                }
            }

            texture.Apply(false, false);
            EditorUtility.SetDirty(texture);
            return texture;
        }

        private static Texture2D EnsureGeneratedRepeatTexture(string id, int width, int height, Func<int, int, Color> sample)
        {
            var path = $"{TextureDirectory}/FastVS_House_{id}.asset";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture != null && (texture.width != width || texture.height != height))
            {
                AssetDatabase.DeleteAsset(path);
                texture = null;
            }

            if (texture == null)
            {
                texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                AssetDatabase.CreateAsset(texture, path);
            }

            texture.name = $"FastVS_House_{id}";
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Repeat;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, sample(x, y));
                }
            }

            texture.Apply(false, false);
            EditorUtility.SetDirty(texture);
            return texture;
        }

        private static Material PaintedSurfaceMaterial(string materialId, string textureId, int width, int height, Func<int, int, Color> sample, bool unlit, Vector2 tiling)
        {
            var material = FlatMaterial(materialId, Color.white, unlit);
            var texture = EnsureGeneratedRepeatTexture(textureId, width, height, sample);
            AssignMaterialTexture(material, texture, tiling);
            return material;
        }

        private static Color SampleCurrentInteriorFloorHd2dPixel(int x, int y)
        {
            return SamplePlankPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.25f, 0.18f, 0.13f, 1f),
                new Color(0.32f, 0.23f, 0.16f, 1f),
                new Color(0.13f, 0.10f, 0.08f, 1f),
                new Color(0.46f, 0.37f, 0.26f, 1f),
                new Color(0.20f, 0.15f, 0.11f, 1f),
                31,
                true);
        }

        private static Color SamplePastWoodFloorHd2dPixel(int x, int y)
        {
            return SamplePlankPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.45f, 0.29f, 0.15f, 1f),
                new Color(0.54f, 0.36f, 0.20f, 1f),
                new Color(0.24f, 0.16f, 0.10f, 1f),
                new Color(0.64f, 0.47f, 0.31f, 1f),
                new Color(0.30f, 0.21f, 0.14f, 1f),
                47,
                false);
        }

        private static Color SampleCurrentInteriorWallHd2dPixel(int x, int y)
        {
            return SampleBrickPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.40f, 0.36f, 0.31f, 1f),
                new Color(0.50f, 0.44f, 0.38f, 1f),
                new Color(0.22f, 0.20f, 0.18f, 1f),
                new Color(0.56f, 0.49f, 0.43f, 1f),
                new Color(0.28f, 0.25f, 0.23f, 1f),
                61,
                true);
        }

        private static Color SamplePastInteriorWallHd2dPixel(int x, int y)
        {
            return SampleBrickPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.50f, 0.41f, 0.28f, 1f),
                new Color(0.63f, 0.52f, 0.35f, 1f),
                new Color(0.27f, 0.22f, 0.17f, 1f),
                new Color(0.68f, 0.58f, 0.42f, 1f),
                new Color(0.34f, 0.28f, 0.22f, 1f),
                73,
                false);
        }

        private static Color SampleCurrentFurnitureHd2dPixel(int x, int y)
        {
            return SampleFurnitureWoodPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.28f, 0.17f, 0.10f, 1f),
                new Color(0.36f, 0.22f, 0.13f, 1f),
                new Color(0.13f, 0.09f, 0.06f, 1f),
                new Color(0.48f, 0.30f, 0.18f, 1f),
                83,
                true);
        }

        private static Color SamplePastFurnitureHd2dPixel(int x, int y)
        {
            return SampleFurnitureWoodPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.43f, 0.26f, 0.13f, 1f),
                new Color(0.54f, 0.34f, 0.18f, 1f),
                new Color(0.20f, 0.13f, 0.08f, 1f),
                new Color(0.64f, 0.41f, 0.24f, 1f),
                97,
                false);
        }

        private static Color SampleBookSpinesHd2dPixel(int x, int y)
        {
            return SampleBookShelfTexturePixel(x, y, 128, 64, 1, 109, false);
        }

        private static Color SampleBookshelfFrontPaintedHd2dPixel(int x, int y)
        {
            return SampleBookShelfTexturePixel(x, y, 256, 128, 3, 131, true);
        }

        private static Color SamplePlankPlatePixel(int x, int y, int width, int height, Color boardA, Color boardB, Color seamColor, Color highlightColor, Color shadowColor, int seed, bool aged)
        {
            var boardWidth = 16;
            var boardIndex = x / boardWidth;
            var boardPhase = x % boardWidth;
            var boardMix = Hash01(boardIndex, y / 8, seed);
            var grain = Mathf.Sin((y * 0.18f) + (boardIndex * 0.75f) + seed * 0.021f) * 0.5f + 0.5f;
            var color = LerpColor(boardA, boardB, boardMix * 0.72f + grain * 0.28f);
            color = LerpColor(color, highlightColor, Mathf.Clamp01(0.22f - (x / (float)(width - 1)) * 0.18f));
            color = LerpColor(color, shadowColor, Mathf.Clamp01((x / (float)(width - 1)) * 0.22f + (y / (float)(height - 1)) * 0.18f));

            if (boardPhase <= 1 || boardPhase >= boardWidth - 2 || x == 0 || x == width - 1)
            {
                color = LerpColor(color, seamColor, 0.78f);
            }

            if ((y == 0 || y == height - 1) || ((y % 32 == 7 || y % 32 == 23) && (boardPhase == 3 || boardPhase == 12)))
            {
                color = LerpColor(color, Darken(seamColor, 0.32f), 0.88f);
            }

            if ((boardPhase == 6 || boardPhase == 7) && (y % 24 == 8 || y % 24 == 19))
            {
                color = LerpColor(color, Darken(highlightColor, 0.55f), 0.72f);
            }

            if (Hash01(x, y, seed + 17) > (aged ? 0.956f : 0.979f))
            {
                var scuff = Hash01(x, y, seed + 19);
                color = scuff > 0.55f ? Darken(color, aged ? 0.22f : 0.12f) : Lighten(color, aged ? 0.10f : 0.08f);
            }

            if (aged)
            {
                var dust = Hash01(x, y, seed + 23);
                if (dust > 0.92f)
                {
                    color = LerpColor(color, new Color(0.26f, 0.22f, 0.18f, 1f), 0.18f);
                }
            }

            return ShadeSurface(color, x, y, width, height, 0.16f, 0.10f);
        }

        private static Color SampleBrickPlatePixel(int x, int y, int width, int height, Color brickA, Color brickB, Color mortar, Color highlightColor, Color shadowColor, int seed, bool dusty)
        {
            var brickW = 16;
            var brickH = 12;
            var row = y / brickH;
            var rowOffset = (row & 1) == 0 ? 0 : brickW / 2;
            var shiftedX = (x + rowOffset) % width;
            var column = shiftedX / brickW;
            var withinX = shiftedX % brickW;
            var withinY = y % brickH;
            var brickTone = LerpColor(brickA, brickB, Hash01(column, row, seed));
            var seam = withinX <= 1 || withinX >= brickW - 2 || withinY <= 1 || withinY >= brickH - 2 || x == 0 || x == width - 1 || y == 0 || y == height - 1;
            if (seam)
            {
                brickTone = LerpColor(brickTone, mortar, 0.84f);
            }

            var diagonalWear = Mathf.Abs(((x * 2) + y + seed) % 29 - 14);
            if (diagonalWear <= 1 && withinX > 1 && withinX < brickW - 2)
            {
                brickTone = LerpColor(brickTone, Darken(brickTone, 0.35f), 0.72f);
            }

            var stainChance = Hash01(column, row, seed + 11);
            if (stainChance > 0.72f)
            {
                var stain = Hash01(x, y, seed + 13);
                var stainColor = dusty ? new Color(0.36f, 0.31f, 0.28f, 1f) : new Color(0.48f, 0.38f, 0.26f, 1f);
                brickTone = LerpColor(brickTone, stainColor, stain * 0.24f);
            }

            if (Hash01(x, y, seed + 17) > 0.985f)
            {
                brickTone = Darken(brickTone, 0.40f);
            }

            return ShadeSurface(brickTone, x, y, width, height, 0.18f, 0.12f);
        }

        private static Color SampleFurnitureWoodPlatePixel(int x, int y, int width, int height, Color woodA, Color woodB, Color seamColor, Color highlightColor, int seed, bool darkerUnderside)
        {
            var boardW = 32;
            var boardIndex = x / boardW;
            var withinX = x % boardW;
            var tone = LerpColor(woodA, woodB, Hash01(boardIndex, y / 6, seed));
            var grain = Mathf.Sin((y * 0.22f) + (boardIndex * 1.2f) + seed * 0.015f) * 0.5f + 0.5f;
            tone = LerpColor(tone, highlightColor, grain * 0.22f);

            if (withinX <= 1 || withinX >= boardW - 2 || x == 0 || x == width - 1)
            {
                tone = LerpColor(tone, seamColor, 0.82f);
            }

            if (Mathf.Abs(((x + seed) % 41) - 20) <= 1 || Mathf.Abs(((y + seed) % 37) - 18) <= 1)
            {
                tone = LerpColor(tone, Darken(seamColor, 0.18f), 0.62f);
            }

            var knotChance = Hash01(boardIndex, y / 18, seed + 7);
            if (knotChance > 0.66f)
            {
                var knotCx = boardIndex * boardW + 8 + (int)(Hash01(boardIndex, y, seed + 9) * 14f);
                var knotCy = 24 + (int)(Hash01(boardIndex, y, seed + 11) * 80f);
                var knotDx = Mathf.Abs(x - knotCx) / 8f;
                var knotDy = Mathf.Abs(y - knotCy) / 6f;
                var knot = Mathf.Clamp01(1f - (knotDx + knotDy));
                if (knot > 0f)
                {
                    tone = LerpColor(tone, Darken(highlightColor, 0.55f), knot * 0.34f);
                }
            }

            if (darkerUnderside)
            {
                tone = LerpColor(tone, seamColor, Mathf.Clamp01((y / (float)(height - 1)) * 0.42f));
            }

            return ShadeSurface(tone, x, y, width, height, 0.20f, 0.12f);
        }

        private static Color SampleBookShelfTexturePixel(int x, int y, int width, int height, int rowCount, int seed, bool bookshelfFront)
        {
            var rowTopMargin = bookshelfFront ? 6 : 5;
            var rowHeight = bookshelfFront ? 34 : 50;
            var rowGap = bookshelfFront ? 6 : 0;
            var rowStride = rowHeight + rowGap;
            var rowIndex = (y - rowTopMargin) / rowStride;
            if (rowIndex < 0 || rowIndex >= rowCount)
            {
                return SampleShelfGapPixel(x, y, width, height, seed, bookshelfFront);
            }

            var rowStart = rowTopMargin + rowIndex * rowStride;
            var rowEnd = rowStart + rowHeight;
            if (y < rowStart || y >= rowEnd)
            {
                return SampleShelfGapPixel(x, y, width, height, seed, bookshelfFront);
            }

            var columnWidth = 8;
            var columnIndex = x / columnWidth;
            var withinColumn = x % columnWidth;
            var color = PickBookSpineColor(columnIndex, rowIndex, seed, bookshelfFront);

            if (withinColumn <= 1)
            {
                color = Darken(color, 0.24f);
            }
            else if (withinColumn >= columnWidth - 2)
            {
                color = Lighten(color, 0.10f);
            }

            if ((withinColumn == 3 || withinColumn == 4) && (y - rowStart) % 9 < 7)
            {
                color = LerpColor(color, new Color(0.95f, 0.88f, 0.72f, 1f), bookshelfFront ? 0.10f : 0.14f);
            }

            if ((withinColumn == 2 || withinColumn == 5) && (y - rowStart) % 13 == 3)
            {
                color = LerpColor(color, Darken(color, 0.42f), 0.68f);
            }

            if ((y == rowStart || y == rowEnd - 1) && withinColumn > 1 && withinColumn < columnWidth - 2)
            {
                color = Darken(color, 0.28f);
            }

            if (Hash01(x, y, seed + 29) > (bookshelfFront ? 0.972f : 0.981f))
            {
                color = Lighten(color, bookshelfFront ? 0.14f : 0.10f);
            }

            return ShadeSurface(color, x, y, width, height, bookshelfFront ? 0.18f : 0.14f, bookshelfFront ? 0.11f : 0.08f);
        }

        private static Color SampleShelfGapPixel(int x, int y, int width, int height, int seed, bool bookshelfFront)
        {
            var shelf = bookshelfFront
                ? new Color(0.18f, 0.11f, 0.07f, 1f)
                : new Color(0.14f, 0.09f, 0.06f, 1f);
            var band = Mathf.Clamp01(1f - Mathf.Abs((y / (float)(height - 1)) - 0.5f) * 2f);
            var shadow = bookshelfFront ? 0.34f : 0.28f;
            shelf = LerpColor(shelf, Darken(shelf, 0.32f), shadow * 0.8f);
            if ((y % 16) <= 1 || (y % 16) >= 14)
            {
                shelf = Lighten(shelf, 0.08f);
            }

            if (Hash01(x, y, seed + 41) > 0.968f)
            {
                shelf = Lighten(shelf, 0.12f);
            }

            return ShadeSurface(shelf, x, y, width, height, 0.16f, 0.05f + band * 0.05f);
        }

        private static Color PickBookSpineColor(int columnIndex, int rowIndex, int seed, bool bookshelfFront)
        {
            var selector = Math.Abs((columnIndex * 7) + (rowIndex * 13) + seed) % (bookshelfFront ? 12 : 8);
            switch (selector)
            {
                case 0:
                    return new Color(0.62f, 0.16f, 0.16f, 1f);
                case 1:
                    return new Color(0.22f, 0.33f, 0.64f, 1f);
                case 2:
                    return new Color(0.70f, 0.55f, 0.18f, 1f);
                case 3:
                    return new Color(0.82f, 0.76f, 0.57f, 1f);
                case 4:
                    return new Color(0.38f, 0.20f, 0.42f, 1f);
                case 5:
                    return new Color(0.22f, 0.49f, 0.36f, 1f);
                case 6:
                    return new Color(0.58f, 0.28f, 0.12f, 1f);
                case 7:
                    return new Color(0.76f, 0.42f, 0.18f, 1f);
                case 8:
                    return new Color(0.76f, 0.18f, 0.22f, 1f);
                case 9:
                    return new Color(0.18f, 0.24f, 0.52f, 1f);
                case 10:
                    return new Color(0.85f, 0.65f, 0.26f, 1f);
                default:
                    return new Color(0.91f, 0.85f, 0.66f, 1f);
            }
        }

        private static Color ShadeSurface(Color color, int x, int y, int width, int height, float shadowAmount, float highlightAmount)
        {
            var u = width <= 1 ? 0f : x / (float)(width - 1);
            var v = height <= 1 ? 0f : y / (float)(height - 1);
            var light = Mathf.Clamp01(1f - (u * 0.62f + v * 0.28f));
            var shaded = LerpColor(Darken(color, shadowAmount), Lighten(color, highlightAmount), light);
            return shaded;
        }

        private static float Hash01(int x, int y, int seed)
        {
            unchecked
            {
                var hash = seed;
                hash ^= x * 374761393;
                hash = (hash << 13) ^ hash;
                hash ^= y * 668265263;
                hash *= 1274126177;
                hash ^= hash >> 16;
                return (hash & 0x7fffffff) / (float)int.MaxValue;
            }
        }

        private static Color LerpColor(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(
                a.r + ((b.r - a.r) * t),
                a.g + ((b.g - a.g) * t),
                a.b + ((b.b - a.b) * t),
                a.a + ((b.a - a.a) * t));
        }

        private static Color Darken(Color color, float amount)
        {
            var factor = 1f - Mathf.Clamp01(amount);
            return new Color(color.r * factor, color.g * factor, color.b * factor, color.a);
        }

        private static Color Lighten(Color color, float amount)
        {
            amount = Mathf.Clamp01(amount);
            return new Color(
                color.r + ((1f - color.r) * amount),
                color.g + ((1f - color.g) * amount),
                color.b + ((1f - color.b) * amount),
                color.a);
        }

        private static void CreateNameLabel(Transform parent, string text, Vector3 localPosition, Material material)
        {
            var label = new GameObject($"{text}_NameLabel");
            label.transform.SetParent(parent, false);
            label.transform.localPosition = localPosition;
            var mesh = label.AddComponent<TextMesh>();
            mesh.text = text;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.fontSize = 54;
            mesh.characterSize = 0.035f;
            mesh.color = Color.white;
            label.GetComponent<MeshRenderer>().sharedMaterial = material;
        }

        private static void CreateWorldLabel(Transform root, string text, Vector3 localPosition, Material material)
        {
            var label = new GameObject(text.Length > 42 ? text.Substring(0, 42) : text);
            label.transform.SetParent(root, false);
            label.transform.localPosition = localPosition;
            label.transform.localRotation = Quaternion.Euler(18f, 0f, 0f);
            var mesh = label.AddComponent<TextMesh>();
            mesh.text = text;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.fontSize = 48;
            mesh.characterSize = 0.075f;
            mesh.color = Color.white;
            label.GetComponent<MeshRenderer>().sharedMaterial = material;
        }

        private static GameObject CreateLandmarkCube(string objectName, Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material, bool keepCollider, TimeWindowPairedSpaceLandmarkKind kind, string landmarkId)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = objectName;
            cube.transform.SetParent(parent, false);
            cube.transform.localPosition = localPosition;
            cube.transform.localRotation = localRotation;
            cube.transform.localScale = localScale;
            cube.GetComponent<Renderer>().sharedMaterial = material;
            if (!keepCollider)
            {
                var collider = cube.GetComponent<Collider>();
                if (collider != null)
                {
                    UnityEngine.Object.DestroyImmediate(collider);
                }
            }

            var landmark = cube.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", landmarkId);
            SerializedSet(landmark, "kind", kind);
            SerializedSet(landmark, "countsForArrival", true);
            return cube;
        }

        private static GameObject CreateInvisibleColliderBox(string objectName, Transform parent, Vector3 localPosition, Vector3 localScale, string landmarkId)
        {
            var guard = new GameObject(objectName);
            guard.transform.SetParent(parent, false);
            guard.transform.localPosition = localPosition;
            guard.transform.localRotation = Quaternion.identity;
            guard.transform.localScale = Vector3.one;
            var collider = guard.AddComponent<BoxCollider>();
            collider.size = localScale;
            var landmark = guard.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", landmarkId);
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.WallOrLandmark);
            SerializedSet(landmark, "countsForArrival", true);
            return guard;
        }

        private static Materials EnsureMaterials()
        {
            return new Materials(
                PixelMaterial("current_ground", new Color32(42, 41, 38, 255), new Color32(63, 58, 51, 255), new Color32(31, 31, 30, 255), PixelPattern.Noise, false, new Vector2(4f, 4f)),
                PixelMaterial("current_grass", new Color32(38, 55, 36, 255), new Color32(53, 76, 48, 255), new Color32(28, 38, 27, 255), PixelPattern.Grass, false, new Vector2(6f, 6f)),
                PixelMaterial("current_path", new Color32(95, 72, 52, 255), new Color32(132, 102, 70, 255), new Color32(65, 52, 42, 255), PixelPattern.Stone, false, new Vector2(4f, 4f)),
                PaintedSurfaceMaterial("current_interior_floor", "current_interior_floor_hd2d_plate", 128, 128, SampleCurrentInteriorFloorHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("current_interior_wall", "current_interior_wall_hd2d_plate", 128, 128, SampleCurrentInteriorWallHd2dPixel, false, new Vector2(4f, 3f)),
                PixelMaterial("current_exterior_wall", new Color32(91, 69, 55, 255), new Color32(124, 89, 65, 255), new Color32(57, 47, 42, 255), PixelPattern.Bricks, false, new Vector2(4f, 3f)),
                PixelMaterial("current_roof", new Color32(87, 39, 35, 255), new Color32(117, 54, 45, 255), new Color32(55, 28, 27, 255), PixelPattern.Roof, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("current_furniture", "current_furniture_hd2d_plate", 128, 128, SampleCurrentFurnitureHd2dPixel, false, new Vector2(2f, 2f)),
                PixelMaterial("current_fence", new Color32(58, 48, 36, 255), new Color32(86, 69, 48, 255), new Color32(40, 34, 28, 255), PixelPattern.Planks, false, new Vector2(6f, 2f)),
                PixelMaterial("current_stone", new Color32(68, 67, 64, 255), new Color32(95, 93, 86, 255), new Color32(43, 43, 41, 255), PixelPattern.Stone, false, new Vector2(3f, 2f)),
                PixelMaterial("current_bed", new Color32(74, 73, 85, 255), new Color32(96, 93, 108, 255), new Color32(54, 53, 62, 255), PixelPattern.Cloth, false, new Vector2(2f, 2f)),
                PixelMaterial("current_leaf", new Color32(38, 65, 40, 255), new Color32(53, 82, 47, 255), new Color32(28, 45, 32, 255), PixelPattern.Grass, false, new Vector2(3f, 3f)),
                PixelMaterial("past_grass", new Color32(58, 106, 65, 255), new Color32(89, 139, 74, 255), new Color32(41, 82, 54, 255), PixelPattern.Grass, false, new Vector2(6f, 6f)),
                PixelMaterial("past_path", new Color32(139, 111, 70, 255), new Color32(171, 139, 87, 255), new Color32(96, 79, 58, 255), PixelPattern.Stone, false, new Vector2(4f, 4f)),
                PaintedSurfaceMaterial("past_wood_floor", "past_wood_floor_hd2d_plate", 128, 128, SamplePastWoodFloorHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("past_interior_wall", "past_interior_wall_hd2d_plate", 128, 128, SamplePastInteriorWallHd2dPixel, false, new Vector2(4f, 3f)),
                PixelMaterial("past_exterior_wall", new Color32(155, 112, 72, 255), new Color32(190, 138, 82, 255), new Color32(111, 78, 54, 255), PixelPattern.Bricks, false, new Vector2(4f, 3f)),
                PixelMaterial("past_roof", new Color32(149, 66, 48, 255), new Color32(194, 89, 58, 255), new Color32(101, 45, 40, 255), PixelPattern.Roof, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("past_furniture", "past_furniture_hd2d_plate", 128, 128, SamplePastFurnitureHd2dPixel, false, new Vector2(2f, 2f)),
                PixelMaterial("past_fence", new Color32(119, 84, 46, 255), new Color32(154, 108, 58, 255), new Color32(83, 62, 42, 255), PixelPattern.Planks, false, new Vector2(6f, 2f)),
                PixelMaterial("past_stone", new Color32(118, 115, 100, 255), new Color32(151, 146, 123, 255), new Color32(83, 82, 75, 255), PixelPattern.Stone, false, new Vector2(3f, 2f)),
                PixelMaterial("past_bed", new Color32(87, 121, 162, 255), new Color32(117, 151, 190, 255), new Color32(61, 82, 120, 255), PixelPattern.Cloth, false, new Vector2(2f, 2f)),
                PixelMaterial("leaf", new Color32(62, 122, 64, 255), new Color32(93, 158, 78, 255), new Color32(39, 91, 53, 255), PixelPattern.Grass, false, new Vector2(3f, 3f)),
                PixelMaterial("pillow", new Color32(212, 204, 177, 255), new Color32(236, 225, 190, 255), new Color32(166, 157, 137, 255), PixelPattern.Cloth, false, new Vector2(1f, 1f)),
                PixelMaterial("dust", new Color32(88, 82, 75, 255), new Color32(111, 104, 92, 255), new Color32(61, 57, 54, 255), PixelPattern.Noise, false, new Vector2(2f, 2f)),
                PaintedSurfaceMaterial("book", "book_spines_hd2d_plate", 128, 64, SampleBookSpinesHd2dPixel, false, new Vector2(1f, 1f)),
                PixelMaterial("lamp", new Color32(255, 204, 88, 255), new Color32(255, 236, 150, 255), new Color32(197, 126, 38, 255), PixelPattern.Checker, true, new Vector2(1f, 1f)),
                FlatMaterial("timewindow_cue_yellow_light", new Color(1.00f, 0.86f, 0.20f, 1f), true),
                FlatMaterial("timewindow_marker_yellow", new Color(1.00f, 0.78f, 0.05f, 1f), true),
                PixelMaterial("window_light", new Color32(133, 211, 255, 255), new Color32(215, 247, 255, 255), new Color32(52, 107, 151, 255), PixelPattern.Window, true, new Vector2(1f, 1f)),
                PixelMaterial("empty_window", new Color32(24, 31, 38, 255), new Color32(52, 60, 64, 255), new Color32(12, 17, 22, 255), PixelPattern.Window, true, new Vector2(1f, 1f)),
                PixelMaterial("water", new Color32(56, 119, 151, 255), new Color32(91, 171, 195, 255), new Color32(33, 72, 112, 255), PixelPattern.Water, true, new Vector2(1f, 1f)),
                PixelMaterial("rope", new Color32(142, 112, 70, 255), new Color32(181, 146, 89, 255), new Color32(89, 70, 47, 255), PixelPattern.Planks, false, new Vector2(1f, 1f)),
                PixelMaterial("flower_red", new Color32(190, 46, 54, 255), new Color32(239, 87, 79, 255), new Color32(117, 35, 53, 255), PixelPattern.Checker, true, new Vector2(1f, 1f)),
                PixelMaterial("flower_yellow", new Color32(225, 177, 62, 255), new Color32(255, 222, 100, 255), new Color32(145, 105, 44, 255), PixelPattern.Checker, true, new Vector2(1f, 1f)),
                PixelMaterial("flower_blue", new Color32(75, 104, 185, 255), new Color32(121, 157, 229, 255), new Color32(45, 61, 125, 255), PixelPattern.Checker, true, new Vector2(1f, 1f)),
                PixelMaterial("laundry_bright", new Color32(218, 219, 196, 255), new Color32(242, 238, 210, 255), new Color32(151, 165, 161, 255), PixelPattern.Cloth, true, new Vector2(1f, 1f)),
                PixelMaterial("laundry_accent", new Color32(109, 145, 192, 255), new Color32(151, 185, 222, 255), new Color32(65, 90, 141, 255), PixelPattern.Cloth, true, new Vector2(1f, 1f)),
                PixelMaterial("sign_paint", new Color32(178, 127, 61, 255), new Color32(211, 161, 82, 255), new Color32(92, 65, 43, 255), PixelPattern.Planks, false, new Vector2(1f, 1f)),
                FlatMaterial("shadow", new Color(0.10f, 0.10f, 0.11f, 1f), true),
                FlatMaterial("doorway_dark", new Color(0.035f, 0.032f, 0.038f, 1f), true),
                FlatMaterial("current_frame", new Color(1.00f, 0.42f, 0.17f, 1f), false),
                FlatMaterial("past_frame", new Color(0.28f, 0.95f, 1.00f, 1f), false),
                FlatMaterial("preview_frame", new Color(0.76f, 0.76f, 0.78f, 1f), false),
                FlatMaterial("threshold", new Color(0.20f, 0.95f, 0.82f, 1f), false),
                FlatMaterial("niro_body", new Color(0.26f, 0.42f, 0.78f, 1f), true),
                FlatMaterial("niro_past_body", new Color(0.46f, 0.72f, 0.96f, 1f), true),
                FlatMaterial("niro_accent", new Color(0.92f, 0.74f, 0.38f, 1f), true),
                FlatMaterial("memory_body", new Color(0.54f, 0.62f, 0.76f, 1f), true),
                FlatMaterial("memory_accent", new Color(0.22f, 0.26f, 0.34f, 1f), true),
                FlatMaterial("card_face", new Color(0.94f, 0.80f, 0.62f, 1f), true),
                FlatMaterial("label", Color.white, true),
                SpriteMaterial("niro_front_sprite", NiroFrontStripPath, Color.white, true),
                SpriteMaterial("niro_past_front_sprite", NiroFrontStripPath, new Color(0.72f, 0.88f, 1.0f, 0.92f), true),
                ApertureMaterial("house_aperture"));
        }

        private static Material SpriteMaterial(string id, string texturePath, Color tint, bool strip = false)
        {
            return SpriteStripMaterial(id, texturePath, tint, strip ? NiroAnimatedFrameCount : 1);
        }

        private static Material SpriteStripMaterial(string id, string texturePath, Color tint, int frameCount)
        {
            EnsureTextureImporter(texturePath);
            var texture = EnsureShadedSpriteTexture(id, texturePath);
            var material = FlatMaterial(id, tint, true);
            if (texture == null)
            {
                Debug.LogWarning($"Fast VS character texture missing: {texturePath}");
                return material;
            }

            if (material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", texture);
                material.SetTextureScale("_BaseMap", frameCount > 1 ? new Vector2(1f / frameCount, 1f) : Vector2.one);
                material.SetTextureOffset("_BaseMap", Vector2.zero);
            }

            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", texture);
                material.SetTextureScale("_MainTex", frameCount > 1 ? new Vector2(1f / frameCount, 1f) : Vector2.one);
                material.SetTextureOffset("_MainTex", Vector2.zero);
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", tint);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", tint);
            }

            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 1f);
            }

            if (material.HasProperty("_AlphaClip"))
            {
                material.SetFloat("_AlphaClip", 0f);
            }

            if (material.HasProperty("_SrcBlend"))
            {
                material.SetFloat("_SrcBlend", 5f);
            }

            if (material.HasProperty("_DstBlend"))
            {
                material.SetFloat("_DstBlend", 10f);
            }

            if (material.HasProperty("_ZWrite"))
            {
                material.SetFloat("_ZWrite", 0f);
            }

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = 3000;
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void EnsureTextureImporter(string texturePath)
        {
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceSynchronousImport);
            var importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            importer.textureType = TextureImporterType.Default;
            importer.alphaIsTransparency = true;
            importer.isReadable = true;
            importer.mipmapEnabled = false;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        private static Texture2D EnsureShadedSpriteTexture(string id, string texturePath)
        {
            var source = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            if (source == null)
            {
                Debug.LogWarning($"Fast VS character texture missing: {texturePath}");
                return null;
            }

            var path = $"{TextureDirectory}/FastVS_House_{id}_shaded.asset";
            var shaded = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (shaded != null && (shaded.width != source.width || shaded.height != source.height))
            {
                AssetDatabase.DeleteAsset(path);
                shaded = null;
            }

            if (shaded == null)
            {
                shaded = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
                AssetDatabase.CreateAsset(shaded, path);
            }

            shaded.name = $"FastVS_House_{id}_shaded";
            shaded.filterMode = FilterMode.Point;
            shaded.wrapMode = TextureWrapMode.Clamp;

            for (var y = 0; y < source.height; y++)
            {
                for (var x = 0; x < source.width; x++)
                {
                    shaded.SetPixel(x, y, ShadeSpritePixel(source.GetPixel(x, y), x, y, source.width, source.height));
                }
            }

            shaded.Apply(false, false);
            EditorUtility.SetDirty(shaded);
            return shaded;
        }

        private static Color ShadeSpritePixel(Color source, int x, int y, int width, int height)
        {
            if (source.a <= 0.03f)
            {
                return new Color(0f, 0f, 0f, 0f);
            }

            var frameX = x % NiroExpectedTextureWidth;
            var u = frameX / (float)(NiroExpectedTextureWidth - 1);
            var v = y / Mathf.Max(1f, height - 1f);
            var rightSide = Mathf.Clamp01((u - 0.54f) / 0.46f);
            var lowerBody = Mathf.Clamp01((0.34f - v) / 0.34f);
            var softShade = 1f - rightSide * 0.16f - lowerBody * 0.10f;
            var softWarmth = 1f + Mathf.Clamp01((0.34f - u) / 0.34f) * Mathf.Clamp01((v - 0.55f) / 0.45f) * 0.035f;
            return new Color(
                Mathf.Clamp01(source.r * softShade * softWarmth),
                Mathf.Clamp01(source.g * softShade * softWarmth),
                Mathf.Clamp01(source.b * softShade * softWarmth),
                source.a);
        }

        private static Material BookshelfFrontMaterial(string panelId, Vector2 textureScale)
        {
            var material = FlatMaterial($"bookshelf_front_painted_hd2d_{panelId}", Color.white, true);
            material.name = $"FastVS_House_bookshelf_front_painted_hd2d_{panelId}";
            var texture = EnsureGeneratedRepeatTexture("bookshelf_front_painted_hd2d", 256, 128, SampleBookshelfFrontPaintedHd2dPixel);
            AssignMaterialTexture(material, texture, textureScale);
            return material;
        }

        private static Material PixelMaterial(string id, Color32 a, Color32 b, Color32 c, PixelPattern pattern, bool unlit, Vector2 tiling)
        {
            var texture = EnsurePixelTexture(id, a, b, c, pattern);
            var material = FlatMaterial(id, Color.white, unlit);
            if (material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", texture);
                material.SetTextureScale("_BaseMap", tiling);
            }

            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", texture);
                material.SetTextureScale("_MainTex", tiling);
            }

            return material;
        }

        private static Texture2D EnsurePixelTexture(string id, Color32 a, Color32 b, Color32 c, PixelPattern pattern)
        {
            var path = $"{TextureDirectory}/FastVS_House_{id}.asset";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
                AssetDatabase.CreateAsset(texture, path);
            }

            texture.name = $"FastVS_House_{id}";
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Repeat;

            for (var y = 0; y < 32; y++)
            {
                for (var x = 0; x < 32; x++)
                {
                    texture.SetPixel(x, y, ResolvePixel(pattern, x, y, a, b, c));
                }
            }

            texture.Apply();
            EditorUtility.SetDirty(texture);
            return texture;
        }

        private static Color32 ResolvePixel(PixelPattern pattern, int x, int y, Color32 a, Color32 b, Color32 c)
        {
            switch (pattern)
            {
                case PixelPattern.Planks:
                    return y % 8 == 0 || x % 15 == 0 ? c : ((x + y) % 5 == 0 ? b : a);
                case PixelPattern.Bricks:
                    return y % 8 == 0 || (x + (y / 8 % 2) * 8) % 16 == 0 ? c : ((x + y) % 7 == 0 ? b : a);
                case PixelPattern.Grass:
                    return (x * 3 + y * 5) % 11 < 3 ? b : ((x + y) % 13 == 0 ? c : a);
                case PixelPattern.Stone:
                    return x % 8 == 0 || y % 8 == 0 ? c : ((x * 7 + y * 3) % 17 < 4 ? b : a);
                case PixelPattern.Roof:
                    return (x + y) % 9 < 2 ? b : (y % 6 == 0 ? c : a);
                case PixelPattern.Cloth:
                    return x % 7 == 0 || y % 7 == 0 ? b : ((x + y) % 6 == 0 ? c : a);
                case PixelPattern.Book:
                    var spineWidth = 6 + ((x / 19) % 4);
                    var spineX = x % spineWidth;
                    var group = x / spineWidth;
                    if (spineX == 0 || spineX == spineWidth - 1)
                    {
                        return c;
                    }

                    var spineColor = group % 3 == 0 ? a : (group % 3 == 1 ? b : c);
                    if (spineX > 1 && spineX < spineWidth - 2 && (y % 18 == 5 || y % 18 == 6))
                    {
                        return group % 2 == 0 ? b : a;
                    }

                    return spineColor;
                case PixelPattern.Window:
                    if (x < 2 || x > 29 || y < 2 || y > 29)
                    {
                        return c;
                    }

                    if ((x + y * 2) % 13 < 2)
                    {
                        return b;
                    }

                    return (x * 5 + y * 3) % 23 < 3 ? c : a;
                case PixelPattern.Checker:
                    return ((x / 4) + (y / 4)) % 2 == 0 ? a : b;
                case PixelPattern.Water:
                    return (x + y * 2) % 10 < 2 ? b : ((x * 3 + y) % 17 < 3 ? c : a);
                default:
                    return (x * 13 + y * 7) % 19 < 5 ? b : ((x * 5 + y * 11) % 23 < 4 ? c : a);
            }
        }

        private static Material FlatMaterial(string id, Color color, bool unlit)
        {
            var path = $"{MaterialDirectory}/FastVS_House_{id}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find(unlit ? "Universal Render Pipeline/Unlit" : "Universal Render Pipeline/Lit");
            if (shader == null)
            {
                throw new InvalidOperationException($"Required shader not found: {id}");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            material.doubleSidedGI = true;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            if (material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", null);
            }

            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", null);
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", 0f);
            }

            if (!unlit && ShouldApplyHd2dMatteMaterial(id))
            {
                if (material.HasProperty("_Metallic"))
                {
                    material.SetFloat("_Metallic", 0f);
                }

                if (material.HasProperty("_Smoothness"))
                {
                    material.SetFloat("_Smoothness", 0.16f);
                }

                if (material.HasProperty("_SpecularHighlights"))
                {
                    material.SetFloat("_SpecularHighlights", 0f);
                }
            }

            return material;
        }

        private static bool ShouldApplyHd2dMatteMaterial(string id)
        {
            return id != "current_frame" &&
                id != "past_frame" &&
                id != "preview_frame" &&
                id != "threshold";
        }

        private static Material ApertureMaterial(string id)
        {
            var path = $"{MaterialDirectory}/FastVS_House_{id}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find("Anemora/Review/PortalApertureOverlay");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Unlit");
            }

            if (shader == null)
            {
                throw new InvalidOperationException("Portal aperture shader not found.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", Color.white);
            }

            return material;
        }

        private static void SerializedSet(UnityEngine.Object target, string fieldName, object value)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(fieldName);
            if (property == null)
            {
                throw new InvalidOperationException($"Serialized field not found: {target.GetType().Name}.{fieldName}");
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = value as UnityEngine.Object;
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = (Vector2)value;
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = (Vector3)value;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = Convert.ToSingle(value);
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = Convert.ToInt32(value);
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = Convert.ToBoolean(value);
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = value as string;
                    break;
                case SerializedPropertyType.Enum:
                    property.enumValueIndex = Convert.ToInt32(value);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported property type for {fieldName}: {property.propertyType}");
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void EnsureExternalCharacterAssets()
        {
            if (!File.Exists(AriaNormalLoopStripPath))
            {
                Debug.LogWarning($"Fast VS bundled Aria character asset missing: {AriaNormalLoopStripPath}");
                return;
            }

            AssetDatabase.ImportAsset(AriaNormalLoopStripPath, ImportAssetOptions.ForceSynchronousImport);
            EnsureTextureImporter(AriaNormalLoopStripPath);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            var parts = path.Split('/');
            var current = parts[0];
            for (var index = 1; index < parts.Length; index++)
            {
                var next = $"{current}/{parts[index]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[index]);
                }

                current = next;
            }
        }

        private enum PixelPattern
        {
            Noise,
            Planks,
            Bricks,
            Grass,
            Stone,
            Roof,
            Cloth,
            Book,
            Window,
            Checker,
            Water
        }

        private readonly struct HouseMapAreas
        {
            public HouseMapAreas(GameObject interior, GameObject exterior, GameObject centralPlaza, GameObject library)
            {
                Interior = interior;
                Exterior = exterior;
                CentralPlaza = centralPlaza;
                Library = library;
            }

            public GameObject Interior { get; }
            public GameObject Exterior { get; }
            public GameObject CentralPlaza { get; }
            public GameObject Library { get; }
        }

        private readonly struct Materials
        {
            public Materials(
                Material currentGround,
                Material currentGrass,
                Material currentPath,
                Material currentInteriorFloor,
                Material currentInteriorWall,
                Material currentExteriorWall,
                Material currentRoof,
                Material currentFurniture,
                Material currentFence,
                Material currentStone,
                Material currentBed,
                Material currentLeaf,
                Material pastGrass,
                Material pastPath,
                Material pastWoodFloor,
                Material pastInteriorWall,
                Material pastExteriorWall,
                Material pastRoof,
                Material pastFurniture,
                Material pastFence,
                Material pastStone,
                Material pastBed,
                Material leaf,
                Material pillow,
                Material dust,
                Material book,
                Material lamp,
                Material redLight,
                Material redMarker,
                Material windowLight,
                Material emptyWindow,
                Material water,
                Material rope,
                Material flowerRed,
                Material flowerYellow,
                Material flowerBlue,
                Material laundryBright,
                Material laundryAccent,
                Material signPaint,
                Material shadow,
                Material doorwayDark,
                Material currentFrame,
                Material pastFrame,
                Material previewFrame,
                Material threshold,
                Material niroBody,
                Material niroPastBody,
                Material niroAccent,
                Material memoryBody,
                Material memoryAccent,
                Material cardFace,
                Material label,
                Material niroSprite,
                Material niroPastSprite,
                Material aperture)
            {
                CurrentGround = currentGround;
                CurrentGrass = currentGrass;
                CurrentPath = currentPath;
                CurrentInteriorFloor = currentInteriorFloor;
                CurrentInteriorWall = currentInteriorWall;
                CurrentExteriorWall = currentExteriorWall;
                CurrentRoof = currentRoof;
                CurrentFurniture = currentFurniture;
                CurrentFence = currentFence;
                CurrentStone = currentStone;
                CurrentBed = currentBed;
                CurrentLeaf = currentLeaf;
                PastGrass = pastGrass;
                PastPath = pastPath;
                PastWoodFloor = pastWoodFloor;
                PastInteriorWall = pastInteriorWall;
                PastExteriorWall = pastExteriorWall;
                PastRoof = pastRoof;
                PastFurniture = pastFurniture;
                PastFence = pastFence;
                PastStone = pastStone;
                PastBed = pastBed;
                Leaf = leaf;
                Pillow = pillow;
                Dust = dust;
                Book = book;
                Lamp = lamp;
                RedLight = redLight;
                RedMarker = redMarker;
                WindowLight = windowLight;
                EmptyWindow = emptyWindow;
                Water = water;
                Rope = rope;
                FlowerRed = flowerRed;
                FlowerYellow = flowerYellow;
                FlowerBlue = flowerBlue;
                LaundryBright = laundryBright;
                LaundryAccent = laundryAccent;
                SignPaint = signPaint;
                Shadow = shadow;
                DoorwayDark = doorwayDark;
                CurrentFrame = currentFrame;
                PastFrame = pastFrame;
                PreviewFrame = previewFrame;
                Threshold = threshold;
                NiroBody = niroBody;
                NiroPastBody = niroPastBody;
                NiroAccent = niroAccent;
                MemoryBody = memoryBody;
                MemoryAccent = memoryAccent;
                CardFace = cardFace;
                Label = label;
                NiroSprite = niroSprite;
                NiroPastSprite = niroPastSprite;
                Aperture = aperture;
            }

            public Material CurrentGround { get; }
            public Material CurrentGrass { get; }
            public Material CurrentPath { get; }
            public Material CurrentInteriorFloor { get; }
            public Material CurrentInteriorWall { get; }
            public Material CurrentExteriorWall { get; }
            public Material CurrentRoof { get; }
            public Material CurrentFurniture { get; }
            public Material CurrentFence { get; }
            public Material CurrentStone { get; }
            public Material CurrentBed { get; }
            public Material CurrentLeaf { get; }
            public Material PastGrass { get; }
            public Material PastPath { get; }
            public Material PastWoodFloor { get; }
            public Material PastInteriorWall { get; }
            public Material PastExteriorWall { get; }
            public Material PastRoof { get; }
            public Material PastFurniture { get; }
            public Material PastFence { get; }
            public Material PastStone { get; }
            public Material PastBed { get; }
            public Material Leaf { get; }
            public Material Pillow { get; }
            public Material Dust { get; }
            public Material Book { get; }
            public Material Lamp { get; }
            public Material RedLight { get; }
            public Material RedMarker { get; }
            public Material WindowLight { get; }
            public Material EmptyWindow { get; }
            public Material Water { get; }
            public Material Rope { get; }
            public Material FlowerRed { get; }
            public Material FlowerYellow { get; }
            public Material FlowerBlue { get; }
            public Material LaundryBright { get; }
            public Material LaundryAccent { get; }
            public Material SignPaint { get; }
            public Material Shadow { get; }
            public Material DoorwayDark { get; }
            public Material CurrentFrame { get; }
            public Material PastFrame { get; }
            public Material PreviewFrame { get; }
            public Material Threshold { get; }
            public Material NiroBody { get; }
            public Material NiroPastBody { get; }
            public Material NiroAccent { get; }
            public Material MemoryBody { get; }
            public Material MemoryAccent { get; }
            public Material CardFace { get; }
            public Material Label { get; }
            public Material NiroSprite { get; }
            public Material NiroPastSprite { get; }
            public Material Aperture { get; }
        }
    }
}
