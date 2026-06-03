using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP1DuskNightFireflyRootName = "FastVS_HD2D_P1_DuskNightFireflies";
        private const string Hd2dAutonomousP1DuskNightFireflySystemName = "FastVS_HD2D_P1_DuskNightFirefly_CPUShuriken";
        private const string Hd2dAutonomousP1DuskNightFireflyProfilePath = "Assets/Settings/FastVS_HD2D_P1_DuskNightFireflyProfile.asset";
        private const string Hd2dAutonomousP1DuskNightFireflyMaterialId = "hd2d_p1_dusk_night_firefly_glow";
        private const string Hd2dAutonomousP1DuskNightFireflyTextureId = "hd2d_p1_dusk_night_firefly_glow";

        public static void CaptureHd2dAutonomousP1Item46DuskNightFirefliesBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            var fireflyRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1DuskNightFireflyRootName);
            var fireflyLayer = fireflyRoot != null ? fireflyRoot.GetComponent<FastVsHd2dDuskNightFireflyLayer>() : null;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || fireflyLayer == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P1-46 dusk/night firefly capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1DuskNightFireflies();
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dDuskNightFireflyProfile>(Hd2dAutonomousP1DuskNightFireflyProfilePath);
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("dusk_night_fireflies");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_noon_fireflies_gated_off.png",
                "02_night_fireflies_bloom_on.png",
                "03_blink_drift_frame_a.png",
                "04_blink_drift_frame_b.png",
                "05_evening_fireflies_fade_in.png"
            };
            var shotRows = new List<string>();
            var captureMultiplier = profile != null ? profile.ReviewCaptureEmissionMultiplierForReview : 1.45f;
            var warmupSeconds = profile != null ? profile.LifetimeForReview : 6.8f;
            try
            {
                guide.SetMovementFrozen(true);

                CaptureHd2dAutonomousP1DuskNightFireflyReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    fireflyLayer,
                    SunPreset.Noon,
                    1f,
                    CentralPlazaVsCenter + new Vector3(0.1f, 0.02f, 3.05f),
                    new Vector3(0.18f, 5.4f, -7.0f),
                    new Vector3(0.00f, 1.28f, 0.18f),
                    35f,
                    0f,
                    true,
                    outputDirectory,
                    screenshotFiles[0],
                    "noon gate off: same bounded volume emits zero/near-zero",
                    shotRows);

                CaptureHd2dAutonomousP1DuskNightFireflyReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    fireflyLayer,
                    SunPreset.Night,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(0.1f, 0.02f, 3.05f),
                    new Vector3(0.18f, 5.4f, -7.0f),
                    new Vector3(0.00f, 1.28f, 0.18f),
                    35f,
                    warmupSeconds,
                    true,
                    outputDirectory,
                    screenshotFiles[1],
                    "night gate on: HDR glow sprites feed bloom",
                    shotRows);

                CaptureHd2dAutonomousP1DuskNightFireflyReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    fireflyLayer,
                    SunPreset.Night,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(0.1f, 0.02f, 3.05f),
                    new Vector3(0.18f, 5.4f, -7.0f),
                    new Vector3(0.00f, 1.28f, 0.18f),
                    35f,
                    0f,
                    false,
                    outputDirectory,
                    screenshotFiles[2],
                    "blink/drift frame A, no restart after night prewarm",
                    shotRows);

                CaptureHd2dAutonomousP1DuskNightFireflyReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    fireflyLayer,
                    SunPreset.Night,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(0.1f, 0.02f, 3.05f),
                    new Vector3(0.18f, 5.4f, -7.0f),
                    new Vector3(0.00f, 1.28f, 0.18f),
                    35f,
                    0.82f,
                    false,
                    outputDirectory,
                    screenshotFiles[3],
                    "blink/drift frame B after 0.82 seconds",
                    shotRows);

                CaptureHd2dAutonomousP1DuskNightFireflyReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    fireflyLayer,
                    SunPreset.Evening,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(0.1f, 0.02f, 3.05f),
                    new Vector3(0.18f, 5.4f, -7.0f),
                    new Vector3(0.00f, 1.28f, 0.18f),
                    35f,
                    warmupSeconds,
                    true,
                    outputDirectory,
                    screenshotFiles[4],
                    "evening gate partially faded in",
                    shotRows);

                var dayNightMetrics = ValidateHd2dAutonomousP1DuskNightFireflyReviewPairDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1], "noon-gated-off-vs-night-on");
                var blinkDriftMetrics = ValidateHd2dAutonomousP1DuskNightFireflyReviewPairDiff(outputDirectory, screenshotFiles[2], screenshotFiles[3], "blink-drift-a-vs-b");
                WriteHd2dAutonomousP1DuskNightFireflyReviewReport(outputDirectory, screenshotFiles, shotRows, dayNightMetrics, blinkDriftMetrics);
            }
            finally
            {
                fireflyLayer.ClearReviewPresetForReview();
                if (profile != null)
                {
                    fireflyLayer.SimulateForReview(profile.LifetimeForReview, true);
                }

                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
            }

            Debug.Log($"Fast VS autonomous P1-46 dusk/night fireflies review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1DuskNightFireflies(Transform currentRoot)
        {
            if (currentRoot == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP1DuskNightFireflyProfileAsset();
            var material = EnsureHd2dAutonomousP1DuskNightFireflyMaterial(profile);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1DuskNightFireflyRootName);
            if (root == null)
            {
                root = new GameObject(Hd2dAutonomousP1DuskNightFireflyRootName, typeof(FastVsHd2dDuskNightFireflyLayer));
            }
            else if (root.GetComponent<FastVsHd2dDuskNightFireflyLayer>() == null)
            {
                root.AddComponent<FastVsHd2dDuskNightFireflyLayer>();
            }

            root.transform.SetParent(currentRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.SetActive(true);

            var systemTransform = root.transform.Find(Hd2dAutonomousP1DuskNightFireflySystemName);
            var systemObject = systemTransform != null ? systemTransform.gameObject : null;
            if (systemObject == null)
            {
                systemObject = new GameObject(Hd2dAutonomousP1DuskNightFireflySystemName, typeof(ParticleSystem), typeof(ParticleSystemRenderer));
                systemObject.transform.SetParent(root.transform, false);
            }

            systemObject.transform.localPosition = profile.CentralPlazaLocalCenterForReview;
            systemObject.transform.localRotation = Quaternion.identity;
            systemObject.transform.localScale = Vector3.one;
            systemObject.layer = CurrentSpaceRenderLayer;
            systemObject.SetActive(true);

            var system = systemObject.GetComponent<ParticleSystem>();
            if (system == null)
            {
                system = systemObject.AddComponent<ParticleSystem>();
            }

            var renderer = systemObject.GetComponent<ParticleSystemRenderer>();
            if (renderer == null)
            {
                renderer = systemObject.AddComponent<ParticleSystemRenderer>();
            }

            ConfigureHd2dAutonomousP1DuskNightFireflyParticleSystem(system, renderer, profile, material);
            var heroLights = CreateHd2dAutonomousP1DuskNightFireflyHeroLights(root.transform, profile);
            var layer = root.GetComponent<FastVsHd2dDuskNightFireflyLayer>();
            var sunDriverRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunDriverRoot != null ? sunDriverRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            layer.ConfigureForReview(
                profile,
                sunDriver,
                new[] { system },
                new[] { renderer },
                heroLights,
                true,
                profile.ConservativeReviewModeForReview,
                profile.RequiresTomArtApprovalForReview);
            layer.SimulateForReview(profile.LifetimeForReview, true);

            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(systemObject);
        }

        private static void ValidateHd2dAutonomousP1DuskNightFireflies()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dDuskNightFireflyProfile>(Hd2dAutonomousP1DuskNightFireflyProfilePath);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1DuskNightFireflyRootName);
            if (profile == null || root == null)
            {
                throw new InvalidOperationException("House slice validation failed: P1-46 needs a dusk/night firefly profile and scene root.");
            }

            var layer = root.GetComponent<FastVsHd2dDuskNightFireflyLayer>();
            var systems = root.GetComponentsInChildren<ParticleSystem>(true);
            var renderers = root.GetComponentsInChildren<ParticleSystemRenderer>(true);
            var pointLights = root.GetComponentsInChildren<Light>(true).WhereLightType(LightType.Point);
            var material = AssetDatabase.LoadAssetAtPath<Material>(GetHd2dAutonomousP1DuskNightFireflyMaterialPath());
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetHd2dAutonomousP1DuskNightFireflyTexturePath());
            if (layer == null ||
                systems.Length != 1 ||
                renderers.Length != 1 ||
                pointLights.Length < 1 ||
                pointLights.Length > 3 ||
                material == null ||
                texture == null ||
                !layer.TodGatedForReview ||
                !profile.ConservativeReviewModeForReview ||
                !profile.RequiresTomArtApprovalForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P1-46 needs one ToD-gated firefly Shuriken system, additive material, glow texture, and 1-3 hero lights.");
            }

            var system = systems[0];
            var main = system.main;
            var shape = system.shape;
            var colorOverLifetime = system.colorOverLifetime;
            var noise = system.noise;
            if (main.maxParticles < 20 ||
                main.maxParticles > 60 ||
                main.simulationSpace != ParticleSystemSimulationSpace.World ||
                !shape.enabled ||
                shape.shapeType != ParticleSystemShapeType.Box ||
                shape.scale.x < 6f ||
                shape.scale.z < 4f ||
                !colorOverLifetime.enabled ||
                !noise.enabled ||
                profile.NoonGateForReview > 0.01f ||
                profile.MorningGateForReview > 0.01f ||
                profile.EveningGateForReview <= 0.1f ||
                profile.NightGateForReview < profile.EveningGateForReview ||
                profile.HdrIntensityForReview <= 1f ||
                profile.BlinkHighAlphaForReview <= profile.BlinkLowAlphaForReview ||
                profile.HeroPointLightCountForReview != pointLights.Length)
            {
                throw new InvalidOperationException("House slice validation failed: P1-46 fireflies must be world-simulated, blinking, noisy, night-weighted, HDR, and daytime-gated.");
            }

            var editorSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.P1DuskNightFireflies.cs");
            var runtimeProfilePath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dDuskNightFireflyProfile.cs");
            var runtimeLayerPath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dDuskNightFireflyLayer.cs");
            var mainSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.cs");
            foreach (var token in new[]
            {
                "CaptureHd2dAutonomousP1Item46DuskNightFirefliesBatch",
                "dusk_night_fireflies",
                "SunPreset.Night",
                "ValidateHd2dAutonomousP1DuskNightFireflies"
            })
            {
                ValidateSourceToken(File.ReadAllText(editorSourcePath), token, editorSourcePath);
            }

            ValidateSourceToken(File.ReadAllText(runtimeProfilePath), "NightGateForReview", runtimeProfilePath);
            ValidateSourceToken(File.ReadAllText(runtimeLayerPath), "ResolveGateForReview", runtimeLayerPath);
            ValidateSourceToken(File.ReadAllText(mainSourcePath), "CreateHd2dAutonomousP1DuskNightFireflies", mainSourcePath);
            ValidateSourceToken(File.ReadAllText(mainSourcePath), "ValidateHd2dAutonomousP1DuskNightFireflies", mainSourcePath);
        }

        private static FastVsHd2dDuskNightFireflyProfile EnsureHd2dAutonomousP1DuskNightFireflyProfileAsset()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dDuskNightFireflyProfile>(Hd2dAutonomousP1DuskNightFireflyProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dDuskNightFireflyProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP1DuskNightFireflyProfilePath);
            }

            profile.ConfigureForReview(
                44,
                8f,
                6.8f,
                7.6f,
                0f,
                0f,
                0.62f,
                1f,
                0.020f,
                0.075f,
                0.040f,
                0.092f,
                CentralPlazaVsCenter + new Vector3(0.0f, 1.42f, 2.90f),
                new Vector3(10.0f, 2.8f, 5.8f),
                0.11f,
                0.22f,
                0.28f,
                new Color(1.45f, 1.18f, 0.44f, 1f),
                new Color(2.35f, 1.72f, 0.62f, 1f),
                2.25f,
                1.45f,
                0.10f,
                0.82f,
                2,
                0.54f,
                2.15f,
                true,
                true,
                "Procedural CC0-safe glow sprites and CPU Shuriken fireflies; Tom should tune final magical density, blink timing, color, bloom, and placement.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Material EnsureHd2dAutonomousP1DuskNightFireflyMaterial(FastVsHd2dDuskNightFireflyProfile profile)
        {
            EnsureFolder(MaterialDirectory);
            var material = AssetDatabase.LoadAssetAtPath<Material>(GetHd2dAutonomousP1DuskNightFireflyMaterialPath());
            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Unlit");
            }

            if (shader == null)
            {
                throw new InvalidOperationException("P1-46 firefly particle shader not found.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, GetHd2dAutonomousP1DuskNightFireflyMaterialPath());
            }

            material.shader = shader;
            ConfigureTransparentParticleMaterial(material, 3018);
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)BlendMode.One);
            material.SetInt("_ZWrite", 0);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = 3018;
            AssignMaterialTexture(material, EnsureHd2dAutonomousP1DuskNightFireflyGlowTexture(), Vector2.one);
            var hdrColor = profile.HdrCoreColorForReview * profile.HdrIntensityForReview;
            hdrColor.a = 1f;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", hdrColor);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", hdrColor);
            }

            ApplyMaterialRole(material, Hd2dAutonomousP1DuskNightFireflyMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP1DuskNightFireflyGlowTexture()
        {
            EnsureFolder(TextureDirectory);
            var path = GetHd2dAutonomousP1DuskNightFireflyTexturePath();
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                texture = new Texture2D(64, 64, TextureFormat.RGBA32, false, true)
                {
                    name = "FastVS_House_" + Hd2dAutonomousP1DuskNightFireflyTextureId,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear
                };
                AssetDatabase.CreateAsset(texture, path);
            }

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var u = (x + 0.5f) / texture.width * 2f - 1f;
                    var v = (y + 0.5f) / texture.height * 2f - 1f;
                    var radius = Mathf.Sqrt((u * u) + (v * v));
                    var core = Mathf.Clamp01(1f - radius * 5.6f);
                    var halo = Mathf.Clamp01(1f - radius * 1.34f);
                    var outer = Mathf.Clamp01(1f - radius * 0.78f);
                    var alpha = Mathf.Clamp01((core * 0.96f) + (halo * halo * 0.40f) + (outer * outer * 0.18f));
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                }
            }

            texture.Apply(false, false);
            EditorUtility.SetDirty(texture);
            AssetDatabase.SaveAssets();
            return texture;
        }

        private static void ConfigureHd2dAutonomousP1DuskNightFireflyParticleSystem(
            ParticleSystem system,
            ParticleSystemRenderer renderer,
            FastVsHd2dDuskNightFireflyProfile profile,
            Material material)
        {
            var main = system.main;
            main.loop = true;
            main.prewarm = true;
            main.playOnAwake = true;
            main.duration = profile.DurationForReview;
            main.startLifetime = new ParticleSystem.MinMaxCurve(profile.LifetimeForReview * 0.78f, profile.LifetimeForReview);
            main.startSpeed = new ParticleSystem.MinMaxCurve(profile.StartSpeedMinForReview, profile.StartSpeedMaxForReview);
            main.startSize = new ParticleSystem.MinMaxCurve(profile.StartSizeMinForReview, profile.StartSizeMaxForReview);
            main.startColor = new ParticleSystem.MinMaxGradient(profile.HdrCoreColorForReview);
            main.maxParticles = profile.MaxParticlesForReview;
            main.gravityModifier = 0f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = false;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);

            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = profile.BoundedVolumeSizeForReview;
            shape.position = Vector3.zero;
            shape.rotation = Vector3.zero;
            shape.randomDirectionAmount = profile.RandomDirectionAmountForReview;

            var noise = system.noise;
            noise.enabled = true;
            noise.strength = new ParticleSystem.MinMaxCurve(profile.NoiseStrengthForReview);
            noise.frequency = profile.NoiseFrequencyForReview;
            noise.damping = true;
            noise.quality = ParticleSystemNoiseQuality.Medium;

            var colorOverLifetime = system.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(profile.HdrCoreColorForReview, 0f),
                    new GradientColorKey(profile.HdrBlinkColorForReview, 0.22f),
                    new GradientColorKey(profile.HdrCoreColorForReview, 0.52f),
                    new GradientColorKey(profile.HdrBlinkColorForReview, 0.78f),
                    new GradientColorKey(profile.HdrCoreColorForReview, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(profile.BlinkLowAlphaForReview, 0f),
                    new GradientAlphaKey(profile.BlinkHighAlphaForReview, 0.22f),
                    new GradientAlphaKey(profile.BlinkLowAlphaForReview, 0.52f),
                    new GradientAlphaKey(profile.BlinkHighAlphaForReview, 0.78f),
                    new GradientAlphaKey(profile.BlinkLowAlphaForReview, 1f)
                });
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.sortingOrder = 3;
            EditorUtility.SetDirty(system);
            EditorUtility.SetDirty(renderer);
        }

        private static Light[] CreateHd2dAutonomousP1DuskNightFireflyHeroLights(Transform root, FastVsHd2dDuskNightFireflyProfile profile)
        {
            var lights = new List<Light>();
            var offsets = new[]
            {
                new Vector3(-2.8f, 0.24f, -1.0f),
                new Vector3(2.2f, -0.18f, 1.1f),
                new Vector3(0.4f, 0.32f, 2.0f)
            };

            for (var i = 0; i < profile.HeroPointLightCountForReview; i++)
            {
                var name = $"FastVS_HD2D_P1_DuskNightFirefly_HeroLight_{i + 1:00}";
                var child = root.Find(name);
                var lightObject = child != null ? child.gameObject : new GameObject(name, typeof(Light));
                lightObject.transform.SetParent(root, false);
                lightObject.transform.localPosition = profile.CentralPlazaLocalCenterForReview + offsets[i];
                lightObject.transform.localRotation = Quaternion.identity;
                var light = lightObject.GetComponent<Light>();
                if (light == null)
                {
                    light = lightObject.AddComponent<Light>();
                }

                light.type = LightType.Point;
                light.color = new Color(1.00f, 0.82f, 0.38f, 1f);
                light.intensity = 0f;
                light.range = profile.HeroPointLightRangeForReview;
                light.shadows = LightShadows.None;
                light.renderMode = LightRenderMode.ForcePixel;
                light.enabled = false;
                lights.Add(light);
                EditorUtility.SetDirty(lightObject);
            }

            return lights.ToArray();
        }

        private static void CaptureHd2dAutonomousP1DuskNightFireflyReviewShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            AnemoraSunCycleDriver sunDriver,
            FastVsHd2dDuskNightFireflyLayer fireflyLayer,
            SunPreset preset,
            float emissionMultiplier,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            float simulateSeconds,
            bool restart,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(anchorLocalPosition + new Vector3(0f, 0.02f, -1.0f));
            guide.ApplyActiveTimeIsolationForReview();
            sunDriver.ApplyPreset(preset, true);
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            fireflyLayer.SetReviewPresetForReview(preset, emissionMultiplier);
            fireflyLayer.SimulateForReview(Mathf.Max(0f, simulateSeconds), restart);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {preset} | {FormatVector3ForReport(anchorLocalPosition)} | {FormatVector3ForReport(cameraOffset)} | {fieldOfView:0.#} | {emissionMultiplier:0.##} |");
        }

        private static void WriteHd2dAutonomousP1DuskNightFireflyReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            Hd2dAutonomousP1DuskNightFireflyDiffMetrics dayNightMetrics,
            Hd2dAutonomousP1DuskNightFireflyDiffMetrics blinkDriftMetrics)
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dDuskNightFireflyProfile>(Hd2dAutonomousP1DuskNightFireflyProfilePath);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1DuskNightFireflyRootName);
            var layer = root != null ? root.GetComponent<FastVsHd2dDuskNightFireflyLayer>() : null;
            var lines = new List<string>
            {
                "# P1-46 Dusk/Night Fireflies and Glowing Pollen Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative ToD-gated emissive firefly/glowing-pollen baseline using CPU Shuriken and generated CC0-safe glow sprites.",
                "- Recommendation: keep the ToD gate, blink/drift modules, HDR additive material, and 1-3 hero light hook; Tom should tune final magical density, blink timing, color, bloom response, and placement.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP1DuskNightFireflyProfilePath}` |",
                $"| Root | `{Hd2dAutonomousP1DuskNightFireflyRootName}` |",
                $"| Particle systems / max particles | {layer?.ParticleSystemCountForReview ?? 0} / {layer?.TotalMaxParticlesForReview ?? 0} |",
                $"| Night emission / capture multiplier | {profile?.NightEmissionRateForReview ?? 0f:0.###} / {profile?.ReviewCaptureEmissionMultiplierForReview ?? 0f:0.###} |",
                $"| Gates morning/noon/evening/night | {profile?.MorningGateForReview ?? 0f:0.###} / {profile?.NoonGateForReview ?? 0f:0.###} / {profile?.EveningGateForReview ?? 0f:0.###} / {profile?.NightGateForReview ?? 0f:0.###} |",
                $"| HDR intensity | {profile?.HdrIntensityForReview ?? 0f:0.###} |",
                $"| Blink low/high alpha | {profile?.BlinkLowAlphaForReview ?? 0f:0.###} / {profile?.BlinkHighAlphaForReview ?? 0f:0.###} |",
                $"| Noise strength / frequency | {profile?.NoiseStrengthForReview ?? 0f:0.###} / {profile?.NoiseFrequencyForReview ?? 0f:0.###} |",
                $"| Hero point lights / intensity / range | {layer?.HeroPointLightCountForReview ?? 0} / {profile?.HeroPointLightIntensityForReview ?? 0f:0.###} / {profile?.HeroPointLightRangeForReview ?? 0f:0.###} |",
                $"| ToD gated / blink gradient / noise drift | {FormatBool(layer != null && layer.TodGatedForReview)} / {FormatBool(layer != null && layer.ColorOverLifetimeBlinkForReview)} / {FormatBool(layer != null && layer.NoiseDriftForReview)} |",
                $"| Requires TOM art approval | {FormatBool(profile != null && profile.RequiresTomArtApprovalForReview)} |",
                $"| Source note | {profile?.SourceNoteForReview ?? "missing"} |",
                string.Empty,
                "| Comparison | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                dayNightMetrics.ToReportRow("noon gated off vs night on"),
                blinkDriftMetrics.ToReportRow("blink/drift frame A vs B"),
                string.Empty,
                "| Screenshot | Label | Preset | Anchor | Offset | FOV | Emission x |",
                "|---|---|---|---|---|---:|---:|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Daytime/noon gate-off control |",
                $"| `{screenshotFiles[1]}` | Night gate-on HDR/bloom check |",
                $"| `{screenshotFiles[2]}` | Blink/drift frame A |",
                $"| `{screenshotFiles[3]}` | Blink/drift frame B after 0.82 seconds |",
                $"| `{screenshotFiles[4]}` | Evening partial fade-in check |"
            });

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllText(Path.Combine(outputDirectory, "dusk_night_fireflies_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP1DuskNightFireflyDiffMetrics ValidateHd2dAutonomousP1DuskNightFireflyReviewPairDiff(string outputDirectory, string firstFile, string secondFile, string label)
        {
            var metrics = MeasureHd2dAutonomousP1DuskNightFireflyDiff(Path.Combine(outputDirectory, firstFile), Path.Combine(outputDirectory, secondFile), 4);
            if (metrics.SampleCount <= 0 || metrics.ChangedPixels <= 0)
            {
                throw new InvalidOperationException($"Fast VS autonomous P1-46 firefly capture failed: {label} images have no measurable difference.");
            }

            return metrics;
        }

        private static Hd2dAutonomousP1DuskNightFireflyDiffMetrics MeasureHd2dAutonomousP1DuskNightFireflyDiff(string firstPath, string secondPath, int threshold)
        {
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP1DuskNightFireflyDiffMetrics(0, 0, 0f, 0f);
                }

                var firstPixels = firstTexture.GetPixels32();
                var secondPixels = secondTexture.GetPixels32();
                var sampleCount = Mathf.Min(firstPixels.Length, secondPixels.Length);
                var changedPixels = 0;
                var totalDelta = 0f;
                for (var i = 0; i < sampleCount; i++)
                {
                    var delta =
                        Mathf.Abs(firstPixels[i].r - secondPixels[i].r) +
                        Mathf.Abs(firstPixels[i].g - secondPixels[i].g) +
                        Mathf.Abs(firstPixels[i].b - secondPixels[i].b);
                    totalDelta += delta / 3f;
                    if (delta > threshold)
                    {
                        changedPixels++;
                    }
                }

                var changedPercent = sampleCount > 0 ? changedPixels * 100f / sampleCount : 0f;
                var meanDelta = sampleCount > 0 ? totalDelta / sampleCount : 0f;
                return new Hd2dAutonomousP1DuskNightFireflyDiffMetrics(sampleCount, changedPixels, changedPercent, meanDelta);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(firstTexture);
                UnityEngine.Object.DestroyImmediate(secondTexture);
            }
        }

        private static string GetHd2dAutonomousP1DuskNightFireflyMaterialPath()
        {
            return MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP1DuskNightFireflyMaterialId + ".mat";
        }

        private static string GetHd2dAutonomousP1DuskNightFireflyTexturePath()
        {
            return TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP1DuskNightFireflyTextureId + ".asset";
        }

        private readonly struct Hd2dAutonomousP1DuskNightFireflyDiffMetrics
        {
            public readonly int SampleCount;
            public readonly int ChangedPixels;
            public readonly float ChangedPercent;
            public readonly float MeanRgbDelta;

            public Hd2dAutonomousP1DuskNightFireflyDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
            {
                SampleCount = sampleCount;
                ChangedPixels = changedPixels;
                ChangedPercent = changedPercent;
                MeanRgbDelta = meanRgbDelta;
            }

            public string ToReportRow(string label)
            {
                return $"| {label} | {SampleCount} | {ChangedPixels} | {ChangedPercent:0.###} | {MeanRgbDelta:0.###} |";
            }
        }
    }

    internal static class Hd2dAutonomousP1DuskNightFireflyLightExtensions
    {
        public static Light[] WhereLightType(this Light[] lights, LightType lightType)
        {
            var filtered = new List<Light>();
            if (lights == null)
            {
                return filtered.ToArray();
            }

            for (var i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null && lights[i].type == lightType)
                {
                    filtered.Add(lights[i]);
                }
            }

            return filtered.ToArray();
        }
    }
}
