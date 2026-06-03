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
        private const string Hd2dAutonomousP2DirectionalWaterFlowDriverName = "FastVS_HD2D_P2_DirectionalWaterFlowDriver";
        private const string Hd2dAutonomousP2DirectionalWaterFlowProfilePath = "Assets/Settings/FastVS_HD2D_P2_DirectionalWaterFlowProfile.asset";
        private const string Hd2dAutonomousP2DirectionalWaterFlowProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dDirectionalWaterFlowProfile.cs";
        private const string Hd2dAutonomousP2DirectionalWaterFlowDriverRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dDirectionalWaterFlowDriver.cs";
        private const string Hd2dAutonomousP2DirectionalWaterFlowMapTextureId = "hd2d_p2_directional_water_flow_map";
        private const string Hd2dAutonomousP2DirectionalWaterFlowReviewTroughRootName = "Past_CentralPlaza_P2_DirectionalWaterFlowReviewTrough";

        public static void CaptureHd2dAutonomousP2Item60DirectionalWaterFlowBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            var driver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dDirectionalWaterFlowDriver>(FindObjectsInactive.Include);
            if (controller == null || visibility == null || guide == null || camera == null || waterMaterial == null || driver == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-60 directional water flow capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2DirectionalWaterFlow();
            var profile = EnsureHd2dAutonomousP2DirectionalWaterFlowProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("directional_water_flow");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_flow_disabled_baseline.png",
                "02_plaza_flow_frame_a.png",
                "03_plaza_flow_frame_b_0p52s.png",
                "04_review_trough_flow_frame_a.png",
                "05_review_trough_flow_frame_b_0p52s.png",
                "06_directional_flow_map_preview.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                SetHd2dAutonomousP1WaterDepthGradientForReview(waterMaterial, Hd2dAutonomousP1WaterDepthGradientStrength);
                SetHd2dAutonomousP1WaterSpecularForReview(waterMaterial, Hd2dAutonomousP1WaterSpecularStrength);

                driver.ApplyReviewStateForReview(false, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                CaptureHd2dAutonomousP2DirectionalWaterFlowShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[0],
                    "plaza fountain baseline: directional flow disabled",
                    false,
                    0f,
                    "Past plaza fountain",
                    shotRows);

                driver.ApplyReviewStateForReview(true, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                CaptureHd2dAutonomousP2DirectionalWaterFlowShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[1],
                    "plaza fountain directional flow frame A",
                    true,
                    0f,
                    "Past plaza fountain",
                    shotRows);

                driver.ApplyReviewStateForReview(true, 0.52f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0.52f);
                CaptureHd2dAutonomousP2DirectionalWaterFlowShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[2],
                    "plaza fountain directional flow frame B after 0.52s review offset",
                    true,
                    0.52f,
                    "Past plaza fountain",
                    shotRows);

                driver.ApplyReviewStateForReview(true, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                CaptureHd2dAutonomousP2DirectionalWaterFlowReviewTroughShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[3]);
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[3]);
                shotRows.Add($"| `{screenshotFiles[3]}` | review trough bend directional flow frame A | yes | 0 | Past plaza review trough |");

                driver.ApplyReviewStateForReview(true, 0.52f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0.52f);
                CaptureHd2dAutonomousP2DirectionalWaterFlowReviewTroughShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[4]);
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[4]);
                shotRows.Add($"| `{screenshotFiles[4]}` | review trough bend directional flow frame B after 0.52s review offset | yes | 0.52 | Past plaza review trough |");

                WriteHd2dAutonomousP2DirectionalWaterFlowMapPreview(profile.FlowMapForReview, Path.Combine(outputDirectory, screenshotFiles[5]), 5);
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[5]);

                var baselineDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
                var plazaMotionDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
                var troughMotionDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[3], screenshotFiles[4]);
                WriteHd2dAutonomousP2DirectionalWaterFlowReviewReport(outputDirectory, screenshotFiles, shotRows, profile, driver, baselineDiff, plazaMotionDiff, troughMotionDiff);
            }
            finally
            {
                driver.ApplyReviewStateForReview(true, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-60 directional water flow review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2DirectionalWaterFlow(Transform pastCentralPlazaRoot)
        {
            var profile = EnsureHd2dAutonomousP2DirectionalWaterFlowProfile();
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(waterMaterial, profile, true, 0f);
            CreateHd2dAutonomousP2DirectionalWaterFlowReviewTrough(pastCentralPlazaRoot, waterMaterial);

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2DirectionalWaterFlowDriverName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2DirectionalWaterFlowDriverName);
            var driver = root.AddComponent<FastVsHd2dDirectionalWaterFlowDriver>();
            driver.ConfigureForReview(profile, waterMaterial);
            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(driver);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2DirectionalWaterFlow()
        {
            var profile = EnsureHd2dAutonomousP2DirectionalWaterFlowProfile();
            var material = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(material, profile, true, 0f);

            var driver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dDirectionalWaterFlowDriver>(FindObjectsInactive.Include);
            if (profile == null ||
                profile.FlowMapForReview == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalDirectionalFlowApprovedForReview ||
                profile.FlowStrengthForReview <= 0.25f ||
                profile.FlowSpeedForReview <= 0.10f ||
                profile.FoamAdvectionStrengthForReview <= 0.25f ||
                profile.PhaseBlendSharpnessForReview <= 0f ||
                material == null ||
                material.shader == null ||
                !string.Equals(material.shader.name, Hd2dAutonomousP1DepthGradientWaterShaderName, StringComparison.Ordinal) ||
                driver == null ||
                !driver.IsReadyForReview ||
                !driver.ConservativeNeedsTomApprovalForReview ||
                CountHd2dAutonomousP2DirectionalWaterFlowReviewTroughSurfaces(material) < 2)
            {
                throw new InvalidOperationException("House slice validation failed: P2-60 needs conservative directional water flow data, driver, flow map, review trough surfaces, and Tom-facing approval flags.");
            }

            if (profile.FlowMapForReview.filterMode != FilterMode.Point ||
                profile.FlowMapForReview.wrapMode != TextureWrapMode.Repeat ||
                !string.Equals(AssetDatabase.GetAssetPath(profile.FlowMapForReview), $"{TextureDirectory}/FastVS_House_{Hd2dAutonomousP2DirectionalWaterFlowMapTextureId}.asset", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("House slice validation failed: P2-60 flow map must be the generated point-sampled repeat texture asset.");
            }

            foreach (var propertyName in new[]
            {
                "_FlowMap",
                "_DirectionalFlowEnabled",
                "_FlowStrength",
                "_FlowSpeed",
                "_FlowTimeOffset",
                "_FlowFoamAdvectionStrength",
                "_FlowSpecularAdvectionStrength",
                "_FlowPhaseBlendSharpness"
            })
            {
                if (!material.HasProperty(propertyName))
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-60 water material must expose {propertyName}.");
                }
            }

            if (!File.Exists(Hd2dAutonomousP1DepthGradientWaterShaderPath))
            {
                throw new InvalidOperationException($"House slice validation failed: missing P2-60 water shader file: {Hd2dAutonomousP1DepthGradientWaterShaderPath}");
            }

            var shaderSource = File.ReadAllText(Hd2dAutonomousP1DepthGradientWaterShaderPath);
            foreach (var token in new[]
            {
                "_FlowMap",
                "_DirectionalFlowEnabled",
                "_FlowStrength",
                "_FlowSpeed",
                "_FlowTimeOffset",
                "_FlowFoamAdvectionStrength",
                "_FlowSpecularAdvectionStrength",
                "_FlowPhaseBlendSharpness",
                "DirectionalWaterFlowPhaseBlend",
                "DecodeDirectionalWaterFlow",
                "frac(flowTime + 0.5f)",
                "phaseBlend"
            })
            {
                ValidateSourceToken(shaderSource, token, Hd2dAutonomousP1DepthGradientWaterShaderPath);
            }

            foreach (var runtimePath in new[] { Hd2dAutonomousP2DirectionalWaterFlowProfileRuntimePath, Hd2dAutonomousP2DirectionalWaterFlowDriverRuntimePath })
            {
                if (!File.Exists(runtimePath))
                {
                    throw new InvalidOperationException($"House slice validation failed: missing P2-60 runtime file: {runtimePath}");
                }
            }
        }

        private static void CreateHd2dAutonomousP2DirectionalWaterFlowReviewTrough(Transform pastCentralPlazaRoot, Material waterMaterial)
        {
            if (pastCentralPlazaRoot == null || waterMaterial == null)
            {
                return;
            }

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2DirectionalWaterFlowReviewTroughRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2DirectionalWaterFlowReviewTroughRootName);
            root.transform.SetParent(pastCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            CreateHd2dAutonomousP2DirectionalWaterFlowReviewTroughSurface(
                root.transform,
                "P2_60_ReviewTrough_WaterRun_A",
                CentralPlazaVsCenter + new Vector3(-3.20f, 0.118f, 2.20f),
                new Vector3(1.36f, 0.055f, 0.38f),
                Quaternion.Euler(0f, -9f, 0f),
                waterMaterial,
                "past.central_plaza.p2_60.review_trough.water_run_a");
            CreateHd2dAutonomousP2DirectionalWaterFlowReviewTroughSurface(
                root.transform,
                "P2_60_ReviewTrough_WaterRun_B",
                CentralPlazaVsCenter + new Vector3(-2.04f, 0.122f, 2.48f),
                new Vector3(1.18f, 0.055f, 0.38f),
                Quaternion.Euler(0f, 18f, 0f),
                waterMaterial,
                "past.central_plaza.p2_60.review_trough.water_run_b");
            CreateHd2dAutonomousP2DirectionalWaterFlowReviewTroughSurface(
                root.transform,
                "P2_60_ReviewTrough_WaterRun_C",
                CentralPlazaVsCenter + new Vector3(-1.22f, 0.126f, 2.02f),
                new Vector3(0.92f, 0.055f, 0.34f),
                Quaternion.Euler(0f, -24f, 0f),
                waterMaterial,
                "past.central_plaza.p2_60.review_trough.water_run_c");

            EditorUtility.SetDirty(root);
        }

        private static void CreateHd2dAutonomousP2DirectionalWaterFlowReviewTroughSurface(
            Transform root,
            string objectName,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation,
            Material waterMaterial,
            string landmarkId)
        {
            var water = CreateLandmarkCube(
                objectName,
                root,
                localPosition,
                localScale,
                localRotation,
                waterMaterial,
                false,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                landmarkId);
            var landmark = water.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark != null)
            {
                SerializedSet(landmark, "countsForArrival", false);
                EditorUtility.SetDirty(landmark);
            }

            EditorUtility.SetDirty(water);
        }

        private static int CountHd2dAutonomousP2DirectionalWaterFlowReviewTroughSurfaces(Material waterMaterial)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2DirectionalWaterFlowReviewTroughRootName);
            if (root == null || waterMaterial == null)
            {
                return 0;
            }

            var renderers = root.GetComponentsInChildren<Renderer>(true);
            var count = 0;
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer != null &&
                    renderer.sharedMaterial != null &&
                    string.Equals(renderer.sharedMaterial.name, waterMaterial.name, StringComparison.Ordinal))
                {
                    count++;
                }
            }

            return count;
        }

        private static FastVsHd2dDirectionalWaterFlowProfile EnsureHd2dAutonomousP2DirectionalWaterFlowProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dDirectionalWaterFlowProfile>(Hd2dAutonomousP2DirectionalWaterFlowProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dDirectionalWaterFlowProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2DirectionalWaterFlowProfilePath);
            }

            profile.ConfigureForReview(
                EnsureHd2dAutonomousP2DirectionalWaterFlowMap(),
                0.82f,
                0.95f,
                1.25f,
                0.72f,
                1f,
                0.18f,
                true,
                true,
                false,
                "Keep P2-60 as conservative flow-map and dual-phase water motion data. Recommendation: Tom should repaint final bend directions, speed, foam density, and pond/trough stillness against approved water art.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Texture2D EnsureHd2dAutonomousP2DirectionalWaterFlowMap()
        {
            return EnsureGeneratedRepeatTexture(
                Hd2dAutonomousP2DirectionalWaterFlowMapTextureId,
                64,
                64,
                (x, y) =>
                {
                    var u = x / 63f;
                    var v = y / 63f;
                    var bend = Mathf.Sin((v * Mathf.PI * 2f) + (u * 1.35f)) * 0.32f;
                    var lateral = Mathf.Lerp(-0.18f, 0.22f, u);
                    var direction = new Vector2(0.78f + lateral, -0.38f + bend).normalized;
                    var bankFade = Mathf.SmoothStep(0.38f, 1f, 1f - Mathf.Abs(v - 0.5f) * 1.65f);
                    var centerPush = Mathf.SmoothStep(0.15f, 0.85f, u) * Mathf.SmoothStep(0.85f, 0.15f, u);
                    var strength = Mathf.Clamp01(0.72f + bankFade * 0.22f + centerPush * 0.08f);
                    return new Color(direction.x * 0.5f + 0.5f, direction.y * 0.5f + 0.5f, strength, 1f);
                });
        }

        private static void ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(
            Material material,
            FastVsHd2dDirectionalWaterFlowProfile profile,
            bool flowEnabled,
            float timeOffset)
        {
            if (material == null || profile == null)
            {
                return;
            }

            if (material.HasProperty("_FlowMap") && profile.FlowMapForReview != null)
            {
                material.SetTexture("_FlowMap", profile.FlowMapForReview);
                material.SetTextureScale("_FlowMap", Vector2.one);
            }

            if (material.HasProperty("_DirectionalFlowEnabled"))
            {
                material.SetFloat("_DirectionalFlowEnabled", flowEnabled ? 1f : 0f);
            }

            if (material.HasProperty("_FlowStrength"))
            {
                material.SetFloat("_FlowStrength", profile.FlowStrengthForReview);
            }

            if (material.HasProperty("_FlowSpeed"))
            {
                material.SetFloat("_FlowSpeed", profile.FlowSpeedForReview);
            }

            if (material.HasProperty("_FlowTimeOffset"))
            {
                material.SetFloat("_FlowTimeOffset", Mathf.Max(0f, timeOffset));
            }

            if (material.HasProperty("_FlowFoamAdvectionStrength"))
            {
                material.SetFloat("_FlowFoamAdvectionStrength", profile.FoamAdvectionStrengthForReview);
            }

            if (material.HasProperty("_FlowSpecularAdvectionStrength"))
            {
                material.SetFloat("_FlowSpecularAdvectionStrength", profile.SpecularAdvectionStrengthForReview);
            }

            if (material.HasProperty("_FlowPhaseBlendSharpness"))
            {
                material.SetFloat("_FlowPhaseBlendSharpness", profile.PhaseBlendSharpnessForReview);
            }

            EditorUtility.SetDirty(material);
        }

        private static void CaptureHd2dAutonomousP2DirectionalWaterFlowShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            string label,
            bool flowEnabled,
            float timeOffset,
            string target,
            ICollection<string> rows)
        {
            CaptureHd2dAutonomousP1WaterFountainCloseup(
                controller,
                visibility,
                guide,
                camera,
                outputDirectory,
                fileName);
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {FormatBool(flowEnabled)} | {timeOffset:0.###} | {target} |");
        }

        private static void CaptureHd2dAutonomousP2DirectionalWaterFlowReviewTroughShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName)
        {
            CaptureCloseOtherTimeReviewScreenshotWithoutPlayer(
                controller,
                visibility,
                guide,
                camera,
                FastVsHouseArea.CentralPlaza,
                CentralPlazaVsCenter + new Vector3(-3.34f, 0.02f, 1.64f),
                CentralPlazaVsCenter + new Vector3(-2.38f, 0.24f, 2.26f),
                new Vector3(0.64f, 1.62f, -2.38f),
                new Vector3(0.00f, 0.02f, 0.08f),
                outputDirectory,
                fileName);
        }

        private static void WriteHd2dAutonomousP2DirectionalWaterFlowMapPreview(Texture2D flowMap, string outputPath, int zoom)
        {
            if (flowMap == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-60 flow map preview failed: flow map is missing.");
            }

            var preview = new Texture2D(flowMap.width * zoom, flowMap.height * zoom, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            try
            {
                var source = flowMap.GetPixels32();
                var target = new Color32[preview.width * preview.height];
                for (var y = 0; y < flowMap.height; y++)
                {
                    for (var x = 0; x < flowMap.width; x++)
                    {
                        var pixel = source[(y * flowMap.width) + x];
                        for (var yy = 0; yy < zoom; yy++)
                        {
                            for (var xx = 0; xx < zoom; xx++)
                            {
                                var tx = (x * zoom) + xx;
                                var ty = (y * zoom) + yy;
                                target[(ty * preview.width) + tx] = pixel;
                            }
                        }
                    }
                }

                preview.SetPixels32(target);
                preview.Apply(false, false);
                File.WriteAllBytes(outputPath, preview.EncodeToPNG());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(preview);
            }
        }

        private static void WriteHd2dAutonomousP2DirectionalWaterFlowReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dDirectionalWaterFlowProfile profile,
            FastVsHd2dDirectionalWaterFlowDriver driver,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics baselineDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics plazaMotionDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics troughMotionDiff)
        {
            var lines = new List<string>
            {
                "# P2-60 Directional Water Flow Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative flow-map and dual-phase scroll data prep for river/trough flow while preserving the existing P1 depth-gradient water baseline.",
                "- Pass2 visibility support: a small past-plaza review trough is generated with the shared water material because the existing ruins-channel camera produced zero motion delta; this is a diagnostic surface, not final environment approval.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2DirectionalWaterFlowProfilePath}` |",
                $"| Driver | `{Hd2dAutonomousP2DirectionalWaterFlowDriverName}` |",
                $"| Water material | `{Hd2dAutonomousP1DepthGradientWaterMaterialPath}` |",
                $"| Flow map | `{AssetDatabase.GetAssetPath(profile.FlowMapForReview)}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalDirectionalFlowApprovedForReview)} |",
                $"| Flow strength / speed | {profile.FlowStrengthForReview:0.###} / {profile.FlowSpeedForReview:0.###} |",
                $"| Foam / specular advection | {profile.FoamAdvectionStrengthForReview:0.###} / {profile.SpecularAdvectionStrengthForReview:0.###} |",
                $"| Dual-phase blend sharpness / pond stillness | {profile.PhaseBlendSharpnessForReview:0.###} / {profile.PondStillnessForReview:0.###} |",
                $"| Driver last enabled / time offset | {FormatBool(driver.LastFlowEnabledForReview)} / {driver.LastTimeOffsetForReview:0.###} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                baselineDiff.ToReportRow("Flow disabled vs enabled frame A"),
                plazaMotionDiff.ToReportRow("Plaza frame A vs frame B 0.52s"),
                troughMotionDiff.ToReportRow("Review trough frame A vs frame B 0.52s"),
                string.Empty,
                "| Screenshot | Label | Flow enabled | Time offset | Target |",
                "|---|---|---|---:|---|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                $"| `{screenshotFiles[5]}` | generated RG flow-map preview (blue = local strength) | yes | n/a | Data diagnostic |",
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|"
            });

            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[i]);
                lines.Add($"| `{screenshotFiles[i]}` | P2-60 review capture {i + 1} |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "directional_water_flow_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(string directory, string beforeFile, string afterFile)
        {
            var beforePath = Path.Combine(directory, beforeFile);
            var afterPath = Path.Combine(directory, afterFile);
            var before = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            var after = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                if (!ImageConversion.LoadImage(before, File.ReadAllBytes(beforePath)) ||
                    !ImageConversion.LoadImage(after, File.ReadAllBytes(afterPath)) ||
                    before.width != after.width ||
                    before.height != after.height)
                {
                    return Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics.Empty;
                }

                var beforePixels = before.GetPixels32();
                var afterPixels = after.GetPixels32();
                long totalDelta = 0;
                var changed = 0;
                for (var i = 0; i < beforePixels.Length; i++)
                {
                    var delta = Mathf.Abs(beforePixels[i].r - afterPixels[i].r) +
                                Mathf.Abs(beforePixels[i].g - afterPixels[i].g) +
                                Mathf.Abs(beforePixels[i].b - afterPixels[i].b);
                    totalDelta += delta;
                    if (delta > 4)
                    {
                        changed++;
                    }
                }

                return new Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics(
                    beforePixels.Length,
                    changed,
                    beforePixels.Length > 0 ? changed * 100f / beforePixels.Length : 0f,
                    beforePixels.Length > 0 ? totalDelta / (float)beforePixels.Length : 0f);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(before);
                UnityEngine.Object.DestroyImmediate(after);
            }
        }

        private readonly struct Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics
        {
            public static readonly Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics Empty = new Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics(0, 0, 0f, 0f);

            private readonly int samples;
            private readonly int changedPixels;
            private readonly float changedPercent;
            private readonly float meanDelta;

            public Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics(int samples, int changedPixels, float changedPercent, float meanDelta)
            {
                this.samples = samples;
                this.changedPixels = changedPixels;
                this.changedPercent = changedPercent;
                this.meanDelta = meanDelta;
            }

            public string ToReportRow(string label)
            {
                return $"| {label} | {samples} | {changedPixels} | {changedPercent:0.###} | {meanDelta:0.###} |";
            }
        }
    }
}
