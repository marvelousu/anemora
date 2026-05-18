using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsRetoWritingState
    {
        WritingRaised,
        Lowering,
        DialogueIdle,
        LookingUp,
        Raising
    }

    public sealed class FastVsRetoWritingAnimator : MonoBehaviour
    {
        [SerializeField] private Renderer spriteRenderer;
        [SerializeField] private Material writingLoopMaterial;
        [SerializeField] private Material lowerArmsMaterial;
        [SerializeField] private Material talkLoopMaterial;
        [SerializeField] private Material raiseArmsMaterial;
        [SerializeField] private int framePixelWidth = 64;
        [SerializeField] private int writingLoopFrameCount = 6;
        [SerializeField] private int lowerArmsFrameCount = 6;
        [SerializeField] private int talkLoopFrameCount = 4;
        [SerializeField] private int raiseArmsFrameCount = 6;
        [SerializeField] private float loopFramesPerSecond = 4f;
        [SerializeField] private float transitionFramesPerSecond = 9f;
        [SerializeField] private FastVsRetoWritingState currentState = FastVsRetoWritingState.WritingRaised;

        private MaterialPropertyBlock propertyBlock;
        private float stateStartedAt;
        private int currentFrame;

        public FastVsRetoWritingState CurrentStateForReview => currentState;
        public int CurrentFrameForReview => currentFrame;
        public Material ActiveMaterialForReview => spriteRenderer != null ? spriteRenderer.sharedMaterial : null;
        public bool HasFinalStateflowMaterialsForReview =>
            spriteRenderer != null &&
            writingLoopMaterial != null &&
            lowerArmsMaterial != null &&
            talkLoopMaterial != null &&
            raiseArmsMaterial != null &&
            MaterialLooksLikeFinalPack(writingLoopMaterial, "reto_v02_writing_loop") &&
            MaterialLooksLikeFinalPack(lowerArmsMaterial, "reto_v02_lower_arms") &&
            MaterialLooksLikeFinalPack(talkLoopMaterial, "reto_v02_talk_loop") &&
            MaterialLooksLikeFinalPack(raiseArmsMaterial, "reto_v02_raise_arms");

        private void Awake()
        {
            ResolveReferences();
            SetWritingImmediateForReview();
        }

        private void Update()
        {
            ResolveReferences();
            UpdateFrame();
        }

        public void SetWritingForReview()
        {
            if (currentState == FastVsRetoWritingState.WritingRaised ||
                currentState == FastVsRetoWritingState.Raising)
            {
                return;
            }

            ChangeState(FastVsRetoWritingState.Raising);
        }

        public void SetDialogueForReview()
        {
            if (currentState == FastVsRetoWritingState.DialogueIdle ||
                currentState == FastVsRetoWritingState.Lowering)
            {
                return;
            }

            if (currentState == FastVsRetoWritingState.LookingUp)
            {
                ChangeState(FastVsRetoWritingState.DialogueIdle, GetDialogueIdleFrameIndex());
                return;
            }

            if (currentState == FastVsRetoWritingState.WritingRaised)
            {
                ChangeState(FastVsRetoWritingState.Lowering);
            }
        }

        public void SetLookingUpForReview()
        {
            ChangeState(FastVsRetoWritingState.LookingUp, GetLookingUpFrameIndex());
        }

        public void SetLoweringForReview()
        {
            ChangeState(FastVsRetoWritingState.Lowering);
        }

        public void SetRaisingForReview()
        {
            ChangeState(FastVsRetoWritingState.Raising);
        }

        public void SetWritingImmediateForReview()
        {
            ChangeState(FastVsRetoWritingState.WritingRaised, 0);
        }

        public void SetDialogueImmediateForReview()
        {
            ChangeState(FastVsRetoWritingState.DialogueIdle, GetDialogueIdleFrameIndex());
        }

        private void ChangeState(FastVsRetoWritingState state, int frameIndex = 0)
        {
            if (currentState == state &&
                currentFrame == frameIndex &&
                spriteRenderer != null &&
                spriteRenderer.sharedMaterial == ResolveMaterial(state))
            {
                return;
            }

            currentState = state;
            stateStartedAt = Time.time;
            currentFrame = Mathf.Max(0, frameIndex);
            ApplyMaterialAndFrame();
        }

        private void UpdateFrame()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            var frameCount = ResolveFrameCount(currentState);
            var fps = currentState == FastVsRetoWritingState.WritingRaised ||
                      currentState == FastVsRetoWritingState.LookingUp ||
                      currentState == FastVsRetoWritingState.DialogueIdle
                ? loopFramesPerSecond
                : transitionFramesPerSecond;

            var elapsed = Mathf.Max(0f, Time.time - stateStartedAt);
            var frame = Mathf.FloorToInt(elapsed * Mathf.Max(1f, fps));
            if (currentState == FastVsRetoWritingState.Lowering && frame >= frameCount)
            {
                ChangeState(FastVsRetoWritingState.DialogueIdle, GetDialogueIdleFrameIndex());
                return;
            }

            if (currentState == FastVsRetoWritingState.Raising && frame >= frameCount)
            {
                ChangeState(FastVsRetoWritingState.WritingRaised);
                return;
            }

            if (currentState == FastVsRetoWritingState.LookingUp)
            {
                frame = GetLookingUpFrameIndex();
            }
            else
            {
                frame = frameCount > 0 ? frame % frameCount : 0;
            }

            if (frame == currentFrame)
            {
                return;
            }

            currentFrame = frame;
            ApplyMaterialAndFrame();
        }

        private void ApplyMaterialAndFrame()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            var material = ResolveMaterial(currentState);
            if (material != null && spriteRenderer.sharedMaterial != material)
            {
                spriteRenderer.sharedMaterial = material;
            }

            var frameCount = Mathf.Max(1, ResolveFrameCount(currentState));
            var frameWidth = 1f / frameCount;
            var offsetX = Mathf.Clamp(currentFrame, 0, frameCount - 1) * frameWidth;
            propertyBlock ??= new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetVector("_BaseMap_ST", new Vector4(frameWidth, 1f, offsetX, 0f));
            propertyBlock.SetVector("_MainTex_ST", new Vector4(frameWidth, 1f, offsetX, 0f));
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        private Material ResolveMaterial(FastVsRetoWritingState state)
        {
            switch (state)
            {
                case FastVsRetoWritingState.Lowering:
                    return lowerArmsMaterial;
                case FastVsRetoWritingState.DialogueIdle:
                    return talkLoopMaterial;
                case FastVsRetoWritingState.LookingUp:
                    return talkLoopMaterial;
                case FastVsRetoWritingState.Raising:
                    return raiseArmsMaterial;
                default:
                    return writingLoopMaterial;
            }
        }

        private int ResolveFrameCount(FastVsRetoWritingState state)
        {
            switch (state)
            {
                case FastVsRetoWritingState.Lowering:
                    return Mathf.Max(1, lowerArmsFrameCount);
                case FastVsRetoWritingState.DialogueIdle:
                    return Mathf.Max(1, talkLoopFrameCount);
                case FastVsRetoWritingState.LookingUp:
                    return Mathf.Max(1, talkLoopFrameCount);
                case FastVsRetoWritingState.Raising:
                    return Mathf.Max(1, raiseArmsFrameCount);
                default:
                    return Mathf.Max(1, writingLoopFrameCount);
            }
        }

        private void ResolveReferences()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<Renderer>();
            }
        }

        private int GetDialogueIdleFrameIndex()
        {
            return 0;
        }

        private int GetLookingUpFrameIndex()
        {
            return Mathf.Max(0, ResolveFrameCount(FastVsRetoWritingState.LookingUp) - 1);
        }

        private static bool MaterialLooksLikeFinalPack(Material material, string expected)
        {
            if (material == null || material.name.IndexOf(expected, System.StringComparison.OrdinalIgnoreCase) < 0)
            {
                return false;
            }

            var texture = ResolveTexture(material);
            return texture != null &&
                   texture.name.IndexOf("resident_b", System.StringComparison.OrdinalIgnoreCase) < 0 &&
                   texture.name.IndexOf("reto_", System.StringComparison.OrdinalIgnoreCase) >= 0 &&
                   texture.width % 64 == 0 &&
                   texture.height == 96;
        }

        private static Texture ResolveTexture(Material material)
        {
            if (material == null)
            {
                return null;
            }

            if (material.HasProperty("_BaseMap"))
            {
                var texture = material.GetTexture("_BaseMap");
                if (texture != null)
                {
                    return texture;
                }
            }

            return material.HasProperty("_MainTex") ? material.GetTexture("_MainTex") : null;
        }
    }
}
