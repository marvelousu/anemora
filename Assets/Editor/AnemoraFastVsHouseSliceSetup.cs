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
            CreateHd2dDepthFraming(currentAreas, pastAreas);
            ApplyInitialReviewLayers(currentRoot, pastRoot, player.transform, camera);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS house slice scene created: {ScenePath}");
        }

        public static void ValidateHouseSliceBatch()
        {
            CreateHouseSliceScene();
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
            ValidateFastVsHd2dFourthCycleHeroPropTextures();
            ValidateFastVsHd2dFifthCycleObjectDetails();
            ValidateFastVsHd2dEighthCycleBookPalette();
            ValidateFastVsHd2dNinthCyclePathStone();
            ValidateFastVsHd2dTenthCycleGrassTexture();
            ValidateFastVsHd2dEleventhCycleOutdoorGroundDetails();
            ValidateFastVsHd2dTwelfthCycleFacadeDetails();
            ValidateFastVsHd2dThirteenthCycleLibraryPropDetails();
            ValidateFastVsHd2dFourteenthCycleHouseInteriorDetails();
            ValidateFastVsHd2dTwentySecondCycleHouseInteriorLifeProps();
            ValidateFastVsHd2dFifteenthCycleCentralPlazaDetails();
            ValidateFastVsHd2dTwentyThirdCycleOutdoorEdgeDressing();
            ValidateFastVsHd2dTwentyFourthCycleLibraryWindowLight();
            ValidateFastVsHd2dSixteenthCycleHouseExteriorDetails();
            ValidateFastVsHd2dSeventeenthCycleCharacterContactShadows();
            ValidateFastVsHd2dTwentyFifthCycleCharacterGroundBounce();
            ValidateFastVsHd2dEighteenthCycleLibraryFacadeCloseDetails();
            ValidateFastVsHd2dNineteenthCycleCurrentLibrarySideShelves();
            ValidateFastVsHd2dTwentiethCycleCurrentLibrarySideShelfVisibility();
            ValidateFastVsHd2dTwentyFirstCycleCurrentLibraryAtmosphere();
            ValidateFastVsHd2dSeventhCycleDepthFraming();
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

        public static void CaptureHd2dFourthCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_hero_props_20260520");
        }

        public static void CaptureHd2dFifthCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_object_details_20260520");
        }

        public static void CaptureHd2dSeventhCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_depth_framing_20260520");
        }

        public static void CaptureHd2dEighthCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_book_palette_20260520");
        }

        public static void CaptureHd2dNinthCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_path_stone_20260520");
        }

        public static void CaptureHd2dTenthCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_grass_texture_20260520");
        }

        public static void CaptureHd2dEleventhCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_outdoor_ground_detail_20260520");
        }

        public static void CaptureHd2dTwelfthCycleScreenshotsBatch()
        {
            CaptureReviewScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_facade_detail_20260520");
        }

        public static void CaptureHd2dThirteenthCycleScreenshotsBatch()
        {
            CaptureHd2dThirteenthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_library_prop_detail_20260520");
        }

        public static void CaptureHd2dFourteenthCycleScreenshotsBatch()
        {
            CaptureHd2dFourteenthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_house_interior_detail_20260520");
        }

        public static void CaptureHd2dFifteenthCycleScreenshotsBatch()
        {
            CaptureHd2dFifteenthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_central_plaza_detail_20260520");
        }

        public static void CaptureHd2dSixteenthCycleScreenshotsBatch()
        {
            CaptureHd2dSixteenthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_house_exterior_detail_20260520");
        }

        public static void CaptureHd2dSeventeenthCycleScreenshotsBatch()
        {
            CaptureHd2dSeventeenthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_character_contact_shadow_20260520");
        }

        public static void CaptureHd2dEighteenthCycleScreenshotsBatch()
        {
            CaptureHd2dEighteenthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_library_facade_close_detail_20260520");
        }

        public static void CaptureHd2dNineteenthCycleScreenshotsBatch()
        {
            CaptureHd2dNineteenthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_current_library_side_shelves_20260520");
        }

        public static void CaptureHd2dTwentiethCycleScreenshotsBatch()
        {
            CaptureHd2dTwentiethCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_current_library_side_shelf_visibility_20260520");
        }

        public static void CaptureHd2dTwentyFirstCycleScreenshotsBatch()
        {
            CaptureHd2dTwentyFirstCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_current_library_atmosphere_20260520");
        }

        public static void CaptureHd2dTwentySecondCycleScreenshotsBatch()
        {
            CaptureHd2dTwentySecondCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_house_interior_life_props_20260520");
        }

        public static void CaptureHd2dTwentyThirdCycleScreenshotsBatch()
        {
            CaptureHd2dTwentyThirdCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_outdoor_edge_dressing_20260520");
        }

        public static void CaptureHd2dTwentyFourthCycleScreenshotsBatch()
        {
            CaptureHd2dTwentyFourthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_library_window_light_20260520");
        }

        public static void CaptureHd2dTwentyFifthCycleScreenshotsBatch()
        {
            CaptureHd2dTwentyFifthCycleScreenshotsToDirectory("docs/devlog/screenshots/fast_vs_hd2d_character_ground_bounce_20260520");
        }

        public static void CaptureHd2dCloseReviewScreenshotsBatch()
        {
            const string outputDirectory = "docs/devlog/screenshots/fast_vs_hd2d_close_review_20260520";

            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS close-review screenshot capture failed: scene review components are missing.");
            }

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(-2.95f, 0.02f, -1.80f),
                HouseInteriorCenter + new Vector3(0.55f, 0.58f, -0.68f),
                new Vector3(0f, 1.10f, -2.55f),
                new Vector3(0f, 0.08f, 0.10f),
                outputDirectory,
                "01_house_interior_bed_book_close.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(-3.40f, 0.02f, 0.55f),
                HouseExteriorCenter + new Vector3(-1.05f, 0.85f, -1.45f),
                new Vector3(0f, 1.22f, -2.12f),
                new Vector3(0f, 0.18f, 0.22f),
                outputDirectory,
                "02_house_exterior_door_close.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                CentralPlazaVsCenter + new Vector3(-2.10f, 0.02f, 5.15f),
                CentralPlazaVsCenter + new Vector3(0f, 1.10f, 7.78f),
                new Vector3(0f, 1.30f, -2.30f),
                new Vector3(0f, 0.16f, 0.22f),
                outputDirectory,
                "03_plaza_library_door_current_close.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                CentralPlazaVsCenter + new Vector3(-2.10f, 0.02f, 5.15f),
                CentralPlazaVsCenter + new Vector3(-2.35f, 1.42f, 7.82f),
                new Vector3(0f, 0.76f, -2.35f),
                new Vector3(0f, 0.02f, 0.18f),
                outputDirectory,
                "04_plaza_library_windows_past_close.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(-3.55f, 0.02f, -1.05f),
                RetoLibraryDeskLocalPosition + new Vector3(-0.30f, 0.42f, 0.10f),
                new Vector3(0f, 1.08f, -1.82f),
                new Vector3(0f, 0.14f, 0.18f),
                outputDirectory,
                "05_library_reto_book_close.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(-2.65f, 0.02f, -2.15f),
                LibraryVsCenter + new Vector3(2.55f, 0.22f, -2.50f),
                new Vector3(0f, 0.82f, -1.38f),
                new Vector3(0f, 0.04f, 0.06f),
                outputDirectory,
                "06_library_rubble_current_close.png");

            ValidateCloseReviewOutputExists(outputDirectory, "01_house_interior_bed_book_close.png");
            ValidateCloseReviewOutputExists(outputDirectory, "02_house_exterior_door_close.png");
            ValidateCloseReviewOutputExists(outputDirectory, "03_plaza_library_door_current_close.png");
            ValidateCloseReviewOutputExists(outputDirectory, "04_plaza_library_windows_past_close.png");
            ValidateCloseReviewOutputExists(outputDirectory, "05_library_reto_book_close.png");
            ValidateCloseReviewOutputExists(outputDirectory, "06_library_rubble_current_close.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS close-review screenshots captured: {Path.GetFullPath(outputDirectory)}");
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

        private static void CaptureHd2dThirteenthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS thirteenth-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                RetoLibraryDeskLocalPosition + new Vector3(-1.08f, 0.02f, -1.18f),
                CurrentLibraryRetoDeskBookInitialLocalPosition + new Vector3(0.34f, 0.03f, -0.24f),
                new Vector3(0.28f, 1.00f, -2.05f),
                new Vector3(0.12f, 0.18f, 0.08f),
                outputDirectory,
                "01_current_library_reto_desk_loose_papers.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(2.95f, 0.02f, -2.90f),
                LibraryVsCenter + new Vector3(4.38f, 0.30f, -3.00f),
                new Vector3(0.24f, 1.00f, -1.95f),
                new Vector3(0.10f, 0.16f, 0.06f),
                outputDirectory,
                "02_current_library_floor_book_stack_west.png");

            CaptureReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(0f, 0.02f, -0.40f),
                $"{outputDirectory}/03_current_library_shelf_debris_east.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(-0.30f, 0.02f, -0.42f),
                LibraryVsCenter + new Vector3(-0.12f, 0.40f, -0.92f),
                new Vector3(0.22f, 1.02f, -2.02f),
                new Vector3(0.10f, 0.18f, 0.06f),
                outputDirectory,
                "04_past_library_long_table_book_pair_a.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(2.70f, 0.02f, -0.45f),
                LibraryVsCenter + new Vector3(-3.98f, 0.48f, 0.82f),
                new Vector3(0.18f, 1.00f, -1.90f),
                new Vector3(0.08f, 0.18f, 0.08f),
                outputDirectory,
                "05_past_library_shelf_ledger_west.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(0.72f, 0.02f, -0.42f),
                LibraryVsCenter + new Vector3(0.92f, 0.40f, -0.92f),
                new Vector3(0.24f, 0.98f, -2.00f),
                new Vector3(0.10f, 0.18f, 0.06f),
                outputDirectory,
                "06_past_library_long_table_book_pair_b.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS thirteenth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dFourteenthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS fourteenth-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(-2.20f, 0.02f, 0.98f),
                HouseInteriorCenter + new Vector3(-1.58f, 0.60f, 0.72f),
                new Vector3(0.08f, 1.00f, -2.10f),
                new Vector3(0.04f, 0.10f, 0.12f),
                outputDirectory,
                "01_current_house_interior_bed_detail.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(1.18f, 0.02f, -0.78f),
                HouseInteriorCenter + new Vector3(1.26f, 0.60f, -0.66f),
                new Vector3(-0.04f, 1.00f, -2.06f),
                new Vector3(-0.02f, 0.10f, 0.08f),
                outputDirectory,
                "02_current_house_interior_table_detail.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(-2.20f, 0.02f, 0.98f),
                HouseInteriorCenter + new Vector3(-1.60f, 0.60f, 0.72f),
                new Vector3(0.08f, 1.00f, -2.10f),
                new Vector3(0.04f, 0.10f, 0.12f),
                outputDirectory,
                "03_past_house_interior_bed_detail.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(1.18f, 0.02f, -0.78f),
                HouseInteriorCenter + new Vector3(1.26f, 0.60f, -0.66f),
                new Vector3(-0.04f, 1.00f, -2.06f),
                new Vector3(-0.02f, 0.10f, 0.08f),
                outputDirectory,
                "04_past_house_interior_table_detail.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS fourteenth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dFifteenthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS fifteenth-cycle screenshot capture failed: scene review components are missing.");
            }

            var fountainPlayerLocal = CentralPlazaVsCenter + new Vector3(-0.72f, 0.02f, -0.46f);
            var libraryApproachPlayerLocal = CentralPlazaVsCenter + new Vector3(3.14f, 0.02f, 3.95f);
            var libraryApproachAnchorLocal = CentralPlazaVsCenter + new Vector3(3.42f, 0.02f, 4.28f);

            CaptureReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                fountainPlayerLocal,
                Path.Combine(outputDirectory, "01_current_plaza_fountain_detail.png"));
            ValidateScreenshotOutputExists(outputDirectory, "01_current_plaza_fountain_detail.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                libraryApproachPlayerLocal,
                libraryApproachAnchorLocal,
                new Vector3(0.20f, 1.02f, -2.12f),
                new Vector3(0.05f, 0.10f, 0.12f),
                outputDirectory,
                "02_current_plaza_library_approach_detail.png");
            ValidateScreenshotOutputExists(outputDirectory, "02_current_plaza_library_approach_detail.png");

            CaptureOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                fountainPlayerLocal,
                Path.Combine(outputDirectory, "03_past_plaza_fountain_detail.png"));
            ValidateScreenshotOutputExists(outputDirectory, "03_past_plaza_fountain_detail.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                libraryApproachPlayerLocal,
                libraryApproachAnchorLocal,
                new Vector3(0.20f, 1.02f, -2.12f),
                new Vector3(0.05f, 0.10f, 0.12f),
                outputDirectory,
                "04_past_plaza_library_approach_detail.png");
            ValidateScreenshotOutputExists(outputDirectory, "04_past_plaza_library_approach_detail.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS fifteenth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dSixteenthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS sixteenth-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(-1.92f, 0.02f, -1.56f),
                $"{outputDirectory}/01_current_house_exterior_porch_detail.png");
            ValidateScreenshotOutputExists(outputDirectory, "01_current_house_exterior_porch_detail.png");

            CaptureReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(4.92f, 0.02f, 0.84f),
                $"{outputDirectory}/02_current_house_exterior_road_detail.png");
            ValidateScreenshotOutputExists(outputDirectory, "02_current_house_exterior_road_detail.png");

            CaptureOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(-1.92f, 0.02f, -1.56f),
                $"{outputDirectory}/03_past_house_exterior_porch_detail.png");
            ValidateScreenshotOutputExists(outputDirectory, "03_past_house_exterior_porch_detail.png");

            CaptureOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(4.92f, 0.02f, 0.84f),
                $"{outputDirectory}/04_past_house_exterior_road_detail.png");
            ValidateScreenshotOutputExists(outputDirectory, "04_past_house_exterior_road_detail.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS sixteenth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dSeventeenthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS seventeenth-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(-0.28f, 0.02f, -0.42f),
                HouseInteriorCenter + new Vector3(-0.28f, 0.12f, -0.24f),
                new Vector3(0.18f, 1.02f, -2.00f),
                new Vector3(0.04f, 0.10f, 0.12f),
                outputDirectory,
                "01_interior_niro_contact_shadow.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                ExteriorDoorExitTarget,
                ExteriorDoorExitTarget + new Vector3(-0.08f, 0.05f, 0.16f),
                new Vector3(0.20f, 1.02f, -1.98f),
                new Vector3(0.08f, 0.12f, 0.10f),
                outputDirectory,
                "02_exterior_niro_contact_shadow.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                RetoLibraryDeskLocalPosition + new Vector3(-1.08f, 0.02f, -1.18f),
                RetoLibraryDeskLocalPosition + new Vector3(0.03f, 0.35f, 0.05f),
                new Vector3(0.22f, 1.02f, -2.00f),
                new Vector3(0.10f, 0.18f, 0.08f),
                outputDirectory,
                "03_library_reto_contact_shadow.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                PastLibraryPersonCueLocalPosition + new Vector3(-1.06f, 0.02f, -1.16f),
                PastLibraryPersonCueLocalPosition + new Vector3(-0.02f, 0.34f, 0.02f),
                new Vector3(0.22f, 1.00f, -1.98f),
                new Vector3(0.08f, 0.18f, 0.08f),
                outputDirectory,
                "04_past_library_aria_contact_shadow.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS seventeenth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dEighteenthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS eighteenth-cycle screenshot capture failed: scene review components are missing.");
            }

            var currentDoorPlayerLocal = CentralPlazaVsCenter + new Vector3(-2.10f, 0.02f, 5.15f);
            var currentDoorAnchorLocal = CentralPlazaVsCenter + new Vector3(0f, 0.78f, 7.64f);
            var currentWindowAnchorLocal = CentralPlazaVsCenter + new Vector3(-2.35f, 1.08f, 7.68f);

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                currentDoorPlayerLocal,
                currentDoorAnchorLocal,
                new Vector3(0f, 1.30f, -1.86f),
                new Vector3(0f, 0.20f, 0.16f),
                outputDirectory,
                "01_current_library_facade_door_close_detail.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                currentDoorPlayerLocal,
                currentWindowAnchorLocal,
                new Vector3(-0.72f, 1.46f, -0.98f),
                new Vector3(-0.30f, 0.34f, 0.12f),
                outputDirectory,
                "02_current_library_facade_window_close_detail.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                currentDoorPlayerLocal,
                currentDoorAnchorLocal,
                new Vector3(0f, 1.30f, -1.86f),
                new Vector3(0f, 0.20f, 0.16f),
                outputDirectory,
                "03_past_library_facade_door_close_detail.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                currentDoorPlayerLocal,
                currentWindowAnchorLocal,
                new Vector3(-0.72f, 1.46f, -0.98f),
                new Vector3(-0.30f, 0.34f, 0.12f),
                outputDirectory,
                "04_past_library_facade_window_close_detail.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS eighteenth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dNineteenthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS nineteenth-cycle screenshot capture failed: scene review components are missing.");
            }

            var currentLeftShelfPlayerLocal = LibraryVsCenter + new Vector3(-4.78f, 0.02f, 1.20f);
            var currentLeftShelfAnchorLocal = LibraryVsCenter + new Vector3(-4.78f, 0.92f, 0.42f);
            var currentRightShelfPlayerLocal = LibraryVsCenter + new Vector3(4.78f, 0.02f, 1.20f);
            var currentRightShelfAnchorLocal = LibraryVsCenter + new Vector3(4.78f, 0.92f, 0.42f);

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                currentLeftShelfPlayerLocal,
                currentLeftShelfAnchorLocal,
                new Vector3(4.15f, 1.52f, 1.18f),
                new Vector3(0.55f, 0.20f, 0.08f),
                outputDirectory,
                "01_current_library_left_empty_shelf.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                currentRightShelfPlayerLocal,
                currentRightShelfAnchorLocal,
                new Vector3(-4.15f, 1.52f, 1.18f),
                new Vector3(-0.55f, 0.20f, 0.08f),
                outputDirectory,
                "02_current_library_right_empty_shelf.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                currentLeftShelfPlayerLocal,
                currentLeftShelfAnchorLocal,
                new Vector3(4.15f, 1.52f, 1.18f),
                new Vector3(0.55f, 0.20f, 0.08f),
                outputDirectory,
                "03_past_library_left_full_shelf_reference.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                currentRightShelfPlayerLocal,
                currentRightShelfAnchorLocal,
                new Vector3(-4.15f, 1.52f, 1.18f),
                new Vector3(-0.55f, 0.20f, 0.08f),
                outputDirectory,
                "04_past_library_right_full_shelf_reference.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS nineteenth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dTwentiethCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS twentieth-cycle screenshot capture failed: scene review components are missing.");
            }

            var currentLeftShelfPlayerLocal = LibraryVsCenter + new Vector3(-4.78f, 0.02f, 1.20f);
            var currentLeftShelfAnchorLocal = LibraryVsCenter + new Vector3(-4.78f, 0.94f, 0.50f);
            var currentRightShelfPlayerLocal = LibraryVsCenter + new Vector3(4.78f, 0.02f, 1.20f);
            var currentRightShelfAnchorLocal = LibraryVsCenter + new Vector3(4.78f, 0.94f, 0.50f);

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                currentLeftShelfPlayerLocal,
                currentLeftShelfAnchorLocal,
                new Vector3(4.05f, 1.60f, 1.44f),
                new Vector3(0.40f, 0.22f, 0.06f),
                outputDirectory,
                "01_current_left_empty_shelf_visibility.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                currentRightShelfPlayerLocal,
                currentRightShelfAnchorLocal,
                new Vector3(-4.05f, 1.60f, 1.44f),
                new Vector3(-0.40f, 0.22f, 0.06f),
                outputDirectory,
                "02_current_right_empty_shelf_visibility.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                currentLeftShelfPlayerLocal,
                currentLeftShelfAnchorLocal,
                new Vector3(4.05f, 1.60f, 1.44f),
                new Vector3(0.40f, 0.22f, 0.06f),
                outputDirectory,
                "03_past_left_full_shelf_reference.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                currentRightShelfPlayerLocal,
                currentRightShelfAnchorLocal,
                new Vector3(-4.05f, 1.60f, 1.44f),
                new Vector3(-0.40f, 0.22f, 0.06f),
                outputDirectory,
                "04_past_right_full_shelf_reference.png");

            ValidateCloseReviewOutputExists(outputDirectory, "01_current_left_empty_shelf_visibility.png");
            ValidateCloseReviewOutputExists(outputDirectory, "02_current_right_empty_shelf_visibility.png");
            ValidateCloseReviewOutputExists(outputDirectory, "03_past_left_full_shelf_reference.png");
            ValidateCloseReviewOutputExists(outputDirectory, "04_past_right_full_shelf_reference.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS twentieth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dTwentyFirstCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS twenty-first-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(0f, 0.02f, -4.52f),
                LibraryVsCenter + new Vector3(0f, 0.32f, -4.12f),
                new Vector3(0.05f, 1.52f, -3.92f),
                new Vector3(0.02f, 0.14f, 0.14f),
                outputDirectory,
                "01_current_library_entry_dust_pool.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(-4.78f, 0.02f, 1.12f),
                LibraryVsCenter + new Vector3(-4.58f, 1.02f, 0.42f),
                new Vector3(0.66f, 1.60f, -3.36f),
                new Vector3(0.10f, 0.30f, 0.10f),
                outputDirectory,
                "02_current_library_left_shelf_dust_lift.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                RetoLibraryDeskLocalPosition + new Vector3(0.00f, 0.02f, -0.18f),
                LibraryVsCenter + new Vector3(2.02f, 0.96f, -0.02f),
                new Vector3(-0.14f, 1.54f, -3.28f),
                new Vector3(0.12f, 0.24f, 0.08f),
                outputDirectory,
                "03_current_library_reto_desk_falloff.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(4.78f, 0.02f, 1.12f),
                LibraryVsCenter + new Vector3(4.58f, 1.02f, 0.42f),
                new Vector3(-0.66f, 1.60f, -3.36f),
                new Vector3(-0.10f, 0.30f, 0.10f),
                outputDirectory,
                "04_past_library_reference_unchanged.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS twenty-first-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dTwentySecondCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS twenty-second-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(-2.10f, 0.02f, 0.62f),
                HouseInteriorCenter + new Vector3(-1.38f, 0.16f, 0.06f),
                new Vector3(0.08f, 0.94f, -2.00f),
                new Vector3(0.05f, 0.06f, 0.08f),
                outputDirectory,
                "01_current_house_bedside_life_props.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(1.18f, 0.02f, -0.94f),
                HouseInteriorCenter + new Vector3(1.10f, 0.64f, -0.86f),
                new Vector3(-0.02f, 1.02f, -2.10f),
                new Vector3(-0.02f, 0.14f, 0.10f),
                outputDirectory,
                "02_current_house_table_life_props.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(-2.08f, 0.02f, 0.72f),
                HouseInteriorCenter + new Vector3(-1.42f, 0.36f, 0.18f),
                new Vector3(0.10f, 1.02f, -2.06f),
                new Vector3(0.06f, 0.10f, 0.10f),
                outputDirectory,
                "03_past_house_bedside_life_props.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorCenter + new Vector3(1.18f, 0.02f, -0.94f),
                HouseInteriorCenter + new Vector3(1.10f, 0.64f, -0.86f),
                new Vector3(-0.02f, 1.02f, -2.10f),
                new Vector3(-0.02f, 0.14f, 0.10f),
                outputDirectory,
                "04_past_house_table_life_props.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS twenty-second-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dTwentyThirdCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS twenty-third-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(4.92f, 0.02f, 0.84f),
                $"{outputDirectory}/01_current_house_exterior_edge_dressing.png");

            CaptureOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(4.92f, 0.02f, 0.84f),
                $"{outputDirectory}/02_past_house_exterior_edge_dressing.png");

            CaptureReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                CentralPlazaVsCenter + new Vector3(0f, 0.02f, -1.10f),
                $"{outputDirectory}/03_current_plaza_edge_dressing.png");

            CaptureOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                CentralPlazaVsCenter + new Vector3(0f, 0.02f, -1.10f),
                $"{outputDirectory}/04_past_plaza_edge_dressing.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS twenty-third-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dTwentyFourthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS twenty-fourth-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(-4.72f, 0.02f, -1.64f),
                LibraryVsCenter + new Vector3(-4.96f, 0.86f, -1.58f),
                new Vector3(5.15f, 1.60f, -4.10f),
                new Vector3(-0.18f, 0.24f, 0.18f),
                outputDirectory,
                "01_current_library_left_window_light.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(4.72f, 0.02f, -1.64f),
                LibraryVsCenter + new Vector3(4.96f, 0.86f, -1.58f),
                new Vector3(-5.15f, 1.60f, -4.10f),
                new Vector3(0.18f, 0.24f, 0.18f),
                outputDirectory,
                "02_current_library_right_window_light.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(-4.72f, 0.02f, -1.64f),
                LibraryVsCenter + new Vector3(-4.96f, 0.86f, -1.58f),
                new Vector3(5.15f, 1.60f, -4.10f),
                new Vector3(-0.18f, 0.24f, 0.18f),
                outputDirectory,
                "03_past_library_left_window_light.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                LibraryVsCenter + new Vector3(4.72f, 0.02f, -1.64f),
                LibraryVsCenter + new Vector3(4.96f, 0.86f, -1.58f),
                new Vector3(-5.15f, 1.60f, -4.10f),
                new Vector3(0.18f, 0.24f, 0.18f),
                outputDirectory,
                "04_past_library_right_window_light.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS twenty-fourth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CaptureHd2dTwentyFifthCycleScreenshotsToDirectory(string outputDirectory)
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(outputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS twenty-fifth-cycle screenshot capture failed: scene review components are missing.");
            }

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Interior,
                HouseInteriorPlayerStart,
                HouseInteriorPlayerStart + new Vector3(0f, 0.10f, 0.18f),
                new Vector3(0.18f, 1.00f, -2.00f),
                new Vector3(0.04f, 0.12f, 0.10f),
                outputDirectory,
                "01_niro_interior_ground_bounce.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Exterior,
                ExteriorDoorExitTarget,
                ExteriorDoorExitTarget + new Vector3(0f, 0.08f, 0.16f),
                new Vector3(0.18f, 1.00f, -1.96f),
                new Vector3(0.04f, 0.12f, 0.08f),
                outputDirectory,
                "02_niro_exterior_ground_bounce.png");

            CaptureCloseReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                RetoLibraryDeskLocalPosition + new Vector3(-1.08f, 0.02f, -1.18f),
                RetoLibraryDeskLocalPosition + new Vector3(0.03f, 0.36f, 0.05f),
                new Vector3(0.22f, 1.00f, -1.98f),
                new Vector3(0.10f, 0.18f, 0.08f),
                outputDirectory,
                "03_reto_library_ground_bounce.png");

            CaptureCloseOtherTimeReviewScreenshot(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.Library,
                PastLibraryPersonCueLocalPosition + new Vector3(-1.06f, 0.02f, -1.16f),
                PastLibraryPersonCueLocalPosition + new Vector3(-0.02f, 0.36f, 0.02f),
                new Vector3(0.22f, 1.00f, -1.96f),
                new Vector3(0.08f, 0.18f, 0.08f),
                outputDirectory,
                "04_aria_past_library_ground_bounce.png");

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS twenty-fifth-cycle screenshots captured: {Path.GetFullPath(outputDirectory)}");
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

        private static void CaptureCloseReviewScreenshot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHouseArea area,
            Vector3 playerLocalPosition,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            string outputDirectory,
            string fileName)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerCurrentLocalForReview(playerLocalPosition);
            guide.ApplyActiveTimeIsolationForReview();
            PositionCloseReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
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

        private static void CaptureCloseOtherTimeReviewScreenshot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHouseArea area,
            Vector3 playerLocalPosition,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            string outputDirectory,
            string fileName)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerOtherTimeLocalForReview(playerLocalPosition);
            guide.ApplyActiveTimeIsolationForReview();
            var previousMask = camera.cullingMask;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            camera.cullingMask = (previousMask & ~currentBit) | otherBit | playerBit;
            PositionCloseReviewCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            camera.cullingMask = previousMask;
            controller.ForcePlayerCurrentLocalForReview(playerLocalPosition);
            guide.ApplyActiveTimeIsolationForReview();
        }

        private static void PositionCloseReviewCamera(Camera camera, Vector3 anchor, Vector3 offset, Vector3 lookOffset)
        {
            var position = anchor + offset;
            var lookAt = anchor + lookOffset;
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
        }

        private static void ValidateCloseReviewOutputExists(string outputDirectory, string fileName)
        {
            var outputPath = Path.Combine(outputDirectory, fileName);
            if (!File.Exists(outputPath))
            {
                throw new InvalidOperationException($"Fast VS close-review screenshot capture failed: missing output file {outputPath}");
            }
        }

        private static void ValidateScreenshotOutputExists(string outputDirectory, string fileName)
        {
            var outputPath = Path.Combine(outputDirectory, fileName);
            if (!File.Exists(outputPath))
            {
                throw new InvalidOperationException($"Fast VS screenshot capture failed: missing output file {outputPath}");
            }
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
                CreateHouseInteriorPropDetailSlab(root, "Past_HouseInterior_PropDetail_BedBlanketFoldA", c + new Vector3(-1.57f, 0.61f, 0.72f), Quaternion.Euler(0f, -6f, 8f), new Vector3(0.46f, 0.05f, 0.18f), bedMaterial, "Past.house_interior.prop_detail.bed_blanket_fold_a");
                CreateHouseInteriorPropDetailSlab(root, "Past_HouseInterior_PropDetail_BedPillowEdge", c + new Vector3(-1.82f, 0.66f, 1.16f), Quaternion.Euler(0f, 8f, -6f), new Vector3(0.30f, 0.04f, 0.10f), materials.Pillow, "Past.house_interior.prop_detail.bed_pillow_edge");
                CreateHouseInteriorPropDetailSlab(root, "Past_HouseInterior_PropDetail_TableLoosePaper", c + new Vector3(1.26f, 0.58f, -0.63f), Quaternion.Euler(0f, -11f, 4f), new Vector3(0.24f, 0.02f, 0.16f), materials.SignPaint, "Past.house_interior.prop_detail.table_loose_paper");
                CreateHouseInteriorPropDetailSlab(root, "Past_HouseInterior_LifeProp_BedsideRug", c + new Vector3(-1.22f, 0.018f, -0.02f), Quaternion.Euler(0f, -2f, 0f), new Vector3(1.12f, 0.025f, 0.42f), bedMaterial, "Past.house_interior.life_prop.bedside_rug");
                CreateLandmarkCube("Past_HouseInterior_LifeProp_TableInkCup", root, c + new Vector3(0.76f, 0.61f, -0.98f), new Vector3(0.10f, 0.10f, 0.10f), Quaternion.identity, materials.PastFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.house_interior.life_prop.table_ink_cup");
                CreateHouseInteriorPropDetailSlab(root, "Past_HouseInterior_LifeProp_TableBrush", c + new Vector3(1.28f, 0.64f, -0.96f), Quaternion.Euler(0f, -18f, 4f), new Vector3(0.34f, 0.018f, 0.045f), materials.PastFence, "Past.house_interior.life_prop.table_brush");
                CreateHouseInteriorPropDetailSlab(root, "Past_HouseInterior_LifeProp_BookPageMarker", c + new Vector3(1.08f, 0.66f, -0.74f), Quaternion.Euler(0f, -10f, 0f), new Vector3(0.16f, 0.010f, 0.045f), materials.SignPaint, "Past.house_interior.life_prop.book_page_marker");
                CreateHouseInteriorPropDetailSlab(root, "Past_HouseInterior_LifeProp_PillowCreaseB", c + new Vector3(-1.92f, 0.68f, 0.76f), Quaternion.Euler(0f, -4f, 0f), new Vector3(0.24f, 0.018f, 0.045f), materials.Pillow, "Past.house_interior.life_prop.pillow_crease_b");
            }
            else
            {
                CreateLandmarkCube("Current_HouseInterior_DustPatch_PixelNoise", root, c + new Vector3(1.55f, 0.015f, 0.42f), new Vector3(1.35f, 0.04f, 0.88f), Quaternion.Euler(0f, -8f, 0f), materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.house_interior.dust");
                CreateReadableBookProp(root, "Current_HouseInterior_TimewriterBookCue", c + new Vector3(1.04f, 0.61f, -0.84f), Quaternion.Euler(0f, -12f, 0f), new Vector3(0.50f, 0.05f, 0.34f), materials.Book, materials.SignPaint, materials.CurrentFence, true, "Current.house_interior.timewriter_book_cue");
                CreateHouseInteriorPropDetailSlab(root, "Current_HouseInterior_PropDetail_BedBlanketFoldA", c + new Vector3(-1.58f, 0.61f, 0.71f), Quaternion.Euler(0f, -7f, 8f), new Vector3(0.48f, 0.05f, 0.18f), bedMaterial, "Current.house_interior.prop_detail.bed_blanket_fold_a");
                CreateHouseInteriorPropDetailSlab(root, "Current_HouseInterior_PropDetail_BedPillowEdge", c + new Vector3(-1.82f, 0.66f, 1.16f), Quaternion.Euler(0f, 8f, -6f), new Vector3(0.30f, 0.04f, 0.10f), materials.Pillow, "Current.house_interior.prop_detail.bed_pillow_edge");
                CreateHouseInteriorPropDetailSlab(root, "Current_HouseInterior_PropDetail_TableLoosePaper", c + new Vector3(1.25f, 0.58f, -0.64f), Quaternion.Euler(0f, -10f, 4f), new Vector3(0.24f, 0.02f, 0.16f), materials.SignPaint, "Current.house_interior.prop_detail.table_loose_paper");
                CreateHouseInteriorPropDetailSlab(root, "Current_HouseInterior_LifeProp_BedsideRug", c + new Vector3(-1.22f, 0.024f, -0.24f), Quaternion.Euler(0f, -4f, 0f), new Vector3(1.12f, 0.025f, 0.50f), materials.CurrentBed, "Current.house_interior.life_prop.bedside_rug");
                CreateLandmarkCube("Current_HouseInterior_LifeProp_TableInkCup", root, c + new Vector3(0.76f, 0.61f, -0.98f), new Vector3(0.10f, 0.10f, 0.10f), Quaternion.identity, materials.DoorwayDark, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.house_interior.life_prop.table_ink_cup");
                CreateHouseInteriorPropDetailSlab(root, "Current_HouseInterior_LifeProp_TableBrush", c + new Vector3(1.28f, 0.64f, -0.96f), Quaternion.Euler(0f, -24f, 5f), new Vector3(0.34f, 0.018f, 0.045f), materials.CurrentFence, "Current.house_interior.life_prop.table_brush");
                CreateHouseInteriorPropDetailSlab(root, "Current_HouseInterior_LifeProp_BookPageMarker", c + new Vector3(1.08f, 0.66f, -0.74f), Quaternion.Euler(0f, -12f, 0f), new Vector3(0.16f, 0.010f, 0.045f), materials.SignPaint, "Current.house_interior.life_prop.book_page_marker");
                CreateHouseInteriorPropDetailSlab(root, "Current_HouseInterior_LifeProp_PillowCreaseB", c + new Vector3(-1.92f, 0.68f, 0.76f), Quaternion.Euler(0f, -5f, 0f), new Vector3(0.24f, 0.018f, 0.045f), materials.Dust, "Current.house_interior.life_prop.pillow_crease_b");
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
            var trim = past ? materials.PastFence : materials.CurrentFence;
            var doorPanelDetail = past ? materials.PastHouseDoorDetail : materials.CurrentHouseDoorDetail;
            var glow = FlatMaterial(
                past ? "past_map_move_floor_glow" : "current_map_move_floor_glow",
                past ? new Color(0.42f, 0.95f, 1.00f, 1f) : new Color(1.00f, 0.56f, 0.20f, 1f),
                true);

            CreateGlowDisc($"{prefix}_HouseInterior_MapMoveGlowPad", interiorRoot, InteriorDoorTriggerCenter + new Vector3(0f, -0.58f, -0.05f), new Vector3(0.68f, 0.018f, 0.45f), glow, true);
            CreateLandmarkCube($"{prefix}_HouseExterior_ReturnDoorHandleCue", exteriorRoot, HouseExteriorCenter + new Vector3(-0.72f, 0.94f, -1.29f), new Vector3(0.10f, 0.10f, 0.08f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.return_door_handle_cue");
            CreateGlowDisc($"{prefix}_HouseExterior_MapMoveGlowPad", exteriorRoot, ExteriorDoorTriggerCenter + new Vector3(0f, -0.58f, 0.02f), new Vector3(0.46f, 0.018f, 0.30f), glow, true);
            CreateGlowDisc($"{prefix}_HouseExterior_DoorEntrySmallGlow", exteriorRoot, ExteriorDoorTriggerCenter + new Vector3(0f, -0.50f, 0.02f), new Vector3(0.30f, 0.015f, 0.20f), glow, true);
            CreateLandmarkCube($"{prefix}_HouseExterior_DoorClosedPanel", exteriorRoot, HouseExteriorCenter + new Vector3(-1.05f, 0.83f, -1.48f), new Vector3(0.74f, 1.38f, 0.07f), Quaternion.identity, doorPanelDetail, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.closed_door_panel");
        }

        private static void CreateHd2dDepthFraming(HouseMapAreas currentAreas, HouseMapAreas pastAreas)
        {
            var depthShadow = EnsureHd2dDepthShadowMaterial();
            var warmPool = EnsureHd2dWarmLightPoolMaterial();
            var currentInteriorRoot = currentAreas.Interior.transform;
            var pastInteriorRoot = pastAreas.Interior.transform;
            var currentExteriorRoot = currentAreas.Exterior.transform;
            var pastExteriorRoot = pastAreas.Exterior.transform;
            var currentPlazaRoot = currentAreas.CentralPlaza.transform;
            var pastPlazaRoot = pastAreas.CentralPlaza.transform;
            var currentLibraryRoot = currentAreas.Library.transform;
            var pastLibraryRoot = pastAreas.Library.transform;

            CreateLandmarkCube(
                "Current_HouseInterior_BackWall_DepthBand",
                currentInteriorRoot,
                HouseInteriorCenter + new Vector3(0f, 0.05f, 2.74f),
                new Vector3(6.80f, 0.05f, 0.04f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.house_interior.back_wall_depth_band");

            CreateLandmarkCube(
                "Past_HouseInterior_BackWall_DepthBand",
                pastInteriorRoot,
                HouseInteriorCenter + new Vector3(0f, 0.05f, 2.74f),
                new Vector3(6.80f, 0.05f, 0.04f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Past.house_interior.back_wall_depth_band");

            CreateLandmarkCube(
                "Current_HouseInterior_Table_WarmLightPool",
                currentInteriorRoot,
                HouseInteriorCenter + new Vector3(1.02f, 0.021f, -0.78f),
                new Vector3(1.24f, 0.04f, 0.88f),
                Quaternion.Euler(0f, -8f, 0f),
                warmPool,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.house_interior.table_warm_pool");

            CreateLandmarkCube(
                "Past_HouseInterior_Table_WarmLightPool",
                pastInteriorRoot,
                HouseInteriorCenter + new Vector3(1.02f, 0.021f, -0.78f),
                new Vector3(1.24f, 0.04f, 0.88f),
                Quaternion.Euler(0f, -8f, 0f),
                warmPool,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Past.house_interior.table_warm_pool");

            CreateLandmarkCube(
                "Current_HouseExterior_Door_DepthPool",
                currentExteriorRoot,
                HouseExteriorCenter + new Vector3(-1.02f, 0.135f, -1.80f),
                new Vector3(1.60f, 0.04f, 0.76f),
                Quaternion.Euler(0f, -5f, 0f),
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.house_exterior.door_depth_pool");

            CreateLandmarkCube(
                "Past_HouseExterior_Door_WarmPool",
                pastExteriorRoot,
                HouseExteriorCenter + new Vector3(-1.02f, 0.135f, -1.76f),
                new Vector3(1.54f, 0.04f, 0.82f),
                Quaternion.Euler(0f, -5f, 0f),
                warmPool,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Past.house_exterior.door_warm_pool");

            CreateLandmarkCube(
                "Current_CentralPlaza_LibraryFacade_DepthUnderEave",
                currentPlazaRoot,
                CentralPlazaVsCenter + new Vector3(0f, 2.71f, 7.96f),
                new Vector3(9.20f, 0.05f, 0.04f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.central_plaza.library_facade.depth_under_eave");

            CreateLandmarkCube(
                "Past_CentralPlaza_LibraryFacade_WindowWarmPool",
                pastPlazaRoot,
                CentralPlazaVsCenter + new Vector3(0f, 0.025f, 7.44f),
                new Vector3(6.60f, 0.04f, 0.56f),
                Quaternion.identity,
                warmPool,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Past.central_plaza.library_facade.window_warm_pool");

            CreateLandmarkCube(
                "Current_Library_BackShelf_DepthBand",
                currentLibraryRoot,
                LibraryVsCenter + new Vector3(0f, 0.61f, 7.18f),
                new Vector3(9.10f, 0.05f, 0.04f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.library.back_shelf_depth_band");

            CreateLandmarkCube(
                "Past_Library_BackShelf_DepthBand",
                pastLibraryRoot,
                LibraryVsCenter + new Vector3(0f, 0.61f, 7.18f),
                new Vector3(9.10f, 0.05f, 0.04f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Past.library.back_shelf_depth_band");

            CreateLandmarkCube(
                "Current_Library_RetoDesk_WarmPool",
                currentLibraryRoot,
                LibraryVsCenter + new Vector3(1.34f, 0.022f, 0.22f),
                new Vector3(1.72f, 0.04f, 0.72f),
                Quaternion.Euler(0f, 4f, 0f),
                warmPool,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.library.reto_desk_warm_pool");

            CreateLandmarkCube(
                "Current_Library_LeftSideShelf_SoftDustLift",
                currentLibraryRoot,
                LibraryVsCenter + new Vector3(-4.58f, 0.045f, 0.35f),
                new Vector3(0.74f, 0.035f, 8.80f),
                Quaternion.identity,
                warmPool,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.library.left_side_shelf.soft_dust_lift");

            CreateLandmarkCube(
                "Current_Library_RightSideShelf_SoftDustLift",
                currentLibraryRoot,
                LibraryVsCenter + new Vector3(4.58f, 0.045f, 0.35f),
                new Vector3(0.74f, 0.035f, 8.80f),
                Quaternion.identity,
                warmPool,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.library.right_side_shelf.soft_dust_lift");

            CreateLandmarkCube(
                "Current_Library_EntryFloor_SoftDustPool",
                currentLibraryRoot,
                LibraryVsCenter + new Vector3(0f, 0.026f, -5.22f),
                new Vector3(3.80f, 0.035f, 1.25f),
                Quaternion.identity,
                warmPool,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.library.entry_floor.soft_dust_pool");

            CreateLandmarkCube(
                "Current_Library_RetoDesk_SideFalloffShadow",
                currentLibraryRoot,
                LibraryVsCenter + new Vector3(2.15f, 0.055f, -0.10f),
                new Vector3(1.30f, 0.035f, 2.15f),
                Quaternion.Euler(0f, 8f, 0f),
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.library.reto_desk.side_falloff_shadow");

            CreateLandmarkCube(
                "Current_Library_SecondFloor_UnderGalleryDepth_Left",
                currentLibraryRoot,
                LibraryVsCenter + new Vector3(-4.05f, 1.94f, 0.10f),
                new Vector3(0.84f, 0.05f, 9.00f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.library.second_floor.under_gallery_depth.left");

            CreateLandmarkCube(
                "Current_Library_SecondFloor_UnderGalleryDepth_Right",
                currentLibraryRoot,
                LibraryVsCenter + new Vector3(4.05f, 1.94f, 0.10f),
                new Vector3(0.84f, 0.05f, 9.00f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Current.library.second_floor.under_gallery_depth.right");

            CreateLandmarkCube(
                "Past_Library_SecondFloor_UnderGalleryDepth_Left",
                pastLibraryRoot,
                LibraryVsCenter + new Vector3(-4.05f, 1.94f, 0.10f),
                new Vector3(0.84f, 0.05f, 9.00f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Past.library.second_floor.under_gallery_depth.left");

            CreateLandmarkCube(
                "Past_Library_SecondFloor_UnderGalleryDepth_Right",
                pastLibraryRoot,
                LibraryVsCenter + new Vector3(4.05f, 1.94f, 0.10f),
                new Vector3(0.84f, 0.05f, 9.00f),
                Quaternion.identity,
                depthShadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                "Past.library.second_floor.under_gallery_depth.right");
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

            CreateHouseExteriorEdgeDressing(root, prefix, past, materials);

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
            CreateCentralPlazaLibraryFacadeDepthDetails(root, prefix, c, past, materials, stone, trim, wall);
            CreateCentralPlazaLibraryFacadeDoor(root, prefix, c + new Vector3(0f, 1.02f, 7.78f), past ? materials.PastFence : materials.CurrentFence, past ? materials.PastLibraryDoorDetail : materials.CurrentLibraryDoorDetail, past ? materials.PastFence : materials.CurrentFence);
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryEntranceStep", root, c + new Vector3(0f, 0.10f, 7.30f), new Vector3(1.70f, 0.12f, 0.78f), Quaternion.identity, stone, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.central_plaza.library_entrance_step");
            CreateCentralPlazaLibraryFacadeWindow(root, prefix, "Left", c + new Vector3(-2.35f, 1.45f, 7.82f), past ? materials.PastFence : materials.CurrentFence, past ? materials.WindowLight : materials.EmptyWindow);
            CreateCentralPlazaLibraryFacadeWindow(root, prefix, "Right", c + new Vector3(2.35f, 1.45f, 7.82f), past ? materials.PastFence : materials.CurrentFence, past ? materials.WindowLight : materials.EmptyWindow);
            CreateCentralPlazaLibraryFacadeCloseDetails(root, prefix, c, past, materials, stone, trim, wall);
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

            if (past)
            {
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_CentralPlaza_GroundDetail_StoneSquareWestEdgePebble", c + new Vector3(-5.40f, 0.03f, 1.28f), new Vector3(0.22f, 0.05f, 0.16f), new Vector3(0.10f, 0.03f, 0.08f), new Vector3(0.26f, 0.02f, 0.18f), -13f, stone, materials.Leaf, materials.Shadow, $"{prefix}.central_plaza.ground_detail.west_edge_pebble");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_CentralPlaza_GroundDetail_FountainSideLeaf", c + new Vector3(1.52f, 0.03f, 3.42f), new Vector3(0.20f, 0.05f, 0.14f), new Vector3(0.09f, 0.03f, 0.08f), new Vector3(0.24f, 0.02f, 0.16f), 17f, materials.Leaf, materials.FlowerYellow, materials.Shadow, $"{prefix}.central_plaza.ground_detail.fountain_side_leaf");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_CentralPlaza_GroundDetail_NoticeBoardShoulder", c + new Vector3(-4.10f, 0.03f, 1.48f), new Vector3(0.24f, 0.04f, 0.16f), new Vector3(0.09f, 0.03f, 0.08f), new Vector3(0.26f, 0.02f, 0.18f), -8f, trim, stone, materials.Shadow, $"{prefix}.central_plaza.ground_detail.notice_board_shoulder");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_CentralPlaza_GroundDetail_LibraryApproachChip", c + new Vector3(3.52f, 0.03f, 4.16f), new Vector3(0.20f, 0.05f, 0.14f), new Vector3(0.10f, 0.03f, 0.08f), new Vector3(0.24f, 0.02f, 0.16f), 9f, stone, materials.PastFence, materials.Shadow, $"{prefix}.central_plaza.ground_detail.library_approach_chip");
            }
            else
            {
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_CentralPlaza_GroundDetail_StoneSquareWestEdgePebble", c + new Vector3(-5.40f, 0.03f, 1.28f), new Vector3(0.22f, 0.05f, 0.16f), new Vector3(0.10f, 0.03f, 0.08f), new Vector3(0.26f, 0.02f, 0.18f), -13f, stone, materials.CurrentLeaf, materials.Shadow, $"{prefix}.central_plaza.ground_detail.west_edge_pebble");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_CentralPlaza_GroundDetail_FountainSideDust", c + new Vector3(1.52f, 0.03f, 3.42f), new Vector3(0.20f, 0.05f, 0.14f), new Vector3(0.09f, 0.03f, 0.08f), new Vector3(0.24f, 0.02f, 0.16f), 17f, materials.Dust, materials.CurrentStone, materials.Shadow, $"{prefix}.central_plaza.ground_detail.fountain_side_dust");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_CentralPlaza_GroundDetail_NoticeBoardShoulder", c + new Vector3(-4.10f, 0.03f, 1.48f), new Vector3(0.24f, 0.04f, 0.16f), new Vector3(0.09f, 0.03f, 0.08f), new Vector3(0.26f, 0.02f, 0.18f), -8f, trim, materials.CurrentStone, materials.Shadow, $"{prefix}.central_plaza.ground_detail.notice_board_shoulder");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_CentralPlaza_GroundDetail_LibraryApproachChip", c + new Vector3(3.52f, 0.03f, 4.16f), new Vector3(0.20f, 0.05f, 0.14f), new Vector3(0.10f, 0.03f, 0.08f), new Vector3(0.24f, 0.02f, 0.16f), 9f, stone, materials.CurrentFence, materials.Shadow, $"{prefix}.central_plaza.ground_detail.library_approach_chip");
            }

            if (past)
            {
                CreateLandmarkCube("Past_CentralPlaza_PropDetail_FountainWaterSparkleA", root, c + new Vector3(-0.42f, 0.51f, 2.10f), new Vector3(0.14f, 0.03f, 0.06f), Quaternion.Euler(0f, -14f, 0f), materials.Water, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.prop_detail.fountain_water_sparkle.a");
                CreateLandmarkCube("Past_CentralPlaza_PropDetail_NoticeBoardPaperA", root, c + new Vector3(-3.10f, 1.41f, 2.10f), new Vector3(0.12f, 0.04f, 0.08f), Quaternion.Euler(0f, -6f, -3f), materials.SignPaint, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.prop_detail.notice_board_paper.a");
                CreateLandmarkCube("Past_CentralPlaza_PropDetail_LibraryApproachPetalA", root, c + new Vector3(3.64f, 0.07f, 4.56f), new Vector3(0.09f, 0.04f, 0.09f), Quaternion.Euler(0f, -18f, 7f), materials.FlowerYellow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.prop_detail.library_approach_petal.a");
            }
            else
            {
                CreateLandmarkCube("Current_CentralPlaza_PropDetail_FountainRimChipA", root, c + new Vector3(0.58f, 0.54f, 2.06f), new Vector3(0.16f, 0.05f, 0.08f), Quaternion.Euler(0f, 12f, 5f), materials.CurrentStone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.prop_detail.fountain_rim_chip.a");
                CreateLandmarkCube("Current_CentralPlaza_PropDetail_NoticeBoardPaperA", root, c + new Vector3(-3.06f, 1.44f, 2.08f), new Vector3(0.12f, 0.04f, 0.08f), Quaternion.Euler(0f, -4f, -3f), materials.SignPaint, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.prop_detail.notice_board_paper.a");
                CreateLandmarkCube("Current_CentralPlaza_PropDetail_LibraryApproachPebbleA", root, c + new Vector3(3.34f, 0.07f, 4.34f), new Vector3(0.10f, 0.04f, 0.08f), Quaternion.Euler(0f, 20f, 10f), materials.CurrentStone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.prop_detail.library_approach_pebble.a");
            }

            CreateCentralPlazaEdgeDressing(root, prefix, past, materials);

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
            else
            {
                CreateCurrentLibrarySideBookshelfSilhouette(root, "Left", c + new Vector3(-4.78f, 0.18f, 0.60f), Quaternion.Euler(0f, 90f, 0f), materials);
                CreateCurrentLibrarySideBookshelfSilhouette(root, "Right", c + new Vector3(4.78f, 0.18f, 0.60f), Quaternion.Euler(0f, -90f, 0f), materials);
            }
            CreateLibraryWindowLightAccents(root, prefix, past, materials);
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
                CreateCharacterContactShadow(
                    "Past_Library_Aria_ContactShadow",
                    root,
                    PastLibraryPersonCueLocalPosition + new Vector3(-0.02f, 0.035f, 0.02f),
                    new Vector3(0.70f, 0.24f, 1f),
                    EnsureAriaContactShadowMaterial());
                CreateCharacterGroundBounce(
                    "Past_Library_Aria_GroundBounce",
                    root,
                    PastLibraryPersonCueLocalPosition + new Vector3(-0.02f, 0.039f, 0.02f),
                    new Vector3(0.60f, 0.20f, 1f),
                    EnsureHd2dCharacterGroundBounceMaterial());
                CreateRedCubeMarkerWithOutline("Past_Library_Aria_RedCubeMarker", root, PastLibraryPersonCueLocalPosition + new Vector3(0f, 1.32f, 0f), PastLibraryTargetBookMarkerScale, Quaternion.Euler(10f, -14f, 0f), materials.RedMarker, materials.DoorwayDark, "Past.library.aria_marker");
            }
            else
            {
                CreateLandmarkCube("Current_Library_Ruin_ScatteredBoardPile", root, c + new Vector3(0.90f, 0.13f, -1.52f), new Vector3(0.86f, 0.08f, 0.22f), Quaternion.Euler(0f, -14f, 7f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.scattered_board_pile");
                CreateLandmarkCube("Current_Library_Ruin_ScatteredBoardPile_PlankA", root, c + new Vector3(0.52f, 0.15f, -1.78f), new Vector3(0.62f, 0.07f, 0.16f), Quaternion.Euler(0f, 22f, -5f), materials.CurrentFurniture, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.scattered_board_pile.plank_a");
                CreateLandmarkCube("Current_Library_Ruin_ScatteredBoardPile_PlankB", root, c + new Vector3(1.32f, 0.14f, -1.30f), new Vector3(0.74f, 0.07f, 0.14f), Quaternion.Euler(0f, -38f, 4f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.scattered_board_pile.plank_b");
                CreateLandmarkCube("Current_Library_Ruin_ScatteredBoardPile_DustPatch", root, c + new Vector3(0.96f, 0.045f, -1.55f), new Vector3(0.72f, 0.035f, 0.40f), Quaternion.Euler(0f, -12f, 0f), materials.Dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.scattered_board_pile.dust_patch");
                CreateLandmarkCube("Current_Library_Ruin_Detail_BookShardA", root, c + new Vector3(3.00f, 0.21f, -2.50f), new Vector3(0.24f, 0.04f, 0.10f), Quaternion.Euler(0f, 18f, -8f), materials.CurrentRubbleDetail, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.detail.book_shard_a");
                CreateLandmarkCube("Current_Library_Ruin_Detail_BookShardB", root, c + new Vector3(3.22f, 0.18f, -2.82f), new Vector3(0.18f, 0.04f, 0.08f), Quaternion.Euler(0f, -32f, 12f), materials.CurrentRubbleDetail, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.detail.book_shard_b");
                CreateLandmarkCube("Current_Library_Ruin_Detail_BrokenPlankA", root, c + new Vector3(0.70f, 0.18f, -1.92f), new Vector3(0.42f, 0.04f, 0.08f), Quaternion.Euler(0f, -24f, 6f), materials.CurrentFence, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.detail.broken_plank_a");
                CreateLandmarkCube("Current_Library_Ruin_Detail_StoneChipA", root, c + new Vector3(1.28f, 0.09f, -1.25f), new Vector3(0.16f, 0.05f, 0.12f), Quaternion.Euler(0f, 14f, -9f), materials.CurrentStone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.library.ruin.detail.stone_chip_a");
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
                CreateCharacterContactShadow(
                    "Current_Library_Reto_ContactShadow",
                    root,
                    RetoLibraryDeskLocalPosition + new Vector3(0.02f, 0.035f, 0.03f),
                    new Vector3(0.66f, 0.24f, 1f),
                    EnsureRetoContactShadowMaterial());
                CreateCharacterGroundBounce(
                    "Current_Library_Reto_GroundBounce",
                    root,
                    RetoLibraryDeskLocalPosition + new Vector3(0.02f, 0.039f, 0.03f),
                    new Vector3(0.58f, 0.19f, 1f),
                    EnsureHd2dCharacterGroundBounceMaterial());
            }

            CreateInvisibleColliderBox($"{prefix}_Library_InvisibleFrontDropGuard", root, c + new Vector3(0f, 0.75f, -7.85f), new Vector3(12.25f, 1.50f, 0.24f), $"{prefix}.library.front_drop_guard");
            CreateInvisibleColliderBox($"{prefix}_Library_InvisibleBackBoundary", root, c + new Vector3(0f, 0.75f, 7.85f), new Vector3(12.25f, 1.50f, 0.24f), $"{prefix}.library.back_boundary");
            CreateInvisibleColliderBox($"{prefix}_Library_InvisibleLeftBoundary", root, c + new Vector3(-5.95f, 0.75f, 0f), new Vector3(0.24f, 1.50f, 15.80f), $"{prefix}.library.left_boundary");
            CreateInvisibleColliderBox($"{prefix}_Library_InvisibleRightBoundary", root, c + new Vector3(5.95f, 0.75f, 0f), new Vector3(0.24f, 1.50f, 15.80f), $"{prefix}.library.right_boundary");

            if (past)
            {
                CreateLibraryPropDetailCluster(
                    root,
                    "Past_Library_PropDetail_LongTableBookPairA",
                    c + new Vector3(-1.10f, 0.405f, -0.95f),
                    Quaternion.Euler(0f, 11f, 0f),
                    new Vector3(0.14f, 0.12f, 0.14f),
                    materials.Book,
                    materials.PastFurniture,
                    materials.PastFence,
                    "Past.library.prop_detail.long_table_book_pair_a");
                CreateLibraryPropDetailCluster(
                    root,
                    "Past_Library_PropDetail_LongTableBookPairB",
                    c + new Vector3(1.08f, 0.405f, -0.95f),
                    Quaternion.Euler(0f, -9f, 0f),
                    new Vector3(0.14f, 0.12f, 0.14f),
                    materials.Book,
                    materials.PastFurniture,
                    materials.PastFence,
                    "Past.library.prop_detail.long_table_book_pair_b");
                CreateLibraryPropDetailCluster(
                    root,
                    "Past_Library_PropDetail_ShelfLedgerWest",
                    c + new Vector3(-4.36f, 0.47f, 0.92f),
                    Quaternion.Euler(0f, 16f, 0f),
                    new Vector3(0.13f, 0.11f, 0.13f),
                    materials.PastFence,
                    materials.Book,
                    materials.PastFurniture,
                    "Past.library.prop_detail.shelf_ledger_west");
            }
            else
            {
                CreateLibraryPropDetailCluster(
                    root,
                    "Current_Library_PropDetail_RetoDeskLoosePapers",
                    CurrentLibraryRetoDeskBookInitialLocalPosition + new Vector3(0.34f, 0.000f, -0.24f),
                    Quaternion.Euler(0f, -14f, 0f),
                    new Vector3(0.13f, 0.11f, 0.13f),
                    materials.Dust,
                    materials.CurrentFurniture,
                    materials.CurrentFence,
                    "Current.library.prop_detail.reto_desk_loose_papers");
                CreateLibraryPropDetailCluster(
                    root,
                    "Current_Library_PropDetail_FloorBookStackWest",
                    c + new Vector3(-3.92f, 0.06f, -3.10f),
                    Quaternion.Euler(0f, 18f, 0f),
                    new Vector3(0.14f, 0.12f, 0.14f),
                    materials.Book,
                    materials.CurrentFurniture,
                    materials.Dust,
                    "Current.library.prop_detail.floor_book_stack_west");
                CreateLibraryPropDetailCluster(
                    root,
                    "Current_Library_PropDetail_ShelfDebrisEast",
                    c + new Vector3(4.42f, 0.30f, 1.88f),
                    Quaternion.Euler(0f, -12f, 0f),
                    new Vector3(0.13f, 0.11f, 0.13f),
                    materials.CurrentFurniture,
                    materials.CurrentRubbleDetail,
                    materials.Dust,
                    "Current.library.prop_detail.shelf_debris_east");
            }
        }

        private static GameObject CreateLibraryPropDetailCluster(Transform root, string objectName, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Material mainMaterial, Material accentMaterial, Material detailMaterial, string landmarkIdBase)
        {
            var slabScale = new Vector3(localScale.x * 2.20f, Mathf.Max(0.018f, localScale.y * 0.24f), localScale.z * 1.34f);
            var accentScale = new Vector3(localScale.x * 1.70f, Mathf.Max(0.014f, localScale.y * 0.18f), localScale.z * 0.96f);
            var lineScale = new Vector3(localScale.x * 1.18f, 0.006f, localScale.z * 0.10f);
            var slipScale = new Vector3(localScale.x * 0.90f, 0.008f, localScale.z * 0.50f);

            var cluster = CreateLandmarkCube(objectName, root, localPosition, slabScale, localRotation, mainMaterial, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, landmarkIdBase);
            CreateLandmarkCube(
                $"{objectName}_Accent",
                root,
                localPosition + localRotation * new Vector3(localScale.x * 0.32f, slabScale.y * 0.78f, -localScale.z * 0.08f),
                accentScale,
                localRotation * Quaternion.Euler(0f, 9f, 0f),
                accentMaterial,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{landmarkIdBase}.accent");
            CreateLandmarkCube(
                $"{objectName}_Detail",
                root,
                localPosition + localRotation * new Vector3(-localScale.x * 0.30f, slabScale.y * 1.22f, -localScale.z * 0.24f),
                lineScale,
                localRotation * Quaternion.Euler(0f, -5f, 0f),
                detailMaterial,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{landmarkIdBase}.detail");
            CreateLandmarkCube(
                $"{objectName}_Slip",
                root,
                localPosition + localRotation * new Vector3(localScale.x * 0.06f, slabScale.y * 1.42f, localScale.z * 0.30f),
                slipScale,
                localRotation * Quaternion.Euler(0f, -14f, 0f),
                detailMaterial,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{landmarkIdBase}.slip");
            return cluster;
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

        private static GameObject CreateCurrentEmptyShelfTexturePanel(string objectName, Transform root, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, string landmarkId)
        {
            var textureScale = new Vector2(Mathf.Max(1f, localScale.x / 1.14f), Mathf.Max(1f, localScale.y / 0.88f));
            return CreateLandmarkCube(objectName, root, localPosition, localScale, localRotation, CurrentEmptyBookshelfFrontMaterial(objectName, textureScale), false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, landmarkId);
        }

        private static void CreateLibraryWindowLightAccents(Transform root, string prefix, bool past, Materials materials)
        {
            _ = materials.WindowLight;
            var windowLight = EnsureHd2dLibraryWindowLightMaterial();
            var shaftScale = past ? new Vector3(1.96f, 1.50f, 1f) : new Vector3(1.80f, 1.38f, 1f);
            var poolScale = past ? new Vector3(1.18f, 1.84f, 1f) : new Vector3(1.10f, 1.72f, 1f);
            var shaftY = 1.12f;
            var poolY = 0.035f;
            var shaftDepth = past ? -1.95f : -1.88f;
            var poolDepth = past ? -1.58f : -1.52f;

            CreateHd2dLibraryWindowLightQuad(
                $"{prefix}_Library_WindowLightShaft_Left",
                root,
                LibraryVsCenter + new Vector3(-5.30f, shaftY, shaftDepth),
                Quaternion.Euler(0f, 90f, -7f),
                shaftScale,
                windowLight,
                $"{prefix}.library.window_light.shaft.left");

            CreateHd2dLibraryWindowLightQuad(
                $"{prefix}_Library_WindowLightShaft_Right",
                root,
                LibraryVsCenter + new Vector3(5.30f, shaftY, shaftDepth),
                Quaternion.Euler(0f, -90f, 7f),
                shaftScale,
                windowLight,
                $"{prefix}.library.window_light.shaft.right");

            CreateHd2dLibraryWindowLightQuad(
                $"{prefix}_Library_WindowLightPool_LeftFloor",
                root,
                LibraryVsCenter + new Vector3(-4.52f, poolY, poolDepth),
                Quaternion.Euler(90f, 0f, 0f),
                poolScale,
                windowLight,
                $"{prefix}.library.window_light.pool.left_floor");

            CreateHd2dLibraryWindowLightQuad(
                $"{prefix}_Library_WindowLightPool_RightFloor",
                root,
                LibraryVsCenter + new Vector3(4.52f, poolY, poolDepth),
                Quaternion.Euler(90f, 0f, 0f),
                poolScale,
                windowLight,
                $"{prefix}.library.window_light.pool.right_floor");
        }

        private static GameObject CreateHd2dLibraryWindowLightQuad(string name, Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Material material, string landmarkId)
        {
            var quad = CreateQuad(name, parent, localPosition, localScale, material);
            quad.transform.localRotation = localRotation;

            var collider = quad.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var landmark = quad.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", landmarkId);
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", true);
            return quad;
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

        private static void CreateCurrentLibrarySideBookshelfSilhouette(Transform root, string side, Vector3 localPosition, Quaternion localRotation, Materials materials)
        {
            CreateCurrentLibraryEmptySideBookshelf(root, side, localPosition, localRotation, materials.CurrentFurniture, materials.Dust, materials.CurrentFence, materials.Shadow, materials.Book);
        }

        private static void CreateCurrentLibraryEmptySideBookshelf(Transform root, string side, Vector3 localPosition, Quaternion localRotation, Material frame, Material dust, Material trim, Material shadow, Material book)
        {
            var shelfRoot = new GameObject($"Current_Library_{side}SideBookshelf");
            shelfRoot.transform.SetParent(root, false);
            shelfRoot.transform.localPosition = localPosition;
            shelfRoot.transform.localRotation = localRotation;

            CreateLibrarySideBookshelfFrame(shelfRoot.transform, "Current", side, frame, false);
            CreateCurrentEmptyShelfTexturePanel(
                $"{shelfRoot.name}_EmptyShelfFrontTexturePanel",
                shelfRoot.transform,
                new Vector3(0f, LibrarySideShelfTexturePanelCenterY, 0.695f),
                new Vector3(LibrarySideShelfRunLength - 0.38f, LibrarySideShelfTexturePanelHeight, 0.035f),
                Quaternion.identity,
                $"Current.library.{side.ToLowerInvariant()}.shelf.empty_front_texture");

            var sideToken = side.ToLowerInvariant();
            const float frontZ = 0.72f;
            CreateLandmarkCube($"{shelfRoot.name}_DustLine_0", shelfRoot.transform, new Vector3(0f, 0.34f, frontZ), new Vector3(LibrarySideShelfRunLength - 0.30f, 0.02f, 0.03f), Quaternion.identity, dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.dust_line.0");
            CreateLandmarkCube($"{shelfRoot.name}_DustLine_1", shelfRoot.transform, new Vector3(0.14f, 0.75f, frontZ), new Vector3(LibrarySideShelfRunLength - 0.68f, 0.02f, 0.03f), Quaternion.identity, dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.dust_line.1");
            CreateLandmarkCube($"{shelfRoot.name}_DustLine_2", shelfRoot.transform, new Vector3(-0.12f, 1.15f, frontZ), new Vector3(LibrarySideShelfRunLength - 0.52f, 0.02f, 0.03f), Quaternion.identity, dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.dust_line.2");
            CreateLandmarkCube($"{shelfRoot.name}_MissingBookGapA", shelfRoot.transform, new Vector3(-1.06f, 0.72f, frontZ + 0.01f), new Vector3(0.18f, 0.48f, 0.02f), Quaternion.identity, shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.missing_gap.a");
            CreateLandmarkCube($"{shelfRoot.name}_MissingBookGapB", shelfRoot.transform, new Vector3(0.76f, 0.72f, frontZ + 0.01f), new Vector3(0.16f, 0.44f, 0.02f), Quaternion.identity, shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.missing_gap.b");
            CreateLandmarkCube($"{shelfRoot.name}_BrokenBoardA", shelfRoot.transform, new Vector3(-1.16f, 0.40f, frontZ + 0.02f), new Vector3(0.88f, 0.05f, 0.10f), Quaternion.Euler(0f, 4f, -14f), trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.broken_board.a");
            CreateLandmarkCube($"{shelfRoot.name}_BrokenBoardB", shelfRoot.transform, new Vector3(1.12f, 0.82f, frontZ + 0.02f), new Vector3(0.72f, 0.05f, 0.10f), Quaternion.Euler(0f, -6f, 13f), frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.broken_board.b");
            CreateLandmarkCube($"{shelfRoot.name}_BrokenBoardC", shelfRoot.transform, new Vector3(-0.36f, 1.25f, frontZ + 0.02f), new Vector3(0.56f, 0.04f, 0.08f), Quaternion.Euler(0f, 11f, -10f), shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.broken_board.c");
            CreateLandmarkCube($"{shelfRoot.name}_ResidualBook_0", shelfRoot.transform, new Vector3(-0.60f, 0.43f, frontZ + 0.04f), new Vector3(0.22f, 0.035f, 0.14f), Quaternion.Euler(0f, 8f, 14f), book, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.residual_book.0");
            CreateLandmarkCube($"{shelfRoot.name}_ResidualBook_1", shelfRoot.transform, new Vector3(0.94f, 0.43f, frontZ + 0.04f), new Vector3(0.20f, 0.033f, 0.13f), Quaternion.Euler(0f, -12f, -9f), book, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.residual_book.1");
            CreateLandmarkCube($"{shelfRoot.name}_PaperSlip_0", shelfRoot.transform, new Vector3(-0.22f, 0.39f, frontZ + 0.05f), new Vector3(0.20f, 0.01f, 0.08f), Quaternion.Euler(0f, 6f, 18f), dust, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.paper_slip.0");
            CreateLandmarkCube($"{shelfRoot.name}_PaperSlip_1", shelfRoot.transform, new Vector3(1.26f, 0.79f, frontZ + 0.05f), new Vector3(0.18f, 0.01f, 0.07f), Quaternion.Euler(0f, -10f, -12f), shadow, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Current.library.{sideToken}.shelf.paper_slip.1");
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
                CreateLandmarkCube($"{objectName}_OpenPageLeft_LineA", book.transform, new Vector3(-localScale.x * 0.12f, 0.048f, -localScale.z * 0.08f), new Vector3(localScale.x * 0.14f, 0.0025f, localScale.z * 0.035f), Quaternion.Euler(0f, 0f, -8f), spine, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.open_page_left.line_a");
                CreateLandmarkCube($"{objectName}_OpenPageLeft_LineB", book.transform, new Vector3(-localScale.x * 0.10f, 0.028f, localScale.z * 0.00f), new Vector3(localScale.x * 0.12f, 0.0022f, localScale.z * 0.030f), Quaternion.Euler(0f, 0f, -5f), pages, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.open_page_left.line_b");
                CreateLandmarkCube($"{objectName}_OpenPageRight_LineA", book.transform, new Vector3(localScale.x * 0.17f, 0.049f, -localScale.z * 0.04f), new Vector3(localScale.x * 0.13f, 0.0025f, localScale.z * 0.034f), Quaternion.Euler(0f, 0f, 7f), spine, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.open_page_right.line_a");
                CreateLandmarkCube($"{objectName}_OpenPageRight_LineB", book.transform, new Vector3(localScale.x * 0.15f, 0.030f, localScale.z * 0.04f), new Vector3(localScale.x * 0.11f, 0.0022f, localScale.z * 0.028f), Quaternion.Euler(0f, 0f, 4f), pages, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkId}.open_page_right.line_b");
            }

            return book;
        }

        private static GameObject CreateHouseInteriorPropDetailSlab(Transform root, string objectName, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Material material, string landmarkId)
        {
            return CreateLandmarkCube(objectName, root, localPosition, localScale, localRotation, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, landmarkId);
        }

        private static GameObject CreateHouseExteriorPropDetailSlab(Transform root, string objectName, Vector3 localPosition, Vector3 localScale, Material material, string landmarkId)
        {
            return CreateLandmarkCube(objectName, root, localPosition, localScale, Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, landmarkId);
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

        private static void CreateLibrarySideBookshelfFrame(Transform shelfRoot, string prefix, string side, Material frame, bool keepCollider)
        {
            var sideToken = side.ToLowerInvariant();
            var halfRun = LibrarySideShelfRunLength * 0.5f;
            var postX = halfRun - 0.07f;
            CreateLandmarkCube($"{shelfRoot.name}_BackPanel", shelfRoot, new Vector3(0f, LibrarySideShelfBackPanelCenterY, 0.08f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfBackPanelHeight, 1.16f), Quaternion.identity, frame, keepCollider, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.back_panel");
            CreateLandmarkCube($"{shelfRoot.name}_LeftPost", shelfRoot, new Vector3(-postX, LibrarySideShelfPostCenterY, 0.02f), new Vector3(LibrarySideShelfPostThickness, LibrarySideShelfPostHeight, LibrarySideShelfPostThickness), Quaternion.identity, frame, keepCollider, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.left_post");
            CreateLandmarkCube($"{shelfRoot.name}_RightPost", shelfRoot, new Vector3(postX, LibrarySideShelfPostCenterY, 0.02f), new Vector3(LibrarySideShelfPostThickness, LibrarySideShelfPostHeight, LibrarySideShelfPostThickness), Quaternion.identity, frame, keepCollider, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.right_post");
            CreateLandmarkCube($"{shelfRoot.name}_TopCap", shelfRoot, new Vector3(0f, LibrarySideShelfTopCapCenterY, 0.02f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfCapThickness, 0.16f), Quaternion.identity, frame, keepCollider, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.top_cap");
            CreateLandmarkCube($"{shelfRoot.name}_BottomBase", shelfRoot, new Vector3(0f, 0.06f, 0.02f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfCapThickness, 0.16f), Quaternion.identity, frame, keepCollider, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.bottom_base");

            for (var row = 0; row < 3; row++)
            {
                var rowY = LibrarySideShelfBoardFirstY + row * LibrarySideShelfBoardStepY;
                CreateLandmarkCube($"{shelfRoot.name}_ShelfBoard_{row}", shelfRoot, new Vector3(0f, rowY, 0.00f), new Vector3(LibrarySideShelfRunLength - 0.10f, LibrarySideShelfBoardThickness, LibrarySideShelfBoardDepth), Quaternion.identity, frame, keepCollider, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.library.{sideToken}.shelf.board.{row}");
            }
        }

        private static void CreatePastLibrarySideBookshelf(Transform root, string side, Vector3 localPosition, Quaternion localRotation, Material frame, Material cover, Material pages, Material spine)
        {
            var shelfRoot = new GameObject($"Past_Library_{side}SideBookshelf");
            shelfRoot.transform.SetParent(root, false);
            shelfRoot.transform.localPosition = localPosition;
            shelfRoot.transform.localRotation = localRotation;

            CreateLibrarySideBookshelfFrame(shelfRoot.transform, "Past", side, frame, true);

            CreateBookRowTexturePanel(
                $"{shelfRoot.name}_BookshelfFrontTexturePanel",
                shelfRoot.transform,
                new Vector3(0f, LibrarySideShelfTexturePanelCenterY, 0.70f),
                new Vector3(LibrarySideShelfRunLength - 0.38f, LibrarySideShelfTexturePanelHeight, 0.040f),
                Quaternion.identity,
                cover,
                $"Past.library.{side.ToLowerInvariant()}.shelf.front_texture");
        }

        private static void CreateCentralPlazaLibraryFacadeDoor(Transform root, string prefix, Vector3 center, Material frame, Material doorPanelDetail, Material handle)
        {
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorFrameTop", root, center + new Vector3(0f, 0.64f, 0f), new Vector3(1.42f, 0.10f, 0.12f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.frame_top");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorFrameLeft", root, center + new Vector3(-0.70f, -0.05f, 0f), new Vector3(0.12f, 1.48f, 0.12f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.frame_left");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorFrameRight", root, center + new Vector3(0.70f, -0.05f, 0f), new Vector3(0.12f, 1.48f, 0.12f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.frame_right");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorPanelsLeft", root, center + new Vector3(-0.24f, -0.05f, 0.01f), new Vector3(0.44f, 1.34f, 0.08f), Quaternion.identity, doorPanelDetail, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.panel_left");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryDoorPanelsRight", root, center + new Vector3(0.24f, -0.05f, 0.01f), new Vector3(0.44f, 1.34f, 0.08f), Quaternion.identity, doorPanelDetail, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_door.panel_right");
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
            CreateExteriorFacadeDepthDetails(root, prefix, c, stone, trim, wood);
        }

        private static void CreateExteriorFacadeDepthDetails(Transform root, string prefix, Vector3 center, Material stone, Material trim, Material wood)
        {
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeDetail_LeftCornerPost", root, center + new Vector3(-3.36f, 1.12f, -1.34f), new Vector3(0.12f, 2.08f, 0.12f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade_detail.left_corner_post");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeDetail_RightCornerPost", root, center + new Vector3(1.26f, 1.12f, -1.34f), new Vector3(0.12f, 2.08f, 0.12f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade_detail.right_corner_post");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeDetail_EaveBraceLeft", root, center + new Vector3(-2.58f, 1.76f, -1.26f), new Vector3(0.12f, 0.54f, 0.10f), Quaternion.Euler(0f, 0f, -13f), wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade_detail.eave_brace_left");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeDetail_EaveBraceRight", root, center + new Vector3(0.50f, 1.76f, -1.26f), new Vector3(0.12f, 0.54f, 0.10f), Quaternion.Euler(0f, 0f, 13f), wood, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade_detail.eave_brace_right");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeDetail_LeftWindowStoneSill", root, center + new Vector3(-2.38f, 0.76f, -1.26f), new Vector3(0.88f, 0.10f, 0.16f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade_detail.left_window_sill");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeDetail_RightWindowStoneSill", root, center + new Vector3(0.22f, 0.76f, -1.26f), new Vector3(0.88f, 0.10f, 0.16f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade_detail.right_window_sill");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeDetail_FoundationJointLeft", root, center + new Vector3(-2.56f, 0.42f, -1.31f), new Vector3(0.10f, 0.24f, 0.10f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade_detail.foundation_joint_left");
            CreateLandmarkCube($"{prefix}_HouseExterior_FacadeDetail_FoundationJointRight", root, center + new Vector3(0.42f, 0.42f, -1.31f), new Vector3(0.10f, 0.24f, 0.10f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.facade_detail.foundation_joint_right");
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

            if (past)
            {
                CreateHouseExteriorPropDetailSlab(root, "Past_HouseExterior_PropDetail_PorchFlowerA", c + new Vector3(-1.98f, 0.16f, -1.60f), new Vector3(0.18f, 0.06f, 0.18f), materials.FlowerYellow, "Past.house_exterior.prop_detail.porch_flower_a");
                CreateHouseExteriorPropDetailSlab(root, "Past_HouseExterior_PropDetail_DoorstepPetalA", c + new Vector3(-1.58f, 0.14f, -2.06f), new Vector3(0.16f, 0.04f, 0.12f), materials.Leaf, "Past.house_exterior.prop_detail.doorstep_petal_a");
                CreateHouseExteriorPropDetailSlab(root, "Past_HouseExterior_PropDetail_NorthEastRoadLeafA", c + new Vector3(4.90f, 0.12f, 0.86f), new Vector3(0.14f, 0.05f, 0.14f), materials.Leaf, "Past.house_exterior.prop_detail.northeast_road_leaf_a");
            }
            else
            {
                CreateHouseExteriorPropDetailSlab(root, "Current_HouseExterior_PropDetail_PorchPebbleA", c + new Vector3(-1.95f, 0.16f, -1.58f), new Vector3(0.18f, 0.06f, 0.16f), materials.CurrentStone, "Current.house_exterior.prop_detail.porch_pebble_a");
                CreateHouseExteriorPropDetailSlab(root, "Current_HouseExterior_PropDetail_DoorstepDustA", c + new Vector3(-1.54f, 0.14f, -2.05f), new Vector3(0.16f, 0.04f, 0.12f), materials.Dust, "Current.house_exterior.prop_detail.doorstep_dust_a");
                CreateHouseExteriorPropDetailSlab(root, "Current_HouseExterior_PropDetail_NorthEastRoadLeafA", c + new Vector3(4.92f, 0.12f, 0.86f), new Vector3(0.14f, 0.05f, 0.14f), materials.CurrentLeaf, "Current.house_exterior.prop_detail.northeast_road_leaf_a");
            }
            if (past)
            {
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_HouseExterior_GroundDetail_FrontYardPebble", c + new Vector3(-2.36f, 0.03f, -2.06f), new Vector3(0.24f, 0.05f, 0.18f), new Vector3(0.11f, 0.03f, 0.08f), new Vector3(0.28f, 0.02f, 0.20f), 12f, materials.PastStone, materials.Leaf, materials.Shadow, $"{prefix}.house_exterior.ground_detail.front_yard_pebble");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_HouseExterior_GroundDetail_NorthEastRoadShoulder", c + new Vector3(4.26f, 0.03f, -0.92f), new Vector3(0.28f, 0.04f, 0.16f), new Vector3(0.10f, 0.03f, 0.10f), new Vector3(0.30f, 0.02f, 0.18f), -18f, materials.PastFence, materials.PastStone, materials.Shadow, $"{prefix}.house_exterior.ground_detail.northeast_road_shoulder");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_HouseExterior_GroundDetail_GardenEdgeLeaf", c + new Vector3(-3.86f, 0.03f, 3.18f), new Vector3(0.22f, 0.05f, 0.18f), new Vector3(0.10f, 0.03f, 0.08f), new Vector3(0.26f, 0.02f, 0.18f), 21f, materials.Leaf, materials.FlowerYellow, materials.Shadow, $"{prefix}.house_exterior.ground_detail.garden_edge_leaf");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_HouseExterior_GroundDetail_SideYardBloom", c + new Vector3(3.52f, 0.03f, -2.02f), new Vector3(0.20f, 0.05f, 0.14f), new Vector3(0.09f, 0.03f, 0.08f), new Vector3(0.24f, 0.02f, 0.16f), -7f, materials.PastStone, materials.FlowerYellow, materials.Shadow, $"{prefix}.house_exterior.ground_detail.side_yard_bloom");
            }
            else
            {
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_HouseExterior_GroundDetail_FrontYardPebble", c + new Vector3(-2.36f, 0.03f, -2.06f), new Vector3(0.24f, 0.05f, 0.18f), new Vector3(0.10f, 0.03f, 0.08f), new Vector3(0.28f, 0.02f, 0.20f), 12f, materials.CurrentStone, materials.CurrentLeaf, materials.Shadow, $"{prefix}.house_exterior.ground_detail.front_yard_pebble");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_HouseExterior_GroundDetail_NorthEastRoadShoulder", c + new Vector3(4.26f, 0.03f, -0.92f), new Vector3(0.28f, 0.04f, 0.16f), new Vector3(0.10f, 0.03f, 0.10f), new Vector3(0.30f, 0.02f, 0.18f), -18f, materials.Dust, materials.CurrentStone, materials.Shadow, $"{prefix}.house_exterior.ground_detail.northeast_road_shoulder");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_HouseExterior_GroundDetail_GardenEdgeLeaf", c + new Vector3(-3.86f, 0.03f, 3.18f), new Vector3(0.22f, 0.05f, 0.18f), new Vector3(0.10f, 0.03f, 0.08f), new Vector3(0.26f, 0.02f, 0.18f), 21f, materials.CurrentLeaf, materials.CurrentFence, materials.Shadow, $"{prefix}.house_exterior.ground_detail.garden_edge_leaf");
                CreateOutdoorGroundDetailCluster(root, $"{prefix}_HouseExterior_GroundDetail_SideYardChip", c + new Vector3(3.52f, 0.03f, -2.02f), new Vector3(0.20f, 0.05f, 0.14f), new Vector3(0.09f, 0.03f, 0.08f), new Vector3(0.24f, 0.02f, 0.16f), -7f, materials.CurrentFence, materials.CurrentStone, materials.Shadow, $"{prefix}.house_exterior.ground_detail.side_yard_chip");
            }
            CreateInvisibleColliderBox(
                $"{prefix}_HouseExterior_InvisibleFrontDropGuard",
                root,
                c + new Vector3(0f, 0.75f, -5.34f),
                new Vector3(13.85f, 1.50f, 0.24f),
                $"{prefix}.house_exterior.front_drop_guard");
        }

        private static void CreateHouseExteriorEdgeDressing(Transform root, string prefix, bool past, Materials materials)
        {
            var c = HouseExteriorCenter;
            var leaf = past ? materials.Leaf : materials.CurrentLeaf;
            var stone = past ? materials.PastStone : materials.CurrentStone;

            CreateLandmarkCube(
                $"{prefix}_HouseExterior_EdgeDressing_NorthHedgeA",
                root,
                c + new Vector3(-2.45f, 0.72f, 5.36f),
                new Vector3(1.42f, 0.58f, 0.38f),
                Quaternion.Euler(0f, -8f, 0f),
                leaf,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{prefix}.house_exterior.edge_dressing.north_hedge_a");

            CreateLandmarkCube(
                $"{prefix}_HouseExterior_EdgeDressing_NorthHedgeB",
                root,
                c + new Vector3(1.18f, 0.84f, 5.44f),
                new Vector3(1.60f, 0.80f, 0.42f),
                Quaternion.Euler(0f, 11f, 0f),
                leaf,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{prefix}.house_exterior.edge_dressing.north_hedge_b");

            CreateLandmarkCube(
                $"{prefix}_HouseExterior_EdgeDressing_WestFenceShadow",
                root,
                c + new Vector3(-4.78f, 0.08f, 1.68f),
                new Vector3(1.36f, 0.10f, 2.34f),
                Quaternion.Euler(0f, -10f, 0f),
                materials.Shadow,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{prefix}.house_exterior.edge_dressing.west_fence_shadow");

            CreateLandmarkCube(
                $"{prefix}_HouseExterior_EdgeDressing_RoadEdgeLowWall",
                root,
                c + new Vector3(4.70f, 0.18f, 4.54f),
                new Vector3(1.86f, 0.30f, 0.26f),
                Quaternion.Euler(0f, -34f, 0f),
                stone,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{prefix}.house_exterior.edge_dressing.road_edge_low_wall");
        }

        private static void CreateWindowTrim(Transform root, string prefix, string side, Vector3 center, Material material)
        {
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowTrim_{side}_Top", root, center + new Vector3(0f, 0.34f, 0f), new Vector3(0.78f, 0.08f, 0.08f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.trim_top");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowTrim_{side}_Bottom", root, center + new Vector3(0f, -0.34f, 0f), new Vector3(0.86f, 0.08f, 0.10f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.trim_bottom");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowTrim_{side}_Left", root, center + new Vector3(-0.39f, 0f, 0f), new Vector3(0.08f, 0.66f, 0.08f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.trim_left");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowTrim_{side}_Right", root, center + new Vector3(0.39f, 0f, 0f), new Vector3(0.08f, 0.66f, 0.08f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.trim_right");
            CreateLandmarkCube($"{prefix}_HouseExterior_WindowShadow_{side}", root, center + new Vector3(0.02f, -0.48f, 0.05f), new Vector3(0.92f, 0.10f, 0.06f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.house_exterior.window.{side}.shadow_lip");
        }

        private static void CreateCentralPlazaLibraryFacadeDepthDetails(Transform root, string prefix, Vector3 center, bool past, Materials materials, Material stone, Material trim, Material wall)
        {
            var roof = past ? materials.PastRoof : materials.CurrentRoof;
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeDetail_LeftPilaster", root, center + new Vector3(-4.18f, 1.48f, 7.74f), new Vector3(0.16f, 2.52f, 0.14f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade_detail.left_pilaster");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeDetail_RightPilaster", root, center + new Vector3(4.18f, 1.48f, 7.74f), new Vector3(0.16f, 2.52f, 0.14f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade_detail.right_pilaster");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand", root, center + new Vector3(0f, 2.62f, 7.70f), new Vector3(8.78f, 0.08f, 0.12f), Quaternion.identity, trim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade_detail.roof_under_thin_band");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeDetail_EntranceCanopyLip", root, center + new Vector3(0f, 1.82f, 7.62f), new Vector3(1.94f, 0.12f, 0.18f), Quaternion.identity, roof, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade_detail.entrance_canopy_lip");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeDetail_LeftWindowStoneSill", root, center + new Vector3(-2.35f, 1.02f, 7.70f), new Vector3(1.00f, 0.08f, 0.16f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade_detail.left_window_sill");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeDetail_RightWindowStoneSill", root, center + new Vector3(2.35f, 1.02f, 7.70f), new Vector3(1.00f, 0.08f, 0.16f), Quaternion.identity, stone, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade_detail.right_window_sill");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeDetail_BaseJointLeft", root, center + new Vector3(-2.62f, 0.28f, 7.68f), new Vector3(0.10f, 0.24f, 0.10f), Quaternion.identity, wall, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade_detail.base_joint_left");
            CreateLandmarkCube($"{prefix}_CentralPlaza_LibraryFacadeDetail_BaseJointRight", root, center + new Vector3(2.62f, 0.28f, 7.68f), new Vector3(0.10f, 0.24f, 0.10f), Quaternion.identity, wall, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.central_plaza.library_facade_detail.base_joint_right");
        }

        private static void CreateCentralPlazaLibraryFacadeCloseDetails(Transform root, string prefix, Vector3 center, bool past, Materials materials, Material stone, Material trim, Material wall)
        {
            var currentDoorKickPlate = past ? materials.PastStone : materials.CurrentFence;
            var currentThreshold = past ? materials.PastStone : materials.CurrentStone;
            var windowTrim = past ? materials.PastFence : materials.Dust;

            if (past)
            {
                CreateLandmarkCube("Past_CentralPlaza_LibraryFacadeCloseDetail_DoorKickPlate", root, center + new Vector3(0f, 0.31f, 7.64f), new Vector3(0.56f, 0.05f, 0.04f), Quaternion.identity, currentDoorKickPlate, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.library_facade_close_detail.door_kick_plate");
                CreateLandmarkCube("Past_CentralPlaza_LibraryFacadeCloseDetail_DoorStudLeft", root, center + new Vector3(-0.36f, 0.66f, 7.62f), new Vector3(0.045f, 0.045f, 0.035f), Quaternion.identity, materials.SignPaint, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.library_facade_close_detail.door_stud_left");
                CreateLandmarkCube("Past_CentralPlaza_LibraryFacadeCloseDetail_DoorStudRight", root, center + new Vector3(0.36f, 0.66f, 7.62f), new Vector3(0.045f, 0.045f, 0.035f), Quaternion.identity, materials.SignPaint, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.library_facade_close_detail.door_stud_right");
                CreateLandmarkCube("Past_CentralPlaza_LibraryFacadeCloseDetail_LeftWindowWarmTrim", root, center + new Vector3(-2.35f, 1.08f, 7.68f), new Vector3(0.80f, 0.04f, 0.05f), Quaternion.identity, windowTrim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.library_facade_close_detail.left_window_warm_trim");
                CreateLandmarkCube("Past_CentralPlaza_LibraryFacadeCloseDetail_RightWindowWarmTrim", root, center + new Vector3(2.35f, 1.08f, 7.68f), new Vector3(0.80f, 0.04f, 0.05f), Quaternion.identity, windowTrim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.library_facade_close_detail.right_window_warm_trim");
                CreateLandmarkCube("Past_CentralPlaza_LibraryFacadeCloseDetail_ThresholdTileA", root, center + new Vector3(0f, 0.18f, 7.60f), new Vector3(0.82f, 0.04f, 0.04f), Quaternion.identity, currentThreshold, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.library_facade_close_detail.threshold_tile_a");
                CreateLandmarkCube("Past_CentralPlaza_LibraryFacadeCloseDetail_ThresholdTileB", root, center + new Vector3(0.72f, 0.18f, 7.60f), new Vector3(0.36f, 0.04f, 0.04f), Quaternion.identity, currentThreshold, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.central_plaza.library_facade_close_detail.threshold_tile_b");
            }
            else
            {
                CreateLandmarkCube("Current_CentralPlaza_LibraryFacadeCloseDetail_DoorKickPlate", root, center + new Vector3(0f, 0.31f, 7.64f), new Vector3(0.56f, 0.05f, 0.04f), Quaternion.identity, currentDoorKickPlate, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.library_facade_close_detail.door_kick_plate");
                CreateLandmarkCube("Current_CentralPlaza_LibraryFacadeCloseDetail_LeftWindowDustLine", root, center + new Vector3(-2.35f, 1.08f, 7.68f), new Vector3(0.80f, 0.04f, 0.05f), Quaternion.identity, windowTrim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.library_facade_close_detail.left_window_dust_line");
                CreateLandmarkCube("Current_CentralPlaza_LibraryFacadeCloseDetail_RightWindowDustLine", root, center + new Vector3(2.35f, 1.08f, 7.68f), new Vector3(0.80f, 0.04f, 0.05f), Quaternion.identity, windowTrim, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.library_facade_close_detail.right_window_dust_line");
                CreateLandmarkCube("Current_CentralPlaza_LibraryFacadeCloseDetail_ThresholdCrackA", root, center + new Vector3(-0.10f, 0.18f, 7.60f), new Vector3(0.62f, 0.035f, 0.035f), Quaternion.Euler(0f, 0f, -4f), materials.DoorwayDark, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.library_facade_close_detail.threshold_crack_a");
                CreateLandmarkCube("Current_CentralPlaza_LibraryFacadeCloseDetail_ThresholdStoneChipA", root, center + new Vector3(0.72f, 0.18f, 7.60f), new Vector3(0.22f, 0.04f, 0.04f), Quaternion.identity, currentThreshold, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.central_plaza.library_facade_close_detail.threshold_stone_chip_a");
            }
        }

        private static void CreateCentralPlazaEdgeDressing(Transform root, string prefix, bool past, Materials materials)
        {
            var c = CentralPlazaVsCenter;
            var stone = past ? materials.PastStone : materials.CurrentStone;
            var leaf = past ? materials.Leaf : materials.CurrentLeaf;

            CreateLandmarkCube(
                $"{prefix}_CentralPlaza_EdgeDressing_WestLowWall",
                root,
                c + new Vector3(-8.18f, 0.24f, 4.82f),
                new Vector3(0.34f, 0.42f, 9.20f),
                Quaternion.identity,
                stone,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{prefix}.central_plaza.edge_dressing.west_low_wall");

            CreateLandmarkCube(
                $"{prefix}_CentralPlaza_EdgeDressing_EastLowWall",
                root,
                c + new Vector3(8.18f, 0.24f, 4.82f),
                new Vector3(0.34f, 0.42f, 9.20f),
                Quaternion.identity,
                stone,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{prefix}.central_plaza.edge_dressing.east_low_wall");

            CreateLandmarkCube(
                $"{prefix}_CentralPlaza_EdgeDressing_NorthTreeLineA",
                root,
                c + new Vector3(-6.42f, 0.86f, 9.46f),
                new Vector3(0.72f, 0.98f, 2.28f),
                Quaternion.Euler(0f, 8f, 0f),
                leaf,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{prefix}.central_plaza.edge_dressing.north_tree_line_a");

            CreateLandmarkCube(
                $"{prefix}_CentralPlaza_EdgeDressing_NorthTreeLineB",
                root,
                c + new Vector3(6.42f, 0.88f, 9.46f),
                new Vector3(0.72f, 1.02f, 2.28f),
                Quaternion.Euler(0f, -11f, 0f),
                leaf,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                $"{prefix}.central_plaza.edge_dressing.north_tree_line_b");
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

        private static void CreateOutdoorGroundDetailCluster(Transform root, string objectName, Vector3 center, Vector3 mainScale, Vector3 accentScale, Vector3 shadowScale, float yaw, Material mainMaterial, Material accentMaterial, Material shadowMaterial, string landmarkIdBase)
        {
            CreateLandmarkCube(objectName, root, center, mainScale, Quaternion.Euler(0f, yaw, 0f), mainMaterial, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkIdBase}.main");
            CreateLandmarkCube($"{objectName}_Accent", root, center + new Vector3(0.06f, 0.01f, -0.03f), accentScale, Quaternion.Euler(0f, yaw + 17f, 0f), accentMaterial, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkIdBase}.accent");
            CreateLandmarkCube($"{objectName}_Shadow", root, center + new Vector3(-0.02f, -0.005f, 0.02f), shadowScale, Quaternion.Euler(0f, yaw - 11f, 0f), shadowMaterial, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{landmarkIdBase}.shadow");
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
            CreateCharacterContactShadow(
                "FastVS_PlayerContactShadow_Niro",
                player.transform,
                new Vector3(0f, 0.022f, -0.02f),
                new Vector3(0.66f, 0.24f, 1f),
                EnsureNiroContactShadowMaterial());
            CreateCharacterGroundBounce(
                "FastVS_PlayerGroundBounce_Niro",
                player.transform,
                new Vector3(0f, 0.026f, -0.02f),
                new Vector3(0.58f, 0.20f, 1f),
                EnsureHd2dCharacterGroundBounceMaterial());
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

        private static void ValidateFastVsHd2dEighteenthCycleLibraryFacadeCloseDetails()
        {
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Current_CentralPlaza_LibraryFacadeCloseDetail_DoorKickPlate", "current_fence", "Current_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Current_CentralPlaza_LibraryFacadeCloseDetail_LeftWindowDustLine", "dust", "Current_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Current_CentralPlaza_LibraryFacadeCloseDetail_RightWindowDustLine", "dust", "Current_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Current_CentralPlaza_LibraryFacadeCloseDetail_ThresholdCrackA", "doorway_dark", "Current_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Past_CentralPlaza_LibraryFacadeCloseDetail_DoorKickPlate", "past_stone", "Past_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Past_CentralPlaza_LibraryFacadeCloseDetail_LeftWindowWarmTrim", "past_fence", "Past_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Past_CentralPlaza_LibraryFacadeCloseDetail_RightWindowWarmTrim", "past_fence", "Past_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Past_CentralPlaza_LibraryFacadeCloseDetail_ThresholdTileA", "past_stone", "Past_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Current_CentralPlaza_LibraryFacadeCloseDetail_ThresholdStoneChipA", "current_stone", "Current_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Past_CentralPlaza_LibraryFacadeCloseDetail_DoorStudLeft", "sign_paint", "Past_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Past_CentralPlaza_LibraryFacadeCloseDetail_DoorStudRight", "sign_paint", "Past_CentralPlazaMap_SeparateSpace");
            ValidateCentralPlazaLibraryFacadeCloseDetailObject("Past_CentralPlaza_LibraryFacadeCloseDetail_ThresholdTileB", "past_stone", "Past_CentralPlazaMap_SeparateSpace");

            if (FindSceneObjectIncludingInactive("Current_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_LibraryNorthFacade") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_LibraryNorthFacade") == null)
            {
                throw new InvalidOperationException("House slice validation failed: the central plaza library facade close-detail pass must keep the library facade and route glow pads present.");
            }

            var currentDoorPanel = FindSceneObjectIncludingInactive("Current_CentralPlaza_LibraryDoorPanelsLeft");
            var currentDoorRenderer = currentDoorPanel != null ? currentDoorPanel.GetComponent<Renderer>() : null;
            if (currentDoorRenderer == null || currentDoorRenderer.sharedMaterial == null || currentDoorRenderer.sharedMaterial.name.IndexOf("doorway_dark", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                throw new InvalidOperationException("House slice validation failed: current central plaza library door must stay readable and must not use the black doorway material.");
            }
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

        private static void ValidateCentralPlazaLibraryFacadeCloseDetailObject(string objectName, string expectedMaterialToken, string expectedParentName)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing central plaza library facade close-detail object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must not have a collider.");
            }

            if (sceneObject.transform.parent == null || sceneObject.transform.parent.name != expectedParentName)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must be parented under {expectedParentName}.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use TimeWindowPairedSpaceLandmarkKind.PropOrFeature.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing {expectedMaterialToken} in its name.");
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

            ValidateLibrarySideBookshelfParent(currentRoot, "Current");
            ValidateLibrarySideBookshelfParent(pastRoot, "Past");
            ValidateVectorNear($"{side} library side bookshelf root position", currentRoot.transform.localPosition, pastRoot.transform.localPosition);
            if (Quaternion.Angle(currentRoot.transform.localRotation, pastRoot.transform.localRotation) > 0.02f)
            {
                throw new InvalidOperationException($"House slice validation failed: current/past {side.ToLowerInvariant()} library side bookshelf roots no longer share the same rotation.");
            }

            ValidateLibrarySideBookshelfFrame(currentRoot.transform, "Current", side, false);
            ValidateLibrarySideBookshelfFrame(pastRoot.transform, "Past", side, true);
            ValidateLibrarySideBookshelfFrameParity(currentRoot.transform, pastRoot.transform, side);

            if (pastRoot.transform.childCount < 9)
            {
                throw new InvalidOperationException($"House slice validation failed: past {side.ToLowerInvariant()} library side bookshelf should keep the frame plus front-facing bookshelf texture panels.");
            }

            if (!HasBookDescendant(pastRoot.transform))
            {
                throw new InvalidOperationException($"House slice validation failed: past {side.ToLowerInvariant()} library side bookshelf must contain book children.");
            }

            if (currentRoot.transform.Find($"{currentRoot.name}_BookshelfFrontTexturePanel") != null)
            {
                throw new InvalidOperationException($"House slice validation failed: current {side.ToLowerInvariant()} library side bookshelf must not use a front texture panel.");
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
        }

        private static void ValidateFastVsHd2dNineteenthCycleCurrentLibrarySideShelves()
        {
            ValidateCurrentLibraryEmptySideShelf("Left");
            ValidateCurrentLibraryEmptySideShelf("Right");
        }

        private static void ValidateFastVsHd2dTwentiethCycleCurrentLibrarySideShelfVisibility()
        {
            ValidateCurrentLibraryEmptyShelfFrontTexturePanel("Left");
            ValidateCurrentLibraryEmptyShelfFrontTexturePanel("Right");
            ValidatePastLibrarySideBookshelfFrontTexturePanel("Left");
            ValidatePastLibrarySideBookshelfFrontTexturePanel("Right");
            ValidateGeneratedRepeatTextureAsset("current_empty_bookshelf_front_hd2d", 256, 128, 24);
        }

        private static void ValidateFastVsHd2dTwentyFirstCycleCurrentLibraryAtmosphere()
        {
            ValidateHd2dDepthFramingObject("Current_Library_LeftSideShelf_SoftDustLift", "hd2d_warm_light_pool", 3005, 3015, "Current_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_Library_RightSideShelf_SoftDustLift", "hd2d_warm_light_pool", 3005, 3015, "Current_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_Library_EntryFloor_SoftDustPool", "hd2d_warm_light_pool", 3005, 3015, "Current_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_Library_RetoDesk_SideFalloffShadow", "hd2d_depth_shadow", 2985, 2995, "Current_LibraryMap_SeparateSpace");

            if (FindSceneObjectIncludingInactive("Past_Library_LeftSideShelf_SoftDustLift") != null ||
                FindSceneObjectIncludingInactive("Past_Library_RightSideShelf_SoftDustLift") != null ||
                FindSceneObjectIncludingInactive("Past_Library_EntryFloor_SoftDustPool") != null ||
                FindSceneObjectIncludingInactive("Past_Library_RetoDesk_SideFalloffShadow") != null)
            {
                throw new InvalidOperationException("House slice validation failed: twenty-first-cycle atmosphere pass must not add past-side counterparts.");
            }
        }

        private static void ValidateCurrentLibraryEmptySideShelf(string side)
        {
            var root = FindSceneObjectIncludingInactive($"Current_Library_{side}SideBookshelf");
            if (root == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing current library side bookshelf root: {side}");
            }

            ValidateLibrarySideBookshelfParent(root, "Current");
            ValidateLibrarySideBookshelfFrame(root.transform, "Current", side, false);

            var required = new[]
            {
                $"{root.name}_BackPanel",
                $"{root.name}_TopCap",
                $"{root.name}_BottomBase",
                $"{root.name}_ShelfBoard_0",
                $"{root.name}_ShelfBoard_1",
                $"{root.name}_ShelfBoard_2",
                $"{root.name}_DustLine_0",
                $"{root.name}_DustLine_1",
                $"{root.name}_DustLine_2",
                $"{root.name}_MissingBookGapA",
                $"{root.name}_MissingBookGapB",
                $"{root.name}_BrokenBoardA",
                $"{root.name}_BrokenBoardB",
                $"{root.name}_BrokenBoardC",
                $"{root.name}_ResidualBook_0",
                $"{root.name}_ResidualBook_1",
                $"{root.name}_PaperSlip_0",
                $"{root.name}_PaperSlip_1"
            };

            foreach (var objectName in required)
            {
                ValidateLibraryShelfDetailObject(objectName, root.name);
            }

            if (FindSceneObjectIncludingInactive($"{root.name}_BookshelfFrontTexturePanel") != null)
            {
                throw new InvalidOperationException($"House slice validation failed: current library side bookshelf must not include a book-filled texture panel: {root.name}");
            }
        }

        private static void ValidateCurrentLibraryEmptyShelfFrontTexturePanel(string side)
        {
            var root = FindSceneObjectIncludingInactive($"Current_Library_{side}SideBookshelf");
            if (root == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing current library side bookshelf root: {side}");
            }

            var panelName = $"{root.name}_EmptyShelfFrontTexturePanel";
            var panel = FindSceneObjectIncludingInactive(panelName);
            if (panel == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing current empty shelf texture panel: {panelName}");
            }

            if (panel.transform.parent == null || panel.transform.parent != root.transform)
            {
                throw new InvalidOperationException($"House slice validation failed: {panelName} must stay parented under {root.name}.");
            }

            if (panel.GetComponent<Collider>() != null || panel.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {panelName} must not have a collider.");
            }

            var renderer = panel.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {panelName} must keep a renderer and material.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf("current_empty_bookshelf_front_hd2d", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {panelName} must use the current_empty_bookshelf_front_hd2d material.");
            }

            ValidateVectorNear($"{side} current empty shelf texture panel position", panel.transform.localPosition, new Vector3(0f, LibrarySideShelfTexturePanelCenterY, 0.695f));
            ValidateVectorNear($"{side} current empty shelf texture panel scale", panel.transform.localScale, new Vector3(LibrarySideShelfRunLength - 0.38f, LibrarySideShelfTexturePanelHeight, 0.035f));
            ValidateSceneObjectMaterialTexture(panelName, "current_empty_bookshelf_front_hd2d");

            if (FindSceneObjectIncludingInactive($"{root.name}_BookshelfFrontTexturePanel") != null)
            {
                throw new InvalidOperationException($"House slice validation failed: current library side bookshelf must not include a book-filled texture panel: {root.name}");
            }
        }

        private static void ValidatePastLibrarySideBookshelfFrontTexturePanel(string side)
        {
            var root = FindSceneObjectIncludingInactive($"Past_Library_{side}SideBookshelf");
            if (root == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing past library side bookshelf root: {side}");
            }

            var panelName = $"{root.name}_BookshelfFrontTexturePanel";
            var panel = FindSceneObjectIncludingInactive(panelName);
            if (panel == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing past bookshelf texture panel: {panelName}");
            }

            if (panel.transform.parent == null || panel.transform.parent != root.transform)
            {
                throw new InvalidOperationException($"House slice validation failed: {panelName} must stay parented under {root.name}.");
            }

            if (panel.GetComponent<Collider>() != null || panel.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {panelName} must not have a collider.");
            }

            var renderer = panel.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {panelName} must keep a renderer and material.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf("bookshelf_front_painted_hd2d", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {panelName} must use the bookshelf_front_painted_hd2d material.");
            }

            ValidateVectorNear($"{side} past bookshelf texture panel position", panel.transform.localPosition, new Vector3(0f, LibrarySideShelfTexturePanelCenterY, 0.70f));
            ValidateVectorNear($"{side} past bookshelf texture panel scale", panel.transform.localScale, new Vector3(LibrarySideShelfRunLength - 0.38f, LibrarySideShelfTexturePanelHeight, 0.040f));
            ValidateSceneObjectMaterialTexture(panelName, "bookshelf_front_painted_hd2d");
        }

        private static void ValidateLibrarySideBookshelfParent(GameObject shelfRoot, string prefix)
        {
            var parent = shelfRoot.transform.parent;
            if (parent == null || parent.name != $"{prefix}_LibraryMap_SeparateSpace")
            {
                throw new InvalidOperationException($"House slice validation failed: {shelfRoot.name} must stay under {prefix.ToLowerInvariant()} library map space.");
            }
        }

        private static void ValidateLibrarySideBookshelfFrame(Transform shelfRoot, string prefix, string side, bool expectCollider)
        {
            var sideToken = side.ToLowerInvariant();
            var halfRun = LibrarySideShelfRunLength * 0.5f;
            var postX = halfRun - 0.07f;
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_BackPanel", new Vector3(0f, LibrarySideShelfBackPanelCenterY, 0.08f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfBackPanelHeight, 1.16f), $"{prefix}.library.{sideToken}.shelf.back_panel", expectCollider);
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_LeftPost", new Vector3(-postX, LibrarySideShelfPostCenterY, 0.02f), new Vector3(LibrarySideShelfPostThickness, LibrarySideShelfPostHeight, LibrarySideShelfPostThickness), $"{prefix}.library.{sideToken}.shelf.left_post", expectCollider);
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_RightPost", new Vector3(postX, LibrarySideShelfPostCenterY, 0.02f), new Vector3(LibrarySideShelfPostThickness, LibrarySideShelfPostHeight, LibrarySideShelfPostThickness), $"{prefix}.library.{sideToken}.shelf.right_post", expectCollider);
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_TopCap", new Vector3(0f, LibrarySideShelfTopCapCenterY, 0.02f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfCapThickness, 0.16f), $"{prefix}.library.{sideToken}.shelf.top_cap", expectCollider);
            ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_BottomBase", new Vector3(0f, 0.06f, 0.02f), new Vector3(LibrarySideShelfRunLength, LibrarySideShelfCapThickness, 0.16f), $"{prefix}.library.{sideToken}.shelf.bottom_base", expectCollider);

            for (var row = 0; row < 3; row++)
            {
                var rowY = LibrarySideShelfBoardFirstY + row * LibrarySideShelfBoardStepY;
                ValidateLibraryShelfChild(shelfRoot, $"{shelfRoot.name}_ShelfBoard_{row}", new Vector3(0f, rowY, 0.00f), new Vector3(LibrarySideShelfRunLength - 0.10f, LibrarySideShelfBoardThickness, LibrarySideShelfBoardDepth), $"{prefix}.library.{sideToken}.shelf.board.{row}", expectCollider);
            }

            var topCap = shelfRoot.Find($"{shelfRoot.name}_TopCap");
            if (topCap == null ||
                shelfRoot.localPosition.y + topCap.localPosition.y + topCap.localScale.y * 0.5f > 2.02f)
            {
                throw new InvalidOperationException($"House slice validation failed: {prefix.ToLowerInvariant()} {side.ToLowerInvariant()} library side bookshelf penetrates the second-floor balcony.");
            }
        }

        private static void ValidateLibraryShelfDetailObject(string objectName, string rootName)
        {
            var detail = FindSceneObjectIncludingInactive(objectName);
            if (detail == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing current library side bookshelf detail: {objectName}");
            }

            if (!objectName.StartsWith(rootName + "_", StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"House slice validation failed: bookshelf detail must remain under the expected shelf root prefix: {objectName}");
            }

            if (detail.GetComponent<Collider>() != null)
            {
                throw new InvalidOperationException($"House slice validation failed: current library side bookshelf detail must not have a collider: {objectName}");
            }

            var renderer = detail.GetComponent<Renderer>() ?? detail.GetComponentInChildren<Renderer>(true);
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: current library side bookshelf detail must keep a renderer and material: {objectName}");
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

        private static void ValidateLibraryShelfChild(Transform shelfRoot, string childName, Vector3 expectedLocalPosition, Vector3 expectedLocalScale, string landmarkId, bool expectCollider)
        {
            var child = shelfRoot.Find(childName);
            if (child == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing library side bookshelf child: {landmarkId}");
            }

            ValidateVectorNear($"{landmarkId} position", child.localPosition, expectedLocalPosition);
            ValidateVectorNear($"{landmarkId} scale", child.localScale, expectedLocalScale);

            var collider = child.GetComponent<Collider>();
            if (expectCollider)
            {
                if (collider == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: library side bookshelf child must keep its collider for the past shelf: {landmarkId}");
                }
            }
            else if (collider != null)
            {
                throw new InvalidOperationException($"House slice validation failed: current library side bookshelf child must not have a collider: {landmarkId}");
            }

            var renderer = child.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: library side bookshelf child must keep a renderer and material: {landmarkId}");
            }
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
                "Current_Library_Ruin_FallenBookSpines",
                "Current_Library_Ruin_Detail_BookShardA",
                "Current_Library_Ruin_Detail_BookShardB",
                "Current_Library_Ruin_Detail_BrokenPlankA",
                "Current_Library_Ruin_Detail_StoneChipA"
            };

            var pastObjects = new[]
            {
                "Past_Library_Ruin_CollapsedShelfPile",
                "Past_Library_Ruin_ScatteredBoardPile",
                "Past_Library_Ruin_ToppledBookStack",
                "Past_Library_Ruin_DustSheetNearEntry",
                "Past_Library_Ruin_BrokenBackShelfBoard",
                "Past_Library_Ruin_FallenBookSpines",
                "Past_Library_Ruin_Detail_BookShardA",
                "Past_Library_Ruin_Detail_BookShardB",
                "Past_Library_Ruin_Detail_BrokenPlankA",
                "Past_Library_Ruin_Detail_StoneChipA"
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

        private static void ValidateFastVsHd2dFourthCycleHeroPropTextures()
        {
            ValidateGeneratedRepeatTextureAsset("current_bed_hd2d_plate", 128, 128, 20);
            ValidateGeneratedRepeatTextureAsset("past_bed_hd2d_plate", 128, 128, 20);
            ValidateGeneratedRepeatTextureAsset("pillow_hd2d_plate", 96, 64, 18);
            ValidateGeneratedRepeatTextureAsset("current_exterior_wall_hd2d_plate", 128, 128, 20);
            ValidateGeneratedRepeatTextureAsset("past_exterior_wall_hd2d_plate", 128, 128, 20);
            ValidateGeneratedRepeatTextureAsset("current_roof_hd2d_plate", 128, 128, 20);
            ValidateGeneratedRepeatTextureAsset("past_roof_hd2d_plate", 128, 128, 20);
            ValidateGeneratedRepeatTextureAsset("window_light_hd2d_plate", 96, 96, 18);
            ValidateGeneratedRepeatTextureAsset("empty_window_hd2d_plate", 96, 96, 18);
            ValidateGeneratedRepeatTextureAsset("current_plank_debris_hd2d_plate", 128, 64, 20);
            ValidateGeneratedRepeatTextureAsset("past_plank_hd2d_plate", 128, 64, 20);

            ValidateGeneratedSurfaceMaterialTexture("current_bed", "current_bed_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_bed", "past_bed_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("pillow", "pillow_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("current_exterior_wall", "current_exterior_wall_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_exterior_wall", "past_exterior_wall_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("current_roof", "current_roof_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_roof", "past_roof_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("window_light", "window_light_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("empty_window", "empty_window_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("current_fence", "current_plank_debris_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_fence", "past_plank_hd2d_plate");

            ValidateSceneObjectMaterialTexture("Current_HouseExterior_WindowLeft", "empty_window_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_HouseExterior_WindowLeft", "window_light_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_HouseExterior_RoofWidePixelPlane", "current_roof_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_HouseExterior_RoofWidePixelPlane", "past_roof_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_NiroBed_PaperPixelBed_Blanket", "current_bed_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_NiroBed_PaperPixelBed_Blanket", "past_bed_hd2d_plate");
        }

        private static void ValidateFastVsHd2dFifthCycleObjectDetails()
        {
            ValidateGeneratedRepeatTextureAsset("current_house_door_detail_hd2d_plate", 96, 160, 18);
            ValidateGeneratedRepeatTextureAsset("past_house_door_detail_hd2d_plate", 96, 160, 18);
            ValidateGeneratedRepeatTextureAsset("current_library_door_detail_hd2d_plate", 96, 160, 18);
            ValidateGeneratedRepeatTextureAsset("past_library_door_detail_hd2d_plate", 96, 160, 18);
            ValidateGeneratedRepeatTextureAsset("current_rubble_detail_hd2d_plate", 128, 64, 18);

            ValidateGeneratedSurfaceMaterialTexture("current_house_door_detail", "current_house_door_detail_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_house_door_detail", "past_house_door_detail_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("current_library_door_detail", "current_library_door_detail_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_library_door_detail", "past_library_door_detail_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("current_rubble_detail", "current_rubble_detail_hd2d_plate");

            ValidateSceneObjectMaterialTexture("Current_HouseExterior_DoorClosedPanel", "current_house_door_detail_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_HouseExterior_DoorClosedPanel", "past_house_door_detail_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_CentralPlaza_LibraryDoorPanelsLeft", "current_library_door_detail_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_CentralPlaza_LibraryDoorPanelsRight", "current_library_door_detail_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_CentralPlaza_LibraryDoorPanelsLeft", "past_library_door_detail_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_CentralPlaza_LibraryDoorPanelsRight", "past_library_door_detail_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_Library_Ruin_Detail_BookShardA", "current_rubble_detail_hd2d_plate");

            ValidateCurrentLibraryRuinObjects();

            var openPageLineDetail = FindSceneObjectIncludingInactive("Current_HouseInterior_TimewriterBookCue_OpenPageLeft_LineA");
            if (openPageLineDetail == null)
            {
                throw new InvalidOperationException("House slice validation failed: open book page line details are missing from the Timewriter cue book.");
            }
        }

        private static void ValidateFastVsHd2dEighthCycleBookPalette()
        {
            ValidateGeneratedRepeatTextureAsset("book_spines_hd2d_plate", 128, 64, 30);
            ValidateGeneratedRepeatTextureAsset("bookshelf_front_painted_hd2d", 256, 128, 30);
            ValidateGeneratedTextureExactSize("book_spines_hd2d_plate", 128, 64);
            ValidateGeneratedTextureExactSize("bookshelf_front_painted_hd2d", 256, 128);
            ValidateSceneObjectMaterialTexture("Past_Library_BackWallBookshelfFrontTexturePanel", "bookshelf_front_painted_hd2d");
            ValidateBookSpineWidthVariation(false);
            ValidateBookSpineWidthVariation(true);
            ValidateBookSpinePaletteSamples(false, 0.75f, 0.84f);
            ValidateBookSpinePaletteSamples(true, 0.70f, 0.78f);
        }

        private static void ValidateFastVsHd2dNinthCyclePathStone()
        {
            ValidateGeneratedRepeatTextureAsset("current_path_hd2d_plate", 128, 128, 30);
            ValidateGeneratedRepeatTextureAsset("past_path_hd2d_plate", 128, 128, 30);
            ValidateGeneratedTextureExactSize("current_path_hd2d_plate", 128, 128);
            ValidateGeneratedTextureExactSize("past_path_hd2d_plate", 128, 128);
            ValidateGeneratedSurfaceMaterialTexture("current_path", "current_path_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_path", "past_path_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_CentralPlaza_StoneSquare", "current_path_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_CentralPlaza_StoneSquare", "past_path_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_HouseExterior_PathToInterior", "current_path_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_HouseExterior_PathToInterior", "past_path_hd2d_plate");
        }

        private static void ValidateFastVsHd2dTenthCycleGrassTexture()
        {
            ValidateGeneratedRepeatTextureAsset("current_grass_hd2d_plate", 128, 128, 30);
            ValidateGeneratedRepeatTextureAsset("past_grass_hd2d_plate", 128, 128, 30);
            ValidateGeneratedTextureExactSize("current_grass_hd2d_plate", 128, 128);
            ValidateGeneratedTextureExactSize("past_grass_hd2d_plate", 128, 128);
            ValidateGeneratedSurfaceMaterialTexture("current_grass", "current_grass_hd2d_plate");
            ValidateGeneratedSurfaceMaterialTexture("past_grass", "past_grass_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_HouseExterior_YardPixelGround", "current_grass_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_HouseExterior_YardPixelGround", "past_grass_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Current_CentralPlaza_PixelGround", "current_grass_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_CentralPlaza_PixelGround", "past_grass_hd2d_plate");
        }

        private static void ValidateFastVsHd2dEleventhCycleOutdoorGroundDetails()
        {
            ValidateOutdoorGroundDetailObject("Current_HouseExterior_GroundDetail_FrontYardPebble", "current_stone", 0.20f);
            ValidateOutdoorGroundDetailObject("Current_HouseExterior_GroundDetail_NorthEastRoadShoulder", "dust", 0.20f);
            ValidateOutdoorGroundDetailObject("Current_HouseExterior_GroundDetail_GardenEdgeLeaf", "current_leaf", 0.20f);
            ValidateOutdoorGroundDetailObject("Current_HouseExterior_GroundDetail_SideYardChip", "current_fence", 0.20f);
            ValidateOutdoorGroundDetailObject("Past_HouseExterior_GroundDetail_FrontYardPebble", "past_stone", 0.20f);
            ValidateOutdoorGroundDetailObject("Past_HouseExterior_GroundDetail_NorthEastRoadShoulder", "past_fence", 0.20f);
            ValidateOutdoorGroundDetailObject("Past_HouseExterior_GroundDetail_GardenEdgeLeaf", "leaf", 0.20f);
            ValidateOutdoorGroundDetailObject("Past_HouseExterior_GroundDetail_SideYardBloom", "past_stone", 0.20f);
            ValidateOutdoorGroundDetailObject("Current_CentralPlaza_GroundDetail_StoneSquareWestEdgePebble", "current_stone", 0.20f);
            ValidateOutdoorGroundDetailObject("Current_CentralPlaza_GroundDetail_FountainSideDust", "dust", 0.20f);
            ValidateOutdoorGroundDetailObject("Current_CentralPlaza_GroundDetail_NoticeBoardShoulder", "current_fence", 0.20f);
            ValidateOutdoorGroundDetailObject("Current_CentralPlaza_GroundDetail_LibraryApproachChip", "current_stone", 0.20f);
            ValidateOutdoorGroundDetailObject("Past_CentralPlaza_GroundDetail_StoneSquareWestEdgePebble", "past_stone", 0.20f);
            ValidateOutdoorGroundDetailObject("Past_CentralPlaza_GroundDetail_FountainSideLeaf", "leaf", 0.20f);
            ValidateOutdoorGroundDetailObject("Past_CentralPlaza_GroundDetail_NoticeBoardShoulder", "past_fence", 0.20f);
            ValidateOutdoorGroundDetailObject("Past_CentralPlaza_GroundDetail_LibraryApproachChip", "past_stone", 0.20f);

            if (FindSceneObjectIncludingInactive("Current_HouseExterior_ToPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_ToPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_ToHouseExterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_ToHouseExterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_ToLibrary_MapMoveGlowPad") == null)
            {
                throw new InvalidOperationException("House slice validation failed: route glow pads must remain present while outdoor ground details are added.");
            }
        }

        private static void ValidateFastVsHd2dTwelfthCycleFacadeDetails()
        {
            ValidateFacadeDetailObject("Current_HouseExterior_FacadeDetail_LeftCornerPost", "current_fence");
            ValidateFacadeDetailObject("Current_HouseExterior_FacadeDetail_EaveBraceLeft", "current_furniture");
            ValidateFacadeDetailObject("Current_HouseExterior_FacadeDetail_LeftWindowStoneSill", "current_stone");
            ValidateFacadeDetailObject("Past_HouseExterior_FacadeDetail_LeftCornerPost", "past_fence");
            ValidateFacadeDetailObject("Past_HouseExterior_FacadeDetail_EaveBraceLeft", "past_furniture");
            ValidateFacadeDetailObject("Past_HouseExterior_FacadeDetail_LeftWindowStoneSill", "past_stone");
            ValidateFacadeDetailObject("Current_CentralPlaza_LibraryFacadeDetail_LeftPilaster", "current_fence");
            ValidateFacadeDetailObject("Current_CentralPlaza_LibraryFacadeDetail_EntranceCanopyLip", "current_roof");
            ValidateFacadeDetailObject("Current_CentralPlaza_LibraryFacadeDetail_LeftWindowStoneSill", "current_stone");
            ValidateFacadeDetailObject("Past_CentralPlaza_LibraryFacadeDetail_LeftPilaster", "past_fence");
            ValidateFacadeDetailObject("Past_CentralPlaza_LibraryFacadeDetail_EntranceCanopyLip", "past_roof");
            ValidateFacadeDetailObject("Past_CentralPlaza_LibraryFacadeDetail_LeftWindowStoneSill", "past_stone");

            ValidateSceneObjectMaterialTexture("Current_CentralPlaza_LibraryDoorPanelsLeft", "current_library_door_detail_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_CentralPlaza_LibraryDoorPanelsLeft", "past_library_door_detail_hd2d_plate");

            if (FindSceneObjectIncludingInactive("Current_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_ToLibrary_MapMoveGlowPad") == null)
            {
                throw new InvalidOperationException("House slice validation failed: facade detail pass must keep library route glow pads present.");
            }
        }

        private static void ValidateFastVsHd2dThirteenthCycleLibraryPropDetails()
        {
            ValidateLibraryPropDetailCluster("Current_Library_PropDetail_RetoDeskLoosePapers");
            ValidateLibraryPropDetailCluster("Current_Library_PropDetail_FloorBookStackWest");
            ValidateLibraryPropDetailCluster("Current_Library_PropDetail_ShelfDebrisEast");
            ValidateLibraryPropDetailCluster("Past_Library_PropDetail_LongTableBookPairA");
            ValidateLibraryPropDetailCluster("Past_Library_PropDetail_LongTableBookPairB");
            ValidateLibraryPropDetailCluster("Past_Library_PropDetail_ShelfLedgerWest");

            var reto = FindSceneObjectIncludingInactive("FastVS_Reto_WritingAtDesk");
            var aria = FindSceneObjectIncludingInactive("Past_Library_AriaIdleAtTable");
            var currentDeskBook = FindSceneObjectIncludingInactive("Current_Library_RetoDeskBook_Initial");
            var returnedDeskBook = FindSceneObjectIncludingInactive("Current_Library_ReturnedBookOnDesk");
            if (reto == null || aria == null || currentDeskBook == null || returnedDeskBook == null)
            {
                throw new InvalidOperationException("House slice validation failed: library prop detail pass must keep the existing Reto, Aria, and desk book setup intact.");
            }
        }

        private static void ValidateFastVsHd2dFourteenthCycleHouseInteriorDetails()
        {
            ValidateHouseInteriorPropDetailObject("Current_HouseInterior_PropDetail_BedBlanketFoldA", "current_bed");
            ValidateHouseInteriorPropDetailObject("Current_HouseInterior_PropDetail_BedPillowEdge", "pillow");
            ValidateHouseInteriorPropDetailObject("Current_HouseInterior_PropDetail_TableLoosePaper", "sign_paint");
            ValidateHouseInteriorPropDetailObject("Past_HouseInterior_PropDetail_BedBlanketFoldA", "past_bed");
            ValidateHouseInteriorPropDetailObject("Past_HouseInterior_PropDetail_BedPillowEdge", "pillow");
            ValidateHouseInteriorPropDetailObject("Past_HouseInterior_PropDetail_TableLoosePaper", "sign_paint");

            if (FindSceneObjectIncludingInactive("Current_NiroBed_PaperPixelBed_Blanket") == null ||
                FindSceneObjectIncludingInactive("Past_NiroBed_PaperPixelBed_Blanket") == null ||
                FindSceneObjectIncludingInactive("Current_HouseInterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseInterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_HouseInterior_TimewriterBookCue") == null)
            {
                throw new InvalidOperationException("House slice validation failed: the bed, glow pad, and current timewriter book cue must remain present while interior details are added.");
            }
        }

        private static void ValidateFastVsHd2dTwentySecondCycleHouseInteriorLifeProps()
        {
            ValidateHouseInteriorLifePropObject("Current_HouseInterior_LifeProp_BedsideRug", "current_bed", "Current_HouseInteriorMap_SeparateSpace", 0.12f);
            ValidateHouseInteriorLifePropObject("Current_HouseInterior_LifeProp_TableInkCup", "doorway_dark", "Current_HouseInteriorMap_SeparateSpace", 0.14f);
            ValidateHouseInteriorLifePropObject("Current_HouseInterior_LifeProp_TableBrush", "current_fence", "Current_HouseInteriorMap_SeparateSpace", 0.12f);
            ValidateHouseInteriorLifePropObject("Current_HouseInterior_LifeProp_BookPageMarker", "sign_paint", "Current_HouseInteriorMap_SeparateSpace", 0.12f);
            ValidateHouseInteriorLifePropObject("Current_HouseInterior_LifeProp_PillowCreaseB", "dust", "Current_HouseInteriorMap_SeparateSpace", 0.12f);

            ValidateHouseInteriorLifePropObject("Past_HouseInterior_LifeProp_BedsideRug", "past_bed", "Past_HouseInteriorMap_SeparateSpace", 0.12f);
            ValidateHouseInteriorLifePropObject("Past_HouseInterior_LifeProp_TableInkCup", "past_fence", "Past_HouseInteriorMap_SeparateSpace", 0.14f);
            ValidateHouseInteriorLifePropObject("Past_HouseInterior_LifeProp_TableBrush", "past_fence", "Past_HouseInteriorMap_SeparateSpace", 0.12f);
            ValidateHouseInteriorLifePropObject("Past_HouseInterior_LifeProp_BookPageMarker", "sign_paint", "Past_HouseInteriorMap_SeparateSpace", 0.12f);
            ValidateHouseInteriorLifePropObject("Past_HouseInterior_LifeProp_PillowCreaseB", "pillow", "Past_HouseInteriorMap_SeparateSpace", 0.12f);

            if (FindSceneObjectIncludingInactive("Current_HouseInterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseInterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_HouseInterior_TimewriterBookCue") == null ||
                FindSceneObjectIncludingInactive("Past_HouseInterior_BookOnTable") == null ||
                FindSceneObjectIncludingInactive("FastVS_Player_NiroHouseSlice") == null)
            {
                throw new InvalidOperationException("House slice validation failed: cycle22 house interior life props must keep the glow pads, book cues, and player root present.");
            }
        }

        private static void ValidateFastVsHd2dTwentyThirdCycleOutdoorEdgeDressing()
        {
            ValidateOutdoorEdgeDressingObject("Current_HouseExterior_EdgeDressing_NorthHedgeA", "current_leaf", "Current_HouseExteriorMap_SeparateSpace", 1.10f);
            ValidateOutdoorEdgeDressingObject("Current_HouseExterior_EdgeDressing_NorthHedgeB", "current_leaf", "Current_HouseExteriorMap_SeparateSpace", 1.10f);
            ValidateOutdoorEdgeDressingObject("Current_HouseExterior_EdgeDressing_WestFenceShadow", "shadow", "Current_HouseExteriorMap_SeparateSpace", 0.16f);
            ValidateOutdoorEdgeDressingObject("Current_HouseExterior_EdgeDressing_RoadEdgeLowWall", "current_stone", "Current_HouseExteriorMap_SeparateSpace", 0.45f);

            ValidateOutdoorEdgeDressingObject("Past_HouseExterior_EdgeDressing_NorthHedgeA", "leaf", "Past_HouseExteriorMap_SeparateSpace", 1.10f);
            ValidateOutdoorEdgeDressingObject("Past_HouseExterior_EdgeDressing_NorthHedgeB", "leaf", "Past_HouseExteriorMap_SeparateSpace", 1.10f);
            ValidateOutdoorEdgeDressingObject("Past_HouseExterior_EdgeDressing_WestFenceShadow", "shadow", "Past_HouseExteriorMap_SeparateSpace", 0.16f);
            ValidateOutdoorEdgeDressingObject("Past_HouseExterior_EdgeDressing_RoadEdgeLowWall", "past_stone", "Past_HouseExteriorMap_SeparateSpace", 0.45f);

            ValidateOutdoorEdgeDressingObject("Current_CentralPlaza_EdgeDressing_WestLowWall", "current_stone", "Current_CentralPlazaMap_SeparateSpace", 0.45f);
            ValidateOutdoorEdgeDressingObject("Current_CentralPlaza_EdgeDressing_EastLowWall", "current_stone", "Current_CentralPlazaMap_SeparateSpace", 0.45f);
            ValidateOutdoorEdgeDressingObject("Current_CentralPlaza_EdgeDressing_NorthTreeLineA", "current_leaf", "Current_CentralPlazaMap_SeparateSpace", 1.10f);
            ValidateOutdoorEdgeDressingObject("Current_CentralPlaza_EdgeDressing_NorthTreeLineB", "current_leaf", "Current_CentralPlazaMap_SeparateSpace", 1.10f);

            ValidateOutdoorEdgeDressingObject("Past_CentralPlaza_EdgeDressing_WestLowWall", "past_stone", "Past_CentralPlazaMap_SeparateSpace", 0.45f);
            ValidateOutdoorEdgeDressingObject("Past_CentralPlaza_EdgeDressing_EastLowWall", "past_stone", "Past_CentralPlazaMap_SeparateSpace", 0.45f);
            ValidateOutdoorEdgeDressingObject("Past_CentralPlaza_EdgeDressing_NorthTreeLineA", "leaf", "Past_CentralPlazaMap_SeparateSpace", 1.10f);
            ValidateOutdoorEdgeDressingObject("Past_CentralPlaza_EdgeDressing_NorthTreeLineB", "leaf", "Past_CentralPlazaMap_SeparateSpace", 1.10f);

            if (FindSceneObjectIncludingInactive("Current_HouseExterior_ToPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_ToPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_ToLibrary_MapMoveGlowPad") == null)
            {
                throw new InvalidOperationException("House slice validation failed: outdoor edge dressing pass must keep the route glow pads present.");
            }
        }

        private static void ValidateFastVsHd2dTwentyFourthCycleLibraryWindowLight()
        {
            ValidateGeneratedTextureExactSize("hd2d_library_window_light_soft", 128, 160);

            ValidateLibraryWindowLightObject("Current_Library_WindowLightShaft_Left", "Current_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(-5.30f, 1.12f, -1.88f), new Vector3(1.80f, 1.38f, 1f), "Current.library.window_light.shaft.left");
            ValidateLibraryWindowLightObject("Current_Library_WindowLightShaft_Right", "Current_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(5.30f, 1.12f, -1.88f), new Vector3(1.80f, 1.38f, 1f), "Current.library.window_light.shaft.right");
            ValidateLibraryWindowLightObject("Current_Library_WindowLightPool_LeftFloor", "Current_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(-4.52f, 0.035f, -1.52f), new Vector3(1.10f, 1.72f, 1f), "Current.library.window_light.pool.left_floor");
            ValidateLibraryWindowLightObject("Current_Library_WindowLightPool_RightFloor", "Current_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(4.52f, 0.035f, -1.52f), new Vector3(1.10f, 1.72f, 1f), "Current.library.window_light.pool.right_floor");

            ValidateLibraryWindowLightObject("Past_Library_WindowLightShaft_Left", "Past_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(-5.30f, 1.12f, -1.95f), new Vector3(1.96f, 1.50f, 1f), "Past.library.window_light.shaft.left");
            ValidateLibraryWindowLightObject("Past_Library_WindowLightShaft_Right", "Past_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(5.30f, 1.12f, -1.95f), new Vector3(1.96f, 1.50f, 1f), "Past.library.window_light.shaft.right");
            ValidateLibraryWindowLightObject("Past_Library_WindowLightPool_LeftFloor", "Past_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(-4.52f, 0.035f, -1.58f), new Vector3(1.18f, 1.84f, 1f), "Past.library.window_light.pool.left_floor");
            ValidateLibraryWindowLightObject("Past_Library_WindowLightPool_RightFloor", "Past_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(4.52f, 0.035f, -1.58f), new Vector3(1.18f, 1.84f, 1f), "Past.library.window_light.pool.right_floor");

            ValidateSceneObjectMaterialTexture("Current_Library_WindowTexture_Left", "empty_window_hd2d_plate");
            ValidateSceneObjectMaterialTexture("Past_Library_WindowTexture_Left", "window_light_hd2d_plate");

            if (FindSceneObjectIncludingInactive("FastVS_Reto_WritingAtDesk") == null ||
                FindSceneObjectIncludingInactive("Past_Library_AriaIdleAtTable") == null)
            {
                throw new InvalidOperationException("House slice validation failed: library window light cycle must keep the existing Reto and Aria setup intact.");
            }
        }

        private static void ValidateFastVsHd2dFifteenthCycleCentralPlazaDetails()
        {
            ValidateOutdoorGroundDetailObject("Current_CentralPlaza_PropDetail_FountainRimChipA", "current_stone", 0.12f);
            ValidateOutdoorGroundDetailObject("Current_CentralPlaza_PropDetail_NoticeBoardPaperA", "sign_paint", 0.12f);
            ValidateOutdoorGroundDetailObject("Current_CentralPlaza_PropDetail_LibraryApproachPebbleA", "current_stone", 0.12f);
            ValidateOutdoorGroundDetailObject("Past_CentralPlaza_PropDetail_FountainWaterSparkleA", "water", 0.12f);
            ValidateOutdoorGroundDetailObject("Past_CentralPlaza_PropDetail_NoticeBoardPaperA", "sign_paint", 0.12f);
            ValidateOutdoorGroundDetailObject("Past_CentralPlaza_PropDetail_LibraryApproachPetalA", "flower_yellow", 0.12f);

            if (FindSceneObjectIncludingInactive("Current_CentralPlaza_FountainNoStepCollider") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_FountainNoStepCollider") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_ToLibrary_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_CentralPlaza_LibraryNorthFacade") == null ||
                FindSceneObjectIncludingInactive("Past_CentralPlaza_LibraryNorthFacade") == null)
            {
                throw new InvalidOperationException("House slice validation failed: central plaza prop detail pass must keep the fountain, route glow pads, and library facade present.");
            }
        }

        private static void ValidateFastVsHd2dSixteenthCycleHouseExteriorDetails()
        {
            ValidateHouseExteriorPropDetailObject("Current_HouseExterior_PropDetail_PorchPebbleA", "current_stone", 0.12f);
            ValidateHouseExteriorPropDetailObject("Current_HouseExterior_PropDetail_DoorstepDustA", "dust", 0.12f);
            ValidateHouseExteriorPropDetailObject("Current_HouseExterior_PropDetail_NorthEastRoadLeafA", "current_leaf", 0.12f);
            ValidateHouseExteriorPropDetailObject("Past_HouseExterior_PropDetail_PorchFlowerA", "flower_yellow", 0.12f);
            ValidateHouseExteriorPropDetailObject("Past_HouseExterior_PropDetail_DoorstepPetalA", "leaf", 0.12f);
            ValidateHouseExteriorPropDetailObject("Past_HouseExterior_PropDetail_NorthEastRoadLeafA", "leaf", 0.12f);

            if (FindSceneObjectIncludingInactive("Current_HouseExterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_DoorEntrySmallGlow") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_DoorEntrySmallGlow") == null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_ToPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_ToPlaza_MapMoveGlowPad") == null ||
                FindSceneObjectIncludingInactive("Current_HouseExterior_DoorClosedPanel") == null ||
                FindSceneObjectIncludingInactive("Past_HouseExterior_DoorClosedPanel") == null)
            {
                throw new InvalidOperationException("House slice validation failed: house exterior detail pass must keep the existing door, glow pad, and route trigger visuals present.");
            }
        }

        private static void ValidateFastVsHd2dSeventeenthCycleCharacterContactShadows()
        {
            ValidateCharacterContactShadowObject("FastVS_PlayerContactShadow_Niro", "FastVS_Player_NiroHouseSlice");
            ValidateCharacterContactShadowObject("Current_Library_Reto_ContactShadow", "Current_LibraryMap_SeparateSpace");
            ValidateCharacterContactShadowObject("Past_Library_Aria_ContactShadow", "Past_LibraryMap_SeparateSpace");

            if (FindSceneObjectIncludingInactive("FastVS_Player_NiroHouseSlice") == null ||
                FindSceneObjectIncludingInactive("FastVS_Reto_WritingAtDesk") == null ||
                FindSceneObjectIncludingInactive("Past_Library_AriaIdleAtTable") == null)
            {
                throw new InvalidOperationException("House slice validation failed: seventeenth-cycle contact-shadow pass must keep Niro, Reto, and Aria present.");
            }
        }

        private static void ValidateFastVsHd2dSeventhCycleDepthFraming()
        {
            ValidateHd2dDepthFramingObject("Current_HouseInterior_BackWall_DepthBand", "hd2d_depth_shadow", 2985, 2995, "Current_HouseInteriorMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Past_HouseInterior_BackWall_DepthBand", "hd2d_depth_shadow", 2985, 2995, "Past_HouseInteriorMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_HouseInterior_Table_WarmLightPool", "hd2d_warm_light_pool", 3005, 3015, "Current_HouseInteriorMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Past_HouseInterior_Table_WarmLightPool", "hd2d_warm_light_pool", 3005, 3015, "Past_HouseInteriorMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_HouseExterior_Door_DepthPool", "hd2d_depth_shadow", 2985, 2995, "Current_HouseExteriorMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Past_HouseExterior_Door_WarmPool", "hd2d_warm_light_pool", 3005, 3015, "Past_HouseExteriorMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_CentralPlaza_LibraryFacade_DepthUnderEave", "hd2d_depth_shadow", 2985, 2995, "Current_CentralPlazaMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Past_CentralPlaza_LibraryFacade_WindowWarmPool", "hd2d_warm_light_pool", 3005, 3015, "Past_CentralPlazaMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_Library_BackShelf_DepthBand", "hd2d_depth_shadow", 2985, 2995, "Current_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Past_Library_BackShelf_DepthBand", "hd2d_depth_shadow", 2985, 2995, "Past_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_Library_RetoDesk_WarmPool", "hd2d_warm_light_pool", 3005, 3015, "Current_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_Library_SecondFloor_UnderGalleryDepth_Left", "hd2d_depth_shadow", 2985, 2995, "Current_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Current_Library_SecondFloor_UnderGalleryDepth_Right", "hd2d_depth_shadow", 2985, 2995, "Current_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Past_Library_SecondFloor_UnderGalleryDepth_Left", "hd2d_depth_shadow", 2985, 2995, "Past_LibraryMap_SeparateSpace");
            ValidateHd2dDepthFramingObject("Past_Library_SecondFloor_UnderGalleryDepth_Right", "hd2d_depth_shadow", 2985, 2995, "Past_LibraryMap_SeparateSpace");
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

        private static void ValidateHd2dCharacterGroundBounceTexture()
        {
            var path = $"{TextureDirectory}/FastVS_House_hd2d_character_ground_bounce_soft.asset";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing ground bounce texture asset {path}.");
            }

            if (texture.width != 96 || texture.height != 48)
            {
                throw new InvalidOperationException($"House slice validation failed: hd2d_character_ground_bounce_soft must be 96x48, but was {texture.width}x{texture.height}.");
            }

            if (texture.filterMode != FilterMode.Bilinear || texture.wrapMode != TextureWrapMode.Clamp)
            {
                throw new InvalidOperationException("House slice validation failed: hd2d_character_ground_bounce_soft must use bilinear clamp sampling.");
            }

            var center = texture.GetPixel(texture.width / 2, texture.height / 2).a;
            var edge = texture.GetPixel(0, texture.height / 2).a;
            var corner = texture.GetPixel(0, 0).a;
            var maxAlpha = 0f;
            foreach (var pixel in texture.GetPixels32())
            {
                maxAlpha = Mathf.Max(maxAlpha, pixel.a / 255f);
            }

            if (center < 0.18f || center > 0.30f)
            {
                throw new InvalidOperationException($"House slice validation failed: hd2d_character_ground_bounce_soft center alpha must stay in the 0.18-0.30 range, but was {center:0.000}.");
            }

            if (edge > center * 0.55f || corner > 0.025f)
            {
                throw new InvalidOperationException($"House slice validation failed: hd2d_character_ground_bounce_soft alpha falloff looks broken. edge={edge:0.000}, corner={corner:0.000}.");
            }

            if (maxAlpha < 0.20f || maxAlpha > 0.30f)
            {
                throw new InvalidOperationException($"House slice validation failed: hd2d_character_ground_bounce_soft max alpha must stay in the 0.20-0.30 range, but was {maxAlpha:0.000}.");
            }
        }

        private static void ValidateSceneObjectMaterialTexture(string objectName, string textureId)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing scene object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            var texture = ResolveMaterialTexture(renderer.sharedMaterial);
            var expectedName = $"FastVS_House_{textureId}";
            if (texture == null || !string.Equals(texture.name, expectedName, StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must reference {expectedName}.");
            }
        }

        private static void ValidateHd2dDepthFramingObject(string objectName, string expectedMaterialToken, int minRenderQueue, int maxRenderQueue, string expectedParentName)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing depth-framing object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must not have a collider.");
            }

            if (sceneObject.transform.parent == null || sceneObject.transform.parent.name != expectedParentName)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must be parented under {expectedParentName}.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use TimeWindowPairedSpaceLandmarkKind.PropOrFeature.");
            }

            if (sceneObject.transform.localScale.y > 0.08f)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay very thin on the Y axis.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing {expectedMaterialToken} in its name.");
            }

            var renderQueue = renderer.sharedMaterial.renderQueue;
            if (renderQueue < minRenderQueue || renderQueue > maxRenderQueue)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep renderQueue in the {minRenderQueue}-{maxRenderQueue} range, but was {renderQueue}.");
            }
        }

        private static void ValidateLibraryWindowLightObject(string objectName, string expectedParentName, Vector3 expectedLocalPosition, Vector3 expectedLocalScale, string expectedLandmarkId)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing library window light object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must not have a collider.");
            }

            if (sceneObject.transform.parent == null || sceneObject.transform.parent.name != expectedParentName)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must be parented under {expectedParentName}.");
            }

            ValidateVectorNear($"{objectName} local position", sceneObject.transform.localPosition, expectedLocalPosition);
            ValidateVectorNear($"{objectName} local scale", sceneObject.transform.localScale, expectedLocalScale);
            if (sceneObject.transform.localScale.x <= 0f ||
                sceneObject.transform.localScale.y <= 0f ||
                Mathf.Abs(sceneObject.transform.localScale.z - 1f) > 0.01f)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay flat and non-degenerate.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            var arrivalProperty = landmarkSerialized.FindProperty("countsForArrival");
            var landmarkIdProperty = landmarkSerialized.FindProperty("landmarkId");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature) ||
                arrivalProperty == null ||
                arrivalProperty.propertyType != SerializedPropertyType.Boolean ||
                !arrivalProperty.boolValue ||
                landmarkIdProperty == null ||
                landmarkIdProperty.propertyType != SerializedPropertyType.String ||
                !string.Equals(landmarkIdProperty.stringValue, expectedLandmarkId, StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep its paired-space landmark metadata.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf("hd2d_library_window_light", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use the hd2d_library_window_light material.");
            }

            ValidateSceneObjectMaterialTexture(objectName, "hd2d_library_window_light_soft");
        }

        private static void ValidateCharacterContactShadowObject(string objectName, string expectedParentName)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing contact-shadow object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must remain non-colliding.");
            }

            if (sceneObject.transform.parent == null || sceneObject.transform.parent.name != expectedParentName)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay parented under {expectedParentName}.");
            }

            if (Quaternion.Angle(sceneObject.transform.localRotation, Quaternion.Euler(90f, 0f, 0f)) > 1.5f)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay horizontal with a local X rotation near 90 degrees.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf("contact_shadow", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing contact_shadow in its name.");
            }
        }

        private static void ValidateFastVsHd2dTwentyFifthCycleCharacterGroundBounce()
        {
            ValidateHd2dCharacterGroundBounceTexture();

            if (FindSceneObjectIncludingInactive("FastVS_PlayerContactShadow_Niro") == null ||
                FindSceneObjectIncludingInactive("Current_Library_Reto_ContactShadow") == null ||
                FindSceneObjectIncludingInactive("Past_Library_Aria_ContactShadow") == null)
            {
                throw new InvalidOperationException("House slice validation failed: the existing character contact shadows must stay present.");
            }

            ValidateCharacterGroundBounceObject(
                "FastVS_PlayerGroundBounce_Niro",
                "FastVS_Player_NiroHouseSlice",
                new Vector3(0f, 0.026f, -0.02f),
                new Vector3(0.58f, 0.20f, 1f));

            ValidateCharacterGroundBounceObject(
                "Current_Library_Reto_GroundBounce",
                "Current_LibraryMap_SeparateSpace",
                RetoLibraryDeskLocalPosition + new Vector3(0.02f, 0.039f, 0.03f),
                new Vector3(0.58f, 0.19f, 1f));

            ValidateCharacterGroundBounceObject(
                "Past_Library_Aria_GroundBounce",
                "Past_LibraryMap_SeparateSpace",
                PastLibraryPersonCueLocalPosition + new Vector3(-0.02f, 0.039f, 0.02f),
                new Vector3(0.60f, 0.20f, 1f));

            if (FindSceneObjectIncludingInactive("FastVS_PlayerVisual_NiroShadingOverlay") != null ||
                FindSceneObjectIncludingInactive("FastVS_PlayerSpriteShadingOverlay_Niro") != null)
            {
                throw new InvalidOperationException("House slice validation failed: the old Niro full-body shading overlay must not exist.");
            }
        }

        private static void ValidateCharacterGroundBounceObject(string objectName, string expectedParentName, Vector3 expectedLocalPosition, Vector3 expectedLocalScale)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing ground-bounce object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null || !renderer.enabled)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have an enabled renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must remain non-colliding.");
            }

            if (sceneObject.transform.parent == null || sceneObject.transform.parent.name != expectedParentName)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay parented under {expectedParentName}.");
            }

            if (Quaternion.Angle(sceneObject.transform.localRotation, Quaternion.Euler(90f, 0f, 0f)) > 1.5f)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay horizontal with a local X rotation near 90 degrees.");
            }

            ValidateVectorNear($"{objectName} local position", sceneObject.transform.localPosition, expectedLocalPosition);
            ValidateVectorNear($"{objectName} local scale", sceneObject.transform.localScale, expectedLocalScale);

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            var texture = ResolveMaterialTexture(renderer.sharedMaterial);
            var textureName = texture?.name ?? string.Empty;
            if (materialName.IndexOf("hd2d_character_ground_bounce", StringComparison.OrdinalIgnoreCase) < 0 &&
                textureName.IndexOf("hd2d_character_ground_bounce", StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a ground-bounce material or texture name.");
            }
        }

        private static void ValidateFacadeDetailObject(string objectName, string expectedMaterialToken)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing facade detail object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must remain non-colliding facade decoration.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use TimeWindowPairedSpaceLandmarkKind.PropOrFeature.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing {expectedMaterialToken} in its name.");
            }
        }

        private static void ValidateLibraryPropDetailCluster(string objectName)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing library prop detail object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must remain non-colliding small prop detail.");
            }

            if (sceneObject.transform.localScale.y > 0.16f)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay small on the Y axis.");
            }
        }

        private static void ValidateHouseInteriorPropDetailObject(string objectName, string expectedMaterialToken)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing house interior prop detail object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must remain non-colliding interior decoration.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use TimeWindowPairedSpaceLandmarkKind.PropOrFeature.");
            }

            if (sceneObject.transform.localScale.y > 0.10f)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay very thin on the Y axis.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing {expectedMaterialToken} in its name.");
            }
        }

        private static void ValidateHouseInteriorLifePropObject(string objectName, string expectedMaterialToken, string expectedParentName, float maxScaleY)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing house interior life prop object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must remain non-colliding interior decoration.");
            }

            if (sceneObject.transform.parent == null || sceneObject.transform.parent.name != expectedParentName)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must be parented under {expectedParentName}.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use TimeWindowPairedSpaceLandmarkKind.PropOrFeature.");
            }

            if (sceneObject.transform.localScale.y > maxScaleY)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay thin on the Y axis.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing {expectedMaterialToken} in its name.");
            }
        }

        private static void ValidateHouseExteriorPropDetailObject(string objectName, string expectedMaterialToken, float maxScaleY)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing house exterior prop detail object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must remain non-colliding exterior decoration.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use TimeWindowPairedSpaceLandmarkKind.PropOrFeature.");
            }

            if (sceneObject.transform.localScale.y > maxScaleY)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay very low to the ground.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing {expectedMaterialToken} in its name.");
            }
        }

        private static void ValidateOutdoorGroundDetailObject(string objectName, string expectedMaterialToken, float maxScaleY)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing outdoor ground detail object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must not have a collider.");
            }

            if (sceneObject.transform.localScale.y > maxScaleY)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay very low to the ground.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use TimeWindowPairedSpaceLandmarkKind.PropOrFeature.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing {expectedMaterialToken} in its name.");
            }
        }

        private static void ValidateOutdoorEdgeDressingObject(string objectName, string expectedMaterialToken, string expectedParentName, float maxScaleY)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing outdoor edge dressing object {objectName}.");
            }

            var renderer = sceneObject.GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must have a renderer with a material.");
            }

            if (sceneObject.GetComponent<Collider>() != null || sceneObject.GetComponentsInChildren<Collider>(true).Length > 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must not have a collider.");
            }

            if (sceneObject.transform.parent == null || sceneObject.transform.parent.name != expectedParentName)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must be parented under {expectedParentName}.");
            }

            var landmark = sceneObject.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark == null)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must keep a TimeWindowPairedSpaceLandmark.");
            }

            var landmarkSerialized = new SerializedObject(landmark);
            var kindProperty = landmarkSerialized.FindProperty("kind");
            if (kindProperty == null ||
                kindProperty.propertyType != SerializedPropertyType.Enum ||
                kindProperty.enumValueIndex != Convert.ToInt32(TimeWindowPairedSpaceLandmarkKind.PropOrFeature))
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use TimeWindowPairedSpaceLandmarkKind.PropOrFeature.");
            }

            if (sceneObject.transform.localScale.y > maxScaleY)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must stay thin on the Y axis.");
            }

            var materialName = renderer.sharedMaterial.name ?? string.Empty;
            if (materialName.IndexOf(expectedMaterialToken, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException($"House slice validation failed: {objectName} must use a material containing {expectedMaterialToken} in its name.");
            }
        }

        private static void ValidateGeneratedTextureExactSize(string textureId, int expectedWidth, int expectedHeight)
        {
            var path = $"{TextureDirectory}/FastVS_House_{textureId}.asset";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: missing generated texture asset {path}.");
            }

            if (texture.width != expectedWidth || texture.height != expectedHeight)
            {
                throw new InvalidOperationException($"House slice validation failed: {textureId} must be exactly {expectedWidth}x{expectedHeight}, but was {texture.width}x{texture.height}.");
            }
        }

        private static void ValidateBookSpinePaletteSamples(bool bookshelfFront, float maxSaturation, float maxValue)
        {
            var label = bookshelfFront ? "bookshelf front" : "book spine";
            var samples = bookshelfFront
                ? new (int columnIndex, int rowIndex, int seed)[]
                {
                    (0, 0, 131),
                    (2, 1, 131),
                    (4, 0, 131),
                    (7, 2, 131),
                    (10, 1, 131),
                    (13, 2, 131)
                }
                : new (int columnIndex, int rowIndex, int seed)[]
                {
                    (0, 0, 109),
                    (2, 1, 109),
                    (4, 0, 109),
                    (7, 1, 109),
                    (10, 0, 109),
                    (13, 2, 109)
                };

            foreach (var sample in samples)
            {
                var color = PickBookSpineColor(sample.columnIndex, sample.rowIndex, sample.seed, bookshelfFront);
                Color.RGBToHSV(color, out _, out var saturation, out var value);
                if (saturation > maxSaturation || value > maxValue)
                {
                    throw new InvalidOperationException($"House slice validation failed: {label} palette sample {sample.columnIndex}/{sample.rowIndex} is too vivid. saturation={saturation:0.000}, value={value:0.000}");
                }
            }
        }

        private static void ValidateBookSpineWidthVariation(bool bookshelfFront)
        {
            var widths = new HashSet<int>();
            for (var index = 0; index < 16; index++)
            {
                var width = GetBookSpineWidth(index, index % 3, bookshelfFront ? 131 : 109, bookshelfFront);
                if (width < 5 || width > 14)
                {
                    throw new InvalidOperationException($"House slice validation failed: {(bookshelfFront ? "bookshelf front" : "book spine")} width {width} is outside the expected 5-14 px range.");
                }

                widths.Add(width);
            }

            if (widths.Count < 4)
            {
                throw new InvalidOperationException($"House slice validation failed: {(bookshelfFront ? "bookshelf front" : "book spine")} widths do not vary enough to avoid barcode-like repetition.");
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

        private static GameObject CreateCharacterContactShadow(string name, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var shadow = CreateQuad(name, parent, localPosition, localScale, material);
            shadow.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            return shadow;
        }

        private static GameObject CreateCharacterGroundBounce(string name, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var bounce = CreateQuad(name, parent, localPosition, localScale, material);
            bounce.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            return bounce;
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
            return EnsureCharacterContactShadowMaterial("niro_contact_shadow");
        }

        private static Material EnsureRetoContactShadowMaterial()
        {
            return EnsureCharacterContactShadowMaterial("reto_contact_shadow");
        }

        private static Material EnsureAriaContactShadowMaterial()
        {
            return EnsureCharacterContactShadowMaterial("aria_contact_shadow");
        }

        private static Material EnsureCharacterContactShadowMaterial(string materialId)
        {
            var material = FlatMaterial(materialId, Color.white, true);
            ConfigureTransparentUnlitMaterial(material, 2995);
            var texture = EnsureCharacterContactShadowTexture();
            AssignMaterialTexture(material, texture, Vector2.one);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(1f, 1f, 1f, 0.96f));
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", new Color(1f, 1f, 1f, 0.96f));
            }

            return material;
        }

        private static Material EnsureHd2dCharacterGroundBounceMaterial()
        {
            var material = FlatMaterial("hd2d_character_ground_bounce", Color.white, true);
            ConfigureTransparentUnlitMaterial(material, 3014);
            var texture = EnsureHd2dCharacterGroundBounceTexture();
            AssignMaterialTexture(material, texture, Vector2.one);

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(1f, 0.86f, 0.58f, 1f));
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", new Color(1f, 0.86f, 0.58f, 1f));
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureHd2dDepthShadowMaterial()
        {
            var material = FlatMaterial("hd2d_depth_shadow", new Color(0.04f, 0.035f, 0.035f, 0.12f), true);
            ConfigureTransparentUnlitMaterial(material, 2990);
            return material;
        }

        private static Material EnsureHd2dWarmLightPoolMaterial()
        {
            var material = FlatMaterial("hd2d_warm_light_pool", new Color(1.0f, 0.72f, 0.30f, 0.18f), true);
            ConfigureTransparentUnlitMaterial(material, 3009);
            return material;
        }

        private static Material EnsureHd2dLibraryWindowLightMaterial()
        {
            var material = FlatMaterial("hd2d_library_window_light", Color.white, true);
            ConfigureTransparentUnlitMaterial(material, 3012);
            var texture = EnsureHd2dLibraryWindowLightTexture();
            AssignMaterialTexture(material, texture, Vector2.one);

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(1f, 1f, 1f, 1f));
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
            }

            EditorUtility.SetDirty(material);
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

        private static Texture2D EnsureHd2dLibraryWindowLightTexture()
        {
            return EnsureGeneratedTexture(
                "hd2d_library_window_light_soft",
                128,
                160,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = x / 127f;
                    var v = y / 159f;
                    var edge = Mathf.SmoothStep(0f, 0.22f, u) * Mathf.SmoothStep(0f, 0.22f, 1f - u);
                    var falloff = Mathf.Lerp(0.78f, 0.16f, v);
                    var diagonalBand = Mathf.Clamp01(1f - Mathf.Abs(((u * 1.06f) + (v * 0.84f)) - 0.95f));
                    var wobble = Mathf.Clamp01(1f - Mathf.Abs(Mathf.Sin((u * 8.1f) + (v * 11.3f) + 0.37f)) * 0.58f);
                    var dither = ((((x * 17) ^ (y * 31) ^ (x * y * 7)) & 15) / 15f);
                    var alpha = ((edge * 0.78f) + (diagonalBand * 0.22f)) * falloff * 0.26f;
                    alpha *= Mathf.Lerp(0.86f, 1f, wobble);
                    alpha *= Mathf.Lerp(0.84f, 1f, dither);
                    alpha = Mathf.Clamp(alpha, 0f, 0.26f);
                    return new Color(1f, 0.78f, 0.34f, alpha);
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
            return EnsureCharacterContactShadowTexture();
        }

        private static Texture2D EnsureCharacterContactShadowTexture()
        {
            return EnsureGeneratedTexture(
                "character_contact_shadow",
                96,
                48,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = x / 95f;
                    var v = y / 47f;
                    var dx = (u - 0.5f) / 0.5f;
                    var dy = (v - 0.5f) / 0.5f;
                    var ellipse = Mathf.Sqrt((dx * dx * 0.88f) + (dy * dy * 2.55f));
                    var core = Mathf.Clamp01(1f - ellipse);
                    var alpha = core * core * 0.34f;
                    if (core < 0.45f)
                    {
                        var dither = (((x * 17) + (y * 31) + (x * y * 7)) & 7) / 7f;
                        alpha *= Mathf.Lerp(0.72f, 1f, dither);
                    }

                    return new Color(0.02f, 0.03f, 0.05f, alpha);
                });
        }

        private static Texture2D EnsureHd2dCharacterGroundBounceTexture()
        {
            return EnsureGeneratedTexture(
                "hd2d_character_ground_bounce_soft",
                96,
                48,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = x / 95f;
                    var v = y / 47f;
                    var dx = (u - 0.50f) / 0.50f;
                    var dy = (v - 0.56f) / 0.44f;
                    var ellipse = Mathf.Sqrt((dx * dx * 0.86f) + (dy * dy * 2.70f));
                    var core = Mathf.Clamp01(1f - ellipse);
                    var glow = Mathf.Clamp01(1f - ellipse * 0.78f);
                    var baseAlpha = (core * core * 0.30f) + (glow * 0.04f);
                    var dither = ((((x * 19) ^ (y * 11) ^ (x * y * 5)) & 15) / 15f);
                    var alpha = Mathf.Clamp(baseAlpha * Mathf.Lerp(0.90f, 1f, dither), 0f, 0.26f);
                    return new Color(1f, 0.78f, 0.40f, alpha);
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

        private static Color SampleCurrentEmptyBookshelfFrontHd2dPixel(int x, int y)
        {
            const int width = 256;
            const int height = 128;
            const int seed = 563;
            const int rowCount = 3;
            const int rowTopMargin = 8;
            const int rowHeight = 31;
            const int rowGap = 5;
            const int rowStride = rowHeight + rowGap;
            const int dividerOne = 83;
            const int dividerTwo = 173;

            if (x <= 0 || y <= 0 || x >= width - 1 || y >= height - 1)
            {
                return ShadeSurface(new Color(0.11f, 0.08f, 0.06f, 1f), x, y, width, height, 0.08f, 0.02f);
            }

            var u = x / (float)(width - 1);
            var v = y / (float)(height - 1);
            var grain = SampleSmoothValueNoise2D((x * 0.075f) + 4.5f, (y * 0.09f) + 11.2f, seed + 3);
            var recessNoise = SampleSmoothValueNoise2D((x * 0.12f) + 19.4f, (y * 0.10f) + 7.6f, seed + 7);
            var dustNoise = SampleSmoothValueNoise2D((x * 0.42f) + 31.1f, (y * 0.45f) + 15.7f, seed + 11);
            var chipNoise = SampleSmoothValueNoise2D((x * 0.21f) + 47.8f, (y * 0.18f) + 2.4f, seed + 17);

            var woodA = new Color(0.18f, 0.13f, 0.09f, 1f);
            var woodB = new Color(0.29f, 0.22f, 0.15f, 1f);
            var woodHighlight = new Color(0.40f, 0.31f, 0.22f, 1f);
            var woodShadow = new Color(0.09f, 0.07f, 0.05f, 1f);
            var recessA = new Color(0.10f, 0.08f, 0.06f, 1f);
            var recessB = new Color(0.15f, 0.11f, 0.08f, 1f);
            var seam = new Color(0.26f, 0.20f, 0.13f, 1f);
            var dust = new Color(0.44f, 0.39f, 0.31f, 1f);
            var paper = new Color(0.76f, 0.71f, 0.58f, 1f);
            var fadedBook = new Color(0.52f, 0.41f, 0.28f, 1f);

            var tone = LerpColor(woodA, woodB, Mathf.Clamp01(grain * 0.78f + (1f - v) * 0.14f));
            tone = LerpColor(tone, woodHighlight, Mathf.Clamp01(0.18f - (u * 0.08f) + grain * 0.05f));
            tone = LerpColor(tone, woodShadow, Mathf.Clamp01((u * 0.20f) + (v * 0.24f)));

            if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
            {
                tone = LerpColor(tone, seam, 0.90f);
            }

            if (Mathf.Abs(x - dividerOne) <= 1 || Mathf.Abs(x - dividerTwo) <= 1)
            {
                tone = LerpColor(tone, seam, 0.82f);
            }

            var rowIndex = (y - rowTopMargin) / rowStride;
            var inRow = rowIndex >= 0 && rowIndex < rowCount;
            var rowStart = rowTopMargin + rowIndex * rowStride;
            var rowEnd = rowStart + rowHeight;
            var rowBand = inRow ? Mathf.Clamp01((y - rowStart) / (float)Mathf.Max(1, rowHeight - 1)) : 0f;

            if (!inRow)
            {
                tone = LerpColor(tone, woodShadow, 0.42f + Mathf.Clamp01(Mathf.Abs((v - 0.5f) * 2f)) * 0.10f);
            }
            else if (y <= rowStart + 2 || y >= rowEnd - 3)
            {
                var boardTone = LerpColor(woodB, woodHighlight, rowBand * 0.18f + (1f - rowBand) * 0.10f);
                boardTone = LerpColor(boardTone, seam, 0.76f);
                tone = LerpColor(tone, boardTone, 0.92f);
            }
            else
            {
                var cavityTone = LerpColor(recessA, recessB, Mathf.Clamp01(recessNoise * 0.90f + (1f - rowBand) * 0.08f));
                cavityTone = LerpColor(cavityTone, woodShadow, 0.54f);
                cavityTone = LerpColor(cavityTone, woodHighlight, Mathf.Clamp01((1f - u) * 0.05f + (1f - v) * 0.03f));
                tone = cavityTone;

                var verticalLight = Mathf.Clamp01(1f - Mathf.Abs(u - 0.5f) * 1.7f);
                tone = LerpColor(tone, woodHighlight, verticalLight * 0.03f);

                if (Mathf.Abs(x - dividerOne) <= 2 || Mathf.Abs(x - dividerTwo) <= 2)
                {
                    tone = LerpColor(tone, seam, 0.54f);
                }

                if ((y == rowStart + 6 || y == rowStart + 14 || y == rowStart + 22) && (x % 9) < 2)
                {
                    tone = LerpColor(tone, dust, 0.22f);
                }

                if (Hash01(x, y, seed + 19) > 0.993f)
                {
                    tone = Lighten(tone, 0.04f);
                }

                if (Hash01(x, y, seed + 23) > 0.996f)
                {
                    tone = Darken(tone, 0.08f);
                }

                if (rowIndex == 1 && Hash01(x, y, seed + 29) > 0.971f)
                {
                    tone = LerpColor(tone, dust, 0.14f);
                }

                if (rowIndex == 2 && chipNoise > 0.72f && x > 8 && x < width - 8)
                {
                    tone = LerpColor(tone, woodHighlight, 0.06f);
                }

                if (Hash01(x, y, seed + 31) > 0.989f)
                {
                    tone = LerpColor(tone, paper, 0.14f);
                }

                if (Hash01(x, y, seed + 37) > 0.996f)
                {
                    tone = LerpColor(tone, fadedBook, 0.16f);
                }
            }

            if (y == rowTopMargin - 1 || y == rowTopMargin + rowStride - 1 || y == rowTopMargin + rowStride * 2 - 1 || y == rowTopMargin + rowStride * 3 - 1)
            {
                tone = LerpColor(tone, seam, 0.68f);
            }

            if (Mathf.Abs(((x + seed) % 37) - 18) <= 1 || Mathf.Abs(((y + seed) % 23) - 11) <= 1)
            {
                tone = LerpColor(tone, dust, 0.11f);
            }

            if (Hash01(x, y, seed + 41) > 0.994f)
            {
                tone = Hash01(x, y, seed + 43) > 0.5f ? Lighten(tone, 0.05f) : Darken(tone, 0.05f);
            }

            return ShadeSurface(tone, x, y, width, height, 0.18f, 0.08f);
        }

        private static Color SampleCurrentBedHd2dPixel(int x, int y)
        {
            return SampleFabricPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.36f, 0.33f, 0.42f, 1f),
                new Color(0.48f, 0.44f, 0.56f, 1f),
                new Color(0.24f, 0.21f, 0.29f, 1f),
                new Color(0.20f, 0.18f, 0.24f, 1f),
                new Color(0.58f, 0.55f, 0.67f, 1f),
                new Color(0.16f, 0.14f, 0.20f, 1f),
                211,
                true);
        }

        private static Color SamplePastBedHd2dPixel(int x, int y)
        {
            return SampleFabricPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.38f, 0.52f, 0.72f, 1f),
                new Color(0.50f, 0.66f, 0.84f, 1f),
                new Color(0.24f, 0.32f, 0.44f, 1f),
                new Color(0.18f, 0.23f, 0.33f, 1f),
                new Color(0.70f, 0.80f, 0.91f, 1f),
                new Color(0.20f, 0.29f, 0.40f, 1f),
                223,
                false);
        }

        private static Color SamplePillowHd2dPixel(int x, int y)
        {
            return SampleFabricPlatePixel(
                x,
                y,
                96,
                64,
                new Color(0.84f, 0.82f, 0.77f, 1f),
                new Color(0.93f, 0.90f, 0.85f, 1f),
                new Color(0.66f, 0.62f, 0.57f, 1f),
                new Color(0.72f, 0.69f, 0.64f, 1f),
                new Color(0.97f, 0.95f, 0.92f, 1f),
                new Color(0.62f, 0.58f, 0.53f, 1f),
                239,
                false);
        }

        private static Color SampleCurrentExteriorWallHd2dPixel(int x, int y)
        {
            return SampleWeatheredWallPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.40f, 0.34f, 0.30f, 1f),
                new Color(0.48f, 0.41f, 0.36f, 1f),
                new Color(0.22f, 0.19f, 0.17f, 1f),
                new Color(0.56f, 0.49f, 0.42f, 1f),
                new Color(0.26f, 0.22f, 0.20f, 1f),
                251,
                true);
        }

        private static Color SamplePastExteriorWallHd2dPixel(int x, int y)
        {
            return SampleWeatheredWallPlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.52f, 0.42f, 0.30f, 1f),
                new Color(0.63f, 0.52f, 0.38f, 1f),
                new Color(0.27f, 0.22f, 0.18f, 1f),
                new Color(0.70f, 0.60f, 0.46f, 1f),
                new Color(0.32f, 0.27f, 0.22f, 1f),
                263,
                false);
        }

        private static Color SampleCurrentRoofHd2dPixel(int x, int y)
        {
            return SampleRoofShinglePlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.34f, 0.17f, 0.16f, 1f),
                new Color(0.42f, 0.21f, 0.19f, 1f),
                new Color(0.18f, 0.09f, 0.09f, 1f),
                new Color(0.52f, 0.27f, 0.23f, 1f),
                new Color(0.26f, 0.13f, 0.12f, 1f),
                277,
                true);
        }

        private static Color SamplePastRoofHd2dPixel(int x, int y)
        {
            return SampleRoofShinglePlatePixel(
                x,
                y,
                128,
                128,
                new Color(0.50f, 0.28f, 0.20f, 1f),
                new Color(0.60f, 0.34f, 0.24f, 1f),
                new Color(0.26f, 0.14f, 0.11f, 1f),
                new Color(0.72f, 0.42f, 0.30f, 1f),
                new Color(0.34f, 0.20f, 0.15f, 1f),
                283,
                false);
        }

        private static Color SampleWindowLightHd2dPixel(int x, int y)
        {
            return SampleWindowPlatePixel(
                x,
                y,
                96,
                96,
                new Color(0.80f, 0.78f, 0.62f, 1f),
                new Color(0.94f, 0.92f, 0.80f, 1f),
                new Color(0.39f, 0.31f, 0.18f, 1f),
                new Color(0.58f, 0.49f, 0.24f, 1f),
                new Color(0.98f, 0.96f, 0.88f, 1f),
                new Color(0.28f, 0.22f, 0.11f, 1f),
                307,
                true);
        }

        private static Color SampleEmptyWindowHd2dPixel(int x, int y)
        {
            return SampleWindowPlatePixel(
                x,
                y,
                96,
                96,
                new Color(0.06f, 0.07f, 0.09f, 1f),
                new Color(0.12f, 0.13f, 0.16f, 1f),
                new Color(0.18f, 0.15f, 0.10f, 1f),
                new Color(0.28f, 0.25f, 0.18f, 1f),
                new Color(0.20f, 0.22f, 0.24f, 1f),
                new Color(0.03f, 0.03f, 0.04f, 1f),
                313,
                false);
        }

        private static Color SampleCurrentPlankDebrisHd2dPixel(int x, int y)
        {
            return SamplePlankDebrisPlatePixel(
                x,
                y,
                128,
                64,
                new Color(0.28f, 0.24f, 0.21f, 1f),
                new Color(0.36f, 0.30f, 0.26f, 1f),
                new Color(0.18f, 0.16f, 0.14f, 1f),
                new Color(0.46f, 0.40f, 0.34f, 1f),
                new Color(0.16f, 0.14f, 0.13f, 1f),
                331,
                true);
        }

        private static Color SampleCurrentHouseDoorDetailHd2dPixel(int x, int y)
        {
            return SampleDoorDetailPlatePixel(x, y, 96, 160,
                new Color(0.30f, 0.20f, 0.12f, 1f),
                new Color(0.39f, 0.27f, 0.16f, 1f),
                new Color(0.16f, 0.12f, 0.09f, 1f),
                new Color(0.48f, 0.35f, 0.21f, 1f),
                new Color(0.56f, 0.47f, 0.28f, 1f),
                new Color(0.30f, 0.28f, 0.18f, 1f),
                359,
                true);
        }

        private static Color SamplePastHouseDoorDetailHd2dPixel(int x, int y)
        {
            return SampleDoorDetailPlatePixel(x, y, 96, 160,
                new Color(0.46f, 0.30f, 0.16f, 1f),
                new Color(0.57f, 0.38f, 0.20f, 1f),
                new Color(0.24f, 0.17f, 0.11f, 1f),
                new Color(0.63f, 0.48f, 0.28f, 1f),
                new Color(0.74f, 0.61f, 0.35f, 1f),
                new Color(0.42f, 0.34f, 0.18f, 1f),
                373,
                false);
        }

        private static Color SampleCurrentLibraryDoorDetailHd2dPixel(int x, int y)
        {
            return SampleDoorDetailPlatePixel(x, y, 96, 160,
                new Color(0.27f, 0.18f, 0.11f, 1f),
                new Color(0.36f, 0.25f, 0.15f, 1f),
                new Color(0.15f, 0.11f, 0.08f, 1f),
                new Color(0.44f, 0.33f, 0.21f, 1f),
                new Color(0.55f, 0.45f, 0.30f, 1f),
                new Color(0.28f, 0.24f, 0.16f, 1f),
                389,
                true);
        }

        private static Color SamplePastLibraryDoorDetailHd2dPixel(int x, int y)
        {
            return SampleDoorDetailPlatePixel(x, y, 96, 160,
                new Color(0.39f, 0.26f, 0.15f, 1f),
                new Color(0.50f, 0.34f, 0.20f, 1f),
                new Color(0.21f, 0.15f, 0.10f, 1f),
                new Color(0.60f, 0.46f, 0.28f, 1f),
                new Color(0.71f, 0.58f, 0.36f, 1f),
                new Color(0.36f, 0.30f, 0.18f, 1f),
                401,
                false);
        }

        private static Color SampleCurrentRubbleDetailHd2dPixel(int x, int y)
        {
            return SampleRubbleDetailPlatePixel(x, y, 128, 64,
                new Color(0.22f, 0.18f, 0.15f, 1f),
                new Color(0.34f, 0.28f, 0.22f, 1f),
                new Color(0.15f, 0.13f, 0.11f, 1f),
                new Color(0.60f, 0.51f, 0.35f, 1f),
                new Color(0.54f, 0.55f, 0.57f, 1f),
                new Color(0.80f, 0.74f, 0.60f, 1f),
                419);
        }

        private static Color SampleDoorDetailPlatePixel(int x, int y, int width, int height, Color boardA, Color boardB, Color seamColor, Color insetColor, Color edgeHighlight, Color handleColor, int seed, bool currentTone)
        {
            var boardWidth = 12;
            var boardIndex = x / boardWidth;
            var withinBoard = x % boardWidth;
            var boardTone = LerpColor(boardA, boardB, Hash01(boardIndex, y / 11, seed));
            var grain = Mathf.Sin((y * 0.15f) + (boardIndex * 0.82f) + seed * 0.011f) * 0.5f + 0.5f;
            boardTone = LerpColor(boardTone, edgeHighlight, grain * 0.20f);
            boardTone = LerpColor(boardTone, Darken(boardTone, 0.26f), Mathf.Clamp01((y / (float)(height - 1)) * 0.18f));

            if (withinBoard <= 1 || withinBoard >= boardWidth - 2 || x == 0 || x == width - 1)
            {
                boardTone = LerpColor(boardTone, seamColor, 0.88f);
            }

            var leftRail = Mathf.Abs(x - width * 0.33f) <= 1.8f;
            var rightRail = Mathf.Abs(x - width * 0.66f) <= 1.8f;
            var upperBand = Mathf.Abs(y - height * 0.30f) <= 2.5f;
            var lowerBand = Mathf.Abs(y - height * 0.67f) <= 2.5f;
            if (leftRail || rightRail || upperBand || lowerBand)
            {
                boardTone = LerpColor(boardTone, insetColor, 0.80f);
            }

            if (Mathf.Abs(x - width * 0.50f) <= 1.0f && y > height * 0.20f && y < height * 0.78f)
            {
                boardTone = LerpColor(boardTone, Darken(seamColor, 0.20f), 0.58f);
            }

            var handleCenterX = width * 0.77f;
            var handleCenterY = height * 0.55f;
            var handleDx = Mathf.Abs(x - handleCenterX);
            var handleDy = Mathf.Abs(y - handleCenterY);
            if (handleDx <= 4f && handleDy <= 10f)
            {
                var handleMask = Mathf.Clamp01(1f - ((handleDx / 4f) * 0.72f + (handleDy / 10f) * 0.48f));
                boardTone = LerpColor(boardTone, handleColor, handleMask * 0.66f);
            }

            var panelMask = Mathf.Clamp01(1f - (Mathf.Abs(x - width * 0.50f) / (width * 0.50f) * 0.84f + Mathf.Abs(y - height * 0.50f) / (height * 0.50f) * 0.52f));
            if (panelMask > 0f)
            {
                boardTone = LerpColor(boardTone, currentTone ? Lighten(insetColor, 0.08f) : Darken(insetColor, 0.04f), panelMask * 0.10f);
            }

            if (Hash01(x, y, seed + 19) > (currentTone ? 0.972f : 0.982f))
            {
                boardTone = Hash01(x, y, seed + 23) > 0.52f ? Darken(boardTone, currentTone ? 0.18f : 0.12f) : Lighten(boardTone, currentTone ? 0.08f : 0.05f);
            }

            return ShadeSurface(boardTone, x, y, width, height, currentTone ? 0.16f : 0.14f, currentTone ? 0.10f : 0.08f);
        }

        private static Color SampleRubbleDetailPlatePixel(int x, int y, int width, int height, Color dustA, Color dustB, Color seamColor, Color boardColor, Color bookColor, Color stoneColor, int seed)
        {
            var cellSize = 8;
            var cellX = x / cellSize;
            var cellY = y / cellSize;
            var withinX = x % cellSize;
            var withinY = y % cellSize;
            var cellType = (int)(Hash01(cellX, cellY, seed) * 4f);
            var baseTone = LerpColor(dustA, dustB, Hash01(cellX * 2, cellY * 3, seed + 5));
            baseTone = LerpColor(baseTone, seamColor, Mathf.Clamp01((withinY / (float)(cellSize - 1)) * 0.12f));

            switch (cellType)
            {
                case 0:
                {
                    var boardMask = Mathf.Clamp01(1f - (Mathf.Abs(withinX - 3.4f) * 0.28f + Mathf.Abs(withinY - 3.7f) * 0.26f));
                    var boardTone = LerpColor(boardColor, Lighten(boardColor, 0.18f), Hash01(cellX, cellY, seed + 11));
                    boardTone = LerpColor(boardTone, seamColor, 0.36f);
                    if (withinX <= 1 || withinX >= cellSize - 2 || withinY <= 1 || withinY >= cellSize - 2)
                    {
                        boardTone = LerpColor(boardTone, Darken(seamColor, 0.12f), 0.70f);
                    }

                    if (withinY == 3 || withinY == 4)
                    {
                        boardTone = LerpColor(boardTone, Darken(boardColor, 0.24f), 0.42f);
                    }

                    if (Hash01(cellX, cellY, seed + 17) > 0.64f)
                    {
                        var crack = Mathf.Abs(withinX - withinY - 1.5f);
                        if (crack <= 1.2f)
                        {
                            boardTone = Darken(boardTone, 0.22f);
                        }
                    }

                    baseTone = LerpColor(baseTone, boardTone, boardMask);
                    break;
                }
                case 1:
                {
                    var bookMask = Mathf.Clamp01(1f - (Mathf.Abs(withinX - 3.1f) * 0.24f + Mathf.Abs(withinY - 3.5f) * 0.26f));
                    var coverTone = LerpColor(bookColor, Darken(bookColor, 0.18f), Hash01(cellX, cellY, seed + 13));
                    var pageTone = LerpColor(new Color(0.80f, 0.75f, 0.62f, 1f), new Color(0.92f, 0.88f, 0.77f, 1f), Hash01(cellX, cellY, seed + 15));
                    if (withinX <= 1)
                    {
                        coverTone = Darken(coverTone, 0.32f);
                    }

                    if (withinX >= cellSize - 2)
                    {
                        pageTone = Lighten(pageTone, 0.12f);
                    }

                    if (withinY == 2 || withinY == 5)
                    {
                        pageTone = LerpColor(pageTone, seamColor, 0.40f);
                    }

                    var split = Mathf.Abs(withinX - 4.4f) <= 0.9f ? coverTone : pageTone;
                    if (withinY < 2 || withinY > 5)
                    {
                        split = LerpColor(split, seamColor, 0.60f);
                    }

                    baseTone = LerpColor(baseTone, split, bookMask);
                    break;
                }
                case 2:
                {
                    var stoneMask = Mathf.Clamp01(1f - (Mathf.Abs(withinX - 3.5f) * 0.22f + Mathf.Abs(withinY - 3.5f) * 0.30f));
                    var stoneTone = LerpColor(stoneColor, Lighten(stoneColor, 0.14f), Hash01(cellX, cellY, seed + 21));
                    stoneTone = LerpColor(stoneTone, seamColor, 0.28f);
                    if (withinX <= 1 || withinX >= cellSize - 2 || withinY <= 1 || withinY >= cellSize - 2)
                    {
                        stoneTone = LerpColor(stoneTone, Darken(seamColor, 0.18f), 0.70f);
                    }

                    if (((withinX + withinY + seed) & 3) == 0)
                    {
                        stoneTone = Darken(stoneTone, 0.20f);
                    }

                    baseTone = LerpColor(baseTone, stoneTone, stoneMask);
                    break;
                }
                default:
                {
                    if (Hash01(x, y, seed + 29) > 0.92f)
                    {
                        baseTone = Lighten(baseTone, 0.08f);
                    }

                    break;
                }
            }

            if (Hash01(x, y, seed + 31) > 0.988f)
            {
                baseTone = Darken(baseTone, 0.20f);
            }

            return ShadeSurface(baseTone, x, y, width, height, 0.16f, 0.08f);
        }

        private static Color SamplePastPlankHd2dPixel(int x, int y)
        {
            return SamplePlankDebrisPlatePixel(
                x,
                y,
                128,
                64,
                new Color(0.40f, 0.29f, 0.18f, 1f),
                new Color(0.51f, 0.38f, 0.24f, 1f),
                new Color(0.23f, 0.17f, 0.12f, 1f),
                new Color(0.62f, 0.48f, 0.30f, 1f),
                new Color(0.24f, 0.18f, 0.14f, 1f),
                347,
                false);
        }

        private static Color SampleCurrentPathHd2dPixel(int x, int y)
        {
            return SamplePathFlagstoneHd2dPixel(
                x,
                y,
                128,
                128,
                401,
                false,
                new Color(0.30f, 0.27f, 0.22f, 1f),
                new Color(0.35f, 0.31f, 0.26f, 1f),
                new Color(0.24f, 0.22f, 0.18f, 1f),
                new Color(0.40f, 0.36f, 0.30f, 1f),
                new Color(0.19f, 0.17f, 0.14f, 1f),
                new Color(0.27f, 0.24f, 0.20f, 1f),
                new Color(0.16f, 0.14f, 0.11f, 1f),
                new Color(0.13f, 0.11f, 0.09f, 1f),
                new Color(0.21f, 0.18f, 0.15f, 1f));
        }

        private static Color SamplePastPathHd2dPixel(int x, int y)
        {
            return SamplePathFlagstoneHd2dPixel(
                x,
                y,
                128,
                128,
                419,
                true,
                new Color(0.36f, 0.31f, 0.25f, 1f),
                new Color(0.42f, 0.36f, 0.29f, 1f),
                new Color(0.30f, 0.26f, 0.21f, 1f),
                new Color(0.48f, 0.42f, 0.34f, 1f),
                new Color(0.23f, 0.19f, 0.15f, 1f),
                new Color(0.32f, 0.28f, 0.23f, 1f),
                new Color(0.18f, 0.15f, 0.12f, 1f),
                new Color(0.15f, 0.12f, 0.10f, 1f),
                new Color(0.27f, 0.22f, 0.18f, 1f));
        }

        private static Color SampleCurrentGrassHd2dPixel(int x, int y)
        {
            return SampleGrassAndSoilHd2dPixel(
                x,
                y,
                128,
                128,
                503,
                false,
                new Color(0.16f, 0.19f, 0.12f, 1f),
                new Color(0.20f, 0.24f, 0.14f, 1f),
                new Color(0.26f, 0.29f, 0.18f, 1f),
                new Color(0.31f, 0.33f, 0.22f, 1f),
                new Color(0.17f, 0.14f, 0.09f, 1f),
                new Color(0.24f, 0.19f, 0.13f, 1f),
                new Color(0.12f, 0.12f, 0.09f, 1f),
                new Color(0.31f, 0.29f, 0.18f, 1f),
                new Color(0.14f, 0.15f, 0.10f, 1f),
                new Color(0.23f, 0.25f, 0.16f, 1f));
        }

        private static Color SamplePastGrassHd2dPixel(int x, int y)
        {
            return SampleGrassAndSoilHd2dPixel(
                x,
                y,
                128,
                128,
                521,
                true,
                new Color(0.19f, 0.25f, 0.13f, 1f),
                new Color(0.24f, 0.30f, 0.16f, 1f),
                new Color(0.28f, 0.35f, 0.19f, 1f),
                new Color(0.34f, 0.39f, 0.23f, 1f),
                new Color(0.19f, 0.16f, 0.10f, 1f),
                new Color(0.27f, 0.22f, 0.14f, 1f),
                new Color(0.14f, 0.15f, 0.11f, 1f),
                new Color(0.40f, 0.37f, 0.24f, 1f),
                new Color(0.17f, 0.18f, 0.12f, 1f),
                new Color(0.30f, 0.31f, 0.19f, 1f));
        }

        private static Color SampleGrassAndSoilHd2dPixel(int x, int y, int width, int height, int seed, bool pastTone, Color grassA, Color grassB, Color grassC, Color grassD, Color soilA, Color soilB, Color seamColor, Color highlightColor, Color shadowColor, Color bladeColor)
        {
            if (x <= 0 || y <= 0 || x >= width - 1 || y >= height - 1)
            {
                var border = LerpColor(seamColor, shadowColor, pastTone ? 0.30f : 0.46f);
                return ShadeSurface(border, x, y, width, height, pastTone ? 0.06f : 0.08f, 0.02f);
            }

            var u = width <= 1 ? 0f : x / (float)(width - 1);
            var v = height <= 1 ? 0f : y / (float)(height - 1);

            var broadNoise = SampleSmoothValueNoise2D((x * 0.0625f) + (seed * 0.11f), (y * 0.0625f) + (seed * 0.07f), seed + 3);
            var mediumNoise = SampleSmoothValueNoise2D((x * 0.125f) + 11.3f, (y * 0.125f) + 7.9f, seed + 7);
            var fineNoise = SampleSmoothValueNoise2D((x * 0.25f) + 3.7f, (y * 0.25f) + 5.1f, seed + 11);
            var leafNoise = SampleSmoothValueNoise2D((x * 0.5f) + 19.4f, (y * 0.5f) + 23.8f, seed + 13);
            var soilNoise = SampleSmoothValueNoise2D((x * 0.08f) + 41.6f, (y * 0.08f) + 27.2f, seed + 17);

            var tone = LerpColor(grassA, grassD, broadNoise * 0.28f);
            tone = LerpColor(tone, grassB, Mathf.Clamp01(mediumNoise * 0.75f));
            tone = LerpColor(tone, grassC, Mathf.Clamp01((1f - broadNoise) * 0.32f));

            var soilTone = LerpColor(soilA, soilB, Mathf.Clamp01(soilNoise));
            var soilMask = Mathf.Clamp01((soilNoise - (pastTone ? 0.44f : 0.50f)) * (pastTone ? 1.55f : 1.75f));
            soilMask = Mathf.Clamp01(soilMask + Mathf.Abs(mediumNoise - 0.5f) * (pastTone ? 0.12f : 0.18f));
            if (soilMask > 0f)
            {
                tone = LerpColor(tone, soilTone, soilMask * (pastTone ? 0.44f : 0.58f));
            }

            var clumpNoise = SampleSmoothValueNoise2D((x * 0.10f) + 8.8f, (y * 0.10f) + 2.4f, seed + 19);
            var clumpMask = Mathf.Clamp01((clumpNoise - (pastTone ? 0.58f : 0.62f)) * 2.1f);
            if (clumpMask > 0f)
            {
                tone = LerpColor(tone, pastTone ? highlightColor : shadowColor, clumpMask * (pastTone ? 0.16f : 0.20f));
            }

            var tuftNoise = SampleSmoothValueNoise2D((x * 0.17f) + 14.7f, (y * 0.17f) + 9.1f, seed + 23);
            if (tuftNoise > (pastTone ? 0.63f : 0.68f))
            {
                var tuftMask = Mathf.Clamp01((tuftNoise - (pastTone ? 0.63f : 0.68f)) * (pastTone ? 2.6f : 2.9f));
                tone = LerpColor(tone, bladeColor, tuftMask * (pastTone ? 0.08f : 0.06f));
            }

            var diagonalA = Mathf.Abs(Mathf.Sin((x * 0.22f) + (y * 0.11f) + seed * 0.013f + broadNoise * 2.4f));
            var diagonalB = Mathf.Abs(Mathf.Sin((x * -0.18f) + (y * 0.31f) + seed * 0.017f + mediumNoise * 1.8f));
            if (diagonalA > 0.965f && y > 1 && y < height - 2)
            {
                tone = LerpColor(tone, bladeColor, pastTone ? 0.11f : 0.09f);
            }

            if (diagonalB > 0.976f && x > 1 && x < width - 2)
            {
                tone = LerpColor(tone, shadowColor, pastTone ? 0.08f : 0.10f);
            }

            if (fineNoise > 0.70f)
            {
                tone = LerpColor(tone, pastTone ? highlightColor : shadowColor, pastTone ? 0.05f : 0.07f);
            }

            if (Hash01(x, y, seed + 29) > (pastTone ? 0.994f : 0.991f))
            {
                tone = pastTone ? Lighten(tone, 0.02f) : Darken(tone, 0.04f);
            }

            var edgeFade = Mathf.Lerp(0.08f, 0.02f, Mathf.Clamp01(1f - Mathf.Abs(0.5f - v) * 1.6f));
            tone = LerpColor(tone, pastTone ? highlightColor : shadowColor, (1f - broadNoise) * edgeFade);

            return ShadeSurface(tone, x, y, width, height, pastTone ? 0.07f : 0.09f, pastTone ? 0.03f : 0.02f);
        }

        private static float SampleSmoothValueNoise2D(float x, float y, int seed)
        {
            var x0 = Mathf.FloorToInt(x);
            var y0 = Mathf.FloorToInt(y);
            var fx = x - x0;
            var fy = y - y0;
            fx = fx * fx * (3f - (2f * fx));
            fy = fy * fy * (3f - (2f * fy));

            var n00 = Hash01(x0, y0, seed);
            var n10 = Hash01(x0 + 1, y0, seed);
            var n01 = Hash01(x0, y0 + 1, seed);
            var n11 = Hash01(x0 + 1, y0 + 1, seed);
            var nx0 = Mathf.Lerp(n00, n10, fx);
            var nx1 = Mathf.Lerp(n01, n11, fx);
            return Mathf.Lerp(nx0, nx1, fy);
        }

        private static Color SamplePathFlagstoneHd2dPixel(int x, int y, int width, int height, int seed, bool past, Color stoneA, Color stoneB, Color stoneC, Color stoneD, Color seamColor, Color highlightColor, Color shadowColor, Color crackColor, Color dustColor)
        {
            if (x <= 0 || y <= 0 || x >= width - 1 || y >= height - 1)
            {
                var edge = LerpColor(seamColor, shadowColor, past ? 0.34f : 0.48f);
                return ShadeSurface(edge, x, y, width, height, 0.04f, 0.01f);
            }

            if (!TryResolvePathFlagstoneCell(x, y, width, height, seed, past, out var cellColumnIndex, out var cellRowIndex, out var cellStartX, out var cellStartY, out var cellWidth, out var cellHeight, out var withinX, out var withinY))
            {
                var fallback = LerpColor(seamColor, shadowColor, past ? 0.52f : 0.66f);
                return ShadeSurface(fallback, x, y, width, height, past ? 0.08f : 0.10f, past ? 0.03f : 0.02f);
            }

            var stoneRoll = Hash01(cellColumnIndex, cellRowIndex, seed + 3);
            var stoneTone = stoneRoll < 0.22f ? stoneA :
                stoneRoll < 0.44f ? stoneB :
                stoneRoll < 0.66f ? stoneC :
                stoneRoll < 0.84f ? stoneD :
                LerpColor(stoneB, stoneD, 0.42f);

            stoneTone = LerpColor(stoneTone, past ? highlightColor : shadowColor, past ? 0.07f + stoneRoll * 0.06f : 0.05f + stoneRoll * 0.05f);

            var edgeBand = withinX <= 1 || withinX >= cellWidth - 2 || withinY <= 1 || withinY >= cellHeight - 2;
            if (edgeBand)
            {
                stoneTone = LerpColor(stoneTone, seamColor, past ? 0.56f : 0.70f);
            }

            if (withinX == 2 || withinY == 2 || withinX == cellWidth - 3 || withinY == cellHeight - 3)
            {
                stoneTone = LerpColor(stoneTone, seamColor, past ? 0.18f : 0.24f);
            }

            var centerLift = Mathf.Clamp01(1f - (
                Mathf.Abs((withinX - ((cellWidth - 1) * 0.5f)) / Mathf.Max(1f, cellWidth * 0.5f)) * 0.92f +
                Mathf.Abs((withinY - ((cellHeight - 1) * 0.5f)) / Mathf.Max(1f, cellHeight * 0.5f)) * 0.86f));
            if (past)
            {
                stoneTone = LerpColor(stoneTone, highlightColor, centerLift * 0.12f);
            }
            else
            {
                stoneTone = LerpColor(stoneTone, shadowColor, (1f - centerLift) * 0.09f);
            }

            var crackRoll = Hash01(cellColumnIndex, cellRowIndex, seed + 11);
            if (crackRoll > (past ? 0.80f : 0.64f))
            {
                var diagonalA = Mathf.Abs(((withinX * 2) + withinY + cellColumnIndex + seed) % 19 - 9);
                var diagonalB = Mathf.Abs((withinX + (withinY * 2) + cellRowIndex + seed + 7) % 17 - 8);
                if (diagonalA <= 1 || diagonalB <= 1 || Mathf.Abs(withinX - withinY) <= 1)
                {
                    stoneTone = LerpColor(stoneTone, crackColor, past ? 0.26f : 0.42f);
                }
            }

            var chipRoll = Hash01(cellStartX + withinX, cellStartY + withinY, seed + 19);
            if (!past && chipRoll > 0.984f)
            {
                stoneTone = LerpColor(stoneTone, shadowColor, 0.62f);
            }
            else if (past && chipRoll > 0.992f)
            {
                stoneTone = LerpColor(stoneTone, highlightColor, 0.16f);
            }

            var dustRoll = Hash01(cellColumnIndex, cellRowIndex, seed + 23);
            if (!past && dustRoll > 0.58f && withinX > 1 && withinX < cellWidth - 2 && withinY > 1 && withinY < cellHeight - 2)
            {
                var dustBand = Mathf.Abs(((withinX + withinY + seed) % 13) - 6);
                if (dustBand <= 1)
                {
                    stoneTone = LerpColor(stoneTone, dustColor, 0.22f);
                }
            }
            else if (past && dustRoll > 0.72f && withinX > 1 && withinX < cellWidth - 2 && withinY > 1 && withinY < cellHeight - 2)
            {
                stoneTone = LerpColor(stoneTone, highlightColor, 0.08f);
            }

            if (Hash01(x, y, seed + 29) > (past ? 0.994f : 0.989f))
            {
                stoneTone = past ? Lighten(stoneTone, 0.02f) : Darken(stoneTone, 0.05f);
            }

            return ShadeSurface(stoneTone, x, y, width, height, past ? 0.08f : 0.11f, past ? 0.05f : 0.03f);
        }

        private static bool TryResolvePathFlagstoneCell(int x, int y, int width, int height, int seed, bool past, out int cellColumnIndex, out int cellRowIndex, out int cellStartX, out int cellStartY, out int cellWidth, out int cellHeight, out int withinX, out int withinY)
        {
            cellColumnIndex = 0;
            cellRowIndex = 0;
            cellStartX = 0;
            cellStartY = 0;
            cellWidth = 0;
            cellHeight = 0;
            withinX = 0;
            withinY = 0;

            var rowCursor = 0;
            for (var rowIndex = 0; rowIndex < 16 && rowCursor < height; rowIndex++)
            {
                var rowHeight = GetPathStoneHeight(rowIndex, seed, past);
                if (rowCursor + rowHeight > height)
                {
                    rowHeight = height - rowCursor;
                }

                if (rowHeight <= 0)
                {
                    break;
                }

                var rowEnd = rowCursor + rowHeight;
                if (y >= rowCursor && y < rowEnd)
                {
                    withinY = y - rowCursor;
                    var columnCursor = 0;
                    for (var columnIndex = 0; columnIndex < 16 && columnCursor < width; columnIndex++)
                    {
                        var columnWidth = GetPathStoneWidth(columnIndex, rowIndex, seed, past);
                        if (columnCursor + columnWidth > width)
                        {
                            columnWidth = width - columnCursor;
                        }

                        if (columnWidth <= 0)
                        {
                            break;
                        }

                        var columnEnd = columnCursor + columnWidth;
                        if (x >= columnCursor && x < columnEnd)
                        {
                            cellColumnIndex = columnIndex;
                            cellRowIndex = rowIndex;
                            cellStartX = columnCursor;
                            cellStartY = rowCursor;
                            cellWidth = columnWidth;
                            cellHeight = rowHeight;
                            withinX = x - columnCursor;
                            return true;
                        }

                        columnCursor = columnEnd;
                    }
                }

                rowCursor = rowEnd;
            }

            return false;
        }

        private static int GetPathStoneWidth(int stoneColumnIndex, int stoneRowIndex, int seed, bool past)
        {
            var roll = Hash01(stoneColumnIndex, stoneRowIndex, seed + (past ? 517 : 509));
            if (past)
            {
                if (roll < 0.12f) return 12;
                if (roll < 0.28f) return 13;
                if (roll < 0.46f) return 14;
                if (roll < 0.63f) return 16;
                if (roll < 0.79f) return 18;
                if (roll < 0.91f) return 20;
                return 22;
            }

            if (roll < 0.12f) return 11;
            if (roll < 0.28f) return 12;
            if (roll < 0.45f) return 13;
            if (roll < 0.62f) return 15;
            if (roll < 0.78f) return 17;
            if (roll < 0.90f) return 19;
            return 22;
        }

        private static int GetPathStoneHeight(int stoneRowIndex, int seed, bool past)
        {
            var roll = Hash01(stoneRowIndex, seed, past ? 541 : 533);
            if (past)
            {
                if (roll < 0.12f) return 12;
                if (roll < 0.28f) return 13;
                if (roll < 0.46f) return 14;
                if (roll < 0.64f) return 15;
                if (roll < 0.80f) return 17;
                if (roll < 0.92f) return 19;
                return 20;
            }

            if (roll < 0.12f) return 11;
            if (roll < 0.28f) return 12;
            if (roll < 0.45f) return 13;
            if (roll < 0.62f) return 14;
            if (roll < 0.78f) return 16;
            if (roll < 0.90f) return 18;
            return 20;
        }

        private static Color SampleFabricPlatePixel(int x, int y, int width, int height, Color fabricA, Color fabricB, Color stitchColor, Color borderColor, Color highlightColor, Color shadowColor, int seed, bool worn)
        {
            var color = LerpColor(fabricA, fabricB, Hash01(x / 4, y / 4, seed));
            var weave = ((x + y + seed) & 3) < 2 ? 0.06f : -0.04f;
            color = LerpColor(color, weave > 0f ? highlightColor : shadowColor, Mathf.Abs(weave));

            if ((x % 4) < 2)
            {
                color = LerpColor(color, highlightColor, 0.07f);
            }
            else
            {
                color = LerpColor(color, shadowColor, 0.05f);
            }

            if ((y % 5) <= 1)
            {
                color = LerpColor(color, stitchColor, 0.28f);
            }

            if ((x % 16) <= 1 || (x % 16) >= 14 || (y % 16) <= 1 || (y % 16) >= 14)
            {
                color = LerpColor(color, borderColor, 0.62f);
            }

            if (x < 3 || x >= width - 3)
            {
                color = LerpColor(color, highlightColor, 0.12f);
            }

            if (y < 3 || y >= height - 3)
            {
                color = LerpColor(color, shadowColor, 0.12f);
            }

            if (Mathf.Abs(((y + seed) % 24) - 12) <= 1)
            {
                color = LerpColor(color, Darken(stitchColor, 0.18f), 0.45f);
            }

            if (Hash01(x, y, seed + 13) > (worn ? 0.962f : 0.982f))
            {
                color = Hash01(x, y, seed + 17) > 0.5f ? Darken(color, worn ? 0.20f : 0.12f) : Lighten(color, worn ? 0.08f : 0.06f);
            }

            if (worn && Hash01(x, y, seed + 19) > 0.948f)
            {
                color = LerpColor(color, new Color(0.30f, 0.27f, 0.25f, 1f), 0.16f);
            }

            return ShadeSurface(color, x, y, width, height, 0.12f, 0.08f);
        }

        private static Color SampleWeatheredWallPlatePixel(int x, int y, int width, int height, Color wallA, Color wallB, Color seamColor, Color highlightColor, Color shadowColor, int seed, bool worn)
        {
            var boardWidth = 14;
            var boardIndex = x / boardWidth;
            var withinX = x % boardWidth;
            var bandTone = LerpColor(wallA, wallB, Hash01(boardIndex, y / 9, seed));
            var verticalLight = Mathf.Clamp01(1f - (y / (float)(height - 1)) * 0.55f);
            bandTone = LerpColor(bandTone, highlightColor, verticalLight * 0.14f);
            bandTone = LerpColor(bandTone, shadowColor, Mathf.Clamp01((y / (float)(height - 1)) * 0.24f));

            if (withinX <= 1 || withinX >= boardWidth - 2 || x == 0 || x == width - 1)
            {
                bandTone = LerpColor(bandTone, seamColor, 0.80f);
            }

            if ((y % 18) <= 1)
            {
                bandTone = LerpColor(bandTone, Darken(seamColor, 0.18f), 0.42f);
            }

            if (((boardIndex + (y / 7) + seed) & 3) == 0 && withinX > 2 && withinX < boardWidth - 2)
            {
                bandTone = LerpColor(bandTone, Lighten(wallB, worn ? 0.05f : 0.10f), worn ? 0.20f : 0.12f);
            }

            if (Hash01(x, y, seed + 11) > 0.972f)
            {
                bandTone = LerpColor(bandTone, new Color(0.67f, 0.62f, 0.56f, 1f), worn ? 0.16f : 0.11f);
            }

            if (Hash01(x, y, seed + 17) > (worn ? 0.974f : 0.986f))
            {
                bandTone = Darken(bandTone, worn ? 0.18f : 0.10f);
            }

            return ShadeSurface(bandTone, x, y, width, height, 0.18f, 0.11f);
        }

        private static Color SampleRoofShinglePlatePixel(int x, int y, int width, int height, Color tileA, Color tileB, Color seamColor, Color highlightColor, Color shadowColor, int seed, bool weathered)
        {
            var rowHeight = 8;
            var rowIndex = y / rowHeight;
            var withinY = y % rowHeight;
            var rowOffset = (rowIndex & 1) == 0 ? 0 : 6;
            var shiftedX = (x + rowOffset) % width;
            var tileWidth = 12;
            var tileIndex = shiftedX / tileWidth;
            var withinX = shiftedX % tileWidth;
            var roofTone = LerpColor(tileA, tileB, Hash01(tileIndex, rowIndex, seed));
            roofTone = LerpColor(roofTone, highlightColor, Mathf.Clamp01(0.22f - withinY * 0.02f));
            roofTone = LerpColor(roofTone, shadowColor, Mathf.Clamp01((withinY / (float)(rowHeight - 1)) * 0.28f));

            if (withinX <= 1 || withinX >= tileWidth - 2 || withinY <= 1 || withinY >= rowHeight - 2 || x == 0 || x == width - 1 || y == 0 || y == height - 1)
            {
                roofTone = LerpColor(roofTone, seamColor, 0.84f);
            }

            if ((withinX == 4 || withinX == 7) && withinY >= 2 && withinY <= 5)
            {
                roofTone = LerpColor(roofTone, Darken(highlightColor, 0.42f), 0.42f);
            }

            if (Hash01(x, y, seed + 19) > (weathered ? 0.966f : 0.981f))
            {
                roofTone = Hash01(x, y, seed + 23) > 0.55f ? Darken(roofTone, weathered ? 0.18f : 0.11f) : Lighten(roofTone, weathered ? 0.08f : 0.05f);
            }

            return ShadeSurface(roofTone, x, y, width, height, 0.16f, 0.10f);
        }

        private static Color SampleWindowPlatePixel(int x, int y, int width, int height, Color glassA, Color glassB, Color frameColor, Color crossbarColor, Color highlightColor, Color shadowColor, int seed, bool lit)
        {
            var color = LerpColor(glassA, glassB, Hash01(x / 4, y / 4, seed));
            if (lit)
            {
                color = LerpColor(color, highlightColor, 0.24f);
                if (Hash01(x, y, seed + 7) > 0.962f)
                {
                    color = Lighten(color, 0.14f);
                }
            }
            else
            {
                color = LerpColor(color, shadowColor, 0.36f);
                if (Hash01(x, y, seed + 7) > 0.968f)
                {
                    color = Lighten(color, 0.08f);
                }
            }

            if (x < 3 || x >= width - 3 || y < 3 || y >= height - 3)
            {
                color = LerpColor(color, frameColor, 0.88f);
            }

            if (Mathf.Abs(x - (width / 2f)) <= 1f || Mathf.Abs(y - (height / 2f)) <= 1f)
            {
                color = LerpColor(color, crossbarColor, 0.84f);
            }

            if (Mathf.Abs(x - (width * 0.34f)) <= 1f && Mathf.Abs(y - (height * 0.34f)) <= 4f)
            {
                color = LerpColor(color, highlightColor, lit ? 0.32f : 0.12f);
            }

            if (Mathf.Abs(x - (width * 0.66f)) <= 1f && Mathf.Abs(y - (height * 0.66f)) <= 4f)
            {
                color = LerpColor(color, shadowColor, 0.18f);
            }

            return ShadeSurface(color, x, y, width, height, lit ? 0.10f : 0.14f, lit ? 0.08f : 0.05f);
        }

        private static Color SamplePlankDebrisPlatePixel(int x, int y, int width, int height, Color woodA, Color woodB, Color seamColor, Color highlightColor, Color shadowColor, int seed, bool worn)
        {
            var boardWidth = 16;
            var boardIndex = x / boardWidth;
            var withinX = x % boardWidth;
            var grain = Mathf.Sin((y * 0.22f) + (boardIndex * 1.1f) + seed * 0.017f) * 0.5f + 0.5f;
            var tone = LerpColor(woodA, woodB, Hash01(boardIndex, y / 7, seed) * 0.66f + grain * 0.34f);
            tone = LerpColor(tone, highlightColor, Mathf.Clamp01(0.24f - (x / (float)(width - 1)) * 0.16f));
            tone = LerpColor(tone, shadowColor, Mathf.Clamp01((x / (float)(width - 1)) * 0.18f + (y / (float)(height - 1)) * 0.20f));

            if (withinX <= 1 || withinX >= boardWidth - 2 || x == 0 || x == width - 1)
            {
                tone = LerpColor(tone, seamColor, 0.84f);
            }

            if ((y % 18) == 7 || (y % 18) == 8)
            {
                tone = LerpColor(tone, Darken(seamColor, 0.14f), 0.44f);
            }

            if (withinX == 5 || withinX == 10)
            {
                tone = LerpColor(tone, highlightColor, 0.12f);
            }

            if (Hash01(x, y, seed + 17) > 0.968f)
            {
                tone = Darken(tone, worn ? 0.34f : 0.22f);
            }

            if (Hash01(x, y, seed + 23) > 0.988f)
            {
                tone = Lighten(tone, 0.16f);
            }

            if (worn && Hash01(x, y, seed + 29) > 0.944f)
            {
                tone = LerpColor(tone, new Color(0.30f, 0.27f, 0.24f, 1f), 0.20f);
            }

            return ShadeSurface(tone, x, y, width, height, 0.18f, 0.11f);
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

            if (!TryResolveBookSpineRun(x, rowIndex, seed, bookshelfFront, width, out var columnIndex, out var bookStart, out var bookWidth, out var withinBook))
            {
                return SampleShelfGapPixel(x, y, width, height, seed, bookshelfFront);
            }

            var color = PickBookSpineColor(columnIndex, rowIndex, seed, bookshelfFront);
            var bookEnd = bookStart + bookWidth - 1;

            if (x == bookStart)
            {
                color = Darken(color, bookshelfFront ? 0.36f : 0.40f);
            }
            else if (withinBook == 1)
            {
                color = LerpColor(color, Darken(color, 0.44f), 0.58f);
            }
            else if (x == bookEnd - 1)
            {
                color = LerpColor(color, Darken(color, 0.38f), 0.42f);
            }
            else if (x == bookEnd)
            {
                color = Lighten(color, bookshelfFront ? 0.03f : 0.05f);
            }

            var centerLine = Mathf.Clamp(bookWidth / 2, 1, Mathf.Max(1, bookWidth - 2));
            var paperAccent = ((columnIndex + rowIndex + seed) % 5) == 0;
            if (paperAccent && (withinBook == centerLine || withinBook == centerLine - 1))
            {
                color = LerpColor(color, new Color(0.82f, 0.76f, 0.62f, 1f), bookshelfFront ? 0.18f : 0.14f);
            }

            if (withinBook > 2 && withinBook < bookWidth - 3 && (withinBook == centerLine || withinBook == centerLine - 1))
            {
                color = LerpColor(color, new Color(0.86f, 0.80f, 0.68f, 1f), bookshelfFront ? 0.06f : 0.08f);
            }

            if (withinBook > 1 && withinBook < bookWidth - 1 && ((y - rowStart) % 11 == 3))
            {
                color = LerpColor(color, Darken(color, 0.30f), 0.38f);
            }

            if ((y == rowStart || y == rowEnd - 1) && withinBook > 0 && withinBook < bookWidth - 1)
            {
                color = Darken(color, bookshelfFront ? 0.28f : 0.32f);
            }

            if ((y == rowStart + 1 || y == rowEnd - 2) && withinBook > 0 && withinBook < bookWidth - 1)
            {
                color = LerpColor(color, new Color(0.84f, 0.79f, 0.68f, 1f), bookshelfFront ? 0.05f : 0.07f);
            }

            if (Hash01(x, y, seed + 29) > (bookshelfFront ? 0.991f : 0.994f))
            {
                color = Lighten(color, bookshelfFront ? 0.04f : 0.03f);
            }

            return ShadeSurface(color, x, y, width, height, bookshelfFront ? 0.11f : 0.09f, bookshelfFront ? 0.04f : 0.03f);
        }

        private static Color SampleShelfGapPixel(int x, int y, int width, int height, int seed, bool bookshelfFront)
        {
            var shelf = bookshelfFront
                ? new Color(0.15f, 0.11f, 0.08f, 1f)
                : new Color(0.12f, 0.09f, 0.06f, 1f);
            var band = Mathf.Clamp01(1f - Mathf.Abs((y / (float)(height - 1)) - 0.5f) * 2f);
            var shadow = bookshelfFront ? 0.24f : 0.20f;
            shelf = LerpColor(shelf, Darken(shelf, 0.24f), shadow * 0.8f);
            if ((y % 16) <= 1 || (y % 16) >= 14)
            {
                shelf = Lighten(shelf, 0.02f);
            }

            if (Hash01(x, y, seed + 41) > 0.988f)
            {
                shelf = Lighten(shelf, 0.03f);
            }

            return ShadeSurface(shelf, x, y, width, height, 0.11f, 0.02f + band * 0.02f);
        }

        private static Color PickBookSpineColor(int columnIndex, int rowIndex, int seed, bool bookshelfFront)
        {
            var selector = Math.Abs((columnIndex * 5) + (rowIndex * 11) + seed) % (bookshelfFront ? 7 : 8);
            if (bookshelfFront)
            {
                switch (selector)
                {
                    case 0:
                        return new Color(0.41f, 0.18f, 0.16f, 1f);
                    case 1:
                        return new Color(0.47f, 0.26f, 0.25f, 1f);
                    case 2:
                        return new Color(0.25f, 0.31f, 0.40f, 1f);
                    case 3:
                        return new Color(0.57f, 0.46f, 0.27f, 1f);
                    case 4:
                        return new Color(0.72f, 0.64f, 0.52f, 1f);
                    case 5:
                        return new Color(0.40f, 0.32f, 0.37f, 1f);
                    default:
                        return new Color(0.28f, 0.39f, 0.31f, 1f);
                }
            }

            switch (selector)
            {
                case 0:
                    return new Color(0.51f, 0.22f, 0.19f, 1f);
                case 1:
                    return new Color(0.27f, 0.36f, 0.51f, 1f);
                case 2:
                    return new Color(0.64f, 0.54f, 0.31f, 1f);
                case 3:
                    return new Color(0.78f, 0.70f, 0.58f, 1f);
                case 4:
                    return new Color(0.50f, 0.36f, 0.41f, 1f);
                case 5:
                    return new Color(0.30f, 0.45f, 0.35f, 1f);
                case 6:
                    return new Color(0.58f, 0.41f, 0.23f, 1f);
                case 7:
                    return new Color(0.38f, 0.44f, 0.40f, 1f);
                default:
                    return new Color(0.44f, 0.36f, 0.28f, 1f);
            }
        }

        private static bool TryResolveBookSpineRun(int x, int rowIndex, int seed, bool bookshelfFront, int width, out int bookIndex, out int bookStart, out int bookWidth, out int withinBook)
        {
            bookIndex = 0;
            bookStart = 0;
            bookWidth = 0;
            withinBook = 0;

            var cursor = 0;
            for (var index = 0; index < 40 && cursor < width; index++)
            {
                var widthForBook = GetBookSpineWidth(index, rowIndex, seed, bookshelfFront);
                if (widthForBook < 5)
                {
                    widthForBook = 5;
                }
                else if (widthForBook > 14)
                {
                    widthForBook = 14;
                }

                if (cursor + widthForBook > width)
                {
                    widthForBook = width - cursor;
                }

                if (widthForBook <= 0)
                {
                    break;
                }

                var bookEnd = cursor + widthForBook;
                if (x >= cursor && x < bookEnd)
                {
                    bookIndex = index;
                    bookStart = cursor;
                    bookWidth = widthForBook;
                    withinBook = x - cursor;
                    return true;
                }

                cursor = bookEnd;
            }

            return false;
        }

        private static int GetBookSpineWidth(int bookIndex, int rowIndex, int seed, bool bookshelfFront)
        {
            var roll = Hash01(bookIndex, rowIndex, seed + (bookshelfFront ? 37 : 43));
            if (bookshelfFront)
            {
                if (roll < 0.16f) return 5;
                if (roll < 0.30f) return 6;
                if (roll < 0.48f) return 7;
                if (roll < 0.65f) return 8;
                if (roll < 0.78f) return 10;
                if (roll < 0.90f) return 12;
                return 14;
            }

            if (roll < 0.10f) return 5;
            if (roll < 0.24f) return 6;
            if (roll < 0.40f) return 7;
            if (roll < 0.58f) return 8;
            if (roll < 0.72f) return 10;
            if (roll < 0.86f) return 12;
            return 14;
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
                PaintedSurfaceMaterial("current_grass", "current_grass_hd2d_plate", 128, 128, SampleCurrentGrassHd2dPixel, false, new Vector2(6f, 6f)),
                PaintedSurfaceMaterial("current_path", "current_path_hd2d_plate", 128, 128, SampleCurrentPathHd2dPixel, false, new Vector2(4f, 4f)),
                PaintedSurfaceMaterial("current_interior_floor", "current_interior_floor_hd2d_plate", 128, 128, SampleCurrentInteriorFloorHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("current_interior_wall", "current_interior_wall_hd2d_plate", 128, 128, SampleCurrentInteriorWallHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("current_exterior_wall", "current_exterior_wall_hd2d_plate", 128, 128, SampleCurrentExteriorWallHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("current_roof", "current_roof_hd2d_plate", 128, 128, SampleCurrentRoofHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("current_furniture", "current_furniture_hd2d_plate", 128, 128, SampleCurrentFurnitureHd2dPixel, false, new Vector2(2f, 2f)),
                PaintedSurfaceMaterial("current_fence", "current_plank_debris_hd2d_plate", 128, 64, SampleCurrentPlankDebrisHd2dPixel, false, new Vector2(6f, 2f)),
                PaintedSurfaceMaterial("current_house_door_detail", "current_house_door_detail_hd2d_plate", 96, 160, SampleCurrentHouseDoorDetailHd2dPixel, false, new Vector2(1f, 1f)),
                PaintedSurfaceMaterial("current_library_door_detail", "current_library_door_detail_hd2d_plate", 96, 160, SampleCurrentLibraryDoorDetailHd2dPixel, false, new Vector2(1f, 1f)),
                PixelMaterial("current_stone", new Color32(68, 67, 64, 255), new Color32(95, 93, 86, 255), new Color32(43, 43, 41, 255), PixelPattern.Stone, false, new Vector2(3f, 2f)),
                PaintedSurfaceMaterial("current_bed", "current_bed_hd2d_plate", 128, 128, SampleCurrentBedHd2dPixel, false, new Vector2(2f, 2f)),
                PixelMaterial("current_leaf", new Color32(38, 65, 40, 255), new Color32(53, 82, 47, 255), new Color32(28, 45, 32, 255), PixelPattern.Grass, false, new Vector2(3f, 3f)),
                PaintedSurfaceMaterial("past_grass", "past_grass_hd2d_plate", 128, 128, SamplePastGrassHd2dPixel, false, new Vector2(6f, 6f)),
                PaintedSurfaceMaterial("past_path", "past_path_hd2d_plate", 128, 128, SamplePastPathHd2dPixel, false, new Vector2(4f, 4f)),
                PaintedSurfaceMaterial("past_wood_floor", "past_wood_floor_hd2d_plate", 128, 128, SamplePastWoodFloorHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("past_interior_wall", "past_interior_wall_hd2d_plate", 128, 128, SamplePastInteriorWallHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("past_exterior_wall", "past_exterior_wall_hd2d_plate", 128, 128, SamplePastExteriorWallHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("past_roof", "past_roof_hd2d_plate", 128, 128, SamplePastRoofHd2dPixel, false, new Vector2(4f, 3f)),
                PaintedSurfaceMaterial("past_furniture", "past_furniture_hd2d_plate", 128, 128, SamplePastFurnitureHd2dPixel, false, new Vector2(2f, 2f)),
                PaintedSurfaceMaterial("past_fence", "past_plank_hd2d_plate", 128, 64, SamplePastPlankHd2dPixel, false, new Vector2(6f, 2f)),
                PaintedSurfaceMaterial("past_house_door_detail", "past_house_door_detail_hd2d_plate", 96, 160, SamplePastHouseDoorDetailHd2dPixel, false, new Vector2(1f, 1f)),
                PaintedSurfaceMaterial("past_library_door_detail", "past_library_door_detail_hd2d_plate", 96, 160, SamplePastLibraryDoorDetailHd2dPixel, false, new Vector2(1f, 1f)),
                PixelMaterial("past_stone", new Color32(118, 115, 100, 255), new Color32(151, 146, 123, 255), new Color32(83, 82, 75, 255), PixelPattern.Stone, false, new Vector2(3f, 2f)),
                PaintedSurfaceMaterial("past_bed", "past_bed_hd2d_plate", 128, 128, SamplePastBedHd2dPixel, false, new Vector2(2f, 2f)),
                PixelMaterial("leaf", new Color32(62, 122, 64, 255), new Color32(93, 158, 78, 255), new Color32(39, 91, 53, 255), PixelPattern.Grass, false, new Vector2(3f, 3f)),
                PaintedSurfaceMaterial("pillow", "pillow_hd2d_plate", 96, 64, SamplePillowHd2dPixel, false, new Vector2(1f, 1f)),
                PixelMaterial("dust", new Color32(88, 82, 75, 255), new Color32(111, 104, 92, 255), new Color32(61, 57, 54, 255), PixelPattern.Noise, false, new Vector2(2f, 2f)),
                PaintedSurfaceMaterial("current_rubble_detail", "current_rubble_detail_hd2d_plate", 128, 64, SampleCurrentRubbleDetailHd2dPixel, false, new Vector2(1f, 1f)),
                PaintedSurfaceMaterial("book", "book_spines_hd2d_plate", 128, 64, SampleBookSpinesHd2dPixel, false, new Vector2(1f, 1f)),
                PixelMaterial("lamp", new Color32(255, 204, 88, 255), new Color32(255, 236, 150, 255), new Color32(197, 126, 38, 255), PixelPattern.Checker, true, new Vector2(1f, 1f)),
                FlatMaterial("timewindow_cue_yellow_light", new Color(1.00f, 0.86f, 0.20f, 1f), true),
                FlatMaterial("timewindow_marker_yellow", new Color(1.00f, 0.78f, 0.05f, 1f), true),
                PaintedSurfaceMaterial("window_light", "window_light_hd2d_plate", 96, 96, SampleWindowLightHd2dPixel, true, new Vector2(1f, 1f)),
                PaintedSurfaceMaterial("empty_window", "empty_window_hd2d_plate", 96, 96, SampleEmptyWindowHd2dPixel, true, new Vector2(1f, 1f)),
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
            var material = FlatMaterial($"bookshelf_front_painted_hd2d_{panelId}", new Color(0.97f, 0.95f, 0.92f, 1f), true);
            material.name = $"FastVS_House_bookshelf_front_painted_hd2d_{panelId}";
            var texture = EnsureGeneratedRepeatTexture("bookshelf_front_painted_hd2d", 256, 128, SampleBookshelfFrontPaintedHd2dPixel);
            AssignMaterialTexture(material, texture, textureScale);
            return material;
        }

        private static Material CurrentEmptyBookshelfFrontMaterial(string panelId, Vector2 textureScale)
        {
            var material = FlatMaterial($"current_empty_bookshelf_front_hd2d_{panelId}", new Color(0.92f, 0.89f, 0.83f, 1f), true);
            material.name = $"FastVS_House_current_empty_bookshelf_front_hd2d_{panelId}";
            var texture = EnsureGeneratedRepeatTexture("current_empty_bookshelf_front_hd2d", 256, 128, SampleCurrentEmptyBookshelfFrontHd2dPixel);
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
                Material currentHouseDoorDetail,
                Material currentLibraryDoorDetail,
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
                Material pastHouseDoorDetail,
                Material pastLibraryDoorDetail,
                Material pastStone,
                Material pastBed,
                Material leaf,
                Material pillow,
                Material dust,
                Material currentRubbleDetail,
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
                CurrentHouseDoorDetail = currentHouseDoorDetail;
                CurrentLibraryDoorDetail = currentLibraryDoorDetail;
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
                PastHouseDoorDetail = pastHouseDoorDetail;
                PastLibraryDoorDetail = pastLibraryDoorDetail;
                PastStone = pastStone;
                PastBed = pastBed;
                Leaf = leaf;
                Pillow = pillow;
                Dust = dust;
                CurrentRubbleDetail = currentRubbleDetail;
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
            public Material CurrentHouseDoorDetail { get; }
            public Material CurrentLibraryDoorDetail { get; }
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
            public Material PastHouseDoorDetail { get; }
            public Material PastLibraryDoorDetail { get; }
            public Material PastStone { get; }
            public Material PastBed { get; }
            public Material Leaf { get; }
            public Material Pillow { get; }
            public Material Dust { get; }
            public Material CurrentRubbleDetail { get; }
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
