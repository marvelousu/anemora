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
using UnityEngine.Rendering.Universal;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2PaniniProjectionProfilePath = "Assets/Settings/FastVS_HD2D_P2_PaniniProjectionProfile.asset";
        private const string Hd2dAutonomousP2PaniniDefaultVolumeProfilePath = "Assets/Settings/DefaultVolumeProfile.asset";
        private const string Hd2dAutonomousP2PaniniReviewProbeRootName = "FastVS_HD2D_P2_PaniniReviewEdgeProbes";

        public static void CaptureHd2dAutonomousP2Item54PaniniProjectionBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHouseLightingDirector>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var volume = FindSceneObjectIncludingInactive("FastVS_HD2D_GlobalVolume")?.GetComponent<Volume>();
            if (controller == null || visibility == null || guide == null || director == null || realtimeRig == null || camera == null || volume == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-54 Panini capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2PaniniProjection();
            var profile = EnsureHd2dAutonomousP2PaniniProjectionProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("panini_projection");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_current_panini_off_edge_verticals.png",
                "02_current_panini_on_edge_verticals.png",
                "03_past_panini_off_edge_verticals.png",
                "04_past_panini_on_edge_verticals.png"
            };

            var currentProbeRoot = CreateHd2dAutonomousP2PaniniReviewProbeRoot(controller.CurrentSpaceRootForReview);
            var pastProbeRoot = CreateHd2dAutonomousP2PaniniReviewProbeRoot(controller.OtherTimeSpaceRootForReview);
            var previousCullingMask = camera.cullingMask;
            try
            {
                CaptureHd2dAutonomousP2PaniniReviewShot(
                    controller,
                    visibility,
                    guide,
                    director,
                    realtimeRig,
                    camera,
                    volume,
                    profile,
                    false,
                    false,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[0]);

                CaptureHd2dAutonomousP2PaniniReviewShot(
                    controller,
                    visibility,
                    guide,
                    director,
                    realtimeRig,
                    camera,
                    volume,
                    profile,
                    true,
                    false,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[1]);

                CaptureHd2dAutonomousP2PaniniReviewShot(
                    controller,
                    visibility,
                    guide,
                    director,
                    realtimeRig,
                    camera,
                    volume,
                    profile,
                    false,
                    true,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[2]);

                CaptureHd2dAutonomousP2PaniniReviewShot(
                    controller,
                    visibility,
                    guide,
                    director,
                    realtimeRig,
                    camera,
                    volume,
                    profile,
                    true,
                    true,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[3]);
            }
            finally
            {
                camera.cullingMask = previousCullingMask;
                if (currentProbeRoot != null)
                {
                    UnityEngine.Object.DestroyImmediate(currentProbeRoot);
                }

                if (pastProbeRoot != null)
                {
                    UnityEngine.Object.DestroyImmediate(pastProbeRoot);
                }

                ApplyHd2dAutonomousP2PaniniProjectionForReview(volume, true, profile.distance, profile.cropToFit);
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
            var metrics = new[]
            {
                MeasureHd2dAutonomousP2PaniniScreenshotMetrics(Path.Combine(outputDirectory, screenshotFiles[0])),
                MeasureHd2dAutonomousP2PaniniScreenshotMetrics(Path.Combine(outputDirectory, screenshotFiles[1])),
                MeasureHd2dAutonomousP2PaniniScreenshotMetrics(Path.Combine(outputDirectory, screenshotFiles[2])),
                MeasureHd2dAutonomousP2PaniniScreenshotMetrics(Path.Combine(outputDirectory, screenshotFiles[3]))
            };
            WriteHd2dAutonomousP2PaniniProjectionReviewReport(outputDirectory, screenshotFiles, profile, currentDiff, pastDiff, metrics);

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-54 Panini Projection review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2PaniniProjection()
        {
            var profile = EnsureHd2dAutonomousP2PaniniProjectionProfile();
            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(Hd2dAutonomousP2PaniniDefaultVolumeProfilePath);
            if (volumeProfile == null)
            {
                throw new InvalidOperationException($"P2-54 Panini setup failed: missing {Hd2dAutonomousP2PaniniDefaultVolumeProfilePath}");
            }

            ApplyHd2dAutonomousP2PaniniProjection(volumeProfile, true, profile.distance, profile.cropToFit);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2PaniniProjection()
        {
            var profile = EnsureHd2dAutonomousP2PaniniProjectionProfile();
            if (!profile.needsTomApproval || profile.finalProjectionApproved)
            {
                throw new InvalidOperationException("House slice validation failed: P2-54 Panini profile must remain NEEDS-TOM and final approval must stay false.");
            }

            if (profile.distance < 0.40f || profile.distance > 0.80f || profile.cropToFit < 0.50f || profile.cropToFit > 1.00f)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-54 Panini profile values are outside conservative bounds. distance={profile.distance:0.###}, crop={profile.cropToFit:0.###}");
            }

            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(Hd2dAutonomousP2PaniniDefaultVolumeProfilePath);
            if (volumeProfile == null || !volumeProfile.TryGet<PaniniProjection>(out var panini))
            {
                throw new InvalidOperationException("House slice validation failed: P2-54 DefaultVolumeProfile must contain a PaniniProjection override.");
            }

            if (!panini.active ||
                !panini.distance.overrideState ||
                !panini.cropToFit.overrideState ||
                Mathf.Abs(panini.distance.value - profile.distance) > 0.001f ||
                Mathf.Abs(panini.cropToFit.value - profile.cropToFit) > 0.001f)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P2-54 Panini override must be active on the shared DefaultVolumeProfile " +
                    $"with distance={profile.distance:0.###} and crop={profile.cropToFit:0.###}.");
            }
        }

        private static FastVsHd2dPaniniProjectionProfile EnsureHd2dAutonomousP2PaniniProjectionProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dPaniniProjectionProfile>(Hd2dAutonomousP2PaniniProjectionProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dPaniniProjectionProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2PaniniProjectionProfilePath);
            }

            profile.distance = 0.55f;
            profile.cropToFit = 0.85f;
            profile.reviewFieldOfView = 32f;
            profile.needsTomApproval = true;
            profile.finalProjectionApproved = false;
            profile.reviewNotes =
                "Conservative P2-54 Panini baseline on the existing shared Global Volume. Tom should tune distance/crop against edge buildings before final approval.";
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static void CaptureHd2dAutonomousP2PaniniReviewShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsHouseLightingDirector director,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            Volume volume,
            FastVsHd2dPaniniProjectionProfile profile,
            bool paniniEnabled,
            bool pastTimeline,
            int previousCullingMask,
            string outputDirectory,
            string fileName)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            director.ApplyAreaForReview(FastVsHouseArea.CentralPlaza);
            var playerLocal = CentralPlazaVsCenter + new Vector3(0.00f, 0.02f, 4.72f);
            var anchorLocal = CentralPlazaVsCenter + new Vector3(0.00f, 0.92f, 7.78f);
            var cameraOffset = new Vector3(0.00f, 2.28f, -7.38f);
            var lookOffset = new Vector3(0.00f, 0.22f, 0.52f);

            if (pastTimeline)
            {
                controller.ForcePlayerOtherTimeLocalForReview(playerLocal);
                var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
                var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
                var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
                camera.cullingMask = (previousCullingMask & ~currentBit) | otherBit | playerBit;
                PositionCloseReviewCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocal), cameraOffset, lookOffset);
            }
            else
            {
                controller.ForcePlayerCurrentLocalForReview(playerLocal);
                camera.cullingMask = previousCullingMask;
                PositionCloseReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocal), cameraOffset, lookOffset);
            }

            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = profile.reviewFieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            var additionalCameraData = camera.GetComponent<UniversalAdditionalCameraData>();
            if (additionalCameraData != null)
            {
                additionalCameraData.renderPostProcessing = true;
                EditorUtility.SetDirty(additionalCameraData);
            }

            ApplyStage7BokehFocusForReview(camera);
            var probeLayer = pastTimeline
                ? Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31)
                : Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var cameraProbeRoot = CreateHd2dAutonomousP2PaniniCameraProbeRoot(camera.transform, probeLayer);
            try
            {
                ApplyHd2dAutonomousP2PaniniProjectionForReview(volume, paniniEnabled, profile.distance, profile.cropToFit);
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
                ValidateCloseReviewOutputExists(outputDirectory, fileName);
            }
            finally
            {
                if (cameraProbeRoot != null)
                {
                    UnityEngine.Object.DestroyImmediate(cameraProbeRoot);
                }
            }
        }

        private static void ApplyHd2dAutonomousP2PaniniProjectionForReview(Volume volume, bool enabled, float distance, float cropToFit)
        {
            if (volume == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-54 Panini failed: global volume is missing.");
            }

            ApplyHd2dAutonomousP2PaniniProjection(volume.sharedProfile, enabled, distance, cropToFit);
            var runtimeProfile = volume.profile;
            if (runtimeProfile != null && !ReferenceEquals(runtimeProfile, volume.sharedProfile))
            {
                ApplyHd2dAutonomousP2PaniniProjection(runtimeProfile, enabled, distance, cropToFit);
            }
        }

        private static void ApplyHd2dAutonomousP2PaniniProjection(VolumeProfile profile, bool enabled, float distance, float cropToFit)
        {
            if (profile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-54 Panini failed: volume profile is missing.");
            }

            if (!profile.TryGet<PaniniProjection>(out var panini))
            {
                panini = profile.Add<PaniniProjection>(true);
            }

            panini.active = enabled;
            panini.distance.overrideState = true;
            panini.distance.value = enabled ? distance : 0f;
            panini.cropToFit.overrideState = true;
            panini.cropToFit.value = enabled ? cropToFit : 1f;
            EditorUtility.SetDirty(panini);
            EditorUtility.SetDirty(profile);
            VolumeManager.instance.ResetMainStack();
        }

        private static GameObject CreateHd2dAutonomousP2PaniniReviewProbeRoot(Transform parent)
        {
            if (parent == null)
            {
                return null;
            }

            var root = new GameObject(Hd2dAutonomousP2PaniniReviewProbeRootName);
            root.transform.SetParent(parent, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            SetLayerRecursive(root.transform, parent.gameObject.layer);

            CreateHd2dAutonomousP2PaniniReviewProbe(
                root.transform,
                "LeftEdgeCyanVerticalProbe",
                CentralPlazaVsCenter + new Vector3(-4.25f, 1.68f, 4.18f),
                new Vector3(0.22f, 3.85f, 0.22f),
                new Color(0.0f, 1.0f, 1.0f, 1f));

            CreateHd2dAutonomousP2PaniniReviewProbe(
                root.transform,
                "RightEdgeMagentaVerticalProbe",
                CentralPlazaVsCenter + new Vector3(4.25f, 1.68f, 4.18f),
                new Vector3(0.22f, 3.85f, 0.22f),
                new Color(1.0f, 0.0f, 1.0f, 1f));

            SetLayerRecursive(root.transform, parent.gameObject.layer);
            return root;
        }

        private static GameObject CreateHd2dAutonomousP2PaniniCameraProbeRoot(Transform cameraTransform, int renderLayer)
        {
            if (cameraTransform == null)
            {
                return null;
            }

            var root = new GameObject($"{Hd2dAutonomousP2PaniniReviewProbeRootName}_Camera");
            root.transform.SetParent(cameraTransform, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            SetLayerRecursive(root.transform, renderLayer);

            CreateHd2dAutonomousP2PaniniReviewProbe(
                root.transform,
                "CameraLeftEdgeCyanVerticalProbe",
                new Vector3(-2.05f, 0.16f, 5.0f),
                new Vector3(0.08f, 2.35f, 0.08f),
                new Color(0.0f, 1.0f, 1.0f, 1f));

            CreateHd2dAutonomousP2PaniniReviewProbe(
                root.transform,
                "CameraRightEdgeMagentaVerticalProbe",
                new Vector3(2.05f, 0.16f, 5.0f),
                new Vector3(0.08f, 2.35f, 0.08f),
                new Color(1.0f, 0.0f, 1.0f, 1f));

            SetLayerRecursive(root.transform, renderLayer);
            return root;
        }

        private static void CreateHd2dAutonomousP2PaniniReviewProbe(
            Transform root,
            string name,
            Vector3 localPosition,
            Vector3 localScale,
            Color color)
        {
            var probe = GameObject.CreatePrimitive(PrimitiveType.Cube);
            probe.name = name;
            probe.transform.SetParent(root, false);
            probe.transform.localPosition = localPosition;
            probe.transform.localRotation = Quaternion.identity;
            probe.transform.localScale = localScale;
            var collider = probe.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var shader = Shader.Find(URPUnlitShaderName);
            var material = new Material(shader != null ? shader : Shader.Find("Sprites/Default"))
            {
                name = $"FastVS_HD2D_P2_Panini_{name}_Mat"
            };
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            var renderer = probe.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        private static Hd2dAutonomousP2PaniniScreenshotMetrics MeasureHd2dAutonomousP2PaniniScreenshotMetrics(string imagePath)
        {
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                if (!ImageConversion.LoadImage(texture, File.ReadAllBytes(imagePath)))
                {
                    throw new InvalidOperationException($"P2-54 Panini metric failed: could not decode {imagePath}");
                }

                var pixels = texture.GetPixels32();
                var borderPixels = 0;
                var blackBorderPixels = 0;
                var cyanCount = 0;
                var magentaCount = 0;
                var cyanMinX = texture.width;
                var cyanMaxX = 0;
                var magentaMinX = texture.width;
                var magentaMaxX = 0;
                for (var y = 0; y < texture.height; y++)
                {
                    for (var x = 0; x < texture.width; x++)
                    {
                        var pixel = pixels[(y * texture.width) + x];
                        if (x < 8 || x >= texture.width - 8 || y < 8 || y >= texture.height - 8)
                        {
                            borderPixels++;
                            if (pixel.r < 4 && pixel.g < 4 && pixel.b < 4)
                            {
                                blackBorderPixels++;
                            }
                        }

                        if (pixel.g > 150 && pixel.b > 150 && pixel.r < 120)
                        {
                            cyanCount++;
                            cyanMinX = Mathf.Min(cyanMinX, x);
                            cyanMaxX = Mathf.Max(cyanMaxX, x);
                        }
                        else if (pixel.r > 150 && pixel.b > 150 && pixel.g < 120)
                        {
                            magentaCount++;
                            magentaMinX = Mathf.Min(magentaMinX, x);
                            magentaMaxX = Mathf.Max(magentaMaxX, x);
                        }
                    }
                }

                return new Hd2dAutonomousP2PaniniScreenshotMetrics
                {
                    FileName = Path.GetFileName(imagePath),
                    Width = texture.width,
                    Height = texture.height,
                    BlackBorderPercent = borderPixels > 0 ? (blackBorderPixels * 100f) / borderPixels : 0f,
                    CyanMarkerPixels = cyanCount,
                    MagentaMarkerPixels = magentaCount,
                    CyanMarkerCenterX = cyanCount > 0 ? (cyanMinX + cyanMaxX) * 0.5f : -1f,
                    MagentaMarkerCenterX = magentaCount > 0 ? (magentaMinX + magentaMaxX) * 0.5f : -1f
                };
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(texture);
            }
        }

        private static void WriteHd2dAutonomousP2PaniniProjectionReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dPaniniProjectionProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics currentDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics pastDiff,
            IReadOnlyList<Hd2dAutonomousP2PaniniScreenshotMetrics> metrics)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# P2-54 Panini Projection Review",
                string.Empty,
                "- Scope: stage a conservative Panini Projection override on the existing shared Global Volume; keep final projection taste as NEEDS-TOM.",
                "- A/B note: captures use temporary cyan/magenta vertical edge probes so the remote review can judge edge compression without relying only on distant building detail.",
                string.Empty,
                "| Profile | Value |",
                "|---|---:|",
                $"| Distance | {profile.distance:0.###} |",
                $"| Crop to fit | {profile.cropToFit:0.###} |",
                $"| Review FOV | {profile.reviewFieldOfView:0.###} |",
                $"| Needs Tom approval | {FormatBool(profile.needsTomApproval)} |",
                $"| Final projection approved | {FormatBool(profile.finalProjectionApproved)} |",
                string.Empty,
                "| A/B evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                currentDiff.ToReportRow("Current plaza Panini off vs on"),
                pastDiff.ToReportRow("Past plaza Panini off vs on"),
                string.Empty,
                "| Screenshot metric | Black border % | Cyan marker px / x | Magenta marker px / x |",
                "|---|---:|---:|---:|"
            };

            for (var i = 0; i < metrics.Count; i++)
            {
                lines.Add(metrics[i].ToReportRow());
            }

            lines.Add(string.Empty);
            lines.Add("| Screenshot | Purpose |");
            lines.Add("|---|---|");
            lines.Add($"| `{screenshotFiles[0]}` | Current Central Plaza/library-edge baseline with Panini disabled |");
            lines.Add($"| `{screenshotFiles[1]}` | Same current view with Panini distance/crop applied |");
            lines.Add($"| `{screenshotFiles[2]}` | Past Central Plaza/library-edge baseline with Panini disabled |");
            lines.Add($"| `{screenshotFiles[3]}` | Same past view with Panini distance/crop applied |");
            lines.Add(string.Empty);
            lines.Add("Recommendation: keep this as conservative data prep only. Tom should compare the edge probes against real building/tree cards and tune Distance/CropToFit before approving final camera projection.");

            File.WriteAllLines(Path.Combine(outputDirectory, "panini_projection_review.md"), lines, Encoding.UTF8);
        }

        private struct Hd2dAutonomousP2PaniniScreenshotMetrics
        {
            public string FileName;
            public int Width;
            public int Height;
            public float BlackBorderPercent;
            public int CyanMarkerPixels;
            public int MagentaMarkerPixels;
            public float CyanMarkerCenterX;
            public float MagentaMarkerCenterX;

            public string ToReportRow()
            {
                return $"| `{FileName}` ({Width}x{Height}) | {BlackBorderPercent:0.###} | {CyanMarkerPixels} / {CyanMarkerCenterX:0.#} | {MagentaMarkerPixels} / {MagentaMarkerCenterX:0.#} |";
            }
        }
    }
}
