using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Anemora.FastVS
{
    public sealed class FastVsStoryRuntimeHud : MonoBehaviour
    {
        private const string CompactAdvanceMarker = "▽";
        private const float ReadableOutlineWidth = 0.04f;
        private const float QuestionHeadWorldOffset = 1.46f;
        private static readonly Vector2 ObjectiveBottomPosition = new Vector2(18f, 18f);

        [SerializeField] private Camera targetCamera;
        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private Texture2D brushIconTexture;
        [SerializeField] private float charactersPerSecond = 28f;

        private Canvas canvas;
        private RectTransform dialoguePanel;
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

        public bool IsTyping => typing;
        public string ActiveFullTextForReview => activeFullText;
        public string VisibleTextForReview => visibleText;
        public float CharactersPerSecondForReview => charactersPerSecond;
        public string FontNameForReview => fontAsset != null ? fontAsset.name : string.Empty;
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
            if (!typing)
            {
                return;
            }

            var count = Mathf.Clamp(Mathf.FloorToInt((Time.unscaledTime - typingStartedAt) * charactersPerSecond), 0, activeFullText.Length);
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
            bodyText.text = visibleText;
            advanceText.text = ResolveAdvanceHintText(advanceHint);
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
            SetPersistentObjective(fullText);
            dialoguePanel.gameObject.SetActive(false);
            guidePanel.gameObject.SetActive(false);
            questionRoot.gameObject.SetActive(false);
            brushRoot.gameObject.SetActive(false);
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
                bodyText.text = visibleText;
                advanceText.text = ResolveAdvanceHintText(advanceHint);
            }
            else
            {
                speakerText.text = string.Empty;
                bodyText.text = string.Empty;
                advanceText.text = string.Empty;
            }
            UpdateQuestionPosition(player, storyCamera);
            ApplyPersistentObjectiveVisibility();
        }

        public void HideAll()
        {
            EnsureUi();
            dialoguePanel.gameObject.SetActive(false);
            guidePanel.gameObject.SetActive(false);
            ApplyPersistentObjectiveVisibility();
            questionRoot.gameObject.SetActive(false);
            brushRoot.gameObject.SetActive(false);
            activeFullText = string.Empty;
            visibleText = string.Empty;
            typing = false;
        }

        public void CompleteTypingNow()
        {
            visibleText = activeFullText;
            typing = false;
            ApplyVisibleText();
        }

        public void SetCameraForReview(Camera camera)
        {
            targetCamera = camera;
            if (canvas != null)
            {
                canvas.worldCamera = targetCamera;
            }
        }

        private void SetActiveText(string fullText, bool typewriter)
        {
            fullText ??= string.Empty;
            if (activeFullText == fullText)
            {
                return;
            }

            activeFullText = fullText;
            visibleText = typewriter ? string.Empty : activeFullText;
            typingStartedAt = Time.unscaledTime;
            typing = typewriter && activeFullText.Length > 0;
        }

        private void ApplyVisibleText()
        {
            if (bodyText != null && dialoguePanel.gameObject.activeSelf)
            {
                bodyText.text = visibleText;
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
                ApplyReadableTextStyle();
                return;
            }

            var canvasObject = new GameObject("FastVS_StoryRuntimeHudCanvas");
            canvasObject.transform.SetParent(transform, false);
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20000;
            canvas.overrideSorting = true;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            if (fontAsset != null)
            {
                TMP_Settings.defaultFontAsset = fontAsset;
            }

            dialoguePanel = CreatePanel("DialoguePanel", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(1092f, 106f), new Vector2(0f, 14f), new Color(0.012f, 0.011f, 0.014f, 0.76f));
            CreateRect(dialoguePanel, "TopLine", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 3f), new Vector2(0f, -1.5f), new Color(0.85f, 0.42f, 0.20f, 0.92f));
            speakerText = CreateText(dialoguePanel, "Speaker", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(240f, 28f), new Vector2(22f, -12f), 21f, FontStyles.Bold, TextAlignmentOptions.TopLeft, new Color(1f, 0.80f, 0.48f, 1f));
            bodyText = CreateText(dialoguePanel, "Body", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-44f, 44f), new Vector2(22f, -44f), 23f, FontStyles.Normal, TextAlignmentOptions.TopLeft, new Color(1f, 0.98f, 0.90f, 1f));
            advanceText = CreateText(dialoguePanel, "Advance", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(190f, 22f), new Vector2(-18f, 7f), 15f, FontStyles.Normal, TextAlignmentOptions.MidlineRight, new Color(0.94f, 0.88f, 0.72f, 1f));

            guidePanel = CreatePanel("GuidePanel", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(780f, 54f), new Vector2(0f, -28f), new Color(0.05f, 0.05f, 0.06f, 0.82f));
            guideText = CreateText(guidePanel, "GuideText", new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(-150f, -12f), new Vector2(18f, 2f), 17f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft, new Color(0.98f, 0.97f, 0.94f, 1f));
            guideAdvanceText = CreateText(guidePanel, "GuideAdvance", new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(70f, -12f), new Vector2(-16f, 6f), 16f, FontStyles.Normal, TextAlignmentOptions.MidlineRight, new Color(0.88f, 0.88f, 0.84f, 1f));
            guideAdvanceText.overflowMode = TextOverflowModes.Truncate;
            guideAdvanceText.enableWordWrapping = false;

            objectivePanel = CreatePanel("ObjectivePanel", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(560f, 42f), ObjectiveBottomPosition, new Color(0.012f, 0.011f, 0.014f, 0.74f));
            objectiveText = CreateText(objectivePanel, "ObjectiveText", Vector2.zero, Vector2.one, new Vector2(-28f, -14f), new Vector2(14f, 2f), 17f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft, new Color(0.95f, 0.92f, 0.82f, 1f));

            questionRoot = CreatePanel("QuestionRoot", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(40f, 40f), Vector2.zero, Color.clear);
            questionText = CreateText(questionRoot, "Question", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, 31f, FontStyles.Bold, TextAlignmentOptions.Center, new Color(1f, 0.92f, 0.45f, 1f));
            questionText.text = "?";

            brushRoot = CreatePanel("BrushRoot", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(224f, 224f), Vector2.zero, new Color(0.035f, 0.030f, 0.026f, 0.78f));
            CreateRect(brushRoot, "Frame", Vector2.zero, Vector2.one, new Vector2(-16f, -16f), Vector2.zero, new Color(0.34f, 0.24f, 0.14f, 0.96f));
            CreateRect(brushRoot, "Pane", Vector2.zero, Vector2.one, new Vector2(-36f, -36f), Vector2.zero, new Color(0.07f, 0.055f, 0.045f, 0.96f));
            brushImage = CreateRawImage(brushRoot, "TimewriterBrushIcon", Vector2.zero, Vector2.one, new Vector2(-48f, -48f), Vector2.zero, brushIconTexture);
            advanceText.overflowMode = TextOverflowModes.Truncate;
            advanceText.enableWordWrapping = false;
            ApplyReadableTextStyle();
        }

        private static RawImage CreateRawImage(RectTransform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Texture texture)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
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
                text.outlineColor = new Color(0f, 0f, 0f, 0.82f);
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
                text.outlineColor = new Color(0f, 0f, 0f, 0.82f);
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
                readableFontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, new Color(0f, 0f, 0f, 0.82f));
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
