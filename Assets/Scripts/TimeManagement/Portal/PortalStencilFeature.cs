using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.TimeManagement.Portal
{
    /// <summary>
    /// URP renderer feature reserved for time portal stencil passes.
    /// </summary>
    public sealed class PortalStencilFeature : ScriptableRendererFeature
    {
        public const int StencilBit = 3;
        public const int StencilMask = 1 << StencilBit;

        [SerializeField] private RenderPassEvent passEvent = RenderPassEvent.AfterRenderingOpaques;
        [SerializeField] private LayerMask portalMaskLayers = ~0;
        [SerializeField] private LayerMask insidePortalLayers = ~0;

        public RenderPassEvent PassEvent => passEvent;
        public LayerMask PortalMaskLayers => portalMaskLayers;
        public LayerMask InsidePortalLayers => insidePortalLayers;

        public static int LastEnqueueFrame { get; private set; } = -1;
        public static int LastEnqueuedPassCount { get; private set; }
        public static string LastCameraName { get; private set; } = string.Empty;

        private RenderObjectsPass maskPass;
        private RenderObjectsPass insidePass;

        public override void Create()
        {
            maskPass = CreatePortalPass(
                "Anemora Portal Mask",
                passEvent,
                new[] { "AnemoraPortalMask" },
                portalMaskLayers);

            insidePass = CreatePortalPass(
                "Anemora Portal Inside",
                (RenderPassEvent)((int)passEvent + 1),
                new[] { "AnemoraPortalInside" },
                insidePortalLayers);
        }

        private static RenderObjectsPass CreatePortalPass(
            string profilerTag,
            RenderPassEvent renderPassEvent,
            string[] shaderTags,
            LayerMask layerMask)
        {
            return new RenderObjectsPass(
                profilerTag,
                renderPassEvent,
                shaderTags,
                RenderQueueType.Opaque,
                layerMask.value,
                new RenderObjects.CustomCameraSettings());
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (maskPass == null || insidePass == null)
            {
                Create();
            }

            renderer.EnqueuePass(maskPass);
            renderer.EnqueuePass(insidePass);

            LastEnqueueFrame = Time.frameCount;
            LastEnqueuedPassCount = 2;
            LastCameraName = renderingData.cameraData.camera != null
                ? renderingData.cameraData.camera.name
                : string.Empty;
        }

        public void SetLayerMasks(LayerMask portalMaskLayers, LayerMask insidePortalLayers)
        {
            this.portalMaskLayers = portalMaskLayers;
            this.insidePortalLayers = insidePortalLayers;
            Create();
        }

        public static void ResetDiagnosticsForTests()
        {
            LastEnqueueFrame = -1;
            LastEnqueuedPassCount = 0;
            LastCameraName = string.Empty;
        }
    }
}
