using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Anemora.FastVS
{
    public sealed class FastVsStoryDialoguePresenter : MonoBehaviour
    {
        private const float QuestionHeadWorldOffset = 1.46f;

        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private Texture2D brushIconTexture;
        [SerializeField] private bool useTmpPresenter = true;

        private Canvas canvas;
        private CanvasScaler scaler;
        private RectTransform panel;
        private TextMeshProUGUI speakerText;
        private TextMeshProUGUI bodyText;
        private TextMeshProUGUI advanceText;
        private RectTransform objectivePanel;
        private TextMeshProUGUI objectiveText;
        private RectTransform questionRoot;
        private TextMeshProUGUI questionText;
        private RectTransform brushRoot;
        private RawImage brushImage;
        private Material readableFontMaterial;
        private string activeText;
        private string activeSpeaker;

        public bool IsReadyForReview => useTmpPresenter && canvas != null && fontAsset != null;
        public string ActiveTextForReview => activeText;
        public string ActiveSpeakerForReview => activeSpeaker;
        public string FontNameForReview => fontAsset != null ? fontAsset.name : string.Empty;

        public bool TryEnsureForReview()
        {
            if (!useTmpPresenter)
            {
                return false;
            }

            EnsureUi();
            return IsReadyForReview;
        }

        private void Awake()
        {
            if (fontAsset == null)
            {
                return;
            }

            EnsureUi();
            HideAll();
        }

        public void ShowDialogue(string speaker, string text, string advance)
        {
            EnsureUi();
            if (!IsReadyForReview)
            {
                return;
            }

            activeSpeaker = speaker;
            activeText = text;
            panel.gameObject.SetActive(true);
            objectivePanel.gameObject.SetActive(false);
            questionRoot.gameObject.SetActive(false);
            brushRoot.gameObject.SetActive(false);
            speakerText.text = speaker;
            bodyText.text = text;
            advanceText.text = advance;
        }

        public void ShowDoorBeat(Transform player, Camera storyCamera, string speaker, string text, string advance, bool showQuestion, bool showBrushIcon)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                ShowDialogue(speaker, text, advance);
            }
            else
            {
                EnsureUi();
                panel.gameObject.SetActive(false);
                objectivePanel.gameObject.SetActive(false);
                activeSpeaker = string.Empty;
                activeText = string.Empty;
            }

            questionRoot.gameObject.SetActive(showQuestion);
            brushRoot.gameObject.SetActive(showBrushIcon);
            UpdateQuestionPosition(player, storyCamera);
        }

        public void ShowObjective(string text)
        {
            EnsureUi();
            if (!IsReadyForReview)
            {
                return;
            }

            activeSpeaker = string.Empty;
            activeText = text;
            panel.gameObject.SetActive(false);
            objectivePanel.gameObject.SetActive(true);
            questionRoot.gameObject.SetActive(false);
            brushRoot.gameObject.SetActive(false);
            objectiveText.text = text;
        }

        public void HideAll()
        {
            EnsureUi();
            if (panel != null)
            {
                panel.gameObject.SetActive(false);
            }

            if (objectivePanel != null)
            {
                objectivePanel.gameObject.SetActive(false);
            }

            if (questionRoot != null)
            {
                questionRoot.gameObject.SetActive(false);
            }

            if (brushRoot != null)
            {
                brushRoot.gameObject.SetActive(false);
            }

            activeSpeaker = string.Empty;
            activeText = string.Empty;
        }

        public void SetCameraForReview(Camera camera)
        {
            targetCamera = camera;
            if (canvas != null)
            {
                canvas.worldCamera = targetCamera;
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

            var canvasObject = new GameObject("FastVS_StoryDialogueCanvas_TMP");
            canvasObject.transform.SetParent(transform, false);
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = targetCamera;
            canvas.planeDistance = 1f;
            canvas.sortingOrder = 5000;
            scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            if (fontAsset != null)
            {
                TMP_Settings.defaultFontAsset = fontAsset;
            }

            panel = CreatePanel("DialoguePanel", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(1172f, 124f), new Vector2(0f, 44f), new Color(0.025f, 0.022f, 0.022f, 0.88f));
            CreateRect(panel, "TopStripe", AnchorTopStretch(), new Vector2(0f, 3f), new Vector2(0f, -1.5f), new Color(0.95f, 0.57f, 0.26f, 0.92f));
            speakerText = CreateText(panel, "Speaker", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(240f, 30f), new Vector2(22f, -14f), 22f, FontStyles.Bold, TextAlignmentOptions.TopLeft, new Color(1f, 0.80f, 0.48f, 1f));
            bodyText = CreateText(panel, "Body", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(-44f, 46f), new Vector2(22f, -48f), 24f, FontStyles.Normal, TextAlignmentOptions.TopLeft, new Color(0.96f, 0.94f, 0.88f, 1f));
            bodyText.textWrappingMode = TextWrappingModes.Normal;
            advanceText = CreateText(panel, "Advance", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(170f, 24f), new Vector2(-20f, 8f), 15f, FontStyles.Normal, TextAlignmentOptions.MidlineRight, new Color(0.80f, 0.77f, 0.68f, 1f));

            objectivePanel = CreatePanel("ObjectivePanel", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(520f, 40f), new Vector2(18f, 18f), new Color(0.025f, 0.022f, 0.022f, 0.72f));
            objectiveText = CreateText(objectivePanel, "Objective", new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(-28f, -16f), new Vector2(14f, 8f), 16f, FontStyles.Normal, TextAlignmentOptions.MidlineLeft, new Color(0.94f, 0.91f, 0.84f, 1f));

            questionRoot = CreatePanel("QuestionRoot", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(36f, 36f), Vector2.zero, Color.clear);
            questionText = CreateText(questionRoot, "Question", new Vector2(0f, 0f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero, 30f, FontStyles.Bold, TextAlignmentOptions.Center, new Color(1f, 0.93f, 0.52f, 1f));
            questionText.text = "?";

            brushRoot = CreatePanel("BrushRoot", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(224f, 224f), Vector2.zero, new Color(0.035f, 0.030f, 0.026f, 0.78f));
            CreateRect(brushRoot, "Frame", (Vector2.zero, Vector2.one), new Vector2(-16f, -16f), Vector2.zero, new Color(0.34f, 0.24f, 0.14f, 0.96f));
            CreateRect(brushRoot, "Pane", (Vector2.zero, Vector2.one), new Vector2(-36f, -36f), Vector2.zero, new Color(0.07f, 0.055f, 0.045f, 0.96f));
            brushImage = CreateRawImage(brushRoot, "TimewriterBrushIcon", Vector2.zero, Vector2.one, new Vector2(-48f, -48f), Vector2.zero, brushIconTexture);
            ApplyReadableTextStyle();
        }

        private static RawImage CreateRawImage(RectTransform parent, string objectName, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Texture texture)
        {
            var imageObject = new GameObject(objectName);
            imageObject.transform.SetParent(parent, false);
            var rect = imageObject.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            var image = imageObject.AddComponent<RawImage>();
            image.texture = texture;
            image.color = Color.white;
            image.raycastTarget = false;
            return image;
        }

        private RectTransform CreatePanel(string objectName, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, Color color)
        {
            var rect = CreateRect((RectTransform)canvas.transform, objectName, (anchorMin, anchorMax), size, position, Color.clear);
            rect.pivot = new Vector2(anchorMin.x <= 0.01f ? 0f : 0.5f, anchorMin.y <= 0.01f && anchorMax.y <= 0.01f ? 0f : 0.5f);
            if (color.a > 0f)
            {
                var background = CreateRect(rect, "Background", (Vector2.zero, Vector2.one), Vector2.zero, Vector2.zero, color);
                background.SetAsFirstSibling();
            }

            return rect;
        }

        private static RectTransform CreateRect(RectTransform parent, string objectName, (Vector2 Min, Vector2 Max) anchors, Vector2 size, Vector2 position, Color color)
        {
            var rectObject = new GameObject(objectName);
            rectObject.transform.SetParent(parent, false);
            var rect = rectObject.AddComponent<RectTransform>();
            rect.anchorMin = anchors.Min;
            rect.anchorMax = anchors.Max;
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            if (color.a > 0f)
            {
                var image = rectObject.AddComponent<RawImage>();
                image.texture = Texture2D.whiteTexture;
                image.color = color;
                image.raycastTarget = false;
            }

            return rect;
        }

        private static RectTransform CreateRect(RectTransform parent, string objectName, (Vector2 Min, Vector2 Max, Vector2 Pivot) anchors, Vector2 size, Vector2 position, Color color)
        {
            var rect = CreateRect(parent, objectName, (anchors.Min, anchors.Max), size, position, color);
            rect.pivot = anchors.Pivot;
            return rect;
        }

        private TextMeshProUGUI CreateText(RectTransform parent, string objectName, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 position, float fontSize, FontStyles style, TextAlignmentOptions alignment, Color color)
        {
            var textObject = new GameObject(objectName);
            textObject.transform.SetParent(parent, false);
            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = new Vector2(anchorMin.x <= 0.01f ? 0f : 1f, anchorMin.y >= 0.99f ? 1f : 0f);
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            var text = textObject.AddComponent<TextMeshProUGUI>();
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
            text.faceColor = Color.white;
            text.outlineColor = new Color(0f, 0f, 0f, 0.84f);
            text.outlineWidth = 0.08f;
            text.raycastTarget = false;
            text.characterSpacing = 0f;
            text.lineSpacing = 4f;
            return text;
        }

        private void ApplyReadableTextStyle()
        {
            if (fontAsset == null)
            {
                return;
            }

            var readableMaterial = GetReadableFontMaterial();
            ApplyReadableTextStyle(speakerText, readableMaterial);
            ApplyReadableTextStyle(bodyText, readableMaterial);
            ApplyReadableTextStyle(advanceText, readableMaterial);
            ApplyReadableTextStyle(objectiveText, readableMaterial);
            ApplyReadableTextStyle(questionText, readableMaterial);
        }

        private void ApplyReadableTextStyle(TextMeshProUGUI text, Material readableMaterial)
        {
            if (text == null)
            {
                return;
            }

            text.font = fontAsset;
            if (readableMaterial != null)
            {
                text.fontMaterial = readableMaterial;
                text.fontSharedMaterial = readableMaterial;
            }

            text.faceColor = Color.white;
            text.outlineColor = new Color(0f, 0f, 0f, 0.84f);
            text.outlineWidth = 0.08f;
            text.ForceMeshUpdate(true, true);
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
                name = $"{fontAsset.name}_FastVSDialogueReadable"
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
                readableFontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, new Color(0f, 0f, 0f, 0.84f));
            }

            if (readableFontMaterial.HasProperty(ShaderUtilities.ID_OutlineWidth))
            {
                readableFontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.08f);
            }

            return readableFontMaterial;
        }

        private static (Vector2 Min, Vector2 Max, Vector2 Pivot) AnchorTopStretch()
        {
            return (new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f));
        }

        private static (Vector2 Min, Vector2 Max, Vector2 Pivot) CenterAnchor()
        {
            return (new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        }
    }
}
