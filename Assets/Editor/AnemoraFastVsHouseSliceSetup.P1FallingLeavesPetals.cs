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
        private const string Hd2dAutonomousP1FallingLeavesRootName = "FastVS_HD2D_P1_FallingLeavesPetalsLayer";
        private const string Hd2dAutonomousP1FallingLeavesForegroundName = "FastVS_HD2D_P1_FallingLeaves_ForegroundBokeh";
        private const string Hd2dAutonomousP1FallingLeavesMidDepthName = "FastVS_HD2D_P1_FallingLeaves_MidDepthSharp";
        private const string Hd2dAutonomousP1FallingLeavesProfilePath = "Assets/Settings/FastVS_HD2D_P1_FallingLeavesProfile.asset";
        private const string Hd2dAutonomousP1FallingLeavesMaterialId = "hd2d_p1_falling_green_leaf";
        private const string Hd2dAutonomousP1FallingPetalsMaterialId = "hd2d_p1_falling_pink_petal";
        private const string Hd2dAutonomousP1FallingLeavesTextureId = "hd2d_p1_falling_green_leaf";
        private const string Hd2dAutonomousP1FallingPetalsTextureId = "hd2d_p1_falling_pink_petal";

        public static void CaptureHd2dAutonomousP1Item47FallingLeavesPetalsBatch()
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
            var layerRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1FallingLeavesRootName);
            var layer = layerRoot != null ? layerRoot.GetComponent<FastVsHd2dFallingLeavesLayer>() : null;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || layer == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P1-47 falling leaves/petals capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1FallingLeavesPetals();
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dFallingLeavesProfile>(Hd2dAutonomousP1FallingLeavesProfilePath);
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("falling_leaves_petals");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_overlay_off_baseline.png",
                "02_green_leaf_drift_overlay.png",
                "03_leaf_drift_frame_a.png",
                "04_leaf_drift_frame_b_1s.png",
                "05_pink_petal_zone_swap.png",
                "06_foreground_bokeh_depth_mix.png"
            };
            var shotRows = new List<string>();
            var captureMultiplier = profile != null ? profile.ReviewCaptureEmissionMultiplierForReview : 1.45f;
            var warmupSeconds = profile != null ? profile.LifetimeForReview : 7.8f;
            try
            {
                guide.SetMovementFrozen(true);
                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();

                CaptureHd2dAutonomousP1FallingLeavesReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHd2dFallingLeavesBiome.GreenLeaf,
                    false,
                    0f,
                    CentralPlazaVsCenter + new Vector3(0.08f, 0.02f, 3.2f),
                    new Vector3(0.20f, 7.8f, -9.7f),
                    new Vector3(0.00f, 1.18f, 0.15f),
                    36f,
                    0f,
                    true,
                    outputDirectory,
                    screenshotFiles[0],
                    "overlay disabled baseline",
                    shotRows);

                CaptureHd2dAutonomousP1FallingLeavesReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHd2dFallingLeavesBiome.GreenLeaf,
                    true,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(0.08f, 0.02f, 3.2f),
                    new Vector3(0.20f, 7.8f, -9.7f),
                    new Vector3(0.00f, 1.18f, 0.15f),
                    36f,
                    warmupSeconds,
                    true,
                    outputDirectory,
                    screenshotFiles[1],
                    "green leaf biome with sparse drift overlay",
                    shotRows);

                CaptureHd2dAutonomousP1FallingLeavesReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHd2dFallingLeavesBiome.GreenLeaf,
                    true,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(0.08f, 0.02f, 3.2f),
                    new Vector3(0.20f, 7.8f, -9.7f),
                    new Vector3(0.00f, 1.18f, 0.15f),
                    36f,
                    0f,
                    false,
                    outputDirectory,
                    screenshotFiles[2],
                    "leaf drift frame A after seeded prewarm",
                    shotRows);

                CaptureHd2dAutonomousP1FallingLeavesReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHd2dFallingLeavesBiome.GreenLeaf,
                    true,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(0.08f, 0.02f, 3.2f),
                    new Vector3(0.20f, 7.8f, -9.7f),
                    new Vector3(0.00f, 1.18f, 0.15f),
                    36f,
                    1.05f,
                    false,
                    outputDirectory,
                    screenshotFiles[3],
                    "leaf drift frame B after 1.05 seconds without restart",
                    shotRows);

                CaptureHd2dAutonomousP1FallingLeavesReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHd2dFallingLeavesBiome.PinkPetal,
                    true,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(0.08f, 0.02f, 3.2f),
                    new Vector3(0.20f, 7.8f, -9.7f),
                    new Vector3(0.00f, 1.18f, 0.15f),
                    36f,
                    warmupSeconds,
                    true,
                    outputDirectory,
                    screenshotFiles[4],
                    "pink petal biome swap using the same runtime layer",
                    shotRows);

                CaptureHd2dAutonomousP1FallingLeavesReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHd2dFallingLeavesBiome.GreenLeaf,
                    true,
                    captureMultiplier,
                    CentralPlazaVsCenter + new Vector3(-0.16f, 0.02f, 2.55f),
                    new Vector3(0.12f, 5.9f, -7.6f),
                    new Vector3(0.00f, 1.10f, 0.08f),
                    32f,
                    warmupSeconds,
                    true,
                    outputDirectory,
                    screenshotFiles[5],
                    "closer frame showing foreground and mid-depth leaf bands",
                    shotRows);

                var offOnMetrics = ValidateHd2dAutonomousP1FallingLeavesReviewPairDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1], "overlay-off-vs-green-leaf");
                var driftMetrics = ValidateHd2dAutonomousP1FallingLeavesReviewPairDiff(outputDirectory, screenshotFiles[2], screenshotFiles[3], "leaf-drift-a-vs-b");
                var biomeMetrics = ValidateHd2dAutonomousP1FallingLeavesReviewPairDiff(outputDirectory, screenshotFiles[1], screenshotFiles[4], "green-leaf-vs-pink-petal");
                WriteHd2dAutonomousP1FallingLeavesReviewReport(outputDirectory, screenshotFiles, shotRows, offOnMetrics, driftMetrics, biomeMetrics);
            }
            finally
            {
                layer.ClearReviewOverrideForReview();
                if (profile != null)
                {
                    layer.SimulateForReview(profile.LifetimeForReview, true);
                }
            }

            Debug.Log($"Fast VS autonomous P1-47 falling leaves/petals review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1FallingLeavesPetals(Camera camera)
        {
            if (camera == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP1FallingLeavesProfileAsset();
            var leafMaterial = profile.GreenLeafMaterialForReview != null ? profile.GreenLeafMaterialForReview : EnsureHd2dAutonomousP1FallingLeavesMaterial(FastVsHd2dFallingLeavesBiome.GreenLeaf, profile.GreenLeafTintForReview);
            var petalMaterial = profile.PinkPetalMaterialForReview != null ? profile.PinkPetalMaterialForReview : EnsureHd2dAutonomousP1FallingLeavesMaterial(FastVsHd2dFallingLeavesBiome.PinkPetal, profile.PinkPetalTintForReview);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1FallingLeavesRootName);
            if (root == null)
            {
                root = new GameObject(Hd2dAutonomousP1FallingLeavesRootName, typeof(FastVsHd2dFallingLeavesLayer));
            }
            else if (root.GetComponent<FastVsHd2dFallingLeavesLayer>() == null)
            {
                root.AddComponent<FastVsHd2dFallingLeavesLayer>();
            }

            root.transform.SetParent(camera.transform, false);
            root.transform.localPosition = profile.CameraLocalOffsetForReview;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.SetActive(true);

            var foregroundSystem = EnsureHd2dAutonomousP1FallingLeavesSystem(
                root.transform,
                Hd2dAutonomousP1FallingLeavesForegroundName,
                profile.ForegroundLocalCenterForReview,
                profile.ForegroundBoxSizeForReview,
                profile.ForegroundMaxParticlesForReview,
                profile,
                leafMaterial,
                47);
            var midDepthSystem = EnsureHd2dAutonomousP1FallingLeavesSystem(
                root.transform,
                Hd2dAutonomousP1FallingLeavesMidDepthName,
                profile.MidDepthLocalCenterForReview,
                profile.MidDepthBoxSizeForReview,
                profile.MidDepthMaxParticlesForReview,
                profile,
                leafMaterial,
                147);

            var layer = root.GetComponent<FastVsHd2dFallingLeavesLayer>();
            layer.ConfigureForReview(
                profile,
                camera,
                foregroundSystem,
                midDepthSystem,
                foregroundSystem.GetComponent<ParticleSystemRenderer>(),
                midDepthSystem.GetComponent<ParticleSystemRenderer>(),
                profile.CameraAttachedForReview,
                profile.BiomeSwappableForReview,
                profile.ForegroundBokehLayerForReview,
                profile.ConservativeReviewModeForReview,
                profile.RequiresTomArtApprovalForReview);
            layer.SimulateForReview(profile.LifetimeForReview, true);

            EditorUtility.SetDirty(root);
        }

        private static void ValidateHd2dAutonomousP1FallingLeavesPetals()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dFallingLeavesProfile>(Hd2dAutonomousP1FallingLeavesProfilePath);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1FallingLeavesRootName);
            var camera = Camera.main;
            if (profile == null || root == null || camera == null)
            {
                throw new InvalidOperationException("House slice validation failed: P1-47 needs a falling leaves profile, camera root, and main camera.");
            }

            var layer = root.GetComponent<FastVsHd2dFallingLeavesLayer>();
            var systems = root.GetComponentsInChildren<ParticleSystem>(true);
            var renderers = root.GetComponentsInChildren<ParticleSystemRenderer>(true);
            var leafMaterial = AssetDatabase.LoadAssetAtPath<Material>(GetHd2dAutonomousP1FallingLeavesMaterialPath(FastVsHd2dFallingLeavesBiome.GreenLeaf));
            var petalMaterial = AssetDatabase.LoadAssetAtPath<Material>(GetHd2dAutonomousP1FallingLeavesMaterialPath(FastVsHd2dFallingLeavesBiome.PinkPetal));
            var leafTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetHd2dAutonomousP1FallingLeavesTexturePath(FastVsHd2dFallingLeavesBiome.GreenLeaf));
            var petalTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(GetHd2dAutonomousP1FallingLeavesTexturePath(FastVsHd2dFallingLeavesBiome.PinkPetal));
            if (layer == null ||
                systems.Length != 2 ||
                renderers.Length != 2 ||
                leafMaterial == null ||
                petalMaterial == null ||
                leafTexture == null ||
                petalTexture == null ||
                !root.transform.IsChildOf(camera.transform) ||
                !layer.CameraAttachedForReview ||
                !layer.BiomeSwappableForReview ||
                !layer.ForegroundBokehLayerForReview ||
                !profile.CpuShurikenForReview ||
                !profile.SimulationSpaceWorldForReview ||
                !layer.UsesWorldSimulationForReview ||
                !layer.BoxEmissionForReview ||
                !layer.NoiseSwayForReview ||
                !layer.VelocityFallForReview ||
                !layer.RotationTumbleForReview ||
                !layer.ShadowlessForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P1-47 must be a two-band camera-attached CPU Shuriken leaf/petal overlay with biome-swappable materials, falling velocity, tumble, noise, and no shadows.");
            }

            if (profile.TotalMaxParticlesForReview < 5 ||
                profile.TotalMaxParticlesForReview > 15 ||
                layer.TotalMaxParticlesForReview != profile.TotalMaxParticlesForReview ||
                profile.EmissionRateForReview <= 0f ||
                profile.LocalFallVelocityForReview.y >= -0.05f ||
                profile.NoiseStrengthForReview <= 0f ||
                profile.RotationOverLifetimeRadiansForReview <= 0f ||
                profile.RotationBySpeedRadiansForReview <= 0f ||
                !profile.HasBothBiomeSpriteRecordsForReview ||
                profile.BiomeRecordCountForReview != 2 ||
                !profile.ConservativeReviewModeForReview ||
                !profile.RequiresTomArtApprovalForReview ||
                !layer.ConservativeReviewModeForReview ||
                !layer.RequiresTomArtApprovalForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P1-47 profile must stay sparse, falling, tumbling, noisy, conservative, and TOM-facing.");
            }

            foreach (var material in new[] { leafMaterial, petalMaterial })
            {
                if (!string.Equals(material.GetTag(MaterialRoleTagName, false, string.Empty), FastVsHd2dMaterialRole.SpriteCard.ToString(), StringComparison.Ordinal) ||
                    material.renderQueue < 3000)
                {
                    throw new InvalidOperationException("House slice validation failed: P1-47 leaf/petal materials must stay transparent SpriteCard particle materials.");
                }
            }

            var editorSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.P1FallingLeavesPetals.cs");
            var runtimeProfilePath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dFallingLeavesProfile.cs");
            var runtimeLayerPath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dFallingLeavesLayer.cs");
            var mainSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.cs");
            foreach (var token in new[]
            {
                "CaptureHd2dAutonomousP1Item47FallingLeavesPetalsBatch",
                "falling_leaves_petals",
                "FastVsHd2dFallingLeavesBiome.PinkPetal",
                "ValidateHd2dAutonomousP1FallingLeavesPetals"
            })
            {
                ValidateSourceToken(File.ReadAllText(editorSourcePath), token, editorSourcePath);
            }

            ValidateSourceToken(File.ReadAllText(runtimeProfilePath), "BiomeSwappableForReview", runtimeProfilePath);
            ValidateSourceToken(File.ReadAllText(runtimeLayerPath), "RotationBySpeedForReview", runtimeLayerPath);
            ValidateSourceToken(File.ReadAllText(mainSourcePath), "CreateHd2dAutonomousP1FallingLeavesPetals", mainSourcePath);
            ValidateSourceToken(File.ReadAllText(mainSourcePath), "ValidateHd2dAutonomousP1FallingLeavesPetals", mainSourcePath);
        }

        private static FastVsHd2dFallingLeavesProfile EnsureHd2dAutonomousP1FallingLeavesProfileAsset()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dFallingLeavesProfile>(Hd2dAutonomousP1FallingLeavesProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dFallingLeavesProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP1FallingLeavesProfilePath);
            }

            var greenLeafTexture = EnsureHd2dAutonomousP1FallingLeavesTexture(FastVsHd2dFallingLeavesBiome.GreenLeaf);
            var pinkPetalTexture = EnsureHd2dAutonomousP1FallingLeavesTexture(FastVsHd2dFallingLeavesBiome.PinkPetal);
            var greenLeafMaterial = EnsureHd2dAutonomousP1FallingLeavesMaterial(FastVsHd2dFallingLeavesBiome.GreenLeaf, new Color(0.84f, 1.00f, 0.46f, 0.88f));
            var pinkPetalMaterial = EnsureHd2dAutonomousP1FallingLeavesMaterial(FastVsHd2dFallingLeavesBiome.PinkPetal, new Color(1.00f, 0.56f, 0.82f, 0.86f));
            profile.ConfigureForReview(
                15,
                5,
                10,
                9.5f,
                7.8f,
                1.75f,
                1.65f,
                Vector3.zero,
                new Vector3(0f, 1.12f, 2.75f),
                new Vector3(0f, 1.62f, 6.85f),
                new Vector3(7.4f, 1.7f, 1.4f),
                new Vector3(10.8f, 2.4f, 3.0f),
                new Vector3(0.10f, -0.46f, -0.03f),
                0.018f,
                0.28f,
                0.22f,
                0.20f,
                2.8f,
                1.45f,
                new Color(0.84f, 1.00f, 0.46f, 0.88f),
                new Color(1.00f, 0.56f, 0.82f, 0.86f),
                "green_leaf",
                "pink_petal",
                greenLeafMaterial,
                pinkPetalMaterial,
                greenLeafTexture,
                pinkPetalTexture,
                FastVsHd2dFallingLeavesBiome.GreenLeaf,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                "Procedural CC0-safe leaf/petal sprites and CPU Shuriken camera overlay; Tom should tune final sprite art, density, wind, bokeh placement, and biome mapping. Pass2 raises readability only while keeping the 15-particle cap.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static ParticleSystem EnsureHd2dAutonomousP1FallingLeavesSystem(
            Transform parent,
            string objectName,
            Vector3 localPosition,
            Vector3 boxSize,
            int maxParticles,
            FastVsHd2dFallingLeavesProfile profile,
            Material material,
            uint randomSeed)
        {
            var child = parent.Find(objectName);
            var systemObject = child != null ? child.gameObject : null;
            if (systemObject == null)
            {
                systemObject = new GameObject(objectName, typeof(ParticleSystem), typeof(ParticleSystemRenderer));
                systemObject.transform.SetParent(parent, false);
            }

            systemObject.transform.localPosition = localPosition;
            systemObject.transform.localRotation = Quaternion.identity;
            systemObject.transform.localScale = Vector3.one;
            systemObject.SetActive(true);

            var system = systemObject.GetComponent<ParticleSystem>();
            if (system == null)
            {
                system = systemObject.AddComponent<ParticleSystem>();
            }

            system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            system.useAutoRandomSeed = false;
            system.randomSeed = randomSeed;

            var main = system.main;
            main.loop = true;
            main.prewarm = true;
            main.playOnAwake = true;
            main.duration = profile.DurationForReview;
            main.startLifetime = new ParticleSystem.MinMaxCurve(profile.LifetimeForReview * 0.72f, profile.LifetimeForReview);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.018f, 0.065f);
            main.startSize = objectName == Hd2dAutonomousP1FallingLeavesForegroundName
                ? new ParticleSystem.MinMaxCurve(0.30f, 0.50f)
                : new ParticleSystem.MinMaxCurve(0.16f, 0.30f);
            main.startRotation = new ParticleSystem.MinMaxCurve(-Mathf.PI, Mathf.PI);
            main.startColor = new ParticleSystem.MinMaxGradient(profile.GreenLeafTintForReview);
            main.maxParticles = maxParticles;
            main.gravityModifier = profile.GravityModifierForReview;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(profile.EmissionRateForReview * 0.5f);

            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = boxSize;
            shape.position = Vector3.zero;
            shape.rotation = Vector3.zero;
            shape.randomDirectionAmount = profile.RandomDirectionAmountForReview;

            var velocity = system.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            velocity.x = new ParticleSystem.MinMaxCurve(profile.LocalFallVelocityForReview.x * 0.25f, profile.LocalFallVelocityForReview.x * 1.5f);
            velocity.y = new ParticleSystem.MinMaxCurve(profile.LocalFallVelocityForReview.y * 1.25f, profile.LocalFallVelocityForReview.y * 0.70f);
            velocity.z = new ParticleSystem.MinMaxCurve(profile.LocalFallVelocityForReview.z - 0.04f, profile.LocalFallVelocityForReview.z + 0.04f);

            var noise = system.noise;
            noise.enabled = true;
            noise.strength = new ParticleSystem.MinMaxCurve(profile.NoiseStrengthForReview);
            noise.frequency = profile.NoiseFrequencyForReview;
            noise.damping = true;
            noise.quality = ParticleSystemNoiseQuality.Medium;

            var rotationOverLifetime = system.rotationOverLifetime;
            rotationOverLifetime.enabled = true;
            rotationOverLifetime.separateAxes = false;
            rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-profile.RotationOverLifetimeRadiansForReview, profile.RotationOverLifetimeRadiansForReview);

            var rotationBySpeed = system.rotationBySpeed;
            rotationBySpeed.enabled = true;
            rotationBySpeed.separateAxes = false;
            rotationBySpeed.z = new ParticleSystem.MinMaxCurve(-profile.RotationBySpeedRadiansForReview, profile.RotationBySpeedRadiansForReview);
            rotationBySpeed.range = new Vector2(0f, 1.2f);

            var renderer = systemObject.GetComponent<ParticleSystemRenderer>();
            if (renderer == null)
            {
                renderer = systemObject.AddComponent<ParticleSystemRenderer>();
            }

            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.sortingOrder = objectName == Hd2dAutonomousP1FallingLeavesForegroundName ? 5 : 3;

            EditorUtility.SetDirty(systemObject);
            EditorUtility.SetDirty(system);
            EditorUtility.SetDirty(renderer);
            return system;
        }

        private static Material EnsureHd2dAutonomousP1FallingLeavesMaterial(FastVsHd2dFallingLeavesBiome biome, Color tint)
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            var id = biome == FastVsHd2dFallingLeavesBiome.PinkPetal ? Hd2dAutonomousP1FallingPetalsMaterialId : Hd2dAutonomousP1FallingLeavesMaterialId;
            var material = FlatMaterial(id, Color.white, true, FastVsHd2dMaterialRole.SpriteCard);
            ConfigureTransparentParticleMaterial(material, 3012);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP1FallingLeavesTexture(biome), Vector2.one);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", tint);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", tint);
            }

            material.enableInstancing = true;
            ApplyMaterialRole(material, id, FastVsHd2dMaterialRole.SpriteCard);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP1FallingLeavesTexture(FastVsHd2dFallingLeavesBiome biome)
        {
            var id = biome == FastVsHd2dFallingLeavesBiome.PinkPetal ? Hd2dAutonomousP1FallingPetalsTextureId : Hd2dAutonomousP1FallingLeavesTextureId;
            return EnsureGeneratedTexture(
                id,
                64,
                64,
                FilterMode.Bilinear,
                (x, y) => biome == FastVsHd2dFallingLeavesBiome.PinkPetal
                    ? SampleHd2dAutonomousP1FallingPetalPixel(x, y)
                    : SampleHd2dAutonomousP1FallingLeafPixel(x, y));
        }

        private static Color SampleHd2dAutonomousP1FallingLeafPixel(int x, int y)
        {
            var u = (x + 0.5f) / 64f * 2f - 1f;
            var v = (y + 0.5f) / 64f * 2f - 1f;
            var taper = Mathf.Clamp01(1f - Mathf.Abs(u) * 0.86f);
            var body = Mathf.Clamp01(1f - Mathf.Abs(v) / Mathf.Max(0.04f, taper * 0.42f));
            var edge = Mathf.Clamp01(1f - Mathf.Abs(u) * 1.05f);
            var vein = Mathf.Abs(v) < 0.035f && Mathf.Abs(u) < 0.86f ? 0.18f : 0f;
            var alpha = Mathf.Clamp01(body * edge);
            return new Color(0.86f + vein, 0.92f + vein * 0.3f, 0.48f, alpha);
        }

        private static Color SampleHd2dAutonomousP1FallingPetalPixel(int x, int y)
        {
            var u = (x + 0.5f) / 64f * 2f - 1f;
            var v = (y + 0.5f) / 64f * 2f - 1f;
            var curve = Mathf.Abs(v + 0.18f * u * u);
            var body = Mathf.Clamp01(1f - curve / 0.46f);
            var side = Mathf.Clamp01(1f - Mathf.Abs(u) / 0.72f);
            var notch = Mathf.Clamp01((Mathf.Abs(u) - 0.08f) * 8f + Mathf.Max(0f, v - 0.34f) * 5f);
            var alpha = Mathf.Clamp01(body * side * (1f - notch * 0.32f));
            return new Color(1.0f, 0.68f, 0.86f, alpha);
        }

        private static void CaptureHd2dAutonomousP1FallingLeavesReviewShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            FastVsHd2dFallingLeavesLayer layer,
            FastVsHd2dFallingLeavesBiome biome,
            bool enabled,
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
            controller.ForcePlayerCurrentLocalForReview(anchorLocalPosition + new Vector3(0f, 0.02f, -0.8f));
            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            layer.SetReviewBiomeForReview(biome);
            layer.SetReviewOverrideForReview(enabled, emissionMultiplier);
            layer.SimulateForReview(Mathf.Max(0f, simulateSeconds), restart);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {biome} | {FormatBool(enabled)} | {FormatVector3ForReport(anchorLocalPosition)} | {FormatVector3ForReport(cameraOffset)} | {fieldOfView:0.#} | {emissionMultiplier:0.##} | {layer.AppliedEmissionRateForReview:0.###} | {layer.ForegroundLiveParticlesForReview}/{layer.MidDepthLiveParticlesForReview}/{layer.TotalLiveParticlesForReview} |");
        }

        private static void WriteHd2dAutonomousP1FallingLeavesReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            Hd2dAutonomousP1FallingLeavesDiffMetrics offOnMetrics,
            Hd2dAutonomousP1FallingLeavesDiffMetrics driftMetrics,
            Hd2dAutonomousP1FallingLeavesDiffMetrics biomeMetrics)
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dFallingLeavesProfile>(Hd2dAutonomousP1FallingLeavesProfilePath);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1FallingLeavesRootName);
            var layer = root != null ? root.GetComponent<FastVsHd2dFallingLeavesLayer>() : null;
            var lines = new List<string>
            {
                "# P1-47 Falling Leaves / Petals Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative camera-attached CPU Shuriken leaf/petal drift overlay with two depth bands and biome-swappable generated sprites.",
                "- Recommendation: keep the camera-parented two-band setup, sparse count, tumble/noise modules, and data-driven material swap; Tom should approve final sprite art, density, wind, bokeh placement, and biome mapping.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP1FallingLeavesProfilePath}` |",
                $"| Root | `{Hd2dAutonomousP1FallingLeavesRootName}` |",
                $"| Particle systems / max particles / live particles | {layer?.ParticleSystemCountForReview ?? 0} / {layer?.TotalMaxParticlesForReview ?? 0} / {layer?.TotalLiveParticlesForReview ?? 0} |",
                $"| Emission / capture multiplier | {profile?.EmissionRateForReview ?? 0f:0.###} / {profile?.ReviewCaptureEmissionMultiplierForReview ?? 0f:0.###} |",
                $"| Foreground center / box | {FormatVector3ForReport(profile != null ? profile.ForegroundLocalCenterForReview : Vector3.zero)} / {FormatVector3ForReport(profile != null ? profile.ForegroundBoxSizeForReview : Vector3.zero)} |",
                $"| Mid-depth center / box | {FormatVector3ForReport(profile != null ? profile.MidDepthLocalCenterForReview : Vector3.zero)} / {FormatVector3ForReport(profile != null ? profile.MidDepthBoxSizeForReview : Vector3.zero)} |",
                $"| Local fall velocity | {FormatVector3ForReport(profile != null ? profile.LocalFallVelocityForReview : Vector3.zero)} |",
                $"| Noise strength / frequency | {profile?.NoiseStrengthForReview ?? 0f:0.###} / {profile?.NoiseFrequencyForReview ?? 0f:0.###} |",
                $"| Rotation lifetime / by speed | {profile?.RotationOverLifetimeRadiansForReview ?? 0f:0.###} / {profile?.RotationBySpeedRadiansForReview ?? 0f:0.###} |",
                $"| Camera attached / biome swappable / foreground bokeh | {FormatBool(layer != null && layer.CameraAttachedForReview)} / {FormatBool(layer != null && layer.BiomeSwappableForReview)} / {FormatBool(layer != null && layer.ForegroundBokehLayerForReview)} |",
                $"| Requires TOM art approval | {FormatBool(profile != null && profile.RequiresTomArtApprovalForReview)} |",
                $"| Source note | {profile?.SourceNoteForReview ?? "missing"} |",
                string.Empty,
                "| Comparison | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                offOnMetrics.ToReportRow("overlay off vs green leaf"),
                driftMetrics.ToReportRow("leaf drift frame A vs B"),
                biomeMetrics.ToReportRow("green leaf vs pink petal"),
                string.Empty,
                "| Screenshot | Label | Biome | Enabled | Anchor | Offset | FOV | Emission x | Applied rate | Fore/Mid/Total live |",
                "|---|---|---|---|---|---|---:|---:|---:|---:|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Overlay-off control |",
                $"| `{screenshotFiles[1]}` | Green leaf drift overlay with foreground and mid-depth bands |",
                $"| `{screenshotFiles[2]}` | Leaf drift frame A |",
                $"| `{screenshotFiles[3]}` | Leaf drift frame B after 1.05 seconds |",
                $"| `{screenshotFiles[4]}` | Pink petal biome/material swap without code change |",
                $"| `{screenshotFiles[5]}` | Closer depth-mix check for foreground bokeh readiness |"
            });

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllText(Path.Combine(outputDirectory, "falling_leaves_petals_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP1FallingLeavesDiffMetrics ValidateHd2dAutonomousP1FallingLeavesReviewPairDiff(string outputDirectory, string firstFile, string secondFile, string label)
        {
            var metrics = MeasureHd2dAutonomousP1FallingLeavesDiff(Path.Combine(outputDirectory, firstFile), Path.Combine(outputDirectory, secondFile), 4);
            if (metrics.SampleCount <= 0 || metrics.ChangedPixels <= 0)
            {
                throw new InvalidOperationException($"Fast VS autonomous P1-47 falling leaves/petals capture failed: {label} images have no measurable overlay/drift/biome difference.");
            }

            return metrics;
        }

        private static Hd2dAutonomousP1FallingLeavesDiffMetrics MeasureHd2dAutonomousP1FallingLeavesDiff(string firstPath, string secondPath, int threshold)
        {
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP1FallingLeavesDiffMetrics(0, 0, 0f, 0f);
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
                return new Hd2dAutonomousP1FallingLeavesDiffMetrics(sampleCount, changedPixels, changedPercent, meanDelta);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(firstTexture);
                UnityEngine.Object.DestroyImmediate(secondTexture);
            }
        }

        private static string GetHd2dAutonomousP1FallingLeavesMaterialPath(FastVsHd2dFallingLeavesBiome biome)
        {
            var id = biome == FastVsHd2dFallingLeavesBiome.PinkPetal ? Hd2dAutonomousP1FallingPetalsMaterialId : Hd2dAutonomousP1FallingLeavesMaterialId;
            return MaterialDirectory + "/FastVS_House_" + id + ".mat";
        }

        private static string GetHd2dAutonomousP1FallingLeavesTexturePath(FastVsHd2dFallingLeavesBiome biome)
        {
            var id = biome == FastVsHd2dFallingLeavesBiome.PinkPetal ? Hd2dAutonomousP1FallingPetalsTextureId : Hd2dAutonomousP1FallingLeavesTextureId;
            return TextureDirectory + "/FastVS_House_" + id + ".asset";
        }

        private readonly struct Hd2dAutonomousP1FallingLeavesDiffMetrics
        {
            public readonly int SampleCount;
            public readonly int ChangedPixels;
            public readonly float ChangedPercent;
            public readonly float MeanRgbDelta;

            public Hd2dAutonomousP1FallingLeavesDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
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
}
