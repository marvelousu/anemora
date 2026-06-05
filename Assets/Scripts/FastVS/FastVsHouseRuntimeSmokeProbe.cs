using System;
using System.Collections;
using Anemora.TimeManagement;
using Anemora.TimeManagement.Portal;
using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsHouseRuntimeSmokeProbe : MonoBehaviour
    {
        private const string EnableArgument = "--anemora-house-slice-smoke";
        private const string PerformanceArgument = "--anemora-house-slice-perf";
        private const string R236RecheckArgument = "--anemora-house-slice-r236-recheck";
        private const string PassMarker = "ANEMORA_HOUSE_SLICE_SMOKE_PASS";
        private const string FailMarker = "ANEMORA_HOUSE_SLICE_SMOKE_FAIL";
        private const string PerformanceMarker = "ANEMORA_HOUSE_SLICE_PERF";
        private const string R236RecheckPassMarker = "ANEMORA_HOUSE_SLICE_R236_RECHECK_PASS";
        private const string R236RecheckFailMarker = "ANEMORA_HOUSE_SLICE_R236_RECHECK_FAIL";
        private static readonly Vector2 PortalDragStart = new Vector2(380f, 215f);
        private static readonly Vector2 PortalDragEnd = new Vector2(850f, 600f);
        private static readonly Vector3 HouseInteriorCenter = new Vector3(-8.35f, 0f, -8.35f);
        private static readonly Vector3 HouseExteriorCenter = new Vector3(8.20f, 0f, 8.20f);
        private static readonly Vector3 CentralPlazaVsCenter = new Vector3(20.80f, 0f, 15.80f);
        private static readonly Vector3 LibraryVsCenter = new Vector3(31.00f, 0f, 20.00f);
        private static readonly Vector3 MiaInteriorCenter = new Vector3(-21.20f, 0f, -8.35f);
        private static readonly Vector3 AriaInteriorCenter = new Vector3(-32.95f, 0f, -8.35f);
        private static readonly Vector3 KaiaInteriorCenter = new Vector3(-44.70f, 0f, -8.35f);
        private static readonly Vector3 RuinsF4InteriorCenter = new Vector3(-56.45f, 0f, -8.35f);
        private static readonly Vector3 CentralPlazaPerformanceStartLocal = new Vector3(20.46f, 0.02f, 19.06f);
        private static readonly R2AreaProbe[] R2AreaProbes =
        {
            new R2AreaProbe(FastVsHouseArea.Interior, "Current_HouseInteriorMap_SeparateSpace", "Past_HouseInteriorMap_SeparateSpace", HouseInteriorCenter + new Vector3(-2.42f, 0.02f, 0.90f), new Vector3(0f, 7.20f, -8.35f), new Vector3(0f, 0.18f, 0.72f), 6),
            new R2AreaProbe(FastVsHouseArea.Exterior, "Current_HouseExteriorMap_SeparateSpace", "Past_HouseExteriorMap_SeparateSpace", HouseExteriorCenter + new Vector3(2.95f, 0.02f, 1.10f), new Vector3(0f, 12.80f, -17.60f), new Vector3(0f, 0.22f, 1.50f), 8),
            new R2AreaProbe(FastVsHouseArea.CentralPlaza, "Current_CentralPlazaMap_SeparateSpace", "Past_CentralPlazaMap_SeparateSpace", CentralPlazaVsCenter + new Vector3(1.45f, 0.02f, -0.20f), new Vector3(0f, 17.90f, -25.20f), new Vector3(0.10f, 0.20f, 2.90f), 10),
            new R2AreaProbe(FastVsHouseArea.Library, "Current_LibraryMap_SeparateSpace", "Past_LibraryMap_SeparateSpace", LibraryVsCenter + new Vector3(-2.20f, 0.02f, -1.85f), new Vector3(0f, 10.40f, -14.60f), new Vector3(0f, 0.18f, 1.10f), 8),
            new R2AreaProbe(FastVsHouseArea.MiaHouse, "Current_MiaHouseMap_SeparateSpace", "Past_MiaHouseMap_SeparateSpace", CentralPlazaVsCenter + new Vector3(3.70f, 0.02f, -1.55f), new Vector3(0f, 17.90f, -25.20f), new Vector3(0.10f, 0.20f, 2.90f), 8),
            new R2AreaProbe(FastVsHouseArea.AriaStreet, "Current_AriaStreetMap_SeparateSpace", "Past_AriaStreetMap_SeparateSpace", CentralPlazaVsCenter + new Vector3(25.50f, 0.02f, -1.75f), new Vector3(0f, 20.35f, -27.80f), new Vector3(0.80f, 0.22f, 4.10f), 8),
            new R2AreaProbe(FastVsHouseArea.KaiaFarm, "Current_KaiaFarmMap_SeparateSpace", "Past_KaiaFarmMap_SeparateSpace", CentralPlazaVsCenter + new Vector3(32.50f, 0.02f, -2.85f), new Vector3(0f, 20.95f, -28.90f), new Vector3(0.85f, 0.24f, 4.60f), 8),
            new R2AreaProbe(FastVsHouseArea.Ruins, "Current_RuinsMap_SeparateSpace", "Past_RuinsMap_SeparateSpace", CentralPlazaVsCenter + new Vector3(45.50f, 0.02f, -0.40f), new Vector3(-0.08f, 25.35f, -40.30f), new Vector3(0.44f, 0.28f, 5.84f), 8),
            new R2AreaProbe(FastVsHouseArea.MiaInterior, "Current_MiaInteriorMap_SeparateSpace", "Past_MiaInteriorMap_SeparateSpace", MiaInteriorCenter + new Vector3(0f, 0.02f, -0.35f), new Vector3(0f, 7.20f, -8.35f), new Vector3(0f, 0.18f, 0.72f), 6),
            new R2AreaProbe(FastVsHouseArea.AriaInterior, "Current_AriaInteriorMap_SeparateSpace", "Past_AriaInteriorMap_SeparateSpace", AriaInteriorCenter + new Vector3(0f, 0.02f, -0.35f), new Vector3(0f, 7.20f, -8.35f), new Vector3(0f, 0.18f, 0.72f), 6),
            new R2AreaProbe(FastVsHouseArea.KaiaInterior, "Current_KaiaInteriorMap_SeparateSpace", "Past_KaiaInteriorMap_SeparateSpace", KaiaInteriorCenter + new Vector3(0f, 0.02f, -0.35f), new Vector3(0f, 7.20f, -8.35f), new Vector3(0f, 0.18f, 0.72f), 6),
            new R2AreaProbe(FastVsHouseArea.RuinsF4Interior, "Current_RuinsF4InteriorMap_SeparateSpace", "Past_RuinsF4InteriorMap_SeparateSpace", RuinsF4InteriorCenter + new Vector3(0f, 0.02f, -0.35f), new Vector3(0f, 7.20f, -8.35f), new Vector3(0f, 0.18f, 0.72f), 6)
        };

        private IEnumerator Start()
        {
            if (ShouldRun(PerformanceArgument))
            {
                yield return RunPerformanceProbe();
                yield break;
            }

            if (ShouldRun(R236RecheckArgument))
            {
                yield return RunR236RecheckProbe();
                yield break;
            }

            if (!ShouldRun(EnableArgument))
            {
                yield break;
            }

            yield return null;
            yield return new WaitForSeconds(3.0f);

            try
            {
                RunChecks();
                Debug.Log($"{PassMarker}: stable startup framing, MiaInterior and AriaInterior door travel, current-time indoor activation, and past-only NPC isolation verified.");
                Application.Quit(0);
            }
            catch (Exception exception)
            {
                Debug.LogError($"{FailMarker}: {exception}");
                Application.Quit(31);
            }
        }

        private static bool ShouldRun(string argument)
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], argument, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerator RunPerformanceProbe()
        {
            Application.runInBackground = true;
            PreparePerformanceProbeArea();
            yield return null;
            yield return new WaitForSecondsRealtime(2.0f);

            var player = FindFirstObjectByType<CharacterController>();
            var visibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            PreparePerformanceProbeArea();
            var elapsed = 0f;
            var frameCount = 0;
            var sumDelta = 0f;
            var minDelta = float.PositiveInfinity;
            var maxDelta = 0f;

            while (elapsed < 20f)
            {
                var delta = Mathf.Max(Time.unscaledDeltaTime, 0.0001f);
                MovePerformanceProbePlayer(player, elapsed, delta);
                elapsed += delta;
                frameCount++;
                sumDelta += delta;
                minDelta = Mathf.Min(minDelta, delta);
                maxDelta = Mathf.Max(maxDelta, delta);
                yield return null;
            }

            CountActiveRenderers(out var activeRenderers, out var visibleRenderers);
            var safeFrames = Mathf.Max(1, frameCount);
            var safeSum = Mathf.Max(0.0001f, sumDelta);
            var area = visibility != null ? visibility.ActiveAreaForReview.ToString() : "unknown";
            Debug.Log(
                $"{PerformanceMarker}: area={area} seconds={elapsed:0.000} frames={frameCount} " +
                $"avgMs={(safeSum / safeFrames) * 1000f:0.00} minMs={minDelta * 1000f:0.00} " +
                $"maxMs={maxDelta * 1000f:0.00} avgFps={safeFrames / safeSum:0.0} " +
                $"activeRenderers={activeRenderers} visibleRenderers={visibleRenderers}");
            Application.Quit(0);
        }

        private static IEnumerator RunR236RecheckProbe()
        {
            Application.runInBackground = true;
            yield return null;
            yield return new WaitForSecondsRealtime(2.0f);

            Exception failure = null;
            yield return RunR2AreaVisibilityRecheck(exception => failure = exception);
            if (failure != null)
            {
                FinishR236RecheckFailure(failure);
                yield break;
            }

            try
            {
                RunR3LibraryFacadeMaterialRecheck();
            }
            catch (Exception exception)
            {
                FinishR236RecheckFailure(exception);
                yield break;
            }

            yield return RunR6PortalTraversalRecheck(exception => failure = exception);
            if (failure != null)
            {
                FinishR236RecheckFailure(failure);
                yield break;
            }

            CountActiveRenderers(out var activeRenderers, out var visibleRenderers);
            Debug.Log(
                $"{R236RecheckPassMarker}: R-2 area root culling, R-3 library facade opaque materials, " +
                $"and R-6 portal traversal/back-side guard verified. activeRenderers={activeRenderers} visibleRenderers={visibleRenderers}");
            Application.Quit(0);
        }

        private static IEnumerator RunR2AreaVisibilityRecheck(Action<Exception> fail)
        {
            var controller = default(TimeWindowPairedSpacePortalController);
            var visibility = default(FastVsHouseAreaVisibility);
            var camera = default(Camera);

            try
            {
                controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
                visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
                camera = RequireObject<Camera>("main camera");
                camera.cullingMask = ~0;
                controller.ApplyReviewVisibilityLayersForReview();
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            for (var index = 0; index < R2AreaProbes.Length; index++)
            {
                var probe = R2AreaProbes[index];
                try
                {
                    visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(false);
                    visibility.SetActiveAreaForReview(probe.Area);
                    controller.ForcePlayerCurrentLocalForReview(probe.PlayerLocal);
                    visibility.ApplyRuntimeTimeSetActiveIsolationForReview(false, false);
                    FrameCameraAtLocal(controller.CurrentSpaceRootForReview, probe.PlayerLocal, probe.CameraOffset, probe.LookAtOffset);
                }
                catch (Exception exception)
                {
                    fail(exception);
                    yield break;
                }

                yield return null;
                yield return new WaitForEndOfFrame();

                try
                {
                    var currentSummary = VerifyVisibleRoot(probe.CurrentRootName, $"{probe.Area} current", probe.MinEnabledRenderers);
                    var inactivePastSummary = VerifyInactiveRoot(probe.PastRootName, $"{probe.Area} past inactive-time");
                    Debug.Log($"[R236] R-2 area={probe.Area} currentRoot {currentSummary}");
                    Debug.Log($"[R236] R-2 area={probe.Area} pastRoot {inactivePastSummary}");
                    visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(true);
                    visibility.ApplyRuntimeTimeSetActiveIsolationForReview(false, true);
                    FrameCameraAtLocal(controller.OtherTimeSpaceRootForReview, probe.PlayerLocal, probe.CameraOffset, probe.LookAtOffset);
                }
                catch (Exception exception)
                {
                    fail(exception);
                    yield break;
                }

                yield return null;
                yield return new WaitForEndOfFrame();

                try
                {
                    var pastSummary = VerifyVisibleRoot(probe.PastRootName, $"{probe.Area} past", probe.MinEnabledRenderers);
                    Debug.Log($"[R236] R-2 area={probe.Area} pastRoot {pastSummary}");
                    visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(false);
                }
                catch (Exception exception)
                {
                    fail(exception);
                    yield break;
                }
            }
        }

        private static void RunR3LibraryFacadeMaterialRecheck()
        {
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(true);
            visibility.ApplyRuntimeTimeSetActiveIsolationForReview(false, true);

            VerifyOpaqueFacadeRenderer("Current_CentralPlaza_LibraryNorthFacade", "current library north facade");
            VerifyOpaqueFacadeRenderer("Past_CentralPlaza_LibraryNorthFacade", "past library north facade");
            VerifyOpaqueFacadeRenderer("Current_CentralPlaza_LibraryFacadeArchitecture_LeftWallPatch", "current library left wall patch");
            VerifyOpaqueFacadeRenderer("Past_CentralPlaza_LibraryFacadeSurfaceBreakup_EntranceWarmBandA", "past library entrance warm band");
            Debug.Log("[R236] R-3 library facade material audit passed: facade renderers are active, opaque queue, _Surface=0, alpha=1.");
        }

        private static IEnumerator RunR6PortalTraversalRecheck(Action<Exception> fail)
        {
            var controller = default(TimeWindowPairedSpacePortalController);
            var visibility = default(FastVsHouseAreaVisibility);
            try
            {
                controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
                visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
                visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(false);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.SetRuntimeInputEnabledForReview(false);
                controller.ClosePortal();
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
                visibility.ApplyRuntimeTimeSetActiveIsolationForReview(false, false);
                FrameCameraAtLocal(controller.CurrentSpaceRootForReview, CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f), new Vector3(0f, 5.8f, -8.5f), new Vector3(0f, 0.28f, 1.10f));
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            yield return null;
            yield return new WaitForEndOfFrame();

            try
            {
                if (!controller.TryOpenPortalForTests(PortalDragStart, PortalDragEnd))
                {
                    throw new InvalidOperationException("R-6 portal drag-open was rejected in runtime recheck.");
                }

                visibility.ApplyRuntimeTimeSetActiveIsolationForReview(controller.PlayerInOtherTime, true);
                if (!controller.HasPortalPair || !controller.HasLiveApertureViewForReview)
                {
                    throw new InvalidOperationException("R-6 runtime recheck did not create a live aperture portal pair.");
                }

                if (controller.PortalBottomLocalYForReview < 0.055f)
                {
                    throw new InvalidOperationException($"R-6 runtime portal bottom is embedded below the visible floor: bottom={controller.PortalBottomLocalYForReview:0.000}.");
                }

                controller.RenderPortalAperturesForReview();
                if (PortalStencilFeature.LastEnqueuedPassCount != 2)
                {
                    throw new InvalidOperationException($"R-6 PortalStencilFeature did not enqueue both passes: count={PortalStencilFeature.LastEnqueuedPassCount} camera={PortalStencilFeature.LastCameraName}.");
                }

                VerifyCurrentBackSideBlock(controller);
                var portalLocal = controller.PortalLocalCenterForReview;
                var enabledWallCollidersInOtherTime = 0;
                controller.TransferCurrentToOtherForReview(new Vector3(portalLocal.x, 0.72f, portalLocal.z + 0.18f));
                visibility.ApplyRuntimeTimeSetActiveIsolationForReview(controller.PlayerInOtherTime, true);
                controller.RenderPortalAperturesForReview();
                if (!controller.PlayerInOtherTime)
                {
                    throw new InvalidOperationException("R-6 runtime transfer to other-time space did not occur.");
                }

                enabledWallCollidersInOtherTime = controller.EnabledOtherTimeWallVolumeColliderCountForReview;
                if (controller.HasGeneratedOtherTimeWallVolumeForReview && enabledWallCollidersInOtherTime == 0)
                {
                    throw new InvalidOperationException("R-6 other-time wall volume colliders are disabled while player is in other-time space.");
                }

                controller.ClosePortal();
                if (!controller.PlayerInOtherTime ||
                    !controller.HasPortalPair ||
                    !controller.CloseRejectedBecausePlayerInOtherTimeForReview)
                {
                    throw new InvalidOperationException("R-6 runtime portal close was not rejected while player remained in other-time space.");
                }

                controller.TransferOtherToCurrentForReview(new Vector3(portalLocal.x, 0.72f, portalLocal.z - 0.18f));
                visibility.ApplyRuntimeTimeSetActiveIsolationForReview(controller.PlayerInOtherTime, true);
                controller.RenderPortalAperturesForReview();
                if (controller.PlayerInOtherTime)
                {
                    throw new InvalidOperationException("R-6 runtime return to current space did not occur.");
                }

                Debug.Log(
                    $"[R236] R-6 portal runtime audit passed: portalLocal={controller.PortalLocalCenterForReview}, " +
                    $"portalSize={controller.PortalSizeForReview}, stencilPasses={PortalStencilFeature.LastEnqueuedPassCount}, " +
                    $"camera={PortalStencilFeature.LastCameraName}, wallCollidersOtherTime={enabledWallCollidersInOtherTime}, " +
                    $"wallCollidersCurrent={controller.EnabledOtherTimeWallVolumeColliderCountForReview}.");
                controller.ClosePortal();
                visibility.ApplyRuntimeTimeSetActiveIsolationForReview(controller.PlayerInOtherTime, false);
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }
        }

        private static void FinishR236RecheckFailure(Exception exception)
        {
            Debug.LogError($"{R236RecheckFailMarker}: {exception}");
            Application.Quit(32);
        }

        private static void PreparePerformanceProbeArea()
        {
            var visibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            if (visibility != null)
            {
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            }

            var controller = FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            if (controller != null)
            {
                controller.SetRuntimeInputEnabledForReview(true);
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaPerformanceStartLocal);
            }
        }

        private static void MovePerformanceProbePlayer(CharacterController player, float elapsed, float delta)
        {
            if (player == null)
            {
                return;
            }

            var phase = Mathf.Repeat(elapsed, 8f);
            var direction = phase < 2f
                ? new Vector3(1f, 0f, 0.25f)
                : phase < 4f
                    ? new Vector3(-1f, 0f, -0.25f)
                    : phase < 6f
                        ? new Vector3(0.25f, 0f, 1f)
                        : new Vector3(-0.25f, 0f, -1f);
            player.Move(direction.normalized * (1.15f * delta));
        }

        private static void CountActiveRenderers(out int activeRenderers, out int visibleRenderers)
        {
            activeRenderers = 0;
            visibleRenderers = 0;
            var renderers = FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            for (var index = 0; index < renderers.Length; index++)
            {
                var renderer = renderers[index];
                if (renderer == null || !renderer.enabled || !renderer.gameObject.activeInHierarchy)
                {
                    continue;
                }

                activeRenderers++;
                if (renderer.isVisible)
                {
                    visibleRenderers++;
                }
            }
        }

        private static void RunChecks()
        {
            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");

            VerifyStartupFraming(controller, visibility);

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_MiaHouse_To_MiaInterior"),
                FastVsHouseArea.MiaHouse,
                FastVsHouseArea.MiaInterior,
                "Mia house to Mia interior");
            RequireActiveRenderer("FastVS_SpriteCharacter_Mia");

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
            RequireActiveRenderer("Current_AriaInteriorMap_SeparateSpace", "current Aria interior");
            var inactivePastSummary = VerifyInactiveRoot("Past_AriaInteriorMap_SeparateSpace", "AriaInterior past-only NPC root");
            Debug.Log($"[SMOKE] AriaInterior past root inactive during current-time travel: {inactivePastSummary}");
            visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(true);
            visibility.ApplyRuntimeTimeSetActiveIsolationForReview(false, true);
            RequireActiveRenderer("FastVS_SpriteCharacter_Karla");
            RequireActiveRenderer("FastVS_SpriteCharacter_Aria");
            visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(false);
            visibility.ApplyRuntimeTimeSetActiveIsolationForReview(false, false);

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_AriaInterior_To_AriaStreet"),
                FastVsHouseArea.AriaInterior,
                FastVsHouseArea.AriaStreet,
                "Aria interior to Aria street");
        }

        private static void VerifyStartupFraming(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility)
        {
            if (visibility.ActiveAreaForReview != FastVsHouseArea.CentralPlaza)
            {
                throw new InvalidOperationException($"Startup active area was {visibility.ActiveAreaForReview}, expected CentralPlaza.");
            }

            if (controller.PlayerInOtherTime)
            {
                throw new InvalidOperationException("Startup player must remain in current time.");
            }

            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            var player = RequireObject<CharacterController>("player controller");
            RequireActiveRenderer("Current_CentralPlazaMap_SeparateSpace", "central plaza stage");
            var playerRenderer = RequireActiveRenderer("FastVS_PlayerVisual_NiroPaper", "player visual");

            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            if ((camera.cullingMask & playerBit) == 0)
            {
                throw new InvalidOperationException("Startup camera culling mask does not include the visible player layer.");
            }

            var viewport = camera.WorldToViewportPoint(player.transform.position + Vector3.up * 0.75f);
            if (viewport.z <= 0f ||
                viewport.x < 0.12f ||
                viewport.x > 0.88f ||
                viewport.y < 0.10f ||
                viewport.y > 0.90f)
            {
                throw new InvalidOperationException($"Startup player framing is out of range: viewport={viewport}.");
            }

            var playerLocal = controller.GetPlayerLocalCoordinateForReview();
            if (playerLocal.y < -0.05f || playerLocal.y > 0.45f)
            {
                throw new InvalidOperationException($"Startup player local height drifted out of range after warmup: local={playerLocal}.");
            }

            VerifyRendererFraming(camera, playerRenderer, "player visual");
        }

        private static void VerifyRendererFraming(Camera camera, Renderer renderer, string label)
        {
            if (renderer == null)
            {
                throw new InvalidOperationException($"Missing {label} renderer.");
            }

            var bounds = renderer.bounds;
            if (bounds.size.y < 0.25f || bounds.size.x < 0.10f)
            {
                throw new InvalidOperationException($"{label} renderer bounds are too small to confirm visibility: size={bounds.size}.");
            }

            var center = camera.WorldToViewportPoint(bounds.center);
            var top = camera.WorldToViewportPoint(bounds.center + Vector3.up * bounds.extents.y);
            var bottom = camera.WorldToViewportPoint(bounds.center - Vector3.up * bounds.extents.y);
            var height = Mathf.Abs(top.y - bottom.y);
            if (center.z <= 0f ||
                center.x < 0.08f ||
                center.x > 0.92f ||
                center.y < 0.08f ||
                center.y > 0.92f ||
                height < 0.045f)
            {
                throw new InvalidOperationException($"{label} renderer is not framed clearly enough: center={center}, viewportHeight={height:0.000}.");
            }
        }

        private static void VerifyTravel(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsAreaDoorTransition transition,
            FastVsHouseArea sourceArea,
            FastVsHouseArea targetArea,
            string label)
        {
            visibility.SetActiveAreaForReview(sourceArea);
            controller.ForcePlayerCurrentLocalForReview(transition.TriggerLocalCenterForReview);
            if (!transition.TryEvaluateCurrentPlayerForReview())
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

        private static Renderer RequireActiveRenderer(string objectName)
        {
            return RequireActiveRenderer(objectName, objectName);
        }

        private static Renderer RequireActiveRenderer(string objectName, string label)
        {
            var gameObject = GameObject.Find(objectName);
            if (gameObject == null)
            {
                throw new InvalidOperationException($"Missing {label} {objectName}.");
            }

            var renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].enabled && renderers[i].gameObject.activeInHierarchy)
                {
                    return renderers[i];
                }
            }

            throw new InvalidOperationException($"{label} {objectName} has no active renderer.");
        }

        private static void FrameCameraAtLocal(Transform spaceRoot, Vector3 localPosition, Vector3 positionOffset, Vector3 lookAtOffset)
        {
            var camera = Camera.main;
            if (camera == null || spaceRoot == null)
            {
                return;
            }

            var anchor = spaceRoot.TransformPoint(localPosition);
            camera.cullingMask = ~0;
            camera.orthographic = false;
            camera.nearClipPlane = Mathf.Max(camera.nearClipPlane, 0.3f);
            camera.farClipPlane = Mathf.Max(camera.farClipPlane, 140f);
            camera.transform.position = anchor + positionOffset;
            camera.transform.LookAt(anchor + lookAtOffset, Vector3.up);
        }

        private static string VerifyVisibleRoot(string rootName, string label, int minEnabledRenderers)
        {
            var root = GameObject.Find(rootName);
            if (root == null)
            {
                throw new InvalidOperationException($"R-2 missing active root {rootName} for {label}.");
            }

            if (!root.activeInHierarchy)
            {
                throw new InvalidOperationException($"R-2 root {rootName} is not active in hierarchy for {label}.");
            }

            var enabledCount = 0;
            var visibleCount = 0;
            var boundsCount = 0;
            var renderers = root.GetComponentsInChildren<Renderer>(true);
            for (var index = 0; index < renderers.Length; index++)
            {
                var renderer = renderers[index];
                if (renderer == null || !renderer.enabled || !renderer.gameObject.activeInHierarchy)
                {
                    continue;
                }

                enabledCount++;
                if (renderer.isVisible)
                {
                    visibleCount++;
                }

                var size = renderer.bounds.size;
                if (IsFinite(size) && size.sqrMagnitude > 0.0001f)
                {
                    boundsCount++;
                }
            }

            if (enabledCount < minEnabledRenderers)
            {
                throw new InvalidOperationException($"R-2 root {rootName} only has {enabledCount} enabled renderer(s), expected at least {minEnabledRenderers}.");
            }

            if (visibleCount <= 0)
            {
                throw new InvalidOperationException($"R-2 root {rootName} has no renderer visible from the runtime probe camera.");
            }

            if (boundsCount < enabledCount / 2)
            {
                throw new InvalidOperationException($"R-2 root {rootName} has too few finite renderer bounds: finite={boundsCount}, enabled={enabledCount}.");
            }

            return $"enabled={enabledCount} visible={visibleCount} finiteBounds={boundsCount}";
        }

        private static string VerifyInactiveRoot(string rootName, string label)
        {
            var root = FindSceneGameObjectByName(rootName);
            if (root == null)
            {
                throw new InvalidOperationException($"R-2 missing root {rootName} for {label}.");
            }

            if (root.activeInHierarchy)
            {
                throw new InvalidOperationException($"R-2 root {rootName} stayed active for inactive-time {label}.");
            }

            return $"activeSelf={root.activeSelf} activeInHierarchy={root.activeInHierarchy}";
        }

        private static void VerifyOpaqueFacadeRenderer(string objectName, string label)
        {
            var renderer = RequireActiveRenderer(objectName, label);
            var material = renderer.sharedMaterial;
            if (material == null)
            {
                throw new InvalidOperationException($"R-3 {label} has no shared material.");
            }

            if (material.renderQueue >= 2990)
            {
                throw new InvalidOperationException($"R-3 {label} uses transparent renderQueue {material.renderQueue} on {material.name}.");
            }

            if (material.HasProperty("_Surface") && Mathf.RoundToInt(material.GetFloat("_Surface")) != 0)
            {
                throw new InvalidOperationException($"R-3 {label} material {material.name} is not opaque _Surface=0.");
            }

            if (material.HasProperty("_BaseColor") && material.GetColor("_BaseColor").a < 0.98f)
            {
                throw new InvalidOperationException($"R-3 {label} material {material.name} has alpha {material.GetColor("_BaseColor").a:0.000}.");
            }

            Debug.Log($"[R236] R-3 {label}: material={material.name} queue={material.renderQueue} alpha={(material.HasProperty("_BaseColor") ? material.GetColor("_BaseColor").a : 1f):0.000}");
        }

        private static void VerifyCurrentBackSideBlock(TimeWindowPairedSpacePortalController controller)
        {
            var player = RequireObject<CharacterController>("player controller");
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
                throw new InvalidOperationException("R-6 Time Window current-side back entry was not rejected.");
            }

            var currentLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            if (currentLocal.z < portalLocal.z + blockDepth - 0.05f)
            {
                throw new InvalidOperationException($"R-6 player pierced through current-side back blocker: currentLocal={currentLocal}, portalLocal={portalLocal}.");
            }
        }

        private static bool IsFinite(Vector3 value)
        {
            return !float.IsNaN(value.x) && !float.IsInfinity(value.x) &&
                   !float.IsNaN(value.y) && !float.IsInfinity(value.y) &&
                   !float.IsNaN(value.z) && !float.IsInfinity(value.z);
        }

        private static GameObject FindSceneGameObjectByName(string objectName)
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (var index = 0; index < objects.Length; index++)
            {
                var candidate = objects[index];
                if (candidate != null &&
                    candidate.scene.IsValid() &&
                    string.Equals(candidate.name, objectName, StringComparison.Ordinal))
                {
                    return candidate;
                }
            }

            return null;
        }

        private sealed class R2AreaProbe
        {
            public readonly FastVsHouseArea Area;
            public readonly string CurrentRootName;
            public readonly string PastRootName;
            public readonly Vector3 PlayerLocal;
            public readonly Vector3 CameraOffset;
            public readonly Vector3 LookAtOffset;
            public readonly int MinEnabledRenderers;

            public R2AreaProbe(
                FastVsHouseArea area,
                string currentRootName,
                string pastRootName,
                Vector3 playerLocal,
                Vector3 cameraOffset,
                Vector3 lookAtOffset,
                int minEnabledRenderers)
            {
                Area = area;
                CurrentRootName = currentRootName;
                PastRootName = pastRootName;
                PlayerLocal = playerLocal;
                CameraOffset = cameraOffset;
                LookAtOffset = lookAtOffset;
                MinEnabledRenderers = minEnabledRenderers;
            }
        }
    }
}
