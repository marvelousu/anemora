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
        private const float Hd2dAutonomousCaptureHarnessProofBloomThreshold = 0.10f;
        private const float Hd2dAutonomousCaptureHarnessProofBloomIntensity = 2.80f;
        private const float Hd2dAutonomousCaptureHarnessProofBloomScatter = 0.88f;
        private const float Hd2dAutonomousCaptureHarnessProofVolumePriority = 5000f;

        public static void CaptureHd2dAutonomousCaptureHarnessProofBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous capture harness proof failed: required review components are missing.");
            }

            ValidateHd2dAutonomousCaptureHarnessProof();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("capture_harness_proof");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_bloom_off_postprocess_proof.png",
                "02_bloom_on_postprocess_proof.png"
            };

            var temporaryObjects = new List<UnityEngine.Object>();
            var previousCullingMask = camera.cullingMask;
            var previousOrthographic = camera.orthographic;
            var previousFieldOfView = camera.fieldOfView;
            try
            {
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                var playerLocal = CentralPlazaVsCenter + new Vector3(0.06f, 0.02f, 4.62f);
                var anchorLocal = CentralPlazaVsCenter + new Vector3(1.45f, 0.02f, -0.20f);
                controller.ForcePlayerCurrentLocalForReview(playerLocal);
                guide.ApplyActiveTimeIsolationForReview();
                PositionChapter1AllMapsCamera(
                    camera,
                    controller.CurrentSpaceRootForReview.TransformPoint(anchorLocal),
                    new Vector3(0f, 17.90f, -25.20f),
                    new Vector3(0.10f, 0.20f, 2.90f));
                camera.orthographic = false;
                camera.fieldOfView = RuntimeVsFollowCameraFov;
                EnsureReviewCameraPostProcessingForCapture(camera);
                ApplyStage7BokehFocusForReview(camera);

                CreateHd2dAutonomousCaptureHarnessProofMarkers(camera, controller, temporaryObjects);
                var proofVolume = CreateHd2dAutonomousCaptureHarnessProofVolume(temporaryObjects);

                ConfigureHd2dAutonomousCaptureHarnessProofBloom(proofVolume.sharedProfile, false);
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, screenshotFiles[0]));

                ConfigureHd2dAutonomousCaptureHarnessProofBloom(proofVolume.sharedProfile, true);
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, screenshotFiles[1]));
            }
            finally
            {
                camera.cullingMask = previousCullingMask;
                camera.orthographic = previousOrthographic;
                camera.fieldOfView = previousFieldOfView;
                for (var i = temporaryObjects.Count - 1; i >= 0; i--)
                {
                    if (temporaryObjects[i] != null)
                    {
                        UnityEngine.Object.DestroyImmediate(temporaryObjects[i]);
                    }
                }
            }

            var bloomDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                4);
            if (bloomDiff.SampleCount <= 0 || bloomDiff.ChangedPixels <= 0)
            {
                throw new InvalidOperationException("Fast VS autonomous capture harness proof failed: known Bloom off/on post-process A/B produced 0 changed pixels.");
            }

            WriteHd2dAutonomousCaptureHarnessProofReviewReport(outputDirectory, screenshotFiles, bloomDiff, camera);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous capture harness proof captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void ValidateHd2dAutonomousCaptureHarnessProof()
        {
            var camera = Camera.main;
            EnsureReviewCameraPostProcessingForCapture(camera);
            var additionalData = camera.GetComponent<UniversalAdditionalCameraData>();
            if (!camera.allowHDR ||
                additionalData == null ||
                !additionalData.renderPostProcessing ||
                !additionalData.requiresDepthTexture ||
                !additionalData.requiresColorTexture)
            {
                throw new InvalidOperationException("House slice validation failed: capture harness camera must have HDR, post-processing, depth texture, and color texture enabled for review captures.");
            }

            var proofSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.CaptureHarnessProof.cs");
            var proofSource = File.ReadAllText(proofSourcePath);
            foreach (var token in new[]
            {
                "CaptureHd2dAutonomousCaptureHarnessProofBatch",
                "capture_harness_proof",
                "01_bloom_off_postprocess_proof.png",
                "02_bloom_on_postprocess_proof.png",
                "known Bloom off/on post-process A/B"
            })
            {
                ValidateSourceToken(proofSource, token, proofSourcePath);
            }

            var editorSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.cs");
            var editorSource = File.ReadAllText(editorSourcePath);
            ValidateSourceToken(editorSource, "EnsureReviewCameraPostProcessingForCapture", editorSourcePath);
        }

        private static Volume CreateHd2dAutonomousCaptureHarnessProofVolume(List<UnityEngine.Object> temporaryObjects)
        {
            var volumeObject = new GameObject("FastVS_HD2D_CaptureHarnessProofVolume");
            temporaryObjects.Add(volumeObject);
            var profile = ScriptableObject.CreateInstance<VolumeProfile>();
            profile.name = "FastVS_HD2D_CaptureHarnessProof_RuntimeVolume";
            temporaryObjects.Add(profile);
            var volume = volumeObject.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = Hd2dAutonomousCaptureHarnessProofVolumePriority;
            volume.weight = 1f;
            volume.sharedProfile = profile;
            ConfigureHd2dAutonomousCaptureHarnessProofBloom(profile, false);
            return volume;
        }

        private static void ConfigureHd2dAutonomousCaptureHarnessProofBloom(VolumeProfile profile, bool enabled)
        {
            if (profile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous capture harness proof failed: proof volume profile is missing.");
            }

            if (!profile.TryGet<Bloom>(out var bloom))
            {
                bloom = profile.Add<Bloom>(true);
            }

            bloom.active = true;
            bloom.threshold.overrideState = true;
            bloom.threshold.value = Hd2dAutonomousCaptureHarnessProofBloomThreshold;
            bloom.intensity.overrideState = true;
            bloom.intensity.value = enabled ? Hd2dAutonomousCaptureHarnessProofBloomIntensity : 0f;
            bloom.scatter.overrideState = true;
            bloom.scatter.value = Hd2dAutonomousCaptureHarnessProofBloomScatter;
            bloom.clamp.overrideState = true;
            bloom.clamp.value = 64f;
            bloom.tint.overrideState = true;
            bloom.tint.value = Color.white;
            bloom.highQualityFiltering.overrideState = true;
            bloom.highQualityFiltering.value = true;
            bloom.filter.overrideState = true;
            bloom.filter.value = BloomFilterMode.Gaussian;
            bloom.downscale.overrideState = true;
            bloom.downscale.value = BloomDownscaleMode.Half;
            bloom.maxIterations.overrideState = true;
            bloom.maxIterations.value = 6;
            bloom.dirtIntensity.overrideState = true;
            bloom.dirtIntensity.value = 0f;
        }

        private static void CreateHd2dAutonomousCaptureHarnessProofMarkers(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            List<UnityEngine.Object> temporaryObjects)
        {
            var markerShader = Shader.Find("Universal Render Pipeline/Unlit");
            if (markerShader == null)
            {
                markerShader = Shader.Find("Unlit/Color");
            }

            if (markerShader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous capture harness proof failed: no unlit shader is available for proof markers.");
            }

            var darkMaterial = new Material(markerShader)
            {
                name = "FastVS_HD2D_CaptureHarnessProof_DarkBackdrop"
            };
            SetHd2dAutonomousCaptureHarnessProofMaterialColor(darkMaterial, new Color(0.025f, 0.023f, 0.021f, 1f));
            temporaryObjects.Add(darkMaterial);

            var hotMaterial = new Material(markerShader)
            {
                name = "FastVS_HD2D_CaptureHarnessProof_HdrChip"
            };
            SetHd2dAutonomousCaptureHarnessProofMaterialColor(hotMaterial, new Color(10.0f, 7.2f, 2.4f, 1f));
            temporaryObjects.Add(hotMaterial);

            var center = camera.transform.position + (camera.transform.forward * 8.0f) + (camera.transform.up * 0.35f);
            var rotation = Quaternion.LookRotation(-camera.transform.forward, camera.transform.up);
            var layer = Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);

            CreateHd2dAutonomousCaptureHarnessProofQuad(
                "FastVS_HD2D_CaptureHarnessProof_DarkBackdrop",
                center + camera.transform.forward * 0.04f,
                rotation,
                new Vector3(2.80f, 1.70f, 1f),
                darkMaterial,
                layer,
                temporaryObjects);
            CreateHd2dAutonomousCaptureHarnessProofQuad(
                "FastVS_HD2D_CaptureHarnessProof_HdrChip",
                center,
                rotation,
                new Vector3(0.74f, 0.74f, 1f),
                hotMaterial,
                layer,
                temporaryObjects);
        }

        private static void CreateHd2dAutonomousCaptureHarnessProofQuad(
            string name,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale,
            Material material,
            int layer,
            List<UnityEngine.Object> temporaryObjects)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = name;
            quad.layer = layer;
            quad.transform.SetPositionAndRotation(position, rotation);
            quad.transform.localScale = scale;
            var collider = quad.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = quad.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            temporaryObjects.Add(quad);
        }

        private static void SetHd2dAutonomousCaptureHarnessProofMaterialColor(Material material, Color color)
        {
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }
        }

        private static void WriteHd2dAutonomousCaptureHarnessProofReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            Hd2dAutonomousP1DepthPrimingDiffMetrics bloomDiff,
            Camera camera)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var additionalData = camera != null ? camera.GetComponent<UniversalAdditionalCameraData>() : null;
            var lines = new List<string>
            {
                "# Capture Harness Proof Review",
                string.Empty,
                "- Scope: prove the batch review camera can capture a known post-processing A/B before more effect items are attempted.",
                "- A/B note: a temporary HDR chip and dark card are placed in the diorama-framed current Central Plaza view; a high-priority temporary Volume toggles only Bloom intensity.",
                string.Empty,
                "| Harness setting | Value |",
                "|---|---:|",
                $"| Review camera HDR | {FormatBool(camera != null && camera.allowHDR)} |",
                $"| Review camera post-processing | {FormatBool(additionalData != null && additionalData.renderPostProcessing)} |",
                $"| Review camera depth texture | {FormatBool(additionalData != null && additionalData.requiresDepthTexture)} |",
                $"| Review camera color texture | {FormatBool(additionalData != null && additionalData.requiresColorTexture)} |",
                $"| Temporary proof Volume priority | {Hd2dAutonomousCaptureHarnessProofVolumePriority:0.###} |",
                $"| Proof Bloom threshold/intensity/scatter | {Hd2dAutonomousCaptureHarnessProofBloomThreshold:0.###} / {Hd2dAutonomousCaptureHarnessProofBloomIntensity:0.###} / {Hd2dAutonomousCaptureHarnessProofBloomScatter:0.###} |",
                string.Empty,
                "| A/B evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                bloomDiff.ToReportRow("Known Bloom off vs on"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Diorama-framed current Central Plaza proof card with Bloom intensity forced to 0 |",
                $"| `{screenshotFiles[1]}` | Same proof card with temporary high-priority Bloom enabled |",
                string.Empty,
                "Recommendation: keep the SaveCameraPng preflight that forces HDR, post-processing, depth texture, color texture, and all volume layers before every automated review capture."
            };

            File.WriteAllText(Path.Combine(outputDirectory, "capture_harness_proof_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }
    }
}
