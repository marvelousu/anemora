using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Anemora.FastVS;
using UnityEditor;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dLightCookieFoundationAudit
    {
        private const string ReportRelativeDirectory = "docs/devlog/screenshots/fast_vs_hd2d_library_window_light_cookie_cycle26_20260522";
        private const string ReportFileName = "library_window_light_cookie_cycle26_20260522.md";
        private const string ReportTitle = "Fast VS HD2D Library Window Light Cookie Cycle 26";
        private const string CookieTextureAssetPath = "Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset";

        private static readonly string ReportDirectory = GetAbsoluteProjectPath(ReportRelativeDirectory);
        private static readonly string ReportPath = Path.Combine(ReportDirectory, ReportFileName);

        [MenuItem("Tools/Anemora/Write HD2D Library Window Light Cookie Cycle 26 Report")]
        public static void WriteLibraryWindowLightCookieCycle26ReportBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();

            var report = BuildReport();

            Directory.CreateDirectory(ReportDirectory);
            File.WriteAllText(ReportPath, BuildMarkdown(report), Encoding.UTF8);
            AssetDatabase.Refresh();

            if (report.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D library window light cookie cycle 26 report failed:\n- " + string.Join("\n- ", report.Issues));
            }

            Debug.Log($"HD2D library window light cookie cycle 26 report written: {ReportPath}");
        }

        private static LibraryWindowLightCookieCycle26Report BuildReport()
        {
            var report = new LibraryWindowLightCookieCycle26Report
            {
                BranchName = "work/fast-vs-hd2d-shading-foundation-20260522",
                WorktreePath = @"C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work",
                TextureAssetPath = CookieTextureAssetPath,
                TextureAssetAbsolutePath = GetAbsoluteProjectPath(CookieTextureAssetPath),
                ReportPathAbsolute = ReportPath
            };

            try
            {
                AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch();
            }
            catch (Exception ex)
            {
                report.Issues.Add(ex.Message);
            }

            var lightObject = GameObject.Find("FastVS_HD2D_LibraryWindowLight");
            var light = lightObject != null ? lightObject.GetComponent<Light>() : null;
            if (light == null)
            {
                report.Issues.Add("FastVS_HD2D_LibraryWindowLight is missing from the scene.");
                return report;
            }

            report.LightType = light.type;
            report.LightEnabled = light.enabled;
            report.Range = light.range;
            report.SpotAngle = light.spotAngle;
            report.Intensity = light.intensity;
            report.CookieAssigned = light.cookie != null;

            var cookie = light.cookie as Texture2D;
            if (cookie == null)
            {
                report.Issues.Add("FastVS_HD2D_LibraryWindowLight does not have a readable Texture2D cookie assigned.");
                return report;
            }

            report.CookieAssetPath = AssetDatabase.GetAssetPath(cookie);
            report.TextureWidth = cookie.width;
            report.TextureHeight = cookie.height;
            report.FilterMode = cookie.filterMode;
            report.WrapMode = cookie.wrapMode;

            var centerX = cookie.width / 2;
            var centerY = cookie.height / 2;
            var verticalMullionX = Mathf.Clamp(Mathf.RoundToInt(cookie.width * 0.34f), 0, cookie.width - 1);
            var horizontalMullionY = Mathf.Clamp(Mathf.RoundToInt(cookie.height * 0.34f), 0, cookie.height - 1);

            report.CenterLuminance = GetLuminance(cookie.GetPixel(centerX, centerY));
            report.VerticalMullionLuminance = GetLuminance(cookie.GetPixel(verticalMullionX, centerY));
            report.HorizontalMullionLuminance = GetLuminance(cookie.GetPixel(centerX, horizontalMullionY));
            report.CornerLuminance = GetLuminance(cookie.GetPixel(0, 0));

            var minLuminance = float.MaxValue;
            var maxLuminance = float.MinValue;
            var totalLuminance = 0f;
            foreach (var pixel in cookie.GetPixels())
            {
                var luminance = GetLuminance(pixel);
                minLuminance = Mathf.Min(minLuminance, luminance);
                maxLuminance = Mathf.Max(maxLuminance, luminance);
                totalLuminance += luminance;
            }

            report.MinLuminance = minLuminance;
            report.MaxLuminance = maxLuminance;
            report.AverageLuminance = totalLuminance / (cookie.width * cookie.height);

            var directorObject = GameObject.Find("FastVS_HD2D_LightingDirector");
            var director = directorObject != null ? directorObject.GetComponent<FastVsHouseLightingDirector>() : null;
            if (director == null)
            {
                report.Issues.Add("FastVS_HD2D_LightingDirector is missing from the scene.");
                return report;
            }

            director.ApplyAreaForReview(FastVsHouseArea.Library);
            report.LibraryAreaEnabled = light.enabled;
            report.LibraryAreaCookieAssigned = light.cookie != null;

            if (report.LightType != LightType.Spot)
            {
                report.Issues.Add($"Expected a Spot light, but found {report.LightType}.");
            }

            if (!string.Equals(report.CookieAssetPath, CookieTextureAssetPath, StringComparison.Ordinal))
            {
                report.Issues.Add($"Cookie asset path must be {CookieTextureAssetPath}, but was {report.CookieAssetPath}.");
            }

            if (report.TextureWidth != 128 || report.TextureHeight != 128)
            {
                report.Issues.Add($"Cookie texture must be exactly 128x128, but was {report.TextureWidth}x{report.TextureHeight}.");
            }

            if (report.FilterMode != FilterMode.Bilinear)
            {
                report.Issues.Add($"Cookie filter mode must be Bilinear, but was {report.FilterMode}.");
            }

            if (report.WrapMode != TextureWrapMode.Clamp)
            {
                report.Issues.Add($"Cookie wrap mode must be Clamp, but was {report.WrapMode}.");
            }

            if ((report.CenterLuminance - report.VerticalMullionLuminance) < 0.12f)
            {
                report.Issues.Add($"Center luminance must be at least 0.120 brighter than the vertical mullion, but delta was {(report.CenterLuminance - report.VerticalMullionLuminance):0.000}.");
            }

            if ((report.CenterLuminance - report.HorizontalMullionLuminance) < 0.12f)
            {
                report.Issues.Add($"Center luminance must be at least 0.120 brighter than the horizontal mullion, but delta was {(report.CenterLuminance - report.HorizontalMullionLuminance):0.000}.");
            }

            if (report.CornerLuminance < 0.40f || report.CornerLuminance > 0.95f)
            {
                report.Issues.Add($"Corner luminance must stay between 0.40 and 0.95, but was {report.CornerLuminance:0.000}.");
            }

            if (report.AverageLuminance < 0.58f || report.AverageLuminance > 0.86f)
            {
                report.Issues.Add($"Average luminance must stay between 0.58 and 0.86, but was {report.AverageLuminance:0.000}.");
            }

            if (!report.LibraryAreaEnabled || !report.LibraryAreaCookieAssigned)
            {
                report.Issues.Add("Library area review must keep the window light enabled with its cookie assigned.");
            }

            return report;
        }

        private static string BuildMarkdown(LibraryWindowLightCookieCycle26Report report)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"# {ReportTitle}");
            builder.AppendLine();
            builder.AppendLine($"- Result: {FormatResult(report.Issues.Count == 0)}");
            builder.AppendLine($"- Branch: `{report.BranchName}`");
            builder.AppendLine($"- Worktree: `{report.WorktreePath}`");
            builder.AppendLine($"- Report path: `{report.ReportPathAbsolute}`");
            builder.AppendLine($"- Texture asset: `{report.TextureAssetAbsolutePath}`");
            builder.AppendLine($"- Texture asset path: `{report.TextureAssetPath}`");
            builder.AppendLine($"- Light type: `{report.LightType}`");
            builder.AppendLine($"- Enabled after library apply: `{report.LibraryAreaEnabled}`");
            builder.AppendLine($"- Cookie assigned after library apply: `{report.LibraryAreaCookieAssigned}`");
            builder.AppendLine($"- Spot angle: `{report.SpotAngle.ToString("0.00", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Range: `{report.Range.ToString("0.00", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Intensity: `{report.Intensity.ToString("0.000", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Texture size: `{report.TextureWidth}x{report.TextureHeight}`");
            builder.AppendLine($"- Filter mode: `{report.FilterMode}`");
            builder.AppendLine($"- Wrap mode: `{report.WrapMode}`");
            builder.AppendLine($"- Average luminance: `{report.AverageLuminance.ToString("0.000", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Minimum luminance: `{report.MinLuminance.ToString("0.000", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Maximum luminance: `{report.MaxLuminance.ToString("0.000", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Center luminance: `{report.CenterLuminance.ToString("0.000", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Vertical mullion luminance: `{report.VerticalMullionLuminance.ToString("0.000", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Horizontal mullion luminance: `{report.HorizontalMullionLuminance.ToString("0.000", CultureInfo.InvariantCulture)}`");
            builder.AppendLine($"- Corner luminance: `{report.CornerLuminance.ToString("0.000", CultureInfo.InvariantCulture)}`");
            builder.AppendLine();
            builder.AppendLine("## Validation Notes");
            builder.AppendLine();
            if (report.Issues.Count == 0)
            {
                builder.AppendLine("- PASS: the cookie stays shaped, soft, and readable without changing map, story, or gameplay content.");
            }
            else
            {
                foreach (var issue in report.Issues)
                {
                    builder.AppendLine($"- FAIL: {issue}");
                }
            }

            return builder.ToString();
        }

        private static float GetLuminance(Color color)
        {
            return (0.2126f * color.r) + (0.7152f * color.g) + (0.0722f * color.b);
        }

        private static string FormatResult(bool passed)
        {
            return passed ? "PASS" : "FAIL";
        }

        private static string GetAbsoluteProjectPath(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", relativePath));
        }

        private sealed class LibraryWindowLightCookieCycle26Report
        {
            public string BranchName;
            public string WorktreePath;
            public string ReportPathAbsolute;
            public string TextureAssetPath;
            public string TextureAssetAbsolutePath;
            public string CookieAssetPath;
            public LightType LightType;
            public bool LightEnabled;
            public bool LibraryAreaEnabled;
            public bool CookieAssigned;
            public bool LibraryAreaCookieAssigned;
            public int TextureWidth;
            public int TextureHeight;
            public FilterMode FilterMode;
            public TextureWrapMode WrapMode;
            public float SpotAngle;
            public float Range;
            public float Intensity;
            public float AverageLuminance;
            public float MinLuminance;
            public float MaxLuminance;
            public float CenterLuminance;
            public float VerticalMullionLuminance;
            public float HorizontalMullionLuminance;
            public float CornerLuminance;
            public readonly List<string> Issues = new List<string>();
        }
    }
}
