using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2SmokeSteamProfilePath = "Assets/Settings/FastVS_HD2D_P2_SmokeSteamProfile.asset";
        private const string Hd2dAutonomousP2SmokeSteamProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dSmokeSteamProfile.cs";
        private const string Hd2dAutonomousP2SmokeSteamColumnRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dSmokeSteamColumn.cs";
        private const string Hd2dAutonomousP2SmokeSteamExteriorRootName = "Current_Exterior_P2_67_SmokeSteamColumns";
        private const string Hd2dAutonomousP2SmokeSteamPlazaRootName = "Current_CentralPlaza_P2_67_SmokeSteamColumns";
        private const string Hd2dAutonomousP2SmokeSteamMainChimneyName = "P2_67_ChimneySmoke_MainColumn";
        private const string Hd2dAutonomousP2SmokeSteamRoofVentName = "P2_67_ChimneySmoke_RoofVentColumn";
        private const string Hd2dAutonomousP2SmokeSteamCookfireName = "P2_67_CookfireSmoke_PlazaColumn";
        private const string Hd2dAutonomousP2SmokeSteamSteamName = "P2_67_AmbientSteamSmoke_GrateColumn";
        private const string Hd2dAutonomousP2SmokeSteamMaterialId = "hd2d_p2_smoke_steam_particle";
        private const string Hd2dAutonomousP2SmokeSteamTextureId = "hd2d_p2_smoke_steam_particle_soft";
        private const string Hd2dAutonomousP2SmokeSteamMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2SmokeSteamMaterialId + ".mat";
        private const string Hd2dAutonomousP2SmokeSteamTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP2SmokeSteamTextureId + ".asset";

        public static void CaptureHd2dAutonomousP2Item67SmokeSteamBatch()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2SmokeSteamExteriorRootName) == null ||
                FindSceneObjectIncludingInactive(Hd2dAutonomousP2SmokeSteamPlazaRootName) == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-67 smoke/steam capture failed: review roots are missing. Run BuildAndValidateBatch before capture.");
            }

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientVfxDirector>(FindObjectsInactive.Include);
            var profile = EnsureHd2dAutonomousP2SmokeSteamProfile();
            if (controller == null || visibility == null || guide == null || camera == null || director == null || profile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-67 smoke/steam capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2SmokeSteamColumns();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("chimney_cookfire_smoke_steam_columns");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_smoke_steam_disabled_town_control.png",
                "02_conservative_chimney_smoke_frame_a.png",
                "03_conservative_chimney_smoke_frame_b.png",
                "04_cookfire_smoke_and_steam_columns.png",
                "05_stronger_density_option_for_tom.png",
                "06_roof_edge_soft_intersection_fade.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                controller.ClosePortal();
                SetHd2dAutonomousP2FootstepFxReviewSurfacesVisible(false);
                HideHd2dAutonomousP2WaterReviewSetsForFogCapture();
                SetHd2dAutonomousP2LocalVolumetricFogVisible(false);

                SetHd2dAutonomousP2SmokeSteamVisible(false);
                SetHd2dAutonomousP2SmokeSteamAllMultipliers(1f);
                CaptureHd2dAutonomousP2SmokeSteamExteriorShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    director,
                    profile,
                    outputDirectory,
                    screenshotFiles[0],
                    "control: P2-67 chimney/cookfire/steam columns disabled",
                    0.10f,
                    true,
                    shotRows);

                SetHd2dAutonomousP2SmokeSteamVisible(true);
                SetHd2dAutonomousP2SmokeSteamAllMultipliers(1f);
                CaptureHd2dAutonomousP2SmokeSteamExteriorShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    director,
                    profile,
                    outputDirectory,
                    screenshotFiles[1],
                    "conservative chimney smoke frame A: soft rise with shared east/north wind lean",
                    4.80f,
                    true,
                    shotRows);

                CaptureHd2dAutonomousP2SmokeSteamExteriorShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    director,
                    profile,
                    outputDirectory,
                    screenshotFiles[2],
                    "conservative chimney smoke frame B: same wind direction after additional drift",
                    3.20f,
                    false,
                    shotRows);

                CaptureHd2dAutonomousP2SmokeSteamPlazaShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    director,
                    profile,
                    outputDirectory,
                    screenshotFiles[3],
                    "central plaza cookfire smoke plus ambient steam column",
                    4.20f,
                    true,
                    shotRows);

                SetHd2dAutonomousP2SmokeSteamAllMultipliers(profile.StrongerOptionMultiplierForReview);
                CaptureHd2dAutonomousP2SmokeSteamExteriorShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    director,
                    profile,
                    outputDirectory,
                    screenshotFiles[4],
                    "stronger density/alpha option for Tom only",
                    4.80f,
                    true,
                    shotRows);

                SetHd2dAutonomousP2SmokeSteamAllMultipliers(1f);
                CaptureHd2dAutonomousP2SmokeSteamRoofEdgeShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    director,
                    profile,
                    outputDirectory,
                    screenshotFiles[5],
                    "roof-edge close-up: soft-particle material and lifetime alpha fade avoid hard sprite seams",
                    5.35f,
                    true,
                    shotRows);
            }
            finally
            {
                SetHd2dAutonomousP2SmokeSteamVisible(true);
                SetHd2dAutonomousP2SmokeSteamAllMultipliers(1f);
                SetHd2dAutonomousP2LocalVolumetricFogVisible(true);
                RestoreHd2dAutonomousP2WaterReviewSetsAfterFogCapture();
                SetHd2dAutonomousP2FootstepFxReviewSurfacesVisible(true);
                director.ClearReviewStateForReview();
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                AssetDatabase.SaveAssets();
            }

            var enableDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var driftDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var strongerDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[4]);
            WriteHd2dAutonomousP2SmokeSteamReviewReport(outputDirectory, screenshotFiles, shotRows, profile, enableDiff, driftDiff, strongerDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-67 smoke/steam review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2SmokeSteamColumns(Transform currentExteriorRoot, Transform currentCentralPlazaRoot)
        {
            var profile = EnsureHd2dAutonomousP2SmokeSteamProfile();
            var material = EnsureHd2dAutonomousP2SmokeSteamMaterial();
            DestroyHd2dAutonomousP2SmokeSteamRoot(Hd2dAutonomousP2SmokeSteamExteriorRootName);
            DestroyHd2dAutonomousP2SmokeSteamRoot(Hd2dAutonomousP2SmokeSteamPlazaRootName);

            if (currentExteriorRoot != null)
            {
                var exteriorRoot = new GameObject(Hd2dAutonomousP2SmokeSteamExteriorRootName);
                exteriorRoot.transform.SetParent(currentExteriorRoot, false);
                exteriorRoot.transform.localPosition = Vector3.zero;
                exteriorRoot.transform.localRotation = Quaternion.identity;
                exteriorRoot.transform.localScale = Vector3.one;
                SetHd2dAutonomousP2SmokeSteamLayerRecursively(exteriorRoot, CurrentSpaceRenderLayer);

                CreateHd2dAutonomousP2SmokeSteamColumn(
                    exteriorRoot.transform,
                    Hd2dAutonomousP2SmokeSteamMainChimneyName,
                    FastVsHd2dSmokeSteamKind.ChimneySmoke,
                    HouseExteriorCenter + new Vector3(0.66f, 3.44f, 0.08f),
                    0.075f,
                    8f,
                    profile,
                    material);

                CreateHd2dAutonomousP2SmokeSteamColumn(
                    exteriorRoot.transform,
                    Hd2dAutonomousP2SmokeSteamRoofVentName,
                    FastVsHd2dSmokeSteamKind.ChimneySmoke,
                    HouseExteriorCenter + new Vector3(-2.34f, 2.58f, -1.22f),
                    0.060f,
                    9f,
                    profile,
                    material);
            }

            if (currentCentralPlazaRoot != null)
            {
                var plazaRoot = new GameObject(Hd2dAutonomousP2SmokeSteamPlazaRootName);
                plazaRoot.transform.SetParent(currentCentralPlazaRoot, false);
                plazaRoot.transform.localPosition = Vector3.zero;
                plazaRoot.transform.localRotation = Quaternion.identity;
                plazaRoot.transform.localScale = Vector3.one;
                SetHd2dAutonomousP2SmokeSteamLayerRecursively(plazaRoot, CurrentSpaceRenderLayer);

                CreateHd2dAutonomousP2SmokeSteamSourcePlate(
                    plazaRoot.transform,
                    "P2_67_CookfireSmoke_SourcePlate",
                    CentralPlazaVsCenter + new Vector3(-1.42f, 0.055f, 3.18f),
                    new Vector3(0.42f, 0.035f, 0.42f),
                    FlatMaterial("hd2d_p2_smoke_steam_source_charcoal", new Color(0.16f, 0.13f, 0.11f, 1f), false));

                CreateHd2dAutonomousP2SmokeSteamColumn(
                    plazaRoot.transform,
                    Hd2dAutonomousP2SmokeSteamCookfireName,
                    FastVsHd2dSmokeSteamKind.CookfireSmoke,
                    CentralPlazaVsCenter + new Vector3(-1.42f, 0.34f, 3.18f),
                    0.090f,
                    10f,
                    profile,
                    material);

                CreateHd2dAutonomousP2SmokeSteamSourcePlate(
                    plazaRoot.transform,
                    "P2_67_AmbientSteamSmoke_GratePlate",
                    CentralPlazaVsCenter + new Vector3(0.92f, 0.055f, 3.42f),
                    new Vector3(0.48f, 0.030f, 0.32f),
                    FlatMaterial("hd2d_p2_smoke_steam_source_grate", new Color(0.22f, 0.25f, 0.26f, 1f), false));

                CreateHd2dAutonomousP2SmokeSteamColumn(
                    plazaRoot.transform,
                    Hd2dAutonomousP2SmokeSteamSteamName,
                    FastVsHd2dSmokeSteamKind.SteamColumn,
                    CentralPlazaVsCenter + new Vector3(0.92f, 0.32f, 3.42f),
                    0.070f,
                    7f,
                    profile,
                    material);
            }

            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2SmokeSteamColumns()
        {
            var profile = EnsureHd2dAutonomousP2SmokeSteamProfile();
            var material = EnsureHd2dAutonomousP2SmokeSteamMaterial();
            if (profile == null ||
                material == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalSmokeSteamApprovedForReview ||
                !profile.PersistentLoopingShurikenForReview ||
                !profile.SharedAmbientVfxWindForReview ||
                !profile.SoftParticlesRequiredForReview ||
                !profile.DistanceCullFarColumnsForReview ||
                !profile.ConservativeDataPrepForReview ||
                profile.MaxParticlesPerColumnForReview > 40 ||
                profile.SmokeLifetimeForReview > 6.2f ||
                profile.StrongerOptionMultiplierForReview <= 1f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-67 needs a conservative non-final smoke/steam profile with looping Shuriken, shared wind, soft particles, and Tom approval left open.");
            }

            if (!material.IsKeywordEnabled("_SOFTPARTICLES_ON") ||
                (material.HasProperty("_SoftParticlesEnabled") && material.GetFloat("_SoftParticlesEnabled") < 0.5f) ||
                (material.HasProperty("_SoftParticlesFarFadeDistance") && material.GetFloat("_SoftParticlesFarFadeDistance") < 0.35f))
            {
                throw new InvalidOperationException("House slice validation failed: P2-67 smoke/steam material must keep URP soft particles enabled for roof-edge intersection data.");
            }

            var chimneyCount = CountHd2dAutonomousP2SmokeSteamColumns(FastVsHd2dSmokeSteamKind.ChimneySmoke);
            var cookfireCount = CountHd2dAutonomousP2SmokeSteamColumns(FastVsHd2dSmokeSteamKind.CookfireSmoke);
            var steamCount = CountHd2dAutonomousP2SmokeSteamColumns(FastVsHd2dSmokeSteamKind.SteamColumn);
            if (chimneyCount < 2 || cookfireCount < 1 || steamCount < 1)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-67 requires at least two chimney smoke columns, one cookfire smoke column, and one steam column. Counts chimney/cookfire/steam={chimneyCount}/{cookfireCount}/{steamCount}.");
            }

            SetHd2dAutonomousP2SmokeSteamVisible(true);
            SetHd2dAutonomousP2SmokeSteamAllMultipliers(1f);
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientVfxDirector>(FindObjectsInactive.Include);
            if (director == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-67 smoke/steam columns must remain driven by the P1-48 shared ambient VFX director.");
            }

            director.ApplyReviewStateForReview(Hd2dAutonomousP1AmbientVfxCentralZoneId, SunPreset.Noon, profile.SharedWindDirectionForReview, profile.ReviewWindStrengthForReview, 0.18f, 0.30f);
            director.SimulateForReview(0.20f, true);

            foreach (var column in FindHd2dAutonomousP2SmokeSteamColumns())
            {
                ValidateHd2dAutonomousP2SmokeSteamColumn(column, profile, material);
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2SmokeSteamProfileRuntimePath), "needsTomApproval", Hd2dAutonomousP2SmokeSteamProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2SmokeSteamProfileRuntimePath), "finalSmokeSteamApproved", Hd2dAutonomousP2SmokeSteamProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2SmokeSteamColumnRuntimePath), "DistanceCullFarMetersForReview", Hd2dAutonomousP2SmokeSteamColumnRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2SmokeSteamColumnRuntimePath), "SetReviewRateMultiplierForReview", Hd2dAutonomousP2SmokeSteamColumnRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2SmokeSteamColumns", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2SmokeSteamColumns", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.P2SmokeSteam.cs"), "new GradientAlphaKey(0f, 1f)", "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2SmokeSteam.cs");
        }

        private static FastVsHd2dSmokeSteamProfile EnsureHd2dAutonomousP2SmokeSteamProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dSmokeSteamProfile>(Hd2dAutonomousP2SmokeSteamProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dSmokeSteamProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2SmokeSteamProfilePath);
            }

            profile.ConfigureForReview(
                36,
                7.6f,
                9.2f,
                7.0f,
                5.2f,
                3.4f,
                0.30f,
                0.22f,
                2.65f,
                0.42f,
                0.54f,
                new Vector3(0.86f, 0f, 0.50f),
                0.78f,
                0.18f,
                0.17f,
                46f,
                1.35f,
                new Color(0.62f, 0.67f, 0.72f, 0.30f),
                new Color(0.54f, 0.49f, 0.42f, 0.32f),
                new Color(0.74f, 0.84f, 0.92f, 0.24f),
                true,
                true,
                true,
                true,
                true,
                true,
                false,
                "Keep the conservative soft-particle smoke/steam columns as data prep. Pass 2 raises readability only; Tom should tune final density, alpha, height, and roof-edge softness against the approved camera and color grade.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Material EnsureHd2dAutonomousP2SmokeSteamMaterial()
        {
            EnsureFolder(MaterialDirectory);
            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2SmokeSteamMaterialPath);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find("Universal Render Pipeline/Unlit");
                if (shader == null)
                {
                    throw new InvalidOperationException("P2-67 smoke/steam particle shader not found.");
                }

                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP2SmokeSteamMaterialPath);
            }

            ConfigureTransparentParticleMaterial(material, 3036);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP2SmokeSteamTexture(), Vector2.one);
            ApplyMaterialRole(material, Hd2dAutonomousP2SmokeSteamMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            material.doubleSidedGI = true;
            material.enableInstancing = true;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(1f, 1f, 1f, 0.82f));
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", new Color(1f, 1f, 1f, 0.82f));
            }

            if (material.HasProperty("_SoftParticlesEnabled"))
            {
                material.SetFloat("_SoftParticlesEnabled", 1f);
            }

            if (material.HasProperty("_SoftParticlesNearFadeDistance"))
            {
                material.SetFloat("_SoftParticlesNearFadeDistance", 0f);
            }

            if (material.HasProperty("_SoftParticlesFarFadeDistance"))
            {
                material.SetFloat("_SoftParticlesFarFadeDistance", 0.65f);
            }

            material.EnableKeyword("_SOFTPARTICLES_ON");
            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);
            material.SetShaderPassEnabled("ShadowCaster", false);
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP2SmokeSteamTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2SmokeSteamTextureId,
                96,
                96,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = ((x + 0.5f) / 96f) * 2f - 1f;
                    var v = ((y + 0.5f) / 96f) * 2f - 1f;
                    var coreDistance = Mathf.Sqrt((u * u * 0.86f) + (v * v * 1.08f));
                    var sideDistance = Mathf.Sqrt(((u + 0.24f) * (u + 0.24f) * 1.30f) + ((v - 0.10f) * (v - 0.10f) * 1.10f));
                    var topDistance = Mathf.Sqrt(((u - 0.18f) * (u - 0.18f) * 1.18f) + ((v + 0.24f) * (v + 0.24f) * 1.36f));
                    var core = Mathf.Clamp01(1f - coreDistance);
                    var side = Mathf.Clamp01(1f - sideDistance);
                    var top = Mathf.Clamp01(1f - topDistance);
                    var feather = Mathf.Clamp01((core * 0.58f) + (side * 0.24f) + (top * 0.22f));
                    var grain = 0.92f + (Mathf.Sin((x * 12.9898f + y * 78.233f) * 0.10f) * 0.04f);
                    var alpha = Mathf.Clamp01(feather * feather * 0.78f * grain);
                    return new Color(0.94f, 0.96f, 0.98f, alpha);
                });
        }

        private static void CreateHd2dAutonomousP2SmokeSteamColumn(
            Transform parent,
            string objectName,
            FastVsHd2dSmokeSteamKind kind,
            Vector3 localPosition,
            float radius,
            float coneAngle,
            FastVsHd2dSmokeSteamProfile profile,
            Material material)
        {
            var columnObject = new GameObject(objectName);
            columnObject.transform.SetParent(parent, false);
            columnObject.transform.localPosition = localPosition;
            columnObject.transform.localRotation = Quaternion.identity;
            columnObject.transform.localScale = Vector3.one;
            columnObject.layer = CurrentSpaceRenderLayer;

            var renderer = columnObject.AddComponent<ParticleSystemRenderer>();
            var system = columnObject.AddComponent<ParticleSystem>();
            ConfigureHd2dAutonomousP2SmokeSteamParticleSystem(system, renderer, kind, radius, coneAngle, profile, material);

            var column = columnObject.AddComponent<FastVsHd2dSmokeSteamColumn>();
            column.ConfigureForReview(profile, kind, system, renderer, profile.SharedAmbientVfxWindForReview, profile.DistanceCullFarColumnsForReview);
            EditorUtility.SetDirty(columnObject);
            EditorUtility.SetDirty(system);
            EditorUtility.SetDirty(renderer);
            EditorUtility.SetDirty(column);
        }

        private static void ConfigureHd2dAutonomousP2SmokeSteamParticleSystem(
            ParticleSystem system,
            ParticleSystemRenderer renderer,
            FastVsHd2dSmokeSteamKind kind,
            float radius,
            float coneAngle,
            FastVsHd2dSmokeSteamProfile profile,
            Material material)
        {
            var lifetime = profile.ResolveLifetimeForReview(kind);
            var startSize = profile.ResolveStartSizeForReview(kind);
            var color = profile.ResolveColorForReview(kind);
            var main = system.main;
            main.loop = true;
            main.prewarm = true;
            main.playOnAwake = true;
            main.duration = Mathf.Max(3.2f, lifetime);
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime * 0.86f, lifetime * 1.08f);
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(startSize * 0.72f, startSize * 1.18f);
            main.startRotation = new ParticleSystem.MinMaxCurve(-0.28f, 0.28f);
            main.startColor = new ParticleSystem.MinMaxGradient(color);
            main.maxParticles = profile.MaxParticlesPerColumnForReview;
            main.gravityModifier = -0.015f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(profile.ResolveEmissionRateForReview(kind));
            emission.SetBursts(Array.Empty<ParticleSystem.Burst>());

            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.radius = radius;
            shape.angle = coneAngle;
            shape.radiusThickness = 0.72f;
            shape.arc = 360f;

            var velocity = system.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            var windVelocityX = profile.SharedWindDirectionForReview.x * profile.ReviewWindStrengthForReview * 0.20f;
            var windVelocityZ = profile.SharedWindDirectionForReview.z * profile.ReviewWindStrengthForReview * 0.20f;
            velocity.y = new ParticleSystem.MinMaxCurve(profile.ResolveRiseVelocityForReview(kind) * 0.86f, profile.ResolveRiseVelocityForReview(kind) * 1.14f);
            velocity.x = new ParticleSystem.MinMaxCurve(windVelocityX * 0.96f, windVelocityX * 1.04f);
            velocity.z = new ParticleSystem.MinMaxCurve(windVelocityZ * 0.96f, windVelocityZ * 1.04f);

            var colorOverLifetime = system.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 0.74f),
                    new GradientColorKey(Color.white, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(0.90f, 0.16f),
                    new GradientAlphaKey(0.48f, 0.68f),
                    new GradientAlphaKey(0f, 1f)
                });
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

            var size = system.sizeOverLifetime;
            size.enabled = true;
            var sizeCurve = new AnimationCurve(
                new Keyframe(0f, 0.42f),
                new Keyframe(0.28f, 1.0f),
                new Keyframe(1f, profile.SizeEndMultiplierForReview));
            size.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

            var noise = system.noise;
            noise.enabled = true;
            noise.strength = new ParticleSystem.MinMaxCurve(profile.NoiseStrengthForReview);
            noise.frequency = profile.NoiseFrequencyForReview;
            noise.scrollSpeed = 0.18f;
            noise.octaveCount = 2;
            noise.damping = true;

            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.sortingOrder = 6;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.forceRenderingOff = false;
            ForceRendererEnabledForReview(renderer);

            system.Clear(true);
            system.Play(true);
        }

        private static void CreateHd2dAutonomousP2SmokeSteamSourcePlate(Transform parent, string objectName, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var plate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plate.name = objectName;
            plate.transform.SetParent(parent, false);
            plate.transform.localPosition = localPosition;
            plate.transform.localRotation = Quaternion.identity;
            plate.transform.localScale = localScale;
            plate.layer = CurrentSpaceRenderLayer;
            var collider = plate.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = plate.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
        }

        private static void ValidateHd2dAutonomousP2SmokeSteamColumn(FastVsHd2dSmokeSteamColumn column, FastVsHd2dSmokeSteamProfile profile, Material expectedMaterial)
        {
            var system = column != null ? column.ParticleSystemForReview : null;
            var renderer = column != null ? column.ParticleRendererForReview : null;
            if (column == null || system == null || renderer == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-67 smoke/steam column is missing runtime component, ParticleSystem, or renderer.");
            }

            var main = system.main;
            var emission = system.emission;
            var shape = system.shape;
            var velocity = system.velocityOverLifetime;
            var color = system.colorOverLifetime;
            var size = system.sizeOverLifetime;
            var noise = system.noise;
            var windX = ReadHd2dAutonomousP2SmokeSteamVelocity(velocity.x);
            var windZ = ReadHd2dAutonomousP2SmokeSteamVelocity(velocity.z);
            var lifetimeMax = ReadHd2dAutonomousP2SmokeSteamCurveMax(main.startLifetime);
            if (column.ProfileForReview != profile ||
                !column.DrivenBySharedAmbientVfxDirectorForReview ||
                !column.DistanceCullEnabledForReview ||
                column.gameObject.layer != CurrentSpaceRenderLayer ||
                !main.loop ||
                !main.playOnAwake ||
                main.simulationSpace != ParticleSystemSimulationSpace.World ||
                main.maxParticles > profile.MaxParticlesPerColumnForReview ||
                lifetimeMax > 6.8f ||
                !emission.enabled ||
                emission.rateOverTime.constant <= 0f ||
                !shape.enabled ||
                shape.shapeType != ParticleSystemShapeType.Cone ||
                !velocity.enabled ||
                windX <= 0f ||
                windZ <= 0f ||
                !color.enabled ||
                !size.enabled ||
                !noise.enabled ||
                renderer.sharedMaterial != expectedMaterial ||
                renderer.forceRenderingOff ||
                renderer.shadowCastingMode != ShadowCastingMode.Off ||
                renderer.receiveShadows)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P2-67 columns must be looping World-space cone Shuriken systems with shared positive wind lean, alpha/size fade, soft material, and no shadows. " +
                    $"{column.name}: kind={column.ColumnKindForReview}, loop={main.loop}, play={main.playOnAwake}, sim={main.simulationSpace}, max={main.maxParticles}/{profile.MaxParticlesPerColumnForReview}, lifetimeMax={lifetimeMax:0.###}, emission={emission.rateOverTime.constant:0.###}, wind={windX:0.###}/{windZ:0.###}, material={(renderer.sharedMaterial != null ? renderer.sharedMaterial.name : "null")}.");
            }
        }

        private static void CaptureHd2dAutonomousP2SmokeSteamExteriorShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHd2dAmbientVfxDirector director,
            FastVsHd2dSmokeSteamProfile profile,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            bool restart,
            ICollection<string> shotRows)
        {
            CaptureHd2dAutonomousP2SmokeSteamShot(
                controller,
                visibility,
                guide,
                camera,
                director,
                profile,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(-0.62f, 2.80f, -0.92f),
                new Vector3(2.15f, 3.05f, -6.75f),
                new Vector3(0.08f, 0.28f, 0.04f),
                34f,
                outputDirectory,
                fileName,
                label,
                simulateSeconds,
                restart,
                shotRows);
        }

        private static void CaptureHd2dAutonomousP2SmokeSteamPlazaShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHd2dAmbientVfxDirector director,
            FastVsHd2dSmokeSteamProfile profile,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            bool restart,
            ICollection<string> shotRows)
        {
            CaptureHd2dAutonomousP2SmokeSteamShot(
                controller,
                visibility,
                guide,
                camera,
                director,
                profile,
                FastVsHouseArea.CentralPlaza,
                CentralPlazaVsCenter + new Vector3(-0.30f, 1.18f, 3.35f),
                new Vector3(1.78f, 2.18f, -4.70f),
                new Vector3(0.02f, 0.16f, 0.10f),
                35f,
                outputDirectory,
                fileName,
                label,
                simulateSeconds,
                restart,
                shotRows);
        }

        private static void CaptureHd2dAutonomousP2SmokeSteamRoofEdgeShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHd2dAmbientVfxDirector director,
            FastVsHd2dSmokeSteamProfile profile,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            bool restart,
            ICollection<string> shotRows)
        {
            CaptureHd2dAutonomousP2SmokeSteamShot(
                controller,
                visibility,
                guide,
                camera,
                director,
                profile,
                FastVsHouseArea.Exterior,
                HouseExteriorCenter + new Vector3(0.54f, 3.03f, -0.08f),
                new Vector3(1.10f, 1.08f, -2.56f),
                new Vector3(0.00f, 0.18f, 0.10f),
                30f,
                outputDirectory,
                fileName,
                label,
                simulateSeconds,
                restart,
                shotRows);
        }

        private static void CaptureHd2dAutonomousP2SmokeSteamShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            FastVsHd2dAmbientVfxDirector director,
            FastVsHd2dSmokeSteamProfile profile,
            FastVsHouseArea activeArea,
            Vector3 anchorLocal,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            bool restart,
            ICollection<string> shotRows)
        {
            visibility.SetActiveAreaForReview(activeArea);
            controller.ForcePlayerCurrentLocalForReview(activeArea == FastVsHouseArea.Exterior
                ? HouseExteriorCenter + new Vector3(-1.08f, 0.02f, -2.02f)
                : CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
            controller.ClosePortal();
            guide.ApplyActiveTimeIsolationForReview();
            director.ApplyReviewStateForReview(Hd2dAutonomousP1AmbientVfxCentralZoneId, SunPreset.Noon, profile.SharedWindDirectionForReview, profile.ReviewWindStrengthForReview, 0.18f, 0.30f);
            SimulateHd2dAutonomousP2SmokeSteamColumns(simulateSeconds, restart);

            var previousMask = camera.cullingMask;
            var previousFov = camera.fieldOfView;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            try
            {
                camera.cullingMask = currentBit | playerBit;
                camera.fieldOfView = fieldOfView;
                PositionCloseReviewCamera(
                    camera,
                    controller.CurrentSpaceRootForReview.TransformPoint(anchorLocal),
                    cameraOffset,
                    lookOffset);
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                camera.fieldOfView = previousFov;
                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | {activeArea} | {simulateSeconds:0.###} | {CountHd2dAutonomousP2SmokeSteamLiveParticles()} | {FormatVector3ForReport(profile.SharedWindDirectionForReview * profile.ReviewWindStrengthForReview)} |");
        }

        private static void SimulateHd2dAutonomousP2SmokeSteamColumns(float seconds, bool restart)
        {
            foreach (var column in FindHd2dAutonomousP2SmokeSteamColumns())
            {
                column?.SimulateForReview(seconds, restart);
            }
        }

        private static void SetHd2dAutonomousP2SmokeSteamVisible(bool visible)
        {
            foreach (var column in FindHd2dAutonomousP2SmokeSteamColumns())
            {
                if (column == null)
                {
                    continue;
                }

                column.SetReviewVisibleForReview(visible);
                EditorUtility.SetDirty(column);
            }
        }

        private static void SetHd2dAutonomousP2SmokeSteamAllMultipliers(float multiplier)
        {
            foreach (var column in FindHd2dAutonomousP2SmokeSteamColumns())
            {
                if (column == null)
                {
                    continue;
                }

                column.SetReviewRateMultiplierForReview(multiplier);
                column.SetReviewAlphaMultiplierForReview(multiplier);
                EditorUtility.SetDirty(column);
            }
        }

        private static FastVsHd2dSmokeSteamColumn[] FindHd2dAutonomousP2SmokeSteamColumns()
        {
            return UnityEngine.Object.FindObjectsByType<FastVsHd2dSmokeSteamColumn>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        private static int CountHd2dAutonomousP2SmokeSteamColumns(FastVsHd2dSmokeSteamKind kind)
        {
            var count = 0;
            foreach (var column in FindHd2dAutonomousP2SmokeSteamColumns())
            {
                if (column != null && column.ColumnKindForReview == kind)
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountHd2dAutonomousP2SmokeSteamLiveParticles()
        {
            var count = 0;
            foreach (var column in FindHd2dAutonomousP2SmokeSteamColumns())
            {
                if (column != null)
                {
                    count += column.LiveParticleCountForReview;
                }
            }

            return count;
        }

        private static float ReadHd2dAutonomousP2SmokeSteamVelocity(ParticleSystem.MinMaxCurve curve)
        {
            return curve.mode switch
            {
                ParticleSystemCurveMode.TwoConstants => Mathf.Abs(curve.constantMax) >= Mathf.Abs(curve.constantMin) ? curve.constantMax : curve.constantMin,
                ParticleSystemCurveMode.Constant => curve.constant,
                _ => curve.constantMax
            };
        }

        private static float ReadHd2dAutonomousP2SmokeSteamCurveMax(ParticleSystem.MinMaxCurve curve)
        {
            return curve.mode switch
            {
                ParticleSystemCurveMode.TwoConstants => Mathf.Max(curve.constantMin, curve.constantMax),
                ParticleSystemCurveMode.Constant => curve.constant,
                _ => curve.constantMax
            };
        }

        private static void DestroyHd2dAutonomousP2SmokeSteamRoot(string rootName)
        {
            var root = FindSceneObjectIncludingInactive(rootName);
            if (root != null)
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void SetHd2dAutonomousP2SmokeSteamLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                if (child != null)
                {
                    SetHd2dAutonomousP2SmokeSteamLayerRecursively(child.gameObject, layer);
                }
            }
        }

        private static void WriteHd2dAutonomousP2SmokeSteamReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dSmokeSteamProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics enableDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics driftDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics strongerDiff)
        {
            var lines = new List<string>
            {
                "# P2-67 Chimney / Cookfire Smoke And Ambient Steam Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative looping Shuriken smoke/steam columns for chimney, cookfire, and grate/steam sources.",
                "- Implementation note: this pass uses self-authored generated smoke sprites and the existing P1-48 Ambient VFX Director shared wind; no external VFX asset was imported.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2SmokeSteamProfilePath}` |",
                $"| Runtime column | `{Hd2dAutonomousP2SmokeSteamColumnRuntimePath}` |",
                $"| Material / texture | `{Hd2dAutonomousP2SmokeSteamMaterialPath}` / `{Hd2dAutonomousP2SmokeSteamTexturePath}` |",
                $"| Needs Tom approval | {FormatBool(profile.NeedsTomApprovalForReview)} |",
                $"| Final approved | {FormatBool(profile.FinalSmokeSteamApprovedForReview)} |",
                $"| Shared wind direction / strength | {FormatVector3ForReport(profile.SharedWindDirectionForReview)} / {profile.ReviewWindStrengthForReview:0.###} |",
                $"| Max particles per column | {profile.MaxParticlesPerColumnForReview} |",
                $"| Emission chimney / cookfire / steam | {profile.ChimneyEmissionRateForReview:0.###} / {profile.CookfireEmissionRateForReview:0.###} / {profile.SteamEmissionRateForReview:0.###} |",
                $"| Lifetime smoke / steam | {profile.SmokeLifetimeForReview:0.###}s / {profile.SteamLifetimeForReview:0.###}s |",
                $"| Start size smoke / steam / end multiplier | {profile.SmokeStartSizeForReview:0.###} / {profile.SteamStartSizeForReview:0.###} / {profile.SizeEndMultiplierForReview:0.###} |",
                $"| Color chimney / cookfire / steam | {FormatColor(profile.ChimneySmokeColorForReview)} / {FormatColor(profile.CookfireSmokeColorForReview)} / {FormatColor(profile.SteamColorForReview)} |",
                $"| Soft particles / distance cull | {FormatBool(profile.SoftParticlesRequiredForReview)} / {profile.DistanceCullFarMetersForReview:0.###}m |",
                $"| Column counts chimney / cookfire / steam | {CountHd2dAutonomousP2SmokeSteamColumns(FastVsHd2dSmokeSteamKind.ChimneySmoke)} / {CountHd2dAutonomousP2SmokeSteamColumns(FastVsHd2dSmokeSteamKind.CookfireSmoke)} / {CountHd2dAutonomousP2SmokeSteamColumns(FastVsHd2dSmokeSteamKind.SteamColumn)} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                enableDiff.ToReportRow("Smoke disabled control vs conservative chimney smoke"),
                driftDiff.ToReportRow("Conservative frame A vs frame B drift"),
                strongerDiff.ToReportRow("Conservative vs stronger option"),
                string.Empty,
                "| Screenshot | Label | Area | Sim seconds | Live particles | Wind vector |",
                "|---|---|---|---:|---:|---|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|"
            });

            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                var file = screenshotFiles[i];
                ValidateScreenshotOutputExists(outputDirectory, file);
                lines.Add($"| `{file}` | P2-67 smoke/steam capture {i + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "chimney_cookfire_smoke_steam_columns_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }
    }
}
