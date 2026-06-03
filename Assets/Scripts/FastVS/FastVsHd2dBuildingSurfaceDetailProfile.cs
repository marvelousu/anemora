using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Building Surface Detail Profile")]
    public sealed class FastVsHd2dBuildingSurfaceDetailProfile : ScriptableObject
    {
        [SerializeField, Range(0.1f, 2f)] private float roofTileRowSpacing = 0.28f;
        [SerializeField, Range(0.005f, 0.08f)] private float roofTileRowThickness = 0.024f;
        [SerializeField, Range(0f, 1f)] private float roofMicroShadowStrength = 0.42f;
        [SerializeField, Range(0.02f, 0.18f)] private float timberBeamThickness = 0.075f;
        [SerializeField, Range(0f, 1f)] private float wallBandingStrength = 0.36f;
        [SerializeField, Range(0f, 1f)] private float generatedNormalStrength = 0.35f;
        [SerializeField, Range(0f, 1f)] private float edgeAccentStrength = 0.48f;
        [SerializeField, Range(0f, 1f)] private float cavityShadeStrength = 0.34f;
        [SerializeField] private Vector2 roofTextureScale = new Vector2(5.5f, 3.2f);
        [SerializeField] private Vector2 wallTextureScale = new Vector2(4.0f, 2.8f);
        [SerializeField] private bool generatedNormalMapsReady;
        [SerializeField] private bool shaderHasNormalMapSlot;
        [SerializeField] private bool shaderHasParallaxSlot;
        [SerializeField] private bool conservativeReviewMode = true;
        [SerializeField] private bool requiresTomArtApproval = true;
        [SerializeField] private string roofAlbedoTexturePath = string.Empty;
        [SerializeField] private string roofNormalTexturePath = string.Empty;
        [SerializeField] private string roofHeightTexturePath = string.Empty;
        [SerializeField] private string wallAlbedoTexturePath = string.Empty;
        [SerializeField] private string wallNormalTexturePath = string.Empty;
        [SerializeField] private string wallHeightTexturePath = string.Empty;
        [SerializeField] private string sourceTextureNote = "Procedural CC0-safe review baseline; replace with approved CC0 roof/wall sources.";

        public float RoofTileRowSpacingForReview => roofTileRowSpacing;
        public float RoofTileRowThicknessForReview => roofTileRowThickness;
        public float RoofMicroShadowStrengthForReview => roofMicroShadowStrength;
        public float TimberBeamThicknessForReview => timberBeamThickness;
        public float WallBandingStrengthForReview => wallBandingStrength;
        public float GeneratedNormalStrengthForReview => generatedNormalStrength;
        public float EdgeAccentStrengthForReview => edgeAccentStrength;
        public float CavityShadeStrengthForReview => cavityShadeStrength;
        public Vector2 RoofTextureScaleForReview => roofTextureScale;
        public Vector2 WallTextureScaleForReview => wallTextureScale;
        public bool GeneratedNormalMapsReadyForReview => generatedNormalMapsReady;
        public bool ShaderHasNormalMapSlotForReview => shaderHasNormalMapSlot;
        public bool ShaderHasParallaxSlotForReview => shaderHasParallaxSlot;
        public bool ConservativeReviewModeForReview => conservativeReviewMode;
        public bool RequiresTomArtApprovalForReview => requiresTomArtApproval;
        public string RoofAlbedoTexturePathForReview => roofAlbedoTexturePath;
        public string RoofNormalTexturePathForReview => roofNormalTexturePath;
        public string RoofHeightTexturePathForReview => roofHeightTexturePath;
        public string WallAlbedoTexturePathForReview => wallAlbedoTexturePath;
        public string WallNormalTexturePathForReview => wallNormalTexturePath;
        public string WallHeightTexturePathForReview => wallHeightTexturePath;
        public string SourceTextureNoteForReview => sourceTextureNote;
        public int LayerCountForReview => 2;
        public bool UsesGeometryFallbackForReview => !shaderHasNormalMapSlot || !shaderHasParallaxSlot;

        public void ConfigureForReview(
            float configuredRoofTileRowSpacing,
            float configuredRoofTileRowThickness,
            float configuredRoofMicroShadowStrength,
            float configuredTimberBeamThickness,
            float configuredWallBandingStrength,
            float configuredGeneratedNormalStrength,
            float configuredEdgeAccentStrength,
            float configuredCavityShadeStrength,
            Vector2 configuredRoofTextureScale,
            Vector2 configuredWallTextureScale,
            bool configuredGeneratedNormalMapsReady,
            bool configuredShaderHasNormalMapSlot,
            bool configuredShaderHasParallaxSlot,
            bool configuredConservativeReviewMode,
            bool configuredRequiresTomArtApproval,
            string configuredRoofAlbedoTexturePath,
            string configuredRoofNormalTexturePath,
            string configuredRoofHeightTexturePath,
            string configuredWallAlbedoTexturePath,
            string configuredWallNormalTexturePath,
            string configuredWallHeightTexturePath,
            string configuredSourceTextureNote)
        {
            roofTileRowSpacing = Mathf.Clamp(configuredRoofTileRowSpacing, 0.1f, 2f);
            roofTileRowThickness = Mathf.Clamp(configuredRoofTileRowThickness, 0.005f, 0.08f);
            roofMicroShadowStrength = Mathf.Clamp01(configuredRoofMicroShadowStrength);
            timberBeamThickness = Mathf.Clamp(configuredTimberBeamThickness, 0.02f, 0.18f);
            wallBandingStrength = Mathf.Clamp01(configuredWallBandingStrength);
            generatedNormalStrength = Mathf.Clamp01(configuredGeneratedNormalStrength);
            edgeAccentStrength = Mathf.Clamp01(configuredEdgeAccentStrength);
            cavityShadeStrength = Mathf.Clamp01(configuredCavityShadeStrength);
            roofTextureScale = configuredRoofTextureScale;
            wallTextureScale = configuredWallTextureScale;
            generatedNormalMapsReady = configuredGeneratedNormalMapsReady;
            shaderHasNormalMapSlot = configuredShaderHasNormalMapSlot;
            shaderHasParallaxSlot = configuredShaderHasParallaxSlot;
            conservativeReviewMode = configuredConservativeReviewMode;
            requiresTomArtApproval = configuredRequiresTomArtApproval;
            roofAlbedoTexturePath = configuredRoofAlbedoTexturePath ?? string.Empty;
            roofNormalTexturePath = configuredRoofNormalTexturePath ?? string.Empty;
            roofHeightTexturePath = configuredRoofHeightTexturePath ?? string.Empty;
            wallAlbedoTexturePath = configuredWallAlbedoTexturePath ?? string.Empty;
            wallNormalTexturePath = configuredWallNormalTexturePath ?? string.Empty;
            wallHeightTexturePath = configuredWallHeightTexturePath ?? string.Empty;
            sourceTextureNote = configuredSourceTextureNote ?? string.Empty;
        }
    }
}
