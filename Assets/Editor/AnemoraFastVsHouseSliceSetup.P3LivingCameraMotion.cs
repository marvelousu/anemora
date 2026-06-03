using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP3LivingCameraMotionRootName = "Current_CentralPlaza_P3_80_LivingCameraMotionReview";
        private const string Hd2dAutonomousP3LivingCameraMotionPreviewName = "P3_80_LivingCameraMotionPreview";
        private const string Hd2dAutonomousP3LivingCameraMotionFollowTargetName = "P3_80_LivingCamera_FollowTarget";
        private const string Hd2dAutonomousP3LivingCameraMotionCameraName = "P3_80_LivingCamera_CinemachineCamera_InactivePreview";
        private const string Hd2dAutonomousP3LivingCameraMotionForegroundMarkerName = "P3_80_LivingCamera_ForegroundParallaxMarker";
        private const string Hd2dAutonomousP3LivingCameraMotionBackgroundMarkerName = "P3_80_LivingCamera_BackgroundParallaxMarker";
        private const string Hd2dAutonomousP3LivingCameraMotionProfilePath = "Assets/Settings/FastVS_HD2D_P3_LivingCameraMotionProfile.asset";
        private const string Hd2dAutonomousP3LivingCameraMotionNoisePath = "Assets/Settings/FastVS_HD2D_P3_LivingCameraBreathingNoise.asset";
        private const string Hd2dAutonomousP3LivingCameraMotionProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dLivingCameraMotionProfile.cs";
        private const string Hd2dAutonomousP3LivingCameraMotionPreviewRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dLivingCameraMotionPreview.cs";
        private const string Hd2dAutonomousP3LivingCameraMotionEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P3LivingCameraMotion.cs";
        private const string Hd2dAutonomousP3LivingCameraMotionForegroundMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p3_80_living_camera_foreground_marker.mat";
        private const string Hd2dAutonomousP3LivingCameraMotionBackgroundMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p3_80_living_camera_background_marker.mat";

        public static void CaptureHd2dAutonomousP3Item80LivingCameraMotionBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var preview = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dLivingCameraMotionPreview>(FindObjectsInactive.Include);
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || preview == null || sunDriver == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-80 living camera motion capture failed: review scene components are missing.");
            }

            ValidateHd2dAutonomousP3LivingCameraMotion();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("living_camera_motion_idle_parallax_damping");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_motion_disabled_locked_baseline.png",
                "02_conservative_breathing_drift_t1.png",
                "03_conservative_breathing_drift_t4.png",
                "04_soft_follow_damping_mid_stop.png",
                "05_accessibility_toggle_motion_off_reset.png"
            };
            var sampleRows = new List<string>();
            FastVsHd2dLivingCameraMotionPreview.SampleResult baselineResult = default;
            FastVsHd2dLivingCameraMotionPreview.SampleResult driftAResult = default;
            FastVsHd2dLivingCameraMotionPreview.SampleResult driftBResult = default;
            FastVsHd2dLivingCameraMotionPreview.SampleResult dampingResult = default;
            FastVsHd2dLivingCameraMotionPreview.SampleResult accessibilityOffResult = default;
            var dampingT = 0f;
            var baseAnchor = GetHd2dAutonomousP3LivingCameraMotionBaseAnchorLocal();
            var stoppedTargetAnchor = baseAnchor + new Vector3(1.42f, 0f, 0.72f);
            var previousMask = camera.cullingMask;
            var previousFov = camera.fieldOfView;
            var previousOrthographic = camera.orthographic;

            try
            {
                guide.SetMovementFrozen(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ClosePortal();
                controller.ForcePlayerCurrentLocalForReview(baseAnchor);
                guide.ApplyActiveTimeIsolationForReview();
                Physics.SyncTransforms();
                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();
                camera.cullingMask = ResolveCurrentTimeReviewCullingMask(controller, previousMask);
                preview.SetReviewMarkersVisibleForReview(true);

                CaptureHd2dAutonomousP3LivingCameraMotionShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    baseAnchor,
                    0f,
                    false,
                    true,
                    outputDirectory,
                    screenshotFiles[0],
                    "locked baseline: motion disabled and accessibility disable path active",
                    sampleRows,
                    out baselineResult);

                CaptureHd2dAutonomousP3LivingCameraMotionShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    baseAnchor,
                    1.25f,
                    true,
                    false,
                    outputDirectory,
                    screenshotFiles[1],
                    "conservative low-frequency living-camera drift sample A",
                    sampleRows,
                    out driftAResult);

                CaptureHd2dAutonomousP3LivingCameraMotionShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    baseAnchor,
                    4.25f,
                    true,
                    false,
                    outputDirectory,
                    screenshotFiles[2],
                    "same drift profile later in the 5s standing-still window",
                    sampleRows,
                    out driftBResult);

                controller.ForcePlayerCurrentLocalForReview(stoppedTargetAnchor);
                Physics.SyncTransforms();
                var dampedAnchor = preview.EvaluateSoftFollowAnchorForReview(baseAnchor, stoppedTargetAnchor, 0.42f, out dampingT);
                CaptureHd2dAutonomousP3LivingCameraMotionShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    dampedAnchor,
                    2.20f,
                    false,
                    false,
                    outputDirectory,
                    screenshotFiles[3],
                    "soft follow damping proof: camera anchor eases part-way after player stop",
                    sampleRows,
                    out dampingResult);

                controller.ForcePlayerCurrentLocalForReview(baseAnchor);
                Physics.SyncTransforms();
                CaptureHd2dAutonomousP3LivingCameraMotionShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    baseAnchor,
                    4.25f,
                    true,
                    true,
                    outputDirectory,
                    screenshotFiles[4],
                    "accessibility toggle proof: same sample time with motion disabled",
                    sampleRows,
                    out accessibilityOffResult);
            }
            finally
            {
                preview.SetReviewMarkersVisibleForReview(false);
                camera.cullingMask = previousMask;
                camera.fieldOfView = previousFov;
                camera.orthographic = previousOrthographic;
                controller.ForcePlayerCurrentLocalForReview(baseAnchor);
                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                AssetDatabase.SaveAssets();
            }

            var baselineVsDriftA = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var driftMotion = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var driftVsDamping = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[2], screenshotFiles[3]);
            var resetDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[4]);
            WriteHd2dAutonomousP3LivingCameraMotionReviewReport(
                outputDirectory,
                screenshotFiles,
                preview,
                sampleRows,
                baselineResult,
                driftAResult,
                driftBResult,
                dampingResult,
                accessibilityOffResult,
                dampingT,
                baselineVsDriftA,
                driftMotion,
                driftVsDamping,
                resetDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P3-80 living camera motion review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP3LivingCameraMotion(Transform currentCentralPlazaRoot, Camera camera)
        {
            if (currentCentralPlazaRoot == null || camera == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP3LivingCameraMotionProfile();
            var noiseSettings = EnsureHd2dAutonomousP3LivingCameraNoiseSettings(profile);
            var foregroundMaterial = EnsureHd2dAutonomousP3LivingCameraMarkerMaterial(
                Hd2dAutonomousP3LivingCameraMotionForegroundMaterialPath,
                "hd2d_p3_80_living_camera_foreground_marker",
                new Color(0.95f, 0.72f, 0.22f, 1f));
            var backgroundMaterial = EnsureHd2dAutonomousP3LivingCameraMarkerMaterial(
                Hd2dAutonomousP3LivingCameraMotionBackgroundMaterialPath,
                "hd2d_p3_80_living_camera_background_marker",
                new Color(0.26f, 0.72f, 1.0f, 1f));
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP3LivingCameraMotionRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP3LivingCameraMotionRootName);
            root.transform.SetParent(currentCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;

            var previewObject = new GameObject(Hd2dAutonomousP3LivingCameraMotionPreviewName);
            previewObject.transform.SetParent(root.transform, false);
            var preview = previewObject.AddComponent<FastVsHd2dLivingCameraMotionPreview>();

            var followTarget = new GameObject(Hd2dAutonomousP3LivingCameraMotionFollowTargetName);
            followTarget.transform.SetParent(root.transform, false);
            followTarget.transform.localPosition = GetHd2dAutonomousP3LivingCameraMotionBaseAnchorLocal();
            followTarget.transform.localRotation = Quaternion.identity;
            followTarget.transform.localScale = Vector3.one;

            var cameraObject = new GameObject(Hd2dAutonomousP3LivingCameraMotionCameraName);
            cameraObject.transform.SetParent(root.transform, false);
            var livingCamera = cameraObject.AddComponent<CinemachineCamera>();
            var composer = cameraObject.AddComponent<CinemachinePositionComposer>();
            var perlin = cameraObject.AddComponent<CinemachineBasicMultiChannelPerlin>();

            var foregroundMarker = CreateHd2dAutonomousP3LivingCameraMarker(
                root.transform,
                Hd2dAutonomousP3LivingCameraMotionForegroundMarkerName,
                GetHd2dAutonomousP3LivingCameraMotionForegroundMarkerLocal(),
                new Vector3(0.22f, 0.98f, 0.08f),
                foregroundMaterial);
            var backgroundMarker = CreateHd2dAutonomousP3LivingCameraMarker(
                root.transform,
                Hd2dAutonomousP3LivingCameraMotionBackgroundMarkerName,
                GetHd2dAutonomousP3LivingCameraMotionBackgroundMarkerLocal(),
                new Vector3(0.34f, 1.40f, 0.08f),
                backgroundMaterial);

            preview.ConfigureForReview(profile, camera, livingCamera, perlin, composer, noiseSettings, followTarget.transform, foregroundMarker, backgroundMarker);
            SetHd2dAutonomousP3LivingCameraMotionLayerRecursively(root, CurrentSpaceRenderLayer);

            var landmark = root.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", "current.central_plaza.hd2d_p3_80.living_camera_motion_review");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);

            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(preview);
            EditorUtility.SetDirty(livingCamera);
            EditorUtility.SetDirty(composer);
            EditorUtility.SetDirty(perlin);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP3LivingCameraMotion()
        {
            var profile = EnsureHd2dAutonomousP3LivingCameraMotionProfile();
            var noiseSettings = EnsureHd2dAutonomousP3LivingCameraNoiseSettings(profile);
            var preview = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dLivingCameraMotionPreview>(FindObjectsInactive.Include);
            var camera = Camera.main;
            if (profile == null ||
                noiseSettings == null ||
                preview == null ||
                camera == null ||
                !preview.IsReadyForReview ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalLivingCameraMotionApprovedForReview ||
                !profile.ConservativeDataPrepForReview ||
                !profile.DirectRuntimeCameraAuthorityDisabledForReview ||
                profile.ActiveOnAwakeForReview ||
                !profile.AccessibilityTogglePreparedForReview ||
                !profile.AccessibilityMotionDisabledByDefaultForReview ||
                !profile.CinemachineNoiseConfiguredForReview ||
                !profile.PositionComposerDampingConfiguredForReview ||
                !preview.DirectRuntimeCameraAuthorityDisabledForReview ||
                !preview.PreviewCameraInactiveForReview ||
                preview.PositionNoiseChannelCountForReview < 1 ||
                preview.OrientationNoiseChannelCountForReview < 1)
            {
                throw new InvalidOperationException("House slice validation failed: P3-80 needs a conservative non-final living-camera profile, inactive Cinemachine preview rig, low-frequency noise asset, accessibility-off default, and no direct runtime camera authority.");
            }

            if (profile.TargetPixelDriftForReview < 0.5f ||
                profile.TargetPixelDriftForReview > 2.2f ||
                profile.FrequencyHzForReview < 0.08f ||
                profile.FrequencyHzForReview > 0.26f ||
                profile.RotationAmplitudeDegreesForReview > 0.001f ||
                profile.FollowDampingSecondsForReview.x < 0.25f ||
                profile.FollowDampingSecondsForReview.y < 0.25f ||
                profile.FollowDampingSecondsForReview.z < 0.25f)
            {
                throw new InvalidOperationException("House slice validation failed: P3-80 camera motion values must stay low-amplitude, low-frequency, no-rotation, and softly damped for Tom review.");
            }

            var baseAnchor = GetHd2dAutonomousP3LivingCameraMotionBaseAnchorLocal();
            var offsetA = preview.CalculateBreathingOffsetForReview(1.25f, true, false);
            var offsetB = preview.CalculateBreathingOffsetForReview(4.25f, true, false);
            var disabledOffset = preview.CalculateBreathingOffsetForReview(4.25f, true, true);
            if (offsetA.magnitude < 0.004f ||
                Vector3.Distance(offsetA, offsetB) < 0.006f ||
                disabledOffset.sqrMagnitude > 0.000001f)
            {
                throw new InvalidOperationException($"House slice validation failed: P3-80 breathing drift must be measurable and fully disabled by accessibility toggle (A={FormatVector3ForReport(offsetA)}, B={FormatVector3ForReport(offsetB)}, disabled={FormatVector3ForReport(disabledOffset)}).");
            }

            var damped = preview.EvaluateSoftFollowAnchorForReview(baseAnchor, baseAnchor + new Vector3(1.42f, 0f, 0.72f), 0.42f, out var dampingT);
            if (dampingT <= 0.05f || dampingT >= 0.98f || Vector3.Distance(damped, baseAnchor) < 0.10f)
            {
                throw new InvalidOperationException($"House slice validation failed: P3-80 soft follow damping must ease part-way rather than snap (t={dampingT:0.###}, damped={FormatVector3ForReport(damped)}).");
            }

            preview.SetReviewMarkersVisibleForReview(true);
            try
            {
                if (!preview.ApplyCameraStateForReview(
                        camera,
                        UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>()?.CurrentSpaceRootForReview,
                        baseAnchor,
                        1.25f,
                        true,
                        false,
                        out var sample) ||
                    sample.ForegroundPixelShift < 0.35f ||
                    sample.ForegroundPixelShift > 8f ||
                    sample.RotationDeltaDegrees > 0.001f)
                {
                    throw new InvalidOperationException($"House slice validation failed: P3-80 review sample must create subtle parallax pixel drift with no rotation jitter (fg={sample.ForegroundPixelShift:0.###}px, bg={sample.BackgroundPixelShift:0.###}px, rot={sample.RotationDeltaDegrees:0.######}).");
                }
            }
            finally
            {
                preview.SetReviewMarkersVisibleForReview(false);
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3LivingCameraMotionProfileRuntimePath), "finalLivingCameraMotionApproved", Hd2dAutonomousP3LivingCameraMotionProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3LivingCameraMotionPreviewRuntimePath), "ApplyCameraStateForReview", Hd2dAutonomousP3LivingCameraMotionPreviewRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3LivingCameraMotionEditorPath), "CinemachineBasicMultiChannelPerlin", Hd2dAutonomousP3LivingCameraMotionEditorPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3LivingCameraMotionEditorPath), "accessibility toggle", Hd2dAutonomousP3LivingCameraMotionEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP3LivingCameraMotion", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP3LivingCameraMotion", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dLivingCameraMotionProfile EnsureHd2dAutonomousP3LivingCameraMotionProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dLivingCameraMotionProfile>(Hd2dAutonomousP3LivingCameraMotionProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dLivingCameraMotionProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP3LivingCameraMotionProfilePath);
            }

            profile.ConfigureForReview(
                true,
                false,
                true,
                true,
                false,
                true,
                true,
                true,
                true,
                1.2f,
                new Vector3(0.010f, 0.006f, 0.003f),
                0.18f,
                0f,
                1f,
                0.18f,
                new Vector3(0.45f, 0.58f, 0.72f),
                0.16f,
                7.5f,
                0.68f,
                38f,
                4.55f,
                new Vector3(0f, 2.75f, -4.55f),
                new Vector3(0f, 0.72f, 0.45f),
                5,
                155,
                "Keep this as conservative living-camera data only. Tom should tune final drift amplitude, damping, look-ahead, and accessibility defaults before this controls the live gameplay camera.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static NoiseSettings EnsureHd2dAutonomousP3LivingCameraNoiseSettings(FastVsHd2dLivingCameraMotionProfile profile)
        {
            EnsureFolder("Assets/Settings");
            var settings = AssetDatabase.LoadAssetAtPath<NoiseSettings>(Hd2dAutonomousP3LivingCameraMotionNoisePath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<NoiseSettings>();
                AssetDatabase.CreateAsset(settings, Hd2dAutonomousP3LivingCameraMotionNoisePath);
            }

            var amplitude = profile != null ? profile.PositionAmplitudeMetersForReview : new Vector3(0.010f, 0.006f, 0.003f);
            var frequency = profile != null ? profile.FrequencyHzForReview : 0.18f;
            settings.PositionNoise = new[]
            {
                new NoiseSettings.TransformNoiseParams
                {
                    X = new NoiseSettings.NoiseParams { Frequency = frequency, Amplitude = amplitude.x, Constant = true },
                    Y = new NoiseSettings.NoiseParams { Frequency = frequency * 0.73f, Amplitude = amplitude.y, Constant = true },
                    Z = new NoiseSettings.NoiseParams { Frequency = frequency * 0.61f, Amplitude = amplitude.z, Constant = true }
                }
            };
            settings.OrientationNoise = new[]
            {
                new NoiseSettings.TransformNoiseParams
                {
                    X = new NoiseSettings.NoiseParams { Frequency = frequency, Amplitude = 0f, Constant = true },
                    Y = new NoiseSettings.NoiseParams { Frequency = frequency, Amplitude = 0f, Constant = true },
                    Z = new NoiseSettings.NoiseParams { Frequency = frequency, Amplitude = 0f, Constant = true }
                }
            };
            EditorUtility.SetDirty(settings);
            return settings;
        }

        private static Material EnsureHd2dAutonomousP3LivingCameraMarkerMaterial(string path, string materialId, Color color)
        {
            EnsureFolder(MaterialDirectory);
            var shader = Shader.Find(URPUnlitShaderName) ?? Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-80 marker material shader is missing.");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            material.renderQueue = (int)RenderQueue.Geometry;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Transform CreateHd2dAutonomousP3LivingCameraMarker(
            Transform parent,
            string objectName,
            Vector3 localPosition,
            Vector3 localScale,
            Material material)
        {
            var marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marker.name = objectName;
            marker.transform.SetParent(parent, false);
            marker.transform.localPosition = localPosition;
            marker.transform.localRotation = Quaternion.identity;
            marker.transform.localScale = localScale;
            var collider = marker.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = marker.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            marker.SetActive(false);
            return marker.transform;
        }

        private static void CaptureHd2dAutonomousP3LivingCameraMotionShot(
            FastVsHd2dLivingCameraMotionPreview preview,
            Camera camera,
            Transform activeRoot,
            Vector3 anchorLocal,
            float sampleSeconds,
            bool motionEnabled,
            bool accessibilityMotionDisabled,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> sampleRows,
            out FastVsHd2dLivingCameraMotionPreview.SampleResult result)
        {
            if (!preview.ApplyCameraStateForReview(camera, activeRoot, anchorLocal, sampleSeconds, motionEnabled, accessibilityMotionDisabled, out result))
            {
                throw new InvalidOperationException($"Fast VS autonomous P3-80 capture failed: could not apply camera sample for {fileName}.");
            }

            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            sampleRows.Add($"| `{fileName}` | {label} | {sampleSeconds:0.###} | {FormatBool(motionEnabled)} | {FormatBool(accessibilityMotionDisabled)} | {FormatVector3ForReport(result.MotionOffset)} | {result.ForegroundPixelShift:0.###} | {result.BackgroundPixelShift:0.###} | {result.RotationDeltaDegrees:0.######} |");
        }

        private static void WriteHd2dAutonomousP3LivingCameraMotionReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dLivingCameraMotionPreview preview,
            IReadOnlyList<string> sampleRows,
            FastVsHd2dLivingCameraMotionPreview.SampleResult baseline,
            FastVsHd2dLivingCameraMotionPreview.SampleResult driftA,
            FastVsHd2dLivingCameraMotionPreview.SampleResult driftB,
            FastVsHd2dLivingCameraMotionPreview.SampleResult damping,
            FastVsHd2dLivingCameraMotionPreview.SampleResult accessibilityOff,
            float dampingT,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics baselineVsDriftA,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics driftMotion,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics driftVsDamping,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics resetDiff)
        {
            var profile = preview.ProfileForReview;
            var lines = new List<string>
            {
                "# P3-80 Living Camera Motion Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative camera data prep. The Cinemachine noise and damping contract exists, but the preview camera remains inactive and does not take live runtime authority.",
                "- Safety note: this item does not change the existing main-camera Brain/guide authority contract; runtime default is motion off and accessibility-disabled until Tom approves live camera feel.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP3LivingCameraMotionProfilePath}` |",
                $"| NoiseSettings | `{Hd2dAutonomousP3LivingCameraMotionNoisePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalLivingCameraMotionApprovedForReview)} |",
                $"| Active on awake / direct runtime authority disabled | {FormatBool(profile.ActiveOnAwakeForReview)} / {FormatBool(profile.DirectRuntimeCameraAuthorityDisabledForReview)} |",
                $"| Accessibility toggle prepared / disabled by default | {FormatBool(profile.AccessibilityTogglePreparedForReview)} / {FormatBool(profile.AccessibilityMotionDisabledByDefaultForReview)} |",
                $"| Cinemachine inactive priority / planned live priority | {profile.InactivePriorityForReview} / {profile.PlannedLivePriorityForReview} |",
                $"| Noise channels position/orientation | {preview.PositionNoiseChannelCountForReview} / {preview.OrientationNoiseChannelCountForReview} |",
                $"| Target pixel drift / frequency | {profile.TargetPixelDriftForReview:0.###}px / {profile.FrequencyHzForReview:0.###}Hz |",
                $"| Position amplitude meters | {FormatVector3ForReport(profile.PositionAmplitudeMetersForReview)} |",
                $"| Rotation amplitude degrees | {profile.RotationAmplitudeDegreesForReview:0.######} |",
                $"| Follow damping XYZ / look-ahead | {FormatVector3ForReport(profile.FollowDampingSecondsForReview)} / {profile.LookAheadTimeForReview:0.###}s |",
                $"| Soft stop ease seconds / damping t in proof | {profile.StopEaseSecondsForReview:0.###} / {dampingT:0.###} |",
                string.Empty,
                "| Camera Sample | Label | Seconds | Motion enabled | Accessibility disabled | Motion offset xyz | Foreground px shift | Background px shift | Rotation delta deg |",
                "|---|---|---:|---|---|---|---:|---:|---:|"
            };
            lines.AddRange(sampleRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                baselineVsDriftA.ToReportRow("locked baseline vs conservative drift sample A"),
                driftMotion.ToReportRow("drift sample A vs sample B over 5s standing still"),
                driftVsDamping.ToReportRow("drift sample B vs soft follow damping proof"),
                resetDiff.ToReportRow("locked baseline vs accessibility-toggle motion-off proof"),
                string.Empty,
                "| Numeric Check | Value |",
                "|---|---:|",
                $"| Drift A foreground/background shift | {driftA.ForegroundPixelShift:0.###} / {driftA.BackgroundPixelShift:0.###} px |",
                $"| Drift B foreground/background shift | {driftB.ForegroundPixelShift:0.###} / {driftB.BackgroundPixelShift:0.###} px |",
                $"| Accessibility-off foreground/background shift | {accessibilityOff.ForegroundPixelShift:0.###} / {accessibilityOff.BackgroundPixelShift:0.###} px |",
                $"| Rotation delta max | {Mathf.Max(baseline.RotationDeltaDegrees, driftA.RotationDeltaDegrees, driftB.RotationDeltaDegrees, damping.RotationDeltaDegrees, accessibilityOff.RotationDeltaDegrees):0.######} deg |",
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Motion disabled locked baseline with diagnostic parallax markers visible. |",
                $"| `{screenshotFiles[1]}` | Conservative living-camera drift sample in the 5s standing-still window. |",
                $"| `{screenshotFiles[2]}` | Later drift sample showing the low-frequency breathing offset changes over time. |",
                $"| `{screenshotFiles[3]}` | Soft-follow damping proof after the player stop target moves ahead while the camera anchor eases part-way. |",
                $"| `{screenshotFiles[4]}` | Accessibility toggle proof: same drift sample time with motion disabled, returning to baseline. |"
            });

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllText(Path.Combine(outputDirectory, "living_camera_motion_idle_parallax_damping_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Vector3 GetHd2dAutonomousP3LivingCameraMotionBaseAnchorLocal()
        {
            return CentralPlazaVsCenter + new Vector3(0.12f, 0.02f, 2.16f);
        }

        private static Vector3 GetHd2dAutonomousP3LivingCameraMotionForegroundMarkerLocal()
        {
            return CentralPlazaVsCenter + new Vector3(-0.92f, 0.72f, 1.18f);
        }

        private static Vector3 GetHd2dAutonomousP3LivingCameraMotionBackgroundMarkerLocal()
        {
            return CentralPlazaVsCenter + new Vector3(2.28f, 1.05f, 5.15f);
        }

        private static void SetHd2dAutonomousP3LivingCameraMotionLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetHd2dAutonomousP3LivingCameraMotionLayerRecursively(child.gameObject, layer);
            }
        }
    }
}
