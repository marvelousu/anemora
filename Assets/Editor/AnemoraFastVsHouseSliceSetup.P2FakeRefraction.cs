using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2FakeRefractionRootName = "Past_CentralPlaza_P2_FakeRefractionReviewPond";
        private const string Hd2dAutonomousP2FakeRefractionProfilePath = "Assets/Settings/FastVS_HD2D_P2_FakeRefractionProfile.asset";
        private const string Hd2dAutonomousP2FakeRefractionProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dFakeRefractionProfile.cs";

        public static void CaptureHd2dAutonomousP2Item61FakeRefractionBatch()
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
                throw new InvalidOperationException("Fast VS autonomous P2-61 fake refraction capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2FakeRefraction();
            var profile = EnsureHd2dAutonomousP2FakeRefractionProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("fake_refraction_water_wobble");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_refraction_zero_pixel_baseline.png",
                "02_refraction_on_frame_a.png",
                "03_refraction_on_frame_b_0p57s.png",
                "04_refraction_edge_guard_close.png",
                "05_no_water_submerged_pattern_control.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                SetHd2dAutonomousP1WaterDepthGradientForReview(waterMaterial, Hd2dAutonomousP1WaterDepthGradientStrength);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                SetHd2dAutonomousP1WaterSpecularForReview(waterMaterial, Hd2dAutonomousP1WaterSpecularStrength);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);

                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, profile, true, 0f, 0f);
                CaptureHd2dAutonomousP2FakeRefractionReviewPondShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[0],
                    false,
                    true);
                shotRows.Add($"| `{screenshotFiles[0]}` | shallow pond straight scene-color baseline with 0px offset | 0px | 0 |");

                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, profile, true, 0f);
                CaptureHd2dAutonomousP2FakeRefractionReviewPondShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[1],
                    false,
                    true);
                shotRows.Add($"| `{screenshotFiles[1]}` | shallow pond refraction enabled frame A | yes | 0 |");

                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, profile, true, 0.57f);
                CaptureHd2dAutonomousP2FakeRefractionReviewPondShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[2],
                    false,
                    true);
                shotRows.Add($"| `{screenshotFiles[2]}` | shallow pond refraction enabled frame B after 0.57s review offset | yes | 0.57 |");

                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, profile, true, 0.57f);
                CaptureHd2dAutonomousP2FakeRefractionReviewPondShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[3],
                    true,
                    true);
                shotRows.Add($"| `{screenshotFiles[3]}` | close edge-guard check: clamped offset near waterline | yes | 0.57 |");

                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, profile, true, 0f, 0f);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(false);
                CaptureHd2dAutonomousP2FakeRefractionReviewPondShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[4],
                    false,
                    false);
                shotRows.Add($"| `{screenshotFiles[4]}` | no-water control showing straight submerged pattern | no water | 0 |");

                var offOnDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
                var motionDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
                var controlDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[4], screenshotFiles[1]);
                WriteHd2dAutonomousP2FakeRefractionReviewReport(outputDirectory, screenshotFiles, shotRows, profile, offOnDiff, motionDiff, controlDiff);
            }
            finally
            {
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, profile, true, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-61 fake refraction review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2FakeRefraction(Transform pastCentralPlazaRoot)
        {
            var profile = EnsureHd2dAutonomousP2FakeRefractionProfile();
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, profile, true, 0f);
            CreateHd2dAutonomousP2FakeRefractionReviewPond(pastCentralPlazaRoot, waterMaterial);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2FakeRefraction()
        {
            var profile = EnsureHd2dAutonomousP2FakeRefractionProfile();
            var material = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(material, profile, true, 0f);

            if (profile == null ||
                !profile.AutoSafeAcceptedForReview ||
                profile.StrengthPixelsForReview <= 0.2f ||
                profile.StrengthPixelsForReview > 4f ||
                profile.SceneBlendForReview <= 0.05f ||
                profile.EdgeGuardForReview < 0.35f ||
                material == null ||
                material.shader == null ||
                !string.Equals(material.shader.name, Hd2dAutonomousP1DepthGradientWaterShaderName, StringComparison.Ordinal) ||
                CountHd2dAutonomousP2FakeRefractionWaterSurfaces() < 1 ||
                CountHd2dAutonomousP2FakeRefractionSubmergedPatternRenderers() < 5)
            {
                throw new InvalidOperationException("House slice validation failed: P2-61 needs auto-safe fake refraction profile, water surface, submerged pattern, and conservative material values.");
            }

            foreach (var propertyName in new[]
            {
                "_FakeRefractionEnabled",
                "_RefractionStrengthPixels",
                "_RefractionNoiseScale",
                "_RefractionScrollSpeed",
                "_RefractionTimeOffset",
                "_RefractionDepthFade",
                "_RefractionSceneBlend",
                "_RefractionEdgeGuard"
            })
            {
                if (!material.HasProperty(propertyName))
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-61 water material must expose {propertyName}.");
                }
            }

            if (!File.Exists(Hd2dAutonomousP1DepthGradientWaterShaderPath))
            {
                throw new InvalidOperationException($"House slice validation failed: missing P2-61 water shader file: {Hd2dAutonomousP1DepthGradientWaterShaderPath}");
            }

            var shaderSource = File.ReadAllText(Hd2dAutonomousP1DepthGradientWaterShaderPath);
            foreach (var token in new[]
            {
                "DeclareOpaqueTexture.hlsl",
                "SampleSceneColor",
                "_FakeRefractionEnabled",
                "_RefractionStrengthPixels",
                "_RefractionTimeOffset",
                "_RefractionEdgeGuard",
                "refractionDepthMask",
                "refractionOffset",
                "clamp(screenUv + refractionOffset"
            })
            {
                ValidateSourceToken(shaderSource, token, Hd2dAutonomousP1DepthGradientWaterShaderPath);
            }

            var urpAssetPath = Path.Combine("Assets", "Settings", "UniversalRenderPipeline.asset");
            var urpAsset = File.Exists(urpAssetPath) ? File.ReadAllText(urpAssetPath) : string.Empty;
            ValidateSourceToken(urpAsset, "m_RequireOpaqueTexture: 1", urpAssetPath);

            if (!File.Exists(Hd2dAutonomousP2FakeRefractionProfileRuntimePath))
            {
                throw new InvalidOperationException($"House slice validation failed: missing P2-61 runtime profile file: {Hd2dAutonomousP2FakeRefractionProfileRuntimePath}");
            }
        }

        private static FastVsHd2dFakeRefractionProfile EnsureHd2dAutonomousP2FakeRefractionProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dFakeRefractionProfile>(Hd2dAutonomousP2FakeRefractionProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dFakeRefractionProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2FakeRefractionProfilePath);
            }

            profile.ConfigureForReview(
                2.4f,
                18f,
                0.82f,
                0.28f,
                0.34f,
                0.78f,
                true,
                "Auto-safe P2-61 baseline: keep fake refraction clamped to a few pixels, depth-faded away from shallow edges, and retune only if final shallow-water art needs more wobble.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static void CreateHd2dAutonomousP2FakeRefractionReviewPond(Transform pastCentralPlazaRoot, Material waterMaterial)
        {
            if (pastCentralPlazaRoot == null || waterMaterial == null)
            {
                return;
            }

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2FakeRefractionRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2FakeRefractionRootName);
            root.transform.SetParent(pastCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            var darkPattern = EnsureHd2dAutonomousP2FakeRefractionSolidMaterial("hd2d_p2_fake_refraction_submerged_dark", new Color(0.08f, 0.11f, 0.12f, 1f));
            var lightPattern = EnsureHd2dAutonomousP2FakeRefractionSolidMaterial("hd2d_p2_fake_refraction_submerged_light", new Color(0.58f, 0.64f, 0.58f, 1f));
            var pebble = EnsureHd2dAutonomousP2FakeRefractionSolidMaterial("hd2d_p2_fake_refraction_submerged_pebble", new Color(0.24f, 0.23f, 0.20f, 1f));

            CreateHd2dAutonomousP2FakeRefractionCube(
                root.transform,
                "P2_61_ReviewPond_SubmergedStripe_A",
                CentralPlazaVsCenter + new Vector3(1.28f, 0.105f, 2.04f),
                new Vector3(1.18f, 0.025f, 0.055f),
                Quaternion.Euler(0f, -18f, 0f),
                darkPattern,
                "past.central_plaza.p2_61.submerged.stripe_a");
            CreateHd2dAutonomousP2FakeRefractionCube(
                root.transform,
                "P2_61_ReviewPond_SubmergedStripe_B",
                CentralPlazaVsCenter + new Vector3(1.74f, 0.108f, 2.22f),
                new Vector3(1.08f, 0.025f, 0.050f),
                Quaternion.Euler(0f, 16f, 0f),
                lightPattern,
                "past.central_plaza.p2_61.submerged.stripe_b");
            CreateHd2dAutonomousP2FakeRefractionCube(
                root.transform,
                "P2_61_ReviewPond_SubmergedStripe_C",
                CentralPlazaVsCenter + new Vector3(2.12f, 0.111f, 2.02f),
                new Vector3(0.86f, 0.025f, 0.052f),
                Quaternion.Euler(0f, -28f, 0f),
                darkPattern,
                "past.central_plaza.p2_61.submerged.stripe_c");

            for (var i = 0; i < 5; i++)
            {
                CreateHd2dAutonomousP2FakeRefractionCube(
                    root.transform,
                    $"P2_61_ReviewPond_SubmergedPebble_{i:00}",
                    CentralPlazaVsCenter + new Vector3(1.18f + i * 0.24f, 0.116f + i * 0.001f, 2.44f + Mathf.Sin(i * 1.3f) * 0.12f),
                    new Vector3(0.10f + (i % 2) * 0.035f, 0.035f, 0.075f + (i % 3) * 0.018f),
                    Quaternion.Euler(0f, i * 31f, 0f),
                    pebble,
                    $"past.central_plaza.p2_61.submerged.pebble_{i:00}");
            }

            var water = CreateHd2dAutonomousP2FakeRefractionCube(
                root.transform,
                "P2_61_ReviewPond_WaterSurface",
                CentralPlazaVsCenter + new Vector3(1.70f, 0.176f, 2.22f),
                new Vector3(1.72f, 0.045f, 0.86f),
                Quaternion.Euler(0f, -7f, 0f),
                waterMaterial,
                "past.central_plaza.p2_61.water_surface");
            water.name = "P2_61_ReviewPond_WaterSurface";
            EditorUtility.SetDirty(root);
        }

        private static GameObject CreateHd2dAutonomousP2FakeRefractionCube(
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

        private static Material EnsureHd2dAutonomousP2FakeRefractionSolidMaterial(string materialId, Color color)
        {
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var shader = Shader.Find(SurfaceRampLitShaderName) ?? Shader.Find(URPLitShaderName) ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Required P2-61 fake refraction review material shader not found: {materialId}.");
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

        private static void ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(
            Material material,
            FastVsHd2dFakeRefractionProfile profile,
            bool enabled,
            float timeOffset,
            float strengthMultiplier = 1f)
        {
            if (material == null || profile == null)
            {
                return;
            }

            SetMaterialFloatIfPresent(material, "_FakeRefractionEnabled", enabled ? 1f : 0f);
            SetMaterialFloatIfPresent(material, "_RefractionStrengthPixels", profile.StrengthPixelsForReview * Mathf.Clamp01(strengthMultiplier));
            SetMaterialFloatIfPresent(material, "_RefractionNoiseScale", profile.NoiseScaleForReview);
            SetMaterialFloatIfPresent(material, "_RefractionScrollSpeed", profile.ScrollSpeedForReview);
            SetMaterialFloatIfPresent(material, "_RefractionTimeOffset", Mathf.Max(0f, timeOffset));
            SetMaterialFloatIfPresent(material, "_RefractionDepthFade", profile.DepthFadeForReview);
            SetMaterialFloatIfPresent(material, "_RefractionSceneBlend", profile.SceneBlendForReview);
            SetMaterialFloatIfPresent(material, "_RefractionEdgeGuard", profile.EdgeGuardForReview);
            EditorUtility.SetDirty(material);
        }

        private static void SetMaterialFloatIfPresent(Material material, string propertyName, float value)
        {
            if (material != null && material.HasProperty(propertyName))
            {
                material.SetFloat(propertyName, value);
            }
        }

        private static void CaptureHd2dAutonomousP2FakeRefractionReviewPondShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            bool closeEdge,
            bool waterVisible)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            var player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            var previousPlayerLocal = Vector3.zero;
            var hasPlayer = player != null && controller.CurrentSpaceRootForReview != null;
            if (hasPlayer)
            {
                previousPlayerLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            }

            var playerLocalPosition = CentralPlazaVsCenter + new Vector3(0.82f, 0.02f, 1.64f);
            var anchorLocalPosition = CentralPlazaVsCenter + (closeEdge ? new Vector3(1.42f, 0.22f, 2.06f) : new Vector3(1.70f, 0.24f, 2.22f));
            var cameraOffset = closeEdge ? new Vector3(0.48f, 1.20f, -1.68f) : new Vector3(0.72f, 1.46f, -2.20f);
            var lookOffset = new Vector3(0.00f, 0.02f, 0.08f);
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
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(waterVisible);
                PositionCloseReviewCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                if (hasPlayer)
                {
                    controller.ForcePlayerCurrentLocalForReview(previousPlayerLocal);
                }

                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
        }

        private static void SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(bool visible)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2FakeRefractionRootName);
            if (root == null)
            {
                return;
            }

            var renderers = root.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer != null && renderer.gameObject.name.Contains("WaterSurface", StringComparison.Ordinal))
                {
                    renderer.gameObject.SetActive(visible);
                    renderer.enabled = visible;
                    EditorUtility.SetDirty(renderer);
                    EditorUtility.SetDirty(renderer.gameObject);
                }
            }
        }

        private static void SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(bool visible)
        {
            var root = FindSceneObjectIncludingInactive("Past_CentralPlaza_P2_DirectionalWaterFlowReviewTrough");
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

        private static int CountHd2dAutonomousP2FakeRefractionWaterSurfaces()
        {
            return CountHd2dAutonomousP2FakeRefractionRenderers("WaterSurface");
        }

        private static int CountHd2dAutonomousP2FakeRefractionSubmergedPatternRenderers()
        {
            return CountHd2dAutonomousP2FakeRefractionRenderers("Submerged");
        }

        private static int CountHd2dAutonomousP2FakeRefractionRenderers(string nameToken)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2FakeRefractionRootName);
            if (root == null)
            {
                return 0;
            }

            var renderers = root.GetComponentsInChildren<Renderer>(true);
            var count = 0;
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].gameObject.name.Contains(nameToken, StringComparison.Ordinal))
                {
                    count++;
                }
            }

            return count;
        }

        private static void WriteHd2dAutonomousP2FakeRefractionReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dFakeRefractionProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics offOnDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics motionDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics controlDiff)
        {
            var lines = new List<string>
            {
                "# P2-61 Fake Refraction Review",
                string.Empty,
                "- Scope: auto-safe fake refraction via URP Opaque Texture scene-color sampling, clamped screen-UV distortion, and depth/edge attenuation on the shared transparent water shader.",
                "- Review setup: a shallow past-plaza diagnostic pond places opaque submerged stripes/pebbles below the transparent water surface so the screen-color wobble can be measured in batch screenshots.",
                $"- Recommendation: {profile.RecommendationForReview}",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2FakeRefractionProfilePath}` |",
                $"| Water material | `{Hd2dAutonomousP1DepthGradientWaterMaterialPath}` |",
                $"| Review pond root | `{Hd2dAutonomousP2FakeRefractionRootName}` |",
                $"| Auto-safe accepted | {FormatBool(profile.AutoSafeAcceptedForReview)} |",
                $"| Strength pixels / scene blend | {profile.StrengthPixelsForReview:0.###} / {profile.SceneBlendForReview:0.###} |",
                $"| Noise scale / scroll speed | {profile.NoiseScaleForReview:0.###} / {profile.ScrollSpeedForReview:0.###} |",
                $"| Depth fade / edge guard | {profile.DepthFadeForReview:0.###} / {profile.EdgeGuardForReview:0.###} |",
                $"| Water surfaces / submerged renderers | {CountHd2dAutonomousP2FakeRefractionWaterSurfaces()} / {CountHd2dAutonomousP2FakeRefractionSubmergedPatternRenderers()} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                offOnDiff.ToReportRow("Refraction off vs on frame A"),
                motionDiff.ToReportRow("Refraction frame A vs frame B 0.57s"),
                controlDiff.ToReportRow("No-water straight pattern vs refracted frame A"),
                string.Empty,
                "| Screenshot | Label | Refraction state | Time offset |",
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
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[i]);
                lines.Add($"| `{screenshotFiles[i]}` | P2-61 review capture {i + 1} |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "fake_refraction_water_wobble_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }
    }
}
