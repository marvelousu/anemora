using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Anemora.TimeManagement;
using Anemora.TimeManagement.Portal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.FastVS
{
    public sealed class FastVsHouseRuntimeSmokeProbe : MonoBehaviour
    {
        private const string EnableArgument = "--anemora-house-slice-smoke";
        private const string PerformanceArgument = "--anemora-house-slice-perf";
        private const string R236RecheckArgument = "--anemora-house-slice-r236-recheck";
        private const string VisualDiagArgument = "--anemora-house-slice-visual-diag";
        private const string LibraryTablePlainProofArgument = "--anemora-house-slice-library-table-plain-proof";
        private const string ReviewDirArgument = "--anemora-house-slice-review-dir";
        private const string PassMarker = "ANEMORA_HOUSE_SLICE_SMOKE_PASS";
        private const string FailMarker = "ANEMORA_HOUSE_SLICE_SMOKE_FAIL";
        private const string PerformanceMarker = "ANEMORA_HOUSE_SLICE_PERF";
        private const string R236RecheckPassMarker = "ANEMORA_HOUSE_SLICE_R236_RECHECK_PASS";
        private const string R236RecheckFailMarker = "ANEMORA_HOUSE_SLICE_R236_RECHECK_FAIL";
        private const string VisualDiagPassMarker = "ANEMORA_HOUSE_SLICE_VISUAL_DIAG_PASS";
        private const string VisualDiagFailMarker = "ANEMORA_HOUSE_SLICE_VISUAL_DIAG_FAIL";
        private const string LibraryTablePlainProofPassMarker = "ANEMORA_HOUSE_SLICE_LIBRARY_TABLE_PLAIN_PROOF_PASS";
        private const string LibraryTablePlainProofFailMarker = "ANEMORA_HOUSE_SLICE_LIBRARY_TABLE_PLAIN_PROOF_FAIL";
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
        private static readonly Vector3 Chapter1MiaHouseMapCenter = CentralPlazaVsCenter + new Vector3(3.70f, 0f, -1.55f);
        private static readonly Vector3 Chapter1AriaStreetMapCenter = CentralPlazaVsCenter + new Vector3(25.50f, 0f, -1.75f);
        private static readonly Vector3 Chapter1KaiaFarmMapCenter = CentralPlazaVsCenter + new Vector3(32.50f, 0f, -2.85f);
        private static readonly Vector3 Chapter1RuinsMapCenter = CentralPlazaVsCenter + new Vector3(45.50f, 0f, 0.05f);
        private static readonly Vector3 Chapter1EndSideViewCenter = CentralPlazaVsCenter + new Vector3(9.10f, 0f, -10.50f);
        private static readonly Vector3 Chapter1EndSideViewCameraAnchor = Chapter1EndSideViewCenter + new Vector3(-1.05f, 1.45f, 0f);
        private static readonly Vector3 Chapter1EndSideViewPreviewTarget = Chapter1EndSideViewCenter + new Vector3(-2.40f, 0.02f, 0f);
        private const float Chapter1EndSideViewOrthographicSize = 2.80f;
        private static readonly Vector3 CentralPlazaPerformanceStartLocal = new Vector3(20.46f, 0.02f, 19.06f);
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
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

            if (ShouldRun(VisualDiagArgument))
            {
                yield return RunVisualDiagProbe();
                yield break;
            }

            if (ShouldRun(LibraryTablePlainProofArgument))
            {
                yield return RunLibraryTablePlainProofProbe();
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

        private static string GetArgumentValue(string argument)
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (string.Equals(args[i], argument, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return string.Empty;
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

        private static IEnumerator RunVisualDiagProbe()
        {
            Application.runInBackground = true;

            var outputDirectory = ResolveVisualDiagOutputDirectory();
            var logPath = Path.Combine(outputDirectory, "visual_diag.log");
            var reportPath = Path.Combine(outputDirectory, "REPORT.md");
            var builder = new StringBuilder(64 * 1024);
            Exception failure = null;

            try
            {
                Directory.CreateDirectory(outputDirectory);
                foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
                {
                    File.Delete(existingPng);
                }

                AppendLine(builder, "# Fast VS House Slice Visual Diag");
                AppendLine(builder, $"timestampLocal={DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff}");
                AppendLine(builder, $"outputDirectory={Path.GetFullPath(outputDirectory)}");
                AppendLine(builder, $"commandLine={string.Join(" ", Environment.GetCommandLineArgs())}");
            }
            catch (Exception exception)
            {
                FinishVisualDiagFailure(exception, logPath, builder);
                yield break;
            }

            for (var frame = 0; frame < 140; frame++)
            {
                yield return null;
            }

            var nearSamples = new List<string>();
            for (var sample = 0; sample < 8; sample++)
            {
                var camera = Camera.main;
                nearSamples.Add(camera != null
                    ? $"frame={Time.frameCount} near={camera.nearClipPlane:0.000} far={camera.farClipPlane:0.000} fov={camera.fieldOfView:0.000} orthographic={camera.orthographic}"
                    : $"frame={Time.frameCount} Camera.main=null");

                for (var wait = 0; wait < 15; wait++)
                {
                    yield return null;
                }
            }

            try
            {
                AppendVisualRuntimeDiagnostics(builder, nearSamples);
            }
            catch (Exception exception)
            {
                FinishVisualDiagFailure(exception, logPath, builder);
                yield break;
            }

            yield return CaptureVisualDiagAllMaps(outputDirectory, builder, exception => failure = exception);
            if (failure != null)
            {
                FinishVisualDiagFailure(failure, logPath, builder);
                yield break;
            }

            try
            {
                File.WriteAllText(logPath, builder.ToString(), Encoding.UTF8);
                WriteVisualDiagReport(outputDirectory, reportPath, logPath);
                Debug.Log(builder.ToString());
                Debug.Log($"{VisualDiagPassMarker}: output={Path.GetFullPath(outputDirectory)} log={Path.GetFullPath(logPath)}");
                Application.Quit(0);
            }
            catch (Exception exception)
            {
                FinishVisualDiagFailure(exception, logPath, builder);
            }
        }

        private static string ResolveVisualDiagOutputDirectory()
        {
            var configured = GetArgumentValue(ReviewDirArgument);
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured.Trim('"');
            }

            return Path.Combine(
                Application.persistentDataPath,
                $"{DateTime.Now:yyyy-MM-ddTHH-mm}_visual_diag");
        }

        private static string ResolveReviewOutputDirectory(string fallbackSuffix)
        {
            var configured = GetArgumentValue(ReviewDirArgument);
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured.Trim('"');
            }

            return Path.Combine(
                Application.persistentDataPath,
                $"{DateTime.Now:yyyy-MM-ddTHH-mm}_{fallbackSuffix}");
        }

        private static IEnumerator RunLibraryTablePlainProofProbe()
        {
            Application.runInBackground = true;

            var outputDirectory = ResolveReviewOutputDirectory("library_table_plain_proof");
            var logPath = Path.Combine(outputDirectory, "library_table_plain_proof.log");
            var reportPath = Path.Combine(outputDirectory, "REPORT.md");
            var builder = new StringBuilder(32 * 1024);
            Exception failure = null;

            try
            {
                Directory.CreateDirectory(outputDirectory);
                foreach (var existingPng in Directory.GetFiles(outputDirectory, "*.png"))
                {
                    File.Delete(existingPng);
                }

                AppendLine(builder, "# Fast VS House Slice Library Table Plain Proof");
                AppendLine(builder, $"timestampLocal={DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff}");
                AppendLine(builder, $"outputDirectory={Path.GetFullPath(outputDirectory)}");
                AppendLine(builder, $"commandLine={string.Join(" ", Environment.GetCommandLineArgs())}");
            }
            catch (Exception exception)
            {
                FinishLibraryTablePlainProofFailure(exception, logPath, builder);
                yield break;
            }

            for (var frame = 0; frame < 140; frame++)
            {
                yield return null;
            }

            yield return CaptureLibraryTablePlainProof(outputDirectory, builder, exception => failure = exception);
            if (failure != null)
            {
                FinishLibraryTablePlainProofFailure(failure, logPath, builder);
                yield break;
            }

            try
            {
                AppendLibraryTableRendererProof(builder);
                File.WriteAllText(logPath, builder.ToString(), Encoding.UTF8);
                WriteLibraryTablePlainProofReport(outputDirectory, reportPath, logPath);
                Debug.Log(builder.ToString());
                Debug.Log($"{LibraryTablePlainProofPassMarker}: output={Path.GetFullPath(outputDirectory)} log={Path.GetFullPath(logPath)}");
                Application.Quit(0);
            }
            catch (Exception exception)
            {
                FinishLibraryTablePlainProofFailure(exception, logPath, builder);
            }
        }

        private static IEnumerator CaptureLibraryTablePlainProof(string outputDirectory, StringBuilder builder, Action<Exception> fail)
        {
            var controller = default(TimeWindowPairedSpacePortalController);
            var visibility = default(FastVsHouseAreaVisibility);
            var guide = default(FastVsVisualDirectionGuide);
            var camera = default(Camera);

            try
            {
                controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
                visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
                guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
                camera = RequireObject<Camera>("main camera");
                AppendLine(builder, string.Empty);
                AppendLine(builder, "## Built Player Library Table Close Captures");
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            var captureFailure = default(Exception);
            Action<Exception> captureFail = exception =>
            {
                captureFailure = exception;
                fail(exception);
            };

            yield return CaptureLibraryTableCloseCurrent(
                controller,
                visibility,
                guide,
                camera,
                LibraryVsCenter + new Vector3(1.12f, 0.02f, 0.12f),
                LibraryVsCenter + new Vector3(1.12f, 0.40f, 0.10f),
                new Vector3(-0.34f, 1.18f, -2.18f),
                new Vector3(0f, 0.14f, 0.06f),
                Path.Combine(outputDirectory, "01_current_library_reto_table_plain_close.png"),
                builder,
                captureFail);
            if (captureFailure != null)
            {
                yield break;
            }

            yield return CaptureLibraryTableClosePast(
                controller,
                visibility,
                guide,
                camera,
                LibraryVsCenter + new Vector3(0f, 0.02f, -0.92f),
                LibraryVsCenter + new Vector3(0f, 0.40f, -0.88f),
                new Vector3(-0.18f, 1.34f, -2.92f),
                new Vector3(0f, 0.12f, -0.18f),
                Path.Combine(outputDirectory, "02_past_library_clean_reading_tables_plain_close.png"),
                builder,
                captureFail);
            if (captureFailure != null)
            {
                yield break;
            }

            yield return CaptureLibraryTableCloseCurrent(
                controller,
                visibility,
                guide,
                camera,
                LibraryVsCenter + new Vector3(-1.72f, 0.02f, 1.42f),
                LibraryVsCenter + new Vector3(-1.72f, 0.40f, 1.40f),
                new Vector3(-0.08f, 1.16f, -2.02f),
                new Vector3(0f, 0.16f, -0.04f),
                Path.Combine(outputDirectory, "03_current_library_side_table_plain_close.png"),
                builder,
                captureFail);
        }

        private static IEnumerator CaptureLibraryTableCloseCurrent(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            Vector3 playerLocalPosition,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            string outputPath,
            StringBuilder builder,
            Action<Exception> fail)
        {
            try
            {
                visibility.SetActiveAreaForReview(FastVsHouseArea.Library);
                visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(false);
                controller.ForcePlayerCurrentLocalForReview(playerLocalPosition);
                guide.ApplyActiveTimeIsolationForReview();
                PositionCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            yield return new WaitForEndOfFrame();

            try
            {
                CaptureCameraPng(camera, outputPath, builder);
            }
            catch (Exception exception)
            {
                fail(exception);
            }
        }

        private static IEnumerator CaptureLibraryTableClosePast(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            Vector3 playerLocalPosition,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            string outputPath,
            StringBuilder builder,
            Action<Exception> fail)
        {
            var previousMask = camera != null ? camera.cullingMask : 0;
            try
            {
                visibility.SetActiveAreaForReview(FastVsHouseArea.Library);
                visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(true);
                controller.ForcePlayerOtherTimeLocalForReview(playerLocalPosition);
                guide.ApplyActiveTimeIsolationForReview();
                var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
                var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
                var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
                camera.cullingMask = (previousMask & ~currentBit) | otherBit | playerBit;
                PositionCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            yield return new WaitForEndOfFrame();

            try
            {
                CaptureCameraPng(camera, outputPath, builder);
            }
            catch (Exception exception)
            {
                fail(exception);
            }
            finally
            {
                if (camera != null)
                {
                    camera.cullingMask = previousMask;
                }

                controller.ForcePlayerCurrentLocalForReview(playerLocalPosition);
                visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(false);
                guide.ApplyActiveTimeIsolationForReview();
            }
        }

        private static void AppendLibraryTableRendererProof(StringBuilder builder)
        {
            AppendLine(builder, string.Empty);
            AppendLine(builder, "## Library Table Renderer Materials");
            var renderers = FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var tableRendererCount = 0;
            var seamRendererCount = 0;
            var logged = 0;
            foreach (var renderer in renderers)
            {
                if (renderer == null)
                {
                    continue;
                }

                var path = RendererPath(renderer);
                var isLibraryTable =
                    path.Contains("Library_ReadingTable", StringComparison.OrdinalIgnoreCase) ||
                    path.Contains("Library_ReadingTableClean", StringComparison.OrdinalIgnoreCase) ||
                    path.Contains("Library_ServiceDesk", StringComparison.OrdinalIgnoreCase);
                if (!isLibraryTable)
                {
                    continue;
                }

                tableRendererCount++;
                if (path.Contains("TabletopPlankSeam", StringComparison.OrdinalIgnoreCase))
                {
                    seamRendererCount++;
                }

                if (logged >= 36)
                {
                    continue;
                }

                var material = renderer.sharedMaterial;
                AppendLine(
                    builder,
                    $"libraryTableRenderer[{logged}] name={path} enabled={renderer.enabled} material={MaterialName(material)} shader={(material != null && material.shader != null ? material.shader.name : "null")} renderQueue={(material != null ? material.renderQueue.ToString(CultureInfo.InvariantCulture) : "n/a")} baseAlpha={(material != null ? MaterialAlpha(material).ToString("0.000", CultureInfo.InvariantCulture) : "n/a")} boundsCenter={FormatVector(renderer.bounds.center)} boundsSize={FormatVector(renderer.bounds.size)}");
                logged++;
            }

            AppendLine(builder, $"libraryTableRenderer.count={tableRendererCount}");
            AppendLine(builder, $"libraryTableRenderer.tabletopPlankSeamCount={seamRendererCount}");
        }

        private static void WriteLibraryTablePlainProofReport(string outputDirectory, string reportPath, string logPath)
        {
            var pngCount = Directory.GetFiles(outputDirectory, "*.png").Length;
            var lines = new[]
            {
                "# HD-2D Library Table Plain Proof",
                string.Empty,
                "- Scope: Phase 1 built-player close-up proof for library table plain surfaces.",
                $"- Log: `{Path.GetFileName(logPath)}`",
                $"- Built-player PNG count: `{pngCount}`",
                $"- Generated: `{DateTime.Now:yyyy-MM-ddTHH:mm:ss}`"
            };
            File.WriteAllLines(reportPath, lines, Encoding.UTF8);
        }

        private static void FinishLibraryTablePlainProofFailure(Exception exception, string logPath, StringBuilder builder)
        {
            try
            {
                AppendLine(builder, string.Empty);
                AppendLine(builder, $"{LibraryTablePlainProofFailMarker}: {exception}");
                var directory = Path.GetDirectoryName(logPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(logPath, builder.ToString(), Encoding.UTF8);
            }
            catch
            {
            }

            Debug.LogError($"{LibraryTablePlainProofFailMarker}: {exception}");
            Application.Quit(34);
        }

        private static void AppendVisualRuntimeDiagnostics(StringBuilder builder, IReadOnlyList<string> nearSamples)
        {
            AppendLine(builder, string.Empty);
            AppendLine(builder, "## A Lighting And Shadows");
            AppendLights(builder);
            AppendUrpAsset(builder);
            AppendRenderSettings(builder);
            AppendEnvironmentRendererShadowSummary(builder);
            AppendRealtimeLightShadowRig(builder);

            AppendLine(builder, string.Empty);
            AppendLine(builder, "## B Camera And Z Fighting");
            AppendCameraDiagnostics(builder, nearSamples);
            AppendZFightCandidates(builder);

            AppendLine(builder, string.Empty);
            AppendLine(builder, "## C Transparency Candidates");
            AppendTransparencyCandidates(builder);
        }

        private static void AppendLights(StringBuilder builder)
        {
            var lights = FindObjectsByType<Light>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            Array.Sort(lights, (left, right) => string.Compare(left != null ? left.name : string.Empty, right != null ? right.name : string.Empty, StringComparison.Ordinal));
            var directionalCount = 0;
            for (var i = 0; i < lights.Length; i++)
            {
                var light = lights[i];
                if (light == null || light.type != LightType.Directional)
                {
                    continue;
                }

                directionalCount++;
                AppendLine(
                    builder,
                    $"light[{directionalCount}] name={light.name} type={light.type} enabled={light.enabled} shadows={light.shadows} shadowStrength={light.shadowStrength:0.000} intensity={light.intensity:0.000} euler={FormatVector(light.transform.eulerAngles)} color={FormatColor(light.color)} shadowBias={light.shadowBias:0.000} shadowNormalBias={light.shadowNormalBias:0.000} shadowNearPlane={light.shadowNearPlane:0.000}");
            }

            if (directionalCount == 0)
            {
                AppendLine(builder, "light directional count=0");
            }
        }

        private static void AppendUrpAsset(StringBuilder builder)
        {
            var pipeline = GraphicsSettings.currentRenderPipeline;
            if (pipeline == null)
            {
                AppendLine(builder, "urpAsset=null");
                return;
            }

            AppendLine(builder, $"urpAsset={pipeline.name} type={pipeline.GetType().FullName}");
            AppendLine(builder, $"urp.supportsMainLightShadows={GetMemberValue(pipeline, "supportsMainLightShadows", "mainLightShadowsSupported", "m_MainLightShadowsSupported")}");
            AppendLine(builder, $"urp.shadowDistance={GetMemberValue(pipeline, "shadowDistance", "m_ShadowDistance")}");
            AppendLine(builder, $"urp.shadowCascadeCount={GetMemberValue(pipeline, "shadowCascadeCount", "m_ShadowCascadeCount")}");
            AppendLine(builder, $"urp.mainLightShadowmapResolution={GetMemberValue(pipeline, "mainLightShadowmapResolution", "m_MainLightShadowmapResolution")}");
            AppendLine(builder, $"urp.additionalLightsShadowmapResolution={GetMemberValue(pipeline, "additionalLightsShadowmapResolution", "m_AdditionalLightsShadowmapResolution")}");
        }

        private static void AppendRenderSettings(StringBuilder builder)
        {
            AppendLine(
                builder,
                $"renderSettings ambientMode={RenderSettings.ambientMode} ambientLight={FormatColor(RenderSettings.ambientLight)} ambientIntensity={RenderSettings.ambientIntensity:0.000} fog={RenderSettings.fog} fogMode={RenderSettings.fogMode} fogColor={FormatColor(RenderSettings.fogColor)} fogDensity={RenderSettings.fogDensity:0.0000} reflectionIntensity={RenderSettings.reflectionIntensity:0.000} skybox={(RenderSettings.skybox != null ? RenderSettings.skybox.name : "null")}");
        }

        private static void AppendEnvironmentRendererShadowSummary(StringBuilder builder)
        {
            var renderers = FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            var shadowOn = 0;
            var shadowOff = 0;
            var receiveOn = 0;
            var receiveOff = 0;
            var envCount = 0;
            var representative = new List<Renderer>();

            Array.Sort(renderers, (left, right) => string.Compare(RendererPath(left), RendererPath(right), StringComparison.Ordinal));
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (!IsEnvironmentRenderer(renderer))
                {
                    continue;
                }

                envCount++;
                if (renderer.shadowCastingMode == ShadowCastingMode.On)
                {
                    shadowOn++;
                }
                else
                {
                    shadowOff++;
                }

                if (renderer.receiveShadows)
                {
                    receiveOn++;
                }
                else
                {
                    receiveOff++;
                }

                if (representative.Count < 10 && IsRepresentativeEnvironmentRendererName(renderer.gameObject.name))
                {
                    representative.Add(renderer);
                }
            }

            AppendLine(builder, $"environmentRendererSummary activeEnvironmentRenderers={envCount} shadowCastingMode.On={shadowOn} shadowCastingMode.OtherOrOff={shadowOff} receiveShadows.true={receiveOn} receiveShadows.false={receiveOff}");
            for (var i = 0; i < representative.Count; i++)
            {
                var renderer = representative[i];
                AppendLine(builder, $"environmentRenderer[{i}] name={RendererPath(renderer)} shadowCastingMode={renderer.shadowCastingMode} receiveShadows={renderer.receiveShadows} material={MaterialName(renderer.sharedMaterial)} boundsCenter={FormatVector(renderer.bounds.center)} boundsSize={FormatVector(renderer.bounds.size)}");
            }
        }

        private static void AppendRealtimeLightShadowRig(StringBuilder builder)
        {
            var rigs = FindObjectsByType<FastVsRealtimeLightShadowRig>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            AppendLine(builder, $"FastVsRealtimeLightShadowRig.count={rigs.Length}");
            for (var i = 0; i < rigs.Length; i++)
            {
                var rig = rigs[i];
                AppendLine(builder, $"FastVsRealtimeLightShadowRig[{i}] name={rig.name} active={rig.gameObject.activeInHierarchy} lateUpdateHeartbeat={rig.LateUpdateHeartbeatForReview} lastWrite={rig.LastRuntimeWriteSummaryForReview}");
            }
        }

        private static void AppendCameraDiagnostics(StringBuilder builder, IReadOnlyList<string> nearSamples)
        {
            var camera = Camera.main;
            if (camera == null)
            {
                AppendLine(builder, "Camera.main=null");
            }
            else
            {
                AppendLine(builder, $"Camera.main name={camera.name} near={camera.nearClipPlane:0.000} far={camera.farClipPlane:0.000} fov={camera.fieldOfView:0.000} orthographic={camera.orthographic} orthographicSize={camera.orthographicSize:0.000} pos={FormatVector(camera.transform.position)} euler={FormatVector(camera.transform.eulerAngles)} cullingMask={camera.cullingMask}");
                var additionalData = camera.GetComponent<UniversalAdditionalCameraData>();
                AppendLine(builder, additionalData != null
                    ? $"Camera.main.urp antialiasing={additionalData.antialiasing} antialiasingQuality={FormatObjectValue(GetMemberValue(additionalData, "antialiasingQuality"))} renderPostProcessing={additionalData.renderPostProcessing} requiresDepthTexture={additionalData.requiresDepthTexture} requiresColorTexture={additionalData.requiresColorTexture}"
                    : "Camera.main.urp=null");
            }

            for (var i = 0; i < nearSamples.Count; i++)
            {
                AppendLine(builder, $"nearSample[{i}] {nearSamples[i]}");
            }

            AppendCinemachineDiagnostics(builder);
        }

        private static void AppendCinemachineDiagnostics(StringBuilder builder)
        {
            var components = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            var count = 0;
            for (var i = 0; i < components.Length; i++)
            {
                var component = components[i];
                if (component == null)
                {
                    continue;
                }

                var typeName = component.GetType().FullName;
                if (typeName == null || typeName.IndexOf("Cinemachine", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                AppendLine(builder, $"cinemachine[{count}] object={component.gameObject.name} component={typeName} enabled={component.enabled}");
                count++;
                if (count >= 16)
                {
                    break;
                }
            }

            if (count == 0)
            {
                AppendLine(builder, "cinemachine.count=0");
            }
        }

        private static void AppendZFightCandidates(StringBuilder builder)
        {
            var renderers = FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            Array.Sort(renderers, (left, right) => string.Compare(RendererPath(left), RendererPath(right), StringComparison.Ordinal));
            var count = 0;
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null)
                {
                    continue;
                }

                var name = renderer.gameObject.name;
                if (!name.Contains("PlazaFloor") &&
                    !name.Contains("StoneSquare") &&
                    !name.Contains("GroundBase") &&
                    !name.Contains("Ground") &&
                    !name.Contains("Floor"))
                {
                    continue;
                }

                var bounds = renderer.bounds;
                AppendLine(builder, $"zFightCandidate[{count}] name={RendererPath(renderer)} worldYMin={bounds.min.y:0.0000} worldYMax={bounds.max.y:0.0000} thicknessY={bounds.size.y:0.0000} center={FormatVector(bounds.center)} size={FormatVector(bounds.size)} shadowCastingMode={renderer.shadowCastingMode} receiveShadows={renderer.receiveShadows} material={MaterialName(renderer.sharedMaterial)}");
                count++;
                if (count >= 30)
                {
                    break;
                }
            }

            if (count == 0)
            {
                AppendLine(builder, "zFightCandidate.count=0");
            }
        }

        private static void AppendTransparencyCandidates(StringBuilder builder)
        {
            var renderers = FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            Array.Sort(renderers, (left, right) => string.Compare(RendererPath(left), RendererPath(right), StringComparison.Ordinal));
            var count = 0;
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null)
                {
                    continue;
                }

                var name = renderer.gameObject.name;
                if (!IsTransparencyCandidateName(name))
                {
                    continue;
                }

                var materials = renderer.sharedMaterials;
                for (var materialIndex = 0; materialIndex < materials.Length; materialIndex++)
                {
                    var material = materials[materialIndex];
                    if (material == null)
                    {
                        continue;
                    }

                    var relevantMaterial =
                        material.renderQueue >= 2450 ||
                        MaterialFloatRounded(material, "_Surface") != "n/a" ||
                        MaterialFloatRounded(material, "_Cull") == "0" ||
                        MaterialFloatRounded(material, "_ZWrite") == "0" ||
                        name.Contains("Library") ||
                        name.Contains("Backdrop") ||
                        name.Contains("Aperture") ||
                        name.Contains("Portal");
                    if (!relevantMaterial)
                    {
                        continue;
                    }

                    AppendLine(
                        builder,
                        $"transparentCandidate[{count}] renderer={RendererPath(renderer)} materialIndex={materialIndex} material={material.name} shader={(material.shader != null ? material.shader.name : "null")} renderQueue={material.renderQueue} _Surface={MaterialFloatRounded(material, "_Surface")} _Blend={MaterialFloatRounded(material, "_Blend")} _Cull={MaterialFloatRounded(material, "_Cull")} _ZWrite={MaterialFloatRounded(material, "_ZWrite")} baseAlpha={MaterialAlpha(material):0.000} keywords={string.Join("|", material.shaderKeywords)} shadowCastingMode={renderer.shadowCastingMode} receiveShadows={renderer.receiveShadows} boundsCenter={FormatVector(renderer.bounds.center)} boundsSize={FormatVector(renderer.bounds.size)}");
                    count++;
                    if (count >= 40)
                    {
                        return;
                    }
                }
            }

            if (count == 0)
            {
                AppendLine(builder, "transparentCandidate.count=0");
            }
        }

        private static IEnumerator CaptureVisualDiagAllMaps(string outputDirectory, StringBuilder builder, Action<Exception> fail)
        {
            var controller = default(TimeWindowPairedSpacePortalController);
            var visibility = default(FastVsHouseAreaVisibility);
            var guide = default(FastVsVisualDirectionGuide);
            var camera = default(Camera);

            try
            {
                controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
                visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");
                guide = RequireObject<FastVsVisualDirectionGuide>("visual direction guide");
                camera = RequireObject<Camera>("main camera");
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            AppendLine(builder, string.Empty);
            AppendLine(builder, "## Built Player All Map Captures");
            var captureFailure = default(Exception);
            Action<Exception> captureFail = exception =>
            {
                captureFailure = exception;
                fail(exception);
            };

            yield return CaptureVisualDiagAllMapPair(controller, visibility, guide, camera, FastVsHouseArea.Exterior, HouseExteriorCenter + new Vector3(2.95f, 0.02f, 1.10f), new Vector3(0f, 13.80f, -18.20f), new Vector3(0f, 0.20f, 1.55f), Path.Combine(outputDirectory, "01_a1_a2_current.png"), Path.Combine(outputDirectory, "02_a1_a2_past.png"), builder, captureFail);
            if (captureFailure != null)
            {
                yield break;
            }

            yield return CaptureVisualDiagAllMapPair(controller, visibility, guide, camera, FastVsHouseArea.CentralPlaza, CentralPlazaVsCenter + new Vector3(1.45f, 0.02f, -0.20f), new Vector3(0f, 13.80f, -18.20f), new Vector3(0f, 0.20f, 1.55f), Path.Combine(outputDirectory, "03_b1_b3_current.png"), Path.Combine(outputDirectory, "04_b1_b3_past.png"), builder, captureFail);
            if (captureFailure != null) yield break;
            yield return CaptureVisualDiagAllMapPair(controller, visibility, guide, camera, FastVsHouseArea.MiaHouse, Chapter1MiaHouseMapCenter + new Vector3(0f, 0.02f, 0f), new Vector3(0f, 17.90f, -25.20f), new Vector3(0.10f, 0.20f, 2.90f), Path.Combine(outputDirectory, "05_c1_c3_current.png"), Path.Combine(outputDirectory, "06_c1_c3_past.png"), builder, captureFail);
            if (captureFailure != null) yield break;
            yield return CaptureVisualDiagAllMapPair(controller, visibility, guide, camera, FastVsHouseArea.AriaStreet, Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, 0f), new Vector3(0f, 20.35f, -27.80f), new Vector3(0.80f, 0.22f, 4.10f), Path.Combine(outputDirectory, "07_d1_d3_current.png"), Path.Combine(outputDirectory, "08_d1_d3_past.png"), builder, captureFail);
            if (captureFailure != null) yield break;
            yield return CaptureVisualDiagAllMapPair(controller, visibility, guide, camera, FastVsHouseArea.KaiaFarm, Chapter1KaiaFarmMapCenter + new Vector3(0f, 0.02f, 0f), new Vector3(0f, 20.95f, -28.90f), new Vector3(0.85f, 0.24f, 4.60f), Path.Combine(outputDirectory, "09_e1_e3_current.png"), Path.Combine(outputDirectory, "10_e1_e3_past.png"), builder, captureFail);
            if (captureFailure != null) yield break;
            yield return CaptureVisualDiagAllMapPair(controller, visibility, guide, camera, FastVsHouseArea.Ruins, Chapter1RuinsMapCenter + new Vector3(0f, 0.02f, -0.45f), new Vector3(-0.08f, 25.35f, -40.30f), new Vector3(0.44f, 0.28f, 5.84f), Path.Combine(outputDirectory, "11_f1_f6_current.png"), Path.Combine(outputDirectory, "12_f1_f6_past.png"), builder, captureFail);
            if (captureFailure != null) yield break;
            yield return CaptureVisualDiagEndSideView(controller, visibility, guide, camera, Path.Combine(outputDirectory, "13_scene6_sideview_auto.png"), builder, captureFail);
        }

        private static IEnumerator CaptureVisualDiagAllMapPair(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHouseArea area,
            Vector3 localPosition,
            Vector3 positionOffset,
            Vector3 lookAtOffset,
            string currentOutputPath,
            string pastOutputPath,
            StringBuilder builder,
            Action<Exception> fail)
        {
            var previousMask = camera != null ? camera.cullingMask : 0;
            try
            {
                visibility.SetRuntimeTimeSetActiveForceKeepBothTimesForReview(false);
                visibility.SetActiveAreaForReview(area);
                controller.ForcePlayerCurrentLocalForReview(localPosition);
                guide.ApplyActiveTimeIsolationForReview();
                PositionCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(localPosition), positionOffset, lookAtOffset);
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            yield return new WaitForEndOfFrame();

            try
            {
                CaptureCameraPng(camera, currentOutputPath, builder);
                controller.ForcePlayerOtherTimeLocalForReview(localPosition);
                guide.ApplyActiveTimeIsolationForReview();
                var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
                var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
                var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
                camera.cullingMask = (previousMask & ~currentBit) | otherBit | playerBit;
                PositionCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(localPosition), positionOffset, lookAtOffset);
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            yield return new WaitForEndOfFrame();

            try
            {
                CaptureCameraPng(camera, pastOutputPath, builder);
                camera.cullingMask = previousMask;
                controller.ForcePlayerCurrentLocalForReview(localPosition);
                guide.ApplyActiveTimeIsolationForReview();
            }
            catch (Exception exception)
            {
                fail(exception);
            }
        }

        private static IEnumerator CaptureVisualDiagEndSideView(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputPath,
            StringBuilder builder,
            Action<Exception> fail)
        {
            var previousOrthographic = camera.orthographic;
            var previousOrthographicSize = camera.orthographicSize;
            try
            {
                visibility.SetActiveAreaForReview(FastVsHouseArea.Chapter1End);
                controller.ForcePlayerCurrentLocalForReview(Chapter1EndSideViewPreviewTarget);
                guide.ApplyActiveTimeIsolationForReview();
                camera.orthographic = true;
                camera.orthographicSize = Chapter1EndSideViewOrthographicSize;
                var anchor = controller.CurrentSpaceRootForReview.TransformPoint(Chapter1EndSideViewCameraAnchor);
                camera.transform.SetPositionAndRotation(anchor + new Vector3(0f, 0f, -13.0f), Quaternion.LookRotation(Vector3.forward, Vector3.up));
            }
            catch (Exception exception)
            {
                fail(exception);
                yield break;
            }

            yield return new WaitForEndOfFrame();

            try
            {
                CaptureCameraPng(camera, outputPath, builder);
            }
            catch (Exception exception)
            {
                fail(exception);
            }
            finally
            {
                camera.orthographic = previousOrthographic;
                camera.orthographicSize = previousOrthographicSize;
            }
        }

        private static void CaptureCameraPng(Camera camera, string outputPath, StringBuilder builder)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? ".");
            EnsureCaptureCamera(camera);
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
                File.WriteAllBytes(outputPath, texture.EncodeToPNG());
                AppendLine(builder, BuildImageStatsLine(outputPath, texture));
            }
            finally
            {
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                renderTexture.Release();
                UnityEngine.Object.Destroy(renderTexture);
                UnityEngine.Object.Destroy(texture);
            }
        }

        private static string BuildImageStatsLine(string outputPath, Texture2D texture)
        {
            var pixels = texture.GetPixels32();
            var count = Math.Max(1, pixels.Length);
            var sumR = 0d;
            var sumG = 0d;
            var sumB = 0d;
            var sumLuma = 0d;
            var sumLumaSq = 0d;
            var sumChroma = 0d;
            for (var i = 0; i < pixels.Length; i++)
            {
                var pixel = pixels[i];
                sumR += pixel.r;
                sumG += pixel.g;
                sumB += pixel.b;
                var luma = (0.2126d * pixel.r) + (0.7152d * pixel.g) + (0.0722d * pixel.b);
                sumLuma += luma;
                sumLumaSq += luma * luma;
                var max = Math.Max(pixel.r, Math.Max(pixel.g, pixel.b));
                var min = Math.Min(pixel.r, Math.Min(pixel.g, pixel.b));
                sumChroma += max - min;
            }

            var meanLuma = sumLuma / count;
            var variance = Math.Max(0d, (sumLumaSq / count) - (meanLuma * meanLuma));
            return string.Format(
                CultureInfo.InvariantCulture,
                "[ALLMAPS] saved {0} size={1}x{2} meanRgb=({3:0.0},{4:0.0},{5:0.0}) stdLum={6:0.0} avgChroma={7:0.0}",
                Path.GetFileName(outputPath),
                texture.width,
                texture.height,
                sumR / count,
                sumG / count,
                sumB / count,
                Math.Sqrt(variance),
                sumChroma / count);
        }

        private static void WriteVisualDiagReport(string outputDirectory, string reportPath, string logPath)
        {
            File.WriteAllText(
                Path.Combine(outputDirectory, "devlog.txt"),
                "docs/HD2D_RUNTIME_INSTRUMENT_FIRST_DIAG_20260606.md" + Environment.NewLine,
                Encoding.UTF8);

            var pngCount = Directory.GetFiles(outputDirectory, "*.png").Length;
            var lines = new[]
            {
                "# HD-2D Runtime Visual Diag",
                string.Empty,
                "- Scope: Phase 0 measurement only. No shadow/flicker/transparency fix is claimed here.",
                $"- Log: `{Path.GetFileName(logPath)}`",
                $"- Built-player PNG count: `{pngCount}`",
                $"- Generated: `{DateTime.Now:yyyy-MM-ddTHH:mm:ss}`"
            };
            File.WriteAllLines(reportPath, lines, Encoding.UTF8);
        }

        private static void FinishVisualDiagFailure(Exception exception, string logPath, StringBuilder builder)
        {
            try
            {
                AppendLine(builder, $"{VisualDiagFailMarker}: {exception}");
                var directory = Path.GetDirectoryName(logPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(logPath, builder.ToString(), Encoding.UTF8);
            }
            catch
            {
                // Best effort: Debug.LogError below is the authoritative failure path if writing fails.
            }

            Debug.LogError($"{VisualDiagFailMarker}: {exception}");
            Application.Quit(33);
        }

        private static void EnsureCaptureCamera(Camera camera)
        {
            if (camera == null)
            {
                throw new InvalidOperationException("Visual diag capture camera is missing.");
            }

            camera.allowHDR = true;
            var additionalData = camera.GetComponent<UniversalAdditionalCameraData>();
            if (additionalData == null)
            {
                additionalData = camera.gameObject.AddComponent<UniversalAdditionalCameraData>();
            }

            additionalData.renderPostProcessing = true;
            additionalData.requiresDepthTexture = true;
            additionalData.requiresColorTexture = true;
            additionalData.volumeLayerMask = ~0;
        }

        private static void PositionCamera(Camera camera, Vector3 anchor, Vector3 positionOffset, Vector3 lookAtOffset)
        {
            camera.orthographic = false;
            camera.fieldOfView = Mathf.Max(camera.fieldOfView, 38f);
            var position = anchor + positionOffset;
            var lookAt = anchor + lookAtOffset;
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
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

        private static string GetMemberValue(object target, params string[] names)
        {
            if (target == null)
            {
                return "null";
            }

            var type = target.GetType();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            for (var i = 0; i < names.Length; i++)
            {
                var property = type.GetProperty(names[i], flags);
                if (property != null && property.CanRead && property.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        return FormatObjectValue(property.GetValue(target, null));
                    }
                    catch (Exception exception)
                    {
                        return $"property-error:{exception.GetType().Name}";
                    }
                }

                var field = type.GetField(names[i], flags);
                if (field != null)
                {
                    try
                    {
                        return FormatObjectValue(field.GetValue(target));
                    }
                    catch (Exception exception)
                    {
                        return $"field-error:{exception.GetType().Name}";
                    }
                }
            }

            return "missing";
        }

        private static string FormatObjectValue(object value)
        {
            if (value == null)
            {
                return "null";
            }

            switch (value)
            {
                case float f:
                    return f.ToString("0.###", CultureInfo.InvariantCulture);
                case double d:
                    return d.ToString("0.###", CultureInfo.InvariantCulture);
                case int i:
                    return i.ToString(CultureInfo.InvariantCulture);
                case bool b:
                    return b ? "true" : "false";
                case Vector2 v2:
                    return FormatVector(v2);
                case Vector3 v3:
                    return FormatVector(v3);
                case Vector4 v4:
                    return string.Format(CultureInfo.InvariantCulture, "({0:0.###},{1:0.###},{2:0.###},{3:0.###})", v4.x, v4.y, v4.z, v4.w);
                case Color c:
                    return FormatColor(c);
                default:
                    return value.ToString();
            }
        }

        private static bool IsEnvironmentRenderer(Renderer renderer)
        {
            if (renderer == null || !renderer.enabled || !renderer.gameObject.activeInHierarchy)
            {
                return false;
            }

            var name = renderer.gameObject.name;
            if (name.Contains("FastVS_SpriteCharacter") ||
                name.Contains("PlayerVisual") ||
                name.Contains("Prompt") ||
                name.Contains("HUD") ||
                name.Contains("Canvas") ||
                name.Contains("Shadow") ||
                name.Contains("LightPool") ||
                name.Contains("Glow") ||
                name.Contains("OpenCue") ||
                name.Contains("Portal") ||
                name.Contains("Aperture") ||
                name.Contains("TimeWindow"))
            {
                return false;
            }

            return name.Contains("Current_") ||
                   name.Contains("Past_") ||
                   name.Contains("House") ||
                   name.Contains("Plaza") ||
                   name.Contains("Library") ||
                   name.Contains("Ground") ||
                   name.Contains("Floor") ||
                   name.Contains("Wall") ||
                   name.Contains("Facade") ||
                   name.Contains("Building") ||
                   name.Contains("Road") ||
                   name.Contains("Path");
        }

        private static bool IsRepresentativeEnvironmentRendererName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!name.Contains("Current_") && !name.Contains("Past_"))
            {
                return false;
            }

            return name.Contains("Current_CentralPlaza") ||
                   name.Contains("Current_HouseExterior") ||
                   name.Contains("Current_Library") ||
                   name.Contains("Past_CentralPlaza") ||
                   name.Contains("Past_Library");
        }

        private static bool IsTransparencyCandidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return name.Contains("Library") ||
                   name.Contains("Facade") ||
                   name.Contains("Backdrop") ||
                   name.Contains("BackPlate") ||
                   name.Contains("BackVolume") ||
                   name.Contains("Occlusion") ||
                   name.Contains("Sky") ||
                   name.Contains("Haze") ||
                   name.Contains("Window") ||
                   name.Contains("Aperture") ||
                   name.Contains("Portal");
        }

        private static string RendererPath(Renderer renderer)
        {
            if (renderer == null)
            {
                return "null";
            }

            var names = new List<string>();
            var current = renderer.transform;
            for (var i = 0; i < 6 && current != null; i++)
            {
                names.Add(current.name);
                current = current.parent;
            }

            names.Reverse();
            return string.Join("/", names);
        }

        private static string MaterialName(Material material)
        {
            return material != null ? material.name : "null";
        }

        private static string MaterialFloatRounded(Material material, string propertyName)
        {
            if (material == null || !material.HasProperty(propertyName))
            {
                return "n/a";
            }

            return material.GetFloat(propertyName).ToString("0.###", CultureInfo.InvariantCulture);
        }

        private static float MaterialAlpha(Material material)
        {
            if (material == null)
            {
                return 1f;
            }

            if (material.HasProperty(BaseColorId))
            {
                return material.GetColor(BaseColorId).a;
            }

            return material.HasProperty(ColorId) ? material.GetColor(ColorId).a : 1f;
        }

        private static string FormatVector(Vector2 value)
        {
            return string.Format(CultureInfo.InvariantCulture, "({0:0.###},{1:0.###})", value.x, value.y);
        }

        private static string FormatVector(Vector3 value)
        {
            return string.Format(CultureInfo.InvariantCulture, "({0:0.###},{1:0.###},{2:0.###})", value.x, value.y, value.z);
        }

        private static string FormatColor(Color value)
        {
            return string.Format(CultureInfo.InvariantCulture, "({0:0.###},{1:0.###},{2:0.###},{3:0.###})", value.r, value.g, value.b, value.a);
        }

        private static void AppendLine(StringBuilder builder, string line)
        {
            builder.AppendLine(line);
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
