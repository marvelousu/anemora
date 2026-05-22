using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dOutdoorScenicBackdropFoundationAudit
    {
        private const string BranchName = "work/fast-vs-hd2d-shading-foundation-20260522";
        private const string WorktreePath = @"C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work";
        private const string ReportRelativeDirectory = "docs/devlog/screenshots/fast_vs_hd2d_outdoor_scenic_backdrop_foundation_cycle29_20260522";
        private const string ReportFileName = "outdoor_scenic_backdrop_foundation_cycle29_20260522.md";
        private const string ReportTitle = "Fast VS HD2D Outdoor Scenic Backdrop Foundation Cycle 29";

        private static readonly string ReportDirectory = GetAbsoluteProjectPath(ReportRelativeDirectory);
        private static readonly string ReportPath = Path.Combine(ReportDirectory, ReportFileName);

        [MenuItem("Tools/Anemora/Write HD2D Outdoor Scenic Backdrop Foundation Cycle 29 Report")]
        public static void WriteOutdoorScenicBackdropFoundationCycle29ReportBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();
            var report = BuildReport();

            Directory.CreateDirectory(ReportDirectory);
            File.WriteAllText(ReportPath, BuildMarkdown(report), Encoding.UTF8);
            AssetDatabase.Refresh();

            if (report.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D outdoor scenic backdrop foundation cycle 29 report failed:\n- " + string.Join("\n- ", report.Issues));
            }

            Debug.Log($"HD2D outdoor scenic backdrop foundation cycle 29 report written: {ReportPath}");
        }

        private static OutdoorScenicBackdropCycle29Report BuildReport()
        {
            var report = new OutdoorScenicBackdropCycle29Report
            {
                BranchName = BranchName,
                WorktreePath = WorktreePath,
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

            CollectExistingNames(report.CurrentHouseExteriorObjectNames, report.Issues, new[]
            {
                "Current_HouseExterior_ScenicBackdrop_SkyCurtainA",
                "Current_HouseExterior_ScenicBackdrop_LowHazeBandA",
                "Current_HouseExterior_ScenicBackdrop_DistantTreeLineA",
                "Current_HouseExterior_ScenicBackdrop_LeftSkyWrapA",
                "Current_HouseExterior_ScenicBackdrop_RightSkyWrapA"
            });

            CollectExistingNames(report.PastHouseExteriorObjectNames, report.Issues, new[]
            {
                "Past_HouseExterior_ScenicBackdrop_SkyCurtainA",
                "Past_HouseExterior_ScenicBackdrop_LowHazeBandA",
                "Past_HouseExterior_ScenicBackdrop_DistantTreeLineA",
                "Past_HouseExterior_ScenicBackdrop_LeftSkyWrapA",
                "Past_HouseExterior_ScenicBackdrop_RightSkyWrapA"
            });

            CollectExistingNames(report.CurrentCentralPlazaObjectNames, report.Issues, new[]
            {
                "Current_CentralPlaza_ScenicBackdrop_SkyCurtainA",
                "Current_CentralPlaza_ScenicBackdrop_LowHazeBandA",
                "Current_CentralPlaza_ScenicBackdrop_DistantRooflineA",
                "Current_CentralPlaza_ScenicBackdrop_LeftSkyWrapA",
                "Current_CentralPlaza_ScenicBackdrop_RightSkyWrapA"
            });

            CollectExistingNames(report.PastCentralPlazaObjectNames, report.Issues, new[]
            {
                "Past_CentralPlaza_ScenicBackdrop_SkyCurtainA",
                "Past_CentralPlaza_ScenicBackdrop_LowHazeBandA",
                "Past_CentralPlaza_ScenicBackdrop_DistantRooflineA",
                "Past_CentralPlaza_ScenicBackdrop_LeftSkyWrapA",
                "Past_CentralPlaza_ScenicBackdrop_RightSkyWrapA"
            });

            return report;
        }

        private static void CollectExistingNames(List<string> destination, List<string> issues, IEnumerable<string> expectedNames)
        {
            foreach (var expectedName in expectedNames)
            {
                if (FindSceneObjectIncludingInactive(expectedName) != null)
                {
                    destination.Add(expectedName);
                }
                else
                {
                    issues.Add($"Missing expected scene object: {expectedName}.");
                }
            }
        }

        private static string BuildMarkdown(OutdoorScenicBackdropCycle29Report report)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"# {ReportTitle}");
            builder.AppendLine();
            builder.AppendLine($"- Result: {FormatResult(report.Issues.Count == 0)}");
            builder.AppendLine($"- Branch: `{report.BranchName}`");
            builder.AppendLine($"- Worktree: `{report.WorktreePath}`");
            builder.AppendLine($"- Report path: `{report.ReportPathAbsolute}`");
            builder.AppendLine($"- Backdrop object count: `{report.BackdropObjectCount}`");
            builder.AppendLine();
            builder.AppendLine("## Scene Objects");
            builder.AppendLine();
            AppendNames(builder, "Current HouseExterior", report.CurrentHouseExteriorObjectNames);
            AppendNames(builder, "Past HouseExterior", report.PastHouseExteriorObjectNames);
            AppendNames(builder, "Current CentralPlaza", report.CurrentCentralPlazaObjectNames);
            AppendNames(builder, "Past CentralPlaza", report.PastCentralPlazaObjectNames);
            builder.AppendLine("## Validation Notes");
            builder.AppendLine();
            if (report.Issues.Count == 0)
            {
                builder.AppendLine("- PASS: the outdoor scenic backdrop foundation adds readable sky, haze, treeline, and roofline depth without colliders or arrival behavior.");
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

        private static void AppendNames(StringBuilder builder, string heading, IReadOnlyCollection<string> names)
        {
            builder.AppendLine($"### {heading}");
            if (names.Count == 0)
            {
                builder.AppendLine("- (none)");
                builder.AppendLine();
                return;
            }

            foreach (var name in names)
            {
                builder.AppendLine($"- `{name}`");
            }

            builder.AppendLine();
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

        private static string FormatResult(bool passed)
        {
            return passed ? "PASS" : "FAIL";
        }

        private static string GetAbsoluteProjectPath(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", relativePath));
        }

        private sealed class OutdoorScenicBackdropCycle29Report
        {
            public string BranchName;
            public string WorktreePath;
            public string ReportPathAbsolute;
            public readonly List<string> Issues = new List<string>();
            public readonly List<string> CurrentHouseExteriorObjectNames = new List<string>();
            public readonly List<string> PastHouseExteriorObjectNames = new List<string>();
            public readonly List<string> CurrentCentralPlazaObjectNames = new List<string>();
            public readonly List<string> PastCentralPlazaObjectNames = new List<string>();

            public int BackdropObjectCount => CurrentHouseExteriorObjectNames.Count + PastHouseExteriorObjectNames.Count + CurrentCentralPlazaObjectNames.Count + PastCentralPlazaObjectNames.Count;
        }
    }
}
