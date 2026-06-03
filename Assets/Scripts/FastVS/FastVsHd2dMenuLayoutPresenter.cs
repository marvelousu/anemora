using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Anemora.FastVS
{
    public enum FastVsHd2dMenuLayoutMode
    {
        Inventory,
        Pause
    }

    public sealed class FastVsHd2dMenuLayoutPresenter : MonoBehaviour
    {
        private const int PixelFontNativeSizeForReview = 16;
        private const int MenuTitleFontSizeForReview = 32;
        private const int MenuBodyFontSizeForReview = 24;
        private const string UiLayerName = "UI";
        private static readonly Vector4 DialogueFrameSpriteBorder = new Vector4(24f, 22f, 24f, 20f);
        private static readonly Vector4 NameplateSpriteBorder = new Vector4(16f, 12f, 16f, 12f);

        [SerializeField] private FastVsHd2dMenuLayoutProfile profile;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private Texture2D dialogueFrameTexture;
        [SerializeField] private Texture2D nameplateTexture;
        [SerializeField] private Sprite dialogueFrameSprite;
        [SerializeField] private Sprite nameplateSprite;
        [SerializeField] private bool visibleOnAwake;

        private Canvas canvas;
        private CanvasScaler canvasScaler;
        private RectTransform menuRoot;
        private RectTransform inventoryPanel;
        private RectTransform detailPanel;
        private RectTransform headerPanel;
        private RectTransform footerPanel;
        private Image backdropDimImage;
        private Image inventoryFrameImage;
        private Image detailFrameImage;
        private Image headerFrameImage;
        private Image footerFrameImage;
        private Image[] selectionFrames;
        private TextMeshProUGUI titleText;
        private TextMeshProUGUI[] listTexts;
        private TextMeshProUGUI detailTitleText;
        private TextMeshProUGUI detailBodyText;
        private TextMeshProUGUI footerText;
        private Material readableFontMaterial;
        private int reviewOutputHeightOverride;
        private int selectedIndexForReview;
        private FastVsHd2dMenuLayoutMode activeModeForReview;

        public bool IsReadyForReview => profile != null && canvas != null && menuRoot != null && inventoryPanel != null && detailPanel != null && selectionFrames != null && selectionFrames.Length >= 5;
        public string CanvasRenderModeForReview => canvas != null ? canvas.renderMode.ToString() : string.Empty;
        public bool CanvasPixelPerfectForReview => canvas != null && canvas.pixelPerfect;
        public float CanvasScaleFactorForReview => canvasScaler != null ? canvasScaler.scaleFactor : 0f;
        public int PanelCountForReview => (inventoryPanel != null ? 1 : 0) + (detailPanel != null ? 1 : 0) + (headerPanel != null ? 1 : 0) + (footerPanel != null ? 1 : 0);
        public bool MenuVisibleForReview => menuRoot != null && menuRoot.gameObject.activeSelf;
        public bool VisibleOnAwakeForReview => visibleOnAwake;
        public int SelectedIndexForReview => selectedIndexForReview;
        public string SelectedFrameImageTypeForReview => selectionFrames != null && selectedIndexForReview >= 0 && selectedIndexForReview < selectionFrames.Length && selectionFrames[selectedIndexForReview] != null ? selectionFrames[selectedIndexForReview].type.ToString() : string.Empty;
        public Vector2 InventoryPanelAnchorForReview => inventoryPanel != null ? inventoryPanel.anchorMin : Vector2.zero;
        public Vector2 DetailPanelAnchorForReview => detailPanel != null ? detailPanel.anchorMin : Vector2.zero;
        public Vector2 InventoryPanelSizeForReview => inventoryPanel != null ? inventoryPanel.sizeDelta : Vector2.zero;
        public Vector2 DetailPanelSizeForReview => detailPanel != null ? detailPanel.sizeDelta : Vector2.zero;
        public bool BackdropDimmingEnabledForReview => backdropDimImage != null && backdropDimImage.color.a > 0.01f;
        public bool DepthBlurBackdropPreparedForReview => profile != null && profile.EditorCompositeBackdropBlurEnabledForReview && profile.RuntimeRendererFeatureDeferredForTomForReview;
        public bool UsesSharedDialogueFrameForReview => dialogueFrameTexture != null && nameplateTexture != null && inventoryFrameImage != null && inventoryFrameImage.type == Image.Type.Sliced;
        public bool FinalMenuLayoutApprovedForReview => profile != null && profile.FinalMenuLayoutApprovedForReview;
        public FastVsHd2dMenuLayoutMode ActiveModeForReview => activeModeForReview;

        private void Awake()
        {
            EnsureUiForReview();
            if (visibleOnAwake)
            {
                ShowMenuForReview(activeModeForReview, selectedIndexForReview);
            }
            else
            {
                HideMenuForReview();
            }
        }

        public void ConfigureForReview(
            FastVsHd2dMenuLayoutProfile layoutProfile,
            Camera camera,
            TMP_FontAsset menuFont,
            Texture2D frameTexture,
            Texture2D tabTexture,
            Sprite frameSprite,
            Sprite tabSprite)
        {
            profile = layoutProfile;
            targetCamera = camera;
            fontAsset = menuFont;
            dialogueFrameTexture = frameTexture;
            nameplateTexture = tabTexture;
            dialogueFrameSprite = frameSprite;
            nameplateSprite = tabSprite;
            activeModeForReview = FastVsHd2dMenuLayoutMode.Inventory;
            selectedIndexForReview = 0;
            EnsureUiForReview();
            HideMenuForReview();
        }

        public void SetPixelReviewOutputHeightForReview(int outputHeight)
        {
            reviewOutputHeightOverride = Mathf.Max(0, outputHeight);
            ApplyCanvasScaleForReview();
        }

        public void ShowMenuForReview(FastVsHd2dMenuLayoutMode mode, int selectedIndex)
        {
            EnsureUiForReview();
            activeModeForReview = mode;
            selectedIndexForReview = Mathf.Clamp(selectedIndex, 0, 4);
            ApplyMenuContentForReview();
            if (menuRoot != null)
            {
                menuRoot.gameObject.SetActive(true);
            }
        }

        public void HideMenuForReview()
        {
            if (menuRoot != null)
            {
                menuRoot.gameObject.SetActive(false);
            }
        }

        private void EnsureUiForReview()
        {
            if (canvas != null)
            {
                ConfigureCanvasForReview();
                ApplyCanvasScaleForReview();
                ApplyMenuContentForReview();
                return;
            }

            var canvasObject = new GameObject("FastVS_HD2D_P2_71_MenuCanvas");
            canvasObject.transform.SetParent(transform, false);
            canvasObject.layer = ResolveUiRenderLayer();
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.sortingOrder = 21000;
            canvas.overrideSorting = true;
            canvas.pixelPerfect = true;
            ConfigureCanvasForReview();
            canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            ApplyCanvasScaleForReview();
            canvasObject.AddComponent<GraphicRaycaster>();

            menuRoot = CreateRect((RectTransform)canvas.transform, "MenuRoot", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Color.clear);
            backdropDimImage = CreateImage(menuRoot, "BackdropDim", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Color.black);

            headerPanel = CreateMenuPanel("HeaderPanel", profile != null ? profile.HeaderPanelAnchorForReview : new Vector2(0.5f, 0.84f), profile != null ? profile.HeaderPanelSizeForReview : new Vector2(720f, 96f), out headerFrameImage);
            inventoryPanel = CreateMenuPanel("InventoryPanel", profile != null ? profile.InventoryPanelThirdsAnchorForReview : new Vector2(1f / 3f, 0.52f), profile != null ? profile.InventoryPanelSizeForReview : new Vector2(560f, 660f), out inventoryFrameImage);
            detailPanel = CreateMenuPanel("DetailPanel", profile != null ? profile.DetailPanelThirdsAnchorForReview : new Vector2(2f / 3f, 0.52f), profile != null ? profile.DetailPanelSizeForReview : new Vector2(620f, 660f), out detailFrameImage);
            footerPanel = CreateMenuPanel("FooterPanel", profile != null ? profile.FooterPanelAnchorForReview : new Vector2(0.5f, 0.15f), profile != null ? profile.FooterPanelSizeForReview : new Vector2(940f, 74f), out footerFrameImage);

            titleText = CreateText(headerPanel, "Title", Vector2.zero, Vector2.one, new Vector2(-80f, -28f), new Vector2(40f, 4f), MenuTitleFontSizeForReview, TextAlignmentOptions.MidlineLeft, new Color(1f, 0.86f, 0.52f, 1f));
            listTexts = new TextMeshProUGUI[5];
            selectionFrames = new Image[5];
            for (var i = 0; i < listTexts.Length; i++)
            {
                var row = CreateRect(inventoryPanel, $"MenuRow_{i + 1:00}", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-92f, 52f), new Vector2(46f, -96f - (i * 78f)), Color.clear);
                row.pivot = new Vector2(0.5f, 1f);
                selectionFrames[i] = CreateSlicedImage(row, "SelectedFrame", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, ResolveDialogueFrameSprite(), DialogueFrameSpriteBorder, Color.clear);
                listTexts[i] = CreateText(row, "Label", Vector2.zero, Vector2.one, new Vector2(-36f, -12f), new Vector2(18f, 1f), MenuBodyFontSizeForReview, TextAlignmentOptions.MidlineLeft, Color.white);
            }

            detailTitleText = CreateText(detailPanel, "DetailTitle", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-92f, 58f), new Vector2(46f, -78f), MenuBodyFontSizeForReview, TextAlignmentOptions.TopLeft, new Color(1f, 0.86f, 0.52f, 1f));
            detailBodyText = CreateText(detailPanel, "DetailBody", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-92f, 360f), new Vector2(46f, -146f), MenuBodyFontSizeForReview, TextAlignmentOptions.TopLeft, new Color(0.92f, 0.90f, 0.82f, 1f));
            footerText = CreateText(footerPanel, "FooterText", Vector2.zero, Vector2.one, new Vector2(-72f, -18f), new Vector2(36f, 2f), MenuBodyFontSizeForReview, TextAlignmentOptions.MidlineLeft, new Color(0.88f, 0.84f, 0.72f, 1f));
            ApplyMenuContentForReview();
        }

        private RectTransform CreateMenuPanel(string objectName, Vector2 anchor, Vector2 size, out Image frameImage)
        {
            var panel = CreateRect(menuRoot, objectName, anchor, anchor, size, Vector2.zero, Color.clear);
            panel.pivot = new Vector2(0.5f, 0.5f);
            CreateImage(panel, "DropShadow", Vector2.zero, Vector2.one, Vector2.zero, new Vector2(8f, -8f), new Color(0f, 0f, 0f, 0.45f));
            CreateImage(panel, "InsetFill", Vector2.zero, Vector2.one, new Vector2(-48f, -42f), new Vector2(0f, -2f), profile != null ? profile.PanelFillColorForReview : new Color(0.024f, 0.020f, 0.024f, 0.92f));
            frameImage = CreateSlicedImage(panel, "Frame9Slice", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, ResolveDialogueFrameSprite(), DialogueFrameSpriteBorder, Color.white);
            CreateImage(panel, "InnerGoldRule", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-82f, 2f), new Vector2(0f, -26f), new Color(0.78f, 0.48f, 0.22f, 0.82f));
            return panel;
        }

        private void ApplyMenuContentForReview()
        {
            if (profile != null && backdropDimImage != null)
            {
                backdropDimImage.color = new Color(0f, 0f, 0f, profile.BackdropDimAlphaForReview);
            }

            if (listTexts == null || selectionFrames == null)
            {
                return;
            }

            var labels = activeModeForReview == FastVsHd2dMenuLayoutMode.Inventory
                ? new[] { "Cinder Key", "Timewriter Brush", "Empty Vial", "Folded Map", "Worn Charm" }
                : new[] { "Continue", "Inventory", "Options", "Load", "Quit" };
            var title = activeModeForReview == FastVsHd2dMenuLayoutMode.Inventory ? "INVENTORY" : "PAUSE";
            var detailTitle = labels[Mathf.Clamp(selectedIndexForReview, 0, labels.Length - 1)];
            var detailBody = activeModeForReview == FastVsHd2dMenuLayoutMode.Inventory
                ? "Panels lock to screen thirds.\nSelected rows swap to a sliced gold frame.\nThe live scene stays blurred and dimmed behind them."
                : "Pause reuses the same frames, panel rhythm, and focus rules.\nMenus stay cohesive across the chapter slice.";

            if (titleText != null)
            {
                titleText.text = title;
            }

            if (detailTitleText != null)
            {
                detailTitleText.text = detailTitle;
            }

            if (detailBodyText != null)
            {
                detailBodyText.text = detailBody;
            }

            if (footerText != null)
            {
                footerText.text = "Select  Confirm  Back";
            }

            for (var i = 0; i < listTexts.Length; i++)
            {
                var selected = i == selectedIndexForReview;
                listTexts[i].text = labels[Mathf.Clamp(i, 0, labels.Length - 1)];
                listTexts[i].color = selected
                    ? profile != null ? profile.SelectedTextColorForReview : new Color(1f, 0.90f, 0.62f, 1f)
                    : profile != null ? profile.UnselectedTextColorForReview : new Color(0.82f, 0.80f, 0.72f, 0.82f);
                selectionFrames[i].sprite = ResolveDialogueFrameSprite();
                selectionFrames[i].type = Image.Type.Sliced;
                selectionFrames[i].color = selected
                    ? profile != null ? profile.SelectedFrameTintForReview : new Color(1f, 0.78f, 0.36f, 0.92f)
                    : Color.clear;
            }
        }

        private Image CreateSlicedImage(RectTransform parent, string objectName, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Sprite sprite, Vector4 fallbackBorder, Color color)
        {
            var image = CreateImage(parent, objectName, anchorMin, anchorMax, size, position, color);
            image.sprite = sprite;
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 1f;
            if (sprite == null && fallbackBorder.sqrMagnitude > 0f)
            {
                image.color = new Color(color.r, color.g, color.b, 0f);
            }

            return image;
        }

        private Image CreateImage(RectTransform parent, string objectName, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Color color)
        {
            var rect = CreateRect(parent, objectName, anchorMin, anchorMax, size, position, Color.clear);
            var image = rect.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private static RectTransform CreateRect(RectTransform parent, string objectName, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Color color)
        {
            var rectObject = new GameObject(objectName);
            rectObject.transform.SetParent(parent, false);
            rectObject.layer = parent.gameObject.layer;
            var rect = rectObject.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            if (color.a > 0f)
            {
                var image = rectObject.AddComponent<Image>();
                image.color = color;
                image.raycastTarget = false;
            }

            return rect;
        }

        private TextMeshProUGUI CreateText(RectTransform parent, string objectName, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, float fontSize, TextAlignmentOptions alignment, Color color)
        {
            var textObject = new GameObject(objectName);
            textObject.transform.SetParent(parent, false);
            textObject.layer = parent.gameObject.layer;
            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            rect.pivot = new Vector2(anchorMin.x <= 0.01f ? 0f : anchorMin.x >= 0.99f ? 1f : 0.5f, anchorMin.y >= 0.99f ? 1f : 0.5f);
            var text = textObject.AddComponent<TextMeshProUGUI>();
            text.font = fontAsset;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = color;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Ellipsis;
            text.characterSpacing = 0f;
            text.lineSpacing = 4f;
            text.raycastTarget = false;
            var readable = GetReadableFontMaterial();
            if (readable != null)
            {
                text.fontMaterial = readable;
                text.fontSharedMaterial = readable;
                text.faceColor = color;
                text.outlineColor = Color.clear;
                text.outlineWidth = 0f;
            }

            return text;
        }

        private Sprite ResolveDialogueFrameSprite()
        {
            return ResolveSlicedSprite(ref dialogueFrameSprite, dialogueFrameTexture, DialogueFrameSpriteBorder, "FastVS_HD2D_P2_MenuFrameRuntime");
        }

        private Sprite ResolveNameplateSprite()
        {
            return ResolveSlicedSprite(ref nameplateSprite, nameplateTexture, NameplateSpriteBorder, "FastVS_HD2D_P2_MenuNameplateRuntime");
        }

        private static Sprite ResolveSlicedSprite(ref Sprite cachedSprite, Texture2D texture, Vector4 border, string objectName)
        {
            if (texture == null)
            {
                return null;
            }

            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            if (cachedSprite != null && cachedSprite.texture == texture && cachedSprite.border == border)
            {
                return cachedSprite;
            }

            cachedSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), PixelFontNativeSizeForReview, 0, SpriteMeshType.FullRect, border);
            cachedSprite.name = objectName;
            return cachedSprite;
        }

        private void ConfigureCanvasForReview()
        {
            if (canvas == null)
            {
                return;
            }

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = targetCamera;
            canvas.planeDistance = 1f;
            if (targetCamera != null)
            {
                targetCamera.cullingMask |= 1 << ResolveUiRenderLayer();
            }
        }

        private void ApplyCanvasScaleForReview()
        {
            if (canvasScaler == null)
            {
                return;
            }

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            canvasScaler.scaleFactor = ResolveIntegerPixelUiScaleForReview();
            canvasScaler.referencePixelsPerUnit = PixelFontNativeSizeForReview;
        }

        private float ResolveIntegerPixelUiScaleForReview()
        {
            var outputHeight = reviewOutputHeightOverride > 0 ? reviewOutputHeightOverride : Screen.height;
            if (outputHeight <= 0)
            {
                outputHeight = 1080;
            }

            return outputHeight >= 2160 ? 2f : 1f;
        }

        private Material GetReadableFontMaterial()
        {
            if (readableFontMaterial != null)
            {
                return readableFontMaterial;
            }

            if (fontAsset == null || fontAsset.material == null)
            {
                return null;
            }

            readableFontMaterial = new Material(fontAsset.material)
            {
                name = $"{fontAsset.name}_FastVSMenuReadable"
            };
            return readableFontMaterial;
        }

        private static int ResolveUiRenderLayer()
        {
            var uiLayer = LayerMask.NameToLayer(UiLayerName);
            return uiLayer >= 0 ? uiLayer : 0;
        }
    }
}
