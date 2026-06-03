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
        private const string Hd2dAutonomousP2WaterSplashRippleRootName = "Past_CentralPlaza_P2_69_WaterSplashRippleReview";
        private const string Hd2dAutonomousP2WaterSplashRippleEmitterName = "P2_69_WaterSplashRippleEmitter";
        private const string Hd2dAutonomousP2WaterSplashRippleProfilePath = "Assets/Settings/FastVS_HD2D_P2_WaterSplashRippleProfile.asset";
        private const string Hd2dAutonomousP2WaterSplashRippleProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dWaterSplashRippleProfile.cs";
        private const string Hd2dAutonomousP2WaterSplashRippleEmitterRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dWaterSplashRippleEmitter.cs";
        private const string Hd2dAutonomousP2WaterSplashRippleRingMaterialId = "hd2d_p2_water_splash_ripple_ring";
        private const string Hd2dAutonomousP2WaterSplashRippleDropletMaterialId = "hd2d_p2_water_splash_droplet";
        private const string Hd2dAutonomousP2WaterSplashRippleMistMaterialId = "hd2d_p2_waterfall_fountain_mist";
        private const string Hd2dAutonomousP2WaterSplashRippleRingTextureId = "hd2d_p2_water_splash_ripple_ring_texture";
        private const string Hd2dAutonomousP2WaterSplashRippleDropletTextureId = "hd2d_p2_water_splash_droplet_texture";
        private const string Hd2dAutonomousP2WaterSplashRippleMistTextureId = "hd2d_p2_waterfall_fountain_mist_texture";
        private const string Hd2dAutonomousP2WaterSplashRippleRingMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2WaterSplashRippleRingMaterialId + ".mat";
        private const string Hd2dAutonomousP2WaterSplashRippleDropletMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2WaterSplashRippleDropletMaterialId + ".mat";
        private const string Hd2dAutonomousP2WaterSplashRippleMistMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2WaterSplashRippleMistMaterialId + ".mat";
        private const string Hd2dAutonomousP2WaterSplashRippleRingTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP2WaterSplashRippleRingTextureId + ".asset";
        private const string Hd2dAutonomousP2WaterSplashRippleDropletTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP2WaterSplashRippleDropletTextureId + ".asset";
        private const string Hd2dAutonomousP2WaterSplashRippleMistTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP2WaterSplashRippleMistTextureId + ".asset";
        private static Vector3 Hd2dAutonomousP2WaterSplashRippleEntryLocalPosition => CentralPlazaVsCenter + new Vector3(-2.38f, 0.188f, 2.26f);
        private static Vector3 Hd2dAutonomousP2WaterSplashRippleMistLocalPosition => CentralPlazaVsCenter + new Vector3(-2.05f, 0.245f, 2.55f);

        public static void CaptureHd2dAutonomousP2Item69WaterSplashRippleBatch()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterSplashRippleRootName) == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-69 water splash/ripple capture failed: review root is missing. Run BuildAndValidateBatch before capture.");
            }

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            var emitter = FindHd2dAutonomousP2WaterSplashRippleEmitter();
            var profile = EnsureHd2dAutonomousP2WaterSplashRippleProfile();
            if (controller == null || visibility == null || guide == null || camera == null || waterMaterial == null || emitter == null || profile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-69 water splash/ripple capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2WaterSplashRipples();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("water_splashes_ripples_waterfall_mist");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_entry_control_disabled_water.png",
                "02_entry_ripple_splash_frame_a.png",
                "03_entry_ripple_splash_expanded_0p45s.png",
                "04_entry_after_1p12s_fade_clear.png",
                "05_mist_disabled_baseline.png",
                "06_fountain_base_mist_conservative.png",
                "07_stronger_mist_splash_option_for_tom.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                controller.ClosePortal();
                SetHd2dAutonomousP1WaterDepthGradientForReview(waterMaterial, Hd2dAutonomousP1WaterDepthGradientStrength);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength * 0.42f, 0f);
                SetHd2dAutonomousP1WaterSpecularForReview(waterMaterial, Hd2dAutonomousP1WaterSpecularStrength * 0.55f);
                HideHd2dAutonomousP2WaterReviewSetsForFogCapture();
                emitter.SetDistanceCullEnabledForReview(false);

                SetHd2dAutonomousP2WaterSplashRippleVisible(false);
                SetHd2dAutonomousP2WaterSplashRippleMultipliers(1f, 1f, 1f);
                CaptureHd2dAutonomousP2WaterSplashRippleEntryShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[0],
                    "control: review water surface with ripple/splash/mist particles disabled",
                    0.05f,
                    false,
                    shotRows);

                SetHd2dAutonomousP2WaterSplashRippleVisible(true);
                SetHd2dAutonomousP2WaterSplashRippleMultipliers(1f, 1f, 1f);
                CaptureHd2dAutonomousP2WaterSplashRippleEntryShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[1],
                    "frame A: entry contact emits a horizontal expanding ring and Birth sub-emitter droplet crown",
                    0.18f,
                    true,
                    shotRows);

                CaptureHd2dAutonomousP2WaterSplashRippleEntryShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[2],
                    "frame B: same contact advanced to a wider fading ring with falling droplets",
                    0.50f,
                    true,
                    shotRows);

                CaptureHd2dAutonomousP2WaterSplashRippleEntryShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[3],
                    "fade proof: ripple and splash clear after the configured one-second lifetime window",
                    1.12f,
                    true,
                    shotRows);

                SetHd2dAutonomousP2WaterSplashRippleVisible(false);
                CaptureHd2dAutonomousP2WaterSplashRippleMistShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[4],
                    "control: fountain/waterfall-style base mist disabled",
                    0.10f,
                    shotRows);

                SetHd2dAutonomousP2WaterSplashRippleVisible(true);
                SetHd2dAutonomousP2WaterSplashRippleMultipliers(1f, 1f, 1f);
                CaptureHd2dAutonomousP2WaterSplashRippleMistShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[5],
                    "conservative: low-alpha soft mist rises from the water impact ledge",
                    1.85f,
                    shotRows);

                SetHd2dAutonomousP2WaterSplashRippleMultipliers(profile.StrongerOptionMultiplierForReview, profile.StrongerOptionMultiplierForReview, profile.StrongerOptionMultiplierForReview);
                CaptureHd2dAutonomousP2WaterSplashRippleMistShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[6],
                    "stronger option for Tom: higher mist opacity plus denser ripple/splash scaling",
                    1.85f,
                    shotRows);
            }
            finally
            {
                emitter.SetDistanceCullEnabledForReview(true);
                SetHd2dAutonomousP2WaterSplashRippleVisible(true);
                SetHd2dAutonomousP2WaterSplashRippleMultipliers(1f, 1f, 1f);
                RestoreHd2dAutonomousP2WaterReviewSetsAfterFogCapture();
                ConfigureHd2dAutonomousP2DirectionalWaterFlowMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2DirectionalWaterFlowProfile(), true, 0f);
                ConfigureHd2dAutonomousP2FakeRefractionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2FakeRefractionProfile(), true, 0f);
                ConfigureHd2dAutonomousP2ToonWaterMotionMaterialForReview(waterMaterial, EnsureHd2dAutonomousP2ToonWaterMotionProfile(), true, true, 0f);
                SetHd2dAutonomousP1WaterFoamForReview(waterMaterial, Hd2dAutonomousP1WaterFoamStrength, 0f);
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                AssetDatabase.SaveAssets();
            }

            var entryEnableDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var entryMotionDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var entryFadeDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[3]);
            var mistEnableDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[4], screenshotFiles[5]);
            var strongerDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[5], screenshotFiles[6]);
            WriteHd2dAutonomousP2WaterSplashRippleReviewReport(outputDirectory, screenshotFiles, shotRows, profile, entryEnableDiff, entryMotionDiff, entryFadeDiff, mistEnableDiff, strongerDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-69 water splash/ripple review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2WaterSplashRipples(Transform pastCentralPlazaRoot)
        {
            var profile = EnsureHd2dAutonomousP2WaterSplashRippleProfile();
            var waterMaterial = EnsureHd2dAutonomousP1DepthGradientWaterMaterial();
            var rippleMaterial = EnsureHd2dAutonomousP2WaterSplashRippleRingMaterial(profile);
            var dropletMaterial = EnsureHd2dAutonomousP2WaterSplashRippleDropletMaterial(profile);
            var mistMaterial = EnsureHd2dAutonomousP2WaterSplashRippleMistMaterial(profile);
            if (pastCentralPlazaRoot == null || waterMaterial == null || profile == null || rippleMaterial == null || dropletMaterial == null || mistMaterial == null)
            {
                return;
            }

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterSplashRippleRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2WaterSplashRippleRootName);
            root.transform.SetParent(pastCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            CreateHd2dAutonomousP2WaterSplashRippleReviewSet(root.transform, waterMaterial);
            CreateHd2dAutonomousP2WaterSplashRippleEmitter(root.transform, profile, rippleMaterial, dropletMaterial, mistMaterial);
            SetHd2dAutonomousP2WaterSplashRippleLayerRecursively(root, OtherTimeSpaceRenderLayer);
            EditorUtility.SetDirty(root);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2WaterSplashRipples()
        {
            var profile = EnsureHd2dAutonomousP2WaterSplashRippleProfile();
            var rippleMaterial = EnsureHd2dAutonomousP2WaterSplashRippleRingMaterial(profile);
            var dropletMaterial = EnsureHd2dAutonomousP2WaterSplashRippleDropletMaterial(profile);
            var mistMaterial = EnsureHd2dAutonomousP2WaterSplashRippleMistMaterial(profile);
            if (profile == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalWaterSplashRippleApprovedForReview ||
                !profile.SubEmitterBirthForReview ||
                !profile.RaycastEntryHookPreparedForReview ||
                !profile.ContinuousMistEmitterForReview ||
                !profile.SoftParticlesRequiredForReview ||
                !profile.FadeCompleteWithinOneSecondForReview ||
                !profile.ConservativeDataPrepForReview ||
                profile.RippleLifetimeForReview > 1.0f ||
                profile.SplashBurstParticlesForReview > 18 ||
                profile.ExpectedMistParticleCountForReview > profile.MaxMistParticlesForReview ||
                profile.StrongerOptionMultiplierForReview <= 1f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-69 needs conservative non-final ripple/splash/mist data, Birth sub-emitter, raycast hook prep, soft-particle mist, <=1s ripple fade, and Tom approval left open.");
            }

            if (rippleMaterial == null || dropletMaterial == null || mistMaterial == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-69 particle materials are missing.");
            }

            ValidateHd2dAutonomousP2WaterSplashRippleParticleMaterial(dropletMaterial, "droplet");
            ValidateHd2dAutonomousP2WaterSplashRippleParticleMaterial(mistMaterial, "mist");

            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterSplashRippleRootName);
            var emitter = FindHd2dAutonomousP2WaterSplashRippleEmitter();
            if (root == null ||
                CountHd2dAutonomousP2WaterSplashRippleReviewRenderers("WaterSurface") < 1 ||
                CountHd2dAutonomousP2WaterSplashRippleReviewRenderers("WaterfallMistLedge") < 1 ||
                emitter == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-69 review root requires water surface, waterfall/fountain ledge, and one runtime emitter.");
            }

            SetHd2dAutonomousP2WaterSplashRippleVisible(true);
            SetHd2dAutonomousP2WaterSplashRippleMultipliers(1f, 1f, 1f);
            ValidateHd2dAutonomousP2WaterSplashRippleEmitter(emitter, profile, rippleMaterial, dropletMaterial, mistMaterial);

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2WaterSplashRippleProfileRuntimePath), "subEmitterBirth", Hd2dAutonomousP2WaterSplashRippleProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2WaterSplashRippleProfileRuntimePath), "raycastEntryHookPrepared", Hd2dAutonomousP2WaterSplashRippleProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2WaterSplashRippleProfileRuntimePath), "finalWaterSplashRippleApproved", Hd2dAutonomousP2WaterSplashRippleProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2WaterSplashRippleEmitterRuntimePath), "TriggerRippleAtForReview", Hd2dAutonomousP2WaterSplashRippleEmitterRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2WaterSplashRippleEmitterRuntimePath), "TryTriggerRippleFromRaycastForReview", Hd2dAutonomousP2WaterSplashRippleEmitterRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.P2WaterSplashRipple.cs"), "ParticleSystemSubEmitterType.Birth", "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2WaterSplashRipple.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2WaterSplashRipples", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2WaterSplashRipples", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dWaterSplashRippleProfile EnsureHd2dAutonomousP2WaterSplashRippleProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dWaterSplashRippleProfile>(Hd2dAutonomousP2WaterSplashRippleProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dWaterSplashRippleProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2WaterSplashRippleProfilePath);
            }

            profile.ConfigureForReview(
                10,
                12,
                40,
                0.96f,
                0.34f,
                1.12f,
                0.36f,
                0.56f,
                0.065f,
                0.120f,
                0.60f,
                1.05f,
                1.70f,
                0.30f,
                12.8f,
                2.35f,
                0.22f,
                0.56f,
                0.18f,
                0.12f,
                0.14f,
                0.32f,
                0.30f,
                0.90f,
                0.48f,
                0.90f,
                38f,
                1.34f,
                new Color(0.68f, 0.94f, 1.15f, 0.36f),
                new Color(1.18f, 1.34f, 1.45f, 1.00f),
                new Color(0.70f, 0.86f, 0.94f, 0.30f),
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                false,
                "Keep this conservative P2-69 water FX data prep. Tom should tune final ripple ring thickness, droplet crown density, mist opacity, and placement against approved water-body art and lighting.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static void CreateHd2dAutonomousP2WaterSplashRippleReviewSet(Transform root, Material waterMaterial)
        {
            var floorMaterial = FlatMaterial("hd2d_p2_water_splash_ripple_dark_basin", new Color(0.050f, 0.070f, 0.075f, 1f), false);
            var stoneMaterial = FlatMaterial("hd2d_p2_water_splash_ripple_mossy_stone", new Color(0.30f, 0.31f, 0.24f, 1f), false);
            var markerMaterial = FlatMaterial("hd2d_p2_water_splash_ripple_contact_marker", new Color(0.10f, 0.12f, 0.13f, 1f), false);

            CreateHd2dAutonomousP2WaterSplashRippleCube(
                root,
                "P2_69_WaterSplashRipple_DarkBasin",
                CentralPlazaVsCenter + new Vector3(-2.48f, 0.095f, 2.28f),
                new Vector3(2.92f, 0.040f, 1.62f),
                Quaternion.Euler(0f, -9f, 0f),
                floorMaterial,
                false,
                "past.central_plaza.p2_69.dark_basin");
            CreateHd2dAutonomousP2WaterSplashRippleCube(
                root,
                "P2_69_WaterSplashRipple_WaterSurface",
                CentralPlazaVsCenter + new Vector3(-2.38f, 0.155f, 2.26f),
                new Vector3(2.32f, 0.040f, 1.18f),
                Quaternion.Euler(0f, -9f, 0f),
                waterMaterial,
                true,
                "past.central_plaza.p2_69.water_surface");
            CreateHd2dAutonomousP2WaterSplashRippleCube(
                root,
                "P2_69_WaterSplashRipple_WaterfallMistLedge",
                CentralPlazaVsCenter + new Vector3(-2.00f, 0.405f, 2.72f),
                new Vector3(1.34f, 0.44f, 0.13f),
                Quaternion.Euler(0f, -9f, 0f),
                stoneMaterial,
                false,
                "past.central_plaza.p2_69.waterfall_mist_ledge");
            CreateHd2dAutonomousP2WaterSplashRippleCube(
                root,
                "P2_69_WaterSplashRipple_ShoreMarkerA",
                CentralPlazaVsCenter + new Vector3(-3.32f, 0.205f, 2.56f),
                new Vector3(0.55f, 0.055f, 0.13f),
                Quaternion.Euler(0f, -22f, 0f),
                stoneMaterial,
                false,
                "past.central_plaza.p2_69.shore_marker_a");
            CreateHd2dAutonomousP2WaterSplashRippleCube(
                root,
                "P2_69_WaterSplashRipple_ShoreMarkerB",
                CentralPlazaVsCenter + new Vector3(-1.62f, 0.210f, 2.02f),
                new Vector3(0.68f, 0.060f, 0.14f),
                Quaternion.Euler(0f, 16f, 0f),
                stoneMaterial,
                false,
                "past.central_plaza.p2_69.shore_marker_b");
            CreateHd2dAutonomousP2WaterSplashRippleCube(
                root,
                "P2_69_WaterSplashRipple_EntryPebbleMarker",
                Hd2dAutonomousP2WaterSplashRippleEntryLocalPosition + new Vector3(0.00f, 0.095f, 0.00f),
                new Vector3(0.12f, 0.10f, 0.12f),
                Quaternion.Euler(0f, 24f, 0f),
                markerMaterial,
                false,
                "past.central_plaza.p2_69.entry_pebble_marker");
        }

        private static GameObject CreateHd2dAutonomousP2WaterSplashRippleCube(
            Transform root,
            string objectName,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation,
            Material material,
            bool keepCollider,
            string landmarkId)
        {
            var cube = CreateLandmarkCube(
                objectName,
                root,
                localPosition,
                localScale,
                localRotation,
                material,
                keepCollider,
                TimeWindowPairedSpaceLandmarkKind.PropOrFeature,
                landmarkId);
            var landmark = cube.GetComponent<TimeWindowPairedSpaceLandmark>();
            if (landmark != null)
            {
                SerializedSet(landmark, "countsForArrival", false);
                EditorUtility.SetDirty(landmark);
            }

            cube.layer = OtherTimeSpaceRenderLayer;
            var renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = true;
            }

            EditorUtility.SetDirty(cube);
            return cube;
        }

        private static void CreateHd2dAutonomousP2WaterSplashRippleEmitter(
            Transform root,
            FastVsHd2dWaterSplashRippleProfile profile,
            Material rippleMaterial,
            Material dropletMaterial,
            Material mistMaterial)
        {
            var emitterObject = new GameObject(Hd2dAutonomousP2WaterSplashRippleEmitterName);
            emitterObject.transform.SetParent(root, false);
            var center = Vector3.Lerp(Hd2dAutonomousP2WaterSplashRippleEntryLocalPosition, Hd2dAutonomousP2WaterSplashRippleMistLocalPosition, 0.5f);
            emitterObject.transform.localPosition = center;
            emitterObject.transform.localRotation = Quaternion.identity;
            emitterObject.transform.localScale = Vector3.one;
            emitterObject.layer = OtherTimeSpaceRenderLayer;

            var rippleSystem = CreateHd2dAutonomousP2WaterSplashRippleParticleChild(emitterObject.transform, "RippleSystem", Hd2dAutonomousP2WaterSplashRippleEntryLocalPosition - center);
            var splashSystem = CreateHd2dAutonomousP2WaterSplashRippleParticleChild(emitterObject.transform, "SplashDropletSubEmitter", Hd2dAutonomousP2WaterSplashRippleEntryLocalPosition - center);
            var mistSystem = CreateHd2dAutonomousP2WaterSplashRippleParticleChild(emitterObject.transform, "MistSystem", Hd2dAutonomousP2WaterSplashRippleMistLocalPosition - center);
            var rippleRenderer = rippleSystem.GetComponent<ParticleSystemRenderer>();
            var splashRenderer = splashSystem.GetComponent<ParticleSystemRenderer>();
            var mistRenderer = mistSystem.GetComponent<ParticleSystemRenderer>();

            ConfigureHd2dAutonomousP2WaterSplashRippleRingSystem(rippleSystem, rippleRenderer, profile, rippleMaterial);
            ConfigureHd2dAutonomousP2WaterSplashRippleSplashSystem(splashSystem, splashRenderer, profile, dropletMaterial);
            ConfigureHd2dAutonomousP2WaterSplashRippleMistSystem(mistSystem, mistRenderer, profile, mistMaterial);
            ConfigureHd2dAutonomousP2WaterSplashRippleBirthSubEmitter(rippleSystem, splashSystem);

            var emitter = emitterObject.AddComponent<FastVsHd2dWaterSplashRippleEmitter>();
            emitter.ConfigureForReview(
                profile,
                rippleSystem,
                splashSystem,
                mistSystem,
                rippleRenderer,
                splashRenderer,
                mistRenderer,
                1 << OtherTimeSpaceRenderLayer,
                profile.RaycastEntryHookPreparedForReview,
                true);

            EditorUtility.SetDirty(emitterObject);
            EditorUtility.SetDirty(emitter);
        }

        private static ParticleSystem CreateHd2dAutonomousP2WaterSplashRippleParticleChild(Transform parent, string objectName, Vector3 localPosition)
        {
            var child = new GameObject(objectName);
            child.transform.SetParent(parent, false);
            child.transform.localPosition = localPosition;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
            child.layer = OtherTimeSpaceRenderLayer;
            var system = child.AddComponent<ParticleSystem>();
            var renderer = child.GetComponent<ParticleSystemRenderer>();
            if (renderer == null)
            {
                renderer = child.AddComponent<ParticleSystemRenderer>();
            }

            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.forceRenderingOff = false;
            ForceRendererEnabledForReview(renderer);
            return system;
        }

        private static void ConfigureHd2dAutonomousP2WaterSplashRippleRingSystem(
            ParticleSystem system,
            ParticleSystemRenderer renderer,
            FastVsHd2dWaterSplashRippleProfile profile,
            Material material)
        {
            var main = system.main;
            main.loop = false;
            main.prewarm = false;
            main.playOnAwake = false;
            main.duration = profile.RippleLifetimeForReview;
            main.startLifetime = new ParticleSystem.MinMaxCurve(profile.RippleLifetimeForReview);
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(profile.RippleEndSizeForReview);
            main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
            main.startColor = new ParticleSystem.MinMaxGradient(profile.RippleTintForReview);
            main.maxParticles = profile.MaxRippleParticlesForReview;
            main.gravityModifier = 0f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = false;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
            emission.SetBursts(Array.Empty<ParticleSystem.Burst>());

            var shape = system.shape;
            shape.enabled = false;

            var color = system.colorOverLifetime;
            color.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(profile.RippleTintForReview, 0f),
                    new GradientColorKey(profile.RippleTintForReview, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(profile.RipplePeakAlphaForReview, 0.08f),
                    new GradientAlphaKey(profile.RipplePeakAlphaForReview * 0.48f, 0.54f),
                    new GradientAlphaKey(0f, 1f)
                });
            color.color = new ParticleSystem.MinMaxGradient(gradient);

            var size = system.sizeOverLifetime;
            size.enabled = true;
            var startScale = profile.RippleStartSizeForReview / Mathf.Max(0.001f, profile.RippleEndSizeForReview);
            size.size = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(
                    new Keyframe(0f, startScale),
                    new Keyframe(0.48f, 0.72f),
                    new Keyframe(1f, 1f)));

            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.HorizontalBillboard;
            renderer.sortingOrder = 8;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.forceRenderingOff = false;
            ForceRendererEnabledForReview(renderer);

            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            system.Clear(true);
        }

        private static void ConfigureHd2dAutonomousP2WaterSplashRippleSplashSystem(
            ParticleSystem system,
            ParticleSystemRenderer renderer,
            FastVsHd2dWaterSplashRippleProfile profile,
            Material material)
        {
            var main = system.main;
            main.loop = false;
            main.prewarm = false;
            main.playOnAwake = false;
            main.duration = profile.SplashLifetimeForReview;
            main.startLifetime = new ParticleSystem.MinMaxCurve(profile.SplashLifetimeForReview * 0.72f, profile.SplashLifetimeForReview * 1.08f);
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(profile.SplashStartSizeMinForReview, profile.SplashStartSizeMaxForReview);
            main.startRotation = new ParticleSystem.MinMaxCurve(-0.35f, 0.35f);
            main.startColor = new ParticleSystem.MinMaxGradient(profile.SplashTintForReview);
            main.maxParticles = Mathf.Max(18, profile.SplashBurstParticlesForReview * 3);
            main.gravityModifier = profile.SplashGravityForReview;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(0f);
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)profile.SplashBurstParticlesForReview) });

            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.radius = 0.075f;
            shape.angle = 34f;
            shape.radiusThickness = 0.80f;
            shape.arc = 360f;
            shape.randomDirectionAmount = 0.12f;

            var velocity = system.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            var horizontal = profile.SplashHorizontalVelocityForReview;
            velocity.x = new ParticleSystem.MinMaxCurve(-horizontal, horizontal);
            velocity.y = new ParticleSystem.MinMaxCurve(profile.SplashUpVelocityMinForReview, profile.SplashUpVelocityMaxForReview);
            velocity.z = new ParticleSystem.MinMaxCurve(-horizontal, horizontal);

            var color = system.colorOverLifetime;
            color.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(profile.SplashTintForReview, 0f),
                    new GradientColorKey(profile.SplashTintForReview * 0.72f, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(profile.SplashTintForReview.a, 0.08f),
                    new GradientAlphaKey(profile.SplashTintForReview.a * 0.64f, 0.42f),
                    new GradientAlphaKey(0f, 1f)
                });
            color.color = new ParticleSystem.MinMaxGradient(gradient);

            var size = system.sizeOverLifetime;
            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(
                    new Keyframe(0f, 0.82f),
                    new Keyframe(0.18f, 1.0f),
                    new Keyframe(1f, 0.28f)));

            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.sortingOrder = 9;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.forceRenderingOff = false;
            ForceRendererEnabledForReview(renderer);

            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            system.Clear(true);
        }

        private static void ConfigureHd2dAutonomousP2WaterSplashRippleMistSystem(
            ParticleSystem system,
            ParticleSystemRenderer renderer,
            FastVsHd2dWaterSplashRippleProfile profile,
            Material material)
        {
            var main = system.main;
            main.loop = true;
            main.prewarm = true;
            main.playOnAwake = true;
            main.duration = Mathf.Max(3.0f, profile.MistLifetimeForReview);
            main.startLifetime = new ParticleSystem.MinMaxCurve(profile.MistLifetimeForReview * 0.78f, profile.MistLifetimeForReview * 1.12f);
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(profile.MistStartSizeMinForReview, profile.MistStartSizeMaxForReview);
            main.startRotation = new ParticleSystem.MinMaxCurve(-0.35f, 0.35f);
            main.startColor = new ParticleSystem.MinMaxGradient(profile.MistTintForReview);
            main.maxParticles = profile.MaxMistParticlesForReview;
            main.gravityModifier = -0.010f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(profile.MistEmissionRateForReview);
            emission.SetBursts(Array.Empty<ParticleSystem.Burst>());

            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.radius = 0.30f;
            shape.angle = 18f;
            shape.radiusThickness = 0.74f;
            shape.arc = 360f;
            shape.randomDirectionAmount = 0.08f;

            var velocity = system.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            var outward = profile.MistOutwardVelocityForReview;
            velocity.x = new ParticleSystem.MinMaxCurve(-outward, outward);
            velocity.y = new ParticleSystem.MinMaxCurve(profile.MistRiseVelocityForReview * 0.78f, profile.MistRiseVelocityForReview * 1.16f);
            velocity.z = new ParticleSystem.MinMaxCurve(-outward * 0.72f, outward * 1.15f);

            var color = system.colorOverLifetime;
            color.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(profile.MistTintForReview, 0f),
                    new GradientColorKey(profile.MistTintForReview * 0.86f, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(profile.MistPeakAlphaForReview, 0.14f),
                    new GradientAlphaKey(profile.MistPeakAlphaForReview * 0.58f, 0.60f),
                    new GradientAlphaKey(0f, 1f)
                });
            color.color = new ParticleSystem.MinMaxGradient(gradient);

            var size = system.sizeOverLifetime;
            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(
                    new Keyframe(0f, 0.62f),
                    new Keyframe(0.45f, 1.04f),
                    new Keyframe(1f, 1.24f)));

            var noise = system.noise;
            noise.enabled = true;
            noise.strength = new ParticleSystem.MinMaxCurve(profile.MistNoiseStrengthForReview);
            noise.frequency = profile.MistNoiseFrequencyForReview;
            noise.scrollSpeed = 0.12f;
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

        private static void ConfigureHd2dAutonomousP2WaterSplashRippleBirthSubEmitter(ParticleSystem rippleSystem, ParticleSystem splashSystem)
        {
            var subEmitters = rippleSystem.subEmitters;
            subEmitters.enabled = true;
            while (subEmitters.subEmittersCount > 0)
            {
                subEmitters.RemoveSubEmitter(0);
            }

            subEmitters.AddSubEmitter(splashSystem, ParticleSystemSubEmitterType.Birth, ParticleSystemSubEmitterProperties.InheritColor);
        }

        private static Material EnsureHd2dAutonomousP2WaterSplashRippleRingMaterial(FastVsHd2dWaterSplashRippleProfile profile)
        {
            var material = EnsureHd2dAutonomousP2WaterSplashRippleParticleMaterial(
                Hd2dAutonomousP2WaterSplashRippleRingMaterialPath,
                Hd2dAutonomousP2WaterSplashRippleRingMaterialId,
                EnsureHd2dAutonomousP2WaterSplashRippleRingTexture(),
                profile.RippleTintForReview,
                3047,
                false);
            return material;
        }

        private static Material EnsureHd2dAutonomousP2WaterSplashRippleDropletMaterial(FastVsHd2dWaterSplashRippleProfile profile)
        {
            return EnsureHd2dAutonomousP2WaterSplashRippleParticleMaterial(
                Hd2dAutonomousP2WaterSplashRippleDropletMaterialPath,
                Hd2dAutonomousP2WaterSplashRippleDropletMaterialId,
                EnsureHd2dAutonomousP2WaterSplashRippleDropletTexture(),
                profile.SplashTintForReview,
                3048,
                true,
                profile.SoftParticleFarFadeForReview);
        }

        private static Material EnsureHd2dAutonomousP2WaterSplashRippleMistMaterial(FastVsHd2dWaterSplashRippleProfile profile)
        {
            return EnsureHd2dAutonomousP2WaterSplashRippleParticleMaterial(
                Hd2dAutonomousP2WaterSplashRippleMistMaterialPath,
                Hd2dAutonomousP2WaterSplashRippleMistMaterialId,
                EnsureHd2dAutonomousP2WaterSplashRippleMistTexture(),
                profile.MistTintForReview,
                3046,
                true,
                profile.SoftParticleFarFadeForReview);
        }

        private static Material EnsureHd2dAutonomousP2WaterSplashRippleParticleMaterial(
            string materialPath,
            string materialId,
            Texture2D texture,
            Color tint,
            int renderQueue,
            bool softParticles,
            float softParticlesFarFade = 0.90f)
        {
            EnsureFolder(MaterialDirectory);
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
            {
                throw new InvalidOperationException($"P2-69 water particle shader not found for {materialId}.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, materialPath);
            }

            material.shader = shader;
            ConfigureTransparentParticleMaterial(material, renderQueue);
            material.SetOverrideTag("RenderType", "Transparent");
            if (material.HasProperty("_Blend"))
            {
                material.SetFloat("_Blend", 0f);
            }

            material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_ZWrite", 0f);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = renderQueue;
            AssignMaterialTexture(material, texture, Vector2.one);
            if (material.HasProperty("_BaseColor"))
            {
                var materialTint = tint;
                materialTint.a = 1f;
                material.SetColor("_BaseColor", materialTint);
            }

            if (material.HasProperty("_Color"))
            {
                var materialTint = tint;
                materialTint.a = 1f;
                material.SetColor("_Color", materialTint);
            }

            if (material.HasProperty("_SoftParticlesEnabled"))
            {
                material.SetFloat("_SoftParticlesEnabled", softParticles ? 1f : 0f);
            }

            if (material.HasProperty("_SoftParticlesNearFadeDistance"))
            {
                material.SetFloat("_SoftParticlesNearFadeDistance", 0f);
            }

            if (material.HasProperty("_SoftParticlesFarFadeDistance"))
            {
                material.SetFloat("_SoftParticlesFarFadeDistance", softParticles ? softParticlesFarFade : 0f);
            }

            if (softParticles)
            {
                material.EnableKeyword("_SOFTPARTICLES_ON");
            }
            else
            {
                material.DisableKeyword("_SOFTPARTICLES_ON");
            }

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);
            material.SetShaderPassEnabled("ShadowCaster", false);
            material.enableInstancing = true;
            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP2WaterSplashRippleRingTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2WaterSplashRippleRingTextureId,
                96,
                96,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = ((x + 0.5f) / 96f) * 2f - 1f;
                    var v = ((y + 0.5f) / 96f) * 2f - 1f;
                    var distance = Mathf.Sqrt(u * u + v * v);
                    var ring = Mathf.SmoothStep(0.64f, 0.72f, distance) * (1f - Mathf.SmoothStep(0.80f, 0.91f, distance));
                    var broken = 0.74f + 0.26f * Mathf.Sin((Mathf.Atan2(v, u) * 9f) + distance * 18f);
                    var alpha = Mathf.Clamp01(ring * broken);
                    return new Color(0.82f, 0.96f, 1f, alpha);
                });
        }

        private static Texture2D EnsureHd2dAutonomousP2WaterSplashRippleDropletTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2WaterSplashRippleDropletTextureId,
                64,
                64,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = ((x + 0.5f) / 64f) * 2f - 1f;
                    var v = ((y + 0.5f) / 64f) * 2f - 1f;
                    var d = Mathf.Sqrt((u * u * 1.35f) + (v * v * 2.1f));
                    var core = Mathf.Clamp01(1f - d);
                    var glint = Mathf.Clamp01(1f - Mathf.Sqrt(((u + 0.24f) * (u + 0.24f) * 8f) + ((v - 0.30f) * (v - 0.30f) * 8f)));
                    var alpha = Mathf.Clamp01(core * 0.88f + glint * 0.42f);
                    return new Color(0.86f, 0.96f, 1f, alpha);
                });
        }

        private static Texture2D EnsureHd2dAutonomousP2WaterSplashRippleMistTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2WaterSplashRippleMistTextureId,
                96,
                96,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = ((x + 0.5f) / 96f) * 2f - 1f;
                    var v = ((y + 0.5f) / 96f) * 2f - 1f;
                    var d = Mathf.Sqrt((u * u * 0.88f) + (v * v * 1.24f));
                    var baseAlpha = Mathf.Clamp01(1f - d);
                    var cellA = Mathf.Sin((u * 12.7f) + (v * 4.1f));
                    var cellB = Mathf.Sin((u * -5.4f) + (v * 15.3f));
                    var noise = 0.74f + 0.13f * cellA + 0.13f * cellB;
                    var alpha = Mathf.Clamp01(baseAlpha * baseAlpha * noise);
                    return new Color(0.82f, 0.92f, 0.96f, alpha);
                });
        }

        private static void ValidateHd2dAutonomousP2WaterSplashRippleParticleMaterial(Material material, string label)
        {
            var softParticlesEnabled =
                material.IsKeywordEnabled("_SOFTPARTICLES_ON") ||
                (material.HasProperty("_SoftParticlesEnabled") && material.GetFloat("_SoftParticlesEnabled") >= 0.5f);
            var softParticlesFarFade = material.HasProperty("_SoftParticlesFarFadeDistance") ? material.GetFloat("_SoftParticlesFarFadeDistance") : 0.90f;
            var transparent = !material.HasProperty("_ZWrite") || material.GetFloat("_ZWrite") < 0.5f;
            if (!softParticlesEnabled || softParticlesFarFade < 0.20f || !transparent)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-69 {label} material must keep soft particles and transparent rendering. soft={softParticlesEnabled}, far={softParticlesFarFade:0.###}, transparent={transparent}.");
            }
        }

        private static void ValidateHd2dAutonomousP2WaterSplashRippleEmitter(
            FastVsHd2dWaterSplashRippleEmitter emitter,
            FastVsHd2dWaterSplashRippleProfile profile,
            Material expectedRippleMaterial,
            Material expectedDropletMaterial,
            Material expectedMistMaterial)
        {
            var ripple = emitter != null ? emitter.RippleSystemForReview : null;
            var splash = emitter != null ? emitter.SplashDropletSystemForReview : null;
            var mist = emitter != null ? emitter.MistSystemForReview : null;
            var rippleRenderer = emitter != null ? emitter.RippleRendererForReview : null;
            var splashRenderer = emitter != null ? emitter.SplashRendererForReview : null;
            var mistRenderer = emitter != null ? emitter.MistRendererForReview : null;
            if (emitter == null || ripple == null || splash == null || mist == null || rippleRenderer == null || splashRenderer == null || mistRenderer == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-69 water splash/ripple emitter is missing runtime component, particle systems, or renderers.");
            }

            var rippleMain = ripple.main;
            var rippleEmission = ripple.emission;
            var rippleColor = ripple.colorOverLifetime;
            var rippleSize = ripple.sizeOverLifetime;
            var splashMain = splash.main;
            var splashEmission = splash.emission;
            var splashVelocity = splash.velocityOverLifetime;
            var mistMain = mist.main;
            var mistEmission = mist.emission;
            var mistVelocity = mist.velocityOverLifetime;
            var mistNoise = mist.noise;
            if (emitter.ProfileForReview != profile ||
                !emitter.RaycastEntryHookPreparedForReview ||
                !emitter.DistanceCullEnabledForReview ||
                !emitter.SubEmitterBirthConfiguredForReview ||
                ripple.gameObject.layer != OtherTimeSpaceRenderLayer ||
                splash.gameObject.layer != OtherTimeSpaceRenderLayer ||
                mist.gameObject.layer != OtherTimeSpaceRenderLayer ||
                rippleMain.loop ||
                rippleMain.playOnAwake ||
                rippleMain.simulationSpace != ParticleSystemSimulationSpace.World ||
                rippleMain.maxParticles > profile.MaxRippleParticlesForReview ||
                rippleMain.startLifetime.constant > 1.0f ||
                rippleEmission.enabled ||
                !rippleColor.enabled ||
                !rippleSize.enabled ||
                splashMain.loop ||
                splashMain.playOnAwake ||
                splashMain.simulationSpace != ParticleSystemSimulationSpace.World ||
                splashEmission.burstCount < 1 ||
                !splashVelocity.enabled ||
                mistMain.loop == false ||
                !mistMain.playOnAwake ||
                mistMain.simulationSpace != ParticleSystemSimulationSpace.World ||
                mistMain.maxParticles > profile.MaxMistParticlesForReview ||
                !mistEmission.enabled ||
                mistEmission.rateOverTime.constant <= 0f ||
                !mistVelocity.enabled ||
                !mistNoise.enabled ||
                rippleRenderer.sharedMaterial != expectedRippleMaterial ||
                splashRenderer.sharedMaterial != expectedDropletMaterial ||
                mistRenderer.sharedMaterial != expectedMistMaterial ||
                rippleRenderer.renderMode != ParticleSystemRenderMode.HorizontalBillboard ||
                rippleRenderer.shadowCastingMode != ShadowCastingMode.Off ||
                splashRenderer.shadowCastingMode != ShadowCastingMode.Off ||
                mistRenderer.shadowCastingMode != ShadowCastingMode.Off ||
                rippleRenderer.receiveShadows ||
                splashRenderer.receiveShadows ||
                mistRenderer.receiveShadows)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P2-69 requires one manual horizontal ripple system with Birth sub-emitter droplets plus one looping soft mist system, all World-space and shadowless. " +
                    $"ripple loop={rippleMain.loop}, life={rippleMain.startLifetime.constant:0.###}, sub={emitter.SubEmitterBirthConfiguredForReview}, splashBursts={splashEmission.burstCount}, mistLoop={mistMain.loop}, mistRate={mistEmission.rateOverTime.constant:0.###}.");
            }
        }

        private static void CaptureHd2dAutonomousP2WaterSplashRippleEntryShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            bool triggerEntry,
            ICollection<string> shotRows)
        {
            var emitter = FindHd2dAutonomousP2WaterSplashRippleEmitter();
            if (emitter == null)
            {
                throw new InvalidOperationException("P2-69 entry capture failed: emitter missing.");
            }

            emitter.ClearTransientEntryForReview();
            if (triggerEntry)
            {
                emitter.TriggerRippleAtForReview(controller.OtherTimeSpaceRootForReview.TransformPoint(Hd2dAutonomousP2WaterSplashRippleEntryLocalPosition));
            }

            emitter.SimulateForReview(simulateSeconds, false);
            CaptureHd2dAutonomousP2WaterSplashRippleCloseShot(
                controller,
                visibility,
                guide,
                camera,
                CentralPlazaVsCenter + new Vector3(-3.34f, 0.02f, 1.64f),
                CentralPlazaVsCenter + new Vector3(-2.38f, 0.24f, 2.26f),
                new Vector3(0.64f, 1.62f, -2.38f),
                new Vector3(0.00f, 0.02f, 0.08f),
                33f,
                outputDirectory,
                fileName);
            shotRows.Add($"| `{fileName}` | {label} | EntryRipple | {simulateSeconds:0.###} | {FormatBool(triggerEntry)} | {emitter.RippleParticleCountForReview} | {emitter.SplashParticleCountForReview} | {emitter.MistParticleCountForReview} | {emitter.AppliedMistEmissionRateForReview:0.###} |");
        }

        private static void CaptureHd2dAutonomousP2WaterSplashRippleMistShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            string label,
            float simulateSeconds,
            ICollection<string> shotRows)
        {
            var emitter = FindHd2dAutonomousP2WaterSplashRippleEmitter();
            if (emitter == null)
            {
                throw new InvalidOperationException("P2-69 mist capture failed: emitter missing.");
            }

            emitter.ClearForReview();
            emitter.SimulateForReview(simulateSeconds, false);
            CaptureHd2dAutonomousP2WaterSplashRippleCloseShot(
                controller,
                visibility,
                guide,
                camera,
                CentralPlazaVsCenter + new Vector3(-3.34f, 0.02f, 1.64f),
                CentralPlazaVsCenter + new Vector3(-2.05f, 0.36f, 2.55f),
                new Vector3(0.64f, 1.62f, -2.38f),
                new Vector3(0.00f, 0.06f, 0.08f),
                34f,
                outputDirectory,
                fileName);
            shotRows.Add($"| `{fileName}` | {label} | MistBase | {simulateSeconds:0.###} | false | {emitter.RippleParticleCountForReview} | {emitter.SplashParticleCountForReview} | {emitter.MistParticleCountForReview} | {emitter.AppliedMistEmissionRateForReview:0.###} |");
        }

        private static void CaptureHd2dAutonomousP2WaterSplashRippleCloseShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            Vector3 playerLocalPosition,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            string outputDirectory,
            string fileName)
        {
            var previousFov = camera.fieldOfView;
            var previousNear = camera.nearClipPlane;
            var previousFar = camera.farClipPlane;
            var previousMask = camera.cullingMask;
            var player = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            var previousPlayerLocal = Vector3.zero;
            var hasPlayer = player != null && controller.CurrentSpaceRootForReview != null;
            if (hasPlayer)
            {
                previousPlayerLocal = controller.CurrentSpaceRootForReview.InverseTransformPoint(player.transform.position);
            }

            try
            {
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ForcePlayerOtherTimeLocalForReview(playerLocalPosition);
                guide.ApplyActiveTimeIsolationForReview();

                var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
                var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
                var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
                camera.cullingMask = ((previousMask & ~currentBit) | otherBit) & ~playerBit;
                camera.fieldOfView = fieldOfView;
                camera.nearClipPlane = 0.03f;
                camera.farClipPlane = 170f;
                PositionCloseReviewCamera(
                    camera,
                    controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition),
                    cameraOffset,
                    lookOffset);

                FindHd2dAutonomousP2WaterSplashRippleEmitter()?.RefreshDistanceCullForReview();
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
                LogHd2dAutonomousP2WaterSplashRippleCaptureDiagnostics(camera, fileName);
            }
            finally
            {
                camera.cullingMask = previousMask;
                camera.fieldOfView = previousFov;
                camera.nearClipPlane = previousNear;
                camera.farClipPlane = previousFar;
                if (hasPlayer)
                {
                    controller.ForcePlayerCurrentLocalForReview(previousPlayerLocal);
                }

                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
        }

        private static void LogHd2dAutonomousP2WaterSplashRippleCaptureDiagnostics(Camera camera, string fileName)
        {
            var emitter = FindHd2dAutonomousP2WaterSplashRippleEmitter();
            if (emitter == null || camera == null)
            {
                return;
            }

            LogParticleRenderer("ripple", emitter.RippleRendererForReview);
            LogParticleRenderer("splash", emitter.SplashRendererForReview);
            LogParticleRenderer("mist", emitter.MistRendererForReview);

            void LogParticleRenderer(string label, ParticleSystemRenderer renderer)
            {
                if (renderer == null)
                {
                    Debug.Log($"P2-69 capture {fileName}: {label} renderer missing.");
                    return;
                }

                var bounds = renderer.bounds;
                var screen = camera.WorldToScreenPoint(bounds.center);
                Debug.Log(
                    $"P2-69 capture {fileName}: {label} enabled={renderer.enabled}, forceOff={renderer.forceRenderingOff}, layer={renderer.gameObject.layer}, material={(renderer.sharedMaterial != null ? renderer.sharedMaterial.name : "null")}, boundsCenter={FormatVector3ForReport(bounds.center)}, boundsSize={FormatVector3ForReport(bounds.size)}, screen={FormatVector3ForReport(screen)}.");
            }
        }

        private static void SetHd2dAutonomousP2WaterSplashRippleVisible(bool visible)
        {
            var emitter = FindHd2dAutonomousP2WaterSplashRippleEmitter();
            if (emitter == null)
            {
                return;
            }

            emitter.SetReviewVisibleForReview(visible);
            EditorUtility.SetDirty(emitter);
        }

        private static void SetHd2dAutonomousP2WaterSplashRippleMultipliers(float splashRippleMultiplier, float mistMultiplier, float alphaMultiplier)
        {
            var emitter = FindHd2dAutonomousP2WaterSplashRippleEmitter();
            if (emitter == null)
            {
                return;
            }

            emitter.SetReviewMultipliersForReview(splashRippleMultiplier, mistMultiplier, alphaMultiplier);
            EditorUtility.SetDirty(emitter);
        }

        private static FastVsHd2dWaterSplashRippleEmitter FindHd2dAutonomousP2WaterSplashRippleEmitter()
        {
            var emitters = UnityEngine.Object.FindObjectsByType<FastVsHd2dWaterSplashRippleEmitter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var emitter in emitters)
            {
                if (emitter != null && emitter.name == Hd2dAutonomousP2WaterSplashRippleEmitterName)
                {
                    return emitter;
                }
            }

            return emitters.Length > 0 ? emitters[0] : null;
        }

        private static int CountHd2dAutonomousP2WaterSplashRippleReviewRenderers(string token)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WaterSplashRippleRootName);
            if (root == null)
            {
                return 0;
            }

            var count = 0;
            foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer != null && renderer.name.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    count++;
                }
            }

            return count;
        }

        private static void SetHd2dAutonomousP2WaterSplashRippleLayerRecursively(GameObject root, int layer)
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
                    SetHd2dAutonomousP2WaterSplashRippleLayerRecursively(child.gameObject, layer);
                }
            }
        }

        private static void WriteHd2dAutonomousP2WaterSplashRippleReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dWaterSplashRippleProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics entryEnableDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics entryMotionDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics entryFadeDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics mistEnableDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics strongerDiff)
        {
            var emitter = FindHd2dAutonomousP2WaterSplashRippleEmitter();
            var lines = new List<string>
            {
                "# P2-69 Water Splashes, Ripples, And Waterfall Mist Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative water FX data prep: one-shot expanding ripple, Birth sub-emitter droplet crown, and low-alpha waterfall/fountain-base mist.",
                "- Review note: the small past-plaza basin/ledge is a diagnostic surface only. Final water-body placement, ring thickness, mist density, and lighting response remain Tom-facing decisions.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2WaterSplashRippleProfilePath}` |",
                $"| Runtime emitter | `{Hd2dAutonomousP2WaterSplashRippleEmitterRuntimePath}` |",
                $"| Ring material / texture | `{Hd2dAutonomousP2WaterSplashRippleRingMaterialPath}` / `{Hd2dAutonomousP2WaterSplashRippleRingTexturePath}` |",
                $"| Droplet material / texture | `{Hd2dAutonomousP2WaterSplashRippleDropletMaterialPath}` / `{Hd2dAutonomousP2WaterSplashRippleDropletTexturePath}` |",
                $"| Mist material / texture | `{Hd2dAutonomousP2WaterSplashRippleMistMaterialPath}` / `{Hd2dAutonomousP2WaterSplashRippleMistTexturePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalWaterSplashRippleApprovedForReview)} |",
                $"| Birth sub-emitter configured | {FormatBool(emitter != null && emitter.SubEmitterBirthConfiguredForReview)} |",
                $"| Raycast entry hook prepared | {FormatBool(profile.RaycastEntryHookPreparedForReview)} |",
                $"| Ripple lifetime / size start/end | {profile.RippleLifetimeForReview:0.###}s / {profile.RippleStartSizeForReview:0.###}-{profile.RippleEndSizeForReview:0.###} |",
                $"| Splash burst / lifetime | {profile.SplashBurstParticlesForReview} particles / {profile.SplashLifetimeForReview:0.###}s |",
                $"| Splash horizontal / up min/max / gravity | {profile.SplashHorizontalVelocityForReview:0.###} / {profile.SplashUpVelocityMinForReview:0.###}-{profile.SplashUpVelocityMaxForReview:0.###} / {profile.SplashGravityForReview:0.###} |",
                $"| Mist emission / expected particles / max | {profile.MistEmissionRateForReview:0.###} / {profile.ExpectedMistParticleCountForReview} / {profile.MaxMistParticlesForReview} |",
                $"| Mist lifetime / rise / outward / alpha | {profile.MistLifetimeForReview:0.###}s / {profile.MistRiseVelocityForReview:0.###} / {profile.MistOutwardVelocityForReview:0.###} / {profile.MistPeakAlphaForReview:0.###} |",
                $"| Soft-particle far fade | {profile.SoftParticleFarFadeForReview:0.###}m |",
                $"| Stronger option multiplier | {profile.StrongerOptionMultiplierForReview:0.###} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                entryEnableDiff.ToReportRow("Entry disabled vs ripple+splash frame A"),
                entryMotionDiff.ToReportRow("Ripple frame A vs expanded frame B"),
                entryFadeDiff.ToReportRow("Ripple frame A vs after-lifetime fade"),
                mistEnableDiff.ToReportRow("Mist disabled vs conservative mist"),
                strongerDiff.ToReportRow("Conservative mist vs stronger option"),
                string.Empty,
                "| Screenshot | Label | Mode | Sim seconds | Triggered | Ripple particles | Splash particles | Mist particles | Mist rate |",
                "|---|---|---|---:|---|---:|---:|---:|---:|"
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
                lines.Add($"| `{file}` | P2-69 water splash/ripple/mist capture {i + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "water_splashes_ripples_waterfall_mist_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }
    }
}
