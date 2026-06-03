using System;
using System.Collections.Generic;
using System.IO;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2CausticsRootName = "Past_CentralPlaza_P2_CausticsReview";
        private const string Hd2dAutonomousP2CausticsProfilePath = "Assets/Settings/FastVS_HD2D_P2_CausticsProfile.asset";
        private const string Hd2dAutonomousP2CausticsProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dCausticsProfile.cs";
        private const string Hd2dAutonomousP2CausticsShaderPath = "Assets/Art/Shaders/FastVS/FastVS_CausticsDecal.shader";
        private const string Hd2dAutonomousP2CausticsShaderName = "Anemora/FastVS/CausticsDecal";
        private const string Hd2dAutonomousP2CausticsMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p2_caustics_decal.mat";

        public static void CaptureHd2dAutonomousP2Item63CausticsBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            var causticsMaterial = EnsureHd2dAutonomousP2CausticsMaterial();
            if (controller == null || visibility == null || guide == null || camera == null || waterMaterial == null || causticsMaterial == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-63 caustics capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2Caustics();
            var profile = EnsureHd2dAutonomousP2CausticsProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("stylized_caustics_review");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_caustics_disabled_baseline.png",
                "02_caustics_enabled_frame_a.png",
                "03_caustics_enabled_frame_b_0p55s.png",
                "04_edge_feather_closeup.png",
                "05_dry_control_no_caustics.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                SetHd2dAutonomousP1WaterDepthGradientForReview(waterMaterial, Hd2dAutonomousP1WaterDepthGradientStrength);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength * 0.55f, 0f);
                SetHd2dAutonomousP1WaterSpecularForReview(waterMaterial, Hd2dAutonomousP1WaterSpecularStrength * 0.75f);
                ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2DirectionalWaterFlowProfile(), false, 0f);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2FakeRefractionProfile(), false, 0f);
                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2ToonWaterMotionProfile(), false, false, 0f);

                ConfigureHd2dAutonomousP2CausticsMaterialForReview(causticsMaterial, profile, false, 0f);
                CaptureHd2dAutonomousP2CausticsShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[0],
                    false,
                    false,
                    "wet floor baseline with caustics disabled",
                    "off",
                    0f,
                    shotRows);

                ConfigureHd2dAutonomousP2CausticsMaterialForReview(causticsMaterial, profile, true, 0f);
                CaptureHd2dAutonomousP2CausticsShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[1],
                    false,
                    false,
                    "wet floor caustics enabled frame A",
                    "on",
                    0f,
                    shotRows);

                ConfigureHd2dAutonomousP2CausticsMaterialForReview(causticsMaterial, profile, true, 0.55f);
                CaptureHd2dAutonomousP2CausticsShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[2],
                    false,
                    false,
                    "wet floor caustics enabled frame B after 0.55s",
                    "on",
                    0.55f,
                    shotRows);

                ConfigureHd2dAutonomousP2CausticsMaterialForReview(causticsMaterial, profile, true, 0.55f);
                CaptureHd2dAutonomousP2CausticsShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[3],
                    true,
                    false,
                    "wet footprint edge feather closeup",
                    "on",
                    0.55f,
                    shotRows);

                ConfigureHd2dAutonomousP2CausticsMaterialForReview(causticsMaterial, profile, true, 0.55f);
                CaptureHd2dAutonomousP2CausticsShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[4],
                    false,
                    true,
                    "dry control plate outside footprint: no caustics plane",
                    "dry control",
                    0.55f,
                    shotRows);

                var baselineDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
                var motionDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
                var dryDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[4], screenshotFiles[2]);
                WriteHd2dAutonomousP2CausticsReviewReport(outputDirectory, screenshotFiles, shotRows, profile, baselineDiff, motionDiff, dryDiff);
            }
            finally
            {
                SetHd2dAutonomousP2CausticsReviewVisible(true);
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                ConfigureHd2dAutonomousP2CausticsMaterialForReview(causticsMaterial, profile, true, 0f);
                ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2DirectionalWaterFlowProfile(), true, 0f);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2FakeRefractionProfile(), true, 0f);
                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2ToonWaterMotionProfile(), true, true, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-63 caustics review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2Caustics(Transform pastCentralPlazaRoot)
        {
            var profile = EnsureHd2dAutonomousP2CausticsProfile();
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            var causticsMaterial = EnsureHd2dAutonomousP2CausticsMaterial();
            ConfigureHd2dAutonomousP2CausticsMaterialForReview(causticsMaterial, profile, true, 0f);
            CreateHd2dAutonomousP2CausticsReviewSet(pastCentralPlazaRoot, waterMaterial, causticsMaterial);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2Caustics()
        {
            var profile = EnsureHd2dAutonomousP2CausticsProfile();
            var material = EnsureHd2dAutonomousP2CausticsMaterial();
            ConfigureHd2dAutonomousP2CausticsMaterialForReview(material, profile, true, 0f);

            if (profile == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalCausticsApprovedForReview ||
                profile.IntensityForReview <= 0.10f ||
                profile.SpeedAForReview <= 0.02f ||
                profile.SpeedBForReview <= 0.02f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-63 caustics needs conservative non-final profile data for Tom review.");
            }

            foreach (var propertyName in new[]
            {
                "_CausticEnabled",
                "_CausticIntensity",
                "_CausticScaleA",
                "_CausticScaleB",
                "_CausticSpeedA",
                "_CausticSpeedB",
                "_CausticCutoff",
                "_CausticEdgeFeather",
                "_CausticDepthFade",
                "_CausticTimeOffset"
            })
            {
                if (material == null || !material.HasProperty(propertyName))
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-63 caustics material missing property {propertyName}.");
                }
            }

            var shaderSource = File.ReadAllText(Hd2dAutonomousP2CausticsShaderPath);
            foreach (var token in new[]
            {
                "CausticVoronoiRidge",
                "min(patternA, patternB)",
                "_CausticEdgeFeather",
                "step(_CausticCutoff",
                "_CausticTimeOffset"
            })
            {
                ValidateSourceToken(shaderSource, token, Hd2dAutonomousP2CausticsShaderPath);
            }

            if (!File.Exists(Hd2dAutonomousP2CausticsProfileRuntimePath) ||
                CountHd2dAutonomousP2CausticsRenderers("CausticsDecal") < 1 ||
                CountHd2dAutonomousP2CausticsRenderers("DryControl") < 1 ||
                CountHd2dAutonomousP2CausticsRenderers("WetFloor") < 1)
            {
                throw new InvalidOperationException("House slice validation failed: P2-63 review set requires runtime profile source, caustics plane, wet floor, and dry control renderers.");
            }
        }

        private static void CreateHd2dAutonomousP2CausticsReviewSet(Transform pastCentralPlazaRoot, Material waterMaterial, Material causticsMaterial)
        {
            if (pastCentralPlazaRoot == null || waterMaterial == null || causticsMaterial == null)
            {
                return;
            }

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CausticsRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2CausticsRootName);
            root.transform.SetParent(pastCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            var wetFloorMaterial = EnsureHd2dAutonomousP2CausticsSolidMaterial("hd2d_p2_caustics_wet_floor", new Color(0.06f, 0.12f, 0.13f, 1f));
            var dryFloorMaterial = EnsureHd2dAutonomousP2CausticsSolidMaterial("hd2d_p2_caustics_dry_control", new Color(0.24f, 0.23f, 0.18f, 1f));

            CreateHd2dAutonomousP2CausticsCube(
                root.transform,
                "P2_63_Caustics_WetFloorContrast",
                CentralPlazaVsCenter + new Vector3(6.38f, 0.118f, 2.20f),
                new Vector3(1.68f, 0.035f, 0.86f),
                Quaternion.Euler(0f, -8f, 0f),
                wetFloorMaterial,
                "past.central_plaza.p2_63.wet_floor");

            CreateHd2dAutonomousP2CausticsCube(
                root.transform,
                "P2_63_Caustics_CausticsDecalPlane",
                CentralPlazaVsCenter + new Vector3(6.38f, 0.154f, 2.20f),
                new Vector3(1.54f, 0.006f, 0.74f),
                Quaternion.Euler(0f, -8f, 0f),
                causticsMaterial,
                "past.central_plaza.p2_63.caustics_decal");

            CreateHd2dAutonomousP2CausticsCube(
                root.transform,
                "P2_63_Caustics_WaterSurface",
                CentralPlazaVsCenter + new Vector3(6.38f, 0.192f, 2.20f),
                new Vector3(1.72f, 0.044f, 0.88f),
                Quaternion.Euler(0f, -8f, 0f),
                waterMaterial,
                "past.central_plaza.p2_63.water_surface");

            CreateHd2dAutonomousP2CausticsCube(
                root.transform,
                "P2_63_Caustics_DryControlPlate",
                CentralPlazaVsCenter + new Vector3(8.34f, 0.184f, 2.18f),
                new Vector3(1.16f, 0.045f, 0.86f),
                Quaternion.Euler(0f, 7f, 0f),
                dryFloorMaterial,
                "past.central_plaza.p2_63.dry_control");

            EditorUtility.SetDirty(root);
        }

        private static GameObject CreateHd2dAutonomousP2CausticsCube(
            Transform root,
            string objectName,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation,
            Material material,
            string landmarkId)
        {
            var cube = CreateLandmarkCube(
                objectName,
                root,
                localPosition,
                localScale,
                localRotation,
                material,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                landmarkId);
            var landmark = cube.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark != null)
            {
                SerializedSet(landmark, "countsForArrival", false);
                EditorUtility.SetDirty(landmark);
            }

            EditorUtility.SetDirty(cube);
            return cube;
        }

        private static FastVsHd2dCausticsProfile EnsureHd2dAutonomousP2CausticsProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dCausticsProfile>(Hd2dAutonomousP2CausticsProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dCausticsProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2CausticsProfilePath);
            }

            profile.ConfigureForReview(
                new Color(0.78f, 1.00f, 0.88f, 0.55f),
                new Color(0.03f, 0.18f, 0.24f, 0.25f),
                0.62f,
                18f,
                33f,
                0.34f,
                0.21f,
                0.58f,
                0.18f,
                0.74f,
                true,
                false,
                "Conservative P2-63 caustics baseline. Tom should tune dapple brightness, line density, and footprint feather after final water-body geometry is approved.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Material EnsureHd2dAutonomousP2CausticsMaterial()
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2CausticsMaterialPath);
            var shader = Shader.Find(Hd2dAutonomousP2CausticsShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Required P2-63 caustics shader not found: {Hd2dAutonomousP2CausticsShaderName}.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP2CausticsMaterialPath);
            }
            else if (material.shader == null || !string.Equals(material.shader.name, shader.name, StringComparison.Ordinal))
            {
                material.shader = shader;
            }

            material.renderQueue = 3020;
            material.enableInstancing = true;
            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);
            material.SetShaderPassEnabled("ShadowCaster", false);
            ApplyMaterialRole(material, "hd2d_p2_caustics_decal", FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureHd2dAutonomousP2CausticsSolidMaterial(string materialId, Color color)
        {
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var shader = Shader.Find(SurfaceRampLitShaderName) ?? Shader.Find(URPLitShaderName) ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Required P2-63 caustics review material shader not found: {materialId}.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, materialPath);
            }
            else if (material.shader == null || !string.Equals(material.shader.name, shader.name, StringComparison.Ordinal))
            {
                material.shader = shader;
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            if (string.Equals(shader.name, SurfaceRampLitShaderName, StringComparison.Ordinal))
            {
                ApplySurfaceRampProfile(material, materialId);
            }

            material.enableInstancing = true;
            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void ConfigureHd2dAutonomousP2CausticsMaterialForReview(
            Material material,
            FastVsHd2dCausticsProfile profile,
            bool enabled,
            float timeOffset)
        {
            if (material == null || profile == null)
            {
                return;
            }

            SetMaterialColorIfPresent(material, "_CausticHighlightColor", profile.HighlightColorForReview);
            SetMaterialColorIfPresent(material, "_CausticShadowColor", profile.ShadowColorForReview);
            SetMaterialFloatIfPresent(material, "_CausticEnabled", enabled ? 1f : 0f);
            SetMaterialFloatIfPresent(material, "_CausticIntensity", profile.IntensityForReview);
            SetMaterialFloatIfPresent(material, "_CausticScaleA", profile.ScaleAForReview);
            SetMaterialFloatIfPresent(material, "_CausticScaleB", profile.ScaleBForReview);
            SetMaterialFloatIfPresent(material, "_CausticSpeedA", profile.SpeedAForReview);
            SetMaterialFloatIfPresent(material, "_CausticSpeedB", profile.SpeedBForReview);
            SetMaterialFloatIfPresent(material, "_CausticCutoff", profile.CutoffForReview);
            SetMaterialFloatIfPresent(material, "_CausticEdgeFeather", profile.EdgeFeatherForReview);
            SetMaterialFloatIfPresent(material, "_CausticDepthFade", profile.DepthFadeForReview);
            SetMaterialFloatIfPresent(material, "_CausticTimeOffset", Mathf.Max(0f, timeOffset));
            EditorUtility.SetDirty(material);
        }

        private static void SetMaterialColorIfPresent(Material material, string propertyName, Color value)
        {
            if (material != null && material.HasProperty(propertyName))
            {
                material.SetColor(propertyName, value);
            }
        }

        private static void CaptureHd2dAutonomousP2CausticsShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            bool closeEdge,
            bool dryControl,
            string label,
            string state,
            float timeOffset,
            ICollection<string> shotRows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            var player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            var previousPlayerLocal = Vector3.zero;
            var hasPlayer = player != null && controller.CurrentSpaceRootForReview != null;
            if (hasPlayer)
            {
                previousPlayerLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            }

            var playerLocalPosition = CentralPlazaVsCenter + new Vector3(5.80f, 0.02f, 1.54f);
            var anchorLocalPosition = CentralPlazaVsCenter + (dryControl
                ? new Vector3(8.34f, 0.30f, 2.18f)
                : closeEdge
                    ? new Vector3(6.38f, 0.22f, 2.20f)
                    : new Vector3(6.38f, 0.24f, 2.20f));
            var cameraOffset = dryControl
                ? new Vector3(0.44f, 1.02f, -1.36f)
                : closeEdge
                    ? new Vector3(0.38f, 0.96f, -1.22f)
                    : new Vector3(0.72f, 1.42f, -2.08f);
            var lookOffset = dryControl
                ? new Vector3(0.00f, 0.02f, 0.06f)
                : closeEdge
                    ? new Vector3(0.00f, 0.01f, 0.04f)
                    : new Vector3(0.00f, 0.02f, 0.08f);

            controller.ForcePlayerOtherTimeLocalForReview(playerLocalPosition);
            guide.ApplyActiveTimeIsolationForReview();
            var previousMask = camera.cullingMask;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            camera.cullingMask = ((previousMask & ~currentBit) | otherBit) & ~playerBit;
            try
            {
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(false);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(false);
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(false);
                SetHd2dAutonomousP2CausticsReviewVisible(true);
                PositionCloseReviewCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
                SetHd2dAutonomousP2CausticsReviewVisible(true);
                if (hasPlayer)
                {
                    controller.ForcePlayerCurrentLocalForReview(previousPlayerLocal);
                }

                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | {state} | {timeOffset:0.##} |");
        }

        private static void SetHd2dAutonomousP2CausticsReviewVisible(bool visible)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CausticsRootName);
            if (root == null)
            {
                return;
            }

            var renderers = root.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null)
                {
                    continue;
                }

                renderer.gameObject.SetActive(visible);
                renderer.enabled = visible;
                EditorUtility.SetDirty(renderer);
                EditorUtility.SetDirty(renderer.gameObject);
            }
        }

        private static int CountHd2dAutonomousP2CausticsRenderers(string nameToken)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CausticsRootName);
            if (root == null)
            {
                return 0;
            }

            var count = 0;
            var renderers = root.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer != null && renderer.gameObject.name.IndexOf(nameToken, StringComparison.Ordinal) >= 0)
                {
                    count++;
                }
            }

            return count;
        }

        private static void WriteHd2dAutonomousP2CausticsReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dCausticsProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics baselineDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics motionDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics dryDiff)
        {
            var lines = new List<string>
            {
                "# P2-63 Stylized Caustics Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative animated caustics overlay for submerged wet floors, confined by a feathered review footprint rather than finalized water-body art.",
                "- Review setup: a wet contrast plate receives one caustics decal plane below a transparent water surface; a separate dry control plate has no caustics plane.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2CausticsProfilePath}` |",
                $"| Caustics material | `{Hd2dAutonomousP2CausticsMaterialPath}` |",
                $"| Review root | `{Hd2dAutonomousP2CausticsRootName}` |",
                $"| Needs Tom approval | {FormatBool(profile.NeedsTomApprovalForReview)} |",
                $"| Final approved | {FormatBool(profile.FinalCausticsApprovedForReview)} |",
                $"| Intensity / cutoff / edge feather | {profile.IntensityForReview:0.###} / {profile.CutoffForReview:0.###} / {profile.EdgeFeatherForReview:0.###} |",
                $"| Scale A/B | {profile.ScaleAForReview:0.###} / {profile.ScaleBForReview:0.###} |",
                $"| Speed A/B | {profile.SpeedAForReview:0.###} / {profile.SpeedBForReview:0.###} |",
                $"| Caustics / wet / dry renderers | {CountHd2dAutonomousP2CausticsRenderers("CausticsDecal")} / {CountHd2dAutonomousP2CausticsRenderers("WetFloor")} / {CountHd2dAutonomousP2CausticsRenderers("DryControl")} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                baselineDiff.ToReportRow("Caustics off vs enabled frame A"),
                motionDiff.ToReportRow("Caustics frame A vs frame B 0.55s"),
                dryDiff.ToReportRow("Dry control vs wet caustics frame B"),
                string.Empty,
                "| Screenshot | Label | Caustics state | Time offset |",
                "|---|---|---|---:|"
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
                lines.Add($"| `{screenshotFiles[i]}` | P2-63 review capture {i + 1} |");
            }

            File.WriteAllLines(Path.Combine(outputDirectory, "stylized_caustics_review.md"), lines);
        }
    }
}
