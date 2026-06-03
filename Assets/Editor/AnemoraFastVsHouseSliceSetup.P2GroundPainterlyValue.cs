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
        private const string Hd2dAutonomousP2GroundPainterlyValueProfilePath = "Assets/Settings/FastVS_HD2D_P2_GroundPainterlyValueProfile.asset";
        private const string PainterlyGroundUseProperty = "_UsePainterlyGroundValue";
        private const string PainterlyGroundCenterProperty = "_PainterlyGroundCenter";
        private const string PainterlyGroundRadiusProperty = "_PainterlyGroundRadius";
        private const string PainterlyGroundValueStrengthProperty = "_PainterlyGroundValueStrength";
        private const string PainterlyGroundAoStrengthProperty = "_PainterlyGroundAOStrength";
        private const string PainterlyGroundWarmTintProperty = "_PainterlyGroundWarmTint";
        private const string PainterlyGroundCoolTintProperty = "_PainterlyGroundCoolTint";

        private static readonly string[] Hd2dAutonomousP2GroundPainterlyMaterialPaths =
        {
            MaterialDirectory + "/FastVS_House_current_ground.mat",
            MaterialDirectory + "/FastVS_House_current_path.mat",
            MaterialDirectory + "/FastVS_House_current_grass.mat",
            MaterialDirectory + "/FastVS_House_past_path.mat",
            MaterialDirectory + "/FastVS_House_past_grass.mat",
            MaterialDirectory + "/FastVS_House_hd2d_p0_vertex_splat_ground.mat"
        };

        public static void CaptureHd2dAutonomousP2Item55GroundPainterlyValueBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHouseLightingDirector>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || director == null || realtimeRig == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-55 ground painterly value capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2GroundPainterlyValue();
            var profile = EnsureHd2dAutonomousP2GroundPainterlyValueProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("ground_painterly_value_ao");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_current_ground_value_off.png",
                "02_current_ground_value_on.png",
                "03_past_ground_value_off.png",
                "04_past_ground_value_on.png"
            };

            var previousCullingMask = camera.cullingMask;
            try
            {
                CaptureHd2dAutonomousP2GroundPainterlyValueShot(controller, visibility, guide, director, realtimeRig, camera, profile, false, false, previousCullingMask, outputDirectory, screenshotFiles[0]);
                CaptureHd2dAutonomousP2GroundPainterlyValueShot(controller, visibility, guide, director, realtimeRig, camera, profile, true, false, previousCullingMask, outputDirectory, screenshotFiles[1]);
                CaptureHd2dAutonomousP2GroundPainterlyValueShot(controller, visibility, guide, director, realtimeRig, camera, profile, false, true, previousCullingMask, outputDirectory, screenshotFiles[2]);
                CaptureHd2dAutonomousP2GroundPainterlyValueShot(controller, visibility, guide, director, realtimeRig, camera, profile, true, true, previousCullingMask, outputDirectory, screenshotFiles[3]);
            }
            finally
            {
                camera.cullingMask = previousCullingMask;
                ApplyHd2dAutonomousP2GroundPainterlyValueToMaterials(profile, true);
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
            WriteHd2dAutonomousP2GroundPainterlyValueReviewReport(outputDirectory, screenshotFiles, profile, currentDiff, pastDiff);

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-55 ground painterly value/AO review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2GroundPainterlyValue()
        {
            var profile = EnsureHd2dAutonomousP2GroundPainterlyValueProfile();
            ApplyHd2dAutonomousP2GroundPainterlyValueToMaterials(profile, true);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2GroundPainterlyValue()
        {
            var profile = EnsureHd2dAutonomousP2GroundPainterlyValueProfile();
            if (!profile.needsTomApproval || profile.finalGroundArtApproved)
            {
                throw new InvalidOperationException("House slice validation failed: P2-55 ground painterly value profile must remain NEEDS-TOM and final art approval must stay false.");
            }

            if (profile.radius < 6f || profile.radius > 24f || profile.valueStrength <= 0f || profile.valueStrength > 0.22f || profile.aoStrength <= 0f || profile.aoStrength > 0.28f)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-55 ground painterly value profile is outside conservative bounds. radius={profile.radius:0.###}, value={profile.valueStrength:0.###}, ao={profile.aoStrength:0.###}");
            }

            var shaderSource = File.Exists(SurfaceRampLitShaderPath) ? File.ReadAllText(SurfaceRampLitShaderPath) : string.Empty;
            ValidateSourceToken(shaderSource, PainterlyGroundUseProperty, SurfaceRampLitShaderPath);
            ValidateSourceToken(shaderSource, "painterlyGroundEnabled", SurfaceRampLitShaderPath);
            ValidateSourceToken(shaderSource, "vertexPaintAo", SurfaceRampLitShaderPath);

            for (var i = 0; i < Hd2dAutonomousP2GroundPainterlyMaterialPaths.Length; i++)
            {
                var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2GroundPainterlyMaterialPaths[i]);
                if (material == null)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-55 expected ground material is missing: {Hd2dAutonomousP2GroundPainterlyMaterialPaths[i]}");
                }

                if (!material.HasProperty(PainterlyGroundUseProperty) ||
                    Mathf.Abs(material.GetFloat(PainterlyGroundUseProperty) - 1f) > 0.001f ||
                    Mathf.Abs(material.GetFloat(PainterlyGroundRadiusProperty) - profile.radius) > 0.001f ||
                    Mathf.Abs(material.GetFloat(PainterlyGroundValueStrengthProperty) - profile.valueStrength) > 0.001f ||
                    Mathf.Abs(material.GetFloat(PainterlyGroundAoStrengthProperty) - profile.aoStrength) > 0.001f)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-55 ground material is not configured from the profile: {Hd2dAutonomousP2GroundPainterlyMaterialPaths[i]}");
                }
            }
        }

        private static FastVsHd2dGroundPainterlyValueProfile EnsureHd2dAutonomousP2GroundPainterlyValueProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dGroundPainterlyValueProfile>(Hd2dAutonomousP2GroundPainterlyValueProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dGroundPainterlyValueProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2GroundPainterlyValueProfilePath);
            }

            profile.centerXZ = new Vector2(CentralPlazaVsCenter.x, CentralPlazaVsCenter.z);
            profile.radius = 12f;
            profile.valueStrength = 0.12f;
            profile.aoStrength = 0.18f;
            profile.warmCenterTint = new Color(1.04f, 1.02f, 0.94f, 1f);
            profile.coolRecessTint = new Color(0.86f, 0.91f, 1.00f, 1f);
            profile.needsTomApproval = true;
            profile.finalGroundArtApproved = false;
            profile.reviewNotes =
                "Conservative P2-55 ground value/AO baseline. Tom should tune center/radius/tints and replace procedural recess AO with painted vertex/texture masks before final approval.";
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static void ApplyHd2dAutonomousP2GroundPainterlyValueToMaterials(FastVsHd2dGroundPainterlyValueProfile profile, bool enabled)
        {
            for (var i = 0; i < Hd2dAutonomousP2GroundPainterlyMaterialPaths.Length; i++)
            {
                var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2GroundPainterlyMaterialPaths[i]);
                if (material == null)
                {
                    continue;
                }

                if (material.HasProperty(PainterlyGroundUseProperty))
                {
                    material.SetFloat(PainterlyGroundUseProperty, enabled ? 1f : 0f);
                }

                if (material.HasProperty(PainterlyGroundCenterProperty))
                {
                    material.SetVector(PainterlyGroundCenterProperty, new Vector4(profile.centerXZ.x, profile.centerXZ.y, 0f, 0f));
                }

                if (material.HasProperty(PainterlyGroundRadiusProperty))
                {
                    material.SetFloat(PainterlyGroundRadiusProperty, profile.radius);
                }

                if (material.HasProperty(PainterlyGroundValueStrengthProperty))
                {
                    material.SetFloat(PainterlyGroundValueStrengthProperty, profile.valueStrength);
                }

                if (material.HasProperty(PainterlyGroundAoStrengthProperty))
                {
                    material.SetFloat(PainterlyGroundAoStrengthProperty, profile.aoStrength);
                }

                if (material.HasProperty(PainterlyGroundWarmTintProperty))
                {
                    material.SetColor(PainterlyGroundWarmTintProperty, profile.warmCenterTint);
                }

                if (material.HasProperty(PainterlyGroundCoolTintProperty))
                {
                    material.SetColor(PainterlyGroundCoolTintProperty, profile.coolRecessTint);
                }

                EditorUtility.SetDirty(material);
            }
        }

        private static void CaptureHd2dAutonomousP2GroundPainterlyValueShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsHouseLightingDirector director,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            FastVsHd2dGroundPainterlyValueProfile profile,
            bool enabled,
            bool pastTimeline,
            int previousCullingMask,
            string outputDirectory,
            string fileName)
        {
            ApplyHd2dAutonomousP2GroundPainterlyValueToMaterials(profile, enabled);
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            director.ApplyAreaForReview(FastVsHouseArea.CentralPlaza);
            var playerLocal = CentralPlazaVsCenter + new Vector3(-0.18f, 0.02f, 4.32f);
            var anchorLocal = CentralPlazaVsCenter + new Vector3(-0.08f, 0.42f, 5.38f);
            var cameraOffset = new Vector3(0.44f, 2.16f, -4.96f);
            var lookOffset = new Vector3(0.02f, -0.10f, 0.72f);

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
            camera.fieldOfView = 31f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            ApplyStage7BokehFocusForReview(camera);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
        }

        private static void WriteHd2dAutonomousP2GroundPainterlyValueReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dGroundPainterlyValueProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics currentDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics pastDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# P2-55 Ground Painterly Value / AO Review",
                string.Empty,
                "- Scope: stage world-space value falloff and procedural/vertex-painted AO support on SurfaceRampLit ground materials; keep final tints and masks as NEEDS-TOM.",
                string.Empty,
                "| Profile | Value |",
                "|---|---:|",
                $"| Center XZ | {profile.centerXZ.x:0.###}, {profile.centerXZ.y:0.###} |",
                $"| Radius | {profile.radius:0.###} |",
                $"| Value strength | {profile.valueStrength:0.###} |",
                $"| AO strength | {profile.aoStrength:0.###} |",
                $"| Needs Tom approval | {FormatBool(profile.needsTomApproval)} |",
                $"| Final ground art approved | {FormatBool(profile.finalGroundArtApproved)} |",
                string.Empty,
                "| A/B evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                currentDiff.ToReportRow("Current ground painterly value off vs on"),
                pastDiff.ToReportRow("Past ground painterly value off vs on"),
                string.Empty,
                "| Material |",
                "|---|"
            };

            for (var i = 0; i < Hd2dAutonomousP2GroundPainterlyMaterialPaths.Length; i++)
            {
                lines.Add($"| `{Hd2dAutonomousP2GroundPainterlyMaterialPaths[i]}` |");
            }

            lines.Add(string.Empty);
            lines.Add("| Screenshot | Purpose |");
            lines.Add("|---|---|");
            lines.Add($"| `{screenshotFiles[0]}` | Current plaza ground baseline with painterly value/AO disabled |");
            lines.Add($"| `{screenshotFiles[1]}` | Same current view with staged painterly value/AO enabled |");
            lines.Add($"| `{screenshotFiles[2]}` | Past plaza ground baseline with painterly value/AO disabled |");
            lines.Add($"| `{screenshotFiles[3]}` | Same past view with staged painterly value/AO enabled |");
            lines.Add(string.Empty);
            lines.Add("Recommendation: keep the shader/material/profile hook if the A/B reads as value structure rather than dirt/noise. Tom should replace the procedural recess mask with hand-painted vertex/texture AO and approve final tint strength.");

            File.WriteAllLines(Path.Combine(outputDirectory, "ground_painterly_value_ao_review.md"), lines, Encoding.UTF8);
        }
    }
}
