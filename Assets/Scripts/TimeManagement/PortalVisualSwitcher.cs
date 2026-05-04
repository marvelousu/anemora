using Anemora.TimeManagement.Portal;
using UnityEngine;

namespace Anemora.TimeManagement
{
    /// <summary>
    /// Applies camera, player layer, portal mask layer, and stencil-layer polarity together.
    /// </summary>
    public sealed class PortalVisualSwitcher : MonoBehaviour
    {
        private const int CurrentColliderLayer = 8;
        private const int PastColliderLayer = 9;
        private const int CurrentVisualLayer = 10;
        private const int PastVisualLayer = 11;
        private const int UiLayer = 5;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask currentVisualMask = 1 << CurrentVisualLayer;
        [SerializeField] private LayerMask pastVisualMask = 1 << PastVisualLayer;
        [SerializeField] private LayerMask currentColliderMask = 1 << CurrentColliderLayer;
        [SerializeField] private LayerMask pastColliderMask = 1 << PastColliderLayer;
        [SerializeField] private LayerMask additionalCameraMask = 1 << UiLayer;
        [SerializeField] private GameObject portalMaskRoot;
        [SerializeField] private PortalStencilFeature[] stencilFeatures;

        public SceneSide LastAppliedSide { get; private set; } = SceneSide.Current;
        public int LastAppliedCameraMask { get; private set; }
        public int LastAppliedPlayerLayer { get; private set; } = CurrentColliderLayer;
        public LayerMask LastPortalMaskLayers { get; private set; }
        public LayerMask LastInsidePortalLayers { get; private set; }

        private void Awake()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        public void SetPortalMaskRoot(GameObject portalMaskRoot)
        {
            this.portalMaskRoot = portalMaskRoot;
        }

        public void ApplyForSide(SceneSide side, GameObject player)
        {
            ApplyForSide(side, player, portalMaskRoot);
        }

        public void ApplyForSide(SceneSide side, GameObject player, GameObject portalRoot)
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            var outsideVisualMask = side == SceneSide.Current ? currentVisualMask : pastVisualMask;
            var insideVisualMask = side == SceneSide.Current ? pastVisualMask : currentVisualMask;
            var playerLayer = LayerFromMask(side == SceneSide.Current ? currentColliderMask : pastColliderMask);
            var portalLayer = LayerFromMask(outsideVisualMask);

            if (mainCamera != null)
            {
                mainCamera.cullingMask = outsideVisualMask.value | additionalCameraMask.value;
                LastAppliedCameraMask = mainCamera.cullingMask;
            }

            if (player != null)
            {
                player.layer = playerLayer;
                LastAppliedPlayerLayer = player.layer;
            }

            if (portalRoot != null)
            {
                SetLayerRecursively(portalRoot, portalLayer);
                portalMaskRoot = portalRoot;
            }

            foreach (var feature in ResolveStencilFeatures())
            {
                if (feature != null)
                {
                    feature.SetLayerMasks(outsideVisualMask, insideVisualMask);
                }
            }

            LastAppliedSide = side;
            LastPortalMaskLayers = outsideVisualMask;
            LastInsidePortalLayers = insideVisualMask;
        }

        private PortalStencilFeature[] ResolveStencilFeatures()
        {
            if (stencilFeatures != null && stencilFeatures.Length > 0)
            {
                return stencilFeatures;
            }

            stencilFeatures = Resources.FindObjectsOfTypeAll<PortalStencilFeature>();
            return stencilFeatures;
        }

        private static int LayerFromMask(LayerMask mask)
        {
            var value = mask.value;
            for (var layer = 0; layer < 32; layer++)
            {
                if ((value & (1 << layer)) != 0)
                {
                    return layer;
                }
            }

            return 0;
        }

        private static void SetLayerRecursively(GameObject root, int layer)
        {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
    }
}
