using System;
using System.Collections.Generic;
using System.IO;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2WaterReflectionRootName = "Past_CentralPlaza_P2_WaterReflectionReview";
        private const string Hd2dAutonomousP2WaterReflectionProfilePath = "Assets/Settings/FastVS_HD2D_P2_WaterReflectionProfile.asset";
        private const string Hd2dAutonomousP2WaterReflectionProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dWaterReflectionProfile.cs";
        private const string Hd2dAutonomousP2WaterReflectionProbeCubemapPath = TextureDirectory + "/FastVS_House_hd2d_p2_water_reflection_review_probe_cubemap.asset";

        public static void CaptureHd2dAutonomousP2Item64WaterReflectionBatch()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterReflectionRootName) == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-64 water reflection capture failed: review root is missing. Run BuildAndValidateBatch before capture.");
            }

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            var profile = EnsureHd2dAutonomousP2WaterReflectionProfile();
            if (controller == null || visibility == null || guide == null || camera == null || waterMaterial == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-64 water reflection capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2WaterReflection();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("tilted_camera_water_reflection");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_reflection_disabled_flat_water.png",
                "02_probe_reflection_conservative_grazing.png",
                "03_probe_reflection_context_tall_geometry.png",
                "04_reflection_close_grazing_edge.png",
                "05_reflection_stronger_option_for_tom.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                SetHd2dAutonomousP1WaterDepthGradientForReview(waterMaterial, Hd2dAutonomousP1WaterDepthGradientStrength);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength * 0.34f, 0f);
                SetHd2dAutonomousP1WaterSpecularForReview(waterMaterial, Hd2dAutonomousP1WaterSpecularStrength * 0.42f);
                ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2DirectionalWaterFlowProfile(), false, 0f);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2FakeRefractionProfile(), false, 0f);
                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2ToonWaterMotionProfile(), false, false, 0f);

                ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(waterMaterial, profile, false, 1f);
                CaptureHd2dAutonomousP2WaterReflectionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[0],
                    false,
                    false,
                    "reflection disabled baseline: still water stays flatter and darker",
                    "off",
                    0f,
                    shotRows);

                ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(waterMaterial, profile, true, 1f);
                CaptureHd2dAutonomousP2WaterReflectionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[1],
                    false,
                    false,
                    "conservative boxed-probe reflection at tilted-camera grazing edge",
                    "conservative",
                    profile.ReflectionStrengthForReview,
                    shotRows);

                ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(waterMaterial, profile, true, 1f);
                CaptureHd2dAutonomousP2WaterReflectionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[2],
                    true,
                    false,
                    "context view: tall warm/cool reflection targets plus still pond",
                    "conservative",
                    profile.ReflectionStrengthForReview,
                    shotRows);

                ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(waterMaterial, profile, true, 1f);
                CaptureHd2dAutonomousP2WaterReflectionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[3],
                    false,
                    true,
                    "close grazing edge check: Fresnel stronger toward the far edge",
                    "conservative",
                    profile.ReflectionStrengthForReview,
                    shotRows);

                ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(waterMaterial, profile, true, 1.38f);
                CaptureHd2dAutonomousP2WaterReflectionShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[4],
                    false,
                    false,
                    "stronger A/B option for Tom: same probe with higher strength multiplier",
                    "stronger option",
                    profile.ReflectionStrengthForReview * 1.38f,
                    shotRows);

                var reflectionDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
                var contextDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[2]);
                var strengthDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[4]);
                WriteHd2dAutonomousP2WaterReflectionReviewReport(outputDirectory, screenshotFiles, shotRows, profile, reflectionDiff, contextDiff, strengthDiff);
            }
            finally
            {
                SetHd2dAutonomousP2WaterReflectionReviewVisible(true);
                SetHd2dAutonomousP2CausticsReviewVisible(true);
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(waterMaterial, profile, true, 1f);
                ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2DirectionalWaterFlowProfile(), true, 0f);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2FakeRefractionProfile(), true, 0f);
                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2ToonWaterMotionProfile(), true, true, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-64 water reflection review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2WaterReflection(Transform pastCentralPlazaRoot)
        {
            var profile = EnsureHd2dAutonomousP2WaterReflectionProfile();
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(waterMaterial, profile, true, 1f);
            CreateHd2dAutonomousP2WaterReflectionReviewSet(pastCentralPlazaRoot, waterMaterial, profile);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2WaterReflection()
        {
            var profile = EnsureHd2dAutonomousP2WaterReflectionProfile();
            var material = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(material, profile, true, 1f);

            if (profile == null ||
                !profile.NeedsTomApprovalForReview ||
                !profile.PlanarReflectionDeferredForReview ||
                profile.FinalWaterReflectionApprovedForReview ||
                profile.ReflectionStrengthForReview <= 0.10f ||
                profile.FresnelBiasForReview <= 0.01f ||
                profile.ProbeResolutionForReview < 64)
            {
                throw new InvalidOperationException("House slice validation failed: P2-64 water reflection needs conservative non-final probe data for Tom review.");
            }

            foreach (var propertyName in new[]
            {
                "_ReflectionEnabled",
                "_ReflectionStrength",
                "_ReflectionFresnelPower",
                "_ReflectionFresnelBias",
                "_ReflectionRoughness",
                "_ReflectionTint",
                "_ReflectionSkyFallback"
            })
            {
                if (material == null || !material.HasProperty(propertyName))
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-64 water material missing property {propertyName}.");
                }
            }

            var shaderSource = File.ReadAllText(Hd2dAutonomousP1DepthGradientWaterShaderPath);
            foreach (var token in new[]
            {
                "_ReflectionEnabled",
                "_ReflectionFresnelPower",
                "GlossyEnvironmentReflection",
                "reflect(-viewDirWS, normalWS)",
                "_ReflectionSkyFallback"
            })
            {
                ValidateSourceToken(shaderSource, token, Hd2dAutonomousP1DepthGradientWaterShaderPath);
            }

            if (!File.Exists(Hd2dAutonomousP2WaterReflectionProfileRuntimePath) ||
                CountHd2dAutonomousP2WaterReflectionRenderers("WaterSurface") < 1 ||
                CountHd2dAutonomousP2WaterReflectionRenderers("TallReflection") < 2 ||
                CountHd2dAutonomousP2WaterReflectionProbes() < 1)
            {
                throw new InvalidOperationException("House slice validation failed: P2-64 review set requires runtime profile source, water surface, tall reflection targets, and a boxed Reflection Probe.");
            }
        }

        private static void CreateHd2dAutonomousP2WaterReflectionReviewSet(
            Transform pastCentralPlazaRoot,
            Material waterMaterial,
            FastVsHd2dWaterReflectionProfile profile)
        {
            if (pastCentralPlazaRoot == null || waterMaterial == null || profile == null)
            {
                return;
            }

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterReflectionRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2WaterReflectionRootName);
            root.transform.SetParent(pastCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            var floorMaterial = EnsureHd2dAutonomousP2WaterReflectionSolidMaterial("hd2d_p2_water_reflection_dark_floor", new Color(0.055f, 0.085f, 0.095f, 1f));
            var warmTowerMaterial = EnsureHd2dAutonomousP2WaterReflectionSolidMaterial("hd2d_p2_water_reflection_tall_warm", new Color(0.95f, 0.70f, 0.34f, 1f));
            var coolTowerMaterial = EnsureHd2dAutonomousP2WaterReflectionSolidMaterial("hd2d_p2_water_reflection_tall_cool", new Color(0.32f, 0.58f, 0.98f, 1f));
            var paleTowerMaterial = EnsureHd2dAutonomousP2WaterReflectionSolidMaterial("hd2d_p2_water_reflection_tall_pale", new Color(0.78f, 0.94f, 0.96f, 1f));
            var shoreMaterial = EnsureHd2dAutonomousP2WaterReflectionSolidMaterial("hd2d_p2_water_reflection_shore_marker", new Color(0.32f, 0.29f, 0.20f, 1f));

            var probe = CreateHd2dAutonomousP2WaterReflectionProbe(root.transform, profile);

            CreateHd2dAutonomousP2WaterReflectionCube(
                root.transform,
                "P2_64_WaterReflection_DarkFloor",
                CentralPlazaVsCenter + new Vector3(10.42f, 0.118f, 2.16f),
                new Vector3(2.28f, 0.035f, 1.08f),
                Quaternion.Euler(0f, -8f, 0f),
                floorMaterial,
                "past.central_plaza.p2_64.dark_floor",
                null);

            var water = CreateHd2dAutonomousP2WaterReflectionCube(
                root.transform,
                "P2_64_WaterReflection_WaterSurface",
                CentralPlazaVsCenter + new Vector3(10.42f, 0.186f, 2.16f),
                new Vector3(2.18f, 0.045f, 1.00f),
                Quaternion.Euler(0f, -8f, 0f),
                waterMaterial,
                "past.central_plaza.p2_64.water_surface",
                probe.transform);

            CreateHd2dAutonomousP2WaterReflectionCube(
                root.transform,
                "P2_64_WaterReflection_ShoreMarker",
                CentralPlazaVsCenter + new Vector3(10.42f, 0.210f, 2.76f),
                new Vector3(2.30f, 0.030f, 0.045f),
                Quaternion.Euler(0f, -8f, 0f),
                shoreMaterial,
                "past.central_plaza.p2_64.shore_marker",
                null);

            CreateHd2dAutonomousP2WaterReflectionCube(
                root.transform,
                "P2_64_TallReflection_WarmTower",
                CentralPlazaVsCenter + new Vector3(9.64f, 0.92f, 3.16f),
                new Vector3(0.24f, 1.42f, 0.24f),
                Quaternion.Euler(0f, -8f, 0f),
                warmTowerMaterial,
                "past.central_plaza.p2_64.tall_reflection_warm",
                null);

            CreateHd2dAutonomousP2WaterReflectionCube(
                root.transform,
                "P2_64_TallReflection_CoolTower",
                CentralPlazaVsCenter + new Vector3(10.42f, 1.02f, 3.26f),
                new Vector3(0.28f, 1.62f, 0.24f),
                Quaternion.Euler(0f, -8f, 0f),
                coolTowerMaterial,
                "past.central_plaza.p2_64.tall_reflection_cool",
                null);

            CreateHd2dAutonomousP2WaterReflectionCube(
                root.transform,
                "P2_64_TallReflection_PaleSkyPanel",
                CentralPlazaVsCenter + new Vector3(11.18f, 0.86f, 3.06f),
                new Vector3(0.34f, 1.20f, 0.20f),
                Quaternion.Euler(0f, -8f, 0f),
                paleTowerMaterial,
                "past.central_plaza.p2_64.tall_reflection_pale",
                null);

            ConfigureHd2dAutonomousP2WaterReflectionRenderer(water.GetComponent<Renderer>(), probe.transform);
            EditorUtility.SetDirty(root);
        }

        private static GameObject CreateHd2dAutonomousP2WaterReflectionCube(
            Transform root,
            string objectName,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation,
            Material material,
            string landmarkId,
            Transform probeAnchor)
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

            ConfigureHd2dAutonomousP2WaterReflectionRenderer(cube.GetComponent<Renderer>(), probeAnchor);
            EditorUtility.SetDirty(cube);
            return cube;
        }

        private static ReflectionProbe CreateHd2dAutonomousP2WaterReflectionProbe(Transform root, FastVsHd2dWaterReflectionProfile profile)
        {
            var probeObject = new GameObject("P2_64_WaterReflection_BoxedProbe");
            probeObject.transform.SetParent(root, false);
            probeObject.transform.localPosition = CentralPlazaVsCenter + new Vector3(10.42f, 0.82f, 2.46f);
            probeObject.transform.localRotation = Quaternion.identity;
            probeObject.transform.localScale = Vector3.one;

            var probe = probeObject.AddComponent<ReflectionProbe>();
            probe.mode = ReflectionProbeMode.Custom;
            probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;
            probe.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
            probe.clearFlags = ReflectionProbeClearFlags.Skybox;
            probe.customBakedTexture = EnsureHd2dAutonomousP2WaterReflectionProbeCubemap();
            probe.boxProjection = true;
            probe.center = Vector3.zero;
            probe.size = profile.ProbeBoxSizeForReview;
            probe.resolution = profile.ProbeResolutionForReview;
            probe.hdr = true;
            probe.intensity = 1.0f;
            probe.importance = 4;
            probe.blendDistance = 0.75f;
            probe.nearClipPlane = 0.05f;
            probe.farClipPlane = 30f;
            probe.cullingMask = ~0;
            EditorUtility.SetDirty(probe);
            EditorUtility.SetDirty(probeObject);
            return probe;
        }

        private static Cubemap EnsureHd2dAutonomousP2WaterReflectionProbeCubemap()
        {
            const int size = 128;
            var cubemap = AssetDatabase.LoadAssetAtPath<Cubemap>(Hd2dAutonomousP2WaterReflectionProbeCubemapPath);
            if (cubemap == null || cubemap.width != size)
            {
                if (cubemap != null)
                {
                    AssetDatabase.DeleteAsset(Hd2dAutonomousP2WaterReflectionProbeCubemapPath);
                }

                cubemap = new Cubemap(size, TextureFormat.RGBA32, false)
                {
                    name = "FastVS_House_hd2d_p2_water_reflection_review_probe_cubemap"
                };
                AssetDatabase.CreateAsset(cubemap, Hd2dAutonomousP2WaterReflectionProbeCubemapPath);
            }

            var sky = new Color(0.36f, 0.54f, 0.78f, 1f);
            var lowSky = new Color(0.28f, 0.48f, 0.68f, 1f);
            var waterDark = new Color(0.035f, 0.11f, 0.16f, 1f);
            var warm = new Color(0.80f, 0.47f, 0.18f, 1f);
            var cool = new Color(0.20f, 0.40f, 0.74f, 1f);
            var pale = new Color(0.66f, 0.84f, 0.88f, 1f);

            PaintHd2dAutonomousP2WaterReflectionProbeFace(cubemap, CubemapFace.PositiveY, size, sky, lowSky, warm, pale, cool, true);
            PaintHd2dAutonomousP2WaterReflectionProbeFace(cubemap, CubemapFace.NegativeY, size, waterDark, waterDark, warm * 0.35f, cool * 0.35f, pale * 0.35f, false);
            PaintHd2dAutonomousP2WaterReflectionProbeFace(cubemap, CubemapFace.PositiveZ, size, lowSky, sky, warm, cool, pale, true);
            PaintHd2dAutonomousP2WaterReflectionProbeFace(cubemap, CubemapFace.NegativeZ, size, lowSky, sky, cool, pale, warm, true);
            PaintHd2dAutonomousP2WaterReflectionProbeFace(cubemap, CubemapFace.PositiveX, size, lowSky, sky, pale, warm, cool, true);
            PaintHd2dAutonomousP2WaterReflectionProbeFace(cubemap, CubemapFace.NegativeX, size, lowSky, sky, cool, warm, pale, true);
            cubemap.Apply(false, false);
            EditorUtility.SetDirty(cubemap);
            AssetDatabase.SaveAssets();
            return cubemap;
        }

        private static void PaintHd2dAutonomousP2WaterReflectionProbeFace(
            Cubemap cubemap,
            CubemapFace face,
            int size,
            Color lowerColor,
            Color upperColor,
            Color markerA,
            Color markerB,
            Color markerC,
            bool includeTallMarkers)
        {
            var pixels = new Color[size * size];
            for (var y = 0; y < size; y++)
            {
                var v = y / (float)(size - 1);
                for (var x = 0; x < size; x++)
                {
                    var color = Color.Lerp(lowerColor, upperColor, Mathf.SmoothStep(0f, 1f, v));
                    if (includeTallMarkers && v > 0.16f && v < 0.88f)
                    {
                        var markerColor = Color.clear;
                        if (x > size * 0.06f && x < size * 0.32f)
                        {
                            markerColor = markerA;
                        }
                        else if (x > size * 0.36f && x < size * 0.64f)
                        {
                            markerColor = markerB;
                        }
                        else if (x > size * 0.68f && x < size * 0.94f)
                        {
                            markerColor = markerC;
                        }

                        if (markerColor.a > 0f)
                        {
                            var verticalMask = Mathf.SmoothStep(0.06f, 0.18f, v) * (1f - Mathf.SmoothStep(0.88f, 0.98f, v));
                            color = Color.Lerp(color, markerColor, 0.85f * verticalMask);
                        }
                    }

                    pixels[y * size + x] = color;
                }
            }

            cubemap.SetPixels(pixels, face);
        }

        private static FastVsHd2dWaterReflectionProfile EnsureHd2dAutonomousP2WaterReflectionProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dWaterReflectionProfile>(Hd2dAutonomousP2WaterReflectionProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dWaterReflectionProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2WaterReflectionProfilePath);
            }

            profile.ConfigureForReview(
                0.62f,
                1.65f,
                0.14f,
                0.12f,
                new Color(0.96f, 0.99f, 1.00f, 1f),
                0.24f,
                new Vector3(5.8f, 3.2f, 4.4f),
                128,
                true,
                true,
                false,
                "Conservative P2-64 boxed-probe reflection baseline using a custom review cubemap derived from warm/cool/pale tall marker colors. Tom should tune strength/Fresnel/roughness against final still ponds, and only add planar reflection to named hero ponds if the doubled reflected draws are acceptable.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Material EnsureHd2dAutonomousP2WaterReflectionSolidMaterial(string materialId, Color color)
        {
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var shader = Shader.Find(SurfaceRampLitShaderName) ?? Shader.Find(URPLitShaderName) ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Required P2-64 water reflection review material shader not found: {materialId}.");
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

        private static void ConfigureHd2dAutonomousP2WaterReflectionMaterialForReview(
            Material material,
            FastVsHd2dWaterReflectionProfile profile,
            bool enabled,
            float strengthMultiplier)
        {
            if (material == null || profile == null)
            {
                return;
            }

            SetMaterialFloatIfPresent(material, "_ReflectionEnabled", enabled ? 1f : 0f);
            SetMaterialFloatIfPresent(material, "_ReflectionStrength", profile.ReflectionStrengthForReview * Mathf.Max(0f, strengthMultiplier));
            SetMaterialFloatIfPresent(material, "_ReflectionFresnelPower", profile.FresnelPowerForReview);
            SetMaterialFloatIfPresent(material, "_ReflectionFresnelBias", profile.FresnelBiasForReview);
            SetMaterialFloatIfPresent(material, "_ReflectionRoughness", profile.RoughnessForReview);
            SetMaterialColorIfPresent(material, "_ReflectionTint", profile.ReflectionTintForReview);
            SetMaterialFloatIfPresent(material, "_ReflectionSkyFallback", profile.SkyFallbackForReview);
            EditorUtility.SetDirty(material);
        }

        private static void ConfigureHd2dAutonomousP2WaterReflectionRenderer(Renderer renderer, Transform probeAnchor)
        {
            if (renderer == null)
            {
                return;
            }

            renderer.reflectionProbeUsage = ReflectionProbeUsage.BlendProbesAndSkybox;
            renderer.probeAnchor = probeAnchor;
            renderer.receiveShadows = false;
            EditorUtility.SetDirty(renderer);
        }

        private static void CaptureHd2dAutonomousP2WaterReflectionShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            bool contextWide,
            bool closeGrazing,
            string label,
            string state,
            float strength,
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

            var playerLocalPosition = CentralPlazaVsCenter + new Vector3(9.82f, 0.02f, 1.38f);
            var anchorLocalPosition = CentralPlazaVsCenter + (contextWide
                ? new Vector3(10.42f, 0.62f, 2.36f)
                : closeGrazing
                    ? new Vector3(10.42f, 0.27f, 2.44f)
                    : new Vector3(10.42f, 0.32f, 2.26f));
            var cameraOffset = contextWide
                ? new Vector3(0.96f, 1.78f, -2.72f)
                : closeGrazing
                    ? new Vector3(0.34f, 0.78f, -1.34f)
                    : new Vector3(0.72f, 1.18f, -1.92f);
            var lookOffset = contextWide
                ? new Vector3(0.00f, 0.10f, 0.10f)
                : closeGrazing
                    ? new Vector3(0.00f, 0.02f, 0.16f)
                    : new Vector3(0.00f, 0.04f, 0.14f);

            controller.ForcePlayerOtherTimeLocalForReview(playerLocalPosition);
            guide.ApplyActiveTimeIsolationForReview();
            var previousMask = camera.cullingMask;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            try
            {
                camera.cullingMask = (previousMask & ~currentBit) | otherBit | playerBit;
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(false);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(false);
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(false);
                SetHd2dAutonomousP2CausticsReviewVisible(false);
                SetHd2dAutonomousP2WaterReflectionReviewVisible(true);
                UpdateHd2dAutonomousP2WaterReflectionProbeForReview();
                PositionCloseReviewCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
                SetHd2dAutonomousP2CausticsReviewVisible(true);
                SetHd2dAutonomousP2WaterReflectionReviewVisible(true);
                if (hasPlayer)
                {
                    controller.ForcePlayerCurrentLocalForReview(previousPlayerLocal);
                }

                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | {state} | {strength:0.###} |");
        }

        private static void UpdateHd2dAutonomousP2WaterReflectionProbeForReview()
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterReflectionRootName);
            if (root == null)
            {
                return;
            }

            var probes = root.GetComponentsInChildren<ReflectionProbe>(true);
            for (var i = 0; i < probes.Length; i++)
            {
                var probe = probes[i];
                if (probe == null)
                {
                    continue;
                }

                probe.mode = ReflectionProbeMode.Custom;
                probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;
                probe.customBakedTexture = EnsureHd2dAutonomousP2WaterReflectionProbeCubemap();
                EditorUtility.SetDirty(probe);
            }
        }

        private static void SetHd2dAutonomousP2WaterReflectionReviewVisible(bool visible)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterReflectionRootName);
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

        private static int CountHd2dAutonomousP2WaterReflectionRenderers(string nameToken)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterReflectionRootName);
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

        private static int CountHd2dAutonomousP2WaterReflectionProbes()
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterReflectionRootName);
            return root == null ? 0 : root.GetComponentsInChildren<ReflectionProbe>(true).Length;
        }

        private static void WriteHd2dAutonomousP2WaterReflectionReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dWaterReflectionProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics reflectionDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics contextDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics strengthDiff)
        {
            var lines = new List<string>
            {
                "# P2-64 Tilted-Camera Water Reflection Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative boxed Reflection Probe baseline for still water seen by the tilted camera; planar reflection is documented as deferred for hero ponds only.",
                "- Review setup: a past-plaza diagnostic pond sits beside warm/cool/pale tall markers inside one box-projected custom review Reflection Probe cubemap.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2WaterReflectionProfilePath}` |",
                $"| Water material | `{Hd2dAutonomousP1DepthGradientWaterMaterialPath}` |",
                $"| Review root | `{Hd2dAutonomousP2WaterReflectionRootName}` |",
                $"| Probe cubemap | `{Hd2dAutonomousP2WaterReflectionProbeCubemapPath}` |",
                $"| Needs Tom approval | {FormatBool(profile.NeedsTomApprovalForReview)} |",
                $"| Final approved | {FormatBool(profile.FinalWaterReflectionApprovedForReview)} |",
                $"| Planar reflection deferred | {FormatBool(profile.PlanarReflectionDeferredForReview)} |",
                $"| Reflection strength / Fresnel power / bias | {profile.ReflectionStrengthForReview:0.###} / {profile.FresnelPowerForReview:0.###} / {profile.FresnelBiasForReview:0.###} |",
                $"| Roughness / sky fallback | {profile.RoughnessForReview:0.###} / {profile.SkyFallbackForReview:0.###} |",
                $"| Probe box size / resolution | {profile.ProbeBoxSizeForReview} / {profile.ProbeResolutionForReview} |",
                $"| Water / tall-target renderers / probes | {CountHd2dAutonomousP2WaterReflectionRenderers("WaterSurface")} / {CountHd2dAutonomousP2WaterReflectionRenderers("TallReflection")} / {CountHd2dAutonomousP2WaterReflectionProbes()} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                reflectionDiff.ToReportRow("Reflection off vs conservative probe"),
                contextDiff.ToReportRow("Reflection off vs context view"),
                strengthDiff.ToReportRow("Conservative vs stronger option"),
                string.Empty,
                "| Screenshot | Label | Reflection state | Strength |",
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
                var file = screenshotFiles[i];
                ValidateScreenshotOutputExists(outputDirectory, file);
                lines.Add($"| `{file}` | P2-64 boxed-probe water reflection capture {i + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "tilted_camera_water_reflection_review.md"), string.Join(Environment.NewLine, lines));
        }
    }
}
