using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Anemora.FastVS;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dOverlayProfileFoundationAudit
    {
        private const string MaterialRoleTagName = "AnemoraFastVsHd2dRole";
        private const float VectorTolerance = 0.005f;
        private const float ColorTolerance = 0.02f;
        private static readonly string Cycle20OutputDirectory = Path.GetFullPath(Path.Combine(
            Application.dataPath,
            "..",
            "docs",
            "devlog",
            "screenshots",
            "fast_vs_hd2d_surface_directional_shade_texture_cycle20_20260522"));
        private const string Cycle20ReportFileName = "surface_directional_shade_texture_cycle20_20260522.md";
        private static readonly string Cycle20ReportPath = Path.Combine(Cycle20OutputDirectory, Cycle20ReportFileName);
        private static readonly string Cycle20TexturePath = Path.GetFullPath(Path.Combine(
            Application.dataPath,
            "Art",
            "Textures",
            "FastVS",
            "HouseSlice",
            "FastVS_House_surface_directional_shade_overlay_soft.png"));
        private static readonly string Cycle21OutputDirectory = Path.GetFullPath(Path.Combine(
            Application.dataPath,
            "..",
            "docs",
            "devlog",
            "screenshots",
            "fast_vs_hd2d_static_directional_shadow_texture_cycle21_20260522"));
        private const string Cycle21ReportFileName = "static_directional_shadow_texture_cycle21_20260522.md";
        private static readonly string Cycle21ReportPath = Path.Combine(Cycle21OutputDirectory, Cycle21ReportFileName);
        private static readonly string Cycle21TexturePath = Path.GetFullPath(Path.Combine(
            Application.dataPath,
            "Art",
            "Textures",
            "FastVS",
            "HouseSlice",
            "FastVS_House_static_directional_cast_shadow_soft.png"));
        private static readonly string Cycle22OutputDirectory = Path.GetFullPath(Path.Combine(
            Application.dataPath,
            "..",
            "docs",
            "devlog",
            "screenshots",
            "fast_vs_hd2d_character_contact_shadow_texture_cycle22_20260522"));
        private const string Cycle22ReportFileName = "character_contact_shadow_texture_cycle22_20260522.md";
        private static readonly string Cycle22ReportPath = Path.Combine(Cycle22OutputDirectory, Cycle22ReportFileName);
        private const string Cycle22TextureAssetPath = "Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset";
        private static readonly string Cycle22TexturePath = Path.GetFullPath(Path.Combine(
            Application.dataPath,
            "Art",
            "Textures",
            "FastVS",
            "HouseSlice",
            "FastVS_House_character_contact_shadow.asset"));

        [MenuItem("Tools/Anemora/Verify HD2D Overlay Profiles V1")]
        public static void VerifyOverlayProfilesV1()
        {
            var issues = new List<string>();

            ValidateProfile(
                issues,
                "FastVS_PlayerContactShadow_Niro",
                FastVsHouseArea.Interior,
                FastVsHd2dOverlayKind.CharacterContactShadow,
                currentWorld: true,
                dynamicSubject: true,
                new Vector2(0.18f, 0.42f),
                new Vector2(0.66f, 0.24f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "FastVS_PlayerFootContact_Niro",
                FastVsHouseArea.Interior,
                FastVsHd2dOverlayKind.CharacterFootContact,
                currentWorld: true,
                dynamicSubject: true,
                new Vector2(0.20f, 0.48f),
                new Vector2(0.34f, 0.075f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "FastVS_PlayerDirectionalCastShadow_Niro",
                FastVsHouseArea.Interior,
                FastVsHd2dOverlayKind.CharacterDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: true,
                new Vector2(0.10f, 0.34f),
                new Vector2(0.72f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_Library_Reto_ContactShadow",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterContactShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.18f, 0.42f),
                new Vector2(0.66f, 0.24f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_Library_Reto_FootContact",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterFootContact,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.20f, 0.48f),
                new Vector2(0.30f, 0.070f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_Library_Reto_DirectionalCastShadow",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(0.60f, 0.16f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_Library_Aria_ContactShadow",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterContactShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.18f, 0.42f),
                new Vector2(0.70f, 0.24f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_Library_Aria_FootContact",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterFootContact,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.20f, 0.48f),
                new Vector2(0.31f, 0.070f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_Library_Aria_DirectionalCastShadow",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterDirectionalCastShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(0.60f, 0.16f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_HouseExterior_StaticDirectionalCastShadow_HouseFacade",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(2.04f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_HouseExterior_StaticDirectionalCastShadow_HouseFacade",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(2.04f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(3.12f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(3.12f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_Library_StaticDirectionalCastShadow_BackShelf",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(4.98f, 0.16f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_Library_StaticDirectionalCastShadow_BackShelf",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(4.98f, 0.16f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_HouseExterior_SurfaceDirectionalShade_FacadeLeft",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(1.30f, 2.10f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_HouseExterior_SurfaceDirectionalShade_FacadeLeft",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(1.24f, 2.04f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_CentralPlaza_SurfaceDirectionalShade_LibraryFacade",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(8.96f, 2.62f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_CentralPlaza_SurfaceDirectionalShade_LibraryFacade",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(8.72f, 2.56f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_Library_SurfaceDirectionalShade_BackShelf",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(8.82f, 2.02f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Past_Library_SurfaceDirectionalShade_BackShelf",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(8.66f, 1.96f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow);

            ValidateProfile(
                issues,
                "Current_HouseInterior_Table_WarmLightPool",
                FastVsHouseArea.Interior,
                FastVsHd2dOverlayKind.LightPool,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.24f),
                new Vector2(1.24f, 0.88f),
                new Color(1.0f, 0.72f, 0.30f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.OverlayGlow);

            ValidateProfile(
                issues,
                "Past_HouseInterior_Table_WarmLightPool",
                FastVsHouseArea.Interior,
                FastVsHd2dOverlayKind.LightPool,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.24f),
                new Vector2(1.24f, 0.88f),
                new Color(1.0f, 0.72f, 0.30f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.OverlayGlow);

            ValidateProfile(
                issues,
                "Past_HouseExterior_Door_WarmPool",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.LightPool,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.24f),
                new Vector2(1.54f, 0.82f),
                new Color(1.0f, 0.72f, 0.30f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.OverlayGlow);

            ValidateProfile(
                issues,
                "Past_CentralPlaza_LibraryFacade_WindowWarmPool",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.LightPool,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.24f),
                new Vector2(6.60f, 0.56f),
                new Color(1.0f, 0.72f, 0.30f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.OverlayGlow);

            ValidateProfile(
                issues,
                "Current_Library_RetoDesk_WarmPool",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.LightPool,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.24f),
                new Vector2(1.72f, 0.72f),
                new Color(1.0f, 0.72f, 0.30f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.OverlayGlow);

            ValidateProfile(
                issues,
                "Current_Library_EntryFloor_SoftDustPool",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.LightPool,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.24f),
                new Vector2(3.80f, 1.25f),
                new Color(1.0f, 0.72f, 0.30f, 0.90f),
                AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.OverlayGlow);

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D overlay profile audit failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("HD2D overlay profile audit passed.");
        }

        [MenuItem("Tools/Anemora/Write HD2D Surface Directional Shade Texture Cycle 20 Report")]
        public static void WriteSurfaceDirectionalShadeTextureCycle20ReportBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();

            var report = BuildSurfaceDirectionalShadeTextureCycle20Report();
            Directory.CreateDirectory(Cycle20OutputDirectory);
            File.WriteAllText(Cycle20ReportPath, BuildSurfaceDirectionalShadeTextureCycle20Markdown(report), Encoding.UTF8);
            AssetDatabase.Refresh();

            if (report.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D surface directional shade texture cycle 20 report failed:\n- " + string.Join("\n- ", report.Issues));
            }

            Debug.Log($"HD2D surface directional shade texture report written: {Cycle20ReportPath}");
        }

        [MenuItem("Tools/Anemora/Write HD2D Static Directional Shadow Texture Cycle 21 Report")]
        public static void WriteStaticDirectionalShadowTextureCycle21ReportBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();

            var report = BuildStaticDirectionalShadowTextureCycle21Report();
            Directory.CreateDirectory(Cycle21OutputDirectory);
            File.WriteAllText(Cycle21ReportPath, BuildStaticDirectionalShadowTextureCycle21Markdown(report), Encoding.UTF8);
            AssetDatabase.Refresh();

            if (report.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D static directional shadow texture cycle 21 report failed:\n- " + string.Join("\n- ", report.Issues));
            }

            Debug.Log($"HD2D static directional shadow texture report written: {Cycle21ReportPath}");
        }

        [MenuItem("Tools/Anemora/Write HD2D Character Contact Shadow Texture Cycle 22 Report")]
        public static void WriteCharacterContactShadowTextureCycle22ReportBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();

            var report = BuildCharacterContactShadowTextureCycle22Report();
            Directory.CreateDirectory(Cycle22OutputDirectory);
            File.WriteAllText(Cycle22ReportPath, BuildCharacterContactShadowTextureCycle22Markdown(report), Encoding.UTF8);
            AssetDatabase.Refresh();

            if (report.Issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D character contact shadow texture cycle 22 report failed:\n- " + string.Join("\n- ", report.Issues));
            }

            Debug.Log($"HD2D character contact shadow texture report written: {Cycle22ReportPath}");
        }

        private static CharacterContactShadowTextureCycle22Report BuildCharacterContactShadowTextureCycle22Report()
        {
            var report = new CharacterContactShadowTextureCycle22Report();
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Cycle22TextureAssetPath);
            if (texture == null)
            {
                report.Issues.Add($"Character contact shadow texture asset is missing at {Cycle22TexturePath}.");
                return report;
            }

            report.Width = texture.width;
            report.Height = texture.height;
            report.CenterAlpha = texture.GetPixel(texture.width / 2, texture.height / 2).a;
            report.LeftEdgeAlpha = texture.GetPixel(0, texture.height / 2).a;
            report.RightEdgeAlpha = texture.GetPixel(texture.width - 1, texture.height / 2).a;
            report.LeftFootAlpha = texture.GetPixel(Mathf.Clamp(Mathf.RoundToInt(texture.width * 0.37f), 0, texture.width - 1), Mathf.Clamp(Mathf.RoundToInt(texture.height * 0.60f), 0, texture.height - 1)).a;
            report.RightFootAlpha = texture.GetPixel(Mathf.Clamp(Mathf.RoundToInt(texture.width * 0.63f), 0, texture.width - 1), Mathf.Clamp(Mathf.RoundToInt(texture.height * 0.60f), 0, texture.height - 1)).a;
            report.TopEdgeAlpha = texture.GetPixel(texture.width / 2, texture.height - 1).a;
            report.BottomEdgeAlpha = texture.GetPixel(texture.width / 2, 0).a;
            report.TopLeftCornerAlpha = texture.GetPixel(0, texture.height - 1).a;
            report.TopRightCornerAlpha = texture.GetPixel(texture.width - 1, texture.height - 1).a;
            report.BottomLeftCornerAlpha = texture.GetPixel(0, 0).a;
            report.BottomRightCornerAlpha = texture.GetPixel(texture.width - 1, 0).a;

            foreach (var pixel in texture.GetPixels32())
            {
                report.MaxAlpha = Mathf.Max(report.MaxAlpha, pixel.a / 255f);
            }

            if (report.Width != 96 || report.Height != 48)
            {
                report.Issues.Add($"Character contact shadow texture must stay exactly 96x48, but was {report.Width}x{report.Height}.");
            }

            if (report.CenterAlpha < 0.15f || report.CenterAlpha > 0.25f)
            {
                report.Issues.Add($"Center alpha {report.CenterAlpha:0.000} is outside the 0.15-0.25 range.");
            }

            if (report.LeftEdgeAlpha > 0.02f || report.RightEdgeAlpha > 0.02f)
            {
                report.Issues.Add($"Edge alpha must stay at or below 0.020. left={report.LeftEdgeAlpha:0.000}, right={report.RightEdgeAlpha:0.000}.");
            }

            if (report.TopEdgeAlpha > 0.03f || report.BottomEdgeAlpha > 0.03f)
            {
                report.Issues.Add($"Top/bottom edge alpha must stay soft. top={report.TopEdgeAlpha:0.000}, bottom={report.BottomEdgeAlpha:0.000}.");
            }

            if (report.LeftFootAlpha < 0.19f || report.LeftFootAlpha > 0.32f)
            {
                report.Issues.Add($"Left foot alpha {report.LeftFootAlpha:0.000} is outside the 0.19-0.32 range.");
            }

            if (report.RightFootAlpha < 0.19f || report.RightFootAlpha > 0.32f)
            {
                report.Issues.Add($"Right foot alpha {report.RightFootAlpha:0.000} is outside the 0.19-0.32 range.");
            }

            if (report.MaxAlpha < 0.22f || report.MaxAlpha > 0.34f)
            {
                report.Issues.Add($"Max alpha {report.MaxAlpha:0.000} is outside the 0.22-0.34 range.");
            }

            if (report.TopLeftCornerAlpha > 0.01f || report.TopRightCornerAlpha > 0.01f || report.BottomLeftCornerAlpha > 0.01f || report.BottomRightCornerAlpha > 0.01f)
            {
                report.Issues.Add($"Corner alpha must stay near transparent. tl={report.TopLeftCornerAlpha:0.000}, tr={report.TopRightCornerAlpha:0.000}, bl={report.BottomLeftCornerAlpha:0.000}, br={report.BottomRightCornerAlpha:0.000}.");
            }

            return report;
        }

        private static StaticDirectionalShadowTextureCycle21Report BuildStaticDirectionalShadowTextureCycle21Report()
        {
            var report = new StaticDirectionalShadowTextureCycle21Report();
            if (!File.Exists(Cycle21TexturePath))
            {
                report.Issues.Add($"Static directional cast shadow texture PNG is missing at {Cycle21TexturePath}.");
                return report;
            }

            var bytes = File.ReadAllBytes(Cycle21TexturePath);
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                if (!texture.LoadImage(bytes, false))
                {
                    report.Issues.Add($"Static directional cast shadow texture PNG could not be read at {Cycle21TexturePath}.");
                    return report;
                }

                texture.name = "FastVS_House_static_directional_cast_shadow_soft";
                report.Width = texture.width;
                report.Height = texture.height;

                var centerX = texture.width / 2;
                var centerY = texture.height / 2;
                var coreX = Mathf.Min(64, texture.width - 1);
                var coreY = Mathf.Min(42, texture.height - 1);
                var tailX = Mathf.Min(108, texture.width - 1);
                var tailY = Mathf.Min(40, texture.height - 1);

                report.CenterAlpha = texture.GetPixel(centerX, centerY).a;
                report.LeftEdgeAlpha = texture.GetPixel(0, centerY).a;
                report.RightEdgeAlpha = texture.GetPixel(texture.width - 1, centerY).a;
                report.CoreAlpha = texture.GetPixel(coreX, coreY).a;
                report.TailAlpha = texture.GetPixel(tailX, tailY).a;
                report.TopLeftCornerAlpha = texture.GetPixel(0, 0).a;
                report.TopRightCornerAlpha = texture.GetPixel(texture.width - 1, 0).a;
                report.BottomLeftCornerAlpha = texture.GetPixel(0, texture.height - 1).a;
                report.BottomRightCornerAlpha = texture.GetPixel(texture.width - 1, texture.height - 1).a;

                foreach (var pixel in texture.GetPixels32())
                {
                    report.MaxAlpha = Mathf.Max(report.MaxAlpha, pixel.a / 255f);
                }

                if (report.Width != 160 || report.Height != 80)
                {
                    report.Issues.Add($"Static directional cast shadow texture must stay exactly 160x80, but was {report.Width}x{report.Height}.");
                }

                if (report.CenterAlpha < 0.12f || report.CenterAlpha > 0.20f)
                {
                    report.Issues.Add($"Center alpha {report.CenterAlpha:0.000} is outside the 0.12-0.20 range.");
                }

                if (report.CoreAlpha < 0.12f || report.CoreAlpha > 0.20f)
                {
                    report.Issues.Add($"Core sample alpha {report.CoreAlpha:0.000} is outside the 0.12-0.20 range.");
                }

                if (report.MaxAlpha < 0.12f || report.MaxAlpha > 0.20f)
                {
                    report.Issues.Add($"Max alpha {report.MaxAlpha:0.000} is outside the 0.12-0.20 range.");
                }

                if (report.TailAlpha < report.LeftEdgeAlpha + 0.025f)
                {
                    report.Issues.Add($"Tail alpha {report.TailAlpha:0.000} must exceed left edge alpha {report.LeftEdgeAlpha:0.000} by at least 0.025.");
                }

                if (report.TailAlpha >= report.CoreAlpha || report.TailAlpha >= report.MaxAlpha)
                {
                    report.Issues.Add($"Tail alpha {report.TailAlpha:0.000} must stay below core alpha {report.CoreAlpha:0.000} and max alpha {report.MaxAlpha:0.000}.");
                }

                if (report.LeftEdgeAlpha > report.CenterAlpha * 0.55f || report.RightEdgeAlpha > report.CenterAlpha * 0.55f)
                {
                    report.Issues.Add($"Edge alpha must stay well below center alpha. left={report.LeftEdgeAlpha:0.000}, right={report.RightEdgeAlpha:0.000}, center={report.CenterAlpha:0.000}.");
                }

                if (report.TopLeftCornerAlpha > 0.02f || report.TopRightCornerAlpha > 0.02f || report.BottomLeftCornerAlpha > 0.02f || report.BottomRightCornerAlpha > 0.02f)
                {
                    report.Issues.Add($"Corner alpha must stay near transparent. tl={report.TopLeftCornerAlpha:0.000}, tr={report.TopRightCornerAlpha:0.000}, bl={report.BottomLeftCornerAlpha:0.000}, br={report.BottomRightCornerAlpha:0.000}.");
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(texture);
            }

            return report;
        }

        private static string BuildStaticDirectionalShadowTextureCycle21Markdown(StaticDirectionalShadowTextureCycle21Report report)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Fast VS HD2D Static Directional Shadow Texture Cycle 21 Report");
            builder.AppendLine();
            builder.AppendLine("Deterministic v2 elongated asymmetric cast-shadow foundation for the house facade, central plaza library facade, and library back shelf. The goal is a restrained painterly shadow that reads as contact and depth without collapsing into a flat black rectangle.");
            builder.AppendLine();
            builder.AppendLine($"- Texture PNG: `{Cycle21TexturePath}`");
            builder.AppendLine($"- Report file: `{Cycle21ReportPath}`");
            builder.AppendLine($"- Result: {report.Result}");
            builder.AppendLine();
            builder.AppendLine("| Metric | Value |");
            builder.AppendLine("|---|---:|");
            builder.AppendLine($"| Width | {report.Width} |");
            builder.AppendLine($"| Height | {report.Height} |");
            builder.AppendLine($"| Center alpha | {report.CenterAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Max alpha | {report.MaxAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Left edge alpha | {report.LeftEdgeAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Right edge alpha | {report.RightEdgeAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Core sample alpha | {report.CoreAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Tail sample alpha | {report.TailAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Top-left corner alpha | {report.TopLeftCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Top-right corner alpha | {report.TopRightCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Bottom-left corner alpha | {report.BottomLeftCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Bottom-right corner alpha | {report.BottomRightCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");

            if (report.Issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                foreach (var issue in report.Issues)
                {
                    builder.AppendLine($"- {issue}");
                }
            }

            return builder.ToString();
        }

        private static string BuildCharacterContactShadowTextureCycle22Markdown(CharacterContactShadowTextureCycle22Report report)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Fast VS HD2D Character Contact Shadow Texture Cycle 22 Report");
            builder.AppendLine();
            builder.AppendLine("Deterministic v2 character contact shadow foundation for Niro, Reto, and Aria. The goal is a grounded HD-2D paper-character contact shadow with broad body weight, two foot-contact lobes, soft edge fade, and no flat rectangular silhouette.");
            builder.AppendLine();
            builder.AppendLine($"- Texture asset: `{Cycle22TexturePath}`");
            builder.AppendLine($"- Report file: `{Cycle22ReportPath}`");
            builder.AppendLine($"- Result: {report.Result}");
            builder.AppendLine();
            builder.AppendLine("| Metric | Value |");
            builder.AppendLine("|---|---:|");
            builder.AppendLine($"| Width | {report.Width} |");
            builder.AppendLine($"| Height | {report.Height} |");
            builder.AppendLine($"| Center alpha | {report.CenterAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Max alpha | {report.MaxAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Left edge alpha | {report.LeftEdgeAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Right edge alpha | {report.RightEdgeAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Left foot alpha | {report.LeftFootAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Right foot alpha | {report.RightFootAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Top edge alpha | {report.TopEdgeAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Bottom edge alpha | {report.BottomEdgeAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Top-left corner alpha | {report.TopLeftCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Top-right corner alpha | {report.TopRightCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Bottom-left corner alpha | {report.BottomLeftCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Bottom-right corner alpha | {report.BottomRightCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Result | {report.Result} |");

            if (report.Issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                foreach (var issue in report.Issues)
                {
                    builder.AppendLine($"- {issue}");
                }
            }

            return builder.ToString();
        }

        private static SurfaceDirectionalShadeTextureCycle20Report BuildSurfaceDirectionalShadeTextureCycle20Report()
        {
            var report = new SurfaceDirectionalShadeTextureCycle20Report();
            if (!File.Exists(Cycle20TexturePath))
            {
                report.Issues.Add($"Surface directional shade texture PNG is missing at {Cycle20TexturePath}.");
                return report;
            }

            var bytes = File.ReadAllBytes(Cycle20TexturePath);
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                if (!texture.LoadImage(bytes, false))
                {
                    report.Issues.Add($"Surface directional shade texture PNG could not be read at {Cycle20TexturePath}.");
                    return report;
                }

                texture.name = "FastVS_House_surface_directional_shade_overlay_soft";
                report.Width = texture.width;
                report.Height = texture.height;

                var centerX = texture.width / 2;
                var centerY = texture.height / 2;
                var topLeftInteriorX = Mathf.Min(32, texture.width - 1);
                var topLeftInteriorY = Mathf.Min(32, texture.height - 1);
                var lowerRightInteriorX = Mathf.Min(96, texture.width - 1);
                var lowerRightInteriorY = Mathf.Min(96, texture.height - 1);

                report.CenterAlpha = texture.GetPixel(centerX, centerY).a;
                report.LeftEdgeAlpha = texture.GetPixel(0, centerY).a;
                report.RightEdgeAlpha = texture.GetPixel(texture.width - 1, centerY).a;
                report.TopLeftInteriorAlpha = texture.GetPixel(topLeftInteriorX, topLeftInteriorY).a;
                report.LowerRightInteriorAlpha = texture.GetPixel(lowerRightInteriorX, lowerRightInteriorY).a;
                report.TopLeftCornerAlpha = texture.GetPixel(0, 0).a;
                report.TopRightCornerAlpha = texture.GetPixel(texture.width - 1, 0).a;
                report.BottomLeftCornerAlpha = texture.GetPixel(0, texture.height - 1).a;
                report.BottomRightCornerAlpha = texture.GetPixel(texture.width - 1, texture.height - 1).a;

                foreach (var pixel in texture.GetPixels32())
                {
                    report.MaxAlpha = Mathf.Max(report.MaxAlpha, pixel.a / 255f);
                }

                if (report.Width != 128 || report.Height != 128)
                {
                    report.Issues.Add($"Surface directional shade texture must stay exactly 128x128, but was {report.Width}x{report.Height}.");
                }

                if (report.MaxAlpha <= 0f)
                {
                    report.Issues.Add("Surface directional shade texture alpha must not be empty.");
                }

                if (report.CenterAlpha < 0.04f || report.CenterAlpha > 0.11f)
                {
                    report.Issues.Add($"Center alpha {report.CenterAlpha:0.000} is outside the 0.04-0.11 range.");
                }

                if (report.MaxAlpha < 0.08f || report.MaxAlpha > 0.16f)
                {
                    report.Issues.Add($"Max alpha {report.MaxAlpha:0.000} is outside the 0.08-0.16 range.");
                }

                if (report.TopLeftInteriorAlpha <= report.LowerRightInteriorAlpha + 0.015f)
                {
                    report.Issues.Add($"Upper-left interior alpha {report.TopLeftInteriorAlpha:0.000} must exceed lower-right interior alpha {report.LowerRightInteriorAlpha:0.000} by at least 0.015.");
                }

                if (report.LeftEdgeAlpha > report.CenterAlpha * 0.60f || report.RightEdgeAlpha > report.CenterAlpha * 0.60f)
                {
                    report.Issues.Add($"Edge alpha must stay well below center alpha. left={report.LeftEdgeAlpha:0.000}, right={report.RightEdgeAlpha:0.000}, center={report.CenterAlpha:0.000}.");
                }

                if (report.TopLeftCornerAlpha > 0.012f || report.TopRightCornerAlpha > 0.012f || report.BottomLeftCornerAlpha > 0.012f || report.BottomRightCornerAlpha > 0.012f)
                {
                    report.Issues.Add($"Corner alpha must stay near transparent. tl={report.TopLeftCornerAlpha:0.000}, tr={report.TopRightCornerAlpha:0.000}, bl={report.BottomLeftCornerAlpha:0.000}, br={report.BottomRightCornerAlpha:0.000}.");
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(texture);
            }

            return report;
        }

        private static string BuildSurfaceDirectionalShadeTextureCycle20Markdown(SurfaceDirectionalShadeTextureCycle20Report report)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# Fast VS HD2D Surface Directional Shade Texture Cycle 20 Report");
            builder.AppendLine();
            builder.AppendLine("Deterministic v2 texture foundation for the house-slice surface directional shade overlay. This step reduces the risk of the overlay reading as a flat dark rectangle.");
            builder.AppendLine();
            builder.AppendLine($"- Texture PNG: `{Cycle20TexturePath}`");
            builder.AppendLine($"- Report file: `{Cycle20ReportPath}`");
            builder.AppendLine($"- Result: {report.Result}");
            builder.AppendLine();
            builder.AppendLine("| Metric | Value |");
            builder.AppendLine("|---|---:|");
            builder.AppendLine($"| Width | {report.Width} |");
            builder.AppendLine($"| Height | {report.Height} |");
            builder.AppendLine($"| Center alpha | {report.CenterAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Max alpha | {report.MaxAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Left edge alpha | {report.LeftEdgeAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Right edge alpha | {report.RightEdgeAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Top-left interior alpha | {report.TopLeftInteriorAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Lower-right interior alpha | {report.LowerRightInteriorAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Top-left corner alpha | {report.TopLeftCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Top-right corner alpha | {report.TopRightCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Bottom-left corner alpha | {report.BottomLeftCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");
            builder.AppendLine($"| Bottom-right corner alpha | {report.BottomRightCornerAlpha.ToString("0.000", CultureInfo.InvariantCulture)} |");

            if (report.Issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                foreach (var issue in report.Issues)
                {
                    builder.AppendLine($"- {issue}");
                }
            }

            return builder.ToString();
        }

        private static void ValidateProfile(
            List<string> issues,
            string objectName,
            FastVsHouseArea expectedArea,
            FastVsHd2dOverlayKind expectedKind,
            bool currentWorld,
            bool dynamicSubject,
            Vector2 expectedOpacityBand,
            Vector2 expectedFootprintWorldSize,
            Color expectedTint,
            AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole expectedMaterialRole)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                issues.Add($"Missing HD2D overlay profile object: {objectName}");
                return;
            }

            if (sceneObject.scene.path != AnemoraFastVsHouseSliceSetup.ScenePath)
            {
                issues.Add($"HD2D overlay profile {objectName} must live in the house slice scene.");
            }

            var profile = sceneObject.GetComponent<FastVsHd2dOverlayProfile>();
            if (profile == null)
            {
                issues.Add($"HD2D overlay profile {objectName} is missing FastVsHd2dOverlayProfile.");
                return;
            }

            if (!string.Equals(profile.OverlayIdForReview, objectName, StringComparison.Ordinal))
            {
                issues.Add($"HD2D overlay profile {objectName} must keep overlayId '{objectName}', but was '{profile.OverlayIdForReview}'.");
            }

            if (profile.AreaIdForReview != expectedArea)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep area {expectedArea}, but was {profile.AreaIdForReview}.");
            }

            if (profile.OverlayKindForReview != expectedKind)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep overlay kind {expectedKind}, but was {profile.OverlayKindForReview}.");
            }

            if (profile.IsCurrentWorldForReview != currentWorld)
            {
                issues.Add($"HD2D overlay profile {objectName} currentWorld must be {currentWorld}.");
            }

            if (profile.IsDynamicSubjectForReview != dynamicSubject)
            {
                issues.Add($"HD2D overlay profile {objectName} dynamicSubject must be {dynamicSubject}.");
            }

            ValidateVector2(issues, objectName, "opacityBand", profile.OpacityBandForReview, expectedOpacityBand);
            ValidateVector2(issues, objectName, "footprintWorldSize", profile.FootprintWorldSizeForReview, expectedFootprintWorldSize);
            ValidateColor(issues, objectName, "intendedTint", profile.IntendedTintForReview, expectedTint);

            var renderer = sceneObject.GetComponent<MeshRenderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep a MeshRenderer with a material.");
                return;
            }

            if (!renderer.enabled)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep its MeshRenderer enabled.");
            }

            if (renderer.shadowCastingMode != ShadowCastingMode.Off || renderer.receiveShadows)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep shadow casting disabled.");
            }

            if (renderer.sharedMaterial.GetTag(MaterialRoleTagName, false, string.Empty) != expectedMaterialRole.ToString())
            {
                var materialRole = renderer.sharedMaterial.GetTag(MaterialRoleTagName, false, string.Empty);
                issues.Add($"HD2D overlay profile {objectName} must keep material tag {MaterialRoleTagName}={expectedMaterialRole}, but was '{materialRole}'.");
            }

            if (expectedKind == FastVsHd2dOverlayKind.LightPool)
            {
                ValidateLightPoolMaterial(issues, objectName, renderer.sharedMaterial);
            }
        }

        private static void ValidateVector2(List<string> issues, string objectName, string fieldName, Vector2 actual, Vector2 expected)
        {
            if (Vector2.Distance(actual, expected) > VectorTolerance)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep {fieldName} near {expected}, but was {actual}.");
            }
        }

        private static void ValidateColor(List<string> issues, string objectName, string fieldName, Color actual, Color expected)
        {
            if (Mathf.Abs(actual.r - expected.r) > ColorTolerance ||
                Mathf.Abs(actual.g - expected.g) > ColorTolerance ||
                Mathf.Abs(actual.b - expected.b) > ColorTolerance ||
                Mathf.Abs(actual.a - expected.a) > ColorTolerance)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep {fieldName} near {expected}, but was {actual}.");
            }
        }

        private static void ValidateLightPoolMaterial(List<string> issues, string objectName, Material material)
        {
            var materialName = material.name ?? string.Empty;
            if (materialName.IndexOf("hd2d_warm_light_pool", StringComparison.OrdinalIgnoreCase) < 0)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep a warm light pool material name.");
            }

            var texture = ResolvePrimaryMaterialTexture(material);
            if (texture == null || !string.Equals(texture.name, "FastVS_House_hd2d_warm_light_pool_soft", StringComparison.Ordinal))
            {
                issues.Add($"HD2D overlay profile {objectName} must keep the FastVS_House_hd2d_warm_light_pool_soft texture.");
            }
        }

        private static Texture ResolvePrimaryMaterialTexture(Material material)
        {
            if (material == null)
            {
                return null;
            }

            if (material.HasProperty("_BaseMap"))
            {
                var baseMap = material.GetTexture("_BaseMap");
                if (baseMap != null)
                {
                    return baseMap;
                }
            }

            if (material.HasProperty("_MainTex"))
            {
                var mainTex = material.GetTexture("_MainTex");
                if (mainTex != null)
                {
                    return mainTex;
                }
            }

            return material.mainTexture;
        }

        private static GameObject FindSceneObjectIncludingInactive(string objectName)
        {
            var active = GameObject.Find(objectName);
            if (active != null)
            {
                return active;
            }

            foreach (var gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (gameObject == null ||
                    !string.Equals(gameObject.name, objectName, StringComparison.Ordinal) ||
                    !gameObject.scene.IsValid() ||
                    gameObject.scene.path != AnemoraFastVsHouseSliceSetup.ScenePath)
                {
                    continue;
                }

                return gameObject;
            }

            return null;
        }

        private sealed class SurfaceDirectionalShadeTextureCycle20Report
        {
            public int Width;
            public int Height;
            public float CenterAlpha;
            public float MaxAlpha;
            public float LeftEdgeAlpha;
            public float RightEdgeAlpha;
            public float TopLeftInteriorAlpha;
            public float LowerRightInteriorAlpha;
            public float TopLeftCornerAlpha;
            public float TopRightCornerAlpha;
            public float BottomLeftCornerAlpha;
            public float BottomRightCornerAlpha;
            public List<string> Issues = new List<string>();
            public string Result => Issues.Count == 0 ? "PASS" : "FAIL";
        }

        private sealed class StaticDirectionalShadowTextureCycle21Report
        {
            public int Width;
            public int Height;
            public float CenterAlpha;
            public float MaxAlpha;
            public float LeftEdgeAlpha;
            public float RightEdgeAlpha;
            public float CoreAlpha;
            public float TailAlpha;
            public float TopLeftCornerAlpha;
            public float TopRightCornerAlpha;
            public float BottomLeftCornerAlpha;
            public float BottomRightCornerAlpha;
            public List<string> Issues = new List<string>();
            public string Result => Issues.Count == 0 ? "PASS" : "FAIL";
        }

        private sealed class CharacterContactShadowTextureCycle22Report
        {
            public int Width;
            public int Height;
            public float CenterAlpha;
            public float MaxAlpha;
            public float LeftEdgeAlpha;
            public float RightEdgeAlpha;
            public float LeftFootAlpha;
            public float RightFootAlpha;
            public float TopEdgeAlpha;
            public float BottomEdgeAlpha;
            public float TopLeftCornerAlpha;
            public float TopRightCornerAlpha;
            public float BottomLeftCornerAlpha;
            public float BottomRightCornerAlpha;
            public List<string> Issues = new List<string>();
            public string Result => Issues.Count == 0 ? "PASS" : "FAIL";
        }
    }
}
