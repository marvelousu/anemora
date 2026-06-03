using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dParallaxBackdropRig : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dParallaxBackdropProfile profile;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Renderer[] layerRenderers = System.Array.Empty<Renderer>();
        [SerializeField] private Vector3 parallaxAnchor;
        [SerializeField] private bool publishEveryFrame = true;

        public bool IsReadyForReview => profile != null && layerRenderers != null && layerRenderers.Length >= profile.LayerCountForReview;
        public int LayerCountForReview => layerRenderers != null ? layerRenderers.Length : 0;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public Vector3 ParallaxAnchorForReview => parallaxAnchor;
        public float LastCameraDeltaXForReview { get; private set; }
        public float LastNearLayerXForReview { get; private set; }
        public float LastFarLayerXForReview { get; private set; }

        private void OnEnable()
        {
            PublishForReview();
        }

        private void LateUpdate()
        {
            if (publishEveryFrame)
            {
                PublishForReview();
            }
        }

        public void ConfigureForReview(
            FastVsHd2dParallaxBackdropProfile authoredProfile,
            Camera camera,
            Renderer[] renderers,
            Vector3 anchor)
        {
            profile = authoredProfile;
            sceneCamera = camera;
            layerRenderers = renderers ?? System.Array.Empty<Renderer>();
            parallaxAnchor = anchor;
            publishEveryFrame = true;
            PublishForReview();
        }

        public void SetLayersVisibleForReview(bool visible)
        {
            if (layerRenderers == null)
            {
                return;
            }

            foreach (var layerRenderer in layerRenderers)
            {
                if (layerRenderer != null)
                {
                    layerRenderer.enabled = visible;
                }
            }
        }

        public Vector3 GetLayerWorldPositionForReview(int index)
        {
            if (layerRenderers == null || index < 0 || index >= layerRenderers.Length || layerRenderers[index] == null)
            {
                return Vector3.zero;
            }

            return layerRenderers[index].transform.position;
        }

        public float GetLayerParallaxFactorForReview(int index)
        {
            var layer = profile != null ? profile.GetLayerForReview(index) : null;
            return layer != null ? layer.ParallaxFactorForReview : 0f;
        }

        public Color GetLayerTintForReview(int index)
        {
            var layer = profile != null ? profile.GetLayerForReview(index) : null;
            return layer != null ? layer.TintForReview : Color.clear;
        }

        public string GetLayerIdForReview(int index)
        {
            var layer = profile != null ? profile.GetLayerForReview(index) : null;
            return layer != null ? layer.LayerIdForReview : string.Empty;
        }

        public void PublishForReview()
        {
            if (profile == null || layerRenderers == null)
            {
                return;
            }

            if (sceneCamera == null)
            {
                sceneCamera = Camera.main;
            }

            LastCameraDeltaXForReview = sceneCamera != null
                ? sceneCamera.transform.position.x - parallaxAnchor.x
                : 0f;

            for (var index = 0; index < layerRenderers.Length; index++)
            {
                var layerRenderer = layerRenderers[index];
                var layer = profile.GetLayerForReview(index);
                if (layerRenderer == null || layer == null)
                {
                    continue;
                }

                var parallaxOffset = new Vector3(LastCameraDeltaXForReview * layer.ParallaxFactorForReview, 0f, 0f);
                layerRenderer.transform.localPosition = layer.LocalPositionForReview + parallaxOffset;
                layerRenderer.transform.localScale = layer.LocalScaleForReview;
                if (sceneCamera != null)
                {
                    var toCamera = layerRenderer.transform.position - sceneCamera.transform.position;
                    if (toCamera.sqrMagnitude > 0.01f)
                    {
                        layerRenderer.transform.rotation = Quaternion.LookRotation(toCamera.normalized, Vector3.up);
                    }
                }

                if (index == 0)
                {
                    LastNearLayerXForReview = layerRenderer.transform.position.x;
                }

                if (index == layerRenderers.Length - 1)
                {
                    LastFarLayerXForReview = layerRenderer.transform.position.x;
                }
            }
        }
    }
}
