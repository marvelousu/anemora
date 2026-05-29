using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using UnityEditor;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dExteriorOcclusionBackdropAudit
    {
        private const string ReportRelativeDirectory = "docs/devlog/screenshots/fast_vs_hd2d_exterior_occlusion_backdrop_cycle27_20260522";
        private const string ReportFileName = "exterior_occlusion_backdrop_cycle27_20260522.md";
        private const string ReportTitle = "Fast VS HD2D Exterior Occlusion Backdrop Cycle 27";

        private static readonly string ReportDirectory = GetAbsoluteProjectPath(ReportRelativeDirectory);
        private static readonly string ReportPath = Path.Combine(ReportDirectory, ReportFileName);

        [MenuItem("Tools/Anemora/Write HD2D Exterior Occlusion Backdrop Cycle 27 Report")]
        public static void WriteExteriorOcclusionBackdropCycle27ReportBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();
            var report = BuildReport();

            Directory.CreateDirectory(ReportDirectory);
            File.WriteAllText(ReportPath, BuildMarkdown(report), Encoding.UTF8);
            AssetDatabase.Refresh();

            if (report.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D exterior occlusion backdrop cycle 27 report failed:\n- " + string.Join("\n- ", report.Issues));
            }

            Debug.Log($"HD2D exterior occlusion backdrop cycle 27 report written: {ReportPath}");
        }

        private static ExteriorOcclusionBackdropCycle27Report BuildReport()
        {
            var report = new ExteriorOcclusionBackdropCycle27Report
            {
                BranchName = "work/fast-vs-hd2d-shading-foundation-20260522",
                WorktreePath = @"C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work",
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

            CollectExistingNames(report.CurrentHouseOcclusionObjectNames, report.Issues, new[]
            {
                "Current_HouseExterior_OcclusionShell_BackPlate",
                "Current_HouseExterior_OcclusionShell_LeftReturnWall",
                "Current_HouseExterior_OcclusionShell_RightReturnWall",
                "Current_HouseExterior_OcclusionShell_RoofDepthCap",
                "Current_HouseExterior_OcclusionShell_UnderEaveMask",
                "Current_HouseExterior_OcclusionShell_DoorwayDarkMask",
                "Current_HouseExterior_OcclusionShell_DoorJambFillLeft",
                "Current_HouseExterior_OcclusionShell_DoorJambFillRight"
            });

            CollectExistingNames(report.PastHouseOcclusionObjectNames, report.Issues, new[]
            {
                "Past_HouseExterior_OcclusionShell_BackPlate",
                "Past_HouseExterior_OcclusionShell_LeftReturnWall",
                "Past_HouseExterior_OcclusionShell_RightReturnWall",
                "Past_HouseExterior_OcclusionShell_RoofDepthCap",
                "Past_HouseExterior_OcclusionShell_UnderEaveMask",
                "Past_HouseExterior_OcclusionShell_DoorwayDarkMask",
                "Past_HouseExterior_OcclusionShell_DoorJambFillLeft",
                "Past_HouseExterior_OcclusionShell_DoorJambFillRight"
            });

            CollectExistingNames(report.CurrentPlazaOcclusionObjectNames, report.Issues, new[]
            {
                "Current_CentralPlaza_LibraryOcclusionShell_BackVolume",
                "Current_CentralPlaza_LibraryOcclusionShell_WestSideReturn",
                "Current_CentralPlaza_LibraryOcclusionShell_EastSideReturn",
                "Current_CentralPlaza_LibraryOcclusionShell_RoofBackCap",
                "Current_CentralPlaza_LibraryOcclusionShell_UnderEaveDepthMask",
                "Current_CentralPlaza_LibraryOcclusionShell_WindowBackingLeft",
                "Current_CentralPlaza_LibraryOcclusionShell_WindowBackingRight"
            });

            CollectExistingNames(report.PastPlazaOcclusionObjectNames, report.Issues, new[]
            {
                "Past_CentralPlaza_LibraryOcclusionShell_BackVolume",
                "Past_CentralPlaza_LibraryOcclusionShell_WestSideReturn",
                "Past_CentralPlaza_LibraryOcclusionShell_EastSideReturn",
                "Past_CentralPlaza_LibraryOcclusionShell_RoofBackCap",
                "Past_CentralPlaza_LibraryOcclusionShell_UnderEaveDepthMask",
                "Past_CentralPlaza_LibraryOcclusionShell_WindowBackingLeft",
                "Past_CentralPlaza_LibraryOcclusionShell_WindowBackingRight"
            });

            CollectExistingNames(report.CurrentHouseBackdropObjectNames, report.Issues, new[]
            {
                "Current_HouseExterior_BackdropFoundation_SkyBackPlane",
                "Current_HouseExterior_BackdropFoundation_HorizonTreeLine"
            });

            CollectExistingNames(report.PastHouseBackdropObjectNames, report.Issues, new[]
            {
                "Past_HouseExterior_BackdropFoundation_SkyBackPlane",
                "Past_HouseExterior_BackdropFoundation_HorizonTreeLine"
            });

            CollectExistingNames(report.CurrentPlazaBackdropObjectNames, report.Issues, new[]
            {
                "Current_CentralPlaza_BackdropFoundation_SkyBackPlane",
                "Current_CentralPlaza_BackdropFoundation_HorizonRoofline"
            });

            CollectExistingNames(report.PastPlazaBackdropObjectNames, report.Issues, new[]
            {
                "Past_CentralPlaza_BackdropFoundation_SkyBackPlane",
                "Past_CentralPlaza_BackdropFoundation_HorizonRoofline"
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

        private static string BuildMarkdown(ExteriorOcclusionBackdropCycle27Report report)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"# {ReportTitle}");
            builder.AppendLine();
            builder.AppendLine($"- Result: {FormatResult(report.Issues.Count == 0)}");
            builder.AppendLine($"- Branch: `{report.BranchName}`");
            builder.AppendLine($"- Worktree: `{report.WorktreePath}`");
            builder.AppendLine($"- Report path: `{report.ReportPathAbsolute}`");
            builder.AppendLine($"- House occlusion object count: `{report.HouseOcclusionObjectCount}`");
            builder.AppendLine($"- Plaza occlusion object count: `{report.LibraryOcclusionObjectCount}`");
            builder.AppendLine($"- Backdrop object count: `{report.BackdropObjectCount}`");
            builder.AppendLine();
            builder.AppendLine("## Scene Objects");
            builder.AppendLine();
            AppendNames(builder, "Current house occlusion", report.CurrentHouseOcclusionObjectNames);
            AppendNames(builder, "Past house occlusion", report.PastHouseOcclusionObjectNames);
            AppendNames(builder, "Current plaza occlusion", report.CurrentPlazaOcclusionObjectNames);
            AppendNames(builder, "Past plaza occlusion", report.PastPlazaOcclusionObjectNames);
            AppendNames(builder, "Current house backdrop", report.CurrentHouseBackdropObjectNames);
            AppendNames(builder, "Past house backdrop", report.PastHouseBackdropObjectNames);
            AppendNames(builder, "Current plaza backdrop", report.CurrentPlazaBackdropObjectNames);
            AppendNames(builder, "Past plaza backdrop", report.PastPlazaBackdropObjectNames);
            builder.AppendLine("## Validation Notes");
            builder.AppendLine();
            if (report.Issues.Count == 0)
            {
                builder.AppendLine("- PASS: the exterior shells block internal/behind-the-scenes visibility and the backdrop foundation gives the map a grounded horizon.");
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

        private sealed class ExteriorOcclusionBackdropCycle27Report
        {
            public string BranchName;
            public string WorktreePath;
            public string ReportPathAbsolute;
            public readonly List<string> Issues = new List<string>();
            public readonly List<string> CurrentHouseOcclusionObjectNames = new List<string>();
            public readonly List<string> PastHouseOcclusionObjectNames = new List<string>();
            public readonly List<string> CurrentPlazaOcclusionObjectNames = new List<string>();
            public readonly List<string> PastPlazaOcclusionObjectNames = new List<string>();
            public readonly List<string> CurrentHouseBackdropObjectNames = new List<string>();
            public readonly List<string> PastHouseBackdropObjectNames = new List<string>();
            public readonly List<string> CurrentPlazaBackdropObjectNames = new List<string>();
            public readonly List<string> PastPlazaBackdropObjectNames = new List<string>();

            public int HouseOcclusionObjectCount => CurrentHouseOcclusionObjectNames.Count + PastHouseOcclusionObjectNames.Count;
            public int LibraryOcclusionObjectCount => CurrentPlazaOcclusionObjectNames.Count + PastPlazaOcclusionObjectNames.Count;
            public int BackdropObjectCount => CurrentHouseBackdropObjectNames.Count + PastHouseBackdropObjectNames.Count + CurrentPlazaBackdropObjectNames.Count + PastPlazaBackdropObjectNames.Count;
        }
    }
}
