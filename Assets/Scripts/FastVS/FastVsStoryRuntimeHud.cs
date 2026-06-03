using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Anemora.FastVS
{
    public sealed class FastVsStoryRuntimeHud : MonoBehaviour
    {
        private const string CompactAdvanceMarker = "▽";
        public const int PixelFontNativeSizeForReview = 16;
        public const int PixelFontCaptionSizeForReview = 16;
        public const int PixelFontBodySizeForReview = 32;
        public const int PixelFontQuestionSizeForReview = 48;
        private const float ReadableOutlineWidth = 0f;
        private const float QuestionHeadWorldOffset = 1.46f;
        private const string UiLayerName = "UI";
        private static readonly Vector2 OrnateDialoguePanelSize = new Vector2(1112f, 176f);
        private static readonly Vector2 OrnateDialoguePanelPosition = new Vector2(0f, 22f);
        private static readonly Vector2 OrnateNameplateSize = new Vector2(336f, 54f);
        private static readonly Vector4 DialogueFrameSpriteBorder = new Vector4(24f, 22f, 24f, 20f);
        private static readonly Vector4 NameplateSpriteBorder = new Vector4(16f, 12f, 16f, 12f);
        private static readonly Vector2 ObjectiveBottomPosition = new Vector2(18f, 18f);

        [SerializeField] private Camera targetCamera;
        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private Texture2D brushIconTexture;
        [SerializeField] private Texture2D dialogueFrameTexture;
        [SerializeField] private Texture2D nameplateTexture;
        [SerializeField] private Sprite dialogueFrameSprite;
        [SerializeField] private Sprite nameplateSprite;
        [SerializeField] private bool ornateDialogueFrameEnabled = true;
        [SerializeField] private float charactersPerSecond = 28f;

        private Canvas canvas;
        private CanvasScaler canvasScaler;
        private RectTransform dialoguePanel;
        private RectTransform nameplatePanel;
        private Image dialogueFrameImage;
        private Image dialogueDropShadowImage;
        private Image nameplateImage;
        private RectTransform fallbackDialogueFill;
        private RectTransform fallbackDialogueTopLine;
        private TextMeshProUGUI speakerText;
        private TextMeshProUGUI bodyText;
        private TextMeshProUGUI advanceText;
        private RectTransform guidePanel;
        private TextMeshProUGUI guideText;
        private TextMeshProUGUI guideAdvanceText;
        private RectTransform objectivePanel;
        private TextMeshProUGUI objectiveText;
        private RectTransform questionRoot;
        private TextMeshProUGUI questionText;
        private RectTransform brushRoot;
        private RawImage brushImage;
        private Material readableFontMaterial;
        private string activeFullText = string.Empty;
        private string visibleText = string.Empty;
        private string persistentObjectiveText = string.Empty;
        private float typingStartedAt;
        private bool typing;
        private int visibleCharacterCount;
        private int reviewOutputHeightOverride;

        public bool IsTyping => typing;
        public string ActiveFullTextForReview => activeFullText;
        public string VisibleTextForReview => visibleText;
        public float CharactersPerSecondForReview => charactersPerSecond;
        public string FontNameForReview => fontAsset != null ? fontAsset.name : string.Empty;
        public string CanvasRenderModeForReview => canvas != null ? canvas.renderMode.ToString() : string.Empty;
        public float PixelFontUiScaleForReview => canvasScaler != null ? canvasScaler.scaleFactor : ResolveIntegerPixelUiScaleForReview();
        public int UiRenderLayerForReview => ResolveUiRenderLayer();
        public bool CanvasPixelPerfectForReview => canvas != null && canvas.pixelPerfect;
        public float ReadableOutlineWidthForReview => ReadableOutlineWidth;
        public int BodyFontSizeForReview => PixelFontBodySizeForReview;
        public int CaptionFontSizeForReview => PixelFontCaptionSizeForReview;
        public int QuestionFontSizeForReview => PixelFontQuestionSizeForReview;
        public bool OrnateDialogueFrameEnabledForReview => ornateDialogueFrameEnabled;
        public string DialogueFrameTextureNameForReview => dialogueFrameTexture != null ? dialogueFrameTexture.name : string.Empty;
        public string NameplateTextureNameForReview => nameplateTexture != null ? nameplateTexture.name : string.Empty;
        public string DialogueFrameImageTypeForReview => dialogueFrameImage != null ? dialogueFrameImage.type.ToString() : string.Empty;
        public string NameplateImageTypeForReview => nameplateImage != null ? nameplateImage.type.ToString() : string.Empty;
        public Vector4 DialogueFrameBorderForReview => dialogueFrameImage != null && dialogueFrameImage.sprite != null ? dialogueFrameImage.sprite.border : Vector4.zero;
        public Vector4 NameplateBorderForReview => nameplateImage != null && nameplateImage.sprite != null ? nameplateImage.sprite.border : Vector4.zero;
        public bool NameplateActiveForReview => nameplatePanel != null && nameplatePanel.gameObject.activeSelf;
        public Vector2 DialoguePanelSizeForReview => dialoguePanel != null ? dialoguePanel.sizeDelta : Vector2.zero;
        public bool ContinueCaretVisibleForReview => advanceText != null && advanceText.enabled && !string.IsNullOrEmpty(advanceText.text);
        public int BodyMaxVisibleCharactersForReview => bodyText != null ? bodyText.maxVisibleCharacters : -1;
        public bool ObjectivePanelActiveForReview => objectivePanel != null && objectivePanel.gameObject.activeSelf;
        public string ObjectiveTextForReview => objectiveText != null ? objectiveText.text : string.Empty;
        public bool QuestionActiveForReview => questionRoot != null && questionRoot.gameObject.activeSelf;
        public bool BrushActiveForReview => brushRoot != null && brushRoot.gameObject.activeSelf;
        public Vector2 BrushAnchoredPositionForReview => brushRoot != null ? brushRoot.anchoredPosition : Vector2.positiveInfinity;
        public string BrushIconTextureNameForReview => brushImage != null && brushImage.texture != null ? brushImage.texture.name : string.Empty;
        public float QuestionHeadWorldOffsetForReview => QuestionHeadWorldOffset;

        private void Awake()
        {
            EnsureUi();
            HideAll();
        }

        private void Update()
        {
            UpdateContinueCaretBlink();
            if (!typing)
            {
                return;
            }

            var count = Mathf.Clamp(Mathf.FloorToInt((Time.unscaledTime - typingStartedAt) * charactersPerSecond), 0, activeFullText.Length);
            visibleCharacterCount = count;
            var next = activeFullText.Substring(0, count);
            if (next == visibleText)
            {
                return;
            }

            visibleText = next;
            ApplyVisibleText();
            if (visibleText.Length >= activeFullText.Length)
            {
                typing = false;
                UpdateContinueCaretBlink(true);
            }
        }

        public void ShowDialogue(string speaker, string fullText, string advanceHint, bool typewriter = true)
        {
            EnsureUi();
            SetActiveText(fullText, typewriter);
            dialoguePanel.gameObject.SetActive(true);
            guidePanel.gameObject.SetActive(false);
            ApplyPersistentObjectiveVisibility();
            questionRoot.gameObject.SetActive(false);
            brushRoot.gameObject.SetActive(false);
            speakerText.text = speaker;
            ApplyVisibleText();
            advanceText.text = ResolveAdvanceHintText(advanceHint);
            UpdateContinueCaretBlink(true);
        }

        public void ShowGuide(string fullText, string advanceHint = "")
        {
            EnsureUi();
            SetActiveText(fullText, true);
            dialoguePanel.gameObject.SetActive(false);
            guidePanel.gameObject.SetActive(true);
            ApplyPersistentObjectiveVisibility();
            questionRoot.gameObject.SetActive(false);
            brushRoot.gameObject.SetActive(false);
            guideText.text = visibleText;
            guideAdvanceText.text = ResolveAdvanceHintText(advanceHint);
        }

        public void ShowObjective(string fullText)
        {
            EnsureUi();
            dialoguePanel.gameObject.SetActive(false);
            guidePanel.gameObject.SetActive(false);
            questionRoot.gameObject.SetActive(false);
            brushRoot.gameObject.SetActive(false);
            SetPersistentObjective(fullText);
        }

        public void ShowDoorBeat(Transform player, Camera storyCamera, string speaker, string fullText, string advanceHint, bool showQuestion, bool showBrushIcon)
        {
            EnsureUi();
            var hasDialogue = !string.IsNullOrWhiteSpace(fullText);
            SetActiveText(fullText, true);
            dialoguePanel.gameObject.SetActive(hasDialogue);
            guidePanel.gameObject.SetActive(false);
            questionRoot.gameObject.SetActive(showQuestion);
            brushRoot.gameObject.SetActive(showBrushIcon);
            if (hasDialogue)
            {
                speakerText.text = speaker;
                ApplyVisibleText();
                advanceText.text = ResolveAdvanceHintText(advanceHint);
                UpdateContinueCaretBlink(true);
            }
            else
            {
                speakerText.text = string.Empty;
                bodyText.text = string.Empty;
                bodyText.maxVisibleCharacters = 0;
                advanceText.text = string.Empty;
                advanceText.enabled = false;
            }
            UpdateQuestionPosition(player, storyCamera);
            ApplyPersistentObjectiveVisibility();
        }

        public void HideAll()
        {
            EnsureUi();
            dialoguePanel.gameObject.SetActive(false);
            guidePanel.gameObject.SetActive(false);
            questionRoot.gameObject.SetActive(false);
            brushRoot.gameObject.SetActive(false);
            ApplyPersistentObjectiveVisibility();
            activeFullText = string.Empty;
            visibleText = string.Empty;
            visibleCharacterCount = 0;
            typing = false;
            if (bodyText != null)
            {
                bodyText.text = string.Empty;
                bodyText.maxVisibleCharacters = 0;
            }

            if (advanceText != null)
            {
                advanceText.text = string.Empty;
                advanceText.enabled = false;
            }
        }

        public void CompleteTypingNow()
        {
            visibleText = activeFullText;
            visibleCharacterCount = activeFullText.Length;
            typing = false;
            ApplyVisibleText();
            UpdateContinueCaretBlink(true);
        }

        public void SetCameraForReview(Camera camera)
        {
            targetCamera = camera;
            ConfigureScreenSpaceCameraCanvas();
        }

        public void SetPixelReviewOutputHeightForReview(int outputHeight)
        {
            reviewOutputHeightOverride = Mathf.Max(0, outputHeight);
            ApplyPixelFontCanvasScale();
        }

        public void SetOrnateDialogueFrameEnabledForReview(bool enabled)
        {
            ornateDialogueFrameEnabled = enabled;
            ApplyOrnateDialogueFrameVisibility();
        }

        private void SetActiveText(string fullText, bool typewriter)
        {
            fullText ??= string.Empty;
            if (activeFullText == fullText)
            {
                if (!typewriter)
                {
                    visibleText = activeFullText;
                    visibleCharacterCount = activeFullText.Length;
                    typing = false;
                }

                return;
            }

            activeFullText = fullText;
            visibleText = typewriter ? string.Empty : activeFullText;
            visibleCharacterCount = visibleText.Length;
            typingStartedAt = Time.unscaledTime;
            typing = typewriter && activeFullText.Length > 0;
        }

        private void ApplyVisibleText()
        {
            if (bodyText != null && dialoguePanel.gameObject.activeSelf)
            {
                bodyText.text = activeFullText;
                bodyText.maxVisibleCharacters = Mathf.Clamp(visibleCharacterCount, 0, activeFullText.Length);
            }

            if (guideText != null && guidePanel.gameObject.activeSelf)
            {
                guideText.text = visibleText;
            }

            if (objectiveText != null && objectivePanel.gameObject.activeSelf)
            {
                objectiveText.text = persistentObjectiveText;
            }
        }

        public void SetPersistentObjective(string fullText)
        {
            EnsureUi();
            persistentObjectiveText = fullText ?? string.Empty;
            ApplyPersistentObjectiveVisibility();
        }

        private void ApplyPersistentObjectiveVisibility()
        {
            if (objectivePanel == null || objectiveText == null)
            {
                return;
            }

            var hasObjective = !string.IsNullOrWhiteSpace(persistentObjectiveText);
            var blockedByStoryUi =
                (dialoguePanel != null && dialoguePanel.gameObject.activeSelf) ||
                (guidePanel != null && guidePanel.gameObject.activeSelf) ||
                (questionRoot != null && questionRoot.gameObject.activeSelf) ||
                (brushRoot != null && brushRoot.gameObject.activeSelf);

            objectiveText.text = persistentObjectiveText;
            objectivePanel.gameObject.SetActive(hasObjective && !blockedByStoryUi);
            if (hasObjective && !blockedByStoryUi)
            {
                objectivePanel.anchoredPosition = ObjectiveBottomPosition;
            }
        }

        private void UpdateQuestionPosition(Transform player, Camera storyCamera)
        {
            if (player == null || storyCamera == null || questionRoot == null || brushRoot == null)
            {
                return;
            }

            var screen = storyCamera.WorldToScreenPoint(player.position + Vector3.up * QuestionHeadWorldOffset);
            if (screen.z <= 0f)
            {
                questionRoot.gameObject.SetActive(false);
                brushRoot.gameObject.SetActive(false);
                return;
            }

            var questionPosition = new Vector2(screen.x - Screen.width * 0.5f, screen.y - Screen.height * 0.5f);
            questionRoot.anchoredPosition = questionPosition;
            brushRoot.anchoredPosition = Vector2.zero;
        }

        private void EnsureUi()
        {
            if (canvas != null)
            {
                ConfigureScreenSpaceCameraCanvas();
                ApplyPixelFontCanvasScale();
                ApplyReadableTextStyle();
                ApplyOrnateDialogueFrameVisibility();
                return;
            }

            var canvasObject = new GameObject("FastVS_StoryRuntimeHudCanvas");
            canvasObject.transform.SetParent(transform, false);
            canvasObject.layer = ResolveUiRenderLayer();
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.sortingOrder = 20000;
            canvas.overrideSorting = true;
            canvas.pixelPerfect = true;
            ConfigureScreenSpaceCameraCanvas();
            canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            ApplyPixelFontCanvasScale();
            canvasObject.AddComponent<GraphicRaycaster>();

            if (fontAsset != null)
            {
                TMP_Settings.defaultFontAsset = fontAsset;
            }

            dialoguePanel = CreatePanel("DialoguePanel", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), OrnateDialoguePanelSize, OrnateDialoguePanelPosition, Color.clear);
            dialogueDropShadowImage = CreateSlicedImage(dialoguePanel, "DropShadow", Vector2.zero, Vector2.one, new Vector2(0f, 0f), new Vector2(8f, -8f), ResolveDialogueFrameSprite(), DialogueFrameSpriteBorder, new Color(0f, 0f, 0f, 0.48f));
            fallbackDialogueFill = CreateRect(dialoguePanel, "InsetFill", Vector2.zero, Vector2.one, new Vector2(-48f, -42f), new Vector2(0f, -2f), new Color(0.028f, 0.022f, 0.026f, 0.90f));
            dialogueFrameImage = CreateSlicedImage(dialoguePanel, "Frame9Slice", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, ResolveDialogueFrameSprite(), DialogueFrameSpriteBorder, Color.white);
            fallbackDialogueTopLine = CreateRect(dialoguePanel, "InnerGoldRule", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-82f, 2f), new Vector2(0f, -25f), new Color(0.78f, 0.48f, 0.22f, 0.82f));
            nameplatePanel = CreateRect(dialoguePanel, "Nameplate", new Vector2(0f, 1f), new Vector2(0f, 1f), OrnateNameplateSize, new Vector2(42f, -8f), Color.clear);
            nameplatePanel.pivot = new Vector2(0f, 1f);
            nameplateImage = CreateSlicedImage(nameplatePanel, "Nameplate9Slice", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, ResolveNameplateSprite(), NameplateSpriteBorder, Color.white);
            speakerText = CreateText(nameplatePanel, "Speaker", Vector2.zero, Vector2.one, new Vector2(-44f, -14f), new Vector2(22f, -2f), PixelFontBodySizeForReview, FontStyles.Normal, TextAlignmentOptions.MidlineLeft, new Color(1f, 0.82f, 0.46f, 1f));
            bodyText = CreateText(dialoguePanel, "Body", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-96f, 82f), new Vector2(48f, -72f), PixelFontBodySizeForReview, FontStyles.Normal, TextAlignmentOptions.TopLeft, new Color(1f, 0.98f, 0.90f, 1f));
            advanceText = CreateText(dialoguePanel, "Advance", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(108f, 28f), new Vector2(-32f, 18f), PixelFontCaptionSizeForReview, FontStyles.Normal, TextAlignmentOptions.MidlineRight, new Color(0.94f, 0.88f, 0.72f, 1f));

            guidePanel = CreatePanel("GuidePanel", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(900f, 72f), new Vector2(0f, -36f), new Color(0.05f, 0.05f, 0.06f, 0.82f));
            guideText = CreateText(guidePanel, "GuideText", new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(-180f, -18f), new Vector2(18f, 2f), PixelFontBodySizeForReview, FontStyles.Normal, TextAlignmentOptions.MidlineLeft, new Color(0.98f, 0.97f, 0.94f, 1f));
            guideAdvanceText = CreateText(guidePanel, "GuideAdvance", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(82f, -18f), new Vector2(-16f, 6f), PixelFontCaptionSizeForReview, FontStyles.Normal, TextAlignmentOptions.MidlineRight, new Color(0.88f, 0.88f, 0.84f, 1f));
            guideAdvanceText.overflowMode = TextOverflowModes.Truncate;
            guideAdvanceText.enableWordWrapping = false;

            objectivePanel = CreatePanel("ObjectivePanel", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(680f, 56f), ObjectiveBottomPosition, new Color(0.012f, 0.011f, 0.014f, 0.74f));
            objectiveText = CreateText(objectivePanel, "ObjectiveText", Vector2.zero, Vector2.one, new Vector2(-28f, -16f), new Vector2(14f, 2f), PixelFontBodySizeForReview, FontStyles.Normal, TextAlignmentOptions.MidlineLeft, new Color(0.95f, 0.92f, 0.82f, 1f));

            questionRoot = CreatePanel("QuestionRoot", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(56f, 56f), Vector2.zero, Color.clear);
            questionText = CreateText(questionRoot, "Question", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, PixelFontQuestionSizeForReview, FontStyles.Normal, TextAlignmentOptions.Center, new Color(1f, 0.92f, 0.45f, 1f));
            questionText.text = "?";

            brushRoot = CreatePanel("BrushRoot", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(224f, 224f), Vector2.zero, new Color(0.035f, 0.030f, 0.026f, 0.78f));
            CreateRect(brushRoot, "Frame", Vector2.zero, Vector2.one, new Vector2(-16f, -16f), Vector2.zero, new Color(0.34f, 0.24f, 0.14f, 0.96f));
            CreateRect(brushRoot, "Pane", Vector2.zero, Vector2.one, new Vector2(-36f, -36f), Vector2.zero, new Color(0.07f, 0.055f, 0.045f, 0.96f));
            brushImage = CreateRawImage(brushRoot, "TimewriterBrushIcon", Vector2.zero, Vector2.one, new Vector2(-48f, -48f), Vector2.zero, brushIconTexture);
            advanceText.overflowMode = TextOverflowModes.Truncate;
            advanceText.enableWordWrapping = false;
            ApplyReadableTextStyle();
            ApplyOrnateDialogueFrameVisibility();
        }

        private Image CreateSlicedImage(RectTransform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Sprite sprite, Vector4 fallbackBorder, Color color)
        {
            var image = CreateImage(parent, name, anchorMin, anchorMax, size, position, color);
            image.sprite = sprite;
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = 1f;
            if (image.sprite == null && fallbackBorder.sqrMagnitude > 0f)
            {
                image.color = new Color(color.r, color.g, color.b, 0f);
            }

            return image;
        }

        private static Image CreateImage(RectTransform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Color color)
        {
            var rect = CreateRect(parent, name, anchorMin, anchorMax, size, position, Color.clear);
            var image = rect.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private Sprite ResolveDialogueFrameSprite()
        {
            return ResolveSlicedSprite(ref dialogueFrameSprite, dialogueFrameTexture, DialogueFrameSpriteBorder, "FastVS_HD2D_P0_DialogueFrameRuntime");
        }

        private Sprite ResolveNameplateSprite()
        {
            return ResolveSlicedSprite(ref nameplateSprite, nameplateTexture, NameplateSpriteBorder, "FastVS_HD2D_P0_NameplateRuntime");
        }

        private static Sprite ResolveSlicedSprite(ref Sprite cachedSprite, Texture2D texture, Vector4 border, string name)
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
            cachedSprite.name = name;
            return cachedSprite;
        }

        private void ApplyOrnateDialogueFrameVisibility()
        {
            var frameSprite = ResolveDialogueFrameSprite();
            var tabSprite = ResolveNameplateSprite();
            var showOrnate = ornateDialogueFrameEnabled && frameSprite != null && tabSprite != null;
            if (dialogueFrameImage != null)
            {
                dialogueFrameImage.sprite = frameSprite;
                dialogueFrameImage.type = Image.Type.Sliced;
                dialogueFrameImage.pixelsPerUnitMultiplier = 1f;
                dialogueFrameImage.gameObject.SetActive(showOrnate);
            }

            if (dialogueDropShadowImage != null)
            {
                dialogueDropShadowImage.sprite = frameSprite;
                dialogueDropShadowImage.type = Image.Type.Sliced;
                dialogueDropShadowImage.pixelsPerUnitMultiplier = 1f;
                dialogueDropShadowImage.gameObject.SetActive(showOrnate);
            }

            if (nameplatePanel != null)
            {
                nameplatePanel.gameObject.SetActive(true);
            }

            if (nameplateImage != null)
            {
                nameplateImage.sprite = tabSprite;
                nameplateImage.type = Image.Type.Sliced;
                nameplateImage.pixelsPerUnitMultiplier = 1f;
                nameplateImage.gameObject.SetActive(showOrnate);
            }
        }

        private static RawImage CreateRawImage(RectTransform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Texture texture)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.layer = parent.gameObject.layer;
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            var image = go.AddComponent<RawImage>();
            image.texture = texture;
            image.color = Color.white;
            image.raycastTarget = false;
            return image;
        }

        private void ApplyPixelFontCanvasScale()
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

        private RectTransform CreatePanel(string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Color color)
        {
            var rect = CreateRect((RectTransform)canvas.transform, name, anchorMin, anchorMax, size, position, Color.clear);
            rect.pivot = new Vector2(anchorMin.x <= 0.01f ? 0f : 0.5f, anchorMin.y <= 0.01f && anchorMax.y <= 0.01f ? 0f : 0.5f);
            if (color.a > 0f)
            {
                var background = CreateRect(rect, "Background", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, color);
                background.SetAsFirstSibling();
            }

            return rect;
        }

        private static RectTransform CreateRect(RectTransform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.layer = parent.gameObject.layer;
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            if (color.a > 0f)
            {
                var image = go.AddComponent<Image>();
                image.color = color;
                image.raycastTarget = false;
            }

            return rect;
        }

        private TextMeshProUGUI CreateText(RectTransform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, float fontSize, FontStyles style, TextAlignmentOptions alignment, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.layer = parent.gameObject.layer;
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = size;
            SetTextPivot(rect, anchorMin, anchorMax, alignment);
            rect.anchoredPosition = position;
            var text = go.AddComponent<TextMeshProUGUI>();
            text.font = fontAsset;
            var readableMaterial = GetReadableFontMaterial();
            if (readableMaterial != null)
            {
                text.fontMaterial = readableMaterial;
                text.fontSharedMaterial = readableMaterial;
            }

            text.fontSize = fontSize;
            text.fontStyle = style;
            text.alignment = alignment;
            text.color = color;
            if (readableMaterial != null)
            {
                text.faceColor = color;
                text.outlineColor = Color.clear;
                text.outlineWidth = ReadableOutlineWidth;
            }

            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.characterSpacing = 0f;
            text.lineSpacing = 4f;
            text.raycastTarget = false;
            rect.SetAsLastSibling();
            return text;
        }

        private void ConfigureScreenSpaceCameraCanvas()
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

        private static int ResolveUiRenderLayer()
        {
            var uiLayer = LayerMask.NameToLayer(UiLayerName);
            return uiLayer >= 0 ? uiLayer : 0;
        }

        private void ApplyReadableTextStyle()
        {
            var readableMaterial = GetReadableFontMaterial();
            ApplyReadableTextStyle(speakerText, readableMaterial);
            ApplyReadableTextStyle(bodyText, readableMaterial);
            ApplyReadableTextStyle(advanceText, readableMaterial);
            ApplyReadableTextStyle(guideText, readableMaterial);
            ApplyReadableTextStyle(guideAdvanceText, readableMaterial);
            ApplyReadableTextStyle(objectiveText, readableMaterial);
            ApplyReadableTextStyle(questionText, readableMaterial);
        }

        private string ResolveAdvanceHintText(string advanceHint)
        {
            return string.IsNullOrWhiteSpace(advanceHint) ? string.Empty : CompactAdvanceMarker;
        }

        private void UpdateContinueCaretBlink(bool forceVisible = false)
        {
            if (advanceText == null)
            {
                return;
            }

            if (dialoguePanel == null ||
                !dialoguePanel.gameObject.activeSelf ||
                typing ||
                string.IsNullOrEmpty(advanceText.text))
            {
                advanceText.enabled = false;
                return;
            }

            advanceText.enabled = true;
            var color = advanceText.color;
            if (forceVisible)
            {
                color.a = 1f;
            }
            else
            {
                color.a = 0.44f + (Mathf.PingPong(Time.unscaledTime * 2.2f, 1f) * 0.56f);
            }

            advanceText.color = color;
        }

        private void ApplyReadableTextStyle(TextMeshProUGUI text, Material readableMaterial)
        {
            if (text == null)
            {
                return;
            }

            if (fontAsset != null)
            {
                text.font = fontAsset;
            }

            if (readableMaterial != null)
            {
                text.fontMaterial = readableMaterial;
                text.fontSharedMaterial = readableMaterial;
            }

            if (readableMaterial != null)
            {
                text.faceColor = text.color;
                text.outlineColor = Color.clear;
                text.outlineWidth = ReadableOutlineWidth;
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
                name = $"{fontAsset.name}_FastVSRuntimeHudReadable"
            };

            if (readableFontMaterial.HasProperty(ShaderUtilities.ID_FaceColor))
            {
                readableFontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.white);
            }

            if (readableFontMaterial.HasProperty("_Color"))
            {
                readableFontMaterial.SetColor("_Color", Color.white);
            }

            if (readableFontMaterial.HasProperty("_FaceColor"))
            {
                readableFontMaterial.SetColor("_FaceColor", Color.white);
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

        private static void SetTextPivot(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, TextAlignmentOptions alignment)
        {
            var pivot = rect.pivot;

            if (Mathf.Approximately(anchorMin.x, anchorMax.x))
            {
                pivot.x = anchorMin.x <= 0.01f ? 0f : anchorMin.x >= 0.99f ? 1f : 0.5f;
            }
            else if (alignment == TextAlignmentOptions.TopLeft ||
                     alignment == TextAlignmentOptions.MidlineLeft ||
                     alignment == TextAlignmentOptions.BottomLeft)
            {
                pivot.x = 0f;
            }
            else if (alignment == TextAlignmentOptions.TopRight ||
                     alignment == TextAlignmentOptions.MidlineRight ||
                     alignment == TextAlignmentOptions.BottomRight)
            {
                pivot.x = 1f;
            }
            else
            {
                pivot.x = 0.5f;
            }

            if (Mathf.Approximately(anchorMin.y, anchorMax.y))
            {
                pivot.y = anchorMin.y <= 0.01f ? 0f : anchorMin.y >= 0.99f ? 1f : 0.5f;
            }

            rect.pivot = pivot;
        }
    }
}
