using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/FastVS/HD2D Menu Layout Profile")]
    public sealed class FastVsHd2dMenuLayoutProfile : ScriptableObject
    {
        [SerializeField] private bool needsTomApproval = true;
        [SerializeField] private bool finalMenuLayoutApproved;
        [SerializeField] private bool reuseDialogueNineSliceFrame = true;
        [SerializeField] private bool thirdsGridLayoutEnabled = true;
        [SerializeField] private bool selectedFrameSwapEnabled = true;
        [SerializeField] private bool screenSpaceCameraCanvas = true;
        [SerializeField] private bool integerPixelUiScale = true;
        [SerializeField] private bool editorCompositeBackdropBlurEnabled = true;
        [SerializeField] private bool runtimeRendererFeatureDeferredForTom = true;
        [SerializeField, Range(0f, 1f)] private float backdropDimAlpha = 0.58f;
        [SerializeField, Range(0f, 0.8f)] private float unselectedFocusAlpha = 0.54f;
        [SerializeField, Range(0, 24)] private int backdropBlurRadiusPixels = 8;
        [SerializeField] private Vector2 inventoryPanelThirdsAnchor = new Vector2(1f / 3f, 0.52f);
        [SerializeField] private Vector2 detailPanelThirdsAnchor = new Vector2(2f / 3f, 0.52f);
        [SerializeField] private Vector2 headerPanelAnchor = new Vector2(0.5f, 0.84f);
        [SerializeField] private Vector2 footerPanelAnchor = new Vector2(0.5f, 0.15f);
        [SerializeField] private Vector2 inventoryPanelSize = new Vector2(560f, 660f);
        [SerializeField] private Vector2 detailPanelSize = new Vector2(620f, 660f);
        [SerializeField] private Vector2 headerPanelSize = new Vector2(720f, 96f);
        [SerializeField] private Vector2 footerPanelSize = new Vector2(940f, 74f);
        [SerializeField] private Color panelFillColor = new Color(0.024f, 0.020f, 0.024f, 0.92f);
        [SerializeField] private Color selectedFrameTint = new Color(1.0f, 0.78f, 0.36f, 0.92f);
        [SerializeField] private Color selectedTextColor = new Color(1.0f, 0.90f, 0.62f, 1f);
        [SerializeField] private Color unselectedTextColor = new Color(0.82f, 0.80f, 0.72f, 0.82f);
        [SerializeField, TextArea] private string recommendation = "Keep this as conservative P2-71 menu layout data only. Tom should approve final frame art, blur strength, panel density, selected-state color, and whether a real Render Graph blur pass replaces the editor composite proof.";

        public bool NeedsTomApprovalForReview => needsTomApproval;
        public bool FinalMenuLayoutApprovedForReview => finalMenuLayoutApproved;
        public bool ReuseDialogueNineSliceFrameForReview => reuseDialogueNineSliceFrame;
        public bool ThirdsGridLayoutEnabledForReview => thirdsGridLayoutEnabled;
        public bool SelectedFrameSwapEnabledForReview => selectedFrameSwapEnabled;
        public bool ScreenSpaceCameraCanvasForReview => screenSpaceCameraCanvas;
        public bool IntegerPixelUiScaleForReview => integerPixelUiScale;
        public bool EditorCompositeBackdropBlurEnabledForReview => editorCompositeBackdropBlurEnabled;
        public bool RuntimeRendererFeatureDeferredForTomForReview => runtimeRendererFeatureDeferredForTom;
        public float BackdropDimAlphaForReview => backdropDimAlpha;
        public float UnselectedFocusAlphaForReview => unselectedFocusAlpha;
        public int BackdropBlurRadiusPixelsForReview => backdropBlurRadiusPixels;
        public Vector2 InventoryPanelThirdsAnchorForReview => inventoryPanelThirdsAnchor;
        public Vector2 DetailPanelThirdsAnchorForReview => detailPanelThirdsAnchor;
        public Vector2 HeaderPanelAnchorForReview => headerPanelAnchor;
        public Vector2 FooterPanelAnchorForReview => footerPanelAnchor;
        public Vector2 InventoryPanelSizeForReview => inventoryPanelSize;
        public Vector2 DetailPanelSizeForReview => detailPanelSize;
        public Vector2 HeaderPanelSizeForReview => headerPanelSize;
        public Vector2 FooterPanelSizeForReview => footerPanelSize;
        public Color PanelFillColorForReview => panelFillColor;
        public Color SelectedFrameTintForReview => selectedFrameTint;
        public Color SelectedTextColorForReview => selectedTextColor;
        public Color UnselectedTextColorForReview => unselectedTextColor;
        public string RecommendationForReview => recommendation;

        public void ConfigureForReview(
            float dimAlpha,
            float focusAlpha,
            int blurRadiusPixels,
            Vector2 inventoryAnchor,
            Vector2 detailAnchor,
            Vector2 headerAnchor,
            Vector2 footerAnchor,
            Vector2 inventorySize,
            Vector2 detailSize,
            Vector2 headerSize,
            Vector2 footerSize,
            Color fillColor,
            Color selectedTint,
            Color selectedText,
            Color unselectedText,
            string recommendationText)
        {
            needsTomApproval = true;
            finalMenuLayoutApproved = false;
            reuseDialogueNineSliceFrame = true;
            thirdsGridLayoutEnabled = true;
            selectedFrameSwapEnabled = true;
            screenSpaceCameraCanvas = true;
            integerPixelUiScale = true;
            editorCompositeBackdropBlurEnabled = true;
            runtimeRendererFeatureDeferredForTom = true;
            backdropDimAlpha = Mathf.Clamp01(dimAlpha);
            unselectedFocusAlpha = Mathf.Clamp01(focusAlpha);
            backdropBlurRadiusPixels = Mathf.Clamp(blurRadiusPixels, 0, 24);
            inventoryPanelThirdsAnchor = inventoryAnchor;
            detailPanelThirdsAnchor = detailAnchor;
            headerPanelAnchor = headerAnchor;
            footerPanelAnchor = footerAnchor;
            inventoryPanelSize = inventorySize;
            detailPanelSize = detailSize;
            headerPanelSize = headerSize;
            footerPanelSize = footerSize;
            panelFillColor = fillColor;
            selectedFrameTint = selectedTint;
            selectedTextColor = selectedText;
            unselectedTextColor = unselectedText;
            recommendation = recommendationText;
        }
    }
}
