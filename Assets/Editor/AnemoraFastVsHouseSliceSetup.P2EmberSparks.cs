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
        private const string Hd2dAutonomousP2EmberSparkProfilePath = "Assets/Settings/FastVS_HD2D_P2_EmberSparkProfile.asset";
        private const string Hd2dAutonomousP2EmberSparkProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dEmberSparkProfile.cs";
        private const string Hd2dAutonomousP2EmberSparkEmitterRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dEmberSparkEmitter.cs";
        private const string Hd2dAutonomousP2EmberSparkLibraryRootName = "Current_Library_P2_68_EmberSparkEmitters";
        private const string Hd2dAutonomousP2EmberSparkPlazaRootName = "Current_CentralPlaza_P2_68_EmberSparkEmitters";
        private const string Hd2dAutonomousP2EmberSparkExteriorRootName = "Current_Exterior_P2_68_EmberSparkEmitters";
        private const string Hd2dAutonomousP2EmberSparkLibraryTorchName = "P2_68_LibraryTorch_EmberSparkEmitter";
        private const string Hd2dAutonomousP2EmberSparkCookfireName = "P2_68_Cookfire_EmberSparkEmitter";
        private const string Hd2dAutonomousP2EmberSparkPorchTorchName = "P2_68_PorchTorch_EmberSparkEmitter";
        private const string Hd2dAutonomousP2EmberSparkMaterialId = "hd2d_p2_ember_spark_particle";
        private const string Hd2dAutonomousP2EmberSparkTextureId = "hd2d_p2_ember_spark_particle_glow";
        private const string Hd2dAutonomousP2EmberSparkMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2EmberSparkMaterialId + ".mat";
        private const string Hd2dAutonomousP2EmberSparkTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP2EmberSparkTextureId + ".asset";

        public static void CaptureHd2dAutonomousP2Item68EmberSparksBatch()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2EmberSparkLibraryRootName) == null ||
                FindSceneObjectIncludingInactive(Hd2dAutonomousP2EmberSparkPlazaRootName) == null ||
                FindSceneObjectIncludingInactive(Hd2dAutonomousP2EmberSparkExteriorRootName) == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-68 ember/spark capture failed: review roots are missing. Run BuildAndValidateBatch before capture.");
            }

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            var profile = EnsureHd2dAutonomousP2EmberSparkProfile();
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || profile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-68 ember/spark capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2EmberSparkEmitters();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("embers_sparks_fires_forges_torches");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_library_torch_embers_disabled_control.png",
                "02_library_torch_embers_conservative_frame_a.png",
                "03_library_torch_embers_conservative_frame_b.png",
                "04_central_plaza_cookfire_embers_rise_fade.png",
                "05_stronger_density_option_for_tom.png",
                "06_after_lifetime_fade_height_proof.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                controller.ClosePortal();
                SetHd2dAutonomousP2FootstepFxReviewSurfacesVisible(false);
                HideHd2dAutonomousP2WaterReviewSetsForFogCapture();
                SetHd2dAutonomousP2LocalVolumetricFogVisible(false);
                SetHd2dAutonomousP2SmokeSteamVisible(false);

                SetHd2dAutonomousP2EmberSparkVisible(false);
                SetHd2dAutonomousP2EmberSparkAllMultipliers(1f, 1f);
                CaptureHd2dAutonomousP2EmberSparkLibraryShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[0],
                    "control: P2-68 library torch embers disabled in night lighting",
                    0.10f,
                    true,
                    shotRows);

                SetHd2dAutonomousP2EmberSparkVisible(true);
                SetHd2dAutonomousP2EmberSparkAllMultipliers(1f, 1f);
                CaptureHd2dAutonomousP2EmberSparkLibraryShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[1],
                    "conservative frame A: HDR orange embers rising from the warm-anchor torch",
                    1.18f,
                    true,
                    shotRows);

                CaptureHd2dAutonomousP2EmberSparkLibraryShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[2],
                    "conservative frame B: same torch after flicker and upward drift",
                    0.72f,
                    false,
                    shotRows);

                CaptureHd2dAutonomousP2EmberSparkPlazaShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[3],
                    "central plaza cookfire: short-lived embers rise through the smoke source and fade",
                    1.35f,
                    true,
                    shotRows);

                SetHd2dAutonomousP2EmberSparkAllMultipliers(profile.StrongerOptionMultiplierForReview, profile.StrongerOptionMultiplierForReview);
                CaptureHd2dAutonomousP2EmberSparkLibraryShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[4],
                    "stronger density and light-flicker option for Tom only",
                    1.18f,
                    true,
                    shotRows);

                SetHd2dAutonomousP2EmberSparkAllMultipliers(0f, 1f);
                CaptureHd2dAutonomousP2EmberSparkLibraryShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[5],
                    "fade proof: emission held at zero after lifetime, no high-altitude hard cutoff remains",
                    profile.EmberLifetimeForReview + 0.32f,
                    false,
                    shotRows);
            }
            finally
            {
                SetHd2dAutonomousP2EmberSparkVisible(true);
                SetHd2dAutonomousP2EmberSparkAllMultipliers(1f, 1f);
                SetHd2dAutonomousP2SmokeSteamVisible(true);
                SetHd2dAutonomousP2LocalVolumetricFogVisible(true);
                RestoreHd2dAutonomousP2WaterReviewSetsAfterFogCapture();
                SetHd2dAutonomousP2FootstepFxReviewSurfacesVisible(true);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                AssetDatabase.SaveAssets();
            }

            var enableDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var flickerDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var strongerDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[4]);
            WriteHd2dAutonomousP2EmberSparkReviewReport(outputDirectory, screenshotFiles, shotRows, profile, enableDiff, flickerDiff, strongerDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-68 ember/spark review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2EmberSparkEmitters(Transform currentLibraryRoot, Transform currentCentralPlazaRoot, Transform currentExteriorRoot)
        {
            var profile = EnsureHd2dAutonomousP2EmberSparkProfile();
            var material = EnsureHd2dAutonomousP2EmberSparkMaterial(profile);
            DestroyHd2dAutonomousP2EmberSparkRoot(Hd2dAutonomousP2EmberSparkLibraryRootName);
            DestroyHd2dAutonomousP2EmberSparkRoot(Hd2dAutonomousP2EmberSparkPlazaRootName);
            DestroyHd2dAutonomousP2EmberSparkRoot(Hd2dAutonomousP2EmberSparkExteriorRootName);

            if (currentLibraryRoot != null)
            {
                var libraryRoot = CreateHd2dAutonomousP2EmberSparkRoot(currentLibraryRoot, Hd2dAutonomousP2EmberSparkLibraryRootName);
                CreateHd2dAutonomousP2EmberSparkEmitter(
                    libraryRoot.transform,
                    Hd2dAutonomousP2EmberSparkLibraryTorchName,
                    FastVsHd2dEmberSparkSourceKind.LibraryTorch,
                    LibraryVsCenter + Stage7LibraryWarmAnchorCenterLocalPosition + new Vector3(0.02f, 0.30f, 0.00f),
                    0.040f,
                    8.0f,
                    true,
                    profile,
                    material);
            }

            if (currentCentralPlazaRoot != null)
            {
                var plazaRoot = CreateHd2dAutonomousP2EmberSparkRoot(currentCentralPlazaRoot, Hd2dAutonomousP2EmberSparkPlazaRootName);
                CreateHd2dAutonomousP2EmberSparkEmitter(
                    plazaRoot.transform,
                    Hd2dAutonomousP2EmberSparkCookfireName,
                    FastVsHd2dEmberSparkSourceKind.Cookfire,
                    CentralPlazaVsCenter + new Vector3(-1.42f, 0.34f, 3.18f),
                    0.070f,
                    11.0f,
                    true,
                    profile,
                    material);
            }

            if (currentExteriorRoot != null)
            {
                var exteriorRoot = CreateHd2dAutonomousP2EmberSparkRoot(currentExteriorRoot, Hd2dAutonomousP2EmberSparkExteriorRootName);
                CreateHd2dAutonomousP2EmberSparkEmitter(
                    exteriorRoot.transform,
                    Hd2dAutonomousP2EmberSparkPorchTorchName,
                    FastVsHd2dEmberSparkSourceKind.PorchTorch,
                    HouseExteriorCenter + new Vector3(-1.14f, 1.05f, -1.92f),
                    0.035f,
                    7.5f,
                    false,
                    profile,
                    material);
            }

            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2EmberSparkEmitters()
        {
            var profile = EnsureHd2dAutonomousP2EmberSparkProfile();
            var material = EnsureHd2dAutonomousP2EmberSparkMaterial(profile);
            if (profile == null ||
                material == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalEmberSparkApprovedForReview ||
                !profile.LoopingShurikenForReview ||
                !profile.AdditiveHdrMaterialForReview ||
                !profile.SoftParticlesRequiredForReview ||
                !profile.UpwardFadeBeforeHeightForReview ||
                !profile.FlickerPointLightsEnabledForReview ||
                !profile.DistanceCullFarEmittersForReview ||
                !profile.ConservativeDataPrepForReview ||
                profile.MaxParticlesPerEmitterForReview > 30 ||
                profile.ResolveExpectedParticleCountForReview(FastVsHd2dEmberSparkSourceKind.Cookfire) > 30 ||
                profile.ExpectedUpperFadeHeightMetersForReview > 1.35f ||
                profile.HdrIntensityForReview <= 1f ||
                profile.StrongerOptionMultiplierForReview <= 1f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-68 needs a conservative non-final HDR additive ember profile with 10-30 particles/source, upward fade, soft particles, and Tom approval left open.");
            }

            var softParticlesEnabled =
                material.IsKeywordEnabled("_SOFTPARTICLES_ON") ||
                (material.HasProperty("_SoftParticlesEnabled") && material.GetFloat("_SoftParticlesEnabled") >= 0.5f);
            var additiveBlendMode = material.HasProperty("_Blend") && Mathf.Abs(material.GetFloat("_Blend") - 1f) < 0.01f;
            var additiveBlendEnabled = additiveBlendMode || !material.HasProperty("_DstBlend") || material.GetInt("_DstBlend") == (int)BlendMode.One;
            var softParticlesFarFade = material.HasProperty("_SoftParticlesFarFadeDistance") ? material.GetFloat("_SoftParticlesFarFadeDistance") : 0.36f;
            var baseColor = material.HasProperty("_BaseColor") ? material.GetColor("_BaseColor") : Color.white;
            if (!softParticlesEnabled ||
                !additiveBlendEnabled ||
                softParticlesFarFade < 0.20f ||
                Mathf.Max(baseColor.r, Mathf.Max(baseColor.g, baseColor.b)) <= 1.05f)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P2-68 ember material must remain HDR additive and soft-particle enabled. " +
                    $"soft={softParticlesEnabled}, blend={(material.HasProperty("_Blend") ? material.GetFloat("_Blend") : -1f):0.###}, dst={(material.HasProperty("_DstBlend") ? material.GetInt("_DstBlend") : -1)}, far={softParticlesFarFade:0.###}, base={FormatColor(baseColor)}.");
            }

            var libraryCount = CountHd2dAutonomousP2EmberSparkEmitters(FastVsHd2dEmberSparkSourceKind.LibraryTorch);
            var cookfireCount = CountHd2dAutonomousP2EmberSparkEmitters(FastVsHd2dEmberSparkSourceKind.Cookfire);
            var porchCount = CountHd2dAutonomousP2EmberSparkEmitters(FastVsHd2dEmberSparkSourceKind.PorchTorch);
            if (libraryCount < 1 || cookfireCount < 1 || porchCount < 1)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-68 requires library torch, cookfire, and porch torch ember emitters. Counts library/cookfire/porch={libraryCount}/{cookfireCount}/{porchCount}.");
            }

            SetHd2dAutonomousP2EmberSparkVisible(true);
            SetHd2dAutonomousP2EmberSparkAllMultipliers(1f, 1f);
            foreach (var emitter in FindHd2dAutonomousP2EmberSparkEmitters())
            {
                ValidateHd2dAutonomousP2EmberSparkEmitter(emitter, profile, material);
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2EmberSparkProfileRuntimePath), "needsTomApproval", Hd2dAutonomousP2EmberSparkProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2EmberSparkProfileRuntimePath), "finalEmberSparkApproved", Hd2dAutonomousP2EmberSparkProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2EmberSparkEmitterRuntimePath), "ResolveFlickeredEmissionRate", Hd2dAutonomousP2EmberSparkEmitterRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2EmberSparkEmitterRuntimePath), "SetReviewRateMultiplierForReview", Hd2dAutonomousP2EmberSparkEmitterRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2EmberSparkEmitters", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2EmberSparkEmitters", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.P2EmberSparks.cs"), "new GradientAlphaKey(0f, 1f)", "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2EmberSparks.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.P2EmberSparks.cs"), "SunPreset.Night", "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2EmberSparks.cs");
        }

        private static FastVsHd2dEmberSparkProfile EnsureHd2dAutonomousP2EmberSparkProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dEmberSparkProfile>(Hd2dAutonomousP2EmberSparkProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dEmberSparkProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2EmberSparkProfilePath);
            }

            profile.ConfigureForReview(
                28,
                13.5f,
                18.0f,
                12.0f,
                1.35f,
                2.25f,
                0.024f,
                0.058f,
                0.58f,
                0.86f,
                0.16f,
                0.105f,
                0.34f,
                0.18f,
                2.35f,
                0.34f,
                34f,
                1.42f,
                new Color(2.15f, 1.14f, 0.34f, 1f),
                new Color(3.15f, 1.68f, 0.48f, 1f),
                new Color(0.44f, 0.075f, 0.018f, 1f),
                new Color(1.00f, 0.58f, 0.24f, 1f),
                2.65f,
                0.11f,
                1.24f,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                false,
                "Keep the conservative HDR additive ember baseline as data prep. Tom should tune final density, bloom intensity, spark size, and point-light flicker after checking it against the approved night grade.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Material EnsureHd2dAutonomousP2EmberSparkMaterial(FastVsHd2dEmberSparkProfile profile)
        {
            EnsureFolder(MaterialDirectory);
            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2EmberSparkMaterialPath);
            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
            {
                throw new InvalidOperationException("P2-68 ember particle shader not found.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP2EmberSparkMaterialPath);
            }

            material.shader = shader;
            ConfigureTransparentParticleMaterial(material, 3046);
            material.SetOverrideTag("RenderType", "Transparent");
            if (material.HasProperty("_Blend"))
            {
                material.SetFloat("_Blend", 1f);
            }

            material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)BlendMode.One);
            material.SetFloat("_ZWrite", 0f);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = 3046;
            AssignMaterialTexture(material, EnsureHd2dAutonomousP2EmberSparkTexture(), Vector2.one);
            var hdrColor = profile.HotCoreColorForReview * profile.HdrIntensityForReview;
            hdrColor.a = 1f;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", hdrColor);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", hdrColor);
            }

            if (material.HasProperty("_SoftParticlesEnabled"))
            {
                material.SetFloat("_SoftParticlesEnabled", 1f);
            }

            if (material.HasProperty("_SoftParticlesNearFadeDistance"))
            {
                material.SetFloat("_SoftParticlesNearFadeDistance", 0f);
            }

            if (material.HasProperty("_SoftParticlesFarFadeDistance"))
            {
                material.SetFloat("_SoftParticlesFarFadeDistance", 0.36f);
            }

            material.EnableKeyword("_SOFTPARTICLES_ON");
            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);
            material.SetShaderPassEnabled("ShadowCaster", false);
            ApplyMaterialRole(material, Hd2dAutonomousP2EmberSparkMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            material.enableInstancing = true;
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP2EmberSparkTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2EmberSparkTextureId,
                64,
                64,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = ((x + 0.5f) / 64f) * 2f - 1f;
                    var v = ((y + 0.5f) / 64f) * 2f - 1f;
                    var coreDistance = Mathf.Sqrt((u * u * 5.8f) + (v * v * 3.4f));
                    var verticalStreak = Mathf.Clamp01(1f - (Mathf.Abs(u) * 6.6f)) * Mathf.Clamp01(1f - Mathf.Abs(v) * 1.25f);
                    var haloDistance = Mathf.Sqrt((u * u * 1.55f) + (v * v * 1.18f));
                    var core = Mathf.Clamp01(1f - coreDistance);
                    var halo = Mathf.Clamp01(1f - haloDistance);
                    var alpha = Mathf.Clamp01((core * 0.96f) + (verticalStreak * 0.42f) + (halo * halo * 0.24f));
                    return new Color(1f, 0.92f, 0.72f, alpha);
                });
        }

        private static GameObject CreateHd2dAutonomousP2EmberSparkRoot(Transform parent, string rootName)
        {
            var root = new GameObject(rootName);
            root.transform.SetParent(parent, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            SetHd2dAutonomousP2EmberSparkLayerRecursively(root, CurrentSpaceRenderLayer);
            return root;
        }

        private static void CreateHd2dAutonomousP2EmberSparkEmitter(
            Transform parent,
            string objectName,
            FastVsHd2dEmberSparkSourceKind sourceKind,
            Vector3 localPosition,
            float radius,
            float coneAngle,
            bool createFlickerLight,
            FastVsHd2dEmberSparkProfile profile,
            Material material)
        {
            var emitterObject = new GameObject(objectName);
            emitterObject.transform.SetParent(parent, false);
            emitterObject.transform.localPosition = localPosition;
            emitterObject.transform.localRotation = Quaternion.identity;
            emitterObject.transform.localScale = Vector3.one;
            emitterObject.layer = CurrentSpaceRenderLayer;

            var system = emitterObject.AddComponent<ParticleSystem>();
            var renderer = emitterObject.GetComponent<ParticleSystemRenderer>();
            if (renderer == null)
            {
                renderer = emitterObject.AddComponent<ParticleSystemRenderer>();
            }

            Light light = null;
            if (createFlickerLight)
            {
                light = emitterObject.AddComponent<Light>();
                light.type = LightType.Point;
                light.color = profile.PointLightColorForReview;
                light.intensity = profile.PointLightIntensityForReview;
                light.range = profile.PointLightRangeForReview;
                light.shadows = LightShadows.None;
                light.cullingMask = 1 << CurrentSpaceRenderLayer;
                light.renderMode = LightRenderMode.ForcePixel;
            }

            ConfigureHd2dAutonomousP2EmberSparkParticleSystem(system, renderer, sourceKind, radius, coneAngle, profile, material);
            var emitter = emitterObject.AddComponent<FastVsHd2dEmberSparkEmitter>();
            emitter.ConfigureForReview(profile, sourceKind, system, renderer, light, createFlickerLight, profile.DistanceCullFarEmittersForReview);
            EditorUtility.SetDirty(emitterObject);
            EditorUtility.SetDirty(system);
            EditorUtility.SetDirty(renderer);
            if (light != null)
            {
                EditorUtility.SetDirty(light);
            }

            EditorUtility.SetDirty(emitter);
        }

        private static void ConfigureHd2dAutonomousP2EmberSparkParticleSystem(
            ParticleSystem system,
            ParticleSystemRenderer renderer,
            FastVsHd2dEmberSparkSourceKind sourceKind,
            float radius,
            float coneAngle,
            FastVsHd2dEmberSparkProfile profile,
            Material material)
        {
            var main = system.main;
            main.loop = true;
            main.prewarm = true;
            main.playOnAwake = true;
            main.duration = profile.SystemDurationForReview;
            main.startLifetime = new ParticleSystem.MinMaxCurve(profile.EmberLifetimeForReview * 0.78f, profile.EmberLifetimeForReview * 1.06f);
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(profile.StartSizeMinForReview, profile.StartSizeMaxForReview);
            main.startRotation = new ParticleSystem.MinMaxCurve(-0.40f, 0.40f);
            main.startColor = new ParticleSystem.MinMaxGradient(Color.white);
            main.maxParticles = profile.MaxParticlesPerEmitterForReview;
            main.gravityModifier = -0.018f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(profile.ResolveEmissionRateForReview(sourceKind));
            emission.SetBursts(Array.Empty<ParticleSystem.Burst>());

            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.radius = radius;
            shape.angle = coneAngle;
            shape.radiusThickness = 0.68f;
            shape.arc = 360f;
            shape.randomDirectionAmount = 0.18f;

            var velocity = system.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            var drift = profile.LateralDriftVelocityForReview;
            velocity.x = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(new Keyframe(0f, -drift), new Keyframe(0.62f, -drift * 0.42f), new Keyframe(1f, -drift * 0.14f)),
                new AnimationCurve(new Keyframe(0f, drift), new Keyframe(0.62f, drift * 0.42f), new Keyframe(1f, drift * 0.14f)));
            velocity.y = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(new Keyframe(0f, profile.RiseVelocityMinForReview), new Keyframe(0.66f, profile.RiseVelocityMinForReview * 0.58f), new Keyframe(1f, profile.RiseVelocityMinForReview * 0.18f)),
                new AnimationCurve(new Keyframe(0f, profile.RiseVelocityMaxForReview), new Keyframe(0.66f, profile.RiseVelocityMaxForReview * 0.58f), new Keyframe(1f, profile.RiseVelocityMaxForReview * 0.18f)));
            velocity.z = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(new Keyframe(0f, -drift * 0.62f), new Keyframe(0.62f, -drift * 0.22f), new Keyframe(1f, -drift * 0.08f)),
                new AnimationCurve(new Keyframe(0f, drift * 0.62f), new Keyframe(0.62f, drift * 0.22f), new Keyframe(1f, drift * 0.08f)));

            var colorOverLifetime = system.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(profile.HotFlickerColorForReview, 0f),
                    new GradientColorKey(profile.HotCoreColorForReview, 0.18f),
                    new GradientColorKey(profile.DarkRedFadeColorForReview, 0.68f),
                    new GradientColorKey(profile.DarkRedFadeColorForReview * 0.20f, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(1f, 0.08f),
                    new GradientAlphaKey(0.78f, 0.28f),
                    new GradientAlphaKey(0.30f, 0.66f),
                    new GradientAlphaKey(0f, 1f)
                });
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

            var size = system.sizeOverLifetime;
            size.enabled = true;
            var sizeCurve = new AnimationCurve(
                new Keyframe(0f, 0.45f),
                new Keyframe(0.12f, 1.0f),
                new Keyframe(0.62f, 0.70f),
                new Keyframe(1f, 0.12f));
            size.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

            var noise = system.noise;
            noise.enabled = true;
            noise.strength = new ParticleSystem.MinMaxCurve(profile.NoiseStrengthForReview);
            noise.frequency = profile.NoiseFrequencyForReview;
            noise.scrollSpeed = 0.24f;
            noise.octaveCount = 2;
            noise.damping = true;
            noise.quality = ParticleSystemNoiseQuality.Medium;

            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.sortingOrder = 7;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.forceRenderingOff = false;
            ForceRendererEnabledForReview(renderer);

            system.Clear(true);
            system.Play(true);
        }

        private static void ValidateHd2dAutonomousP2EmberSparkEmitter(FastVsHd2dEmberSparkEmitter emitter, FastVsHd2dEmberSparkProfile profile, Material expectedMaterial)
        {
            var system = emitter != null ? emitter.ParticleSystemForReview : null;
            var renderer = emitter != null ? emitter.ParticleRendererForReview : null;
            if (emitter == null || system == null || renderer == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-68 ember emitter is missing runtime component, ParticleSystem, or renderer.");
            }

            var main = system.main;
            var emission = system.emission;
            var shape = system.shape;
            var velocity = system.velocityOverLifetime;
            var color = system.colorOverLifetime;
            var size = system.sizeOverLifetime;
            var noise = system.noise;
            var riseVelocity = ReadHd2dAutonomousP2EmberSparkCurveMax(velocity.y);
            var lifetimeMax = ReadHd2dAutonomousP2EmberSparkCurveMax(main.startLifetime);
            var expectedCount = profile.ResolveExpectedParticleCountForReview(emitter.SourceKindForReview);
            if (emitter.ProfileForReview != profile ||
                !emitter.DistanceCullEnabledForReview ||
                emitter.gameObject.layer != CurrentSpaceRenderLayer ||
                !main.loop ||
                !main.playOnAwake ||
                main.simulationSpace != ParticleSystemSimulationSpace.World ||
                main.maxParticles > profile.MaxParticlesPerEmitterForReview ||
                expectedCount < 10 ||
                expectedCount > 30 ||
                lifetimeMax > 1.55f ||
                !emission.enabled ||
                emission.rateOverTime.constant <= 0f ||
                !shape.enabled ||
                shape.shapeType != ParticleSystemShapeType.Cone ||
                !velocity.enabled ||
                riseVelocity <= 0.40f ||
                velocity.x.mode != velocity.y.mode ||
                velocity.y.mode != velocity.z.mode ||
                !color.enabled ||
                !size.enabled ||
                !noise.enabled ||
                renderer.sharedMaterial != expectedMaterial ||
                renderer.forceRenderingOff ||
                renderer.shadowCastingMode != ShadowCastingMode.Off ||
                renderer.receiveShadows)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P2-68 embers must be looping World-space cone Shuriken systems with HDR additive material, upward TwoCurve velocity, flicker/color/size fade, noise drift, and no shadows. " +
                    $"{emitter.name}: kind={emitter.SourceKindForReview}, loop={main.loop}, play={main.playOnAwake}, sim={main.simulationSpace}, max={main.maxParticles}/{profile.MaxParticlesPerEmitterForReview}, expected={expectedCount}, lifetimeMax={lifetimeMax:0.###}, emission={emission.rateOverTime.constant:0.###}, rise={riseVelocity:0.###}, material={(renderer.sharedMaterial != null ? renderer.sharedMaterial.name : "null")}.");
            }

            if (emitter.FlickerLightEnabledForReview && emitter.FlickerPointLightForReview == null)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-68 emitter {emitter.name} has flicker light enabled but no point light.");
            }
        }

        private static void CaptureHd2dAutonomousP2EmberSparkLibraryShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            AnemoraSunCycleDriver sunDriver,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            bool restart,
            ICollection<string> shotRows)
        {
            CaptureHd2dAutonomousP2EmberSparkShot(
                controller,
                visibility,
                guide,
                realtimeRig,
                camera,
                sunDriver,
                FastVsHouseArea.Library,
                LibraryVsCenter + Stage7LibraryWarmAnchorCenterLocalPosition + new Vector3(0.02f, 0.42f, 0.02f),
                Stage7LibraryWarmAnchorCaptureCameraOffset,
                Stage7LibraryWarmAnchorCaptureLookOffset,
                31f,
                outputDirectory,
                fileName,
                label,
                simulateSeconds,
                restart,
                shotRows);
        }

        private static void CaptureHd2dAutonomousP2EmberSparkPlazaShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            AnemoraSunCycleDriver sunDriver,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            bool restart,
            ICollection<string> shotRows)
        {
            CaptureHd2dAutonomousP2EmberSparkShot(
                controller,
                visibility,
                guide,
                realtimeRig,
                camera,
                sunDriver,
                FastVsHouseArea.CentralPlaza,
                CentralPlazaVsCenter + new Vector3(-1.42f, 0.70f, 3.18f),
                new Vector3(1.38f, 1.74f, -4.18f),
                new Vector3(0.00f, 0.18f, 0.08f),
                33f,
                outputDirectory,
                fileName,
                label,
                simulateSeconds,
                restart,
                shotRows);
        }

        private static void CaptureHd2dAutonomousP2EmberSparkShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            AnemoraSunCycleDriver sunDriver,
            FastVsHouseArea activeArea,
            Vector3 anchorLocal,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            bool restart,
            ICollection<string> shotRows)
        {
            visibility.SetActiveAreaForReview(activeArea);
            controller.ForcePlayerCurrentLocalForReview(activeArea == FastVsHouseArea.Library
                ? LibraryVsCenter + Stage7LibraryWarmAnchorCenterLocalPosition + new Vector3(0.12f, 0.02f, -0.82f)
                : CentralPlazaVsCenter + new Vector3(-1.22f, 0.02f, 2.18f));
            controller.ClosePortal();
            guide.ApplyActiveTimeIsolationForReview();
            sunDriver.ApplyPreset(SunPreset.Night, true);
            realtimeRig.ApplyNowForReview();
            SimulateHd2dAutonomousP2EmberSparkEmitters(simulateSeconds, restart);

            var previousMask = camera.cullingMask;
            var previousFov = camera.fieldOfView;
            var previousNear = camera.nearClipPlane;
            var previousFar = camera.farClipPlane;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            try
            {
                camera.cullingMask = currentBit | playerBit;
                camera.orthographic = false;
                camera.fieldOfView = fieldOfView;
                camera.nearClipPlane = 0.03f;
                camera.farClipPlane = 170f;
                PositionCloseReviewCamera(
                    camera,
                    controller.CurrentSpaceRootForReview.TransformPoint(anchorLocal),
                    cameraOffset,
                    lookOffset);
                ApplyStage7BokehFocusForReview(camera);
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                camera.fieldOfView = previousFov;
                camera.nearClipPlane = previousNear;
                camera.farClipPlane = previousFar;
                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | {activeArea} | {simulateSeconds:0.###} | {CountHd2dAutonomousP2EmberSparkLiveParticles()} | {SumHd2dAutonomousP2EmberSparkAppliedRates():0.###} | {SumHd2dAutonomousP2EmberSparkAppliedLights():0.###} |");
        }

        private static void SimulateHd2dAutonomousP2EmberSparkEmitters(float seconds, bool restart)
        {
            foreach (var emitter in FindHd2dAutonomousP2EmberSparkEmitters())
            {
                emitter?.SimulateForReview(seconds, restart);
            }
        }

        private static void SetHd2dAutonomousP2EmberSparkVisible(bool visible)
        {
            foreach (var emitter in FindHd2dAutonomousP2EmberSparkEmitters())
            {
                if (emitter == null)
                {
                    continue;
                }

                emitter.SetReviewVisibleForReview(visible);
                EditorUtility.SetDirty(emitter);
            }
        }

        private static void SetHd2dAutonomousP2EmberSparkAllMultipliers(float rateMultiplier, float lightMultiplier)
        {
            foreach (var emitter in FindHd2dAutonomousP2EmberSparkEmitters())
            {
                if (emitter == null)
                {
                    continue;
                }

                emitter.SetReviewRateMultiplierForReview(rateMultiplier);
                emitter.SetReviewLightMultiplierForReview(lightMultiplier);
                EditorUtility.SetDirty(emitter);
            }
        }

        private static FastVsHd2dEmberSparkEmitter[] FindHd2dAutonomousP2EmberSparkEmitters()
        {
            return UnityEngine.Object.FindObjectsByType<FastVsHd2dEmberSparkEmitter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        private static int CountHd2dAutonomousP2EmberSparkEmitters(FastVsHd2dEmberSparkSourceKind sourceKind)
        {
            var count = 0;
            foreach (var emitter in FindHd2dAutonomousP2EmberSparkEmitters())
            {
                if (emitter != null && emitter.SourceKindForReview == sourceKind)
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountHd2dAutonomousP2EmberSparkLiveParticles()
        {
            var count = 0;
            foreach (var emitter in FindHd2dAutonomousP2EmberSparkEmitters())
            {
                if (emitter != null)
                {
                    count += emitter.LiveParticleCountForReview;
                }
            }

            return count;
        }

        private static float SumHd2dAutonomousP2EmberSparkAppliedRates()
        {
            var total = 0f;
            foreach (var emitter in FindHd2dAutonomousP2EmberSparkEmitters())
            {
                if (emitter != null)
                {
                    total += emitter.AppliedEmissionRateForReview;
                }
            }

            return total;
        }

        private static float SumHd2dAutonomousP2EmberSparkAppliedLights()
        {
            var total = 0f;
            foreach (var emitter in FindHd2dAutonomousP2EmberSparkEmitters())
            {
                if (emitter != null)
                {
                    total += emitter.AppliedLightIntensityForReview;
                }
            }

            return total;
        }

        private static float ReadHd2dAutonomousP2EmberSparkCurveMax(ParticleSystem.MinMaxCurve curve)
        {
            switch (curve.mode)
            {
                case ParticleSystemCurveMode.TwoConstants:
                    return Mathf.Max(curve.constantMin, curve.constantMax);
                case ParticleSystemCurveMode.Constant:
                    return curve.constant;
                case ParticleSystemCurveMode.Curve:
                    return SampleHd2dAutonomousP2EmberSparkCurveMax(curve.curve);
                case ParticleSystemCurveMode.TwoCurves:
                    return Mathf.Max(SampleHd2dAutonomousP2EmberSparkCurveMax(curve.curveMin), SampleHd2dAutonomousP2EmberSparkCurveMax(curve.curveMax));
                default:
                    return curve.constantMax;
            }
        }

        private static float SampleHd2dAutonomousP2EmberSparkCurveMax(AnimationCurve curve)
        {
            if (curve == null || curve.length == 0)
            {
                return 0f;
            }

            var max = float.MinValue;
            for (var i = 0; i <= 8; i++)
            {
                max = Mathf.Max(max, curve.Evaluate(i / 8f));
            }

            return max;
        }

        private static void DestroyHd2dAutonomousP2EmberSparkRoot(string rootName)
        {
            var root = FindSceneObjectIncludingInactive(rootName);
            if (root != null)
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void SetHd2dAutonomousP2EmberSparkLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                if (child != null)
                {
                    SetHd2dAutonomousP2EmberSparkLayerRecursively(child.gameObject, layer);
                }
            }
        }

        private static void WriteHd2dAutonomousP2EmberSparkReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dEmberSparkProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics enableDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics flickerDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics strongerDiff)
        {
            var lines = new List<string>
            {
                "# P2-68 Embers / Sparks For Fires, Forges, And Torches Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative HDR additive ember/spark emitters for library torch/warm-anchor, central-plaza cookfire, and exterior porch torch.",
                "- Implementation note: this pass uses self-authored generated spark sprites. Kenney CC0 spark sprites remain a recommended art replacement, but no external asset was imported in this cycle.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2EmberSparkProfilePath}` |",
                $"| Runtime emitter | `{Hd2dAutonomousP2EmberSparkEmitterRuntimePath}` |",
                $"| Material / texture | `{Hd2dAutonomousP2EmberSparkMaterialPath}` / `{Hd2dAutonomousP2EmberSparkTexturePath}` |",
                $"| Needs Tom approval | {FormatBool(profile.NeedsTomApprovalForReview)} |",
                $"| Final approved | {FormatBool(profile.FinalEmberSparkApprovedForReview)} |",
                $"| Max particles per emitter | {profile.MaxParticlesPerEmitterForReview} |",
                $"| Expected particles library / cookfire / porch | {profile.ResolveExpectedParticleCountForReview(FastVsHd2dEmberSparkSourceKind.LibraryTorch)} / {profile.ResolveExpectedParticleCountForReview(FastVsHd2dEmberSparkSourceKind.Cookfire)} / {profile.ResolveExpectedParticleCountForReview(FastVsHd2dEmberSparkSourceKind.PorchTorch)} |",
                $"| Emission library / cookfire / porch | {profile.LibraryTorchEmissionRateForReview:0.###} / {profile.CookfireEmissionRateForReview:0.###} / {profile.PorchTorchEmissionRateForReview:0.###} |",
                $"| Lifetime / upper fade height | {profile.EmberLifetimeForReview:0.###}s / {profile.ExpectedUpperFadeHeightMetersForReview:0.###}m |",
                $"| Start size min/max | {profile.StartSizeMinForReview:0.###} / {profile.StartSizeMaxForReview:0.###} |",
                $"| Rise velocity min/max | {profile.RiseVelocityMinForReview:0.###} / {profile.RiseVelocityMaxForReview:0.###} |",
                $"| HDR intensity / additive / soft particles | {profile.HdrIntensityForReview:0.###} / {FormatBool(profile.AdditiveHdrMaterialForReview)} / {FormatBool(profile.SoftParticlesRequiredForReview)} |",
                $"| Flicker frequency / amplitude | {profile.FlickerFrequencyForReview:0.###}Hz / {profile.FlickerAmplitudeForReview:0.###} |",
                $"| Point light intensity / range | {profile.PointLightIntensityForReview:0.###} / {profile.PointLightRangeForReview:0.###}m |",
                $"| Source counts library / cookfire / porch | {CountHd2dAutonomousP2EmberSparkEmitters(FastVsHd2dEmberSparkSourceKind.LibraryTorch)} / {CountHd2dAutonomousP2EmberSparkEmitters(FastVsHd2dEmberSparkSourceKind.Cookfire)} / {CountHd2dAutonomousP2EmberSparkEmitters(FastVsHd2dEmberSparkSourceKind.PorchTorch)} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                enableDiff.ToReportRow("Embers disabled control vs conservative library torch"),
                flickerDiff.ToReportRow("Conservative frame A vs frame B flicker/drift"),
                strongerDiff.ToReportRow("Conservative vs stronger density option"),
                string.Empty,
                "| Screenshot | Label | Area | Sim seconds | Live particles | Applied rate sum | Light intensity sum |",
                "|---|---|---|---:|---:|---:|---:|"
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
                lines.Add($"| `{file}` | P2-68 ember/spark capture {i + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "embers_sparks_fires_forges_torches_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }
    }
}
