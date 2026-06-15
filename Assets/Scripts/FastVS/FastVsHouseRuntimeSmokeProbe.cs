using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.FastVS
{
    public sealed class FastVsHouseRuntimeSmokeProbe : MonoBehaviour
    {
        private const string EnableArgument = "--anemora-house-slice-smoke";
        private const string CaptureDirectoryArgument = "--anemora-house-slice-capture-dir";
        private const string TableObjectCaptureDirectoryArgument = "--anemora-house-slice-table-object-capture-dir";
        private const string TimeWindowProbeCaptureDirectoryArgument = "--anemora-house-slice-time-window-probe-dir";
        private const string TimeWindowVisualPhysicsDirectoryArgument = "--anemora-house-slice-time-window-visual-physics-dir";
        private const string RendererDiagnosticsDirectoryArgument = "--anemora-house-slice-renderer-diagnostics-dir";
        private const string RendererIsolationDirectoryArgument = "--anemora-house-slice-renderer-isolation-dir";
        private const string RendererFlickerProbeDirectoryArgument = "--anemora-house-slice-renderer-flicker-dir";
        private const string RendererMotionProbeDirectoryArgument = "--anemora-house-slice-renderer-motion-dir";
        private const string LibraryRearCloseProbeDirectoryArgument = "--anemora-house-slice-library-rear-close-dir";
        private const string RendererStaticProbeDirectoryArgument = "--anemora-house-slice-renderer-static-dir";
        private const string WindowDoorReviewDirectoryArgument = "--anemora-house-slice-window-door-review-dir";
        private const string BridgeTraversalProofDirectoryArgument = "--anemora-house-slice-bridge-proof-dir";
        private const string PassMarker = "ANEMORA_HOUSE_SLICE_SMOKE_PASS";
        private const string FailMarker = "ANEMORA_HOUSE_SLICE_SMOKE_FAIL";
        private const string CaptureMarker = "ANEMORA_HOUSE_SLICE_CAPTURE";
        private const string TableObjectCaptureMarker = "ANEMORA_HOUSE_SLICE_TABLE_OBJECT_CAPTURE";
        private const string TimeWindowProbeMarker = "ANEMORA_HOUSE_SLICE_TIME_WINDOW_PROBE";
        private const string TimeWindowVisualPhysicsMarker = "ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS";
        private const string RendererDiagnosticsMarker = "ANEMORA_HOUSE_SLICE_RENDERER_DIAGNOSTICS";
        private const string RendererIsolationMarker = "ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION";
        private const string RendererFlickerProbeMarker = "ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE";
        private const string RendererMotionProbeMarker = "ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE";
        private const string LibraryRearCloseProbeMarker = "ANEMORA_HOUSE_SLICE_LIBRARY_REAR_CLOSE_PROBE";
        private const string WindowDoorReviewMarker = "ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW";
        private const string BridgeTraversalProofMarker = "ANEMORA_HOUSE_SLICE_BRIDGE_TRAVERSAL";
        private const string RendererContractMarker = "ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT";
        private const string LightingStateMarker = "ANEMORA_HOUSE_SLICE_LIGHTING_STATE";
        private const float RuntimeVsFollowCameraFov = 38f;
        private const float Chapter1EndSideViewOrthographicSize = 2.80f;
        private const int WindowDoorPillarRoiIsolationCaptureLimit = 12;
        private static readonly Vector2 TimeWindowDragStart = new Vector2(380f, 215f);
        private static readonly Vector2 TimeWindowDragEnd = new Vector2(850f, 600f);
        private static readonly Vector3 HouseExteriorCenter = new Vector3(8.20f, 0f, 8.20f);
        private static readonly Vector3 CentralPlazaVsCenter = new Vector3(20.80f, 0f, 15.80f);
        private static readonly Vector3 LibraryVsCenter = new Vector3(31.00f, 0f, 20.00f);
        private static readonly Vector3 Chapter1MiaHouseMapCenter = CentralPlazaVsCenter + new Vector3(3.70f, 0f, -1.55f);
        private static readonly Vector3 Chapter1AriaStreetMapCenter = CentralPlazaVsCenter + new Vector3(25.50f, 0f, -1.75f);
        private static readonly Vector3 Chapter1KaiaFarmMapCenter = CentralPlazaVsCenter + new Vector3(32.50f, 0f, -2.85f);
        private static readonly Vector3 Chapter1F1RouteTriggerCenter = CentralPlazaVsCenter + new Vector3(21.00f, 0.70f, 0.15f);
        private static readonly Vector3 Chapter1F6RouteTriggerCenter = CentralPlazaVsCenter + new Vector3(68.10f, 0.70f, 0.15f);
        private static readonly Vector3 Chapter1RuinsMapCenter = CentralPlazaVsCenter + new Vector3(45.50f, 0f, 0.05f);
        private static readonly Vector3 Chapter1F1FromE3Target = Chapter1F1RouteTriggerCenter + new Vector3(-0.08f, -0.68f, -1.70f);
        private static readonly Vector3 Chapter1EndSideViewCenter = CentralPlazaVsCenter + new Vector3(9.10f, 0f, -10.50f);
        private static readonly Vector3 Chapter1EndSideViewCameraAnchor = Chapter1EndSideViewCenter + new Vector3(-1.05f, 1.45f, 0f);
        private static readonly Vector3 Chapter1EndSideViewPreviewTarget = Chapter1EndSideViewCenter + new Vector3(-2.40f, 0.02f, 0f);
        private static readonly Rect WindowDoorPillarRoiViewport = new Rect(0.35f, 0.43f, 0.22f, 0.25f);
        private static readonly Rect RendererRightOuterRoadRoiViewport = new Rect(0.73f, 0.12f, 0.26f, 0.76f);
        private static readonly Rect RendererFarRightLooseRoadRoiViewport = new Rect(0.78f, 0.38f, 0.21f, 0.28f);
        private static readonly Vector2Int[] RendererRightmostDetachedProbePixels =
        {
            new Vector2Int(1076, 230),
            new Vector2Int(1142, 250),
            new Vector2Int(1145, 340),
            new Vector2Int(1220, 315)
        };
        private static readonly string[] RendererRightmostDetachedProbeLabels = { "A", "B", "C", "D" };
        private static readonly Vector2Int[] RendererFarRightStripProbePixels =
        {
            new Vector2Int(1017, 185),
            new Vector2Int(1058, 242),
            new Vector2Int(1113, 322),
            new Vector2Int(1183, 395),
            new Vector2Int(1096, 226),
            new Vector2Int(1156, 292),
            new Vector2Int(1209, 355)
        };
        private static readonly string[] RendererFarRightStripProbeLabels = { "StoneTop", "StoneUpper", "StoneMid", "StoneLower", "WoodTop", "WoodMid", "WoodLower" };
        private static readonly Vector2Int[] RendererAllMapLowerFrontProbePixels =
        {
            new Vector2Int(963, 706),
            new Vector2Int(117, 707),
            new Vector2Int(865, 552),
            new Vector2Int(586, 577)
        };
        private static readonly string[] RendererAllMapLowerFrontProbeLabels = { "BottomRightTile", "BottomLeftTile", "FrontRightDiagonal", "FrontCenterBrick" };
        private static readonly string[] WindowDoorReviewDoorCandidates =
        {
            "Current_CentralPlaza_Chapter1_B2_LibraryFrontMarker",
            "Current_CentralPlaza_LibraryDoorCenterPlank",
            "Current_CentralPlaza_LibraryDoorPanelsLeft",
            "Current_CentralPlaza_LibraryDoorPanelsRight",
            "Current_CentralPlaza_LibraryDoorRelief_LeftRevealStripA",
            "Current_CentralPlaza_LibraryDoorRelief_RightRevealStripA",
            "Current_CentralPlaza_LibraryEntryDepth_DoorRecessShadowA"
        };
        private static readonly string[] WindowDoorReviewLightTokens =
        {
            "DynamicSunShaft",
            "BroadSunshaftReceiver",
            "LightComposition_LibraryDoor",
            "FramedLightPlanes_LibraryDoor",
            "ReferenceLightColumn"
        };
        private static readonly string[] WindowDoorReviewSunbeamTokens =
        {
            "LivePortalAperture",
            "PortalThresholdLine",
            "Current_Frame_",
            "Cycle125_ReferenceDioramaShadow_HighSunbeamColumnA",
            "Cycle125_ReferenceDioramaShadow_CenterChalkSunCatchA",
            "Cycle125_ReferenceDioramaShadow",
            "ReferenceLightColumn",
            "DynamicSunShaft",
            "BroadSunshaftReceiver"
        };
        private static readonly string[] WindowDoorReviewSunbeamExactNames =
        {
            "Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_HighSunbeamColumnA",
            "Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_CenterChalkSunCatchA",
            "Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_BackDepthHazeA",
            "Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_LibraryEaveHardContactA",
            "Current_CentralPlaza_Cycle126_CloseShadowBarMute_GroundAirUnifierA"
        };

        private IEnumerator Start()
        {
            var rendererContract = InspectRendererContract();
            LogRendererContract(rendererContract);

            var captureDirectory = GetArgumentValue(CaptureDirectoryArgument);
            if (!string.IsNullOrEmpty(captureDirectory))
            {
                yield return RunCapture(captureDirectory, rendererContract);
                yield break;
            }

            var tableObjectCaptureDirectory = GetArgumentValue(TableObjectCaptureDirectoryArgument);
            if (!string.IsNullOrEmpty(tableObjectCaptureDirectory))
            {
                yield return RunTableObjectCapture(tableObjectCaptureDirectory, rendererContract);
                yield break;
            }

            var timeWindowProbeDirectory = GetArgumentValue(TimeWindowProbeCaptureDirectoryArgument);
            if (!string.IsNullOrEmpty(timeWindowProbeDirectory))
            {
                yield return RunTimeWindowProbe(timeWindowProbeDirectory, rendererContract);
                yield break;
            }

            var timeWindowVisualPhysicsDirectory = GetArgumentValue(TimeWindowVisualPhysicsDirectoryArgument);
            if (!string.IsNullOrEmpty(timeWindowVisualPhysicsDirectory))
            {
                yield return RunTimeWindowVisualPhysicsProbe(timeWindowVisualPhysicsDirectory, rendererContract);
                yield break;
            }

            var rendererDiagnosticsDirectory = GetArgumentValue(RendererDiagnosticsDirectoryArgument);
            if (!string.IsNullOrEmpty(rendererDiagnosticsDirectory))
            {
                yield return RunRendererDiagnostics(rendererDiagnosticsDirectory, rendererContract);
                yield break;
            }

            var rendererIsolationDirectory = GetArgumentValue(RendererIsolationDirectoryArgument);
            if (!string.IsNullOrEmpty(rendererIsolationDirectory))
            {
                yield return RunRendererIsolation(rendererIsolationDirectory, rendererContract);
                yield break;
            }

            var rendererFlickerProbeDirectory = GetArgumentValue(RendererFlickerProbeDirectoryArgument);
            if (!string.IsNullOrEmpty(rendererFlickerProbeDirectory))
            {
                yield return RunRendererFlickerProbe(rendererFlickerProbeDirectory, rendererContract);
                yield break;
            }

            var rendererMotionProbeDirectory = GetArgumentValue(RendererMotionProbeDirectoryArgument);
            if (!string.IsNullOrEmpty(rendererMotionProbeDirectory))
            {
                yield return RunRendererMotionProbe(rendererMotionProbeDirectory, rendererContract);
                yield break;
            }

            var libraryRearCloseProbeDirectory = GetArgumentValue(LibraryRearCloseProbeDirectoryArgument);
            if (!string.IsNullOrEmpty(libraryRearCloseProbeDirectory))
            {
                yield return RunLibraryRearCloseProbe(libraryRearCloseProbeDirectory, rendererContract);
                yield break;
            }

            var rendererStaticProbeDirectory = GetArgumentValue(RendererStaticProbeDirectoryArgument);
            if (!string.IsNullOrEmpty(rendererStaticProbeDirectory))
            {
                yield return RunRendererStaticProbe(rendererStaticProbeDirectory, rendererContract);
                yield break;
            }

            var windowDoorReviewDirectory = GetArgumentValue(WindowDoorReviewDirectoryArgument);
            if (!string.IsNullOrEmpty(windowDoorReviewDirectory))
            {
                yield return RunWindowDoorReviewProbe(windowDoorReviewDirectory, rendererContract);
                yield break;
            }

            var bridgeTraversalProofDirectory = GetArgumentValue(BridgeTraversalProofDirectoryArgument);
            if (!string.IsNullOrEmpty(bridgeTraversalProofDirectory))
            {
                yield return RunBridgeTraversalProof(bridgeTraversalProofDirectory, rendererContract);
                yield break;
            }

            if (!ShouldRun())
            {
                yield break;
            }

            Debug.Log("ANEMORA_HOUSE_SLICE_SMOKE_ENABLED");
            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            try
            {
                LogSmokeStep("RunChecks.begin");
                RunChecks(rendererContract);
                LogSmokeStep("RunChecks.end");
                Debug.Log($"{PassMarker}: MiaInterior and AriaInterior door travel plus indoor character activation verified.");
                Application.Quit(0);
            }
            catch (Exception exception)
            {
                Debug.LogError($"{FailMarker}: {exception}");
                Application.Quit(31);
            }
        }

        private static bool ShouldRun()
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], EnableArgument, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetArgumentValue(string argumentName)
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (string.Equals(args[i], argumentName, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return string.Empty;
        }

        private static IEnumerator RunCapture(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{CaptureMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);
            LogLightingState("capture.ready", visibility.ActiveAreaForReview);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var count = 0;

            CaptureChapter1AllMapsPair(controller, visibility, guide, camera, FastVsHouseArea.Exterior, HouseExteriorCenter + new Vector3(2.95f, 0.02f, 1.10f), outputDirectory, "01_a1_a2_current.png", "02_a1_a2_past.png", ref count);
            yield return new WaitForSecondsRealtime(0.10f);
            CaptureChapter1AllMapsPair(controller, visibility, guide, camera, FastVsHouseArea.CentralPlaza, CentralPlazaVsCenter + new Vector3(1.45f, 0.02f, -0.20f), outputDirectory, "03_b1_b3_current.png", "04_b1_b3_past.png", ref count);
            PositionChapter1AllMapsCamera(
                camera,
                controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(1.45f, 0.02f, -0.20f)),
                new Vector3(0f, 13.80f, -18.20f),
                new Vector3(0f, 0.20f, 1.55f));
            LogAllMapLowerFrontPixelContributors(camera, "CentralPlaza.current", 180, 80);
            yield return new WaitForSecondsRealtime(0.10f);
            CaptureChapter1AllMapsPair(controller, visibility, guide, camera, FastVsHouseArea.MiaHouse, Chapter1MiaHouseMapCenter + new Vector3(0f, 0.02f, 0f), outputDirectory, "05_c1_c3_current.png", "06_c1_c3_past.png", ref count, new Vector3(0f, 17.90f, -25.20f), new Vector3(0.10f, 0.20f, 2.90f));
            yield return new WaitForSecondsRealtime(0.10f);
            CaptureChapter1AllMapsPair(controller, visibility, guide, camera, FastVsHouseArea.AriaStreet, Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 0f), outputDirectory, "07_d1_d3_current.png", "08_d1_d3_past.png", ref count, new Vector3(0f, 20.35f, -27.80f), new Vector3(0.80f, 0.22f, 4.10f));
            yield return new WaitForSecondsRealtime(0.10f);
            CaptureChapter1AllMapsPair(controller, visibility, guide, camera, FastVsHouseArea.KaiaFarm, Chapter1KaiaFarmMapCenter + new Vector3(0f, 0.02f, 0f), outputDirectory, "09_e1_e3_current.png", "10_e1_e3_past.png", ref count, new Vector3(0f, 20.95f, -28.90f), new Vector3(0.85f, 0.24f, 4.60f));
            yield return new WaitForSecondsRealtime(0.10f);
            CaptureChapter1AllMapsPair(controller, visibility, guide, camera, FastVsHouseArea.Ruins, Chapter1RuinsMapCenter + new Vector3(0f, 0.02f, -0.45f), outputDirectory, "11_f1_f6_current.png", "12_f1_f6_past.png", ref count, new Vector3(-0.08f, 25.35f, -40.30f), new Vector3(0.44f, 0.28f, 5.84f));
            yield return new WaitForSecondsRealtime(0.10f);
            CaptureChapter1EndSideViewPreview(controller, visibility, guide, camera, outputDirectory, "13_scene6_sideview_auto.png", ref count);

            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{CaptureMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunBridgeTraversalProof(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{BridgeTraversalProofMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var player = RequireObject<CharacterController>("player character controller");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var count = 0;
            Exception failure = null;

            try
            {
                controller.ClosePortal();
                controller.SetRuntimeInputEnabledForReview(false);
                RunBridgeTraversalProofSide(controller, visibility, guide, player, camera, outputDirectory, false, previousMask, ref count);
                RunBridgeTraversalProofSide(controller, visibility, guide, player, camera, outputDirectory, true, previousMask, ref count);
            }
            catch (Exception exception)
            {
                failure = exception;
                Debug.LogError($"{BridgeTraversalProofMarker}: fail {exception}");
            }
            finally
            {
                camera.cullingMask = previousMask;
                camera.orthographic = previousOrthographic;
                camera.orthographicSize = previousOrthographicSize;
                camera.fieldOfView = previousFieldOfView;
                controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
                guide.ApplyActiveTimeIsolationForReview();
            }

            if (failure != null)
            {
                Application.Quit(42);
                yield break;
            }

            Debug.Log($"{BridgeTraversalProofMarker}: pass count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunTableObjectCapture(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{TableObjectCaptureMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousMask = camera.cullingMask;
            var count = 0;

            visibility.SetActiveAreaForReview(FastVsHouseArea.Library);
            controller.ForcePlayerOtherTimeLocalForReview(LibraryVsCenter + new Vector3(0f, 0.02f, -0.95f));
            guide.ApplyActiveTimeIsolationForReview();
            camera.cullingMask = BuildOtherTimeCameraMask(previousMask, controller);

            Debug.Log($"{TableObjectCaptureMarker}: status {BuildTableObjectStatus()}");
            CapturePastLibraryTableObject(camera, controller, outputDirectory, "01_past_library_long_table_wide.png", LibraryVsCenter + new Vector3(0f, 0.43f, -0.95f), new Vector3(0f, 1.45f, -2.25f), new Vector3(0f, 0.02f, 0.12f), ref count);
            yield return new WaitForSecondsRealtime(0.10f);
            CapturePastLibraryTableObject(camera, controller, outputDirectory, "02_past_library_long_table_pair_a_close.png", LibraryVsCenter + new Vector3(-1.10f, 0.43f, -0.95f), new Vector3(0.08f, 1.02f, -1.45f), new Vector3(0f, 0.02f, 0.05f), ref count);
            yield return new WaitForSecondsRealtime(0.10f);
            CapturePastLibraryTableObject(camera, controller, outputDirectory, "03_past_library_long_table_pair_b_close.png", LibraryVsCenter + new Vector3(1.08f, 0.43f, -0.95f), new Vector3(-0.08f, 1.02f, -1.45f), new Vector3(0f, 0.02f, 0.05f), ref count);

            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{TableObjectCaptureMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunTimeWindowProbe(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{TimeWindowProbeMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var previousMask = camera.cullingMask;
            var count = 0;

            controller.ClosePortal();
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            var aperturePlayerLocal = CentralPlazaVsCenter + new Vector3(-0.80f, 0.02f, 2.35f);
            controller.ForcePlayerCurrentLocalForReview(aperturePlayerLocal);
            guide.ApplyActiveTimeIsolationForReview();
            camera.orthographic = false;
            camera.fieldOfView = RuntimeVsFollowCameraFov;
            PositionReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(aperturePlayerLocal));

            var opened = controller.TryOpenPortalForTests(TimeWindowDragStart, TimeWindowDragEnd);
            yield return null;
            controller.RenderPortalAperturesForReview();
            guide.ApplyActiveTimeIsolationForReview();
            Debug.Log($"{TimeWindowProbeMarker}: open opened={opened} {BuildTimeWindowProbeState(controller)}");
            if (!opened || !controller.HasPortalPair)
            {
                Debug.LogError($"{TimeWindowProbeMarker}: portal did not open; aborting probe.");
                Application.Quit(41);
                yield break;
            }

            CaptureTimeWindowProbeView(camera, controller, outputDirectory, "01_open_current_front.png", false, ref count);

            var portal = controller.PortalLocalCenterForReview;
            var frontStart = new Vector3(portal.x, 0.72f, portal.z - 0.24f);
            controller.ForcePlayerCurrentLocalForReview(frontStart);
            guide.ApplyActiveTimeIsolationForReview();
            Debug.Log($"{TimeWindowProbeMarker}: front_before {BuildTimeWindowProbeState(controller)}");
            MovePlayerLocalForReview(controller, new Vector3(0f, 0f, 0.62f), false);
            yield return null;
            guide.ApplyActiveTimeIsolationForReview();
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowProbeMarker}: front_after mode=bypassLocal expectedTransfer=True {BuildTimeWindowProbeState(controller)}");
            CaptureTimeWindowProbeView(camera, controller, outputDirectory, "02_front_after.png", controller.PlayerInOtherTime, ref count);

            var blockPlane = portal.z + Mathf.Max(controller.CurrentBackSideBlockDepthForReview, 0.18f + 0.075f);
            var backStart = new Vector3(portal.x, 0.72f, blockPlane + 0.24f);
            controller.ForcePlayerCurrentLocalForReview(backStart);
            guide.ApplyActiveTimeIsolationForReview();
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowProbeMarker}: back_before blockPlane={blockPlane:0.000} {BuildTimeWindowProbeState(controller)}");
            CaptureTimeWindowProbeView(camera, controller, outputDirectory, "03_back_before.png", false, ref count);
            MovePlayerLocalForReview(controller, new Vector3(0f, 0f, -0.82f), false);
            yield return null;
            guide.ApplyActiveTimeIsolationForReview();
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowProbeMarker}: back_after mode=bypassLocal expectedTransfer=False expectedNoSnap=True measuredBlockPlane={blockPlane:0.000} {BuildTimeWindowProbeState(controller)}");
            CaptureTimeWindowProbeView(camera, controller, outputDirectory, "04_back_after.png", controller.PlayerInOtherTime, ref count);

            var otherStart = new Vector3(portal.x, 0.72f, portal.z + 0.24f);
            controller.ForcePlayerOtherTimeLocalForReview(otherStart);
            guide.ApplyActiveTimeIsolationForReview();
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowProbeMarker}: other_return_before {BuildTimeWindowProbeState(controller)}");
            MovePlayerLocalForReview(controller, new Vector3(0f, 0f, -0.62f), false);
            yield return null;
            guide.ApplyActiveTimeIsolationForReview();
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowProbeMarker}: other_return_after mode=bypassLocal expectedReturnCurrent=True {BuildTimeWindowProbeState(controller)}");
            CaptureTimeWindowProbeView(camera, controller, outputDirectory, "05_other_return_after.png", controller.PlayerInOtherTime, ref count);

            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            camera.fieldOfView = previousFieldOfView;
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{TimeWindowProbeMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunTimeWindowVisualPhysicsProbe(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var count = 0;

            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ClosePortal();
            controller.SetRuntimeInputEnabledForReview(false);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.90f, 0.02f, -0.45f));
            guide.ApplyActiveTimeIsolationForReview();

            var opened = controller.TryOpenPortalForTests(TimeWindowDragStart, TimeWindowDragEnd);
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: open opened={opened} {BuildTimeWindowVisualPhysicsState(controller, camera)}");
            if (!opened || !controller.HasPortalPair)
            {
                Debug.LogError($"{TimeWindowVisualPhysicsMarker}: portal did not open; aborting probe.");
                Application.Quit(41);
                yield break;
            }

            var portal = controller.PortalLocalCenterForReview;
            var frontCurrent = new Vector3(portal.x, 0.72f, portal.z - 0.55f);
            var farCurrent = new Vector3(portal.x, 0.72f, portal.z + 0.72f);
            var otherFar = new Vector3(portal.x, 0.72f, portal.z + 0.72f);

            controller.ForcePlayerCurrentLocalForReview(frontCurrent);
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: current_front_before {BuildTimeWindowVisualPhysicsState(controller, camera)}");
            CaptureTimeWindowVisualPhysicsView(camera, controller, outputDirectory, "01_current_front_before.png", false, ref count);

            controller.ForcePlayerCurrentLocalForReview(farCurrent);
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: current_far_side_before {BuildTimeWindowVisualPhysicsState(controller, camera)}");
            CaptureTimeWindowVisualPhysicsView(camera, controller, outputDirectory, "02_current_far_side_before.png", false, ref count);

            MovePlayerLocalForReview(controller, new Vector3(0f, 0f, -1.25f), true);
            yield return null;
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: current_far_to_front_after_cc expectedProbe=currentBackPhysicalBlock {BuildTimeWindowVisualPhysicsState(controller, camera)}");
            CaptureTimeWindowVisualPhysicsView(camera, controller, outputDirectory, "03_current_far_to_front_after_cc.png", false, ref count);

            controller.ForcePlayerCurrentLocalForReview(frontCurrent);
            MovePlayerLocalForReview(controller, new Vector3(0f, 0f, 1.25f), true);
            yield return null;
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: current_front_to_far_after_cc expectedTransferMaybe {BuildTimeWindowVisualPhysicsState(controller, camera)}");
            CaptureTimeWindowVisualPhysicsView(camera, controller, outputDirectory, "04_current_front_to_far_after_cc.png", controller.PlayerInOtherTime, ref count);

            controller.ForcePlayerOtherTimeLocalForReview(otherFar);
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: other_time_far_before {BuildTimeWindowVisualPhysicsState(controller, camera)}");
            CaptureTimeWindowVisualPhysicsView(camera, controller, outputDirectory, "05_other_time_far_before.png", true, ref count);

            MovePlayerLocalForReview(controller, new Vector3(0f, 0f, -1.25f), true);
            yield return null;
            controller.RenderPortalAperturesForReview();
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: other_time_far_to_front_after_cc expectedReturnOrBlock {BuildTimeWindowVisualPhysicsState(controller, camera)}");
            CaptureTimeWindowVisualPhysicsView(camera, controller, outputDirectory, "06_other_time_far_to_front_after_cc.png", controller.PlayerInOtherTime, ref count);

            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            camera.fieldOfView = previousFieldOfView;
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{TimeWindowVisualPhysicsMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunRendererDiagnostics(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{RendererDiagnosticsMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var count = 0;

            controller.ClosePortal();
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.65f, 0.02f, 0.35f));
            guide.ApplyActiveTimeIsolationForReview();
            PositionChapter1AllMapsCamera(
                camera,
                controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(1.45f, 0.02f, -0.20f)),
                new Vector3(0f, 13.80f, -18.20f),
                new Vector3(0f, 0.20f, 1.55f));
            WriteCameraPng(camera, Path.Combine(outputDirectory, "01_current_plaza_library_facade.png"));
            count++;

            camera.cullingMask = BuildOtherTimeCameraMask(previousMask, controller);
            controller.ForcePlayerOtherTimeLocalForReview(LibraryVsCenter + new Vector3(0f, 0.02f, -0.95f));
            guide.ApplyActiveTimeIsolationForReview();
            PositionChapter1AllMapsCamera(
                camera,
                controller.OtherTimeSpaceRootForReview.TransformPoint(LibraryVsCenter + new Vector3(0f, 0.02f, -0.95f)),
                new Vector3(0f, 13.80f, -18.20f),
                new Vector3(0f, 0.20f, 1.55f));
            WriteCameraPng(camera, Path.Combine(outputDirectory, "02_past_library_facade_long_road.png"));
            count++;

            camera.cullingMask = previousMask;
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.65f, 0.02f, 0.35f));
            guide.ApplyActiveTimeIsolationForReview();
            CaptureRendererDiagnosticsClose(camera, controller, outputDirectory, "03_current_library_facade_close.png", ref count);

            LogRendererMaterialDiagnostics();
            LogRendererOverlapDiagnostics();
            yield return SampleRendererVisibilityDiagnostics(camera, 120);

            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            camera.fieldOfView = previousFieldOfView;
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{RendererDiagnosticsMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunRendererIsolation(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{RendererIsolationMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var count = 0;

            controller.ClosePortal();
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.65f, 0.02f, 0.35f));
            guide.ApplyActiveTimeIsolationForReview();

            var baselineWide = CaptureRendererIsolationWide(camera, controller, outputDirectory, "01_baseline_current_plaza_library_facade.png", ref count);
            var baselineClose = CaptureRendererIsolationClose(camera, controller, outputDirectory, "02_baseline_current_library_facade_close.png", ref count);
            PositionRendererIsolationWideCamera(camera, controller);
            LogRendererIsolationRightOuterRoadRoiCandidates(camera, 160);
            LogRendererIsolationRoiCandidates(camera, "farRightLooseRoadRoi", RendererFarRightLooseRoadRoiViewport, 160);
            LogRightmostDetachedPixelContributors(camera, "baseline", 80, 60);
            LogRightmostDetachedPixelContributorsWithPreset(
                camera,
                "afterRightRuinBlockOff",
                IsCentralPlazaRightRuinBlockIsolationTarget,
                80,
                60);
            LogFarRightStripPixelContributors(camera, "baseline", 220, 80);

            CaptureRendererIsolationVariant(
                "transparentOverlayOff",
                IsTransparentOverlayIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "03_no_transparent_overlay_current_plaza_library_facade.png",
                "04_no_transparent_overlay_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "longRoadStackOff",
                IsLongRoadIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "05_no_long_road_stack_current_plaza_library_facade.png",
                "06_no_long_road_stack_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "pastCentralPlazaLongRoadOff",
                IsPastCentralPlazaLongRoadIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "07_no_past_long_road_current_plaza_library_facade.png",
                "08_no_past_long_road_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "backgroundSkyDepthOff",
                IsCentralPlazaBackgroundSkyDepthIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "09_no_background_sky_depth_current_plaza_library_facade.png",
                "10_no_background_sky_depth_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "outdoorSkyDetailOff",
                IsCentralPlazaOutdoorSkyDetailIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "11_no_outdoor_sky_detail_current_plaza_library_facade.png",
                "12_no_outdoor_sky_detail_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "scenicBackdropOff",
                IsCentralPlazaScenicBackdropIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "13_no_scenic_backdrop_current_plaza_library_facade.png",
                "14_no_scenic_backdrop_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "roadGeometryOff",
                IsCentralPlazaRoadGeometryIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "15_no_road_geometry_current_plaza_library_facade.png",
                "16_no_road_geometry_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "backgroundEnvelopeOff",
                IsCentralPlazaBackgroundEnvelopeIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "17_no_background_envelope_current_plaza_library_facade.png",
                "18_no_background_envelope_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "rightOuterRoadShoulderOff",
                IsCentralPlazaRightOuterRoadShoulderIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "51_no_right_outer_road_shoulder_current_plaza_library_facade.png",
                "52_no_right_outer_road_shoulder_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "eastPerimeterFieldOff",
                IsCentralPlazaEastPerimeterFieldIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "53_no_east_perimeter_field_current_plaza_library_facade.png",
                "54_no_east_perimeter_field_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "perimeterBackFieldOff",
                IsCentralPlazaPerimeterBackFieldIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "79_no_perimeter_back_field_current_plaza_library_facade.png",
                "80_no_perimeter_back_field_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "perimeterBackRidgeOff",
                IsCentralPlazaPerimeterBackRidgeIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "81_no_perimeter_back_ridge_current_plaza_library_facade.png",
                "82_no_perimeter_back_ridge_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "perimeterBackFieldRidgeOff",
                IsCentralPlazaPerimeterBackFieldRidgeIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "83_no_perimeter_back_field_ridge_current_plaza_library_facade.png",
                "84_no_perimeter_back_field_ridge_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "backPathBandsOff",
                IsCentralPlazaBackPathBandsIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "55_no_back_path_bands_current_plaza_library_facade.png",
                "56_no_back_path_bands_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "horizonRightDepthOff",
                IsCentralPlazaHorizonRightDepthIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "57_no_horizon_right_depth_current_plaza_library_facade.png",
                "58_no_horizon_right_depth_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "rightRuinBlockOff",
                IsCentralPlazaRightRuinBlockIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "59_no_right_ruin_block_current_plaza_library_facade.png",
                "60_no_right_ruin_block_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "rightLibrarySideDepthStackOff",
                IsCentralPlazaRightLibrarySideDepthStackIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "85_no_right_library_side_depth_stack_current_plaza_library_facade.png",
                "86_no_right_library_side_depth_stack_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "rightLotHardscapeOff",
                IsCentralPlazaRightLotHardscapeIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "61_no_right_lot_hardscape_current_plaza_library_facade.png",
                "62_no_right_lot_hardscape_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "rightLotPlantingOff",
                IsCentralPlazaRightLotPlantingIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "63_no_right_lot_planting_current_plaza_library_facade.png",
                "64_no_right_lot_planting_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "rightOuterCycle4347Off",
                IsCentralPlazaRightOuterCycle4347IsolationTarget,
                camera,
                controller,
                outputDirectory,
                "65_no_right_outer_cycle43_47_current_plaza_library_facade.png",
                "66_no_right_outer_cycle43_47_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "rightOuterCycle62Off",
                IsCentralPlazaRightOuterCycle62IsolationTarget,
                camera,
                controller,
                outputDirectory,
                "67_no_right_outer_cycle62_current_plaza_library_facade.png",
                "68_no_right_outer_cycle62_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "outdoorWorldEnvelopeRightOff",
                IsCentralPlazaOutdoorWorldEnvelopeRightIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "69_no_outdoor_world_envelope_right_current_plaza_library_facade.png",
                "70_no_outdoor_world_envelope_right_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "outdoorWorldRearSideRidgeRightOff",
                IsCentralPlazaOutdoorWorldRearSideRidgeRightIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "71_no_outdoor_world_rear_side_ridge_right_current_plaza_library_facade.png",
                "72_no_outdoor_world_rear_side_ridge_right_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle63ScenicHorizonEastOff",
                IsCentralPlazaCycle63ScenicHorizonEastIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "73_no_cycle63_scenic_horizon_east_current_plaza_library_facade.png",
                "74_no_cycle63_scenic_horizon_east_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "outdoorVoidNorthSilhouetteRightOff",
                IsCentralPlazaOutdoorVoidNorthSilhouetteRightIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "75_no_outdoor_void_north_silhouette_right_current_plaza_library_facade.png",
                "76_no_outdoor_void_north_silhouette_right_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "farRightLooseDepthComboOff",
                IsCentralPlazaFarRightLooseDepthComboIsolationTarget,
                camera,
                controller,
                outputDirectory,
                "77_no_far_right_loose_depth_combo_current_plaza_library_facade.png",
                "78_no_far_right_loose_depth_combo_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "transparentCycle120Off",
                IsTransparentCycle120IsolationTarget,
                camera,
                controller,
                outputDirectory,
                "19_no_cycle120_reference_light_column_current_plaza_library_facade.png",
                "20_no_cycle120_reference_light_column_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "transparentCycle125Off",
                IsTransparentCycle125IsolationTarget,
                camera,
                controller,
                outputDirectory,
                "21_no_cycle125_reference_diorama_shadow_current_plaza_library_facade.png",
                "22_no_cycle125_reference_diorama_shadow_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "transparentCycle126Off",
                IsTransparentCycle126IsolationTarget,
                camera,
                controller,
                outputDirectory,
                "23_no_cycle126_close_shadow_bar_mute_current_plaza_library_facade.png",
                "24_no_cycle126_close_shadow_bar_mute_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "transparentShadowFoundation70Off",
                IsTransparentShadowFoundation70IsolationTarget,
                camera,
                controller,
                outputDirectory,
                "25_no_shadow_foundation_cycle70_current_plaza_library_facade.png",
                "26_no_shadow_foundation_cycle70_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125LibraryEaveHardContactOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "LibraryEaveHardContactA"),
                camera,
                controller,
                outputDirectory,
                "27_no_cycle125_library_eave_hard_contact_current_plaza_library_facade.png",
                "28_no_cycle125_library_eave_hard_contact_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125CenterChalkSunCatchOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "CenterChalkSunCatchA"),
                camera,
                controller,
                outputDirectory,
                "29_no_cycle125_center_chalk_sun_catch_current_plaza_library_facade.png",
                "30_no_cycle125_center_chalk_sun_catch_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125RightCrateProjectedCastOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "RightCrateProjectedCastA"),
                camera,
                controller,
                outputDirectory,
                "31_no_cycle125_right_crate_projected_cast_current_plaza_library_facade.png",
                "32_no_cycle125_right_crate_projected_cast_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125LeftCanopyDappleGroundOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "LeftCanopyDappleGroundA"),
                camera,
                controller,
                outputDirectory,
                "33_no_cycle125_left_canopy_dapple_ground_current_plaza_library_facade.png",
                "34_no_cycle125_left_canopy_dapple_ground_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125BackDepthHazeOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "BackDepthHazeA"),
                camera,
                controller,
                outputDirectory,
                "35_no_cycle125_back_depth_haze_current_plaza_library_facade.png",
                "36_no_cycle125_back_depth_haze_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125HighSunbeamColumnOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "HighSunbeamColumnA"),
                camera,
                controller,
                outputDirectory,
                "37_no_cycle125_high_sunbeam_column_current_plaza_library_facade.png",
                "38_no_cycle125_high_sunbeam_column_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125PlayerTinyContactOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "PlayerTinyContactA"),
                camera,
                controller,
                outputDirectory,
                "39_no_cycle125_player_tiny_contact_current_plaza_library_facade.png",
                "40_no_cycle125_player_tiny_contact_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125ReferenceReceiverLiftOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "ReferenceReceiverLiftA"),
                camera,
                controller,
                outputDirectory,
                "41_no_cycle125_reference_receiver_lift_current_plaza_library_facade.png",
                "42_no_cycle125_reference_receiver_lift_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125StoneSunMatteFieldOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "StoneSunMatteFieldA"),
                camera,
                controller,
                outputDirectory,
                "43_no_cycle125_stone_sun_matte_field_current_plaza_library_facade.png",
                "44_no_cycle125_stone_sun_matte_field_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125CloseSeamSunMuteOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "CloseSeamSunMuteA"),
                camera,
                controller,
                outputDirectory,
                "45_no_cycle125_close_seam_sun_mute_current_plaza_library_facade.png",
                "46_no_cycle125_close_seam_sun_mute_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125BackStepPaleSunOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "BackStepPaleSunA"),
                camera,
                controller,
                outputDirectory,
                "47_no_cycle125_back_step_pale_sun_current_plaza_library_facade.png",
                "48_no_cycle125_back_step_pale_sun_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            CaptureRendererIsolationVariant(
                "cycle125FacadeReferenceSunPatchOff",
                path => IsTransparentCycle125ObjectIsolationTarget(path, "FacadeReferenceSunPatchA"),
                camera,
                controller,
                outputDirectory,
                "49_no_cycle125_facade_reference_sun_patch_current_plaza_library_facade.png",
                "50_no_cycle125_facade_reference_sun_patch_current_library_facade_close.png",
                baselineWide,
                baselineClose,
                ref count);

            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            camera.fieldOfView = previousFieldOfView;
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{RendererIsolationMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunRendererFlickerProbe(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{RendererFlickerProbeMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var count = 0;

            controller.ClosePortal();
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.65f, 0.02f, 0.35f));
            guide.ApplyActiveTimeIsolationForReview();
            camera.cullingMask = BuildCurrentTimeCameraMask(previousMask, controller);
            LogLightingState("rendererFlickerProbe.ready", FastVsHouseArea.CentralPlaza);

            PositionChapter1AllMapsCamera(
                camera,
                controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(1.45f, 0.02f, -0.20f)),
                new Vector3(0f, 13.80f, -18.20f),
                new Vector3(0f, 0.20f, 1.55f));
            WriteCameraPng(camera, Path.Combine(outputDirectory, "00_context_current_plaza_library_facade.png"));
            count++;
            Debug.Log($"{RendererFlickerProbeMarker}: saved path={Path.Combine(outputDirectory, "00_context_current_plaza_library_facade.png")}");
            PositionRendererFacadeCloseCamera(camera, controller);

            const int sampleFrames = 24;
            var stats = BuildRendererFlickerStats();
            var visibleCounts = new int[sampleFrames];
            var enabledCounts = new int[sampleFrames];
            var visibleHashChanges = 0;
            var imageMeanAbs = new float[Mathf.Max(sampleFrames - 1, 1)];
            var imageChangedPct = new float[Mathf.Max(sampleFrames - 1, 1)];
            var deltaTimes = new float[sampleFrames];
            var unscaledDeltaTimes = new float[sampleFrames];
            uint previousVisibleHash = 0;
            var previousVisiblePaths = new List<string>();
            FramePixelSample previousImage = null;

            for (var frame = 0; frame < sampleFrames; frame++)
            {
                yield return null;
                PositionRendererFacadeCloseCamera(camera, controller);

                var fileName = $"{frame + 1:00}_close_frame_{frame + 1:00}.png";
                var image = WriteCameraPngAndSample(camera, Path.Combine(outputDirectory, fileName), 8);
                count++;

                var visiblePaths = new List<string>();
                var enabledCount = 0;
                foreach (var item in stats.Values)
                {
                    item.Sample(frame);
                    if (item.LastEnabled)
                    {
                        enabledCount++;
                    }

                    if (item.LastVisible)
                    {
                        visiblePaths.Add(item.Path);
                    }
                }

                visiblePaths.Sort(StringComparer.Ordinal);
                var visibleHash = BuildStableStringHash(visiblePaths);
                if (frame > 0 && visibleHash != previousVisibleHash)
                {
                    visibleHashChanges++;
                    LogVisiblePathDelta(frame, previousVisiblePaths, visiblePaths);
                }

                var deltaSummary = frame == 0
                    ? FrameImageDelta.Empty
                    : CompareFrameSamples(previousImage, image);
                if (frame > 0)
                {
                    imageMeanAbs[frame - 1] = deltaSummary.MeanAbsRgb;
                    imageChangedPct[frame - 1] = deltaSummary.ChangedSamplePct;
                }

                deltaTimes[frame] = Time.deltaTime;
                unscaledDeltaTimes[frame] = Time.unscaledDeltaTime;
                visibleCounts[frame] = visiblePaths.Count;
                enabledCounts[frame] = enabledCount;

                Debug.Log(
                    $"{RendererFlickerProbeMarker}: frame index={frame} saved={fileName} " +
                    $"deltaTime={Time.deltaTime:0.0000} unscaledDeltaTime={Time.unscaledDeltaTime:0.0000} realtime={Time.realtimeSinceStartup:0.000} " +
                    $"visible={visiblePaths.Count} enabled={enabledCount} visibleHash=0x{visibleHash:X8} " +
                    $"imageMeanAbsRgb={deltaSummary.MeanAbsRgb:0.000} imageChangedSamplePct={deltaSummary.ChangedSamplePct:0.000} " +
                    $"cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)} fov={camera.fieldOfView:0.000} mask={camera.cullingMask}");

                previousVisibleHash = visibleHash;
                previousVisiblePaths = visiblePaths;
                previousImage = image;
            }

            LogRendererFlickerToggleSummary(stats, sampleFrames, "captureFrames");
            Debug.Log(
                $"{RendererFlickerProbeMarker}: summary phase=captureFrames frames={sampleFrames} saved={count} tracked={stats.Count} " +
                $"visible={BuildSeriesSummary(visibleCounts)} enabled={BuildSeriesSummary(enabledCounts)} " +
                $"visibleHashChanges={visibleHashChanges} imageMeanAbsRgb={BuildSeriesSummary(imageMeanAbs)} imageChangedSamplePct={BuildSeriesSummary(imageChangedPct)} " +
                $"deltaTime={BuildSeriesSummary(deltaTimes)} unscaledDeltaTime={BuildSeriesSummary(unscaledDeltaTimes)}");

            yield return SampleRendererFlickerRuntime(camera, 180, "runtimeWarmRenderNoSave");

            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            camera.fieldOfView = previousFieldOfView;
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{RendererFlickerProbeMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunRendererMotionProbe(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{RendererMotionProbeMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var count = 0;

            controller.ClosePortal();
            controller.SetRuntimeInputEnabledForReview(false);
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            camera.cullingMask = BuildCurrentTimeCameraMask(previousMask, controller);
            LogLightingState("rendererMotionProbe.ready", FastVsHouseArea.CentralPlaza);
            LogRendererMotionBackgroundDiagnosticTargets();
            var hiddenDynamicRenderers = DisableRenderersForIsolation(
                "motionDynamicCharactersHidden",
                IsRendererMotionDynamicCharacterPath);

            const int frameCount = 180;
            const int captureInterval = 15;
            var stats = BuildRendererFlickerStats();
            var visibleCounts = new int[frameCount];
            var enabledCounts = new int[frameCount];
            var backgroundVisibleCounts = new int[frameCount];
            var visibleHashChanges = 0;
            var backgroundHashChanges = 0;
            var deltaTimes = new float[frameCount];
            var unscaledDeltaTimes = new float[frameCount];
            var imageMeanAbs = new List<float>();
            var imageChangedPct = new List<float>();
            uint previousVisibleHash = 0;
            uint previousBackgroundHash = 0;
            var previousVisiblePaths = new List<string>();
            var baselineCapturedFrames = new Dictionary<int, FramePixelSample>();
            var baselineCameraPositions = new Dictionary<int, Vector3>();
            var baselineCameraRotations = new Dictionary<int, Quaternion>();
            var baselinePlayerLocals = new Dictionary<int, Vector3>();
            FramePixelSample previousImage = null;

            for (var frame = 0; frame < frameCount; frame++)
            {
                var t = frameCount <= 1 ? 1f : frame / (float)(frameCount - 1);
                var playerLocal = EvaluateRendererMotionProbeLocal(t);
                PositionRendererMotionProbeCamera(camera, controller, guide, playerLocal);
                yield return null;
                RenderCameraForVisibilitySample(camera);

                var visiblePaths = new List<string>();
                var backgroundPaths = new List<string>();
                var enabledCount = 0;
                foreach (var item in stats.Values)
                {
                    item.Sample(frame);
                    if (item.LastEnabled)
                    {
                        enabledCount++;
                    }

                    if (!item.LastVisible)
                    {
                        continue;
                    }

                    visiblePaths.Add(item.Path);
                    if (IsRendererMotionBackgroundPath(item.Path))
                    {
                        backgroundPaths.Add(item.Path);
                    }
                }

                visiblePaths.Sort(StringComparer.Ordinal);
                backgroundPaths.Sort(StringComparer.Ordinal);
                var visibleHash = BuildStableStringHash(visiblePaths);
                var backgroundHash = BuildStableStringHash(backgroundPaths);
                if (frame > 0 && visibleHash != previousVisibleHash)
                {
                    visibleHashChanges++;
                    Debug.Log($"{RendererMotionProbeMarker}: visibleDelta frame={frame}");
                    LogVisiblePathDelta(frame, previousVisiblePaths, visiblePaths);
                }

                if (frame > 0 && backgroundHash != previousBackgroundHash)
                {
                    backgroundHashChanges++;
                    Debug.Log(
                        $"{RendererMotionProbeMarker}: backgroundVisibleDelta frame={frame} " +
                        $"previous={previousBackgroundHash:X8} current={backgroundHash:X8} " +
                        $"currentSample=[{BuildPathSample(backgroundPaths, 10)}]");
                }

                var shouldCapture = frame == 0 || frame == frameCount - 1 || frame % captureInterval == 0;
                if (shouldCapture)
                {
                    var fileName = $"{count:00}_motion_frame_{frame:000}.png";
                    var image = WriteCameraPngAndSample(camera, Path.Combine(outputDirectory, fileName), 8);
                    var deltaSummary = previousImage == null ? FrameImageDelta.Empty : CompareFrameSamples(previousImage, image);
                    if (previousImage != null)
                    {
                        imageMeanAbs.Add(deltaSummary.MeanAbsRgb);
                        imageChangedPct.Add(deltaSummary.ChangedSamplePct);
                    }

                    previousImage = image;
                    baselineCapturedFrames[frame] = image;
                    baselineCameraPositions[frame] = camera.transform.position;
                    baselineCameraRotations[frame] = camera.transform.rotation;
                    baselinePlayerLocals[frame] = playerLocal;
                    count++;
                    Debug.Log(
                        $"{RendererMotionProbeMarker}: capture frame={frame} saved={fileName} " +
                        $"imageMeanAbsRgb={deltaSummary.MeanAbsRgb:0.000} imageChangedSamplePct={deltaSummary.ChangedSamplePct:0.000} " +
                        $"playerLocal={FormatVector(playerLocal)} cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)}");
                }

                visibleCounts[frame] = visiblePaths.Count;
                enabledCounts[frame] = enabledCount;
                backgroundVisibleCounts[frame] = backgroundPaths.Count;
                deltaTimes[frame] = Time.deltaTime;
                unscaledDeltaTimes[frame] = Time.unscaledDeltaTime;

                if (frame == 0 || frame == frameCount - 1 || frame % 30 == 0)
                {
                    Debug.Log(
                        $"{RendererMotionProbeMarker}: frame index={frame} deltaTime={Time.deltaTime:0.0000} unscaledDeltaTime={Time.unscaledDeltaTime:0.0000} realtime={Time.realtimeSinceStartup:0.000} " +
                        $"visible={visiblePaths.Count} enabled={enabledCount} backgroundVisible={backgroundPaths.Count} visibleHash=0x{visibleHash:X8} backgroundHash=0x{backgroundHash:X8} " +
                        $"playerLocal={FormatVector(playerLocal)} cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)} fov={camera.fieldOfView:0.000} mask={camera.cullingMask}");
                }

                previousVisibleHash = visibleHash;
                previousBackgroundHash = backgroundHash;
                previousVisiblePaths = visiblePaths;
            }

            LogRendererMotionToggleSummary(stats, frameCount, "motionFollowCapture");
            Debug.Log(
                $"{RendererMotionProbeMarker}: summary phase=motionFollowCapture frames={frameCount} saved={count} tracked={stats.Count} " +
                $"visible={BuildSeriesSummary(visibleCounts)} enabled={BuildSeriesSummary(enabledCounts)} backgroundVisible={BuildSeriesSummary(backgroundVisibleCounts)} " +
                $"visibleHashChanges={visibleHashChanges} backgroundHashChanges={backgroundHashChanges} " +
                $"imageMeanAbsRgb={BuildSeriesSummary(imageMeanAbs.ToArray())} imageChangedSamplePct={BuildSeriesSummary(imageChangedPct.ToArray())} " +
                $"deltaTime={BuildSeriesSummary(deltaTimes)} unscaledDeltaTime={BuildSeriesSummary(unscaledDeltaTimes)}");

            CaptureRendererMotionIsolationVariant(
                "outdoorVoidBackgroundEastEdgeWashOff",
                IsCurrentCentralPlazaOutdoorVoidBackgroundEastEdgeWashPath,
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "outdoorVoidBackgroundNorthSilhouettesOff",
                IsCurrentCentralPlazaOutdoorVoidBackgroundNorthSilhouettePath,
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "outdoorVoidBackgroundAllCurrentCentralPlazaOff",
                IsCurrentCentralPlazaOutdoorVoidBackgroundPath,
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "frontRoadLongThinGroundOff",
                IsCurrentCentralPlazaFrontRoadLongThinGroundPath,
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryTransparentDepthPlanesOff",
                IsCurrentCentralPlazaLibraryTransparentDepthPlanePath,
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRearThinOpaquesOff",
                IsCurrentCentralPlazaLibraryRearThinOpaquePath,
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_Cycle60EastMidStoneCourseAOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseA"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_Cycle60WestMidStoneCourseAOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseA"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_Cycle60EastMidStoneCourseBOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseB"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_Cycle60WestMidStoneCourseBOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseB"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_SideSurfaceRearWallBandOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibrarySideSurfaceBreakup_RearWallBandA"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_RoofSideRearWallBandOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibraryRoofSideDepth_RearWallBandA"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_EntryRoofLipUndersideOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibraryEntryDepth_RoofLipUndersideShadowA"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_FrontUnderEaveDepthLineOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibraryFrontDepth_UnderEaveDepthLineA"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_FacadeRoofUnderThinBandOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            CaptureRendererMotionIsolationVariant(
                "libraryRear_ArchitectureUpperBrowOff",
                path => IsExactRendererNamePath(path, "Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA"),
                camera,
                controller,
                guide,
                outputDirectory,
                baselineCapturedFrames,
                baselineCameraPositions,
                baselineCameraRotations,
                baselinePlayerLocals,
                ref count);

            yield return SampleRendererMotionRuntime(camera, controller, guide, 180, "motionFollowRenderNoSave");

            RestoreRenderersForIsolation("motionDynamicCharactersHidden", hiddenDynamicRenderers);
            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            camera.fieldOfView = previousFieldOfView;
            controller.SetRuntimeInputEnabledForReview(true);
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{RendererMotionProbeMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunLibraryRearCloseProbe(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{LibraryRearCloseProbeMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var previousPosition = camera.transform.position;
            var previousRotation = camera.transform.rotation;
            var count = 0;

            var hiddenDynamicRenderers = new List<RendererEnabledState>();
            try
            {
                controller.ClosePortal();
                controller.SetRuntimeInputEnabledForReview(false);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                camera.cullingMask = BuildCurrentTimeCameraMask(previousMask, controller);
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.15f, 0.02f, 6.25f));
                guide.ApplyActiveTimeIsolationForReview();
                LogLightingState("libraryRearClose.ready", FastVsHouseArea.CentralPlaza);

                hiddenDynamicRenderers = DisableRenderersForIsolation(
                    "libraryRearCloseDynamicCharactersHidden",
                    IsRendererMotionDynamicCharacterPath);

                var views = BuildLibraryRearCloseViews();
                var baselineSamples = new Dictionary<string, FramePixelSample>();
                for (var index = 0; index < views.Length; index++)
                {
                    var view = views[index];
                    var fileName = $"{count:00}_baseline_{view.Label}.png";
                    var sample = CaptureLibraryRearCloseView(camera, controller, view, outputDirectory, fileName);
                    baselineSamples[view.Label] = sample;
                    count++;
                    Debug.Log(
                        $"{LibraryRearCloseProbeMarker}: baseline view={view.Label} saved={fileName} " +
                        $"playerLocal={FormatVector(controller.GetPlayerLocalCoordinateForReview())} " +
                        $"cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)} fov={camera.fieldOfView:0.000}");
                }

                var variants = BuildLibraryRearCloseVariants();
                for (var variantIndex = 0; variantIndex < variants.Length; variantIndex++)
                {
                    var variant = variants[variantIndex];
                    LogLibraryRearCloseCandidateDetails(variant.Label, variant.ShouldDisable, 24);
                    var disabledRenderers = DisableRenderersForIsolation(variant.Label, variant.ShouldDisable);
                    try
                    {
                        for (var viewIndex = 0; viewIndex < views.Length; viewIndex++)
                        {
                            var view = views[viewIndex];
                            var fileName = $"{count:00}_{variant.Label}_{view.Label}.png";
                            var sample = CaptureLibraryRearCloseView(camera, controller, view, outputDirectory, fileName);
                            var baseline = baselineSamples.TryGetValue(view.Label, out var capturedBaseline)
                                ? capturedBaseline
                                : null;
                            var delta = CompareFrameSamples(baseline, sample);
                            count++;
                            Debug.Log(
                                $"{LibraryRearCloseProbeMarker}: variant={variant.Label} view={view.Label} saved={fileName} " +
                                $"disabled={disabledRenderers.Count} baselineDelta meanAbsRgb={delta.MeanAbsRgb:0.000} " +
                                $"changedSamplePct={delta.ChangedSamplePct:0.000} changed={delta.ChangedSamples} samples={delta.TotalSamples} " +
                                $"cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)}");
                        }
                    }
                    finally
                    {
                        RestoreRenderersForIsolation(variant.Label, disabledRenderers);
                    }
                }
            }
            finally
            {
                RestoreRenderersForIsolation("libraryRearCloseDynamicCharactersHidden", hiddenDynamicRenderers);
                camera.cullingMask = previousMask;
                camera.orthographic = previousOrthographic;
                camera.orthographicSize = previousOrthographicSize;
                camera.fieldOfView = previousFieldOfView;
                camera.transform.SetPositionAndRotation(previousPosition, previousRotation);
                controller.SetRuntimeInputEnabledForReview(true);
                controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
                guide.ApplyActiveTimeIsolationForReview();
            }

            Debug.Log($"{LibraryRearCloseProbeMarker}: end count={count}");
            Application.Quit(0);
        }

        private static IEnumerator RunRendererStaticProbe(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{RendererMotionProbeMarker}: staticBegin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var captureCount = new[] { 0 };

            controller.ClosePortal();
            controller.SetRuntimeInputEnabledForReview(false);
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            camera.cullingMask = BuildCurrentTimeCameraMask(previousMask, controller);
            LogLightingState("rendererStaticProbe.ready", FastVsHouseArea.CentralPlaza);
            LogRendererMotionBackgroundDiagnosticTargets();
            var hiddenDynamicRenderers = DisableRenderersForIsolation(
                "staticDynamicCharactersHidden",
                IsRendererMotionDynamicCharacterPath);

            yield return SampleRendererStaticRuntime(
                camera,
                controller,
                guide,
                outputDirectory,
                "static_start_frame000",
                EvaluateRendererMotionProbeLocal(0f),
                180,
                15,
                captureCount);

            yield return SampleRendererStaticRuntime(
                camera,
                controller,
                guide,
                outputDirectory,
                "static_mid_frame090",
                EvaluateRendererMotionProbeLocal(90f / 179f),
                180,
                15,
                captureCount);

            yield return SampleRendererStaticRuntime(
                camera,
                controller,
                guide,
                outputDirectory,
                "static_end_frame179",
                EvaluateRendererMotionProbeLocal(1f),
                180,
                15,
                captureCount);

            RestoreRenderersForIsolation("staticDynamicCharactersHidden", hiddenDynamicRenderers);
            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            camera.fieldOfView = previousFieldOfView;
            controller.SetRuntimeInputEnabledForReview(true);
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{RendererMotionProbeMarker}: staticEnd count={captureCount[0]}");
            Application.Quit(0);
        }

        private static IEnumerator RunWindowDoorReviewProbe(string outputDirectory, RendererContractSnapshot rendererContract)
        {
            Debug.Log($"{WindowDoorReviewMarker}: begin outputDirectory={outputDirectory}");
            Directory.CreateDirectory(outputDirectory);
            VerifyRendererContract(rendererContract);

            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            var guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            yield return null;
            yield return new WaitForSecondsRealtime(0.5f);

            foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
            {
                File.Delete(existingPng);
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            var previousFieldOfView = camera.fieldOfView;
            var count = 0;

            controller.ClosePortal();
            controller.SetRuntimeInputEnabledForReview(false);
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.65f, 0.02f, 0.35f));
            guide.ApplyActiveTimeIsolationForReview();
            camera.cullingMask = BuildCurrentTimeCameraMask(previousMask, controller);
            PositionWindowDoorReviewCamera(camera, controller);
            LogLightingState("windowDoorReview.closed.ready", FastVsHouseArea.CentralPlaza);
            LogWindowDoorReviewState("closed.ready", controller, camera);

            var closedDoorBaseline = CaptureWindowDoorReview(camera, outputDirectory, "01_closed_door_light_baseline.png", ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            var pillarRoiCandidates = LogWindowDoorPillarRoiCandidates("closed.baseline", camera, 48);
            CaptureWindowDoorReviewRoiCandidateVariants(pillarRoiCandidates, camera, controller, outputDirectory, ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            CaptureWindowDoorReviewIsolationVariant(
                "libraryFrontMarkerOff",
                path => path.IndexOf("Current_CentralPlaza_Chapter1_B2_LibraryFrontMarker", StringComparison.OrdinalIgnoreCase) >= 0,
                camera,
                controller,
                outputDirectory,
                "02_no_library_front_marker.png",
                ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            CaptureWindowDoorReviewIsolationVariant(
                "centerPlankOff",
                path => path.IndexOf("Current_CentralPlaza_LibraryDoorCenterPlank", StringComparison.OrdinalIgnoreCase) >= 0,
                camera,
                controller,
                outputDirectory,
                "03_no_center_plank.png",
                ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            CaptureWindowDoorReviewIsolationVariant(
                "doorPanelsOff",
                path => path.IndexOf("Current_CentralPlaza_LibraryDoorPanelsLeft", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        path.IndexOf("Current_CentralPlaza_LibraryDoorPanelsRight", StringComparison.OrdinalIgnoreCase) >= 0,
                camera,
                controller,
                outputDirectory,
                "04_no_door_panels.png",
                ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            CaptureWindowDoorReviewIsolationVariant(
                "doorReliefOff",
                path => path.IndexOf("Current_CentralPlaza_LibraryDoorRelief", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        path.IndexOf("Current_CentralPlaza_LibraryEntryDepth", StringComparison.OrdinalIgnoreCase) >= 0,
                camera,
                controller,
                outputDirectory,
                "06_no_door_relief_depth.png",
                ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            controller.ClosePortal();
            guide.ApplyActiveTimeIsolationForReview();
            PositionWindowDoorReviewCamera(camera, controller);
            var closedBeforePortal = CaptureWindowDoorReview(camera, outputDirectory, "10_window_closed_before_open.png", ref count);
            LogWindowDoorReviewState("window.closed.beforeOpen", controller, camera);

            var opened = controller.TryOpenPortalForTests(TimeWindowDragStart, TimeWindowDragEnd);
            yield return null;
            controller.RenderPortalAperturesForReview();
            guide.ApplyActiveTimeIsolationForReview();
            PositionWindowDoorReviewCamera(camera, controller);
            LogLightingState("windowDoorReview.open.ready", FastVsHouseArea.CentralPlaza);
            LogWindowDoorReviewState($"window.open opened={opened}", controller, camera);
            var openedSample = CaptureWindowDoorReview(camera, outputDirectory, "11_window_open.png", ref count);
            var closedOpenDelta = CompareFrameSamples(closedBeforePortal, openedSample);
            Debug.Log(
                $"{WindowDoorReviewMarker}: imageDelta closedBefore_vs_open " +
                $"meanAbsRgb={closedOpenDelta.MeanAbsRgb:0.000} changedSamplePct={closedOpenDelta.ChangedSamplePct:0.000} " +
                $"changed={closedOpenDelta.ChangedSamples} samples={closedOpenDelta.TotalSamples}");
            yield return new WaitForSecondsRealtime(0.08f);

            CaptureWindowDoorReviewOpenIsolationVariant(
                "currentApertureOff",
                path => path.IndexOf("TW_V25_Current_LivePortalAperture", StringComparison.OrdinalIgnoreCase) >= 0,
                camera,
                controller,
                outputDirectory,
                "13_window_open_current_aperture_off.png",
                openedSample,
                closedBeforePortal,
                ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            CaptureWindowDoorReviewOpenIsolationVariant(
                "currentPortalFrameOnlyOff",
                path => path.IndexOf("TW_V21_CurrentPortal_", StringComparison.OrdinalIgnoreCase) >= 0 &&
                        path.IndexOf("LivePortalAperture", StringComparison.OrdinalIgnoreCase) < 0,
                camera,
                controller,
                outputDirectory,
                "14_window_open_current_frame_only_off.png",
                openedSample,
                closedBeforePortal,
                ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            CaptureWindowDoorReviewOpenIsolationVariant(
                "currentPortalAllOff",
                path => path.IndexOf("TW_V21_CurrentPortal_", StringComparison.OrdinalIgnoreCase) >= 0,
                camera,
                controller,
                outputDirectory,
                "15_window_open_current_portal_all_off.png",
                openedSample,
                closedBeforePortal,
                ref count);
            yield return new WaitForSecondsRealtime(0.08f);

            controller.ClosePortal();
            yield return null;
            guide.ApplyActiveTimeIsolationForReview();
            PositionWindowDoorReviewCamera(camera, controller);
            LogLightingState("windowDoorReview.closedAfter.ready", FastVsHouseArea.CentralPlaza);
            LogWindowDoorReviewState("window.closed.afterOpen", controller, camera);
            var closedAfterSample = CaptureWindowDoorReview(camera, outputDirectory, "12_window_closed_after_open.png", ref count);
            var closedAfterDelta = CompareFrameSamples(closedBeforePortal, closedAfterSample);
            Debug.Log(
                $"{WindowDoorReviewMarker}: imageDelta closedBefore_vs_closedAfter " +
                $"meanAbsRgb={closedAfterDelta.MeanAbsRgb:0.000} changedSamplePct={closedAfterDelta.ChangedSamplePct:0.000} " +
                $"changed={closedAfterDelta.ChangedSamples} samples={closedAfterDelta.TotalSamples}");

            var doorBaselineRepeatDelta = CompareFrameSamples(closedDoorBaseline, closedBeforePortal);
            Debug.Log(
                $"{WindowDoorReviewMarker}: imageDelta closedDoorBaseline_vs_closedBeforePortal " +
                $"meanAbsRgb={doorBaselineRepeatDelta.MeanAbsRgb:0.000} changedSamplePct={doorBaselineRepeatDelta.ChangedSamplePct:0.000} " +
                $"changed={doorBaselineRepeatDelta.ChangedSamples} samples={doorBaselineRepeatDelta.TotalSamples}");

            camera.cullingMask = previousMask;
            camera.orthographic = previousOrthographic;
            camera.orthographicSize = previousOrthographicSize;
            camera.fieldOfView = previousFieldOfView;
            controller.SetRuntimeInputEnabledForReview(true);
            controller.ForcePlayerCurrentLocalForReview(HouseExteriorCenter + new Vector3(0f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();

            Debug.Log($"{WindowDoorReviewMarker}: end count={count}");
            Application.Quit(0);
        }

        private static void CapturePastLibraryTableObject(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            string fileName,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            ref int count)
        {
            PositionChapter1AllMapsCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            WriteCameraPng(camera, Path.Combine(outputDirectory, fileName));
            count++;
            Debug.Log($"{TableObjectCaptureMarker}: saved path={Path.Combine(outputDirectory, fileName)} anchorLocal={anchorLocalPosition}");
        }

        private static int BuildOtherTimeCameraMask(int previousMask, TimeWindowPairedSpacePortalController controller)
        {
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            return (previousMask & ~currentBit) | otherBit | playerBit;
        }

        private static int BuildCurrentTimeCameraMask(int previousMask, TimeWindowPairedSpacePortalController controller)
        {
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            return (previousMask & ~otherBit) | currentBit;
        }

        private static int BuildBridgeTraversalProofCameraMask(int previousMask, TimeWindowPairedSpacePortalController controller, bool otherTime)
        {
            var mask = otherTime
                ? BuildOtherTimeCameraMask(previousMask, controller)
                : BuildCurrentTimeCameraMask(previousMask, controller);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            return mask | playerBit;
        }

        private static void RunBridgeTraversalProofSide(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            CharacterController player,
            Camera camera,
            string outputDirectory,
            bool otherTime,
            int previousMask,
            ref int count)
        {
            var route = BuildBridgeTraversalRoute();
            var sideLabel = otherTime ? "past" : "current";
            visibility.SetActiveAreaForReview(FastVsHouseArea.Ruins);
            if (otherTime)
            {
                controller.ForcePlayerOtherTimeLocalForReview(route[0]);
            }
            else
            {
                controller.ForcePlayerCurrentLocalForReview(route[0]);
            }

            guide.ApplyActiveTimeIsolationForReview();
            camera.cullingMask = BuildBridgeTraversalProofCameraMask(previousMask, controller, otherTime);
            Physics.SyncTransforms();
            Debug.Log($"{BridgeTraversalProofMarker}: side={sideLabel} start local={controller.GetPlayerLocalCoordinateForReview()}");

            for (var i = 0; i < route.Length; i++)
            {
                ValidateBridgeTraversalSupportPoint(controller, route[i], $"{sideLabel} route support {i + 1}");
            }

            CaptureBridgeTraversalProofFrame(controller, camera, outputDirectory, $"{sideLabel}_01_start.png", otherTime, route[0], ref count);

            for (var i = 1; i < route.Length; i++)
            {
                MoveCharacterControllerAlongBridgeSegment(controller, player, route[i], otherTime, $"{sideLabel} segment {i}");
                guide.ApplyActiveTimeIsolationForReview();
                if (i == 4)
                {
                    CaptureBridgeTraversalProofFrame(controller, camera, outputDirectory, $"{sideLabel}_02_midspan.png", otherTime, route[i], ref count);
                }

                Debug.Log($"{BridgeTraversalProofMarker}: side={sideLabel} waypoint={i + 1}/{route.Length} local={controller.GetPlayerLocalCoordinateForReview()}");
            }

            var finalLocal = controller.GetPlayerLocalCoordinateForReview();
            var targetLocal = route[route.Length - 1];
            var finalDelta = new Vector2(finalLocal.x - targetLocal.x, finalLocal.z - targetLocal.z).magnitude;
            if (finalDelta > 0.38f)
            {
                throw new InvalidOperationException($"{sideLabel} bridge traversal did not reach F6. final={finalLocal}, target={targetLocal}, delta={finalDelta:0.000}.");
            }

            CaptureBridgeTraversalProofFrame(controller, camera, outputDirectory, $"{sideLabel}_03_f6_exit.png", otherTime, targetLocal, ref count);
            Debug.Log($"{BridgeTraversalProofMarker}: side={sideLabel} pass final={finalLocal} delta={finalDelta:0.000}");
        }

        private static Vector3[] BuildBridgeTraversalRoute()
        {
            return new[]
            {
                BridgeTraversalProbePoint(Chapter1F1FromE3Target),
                Chapter1RuinsMapCenter + new Vector3(-10.80f, 0.18f, 0.02f),
                Chapter1RuinsMapCenter + new Vector3(-6.72f, 0.18f, 0.04f),
                Chapter1RuinsMapCenter + new Vector3(-2.20f, 0.18f, -0.01f),
                Chapter1RuinsMapCenter + new Vector3(0.00f, 0.18f, -0.02f),
                Chapter1RuinsMapCenter + new Vector3(2.20f, 0.18f, 0.01f),
                Chapter1RuinsMapCenter + new Vector3(6.72f, 0.18f, 0.04f),
                Chapter1RuinsMapCenter + new Vector3(10.80f, 0.18f, 0.02f),
                BridgeTraversalProbePoint(Chapter1F6RouteTriggerCenter)
            };
        }

        private static Vector3 BridgeTraversalProbePoint(Vector3 localPoint)
        {
            return new Vector3(localPoint.x, 0.18f, localPoint.z);
        }

        private static void ValidateBridgeTraversalSupportPoint(TimeWindowPairedSpacePortalController controller, Vector3 localPoint, string label)
        {
            var root = controller.PlayerInOtherTime ? controller.OtherTimeSpaceRootForReview : controller.CurrentSpaceRootForReview;
            if (root == null)
            {
                throw new InvalidOperationException($"Missing active root for {label}.");
            }

            var origin = root.TransformPoint(localPoint + Vector3.up * 1.25f);
            var hits = Physics.RaycastAll(origin, Vector3.down, 1.70f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            foreach (var hit in hits)
            {
                if (hit.collider == null ||
                    hit.collider.GetComponentInParent<CharacterController>() != null)
                {
                    continue;
                }

                if (IsBlockingContinuationRouteCollider(hit.collider))
                {
                    throw new InvalidOperationException($"{label} is covered by blocking collider {hit.collider.name}.");
                }

                var landmark = hit.collider.GetComponentInParent<TimeWindowPairedSpaceLandmark>();
                if (landmark != null && landmark.Kind == TimeWindowPairedSpaceLandmarkKind.PathOrFloor)
                {
                    return;
                }
            }

            throw new InvalidOperationException($"{label} has no PathOrFloor support below the player capsule.");
        }

        private static void MoveCharacterControllerAlongBridgeSegment(
            TimeWindowPairedSpacePortalController controller,
            CharacterController player,
            Vector3 targetLocal,
            bool otherTime,
            string label)
        {
            const float StepDistance = 0.18f;
            const float GroundingDrop = 0.035f;
            const int MaxSteps = 180;

            for (var stepIndex = 0; stepIndex < MaxSteps; stepIndex++)
            {
                if (controller.PlayerInOtherTime != otherTime)
                {
                    throw new InvalidOperationException($"{label} changed the active time side during movement.");
                }

                var beforeLocal = controller.GetPlayerLocalCoordinateForReview();
                var horizontal = new Vector2(targetLocal.x - beforeLocal.x, targetLocal.z - beforeLocal.z);
                if (horizontal.magnitude <= 0.22f)
                {
                    return;
                }

                var beforeXZ = new Vector2(beforeLocal.x, beforeLocal.z);
                var step = new Vector3(horizontal.x, 0f, horizontal.y).normalized * Mathf.Min(StepDistance, horizontal.magnitude);
                step.y = -GroundingDrop;
                controller.MovePlayerLocalForReview(step, true);
                Physics.SyncTransforms();

                var afterLocal = controller.GetPlayerLocalCoordinateForReview();
                var afterXZ = new Vector2(afterLocal.x, afterLocal.z);
                var moved = Vector2.Distance(beforeXZ, afterXZ);
                if (moved < 0.035f)
                {
                    throw new InvalidOperationException($"{label} was blocked before reaching the next bridge waypoint. before={beforeLocal}, after={afterLocal}, target={targetLocal}, player={player.name}.");
                }

                if (afterLocal.y < -0.12f || afterLocal.y > 0.82f)
                {
                    throw new InvalidOperationException($"{label} left the playable bridge height band. local={afterLocal}, target={targetLocal}.");
                }
            }

            throw new InvalidOperationException($"{label} exceeded the step budget before reaching {targetLocal}.");
        }

        private static bool IsBlockingContinuationRouteCollider(Collider collider)
        {
            if (collider == null ||
                collider.GetComponentInParent<CharacterController>() != null)
            {
                return false;
            }

            var landmark = collider.GetComponentInParent<TimeWindowPairedSpaceLandmark>();
            return landmark == null || landmark.Kind != TimeWindowPairedSpaceLandmarkKind.PathOrFloor;
        }

        private static void CaptureBridgeTraversalProofFrame(
            TimeWindowPairedSpacePortalController controller,
            Camera camera,
            string outputDirectory,
            string fileName,
            bool otherTime,
            Vector3 anchorLocal,
            ref int count)
        {
            var root = otherTime ? controller.OtherTimeSpaceRootForReview : controller.CurrentSpaceRootForReview;
            if (root == null)
            {
                throw new InvalidOperationException($"Missing root while capturing bridge traversal frame {fileName}.");
            }

            PositionChapter1AllMapsCamera(
                camera,
                root.TransformPoint(anchorLocal),
                new Vector3(0f, 7.80f, -11.60f),
                new Vector3(0f, 0.26f, 1.35f));
            var path = Path.Combine(outputDirectory, fileName);
            WriteCameraPng(camera, path);
            count++;
            Debug.Log($"{BridgeTraversalProofMarker}: saved side={(otherTime ? "past" : "current")} path={path} local={anchorLocal}");
        }

        private static string BuildTableObjectStatus()
        {
            var names = new[]
            {
                "Past_Library_PropDetail_LongTableBookPairA",
                "Past_Library_PropDetail_LongTableBookPairA_Accent",
                "Past_Library_PropDetail_LongTableBookPairA_Detail",
                "Past_Library_PropDetail_LongTableBookPairA_Slip",
                "Past_Library_PropDetail_LongTableBookPairB",
                "Past_Library_PropDetail_LongTableBookPairB_Accent",
                "Past_Library_PropDetail_LongTableBookPairB_Detail",
                "Past_Library_PropDetail_LongTableBookPairB_Slip",
                "Past_Library_TargetBook_ForPickup",
                "Past_Library_OrderedFloorDetail_BookBundleA",
                "Past_Library_OrderedFloorDetail_BookBundleC",
                "Past_Library_BackWallBookshelfFrontTexturePanel",
                "Past_Library_LeftSideBookshelf_BookshelfFrontTexturePanel",
                "Past_Library_RightSideBookshelf_BookshelfFrontTexturePanel",
                "Past_Library_ReadingSurfaceDensity_LongTableOrderBookA",
                "Past_Library_ReadingSurfaceDensity_LongTableOrderBookB",
                "Past_Library_ReadingSurfaceDensity_SideTableOrderStackA",
                "Past_Library_ReadingSurfaceDensity_SideTableOrderStackB",
                "Past_Library_PropDetail_ShelfLedgerWest",
                "Past_Library_PropDetail_ShelfLedgerWest_Accent",
                "Past_Library_PropDetail_ShelfLedgerWest_Detail",
                "Past_Library_PropDetail_ShelfLedgerWest_Slip",
                "Past_Library_ReadableMicroprops_TableOpenBook_LeftFront",
                "Past_Library_ReadableMicroprops_TableOpenBook_LeftFront_Detail",
                "Past_Library_ReadableMicroprops_TableClosedBook_CenterRear",
                "Past_Library_ReadableMicroprops_TableClosedBook_CenterRear_Detail",
                "Past_Library_ReadableMicroprops_LeftShelfLedgerA",
                "Past_Library_ReadableMicroprops_RightShelfLedgerA",
                "Current_Library_TableOpenBook",
                "Current_Library_Ruin_ToppledBookStack",
                "Current_Library_Ruin_FallenBookSpines",
                "Current_Library_ReadableMicroprops_FloorOpenBookA",
                "Current_Library_ReadableMicroprops_FloorOpenBookA_Detail",
                "Current_Library_ReadableMicroprops_LeftShelfLooseBookA",
                "Current_Library_ReadableMicroprops_RightShelfLooseBookA",
                "Current_Library_ReadingSurfaceDensity_LongTableDustBookA",
                "Current_Library_ReadingSurfaceDensity_SideTableFallenBookA",
                "Current_Library_Stage8j_LongTableReadableOpenBookA",
                "Current_Library_Stage8j_LongTableReadableOpenBookA_Cover",
                "Current_Library_Stage8j_LongTableReadableOpenBookA_Pages",
                "Current_Library_Stage8j_LongTableReadableOpenBookA_Spine",
                "Current_Library_Stage8m_RightDeskShelfEchoBookA",
                "Current_Library_Stage8n_RightDeskStackBookA",
                "Current_Library_Stage8n_FloorClosedBookA",
                "Current_Library_Stage8b_TableSideA_ColorStackA",
                "Current_Library_Stage8b_TableSideB_ColorStackA",
                "Current_Library_Stage8h_LongTableColorBookA",
                "Past_Library_ReadingTableGrounding_LeftBookContactA",
                "Past_Library_ReadingTableGrounding_RightBookContactA",
                "Past_Library_EntryTableContrast_LeftTableBookLineA",
                "Past_Library_EntryTableContrast_RightTableBookLineA",
                "Past_Library_ReadingTableClean_LeftFront_BookA",
                "Past_Library_ReadingTableClean_LeftFront_BookB",
                "Past_Library_ReadingTableClean_LeftFront_BookC",
                "Past_Library_ReadingTableClean_CenterFront_BookA",
                "Past_Library_ReadingTableClean_CenterFront_BookB",
                "Past_Library_ReadingTableClean_CenterFront_BookC",
                "Past_Library_ReadingTableClean_RightFront_BookA",
                "Past_Library_ReadingTableClean_RightFront_BookB",
                "Past_Library_ReadingTableClean_RightFront_BookC",
                "Past_Library_ReadingTableClean_LeftRear_BookA",
                "Past_Library_ReadingTableClean_LeftRear_BookB",
                "Past_Library_ReadingTableClean_LeftRear_BookC",
                "Past_Library_ReadingTableClean_CenterRear_BookA",
                "Past_Library_ReadingTableClean_CenterRear_BookB",
                "Past_Library_ReadingTableClean_CenterRear_BookC",
                "Past_Library_ReadingTableClean_RightRear_BookA",
                "Past_Library_ReadingTableClean_RightRear_BookB",
                "Past_Library_ReadingTableClean_RightRear_BookC"
            };

            var builder = new StringBuilder();
            for (var i = 0; i < names.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append("; ");
                }

                var sceneObject = FindSceneObjectIncludingInactive(names[i]);
                builder.Append(names[i]).Append('=').Append(DescribeSceneObjectState(sceneObject));
            }

            builder.Append("; Past_Library_TargetBook_ForPickup_RendererCount=")
                .Append(CountRenderers("Past_Library_TargetBook_ForPickup"));

            return builder.ToString();
        }

        private static int CountRenderers(string objectName)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            return sceneObject != null ? sceneObject.GetComponentsInChildren<Renderer>(true).Length : -1;
        }

        private static string DescribeSceneObjectState(GameObject sceneObject)
        {
            if (sceneObject == null)
            {
                return "missing";
            }

            return sceneObject.activeInHierarchy ? "present-active" : "present-inactive";
        }

        private static GameObject FindSceneObjectIncludingInactive(string objectName)
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var sceneObject in objects)
            {
                if (sceneObject.scene.IsValid() && string.Equals(sceneObject.name, objectName, StringComparison.Ordinal))
                {
                    return sceneObject;
                }
            }

            return null;
        }

        private static void MovePlayerLocalForReview(TimeWindowPairedSpacePortalController controller, Vector3 localDelta, bool useCharacterController)
        {
            controller.MovePlayerLocalForReview(localDelta, useCharacterController);
        }

        private static void CaptureTimeWindowProbeView(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            string fileName,
            bool otherTimeView,
            ref int count)
        {
            var portalRoot = otherTimeView
                ? controller.OtherTimePortalRootForReview
                : controller.CurrentPortalRootForReview;
            if (portalRoot == null)
            {
                Debug.LogWarning($"{TimeWindowProbeMarker}: skipCapture missingPortalRoot file={fileName} otherTimeView={otherTimeView}");
                return;
            }

            var previousMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousFieldOfView = camera.fieldOfView;
            try
            {
                camera.orthographic = false;
                camera.fieldOfView = RuntimeVsFollowCameraFov;
                if (otherTimeView)
                {
                    camera.cullingMask = BuildOtherTimeCameraMask(previousMask, controller);
                }

                var portal = portalRoot.transform;
                var position = portal.TransformPoint(new Vector3(0f, 1.15f, -3.25f));
                var lookAt = portal.TransformPoint(new Vector3(0f, 0.45f, 0.65f));
                camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
                controller.RenderPortalAperturesForReview();
                WriteCameraPng(camera, Path.Combine(outputDirectory, fileName));
                count++;
                Debug.Log($"{TimeWindowProbeMarker}: saved path={Path.Combine(outputDirectory, fileName)} otherTimeView={otherTimeView}");
            }
            finally
            {
                camera.cullingMask = previousMask;
                camera.orthographic = previousOrthographic;
                camera.fieldOfView = previousFieldOfView;
            }
        }

        private static void CaptureTimeWindowVisualPhysicsView(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            string fileName,
            bool otherTimeView,
            ref int count)
        {
            CaptureTimeWindowProbeView(camera, controller, outputDirectory, fileName, otherTimeView, ref count);
            Debug.Log($"{TimeWindowVisualPhysicsMarker}: saved path={Path.Combine(outputDirectory, fileName)} otherTimeView={otherTimeView} {BuildTimeWindowVisualPhysicsState(controller, camera)}");
        }

        private static void CaptureRendererDiagnosticsClose(Camera camera, TimeWindowPairedSpacePortalController controller, string outputDirectory, string fileName, ref int count)
        {
            PositionRendererFacadeCloseCamera(camera, controller);
            WriteCameraPng(camera, Path.Combine(outputDirectory, fileName));
            count++;
        }

        private static void CaptureRendererIsolationVariant(
            string variant,
            Predicate<string> shouldDisable,
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            string wideFileName,
            string closeFileName,
            FramePixelSample baselineWide,
            FramePixelSample baselineClose,
            ref int count)
        {
            var disabledRenderers = DisableRenderersForIsolation(variant, shouldDisable);
            try
            {
                var wide = CaptureRendererIsolationWide(camera, controller, outputDirectory, wideFileName, ref count);
                var close = CaptureRendererIsolationClose(camera, controller, outputDirectory, closeFileName, ref count);
                LogRendererIsolationDelta(variant, "wide", baselineWide, wide);
                LogRendererIsolationDelta(variant, "close", baselineClose, close);
            }
            finally
            {
                RestoreRenderersForIsolation(variant, disabledRenderers);
            }
        }

        private static FramePixelSample CaptureRendererIsolationWide(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            string fileName,
            ref int count)
        {
            PositionRendererIsolationWideCamera(camera, controller);
            var sample = WriteCameraPngAndSample(camera, Path.Combine(outputDirectory, fileName), 4);
            count++;
            Debug.Log($"{RendererIsolationMarker}: saved path={Path.Combine(outputDirectory, fileName)}");
            return sample;
        }

        private static void PositionRendererIsolationWideCamera(Camera camera, TimeWindowPairedSpacePortalController controller)
        {
            camera.cullingMask = BuildCurrentTimeCameraMask(camera.cullingMask, controller);
            PositionChapter1AllMapsCamera(
                camera,
                controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(1.45f, 0.02f, -0.20f)),
                new Vector3(0f, 13.80f, -18.20f),
                new Vector3(0f, 0.20f, 1.55f));
        }

        private static FramePixelSample CaptureRendererIsolationClose(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            string fileName,
            ref int count)
        {
            camera.cullingMask = BuildCurrentTimeCameraMask(camera.cullingMask, controller);
            PositionRendererFacadeCloseCamera(camera, controller);
            var sample = WriteCameraPngAndSample(camera, Path.Combine(outputDirectory, fileName), 4);
            count++;
            Debug.Log($"{RendererIsolationMarker}: saved path={Path.Combine(outputDirectory, fileName)}");
            return sample;
        }

        private static void LogRendererIsolationDelta(string variant, string view, FramePixelSample baseline, FramePixelSample current)
        {
            var delta = CompareFrameSamples(baseline, current);
            Debug.Log(
                $"{RendererIsolationMarker}: variant={variant} view={view} baselineDelta " +
                $"meanAbsRgb={delta.MeanAbsRgb:0.000} changedSamplePct={delta.ChangedSamplePct:0.000} " +
                $"changed={delta.ChangedSamples} samples={delta.TotalSamples}");
        }

        private static void PositionRendererFacadeCloseCamera(Camera camera, TimeWindowPairedSpacePortalController controller)
        {
            camera.orthographic = false;
            camera.fieldOfView = RuntimeVsFollowCameraFov;
            var anchor = controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(1.25f, 0.02f, 2.20f));
            var position = anchor + new Vector3(0f, 2.05f, -5.10f);
            var lookAt = anchor + new Vector3(0f, 0.62f, 2.85f);
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
        }

        private static void PositionWindowDoorReviewCamera(Camera camera, TimeWindowPairedSpacePortalController controller)
        {
            camera.orthographic = false;
            camera.fieldOfView = RuntimeVsFollowCameraFov;
            camera.cullingMask = BuildCurrentTimeCameraMask(camera.cullingMask, controller);
            var anchor = controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(0.95f, 0.02f, 2.70f));
            var position = anchor + new Vector3(0f, 2.10f, -5.35f);
            var lookAt = anchor + new Vector3(0f, 0.58f, 3.05f);
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
        }

        private static FramePixelSample CaptureWindowDoorReview(Camera camera, string outputDirectory, string fileName, ref int count)
        {
            var path = Path.Combine(outputDirectory, fileName);
            var sample = WriteCameraPngAndSample(camera, path, 4);
            count++;
            Debug.Log($"{WindowDoorReviewMarker}: saved path={path}");
            return sample;
        }

        private static List<RendererRoiCandidate> LogWindowDoorPillarRoiCandidates(string label, Camera camera, int maxItems)
        {
            var candidates = FindWindowDoorPillarRoiCandidates(camera);
            var builder = new StringBuilder();
            var logged = Mathf.Min(maxItems, candidates.Count);
            for (var index = 0; index < logged; index++)
            {
                if (index > 0)
                {
                    builder.Append(" | ");
                }

                builder.Append(DescribeWindowDoorPillarRoiCandidate(index + 1, candidates[index]));
            }

            Debug.Log(
                $"{WindowDoorReviewMarker}: pillarRoi label={label} " +
                $"roiViewport={FormatRect(WindowDoorPillarRoiViewport)} candidates={candidates.Count} logged={logged} " +
                $"items=[{builder}]");
            return candidates;
        }

        private static List<RendererRoiCandidate> FindWindowDoorPillarRoiCandidates(Camera camera)
        {
            return FindRendererRoiCandidates(camera, WindowDoorPillarRoiViewport);
        }

        private static void LogRendererIsolationRightOuterRoadRoiCandidates(Camera camera, int maxItems)
        {
            LogRendererIsolationRoiCandidates(camera, "rightOuterRoadRoi", RendererRightOuterRoadRoiViewport, maxItems);
        }

        private static void LogRendererIsolationRoiCandidates(Camera camera, string label, Rect roiViewport, int maxItems)
        {
            var candidates = FindRendererRoiCandidates(camera, roiViewport);
            var builder = new StringBuilder();
            var logged = Mathf.Min(maxItems, candidates.Count);
            for (var index = 0; index < logged; index++)
            {
                if (index > 0)
                {
                    builder.Append(" | ");
                }

                builder.Append(DescribeWindowDoorPillarRoiCandidate(index + 1, candidates[index]));
            }

            Debug.Log(
                $"{RendererIsolationMarker}: {label} " +
                $"roiViewport={FormatRect(roiViewport)} candidates={candidates.Count} logged={logged} " +
                $"items=[{builder}]");
        }

        private static void LogRightmostDetachedPixelContributorsWithPreset(
            Camera camera,
            string phase,
            Predicate<string> presetDisable,
            int maxCandidatesPerPoint,
            int maxLoggedResults)
        {
            var disabledRenderers = DisableRenderersForIsolation($"rightmostPixelPreset_{phase}", presetDisable);
            try
            {
                LogRightmostDetachedPixelContributors(camera, phase, maxCandidatesPerPoint, maxLoggedResults);
            }
            finally
            {
                RestoreRenderersForIsolation($"rightmostPixelPreset_{phase}", disabledRenderers);
            }
        }

        private static void LogRightmostDetachedPixelContributors(Camera camera, string phase, int maxCandidatesPerPoint, int maxLoggedResults)
        {
            LogPixelContributors(
                camera,
                "rightmostPixel",
                RendererRightmostDetachedProbePixels,
                RendererRightmostDetachedProbeLabels,
                phase,
                maxCandidatesPerPoint,
                maxLoggedResults);
        }

        private static void LogFarRightStripPixelContributors(Camera camera, string phase, int maxCandidatesPerPoint, int maxLoggedResults)
        {
            LogPixelContributors(
                camera,
                "farRightStripPixel",
                RendererFarRightStripProbePixels,
                RendererFarRightStripProbeLabels,
                phase,
                maxCandidatesPerPoint,
                maxLoggedResults);
        }

        private static void LogAllMapLowerFrontPixelContributors(Camera camera, string phase, int maxCandidatesPerPoint, int maxLoggedResults)
        {
            LogPixelContributors(
                camera,
                "allMapLowerFrontPixel",
                RendererAllMapLowerFrontProbePixels,
                RendererAllMapLowerFrontProbeLabels,
                phase,
                maxCandidatesPerPoint,
                maxLoggedResults);
        }

        private static void LogPixelContributors(
            Camera camera,
            string logPrefix,
            Vector2Int[] probePixels,
            string[] probeLabels,
            string phase,
            int maxCandidatesPerPoint,
            int maxLoggedResults)
        {
            var baseline = SampleCameraPixelPoints(camera, probePixels);
            Debug.Log(
                $"{RendererIsolationMarker}: {logPrefix}Baseline phase={phase} " +
                $"points=[{BuildProbePointSummary(probePixels, probeLabels, baseline)}]");

            var candidates = BuildPixelCandidateRenderers(camera, logPrefix, probePixels, probeLabels, phase, maxCandidatesPerPoint);
            var results = new List<RendererPixelContributionResult>();
            for (var index = 0; index < candidates.Count; index++)
            {
                var renderer = candidates[index];
                if (renderer == null || renderer.gameObject == null || !renderer.enabled)
                {
                    continue;
                }

                var wasEnabled = renderer.enabled;
                renderer.enabled = false;
                try
                {
                    var sample = SampleCameraPixelPoints(camera, probePixels);
                    var totalDelta = 0;
                    var maxDelta = 0;
                    var builder = new StringBuilder();
                    for (var point = 0; point < baseline.Length && point < sample.Length; point++)
                    {
                        var delta = ColorDeltaRgb(baseline[point], sample[point]);
                        totalDelta += delta;
                        maxDelta = Mathf.Max(maxDelta, delta);
                        if (builder.Length > 0)
                        {
                            builder.Append(",");
                        }

                        builder.Append(probeLabels[point])
                            .Append("=")
                            .Append(delta)
                            .Append(":")
                            .Append(FormatColor32(sample[point]));
                    }

                    if (maxDelta > 0)
                    {
                        results.Add(new RendererPixelContributionResult(
                            BuildHierarchyPath(renderer.transform),
                            DescribePrimaryMaterial(renderer),
                            totalDelta,
                            maxDelta,
                            builder.ToString()));
                    }
                }
                finally
                {
                    renderer.enabled = wasEnabled;
                }
            }

            results.Sort((a, b) =>
            {
                var byMax = b.MaxDelta.CompareTo(a.MaxDelta);
                if (byMax != 0)
                {
                    return byMax;
                }

                var byTotal = b.TotalDelta.CompareTo(a.TotalDelta);
                return byTotal != 0 ? byTotal : string.Compare(a.Path, b.Path, StringComparison.Ordinal);
            });

            var logged = Mathf.Min(maxLoggedResults, results.Count);
            for (var index = 0; index < logged; index++)
            {
                var result = results[index];
                Debug.Log(
                    $"{RendererIsolationMarker}: {logPrefix}Contributor phase={phase} rank={index + 1} " +
                    $"maxDelta={result.MaxDelta} totalDelta={result.TotalDelta} deltas=[{result.Deltas}] " +
                    $"path=\"{result.Path}\" mat={result.Material}");
            }

            Debug.Log(
                $"{RendererIsolationMarker}: {logPrefix}ContributorSummary phase={phase} " +
                $"candidateRenderers={candidates.Count} positive={results.Count} logged={logged}");
        }

        private static List<Renderer> BuildRightmostDetachedPixelCandidateRenderers(Camera camera, string phase, int maxCandidatesPerPoint)
        {
            return BuildPixelCandidateRenderers(
                camera,
                "rightmostPixel",
                RendererRightmostDetachedProbePixels,
                RendererRightmostDetachedProbeLabels,
                phase,
                maxCandidatesPerPoint);
        }

        private static List<Renderer> BuildPixelCandidateRenderers(
            Camera camera,
            string logPrefix,
            Vector2Int[] probePixels,
            string[] probeLabels,
            string phase,
            int maxCandidatesPerPoint)
        {
            var renderers = new List<Renderer>();
            var seen = new HashSet<int>();
            for (var point = 0; point < probePixels.Length; point++)
            {
                var pixel = probePixels[point];
                var viewport = new Vector2(
                    Mathf.Clamp01(pixel.x / 1280f),
                    Mathf.Clamp01(1f - pixel.y / 720f));
                var roi = new Rect(viewport.x - 0.018f, viewport.y - 0.018f, 0.036f, 0.036f);
                var candidates = FindRendererRoiCandidates(camera, roi);
                var logged = Mathf.Min(36, candidates.Count);
                var builder = new StringBuilder();
                for (var index = 0; index < logged; index++)
                {
                    if (index > 0)
                    {
                        builder.Append(" | ");
                    }

                    builder.Append(DescribeWindowDoorPillarRoiCandidate(index + 1, candidates[index]));
                }

                Debug.Log(
                    $"{RendererIsolationMarker}: {logPrefix}Roi phase={phase} label={probeLabels[point]} " +
                    $"pixel=({pixel.x},{pixel.y}) viewport=({viewport.x:0.000},{viewport.y:0.000}) roi={FormatRect(roi)} " +
                    $"candidates={candidates.Count} logged={logged} items=[{builder}]");

                var take = Mathf.Min(maxCandidatesPerPoint, candidates.Count);
                for (var index = 0; index < take; index++)
                {
                    var renderer = candidates[index].Renderer;
                    if (renderer == null)
                    {
                        continue;
                    }

                    var id = renderer.GetInstanceID();
                    if (seen.Add(id))
                    {
                        renderers.Add(renderer);
                    }
                }
            }

            return renderers;
        }

        private static List<RendererRoiCandidate> FindRendererRoiCandidates(Camera camera, Rect roiViewport)
        {
            var result = new List<RendererRoiCandidate>();
            var renderers = FindSceneRenderers();
            foreach (var renderer in renderers)
            {
                if (renderer == null ||
                    renderer.gameObject == null ||
                    !renderer.gameObject.activeInHierarchy ||
                    !renderer.enabled)
                {
                    continue;
                }

                var layer = renderer.gameObject.layer;
                if (layer < 0 || layer > 31 || (camera.cullingMask & (1 << layer)) == 0)
                {
                    continue;
                }

                if (!TryProjectRendererBoundsToViewport(camera, renderer, out var viewportRect, out var minDepth))
                {
                    continue;
                }

                if (!RectsOverlap(viewportRect, roiViewport))
                {
                    continue;
                }

                var overlapArea = RectOverlapArea(viewportRect, roiViewport);
                var projectedArea = Mathf.Max(viewportRect.width * viewportRect.height, 0.0001f);
                var roiArea = Mathf.Max(roiViewport.width * roiViewport.height, 0.0001f);
                var centerDistance = Vector2.Distance(viewportRect.center, roiViewport.center);
                var objectCoverage = Mathf.Clamp01(overlapArea / projectedArea);
                var roiCoverage = Mathf.Clamp01(overlapArea / roiArea);
                var score = objectCoverage * 2.0f + roiCoverage - centerDistance * 1.5f - Mathf.Clamp01(projectedArea) * 0.15f;
                result.Add(new RendererRoiCandidate(
                    renderer,
                    BuildHierarchyPath(renderer.transform),
                    viewportRect,
                    minDepth,
                    overlapArea,
                    centerDistance,
                    objectCoverage,
                    roiCoverage,
                    score));
            }

            result.Sort((a, b) =>
            {
                var scoreCompare = b.Score.CompareTo(a.Score);
                if (scoreCompare != 0)
                {
                    return scoreCompare;
                }

                var distanceCompare = a.CenterDistance.CompareTo(b.CenterDistance);
                if (distanceCompare != 0)
                {
                    return distanceCompare;
                }

                return a.MinDepth.CompareTo(b.MinDepth);
            });

            return result;
        }

        private static bool TryProjectRendererBoundsToViewport(Camera camera, Renderer renderer, out Rect viewportRect, out float minDepth)
        {
            var bounds = renderer.bounds;
            var min = bounds.min;
            var max = bounds.max;
            var corners = new[]
            {
                new Vector3(min.x, min.y, min.z),
                new Vector3(min.x, min.y, max.z),
                new Vector3(min.x, max.y, min.z),
                new Vector3(min.x, max.y, max.z),
                new Vector3(max.x, min.y, min.z),
                new Vector3(max.x, min.y, max.z),
                new Vector3(max.x, max.y, min.z),
                new Vector3(max.x, max.y, max.z)
            };

            var any = false;
            var minX = float.PositiveInfinity;
            var minY = float.PositiveInfinity;
            var maxX = float.NegativeInfinity;
            var maxY = float.NegativeInfinity;
            minDepth = float.PositiveInfinity;
            for (var index = 0; index < corners.Length; index++)
            {
                var viewport = camera.WorldToViewportPoint(corners[index]);
                if (viewport.z <= camera.nearClipPlane)
                {
                    continue;
                }

                any = true;
                minX = Mathf.Min(minX, viewport.x);
                minY = Mathf.Min(minY, viewport.y);
                maxX = Mathf.Max(maxX, viewport.x);
                maxY = Mathf.Max(maxY, viewport.y);
                minDepth = Mathf.Min(minDepth, viewport.z);
            }

            if (!any)
            {
                viewportRect = default;
                minDepth = 0f;
                return false;
            }

            viewportRect = Rect.MinMaxRect(minX, minY, maxX, maxY);
            return viewportRect.width > 0f && viewportRect.height > 0f;
        }

        private static void CaptureWindowDoorReviewRoiCandidateVariants(
            List<RendererRoiCandidate> candidates,
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            ref int count)
        {
            var captured = 0;
            var usedPaths = new HashSet<string>();
            for (var index = 0; index < candidates.Count && captured < WindowDoorPillarRoiIsolationCaptureLimit; index++)
            {
                var candidate = candidates[index];
                if (candidate == null ||
                    candidate.Renderer == null ||
                    candidate.Renderer.gameObject == null ||
                    !usedPaths.Add(candidate.Path))
                {
                    continue;
                }

                captured++;
                var exactPath = candidate.Path;
                var safeName = SanitizeFileName(candidate.Renderer.gameObject.name);
                if (safeName.Length > 56)
                {
                    safeName = safeName.Substring(0, 56);
                }

                CaptureWindowDoorReviewIsolationVariant(
                    $"pillarRoiCandidate{captured:00}",
                    path => string.Equals(path, exactPath, StringComparison.Ordinal),
                    camera,
                    controller,
                    outputDirectory,
                    $"05_roi_candidate_{captured:00}_{safeName}.png",
                    ref count);
            }

            Debug.Log($"{WindowDoorReviewMarker}: pillarRoi isolationCaptures={captured} limit={WindowDoorPillarRoiIsolationCaptureLimit}");
        }

        private static string DescribeWindowDoorPillarRoiCandidate(int rank, RendererRoiCandidate candidate)
        {
            return
                $"rank={rank},score={candidate.Score:0.000},path=\"{candidate.Path}\",rect={FormatRect(candidate.ViewportRect)}," +
                $"overlap={candidate.OverlapArea:0.0000},objectCoverage={candidate.ObjectCoverage:0.000},roiCoverage={candidate.RoiCoverage:0.000}," +
                $"centerDistance={candidate.CenterDistance:0.000},minDepth={candidate.MinDepth:0.000},bounds={FormatBounds(candidate.Renderer.bounds)}," +
                $"mat={DescribePrimaryMaterial(candidate.Renderer)}";
        }

        private static string FormatRect(Rect rect)
        {
            return $"({rect.xMin:0.000},{rect.yMin:0.000},{rect.xMax:0.000},{rect.yMax:0.000})";
        }

        private static bool RectsOverlap(Rect a, Rect b)
        {
            return a.xMin < b.xMax && a.xMax > b.xMin && a.yMin < b.yMax && a.yMax > b.yMin;
        }

        private static float RectOverlapArea(Rect a, Rect b)
        {
            var width = Mathf.Max(0f, Mathf.Min(a.xMax, b.xMax) - Mathf.Max(a.xMin, b.xMin));
            var height = Mathf.Max(0f, Mathf.Min(a.yMax, b.yMax) - Mathf.Max(a.yMin, b.yMin));
            return width * height;
        }

        private static void CaptureWindowDoorReviewIsolationVariant(
            string variant,
            Predicate<string> shouldDisable,
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            string fileName,
            ref int count)
        {
            var disabledRenderers = DisableRenderersForIsolation(variant, shouldDisable);
            try
            {
                PositionWindowDoorReviewCamera(camera, controller);
                LogWindowDoorReviewState($"isolation.{variant}", controller, camera);
                CaptureWindowDoorReview(camera, outputDirectory, fileName, ref count);
            }
            finally
            {
                RestoreRenderersForIsolation(variant, disabledRenderers);
            }
        }

        private static void CaptureWindowDoorReviewOpenIsolationVariant(
            string variant,
            Predicate<string> shouldDisable,
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            string outputDirectory,
            string fileName,
            FramePixelSample openedSample,
            FramePixelSample closedBeforeSample,
            ref int count)
        {
            var disabledRenderers = DisableRenderersForIsolation(variant, shouldDisable);
            try
            {
                controller.RenderPortalAperturesForReview();
                PositionWindowDoorReviewCamera(camera, controller);
                LogWindowDoorReviewState($"window.open.isolation.{variant}", controller, camera);
                var sample = CaptureWindowDoorReview(camera, outputDirectory, fileName, ref count);
                var openDelta = CompareFrameSamples(openedSample, sample);
                Debug.Log(
                    $"{WindowDoorReviewMarker}: imageDelta open_vs_{variant} " +
                    $"meanAbsRgb={openDelta.MeanAbsRgb:0.000} changedSamplePct={openDelta.ChangedSamplePct:0.000} " +
                    $"changed={openDelta.ChangedSamples} samples={openDelta.TotalSamples}");
                var closedDelta = CompareFrameSamples(closedBeforeSample, sample);
                Debug.Log(
                    $"{WindowDoorReviewMarker}: imageDelta closedBefore_vs_{variant} " +
                    $"meanAbsRgb={closedDelta.MeanAbsRgb:0.000} changedSamplePct={closedDelta.ChangedSamplePct:0.000} " +
                    $"changed={closedDelta.ChangedSamples} samples={closedDelta.TotalSamples}");
            }
            finally
            {
                RestoreRenderersForIsolation(variant, disabledRenderers);
                controller.RenderPortalAperturesForReview();
            }
        }

        private static void LogWindowDoorReviewState(string label, TimeWindowPairedSpacePortalController controller, Camera camera)
        {
            Debug.Log(
                $"{WindowDoorReviewMarker}: state label={label} " +
                $"{BuildTimeWindowProbeState(controller)} " +
                $"cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)} fov={camera.fieldOfView:0.000} mask={camera.cullingMask} " +
                $"doorCandidates=[{BuildRendererStatusForNames(WindowDoorReviewDoorCandidates)}] " +
                $"lightRenderers=[{BuildRendererStatusForTokens(WindowDoorReviewLightTokens, 36)}] " +
                $"sunbeamExact=[{BuildRendererStatusForNames(WindowDoorReviewSunbeamExactNames)}] " +
                $"sunbeamRenderers=[{BuildRendererDetailsForTokens(WindowDoorReviewSunbeamTokens, 18)}]");
        }

        private static string BuildRendererStatusForNames(string[] names)
        {
            var builder = new StringBuilder();
            for (var index = 0; index < names.Length; index++)
            {
                if (index > 0)
                {
                    builder.Append(" | ");
                }

                var renderer = FindRendererByObjectName(names[index]);
                builder.Append(names[index]).Append('=');
                builder.Append(renderer != null ? DescribeCompactRenderer(renderer) : "missing");
            }

            return builder.ToString();
        }

        private static string BuildRendererStatusForTokens(string[] tokens, int maxItems)
        {
            var renderers = FindSceneRenderers();
            var builder = new StringBuilder();
            var logged = 0;
            for (var index = 0; index < renderers.Count; index++)
            {
                var renderer = renderers[index];
                if (renderer == null)
                {
                    continue;
                }

                var path = BuildHierarchyPath(renderer.transform);
                if (!ContainsAny(path, tokens))
                {
                    continue;
                }

                if (logged > 0)
                {
                    builder.Append(" | ");
                }

                builder.Append(DescribeCompactRenderer(renderer));
                logged++;
                if (logged >= maxItems)
                {
                    builder.Append(" | ...");
                    break;
                }
            }

            return logged > 0 ? builder.ToString() : "none";
        }

        private static string BuildRendererDetailsForTokens(string[] tokens, int maxItems)
        {
            var renderers = FindSceneRenderers();
            var builder = new StringBuilder();
            var logged = 0;
            for (var index = 0; index < renderers.Count; index++)
            {
                var renderer = renderers[index];
                if (renderer == null)
                {
                    continue;
                }

                var path = BuildHierarchyPath(renderer.transform);
                if (!ContainsAny(path, tokens))
                {
                    continue;
                }

                if (logged > 0)
                {
                    builder.Append(" | ");
                }

                builder.Append(DescribeRenderer(renderer));
                logged++;
                if (logged >= maxItems)
                {
                    builder.Append(" | ...");
                    break;
                }
            }

            return logged > 0 ? builder.ToString() : "none";
        }

        private static Renderer FindRendererByObjectName(string objectName)
        {
            var renderers = FindSceneRenderers();
            for (var index = 0; index < renderers.Count; index++)
            {
                var renderer = renderers[index];
                if (renderer != null &&
                    renderer.gameObject != null &&
                    string.Equals(renderer.gameObject.name, objectName, StringComparison.Ordinal))
                {
                    return renderer;
                }
            }

            return null;
        }

        private static string DescribeCompactRenderer(Renderer renderer)
        {
            if (renderer == null)
            {
                return "missing";
            }

            return
                $"{renderer.gameObject.name}:active={renderer.gameObject.activeInHierarchy},enabled={renderer.enabled},visible={renderer.isVisible}," +
                $"layer={renderer.gameObject.layer},bounds={FormatBounds(renderer.bounds)},mat={DescribePrimaryMaterial(renderer)}";
        }

        private static List<RendererEnabledState> DisableRenderersForIsolation(string variant, Predicate<string> shouldDisable)
        {
            var disabledRenderers = new List<RendererEnabledState>();
            var matched = 0;
            var logged = 0;
            var builder = new StringBuilder();
            var renderers = FindSceneRenderers();
            foreach (var renderer in renderers)
            {
                if (renderer == null || renderer.gameObject == null || !renderer.gameObject.scene.IsValid())
                {
                    continue;
                }

                var path = BuildHierarchyPath(renderer.transform);
                if (!shouldDisable(path))
                {
                    continue;
                }

                matched++;
                if (logged < 40)
                {
                    if (logged > 0)
                    {
                        builder.Append(" | ");
                    }

                    builder.Append(path);
                    logged++;
                }

                if (renderer.enabled)
                {
                    disabledRenderers.Add(new RendererEnabledState(renderer, renderer.enabled));
                    renderer.enabled = false;
                }
            }

            Debug.Log($"{RendererIsolationMarker}: variant={variant} matched={matched} disabled={disabledRenderers.Count} logged=[{builder}]");
            return disabledRenderers;
        }

        private static void RestoreRenderersForIsolation(string variant, List<RendererEnabledState> disabledRenderers)
        {
            for (var index = disabledRenderers.Count - 1; index >= 0; index--)
            {
                var item = disabledRenderers[index];
                if (item.Renderer != null)
                {
                    item.Renderer.enabled = item.WasEnabled;
                }
            }

            Debug.Log($"{RendererIsolationMarker}: variant={variant} restored={disabledRenderers.Count}");
        }

        private static bool IsTransparentOverlayIsolationTarget(string path)
        {
            return IsTransparentCycle120IsolationTarget(path) ||
                   IsTransparentCycle125IsolationTarget(path) ||
                   IsTransparentCycle126IsolationTarget(path) ||
                   IsTransparentShadowFoundation70IsolationTarget(path);
        }

        private static bool IsTransparentCycle120IsolationTarget(string path)
        {
            return path.IndexOf("Cycle120_ReferenceLightColumn", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsTransparentCycle125IsolationTarget(string path)
        {
            return path.IndexOf("Cycle125_ReferenceDioramaShadow", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsTransparentCycle125ObjectIsolationTarget(string path, string suffix)
        {
            return path.IndexOf("Cycle125_ReferenceDioramaShadow_" + suffix, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsTransparentCycle126IsolationTarget(string path)
        {
            return path.IndexOf("Cycle126_CloseShadowBarMute", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsTransparentShadowFoundation70IsolationTarget(string path)
        {
            return path.IndexOf("ShadowFoundationCycle70_LibraryDiagonalCastA", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsLongRoadIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_RoadToHouseExterior",
                "CentralPlaza_RoadToSouthEastQuarter",
                "Cycle62_OuterGroundSkirt_NorthLowStreetContinuationA",
                "ScenicBackdrop_DistantRooflineA");
        }

        private static bool IsPastCentralPlazaLongRoadIsolationTarget(string path)
        {
            return path.IndexOf("Past_CentralPlaza_", StringComparison.OrdinalIgnoreCase) >= 0 &&
                   IsLongRoadIsolationTarget(path);
        }

        private static bool IsCentralPlazaBackgroundSkyDepthIsolationTarget(string path)
        {
            return path.IndexOf("CentralPlaza_OutdoorBackgroundSkyDepth", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsCentralPlazaOutdoorSkyDetailIsolationTarget(string path)
        {
            return path.IndexOf("CentralPlaza_OutdoorSkyDetail", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsCentralPlazaScenicBackdropIsolationTarget(string path)
        {
            return path.IndexOf("CentralPlaza_ScenicBackdrop", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsCentralPlazaRoadGeometryIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_RoadToHouseExterior",
                "CentralPlaza_RoadToSouthEastQuarter",
                "Cycle62_OuterGroundSkirt_NorthLowStreetContinuationA");
        }

        private static bool IsCentralPlazaBackgroundEnvelopeIsolationTarget(string path)
        {
            return IsCentralPlazaBackgroundSkyDepthIsolationTarget(path) ||
                   IsCentralPlazaOutdoorSkyDetailIsolationTarget(path) ||
                   path.IndexOf("CentralPlaza_OutdoorSkyHorizonLayering", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   IsCentralPlazaScenicBackdropIsolationTarget(path);
        }

        private static void LogRendererMaterialDiagnostics()
        {
            var renderers = FindSceneRenderers();
            var interesting = 0;
            var logged = 0;
            const int maxLogs = 180;
            foreach (var renderer in renderers)
            {
                if (!ShouldIncludeRendererDiagnostic(renderer))
                {
                    continue;
                }

                interesting++;
                if (logged >= maxLogs)
                {
                    continue;
                }

                Debug.Log($"{RendererDiagnosticsMarker}: material {DescribeRenderer(renderer)}");
                logged++;
            }

            Debug.Log($"{RendererDiagnosticsMarker}: materialSummary totalSceneRenderers={renderers.Count} interesting={interesting} logged={logged}");
        }

        private static void LogRendererOverlapDiagnostics()
        {
            var renderers = FindSceneRenderers();
            var candidates = new List<RendererDiagnosticsInfo>();
            foreach (var renderer in renderers)
            {
                if (renderer == null || !renderer.enabled || !renderer.gameObject.activeInHierarchy)
                {
                    continue;
                }

                var path = BuildHierarchyPath(renderer.transform);
                var lower = path.ToLowerInvariant();
                var bounds = renderer.bounds;
                if (bounds.size.y <= 0.22f ||
                    ContainsAny(lower, "floor", "dust", "decal", "road", "path", "paving", "shadow", "plaza", "library"))
                {
                    candidates.Add(new RendererDiagnosticsInfo(renderer, path, bounds));
                }
            }

            var pairs = new List<RendererOverlapInfo>();
            for (var i = 0; i < candidates.Count; i++)
            {
                for (var j = i + 1; j < candidates.Count; j++)
                {
                    var a = candidates[i];
                    var b = candidates[j];
                    var overlapX = Mathf.Min(a.Bounds.max.x, b.Bounds.max.x) - Mathf.Max(a.Bounds.min.x, b.Bounds.min.x);
                    var overlapZ = Mathf.Min(a.Bounds.max.z, b.Bounds.max.z) - Mathf.Max(a.Bounds.min.z, b.Bounds.min.z);
                    if (overlapX <= 0f || overlapZ <= 0f)
                    {
                        continue;
                    }

                    var overlapArea = overlapX * overlapZ;
                    var centerYDelta = Mathf.Abs(a.Bounds.center.y - b.Bounds.center.y);
                    var minYGap = Mathf.Max(0f, Mathf.Max(a.Bounds.min.y, b.Bounds.min.y) - Mathf.Min(a.Bounds.max.y, b.Bounds.max.y));
                    if (overlapArea < 0.08f || (centerYDelta > 0.08f && minYGap > 0.035f))
                    {
                        continue;
                    }

                    pairs.Add(new RendererOverlapInfo(a, b, overlapArea, centerYDelta, minYGap));
                }
            }

            pairs.Sort((a, b) => b.OverlapArea.CompareTo(a.OverlapArea));
            var logged = Mathf.Min(pairs.Count, 120);
            for (var index = 0; index < logged; index++)
            {
                var pair = pairs[index];
                Debug.Log(
                    $"{RendererDiagnosticsMarker}: overlap index={index} areaXZ={pair.OverlapArea:0.000} centerYDelta={pair.CenterYDelta:0.000} minYGap={pair.MinYGap:0.000} " +
                    $"a=\"{pair.A.Path}\" aBounds={FormatBounds(pair.A.Bounds)} aMat={DescribePrimaryMaterial(pair.A.Renderer)} " +
                    $"b=\"{pair.B.Path}\" bBounds={FormatBounds(pair.B.Bounds)} bMat={DescribePrimaryMaterial(pair.B.Renderer)}");
            }

            Debug.Log($"{RendererDiagnosticsMarker}: overlapSummary candidates={candidates.Count} pairs={pairs.Count} logged={logged}");
        }

        private static IEnumerator SampleRendererVisibilityDiagnostics(Camera camera, int frameCount)
        {
            var renderers = FindSceneRenderers();
            var stats = new Dictionary<int, RendererVisibilityStats>();
            foreach (var renderer in renderers)
            {
                if (!ShouldIncludeRendererDiagnostic(renderer))
                {
                    continue;
                }

                var id = renderer.GetInstanceID();
                if (!stats.ContainsKey(id))
                {
                    stats.Add(id, new RendererVisibilityStats(renderer));
                }
            }

            var visibleCounts = new int[Mathf.Max(frameCount, 1)];
            var enabledCounts = new int[Mathf.Max(frameCount, 1)];
            for (var frame = 0; frame < frameCount; frame++)
            {
                yield return null;
                var visibleCount = 0;
                var enabledCount = 0;
                foreach (var item in stats.Values)
                {
                    item.Sample();
                    if (item.LastEnabled)
                    {
                        enabledCount++;
                    }

                    if (item.LastVisible)
                    {
                        visibleCount++;
                    }
                }

                visibleCounts[frame] = visibleCount;
                enabledCounts[frame] = enabledCount;
            }

            var toggles = new List<RendererVisibilityStats>();
            foreach (var item in stats.Values)
            {
                if (item.VisibleToggleCount > 0 || item.EnabledToggleCount > 0)
                {
                    toggles.Add(item);
                }
            }

            toggles.Sort((a, b) =>
            {
                var byToggle = (b.VisibleToggleCount + b.EnabledToggleCount).CompareTo(a.VisibleToggleCount + a.EnabledToggleCount);
                return byToggle != 0 ? byToggle : string.Compare(a.Path, b.Path, StringComparison.Ordinal);
            });

            var logged = Mathf.Min(toggles.Count, 80);
            for (var index = 0; index < logged; index++)
            {
                var item = toggles[index];
                Debug.Log(
                    $"{RendererDiagnosticsMarker}: visibilityToggle index={index} visibleToggles={item.VisibleToggleCount} enabledToggles={item.EnabledToggleCount} " +
                    $"visibleFrames={item.VisibleFrames}/{frameCount} enabledFrames={item.EnabledFrames}/{frameCount} path=\"{item.Path}\" mat={DescribePrimaryMaterial(item.Renderer)} bounds={FormatBounds(item.Bounds)}");
            }

            Debug.Log(
                $"{RendererDiagnosticsMarker}: visibilitySummary tracked={stats.Count} frames={frameCount} " +
                $"visible={BuildSeriesSummary(visibleCounts)} enabled={BuildSeriesSummary(enabledCounts)} toggled={toggles.Count} logged={logged} camera=\"{camera.name}\"");
        }

        private static Dictionary<int, RendererFlickerStats> BuildRendererFlickerStats()
        {
            var renderers = FindSceneRenderers();
            var stats = new Dictionary<int, RendererFlickerStats>();
            foreach (var renderer in renderers)
            {
                if (!ShouldIncludeRendererDiagnostic(renderer))
                {
                    continue;
                }

                var id = renderer.GetInstanceID();
                if (!stats.ContainsKey(id))
                {
                    stats.Add(id, new RendererFlickerStats(renderer));
                }
            }

            return stats;
        }

        private static void LogVisiblePathDelta(int frame, List<string> previous, List<string> current)
        {
            var added = new List<string>();
            var removed = new List<string>();
            var previousIndex = 0;
            var currentIndex = 0;
            while (previousIndex < previous.Count || currentIndex < current.Count)
            {
                if (previousIndex >= previous.Count)
                {
                    added.Add(current[currentIndex++]);
                    continue;
                }

                if (currentIndex >= current.Count)
                {
                    removed.Add(previous[previousIndex++]);
                    continue;
                }

                var compare = string.Compare(previous[previousIndex], current[currentIndex], StringComparison.Ordinal);
                if (compare == 0)
                {
                    previousIndex++;
                    currentIndex++;
                }
                else if (compare < 0)
                {
                    removed.Add(previous[previousIndex++]);
                }
                else
                {
                    added.Add(current[currentIndex++]);
                }
            }

            Debug.Log(
                $"{RendererFlickerProbeMarker}: visibleDelta frame={frame} added={added.Count} removed={removed.Count} " +
                $"addedSample=[{BuildPathSample(added, 12)}] removedSample=[{BuildPathSample(removed, 12)}]");
        }

        private static string BuildPathSample(List<string> paths, int maxCount)
        {
            if (paths == null || paths.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            var logged = Mathf.Min(paths.Count, maxCount);
            for (var index = 0; index < logged; index++)
            {
                if (index > 0)
                {
                    builder.Append(" | ");
                }

                builder.Append(paths[index]);
            }

            if (paths.Count > logged)
            {
                builder.Append(" | ...");
            }

            return builder.ToString();
        }

        private static uint BuildStableStringHash(List<string> values)
        {
            unchecked
            {
                uint hash = 2166136261;
                for (var index = 0; index < values.Count; index++)
                {
                    var value = values[index];
                    for (var charIndex = 0; charIndex < value.Length; charIndex++)
                    {
                        hash ^= value[charIndex];
                        hash *= 16777619;
                    }

                    hash ^= 0xff;
                    hash *= 16777619;
                }

                return hash;
            }
        }

        private static IEnumerator SampleRendererFlickerRuntime(Camera camera, int frameCount, string label)
        {
            var stats = BuildRendererFlickerStats();
            var visibleCounts = new int[Mathf.Max(frameCount, 1)];
            var enabledCounts = new int[Mathf.Max(frameCount, 1)];
            var deltaTimes = new float[Mathf.Max(frameCount, 1)];
            var unscaledDeltaTimes = new float[Mathf.Max(frameCount, 1)];
            var visibleHashChanges = 0;
            uint previousVisibleHash = 0;
            var previousVisiblePaths = new List<string>();

            for (var frame = 0; frame < frameCount; frame++)
            {
                yield return null;
                RenderCameraForVisibilitySample(camera);
                var visiblePaths = new List<string>();
                var enabledCount = 0;
                foreach (var item in stats.Values)
                {
                    item.Sample(frame);
                    if (item.LastEnabled)
                    {
                        enabledCount++;
                    }

                    if (item.LastVisible)
                    {
                        visiblePaths.Add(item.Path);
                    }
                }

                visiblePaths.Sort(StringComparer.Ordinal);
                var visibleHash = BuildStableStringHash(visiblePaths);
                if (frame > 0 && visibleHash != previousVisibleHash)
                {
                    visibleHashChanges++;
                    Debug.Log($"{RendererFlickerProbeMarker}: runtimeVisibleDelta phase={label} frame={frame}");
                    LogVisiblePathDelta(frame, previousVisiblePaths, visiblePaths);
                }

                visibleCounts[frame] = visiblePaths.Count;
                enabledCounts[frame] = enabledCount;
                deltaTimes[frame] = Time.deltaTime;
                unscaledDeltaTimes[frame] = Time.unscaledDeltaTime;

                if (frame == 0 || frame == frameCount - 1 || frame % 30 == 0)
                {
                    Debug.Log(
                        $"{RendererFlickerProbeMarker}: runtimeFrame phase={label} index={frame} " +
                        $"deltaTime={Time.deltaTime:0.0000} unscaledDeltaTime={Time.unscaledDeltaTime:0.0000} realtime={Time.realtimeSinceStartup:0.000} " +
                        $"visible={visiblePaths.Count} enabled={enabledCount} visibleHash=0x{visibleHash:X8} " +
                        $"cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)} fov={camera.fieldOfView:0.000} mask={camera.cullingMask}");
                }

                previousVisibleHash = visibleHash;
                previousVisiblePaths = visiblePaths;
            }

            LogRendererFlickerToggleSummary(stats, frameCount, label);
            Debug.Log(
                $"{RendererFlickerProbeMarker}: runtimeSummary phase={label} frames={frameCount} tracked={stats.Count} " +
                $"visible={BuildSeriesSummary(visibleCounts)} enabled={BuildSeriesSummary(enabledCounts)} visibleHashChanges={visibleHashChanges} " +
                $"deltaTime={BuildSeriesSummary(deltaTimes)} unscaledDeltaTime={BuildSeriesSummary(unscaledDeltaTimes)}");
        }

        private static IEnumerator SampleRendererMotionRuntime(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            FastVsVisualDirectionGuide guide,
            int frameCount,
            string label)
        {
            var stats = BuildRendererFlickerStats();
            var visibleCounts = new int[Mathf.Max(frameCount, 1)];
            var enabledCounts = new int[Mathf.Max(frameCount, 1)];
            var backgroundVisibleCounts = new int[Mathf.Max(frameCount, 1)];
            var deltaTimes = new float[Mathf.Max(frameCount, 1)];
            var unscaledDeltaTimes = new float[Mathf.Max(frameCount, 1)];
            var visibleHashChanges = 0;
            var backgroundHashChanges = 0;
            uint previousVisibleHash = 0;
            uint previousBackgroundHash = 0;
            var previousVisiblePaths = new List<string>();

            for (var frame = 0; frame < frameCount; frame++)
            {
                var t = frameCount <= 1 ? 1f : frame / (float)(frameCount - 1);
                var playerLocal = EvaluateRendererMotionProbeLocal(t);
                PositionRendererMotionProbeCamera(camera, controller, guide, playerLocal);
                yield return null;
                RenderCameraForVisibilitySample(camera);

                var visiblePaths = new List<string>();
                var backgroundPaths = new List<string>();
                var enabledCount = 0;
                foreach (var item in stats.Values)
                {
                    item.Sample(frame);
                    if (item.LastEnabled)
                    {
                        enabledCount++;
                    }

                    if (!item.LastVisible)
                    {
                        continue;
                    }

                    visiblePaths.Add(item.Path);
                    if (IsRendererMotionBackgroundPath(item.Path))
                    {
                        backgroundPaths.Add(item.Path);
                    }
                }

                visiblePaths.Sort(StringComparer.Ordinal);
                backgroundPaths.Sort(StringComparer.Ordinal);
                var visibleHash = BuildStableStringHash(visiblePaths);
                var backgroundHash = BuildStableStringHash(backgroundPaths);
                if (frame > 0 && visibleHash != previousVisibleHash)
                {
                    visibleHashChanges++;
                    Debug.Log($"{RendererMotionProbeMarker}: runtimeVisibleDelta phase={label} frame={frame}");
                    LogVisiblePathDelta(frame, previousVisiblePaths, visiblePaths);
                }

                if (frame > 0 && backgroundHash != previousBackgroundHash)
                {
                    backgroundHashChanges++;
                    Debug.Log(
                        $"{RendererMotionProbeMarker}: runtimeBackgroundVisibleDelta phase={label} frame={frame} " +
                        $"previous={previousBackgroundHash:X8} current={backgroundHash:X8} currentSample=[{BuildPathSample(backgroundPaths, 10)}]");
                }

                visibleCounts[frame] = visiblePaths.Count;
                enabledCounts[frame] = enabledCount;
                backgroundVisibleCounts[frame] = backgroundPaths.Count;
                deltaTimes[frame] = Time.deltaTime;
                unscaledDeltaTimes[frame] = Time.unscaledDeltaTime;

                if (frame == 0 || frame == frameCount - 1 || frame % 30 == 0)
                {
                    Debug.Log(
                        $"{RendererMotionProbeMarker}: runtimeFrame phase={label} index={frame} deltaTime={Time.deltaTime:0.0000} unscaledDeltaTime={Time.unscaledDeltaTime:0.0000} realtime={Time.realtimeSinceStartup:0.000} " +
                        $"visible={visiblePaths.Count} enabled={enabledCount} backgroundVisible={backgroundPaths.Count} visibleHash=0x{visibleHash:X8} backgroundHash=0x{backgroundHash:X8} " +
                        $"playerLocal={FormatVector(playerLocal)} cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)} fov={camera.fieldOfView:0.000} mask={camera.cullingMask}");
                }

                previousVisibleHash = visibleHash;
                previousBackgroundHash = backgroundHash;
                previousVisiblePaths = visiblePaths;
            }

            LogRendererMotionToggleSummary(stats, frameCount, label);
            Debug.Log(
                $"{RendererMotionProbeMarker}: runtimeSummary phase={label} frames={frameCount} tracked={stats.Count} " +
                $"visible={BuildSeriesSummary(visibleCounts)} enabled={BuildSeriesSummary(enabledCounts)} backgroundVisible={BuildSeriesSummary(backgroundVisibleCounts)} " +
                $"visibleHashChanges={visibleHashChanges} backgroundHashChanges={backgroundHashChanges} " +
                $"deltaTime={BuildSeriesSummary(deltaTimes)} unscaledDeltaTime={BuildSeriesSummary(unscaledDeltaTimes)}");
        }

        private static IEnumerator SampleRendererStaticRuntime(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            FastVsVisualDirectionGuide guide,
            string outputDirectory,
            string label,
            Vector3 playerLocal,
            int frameCount,
            int captureInterval,
            int[] captureCount)
        {
            Debug.Log($"{RendererMotionProbeMarker}: staticPhaseBegin phase={label} playerLocal={FormatVector(playerLocal)}");
            var stats = BuildRendererFlickerStats();
            var safeFrameCount = Mathf.Max(frameCount, 1);
            var visibleCounts = new int[safeFrameCount];
            var enabledCounts = new int[safeFrameCount];
            var backgroundVisibleCounts = new int[safeFrameCount];
            var deltaTimes = new float[safeFrameCount];
            var unscaledDeltaTimes = new float[safeFrameCount];
            var imageMeanAbs = new List<float>();
            var imageChangedPct = new List<float>();
            var visibleHashChanges = 0;
            var backgroundHashChanges = 0;
            uint previousVisibleHash = 0;
            uint previousBackgroundHash = 0;
            var previousVisiblePaths = new List<string>();
            FramePixelSample previousImage = null;

            for (var frame = 0; frame < safeFrameCount; frame++)
            {
                yield return null;
                PositionRendererMotionProbeCamera(camera, controller, guide, playerLocal);

                var shouldCapture = frame == 0 || frame == safeFrameCount - 1 || frame % Mathf.Max(captureInterval, 1) == 0;
                FramePixelSample image;
                if (shouldCapture)
                {
                    var fileName = $"{captureCount[0]:00}_{SanitizeFileName(label)}_frame_{frame:000}.png";
                    image = WriteCameraPngAndSample(camera, Path.Combine(outputDirectory, fileName), 8);
                    Debug.Log(
                        $"{RendererMotionProbeMarker}: staticCapture phase={label} frame={frame} saved={fileName} " +
                        $"playerLocal={FormatVector(playerLocal)} cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)}");
                    captureCount[0]++;
                }
                else
                {
                    image = SampleCameraPixels(camera, 8);
                }

                var imageDelta = previousImage == null ? FrameImageDelta.Empty : CompareFrameSamples(previousImage, image);
                if (previousImage != null)
                {
                    imageMeanAbs.Add(imageDelta.MeanAbsRgb);
                    imageChangedPct.Add(imageDelta.ChangedSamplePct);
                }

                previousImage = image;

                var visiblePaths = new List<string>();
                var backgroundPaths = new List<string>();
                var enabledCount = 0;
                foreach (var item in stats.Values)
                {
                    item.Sample(frame);
                    if (item.LastEnabled)
                    {
                        enabledCount++;
                    }

                    if (!item.LastVisible)
                    {
                        continue;
                    }

                    visiblePaths.Add(item.Path);
                    if (IsRendererMotionBackgroundPath(item.Path))
                    {
                        backgroundPaths.Add(item.Path);
                    }
                }

                visiblePaths.Sort(StringComparer.Ordinal);
                backgroundPaths.Sort(StringComparer.Ordinal);
                var visibleHash = BuildStableStringHash(visiblePaths);
                var backgroundHash = BuildStableStringHash(backgroundPaths);
                if (frame > 0 && visibleHash != previousVisibleHash)
                {
                    visibleHashChanges++;
                    Debug.Log($"{RendererMotionProbeMarker}: staticVisibleDelta phase={label} frame={frame}");
                    LogVisiblePathDelta(frame, previousVisiblePaths, visiblePaths);
                }

                if (frame > 0 && backgroundHash != previousBackgroundHash)
                {
                    backgroundHashChanges++;
                    Debug.Log(
                        $"{RendererMotionProbeMarker}: staticBackgroundVisibleDelta phase={label} frame={frame} " +
                        $"previous={previousBackgroundHash:X8} current={backgroundHash:X8} currentSample=[{BuildPathSample(backgroundPaths, 10)}]");
                }

                visibleCounts[frame] = visiblePaths.Count;
                enabledCounts[frame] = enabledCount;
                backgroundVisibleCounts[frame] = backgroundPaths.Count;
                deltaTimes[frame] = Time.deltaTime;
                unscaledDeltaTimes[frame] = Time.unscaledDeltaTime;

                if (frame == 0 || frame == safeFrameCount - 1 || frame % 30 == 0)
                {
                    Debug.Log(
                        $"{RendererMotionProbeMarker}: staticFrame phase={label} index={frame} deltaTime={Time.deltaTime:0.0000} unscaledDeltaTime={Time.unscaledDeltaTime:0.0000} realtime={Time.realtimeSinceStartup:0.000} " +
                        $"visible={visiblePaths.Count} enabled={enabledCount} backgroundVisible={backgroundPaths.Count} visibleHash=0x{visibleHash:X8} backgroundHash=0x{backgroundHash:X8} " +
                        $"imageMeanAbsRgb={imageDelta.MeanAbsRgb:0.000} imageChangedSamplePct={imageDelta.ChangedSamplePct:0.000} " +
                        $"playerLocal={FormatVector(playerLocal)} cameraPos={FormatVector(camera.transform.position)} cameraEuler={FormatVector(camera.transform.rotation.eulerAngles)} fov={camera.fieldOfView:0.000} mask={camera.cullingMask}");
                }

                previousVisibleHash = visibleHash;
                previousBackgroundHash = backgroundHash;
                previousVisiblePaths = visiblePaths;
            }

            LogRendererMotionToggleSummary(stats, safeFrameCount, label);
            Debug.Log(
                $"{RendererMotionProbeMarker}: staticSummary phase={label} frames={safeFrameCount} tracked={stats.Count} " +
                $"visible={BuildSeriesSummary(visibleCounts)} enabled={BuildSeriesSummary(enabledCounts)} backgroundVisible={BuildSeriesSummary(backgroundVisibleCounts)} " +
                $"visibleHashChanges={visibleHashChanges} backgroundHashChanges={backgroundHashChanges} " +
                $"imageMeanAbsRgb={BuildSeriesSummary(imageMeanAbs.ToArray())} imageChangedSamplePct={BuildSeriesSummary(imageChangedPct.ToArray())} " +
                $"deltaTime={BuildSeriesSummary(deltaTimes)} unscaledDeltaTime={BuildSeriesSummary(unscaledDeltaTimes)}");
        }

        private static Vector3 EvaluateRendererMotionProbeLocal(float t)
        {
            var eased = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
            var start = CentralPlazaVsCenter + new Vector3(-2.70f, 0.02f, -2.10f);
            var mid = CentralPlazaVsCenter + new Vector3(0.60f, 0.02f, 1.85f);
            var end = CentralPlazaVsCenter + new Vector3(0.15f, 0.02f, 6.25f);
            if (eased < 0.55f)
            {
                return Vector3.Lerp(start, mid, eased / 0.55f);
            }

            return Vector3.Lerp(mid, end, (eased - 0.55f) / 0.45f);
        }

        private static void PositionRendererMotionProbeCamera(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            FastVsVisualDirectionGuide guide,
            Vector3 playerLocal)
        {
            controller.ForcePlayerCurrentLocalForReview(playerLocal);
            guide.ApplyActiveTimeIsolationForReview();
            camera.orthographic = false;
            camera.fieldOfView = RuntimeVsFollowCameraFov;
            PositionReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(playerLocal));
        }

        private static bool IsRendererMotionBackgroundPath(string path)
        {
            return ContainsAny(
                path,
                "OutdoorBackgroundSkyDepth",
                "OutdoorSkyDetail",
                "OutdoorSkyHorizonLayering",
                "OutdoorSkyAtmosphereDepth",
                "ScenicBackdrop",
                "BackdropFoundation",
                "OutdoorVoidBackground");
        }

        private static bool IsRendererMotionDynamicCharacterPath(string path)
        {
            return ContainsAny(
                path,
                "FastVS_Player_NiroHouseSlice",
                "FastVS_PlayerVisual_NiroPaper",
                "Niro_Sprite64x96",
                "FastVS_PlayerContactShadow_Niro",
                "FastVS_PlayerGroundBounce_Niro",
                "FastVS_PlayerFootContact_Niro",
                "Current_CentralPlaza_Cycle119_ReferenceComposite_SunFleckPlayerA",
                "Current_CentralPlaza_Cycle121_LegacySunRibbonCleanSun_PlayerShoulderFleckA",
                "Current_CentralPlaza_Cycle121_LegacySunRibbonLineKill_PlayerLineClampA",
                "Current_CentralPlaza_Cycle123_ReferenceAerialLift_PlayerLanePaleCatchA",
                "Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_PlayerTinyContactA",
                "Current_CentralPlaza_Cycle126_CloseShadowBarMute_PlayerReferenceContactA",
                "FastVS_SpriteCharacter_");
        }

        private static void LogRendererMotionBackgroundDiagnosticTargets()
        {
            var renderers = FindSceneRenderers();
            var matched = 0;
            foreach (var renderer in renderers)
            {
                if (renderer == null || renderer.gameObject == null || !renderer.gameObject.scene.IsValid())
                {
                    continue;
                }

                var path = BuildHierarchyPath(renderer.transform);
                if (!IsCurrentCentralPlazaOutdoorVoidBackgroundPath(path))
                {
                    continue;
                }

                matched++;
                Debug.Log(
                    $"{RendererMotionProbeMarker}: backgroundDiagnostic index={matched - 1} " +
                    $"active={renderer.gameObject.activeInHierarchy} enabled={renderer.enabled} layer={renderer.gameObject.layer} " +
                    $"path=\"{path}\" position={FormatVector(renderer.transform.position)} localPosition={FormatVector(renderer.transform.localPosition)} " +
                    $"rotation={FormatVector(renderer.transform.rotation.eulerAngles)} localScale={FormatVector(renderer.transform.localScale)} " +
                    $"bounds={FormatBounds(renderer.bounds)} mat={DescribePrimaryMaterial(renderer)}");
            }

            Debug.Log($"{RendererMotionProbeMarker}: backgroundDiagnosticSummary matched={matched}");
        }

        private static void CaptureRendererMotionIsolationVariant(
            string variant,
            Predicate<string> shouldDisable,
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            FastVsVisualDirectionGuide guide,
            string outputDirectory,
            Dictionary<int, FramePixelSample> baselineCapturedFrames,
            Dictionary<int, Vector3> baselineCameraPositions,
            Dictionary<int, Quaternion> baselineCameraRotations,
            Dictionary<int, Vector3> baselinePlayerLocals,
            ref int captureCount)
        {
            var disabledRenderers = DisableRenderersForIsolation(variant, shouldDisable);
            try
            {
                var frames = new[] { 0, 75, 90, 105, 120, 150, 165, 179 };
                for (var index = 0; index < frames.Length; index++)
                {
                    var frame = frames[index];
                    var playerLocal = baselinePlayerLocals.TryGetValue(frame, out var capturedPlayerLocal)
                        ? capturedPlayerLocal
                        : EvaluateRendererMotionProbeLocal(frame / 179f);
                    controller.ForcePlayerCurrentLocalForReview(playerLocal);
                    guide.ApplyActiveTimeIsolationForReview();
                    camera.orthographic = false;
                    camera.fieldOfView = RuntimeVsFollowCameraFov;
                    if (baselineCameraPositions.TryGetValue(frame, out var cameraPosition) &&
                        baselineCameraRotations.TryGetValue(frame, out var cameraRotation))
                    {
                        camera.transform.SetPositionAndRotation(cameraPosition, cameraRotation);
                    }
                    else
                    {
                        PositionReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(playerLocal));
                    }

                    var fileName = $"{captureCount:00}_{variant}_frame_{frame:000}.png";
                    var current = WriteCameraPngAndSample(camera, Path.Combine(outputDirectory, fileName), 8);
                    var baseline = baselineCapturedFrames.TryGetValue(frame, out var capturedBaseline)
                        ? capturedBaseline
                        : null;
                    var delta = baseline == null ? FrameImageDelta.Empty : CompareFrameSamples(baseline, current);
                    captureCount++;

                    Debug.Log(
                        $"{RendererMotionProbeMarker}: isolationVariant={variant} frame={frame} saved={fileName} " +
                        $"disabled={disabledRenderers.Count} baselineDelta meanAbsRgb={delta.MeanAbsRgb:0.000} " +
                        $"changedSamplePct={delta.ChangedSamplePct:0.000} changed={delta.ChangedSamples} samples={delta.TotalSamples} " +
                        $"playerLocal={FormatVector(playerLocal)} cameraPos={FormatVector(camera.transform.position)}");
                }
            }
            finally
            {
                RestoreRenderersForIsolation(variant, disabledRenderers);
            }
        }

        private static LibraryRearCloseView[] BuildLibraryRearCloseViews()
        {
            return new[]
            {
                new LibraryRearCloseView(
                    "rearCenter",
                    CentralPlazaVsCenter + new Vector3(0.00f, 3.42f, 4.42f),
                    CentralPlazaVsCenter + new Vector3(0.00f, 2.26f, 9.82f),
                    27f),
                new LibraryRearCloseView(
                    "rearEastOblique",
                    CentralPlazaVsCenter + new Vector3(2.15f, 3.34f, 4.72f),
                    CentralPlazaVsCenter + new Vector3(4.78f, 2.34f, 9.86f),
                    28f),
                new LibraryRearCloseView(
                    "facadeUpper",
                    CentralPlazaVsCenter + new Vector3(0.00f, 3.20f, 3.72f),
                    CentralPlazaVsCenter + new Vector3(0.00f, 2.60f, 8.04f),
                    25f),
            };
        }

        private static LibraryRearCloseVariant[] BuildLibraryRearCloseVariants()
        {
            return new[]
            {
                new LibraryRearCloseVariant("libraryRearThinOpaquesOff", IsCurrentCentralPlazaLibraryRearThinOpaquePath),
                new LibraryRearCloseVariant(
                    "libraryRear_RearDustBreakOff",
                    path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibraryRearVolume_RearDustBreakA")),
                new LibraryRearCloseVariant(
                    "libraryRear_SideSurfaceRearWallBandOff",
                    path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibrarySideSurfaceBreakup_RearWallBandA")),
                new LibraryRearCloseVariant(
                    "libraryRear_RoofSideRearWallBandOff",
                    path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibraryRoofSideDepth_RearWallBandA")),
                new LibraryRearCloseVariant("libraryRear_ThinShadowBandsOff", IsCurrentCentralPlazaLibraryThinShadowBandPath),
                new LibraryRearCloseVariant(
                    "libraryRear_FacadeRoofUnderThinBandOff",
                    path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand")),
                new LibraryRearCloseVariant("libraryRear_Cycle60MidStoneCoursesOff", IsCurrentCentralPlazaLibraryCycle60MidStoneCoursePath),
                new LibraryRearCloseVariant("libraryRear_FacadeUpperDustChipsOff", IsCurrentCentralPlazaLibraryFacadeUpperDustChipPath),
                new LibraryRearCloseVariant(
                    "libraryRear_DeepRearCrackDustOff",
                    path => IsExactRendererNamePath(path, "Current_CentralPlaza_LibraryDeepExteriorVolume_RearWallCrackDustA")),
                new LibraryRearCloseVariant("libraryTransparentDepthPlanesOff", IsCurrentCentralPlazaLibraryTransparentDepthPlanePath),
                new LibraryRearCloseVariant("libraryTransparentSideCoolFalloffOff", IsCurrentCentralPlazaLibrarySideCoolFalloffPath),
                new LibraryRearCloseVariant(
                    "libraryTransparentFacadeOcclusionGradientOff",
                    path => IsExactRendererNamePath(path, "Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA")),
            };
        }

        private static FramePixelSample CaptureLibraryRearCloseView(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            LibraryRearCloseView view,
            string outputDirectory,
            string fileName)
        {
            PositionLibraryRearCloseCamera(camera, controller, view);
            return WriteCameraPngAndSample(camera, Path.Combine(outputDirectory, fileName), 4);
        }

        private static void PositionLibraryRearCloseCamera(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            LibraryRearCloseView view)
        {
            var root = controller.CurrentSpaceRootForReview;
            var cameraPosition = root.TransformPoint(view.CameraLocal);
            var lookAt = root.TransformPoint(view.TargetLocal);
            camera.orthographic = false;
            camera.fieldOfView = view.FieldOfView;
            camera.transform.SetPositionAndRotation(cameraPosition, Quaternion.LookRotation(lookAt - cameraPosition, Vector3.up));
        }

        private static void LogLibraryRearCloseCandidateDetails(string variant, Predicate<string> shouldLog, int maxItems)
        {
            var renderers = FindSceneRenderers();
            var matched = 0;
            var logged = 0;
            foreach (var renderer in renderers)
            {
                if (renderer == null || renderer.gameObject == null || !renderer.gameObject.scene.IsValid())
                {
                    continue;
                }

                var path = BuildHierarchyPath(renderer.transform);
                if (!shouldLog(path))
                {
                    continue;
                }

                matched++;
                if (logged < maxItems)
                {
                    Debug.Log($"{LibraryRearCloseProbeMarker}: candidate variant={variant} index={logged} {DescribeRenderer(renderer)}");
                    logged++;
                }
            }

            Debug.Log($"{LibraryRearCloseProbeMarker}: candidateSummary variant={variant} matched={matched} logged={logged}");
        }

        private static bool IsCurrentCentralPlazaOutdoorVoidBackgroundPath(string path)
        {
            return path.IndexOf("Current_CentralPlaza_OutdoorVoidBackground_", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsCurrentCentralPlazaOutdoorVoidBackgroundEastEdgeWashPath(string path)
        {
            return path.IndexOf("Current_CentralPlaza_OutdoorVoidBackground_EastEdgeWash", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsCurrentCentralPlazaOutdoorVoidBackgroundNorthSilhouettePath(string path)
        {
            return path.IndexOf("Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouette", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsCurrentCentralPlazaFrontRoadLongThinGroundPath(string path)
        {
            return ContainsAny(
                path,
                "Current_CentralPlaza_Cycle62_OuterGroundSkirt_EastStreetContinuationA",
                "Current_CentralPlaza_Cycle43_OuterEastShelfA",
                "Current_CentralPlaza_EdgeDressing_EastLowWall",
                "Current_CentralPlaza_OutdoorWorldEnvelope_RightPerimeterShoulderB");
        }

        private static bool IsCentralPlazaRightOuterRoadShoulderIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Cycle43_OuterEastShelfA",
                "CentralPlaza_Cycle47_GroundSkirtEastA",
                "CentralPlaza_Cycle62_OuterGroundSkirt_EastStreetContinuationA",
                "CentralPlaza_EdgeDressing_EastLowWall",
                "CentralPlaza_OutdoorWorldEnvelope_RightPerimeterShoulderA",
                "CentralPlaza_OutdoorWorldEnvelope_RightPerimeterShoulderB",
                "CentralPlaza_OutdoorWorldGrounding_EastShoulderA");
        }

        private static bool IsCentralPlazaRightRuinBlockIsolationTarget(string path)
        {
            return ContainsAny(path, "CentralPlaza_Chapter1_B_RightRuinBlock");
        }

        private static bool IsCentralPlazaRightLibrarySideDepthStackIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Chapter1_B_RightRuinBlock",
                "CentralPlaza_LibraryEastWing",
                "CentralPlaza_LibraryOcclusionShell_EastSideReturn",
                "CentralPlaza_Cycle56_LibraryMassClosure_EastFrontSideFillA",
                "CentralPlaza_PlazaLibraryVerticality_RightUpperReturnA",
                "CentralPlaza_SurfaceDepth_LibraryEastTowerReturnShadeA",
                "CentralPlaza_Cycle58_LibrarySideRelief_East",
                "CentralPlaza_Cycle59_LibraryRoofUndersideShadow_East",
                "CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_East",
                "CentralPlaza_LibrarySideWallMasonryRelief_East");
        }

        private static bool IsCentralPlazaRightLotHardscapeIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Chapter1_Cycle80_RightLotFloor",
                "CentralPlaza_Chapter1_Cycle80_RightLotBackGap",
                "CentralPlaza_Chapter1_Cycle80_RightLotFoundationEast",
                "CentralPlaza_Chapter1_Cycle80_RightLotFoundationSouth",
                "CentralPlaza_Chapter1_Cycle80_RightLotThreshold",
                "CentralPlaza_Chapter1_Cycle80_RightLotBrokenEdge");
        }

        private static bool IsCentralPlazaRightLotPlantingIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Chapter1_Cycle79_RightFootprintPlantBed",
                "CentralPlaza_Chapter1_Cycle80_RightLotPlantBed",
                "CentralPlaza_Chapter1_Cycle80_RightLotScatter");
        }

        private static bool IsCentralPlazaRightOuterCycle4347IsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Cycle43_OuterEastShelfA",
                "CentralPlaza_Cycle47_GroundSkirtEastA",
                "CentralPlaza_Cycle47_GroundShelfRearRightA");
        }

        private static bool IsCentralPlazaRightOuterCycle62IsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Cycle62_OuterGroundSkirt_EastStreetContinuationA",
                "CentralPlaza_Cycle62_OuterGroundSkirt_NorthEastCornerChipA");
        }

        private static bool IsCentralPlazaOutdoorWorldEnvelopeRightIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_OutdoorWorldEnvelope_RightPerimeterShoulderA",
                "CentralPlaza_OutdoorWorldEnvelope_RightPerimeterShoulderB");
        }

        private static bool IsCentralPlazaOutdoorWorldRearSideRidgeRightIsolationTarget(string path)
        {
            return ContainsAny(path, "CentralPlaza_OutdoorWorldEnvelope_RearSideRidgeB");
        }

        private static bool IsCentralPlazaCycle63ScenicHorizonEastIsolationTarget(string path)
        {
            return ContainsAny(path, "CentralPlaza_Cycle63_ScenicHorizonGrounding_East");
        }

        private static bool IsCentralPlazaOutdoorVoidNorthSilhouetteRightIsolationTarget(string path)
        {
            return ContainsAny(path, "CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight");
        }

        private static bool IsCentralPlazaFarRightLooseDepthComboIsolationTarget(string path)
        {
            return IsCentralPlazaRightOuterCycle4347IsolationTarget(path) ||
                IsCentralPlazaRightOuterCycle62IsolationTarget(path) ||
                IsCentralPlazaOutdoorWorldRearSideRidgeRightIsolationTarget(path) ||
                IsCentralPlazaCycle63ScenicHorizonEastIsolationTarget(path) ||
                IsCentralPlazaOutdoorVoidNorthSilhouetteRightIsolationTarget(path) ||
                ContainsAny(
                    path,
                    "CentralPlaza_HorizonScenicDepth_FarRightBlockA",
                    "CentralPlaza_HorizonScenicDepth_RightParapetA");
        }

        private static bool IsCentralPlazaEastPerimeterFieldIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Cycle53_PerimeterWorld_EastFieldA");
        }

        private static bool IsCentralPlazaPerimeterBackFieldIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Cycle53_PerimeterWorld_BackFieldA");
        }

        private static bool IsCentralPlazaPerimeterBackRidgeIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Cycle53_PerimeterWorld_BackRidgeA");
        }

        private static bool IsCentralPlazaPerimeterBackFieldRidgeIsolationTarget(string path)
        {
            return IsCentralPlazaPerimeterBackFieldIsolationTarget(path) ||
                IsCentralPlazaPerimeterBackRidgeIsolationTarget(path);
        }

        private static bool IsCentralPlazaBackPathBandsIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_Cycle54_EdgeBreakup_BackRightTerraceA",
                "CentralPlaza_Cycle55_HorizonSilhouette_BackPathBandA");
        }

        private static bool IsCentralPlazaHorizonRightDepthIsolationTarget(string path)
        {
            return ContainsAny(
                path,
                "CentralPlaza_HorizonScenicDepth_FarRightBlockA",
                "CentralPlaza_HorizonScenicDepth_RightDetailClusterA",
                "CentralPlaza_HorizonScenicDepth_RightDetailClusterA_Accent",
                "CentralPlaza_HorizonScenicDepth_RightDetailClusterA_Shadow",
                "CentralPlaza_HorizonScenicDepth_RightParapetA");
        }

        private static bool IsCurrentCentralPlazaLibraryTransparentDepthPlanePath(string path)
        {
            return ContainsAny(
                path,
                "Current_CentralPlaza_Cycle59_LibraryRoofUndersideShadow_EastSideWallCoolFalloffA",
                "Current_CentralPlaza_Cycle59_LibraryRoofUndersideShadow_WestSideWallCoolFalloffA",
                "Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA");
        }

        private static bool IsCurrentCentralPlazaLibraryRearThinOpaquePath(string path)
        {
            return ContainsAny(
                path,
                "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseA",
                "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseA",
                "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseB",
                "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseB",
                "Current_CentralPlaza_LibrarySideSurfaceBreakup_RearWallBandA",
                "Current_CentralPlaza_LibraryRoofSideDepth_RearWallBandA",
                "Current_CentralPlaza_LibraryEntryDepth_RoofLipUndersideShadowA",
                "Current_CentralPlaza_LibraryFrontDepth_UnderEaveDepthLineA",
                "Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand",
                "Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA");
        }

        private static bool IsCurrentCentralPlazaLibraryCycle60MidStoneCoursePath(string path)
        {
            return ContainsAny(
                path,
                "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseA",
                "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseA",
                "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseB",
                "Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseB");
        }

        private static bool IsCurrentCentralPlazaLibraryThinShadowBandPath(string path)
        {
            return ContainsAny(
                path,
                "Current_CentralPlaza_LibrarySideSurfaceBreakup_RearWallBandA",
                "Current_CentralPlaza_LibraryRoofSideDepth_RearWallBandA",
                "Current_CentralPlaza_LibraryEntryDepth_RoofLipUndersideShadowA",
                "Current_CentralPlaza_LibraryFrontDepth_UnderEaveDepthLineA");
        }

        private static bool IsCurrentCentralPlazaLibraryFacadeUpperDustChipPath(string path)
        {
            return ContainsAny(
                path,
                "Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipLeftA",
                "Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipCenterA",
                "Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipRightA");
        }

        private static bool IsCurrentCentralPlazaLibrarySideCoolFalloffPath(string path)
        {
            return ContainsAny(
                path,
                "Current_CentralPlaza_Cycle59_LibraryRoofUndersideShadow_EastSideWallCoolFalloffA",
                "Current_CentralPlaza_Cycle59_LibraryRoofUndersideShadow_WestSideWallCoolFalloffA");
        }

        private static bool IsExactRendererNamePath(string path, string rendererName)
        {
            return string.Equals(path, rendererName, StringComparison.Ordinal) ||
                path.EndsWith("/" + rendererName, StringComparison.Ordinal);
        }

        private static void LogRendererMotionToggleSummary(Dictionary<int, RendererFlickerStats> stats, int frameCount, string label)
        {
            var toggles = new List<RendererFlickerStats>();
            foreach (var item in stats.Values)
            {
                if (item.VisibleToggleCount > 0 || item.EnabledToggleCount > 0)
                {
                    toggles.Add(item);
                }
            }

            toggles.Sort((a, b) =>
            {
                var byToggle = (b.VisibleToggleCount + b.EnabledToggleCount).CompareTo(a.VisibleToggleCount + a.EnabledToggleCount);
                return byToggle != 0 ? byToggle : string.Compare(a.Path, b.Path, StringComparison.Ordinal);
            });

            var logged = Mathf.Min(toggles.Count, 140);
            for (var index = 0; index < logged; index++)
            {
                var item = toggles[index];
                Debug.Log(
                    $"{RendererMotionProbeMarker}: rendererToggle phase={label} index={index} visibleToggles={item.VisibleToggleCount} enabledToggles={item.EnabledToggleCount} " +
                    $"visibleFrames={item.VisibleFrames}/{frameCount} enabledFrames={item.EnabledFrames}/{frameCount} visiblePattern={item.VisiblePattern} " +
                    $"isBackgroundCandidate={IsRendererMotionBackgroundPath(item.Path)} path=\"{item.Path}\" mat={DescribePrimaryMaterial(item.Renderer)} bounds={FormatBounds(item.Bounds)}");
            }

            Debug.Log($"{RendererMotionProbeMarker}: toggleSummary phase={label} toggled={toggles.Count} logged={logged}");
        }

        private static void LogRendererFlickerToggleSummary(Dictionary<int, RendererFlickerStats> stats, int frameCount, string label)
        {
            var toggles = new List<RendererFlickerStats>();
            foreach (var item in stats.Values)
            {
                if (item.VisibleToggleCount > 0 || item.EnabledToggleCount > 0)
                {
                    toggles.Add(item);
                }
            }

            toggles.Sort((a, b) =>
            {
                var byToggle = (b.VisibleToggleCount + b.EnabledToggleCount).CompareTo(a.VisibleToggleCount + a.EnabledToggleCount);
                return byToggle != 0 ? byToggle : string.Compare(a.Path, b.Path, StringComparison.Ordinal);
            });

            var logged = Mathf.Min(toggles.Count, 120);
            for (var index = 0; index < logged; index++)
            {
                var item = toggles[index];
                Debug.Log(
                    $"{RendererFlickerProbeMarker}: rendererToggle phase={label} index={index} visibleToggles={item.VisibleToggleCount} enabledToggles={item.EnabledToggleCount} " +
                    $"visibleFrames={item.VisibleFrames}/{frameCount} enabledFrames={item.EnabledFrames}/{frameCount} visiblePattern={item.VisiblePattern} " +
                    $"path=\"{item.Path}\" mat={DescribePrimaryMaterial(item.Renderer)} bounds={FormatBounds(item.Bounds)}");
            }

            Debug.Log($"{RendererFlickerProbeMarker}: toggleSummary phase={label} toggled={toggles.Count} logged={logged}");
        }

        private static void PositionReviewCamera(Camera camera, Vector3 anchor)
        {
            camera.orthographic = false;
            var position = anchor + new Vector3(0f, 2.75f, -4.55f);
            var lookAt = anchor + new Vector3(0f, 0.72f, 0.45f);
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
        }

        private static string BuildTimeWindowProbeState(TimeWindowPairedSpacePortalController controller)
        {
            var local = controller.GetPlayerLocalCoordinateForReview();
            var portal = controller.PortalLocalCenterForReview;
            var size = controller.PortalSizeForReview;
            return
                $"hasPair={controller.HasPortalPair} playerInOtherTime={controller.PlayerInOtherTime} " +
                $"playerLocal={FormatVector(local)} portalLocal={FormatVector(portal)} portalSize=({size.x:0.000},{size.y:0.000}) " +
                $"outsideRejected={controller.OutsideCrossingRejected} backRejected={controller.BackSideCrossingRejected} " +
                $"lastTransition=\"{controller.LastTransitionForReview}\" " +
                $"apertureLive={controller.HasLiveApertureViewForReview} apertureEnabled={controller.EnabledApertureRendererCountForReview} " +
                $"currentIncludesPlayer={controller.CurrentToOtherApertureIncludesPlayerForReview} otherIncludesPlayer={controller.OtherToCurrentApertureIncludesPlayerForReview} " +
                $"currentMask={controller.CurrentToOtherPortalCameraCullingMaskForReview} otherMask={controller.OtherToCurrentPortalCameraCullingMaskForReview} " +
                $"playerLayer={controller.PlayerRenderLayerForReview} playerVisibleLayer={controller.PlayerVisibleRenderLayerForReview} " +
                $"suppressedRenderers={controller.ApertureSuppressedRendererCountForReview} " +
                $"suppressedSample=\"{controller.ApertureSuppressedRendererSummaryForReview}\" " +
                $"visualOverlayExemptedSample=\"{controller.ApertureVisualOverlayExemptionSummaryForReview}\" " +
                $"currentBackColliders={controller.CurrentBackSidePhysicalBlockColliderCountForReview} enabledCurrentBackColliders={controller.EnabledCurrentBackSidePhysicalBlockColliderCountForReview} " +
                $"wallColliders={controller.OtherTimeWallVolumeColliderCountForReview} enabledWallColliders={controller.EnabledOtherTimeWallVolumeColliderCountForReview} " +
                $"wallSummary=\"{controller.OtherTimeWallVolumeSummaryForReview}\"";
        }

        private static string BuildTimeWindowVisualPhysicsState(TimeWindowPairedSpacePortalController controller, Camera camera)
        {
            var playerObject = GameObject.FindWithTag("Player");
            var playerRenderer = playerObject != null ? playerObject.GetComponentInChildren<Renderer>() : null;
            var playerLayer = playerObject != null ? playerObject.layer : -1;
            var viewport = Vector3.zero;
            var mainCameraIncludesPlayerLayer = false;
            if (camera != null && playerRenderer != null)
            {
                viewport = camera.WorldToViewportPoint(playerRenderer.bounds.center);
                mainCameraIncludesPlayerLayer = MaskIncludesLayerForReview(camera.cullingMask, playerLayer);
            }

            return
                $"{BuildTimeWindowProbeState(controller)} " +
                $"playerRendererActive={(playerRenderer != null && playerRenderer.gameObject.activeInHierarchy)} " +
                $"playerRendererEnabled={(playerRenderer != null && playerRenderer.enabled)} " +
                $"playerRendererVisible={(playerRenderer != null && playerRenderer.isVisible)} " +
                $"playerRendererLayer={playerLayer} playerViewport={FormatVector(viewport)} " +
                $"mainCameraMask={camera?.cullingMask ?? 0} mainCameraIncludesPlayerLayer={mainCameraIncludesPlayerLayer}";
        }

        private static bool MaskIncludesLayerForReview(int mask, int layer)
        {
            return layer >= 0 && layer < 32 && (mask & (1 << layer)) != 0;
        }

        private static void LogLightingState(string label, FastVsHouseArea expectedArea)
        {
            var areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var activeArea = areaVisibility != null ? areaVisibility.ActiveAreaForReview : expectedArea;
            var sunCycle = AnemoraSunCycleDriver.Instance != null
                ? AnemoraSunCycleDriver.Instance
                : FindFirstObjectByType<AnemoraSunCycleDriver>();

            Debug.Log(
                $"{LightingStateMarker}: label={label} expectedArea={expectedArea} activeArea={activeArea} " +
                $"sunCurrent={(sunCycle != null ? sunCycle.CurrentPreset.ToString() : "<none>")} " +
                $"sunTarget={(sunCycle != null ? sunCycle.TargetPreset.ToString() : "<none>")} " +
                $"sunTransitioning={(sunCycle != null && sunCycle.IsTransitioning)} " +
                $"indoorSunSuppression={(sunCycle != null && sunCycle.IndoorSunSuppressionActiveForReview)} " +
                $"main={DescribeLight(FindLightByName("Directional Light"))} " +
                $"warm={DescribeLight(FindLightByName("FastVS_HD2D_WarmFillLight"))} " +
                $"cool={DescribeLight(FindLightByName("FastVS_HD2D_CoolRimLight"))} " +
                $"ambientMode={RenderSettings.ambientMode} ambient={FormatColor(RenderSettings.ambientLight)} " +
                $"ambientMax={RenderSettings.ambientLight.maxColorComponent:0.000} " +
                $"fog={RenderSettings.fog} fogColor={FormatColor(RenderSettings.fogColor)} fogDensity={RenderSettings.fogDensity:0.000} " +
                $"shadowPolicy={BuildShadowPolicySummary()}");
        }

        private static string DescribeLight(Light light)
        {
            if (light == null)
            {
                return "<missing>";
            }

            return
                $"name={light.name},enabled={light.enabled},type={light.type},intensity={light.intensity:0.000}," +
                $"color={FormatColor(light.color)},shadows={light.shadows},shadowStrength={light.shadowStrength:0.000}," +
                $"shadowBias={light.shadowBias:0.000},shadowNormalBias={light.shadowNormalBias:0.000}";
        }

        private static Light FindLightByName(string objectName)
        {
            var gameObject = GameObject.Find(objectName);
            return gameObject != null ? gameObject.GetComponent<Light>() : null;
        }

        private static string BuildShadowPolicySummary()
        {
            var renderers = FindSceneRenderers();
            var active = 0;
            var enabled = 0;
            var castOn = 0;
            var castShadowsOnly = 0;
            var receive = 0;
            var activeCastOn = 0;
            var activeReceive = 0;
            for (var index = 0; index < renderers.Count; index++)
            {
                var renderer = renderers[index];
                if (renderer == null)
                {
                    continue;
                }

                var activeInHierarchy = renderer.gameObject.activeInHierarchy;
                if (activeInHierarchy)
                {
                    active++;
                }

                if (renderer.enabled)
                {
                    enabled++;
                }

                if (renderer.shadowCastingMode == ShadowCastingMode.On)
                {
                    castOn++;
                    if (activeInHierarchy)
                    {
                        activeCastOn++;
                    }
                }
                else if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
                {
                    castShadowsOnly++;
                }

                if (renderer.receiveShadows)
                {
                    receive++;
                    if (activeInHierarchy)
                    {
                        activeReceive++;
                    }
                }
            }

            return
                $"renderers={renderers.Count},active={active},enabled={enabled}," +
                $"castOn={castOn},shadowsOnly={castShadowsOnly},receive={receive}," +
                $"activeCastOn={activeCastOn},activeReceive={activeReceive}";
        }

        private static string FormatColor(Color value)
        {
            return $"({value.r:0.000},{value.g:0.000},{value.b:0.000},{value.a:0.000})";
        }

        private static string FormatColor32(Color32 value)
        {
            return $"({value.r},{value.g},{value.b},{value.a})";
        }

        private static string FormatVector(Vector3 value)
        {
            return $"({value.x:0.000},{value.y:0.000},{value.z:0.000})";
        }

        private static List<Renderer> FindSceneRenderers()
        {
            var result = new List<Renderer>();
            var renderers = Resources.FindObjectsOfTypeAll<Renderer>();
            foreach (var renderer in renderers)
            {
                if (renderer == null || renderer.gameObject == null || !renderer.gameObject.scene.IsValid())
                {
                    continue;
                }

                result.Add(renderer);
            }

            return result;
        }

        private static bool ShouldIncludeRendererDiagnostic(Renderer renderer)
        {
            if (renderer == null || renderer.gameObject == null || !renderer.gameObject.scene.IsValid())
            {
                return false;
            }

            var path = BuildHierarchyPath(renderer.transform);
            var lower = path.ToLowerInvariant();
            var bounds = renderer.bounds;
            return ContainsAny(lower, "library", "facade", "road", "path", "floor", "dust", "decal", "paving", "shadow", "plaza", "long", "buto") ||
                   bounds.size.x >= 5.5f ||
                   bounds.size.z >= 5.5f ||
                   HasMaterialRenderConcern(renderer);
        }

        private static bool HasMaterialRenderConcern(Renderer renderer)
        {
            var materials = renderer.sharedMaterials;
            if (materials == null)
            {
                return false;
            }

            foreach (var material in materials)
            {
                if (material == null)
                {
                    continue;
                }

                var shaderName = material.shader != null ? material.shader.name : string.Empty;
                if (material.renderQueue >= 2450 ||
                    GetMaterialFloat(material, "_Surface") > 0.5f ||
                    Mathf.Approximately(GetMaterialFloat(material, "_ZWrite"), 0f) ||
                    Mathf.Approximately(GetMaterialFloat(material, "_Cull"), 0f) ||
                    shaderName.IndexOf("transparent", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static string DescribeRenderer(Renderer renderer)
        {
            return
                $"path=\"{BuildHierarchyPath(renderer.transform)}\" active={renderer.gameObject.activeInHierarchy} enabled={renderer.enabled} visible={renderer.isVisible} " +
                $"layer={renderer.gameObject.layer} sortingLayer={renderer.sortingLayerID} sortingOrder={renderer.sortingOrder} bounds={FormatBounds(renderer.bounds)} materials=[{DescribeMaterials(renderer)}]";
        }

        private static string DescribeMaterials(Renderer renderer)
        {
            var materials = renderer.sharedMaterials;
            if (materials == null || materials.Length == 0)
            {
                return "<none>";
            }

            var builder = new StringBuilder();
            for (var index = 0; index < materials.Length; index++)
            {
                if (index > 0)
                {
                    builder.Append(" | ");
                }

                builder.Append(DescribeMaterial(materials[index]));
            }

            return builder.ToString();
        }

        private static string DescribePrimaryMaterial(Renderer renderer)
        {
            var materials = renderer.sharedMaterials;
            return materials != null && materials.Length > 0
                ? DescribeMaterial(materials[0])
                : "<none>";
        }

        private static string DescribeMaterial(Material material)
        {
            if (material == null)
            {
                return "<null>";
            }

            var shaderName = material.shader != null ? material.shader.name : "<missing>";
            return
                $"name={material.name},shader={shaderName},queue={material.renderQueue},tagQueue={material.GetTag("Queue", false, "<none>")},tagRenderType={material.GetTag("RenderType", false, "<none>")}," +
                $"_Surface={FormatMaterialFloat(material, "_Surface")},_Cull={FormatMaterialFloat(material, "_Cull")},_ZWrite={FormatMaterialFloat(material, "_ZWrite")}," +
                $"_ZTest={FormatMaterialFloat(material, "_ZTest")},_AlphaClip={FormatMaterialFloat(material, "_AlphaClip")},_SrcBlend={FormatMaterialFloat(material, "_SrcBlend")},_DstBlend={FormatMaterialFloat(material, "_DstBlend")}," +
                $"_Color={FormatMaterialColor(material, "_Color")},_BaseColor={FormatMaterialColor(material, "_BaseColor")}";
        }

        private static float GetMaterialFloat(Material material, string propertyName)
        {
            return material != null && material.HasProperty(propertyName)
                ? material.GetFloat(propertyName)
                : float.NaN;
        }

        private static string FormatMaterialFloat(Material material, string propertyName)
        {
            var value = GetMaterialFloat(material, propertyName);
            return float.IsNaN(value) ? "<missing>" : value.ToString("0.###");
        }

        private static string FormatMaterialColor(Material material, string propertyName)
        {
            if (material == null || !material.HasProperty(propertyName))
            {
                return "<missing>";
            }

            var color = material.GetColor(propertyName);
            return $"({color.r:0.###},{color.g:0.###},{color.b:0.###},{color.a:0.###})";
        }

        private static string FormatBounds(Bounds bounds)
        {
            return $"center={FormatVector(bounds.center)},size={FormatVector(bounds.size)},min={FormatVector(bounds.min)},max={FormatVector(bounds.max)}";
        }

        private static string BuildHierarchyPath(Transform transform)
        {
            if (transform == null)
            {
                return "<null>";
            }

            var builder = new StringBuilder(transform.name);
            var parent = transform.parent;
            while (parent != null)
            {
                builder.Insert(0, "/");
                builder.Insert(0, parent.name);
                parent = parent.parent;
            }

            return builder.ToString();
        }

        private static bool ContainsAny(string value, params string[] needles)
        {
            for (var index = 0; index < needles.Length; index++)
            {
                if (value.IndexOf(needles[index], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static string BuildSeriesSummary(int[] values)
        {
            if (values == null || values.Length == 0)
            {
                return "min=0,max=0,mean=0.000,stddev=0.000";
            }

            var min = values[0];
            var max = values[0];
            double sum = 0;
            for (var index = 0; index < values.Length; index++)
            {
                var value = values[index];
                min = Mathf.Min(min, value);
                max = Mathf.Max(max, value);
                sum += value;
            }

            var mean = sum / values.Length;
            double variance = 0;
            for (var index = 0; index < values.Length; index++)
            {
                var delta = values[index] - mean;
                variance += delta * delta;
            }

            var stddev = Math.Sqrt(variance / values.Length);
            return $"min={min},max={max},mean={mean:0.000},stddev={stddev:0.000}";
        }

        private static string BuildSeriesSummary(float[] values)
        {
            if (values == null || values.Length == 0)
            {
                return "min=0.000,max=0.000,mean=0.000,stddev=0.000";
            }

            var min = values[0];
            var max = values[0];
            double sum = 0;
            for (var index = 0; index < values.Length; index++)
            {
                var value = values[index];
                min = Mathf.Min(min, value);
                max = Mathf.Max(max, value);
                sum += value;
            }

            var mean = sum / values.Length;
            double variance = 0;
            for (var index = 0; index < values.Length; index++)
            {
                var delta = values[index] - mean;
                variance += delta * delta;
            }

            var stddev = Math.Sqrt(variance / values.Length);
            return $"min={min:0.000},max={max:0.000},mean={mean:0.000},stddev={stddev:0.000}";
        }

        private static void CaptureChapter1AllMapsPair(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHouseArea area,
            Vector3 localPosition,
            string outputDirectory,
            string currentFileName,
            string pastFileName,
            ref int count)
        {
            CaptureChapter1AllMapsPair(controller, visibility, guide, camera, area, localPosition, outputDirectory, currentFileName, pastFileName, ref count, new Vector3(0f, 13.80f, -18.20f), new Vector3(0f, 0.20f, 1.55f));
        }

        private static void CaptureChapter1AllMapsPair(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHouseArea area,
            Vector3 localPosition,
            string outputDirectory,
            string currentFileName,
            string pastFileName,
            ref int count,
            Vector3 positionOffset,
            Vector3 lookAtOffset)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerCurrentLocalForReview(localPosition);
            guide.ApplyActiveTimeIsolationForReview();
            LogLightingState($"{area}.current.beforeCapture", area);
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(localPosition), positionOffset, lookAtOffset);
            WriteCameraPng(camera, Path.Combine(outputDirectory, currentFileName));
            count++;
            Debug.Log($"{CaptureMarker}: saved area={area} era=current path={Path.Combine(outputDirectory, currentFileName)} local={localPosition}");

            controller.ForcePlayerOtherTimeLocalForReview(localPosition);
            guide.ApplyActiveTimeIsolationForReview();
            LogLightingState($"{area}.past.beforeCapture", area);
            var previousMask = camera.cullingMask;
            camera.cullingMask = BuildOtherTimeCameraMask(previousMask, controller);
            PositionChapter1AllMapsCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(localPosition), positionOffset, lookAtOffset);
            WriteCameraPng(camera, Path.Combine(outputDirectory, pastFileName));
            count++;
            Debug.Log($"{CaptureMarker}: saved area={area} era=past path={Path.Combine(outputDirectory, pastFileName)} local={localPosition}");
            camera.cullingMask = previousMask;
            controller.ForcePlayerCurrentLocalForReview(localPosition);
            guide.ApplyActiveTimeIsolationForReview();
        }

        private static void CaptureChapter1EndSideViewPreview(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            ref int count)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.Chapter1End);
            controller.ForcePlayerCurrentLocalForReview(Chapter1EndSideViewPreviewTarget);
            guide.ApplyActiveTimeIsolationForReview();
            PositionChapter1EndSideViewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(Chapter1EndSideViewCameraAnchor));
            WriteCameraPng(camera, Path.Combine(outputDirectory, fileName));
            count++;
            Debug.Log($"{CaptureMarker}: saved area={FastVsHouseArea.Chapter1End} era=current path={Path.Combine(outputDirectory, fileName)} local={Chapter1EndSideViewPreviewTarget}");
            controller.ForcePlayerCurrentLocalForReview(Chapter1EndSideViewCenter + new Vector3(-6.50f, 0.02f, 0f));
            guide.ApplyActiveTimeIsolationForReview();
        }

        private static void PositionChapter1AllMapsCamera(Camera camera, Vector3 anchor, Vector3 positionOffset, Vector3 lookAtOffset)
        {
            camera.orthographic = false;
            var position = anchor + positionOffset;
            var lookAt = anchor + lookAtOffset;
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
        }

        private static void PositionChapter1EndSideViewCamera(Camera camera, Vector3 anchor)
        {
            camera.orthographic = true;
            camera.orthographicSize = Chapter1EndSideViewOrthographicSize;
            camera.transform.SetPositionAndRotation(anchor + new Vector3(0f, 0f, -13.0f), Quaternion.LookRotation(Vector3.forward, Vector3.up));
        }

        private static void WriteCameraPng(Camera camera, string path)
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
                ForceOpaqueAlpha(texture);
                File.WriteAllBytes(path, texture.EncodeToPNG());
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                renderTexture.Release();
                Destroy(renderTexture);
                Destroy(texture);
            }
        }

        private static void RenderCameraForVisibilitySample(Camera camera)
        {
            var previousTarget = camera.targetTexture;
            var previousActive = RenderTexture.active;
            var renderTexture = new RenderTexture(1280, 720, 24, RenderTextureFormat.ARGB32);
            try
            {
                camera.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;
                Canvas.ForceUpdateCanvases();
                camera.Render();
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                renderTexture.Release();
                Destroy(renderTexture);
            }
        }

        private static FramePixelSample WriteCameraPngAndSample(Camera camera, string path, int sampleStep)
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
                ForceOpaqueAlpha(texture);
                var sample = CapturePixelSample(texture, sampleStep);
                File.WriteAllBytes(path, texture.EncodeToPNG());
                return sample;
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                renderTexture.Release();
                Destroy(renderTexture);
                Destroy(texture);
            }
        }

        private static FramePixelSample SampleCameraPixels(Camera camera, int sampleStep)
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
                ForceOpaqueAlpha(texture);
                return CapturePixelSample(texture, sampleStep);
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                renderTexture.Release();
                Destroy(renderTexture);
                Destroy(texture);
            }
        }

        private static Color32[] SampleCameraPixelPoints(Camera camera, Vector2Int[] topLeftPixels)
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
                ForceOpaqueAlpha(texture);

                var result = new Color32[topLeftPixels.Length];
                for (var index = 0; index < topLeftPixels.Length; index++)
                {
                    var pixel = topLeftPixels[index];
                    var x = Mathf.Clamp(pixel.x, 0, texture.width - 1);
                    var y = Mathf.Clamp(texture.height - 1 - pixel.y, 0, texture.height - 1);
                    result[index] = texture.GetPixel(x, y);
                }

                return result;
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                renderTexture.Release();
                Destroy(renderTexture);
                Destroy(texture);
            }
        }

        private static FramePixelSample CapturePixelSample(Texture2D texture, int sampleStep)
        {
            var step = Mathf.Max(sampleStep, 1);
            var allPixels = texture.GetPixels32();
            var samples = new List<Color32>();
            for (var y = 0; y < texture.height; y += step)
            {
                var row = y * texture.width;
                for (var x = 0; x < texture.width; x += step)
                {
                    samples.Add(allPixels[row + x]);
                }
            }

            return new FramePixelSample(samples.ToArray());
        }

        private static string BuildProbePointSummary(Vector2Int[] points, string[] labels, Color32[] colors)
        {
            var builder = new StringBuilder();
            for (var index = 0; index < points.Length && index < colors.Length; index++)
            {
                if (index > 0)
                {
                    builder.Append(" | ");
                }

                var label = index < labels.Length ? labels[index] : $"P{index}";
                builder.Append(label)
                    .Append("=(")
                    .Append(points[index].x)
                    .Append(",")
                    .Append(points[index].y)
                    .Append("):")
                    .Append(FormatColor32(colors[index]));
            }

            return builder.ToString();
        }

        private static int ColorDeltaRgb(Color32 previous, Color32 current)
        {
            return Mathf.Abs(previous.r - current.r) +
                Mathf.Abs(previous.g - current.g) +
                Mathf.Abs(previous.b - current.b);
        }

        private static FrameImageDelta CompareFrameSamples(FramePixelSample previous, FramePixelSample current)
        {
            if (previous == null || current == null || previous.Pixels == null || current.Pixels == null)
            {
                return FrameImageDelta.Empty;
            }

            var count = Mathf.Min(previous.Pixels.Length, current.Pixels.Length);
            if (count == 0)
            {
                return FrameImageDelta.Empty;
            }

            double sum = 0;
            var changed = 0;
            for (var index = 0; index < count; index++)
            {
                var a = previous.Pixels[index];
                var b = current.Pixels[index];
                var delta = Math.Abs(a.r - b.r) + Math.Abs(a.g - b.g) + Math.Abs(a.b - b.b);
                sum += delta / 3.0;
                if (delta > 9)
                {
                    changed++;
                }
            }

            return new FrameImageDelta((float)(sum / count), (float)(changed * 100.0 / count), changed, count);
        }

        private static void ForceOpaqueAlpha(Texture2D texture)
        {
            var pixels = texture.GetPixels32();
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i].a = 255;
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, false);
        }

        private static string SanitizeFileName(string value)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var builder = new StringBuilder(value.Length);
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                var safe = true;
                for (var j = 0; j < invalid.Length; j++)
                {
                    if (c == invalid[j])
                    {
                        safe = false;
                        break;
                    }
                }

                builder.Append(safe ? c : '_');
            }

            return builder.ToString();
        }

        private static void RunChecks(RendererContractSnapshot rendererContract)
        {
            LogSmokeStep("VerifyRendererContract.begin");
            VerifyRendererContract(rendererContract);
            LogSmokeStep("VerifyRendererContract.end");

            LogSmokeStep("RequireController.begin");
            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            LogSmokeStep("RequireController.end");
            LogSmokeStep("RequireVisibility.begin");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            LogSmokeStep("RequireVisibility.end");

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_MiaHouse_To_MiaInterior"),
                FastVsHouseArea.MiaHouse,
                FastVsHouseArea.MiaInterior,
                "Mia house to Mia interior");
            LogSmokeStep("RequireRenderer.Mia.begin");
            RequireActiveRenderer("FastVS_SpriteCharacter_Mia");
            LogSmokeStep("RequireRenderer.Mia.end");

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_MiaInterior_To_MiaHouse"),
                FastVsHouseArea.MiaInterior,
                FastVsHouseArea.MiaHouse,
                "Mia interior to Mia house");

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_AriaStreet_To_AriaInterior"),
                FastVsHouseArea.AriaStreet,
                FastVsHouseArea.AriaInterior,
                "Aria street to Aria interior");
            LogSmokeStep("RequireRenderer.Karla.begin");
            RequireActiveRenderer("FastVS_SpriteCharacter_Karla");
            LogSmokeStep("RequireRenderer.Karla.end");
            LogSmokeStep("RequireRenderer.Aria.begin");
            RequireActiveRenderer("FastVS_SpriteCharacter_Aria");
            LogSmokeStep("RequireRenderer.Aria.end");

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_AriaInterior_To_AriaStreet"),
                FastVsHouseArea.AriaInterior,
                FastVsHouseArea.AriaStreet,
                "Aria interior to Aria street");
        }

        private static void VerifyTravel(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsAreaDoorTransition transition,
            FastVsHouseArea sourceArea,
            FastVsHouseArea targetArea,
            string label)
        {
            LogSmokeStep($"{label}.begin");
            LogSmokeStep($"{label}.set-area.begin");
            visibility.SetActiveAreaForReview(sourceArea);
            LogSmokeStep($"{label}.set-area.end active={visibility.ActiveAreaForReview}");
            LogSmokeStep($"{label}.force-local.begin target={transition.TriggerLocalCenterForReview}");
            controller.ForcePlayerCurrentLocalForReview(transition.TriggerLocalCenterForReview);
            LogSmokeStep($"{label}.force-local.end");
            LogSmokeStep($"{label}.try-evaluate.begin");
            var triggered = transition.TryEvaluateCurrentPlayerForReview();
            LogSmokeStep($"{label}.try-evaluate.end triggered={triggered}");
            if (!triggered)
            {
                throw new InvalidOperationException($"{label} did not trigger.");
            }

            if (visibility.ActiveAreaForReview != targetArea)
            {
                throw new InvalidOperationException($"{label} activated {visibility.ActiveAreaForReview}, expected {targetArea}.");
            }

            var actualLocal = controller.GetPlayerLocalCoordinateForReview();
            var expectedLocal = transition.TargetLocalPositionForReview;
            if ((actualLocal - expectedLocal).sqrMagnitude > 0.01f)
            {
                throw new InvalidOperationException($"{label} placed player at {actualLocal}, expected {expectedLocal}.");
            }

            LogSmokeStep($"{label}.end active={visibility.ActiveAreaForReview} local={actualLocal}");
        }

        private static void LogSmokeStep(string step)
        {
            Debug.Log($"ANEMORA_HOUSE_SLICE_SMOKE_STEP: {step}");
        }

        private static T RequireObject<T>(string label)
            where T : UnityEngine.Object
        {
            var found = FindFirstObjectByType<T>();
            if (found == null)
            {
                throw new InvalidOperationException($"Missing {label}.");
            }

            return found;
        }

        private static FastVsAreaDoorTransition RequireTransition(string objectName)
        {
            var gameObject = GameObject.Find(objectName);
            var transition = gameObject != null ? gameObject.GetComponent<FastVsAreaDoorTransition>() : null;
            if (transition == null)
            {
                throw new InvalidOperationException($"Missing door transition {objectName}.");
            }

            return transition;
        }

        private static void RequireActiveRenderer(string objectName)
        {
            var gameObject = GameObject.Find(objectName);
            if (gameObject == null)
            {
                throw new InvalidOperationException($"Missing indoor character {objectName}.");
            }

            var renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].enabled && renderers[i].gameObject.activeInHierarchy)
                {
                    return;
                }
            }

            throw new InvalidOperationException($"Indoor character {objectName} has no active renderer.");
        }

        private static RendererContractSnapshot InspectRendererContract()
        {
            var snapshot = new RendererContractSnapshot();
            var pipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if (pipelineAsset == null)
            {
                snapshot.Error = "current render pipeline is not a UniversalRenderPipelineAsset";
                return snapshot;
            }

            snapshot.PipelineName = pipelineAsset.name;
            var rendererData = ResolveDefaultRendererData(pipelineAsset);
            if (rendererData == null)
            {
                snapshot.Error = "could not resolve default UniversalRendererData";
                return snapshot;
            }

            snapshot.RendererName = rendererData.name;
            snapshot.RenderingMode = ReadIntField(rendererData, "m_RenderingMode");
            snapshot.DepthPrimingMode = ReadIntField(rendererData, "m_DepthPrimingMode");
            snapshot.CopyDepthMode = ReadIntField(rendererData, "m_CopyDepthMode");
            snapshot.Features = BuildRendererFeatureSummary(rendererData);
            snapshot.PortalStencilFeatureActive = HasActiveFeature(rendererData, "PortalStencilFeature");
            return snapshot;
        }

        private static UniversalRendererData ResolveDefaultRendererData(UniversalRenderPipelineAsset pipelineAsset)
        {
            var rendererListField = FindField(typeof(UniversalRenderPipelineAsset), "m_RendererDataList");
            var defaultIndexField = FindField(typeof(UniversalRenderPipelineAsset), "m_DefaultRendererIndex");
            var rendererList = rendererListField != null ? rendererListField.GetValue(pipelineAsset) as ScriptableRendererData[] : null;
            if (rendererList == null || rendererList.Length == 0)
            {
                return null;
            }

            var defaultIndex = 0;
            if (defaultIndexField != null)
            {
                defaultIndex = Mathf.Clamp(Convert.ToInt32(defaultIndexField.GetValue(pipelineAsset)), 0, rendererList.Length - 1);
            }

            return rendererList[defaultIndex] as UniversalRendererData;
        }

        private static int ReadIntField(object target, string fieldName)
        {
            var field = FindField(target.GetType(), fieldName);
            if (field == null)
            {
                return int.MinValue;
            }

            var value = field.GetValue(target);
            return value != null ? Convert.ToInt32(value) : int.MinValue;
        }

        private static FieldInfo FindField(Type type, string fieldName)
        {
            while (type != null)
            {
                var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    return field;
                }

                type = type.BaseType;
            }

            return null;
        }

        private static string BuildRendererFeatureSummary(UniversalRendererData rendererData)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append("; ");
                }

                var feature = rendererData.rendererFeatures[i];
                if (feature == null)
                {
                    builder.Append(i).Append(":<null>:inactive");
                    continue;
                }

                builder
                    .Append(i)
                    .Append(':')
                    .Append(feature.name)
                    .Append('(')
                    .Append(feature.GetType().Name)
                    .Append("):")
                    .Append(feature.isActive ? "active" : "inactive");
            }

            return builder.ToString();
        }

        private static bool HasActiveFeature(UniversalRendererData rendererData, string featureTypeName)
        {
            for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
            {
                var feature = rendererData.rendererFeatures[i];
                if (feature != null &&
                    feature.isActive &&
                    string.Equals(feature.GetType().Name, featureTypeName, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static void LogRendererContract(RendererContractSnapshot snapshot)
        {
            Debug.Log(
                $"{RendererContractMarker}: pipeline={snapshot.PipelineName ?? "<missing>"} renderer={snapshot.RendererName ?? "<missing>"} " +
                $"RenderingMode={snapshot.RenderingMode} DepthPrimingMode={snapshot.DepthPrimingMode} CopyDepthMode={snapshot.CopyDepthMode} " +
                $"PortalStencilFeatureActive={snapshot.PortalStencilFeatureActive} features=[{snapshot.Features ?? string.Empty}] error={snapshot.Error ?? "<none>"}");
        }

        private static void VerifyRendererContract(RendererContractSnapshot snapshot)
        {
            if (!string.IsNullOrEmpty(snapshot.Error))
            {
                throw new InvalidOperationException($"Renderer contract inspection failed: {snapshot.Error}.");
            }

            if (snapshot.RenderingMode != 2)
            {
                throw new InvalidOperationException($"Renderer contract failed: RenderingMode={snapshot.RenderingMode}, expected 2.");
            }

            if (snapshot.DepthPrimingMode != 0)
            {
                throw new InvalidOperationException($"Renderer contract failed: DepthPrimingMode={snapshot.DepthPrimingMode}, expected 0.");
            }

            if (snapshot.CopyDepthMode != 0)
            {
                throw new InvalidOperationException($"Renderer contract failed: CopyDepthMode={snapshot.CopyDepthMode}, expected 0.");
            }

            if (!snapshot.PortalStencilFeatureActive)
            {
                throw new InvalidOperationException("Renderer contract failed: PortalStencilFeature is missing or inactive.");
            }
        }

        private sealed class RendererDiagnosticsInfo
        {
            public RendererDiagnosticsInfo(Renderer renderer, string path, Bounds bounds)
            {
                Renderer = renderer;
                Path = path;
                Bounds = bounds;
            }

            public readonly Renderer Renderer;
            public readonly string Path;
            public readonly Bounds Bounds;
        }

        private sealed class RendererOverlapInfo
        {
            public RendererOverlapInfo(RendererDiagnosticsInfo a, RendererDiagnosticsInfo b, float overlapArea, float centerYDelta, float minYGap)
            {
                A = a;
                B = b;
                OverlapArea = overlapArea;
                CenterYDelta = centerYDelta;
                MinYGap = minYGap;
            }

            public readonly RendererDiagnosticsInfo A;
            public readonly RendererDiagnosticsInfo B;
            public readonly float OverlapArea;
            public readonly float CenterYDelta;
            public readonly float MinYGap;
        }

        private sealed class RendererRoiCandidate
        {
            public RendererRoiCandidate(
                Renderer renderer,
                string path,
                Rect viewportRect,
                float minDepth,
                float overlapArea,
                float centerDistance,
                float objectCoverage,
                float roiCoverage,
                float score)
            {
                Renderer = renderer;
                Path = path;
                ViewportRect = viewportRect;
                MinDepth = minDepth;
                OverlapArea = overlapArea;
                CenterDistance = centerDistance;
                ObjectCoverage = objectCoverage;
                RoiCoverage = roiCoverage;
                Score = score;
            }

            public readonly Renderer Renderer;
            public readonly string Path;
            public readonly Rect ViewportRect;
            public readonly float MinDepth;
            public readonly float OverlapArea;
            public readonly float CenterDistance;
            public readonly float ObjectCoverage;
            public readonly float RoiCoverage;
            public readonly float Score;
        }

        private sealed class RendererPixelContributionResult
        {
            public RendererPixelContributionResult(string path, string material, int totalDelta, int maxDelta, string deltas)
            {
                Path = path;
                Material = material;
                TotalDelta = totalDelta;
                MaxDelta = maxDelta;
                Deltas = deltas;
            }

            public readonly string Path;
            public readonly string Material;
            public readonly int TotalDelta;
            public readonly int MaxDelta;
            public readonly string Deltas;
        }

        private sealed class RendererEnabledState
        {
            public RendererEnabledState(Renderer renderer, bool wasEnabled)
            {
                Renderer = renderer;
                WasEnabled = wasEnabled;
            }

            public readonly Renderer Renderer;
            public readonly bool WasEnabled;
        }

        private sealed class RendererVisibilityStats
        {
            public RendererVisibilityStats(Renderer renderer)
            {
                Renderer = renderer;
                Path = BuildHierarchyPath(renderer.transform);
                Bounds = renderer.bounds;
            }

            public readonly Renderer Renderer;
            public readonly string Path;
            public readonly Bounds Bounds;
            public int VisibleToggleCount;
            public int EnabledToggleCount;
            public int VisibleFrames;
            public int EnabledFrames;
            public bool LastVisible;
            public bool LastEnabled;
            private bool hasSample;

            public void Sample()
            {
                var enabled = Renderer != null && Renderer.enabled && Renderer.gameObject.activeInHierarchy;
                var visible = enabled && Renderer.isVisible;
                if (hasSample)
                {
                    if (visible != LastVisible)
                    {
                        VisibleToggleCount++;
                    }

                    if (enabled != LastEnabled)
                    {
                        EnabledToggleCount++;
                    }
                }

                LastVisible = visible;
                LastEnabled = enabled;
                hasSample = true;
                if (visible)
                {
                    VisibleFrames++;
                }

                if (enabled)
                {
                    EnabledFrames++;
                }
            }
        }

        private sealed class FramePixelSample
        {
            public FramePixelSample(Color32[] pixels)
            {
                Pixels = pixels;
            }

            public readonly Color32[] Pixels;
        }

        private sealed class LibraryRearCloseView
        {
            public LibraryRearCloseView(string label, Vector3 cameraLocal, Vector3 targetLocal, float fieldOfView)
            {
                Label = label;
                CameraLocal = cameraLocal;
                TargetLocal = targetLocal;
                FieldOfView = fieldOfView;
            }

            public readonly string Label;
            public readonly Vector3 CameraLocal;
            public readonly Vector3 TargetLocal;
            public readonly float FieldOfView;
        }

        private sealed class LibraryRearCloseVariant
        {
            public LibraryRearCloseVariant(string label, Predicate<string> shouldDisable)
            {
                Label = label;
                ShouldDisable = shouldDisable;
            }

            public readonly string Label;
            public readonly Predicate<string> ShouldDisable;
        }

        private struct FrameImageDelta
        {
            public FrameImageDelta(float meanAbsRgb, float changedSamplePct, int changedSamples, int totalSamples)
            {
                MeanAbsRgb = meanAbsRgb;
                ChangedSamplePct = changedSamplePct;
                ChangedSamples = changedSamples;
                TotalSamples = totalSamples;
            }

            public static readonly FrameImageDelta Empty = new FrameImageDelta(0f, 0f, 0, 0);
            public readonly float MeanAbsRgb;
            public readonly float ChangedSamplePct;
            public readonly int ChangedSamples;
            public readonly int TotalSamples;
        }

        private sealed class RendererFlickerStats
        {
            public RendererFlickerStats(Renderer renderer)
            {
                Renderer = renderer;
                Path = BuildHierarchyPath(renderer.transform);
                Bounds = renderer.bounds;
            }

            public readonly Renderer Renderer;
            public readonly string Path;
            public readonly Bounds Bounds;
            public int VisibleToggleCount;
            public int EnabledToggleCount;
            public int VisibleFrames;
            public int EnabledFrames;
            public bool LastVisible;
            public bool LastEnabled;
            private readonly StringBuilder visiblePattern = new StringBuilder();
            private bool hasSample;

            public string VisiblePattern => visiblePattern.ToString();

            public void Sample(int frame)
            {
                var enabled = Renderer != null && Renderer.enabled && Renderer.gameObject.activeInHierarchy;
                var visible = enabled && Renderer.isVisible;
                if (hasSample)
                {
                    if (visible != LastVisible)
                    {
                        VisibleToggleCount++;
                    }

                    if (enabled != LastEnabled)
                    {
                        EnabledToggleCount++;
                    }
                }

                LastVisible = visible;
                LastEnabled = enabled;
                hasSample = true;
                visiblePattern.Append(visible ? '1' : '0');
                if (visible)
                {
                    VisibleFrames++;
                }

                if (enabled)
                {
                    EnabledFrames++;
                }
            }
        }

        private sealed class RendererContractSnapshot
        {
            public string PipelineName;
            public string RendererName;
            public int RenderingMode = int.MinValue;
            public int DepthPrimingMode = int.MinValue;
            public int CopyDepthMode = int.MinValue;
            public bool PortalStencilFeatureActive;
            public string Features;
            public string Error;
        }
    }
}
