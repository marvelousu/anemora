using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Hero NPC Outline Preview")]
    public sealed class FastVsHd2dHeroNpcOutlinePreview : MonoBehaviour
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int OutlineWidthTexelsId = Shader.PropertyToID("_OutlineWidthTexels");
        private static readonly int OutlineAlphaId = Shader.PropertyToID("_OutlineAlpha");
        private static readonly int CutoffId = Shader.PropertyToID("_Cutoff");

        [SerializeField] private FastVsHd2dHeroNpcOutlineProfile profile;
        [SerializeField] private MeshRenderer baseSpriteRenderer;
        [SerializeField] private MeshRenderer outlineRenderer;
        [SerializeField] private MeshRenderer washedBackdropRenderer;
        [SerializeField] private bool activeOnAwake;
        [SerializeField] private bool currentOutlineEnabled;
        [SerializeField] private bool currentWashedScene;
        [SerializeField] private float currentOutlineWidthTexels;

        public bool IsReadyForReview => profile != null && baseSpriteRenderer != null && outlineRenderer != null && washedBackdropRenderer != null;
        public FastVsHd2dHeroNpcOutlineProfile ProfileForReview => profile;
        public MeshRenderer BaseSpriteRendererForReview => baseSpriteRenderer;
        public MeshRenderer OutlineRendererForReview => outlineRenderer;
        public MeshRenderer WashedBackdropRendererForReview => washedBackdropRenderer;
        public bool ActiveOnAwakeForReview => activeOnAwake;
        public bool CurrentOutlineEnabledForReview => currentOutlineEnabled;
        public bool CurrentWashedSceneForReview => currentWashedScene;
        public float CurrentOutlineWidthTexelsForReview => currentOutlineWidthTexels;
        public bool BaseSpriteVisibleForReview => baseSpriteRenderer != null && baseSpriteRenderer.enabled;
        public bool OutlineVisibleForReview => outlineRenderer != null && outlineRenderer.enabled;
        public bool WashedBackdropVisibleForReview => washedBackdropRenderer != null && washedBackdropRenderer.enabled;
        public Material OutlineMaterialForReview => outlineRenderer != null ? outlineRenderer.sharedMaterial : null;

        private void OnEnable()
        {
            if (!activeOnAwake)
            {
                ApplyDefaultReviewStateForReview();
            }
        }

        private void OnValidate()
        {
            currentOutlineWidthTexels = Mathf.Clamp(currentOutlineWidthTexels, 0f, 3f);
        }

        public void ConfigureForReview(
            FastVsHd2dHeroNpcOutlineProfile configuredProfile,
            MeshRenderer configuredBaseSpriteRenderer,
            MeshRenderer configuredOutlineRenderer,
            MeshRenderer configuredWashedBackdropRenderer)
        {
            profile = configuredProfile;
            baseSpriteRenderer = configuredBaseSpriteRenderer;
            outlineRenderer = configuredOutlineRenderer;
            washedBackdropRenderer = configuredWashedBackdropRenderer;
            activeOnAwake = profile != null && profile.ActiveOnAwakeForReview;
            ApplyDefaultReviewStateForReview();
        }

        public void ApplyDefaultReviewStateForReview()
        {
            currentOutlineEnabled = false;
            currentWashedScene = false;
            currentOutlineWidthTexels = 0f;
            SetRenderersVisible(false);
        }

        public void SetReviewVisibleForReview(bool visible)
        {
            SetRenderersVisible(visible);
        }

        public void ApplyReviewStateForReview(bool outlineEnabled, float outlineWidthTexels, bool washedScene, bool strongerOption)
        {
            currentOutlineEnabled = outlineEnabled;
            currentWashedScene = washedScene;
            currentOutlineWidthTexels = Mathf.Clamp(outlineWidthTexels, 0.25f, 3f);
            PublishMaterialState(strongerOption);
            SetRenderersVisible(true);
        }

        private void SetRenderersVisible(bool visible)
        {
            if (baseSpriteRenderer != null)
            {
                baseSpriteRenderer.enabled = visible;
            }

            if (washedBackdropRenderer != null)
            {
                washedBackdropRenderer.enabled = visible && currentWashedScene;
            }

            if (outlineRenderer != null)
            {
                outlineRenderer.enabled = visible && currentOutlineEnabled;
            }
        }

        private void PublishMaterialState(bool strongerOption)
        {
            if (profile == null)
            {
                return;
            }

            var baseMaterial = baseSpriteRenderer != null ? baseSpriteRenderer.sharedMaterial : null;
            if (baseMaterial != null)
            {
                var washedColor = new Color(0.86f, 0.84f, 0.78f, profile.WashedSpriteAlphaForReview);
                if (baseMaterial.HasProperty(BaseColorId))
                {
                    baseMaterial.SetColor(BaseColorId, washedColor);
                }

                if (baseMaterial.HasProperty(ColorId))
                {
                    baseMaterial.SetColor(ColorId, washedColor);
                }
            }

            var outlineMaterial = OutlineMaterialForReview;
            if (outlineMaterial == null)
            {
                return;
            }

            var outlineColor = profile.OutlineColorForReview;
            outlineColor.a = Mathf.Clamp01(profile.OutlineAlphaForReview * (strongerOption ? 1.0f : 0.92f));
            if (outlineMaterial.HasProperty(BaseColorId))
            {
                outlineMaterial.SetColor(BaseColorId, outlineColor);
            }

            if (outlineMaterial.HasProperty(ColorId))
            {
                outlineMaterial.SetColor(ColorId, outlineColor);
            }

            if (outlineMaterial.HasProperty(OutlineWidthTexelsId))
            {
                outlineMaterial.SetFloat(OutlineWidthTexelsId, currentOutlineWidthTexels);
            }

            if (outlineMaterial.HasProperty(OutlineAlphaId))
            {
                outlineMaterial.SetFloat(OutlineAlphaId, outlineColor.a);
            }

            if (outlineMaterial.HasProperty(CutoffId))
            {
                outlineMaterial.SetFloat(CutoffId, profile.AlphaCutoffForReview);
            }
        }
    }
}
