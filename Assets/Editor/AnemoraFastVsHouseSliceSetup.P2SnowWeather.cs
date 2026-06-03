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
        private const string Hd2dAutonomousP2SnowWeatherRootName = "FastVS_HD2D_P2_72_SnowWeatherState";
        private const string Hd2dAutonomousP2SnowWeatherProfilePath = "Assets/Settings/FastVS_HD2D_P2_SnowWeatherProfile.asset";
        private const string Hd2dAutonomousP2SnowWeatherDriverRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dSnowWeatherState.cs";
        private const string Hd2dAutonomousP2SnowWeatherProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dSnowWeatherProfile.cs";
        private const string Hd2dAutonomousP2SnowFlakeMaterialId = "hd2d_p2_snow_weather_flake_particle";
        private const string Hd2dAutonomousP2SnowFlakeMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2SnowFlakeMaterialId + ".mat";
        private const string Hd2dAutonomousP2SnowFlakeTextureId = "hd2d_p2_snow_weather_flake_particle";
        private const string Hd2dAutonomousP2SnowAccumulationReviewMaterialId = "hd2d_p2_snow_accumulation_review_surface";
        private const string Hd2dAutonomousP2SnowAccumulationReviewMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2SnowAccumulationReviewMaterialId + ".mat";
        private const string Hd2dAutonomousP2SnowParticleObjectName = "FastVS_HD2D_P2_72_SnowWeather_DriftingFlakes";
        private const string Hd2dAutonomousP2SnowProxyFlakeRootName = "FastVS_HD2D_P2_72_SnowWeather_WindDriftProxyFlakes";
        private const string Hd2dAutonomousP2SnowProbeRootName = "FastVS_HD2D_P2_72_SnowAccumulationTopSideProbe";

        public static void CaptureHd2dAutonomousP2Item72SnowWeatherBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var snow = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dSnowWeatherState>(FindObjectsInactive.Include);
            var skyDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || snow == null || skyDriver == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-72 snow weather capture failed: review scene components are missing.");
            }

            ValidateHd2dAutonomousP2SnowWeatherState();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("stylized_snow_weather_state");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_snow_amount_0_clear_baseline.png",
                "02_conservative_snow_accumulation_flakes_a.png",
                "03_conservative_snow_accumulation_flakes_b_wind_drift.png",
                "04_stronger_snow_amount_option_for_tom.png",
                "05_snow_amount_0_reset_proof.png"
            };

            var profile = snow.ProfileForReview;
            var probeAnchor = controller.CurrentSpaceRootForReview.TransformPoint(GetHd2dAutonomousP2SnowWeatherProbeLocalPosition());
            try
            {
                guide.SetMovementFrozen(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.1f, 0.02f, 1.9f));
                guide.ApplyActiveTimeIsolationForReview();
                Physics.SyncTransforms();
                realtimeRig.ApplyNowForReview();
                camera.fieldOfView = 34f;
                PositionCloseReviewCamera(camera, probeAnchor, new Vector3(2.8f, 2.15f, -3.55f), new Vector3(0.08f, 0.44f, 0.02f));
                WarmUpCameraRender(camera);

                CaptureHd2dAutonomousP2SnowWeatherShot(snow, skyDriver, 0f, false, 0f, camera, outputDirectory, screenshotFiles[0]);
                CaptureHd2dAutonomousP2SnowWeatherShot(snow, skyDriver, profile.ConservativeSnowAmountForReview, true, 4.2f, camera, outputDirectory, screenshotFiles[1]);
                snow.SimulateForReview(0.85f);
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, screenshotFiles[2]));
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[2]);
                CaptureHd2dAutonomousP2SnowWeatherShot(snow, skyDriver, profile.StrongerSnowAmountForReview, true, 4.8f, camera, outputDirectory, screenshotFiles[3]);
                CaptureHd2dAutonomousP2SnowWeatherShot(snow, skyDriver, 0f, false, 0f, camera, outputDirectory, screenshotFiles[4]);
            }
            finally
            {
                snow.ApplyDefaultReviewStateForReview();
                skyDriver.ApplyReviewSunDirectionForReview(28f, -128f, 0f);
                guide.SetMovementFrozen(false);
            }

            var baselineVsSnow = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var driftDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var snowVsStronger = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[3]);
            var resetDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[4]);
            WriteHd2dAutonomousP2SnowWeatherReviewReport(outputDirectory, screenshotFiles, snow, baselineVsSnow, driftDiff, snowVsStronger, resetDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-72 stylized snow weather review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2SnowWeatherState(Transform currentCentralPlazaRoot, Camera camera)
        {
            if (currentCentralPlazaRoot == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP2SnowWeatherProfile();
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2SnowWeatherRootName);
            if (root != null)
            {
                UnityEngine.Object.DestroyImmediate(root);
            }

            root = new GameObject(Hd2dAutonomousP2SnowWeatherRootName);
            root.transform.SetParent(currentCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;

            CreateHd2dAutonomousP2SnowAccumulationProbe(root.transform, EnsureHd2dAutonomousP2SnowAccumulationReviewMaterial());
            var flakeMaterial = EnsureHd2dAutonomousP2SnowFlakeMaterial();
            var particleSystem = CreateHd2dAutonomousP2SnowParticleSystem(root.transform, flakeMaterial);
            var proxyFlakeRoot = CreateHd2dAutonomousP2SnowProxyFlakes(root.transform, flakeMaterial, camera);
            var snow = root.AddComponent<FastVsHd2dSnowWeatherState>();
            var skyDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGradientSkyDriver>(FindObjectsInactive.Include);
            var directionalSun = FindSceneObjectIncludingInactive("Directional Light")?.GetComponent<Light>();
            snow.ConfigureForReview(profile, skyDriver, particleSystem, proxyFlakeRoot, directionalSun);

            var landmark = root.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", "current.central_plaza.hd2d_p2_72.snow_weather_state");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);
            SetHd2dAutonomousP2SnowLayerRecursively(root, CurrentSpaceRenderLayer);
        }

        private static void ValidateHd2dAutonomousP2SnowWeatherState()
        {
            var profile = EnsureHd2dAutonomousP2SnowWeatherProfile();
            var snow = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dSnowWeatherState>(FindObjectsInactive.Include);
            var particleMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2SnowFlakeMaterialPath);
            var reviewMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2SnowAccumulationReviewMaterialPath);
            var particleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP2SnowFlakeTextureId + ".asset");
            if (profile == null ||
                snow == null ||
                !snow.IsReadyForReview ||
                snow.ReviewFlakeProxyRootForReview == null ||
                snow.ActiveOnAwakeForReview ||
                profile.FinalSnowWeatherApprovedForReview ||
                !profile.NeedsTomApprovalForReview ||
                !profile.ParticleSystemFallbackForReview ||
                !profile.VfxGraphDeferredForTomForReview ||
                particleMaterial == null ||
                reviewMaterial == null ||
                particleTexture == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-72 needs non-final snow profile, hidden/off driver, particle material/texture, and review accumulation material.");
            }

            if (profile.DefaultSnowAmountForReview > 0.001f ||
                profile.ConservativeSnowAmountForReview < 0.18f ||
                profile.ConservativeSnowAmountForReview > 0.38f ||
                profile.StrongerSnowAmountForReview <= profile.ConservativeSnowAmountForReview ||
                profile.TopNormalPowerForReview < 2f ||
                profile.WindDriftSpeedForReview <= 0.05f ||
                profile.FallSpeedForReview <= 0.05f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-72 snow profile must stay disabled by default with conservative/stronger Tom-facing snow amounts and wind drift values.");
            }

            snow.ApplySnowAmountForReview(profile.ConservativeSnowAmountForReview, true, 3.2f);
            if (Mathf.Abs(Shader.GetGlobalFloat("_AnemoraHd2dSnowAmount") - profile.ConservativeSnowAmountForReview) > 0.002f ||
                !snow.ParticlesEnabledForReview ||
                snow.LiveParticleCountForReview <= 0)
            {
                throw new InvalidOperationException("House slice validation failed: P2-72 conservative snow state must publish _SnowAmount and produce drifting particle evidence.");
            }

            if (snow.ReviewFlakeProxyVisibleCountForReview < 18)
            {
                throw new InvalidOperationException("House slice validation failed: P2-72 conservative snow state must show deterministic review proxy flakes for wind-drift screenshots.");
            }

            snow.ApplyDefaultReviewStateForReview();
            if (Shader.GetGlobalFloat("_AnemoraHd2dSnowAmount") > 0.002f || snow.LiveParticleCountForReview != 0)
            {
                throw new InvalidOperationException("House slice validation failed: P2-72 default snow state must reset _SnowAmount to 0 and clear particles.");
            }

            if (snow.ReviewFlakeProxyRootForReview.gameObject.activeSelf)
            {
                throw new InvalidOperationException("House slice validation failed: P2-72 default snow state must hide review proxy flakes.");
            }

            var probeRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP2SnowProbeRootName);
            if (probeRoot == null || probeRoot.GetComponentsInChildren<MeshRenderer>(true).Length < 3)
            {
                throw new InvalidOperationException("House slice validation failed: P2-72 needs top/side/slope review probe geometry using the surface ramp shader.");
            }

            var shaderSource = File.ReadAllText("Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader");
            ValidateSourceToken(shaderSource, "_AnemoraHd2dSnowAmount", "Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader");
            ValidateSourceToken(shaderSource, "pow(saturate(normalWS.y)", "Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader");
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2SnowWeatherDriverRuntimePath), "ApplySnowAmountForReview", Hd2dAutonomousP2SnowWeatherDriverRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2SnowWeatherProfileRuntimePath), "finalSnowWeatherApproved", Hd2dAutonomousP2SnowWeatherProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2SnowWeatherState", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2SnowWeatherState", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dSnowWeatherProfile EnsureHd2dAutonomousP2SnowWeatherProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dSnowWeatherProfile>(Hd2dAutonomousP2SnowWeatherProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dSnowWeatherProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2SnowWeatherProfilePath);
            }

            profile.ConfigureForReview(
                0f,
                0.28f,
                0.54f,
                new Color(0.92f, 0.965f, 1.0f, 1f),
                4.4f,
                1.85f,
                0.18f,
                new Color(0.76f, 0.84f, 0.92f, 1f),
                new Color(0.48f, 0.58f, 0.70f, 1f),
                new Color(0.72f, 0.82f, 0.92f, 1f),
                0.018f,
                1.08f,
                new Vector3(0.78f, 0f, 0.62f),
                0.48f,
                0.58f,
                0.34f,
                92f,
                360,
                0.075f,
                0.130f,
                5.2f,
                "Keep this as conservative snow-weather data only. Tom should approve final snow amount, sky brightness, flake density, drift speed, and whether to replace the ParticleSystem fallback with VFX Graph turbulence.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Material EnsureHd2dAutonomousP2SnowAccumulationReviewMaterial()
        {
            EnsureFolder(MaterialDirectory);
            AssetDatabase.ImportAsset(SurfaceRampLitShaderPath, ImportAssetOptions.ForceSynchronousImport);
            var shader = Shader.Find(SurfaceRampLitShaderName) ?? Shader.Find(URPLitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-72 snow accumulation shader is missing.");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2SnowAccumulationReviewMaterialPath);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP2SnowAccumulationReviewMaterialPath);
            }

            material.shader = shader;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(0.48f, 0.42f, 0.34f, 1f));
            }

            if (material.HasProperty("_SurfaceRampStrength"))
            {
                material.SetFloat("_SurfaceRampStrength", 0.18f);
            }

            if (material.HasProperty("_SpecularHighlights"))
            {
                material.SetFloat("_SpecularHighlights", 0.08f);
            }

            ApplyMaterialRole(material, Hd2dAutonomousP2SnowAccumulationReviewMaterialId, FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureHd2dAutonomousP2SnowFlakeMaterial()
        {
            EnsureFolder(MaterialDirectory);
            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-72 snowflake particle shader is missing.");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2SnowFlakeMaterialPath);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP2SnowFlakeMaterialPath);
            }

            ConfigureTransparentMaterial(material, 3004, shader.name, URPUnlitShaderName);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP2SnowFlakeTexture(), Vector2.one);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", new Color(0.94f, 0.985f, 1.0f, 0.96f));
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", new Color(0.94f, 0.985f, 1.0f, 0.96f));
            }

            ApplyMaterialRole(material, Hd2dAutonomousP2SnowFlakeMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP2SnowFlakeTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2SnowFlakeTextureId,
                64,
                64,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = (x + 0.5f) / 64f;
                    var v = (y + 0.5f) / 64f;
                    var dx = u - 0.5f;
                    var dy = v - 0.5f;
                    var radius = Mathf.Sqrt((dx * dx) + (dy * dy));
                    var core = SmoothFade01(0.20f, 0.02f, radius);
                    var armA = SmoothFade01(0.030f, 0.004f, Mathf.Abs(dy)) * SmoothFade01(0.34f, 0.02f, Mathf.Abs(dx));
                    var armB = SmoothFade01(0.030f, 0.004f, Mathf.Abs(dx)) * SmoothFade01(0.34f, 0.02f, Mathf.Abs(dy));
                    var alpha = Mathf.Clamp01((core * 0.78f) + (armA * 0.28f) + (armB * 0.28f));
                    alpha *= SmoothFade01(0.48f, 0.32f, radius);
                    return new Color(0.90f, 0.96f, 1.0f, alpha);
                });
        }

        private static ParticleSystem CreateHd2dAutonomousP2SnowParticleSystem(Transform root, Material material)
        {
            var particleObject = FindSceneObjectIncludingInactive(Hd2dAutonomousP2SnowParticleObjectName);
            if (particleObject == null)
            {
                particleObject = new GameObject(Hd2dAutonomousP2SnowParticleObjectName, typeof(ParticleSystem), typeof(ParticleSystemRenderer));
            }

            particleObject.transform.SetParent(root, false);
            particleObject.transform.localPosition = GetHd2dAutonomousP2SnowWeatherProbeLocalPosition() + new Vector3(0f, 1.55f, 0f);
            particleObject.transform.localRotation = Quaternion.identity;
            particleObject.transform.localScale = Vector3.one;
            particleObject.layer = CurrentSpaceRenderLayer;

            var system = particleObject.GetComponent<ParticleSystem>();
            var renderer = particleObject.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            return system;
        }

        private static Transform CreateHd2dAutonomousP2SnowProxyFlakes(Transform root, Material material, Camera camera)
        {
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2SnowProxyFlakeRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var proxyRoot = new GameObject(Hd2dAutonomousP2SnowProxyFlakeRootName);
            proxyRoot.transform.SetParent(root, false);
            proxyRoot.transform.localPosition = GetHd2dAutonomousP2SnowWeatherProbeLocalPosition() + new Vector3(0f, 0.25f, 0f);
            proxyRoot.transform.localRotation = Quaternion.identity;
            proxyRoot.transform.localScale = Vector3.one;
            proxyRoot.layer = CurrentSpaceRenderLayer;

            var flakeRotation = camera != null ? camera.transform.rotation : Quaternion.Euler(54f, 0f, 0f);
            for (var i = 0; i < 28; i++)
            {
                var flake = GameObject.CreatePrimitive(PrimitiveType.Quad);
                flake.name = $"{Hd2dAutonomousP2SnowProxyFlakeRootName}_{i:00}";
                flake.transform.SetParent(proxyRoot.transform, false);
                flake.transform.rotation = flakeRotation;
                flake.layer = CurrentSpaceRenderLayer;

                var renderer = flake.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = material;
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                    renderer.enabled = false;
                }

                var collider = flake.GetComponent<Collider>();
                if (collider != null)
                {
                    UnityEngine.Object.DestroyImmediate(collider);
                }
            }

            proxyRoot.SetActive(false);
            return proxyRoot.transform;
        }

        private static void CreateHd2dAutonomousP2SnowAccumulationProbe(Transform root, Material material)
        {
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2SnowProbeRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var probeRoot = new GameObject(Hd2dAutonomousP2SnowProbeRootName);
            probeRoot.transform.SetParent(root, false);
            probeRoot.transform.localPosition = GetHd2dAutonomousP2SnowWeatherProbeLocalPosition();
            probeRoot.transform.localRotation = Quaternion.Euler(0f, -24f, 0f);
            probeRoot.transform.localScale = Vector3.one;
            probeRoot.layer = CurrentSpaceRenderLayer;

            CreateHd2dAutonomousP2SnowProbeCube(probeRoot.transform, "GroundTop", new Vector3(-0.48f, 0.04f, 0f), new Vector3(1.25f, 0.08f, 1.05f), Quaternion.identity, material);
            CreateHd2dAutonomousP2SnowProbeCube(probeRoot.transform, "VerticalWall", new Vector3(0.44f, 0.42f, 0.12f), new Vector3(0.16f, 0.84f, 1.12f), Quaternion.identity, material);
            CreateHd2dAutonomousP2SnowProbeCube(probeRoot.transform, "SlopedRoof", new Vector3(0.03f, 0.74f, -0.56f), new Vector3(1.22f, 0.08f, 0.72f), Quaternion.Euler(0f, 0f, -23f), material);

            var landmark = probeRoot.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", "current.central_plaza.hd2d_p2_72.snow_accumulation_top_side_probe");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);
        }

        private static void CreateHd2dAutonomousP2SnowProbeCube(Transform parent, string suffix, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"{Hd2dAutonomousP2SnowProbeRootName}_{suffix}";
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

        private static Vector3 GetHd2dAutonomousP2SnowWeatherProbeLocalPosition()
        {
            return CentralPlazaVsCenter + new Vector3(-1.35f, 0.10f, 2.15f);
        }

        private static void CaptureHd2dAutonomousP2SnowWeatherShot(
            FastVsHd2dSnowWeatherState snow,
            FastVsHd2dGradientSkyDriver skyDriver,
            float amount,
            bool particlesEnabled,
            float simulateSeconds,
            Camera camera,
            string outputDirectory,
            string fileName)
        {
            skyDriver.ApplyReviewSunDirectionForReview(58f, 172f, 0.22f);
            snow.ApplySnowAmountForReview(amount, particlesEnabled, simulateSeconds);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
        }

        private static void WriteHd2dAutonomousP2SnowWeatherReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dSnowWeatherState snow,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics baselineVsSnow,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics driftDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics snowVsStronger,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics resetDiff)
        {
            var profile = snow.ProfileForReview;
            var lines = new List<string>
            {
                "# P2-72 Stylized Snow Weather State Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative snow-weather state data. Runtime default is snow amount 0; review captures explicitly enable conservative and stronger snow states.",
                "- Recommendation: " + profile.RecommendationForReview,
                "- Technical note: VFX Graph turbulence is deferred for Tom; this pass uses a ParticleSystem fallback with wind-biased velocity/noise and a SurfaceRampLit global top-normal accumulation term.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2SnowWeatherProfilePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalSnowWeatherApprovedForReview)} |",
                $"| Default / conservative / stronger snow amount | {profile.DefaultSnowAmountForReview:0.###} / {profile.ConservativeSnowAmountForReview:0.###} / {profile.StrongerSnowAmountForReview:0.###} |",
                $"| Snow color / top power / noise | {FormatColorForReport(profile.SnowColorForReview)} / {profile.TopNormalPowerForReview:0.###} / scale {profile.AccumulationNoiseScaleForReview:0.###}, strength {profile.AccumulationNoiseStrengthForReview:0.###} |",
                $"| Wind drift / fall / turbulence | dir ({profile.WindDirectionForReview.x:0.###},{profile.WindDirectionForReview.z:0.###}), drift {profile.WindDriftSpeedForReview:0.###}, fall {profile.FallSpeedForReview:0.###}, turbulence {profile.TurbulenceForReview:0.###} |",
                $"| Flake fallback | ParticleSystem={FormatBool(profile.ParticleSystemFallbackForReview)}, VFX Graph deferred={FormatBool(profile.VfxGraphDeferredForTomForReview)}, max {profile.MaxParticlesForReview}, emission {profile.FlakeEmissionRateForReview:0.###}/s |",
                $"| Last enabled / live particles at report time | {snow.LastEnabledParticleCountForReview} / {snow.LiveParticleCountForReview} |",
                $"| Review proxy flakes visible at last enabled state | {snow.LastEnabledReviewFlakeProxyVisibleCountForReview} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                baselineVsSnow.ToReportRow("snow amount 0 vs conservative snow"),
                driftDiff.ToReportRow("conservative snow frame A vs frame B wind drift"),
                snowVsStronger.ToReportRow("conservative snow vs stronger Tom option"),
                resetDiff.ToReportRow("snow amount 0 baseline vs reset proof"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|"
            };

            var purposes = new[]
            {
                "_SnowAmount=0 baseline with clear sky/state reset.",
                "Conservative snow amount: drifting flakes plus top-face accumulation on the probe.",
                "Same conservative snow state advanced in time to show wind drift rather than straight-down static flakes.",
                "Stronger Tom-facing option for snow amount/density comparison.",
                "_SnowAmount=0 reset proof after enabled captures."
            };

            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[i]);
                lines.Add($"| `{screenshotFiles[i]}` | {purposes[Mathf.Min(i, purposes.Length - 1)]} |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "stylized_snow_weather_state_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static void SetHd2dAutonomousP2SnowLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetHd2dAutonomousP2SnowLayerRecursively(child.gameObject, layer);
            }
        }
    }
}
