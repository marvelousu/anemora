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
        private const string Hd2dAutonomousP2FootstepFxProfilePath = "Assets/Settings/FastVS_HD2D_P2_FootstepFxProfile.asset";
        private const string Hd2dAutonomousP2FootstepFxProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dFootstepFxProfile.cs";
        private const string Hd2dAutonomousP2FootstepFxEmitterRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dFootstepFxEmitter.cs";
        private const string Hd2dAutonomousP2FootstepFxSystemName = "FastVS_HD2D_P2_FootstepScuffPuff_PooledShuriken";
        private const string Hd2dAutonomousP2FootstepFxSurfaceRootName = "Current_CentralPlaza_P2_FootstepFxReviewSurfaces";
        private const string Hd2dAutonomousP2FootstepFxDustSurfaceName = "P2_66_FootstepFx_DustSurface";
        private const string Hd2dAutonomousP2FootstepFxGrassSurfaceName = "P2_66_FootstepFx_GrassSurface";
        private const string Hd2dAutonomousP2FootstepFxWaterSurfaceName = "P2_66_FootstepFx_WaterSurface";
        private const string Hd2dAutonomousP2FootstepFxParticleMaterialId = "hd2d_p2_footstep_scuff_particle";
        private const string Hd2dAutonomousP2FootstepFxParticleTextureId = "hd2d_p2_footstep_scuff_particle_soft";

        public static void CaptureHd2dAutonomousP2Item66FootstepFxBatch()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var playerController = UnityEngine.Object.FindFirstObjectByType<CharacterController>();
            var emitter = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dFootstepFxEmitter>(FindObjectsInactive.Include);
            var animator = UnityEngine.Object.FindFirstObjectByType<FastVsDirectionalSpriteAnimator>(FindObjectsInactive.Include);
            if (controller == null || visibility == null || guide == null || camera == null || playerController == null || emitter == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-66 footstep FX capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP2FootstepFx();
            var profile = EnsureHd2dAutonomousP2FootstepFxProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("footstep_dust_scuff_puffs");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_no_footstep_particles_control.png",
                "02_left_foot_dust_contact_puff.png",
                "03_alternating_two_foot_scuff_puffs.png",
                "04_water_surface_splash_variant.png",
                "05_grass_surface_flick_variant.png",
                "06_puffs_faded_after_half_second.png"
            };
            var rows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                controller.ClosePortal();
                emitter.SetRuntimeCadenceEnabledForReview(false);
                SetHd2dAutonomousP2FootstepFxReviewSurfacesVisible(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                guide.ApplyActiveTimeIsolationForReview();

                CaptureHd2dAutonomousP2FootstepFxShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    playerController,
                    animator,
                    emitter,
                    FastVsHd2dFootstepSurfaceKind.Dust,
                    false,
                    false,
                    0.04f,
                    outputDirectory,
                    screenshotFiles[0],
                    "control: player foot contact framing with particle pool cleared",
                    rows);

                CaptureHd2dAutonomousP2FootstepFxShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    playerController,
                    animator,
                    emitter,
                    FastVsHd2dFootstepSurfaceKind.Dust,
                    true,
                    false,
                    0.055f,
                    outputDirectory,
                    screenshotFiles[1],
                    "left-foot dust puff: offset from sprite pivot to foot contact",
                    rows);

                CaptureHd2dAutonomousP2FootstepFxShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    playerController,
                    animator,
                    emitter,
                    FastVsHd2dFootstepSurfaceKind.Dust,
                    true,
                    true,
                    0.070f,
                    outputDirectory,
                    screenshotFiles[2],
                    "alternating left/right dust puffs prove cadence can hit both feet",
                    rows);

                CaptureHd2dAutonomousP2FootstepFxShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    playerController,
                    animator,
                    emitter,
                    FastVsHd2dFootstepSurfaceKind.Water,
                    true,
                    false,
                    0.055f,
                    outputDirectory,
                    screenshotFiles[3],
                    "water surface splash variant: blue upward burst",
                    rows);

                CaptureHd2dAutonomousP2FootstepFxShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    playerController,
                    animator,
                    emitter,
                    FastVsHd2dFootstepSurfaceKind.Grass,
                    true,
                    false,
                    0.055f,
                    outputDirectory,
                    screenshotFiles[4],
                    "grass surface flick variant: green side scuff",
                    rows);

                CaptureHd2dAutonomousP2FootstepFxShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    playerController,
                    animator,
                    emitter,
                    FastVsHd2dFootstepSurfaceKind.Dust,
                    true,
                    true,
                    profile.LifetimeForReview + 0.22f,
                    outputDirectory,
                    screenshotFiles[5],
                    "fade proof: same one-shot particles gone after lifetime",
                    rows);
            }
            finally
            {
                emitter.ClearForReview();
                emitter.SetRuntimeCadenceEnabledForReview(true);
                SetHd2dAutonomousP2FootstepFxReviewSurfacesVisible(true);
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                AssetDatabase.SaveAssets();
            }

            var dustDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var waterDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[3]);
            var fadeDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[2], screenshotFiles[5]);
            WriteHd2dAutonomousP2FootstepFxReviewReport(outputDirectory, screenshotFiles, rows, profile, dustDiff, waterDiff, fadeDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-66 footstep FX review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2FootstepFx(CharacterController playerController, Transform currentCentralPlazaRoot)
        {
            if (playerController == null)
            {
                return;
            }

            var player = playerController.gameObject;
            var profile = EnsureHd2dAutonomousP2FootstepFxProfile();
            var material = EnsureHd2dAutonomousP2FootstepFxParticleMaterial();
            var emitter = player.GetComponent<FastVsHd2dFootstepFxEmitter>();
            if (emitter == null)
            {
                emitter = player.AddComponent<FastVsHd2dFootstepFxEmitter>();
            }

            var systemTransform = player.transform.Find(Hd2dAutonomousP2FootstepFxSystemName);
            var systemObject = systemTransform != null ? systemTransform.gameObject : null;
            if (systemObject == null)
            {
                systemObject = new GameObject(Hd2dAutonomousP2FootstepFxSystemName);
                systemObject.transform.SetParent(player.transform, false);
            }

            systemObject.transform.localPosition = Vector3.zero;
            systemObject.transform.localRotation = Quaternion.identity;
            systemObject.transform.localScale = Vector3.one;
            systemObject.layer = PlayerVisibleRenderLayer;
            systemObject.SetActive(true);

            var renderer = systemObject.GetComponent<ParticleSystemRenderer>();
            if (renderer == null)
            {
                renderer = systemObject.AddComponent<ParticleSystemRenderer>();
            }

            var system = systemObject.GetComponent<ParticleSystem>();
            if (system == null)
            {
                system = systemObject.AddComponent<ParticleSystem>();
            }

            ConfigureHd2dAutonomousP2FootstepFxParticleSystem(system, renderer, profile, material);
            emitter.ConfigureForReview(profile, player.transform, playerController, system);

            CreateHd2dAutonomousP2FootstepFxReviewSurfaces(currentCentralPlazaRoot);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2FootstepFx()
        {
            var profile = EnsureHd2dAutonomousP2FootstepFxProfile();
            var emitter = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dFootstepFxEmitter>(FindObjectsInactive.Include);
            var player = GameObject.Find("FastVS_Player_NiroHouseSlice");
            var system = emitter != null ? emitter.ParticleSystemForReview : null;
            var renderer = system != null ? system.GetComponent<ParticleSystemRenderer>() : null;
            if (profile == null ||
                emitter == null ||
                player == null ||
                system == null ||
                renderer == null ||
                emitter.ProfileForReview != profile ||
                !emitter.RuntimeCadenceEnabledForReview ||
                !emitter.AnimationEventEntryPointAvailableForReview ||
                !emitter.PooledShurikenForReview ||
                !profile.StepCadenceRuntimeForReview ||
                !profile.SurfaceRaycastRuntimeForReview ||
                !profile.AutoSafeCompleteForReview ||
                profile.StepDistanceForReview <= 0.20f ||
                profile.FootLateralOffsetForReview < 0.10f ||
                profile.LifetimeForReview > 0.55f ||
                profile.SplashBurstParticlesForReview <= profile.DustBurstParticlesForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P2-66 footstep FX requires a pooled Shuriken emitter on Niro with cadence, surface raycast, animation-event entry, short lifetime, and distinct water burst settings.");
            }

            var main = system.main;
            var emission = system.emission;
            var shape = system.shape;
            if (main.simulationSpace != ParticleSystemSimulationSpace.World ||
                main.maxParticles < profile.MaxParticlesForReview ||
                main.loop ||
                emission.enabled ||
                shape.enabled ||
                renderer.sharedMaterial == null ||
                renderer.forceRenderingOff ||
                renderer.shadowCastingMode != ShadowCastingMode.Off ||
                renderer.receiveShadows)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P2-66 footstep particle system must be manual-emission World-space pooled Shuriken with shadows disabled. " +
                    $"simulationSpace={main.simulationSpace}, maxParticles={main.maxParticles}/{profile.MaxParticlesForReview}, loop={main.loop}, emission={emission.enabled}, shape={shape.enabled}, forceRenderingOff={renderer.forceRenderingOff}, material={(renderer.sharedMaterial != null ? renderer.sharedMaterial.name : "null")}, shadows={renderer.shadowCastingMode}, receiveShadows={renderer.receiveShadows}.");
            }

            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2FootstepFxSurfaceRootName) == null ||
                FindSceneObjectIncludingInactive(Hd2dAutonomousP2FootstepFxDustSurfaceName) == null ||
                FindSceneObjectIncludingInactive(Hd2dAutonomousP2FootstepFxGrassSurfaceName) == null ||
                FindSceneObjectIncludingInactive(Hd2dAutonomousP2FootstepFxWaterSurfaceName) == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-66 review requires dust/grass/water surface probes for surface-reactive capture evidence.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2FootstepFxEmitterRuntimePath), "PlayFootstepFX", Hd2dAutonomousP2FootstepFxEmitterRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2FootstepFxEmitterRuntimePath), "FastVsHd2dFootstepSurfaceKind.Water", Hd2dAutonomousP2FootstepFxEmitterRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2FootstepFxEmitterRuntimePath), "StepDistanceForReview", Hd2dAutonomousP2FootstepFxEmitterRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2FootstepFxProfileRuntimePath), "animationEventEntryPoint", Hd2dAutonomousP2FootstepFxProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2FootstepFx", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2FootstepFx", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dFootstepFxProfile EnsureHd2dAutonomousP2FootstepFxProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dFootstepFxProfile>(Hd2dAutonomousP2FootstepFxProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dFootstepFxProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2FootstepFxProfilePath);
            }

            profile.ConfigureForReview(
                128,
                0.58f,
                0.22f,
                0.18f,
                0.10f,
                0.65f,
                1.35f,
                20,
                24,
                0.48f,
                0.075f,
                0.180f,
                0.44f,
                0.42f,
                0.68f,
                0.25f,
                new Color(0.86f, 0.70f, 0.46f, 0.82f),
                new Color(0.56f, 0.78f, 0.36f, 0.78f),
                new Color(0.55f, 0.88f, 1.00f, 0.85f),
                true,
                true,
                true,
                true,
                true,
                "Auto-safe pooled Shuriken footstep dust/scuff puffs. Cadence is distance-based for billboard movement; PlayFootstepFX is available for authored Animation Events.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static void ConfigureHd2dAutonomousP2FootstepFxParticleSystem(
            ParticleSystem system,
            ParticleSystemRenderer renderer,
            FastVsHd2dFootstepFxProfile profile,
            Material material)
        {
            var main = system.main;
            main.loop = false;
            main.prewarm = false;
            main.playOnAwake = false;
            main.duration = 1.0f;
            main.startLifetime = new ParticleSystem.MinMaxCurve(profile.LifetimeForReview);
            main.startSpeed = 0f;
            main.startSize = new ParticleSystem.MinMaxCurve(profile.StartSizeMinForReview, profile.StartSizeMaxForReview);
            main.startColor = profile.DustColorForReview;
            main.maxParticles = profile.MaxParticlesForReview;
            main.gravityModifier = 0.04f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = false;
            emission.rateOverTime = 0f;
            emission.SetBursts(Array.Empty<ParticleSystem.Burst>());

            var shape = system.shape;
            shape.enabled = false;

            var velocity = system.velocityOverLifetime;
            velocity.enabled = false;

            var color = system.colorOverLifetime;
            color.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0.95f, 0f),
                    new GradientAlphaKey(0.70f, 0.42f),
                    new GradientAlphaKey(0f, 1f)
                });
            color.color = new ParticleSystem.MinMaxGradient(gradient);

            var size = system.sizeOverLifetime;
            size.enabled = true;
            var curve = new AnimationCurve(
                new Keyframe(0f, 0.55f),
                new Keyframe(0.25f, 1.08f),
                new Keyframe(1f, 1.35f));
            size.size = new ParticleSystem.MinMaxCurve(1f, curve);

            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.sortingOrder = 5;
            renderer.forceRenderingOff = false;
            ForceRendererEnabledForReview(renderer);
            EditorUtility.SetDirty(system);
            EditorUtility.SetDirty(renderer);
        }

        private static void ForceRendererEnabledForReview(Renderer renderer)
        {
            if (renderer == null)
            {
                return;
            }

            var serialized = new SerializedObject(renderer);
            var enabled = serialized.FindProperty("m_Enabled");
            if (enabled != null)
            {
                enabled.boolValue = true;
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }

            renderer.enabled = true;
        }

        private static Material EnsureHd2dAutonomousP2FootstepFxParticleMaterial()
        {
            EnsureFolder(MaterialDirectory);
            var path = $"{MaterialDirectory}/FastVS_House_{Hd2dAutonomousP2FootstepFxParticleMaterialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ?? Shader.Find("Universal Render Pipeline/Unlit");
                if (shader == null)
                {
                    throw new InvalidOperationException("P2-66 footstep particle shader not found.");
                }

                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            ConfigureTransparentParticleMaterial(material, 3035);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP2FootstepFxParticleTexture(), Vector2.one);
            ApplyMaterialRole(material, Hd2dAutonomousP2FootstepFxParticleMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", Color.white);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", Color.white);
            }

            if (material.HasProperty("_SoftParticlesEnabled"))
            {
                material.SetFloat("_SoftParticlesEnabled", 0f);
            }

            material.DisableKeyword("_SOFTPARTICLES_ON");
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP2FootstepFxParticleTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2FootstepFxParticleTextureId,
                64,
                64,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = ((x + 0.5f) / 64f) * 2f - 1f;
                    var v = ((y + 0.5f) / 64f) * 2f - 1f;
                    var squashed = Mathf.Sqrt((u * u * 0.86f) + (v * v * 1.45f));
                    var core = Mathf.Clamp01(1f - squashed);
                    var skirt = Mathf.Clamp01(1f - Mathf.Abs(squashed - 0.46f) * 2.8f);
                    var alpha = Mathf.Clamp((core * core * 0.72f) + (skirt * 0.18f), 0f, 0.86f);
                    var warm = 0.90f + core * 0.08f;
                    return new Color(warm, warm, warm, alpha);
                });
        }

        private static void CreateHd2dAutonomousP2FootstepFxReviewSurfaces(Transform currentCentralPlazaRoot)
        {
            if (currentCentralPlazaRoot == null)
            {
                return;
            }

            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2FootstepFxSurfaceRootName);
            if (root == null)
            {
                root = new GameObject(Hd2dAutonomousP2FootstepFxSurfaceRootName);
            }

            root.transform.SetParent(currentCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            SetHd2dAutonomousP2FootstepFxLayerRecursively(root, CurrentSpaceRenderLayer);

            CreateHd2dAutonomousP2FootstepFxSurface(
                root.transform,
                Hd2dAutonomousP2FootstepFxDustSurfaceName,
                CentralPlazaVsCenter + new Vector3(-1.08f, 0.018f, 3.12f),
                new Vector3(0.92f, 0.035f, 0.66f),
                EnsureHd2dAutonomousP2FootstepFxSurfaceMaterial("hd2d_p2_footstep_dust_surface", new Color(0.45f, 0.36f, 0.25f, 1f)));
            CreateHd2dAutonomousP2FootstepFxSurface(
                root.transform,
                Hd2dAutonomousP2FootstepFxGrassSurfaceName,
                CentralPlazaVsCenter + new Vector3(0.02f, 0.018f, 3.12f),
                new Vector3(0.92f, 0.035f, 0.66f),
                EnsureHd2dAutonomousP2FootstepFxSurfaceMaterial("hd2d_p2_footstep_grass_surface", new Color(0.23f, 0.40f, 0.20f, 1f)));
            CreateHd2dAutonomousP2FootstepFxSurface(
                root.transform,
                Hd2dAutonomousP2FootstepFxWaterSurfaceName,
                CentralPlazaVsCenter + new Vector3(1.12f, 0.018f, 3.12f),
                new Vector3(0.92f, 0.035f, 0.66f),
                EnsureHd2dAutonomousP2FootstepFxSurfaceMaterial("hd2d_p2_footstep_water_surface", new Color(0.18f, 0.45f, 0.62f, 1f)));
        }

        private static void CreateHd2dAutonomousP2FootstepFxSurface(
            Transform parent,
            string objectName,
            Vector3 localPosition,
            Vector3 localScale,
            Material material)
        {
            var existing = parent.Find(objectName);
            var surface = existing != null ? existing.gameObject : GameObject.CreatePrimitive(PrimitiveType.Cube);
            surface.name = objectName;
            surface.transform.SetParent(parent, false);
            surface.transform.localPosition = localPosition;
            surface.transform.localRotation = Quaternion.identity;
            surface.transform.localScale = localScale;
            surface.layer = CurrentSpaceRenderLayer;
            var renderer = surface.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            var collider = surface.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.isTrigger = false;
            }
        }

        private static Material EnsureHd2dAutonomousP2FootstepFxSurfaceMaterial(string materialId, Color color)
        {
            EnsureFolder(MaterialDirectory);
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null)
            {
                material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
                material.name = Path.GetFileNameWithoutExtension(materialPath);
                AssetDatabase.CreateAsset(material, materialPath);
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            if (material.HasProperty("_Smoothness"))
            {
                material.SetFloat("_Smoothness", 0f);
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static void SetHd2dAutonomousP2FootstepFxLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            for (var index = 0; index < root.transform.childCount; index++)
            {
                SetHd2dAutonomousP2FootstepFxLayerRecursively(root.transform.GetChild(index).gameObject, layer);
            }
        }

        private static void CaptureHd2dAutonomousP2FootstepFxShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            CharacterController playerController,
            FastVsDirectionalSpriteAnimator animator,
            FastVsHd2dFootstepFxEmitter emitter,
            FastVsHd2dFootstepSurfaceKind surfaceKind,
            bool emitPrimary,
            bool emitSecondary,
            float simulateSeconds,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            var anchorLocal = ResolveHd2dAutonomousP2FootstepFxAnchorLocal(surfaceKind);
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(anchorLocal);
            guide.ApplyActiveTimeIsolationForReview();
            if (animator != null)
            {
                animator.SetPoseForReview(FastVsCharacterDirection.Front, false, true);
            }

            var direction = Vector3.forward;
            var worldAnchor = controller.CurrentSpaceRootForReview.TransformPoint(anchorLocal);
            camera.orthographic = false;
            camera.fieldOfView = 34f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 120f;
            PositionChapter1AllMapsCamera(camera, worldAnchor, new Vector3(0.20f, 2.55f, -3.35f), new Vector3(0.00f, 0.34f, 0.05f));
            ApplyStage7BokehFocusForReview(camera);
            emitter.ClearForReview();
            if (emitPrimary)
            {
                emitter.EmitFootstepForReview(surfaceKind, ResolveHd2dAutonomousP2FootstepFxFootWorld(playerController.transform.position, direction, true), direction, true);
            }

            if (emitSecondary)
            {
                emitter.EmitFootstepForReview(surfaceKind, ResolveHd2dAutonomousP2FootstepFxFootWorld(playerController.transform.position, direction, false), direction, false);
            }

            emitter.SimulateForReview(Mathf.Max(0f, simulateSeconds), false);
            var liveParticles = emitter.LiveParticleCountForReview;
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {surfaceKind} | {emitPrimary} | {emitSecondary} | {simulateSeconds:0.###} | {liveParticles} | {FormatVector3ForReport(anchorLocal)} | {FormatVector3ForReport(emitter.LastFootWorldPositionForReview)} |");
        }

        private static Vector3 ResolveHd2dAutonomousP2FootstepFxAnchorLocal(FastVsHd2dFootstepSurfaceKind surfaceKind)
        {
            switch (surfaceKind)
            {
                case FastVsHd2dFootstepSurfaceKind.Grass:
                    return CentralPlazaVsCenter + new Vector3(0.02f, 0.05f, 3.12f);
                case FastVsHd2dFootstepSurfaceKind.Water:
                    return CentralPlazaVsCenter + new Vector3(1.12f, 0.05f, 3.12f);
                default:
                    return CentralPlazaVsCenter + new Vector3(-1.08f, 0.05f, 3.12f);
            }
        }

        private static Vector3 ResolveHd2dAutonomousP2FootstepFxFootWorld(Vector3 playerWorld, Vector3 direction, bool leftFoot)
        {
            var profile = EnsureHd2dAutonomousP2FootstepFxProfile();
            var move = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector3.forward;
            var right = Vector3.Cross(Vector3.up, move);
            if (right.sqrMagnitude <= 0.0001f)
            {
                right = Vector3.right;
            }

            right.Normalize();
            return playerWorld +
                   move * profile.FootForwardOffsetForReview +
                   right * ((leftFoot ? -1f : 1f) * profile.FootLateralOffsetForReview) +
                   Vector3.up * 0.055f;
        }

        private static void SetHd2dAutonomousP2FootstepFxReviewSurfacesVisible(bool visible)
        {
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2FootstepFxSurfaceRootName);
            if (root != null)
            {
                root.SetActive(visible);
            }
        }

        private static void WriteHd2dAutonomousP2FootstepFxReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dFootstepFxProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics dustDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics waterDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics fadeDiff)
        {
            var lines = new List<string>
            {
                "# P2-66 Footstep Dust / Scuff Puff Review",
                string.Empty,
                "- Scope: auto-safe pooled Shuriken one-shot footstep puffs driven by distance cadence, with Animation Event entry point and surface-reactive dust/grass/water variants.",
                "- Recommendation: keep this as the baseline grounding FX. Later authored Animation Events can call `PlayFootstepFX(footTransform)` on exact flipbook footplant frames; the current billboard runtime uses distance cadence until those events exist.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2FootstepFxProfilePath}` |",
                $"| Runtime emitter | `{Hd2dAutonomousP2FootstepFxEmitterRuntimePath}` |",
                $"| Step distance / min speed | {profile.StepDistanceForReview:0.###} / {profile.MinMoveSpeedForReview:0.###} |",
                $"| Foot lateral / forward offset | {profile.FootLateralOffsetForReview:0.###} / {profile.FootForwardOffsetForReview:0.###} |",
                $"| Dust / splash particles | {profile.DustBurstParticlesForReview} / {profile.SplashBurstParticlesForReview} |",
                $"| Lifetime / size | {profile.LifetimeForReview:0.###}s / {profile.StartSizeMinForReview:0.###}-{profile.StartSizeMaxForReview:0.###} |",
                $"| Dust / grass / water color | {FormatColor(profile.DustColorForReview)} / {FormatColor(profile.GrassColorForReview)} / {FormatColor(profile.WaterColorForReview)} |",
                $"| Cadence / raycast / pooled / animation-event entry | {FormatBool(profile.StepCadenceRuntimeForReview)} / {FormatBool(profile.SurfaceRaycastRuntimeForReview)} / {FormatBool(profile.PooledShurikenRuntimeForReview)} / {FormatBool(profile.AnimationEventEntryPointForReview)} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                dustDiff.ToReportRow("Control vs left-foot dust puff"),
                waterDiff.ToReportRow("Dust puff vs water splash variant"),
                fadeDiff.ToReportRow("Active two-foot puffs vs faded frame"),
                string.Empty,
                "| Screenshot | Label | Surface | Primary | Secondary | Sim seconds | Live particles | Anchor local | Last foot world |",
                "|---|---|---|---:|---:|---:|---:|---|---|"
            };
            lines.AddRange(shotRows);
            lines.Add(string.Empty);
            lines.Add("| Screenshot | Purpose |");
            lines.Add("|---|---|");
            for (var index = 0; index < screenshotFiles.Count; index++)
            {
                lines.Add($"| `{screenshotFiles[index]}` | P2-66 footstep FX capture {index + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "footstep_dust_scuff_puffs_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }
    }
}
