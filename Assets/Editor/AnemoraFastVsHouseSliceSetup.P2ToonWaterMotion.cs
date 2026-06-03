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
        private const string Hd2dAutonomousP2ToonWaterMotionRootName = "Past_CentralPlaza_P2_ToonWaterMotionReview";
        private const string Hd2dAutonomousP2ToonWaterMotionProfilePath = "Assets/Settings/FastVS_HD2D_P2_ToonWaterMotionProfile.asset";
        private const string Hd2dAutonomousP2ToonWaterMotionProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dToonWaterMotionProfile.cs";

        public static void CaptureHd2dAutonomousP2Item62ToonWaterMotionBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            if (controller == null || visibility == null || guide == null || camera == null || waterMaterial == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-62 toon water motion capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2ToonWaterMotion();
            var profile = EnsureHd2dAutonomousP2ToonWaterMotionProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("toon_water_specular_vertex_ripple");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_toon_motion_disabled_baseline.png",
                "02_toon_glint_ripple_frame_a.png",
                "03_toon_glint_ripple_frame_b_0p49s.png",
                "04_side_on_vertex_ripple_frame_a.png",
                "05_side_on_vertex_ripple_frame_b_0p49s.png",
                "06_glint_band_closeup.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                SetHd2dAutonomousP1WaterDepthGradientForReview(waterMaterial, Hd2dAutonomousP1WaterDepthGradientStrength);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength * 0.65f, 0f);
                SetHd2dAutonomousP1WaterSpecularForReview(waterMaterial, Hd2dAutonomousP1WaterSpecularStrength);
                ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2DirectionalWaterFlowProfile(), false, 0f);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2FakeRefractionProfile(), false, 0f);

                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, profile, false, true, 0f);
                CaptureHd2dAutonomousP2ToonWaterMotionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[0],
                    false,
                    false,
                    "review water baseline: toon specular disabled, ripple reference enabled",
                    "toon off / ripple ref",
                    0f,
                    shotRows);

                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, profile, true, true, 0f);
                CaptureHd2dAutonomousP2ToonWaterMotionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[1],
                    false,
                    false,
                    "review water frame A: stepped glints and gentle ripple enabled",
                    "on",
                    0f,
                    shotRows);

                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, profile, true, true, 0.49f);
                CaptureHd2dAutonomousP2ToonWaterMotionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[2],
                    false,
                    false,
                    "review water frame B after 0.49s: glints/ripple shifted",
                    "on",
                    0.49f,
                    shotRows);

                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, profile, true, true, 0f);
                CaptureHd2dAutonomousP2ToonWaterMotionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[3],
                    true,
                    false,
                    "side-on vertex ripple frame A",
                    "on",
                    0f,
                    shotRows);

                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, profile, true, true, 0.49f);
                CaptureHd2dAutonomousP2ToonWaterMotionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[4],
                    true,
                    false,
                    "side-on vertex ripple frame B after 0.49s",
                    "on",
                    0.49f,
                    shotRows);

                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, profile, true, true, 0.49f);
                CaptureHd2dAutonomousP2ToonWaterMotionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[5],
                    false,
                    true,
                    "close glint band check: hard-edged highlight patches",
                    "on",
                    0.49f,
                    shotRows);

                var baselineDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
                var motionDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
                var sideMotionDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[3], screenshotFiles[4]);
                WriteHd2dAutonomousP2ToonWaterMotionReviewReport(outputDirectory, screenshotFiles, shotRows, profile, baselineDiff, motionDiff, sideMotionDiff);
            }
            finally
            {
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2DirectionalWaterFlowProfile(), true, 0f);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2FakeRefractionProfile(), true, 0f);
                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, profile, true, true, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-62 toon water motion review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2ToonWaterMotion(Transform pastCentralPlazaRoot)
        {
            var profile = EnsureHd2dAutonomousP2ToonWaterMotionProfile();
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, profile, true, true, 0f);
            CreateHd2dAutonomousP2ToonWaterMotionReviewSet(pastCentralPlazaRoot, waterMaterial);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2ToonWaterMotion()
        {
            var profile = EnsureHd2dAutonomousP2ToonWaterMotionProfile();
            var material = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(material, profile, true, true, 0f);

            if (profile == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalToonWaterMotionApprovedForReview ||
                profile.ToonSpecularIntensityForReview <= 0.08f ||
                profile.VertexRippleAmplitudeForReview <= 0.004f ||
                profile.VertexRippleSpeedForReview <= 0.05f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-62 toon water motion needs conservative non-final profile data for Tom review.");
            }

            foreach (var propertyName in new[]
            {
                "_ToonWaterSpecularEnabled",
                "_ToonWaterSpecularIntensity",
                "_ToonWaterSpecularSteps",
                "_ToonWaterSpecularCutoff",
                "_VertexRippleEnabled",
                "_VertexRippleAmplitude",
                "_VertexRippleFrequency",
                "_VertexRippleSpeed",
                "_VertexRippleTimeOffset",
                "_VertexRippleNormalStrength"
            })
            {
                if (material == null || !material.HasProperty(propertyName))
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-62 water material missing property {propertyName}.");
                }
            }

            var shaderSource = File.ReadAllText(Hd2dAutonomousP1DepthGradientWaterShaderPath);
            foreach (var token in new[]
            {
                "_ToonWaterSpecularEnabled",
                "_VertexRippleEnabled",
                "_VertexRippleTimeOffset",
                "positionWS.y +=",
                "rippleNormalWS",
                "floor(toonGlintSeed",
                "_ToonWaterSpecularSteps"
            })
            {
                ValidateSourceToken(shaderSource, token, Hd2dAutonomousP1DepthGradientWaterShaderPath);
            }

            if (!File.Exists(Hd2dAutonomousP2ToonWaterMotionProfileRuntimePath) ||
                CountHd2dAutonomousP2ToonWaterMotionRenderers("Water") < 6 ||
                CountHd2dAutonomousP2ToonWaterMotionRenderers("Contrast") < 1)
            {
                throw new InvalidOperationException("House slice validation failed: P2-62 review set requires runtime profile source plus water and contrast renderers.");
            }
        }

        private static void CreateHd2dAutonomousP2ToonWaterMotionReviewSet(Transform pastCentralPlazaRoot, Material waterMaterial)
        {
            if (pastCentralPlazaRoot == null || waterMaterial == null)
            {
                return;
            }

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2ToonWaterMotionRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2ToonWaterMotionRootName);
            root.transform.SetParent(pastCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            var contrastMaterial = EnsureHd2dAutonomousP2ToonWaterMotionSolidMaterial(
                "hd2d_p2_toon_water_motion_submerged_contrast",
                new Color(0.07f, 0.10f, 0.12f, 1f));
            var edgeMaterial = EnsureHd2dAutonomousP2ToonWaterMotionSolidMaterial(
                "hd2d_p2_toon_water_motion_wave_edge",
                new Color(0.58f, 0.72f, 0.66f, 1f));

            CreateHd2dAutonomousP2ToonWaterMotionCube(
                root.transform,
                "P2_62_ToonWaterMotion_ContrastPlate",
                CentralPlazaVsCenter + new Vector3(4.28f, 0.118f, 2.22f),
                new Vector3(1.86f, 0.035f, 0.92f),
                Quaternion.Euler(0f, -8f, 0f),
                contrastMaterial,
                "past.central_plaza.p2_62.contrast_plate");

            CreateHd2dAutonomousP2ToonWaterMotionCube(
                root.transform,
                "P2_62_ToonWaterMotion_WaterMain",
                CentralPlazaVsCenter + new Vector3(4.28f, 0.182f, 2.22f),
                new Vector3(1.78f, 0.045f, 0.86f),
                Quaternion.Euler(0f, -8f, 0f),
                waterMaterial,
                "past.central_plaza.p2_62.water_main");

            for (var i = 0; i < 7; i++)
            {
                var x = 3.44f + i * 0.28f;
                CreateHd2dAutonomousP2ToonWaterMotionCube(
                    root.transform,
                    $"P2_62_ToonWaterMotion_WaterWaveStrip_{i:00}",
                    CentralPlazaVsCenter + new Vector3(x, 0.202f, 3.22f + Mathf.Sin(i * 0.71f) * 0.035f),
                    new Vector3(0.17f, 0.065f, 0.72f),
                    Quaternion.Euler(0f, -4f + i * 2.5f, 0f),
                    waterMaterial,
                    $"past.central_plaza.p2_62.water_wave_strip_{i:00}");
            }

            CreateHd2dAutonomousP2ToonWaterMotionCube(
                root.transform,
                "P2_62_ToonWaterMotion_WaveEdgeMarker",
                CentralPlazaVsCenter + new Vector3(4.28f, 0.128f, 3.22f),
                new Vector3(1.96f, 0.030f, 0.045f),
                Quaternion.Euler(0f, -5f, 0f),
                edgeMaterial,
                "past.central_plaza.p2_62.wave_edge_marker");

            EditorUtility.SetDirty(root);
        }

        private static GameObject CreateHd2dAutonomousP2ToonWaterMotionCube(
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

        private static FastVsHd2dToonWaterMotionProfile EnsureHd2dAutonomousP2ToonWaterMotionProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dToonWaterMotionProfile>(Hd2dAutonomousP2ToonWaterMotionProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dToonWaterMotionProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2ToonWaterMotionProfilePath);
            }

            profile.ConfigureForReview(
                0.52f,
                2f,
                0.42f,
                0.032f,
                3.2f,
                0.72f,
                0.55f,
                true,
                false,
                "Conservative P2-62 toon-water motion baseline. Tom should tune final glint band count, highlight color/intensity, and ripple amplitude per approved water mesh after water art sign-off.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Material EnsureHd2dAutonomousP2ToonWaterMotionSolidMaterial(string materialId, Color color)
        {
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var shader = Shader.Find(SurfaceRampLitShaderName) ?? Shader.Find(URPLitShaderName) ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Required P2-62 toon water motion review material shader not found: {materialId}.");
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

        private static void ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(
            Material material,
            FastVsHd2dToonWaterMotionProfile profile,
            bool toonSpecularEnabled,
            bool vertexRippleEnabled,
            float timeOffset)
        {
            if (material == null || profile == null)
            {
                return;
            }

            SetMaterialFloatIfPresent(material, "_ToonWaterSpecularEnabled", toonSpecularEnabled ? 1f : 0f);
            SetMaterialFloatIfPresent(material, "_ToonWaterSpecularIntensity", profile.ToonSpecularIntensityForReview);
            SetMaterialFloatIfPresent(material, "_ToonWaterSpecularSteps", profile.ToonSpecularStepsForReview);
            SetMaterialFloatIfPresent(material, "_ToonWaterSpecularCutoff", profile.ToonSpecularCutoffForReview);
            SetMaterialFloatIfPresent(material, "_VertexRippleEnabled", vertexRippleEnabled ? 1f : 0f);
            SetMaterialFloatIfPresent(material, "_VertexRippleAmplitude", profile.VertexRippleAmplitudeForReview);
            SetMaterialFloatIfPresent(material, "_VertexRippleFrequency", profile.VertexRippleFrequencyForReview);
            SetMaterialFloatIfPresent(material, "_VertexRippleSpeed", profile.VertexRippleSpeedForReview);
            SetMaterialFloatIfPresent(material, "_VertexRippleTimeOffset", Mathf.Max(0f, timeOffset));
            SetMaterialFloatIfPresent(material, "_VertexRippleNormalStrength", profile.VertexRippleNormalStrengthForReview);
            EditorUtility.SetDirty(material);
        }

        private static void CaptureHd2dAutonomousP2ToonWaterMotionShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            bool sideOn,
            bool closeGlint,
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

            var playerLocalPosition = CentralPlazaVsCenter + new Vector3(3.80f, 0.02f, 1.54f);
            var anchorLocalPosition = CentralPlazaVsCenter + (sideOn
                ? new Vector3(4.28f, 0.24f, 3.22f)
                : closeGlint
                    ? new Vector3(4.28f, 0.28f, 2.22f)
                    : new Vector3(4.28f, 0.24f, 2.22f));
            var cameraOffset = sideOn
                ? new Vector3(0.18f, 0.64f, -1.36f)
                : closeGlint
                    ? new Vector3(0.38f, 1.02f, -1.36f)
                    : new Vector3(0.72f, 1.42f, -2.14f);
            var lookOffset = sideOn
                ? new Vector3(0.00f, 0.00f, 0.12f)
                : closeGlint
                    ? new Vector3(0.00f, 0.02f, 0.06f)
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
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
                PositionCloseReviewCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
                if (hasPlayer)
                {
                    controller.ForcePlayerCurrentLocalForReview(previousPlayerLocal);
                }

                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | {state} | {timeOffset:0.##} |");
        }

        private static void SetHd2dAutonomousP2ToonWaterMotionReviewVisible(bool visible)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2ToonWaterMotionRootName);
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

        private static int CountHd2dAutonomousP2ToonWaterMotionRenderers(string nameToken)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2ToonWaterMotionRootName);
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

        private static void WriteHd2dAutonomousP2ToonWaterMotionReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dToonWaterMotionProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics baselineDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics motionDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics sideMotionDiff)
        {
            var lines = new List<string>
            {
                "# P2-62 Toon Water Motion Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative toon-stepped water specular highlights plus gentle vertex ripple waves on the shared depth-gradient water shader.",
                "- Review setup: a past-plaza diagnostic water pad and side-on wave-strip row isolate the shader controls from final environment art.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2ToonWaterMotionProfilePath}` |",
                $"| Water material | `{Hd2dAutonomousP1DepthGradientWaterMaterialPath}` |",
                $"| Review root | `{Hd2dAutonomousP2ToonWaterMotionRootName}` |",
                $"| Needs Tom approval | {FormatBool(profile.NeedsTomApprovalForReview)} |",
                $"| Final approved | {FormatBool(profile.FinalToonWaterMotionApprovedForReview)} |",
                $"| Toon intensity / steps / cutoff | {profile.ToonSpecularIntensityForReview:0.###} / {profile.ToonSpecularStepsForReview:0.###} / {profile.ToonSpecularCutoffForReview:0.###} |",
                $"| Ripple amplitude / frequency / speed | {profile.VertexRippleAmplitudeForReview:0.###} / {profile.VertexRippleFrequencyForReview:0.###} / {profile.VertexRippleSpeedForReview:0.###} |",
                $"| Ripple normal strength | {profile.VertexRippleNormalStrengthForReview:0.###} |",
                $"| Water / contrast renderers | {CountHd2dAutonomousP2ToonWaterMotionRenderers("Water")} / {CountHd2dAutonomousP2ToonWaterMotionRenderers("Contrast")} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                baselineDiff.ToReportRow("Toon/ripple off vs frame A"),
                motionDiff.ToReportRow("Frame A vs frame B 0.49s"),
                sideMotionDiff.ToReportRow("Side-on ripple frame A vs B 0.49s"),
                string.Empty,
                "| Screenshot | Label | Toon/ripple state | Time offset |",
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
                lines.Add($"| `{screenshotFiles[i]}` | P2-62 review capture {i + 1} |");
            }

            File.WriteAllLines(Path.Combine(outputDirectory, "toon_water_specular_vertex_ripple_review.md"), lines);
        }
    }
}
