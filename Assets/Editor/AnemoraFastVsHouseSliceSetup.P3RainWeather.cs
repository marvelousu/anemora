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
using UnityEngine.SceneManagement;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP3RainWeatherRootName = "FastVS_HD2D_P3_79_RainWeatherState";
        private const string Hd2dAutonomousP3RainWeatherProfilePath = "Assets/Settings/FastVS_HD2D_P3_RainWeatherProfile.asset";
        private const string Hd2dAutonomousP3RainWeatherRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dRainWeatherState.cs";
        private const string Hd2dAutonomousP3RainWeatherProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dRainWeatherProfile.cs";
        private const string Hd2dAutonomousP3RainWeatherEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P3RainWeather.cs";
        private const string Hd2dAutonomousP3RainParticleObjectName = "FastVS_HD2D_P3_79_RainWeather_CameraStreaks";
        private const string Hd2dAutonomousP3RainProxyRootName = "FastVS_HD2D_P3_79_RainWeather_ReviewProxyStreaks";
        private const string Hd2dAutonomousP3RainWetProbeRootName = "FastVS_HD2D_P3_79_RainWeather_WetGroundProbe";
        private const string Hd2dAutonomousP3RainStreakMaterialId = "hd2d_p3_79_rain_streak_particle";
        private const string Hd2dAutonomousP3RainStreakTextureId = "hd2d_p3_79_rain_streak_particle";
        private const string Hd2dAutonomousP3RainWetProbeMaterialId = "hd2d_p3_79_rain_wet_ground_probe";
        private const string Hd2dAutonomousP3RainStreakMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP3RainStreakMaterialId + ".mat";
        private const string Hd2dAutonomousP3RainWetProbeMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP3RainWetProbeMaterialId + ".mat";

        public static void CaptureHd2dAutonomousP3Item79RainWeatherBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var rain = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dRainWeatherState>(FindObjectsInactive.Include);
            var skyDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || rain == null || skyDriver == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-79 rain weather capture failed: review scene components are missing.");
            }

            ValidateHd2dAutonomousP3RainWeatherState();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("stylized_rain_weather_wet_sky_fog");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_clear_weather_dry_ground_baseline.png",
                "02_conservative_rain_streaks_wet_sky_fog_a.png",
                "03_conservative_rain_streaks_motion_b.png",
                "04_lightning_flash_option_for_tom.png",
                "05_rain_off_reset_proof.png"
            };

            try
            {
                guide.SetMovementFrozen(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ClosePortal();
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.18f, 0.02f, 1.82f));
                guide.ApplyActiveTimeIsolationForReview();
                Physics.SyncTransforms();
                realtimeRig.ApplyNowForReview();
                rain.SetReviewWetProbeVisibleForReview(true);
                var anchor = controller.CurrentSpaceRootForReview.TransformPoint(GetHd2dAutonomousP3RainWeatherProbeLocalPosition());
                camera.fieldOfView = 39f;
                PositionCloseReviewCamera(camera, anchor, new Vector3(2.65f, 2.05f, -3.45f), new Vector3(0.05f, 0.38f, 0.16f));
                WarmUpCameraRender(camera);

                CaptureHd2dAutonomousP3RainWeatherShot(rain, skyDriver, 0f, false, 0f, 0f, camera, outputDirectory, screenshotFiles[0]);
                CaptureHd2dAutonomousP3RainWeatherShot(rain, skyDriver, rain.ProfileForReview.ConservativeRainIntensityForReview, true, 0.45f, 0f, camera, outputDirectory, screenshotFiles[1]);
                rain.SimulateForReview(1.25f);
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, screenshotFiles[2]));
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[2]);
                CaptureHd2dAutonomousP3RainWeatherShot(rain, skyDriver, rain.ProfileForReview.ConservativeRainIntensityForReview, true, 0.45f, 1f, camera, outputDirectory, screenshotFiles[3]);
                CaptureHd2dAutonomousP3RainWeatherShot(rain, skyDriver, 0f, false, 0f, 0f, camera, outputDirectory, screenshotFiles[4]);
            }
            finally
            {
                rain.ApplyDefaultReviewStateForReview();
                rain.SetReviewWetProbeVisibleForReview(false);
                skyDriver.ApplyReviewSunDirectionForReview(58f, 172f, 0.18f);
                realtimeRig.ApplyNowForReview();
                guide.SetMovementFrozen(false);
                AssetDatabase.SaveAssets();
            }

            var clearVsRain = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var rainMotion = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var rainVsLightning = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[3]);
            var resetDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[4]);
            WriteHd2dAutonomousP3RainWeatherReviewReport(outputDirectory, screenshotFiles, rain, clearVsRain, rainMotion, rainVsLightning, resetDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P3-79 stylized rain weather review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP3RainWeatherState(Transform currentCentralPlazaRoot, Camera camera)
        {
            if (currentCentralPlazaRoot == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP3RainWeatherProfile();
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP3RainWeatherRootName);
            if (root != null)
            {
                UnityEngine.Object.DestroyImmediate(root);
            }

            root = new GameObject(Hd2dAutonomousP3RainWeatherRootName);
            root.transform.SetParent(currentCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;

            var rainMaterial = EnsureHd2dAutonomousP3RainStreakMaterial();
            var wetProbeMaterial = EnsureHd2dAutonomousP3RainWetProbeMaterial();
            var wetProbeRoot = CreateHd2dAutonomousP3RainWetProbe(root.transform, wetProbeMaterial);
            var particleSystem = CreateHd2dAutonomousP3RainParticleSystem(camera, rainMaterial);
            var proxyRoot = CreateHd2dAutonomousP3RainProxyStreaks(camera, rainMaterial, profile);
            var rain = root.AddComponent<FastVsHd2dRainWeatherState>();
            var skyDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include);
            var directionalSun = FindSceneObjectIncludingInactive("Directional Light")?.GetComponent<Light>();
            rain.ConfigureForReview(profile, skyDriver, particleSystem, proxyRoot, wetProbeRoot, directionalSun);

            var landmark = root.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", "current.central_plaza.hd2d_p3_79.rain_weather_state");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);
            SetHd2dAutonomousP3RainLayerRecursively(root, CurrentSpaceRenderLayer);
        }

        private static void ValidateHd2dAutonomousP3RainWeatherState()
        {
            var profile = EnsureHd2dAutonomousP3RainWeatherProfile();
            var rain = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dRainWeatherState>(FindObjectsInactive.Include);
            var particleMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3RainStreakMaterialPath);
            var wetProbeMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3RainWetProbeMaterialPath);
            var particleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP3RainStreakTextureId + ".asset");
            if (profile == null ||
                rain == null ||
                !rain.IsReadyForReview ||
                rain.ActiveOnAwakeForReview ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalRainWeatherApprovedForReview ||
                !profile.ParticleSystemFallbackForReview ||
                !profile.VfxGraphDeferredForTomForReview ||
                particleMaterial == null ||
                wetProbeMaterial == null ||
                particleTexture == null)
            {
                throw new InvalidOperationException("House slice validation failed: P3-79 needs a non-final rain profile, fallback material/texture, hidden/off driver, and wetness review material.");
            }

            if (profile.DefaultRainIntensityForReview > 0.001f ||
                profile.ConservativeRainIntensityForReview < 0.20f ||
                profile.ConservativeRainIntensityForReview > 0.50f ||
                profile.StrongerRainIntensityForReview <= profile.ConservativeRainIntensityForReview ||
                profile.RainyFogDensityForReview <= 0.012f ||
                profile.WetnessScaleForReview <= 0.20f ||
                profile.RainEmissionRateForReview < 80f ||
                profile.MaxParticlesForReview < 128 ||
                profile.ReviewProxyStreakCountForReview < 24)
            {
                throw new InvalidOperationException("House slice validation failed: P3-79 rain profile must stay disabled by default while exposing conservative/stronger rain, wetness, and visible streak budgets.");
            }

            rain.SetReviewWetProbeVisibleForReview(true);
            rain.ApplyRainAmountForReview(profile.ConservativeRainIntensityForReview, true, 0.75f, 0f);
            if (Mathf.Abs(Shader.GetGlobalFloat("_AnemoraHd2dRainWetness") - rain.CurrentWetnessForReview) > 0.002f ||
                !rain.ParticlesEnabledForReview ||
                rain.LiveParticleCountForReview <= 0 ||
                rain.ReviewRainProxyVisibleCountForReview < 24)
            {
                throw new InvalidOperationException("House slice validation failed: P3-79 conservative rain state must publish wetness and produce particle/proxy streak evidence.");
            }

            rain.ApplyRainAmountForReview(profile.ConservativeRainIntensityForReview, true, 0.75f, 1f);
            if (rain.LastLightningAmountForReview < 0.99f)
            {
                throw new InvalidOperationException("House slice validation failed: P3-79 lightning review flash path did not record a full flash amount.");
            }

            rain.ApplyDefaultReviewStateForReview();
            rain.SetReviewWetProbeVisibleForReview(false);
            if (Shader.GetGlobalFloat("_AnemoraHd2dRainWetness") > 0.002f ||
                rain.LiveParticleCountForReview != 0 ||
                rain.ReviewRainProxyRootForReview.gameObject.activeSelf ||
                rain.ReviewWetProbeVisibleForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-79 default rain state must reset wetness, clear particles, hide proxies, and hide the review wetness probe.");
            }

            var probeRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP3RainWetProbeRootName);
            if (probeRoot == null || probeRoot.GetComponentsInChildren<MeshRenderer>(true).Length < 3)
            {
                throw new InvalidOperationException("House slice validation failed: P3-79 needs dry/wet probe geometry using SurfaceRampLit.");
            }

            var shaderSource = File.ReadAllText("Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader");
            ValidateSourceToken(shaderSource, "_AnemoraHd2dRainWetness", "Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader");
            ValidateSourceToken(shaderSource, "_AnemoraHd2dRainSpecBoost", "Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader");
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3RainWeatherRuntimePath), "ApplyRainAmountForReview", Hd2dAutonomousP3RainWeatherRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3RainWeatherProfileRuntimePath), "finalRainWeatherApproved", Hd2dAutonomousP3RainWeatherProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3RainWeatherEditorPath), "VfxGraphDeferredForTomForReview", Hd2dAutonomousP3RainWeatherEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP3RainWeatherState", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP3RainWeatherState", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dRainWeatherProfile EnsureHd2dAutonomousP3RainWeatherProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dRainWeatherProfile>(Hd2dAutonomousP3RainWeatherProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dRainWeatherProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP3RainWeatherProfilePath);
            }

            profile.ConfigureForReview(
                0f,
                0.42f,
                0.62f,
                new Color(0.42f, 0.48f, 0.56f, 1f),
                new Color(0.22f, 0.28f, 0.36f, 1f),
                new Color(0.45f, 0.52f, 0.59f, 1f),
                0.024f,
                0.74f,
                0.66f,
                0.54f,
                0.34f,
                0.48f,
                new Vector3(0.62f, 0f, 0.78f),
                0.46f,
                8.4f,
                0.34f,
                520f,
                1200,
                0.042f,
                1.05f,
                0.58f,
                76,
                1.65f,
                0.36f,
                "Keep this as conservative rain-weather data only. Tom should approve final VFX Graph rain density, sky grade, fog density, wetness/specular strength, and lightning cadence.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Material EnsureHd2dAutonomousP3RainStreakMaterial()
        {
            EnsureFolder(MaterialDirectory);
            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-79 rain streak particle shader is missing.");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3RainStreakMaterialPath);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP3RainStreakMaterialPath);
            }

            ConfigureTransparentMaterial(material, 3012, shader.name, URPUnlitShaderName);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP3RainStreakTexture(), Vector2.one);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(0.70f, 0.84f, 1.0f, 0.88f));
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", new Color(0.70f, 0.84f, 1.0f, 0.88f));
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", 0f);
            }

            ApplyMaterialRole(material, Hd2dAutonomousP3RainStreakMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP3RainStreakTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP3RainStreakTextureId,
                32,
                128,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = (x + 0.5f) / 32f;
                    var v = (y + 0.5f) / 128f;
                    var line = SmoothFade01(0.250f, 0.024f, Mathf.Abs(u - 0.5f));
                    var tail = SmoothFade01(0.50f, 0.04f, Mathf.Abs(v - 0.52f));
                    var taper = Mathf.SmoothStep(0.18f, 1.0f, v) * (1f - Mathf.SmoothStep(0.90f, 1.0f, v));
                    var alpha = Mathf.Clamp01(line * tail * taper);
                    return new Color(0.68f, 0.82f, 1.0f, alpha);
                });
        }

        private static Material EnsureHd2dAutonomousP3RainWetProbeMaterial()
        {
            EnsureFolder(MaterialDirectory);
            AssetDatabase.ImportAsset(SurfaceRampLitShaderPath, ImportAssetOptions.ForceSynchronousImport);
            var shader = Shader.Find(SurfaceRampLitShaderName) ?? Shader.Find(URPLitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-79 wet probe shader is missing.");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3RainWetProbeMaterialPath);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP3RainWetProbeMaterialPath);
            }

            material.shader = shader;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(0.50f, 0.43f, 0.34f, 1f));
            }

            if (material.HasProperty("_SurfaceRampStrength"))
            {
                material.SetFloat("_SurfaceRampStrength", 0.20f);
            }

            if (material.HasProperty("_Smoothness"))
            {
                material.SetFloat("_Smoothness", 0.62f);
            }

            if (material.HasProperty("_SpecularHighlights"))
            {
                material.SetFloat("_SpecularHighlights", 0.42f);
            }

            if (material.HasProperty("_SpecularStep"))
            {
                material.SetFloat("_SpecularStep", 0.58f);
            }

            ApplyMaterialRole(material, Hd2dAutonomousP3RainWetProbeMaterialId, FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static ParticleSystem CreateHd2dAutonomousP3RainParticleSystem(Camera camera, Material material)
        {
            var particleObject = FindSceneObjectIncludingInactive(Hd2dAutonomousP3RainParticleObjectName);
            if (particleObject == null)
            {
                particleObject = new GameObject(Hd2dAutonomousP3RainParticleObjectName, typeof(ParticleSystem), typeof(ParticleSystemRenderer));
            }

            var parent = camera != null ? camera.transform : null;
            particleObject.transform.SetParent(parent, false);
            particleObject.transform.localPosition = new Vector3(0f, 0.35f, 4.2f);
            particleObject.transform.localRotation = Quaternion.identity;
            particleObject.transform.localScale = Vector3.one;
            particleObject.layer = CurrentSpaceRenderLayer;

            var system = particleObject.GetComponent<ParticleSystem>();
            var renderer = particleObject.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.renderMode = ParticleSystemRenderMode.Stretch;
                renderer.alignment = ParticleSystemRenderSpace.View;
                renderer.lengthScale = 3.8f;
                renderer.velocityScale = 0.28f;
                renderer.cameraVelocityScale = 0f;
                renderer.sortingOrder = 12;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                renderer.forceRenderingOff = false;
            }

            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            return system;
        }

        private static Transform CreateHd2dAutonomousP3RainProxyStreaks(Camera camera, Material material, FastVsHd2dRainWeatherProfile profile)
        {
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP3RainProxyRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var proxyRoot = new GameObject(Hd2dAutonomousP3RainProxyRootName);
            proxyRoot.transform.SetParent(camera != null ? camera.transform : null, false);
            proxyRoot.transform.localPosition = new Vector3(0f, 0.25f, 4.0f);
            proxyRoot.transform.localRotation = Quaternion.identity;
            proxyRoot.transform.localScale = Vector3.one;
            proxyRoot.layer = CurrentSpaceRenderLayer;

            var count = profile != null ? profile.ReviewProxyStreakCountForReview : 52;
            for (var i = 0; i < count; i++)
            {
                var streak = GameObject.CreatePrimitive(PrimitiveType.Quad);
                streak.name = $"{Hd2dAutonomousP3RainProxyRootName}_{i:00}";
                streak.transform.SetParent(proxyRoot.transform, false);
                streak.transform.localRotation = Quaternion.Euler(0f, 0f, -13f);
                streak.layer = CurrentSpaceRenderLayer;
                var renderer = streak.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = material;
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                    renderer.enabled = false;
                }

                var collider = streak.GetComponent<Collider>();
                if (collider != null)
                {
                    UnityEngine.Object.DestroyImmediate(collider);
                }
            }

            proxyRoot.SetActive(false);
            return proxyRoot.transform;
        }

        private static Transform CreateHd2dAutonomousP3RainWetProbe(Transform root, Material material)
        {
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP3RainWetProbeRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var probeRoot = new GameObject(Hd2dAutonomousP3RainWetProbeRootName);
            probeRoot.transform.SetParent(root, false);
            probeRoot.transform.localPosition = GetHd2dAutonomousP3RainWeatherProbeLocalPosition();
            probeRoot.transform.localRotation = Quaternion.Euler(0f, -18f, 0f);
            probeRoot.transform.localScale = Vector3.one;
            probeRoot.layer = CurrentSpaceRenderLayer;

            CreateHd2dAutonomousP3RainWetProbeCube(probeRoot.transform, "GroundSlab", new Vector3(-0.42f, 0.04f, 0f), new Vector3(1.50f, 0.08f, 1.18f), Quaternion.identity, material);
            CreateHd2dAutonomousP3RainWetProbeCube(probeRoot.transform, "RaisedStone", new Vector3(0.44f, 0.16f, -0.22f), new Vector3(0.56f, 0.20f, 0.42f), Quaternion.identity, material);
            CreateHd2dAutonomousP3RainWetProbeCube(probeRoot.transform, "VerticalFace", new Vector3(0.72f, 0.42f, 0.36f), new Vector3(0.12f, 0.80f, 0.78f), Quaternion.identity, material);

            var landmark = probeRoot.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", "current.central_plaza.hd2d_p3_79.rain_wet_ground_probe");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);
            probeRoot.SetActive(false);
            return probeRoot.transform;
        }

        private static void CreateHd2dAutonomousP3RainWetProbeCube(Transform parent, string suffix, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"{Hd2dAutonomousP3RainWetProbeRootName}_{suffix}";
            cube.transform.SetParent(parent, false);
            cube.transform.localPosition = localPosition;
            cube.transform.localScale = localScale;
            cube.transform.localRotation = localRotation;
            cube.layer = CurrentSpaceRenderLayer;
            var renderer = cube.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            var collider = cube.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }

        private static Vector3 GetHd2dAutonomousP3RainWeatherProbeLocalPosition()
        {
            return CentralPlazaVsCenter + new Vector3(1.20f, 0.11f, 2.12f);
        }

        private static void CaptureHd2dAutonomousP3RainWeatherShot(
            FastVsHd2dRainWeatherState rain,
            FastVsHd2dGradientSkyDriver skyDriver,
            float amount,
            bool particlesEnabled,
            float simulateSeconds,
            float lightningAmount,
            Camera camera,
            string outputDirectory,
            string fileName)
        {
            skyDriver.ApplyReviewSunDirectionForReview(58f, 172f, 0.18f);
            rain.ApplyRainAmountForReview(amount, particlesEnabled, simulateSeconds, lightningAmount);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
        }

        private static void WriteHd2dAutonomousP3RainWeatherReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dRainWeatherState rain,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics clearVsRain,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics rainMotion,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics rainVsLightning,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics resetDiff)
        {
            var profile = rain.ProfileForReview;
            var lines = new List<string>
            {
                "# P3-79 Stylized Rain Weather State Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative rain-weather state. Runtime default is rain intensity 0; review captures explicitly enable conservative rain and a lightning option.",
                "- Recommendation: " + profile.RecommendationForReview,
                "- Technical note: VFX Graph is deferred for Tom because the package is not present in the project manifest; this pass uses a camera-parented ParticleSystem fallback plus deterministic review proxy streaks.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP3RainWeatherProfilePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalRainWeatherApprovedForReview)} |",
                $"| Default / conservative / stronger intensity | {profile.DefaultRainIntensityForReview:0.###} / {profile.ConservativeRainIntensityForReview:0.###} / {profile.StrongerRainIntensityForReview:0.###} |",
                $"| Rainy horizon / zenith / fog | {FormatColorForReport(profile.RainyHorizonForReview)} / {FormatColorForReport(profile.RainyZenithForReview)} / {FormatColorForReport(profile.RainyFogColorForReview)} |",
                $"| Fog density / ambient / light multiplier | {profile.RainyFogDensityForReview:0.###} / {profile.RainyAmbientIntensityForReview:0.###} / {profile.DirectionalLightRainMultiplierForReview:0.###} |",
                $"| Wetness scale / darken / spec boost | {profile.WetnessScaleForReview:0.###} / {profile.WetDarkenForReview:0.###} / {profile.WetSpecularBoostForReview:0.###} |",
                $"| Rain streak fallback | ParticleSystem={FormatBool(profile.ParticleSystemFallbackForReview)}, VFX Graph deferred={FormatBool(profile.VfxGraphDeferredForTomForReview)}, max {profile.MaxParticlesForReview}, emission {profile.RainEmissionRateForReview:0.###}/s |",
                $"| Streak width / length / lifetime | {profile.StreakWidthForReview:0.###} / {profile.StreakLengthForReview:0.###} / {profile.StreakLifetimeForReview:0.###}s |",
                $"| Last enabled / live particles at report time | {rain.LastEnabledParticleCountForReview} / {rain.LiveParticleCountForReview} |",
                $"| Review proxy streaks visible at last enabled state | {rain.LastEnabledReviewRainProxyVisibleCountForReview} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                clearVsRain.ToReportRow("clear/dry baseline vs conservative rain"),
                rainMotion.ToReportRow("conservative rain frame A vs frame B falling streak motion"),
                rainVsLightning.ToReportRow("conservative rain vs lightning flash option"),
                resetDiff.ToReportRow("clear baseline vs rain-off reset proof"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Clear-weather dry baseline with the wetness probe visible but global wetness at 0. |",
                $"| `{screenshotFiles[1]}` | Conservative rain: falling streaks, gray sky/fog coupling, and wet/darker top surfaces. |",
                $"| `{screenshotFiles[2]}` | Same conservative rain state advanced in time to show falling streak movement. |",
                $"| `{screenshotFiles[3]}` | Optional one-frame lightning flash value for Tom to approve or reject. |",
                $"| `{screenshotFiles[4]}` | Rain-off reset proof after enabled captures. |",
            };

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllText(Path.Combine(outputDirectory, "stylized_rain_weather_wet_sky_fog_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static void SetHd2dAutonomousP3RainLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetHd2dAutonomousP3RainLayerRecursively(child.gameObject, layer);
            }
        }
    }
}
