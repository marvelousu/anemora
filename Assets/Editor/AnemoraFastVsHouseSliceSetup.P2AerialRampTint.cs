using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        private const string Hd2dAutonomousP2AerialRampTintProfilePath = "Assets/Settings/FastVS_HD2D_P2_AerialRampTintProfile.asset";
        private const string Hd2dAutonomousP2AerialRampUseProperty = "_UseAerialRampTint";
        private const string Hd2dAutonomousP2AerialRampTintGlobalName = "_AerialTint";
        private const string Hd2dAutonomousP2AerialRampTintDistanceGlobalName = "_AerialTintDistance";
        private const string Hd2dAutonomousP2AerialRampTintStrengthGlobalName = "_AerialTintStrength";
        private const string Hd2dAutonomousP2AerialRampTintProbeMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p1_modular_building_wall.mat";
        private const string Hd2dAutonomousP2AerialRampTintProbeRootName = "FastVS_HD2D_P2_AerialRampTintReviewProbes";

        private static readonly string[] Hd2dAutonomousP2AerialRampTintMaterialPaths =
        {
            MaterialDirectory + "/FastVS_House_current_ground.mat",
            MaterialDirectory + "/FastVS_House_current_path.mat",
            MaterialDirectory + "/FastVS_House_current_grass.mat",
            MaterialDirectory + "/FastVS_House_past_path.mat",
            MaterialDirectory + "/FastVS_House_past_grass.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p0_vertex_splat_ground.mat",
            MaterialDirectory + "/FastVS_House_current_exterior_wall.mat",
            MaterialDirectory + "/FastVS_House_past_exterior_wall.mat",
            MaterialDirectory + "/FastVS_House_current_roof.mat",
            MaterialDirectory + "/FastVS_House_past_roof.mat",
            MaterialDirectory + "/FastVS_House_current_stone.mat",
            MaterialDirectory + "/FastVS_House_past_stone.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p1_modular_building_wall.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p1_modular_building_roof.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p1_modular_building_trim.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p1_modular_building_floor.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p1_building_roof_tile_edge.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p1_building_wall_timber_frame.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p1_building_wall_cavity_accent.mat"
        };

        public static void CaptureHd2dAutonomousP2Item56AerialRampTintBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHouseLightingDirector>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var atmosphericDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAtmosphericPerspectiveDriver>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || director == null ||
                realtimeRig == null || atmosphericDriver == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-56 aerial ramp tint capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2AerialRampTint();
            var profile = EnsureHd2dAutonomousP2AerialRampTintProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("aerial_ramp_tint");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_current_aerial_tint_off_receding_buildings.png",
                "02_current_aerial_tint_on_receding_buildings.png",
                "03_past_aerial_tint_off_receding_buildings.png",
                "04_past_aerial_tint_on_receding_buildings.png"
            };

            var previousCullingMask = camera.cullingMask;
            try
            {
                SetHd2dAutonomousP0AtmosphericPerspectiveMaterialIntensity(0f);
                CaptureHd2dAutonomousP2AerialRampTintShot(controller, visibility, guide, director, realtimeRig, atmosphericDriver, camera, profile, false, false, previousCullingMask, outputDirectory, screenshotFiles[0]);
                CaptureHd2dAutonomousP2AerialRampTintShot(controller, visibility, guide, director, realtimeRig, atmosphericDriver, camera, profile, true, false, previousCullingMask, outputDirectory, screenshotFiles[1]);
                CaptureHd2dAutonomousP2AerialRampTintShot(controller, visibility, guide, director, realtimeRig, atmosphericDriver, camera, profile, false, true, previousCullingMask, outputDirectory, screenshotFiles[2]);
                CaptureHd2dAutonomousP2AerialRampTintShot(controller, visibility, guide, director, realtimeRig, atmosphericDriver, camera, profile, true, true, previousCullingMask, outputDirectory, screenshotFiles[3]);
            }
            finally
            {
                camera.cullingMask = previousCullingMask;
                SetHd2dAutonomousP0AtmosphericPerspectiveMaterialIntensity(1f);
                ApplyHd2dAutonomousP2AerialRampTintToMaterials(profile, true);
                ConfigureHd2dAutonomousP2AerialRampTintDriver(profile);
                atmosphericDriver.PublishCurrentForReview();
                AssetDatabase.SaveAssets();
            }

            var currentDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                4);
            var pastDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[2]),
                Path.Combine(outputDirectory, screenshotFiles[3]),
                4);
            WriteHd2dAutonomousP2AerialRampTintReviewReport(outputDirectory, screenshotFiles, profile, currentDiff, pastDiff);

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-56 aerial ramp tint review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2AerialRampTint()
        {
            var profile = EnsureHd2dAutonomousP2AerialRampTintProfile();
            ApplyHd2dAutonomousP2AerialRampTintToMaterials(profile, true);
            ConfigureHd2dAutonomousP2AerialRampTintDriver(profile);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2AerialRampTint()
        {
            var profile = EnsureHd2dAutonomousP2AerialRampTintProfile();
            if (!profile.needsTomApproval || profile.finalAerialTintApproved)
            {
                throw new InvalidOperationException("House slice validation failed: P2-56 aerial ramp tint profile must remain NEEDS-TOM and final approval must stay false.");
            }

            if (!profile.enableGroundAndBuildingMaterials ||
                profile.strength <= 0f ||
                profile.strength > 0.22f ||
                profile.distanceStartOffset < 0f ||
                profile.distanceStartOffset > 4f ||
                profile.distanceEndOffset < profile.distanceStartOffset + 0.5f ||
                profile.distanceEndOffset > 10f)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-56 aerial ramp tint profile is outside conservative bounds. strength={profile.strength:0.###}, startOffset={profile.distanceStartOffset:0.###}, endOffset={profile.distanceEndOffset:0.###}");
            }

            AssetDatabase.ImportAsset(SurfaceRampLitShaderPath, ImportAssetOptions.ForceSynchronousImport);
            var shaderSource = File.Exists(SurfaceRampLitShaderPath) ? File.ReadAllText(SurfaceRampLitShaderPath) : string.Empty;
            ValidateSourceToken(shaderSource, Hd2dAutonomousP2AerialRampUseProperty, SurfaceRampLitShaderPath);
            ValidateSourceToken(shaderSource, Hd2dAutonomousP2AerialRampTintGlobalName, SurfaceRampLitShaderPath);
            ValidateSourceToken(shaderSource, Hd2dAutonomousP2AerialRampTintDistanceGlobalName, SurfaceRampLitShaderPath);
            ValidateSourceToken(shaderSource, "aerialPreservedLuma", SurfaceRampLitShaderPath);

            var driverPath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dAtmosphericPerspectiveDriver.cs");
            var driverSource = File.Exists(driverPath) ? File.ReadAllText(driverPath) : string.Empty;
            ValidateSourceToken(driverSource, Hd2dAutonomousP2AerialRampTintGlobalName, driverPath);
            ValidateSourceToken(driverSource, Hd2dAutonomousP2AerialRampTintDistanceGlobalName, driverPath);
            ValidateSourceToken(driverSource, "settings.farColor.value", driverPath);

            var driver = FindHd2dAutonomousP0AtmosphericPerspectiveDriver();
            if (driver == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-56 scene is missing the atmospheric driver used to publish aerial ramp tint globals.");
            }

            var serializedDriver = new SerializedObject(driver);
            if (!serializedDriver.FindProperty("publishAerialRampTint").boolValue ||
                Mathf.Abs(serializedDriver.FindProperty("aerialRampTintStrength").floatValue - profile.strength) > 0.001f ||
                (serializedDriver.FindProperty("aerialRampTintDistancePadding").vector2Value - new Vector2(profile.distanceStartOffset, profile.distanceEndOffset)).sqrMagnitude > 0.0001f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-56 atmospheric driver is not publishing the staged aerial ramp tint profile.");
            }

            for (var i = 0; i < Hd2dAutonomousP2AerialRampTintMaterialPaths.Length; i++)
            {
                var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2AerialRampTintMaterialPaths[i]);
                if (material == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-56 expected ground/building material is missing: {Hd2dAutonomousP2AerialRampTintMaterialPaths[i]}");
                }

                if (!material.HasProperty(Hd2dAutonomousP2AerialRampUseProperty) ||
                    !material.HasProperty(Hd2dAutonomousP2AerialRampTintGlobalName) ||
                    !material.HasProperty(Hd2dAutonomousP2AerialRampTintDistanceGlobalName) ||
                    !material.HasProperty(Hd2dAutonomousP2AerialRampTintStrengthGlobalName) ||
                    Mathf.Abs(material.GetFloat(Hd2dAutonomousP2AerialRampUseProperty) - 1f) > 0.001f ||
                    Mathf.Abs(material.GetFloat(Hd2dAutonomousP2AerialRampTintStrengthGlobalName) - profile.strength) > 0.001f)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-56 material is not opt-in for aerial ramp tint: {Hd2dAutonomousP2AerialRampTintMaterialPaths[i]}");
                }
            }

            _ = ReadHd2dAutonomousP2AerialRampTintSettings(false, profile);
            _ = ReadHd2dAutonomousP2AerialRampTintSettings(true, profile);
        }

        private static FastVsHd2dAerialRampTintProfile EnsureHd2dAutonomousP2AerialRampTintProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAerialRampTintProfile>(Hd2dAutonomousP2AerialRampTintProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dAerialRampTintProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2AerialRampTintProfilePath);
            }

            profile.strength = 0.16f;
            profile.distanceStartOffset = 1.0f;
            profile.distanceEndOffset = 5.0f;
            profile.enableGroundAndBuildingMaterials = true;
            profile.needsTomApproval = true;
            profile.finalAerialTintApproved = false;
            if (TryReadHd2dAutonomousP2AerialRampFogFarColor(false, out var currentTint))
            {
                profile.currentTint = currentTint;
            }

            if (TryReadHd2dAutonomousP2AerialRampFogFarColor(true, out var pastTint))
            {
                profile.pastTint = pastTint;
            }

            profile.reviewNotes =
                "Conservative P2-56 shader-side aerial tint data prep. The atmospheric driver publishes Volume farColor/distance globals; Tom should tune strength and distance offsets before final approval.";
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static bool TryReadHd2dAutonomousP2AerialRampFogFarColor(bool past, out Color farColor)
        {
            farColor = Color.white;
            var profilePath = past ? Hd2dAutonomousP0AtmosphericPastProfilePath : Hd2dAutonomousP0AtmosphericCurrentProfilePath;
            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(profilePath);
            if (!FastVsHd2dAtmosphericPerspectiveDriver.TryReadSettings(volumeProfile, out var settings))
            {
                return false;
            }

            farColor = settings.farColor.value;
            return true;
        }

        private static void ConfigureHd2dAutonomousP2AerialRampTintDriver(FastVsHd2dAerialRampTintProfile profile)
        {
            var driver = FindHd2dAutonomousP0AtmosphericPerspectiveDriver();
            if (driver == null)
            {
                return;
            }

            var serializedDriver = new SerializedObject(driver);
            serializedDriver.FindProperty("publishAerialRampTint").boolValue = true;
            serializedDriver.FindProperty("aerialRampTintStrength").floatValue = profile.strength;
            serializedDriver.FindProperty("aerialRampTintDistancePadding").vector2Value = new Vector2(profile.distanceStartOffset, profile.distanceEndOffset);
            serializedDriver.ApplyModifiedPropertiesWithoutUndo();
            driver.PublishCurrentForReview();
            EditorUtility.SetDirty(driver);
        }

        private static void ApplyHd2dAutonomousP2AerialRampTintToMaterials(FastVsHd2dAerialRampTintProfile profile, bool enabled)
        {
            var settings = ReadHd2dAutonomousP2AerialRampTintSettings(false, profile);
            for (var i = 0; i < Hd2dAutonomousP2AerialRampTintMaterialPaths.Length; i++)
            {
                var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2AerialRampTintMaterialPaths[i]);
                if (material == null || !material.HasProperty(Hd2dAutonomousP2AerialRampUseProperty))
                {
                    continue;
                }

                material.SetFloat(Hd2dAutonomousP2AerialRampUseProperty, enabled && profile.enableGroundAndBuildingMaterials ? 1f : 0f);
                material.SetColor(Hd2dAutonomousP2AerialRampTintGlobalName, settings.Tint);
                material.SetVector(Hd2dAutonomousP2AerialRampTintDistanceGlobalName, new Vector4(settings.DistanceStart, settings.DistanceEnd, 0f, 0f));
                material.SetFloat(Hd2dAutonomousP2AerialRampTintStrengthGlobalName, profile.strength);
                EditorUtility.SetDirty(material);
            }
        }

        private static void CaptureHd2dAutonomousP2AerialRampTintShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsHouseLightingDirector director,
            FastVsRealtimeLightShadowRig realtimeRig,
            FastVsHd2dAtmosphericPerspectiveDriver atmosphericDriver,
            Camera camera,
            FastVsHd2dAerialRampTintProfile profile,
            bool enabled,
            bool pastTimeline,
            int previousCullingMask,
            string outputDirectory,
            string fileName)
        {
            ApplyHd2dAutonomousP2AerialRampTintToMaterials(profile, enabled);
            ConfigureHd2dAutonomousP2AerialRampTintDriver(profile);
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            director.ApplyAreaForReview(FastVsHouseArea.CentralPlaza);
            var playerLocal = CentralPlazaVsCenter + new Vector3(-0.10f, 0.02f, 4.62f);
            var anchorLocal = CentralPlazaVsCenter + new Vector3(0.00f, 0.92f, 7.64f);
            var cameraOffset = new Vector3(0.00f, 2.30f, -7.28f);
            var lookOffset = new Vector3(0.00f, 0.20f, 0.54f);

            if (pastTimeline)
            {
                controller.ForcePlayerOtherTimeLocalForReview(playerLocal);
                var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
                var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
                var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
                camera.cullingMask = (previousCullingMask & ~currentBit) | otherBit | playerBit;
                PositionCloseReviewCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocal), cameraOffset, lookOffset);
                atmosphericDriver.PublishPastForReview();
            }
            else
            {
                controller.ForcePlayerCurrentLocalForReview(playerLocal);
                camera.cullingMask = previousCullingMask;
                PositionCloseReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocal), cameraOffset, lookOffset);
                atmosphericDriver.PublishCurrentForReview();
            }

            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = 32f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            ApplyStage7BokehFocusForReview(camera);

            var probeLayer = pastTimeline
                ? Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31)
                : Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var probeRoot = CreateHd2dAutonomousP2AerialRampTintCameraProbeRoot(camera, profile, enabled, pastTimeline, probeLayer);
            try
            {
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
                ValidateCloseReviewOutputExists(outputDirectory, fileName);
            }
            finally
            {
                DestroyHd2dAutonomousP2AerialRampTintProbeRoot(probeRoot);
            }
        }

        private static GameObject CreateHd2dAutonomousP2AerialRampTintCameraProbeRoot(Camera camera, FastVsHd2dAerialRampTintProfile profile, bool enabled, bool pastTimeline, int renderLayer)
        {
            var sourceMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2AerialRampTintProbeMaterialPath);
            if (camera == null || sourceMaterial == null)
            {
                throw new InvalidOperationException($"Fast VS autonomous P2-56 aerial ramp tint capture failed: missing probe material {Hd2dAutonomousP2AerialRampTintProbeMaterialPath}.");
            }

            var probeMaterial = new Material(sourceMaterial)
            {
                name = enabled ? "FastVS_HD2D_P2_AerialRampTintProbe_On" : "FastVS_HD2D_P2_AerialRampTintProbe_Off",
                hideFlags = HideFlags.HideAndDontSave
            };
            if (probeMaterial.HasProperty(Hd2dAutonomousP2AerialRampUseProperty))
            {
                probeMaterial.SetFloat(Hd2dAutonomousP2AerialRampUseProperty, enabled ? 1f : 0f);
            }

            var settings = ReadHd2dAutonomousP2AerialRampTintSettings(pastTimeline, profile);
            if (probeMaterial.HasProperty(Hd2dAutonomousP2AerialRampTintGlobalName))
            {
                probeMaterial.SetColor(Hd2dAutonomousP2AerialRampTintGlobalName, settings.Tint);
            }

            if (probeMaterial.HasProperty(Hd2dAutonomousP2AerialRampTintDistanceGlobalName))
            {
                probeMaterial.SetVector(Hd2dAutonomousP2AerialRampTintDistanceGlobalName, new Vector4(settings.DistanceStart, settings.DistanceEnd, 0f, 0f));
            }

            if (probeMaterial.HasProperty(Hd2dAutonomousP2AerialRampTintStrengthGlobalName))
            {
                probeMaterial.SetFloat(Hd2dAutonomousP2AerialRampTintStrengthGlobalName, enabled ? settings.Strength : 0f);
            }

            if (probeMaterial.HasProperty("_BaseColor"))
            {
                probeMaterial.SetColor("_BaseColor", new Color(0.72f, 0.62f, 0.46f, 1f));
            }

            var root = new GameObject(Hd2dAutonomousP2AerialRampTintProbeRootName);
            root.transform.SetParent(camera.transform, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            for (var i = 0; i < 5; i++)
            {
                var slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
                slab.name = $"RecedingBuildingSlab_{i + 1:00}";
                slab.transform.SetParent(root.transform, false);
                slab.transform.localPosition = new Vector3(-1.56f + (i * 0.78f), -0.04f, 1.8f + (i * 1.45f));
                slab.transform.localScale = new Vector3(0.44f, 1.32f, 0.12f);
                var collider = slab.GetComponent<Collider>();
                if (collider != null)
                {
                    UnityEngine.Object.DestroyImmediate(collider);
                }

                var renderer = slab.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = probeMaterial;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                }
            }

            SetLayerRecursive(root.transform, renderLayer);
            return root;
        }

        private static void DestroyHd2dAutonomousP2AerialRampTintProbeRoot(GameObject probeRoot)
        {
            if (probeRoot == null)
            {
                return;
            }

            var renderers = probeRoot.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                var material = renderers[i] != null ? renderers[i].sharedMaterial : null;
                if (material != null && material.hideFlags == HideFlags.HideAndDontSave)
                {
                    UnityEngine.Object.DestroyImmediate(material);
                }
            }

            UnityEngine.Object.DestroyImmediate(probeRoot);
        }

        private static Hd2dAutonomousP2AerialRampTintSettings ReadHd2dAutonomousP2AerialRampTintSettings(bool past, FastVsHd2dAerialRampTintProfile profile)
        {
            var profilePath = past ? Hd2dAutonomousP0AtmosphericPastProfilePath : Hd2dAutonomousP0AtmosphericCurrentProfilePath;
            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(profilePath);
            if (!FastVsHd2dAtmosphericPerspectiveDriver.TryReadSettings(volumeProfile, out var settings))
            {
                throw new InvalidOperationException($"House slice validation failed: P2-56 cannot read P0 atmospheric profile {profilePath}.");
            }

            var distanceStart = Mathf.Max(0f, settings.distanceStart.value);
            var distanceEnd = Mathf.Max(distanceStart + 0.25f, settings.distanceEnd.value);
            var tintStart = Mathf.Max(0f, distanceStart + Mathf.Min(profile.distanceStartOffset, profile.distanceEndOffset - 0.25f));
            var tintEnd = Mathf.Max(tintStart + 0.25f, distanceEnd + Mathf.Max(profile.distanceEndOffset, 0.25f));
            return new Hd2dAutonomousP2AerialRampTintSettings(settings.farColor.value, tintStart, tintEnd, profile.strength);
        }

        private static void WriteHd2dAutonomousP2AerialRampTintReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dAerialRampTintProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics currentDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics pastDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var currentSettings = ReadHd2dAutonomousP2AerialRampTintSettings(false, profile);
            var pastSettings = ReadHd2dAutonomousP2AerialRampTintSettings(true, profile);
            var lines = new List<string>
            {
                "# P2-56 Aerial Ramp Tint Review",
                string.Empty,
                "- Scope: stage shader-side distance tint after SurfaceRampLit lighting/ramp math while keeping fullscreen fog disabled for this A/B evidence.",
                "- A/B note: temporary identical receding building slabs are camera-child probes using the same SurfaceRampLit building material path, so distance tint can be judged without relying on scene composition.",
                string.Empty,
                "| Profile | Value |",
                "|---|---:|",
                $"| Strength | {profile.strength:0.###} |",
                $"| Distance start/end offset | {profile.distanceStartOffset:0.###} / {profile.distanceEndOffset:0.###} |",
                $"| Current tint / band | {FormatHd2dAutonomousP2AerialRampTintSettings(currentSettings)} |",
                $"| Past tint / band | {FormatHd2dAutonomousP2AerialRampTintSettings(pastSettings)} |",
                $"| Needs Tom approval | {FormatBool(profile.needsTomApproval)} |",
                $"| Final aerial tint approved | {FormatBool(profile.finalAerialTintApproved)} |",
                string.Empty,
                "| A/B evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                currentDiff.ToReportRow("Current receding building slabs aerial tint off vs on"),
                pastDiff.ToReportRow("Past receding building slabs aerial tint off vs on"),
                string.Empty,
                "| Material opt-in set | Count |",
                "|---|---:|",
                $"| Ground/building SurfaceRampLit materials | {Hd2dAutonomousP2AerialRampTintMaterialPaths.Length} |",
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Current timeline shader-only baseline, aerial ramp tint disabled |",
                $"| `{screenshotFiles[1]}` | Current timeline shader-side aerial tint enabled from current atmospheric far color |",
                $"| `{screenshotFiles[2]}` | Past timeline shader-only baseline, aerial ramp tint disabled |",
                $"| `{screenshotFiles[3]}` | Past timeline shader-side aerial tint enabled from past atmospheric far color |",
                string.Empty,
                "Recommendation: keep this as conservative data prep only. Tom should tune tint strength and distance offsets against real receding buildings once the final camera/fog grade is approved."
            };

            File.WriteAllLines(Path.Combine(outputDirectory, "aerial_ramp_tint_review.md"), lines, Encoding.UTF8);
        }

        private static string FormatHd2dAutonomousP2AerialRampTintSettings(Hd2dAutonomousP2AerialRampTintSettings settings)
        {
            return $"rgba({settings.Tint.r:0.###}, {settings.Tint.g:0.###}, {settings.Tint.b:0.###}, {settings.Tint.a:0.###}) / {settings.DistanceStart:0.###}-{settings.DistanceEnd:0.###} / strength {settings.Strength:0.###}";
        }

        private readonly struct Hd2dAutonomousP2AerialRampTintSettings
        {
            public readonly Color Tint;
            public readonly float DistanceStart;
            public readonly float DistanceEnd;
            public readonly float Strength;

            public Hd2dAutonomousP2AerialRampTintSettings(Color tint, float distanceStart, float distanceEnd, float strength)
            {
                Tint = tint;
                DistanceStart = distanceStart;
                DistanceEnd = distanceEnd;
                Strength = strength;
            }
        }
    }
}
