using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

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

        private ScriptableRenderPass maskPass;
        private ScriptableRenderPass insidePass;

        public override void Create()
        {
            var disabledStencil = StencilState.defaultValue;
            maskPass = new DrawObjectsPass(
                "Anemora Portal Mask",
                new[] { new ShaderTagId("AnemoraPortalMask") },
                true,
                passEvent,
                RenderQueueRange.all,
                portalMaskLayers,
                disabledStencil,
                0);

            insidePass = new DrawObjectsPass(
                "Anemora Portal Inside",
                new[] { new ShaderTagId("AnemoraPortalInside") },
                true,
                (RenderPassEvent)((int)passEvent + 1),
                RenderQueueRange.all,
                insidePortalLayers,
                disabledStencil,
                0);
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

        public static void ResetDiagnosticsForTests()
        {
            LastEnqueueFrame = -1;
            LastEnqueuedPassCount = 0;
            LastCameraName = string.Empty;
        }
    }
}
