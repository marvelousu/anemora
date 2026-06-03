using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private const string Hd2dAutonomousP2IdleEmissiveProfilePath = "Assets/Settings/FastVS_HD2D_P2_IdleEmissiveProfile.asset";
        private const string Hd2dAutonomousP2IdleEmissiveProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dIdleEmissiveProfile.cs";
        private const string Hd2dAutonomousP2IdleSecondaryMotionRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dIdleSecondaryMotion.cs";
        private const string Hd2dAutonomousP2IdleEmissiveAccentRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dIdleEmissiveAccent.cs";
        private const string Hd2dAutonomousP2IdleEmissiveMarkerRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dIdleEmissiveMarker.cs";
        private const string Hd2dAutonomousP2IdleEmissiveEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2IdleBreathingEmissiveAccents.cs";
        private const string Hd2dAutonomousP2IdleEmissiveMiaMaterialId = "mia_normal_loop_breath_sprite_p2_76_idle_emissive";
        private const string Hd2dAutonomousP2IdleEmissiveMaskTextureId = "hd2d_p2_76_idle_emissive_mia_eye_lantern_mask";
        private const string Hd2dAutonomousP2IdleEmissiveHaloMaterialId = "hd2d_p2_76_idle_emissive_halo";
        private const string Hd2dAutonomousP2IdleEmissiveHaloTextureId = "hd2d_p2_76_idle_emissive_halo_soft";
        private const string Hd2dAutonomousP2IdleEmissiveReviewCharacterName = "P2_76_IdleEmissiveReviewCharacter";
        private const string Hd2dAutonomousP2IdleEmissiveReviewSpriteMaterialId = "hd2d_p2_76_idle_review_character_sprite";
        private const string Hd2dAutonomousP2IdleEmissiveReviewSpriteTextureId = "hd2d_p2_76_idle_review_character_sprite_cutout";
        private const string Hd2dAutonomousP2IdleEmissiveReviewMaskTextureId = "hd2d_p2_76_idle_review_character_emission_mask";
        private const string Hd2dAutonomousP2IdleEmissiveMiaMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2IdleEmissiveMiaMaterialId + ".mat";
        private const string Hd2dAutonomousP2IdleEmissiveMaskTexturePath = TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP2IdleEmissiveMaskTextureId + ".asset";
        private const string Hd2dAutonomousP2IdleEmissiveHaloMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2IdleEmissiveHaloMaterialId + ".mat";
        private const string Hd2dAutonomousP2IdleEmissiveReviewSpriteMaterialPath = MaterialDirectory + "/FastVS_House_" + Hd2dAutonomousP2IdleEmissiveReviewSpriteMaterialId + ".mat";

        public static void CaptureHd2dAutonomousP2Item76IdleBreathingEmissiveAccentsBatch()
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
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-76 idle/emissive capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2IdleBreathingEmissiveAccents();
            var profile = EnsureHd2dAutonomousP2IdleEmissiveProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("idle_breathing_secondary_motion_emissive_accents");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_idle_motion_frame_a.png",
                "02_idle_motion_frame_b.png",
                "03_emissive_off_night_baseline.png",
                "04_emissive_on_bloom_light_pool.png",
                "05_emissive_close_diagnostic.png"
            };
            var shotRows = new List<string>();
            var previousCullingMask = camera.cullingMask;
            var previousFieldOfView = camera.fieldOfView;
            var previousNear = camera.nearClipPlane;
            var previousFar = camera.farClipPlane;
            var previousWindowEmission = Shader.GetGlobalFloat(Shader.PropertyToID(Hd2dAutonomousP0WindowEmissionStrengthGlobalName));
            var temporaryObjects = new List<UnityEngine.Object>();

            try
            {
                guide.SetMovementFrozen(true);
                controller.ClosePortal();
                CreateHd2dAutonomousP2IdleEmissiveReviewCharacter(controller, profile, temporaryObjects);
                SetHd2dAutonomousP2IdleEmissiveAccentVisible(false);
                SetHd2dAutonomousP2IdleEmissiveAccentMultipliers(0f, 0f);
                Shader.SetGlobalFloat(Hd2dAutonomousP0WindowEmissionStrengthGlobalName, 0f);
                CaptureHd2dAutonomousP2IdleEmissiveMiaShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[0],
                    "idle secondary motion frame A, accent hidden to isolate silhouette",
                    0.00f,
                    SunPreset.Noon,
                    false,
                    0f,
                    26f,
                    new Vector3(1.16f, 1.22f, -2.28f),
                    new Vector3(0.02f, 0.15f, 0.00f),
                    shotRows);

                CaptureHd2dAutonomousP2IdleEmissiveMiaShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[1],
                    "idle secondary motion frame B, same camera after phase sample",
                    0.34f,
                    SunPreset.Noon,
                    false,
                    0f,
                    26f,
                    new Vector3(1.16f, 1.22f, -2.28f),
                    new Vector3(0.02f, 0.15f, 0.00f),
                    shotRows);

                CaptureHd2dAutonomousP2IdleEmissiveMiaShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[2],
                    "Night A/B baseline with Mia eye/lantern emissive and light disabled",
                    0.18f,
                    SunPreset.Night,
                    false,
                    0f,
                    25f,
                    new Vector3(1.08f, 1.12f, -2.14f),
                    new Vector3(0.02f, 0.12f, 0.00f),
                    shotRows);

                CaptureHd2dAutonomousP2IdleEmissiveMiaShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[3],
                    "Night A/B with Mia emissive mask, halo, and attached point light enabled",
                    0.58f,
                    SunPreset.Night,
                    true,
                    profile.ReviewWindowEmissionStrengthForReview,
                    25f,
                    new Vector3(1.08f, 1.12f, -2.14f),
                    new Vector3(0.02f, 0.12f, 0.00f),
                    shotRows);

                CaptureHd2dAutonomousP2IdleEmissiveMiaShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    sunDriver,
                    outputDirectory,
                    screenshotFiles[4],
                    "closer diagnostic: glow mask, halo card, and light pool alignment",
                    0.58f,
                    SunPreset.Night,
                    true,
                    profile.ReviewWindowEmissionStrengthForReview,
                    18f,
                    new Vector3(0.76f, 0.86f, -1.48f),
                    new Vector3(0.00f, 0.10f, 0.00f),
                    shotRows);
            }
            finally
            {
                camera.cullingMask = previousCullingMask;
                camera.fieldOfView = previousFieldOfView;
                camera.nearClipPlane = previousNear;
                camera.farClipPlane = previousFar;
                Shader.SetGlobalFloat(Hd2dAutonomousP0WindowEmissionStrengthGlobalName, previousWindowEmission);
                SetHd2dAutonomousP2IdleEmissiveAccentVisible(true);
                SetHd2dAutonomousP2IdleEmissiveAccentMultipliers(1f, 1f);
                ClearHd2dAutonomousP2IdleEmissiveReviewTimes();
                ResetHd2dAutonomousP2IdleEmissiveMotionPoses();
                for (var i = temporaryObjects.Count - 1; i >= 0; i--)
                {
                    if (temporaryObjects[i] != null)
                    {
                        UnityEngine.Object.DestroyImmediate(temporaryObjects[i]);
                    }
                }

                guide.SetMovementFrozen(false);
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                guide.ApplyActiveTimeIsolationForReview();
            }

            var motionDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                3);
            var emissiveDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[2]),
                Path.Combine(outputDirectory, screenshotFiles[3]),
                4);
            if (motionDiff.SampleCount <= 0 || motionDiff.ChangedPixels <= 0)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-76 capture failed: idle frame A/B produced no measurable changed pixels.");
            }

            if (emissiveDiff.SampleCount <= 0 || emissiveDiff.ChangedPixels <= 0)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-76 capture failed: emissive Night A/B produced no measurable changed pixels.");
            }

            WriteHd2dAutonomousP2IdleEmissiveReviewReport(outputDirectory, screenshotFiles, shotRows, profile, motionDiff, emissiveDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-76 idle/emissive review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2IdleBreathingEmissiveAccents()
        {
            EnsureHd2dAutonomousP2IdleEmissiveProfile();
            EnsureHd2dAutonomousP2IdleEmissiveMiaMaterial();
            EnsureHd2dAutonomousP2IdleEmissiveReviewSpriteMaterial();
            EnsureHd2dAutonomousP2IdleEmissiveHaloMaterial(EnsureHd2dAutonomousP2IdleEmissiveProfile());
            AssetDatabase.SaveAssets();
        }

        private static void ConfigureHd2dAutonomousP2IdleEmissiveCharacter(GameObject character, string characterId, FastVsHouseArea area, bool currentWorld)
        {
            if (character == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP2IdleEmissiveProfile();
            var renderer = character.GetComponentInChildren<Renderer>(true);
            var animator = character.GetComponent<FastVsSpriteStripLoopAnimator>();
            if (renderer == null || animator == null)
            {
                return;
            }

            var primaryAccent = currentWorld && string.Equals(character.name, "FastVS_SpriteCharacter_Mia", StringComparison.Ordinal);
            var accentKind = primaryAccent ? FastVsHd2dIdleEmissiveAccentKind.EyeAndLantern : FastVsHd2dIdleEmissiveAccentKind.None;
            if (primaryAccent)
            {
                renderer.sharedMaterial = EnsureHd2dAutonomousP2IdleEmissiveMiaMaterial();
            }

            var motion = character.GetComponent<FastVsHd2dIdleSecondaryMotion>();
            if (motion == null)
            {
                motion = character.AddComponent<FastVsHd2dIdleSecondaryMotion>();
            }

            motion.ConfigureForReview(profile, renderer.transform, ResolveHd2dAutonomousP2IdleEmissivePhase(character.name, profile));

            FastVsHd2dIdleEmissiveAccent accent = null;
            if (primaryAccent)
            {
                accent = EnsureHd2dAutonomousP2IdleEmissiveAccent(character, renderer, profile, accentKind, currentWorld);
            }

            var marker = character.GetComponent<FastVsHd2dIdleEmissiveMarker>();
            if (marker == null)
            {
                marker = character.AddComponent<FastVsHd2dIdleEmissiveMarker>();
            }

            marker.ConfigureForReview(
                characterId,
                profile,
                renderer,
                animator,
                motion,
                accent,
                accentKind,
                currentWorld,
                (int)area,
                primaryAccent);
            EditorUtility.SetDirty(character);
        }

        private static void ValidateHd2dAutonomousP2IdleBreathingEmissiveAccents()
        {
            var profile = EnsureHd2dAutonomousP2IdleEmissiveProfile();
            var material = EnsureHd2dAutonomousP2IdleEmissiveMiaMaterial();
            var reviewMaterial = EnsureHd2dAutonomousP2IdleEmissiveReviewSpriteMaterial();
            if (!profile.NeedsTomApprovalForReview ||
                profile.FinalIdleEmissiveApprovedForReview ||
                !profile.ConservativeDataPrepForReview ||
                profile.LoopFrameCountForReview != NiroAnimatedFrameCount ||
                profile.VerticalBreathMetersForReview <= 0f ||
                profile.VerticalBreathMetersForReview > 0.025f ||
                profile.SquashStretchScaleForReview <= 0f ||
                profile.SpriteEmissionIntensityForReview < 2.0f ||
                profile.ReviewWindowEmissionStrengthForReview < 0.8f ||
                profile.PointLightIntensityForReview <= 0f ||
                profile.PointLightRangeMetersForReview < 0.8f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-76 must stay a conservative NEEDS-TOM profile with 4-frame idle loops, small breathing/sway, HDR emissive mask, and nonzero attached light.");
            }

            if (material == null ||
                !material.HasProperty(Stage2EmissionMapPropertyName) ||
                !material.HasProperty(Stage2EmissionColorPropertyName) ||
                !material.HasProperty(Stage2EmissionIntensityPropertyName) ||
                material.GetTexture(Stage2EmissionMapPropertyName) == null ||
                material.GetFloat(Stage2EmissionIntensityPropertyName) < profile.SpriteEmissionIntensityForReview - 0.01f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-76 Mia material must carry an emissive eye/lantern mask and HDR intensity.");
            }

            if (reviewMaterial == null ||
                reviewMaterial.GetTexture(Stage2EmissionMapPropertyName) == null ||
                reviewMaterial.GetTexture("_BaseMap") == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-76 review sprite material must carry a generated cutout and matching emissive mask for capture.");
            }

            var markers = FindHd2dAutonomousP2IdleEmissiveMarkers()
                .Where(marker => marker != null && marker.gameObject.scene.IsValid())
                .ToArray();
            var animatedMarkers = markers.Count(marker => marker.HasFourFrameIdleLoopForReview && marker.HasSecondaryMotionForReview);
            var accents = markers.Where(marker => marker.HasEmissiveAccentForReview).ToArray();
            var primary = markers.FirstOrDefault(marker => marker.PrimaryReviewAccentForReview);
            if (animatedMarkers < profile.MinimumAnimatedMarkersForReview ||
                accents.Length < profile.MinimumEmissiveAccentMarkersForReview ||
                primary == null ||
                primary.EmissiveAccentForReview == null ||
                primary.EmissiveAccentForReview.AccentKindForReview != FastVsHd2dIdleEmissiveAccentKind.EyeAndLantern)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: P2-76 needs animated idle markers and a primary emissive accent. animated={animatedMarkers}, accents={accents.Length}, primary={primary != null}.");
            }

            foreach (var marker in markers)
            {
                ValidateHd2dAutonomousP2IdleEmissiveMarker(marker, profile);
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2IdleEmissiveProfileRuntimePath), "needsTomApproval", Hd2dAutonomousP2IdleEmissiveProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2IdleEmissiveProfileRuntimePath), "finalIdleEmissiveApproved", Hd2dAutonomousP2IdleEmissiveProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2IdleSecondaryMotionRuntimePath), "SampleForReview", Hd2dAutonomousP2IdleSecondaryMotionRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2IdleEmissiveAccentRuntimePath), "SetReviewMultipliersForReview", Hd2dAutonomousP2IdleEmissiveAccentRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2IdleEmissiveMarkerRuntimePath), "HasFourFrameIdleLoopForReview", Hd2dAutonomousP2IdleEmissiveMarkerRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2IdleEmissiveEditorPath), "CaptureHd2dAutonomousP2Item76IdleBreathingEmissiveAccentsBatch", Hd2dAutonomousP2IdleEmissiveEditorPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2IdleEmissiveEditorPath), Hd2dAutonomousP2IdleEmissiveReviewCharacterName, Hd2dAutonomousP2IdleEmissiveEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ConfigureHd2dAutonomousP2IdleEmissiveCharacter", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2IdleBreathingEmissiveAccents", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dIdleEmissiveProfile EnsureHd2dAutonomousP2IdleEmissiveProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dIdleEmissiveProfile>(Hd2dAutonomousP2IdleEmissiveProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dIdleEmissiveProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2IdleEmissiveProfilePath);
            }

            profile.ConfigureForReview(
                NiroAnimatedFrameCount,
                0.72f,
                0.018f,
                0.006f,
                0.012f,
                0.71f,
                0.86f,
                0.22f,
                new Color(1.0f, 0.62f, 0.28f, 1f),
                3.35f,
                1.18f,
                0.36f,
                1.65f,
                0.46f,
                8,
                1,
                true,
                true,
                true,
                false,
                "Keep the conservative breathing/sway and Mia lantern/eye glow as A/B data only. Tom should approve final amplitude, blink timing, glow color, bloom strength, and which character accents are canonical.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Material EnsureHd2dAutonomousP2IdleEmissiveMiaMaterial()
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            EnsureTextureImporter(MiaNormalLoopStripPath);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(MiaNormalLoopStripPath);
            if (texture == null)
            {
                throw new InvalidOperationException($"P2-76 Mia emissive material missing source texture: {MiaNormalLoopStripPath}");
            }

            var profile = EnsureHd2dAutonomousP2IdleEmissiveProfile();
            var material = CreateSpriteCardMaterial(Hd2dAutonomousP2IdleEmissiveMiaMaterialId, Color.white, 3011);
            AssignMaterialTexture(material, texture, new Vector2(1f / NiroAnimatedFrameCount, 1f));
            material.SetTexture(Stage2EmissionMapPropertyName, EnsureHd2dAutonomousP2IdleEmissiveMaskTexture());
            material.SetColor(Stage2EmissionColorPropertyName, profile.EmissiveColorForReview);
            material.SetFloat(Stage2EmissionIntensityPropertyName, profile.SpriteEmissionIntensityForReview);
            material.EnableKeyword("_EMISSION");
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
            material.enableInstancing = true;
            ApplyMaterialRole(material, Hd2dAutonomousP2IdleEmissiveMiaMaterialId, FastVsHd2dMaterialRole.SpriteCard);
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            return material;
        }

        private static Material EnsureHd2dAutonomousP2IdleEmissiveReviewSpriteMaterial()
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            var profile = EnsureHd2dAutonomousP2IdleEmissiveProfile();
            var material = CreateSpriteCardMaterial(Hd2dAutonomousP2IdleEmissiveReviewSpriteMaterialId, Color.white, 3011);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP2IdleEmissiveReviewSpriteTexture(), Vector2.one);
            material.SetTexture(Stage2EmissionMapPropertyName, EnsureHd2dAutonomousP2IdleEmissiveReviewMaskTexture());
            material.SetColor(Stage2EmissionColorPropertyName, profile.EmissiveColorForReview);
            material.SetFloat(Stage2EmissionIntensityPropertyName, profile.SpriteEmissionIntensityForReview);
            material.EnableKeyword("_EMISSION");
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
            material.enableInstancing = true;
            ApplyMaterialRole(material, Hd2dAutonomousP2IdleEmissiveReviewSpriteMaterialId, FastVsHd2dMaterialRole.SpriteCard);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP2IdleEmissiveReviewSpriteTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2IdleEmissiveReviewSpriteTextureId,
                NiroExpectedTextureWidth,
                NiroExpectedTextureHeight,
                FilterMode.Point,
                SampleHd2dAutonomousP2IdleEmissiveReviewSpritePixel);
        }

        private static Texture2D EnsureHd2dAutonomousP2IdleEmissiveReviewMaskTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2IdleEmissiveReviewMaskTextureId,
                NiroExpectedTextureWidth,
                NiroExpectedTextureHeight,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = x / (NiroExpectedTextureWidth - 1f);
                    var v = y / (NiroExpectedTextureHeight - 1f);
                    var eyes = Mathf.Max(
                        SoftCircle01(u, v, 0.455f, 0.705f, 0.030f, 0.022f),
                        SoftCircle01(u, v, 0.545f, 0.705f, 0.030f, 0.022f));
                    var lantern = SoftCircle01(u, v, 0.635f, 0.405f, 0.090f, 0.075f);
                    var halo = SoftCircle01(u, v, 0.635f, 0.405f, 0.165f, 0.135f) * 0.30f;
                    var mask = Mathf.Clamp01(eyes * 0.90f + lantern + halo);
                    return new Color(mask, mask * 0.76f, mask * 0.42f, 1f);
                });
        }

        private static Color SampleHd2dAutonomousP2IdleEmissiveReviewSpritePixel(int x, int y)
        {
            var u = x / (NiroExpectedTextureWidth - 1f);
            var v = y / (NiroExpectedTextureHeight - 1f);
            var head = SoftCircle01(u, v, 0.50f, 0.71f, 0.145f, 0.120f);
            var hair = SoftCircle01(u, v, 0.50f, 0.765f, 0.170f, 0.115f);
            var torso = RoundedRect01(u, v, 0.50f, 0.465f, 0.165f, 0.245f, 0.055f);
            var skirt = RoundedRect01(u, v, 0.50f, 0.285f, 0.215f, 0.155f, 0.045f);
            var leftArm = RoundedRect01(u, v, 0.332f, 0.440f, 0.055f, 0.225f, 0.035f);
            var rightArm = RoundedRect01(u, v, 0.668f, 0.425f, 0.055f, 0.210f, 0.035f);
            var leftLeg = RoundedRect01(u, v, 0.435f, 0.115f, 0.055f, 0.160f, 0.030f);
            var rightLeg = RoundedRect01(u, v, 0.565f, 0.115f, 0.055f, 0.160f, 0.030f);
            var lantern = SoftCircle01(u, v, 0.635f, 0.405f, 0.080f, 0.065f);
            var alpha = Mathf.Clamp01(Mathf.Max(Mathf.Max(head, torso), Mathf.Max(skirt, Mathf.Max(leftArm, rightArm))) + Mathf.Max(leftLeg, rightLeg) + lantern);
            if (alpha <= 0.01f)
            {
                return new Color(0f, 0f, 0f, 0f);
            }

            var skin = new Color(0.90f, 0.62f, 0.42f, 1f);
            var hairColor = new Color(0.23f, 0.12f, 0.08f, 1f);
            var cloth = new Color(0.36f, 0.42f, 0.74f, 1f);
            var shadow = new Color(0.18f, 0.16f, 0.22f, 1f);
            var warm = new Color(1.0f, 0.68f, 0.26f, 1f);
            var color = shadow;
            if (skirt > 0.05f || torso > 0.05f)
            {
                color = cloth * (0.80f + v * 0.25f);
                color.a = 1f;
            }

            if (leftArm > 0.05f || rightArm > 0.05f || leftLeg > 0.05f || rightLeg > 0.05f)
            {
                color = Color.Lerp(color, skin, 0.55f);
            }

            if (head > 0.05f)
            {
                color = skin;
            }

            if (hair > 0.05f && v > 0.72f)
            {
                color = hairColor;
            }

            if (lantern > 0.05f)
            {
                color = Color.Lerp(color, warm, 0.86f);
            }

            var line = ((x + y * 3) % 17) == 0 ? 0.86f : 1f;
            color *= line;
            color.a = Mathf.Clamp01(alpha);
            return color;
        }

        private static Texture2D EnsureHd2dAutonomousP2IdleEmissiveMaskTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2IdleEmissiveMaskTextureId,
                NiroExpectedTextureWidth * NiroAnimatedFrameCount,
                NiroExpectedTextureHeight,
                FilterMode.Bilinear,
                SampleHd2dAutonomousP2IdleEmissiveMaskPixel);
        }

        private static Color SampleHd2dAutonomousP2IdleEmissiveMaskPixel(int x, int y)
        {
            var frameX = x % NiroExpectedTextureWidth;
            var u = frameX / (NiroExpectedTextureWidth - 1f);
            var v = y / (NiroExpectedTextureHeight - 1f);
            var leftEye = SoftCircle01(u, v, 0.442f, 0.686f, 0.028f, 0.020f);
            var rightEye = SoftCircle01(u, v, 0.558f, 0.686f, 0.028f, 0.020f);
            var lanternCore = SoftCircle01(u, v, 0.642f, 0.435f, 0.072f, 0.064f);
            var lanternHalo = SoftCircle01(u, v, 0.642f, 0.435f, 0.132f, 0.118f) * 0.38f;
            var mask = Mathf.Clamp01(Mathf.Max(Mathf.Max(leftEye, rightEye) * 0.92f, lanternCore) + lanternHalo);
            return new Color(mask, mask * 0.78f, mask * 0.44f, 1f);
        }

        private static Material EnsureHd2dAutonomousP2IdleEmissiveHaloMaterial(FastVsHd2dIdleEmissiveProfile profile)
        {
            EnsureFolder(MaterialDirectory);
            EnsureFolder(TextureDirectory);
            var material = FlatMaterial(Hd2dAutonomousP2IdleEmissiveHaloMaterialId, Color.white, true, FastVsHd2dMaterialRole.OverlayGlow);
            ConfigureTransparentMaterial(material, 3062, URPUnlitShaderName, SpriteCardRampShaderName);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP2IdleEmissiveHaloTexture(), Vector2.one);
            var hdr = profile.EmissiveColorForReview * 1.85f;
            hdr.a = 0.72f;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", hdr);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", hdr);
            }

            if (material.HasProperty(Stage2EmissionColorPropertyName))
            {
                material.SetColor(Stage2EmissionColorPropertyName, profile.EmissiveColorForReview * 1.5f);
            }

            if (material.HasProperty(Stage2EmissionIntensityPropertyName))
            {
                material.SetFloat(Stage2EmissionIntensityPropertyName, profile.SpriteEmissionIntensityForReview);
            }

            material.EnableKeyword("_EMISSION");
            material.enableInstancing = true;
            ApplyMaterialRole(material, Hd2dAutonomousP2IdleEmissiveHaloMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP2IdleEmissiveHaloTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2IdleEmissiveHaloTextureId,
                96,
                96,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = (x / 95f - 0.5f) * 2f;
                    var v = (y / 95f - 0.5f) * 2f;
                    var radius = Mathf.Sqrt((u * u * 0.82f) + (v * v * 1.08f));
                    var core = Mathf.Clamp01(1f - radius * 1.72f);
                    var halo = Mathf.Clamp01(1f - radius * 0.84f);
                    var alpha = Mathf.Clamp01(core * 0.72f + halo * halo * 0.34f);
                    return new Color(1f, 0.70f, 0.34f, alpha);
                });
        }

        private static FastVsHd2dIdleEmissiveAccent EnsureHd2dAutonomousP2IdleEmissiveAccent(
            GameObject character,
            Renderer spriteRenderer,
            FastVsHd2dIdleEmissiveProfile profile,
            FastVsHd2dIdleEmissiveAccentKind accentKind,
            bool currentWorld)
        {
            var accentRoot = new GameObject("P2_76_IdleEmissiveAccentRoot");
            accentRoot.transform.SetParent(character.transform, false);
            accentRoot.transform.localPosition = new Vector3(0.135f, 0.62f, -0.045f);
            accentRoot.transform.localRotation = Quaternion.identity;
            accentRoot.transform.localScale = Vector3.one;
            SetHd2dAutonomousP2IdleEmissiveLayerRecursively(accentRoot, currentWorld ? CurrentSpaceRenderLayer : OtherTimeSpaceRenderLayer);

            var halo = CreateQuad(
                "P2_76_Mia_EyeLanternHalo",
                accentRoot.transform,
                Vector3.zero,
                new Vector3(profile.HaloWorldSizeForReview, profile.HaloWorldSizeForReview * 0.92f, 1f),
                EnsureHd2dAutonomousP2IdleEmissiveHaloMaterial(profile));
            var haloRenderer = halo.GetComponent<Renderer>();
            if (haloRenderer != null)
            {
                haloRenderer.shadowCastingMode = ShadowCastingMode.Off;
                haloRenderer.receiveShadows = false;
            }

            var lightObject = new GameObject("P2_76_Mia_EyeLanternPointLight");
            lightObject.transform.SetParent(accentRoot.transform, false);
            lightObject.transform.localPosition = new Vector3(0f, -0.02f, -0.11f);
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = profile.EmissiveColorForReview;
            light.intensity = profile.PointLightIntensityForReview;
            light.range = profile.PointLightRangeMetersForReview;
            light.shadows = LightShadows.None;

            var accent = character.GetComponent<FastVsHd2dIdleEmissiveAccent>();
            if (accent == null)
            {
                accent = character.AddComponent<FastVsHd2dIdleEmissiveAccent>();
            }

            accent.ConfigureForReview(profile, accentKind, spriteRenderer, haloRenderer, light);
            return accent;
        }

        private static void CreateHd2dAutonomousP2IdleEmissiveReviewCharacter(
            TimeWindowPairedSpacePortalController controller,
            FastVsHd2dIdleEmissiveProfile profile,
            ICollection<UnityEngine.Object> temporaryObjects)
        {
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2IdleEmissiveReviewCharacterName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2IdleEmissiveReviewCharacterName);
            temporaryObjects?.Add(root);
            root.transform.SetParent(controller.CurrentSpaceRootForReview, false);
            root.transform.localPosition = Chapter1MiaRuntimePosition + new Vector3(0.72f, 0.00f, -0.18f);
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;
            root.AddComponent<FastVsPaperBillboard>();

            var bodyMaterial = FlatMaterial("hd2d_p2_76_review_character_body", new Color(0.36f, 0.42f, 0.74f, 1f), false, FastVsHd2dMaterialRole.SpriteCard);
            var skinMaterial = FlatMaterial("hd2d_p2_76_review_character_skin", new Color(0.90f, 0.62f, 0.42f, 1f), false, FastVsHd2dMaterialRole.SpriteCard);
            var hairMaterial = FlatMaterial("hd2d_p2_76_review_character_hair", new Color(0.23f, 0.12f, 0.08f, 1f), false, FastVsHd2dMaterialRole.SpriteCard);
            var bootMaterial = FlatMaterial("hd2d_p2_76_review_character_boot", new Color(0.16f, 0.12f, 0.10f, 1f), false, FastVsHd2dMaterialRole.SpriteCard);
            var renderer = CreateQuad(
                "P2_76_ReviewCharacter_Body",
                root.transform,
                new Vector3(0f, 0.52f, 0f),
                new Vector3(0.34f, 0.52f, 1f),
                bodyMaterial).GetComponent<Renderer>();
            CreateQuad("P2_76_ReviewCharacter_Head", root.transform, new Vector3(0f, 0.88f, -0.012f), new Vector3(0.24f, 0.22f, 1f), skinMaterial);
            CreateQuad("P2_76_ReviewCharacter_Hair", root.transform, new Vector3(0f, 1.00f, -0.024f), new Vector3(0.30f, 0.14f, 1f), hairMaterial);
            CreateQuad("P2_76_ReviewCharacter_LeftArm", root.transform, new Vector3(-0.24f, 0.50f, -0.006f), new Vector3(0.08f, 0.34f, 1f), skinMaterial);
            CreateQuad("P2_76_ReviewCharacter_RightArm", root.transform, new Vector3(0.24f, 0.50f, -0.006f), new Vector3(0.08f, 0.34f, 1f), skinMaterial);
            CreateQuad("P2_76_ReviewCharacter_LeftLeg", root.transform, new Vector3(-0.08f, 0.18f, -0.006f), new Vector3(0.08f, 0.26f, 1f), bootMaterial);
            CreateQuad("P2_76_ReviewCharacter_RightLeg", root.transform, new Vector3(0.08f, 0.18f, -0.006f), new Vector3(0.08f, 0.26f, 1f), bootMaterial);
            foreach (var partRenderer in root.GetComponentsInChildren<Renderer>(true))
            {
                partRenderer.shadowCastingMode = ShadowCastingMode.Off;
                partRenderer.receiveShadows = true;
            }

            var animator = root.AddComponent<FastVsSpriteStripLoopAnimator>();
            SerializedSet(animator, "spriteRenderer", renderer);
            SerializedSet(animator, "frameCount", NiroAnimatedFrameCount);
            SerializedSet(animator, "framesPerSecond", 2.2f);

            var motion = root.AddComponent<FastVsHd2dIdleSecondaryMotion>();
            motion.ConfigureForReview(profile, root.transform, 0.42f);
            var accent = EnsureHd2dAutonomousP2IdleEmissiveAccent(root, renderer, profile, FastVsHd2dIdleEmissiveAccentKind.EyeAndLantern, true);
            var marker = root.AddComponent<FastVsHd2dIdleEmissiveMarker>();
            marker.ConfigureForReview(
                "P2-76 Review Sprite",
                profile,
                renderer,
                animator,
                motion,
                accent,
                FastVsHd2dIdleEmissiveAccentKind.EyeAndLantern,
                true,
                (int)FastVsHouseArea.MiaInterior,
                true);
            SetHd2dAutonomousP2IdleEmissiveLayerRecursively(root, CurrentSpaceRenderLayer);
        }

        private static void ValidateHd2dAutonomousP2IdleEmissiveMarker(FastVsHd2dIdleEmissiveMarker marker, FastVsHd2dIdleEmissiveProfile profile)
        {
            if (marker == null)
            {
                return;
            }

            if (marker.ProfileForReview != profile ||
                marker.SpriteRendererForReview == null ||
                marker.StripAnimatorForReview == null ||
                marker.SecondaryMotionForReview == null ||
                !marker.HasFourFrameIdleLoopForReview ||
                !marker.HasSecondaryMotionForReview)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-76 idle marker {marker.CharacterIdForReview} is missing renderer, 4-frame animator, profile, or secondary motion.");
            }

            if (marker.PrimaryReviewAccentForReview)
            {
                var accent = marker.EmissiveAccentForReview;
                if (accent == null ||
                    accent.HaloRendererForReview == null ||
                    accent.PointLightForReview == null ||
                    accent.PointLightForReview.range < profile.PointLightRangeMetersForReview - 0.01f ||
                    accent.PointLightForReview.intensity <= 0f)
                {
                    throw new InvalidOperationException("House slice validation failed: P2-76 primary accent must have a halo renderer and attached point light.");
                }
            }
        }

        private static void CaptureHd2dAutonomousP2IdleEmissiveMiaShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            AnemoraSunCycleDriver sunDriver,
            string outputDirectory,
            string fileName,
            string label,
            float sampleSeconds,
            SunPreset sunPreset,
            bool emissiveVisible,
            float windowEmissionStrength,
            float fieldOfView,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            ICollection<string> shotRows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.MiaInterior);
            controller.ForcePlayerCurrentLocalForReview(Chapter1MiaRuntimePosition + new Vector3(-0.72f, 0.02f, -0.54f));
            controller.ClosePortal();
            guide.ApplyActiveTimeIsolationForReview();
            sunDriver.ApplyPreset(sunPreset, true);
            Shader.SetGlobalFloat(Hd2dAutonomousP0WindowEmissionStrengthGlobalName, Mathf.Max(0f, windowEmissionStrength));
            realtimeRig.ApplyNowForReview();
            SampleHd2dAutonomousP2IdleEmissiveReviewTime(sampleSeconds);
            SetHd2dAutonomousP2IdleEmissiveAccentVisible(emissiveVisible);
            SetHd2dAutonomousP2IdleEmissiveAccentMultipliers(emissiveVisible ? 1f : 0f, emissiveVisible ? 1f : 0f);

            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            camera.cullingMask = currentBit | playerBit;
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 120f;
            EnsureReviewCameraPostProcessingForCapture(camera);
            PositionCloseReviewCamera(
                camera,
                ResolveHd2dAutonomousP2IdleEmissivePrimaryFocusWorld(controller),
                cameraOffset,
                lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            Physics.SyncTransforms();
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | {sunPreset} | {sampleSeconds:0.###} | {FormatBool(emissiveVisible)} | {windowEmissionStrength:0.###} | {SumHd2dAutonomousP2IdleEmissiveVerticalMotion():0.####} | {SumHd2dAutonomousP2IdleEmissiveLightIntensity():0.###} |");
        }

        private static void SampleHd2dAutonomousP2IdleEmissiveReviewTime(float seconds)
        {
            foreach (var marker in FindHd2dAutonomousP2IdleEmissiveMarkers())
            {
                marker?.SecondaryMotionForReview?.SampleForReview(seconds);
                marker?.EmissiveAccentForReview?.SetReviewTimeForReview(seconds);
            }
        }

        private static void SetHd2dAutonomousP2IdleEmissiveAccentVisible(bool visible)
        {
            foreach (var marker in FindHd2dAutonomousP2IdleEmissiveMarkers())
            {
                marker?.EmissiveAccentForReview?.SetReviewVisibleForReview(visible);
            }
        }

        private static void SetHd2dAutonomousP2IdleEmissiveAccentMultipliers(float emissionMultiplier, float lightMultiplier)
        {
            foreach (var marker in FindHd2dAutonomousP2IdleEmissiveMarkers())
            {
                marker?.EmissiveAccentForReview?.SetReviewMultipliersForReview(emissionMultiplier, lightMultiplier);
            }
        }

        private static void ClearHd2dAutonomousP2IdleEmissiveReviewTimes()
        {
            foreach (var marker in FindHd2dAutonomousP2IdleEmissiveMarkers())
            {
                marker?.SecondaryMotionForReview?.ClearReviewTimeForReview();
                marker?.EmissiveAccentForReview?.ClearReviewTimeForReview();
            }
        }

        private static void ResetHd2dAutonomousP2IdleEmissiveMotionPoses()
        {
            foreach (var marker in FindHd2dAutonomousP2IdleEmissiveMarkers())
            {
                marker?.SecondaryMotionForReview?.ResetPoseForReview();
            }
        }

        private static FastVsHd2dIdleEmissiveMarker[] FindHd2dAutonomousP2IdleEmissiveMarkers()
        {
            return UnityEngine.Object.FindObjectsByType<FastVsHd2dIdleEmissiveMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        private static float SumHd2dAutonomousP2IdleEmissiveVerticalMotion()
        {
            var sum = 0f;
            foreach (var marker in FindHd2dAutonomousP2IdleEmissiveMarkers())
            {
                if (marker?.SecondaryMotionForReview != null)
                {
                    sum += Mathf.Abs(marker.SecondaryMotionForReview.LastAppliedVerticalMetersForReview);
                }
            }

            return sum;
        }

        private static float SumHd2dAutonomousP2IdleEmissiveLightIntensity()
        {
            var sum = 0f;
            foreach (var marker in FindHd2dAutonomousP2IdleEmissiveMarkers())
            {
                if (marker?.EmissiveAccentForReview != null)
                {
                    sum += marker.EmissiveAccentForReview.AppliedLightIntensityForReview;
                }
            }

            return sum;
        }

        private static Vector3 ResolveHd2dAutonomousP2IdleEmissivePrimaryFocusWorld(TimeWindowPairedSpacePortalController controller)
        {
            var primary = FindHd2dAutonomousP2IdleEmissiveMarkers()
                .FirstOrDefault(marker => marker != null && marker.gameObject.name == Hd2dAutonomousP2IdleEmissiveReviewCharacterName && marker.SpriteRendererForReview != null);
            primary ??= FindHd2dAutonomousP2IdleEmissiveMarkers()
                .FirstOrDefault(marker => marker != null && marker.PrimaryReviewAccentForReview && marker.SpriteRendererForReview != null);
            if (primary != null)
            {
                var bounds = primary.SpriteRendererForReview.bounds;
                return bounds.center + new Vector3(0f, bounds.extents.y * 0.08f, 0f);
            }

            if (controller != null && controller.CurrentSpaceRootForReview != null)
            {
                return controller.CurrentSpaceRootForReview.TransformPoint(Chapter1MiaRuntimePosition + new Vector3(0f, 0.64f, 0f));
            }

            return Chapter1MiaRuntimePosition + new Vector3(0f, 0.64f, 0f);
        }

        private static void WriteHd2dAutonomousP2IdleEmissiveReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dIdleEmissiveProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics motionDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics emissiveDiff)
        {
            var markers = FindHd2dAutonomousP2IdleEmissiveMarkers()
                .Where(marker => marker != null && marker.gameObject.scene.IsValid())
                .ToArray();
            var animatedMarkers = markers.Count(marker => marker.HasFourFrameIdleLoopForReview && marker.HasSecondaryMotionForReview);
            var accentMarkers = markers.Count(marker => marker.HasEmissiveAccentForReview);
            var lines = new List<string>
            {
                "# P2-76 Idle Breathing + Secondary Motion And Emissive Accents Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data pass for 4-frame idle loops, subtle per-instance breathing/sway, and one Mia eye/lantern HDR emissive accent tied to a small point light and bloom-ready halo.",
                "- Final approval remains false; this pass records measurable A/B evidence and leaves amplitude, blink cadence, color, and canonical accent assignment to Tom.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2IdleEmissiveProfilePath}` |",
                $"| Runtime motion / accent / marker | `{Hd2dAutonomousP2IdleSecondaryMotionRuntimePath}` / `{Hd2dAutonomousP2IdleEmissiveAccentRuntimePath}` / `{Hd2dAutonomousP2IdleEmissiveMarkerRuntimePath}` |",
                $"| Mia emissive material / mask | `{Hd2dAutonomousP2IdleEmissiveMiaMaterialPath}` / `{Hd2dAutonomousP2IdleEmissiveMaskTexturePath}` |",
                $"| Halo material | `{Hd2dAutonomousP2IdleEmissiveHaloMaterialPath}` |",
                $"| Needs Tom approval | {FormatBool(profile.NeedsTomApprovalForReview)} |",
                $"| Final approved | {FormatBool(profile.FinalIdleEmissiveApprovedForReview)} |",
                $"| Loop frames | {profile.LoopFrameCountForReview} |",
                $"| Idle frequency / vertical / sway / squash | {profile.IdleFrequencyHzForReview:0.###}Hz / {profile.VerticalBreathMetersForReview:0.###}m / {profile.HorizontalSwayMetersForReview:0.###}m / {profile.SquashStretchScaleForReview:0.###} |",
                $"| Pulse frequency / amplitude | {profile.EmissivePulseFrequencyHzForReview:0.###}Hz / {profile.EmissivePulseAmplitudeForReview:0.###} |",
                $"| Sprite emission intensity / review global strength | {profile.SpriteEmissionIntensityForReview:0.###} / {profile.ReviewWindowEmissionStrengthForReview:0.###} |",
                $"| Point light intensity / range | {profile.PointLightIntensityForReview:0.###} / {profile.PointLightRangeMetersForReview:0.###}m |",
                $"| Marker counts animated / emissive | {animatedMarkers} / {accentMarkers} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                motionDiff.ToReportRow("Idle secondary motion frame A vs B"),
                emissiveDiff.ToReportRow("Night emissive disabled vs enabled"),
                string.Empty,
                "| Screenshot | Label | Sun preset | Sample seconds | Emissive visible | Window emission strength | Motion sum | Light intensity sum |",
                "|---|---|---|---:|---|---:|---:|---:|"
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
                lines.Add($"| `{file}` | P2-76 idle/emissive capture {i + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "idle_breathing_secondary_motion_emissive_accents_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static float ResolveHd2dAutonomousP2IdleEmissivePhase(string key, FastVsHd2dIdleEmissiveProfile profile)
        {
            if (profile == null || !profile.PhaseOffsetPerInstanceForReview)
            {
                return 0f;
            }

            unchecked
            {
                var hash = 17;
                for (var i = 0; i < (key ?? string.Empty).Length; i++)
                {
                    hash = hash * 31 + key[i];
                }

                var bucket = Mathf.Abs(hash % 9);
                return Mathf.Repeat(bucket * profile.PhaseStepRadiansForReview, Mathf.PI * 2f);
            }
        }

        private static float SoftCircle01(float u, float v, float centerU, float centerV, float radiusU, float radiusV)
        {
            var dx = (u - centerU) / Mathf.Max(radiusU, 0.0001f);
            var dy = (v - centerV) / Mathf.Max(radiusV, 0.0001f);
            var distance = Mathf.Sqrt((dx * dx) + (dy * dy));
            return Mathf.Clamp01(1f - distance);
        }

        private static float RoundedRect01(float u, float v, float centerU, float centerV, float halfWidth, float halfHeight, float feather)
        {
            var dx = Mathf.Abs(u - centerU) - halfWidth;
            var dy = Mathf.Abs(v - centerV) - halfHeight;
            var outside = Mathf.Sqrt(Mathf.Max(dx, 0f) * Mathf.Max(dx, 0f) + Mathf.Max(dy, 0f) * Mathf.Max(dy, 0f));
            return Mathf.Clamp01(1f - outside / Mathf.Max(feather, 0.0001f));
        }

        private static void SetHd2dAutonomousP2IdleEmissiveLayerRecursively(GameObject root, int layer)
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
                    SetHd2dAutonomousP2IdleEmissiveLayerRecursively(child.gameObject, layer);
                }
            }
        }
    }
}
