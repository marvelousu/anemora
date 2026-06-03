using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.TimeManagement;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2MenuLayoutRootName = "FastVS_HD2D_P2_71_MenuLayoutSystem";
        private const string Hd2dAutonomousP2MenuLayoutProfilePath = "Assets/Settings/FastVS_HD2D_P2_MenuLayoutProfile.asset";
        private const string Hd2dAutonomousP2MenuLayoutProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dMenuLayoutProfile.cs";
        private const string Hd2dAutonomousP2MenuLayoutPresenterRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dMenuLayoutPresenter.cs";
        private const int Hd2dAutonomousP2MenuReviewWidth = 1920;
        private const int Hd2dAutonomousP2MenuReviewHeight = 1080;

        public static void CaptureHd2dAutonomousP2Item71MenuLayoutBatch()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2MenuLayoutRootName) == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-71 menu layout capture failed: review root is missing. Run BuildAndValidateBatch before capture.");
            }

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var presenter = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dMenuLayoutPresenter>(FindObjectsInactive.Include);
            var profile = EnsureHd2dAutonomousP2MenuLayoutProfile();
            if (controller == null || visibility == null || guide == null || camera == null || presenter == null || profile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-71 menu layout capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2MenuLayoutSystem();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("hd2d_menu_layout_system");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_live_scene_no_menu_baseline.png",
                "02_inventory_menu_blur_dim_thirds_grid.png",
                "03_pause_menu_shared_frame_selected_highlight.png",
                "04_inventory_selected_second_item_option_for_tom.png",
                "05_inventory_1440p_integer_scale_check.png"
            };

            var previousMask = camera.cullingMask;
            try
            {
                guide.SetMovementFrozen(true);
                controller.ClosePortal();
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.32f, 0.02f, 3.10f));
                guide.ApplyActiveTimeIsolationForReview();
                presenter.HideMenuForReview();
                IncludeUiLayerInReviewCamera(camera);
                camera.cullingMask = ResolveCurrentTimeOnlyCullingMaskForP2Menu(controller, previousMask);
                PositionReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(-0.25f, 0.02f, 3.10f)));
                WarmUpCameraRender(camera);

                CaptureHd2dAutonomousP2MenuSceneBase(camera, outputDirectory, screenshotFiles[0], Hd2dAutonomousP2MenuReviewWidth, Hd2dAutonomousP2MenuReviewHeight);
                CaptureHd2dAutonomousP2MenuComposite(camera, profile, presenter, outputDirectory, screenshotFiles[1], Hd2dAutonomousP2MenuReviewWidth, Hd2dAutonomousP2MenuReviewHeight, FastVsHd2dMenuLayoutMode.Inventory, 0);
                CaptureHd2dAutonomousP2MenuComposite(camera, profile, presenter, outputDirectory, screenshotFiles[2], Hd2dAutonomousP2MenuReviewWidth, Hd2dAutonomousP2MenuReviewHeight, FastVsHd2dMenuLayoutMode.Pause, 1);
                CaptureHd2dAutonomousP2MenuComposite(camera, profile, presenter, outputDirectory, screenshotFiles[3], Hd2dAutonomousP2MenuReviewWidth, Hd2dAutonomousP2MenuReviewHeight, FastVsHd2dMenuLayoutMode.Inventory, 1);
                CaptureHd2dAutonomousP2MenuComposite(camera, profile, presenter, outputDirectory, screenshotFiles[4], 2560, 1440, FastVsHd2dMenuLayoutMode.Inventory, 0);
            }
            finally
            {
                camera.cullingMask = previousMask;
                presenter.HideMenuForReview();
                controller.ClosePortal();
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                AssetDatabase.SaveAssets();
            }

            var blurDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var selectedDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[3]);
            var pauseDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            WriteHd2dAutonomousP2MenuLayoutReviewReport(outputDirectory, screenshotFiles, profile, presenter, blurDiff, selectedDiff, pauseDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-71 HD-2D menu layout review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2MenuLayoutSystem(Camera camera)
        {
            var profile = EnsureHd2dAutonomousP2MenuLayoutProfile();
            var font = EnsureFastVsDialogueTmpFontAsset();
            var frameTexture = EnsureHd2dAutonomousP0DialogueFrameTexture();
            var nameplateTexture = EnsureHd2dAutonomousP0DialogueNameplateTexture();
            var frameSprite = AssetDatabase.LoadAssetAtPath<Sprite>(Hd2dAutonomousP0DialogueFrameTexturePath);
            var nameplateSprite = AssetDatabase.LoadAssetAtPath<Sprite>(Hd2dAutonomousP0DialogueNameplateTexturePath);
            if (camera == null || profile == null || font == null || frameTexture == null || nameplateTexture == null || frameSprite == null || nameplateSprite == null)
            {
                return;
            }

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2MenuLayoutRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2MenuLayoutRootName);
            var presenter = root.AddComponent<FastVsHd2dMenuLayoutPresenter>();
            presenter.ConfigureForReview(profile, camera, font, frameTexture, nameplateTexture, frameSprite, nameplateSprite);
            presenter.HideMenuForReview();
            SetHd2dAutonomousP2MenuLayoutLayerRecursively(root, LayerMask.NameToLayer("UI") >= 0 ? LayerMask.NameToLayer("UI") : 0);
            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(presenter);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2MenuLayoutSystem()
        {
            var profile = EnsureHd2dAutonomousP2MenuLayoutProfile();
            var presenter = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dMenuLayoutPresenter>(FindObjectsInactive.Include);
            var camera = Camera.main;
            if (profile == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalMenuLayoutApprovedForReview ||
                !profile.ReuseDialogueNineSliceFrameForReview ||
                !profile.ThirdsGridLayoutEnabledForReview ||
                !profile.SelectedFrameSwapEnabledForReview ||
                !profile.ScreenSpaceCameraCanvasForReview ||
                !profile.IntegerPixelUiScaleForReview ||
                !profile.EditorCompositeBackdropBlurEnabledForReview ||
                !profile.RuntimeRendererFeatureDeferredForTomForReview ||
                profile.BackdropBlurRadiusPixelsForReview < 4 ||
                profile.BackdropDimAlphaForReview < 0.35f ||
                presenter == null ||
                camera == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-71 needs a conservative non-final menu layout profile, shared-frame plan, thirds-grid layout, selected-state frame swap, and editor-composited blur proof.");
            }

            presenter.SetPixelReviewOutputHeightForReview(1080);
            presenter.ShowMenuForReview(FastVsHd2dMenuLayoutMode.Inventory, 1);
            Canvas.ForceUpdateCanvases();
            if (!presenter.IsReadyForReview ||
                presenter.CanvasRenderModeForReview != "ScreenSpaceCamera" ||
                !presenter.CanvasPixelPerfectForReview ||
                presenter.CanvasScaleFactorForReview < 1f ||
                presenter.PanelCountForReview < 4 ||
                !presenter.UsesSharedDialogueFrameForReview ||
                !presenter.BackdropDimmingEnabledForReview ||
                !presenter.DepthBlurBackdropPreparedForReview ||
                presenter.FinalMenuLayoutApprovedForReview ||
                presenter.SelectedIndexForReview != 1 ||
                presenter.SelectedFrameImageTypeForReview != "Sliced" ||
                Mathf.Abs(presenter.InventoryPanelAnchorForReview.x - (1f / 3f)) > 0.025f ||
                Mathf.Abs(presenter.DetailPanelAnchorForReview.x - (2f / 3f)) > 0.025f ||
                presenter.InventoryPanelSizeForReview.x < 480f ||
                presenter.DetailPanelSizeForReview.x < 540f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-71 menu presenter must show shared sliced-frame panels on screen thirds with dim backdrop and a selected sliced-frame row.");
            }

            presenter.HideMenuForReview();
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2MenuLayoutProfileRuntimePath), "finalMenuLayoutApproved", Hd2dAutonomousP2MenuLayoutProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2MenuLayoutProfileRuntimePath), "runtimeRendererFeatureDeferredForTom", Hd2dAutonomousP2MenuLayoutProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2MenuLayoutPresenterRuntimePath), "Image.Type.Sliced", Hd2dAutonomousP2MenuLayoutPresenterRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2MenuLayoutPresenterRuntimePath), "CanvasScaler.ScaleMode.ConstantPixelSize", Hd2dAutonomousP2MenuLayoutPresenterRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2MenuLayoutPresenterRuntimePath), "ShowMenuForReview", Hd2dAutonomousP2MenuLayoutPresenterRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.P2MenuLayoutSystem.cs"), "CompositeHd2dAutonomousP2MenuLayoutHud", "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2MenuLayoutSystem.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2MenuLayoutSystem", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2MenuLayoutSystem", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dMenuLayoutProfile EnsureHd2dAutonomousP2MenuLayoutProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dMenuLayoutProfile>(Hd2dAutonomousP2MenuLayoutProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dMenuLayoutProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2MenuLayoutProfilePath);
            }

            profile.ConfigureForReview(
                0.58f,
                0.54f,
                8,
                new Vector2(1f / 3f, 0.52f),
                new Vector2(2f / 3f, 0.52f),
                new Vector2(0.5f, 0.84f),
                new Vector2(0.5f, 0.15f),
                new Vector2(560f, 660f),
                new Vector2(620f, 660f),
                new Vector2(720f, 96f),
                new Vector2(940f, 74f),
                new Color(0.024f, 0.020f, 0.024f, 0.92f),
                new Color(1.0f, 0.78f, 0.36f, 0.92f),
                new Color(1.0f, 0.90f, 0.62f, 1f),
                new Color(0.82f, 0.80f, 0.72f, 0.82f),
                "Keep this as conservative P2-71 menu layout data only. Tom should approve final frame art, blur strength, panel density, selected-state color, and whether a real Render Graph blur pass replaces the editor composite proof.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static void CaptureHd2dAutonomousP2MenuSceneBase(Camera camera, string outputDirectory, string fileName, int width, int height)
        {
            var outputPath = Path.Combine(outputDirectory, fileName);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, outputPath, width, height);
            ValidateScreenshotOutputExists(outputDirectory, fileName);
        }

        private static void CaptureHd2dAutonomousP2MenuComposite(
            Camera camera,
            FastVsHd2dMenuLayoutProfile profile,
            FastVsHd2dMenuLayoutPresenter presenter,
            string outputDirectory,
            string fileName,
            int width,
            int height,
            FastVsHd2dMenuLayoutMode mode,
            int selectedIndex)
        {
            var outputPath = Path.Combine(outputDirectory, fileName);
            presenter.SetPixelReviewOutputHeightForReview(height);
            presenter.ShowMenuForReview(mode, selectedIndex);
            Canvas.ForceUpdateCanvases();
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, outputPath, width, height);
            CompositeHd2dAutonomousP2MenuLayoutHud(outputPath, width, height, height >= 2160 ? 2 : 1, profile, mode, selectedIndex);
            ValidateScreenshotOutputExists(outputDirectory, fileName);
        }

        private static void CompositeHd2dAutonomousP2MenuLayoutHud(string imagePath, int width, int height, int uiScale, FastVsHd2dMenuLayoutProfile profile, FastVsHd2dMenuLayoutMode mode, int selectedIndex)
        {
            var target = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            var atlasCopy = (Texture2D)null;
            try
            {
                if (!ImageConversion.LoadImage(target, File.ReadAllBytes(imagePath)))
                {
                    throw new InvalidOperationException($"Fast VS autonomous P2-71 menu composite failed: could not load {imagePath}.");
                }

                var fontAsset = EnsureFastVsDialogueTmpFontAsset();
                var atlas = fontAsset != null && fontAsset.atlasTextures != null && fontAsset.atlasTextures.Length > 0
                    ? fontAsset.atlasTextures[0]
                    : null;
                var frameTexture = EnsureHd2dAutonomousP0DialogueFrameTexture();
                var nameplateTexture = EnsureHd2dAutonomousP0DialogueNameplateTexture();
                if (fontAsset == null || atlas == null || frameTexture == null || nameplateTexture == null)
                {
                    throw new InvalidOperationException("Fast VS autonomous P2-71 menu composite failed: missing TMP font atlas or shared dialogue 9-slice texture.");
                }

                atlasCopy = CopyHd2dAutonomousP0PixelFontAtlasForReadback(atlas);
                var targetPixels = target.GetPixels32();
                if (profile.EditorCompositeBackdropBlurEnabledForReview && profile.BackdropBlurRadiusPixelsForReview > 0)
                {
                    targetPixels = ApplyHd2dAutonomousP2MenuSeparableBlur(targetPixels, width, height, profile.BackdropBlurRadiusPixelsForReview);
                }

                BlendHd2dAutonomousP0PixelFontRect(targetPixels, width, height, 0, 0, width, height, new Color(0f, 0f, 0f, profile.BackdropDimAlphaForReview));
                var atlasPixels = atlasCopy.GetPixels32();
                var framePixels = frameTexture.GetPixels32();
                var nameplatePixels = nameplateTexture.GetPixels32();
                DrawHd2dAutonomousP2MenuPanel(targetPixels, width, height, framePixels, frameTexture.width, frameTexture.height, nameplatePixels, nameplateTexture.width, nameplateTexture.height, atlasPixels, atlasCopy.width, atlasCopy.height, fontAsset, profile, uiScale, profile.HeaderPanelAnchorForReview, profile.HeaderPanelSizeForReview, mode == FastVsHd2dMenuLayoutMode.Inventory ? "INVENTORY" : "PAUSE", "shared frame");
                DrawHd2dAutonomousP2MenuListPanel(targetPixels, width, height, framePixels, frameTexture.width, frameTexture.height, nameplatePixels, nameplateTexture.width, nameplateTexture.height, atlasPixels, atlasCopy.width, atlasCopy.height, fontAsset, profile, uiScale, mode, selectedIndex);
                DrawHd2dAutonomousP2MenuDetailPanel(targetPixels, width, height, framePixels, frameTexture.width, frameTexture.height, atlasPixels, atlasCopy.width, atlasCopy.height, fontAsset, profile, uiScale, mode, selectedIndex);
                DrawHd2dAutonomousP2MenuFooterPanel(targetPixels, width, height, framePixels, frameTexture.width, frameTexture.height, atlasPixels, atlasCopy.width, atlasCopy.height, fontAsset, profile, uiScale);

                target.SetPixels32(targetPixels);
                target.Apply(false, false);
                ForceOpaqueAlpha(target);
                File.WriteAllBytes(imagePath, target.EncodeToPNG());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(target);
                if (atlasCopy != null)
                {
                    UnityEngine.Object.DestroyImmediate(atlasCopy);
                }
            }
        }

        private static Color32[] ApplyHd2dAutonomousP2MenuSeparableBlur(Color32[] source, int width, int height, int radius)
        {
            radius = Mathf.Clamp(radius, 0, 24);
            if (radius <= 0)
            {
                return source;
            }

            var horizontal = new Color32[source.Length];
            var output = new Color32[source.Length];
            var span = (radius * 2) + 1;
            for (var y = 0; y < height; y++)
            {
                var row = y * width;
                for (var x = 0; x < width; x++)
                {
                    var r = 0;
                    var g = 0;
                    var b = 0;
                    for (var dx = -radius; dx <= radius; dx++)
                    {
                        var sx = Mathf.Clamp(x + dx, 0, width - 1);
                        var c = source[row + sx];
                        r += c.r;
                        g += c.g;
                        b += c.b;
                    }

                    horizontal[row + x] = new Color32((byte)(r / span), (byte)(g / span), (byte)(b / span), 255);
                }
            }

            for (var y = 0; y < height; y++)
            {
                var row = y * width;
                for (var x = 0; x < width; x++)
                {
                    var r = 0;
                    var g = 0;
                    var b = 0;
                    for (var dy = -radius; dy <= radius; dy++)
                    {
                        var sy = Mathf.Clamp(y + dy, 0, height - 1);
                        var c = horizontal[(sy * width) + x];
                        r += c.r;
                        g += c.g;
                        b += c.b;
                    }

                    output[row + x] = new Color32((byte)(r / span), (byte)(g / span), (byte)(b / span), 255);
                }
            }

            return output;
        }

        private static void DrawHd2dAutonomousP2MenuPanel(
            Color32[] pixels,
            int width,
            int height,
            Color32[] framePixels,
            int frameWidth,
            int frameHeight,
            Color32[] nameplatePixels,
            int nameplateWidth,
            int nameplateHeight,
            Color32[] atlasPixels,
            int atlasWidth,
            int atlasHeight,
            TMP_FontAsset fontAsset,
            FastVsHd2dMenuLayoutProfile profile,
            int uiScale,
            Vector2 anchor,
            Vector2 size,
            string title,
            string caption)
        {
            var panelWidth = Mathf.RoundToInt(size.x * uiScale);
            var panelHeight = Mathf.RoundToInt(size.y * uiScale);
            var panelX = Mathf.RoundToInt((width * anchor.x) - (panelWidth * 0.5f));
            var panelBottomY = Mathf.RoundToInt((height * anchor.y) - (panelHeight * 0.5f));
            DrawHd2dAutonomousP2MenuFrame(pixels, width, height, framePixels, frameWidth, frameHeight, panelX, panelBottomY, panelWidth, panelHeight, uiScale, profile.PanelFillColorForReview);
            var tabWidth = Mathf.RoundToInt(Mathf.Min(336f * uiScale, panelWidth - (80f * uiScale)));
            var tabHeight = Mathf.RoundToInt(54f * uiScale);
            var tabX = panelX + Mathf.RoundToInt(38f * uiScale);
            var tabBottom = panelBottomY + panelHeight - Mathf.RoundToInt(62f * uiScale);
            DrawHd2dAutonomousP0NineSliceTexture(pixels, width, height, nameplatePixels, nameplateWidth, nameplateHeight, tabX, tabBottom, tabWidth, tabHeight, Hd2dAutonomousP0DialogueNameplateSpriteBorder, uiScale);
            DrawHd2dAutonomousP0PixelFontText(pixels, width, height, atlasPixels, atlasWidth, atlasHeight, fontAsset, title, tabX + Mathf.RoundToInt(20f * uiScale), height - tabBottom - tabHeight + Mathf.RoundToInt(13f * uiScale), 32 * uiScale, new Color32(255, 219, 132, 255));
            DrawHd2dAutonomousP0PixelFontText(pixels, width, height, atlasPixels, atlasWidth, atlasHeight, fontAsset, caption, panelX + panelWidth - Mathf.RoundToInt(220f * uiScale), height - panelBottomY - panelHeight + Mathf.RoundToInt(28f * uiScale), 16 * uiScale, new Color32(212, 190, 148, 255));
        }

        private static void DrawHd2dAutonomousP2MenuListPanel(
            Color32[] pixels,
            int width,
            int height,
            Color32[] framePixels,
            int frameWidth,
            int frameHeight,
            Color32[] nameplatePixels,
            int nameplateWidth,
            int nameplateHeight,
            Color32[] atlasPixels,
            int atlasWidth,
            int atlasHeight,
            TMP_FontAsset fontAsset,
            FastVsHd2dMenuLayoutProfile profile,
            int uiScale,
            FastVsHd2dMenuLayoutMode mode,
            int selectedIndex)
        {
            var labels = mode == FastVsHd2dMenuLayoutMode.Inventory
                ? new[] { "Cinder Key", "Timewriter Brush", "Empty Vial", "Folded Map", "Worn Charm" }
                : new[] { "Continue", "Inventory", "Options", "Load", "Quit" };
            DrawHd2dAutonomousP2MenuPanel(pixels, width, height, framePixels, frameWidth, frameHeight, nameplatePixels, nameplateWidth, nameplateHeight, atlasPixels, atlasWidth, atlasHeight, fontAsset, profile, uiScale, profile.InventoryPanelThirdsAnchorForReview, profile.InventoryPanelSizeForReview, "ITEMS", "left third");
            var panelWidth = Mathf.RoundToInt(profile.InventoryPanelSizeForReview.x * uiScale);
            var panelHeight = Mathf.RoundToInt(profile.InventoryPanelSizeForReview.y * uiScale);
            var panelX = Mathf.RoundToInt((width * profile.InventoryPanelThirdsAnchorForReview.x) - (panelWidth * 0.5f));
            var panelBottomY = Mathf.RoundToInt((height * profile.InventoryPanelThirdsAnchorForReview.y) - (panelHeight * 0.5f));
            var rowWidth = panelWidth - Mathf.RoundToInt(92f * uiScale);
            var rowHeight = Mathf.RoundToInt(54f * uiScale);
            for (var i = 0; i < labels.Length; i++)
            {
                var rowX = panelX + Mathf.RoundToInt(46f * uiScale);
                var rowBottom = panelBottomY + panelHeight - Mathf.RoundToInt((154f + (i * 78f)) * uiScale);
                var selected = i == Mathf.Clamp(selectedIndex, 0, labels.Length - 1);
                if (selected)
                {
                    BlendHd2dAutonomousP0PixelFontRect(pixels, width, height, rowX + Mathf.RoundToInt(8f * uiScale), rowBottom - Mathf.RoundToInt(6f * uiScale), rowWidth, rowHeight, new Color(0f, 0f, 0f, 0.38f));
                    DrawHd2dAutonomousP0NineSliceTexture(pixels, width, height, framePixels, frameWidth, frameHeight, rowX, rowBottom, rowWidth, rowHeight, Hd2dAutonomousP0DialogueFrameSpriteBorder, uiScale);
                    BlendHd2dAutonomousP0PixelFontRect(pixels, width, height, rowX + Mathf.RoundToInt(18f * uiScale), rowBottom + Mathf.RoundToInt(12f * uiScale), Mathf.RoundToInt(4f * uiScale), rowHeight - Mathf.RoundToInt(24f * uiScale), profile.SelectedFrameTintForReview);
                }
                else
                {
                    BlendHd2dAutonomousP0PixelFontRect(pixels, width, height, rowX, rowBottom, rowWidth, rowHeight, new Color(0.02f, 0.018f, 0.02f, 0.18f));
                }

                var color = selected ? (Color32)profile.SelectedTextColorForReview : (Color32)profile.UnselectedTextColorForReview;
                DrawHd2dAutonomousP0PixelFontText(pixels, width, height, atlasPixels, atlasWidth, atlasHeight, fontAsset, labels[i], rowX + Mathf.RoundToInt(36f * uiScale), height - rowBottom - rowHeight + Mathf.RoundToInt(12f * uiScale), 24 * uiScale, color);
            }
        }

        private static void DrawHd2dAutonomousP2MenuDetailPanel(
            Color32[] pixels,
            int width,
            int height,
            Color32[] framePixels,
            int frameWidth,
            int frameHeight,
            Color32[] atlasPixels,
            int atlasWidth,
            int atlasHeight,
            TMP_FontAsset fontAsset,
            FastVsHd2dMenuLayoutProfile profile,
            int uiScale,
            FastVsHd2dMenuLayoutMode mode,
            int selectedIndex)
        {
            var labels = mode == FastVsHd2dMenuLayoutMode.Inventory
                ? new[] { "Cinder Key", "Timewriter Brush", "Empty Vial", "Folded Map", "Worn Charm" }
                : new[] { "Continue", "Inventory", "Options", "Load", "Quit" };
            var panelWidth = Mathf.RoundToInt(profile.DetailPanelSizeForReview.x * uiScale);
            var panelHeight = Mathf.RoundToInt(profile.DetailPanelSizeForReview.y * uiScale);
            var panelX = Mathf.RoundToInt((width * profile.DetailPanelThirdsAnchorForReview.x) - (panelWidth * 0.5f));
            var panelBottomY = Mathf.RoundToInt((height * profile.DetailPanelThirdsAnchorForReview.y) - (panelHeight * 0.5f));
            DrawHd2dAutonomousP2MenuFrame(pixels, width, height, framePixels, frameWidth, frameHeight, panelX, panelBottomY, panelWidth, panelHeight, uiScale, profile.PanelFillColorForReview);
            var itemName = labels[Mathf.Clamp(selectedIndex, 0, labels.Length - 1)];
            var body = mode == FastVsHd2dMenuLayoutMode.Inventory
                ? "Panels lock to screen thirds.\nSelected rows swap to a\nsliced gold frame.\nLive scene stays blurred\nand dimmed behind them."
                : "Pause reuses the same\nframes, panel rhythm,\nand focus rules.\nMenus stay cohesive.";
            DrawHd2dAutonomousP0PixelFontText(pixels, width, height, atlasPixels, atlasWidth, atlasHeight, fontAsset, itemName, panelX + Mathf.RoundToInt(48f * uiScale), height - panelBottomY - panelHeight + Mathf.RoundToInt(74f * uiScale), 24 * uiScale, new Color32(255, 219, 132, 255));
            DrawHd2dAutonomousP0PixelFontText(pixels, width, height, atlasPixels, atlasWidth, atlasHeight, fontAsset, body, panelX + Mathf.RoundToInt(48f * uiScale), height - panelBottomY - panelHeight + Mathf.RoundToInt(152f * uiScale), 24 * uiScale, new Color32(232, 228, 208, 255));
        }

        private static void DrawHd2dAutonomousP2MenuFooterPanel(
            Color32[] pixels,
            int width,
            int height,
            Color32[] framePixels,
            int frameWidth,
            int frameHeight,
            Color32[] atlasPixels,
            int atlasWidth,
            int atlasHeight,
            TMP_FontAsset fontAsset,
            FastVsHd2dMenuLayoutProfile profile,
            int uiScale)
        {
            var panelWidth = Mathf.RoundToInt(profile.FooterPanelSizeForReview.x * uiScale);
            var panelHeight = Mathf.RoundToInt(profile.FooterPanelSizeForReview.y * uiScale);
            var panelX = Mathf.RoundToInt((width * profile.FooterPanelAnchorForReview.x) - (panelWidth * 0.5f));
            var panelBottomY = Mathf.RoundToInt((height * profile.FooterPanelAnchorForReview.y) - (panelHeight * 0.5f));
            DrawHd2dAutonomousP2MenuFrame(pixels, width, height, framePixels, frameWidth, frameHeight, panelX, panelBottomY, panelWidth, panelHeight, uiScale, new Color(0.018f, 0.016f, 0.018f, 0.84f));
            DrawHd2dAutonomousP0PixelFontText(pixels, width, height, atlasPixels, atlasWidth, atlasHeight, fontAsset, "Select   Confirm   Back", panelX + Mathf.RoundToInt(38f * uiScale), height - panelBottomY - panelHeight + Mathf.RoundToInt(22f * uiScale), 24 * uiScale, new Color32(224, 210, 168, 255));
        }

        private static void DrawHd2dAutonomousP2MenuFrame(Color32[] pixels, int width, int height, Color32[] framePixels, int frameWidth, int frameHeight, int panelX, int panelBottomY, int panelWidth, int panelHeight, int uiScale, Color fillColor)
        {
            BlendHd2dAutonomousP0PixelFontRect(pixels, width, height, panelX + (8 * uiScale), panelBottomY - (8 * uiScale), panelWidth, panelHeight, new Color(0f, 0f, 0f, 0.45f));
            BlendHd2dAutonomousP0PixelFontRect(pixels, width, height, panelX + (24 * uiScale), panelBottomY + (20 * uiScale), panelWidth - (48 * uiScale), panelHeight - (42 * uiScale), fillColor);
            DrawHd2dAutonomousP0NineSliceTexture(pixels, width, height, framePixels, frameWidth, frameHeight, panelX, panelBottomY, panelWidth, panelHeight, Hd2dAutonomousP0DialogueFrameSpriteBorder, uiScale);
            BlendHd2dAutonomousP0PixelFontRect(pixels, width, height, panelX + Mathf.RoundToInt(44f * uiScale), panelBottomY + panelHeight - Mathf.RoundToInt(28f * uiScale), panelWidth - Mathf.RoundToInt(88f * uiScale), Mathf.Max(1, 2 * uiScale), new Color(0.78f, 0.48f, 0.22f, 0.82f));
        }

        private static void WriteHd2dAutonomousP2MenuLayoutReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dMenuLayoutProfile profile,
            FastVsHd2dMenuLayoutPresenter presenter,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics blurDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics selectedDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics pauseDiff)
        {
            var lines = new List<string>
            {
                "# P2-71 HD-2D Menu Layout System Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative menu layout data prep. Runtime gets a shared uGUI ScreenSpaceCamera presenter that reuses the P0-20 ornate 9-slice frame assets, while editor captures composite a blurred/dimmed live-scene proof for remote review.",
                "- Recommendation: " + profile.RecommendationForReview,
                "- Capture note: Camera.Render does not reliably include uGUI in batch capture, so the review PNGs composite the same shared 9-slice textures, TMP atlas, dim fill, and separable blur over the live scene render. The runtime presenter remains in the scene but hidden by default.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2MenuLayoutProfilePath}` |",
                $"| Runtime presenter | `{Hd2dAutonomousP2MenuLayoutPresenterRuntimePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalMenuLayoutApprovedForReview)} |",
                $"| Shared dialogue 9-slice frame | {FormatBool(profile.ReuseDialogueNineSliceFrameForReview && presenter.UsesSharedDialogueFrameForReview)} |",
                $"| ScreenSpaceCamera / pixel perfect / integer scale | {presenter.CanvasRenderModeForReview} / {FormatBool(presenter.CanvasPixelPerfectForReview)} / {presenter.CanvasScaleFactorForReview:0.###} |",
                $"| Thirds anchors inventory/detail | ({profile.InventoryPanelThirdsAnchorForReview.x:0.###},{profile.InventoryPanelThirdsAnchorForReview.y:0.###}) / ({profile.DetailPanelThirdsAnchorForReview.x:0.###},{profile.DetailPanelThirdsAnchorForReview.y:0.###}) |",
                $"| Panel sizes inventory/detail/header/footer | {FormatHd2dAutonomousP2MenuVector2ForReport(profile.InventoryPanelSizeForReview)} / {FormatHd2dAutonomousP2MenuVector2ForReport(profile.DetailPanelSizeForReview)} / {FormatHd2dAutonomousP2MenuVector2ForReport(profile.HeaderPanelSizeForReview)} / {FormatHd2dAutonomousP2MenuVector2ForReport(profile.FooterPanelSizeForReview)} |",
                $"| Blur radius / dim alpha / unselected alpha | {profile.BackdropBlurRadiusPixelsForReview}px / {profile.BackdropDimAlphaForReview:0.###} / {profile.UnselectedFocusAlphaForReview:0.###} |",
                $"| RendererFeature status | deferred for Tom: {FormatBool(profile.RuntimeRendererFeatureDeferredForTomForReview)} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                blurDiff.ToReportRow("Live scene baseline vs inventory blur/dim menu"),
                selectedDiff.ToReportRow("Inventory selected item A vs selected item B"),
                pauseDiff.ToReportRow("Inventory menu vs pause menu reuse"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|"
            };

            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[i]);
                lines.Add($"| `{screenshotFiles[i]}` | P2-71 menu layout review capture {i + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "hd2d_menu_layout_system_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static string FormatHd2dAutonomousP2MenuVector2ForReport(Vector2 value)
        {
            return $"({value.x:0.###}, {value.y:0.###})";
        }

        private static void SetHd2dAutonomousP2MenuLayoutLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetHd2dAutonomousP2MenuLayoutLayerRecursively(child.gameObject, layer);
            }
        }

        private static int ResolveCurrentTimeOnlyCullingMaskForP2Menu(TimeWindowPairedSpacePortalController controller, int previousMask)
        {
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            return (previousMask & ~otherBit) | currentBit | playerBit;
        }
    }
}
