using UnityEngine;
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

        public RenderPassEvent PassEvent => passEvent;

        public override void Create()
        {
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            // E1 will enqueue mask and inside-portal draw passes here.
        }
    }
}
