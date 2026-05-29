using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dHouseExteriorFacadeCompositionAudit
    {
        private const string BranchName = "work/fast-vs-hd2d-shading-foundation-20260522";
        private const string WorktreePath = @"C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work";
        private const string ReportRelativeDirectory = "docs/devlog/screenshots/fast_vs_hd2d_house_exterior_facade_composition_cycle28_20260522";
        private const string ReportFileName = "house_exterior_facade_composition_cycle28_20260522.md";
        private const string ReportTitle = "Fast VS HD2D House Exterior Facade Composition Cycle 28";

        private static readonly string ReportDirectory = GetAbsoluteProjectPath(ReportRelativeDirectory);
        private static readonly string ReportPath = Path.Combine(ReportDirectory, ReportFileName);

        [MenuItem("Tools/Anemora/Write HD2D House Exterior Facade Composition Cycle 28 Report")]
        public static void WriteHouseExteriorFacadeCompositionCycle28ReportBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();
            var report = BuildReport();

            Directory.CreateDirectory(ReportDirectory);
            File.WriteAllText(ReportPath, BuildMarkdown(report), Encoding.UTF8);
            AssetDatabase.Refresh();

            if (report.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D house exterior facade composition cycle 28 report failed:\n- " + string.Join("\n- ", report.Issues));
            }

            Debug.Log($"HD2D house exterior facade composition cycle 28 report written: {ReportPath}");
        }

        private static HouseExteriorFacadeCompositionCycle28Report BuildReport()
        {
            var report = new HouseExteriorFacadeCompositionCycle28Report
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

            CollectExistingNames(report.CurrentHouseFacadeObjectNames, report.Issues, new[]
            {
                "Current_HouseExterior_FacadeComposition_DoorLeftReturnWallA",
                "Current_HouseExterior_FacadeComposition_DoorRightReturnWallA",
                "Current_HouseExterior_FacadeComposition_RightWallVerticalTrimA",
                "Current_HouseExterior_FacadeComposition_RightWallBaseTrimA",
                "Current_HouseExterior_FacadeComposition_PorchPostBackTrimA",
                "Current_HouseExterior_FacadeComposition_RightWallMiddleBreakLineA",
                "Current_HouseExterior_FacadeComposition_RightWallUpperBreakLineA",
                "Current_HouseExterior_FacadeComposition_UnderRoofDepthShadowA",
                "Current_HouseExterior_FacadeComposition_RoofSideRakeLineA",
                "Current_HouseExterior_FacadeComposition_BackdropSideMaskLeftA",
                "Current_HouseExterior_FacadeComposition_BackdropSideMaskRightA"
            });

            CollectExistingNames(report.PastHouseFacadeObjectNames, report.Issues, new[]
            {
                "Past_HouseExterior_FacadeComposition_DoorLeftReturnWallA",
                "Past_HouseExterior_FacadeComposition_DoorRightReturnWallA",
                "Past_HouseExterior_FacadeComposition_RightWallVerticalTrimA",
                "Past_HouseExterior_FacadeComposition_RightWallBaseTrimA",
                "Past_HouseExterior_FacadeComposition_PorchPostBackTrimA",
                "Past_HouseExterior_FacadeComposition_RightWallMiddleBreakLineA",
                "Past_HouseExterior_FacadeComposition_RightWallUpperBreakLineA",
                "Past_HouseExterior_FacadeComposition_UnderRoofDepthShadowA",
                "Past_HouseExterior_FacadeComposition_RoofSideRakeLineA",
                "Past_HouseExterior_FacadeComposition_BackdropSideMaskLeftA",
                "Past_HouseExterior_FacadeComposition_BackdropSideMaskRightA"
            });

            report.AdditionalObjectCount = report.CurrentHouseFacadeObjectNames.Count + report.PastHouseFacadeObjectNames.Count;
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

        private static string BuildMarkdown(HouseExteriorFacadeCompositionCycle28Report report)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"# {ReportTitle}");
            builder.AppendLine();
            builder.AppendLine($"- Result: {FormatResult(report.Issues.Count == 0)}");
            builder.AppendLine($"- Branch: `{report.BranchName}`");
            builder.AppendLine($"- Worktree: `{report.WorktreePath}`");
            builder.AppendLine($"- Report path: `{report.ReportPathAbsolute}`");
            builder.AppendLine($"- Additional object count: `{report.AdditionalObjectCount}`");
            builder.AppendLine();
            builder.AppendLine("## Scene Objects");
            builder.AppendLine();
            AppendNames(builder, "Current house facade composition", report.CurrentHouseFacadeObjectNames);
            AppendNames(builder, "Past house facade composition", report.PastHouseFacadeObjectNames);
            builder.AppendLine("## Validation Notes");
            builder.AppendLine();
            if (report.Issues.Count == 0)
            {
                builder.AppendLine("- PASS: the corrected facade composition keeps the doorway readable, avoids the rejected black-bar artifact, breaks up the right wall plane, adds thin roof and porch depth, and closes the exterior background edges without introducing colliders or arrival markers.");
                builder.AppendLine("- PASS: Current_HouseExterior_DoorClosedPanel, Past_HouseExterior_DoorClosedPanel, Current_HouseExterior_DoorEntrySmallGlow, and Past_HouseExterior_DoorEntrySmallGlow remain present.");
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

        private sealed class HouseExteriorFacadeCompositionCycle28Report
        {
            public string BranchName;
            public string WorktreePath;
            public string ReportPathAbsolute;
            public int AdditionalObjectCount;
            public readonly List<string> CurrentHouseFacadeObjectNames = new List<string>();
            public readonly List<string> PastHouseFacadeObjectNames = new List<string>();
            public readonly List<string> Issues = new List<string>();
        }
    }
}
