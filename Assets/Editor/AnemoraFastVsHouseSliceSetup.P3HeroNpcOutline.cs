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
        private const string Hd2dAutonomousP3HeroNpcOutlineRootName = "Current_CentralPlaza_P3_81_HeroNpcReadabilityOutlineReview";
        private const string Hd2dAutonomousP3HeroNpcOutlinePreviewName = "P3_81_HeroNpcReadabilityOutlinePreview";
        private const string Hd2dAutonomousP3HeroNpcOutlineBackdropName = "P3_81_HeroNpcOutline_WashedBackdrop";
        private const string Hd2dAutonomousP3HeroNpcOutlineBaseSpriteName = "P3_81_HeroNpcOutline_BaseSprite";
        private const string Hd2dAutonomousP3HeroNpcOutlineRendererName = "P3_81_HeroNpcOutline_OutlineRenderer";
        private const string Hd2dAutonomousP3HeroNpcOutlineProfilePath = "Assets/Settings/FastVS_HD2D_P3_HeroNpcOutlineProfile.asset";
        private const string Hd2dAutonomousP3HeroNpcOutlineProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dHeroNpcOutlineProfile.cs";
        private const string Hd2dAutonomousP3HeroNpcOutlinePreviewRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dHeroNpcOutlinePreview.cs";
        private const string Hd2dAutonomousP3HeroNpcOutlineEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P3HeroNpcOutline.cs";
        private const string Hd2dAutonomousP3HeroNpcOutlineShaderName = "Anemora/FastVS/HeroNpcSpriteOutline";
        private const string Hd2dAutonomousP3HeroNpcOutlineShaderPath = "Assets/Art/Shaders/FastVS/FastVS_HeroNpcSpriteOutline.shader";
        private const string Hd2dAutonomousP3HeroNpcOutlineSilhouetteTextureId = "hd2d_p3_81_hero_npc_outline_silhouette";
        private const string Hd2dAutonomousP3HeroNpcOutlineBackdropTextureId = "hd2d_p3_81_washed_scene_backdrop";
        private const string Hd2dAutonomousP3HeroNpcOutlineBaseMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p3_81_hero_npc_outline_base_sprite.mat";
        private const string Hd2dAutonomousP3HeroNpcOutlineMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p3_81_hero_npc_sprite_space_outline.mat";
        private const string Hd2dAutonomousP3HeroNpcOutlineBackdropMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p3_81_washed_scene_backdrop.mat";

        public static void CaptureHd2dAutonomousP3Item81HeroNpcOutlineBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var preview = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dHeroNpcOutlinePreview>(FindObjectsInactive.Include);
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || preview == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-81 hero NPC outline capture failed: review scene components are missing.");
            }

            ValidateHd2dAutonomousP3HeroNpcOutline();
            var profile = preview.ProfileForReview;
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("hero_npc_readability_outline_sprite_space");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_outline_off_washed_scene_baseline.png",
                "02_conservative_sprite_space_outline_1px.png",
                "03_stronger_sprite_space_outline_2px_option_for_tom.png",
                "04_outline_off_reset_proof.png",
                "05_close_crop_thin_features_conservative.png"
            };

            var rows = new List<string>();
            var anchorLocal = GetHd2dAutonomousP3HeroNpcOutlineAnchorLocal();
            var previousMask = camera.cullingMask;
            var previousFov = camera.fieldOfView;
            var previousOrthographic = camera.orthographic;

            try
            {
                guide.SetMovementFrozen(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ClosePortal();
                controller.ForcePlayerCurrentLocalForReview(anchorLocal + new Vector3(-0.18f, 0f, -0.78f));
                guide.ApplyActiveTimeIsolationForReview();
                Physics.SyncTransforms();
                realtimeRig.ApplyNowForReview();
                camera.cullingMask = ResolveCurrentTimeReviewCullingMask(controller, previousMask);

                CaptureHd2dAutonomousP3HeroNpcOutlineShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    false,
                    profile.ConservativeOutlineWidthTexelsForReview,
                    true,
                    false,
                    false,
                    outputDirectory,
                    screenshotFiles[0],
                    "outline off baseline in a pale washed-scene proxy",
                    rows);
                CaptureHd2dAutonomousP3HeroNpcOutlineShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    true,
                    profile.ConservativeOutlineWidthTexelsForReview,
                    true,
                    false,
                    false,
                    outputDirectory,
                    screenshotFiles[1],
                    "preferred per-character sprite-space outline, conservative 1px-class width",
                    rows);
                CaptureHd2dAutonomousP3HeroNpcOutlineShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    true,
                    profile.StrongerOutlineWidthTexelsForReview,
                    true,
                    false,
                    true,
                    outputDirectory,
                    screenshotFiles[2],
                    "stronger 2px-class option for Tom, still opt-in only",
                    rows);
                CaptureHd2dAutonomousP3HeroNpcOutlineShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    false,
                    profile.ConservativeOutlineWidthTexelsForReview,
                    true,
                    false,
                    false,
                    outputDirectory,
                    screenshotFiles[3],
                    "outline off reset proof",
                    rows);
                CaptureHd2dAutonomousP3HeroNpcOutlineShot(
                    preview,
                    camera,
                    controller.CurrentSpaceRootForReview,
                    true,
                    profile.ConservativeOutlineWidthTexelsForReview,
                    true,
                    true,
                    false,
                    outputDirectory,
                    screenshotFiles[4],
                    "close crop: conservative outline around hair tips, arms, and legs",
                    rows);
            }
            finally
            {
                preview.ApplyDefaultReviewStateForReview();
                camera.cullingMask = previousMask;
                camera.fieldOfView = previousFov;
                camera.orthographic = previousOrthographic;
                controller.ForcePlayerCurrentLocalForReview(anchorLocal);
                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                realtimeRig.ApplyNowForReview();
                AssetDatabase.SaveAssets();
            }

            var offVsConservative = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var conservativeVsStronger = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var resetDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[3]);

            WriteHd2dAutonomousP3HeroNpcOutlineReviewReport(
                outputDirectory,
                screenshotFiles,
                rows,
                preview,
                CountHd2dAutonomousP3HeroNpcOutlineOpaquePixels(EnsureHd2dAutonomousP3HeroNpcOutlineSilhouetteTexture(), profile.AlphaCutoffForReview),
                offVsConservative,
                conservativeVsStronger,
                resetDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P3-81 hero NPC readability outline review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP3HeroNpcOutline(Transform currentCentralPlazaRoot, Camera camera)
        {
            if (currentCentralPlazaRoot == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP3HeroNpcOutlineProfile();
            var baseMaterial = EnsureHd2dAutonomousP3HeroNpcOutlineBaseMaterial(profile);
            var outlineMaterial = EnsureHd2dAutonomousP3HeroNpcOutlineMaterial(profile);
            var backdropMaterial = EnsureHd2dAutonomousP3HeroNpcOutlineBackdropMaterial(profile);
            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP3HeroNpcOutlineRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP3HeroNpcOutlineRootName);
            root.transform.SetParent(currentCentralPlazaRoot, false);
            root.transform.localPosition = GetHd2dAutonomousP3HeroNpcOutlineAnchorLocal();
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;

            var billboard = root.AddComponent<FastVsPaperBillboard>();
            if (camera != null)
            {
                SerializedSet(billboard, "targetCamera", camera);
            }

            var backdrop = CreateQuad(
                Hd2dAutonomousP3HeroNpcOutlineBackdropName,
                root.transform,
                new Vector3(0f, 0.68f, 0.060f),
                new Vector3(1.22f, 1.52f, 1f),
                backdropMaterial);
            var baseSprite = CreateQuad(
                Hd2dAutonomousP3HeroNpcOutlineBaseSpriteName,
                root.transform,
                new Vector3(0f, 0.62f, 0f),
                new Vector3(0.68f, 1.04f, 1f),
                baseMaterial);
            var outline = CreateQuad(
                Hd2dAutonomousP3HeroNpcOutlineRendererName,
                root.transform,
                new Vector3(0f, 0.62f, -0.018f),
                new Vector3(0.68f, 1.04f, 1f),
                outlineMaterial);

            var backdropRenderer = ConfigureHd2dAutonomousP3HeroNpcOutlineRenderer(backdrop);
            var baseRenderer = ConfigureHd2dAutonomousP3HeroNpcOutlineRenderer(baseSprite);
            var outlineRenderer = ConfigureHd2dAutonomousP3HeroNpcOutlineRenderer(outline);

            var previewObject = new GameObject(Hd2dAutonomousP3HeroNpcOutlinePreviewName);
            previewObject.transform.SetParent(root.transform, false);
            previewObject.transform.localPosition = Vector3.zero;
            previewObject.transform.localRotation = Quaternion.identity;
            previewObject.transform.localScale = Vector3.one;
            previewObject.layer = CurrentSpaceRenderLayer;
            var preview = previewObject.AddComponent<FastVsHd2dHeroNpcOutlinePreview>();
            preview.ConfigureForReview(profile, baseRenderer, outlineRenderer, backdropRenderer);

            var landmark = root.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", "current.central_plaza.hd2d_p3_81.hero_npc_readability_outline_review");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);

            SetHd2dAutonomousP3HeroNpcOutlineLayerRecursively(root, CurrentSpaceRenderLayer);
            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(preview);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP3HeroNpcOutline()
        {
            var profile = EnsureHd2dAutonomousP3HeroNpcOutlineProfile();
            var baseMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3HeroNpcOutlineBaseMaterialPath);
            var outlineMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3HeroNpcOutlineMaterialPath);
            var backdropMaterial = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3HeroNpcOutlineBackdropMaterialPath);
            var silhouetteTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(TextureDirectory + "/FastVS_House_" + Hd2dAutonomousP3HeroNpcOutlineSilhouetteTextureId + ".asset");
            var preview = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dHeroNpcOutlinePreview>(FindObjectsInactive.Include);
            if (profile == null ||
                baseMaterial == null ||
                outlineMaterial == null ||
                backdropMaterial == null ||
                silhouetteTexture == null ||
                preview == null ||
                !preview.IsReadyForReview ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalHeroNpcOutlineApprovedForReview ||
                !profile.ConservativeDataPrepForReview ||
                !profile.SpriteSpaceOutlinePreferredForReview ||
                !profile.UrpRenderGraphEdgeDetectDeferredForReview ||
                profile.ActiveOnAwakeForReview ||
                !profile.PerCharacterOptInOnlyForReview ||
                !profile.WashedSceneFallbackPreparedForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-81 needs a conservative non-final hero NPC outline profile, opt-in sprite-space path, deferred Render Graph edge-detect, generated silhouette texture, and review proxy renderers.");
            }

            if (profile.ConservativeOutlineWidthTexelsForReview < 0.35f ||
                profile.ConservativeOutlineWidthTexelsForReview > 0.80f ||
                profile.StrongerOutlineWidthTexelsForReview < 0.90f ||
                profile.StrongerOutlineWidthTexelsForReview <= profile.ConservativeOutlineWidthTexelsForReview ||
                profile.OutlineAlphaForReview < 0.45f ||
                profile.AlphaCutoffForReview < 0.05f)
            {
                throw new InvalidOperationException("House slice validation failed: P3-81 outline profile must keep a 1px-class conservative width, a stronger Tom option, and visible alpha/cutoff values.");
            }

            if (!string.Equals(outlineMaterial.shader != null ? outlineMaterial.shader.name : string.Empty, Hd2dAutonomousP3HeroNpcOutlineShaderName, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("House slice validation failed: P3-81 outline material must use the custom sprite-space alpha-neighbor outline shader.");
            }

            var opaquePixels = CountHd2dAutonomousP3HeroNpcOutlineOpaquePixels(silhouetteTexture, profile.AlphaCutoffForReview);
            if (opaquePixels < 800 || silhouetteTexture.width != 96 || silhouetteTexture.height != 128)
            {
                throw new InvalidOperationException($"House slice validation failed: P3-81 silhouette texture must contain a non-empty 96x128 transparent sprite alpha shape (opaque={opaquePixels}).");
            }

            preview.ApplyReviewStateForReview(true, profile.ConservativeOutlineWidthTexelsForReview, true, false);
            try
            {
                var material = preview.OutlineMaterialForReview;
                if (!preview.BaseSpriteVisibleForReview ||
                    !preview.WashedBackdropVisibleForReview ||
                    !preview.OutlineVisibleForReview ||
                    material == null ||
                    Mathf.Abs(material.GetFloat("_OutlineWidthTexels") - profile.ConservativeOutlineWidthTexelsForReview) > 0.01f ||
                    Mathf.Abs(material.GetFloat("_OutlineAlpha") - (profile.OutlineAlphaForReview * 0.92f)) > 0.02f)
                {
                    throw new InvalidOperationException("House slice validation failed: P3-81 conservative review state must enable base/backdrop/outline and publish outline material width/alpha.");
                }

                preview.ApplyReviewStateForReview(true, profile.StrongerOutlineWidthTexelsForReview, true, true);
                if (Mathf.Abs(material.GetFloat("_OutlineWidthTexels") - profile.StrongerOutlineWidthTexelsForReview) > 0.01f)
                {
                    throw new InvalidOperationException("House slice validation failed: P3-81 stronger Tom option must publish a wider outline width.");
                }
            }
            finally
            {
                preview.ApplyDefaultReviewStateForReview();
            }

            if (preview.BaseSpriteVisibleForReview || preview.OutlineVisibleForReview || preview.WashedBackdropVisibleForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-81 default state must hide all review proxy renderers.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3HeroNpcOutlineShaderPath), "neighborAlpha", Hd2dAutonomousP3HeroNpcOutlineShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3HeroNpcOutlineShaderPath), "_OutlineWidthTexels", Hd2dAutonomousP3HeroNpcOutlineShaderPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3HeroNpcOutlineProfileRuntimePath), "finalHeroNpcOutlineApproved", Hd2dAutonomousP3HeroNpcOutlineProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3HeroNpcOutlinePreviewRuntimePath), "ApplyReviewStateForReview", Hd2dAutonomousP3HeroNpcOutlinePreviewRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3HeroNpcOutlineEditorPath), "UrpRenderGraphEdgeDetectDeferredForReview", Hd2dAutonomousP3HeroNpcOutlineEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP3HeroNpcOutline", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP3HeroNpcOutline", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dHeroNpcOutlineProfile EnsureHd2dAutonomousP3HeroNpcOutlineProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dHeroNpcOutlineProfile>(Hd2dAutonomousP3HeroNpcOutlineProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dHeroNpcOutlineProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP3HeroNpcOutlineProfilePath);
            }

            profile.ConfigureForReview(
                true,
                false,
                true,
                true,
                true,
                false,
                true,
                true,
                0.55f,
                1.15f,
                0.64f,
                0.16f,
                0.68f,
                new Color(0.08f, 0.10f, 0.14f, 1f),
                new Color(0.76f, 0.80f, 0.82f, 0.72f),
                "Keep this as conservative sprite-space outline data only. Tom should approve final opt-in characters, outline color, 1px/2px width, and any scene-wide Render Graph edge-detect fallback.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Material EnsureHd2dAutonomousP3HeroNpcOutlineBaseMaterial(FastVsHd2dHeroNpcOutlineProfile profile)
        {
            EnsureFolder(MaterialDirectory);
            var shader = Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-81 base sprite material shader is missing.");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3HeroNpcOutlineBaseMaterialPath);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP3HeroNpcOutlineBaseMaterialPath);
            }

            ConfigureTransparentMaterial(material, 3016, URPUnlitShaderName);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP3HeroNpcOutlineSilhouetteTexture(), Vector2.one);
            var color = new Color(0.86f, 0.84f, 0.78f, profile != null ? profile.WashedSpriteAlphaForReview : 0.68f);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            if (material.HasProperty("_ZTest"))
            {
                material.SetFloat("_ZTest", (float)CompareFunction.Always);
            }

            ApplyMaterialRole(material, "hd2d_p3_81_hero_npc_outline_base_sprite", FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureHd2dAutonomousP3HeroNpcOutlineMaterial(FastVsHd2dHeroNpcOutlineProfile profile)
        {
            EnsureFolder(MaterialDirectory);
            AssetDatabase.ImportAsset(Hd2dAutonomousP3HeroNpcOutlineShaderPath, ImportAssetOptions.ForceSynchronousImport);
            var shader = Shader.Find(Hd2dAutonomousP3HeroNpcOutlineShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-81 outline shader is missing.");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3HeroNpcOutlineMaterialPath);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP3HeroNpcOutlineMaterialPath);
            }

            ConfigureTransparentMaterial(material, 3018, Hd2dAutonomousP3HeroNpcOutlineShaderName);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP3HeroNpcOutlineSilhouetteTexture(), Vector2.one);
            var outlineColor = profile != null ? profile.OutlineColorForReview : new Color(0.08f, 0.10f, 0.14f, 1f);
            outlineColor.a = profile != null ? profile.OutlineAlphaForReview : 0.78f;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", outlineColor);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", outlineColor);
            }

            if (material.HasProperty("_OutlineWidthTexels"))
            {
                material.SetFloat("_OutlineWidthTexels", profile != null ? profile.ConservativeOutlineWidthTexelsForReview : 1.10f);
            }

            if (material.HasProperty("_OutlineAlpha"))
            {
                material.SetFloat("_OutlineAlpha", outlineColor.a);
            }

            if (material.HasProperty("_Cutoff"))
            {
                material.SetFloat("_Cutoff", profile != null ? profile.AlphaCutoffForReview : 0.16f);
            }

            if (material.HasProperty("_ZTest"))
            {
                material.SetFloat("_ZTest", (float)CompareFunction.Always);
            }

            ApplyMaterialRole(material, "hd2d_p3_81_hero_npc_sprite_space_outline", FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureHd2dAutonomousP3HeroNpcOutlineBackdropMaterial(FastVsHd2dHeroNpcOutlineProfile profile)
        {
            EnsureFolder(MaterialDirectory);
            var shader = Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-81 washed backdrop material shader is missing.");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP3HeroNpcOutlineBackdropMaterialPath);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP3HeroNpcOutlineBackdropMaterialPath);
            }

            ConfigureTransparentMaterial(material, 3009, URPUnlitShaderName);
            AssignMaterialTexture(material, EnsureHd2dAutonomousP3HeroNpcOutlineBackdropTexture(), Vector2.one);
            var color = profile != null ? profile.WashedBackdropTintForReview : new Color(0.76f, 0.80f, 0.82f, 0.72f);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            if (material.HasProperty("_ZTest"))
            {
                material.SetFloat("_ZTest", (float)CompareFunction.Always);
            }

            ApplyMaterialRole(material, "hd2d_p3_81_washed_scene_backdrop", FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Texture2D EnsureHd2dAutonomousP3HeroNpcOutlineSilhouetteTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP3HeroNpcOutlineSilhouetteTextureId,
                96,
                128,
                FilterMode.Bilinear,
                SampleHd2dAutonomousP3HeroNpcOutlineSilhouette);
        }

        private static Texture2D EnsureHd2dAutonomousP3HeroNpcOutlineBackdropTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP3HeroNpcOutlineBackdropTextureId,
                96,
                96,
                FilterMode.Bilinear,
                (x, y) =>
                {
                    var u = x / 95f;
                    var v = y / 95f;
                    var diagonal = Mathf.Clamp01((u * 0.38f) + (v * 0.62f));
                    var stripe = (Mathf.Sin((u * 16.0f) + (v * 7.0f)) * 0.5f) + 0.5f;
                    var tone = 0.74f + (diagonal * 0.12f) + (stripe * 0.025f);
                    return new Color(tone, tone + 0.015f, tone + 0.030f, 0.72f);
                });
        }

        private static MeshRenderer ConfigureHd2dAutonomousP3HeroNpcOutlineRenderer(GameObject gameObject)
        {
            var renderer = gameObject != null ? gameObject.GetComponent<MeshRenderer>() : null;
            if (renderer == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-81 outline proxy is missing a MeshRenderer.");
            }

            renderer.enabled = false;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            return renderer;
        }

        private static void CaptureHd2dAutonomousP3HeroNpcOutlineShot(
            FastVsHd2dHeroNpcOutlinePreview preview,
            Camera camera,
            Transform currentRoot,
            bool outlineEnabled,
            float outlineWidthTexels,
            bool washedScene,
            bool closeCrop,
            bool strongerOption,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            preview.ApplyReviewStateForReview(outlineEnabled, outlineWidthTexels, washedScene, strongerOption && outlineEnabled);
            var anchor = currentRoot.TransformPoint(GetHd2dAutonomousP3HeroNpcOutlineAnchorLocal());
            camera.orthographic = false;
            camera.fieldOfView = closeCrop ? 28f : 34f;
            PositionCloseReviewCamera(
                camera,
                anchor,
                closeCrop ? new Vector3(0.34f, 1.02f, -1.42f) : new Vector3(0.82f, 1.38f, -2.42f),
                closeCrop ? new Vector3(0.00f, 0.72f, 0.04f) : new Vector3(0.00f, 0.72f, 0.12f));
            FaceHd2dAutonomousP3HeroNpcOutlineProxyToCamera(preview, camera);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            rows.Add($"| `{fileName}` | {label} | {FormatBool(outlineEnabled)} | {outlineWidthTexels:0.###} | {FormatBool(washedScene)} | {FormatBool(closeCrop)} | {FormatBool(preview.OutlineVisibleForReview)} |");
        }

        private static void FaceHd2dAutonomousP3HeroNpcOutlineProxyToCamera(FastVsHd2dHeroNpcOutlinePreview preview, Camera camera)
        {
            if (preview == null || camera == null)
            {
                return;
            }

            var direction = -camera.transform.forward;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = Vector3.forward;
            }

            preview.transform.parent.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }

        private static void WriteHd2dAutonomousP3HeroNpcOutlineReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> rows,
            FastVsHd2dHeroNpcOutlinePreview preview,
            int silhouetteOpaquePixels,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics offVsConservative,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics conservativeVsStronger,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics resetDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var profile = preview.ProfileForReview;
            var lines = new List<string>
            {
                "# P3-81 Hero NPC Readability Outline Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for an optional per-character readability outline. Runtime default is hidden/off; this does not approve final character art treatment.",
                "- Recommendation: " + profile.RecommendationForReview,
                "- Technical note: sprite-space alpha-neighbor outline is preferred for opt-in hero NPCs; full-screen URP Render Graph edge-detect is deferred as a scene-wide fallback only.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP3HeroNpcOutlineProfilePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalHeroNpcOutlineApprovedForReview)} |",
                $"| Sprite-space preferred / Render Graph edge-detect deferred | {FormatBool(profile.SpriteSpaceOutlinePreferredForReview)} / {FormatBool(profile.UrpRenderGraphEdgeDetectDeferredForReview)} |",
                $"| Active on awake / opt-in only | {FormatBool(profile.ActiveOnAwakeForReview)} / {FormatBool(profile.PerCharacterOptInOnlyForReview)} |",
                $"| Conservative / stronger outline width | {profile.ConservativeOutlineWidthTexelsForReview:0.###} / {profile.StrongerOutlineWidthTexelsForReview:0.###} texels |",
                $"| Outline alpha / cutoff / color | {profile.OutlineAlphaForReview:0.###} / {profile.AlphaCutoffForReview:0.###} / {FormatColorForReport(profile.OutlineColorForReview)} |",
                $"| Washed sprite alpha / backdrop tint | {profile.WashedSpriteAlphaForReview:0.###} / {FormatColorForReport(profile.WashedBackdropTintForReview)} |",
                $"| Generated silhouette opaque pixels | {silhouetteOpaquePixels} |",
                $"| Shader | `{Hd2dAutonomousP3HeroNpcOutlineShaderPath}` |",
                string.Empty,
                "| Capture | Label | Outline on | Width texels | Washed scene | Close crop | Renderer visible |",
                "|---|---|---|---:|---|---|---|"
            };
            lines.AddRange(rows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                offVsConservative.ToReportRow("outline off baseline vs conservative sprite-space outline"),
                conservativeVsStronger.ToReportRow("conservative 1px-class outline vs stronger 2px-class Tom option"),
                resetDiff.ToReportRow("outline off baseline vs outline-off reset proof"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Washed scene baseline with no outline. |",
                $"| `{screenshotFiles[1]}` | Conservative per-character sprite-space outline; target is a subtle continuous 1px-class silhouette edge. |",
                $"| `{screenshotFiles[2]}` | Stronger 2px-class option for Tom to compare, not final approval. |",
                $"| `{screenshotFiles[3]}` | Reset proof after enabled captures, returning to outline off. |",
                $"| `{screenshotFiles[4]}` | Close crop showing thin hair/arm/leg features with conservative outline. |"
            });

            File.WriteAllText(Path.Combine(outputDirectory, "hero_npc_readability_outline_sprite_space_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Vector3 GetHd2dAutonomousP3HeroNpcOutlineAnchorLocal()
        {
            return CentralPlazaVsCenter + new Vector3(-0.58f, 0.02f, 2.48f);
        }

        private static Color SampleHd2dAutonomousP3HeroNpcOutlineSilhouette(int x, int y)
        {
            var p = new Vector2((x + 0.5f) / 96f, (y + 0.5f) / 128f);
            var alpha = 0f;
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3EllipseAlpha(p, new Vector2(0.50f, 0.73f), new Vector2(0.145f, 0.145f), 0.10f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3EllipseAlpha(p, new Vector2(0.47f, 0.82f), new Vector2(0.185f, 0.075f), 0.08f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3SegmentAlpha(p, new Vector2(0.50f, 0.62f), new Vector2(0.50f, 0.35f), 0.150f, 0.018f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3SoftRectAlpha(p, new Vector2(0.50f, 0.42f), new Vector2(0.180f, 0.145f), 0.026f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3SegmentAlpha(p, new Vector2(0.37f, 0.60f), new Vector2(0.255f, 0.455f), 0.034f, 0.016f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3SegmentAlpha(p, new Vector2(0.63f, 0.60f), new Vector2(0.755f, 0.490f), 0.034f, 0.016f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3SegmentAlpha(p, new Vector2(0.44f, 0.305f), new Vector2(0.385f, 0.145f), 0.043f, 0.016f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3SegmentAlpha(p, new Vector2(0.56f, 0.305f), new Vector2(0.620f, 0.145f), 0.043f, 0.016f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3SegmentAlpha(p, new Vector2(0.36f, 0.775f), new Vector2(0.260f, 0.690f), 0.022f, 0.014f));
            alpha = Mathf.Max(alpha, Hd2dAutonomousP3SegmentAlpha(p, new Vector2(0.60f, 0.800f), new Vector2(0.720f, 0.720f), 0.022f, 0.014f));
            alpha = Mathf.Clamp01(alpha);
            var shade = Mathf.Clamp01((p.y * 0.38f) + ((1f - p.x) * 0.18f));
            return new Color(0.45f + shade * 0.14f, 0.40f + shade * 0.12f, 0.34f + shade * 0.10f, alpha);
        }

        private static float Hd2dAutonomousP3EllipseAlpha(Vector2 p, Vector2 center, Vector2 radius, float softness)
        {
            var d = new Vector2((p.x - center.x) / Mathf.Max(radius.x, 0.0001f), (p.y - center.y) / Mathf.Max(radius.y, 0.0001f)).magnitude;
            return 1f - Hd2dAutonomousP3SmoothStep01(1f - softness, 1f + softness, d);
        }

        private static float Hd2dAutonomousP3SegmentAlpha(Vector2 p, Vector2 a, Vector2 b, float radius, float softness)
        {
            var segment = b - a;
            var t = Mathf.Clamp01(Vector2.Dot(p - a, segment) / Mathf.Max(segment.sqrMagnitude, 0.0001f));
            var closest = a + segment * t;
            var distance = Vector2.Distance(p, closest);
            return 1f - Hd2dAutonomousP3SmoothStep01(radius - softness, radius + softness, distance);
        }

        private static float Hd2dAutonomousP3SoftRectAlpha(Vector2 p, Vector2 center, Vector2 halfSize, float softness)
        {
            var delta = new Vector2(Mathf.Abs(p.x - center.x) - halfSize.x, Mathf.Abs(p.y - center.y) - halfSize.y);
            var outside = new Vector2(Mathf.Max(delta.x, 0f), Mathf.Max(delta.y, 0f)).magnitude;
            return 1f - Hd2dAutonomousP3SmoothStep01(0f, softness, outside);
        }

        private static float Hd2dAutonomousP3SmoothStep01(float edge0, float edge1, float value)
        {
            var t = Mathf.Clamp01((value - edge0) / Mathf.Max(edge1 - edge0, 0.0001f));
            return t * t * (3f - (2f * t));
        }

        private static int CountHd2dAutonomousP3HeroNpcOutlineOpaquePixels(Texture2D texture, float cutoff)
        {
            if (texture == null)
            {
                return 0;
            }

            var count = 0;
            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    if (texture.GetPixel(x, y).a >= cutoff)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static void SetHd2dAutonomousP3HeroNpcOutlineLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetHd2dAutonomousP3HeroNpcOutlineLayerRecursively(child.gameObject, layer);
            }
        }
    }
}
