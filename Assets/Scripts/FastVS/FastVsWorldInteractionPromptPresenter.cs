using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Anemora.FastVS
{
    public sealed class FastVsWorldInteractionPromptPresenter : MonoBehaviour
    {
        private const string UiLayerName = "UI";
        private const int GlyphTileSize = 32;
        private const int LabelBitmapWidth = 136;
        private const int LabelBitmapHeight = 28;
        private const int LabelBitmapGlyphScale = 3;
        private const float ReadableOutlineWidth = 0f;
        private static readonly Vector2 PromptSize = new Vector2(224f, 46f);

        [SerializeField] private Camera targetCamera;
        [SerializeField] private Transform player;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private Texture2D glyphSheetTexture;
        [SerializeField] private FastVsWorldInteractionPromptInputScheme inputScheme = FastVsWorldInteractionPromptInputScheme.KeyboardMouse;
        [SerializeField] private float fadeSpeed = 9f;
        [SerializeField] private float promptScreenYOffset = 18f;

        private Canvas canvas;
        private CanvasScaler canvasScaler;
        private CanvasGroup canvasGroup;
        private RectTransform promptRoot;
        private RawImage buttonGlyphImage;
        private RawImage promptIconImage;
        private RawImage labelBitmapImage;
        private Texture2D labelBitmapTexture;
        private string labelBitmapText = string.Empty;
        private TextMeshProUGUI labelText;
        private Material readableFontMaterial;
        private FastVsWorldInteractionPromptTarget activeTarget;
        private float visibleAmount;
        private Vector2 reviewScreenSizeOverride;

        public bool IsReadyForReview => canvas != null && promptRoot != null && glyphSheetTexture != null && labelBitmapImage != null;
        public bool IsVisibleForReview => promptRoot != null && promptRoot.gameObject.activeSelf && visibleAmount > 0.95f;
        public string CanvasRenderModeForReview => canvas != null ? canvas.renderMode.ToString() : string.Empty;
        public bool CanvasPixelPerfectForReview => canvas != null && canvas.pixelPerfect;
        public string ActivePromptIdForReview => activeTarget != null ? activeTarget.PromptIdForReview : string.Empty;
        public string ActivePromptLabelForReview => labelText != null ? labelText.text : string.Empty;
        public FastVsWorldInteractionPromptKind ActivePromptKindForReview => activeTarget != null ? activeTarget.PromptKindForReview : FastVsWorldInteractionPromptKind.Examine;
        public FastVsWorldInteractionPromptInputScheme InputSchemeForReview => inputScheme;
        public string ActiveButtonGlyphForReview => inputScheme == FastVsWorldInteractionPromptInputScheme.Gamepad ? "A" : "E";
        public FilterMode GlyphFilterModeForReview => glyphSheetTexture != null ? glyphSheetTexture.filterMode : FilterMode.Bilinear;
        public int GlyphSheetWidthForReview => glyphSheetTexture != null ? glyphSheetTexture.width : 0;
        public int GlyphSheetHeightForReview => glyphSheetTexture != null ? glyphSheetTexture.height : 0;
        public string LabelFontNameForReview => labelText != null && labelText.font != null ? labelText.font.name : string.Empty;
        public float LabelPreferredWidthForReview => labelText != null ? labelText.preferredWidth : 0f;
        public bool LabelBitmapReadyForReview => labelBitmapImage != null && labelBitmapImage.texture != null && labelBitmapText == ActivePromptLabelForReview;
        public int LabelBitmapWidthForReview => labelBitmapTexture != null ? labelBitmapTexture.width : 0;
        public int LabelBitmapHeightForReview => labelBitmapTexture != null ? labelBitmapTexture.height : 0;
        public Vector2 PromptAnchoredPositionForReview => promptRoot != null ? promptRoot.anchoredPosition : Vector2.positiveInfinity;

        private void Awake()
        {
            EnsureUi();
            HideImmediate();
        }

        private void LateUpdate()
        {
            RefreshPrompt(false);
        }

        public bool TryEnsureForReview()
        {
            EnsureUi();
            return IsReadyForReview;
        }

        public void SetCameraForReview(Camera camera)
        {
            targetCamera = camera;
            ConfigureCanvasCamera();
        }

        public void ConfigureForReview(
            Camera camera,
            Transform playerTransform,
            FastVsHouseAreaVisibility visibility,
            TMP_FontAsset font,
            Texture2D glyphSheet)
        {
            targetCamera = camera;
            player = playerTransform;
            areaVisibility = visibility;
            fontAsset = font;
            glyphSheetTexture = glyphSheet;
            EnsureUi();
            ApplyUiAssets();
        }

        public void SetReviewScreenSizeForReview(int width, int height)
        {
            reviewScreenSizeOverride = new Vector2(Mathf.Max(1, width), Mathf.Max(1, height));
        }

        public void SetInputSchemeForReview(FastVsWorldInteractionPromptInputScheme nextScheme)
        {
            inputScheme = nextScheme;
            ApplyGlyphs();
        }

        public void RefreshPromptForReview(bool immediate)
        {
            RefreshPrompt(immediate);
        }

        private void RefreshPrompt(bool immediate)
        {
            ResolveReferences();
            EnsureUi();

            var nextTarget = ResolveActiveTarget();
            activeTarget = nextTarget;
            if (activeTarget == null || targetCamera == null)
            {
                visibleAmount = immediate ? 0f : Mathf.MoveTowards(visibleAmount, 0f, Time.unscaledDeltaTime * fadeSpeed);
                ApplyVisibility();
                return;
            }

            var screen = targetCamera.WorldToScreenPoint(activeTarget.AnchorWorldPositionForReview);
            if (screen.z <= 0f)
            {
                visibleAmount = immediate ? 0f : Mathf.MoveTowards(visibleAmount, 0f, Time.unscaledDeltaTime * fadeSpeed);
                ApplyVisibility();
                return;
            }

            var screenSize = ResolveScreenSizeForPromptPlacement();
            var anchored = new Vector2(screen.x - screenSize.x * 0.5f, screen.y - screenSize.y * 0.5f + promptScreenYOffset);
            promptRoot.anchoredPosition = RoundToPixel(anchored);
            labelText.text = activeTarget.PromptLabelForReview;
            ApplyReadableTextStyle(labelText);
            UpdateLabelBitmap(activeTarget.PromptLabelForReview);
            ApplyGlyphs();
            labelText.ForceMeshUpdate(true, true);
            visibleAmount = immediate ? 1f : Mathf.MoveTowards(visibleAmount, 1f, Time.unscaledDeltaTime * fadeSpeed);
            ApplyVisibility();
        }

        private FastVsWorldInteractionPromptTarget ResolveActiveTarget()
        {
            if (player == null)
            {
                return null;
            }

            var currentArea = areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.Interior;
            var targets = FindObjectsByType<FastVsWorldInteractionPromptTarget>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            var bestDistance = float.PositiveInfinity;
            FastVsWorldInteractionPromptTarget best = null;
            for (var index = 0; index < targets.Length; index++)
            {
                var target = targets[index];
                if (target == null || !target.IsEligibleForReview(player.position, currentArea))
                {
                    continue;
                }

                var distance = (target.transform.position - player.position).sqrMagnitude;
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = target;
                }
            }

            return best;
        }

        private void EnsureUi()
        {
            if (canvas != null)
            {
                ConfigureCanvasCamera();
                ApplyUiAssets();
                return;
            }

            var canvasObject = new GameObject("FastVS_WorldInteractionPromptCanvas");
            canvasObject.transform.SetParent(transform, false);
            canvasObject.layer = ResolveUiRenderLayer();
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.sortingOrder = 5200;
            canvas.pixelPerfect = true;
            ConfigureCanvasCamera();

            canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            canvasScaler.scaleFactor = 1f;
            canvasScaler.referencePixelsPerUnit = 16f;
            canvasObject.AddComponent<GraphicRaycaster>();

            promptRoot = CreateRect(canvasObject.transform, "PromptRoot", PromptSize, Vector2.zero, Color.clear);
            canvasGroup = promptRoot.gameObject.AddComponent<CanvasGroup>();
            var shadow = CreateRect(promptRoot, "DropShadow", PromptSize, new Vector2(3f, -3f), new Color(0f, 0f, 0f, 0.52f));
            shadow.SetSiblingIndex(0);
            CreateRect(promptRoot, "Panel", PromptSize, Vector2.zero, new Color(0.030f, 0.026f, 0.020f, 0.88f));
            CreateRect(promptRoot, "PanelEdge", new Vector2(PromptSize.x - 6f, PromptSize.y - 6f), Vector2.zero, new Color(0.33f, 0.24f, 0.12f, 0.98f));
            CreateRect(promptRoot, "PanelCore", new Vector2(PromptSize.x - 12f, PromptSize.y - 12f), Vector2.zero, new Color(0.055f, 0.047f, 0.038f, 0.98f));

            promptIconImage = CreateRawImage(promptRoot, "PromptIcon", new Vector2(28f, 28f), new Vector2(-84f, 0f));
            buttonGlyphImage = CreateRawImage(promptRoot, "ButtonGlyph", new Vector2(30f, 30f), new Vector2(-48f, 0f));
            labelText = CreateText(promptRoot, "PromptLabel", new Vector2(136f, 34f), new Vector2(34f, -1f));
            labelBitmapImage = CreateLabelBitmapImage(promptRoot, "PromptLabelBitmap", new Vector2(LabelBitmapWidth, LabelBitmapHeight), new Vector2(44f, 0f));
            ApplyUiAssets();
            ApplyGlyphs();
            HideImmediate();
        }

        private void ConfigureCanvasCamera()
        {
            if (canvas == null)
            {
                return;
            }

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = targetCamera;
            canvas.planeDistance = 0.6f;
            if (targetCamera != null)
            {
                targetCamera.cullingMask |= 1 << ResolveUiRenderLayer();
            }
        }

        private RectTransform CreateRect(Transform parent, string objectName, Vector2 size, Vector2 position, Color color)
        {
            var rectObject = new GameObject(objectName);
            rectObject.transform.SetParent(parent, false);
            rectObject.layer = ResolveUiRenderLayer();
            var rect = rectObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
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

        private RawImage CreateRawImage(RectTransform parent, string objectName, Vector2 size, Vector2 position)
        {
            var rect = CreateRect(parent, objectName, size, position, Color.clear);
            var image = rect.gameObject.AddComponent<RawImage>();
            image.texture = glyphSheetTexture;
            image.raycastTarget = false;
            return image;
        }

        private RawImage CreateLabelBitmapImage(RectTransform parent, string objectName, Vector2 size, Vector2 position)
        {
            var rect = CreateRect(parent, objectName, size, position, Color.clear);
            var image = rect.gameObject.AddComponent<RawImage>();
            image.raycastTarget = false;
            image.uvRect = new Rect(0f, 0f, 1f, 1f);
            rect.SetAsLastSibling();
            return image;
        }

        private TextMeshProUGUI CreateText(RectTransform parent, string objectName, Vector2 size, Vector2 position)
        {
            var rect = CreateRect(parent, objectName, size, position, Color.clear);
            rect.gameObject.SetActive(false);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.font = fontAsset;
            text.fontSize = 18f;
            text.enableWordWrapping = false;
            text.overflowMode = TextOverflowModes.Overflow;
            text.alignment = TextAlignmentOptions.MidlineLeft;
            text.color = new Color(0.96f, 0.91f, 0.76f, 1f);
            text.raycastTarget = false;
            text.characterSpacing = 0f;
            ApplyReadableTextStyle(text);
            rect.gameObject.SetActive(true);
            return text;
        }

        private void ApplyUiAssets()
        {
            if (labelText != null)
            {
                labelText.font = fontAsset;
                ApplyReadableTextStyle(labelText);
            }

            if (buttonGlyphImage != null)
            {
                buttonGlyphImage.texture = glyphSheetTexture;
            }

            if (promptIconImage != null)
            {
                promptIconImage.texture = glyphSheetTexture;
            }

            if (labelBitmapImage != null)
            {
                labelBitmapImage.uvRect = new Rect(0f, 0f, 1f, 1f);
            }
        }

        private void ApplyGlyphs()
        {
            if (buttonGlyphImage == null || promptIconImage == null || glyphSheetTexture == null)
            {
                return;
            }

            buttonGlyphImage.texture = glyphSheetTexture;
            promptIconImage.texture = glyphSheetTexture;
            buttonGlyphImage.uvRect = ResolveTileUv(inputScheme == FastVsWorldInteractionPromptInputScheme.Gamepad ? 1 : 0);
            promptIconImage.uvRect = ResolveTileUv(ResolveIconTile(activeTarget != null ? activeTarget.PromptKindForReview : FastVsWorldInteractionPromptKind.Examine));
        }

        private void ApplyReadableTextStyle(TextMeshProUGUI text)
        {
            if (text == null)
            {
                return;
            }

            text.font = fontAsset;
            var readableMaterial = GetReadableFontMaterial();
            if (readableMaterial != null)
            {
                text.fontMaterial = readableMaterial;
                text.fontSharedMaterial = readableMaterial;
            }

            var labelColor = new Color(0.96f, 0.91f, 0.76f, 1f);
            text.color = labelColor;
            text.faceColor = labelColor;
            text.outlineColor = Color.clear;
            text.outlineWidth = ReadableOutlineWidth;
        }

        private void UpdateLabelBitmap(string label)
        {
            if (labelBitmapImage == null)
            {
                return;
            }

            label ??= string.Empty;
            if (labelBitmapTexture == null)
            {
                labelBitmapTexture = new Texture2D(LabelBitmapWidth, LabelBitmapHeight, TextureFormat.RGBA32, false, true)
                {
                    name = "FastVS_WorldInteractionPrompt_LabelBitmap",
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp
                };
            }

            if (labelBitmapText == label && labelBitmapImage.texture == labelBitmapTexture)
            {
                return;
            }

            labelBitmapText = label;
            var pixels = new Color32[LabelBitmapWidth * LabelBitmapHeight];
            var normalized = label.Trim().ToUpperInvariant();
            if (!string.IsNullOrEmpty(normalized))
            {
                var textWidth = CalculateBitmapTextWidth(normalized);
                var x = Mathf.Max(2, Mathf.FloorToInt((LabelBitmapWidth - textWidth) * 0.5f));
                var y = Mathf.FloorToInt((LabelBitmapHeight - 7 * LabelBitmapGlyphScale) * 0.5f);
                DrawBitmapText(pixels, normalized, x + 1, y - 1, new Color32(40, 26, 12, 210));
                DrawBitmapText(pixels, normalized, x, y, new Color32(246, 230, 184, 255));
            }

            labelBitmapTexture.SetPixels32(pixels);
            labelBitmapTexture.Apply(false, false);
            labelBitmapImage.texture = labelBitmapTexture;
            labelBitmapImage.enabled = !string.IsNullOrEmpty(normalized);
        }

        private static int CalculateBitmapTextWidth(string text)
        {
            var width = 0;
            for (var index = 0; index < text.Length; index++)
            {
                if (text[index] == ' ')
                {
                    width += 3 * LabelBitmapGlyphScale;
                }
                else
                {
                    width += 5 * LabelBitmapGlyphScale;
                }

                if (index < text.Length - 1)
                {
                    width += LabelBitmapGlyphScale;
                }
            }

            return width;
        }

        private static void DrawBitmapText(Color32[] pixels, string text, int x, int y, Color32 color)
        {
            var cursor = x;
            for (var index = 0; index < text.Length; index++)
            {
                var character = text[index];
                if (character == ' ')
                {
                    cursor += 4 * LabelBitmapGlyphScale;
                    continue;
                }

                DrawBitmapGlyph(pixels, ResolveBitmapGlyph(character), cursor, y, color);
                cursor += 6 * LabelBitmapGlyphScale;
            }
        }

        private static void DrawBitmapGlyph(Color32[] pixels, string[] glyph, int x, int y, Color32 color)
        {
            for (var row = 0; row < glyph.Length; row++)
            {
                var pattern = glyph[row];
                for (var column = 0; column < pattern.Length; column++)
                {
                    if (pattern[column] != '1')
                    {
                        continue;
                    }

                    var targetX = x + column * LabelBitmapGlyphScale;
                    var targetY = y + (glyph.Length - 1 - row) * LabelBitmapGlyphScale;
                    FillBitmapGlyphPixel(pixels, targetX, targetY, color);
                }
            }
        }

        private static void FillBitmapGlyphPixel(Color32[] pixels, int x, int y, Color32 color)
        {
            for (var yy = 0; yy < LabelBitmapGlyphScale; yy++)
            {
                var py = y + yy;
                if (py < 0 || py >= LabelBitmapHeight)
                {
                    continue;
                }

                for (var xx = 0; xx < LabelBitmapGlyphScale; xx++)
                {
                    var px = x + xx;
                    if (px < 0 || px >= LabelBitmapWidth)
                    {
                        continue;
                    }

                    pixels[py * LabelBitmapWidth + px] = color;
                }
            }
        }

        private static string[] ResolveBitmapGlyph(char character)
        {
            switch (character)
            {
                case 'A':
                    return new[] { "01110", "10001", "10001", "11111", "10001", "10001", "10001" };
                case 'E':
                    return new[] { "11111", "10000", "10000", "11110", "10000", "10000", "11111" };
                case 'I':
                    return new[] { "11111", "00100", "00100", "00100", "00100", "00100", "11111" };
                case 'K':
                    return new[] { "10001", "10010", "10100", "11000", "10100", "10010", "10001" };
                case 'L':
                    return new[] { "10000", "10000", "10000", "10000", "10000", "10000", "11111" };
                case 'M':
                    return new[] { "10001", "11011", "10101", "10101", "10001", "10001", "10001" };
                case 'N':
                    return new[] { "10001", "11001", "10101", "10011", "10001", "10001", "10001" };
                case 'R':
                    return new[] { "11110", "10001", "10001", "11110", "10100", "10010", "10001" };
                case 'T':
                    return new[] { "11111", "00100", "00100", "00100", "00100", "00100", "00100" };
                case 'X':
                    return new[] { "10001", "01010", "00100", "00100", "00100", "01010", "10001" };
                default:
                    return new[] { "11111", "00001", "00010", "00100", "01000", "00000", "01000" };
            }
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
                name = $"{fontAsset.name}_FastVSPromptReadable"
            };

            var labelColor = new Color(0.96f, 0.91f, 0.76f, 1f);
            if (readableFontMaterial.HasProperty(ShaderUtilities.ID_FaceColor))
            {
                readableFontMaterial.SetColor(ShaderUtilities.ID_FaceColor, labelColor);
            }

            if (readableFontMaterial.HasProperty("_Color"))
            {
                readableFontMaterial.SetColor("_Color", labelColor);
            }

            if (readableFontMaterial.HasProperty("_FaceColor"))
            {
                readableFontMaterial.SetColor("_FaceColor", labelColor);
            }

            if (readableFontMaterial.HasProperty(ShaderUtilities.ID_OutlineColor))
            {
                readableFontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.clear);
            }

            if (readableFontMaterial.HasProperty(ShaderUtilities.ID_OutlineWidth))
            {
                readableFontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, ReadableOutlineWidth);
            }

            return readableFontMaterial;
        }

        private Rect ResolveTileUv(int tileIndex)
        {
            var columns = Mathf.Max(1, glyphSheetTexture.width / GlyphTileSize);
            var rows = Mathf.Max(1, glyphSheetTexture.height / GlyphTileSize);
            var x = tileIndex % columns;
            var y = tileIndex / columns;
            return new Rect(x / (float)columns, 1f - (y + 1f) / rows, 1f / columns, 1f / rows);
        }

        private static int ResolveIconTile(FastVsWorldInteractionPromptKind kind)
        {
            switch (kind)
            {
                case FastVsWorldInteractionPromptKind.Talk:
                    return 2;
                case FastVsWorldInteractionPromptKind.Enter:
                    return 4;
                case FastVsWorldInteractionPromptKind.Examine:
                default:
                    return 3;
            }
        }

        private void ApplyVisibility()
        {
            if (promptRoot == null || canvasGroup == null)
            {
                return;
            }

            var visible = visibleAmount > 0.001f;
            promptRoot.gameObject.SetActive(visible);
            canvasGroup.alpha = Mathf.Clamp01(visibleAmount);
            var scale = Mathf.Lerp(0.92f, 1f, Mathf.Clamp01(visibleAmount));
            promptRoot.localScale = new Vector3(scale, scale, 1f);
        }

        private void HideImmediate()
        {
            visibleAmount = 0f;
            ApplyVisibility();
        }

        private void ResolveReferences()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            if (player == null)
            {
                var playerController = FindFirstObjectByType<CharacterController>();
                player = playerController != null ? playerController.transform : null;
            }

            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }
        }

        private static Vector2 RoundToPixel(Vector2 value)
        {
            return new Vector2(Mathf.Round(value.x), Mathf.Round(value.y));
        }

        private static int ResolveUiRenderLayer()
        {
            var layer = LayerMask.NameToLayer(UiLayerName);
            return layer >= 0 ? layer : 0;
        }

        private Vector2 ResolveScreenSizeForPromptPlacement()
        {
            if (reviewScreenSizeOverride.x >= 1f && reviewScreenSizeOverride.y >= 1f)
            {
                return reviewScreenSizeOverride;
            }

            if (targetCamera != null && targetCamera.targetTexture != null)
            {
                return new Vector2(targetCamera.targetTexture.width, targetCamera.targetTexture.height);
            }

            if (targetCamera != null && targetCamera.pixelWidth > 1 && targetCamera.pixelHeight > 1)
            {
                return new Vector2(targetCamera.pixelWidth, targetCamera.pixelHeight);
            }

            return new Vector2(Mathf.Max(1, Screen.width), Mathf.Max(1, Screen.height));
        }
    }
}
