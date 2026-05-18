using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsSpriteStripLoopAnimator : MonoBehaviour
    {
        [SerializeField] private Renderer spriteRenderer;
        [SerializeField] private int frameCount = 4;
        [SerializeField] private float framesPerSecond = 2.2f;

        private MaterialPropertyBlock propertyBlock;
        private int currentFrame;

        public int CurrentFrameForReview => currentFrame;
        public int FrameCountForReview => frameCount;

        private void Awake()
        {
            ResolveReferences();
            ApplyFrame();
        }

        private void Update()
        {
            ResolveReferences();
            if (spriteRenderer == null)
            {
                return;
            }

            var frames = Mathf.Max(1, frameCount);
            var nextFrame = Mathf.FloorToInt(Time.time * Mathf.Max(0.1f, framesPerSecond)) % frames;
            if (nextFrame == currentFrame)
            {
                return;
            }

            currentFrame = nextFrame;
            ApplyFrame();
        }

        private void ApplyFrame()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            var frames = Mathf.Max(1, frameCount);
            var frameWidth = 1f / frames;
            var offsetX = Mathf.Clamp(currentFrame, 0, frames - 1) * frameWidth;
            propertyBlock ??= new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetVector("_BaseMap_ST", new Vector4(frameWidth, 1f, offsetX, 0f));
            propertyBlock.SetVector("_MainTex_ST", new Vector4(frameWidth, 1f, offsetX, 0f));
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        private void ResolveReferences()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<Renderer>();
            }
        }
    }
}
