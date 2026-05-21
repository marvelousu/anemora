using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Anemora.FastVS;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dSurfaceTextureMetricAudit
    {
        private const int SampleStep = 8;
        private const int DistinctBucketCount = 16;
        private const float AverageLuminanceMin = 0.04f;
        private const float AverageLuminanceMax = 0.92f;
        private const float LuminanceRangeMin = 0.015f;
        private const float WallColorFallbackLuminanceRangeMin = 0.005f;
        private const float BandLowerPadding = 0.12f;
        private const float BandUpperPadding = 0.18f;
        private static readonly string OutputDirectory = Path.GetFullPath(Path.Combine(
            Application.dataPath,
            "..",
            "docs",
            "devlog",
            "screenshots",
            "fast_vs_hd2d_surface_texture_metrics_cycle16_20260522"));
        private const string MetricsFileName = "surface_texture_metrics_cycle16_20260522.md";

        [MenuItem("Tools/Anemora/Verify HD2D Surface Texture Metrics V1")]
        public static void VerifySurfaceTextureMetricsV1()
        {
            var batch = RunSurfaceTextureMetricAudit(writeReport: false);
            if (batch.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D surface texture metric audit failed:\n- " + string.Join("\n- ", batch.Issues));
            }

            Debug.Log("HD2D surface texture metric audit passed");
        }

        [MenuItem("Tools/Anemora/Write HD2D Surface Texture Metrics V1")]
        public static void WriteSurfaceTextureMetricsV1Batch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();
            var batch = RunSurfaceTextureMetricAudit(writeReport: true);
            if (batch.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D surface texture metric audit failed:\n- " + string.Join("\n- ", batch.Issues));
            }

            Debug.Log($"HD2D surface texture metric report written: {Path.Combine(OutputDirectory, MetricsFileName)}");
        }

        private static SurfaceTextureMetricBatch RunSurfaceTextureMetricAudit(bool writeReport)
        {
            EnsureHouseSliceSceneLoadedForReview();

            var batch = BuildSurfaceTextureMetricBatch();
            if (writeReport)
            {
                Directory.CreateDirectory(OutputDirectory);
                var metricsPath = Path.Combine(OutputDirectory, MetricsFileName);
                File.WriteAllText(metricsPath, BuildMetricsMarkdown(batch, metricsPath), Encoding.UTF8);
                AssetDatabase.Refresh();
            }

            return batch;
        }

        private static void EnsureHouseSliceSceneLoadedForReview()
        {
            var scene = EditorSceneManager.GetActiveScene();
            if (!scene.IsValid() || !string.Equals(scene.path, AnemoraFastVsHouseSliceSetup.ScenePath, StringComparison.Ordinal))
            {
                EditorSceneManager.OpenScene(AnemoraFastVsHouseSliceSetup.ScenePath, OpenSceneMode.Single);
            }
        }

        private static SurfaceTextureMetricBatch BuildSurfaceTextureMetricBatch()
        {
            var batch = new SurfaceTextureMetricBatch();
            var profiles = Resources.FindObjectsOfTypeAll<FastVsHd2dSurfaceProfile>();
            var cache = new Dictionary<int, SurfaceTextureMetricMeasurement>();

            foreach (var profile in profiles)
            {
                if (profile == null || profile.gameObject == null)
                {
                    continue;
                }

                var scene = profile.gameObject.scene;
                if (!scene.IsValid() || !string.Equals(scene.path, AnemoraFastVsHouseSliceSetup.ScenePath, StringComparison.Ordinal))
                {
                    continue;
                }

                var row = BuildSurfaceTextureMetricRow(profile, cache, batch.Issues);
                batch.Rows.Add(row);
            }

            batch.Rows.Sort((left, right) => string.Compare(left.SurfaceId, right.SurfaceId, StringComparison.Ordinal));

            if (batch.Rows.Count == 0)
            {
                batch.Issues.Add("No HD2D surface profiles were found in the house slice scene.");
            }

            return batch;
        }

        private static SurfaceTextureMetricRow BuildSurfaceTextureMetricRow(
            FastVsHd2dSurfaceProfile profile,
            Dictionary<int, SurfaceTextureMetricMeasurement> cache,
            List<string> issues)
        {
            var gameObject = profile.gameObject;
            var renderer = gameObject.GetComponent<MeshRenderer>();
            var row = new SurfaceTextureMetricRow
            {
                SurfaceId = profile.SurfaceIdForReview ?? string.Empty,
                GameObjectName = gameObject.name,
                Area = profile.AreaIdForReview.ToString(),
                Kind = profile.SurfaceKindForReview.ToString(),
                Current = profile.IsCurrentWorldForReview
            };

            if (renderer == null || renderer.sharedMaterial == null)
            {
                issues.Add($"Surface {row.SurfaceId} on {row.GameObjectName} is missing a MeshRenderer shared material.");
                row.Material = "(missing material)";
                row.Texture = "(missing material)";
                row.Result = "FAIL: missing MeshRenderer shared material";
                return row;
            }

            var material = renderer.sharedMaterial;
            var measurement = GetOrCreateMeasurement(material, cache);
            row.Material = material.name ?? string.Empty;
            row.Texture = measurement.TextureLabel;
            row.AverageLuminance = measurement.AverageLuminance;
            row.LuminanceRange = measurement.LuminanceRange;
            row.LocalContrast = measurement.LocalContrast;
            row.DistinctBuckets = measurement.DistinctBuckets;

            var rowIssues = ValidateMeasurement(profile, measurement);
            if (rowIssues.Count == 0)
            {
                row.Result = measurement.UsedColorFallback
                    ? "PASS (color fallback)"
                    : "PASS";
            }
            else
            {
                issues.AddRange(PrefixIssues(profile, rowIssues));
                row.Result = "FAIL: " + string.Join("; ", rowIssues);
            }

            if (measurement.UsedColorFallback && rowIssues.Count == 0)
            {
                row.Result = measurement.TextureUnreadable
                    ? "PASS (texture unreadable, color fallback)"
                    : "PASS (color fallback)";
            }

            if (measurement.UsedColorFallback && measurement.DistinctBuckets == 1)
            {
                row.Result += " [1 bucket]";
            }

            return row;
        }

        private static List<string> ValidateMeasurement(FastVsHd2dSurfaceProfile profile, SurfaceTextureMetricMeasurement measurement)
        {
            var issues = new List<string>();
            var surfaceId = profile.SurfaceIdForReview ?? string.Empty;
            var gameObjectName = profile.gameObject != null ? profile.gameObject.name : "(missing gameObject)";

            if (!measurement.HasMaterialOrColor)
            {
                issues.Add($"{surfaceId} on {gameObjectName} has no readable material texture or color fallback.");
                return issues;
            }

            if (measurement.AverageLuminance < AverageLuminanceMin || measurement.AverageLuminance > AverageLuminanceMax)
            {
                issues.Add($"{surfaceId} on {gameObjectName} average luminance {measurement.AverageLuminance:0.000} is outside {AverageLuminanceMin:0.000}-{AverageLuminanceMax:0.000}.");
            }

            var rangeMin = profile.SurfaceKindForReview == FastVsHd2dSurfaceKind.Wall && measurement.UsedColorFallback
                ? WallColorFallbackLuminanceRangeMin
                : LuminanceRangeMin;
            if (measurement.LuminanceRange < rangeMin)
            {
                issues.Add($"{surfaceId} on {gameObjectName} luminance range {measurement.LuminanceRange:0.000} is below {rangeMin:0.000}.");
            }

            if (measurement.DistinctBuckets < 3 && !measurement.UsedColorFallback)
            {
                issues.Add($"{surfaceId} on {gameObjectName} distinct luminance buckets {measurement.DistinctBuckets} is below 3.");
            }

            var expandedBand = GetExpandedBand(profile.TargetLuminanceBandForReview);
            if (measurement.AverageLuminance < expandedBand.x || measurement.AverageLuminance > expandedBand.y)
            {
                issues.Add($"{surfaceId} on {gameObjectName} average luminance {measurement.AverageLuminance:0.000} is outside review band {expandedBand.x:0.000}-{expandedBand.y:0.000}.");
            }

            return issues;
        }

        private static SurfaceTextureMetricMeasurement GetOrCreateMeasurement(Material material, Dictionary<int, SurfaceTextureMetricMeasurement> cache)
        {
            var materialKey = material.GetInstanceID();
            if (cache.TryGetValue(materialKey, out var cached))
            {
                return cached;
            }

            var measurement = ComputeMeasurement(material);
            cache[materialKey] = measurement;
            return measurement;
        }

        private static SurfaceTextureMetricMeasurement ComputeMeasurement(Material material)
        {
            var measurement = new SurfaceTextureMetricMeasurement
            {
                MaterialLabel = material.name ?? string.Empty
            };

            var texture = ResolvePrimaryTexture(material);
            if (texture != null)
            {
                measurement.TextureLabel = DescribeTexture(texture);
                if (texture is Texture2D texture2D)
                {
                    try
                    {
                        if (texture2D.isReadable)
                        {
                            ComputeTextureMetrics(texture2D, measurement);
                            measurement.UsedTexture = true;
                            return measurement;
                        }

                        measurement.TextureUnreadable = true;
                    }
                    catch (Exception)
                    {
                        measurement.TextureUnreadable = true;
                    }
                }
                else
                {
                    measurement.TextureUnreadable = true;
                }
            }

            if (TryResolveMaterialColor(material, out var color, out var colorSource))
            {
                measurement.UsedColorFallback = true;
                measurement.TextureLabel = texture == null
                    ? $"color fallback ({colorSource})"
                    : $"{DescribeTexture(texture)} (color fallback)";
                measurement.HasMaterialOrColor = true;
                measurement.AverageLuminance = GetLuminance(color);
                measurement.LuminanceMin = measurement.AverageLuminance;
                measurement.LuminanceMax = measurement.AverageLuminance;
                measurement.LuminanceRange = 0f;
                measurement.LocalContrast = 0f;
                measurement.DistinctBuckets = 1;
                return measurement;
            }

            if (texture != null && measurement.TextureUnreadable)
            {
                measurement.TextureLabel = DescribeTexture(texture) + " (unreadable)";
            }

            measurement.HasMaterialOrColor = false;
            if (string.IsNullOrEmpty(measurement.TextureLabel))
            {
                measurement.TextureLabel = "(missing texture)";
            }

            return measurement;
        }

        private static void ComputeTextureMetrics(Texture2D texture, SurfaceTextureMetricMeasurement measurement)
        {
            var pixels = texture.GetPixels32();
            var sampleXs = BuildSampleIndices(texture.width);
            var sampleYs = BuildSampleIndices(texture.height);
            var sampleCount = sampleXs.Length * sampleYs.Length;
            var luminances = new float[sampleCount];
            var distinctBuckets = new HashSet<int>();

            var index = 0;
            for (var yIndex = 0; yIndex < sampleYs.Length; yIndex++)
            {
                var y = sampleYs[yIndex];
                var rowOffset = y * texture.width;
                for (var xIndex = 0; xIndex < sampleXs.Length; xIndex++)
                {
                    var x = sampleXs[xIndex];
                    var luminance = GetLuminance(pixels[rowOffset + x]);
                    luminances[index++] = luminance;
                    measurement.AverageLuminance += luminance;
                    measurement.LuminanceMin = Mathf.Min(measurement.LuminanceMin, luminance);
                    measurement.LuminanceMax = Mathf.Max(measurement.LuminanceMax, luminance);
                    distinctBuckets.Add(GetLuminanceBucket(luminance));
                }
            }

            if (sampleCount > 0)
            {
                measurement.AverageLuminance /= sampleCount;
            }

            if (float.IsPositiveInfinity(measurement.LuminanceMin))
            {
                measurement.LuminanceMin = 0f;
            }

            if (float.IsNegativeInfinity(measurement.LuminanceMax))
            {
                measurement.LuminanceMax = 0f;
            }

            measurement.LuminanceRange = measurement.LuminanceMax - measurement.LuminanceMin;
            measurement.LocalContrast = ComputeAverageAbsoluteNeighborDelta(luminances, sampleXs.Length, sampleYs.Length);
            measurement.DistinctBuckets = distinctBuckets.Count;
            measurement.HasMaterialOrColor = true;
        }

        private static float ComputeAverageAbsoluteNeighborDelta(float[] luminances, int width, int height)
        {
            var totalDifference = 0f;
            var sampleCount = 0;

            for (var y = 0; y < height; y++)
            {
                var rowOffset = y * width;
                for (var x = 0; x < width; x++)
                {
                    var index = rowOffset + x;
                    var luminance = luminances[index];

                    if (x + 1 < width)
                    {
                        totalDifference += Mathf.Abs(luminance - luminances[index + 1]);
                        sampleCount++;
                    }

                    if (y + 1 < height)
                    {
                        totalDifference += Mathf.Abs(luminance - luminances[index + width]);
                        sampleCount++;
                    }
                }
            }

            return sampleCount > 0 ? totalDifference / sampleCount : 0f;
        }

        private static int[] BuildSampleIndices(int length)
        {
            var values = new List<int>();
            for (var index = 0; index < length; index += SampleStep)
            {
                values.Add(index);
            }

            var lastIndex = length - 1;
            if (values.Count == 0)
            {
                values.Add(0);
            }
            else if (values[values.Count - 1] != lastIndex)
            {
                values.Add(lastIndex);
            }

            return values.ToArray();
        }

        private static Vector2 GetExpandedBand(Vector2 band)
        {
            var lower = Mathf.Clamp01(band.x - BandLowerPadding);
            var upper = Mathf.Clamp01(band.y + BandUpperPadding);
            return new Vector2(lower, upper);
        }

        private static string DescribeTexture(Texture texture)
        {
            if (texture == null)
            {
                return "(missing texture)";
            }

            var assetPath = AssetDatabase.GetAssetPath(texture);
            if (!string.IsNullOrWhiteSpace(assetPath))
            {
                return assetPath;
            }

            return texture.name ?? string.Empty;
        }

        private static bool TryResolveMaterialColor(Material material, out Color color, out string colorSource)
        {
            if (material.HasProperty("_BaseColor"))
            {
                color = material.GetColor("_BaseColor");
                colorSource = "_BaseColor";
                return true;
            }

            if (material.HasProperty("_Color"))
            {
                color = material.GetColor("_Color");
                colorSource = "_Color";
                return true;
            }

            try
            {
                color = material.color;
                colorSource = "color";
                return true;
            }
            catch
            {
                color = default;
                colorSource = string.Empty;
                return false;
            }
        }

        private static Texture ResolvePrimaryTexture(Material material)
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

            if (material.HasProperty("_MainTex"))
            {
                var texture = material.GetTexture("_MainTex");
                if (texture != null)
                {
                    return texture;
                }
            }

            return material.mainTexture;
        }

        private static int GetLuminanceBucket(float luminance)
        {
            return Mathf.Clamp(Mathf.FloorToInt(luminance * DistinctBucketCount), 0, DistinctBucketCount - 1);
        }

        private static float GetLuminance(Color color)
        {
            return (color.r * 0.2126f) + (color.g * 0.7152f) + (color.b * 0.0722f);
        }

        private static float GetLuminance(Color32 color)
        {
            return ((color.r / 255f) * 0.2126f) + ((color.g / 255f) * 0.7152f) + ((color.b / 255f) * 0.0722f);
        }

        private static string[] PrefixIssues(FastVsHd2dSurfaceProfile profile, List<string> rowIssues)
        {
            var result = new string[rowIssues.Count];
            for (var i = 0; i < rowIssues.Count; i++)
            {
                result[i] = $"{profile.SurfaceIdForReview ?? string.Empty} on {profile.gameObject.name}: {rowIssues[i]}";
            }

            return result;
        }

        private static string BuildMetricsMarkdown(SurfaceTextureMetricBatch batch, string metricsPath)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Fast VS HD2D Surface Texture Metric Audit Cycle 16");
            builder.AppendLine();
            builder.AppendLine("This is an audit and reporting foundation for major HD-2D surface texture readability. It does not change shading or lighting.");
            builder.AppendLine();
            builder.AppendLine($"- Metrics file: `{metricsPath}`");
            builder.AppendLine($"- Output directory: `{OutputDirectory}`");
            builder.AppendLine($"- Result: {(batch.Issues.Count == 0 ? "Pass" : "Fail")}");
            builder.AppendLine();
            builder.AppendLine("| SurfaceId | GameObject | Area | Kind | Current | Material | Texture | AvgLum | Range | LocalContrast | DistinctBuckets | Result |");
            builder.AppendLine("|---|---|---|---|---|---|---|---:|---:|---:|---:|---|");

            foreach (var row in batch.Rows)
            {
                builder.Append("| ");
                builder.Append(EscapeTableCell(row.SurfaceId));
                builder.Append(" | ");
                builder.Append(EscapeTableCell(row.GameObjectName));
                builder.Append(" | ");
                builder.Append(EscapeTableCell(row.Area));
                builder.Append(" | ");
                builder.Append(EscapeTableCell(row.Kind));
                builder.Append(" | ");
                builder.Append(row.Current ? "True" : "False");
                builder.Append(" | ");
                builder.Append(EscapeTableCell(row.Material));
                builder.Append(" | ");
                builder.Append(EscapeTableCell(row.Texture));
                builder.Append(" | ");
                builder.Append(row.AverageLuminance.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(row.LuminanceRange.ToString("0.000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(row.LocalContrast.ToString("0.0000", CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(row.DistinctBuckets.ToString(CultureInfo.InvariantCulture));
                builder.Append(" | ");
                builder.Append(EscapeTableCell(row.Result));
                builder.AppendLine(" |");
            }

            if (batch.Issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                foreach (var issue in batch.Issues)
                {
                    builder.AppendLine($"- {issue}");
                }
            }

            return builder.ToString();
        }

        private static string EscapeTableCell(string value)
        {
            return (value ?? string.Empty).Replace("|", "\\|");
        }

        private sealed class SurfaceTextureMetricBatch
        {
            public SurfaceTextureMetricBatch()
            {
                Rows = new List<SurfaceTextureMetricRow>();
                Issues = new List<string>();
            }

            public List<SurfaceTextureMetricRow> Rows { get; }
            public List<string> Issues { get; }
        }

        private sealed class SurfaceTextureMetricRow
        {
            public string SurfaceId;
            public string GameObjectName;
            public string Area;
            public string Kind;
            public bool Current;
            public string Material;
            public string Texture;
            public float AverageLuminance;
            public float LuminanceRange;
            public float LocalContrast;
            public int DistinctBuckets;
            public string Result;
        }

        private sealed class SurfaceTextureMetricMeasurement
        {
            public string MaterialLabel;
            public string TextureLabel;
            public float AverageLuminance;
            public float LuminanceMin = float.PositiveInfinity;
            public float LuminanceMax = float.NegativeInfinity;
            public float LuminanceRange;
            public float LocalContrast;
            public int DistinctBuckets;
            public bool HasMaterialOrColor;
            public bool UsedTexture;
            public bool UsedColorFallback;
            public bool TextureUnreadable;
        }
    }
}
