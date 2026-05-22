using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dVisualSnapshotAudit
    {
        private static readonly string OutputDirectory = Path.GetFullPath(Path.Combine(
            Application.dataPath,
            "..",
            "docs",
            "devlog",
            "screenshots",
            "fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522"));
        private const string MetricsFileName = "visual_snapshot_metrics_cycle10_20260522.md";
        private const int CaptureWidth = 1280;
        private const int CaptureHeight = 720;
        private const float AverageLuminanceMin = 0.06f;
        private const float AverageLuminanceMax = 0.88f;
        private const float LuminanceRangeMin = 0.12f;
        private const float LocalContrastMin = 0.015f;
        private const float DarkPixelRatioMax = 0.60f;
        private const float BrightPixelRatioMax = 0.70f;
        private const long MinimumPngBytes = 5000;
        private static readonly Vector3 HouseInteriorCenter = new Vector3(-8.35f, 0f, -8.35f);
        private static readonly Vector3 HouseInteriorOverviewLocal = HouseInteriorCenter + new Vector3(-0.36f, 0.02f, 0.12f);
        private static readonly Vector3 HouseExteriorCenter = new Vector3(8.20f, 0f, 8.20f);
        private static readonly Vector3 CentralPlazaVsCenter = new Vector3(20.80f, 0f, 15.80f);
        private static readonly Vector3 LibraryVsCenter = new Vector3(31.00f, 0f, 20.00f);
        private static readonly Vector3 CameraOffset = new Vector3(0f, 2.75f, -4.55f);
        private static readonly Vector3 LookOffset = new Vector3(0f, 0.72f, 0.45f);
        private static readonly Vector3 HouseExteriorSnapshotCameraOffset = new Vector3(0f, 3.35f, -6.35f);
        private static readonly Vector3 HouseExteriorSnapshotLookOffset = new Vector3(0f, 0.88f, 0.95f);
        private static readonly Vector3 CentralPlazaSnapshotCameraOffset = new Vector3(0f, 3.95f, -7.40f);
        private static readonly Vector3 CentralPlazaSnapshotLookOffset = new Vector3(0f, 0.95f, 2.20f);
        private static readonly Vector3 HouseInteriorOverviewCameraOffset = new Vector3(0f, 4.25f, -7.20f);
        private static readonly Vector3 HouseInteriorOverviewLookOffset = new Vector3(0f, 1.10f, 0.90f);
        private const float HouseInteriorSnapshotTargetFov = 14f;
        private const float HouseExteriorSnapshotTargetFov = 36f;
        private const float CentralPlazaSnapshotTargetFov = 36f;
        private const float LibrarySnapshotTargetFov = 38f;

        [MenuItem("Tools/Anemora/Capture Fast VS HD2D Visual Snapshot Audit")]
        public static void CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch()
        {
            RunCaptureAndVerifyBatch();
        }

        private static void RunCaptureAndVerifyBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();
            EditorSceneManager.OpenScene(AnemoraFastVsHouseSliceSetup.ScenePath, OpenSceneMode.Single);
            Directory.CreateDirectory(OutputDirectory);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS HD2D visual snapshot audit failed: required review components are missing.");
            }

            if (controller.CurrentSpaceRootForReview == null)
            {
                throw new InvalidOperationException("Fast VS HD2D visual snapshot audit failed: current-space root is missing.");
            }

            var shots = new[]
            {
                new VisualSnapshotShot("house_interior", FastVsHouseArea.Interior, HouseInteriorOverviewLocal, HouseInteriorOverviewLocal, HouseInteriorOverviewCameraOffset, HouseInteriorOverviewLookOffset, HouseInteriorSnapshotTargetFov, "01_current_house_interior_visual_snapshot.png"),
                new VisualSnapshotShot("house_exterior", FastVsHouseArea.Exterior, HouseExteriorCenter, HouseExteriorCenter, HouseExteriorSnapshotCameraOffset, HouseExteriorSnapshotLookOffset, HouseExteriorSnapshotTargetFov, "02_current_house_exterior_visual_snapshot.png"),
                new VisualSnapshotShot("central_plaza", FastVsHouseArea.CentralPlaza, CentralPlazaVsCenter, CentralPlazaVsCenter, CentralPlazaSnapshotCameraOffset, CentralPlazaSnapshotLookOffset, CentralPlazaSnapshotTargetFov, "03_current_central_plaza_visual_snapshot.png"),
                new VisualSnapshotShot("library", FastVsHouseArea.Library, LibraryVsCenter, LibraryVsCenter, CameraOffset, LookOffset, LibrarySnapshotTargetFov, "04_current_library_visual_snapshot.png")
            };

            var results = new List<VisualSnapshotResult>(shots.Length);
            var issues = new List<string>();

            foreach (var shot in shots)
            {
                try
                {
                    var result = CaptureShot(controller, visibility, guide, camera, shot);
                    results.Add(result);
                    issues.AddRange(ValidateShot(result));
                }
                catch (Exception ex)
                {
                    issues.Add($"{shot.Label}: {ex.Message}");
                }
            }

            var metricsPath = Path.Combine(OutputDirectory, MetricsFileName);
            File.WriteAllText(metricsPath, BuildMetricsMarkdown(results, issues, metricsPath), Encoding.UTF8);

            AssetDatabase.Refresh();

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("Fast VS HD2D visual snapshot audit failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log($"Fast VS HD2D visual snapshot audit passed: {Path.GetFullPath(OutputDirectory)}");
        }

        private static VisualSnapshotResult CaptureShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            VisualSnapshotShot shot)
        {
            visibility.SetActiveAreaForReview(shot.Area);
            controller.ForcePlayerCurrentLocalForReview(shot.PlayerLocal);
            guide.ApplyActiveTimeIsolationForReview();

            var anchor = controller.CurrentSpaceRootForReview.TransformPoint(shot.CameraAnchorLocal);
            var position = anchor + shot.CameraOffset;
            var lookAt = anchor + shot.LookOffset;
            var previousFieldOfView = camera.fieldOfView;
            camera.fieldOfView = shot.TargetFov;

            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));

            var outputPath = Path.Combine(OutputDirectory, shot.FileName);
            var previousTarget = camera.targetTexture;
            var previousActive = RenderTexture.active;
            var renderTexture = new RenderTexture(CaptureWidth, CaptureHeight, 24, RenderTextureFormat.ARGB32);
            var texture = new Texture2D(CaptureWidth, CaptureHeight, TextureFormat.RGBA32, false);

            try
            {
                camera.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;
                Canvas.ForceUpdateCanvases();
                camera.Render();
                texture.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply(false, false);

                var stats = ComputeStats(texture);
                var pngBytes = texture.EncodeToPNG();
                File.WriteAllBytes(outputPath, pngBytes);
                var fileInfo = new FileInfo(outputPath);

                return new VisualSnapshotResult(
                    shot,
                    outputPath,
                    CaptureWidth,
                    CaptureHeight,
                    fileInfo.Exists ? fileInfo.Length : 0L,
                    stats.AverageLuminance,
                    stats.LuminanceMin,
                    stats.LuminanceMax,
                    stats.LuminanceRange,
                    stats.DarkPixelRatio,
                    stats.BrightPixelRatio,
                    stats.LocalContrast);
            }
            finally
            {
                camera.fieldOfView = previousFieldOfView;
                camera.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                renderTexture.Release();
                UnityEngine.Object.DestroyImmediate(renderTexture);
                UnityEngine.Object.DestroyImmediate(texture);
            }
        }

        private static List<string> ValidateShot(VisualSnapshotResult result)
        {
            var issues = new List<string>();

            if (result.Width != CaptureWidth || result.Height != CaptureHeight)
            {
                issues.Add($"{result.Shot.Label}: unexpected dimensions {result.Width}x{result.Height}.");
            }

            if (result.AverageLuminance < AverageLuminanceMin || result.AverageLuminance > AverageLuminanceMax)
            {
                issues.Add($"{result.Shot.Label}: average luminance out of band ({result.AverageLuminance:0.000}).");
            }

            if (result.LuminanceRange < LuminanceRangeMin)
            {
                issues.Add($"{result.Shot.Label}: luminance range too small ({result.LuminanceRange:0.000}).");
            }

            if (result.LocalContrast < LocalContrastMin)
            {
                issues.Add($"{result.Shot.Label}: local contrast too low ({result.LocalContrast:0.0000}).");
            }

            if (result.DarkPixelRatio > DarkPixelRatioMax)
            {
                issues.Add($"{result.Shot.Label}: dark pixel ratio too high ({result.DarkPixelRatio:0.000}).");
            }

            if (result.BrightPixelRatio > BrightPixelRatioMax)
            {
                issues.Add($"{result.Shot.Label}: bright pixel ratio too high ({result.BrightPixelRatio:0.000}).");
            }

            if (!File.Exists(result.FullPath))
            {
                issues.Add($"{result.Shot.Label}: file was not written ({result.FullPath}).");
            }
            else if (result.FileLengthBytes <= MinimumPngBytes)
            {
                issues.Add($"{result.Shot.Label}: file is unexpectedly small ({result.FileLengthBytes} bytes).");
            }

            return issues;
        }

        private static SnapshotStats ComputeStats(Texture2D texture)
        {
            var stats = new SnapshotStats
            {
                LuminanceMin = float.PositiveInfinity,
                LuminanceMax = float.NegativeInfinity
            };

            var width = texture.width;
            var height = texture.height;
            var pixels = texture.GetPixels32();
            var pixelCount = pixels.Length;
            var luminanceValues = new float[pixelCount];

            for (var i = 0; i < pixelCount; i++)
            {
                var luminance = GetLuminance(pixels[i]);
                luminanceValues[i] = luminance;
                stats.AverageLuminance += luminance;
                stats.LuminanceMin = Mathf.Min(stats.LuminanceMin, luminance);
                stats.LuminanceMax = Mathf.Max(stats.LuminanceMax, luminance);

                if (luminance < 0.04f)
                {
                    stats.DarkPixelCount++;
                }

                if (luminance > 0.96f)
                {
                    stats.BrightPixelCount++;
                }
            }

            if (pixelCount > 0)
            {
                stats.AverageLuminance /= pixelCount;
                stats.DarkPixelRatio = stats.DarkPixelCount / (float)pixelCount;
                stats.BrightPixelRatio = stats.BrightPixelCount / (float)pixelCount;
            }

            if (float.IsPositiveInfinity(stats.LuminanceMin))
            {
                stats.LuminanceMin = 0f;
            }

            if (float.IsNegativeInfinity(stats.LuminanceMax))
            {
                stats.LuminanceMax = 0f;
            }

            stats.LuminanceRange = stats.LuminanceMax - stats.LuminanceMin;
            stats.LocalContrast = ComputeLocalContrast(luminanceValues, width, height);
            return stats;
        }

        private static float ComputeLocalContrast(float[] luminanceValues, int width, int height)
        {
            var totalDifference = 0f;
            var sampleCount = 0;

            for (var y = 0; y < height; y += 4)
            {
                var row = y * width;
                for (var x = 0; x < width; x += 4)
                {
                    var index = row + x;
                    var luminance = luminanceValues[index];

                    if (x + 4 < width)
                    {
                        totalDifference += Mathf.Abs(luminance - luminanceValues[index + 4]);
                        sampleCount++;
                    }

                    if (y + 4 < height)
                    {
                        totalDifference += Mathf.Abs(luminance - luminanceValues[index + (4 * width)]);
                        sampleCount++;
                    }
                }
            }

            return sampleCount > 0 ? totalDifference / sampleCount : 0f;
        }

        private static string BuildMetricsMarkdown(
            IReadOnlyList<VisualSnapshotResult> results,
            IReadOnlyCollection<string> issues,
            string metricsPath)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Fast VS HD2D Visual Snapshot Audit Cycle 10");
            builder.AppendLine();
            builder.AppendLine("This captures representative current-world views for a repeatable visual gate. It is validation and evidence capture, not final art polish.");
            builder.AppendLine();
            builder.AppendLine($"- Metrics file: `{metricsPath}`");
            builder.AppendLine($"- Output directory: `{OutputDirectory}`");
            builder.AppendLine($"- Result: {(issues.Count == 0 ? "Pass" : "Fail")}");
            builder.AppendLine();
            builder.AppendLine("| Shot | PNG | Width | Height | Avg Lum | Min Lum | Max Lum | Range | Dark Ratio | Bright Ratio | Local Contrast | Bytes |");
            builder.AppendLine("|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|");

            for (var i = 0; i < results.Count; i++)
            {
                var result = results[i];
                builder.Append("| ");
                builder.Append(result.Shot.Label);
                builder.Append(" | ");
                builder.Append(result.FullPath.Replace("|", "\\|"));
                builder.Append(" | ");
                builder.Append(result.Width.ToString(CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.Height.ToString(CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.AverageLuminance.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.LuminanceMin.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.LuminanceMax.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.LuminanceRange.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.DarkPixelRatio.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.BrightPixelRatio.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.LocalContrast.ToString("0.0000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(result.FileLengthBytes.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(" |");
            }

            if (issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                foreach (var issue in issues)
                {
                    builder.AppendLine($"- {issue}");
                }
            }

            return builder.ToString();
        }

        private static float GetLuminance(Color32 pixel)
        {
            return ((pixel.r / 255f) * 0.2126f) + ((pixel.g / 255f) * 0.7152f) + ((pixel.b / 255f) * 0.0722f);
        }

        private readonly struct VisualSnapshotShot
        {
            public readonly string Label;
            public readonly FastVsHouseArea Area;
            public readonly Vector3 PlayerLocal;
            public readonly Vector3 CameraAnchorLocal;
            public readonly Vector3 CameraOffset;
            public readonly Vector3 LookOffset;
            public readonly float TargetFov;
            public readonly string FileName;

            public VisualSnapshotShot(
                string label,
                FastVsHouseArea area,
                Vector3 playerLocal,
                Vector3 cameraAnchorLocal,
                Vector3 cameraOffset,
                Vector3 lookOffset,
                float targetFov,
                string fileName)
            {
                Label = label;
                Area = area;
                PlayerLocal = playerLocal;
                CameraAnchorLocal = cameraAnchorLocal;
                CameraOffset = cameraOffset;
                LookOffset = lookOffset;
                TargetFov = targetFov;
                FileName = fileName;
            }
        }

        private sealed class VisualSnapshotResult
        {
            public readonly VisualSnapshotShot Shot;
            public readonly string FullPath;
            public readonly int Width;
            public readonly int Height;
            public readonly long FileLengthBytes;
            public readonly float AverageLuminance;
            public readonly float LuminanceMin;
            public readonly float LuminanceMax;
            public readonly float LuminanceRange;
            public readonly float DarkPixelRatio;
            public readonly float BrightPixelRatio;
            public readonly float LocalContrast;

            public VisualSnapshotResult(
                VisualSnapshotShot shot,
                string fullPath,
                int width,
                int height,
                long fileLengthBytes,
                float averageLuminance,
                float luminanceMin,
                float luminanceMax,
                float luminanceRange,
                float darkPixelRatio,
                float brightPixelRatio,
                float localContrast)
            {
                Shot = shot;
                FullPath = fullPath;
                Width = width;
                Height = height;
                FileLengthBytes = fileLengthBytes;
                AverageLuminance = averageLuminance;
                LuminanceMin = luminanceMin;
                LuminanceMax = luminanceMax;
                LuminanceRange = luminanceRange;
                DarkPixelRatio = darkPixelRatio;
                BrightPixelRatio = brightPixelRatio;
                LocalContrast = localContrast;
            }
        }

        private struct SnapshotStats
        {
            public float AverageLuminance;
            public float LuminanceMin;
            public float LuminanceMax;
            public float LuminanceRange;
            public float DarkPixelRatio;
            public float BrightPixelRatio;
            public float LocalContrast;
            public int DarkPixelCount;
            public int BrightPixelCount;
        }
    }
}
