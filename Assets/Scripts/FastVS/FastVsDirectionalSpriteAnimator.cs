using Anemora.TimeManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    public enum FastVsCharacterDirection
    {
        Front,
        Back,
        Left,
        Right
    }

    public sealed class FastVsDirectionalSpriteAnimator : MonoBehaviour
    {
        [SerializeField] private TimeWindowPairedSpacePortalController portalController;
        [SerializeField] private FastVsVisualDirectionGuide movementGuide;
        [SerializeField] private Transform player;
        [SerializeField] private Renderer spriteRenderer;
        [SerializeField] private Material currentFrontMaterial;
        [SerializeField] private Material currentBackMaterial;
        [SerializeField] private Material currentLeftMaterial;
        [SerializeField] private Material currentRightMaterial;
        [SerializeField] private Material currentWalkFrontMaterial;
        [SerializeField] private Material currentWalkBackMaterial;
        [SerializeField] private Material currentWalkLeftMaterial;
        [SerializeField] private Material currentWalkRightMaterial;
        [SerializeField] private Material otherFrontMaterial;
        [SerializeField] private Material otherBackMaterial;
        [SerializeField] private Material otherLeftMaterial;
        [SerializeField] private Material otherRightMaterial;
        [SerializeField] private Material otherWalkFrontMaterial;
        [SerializeField] private Material otherWalkBackMaterial;
        [SerializeField] private Material otherWalkLeftMaterial;
        [SerializeField] private Material otherWalkRightMaterial;
        [SerializeField] private float inputDeadZone = 0.12f;
        [SerializeField] private float localMoveDeadZone = 0.012f;
        [SerializeField] private int animationFrameCount = 4;
        [SerializeField] private int framePixelWidth = 64;
        [SerializeField] private float walkFramesPerSecond = 8f;
        [SerializeField] private float idleFramesPerSecond = 2f;

        private static readonly int CharacterBillboardShadowFixId = Shader.PropertyToID("_CharacterBillboardShadowFix");
        private static readonly int ReceiveWorldShadowsId = Shader.PropertyToID("_ReceiveWorldShadows");
        private static readonly int UseSpriteDirectionalNormalsId = Shader.PropertyToID("_UseSpriteDirectionalNormals");
        private static readonly int SpriteNormalStrengthId = Shader.PropertyToID("_SpriteNormalStrength");
        private static readonly int SpriteDirectionalLightStrengthId = Shader.PropertyToID("_SpriteDirectionalLightStrength");
        private static readonly int ZWriteId = Shader.PropertyToID("_ZWrite");
        private static readonly int ZTestId = Shader.PropertyToID("_ZTest");

        private MaterialPropertyBlock propertyBlock;
        private readonly Dictionary<Material, Material> niroRuntimeVisibilityMaterials = new Dictionary<Material, Material>();
        private bool hasPreviousLocal;
        private Vector3 previousLocal;
        private FastVsCharacterDirection currentDirection = FastVsCharacterDirection.Front;
        private bool lastOtherTime;
        private bool moving;
        private int currentFrame;
        private int activeFrameCount = 1;

        public FastVsCharacterDirection CurrentDirectionForReview => currentDirection;
        public int CurrentFrameForReview => currentFrame;
        public int AnimationFrameCountForReview => animationFrameCount;
        public int ActiveFrameCountForReview => activeFrameCount;
        public bool IsMovingForReview => moving;
        public Material ActiveMaterialForReview => spriteRenderer != null ? spriteRenderer.sharedMaterial : null;
        public bool HasAllDirectionMaterialsForReview =>
            currentFrontMaterial != null &&
            currentBackMaterial != null &&
            currentLeftMaterial != null &&
            currentRightMaterial != null &&
            currentWalkFrontMaterial != null &&
            currentWalkBackMaterial != null &&
            currentWalkLeftMaterial != null &&
            currentWalkRightMaterial != null &&
            otherFrontMaterial != null &&
            otherBackMaterial != null &&
            otherLeftMaterial != null &&
            otherRightMaterial != null &&
            otherWalkFrontMaterial != null &&
            otherWalkBackMaterial != null &&
            otherWalkLeftMaterial != null &&
            otherWalkRightMaterial != null;

        private void Awake()
        {
            ResolveReferences();
            ApplyMaterial();
        }

        private void LateUpdate()
        {
            ResolveReferences();
            if (spriteRenderer == null)
            {
                return;
            }

            EnsureNiroRuntimeVisibilityRecoveryApplied();
            var otherTime = portalController != null && portalController.PlayerInOtherTime;
            var direction = currentDirection;
            var movementFrozen = IsMovementFrozenForReview();
            if (movementFrozen)
            {
                SyncPreviousLocal();
            }

            var hasDirection = false;
            if (!movementFrozen)
            {
                hasDirection = TryResolveInputDirection(out direction) ||
                               TryResolveMotionDirection(out direction);
            }

            var wasMoving = moving;
            moving = hasDirection;
            if (hasDirection || otherTime != lastOtherTime || moving != wasMoving)
            {
                currentDirection = direction;
                lastOtherTime = otherTime;
                ApplyMaterial();
            }

            UpdateAnimationFrame();
            EnsureNiroRuntimeVisibilityRecoveryApplied();
        }

        public void SetDirectionForReview(FastVsCharacterDirection direction, bool otherTime)
        {
            SetPoseForReview(direction, otherTime, true);
        }

        public void SetPoseForReview(FastVsCharacterDirection direction, bool otherTime, bool isMoving)
        {
            currentDirection = direction;
            lastOtherTime = otherTime;
            moving = isMoving;
            ApplyMaterial();
        }

        private bool TryResolveInputDirection(out FastVsCharacterDirection direction)
        {
            var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude <= inputDeadZone * inputDeadZone)
            {
                direction = currentDirection;
                return false;
            }

            direction = DirectionFromVector(input);
            return true;
        }

        private bool TryResolveMotionDirection(out FastVsCharacterDirection direction)
        {
            var local = portalController != null ? portalController.GetPlayerLocalCoordinateForReview() : ResolveFallbackLocal();
            if (!hasPreviousLocal)
            {
                previousLocal = local;
                hasPreviousLocal = true;
                direction = currentDirection;
                return false;
            }

            var delta = local - previousLocal;
            previousLocal = local;
            delta.y = 0f;
            if (delta.sqrMagnitude <= localMoveDeadZone * localMoveDeadZone)
            {
                direction = currentDirection;
                return false;
            }

            direction = DirectionFromVector(delta);
            return true;
        }

        private Vector3 ResolveFallbackLocal()
        {
            return player != null ? player.position : transform.position;
        }

        private void ApplyMaterial()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            var material = ResolveMaterial(currentDirection, lastOtherTime, moving);
            if (material != null)
            {
                spriteRenderer.sharedMaterial = ResolveRuntimeMaterial(material);
            }

            activeFrameCount = ResolveFrameCount(material);
            currentFrame = Mathf.Clamp(currentFrame, 0, Mathf.Max(1, activeFrameCount) - 1);
            ApplyFrame();
        }

        private void UpdateAnimationFrame()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            activeFrameCount = ResolveFrameCount(spriteRenderer.sharedMaterial);
            if (activeFrameCount <= 1)
            {
                return;
            }

            var framesPerSecond = moving ? walkFramesPerSecond : idleFramesPerSecond;
            var frame = Mathf.FloorToInt(Time.time * Mathf.Max(1f, framesPerSecond)) % activeFrameCount;
            if (frame == currentFrame)
            {
                return;
            }

            currentFrame = frame;
            ApplyFrame();
        }

        private void ApplyFrame()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            activeFrameCount = ResolveFrameCount(spriteRenderer.sharedMaterial);
            var frames = Mathf.Max(1, activeFrameCount);
            var frameWidth = 1f / frames;
            var offsetX = Mathf.Clamp(currentFrame, 0, frames - 1) * frameWidth;
            propertyBlock ??= new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetVector("_BaseMap_ST", new Vector4(frameWidth, 1f, offsetX, 0f));
            propertyBlock.SetVector("_MainTex_ST", new Vector4(frameWidth, 1f, offsetX, 0f));
            if (UsesNiroRuntimeVisibilityRecovery())
            {
                propertyBlock.SetFloat(CharacterBillboardShadowFixId, 0f);
                propertyBlock.SetFloat(ReceiveWorldShadowsId, 1f);
                propertyBlock.SetFloat(UseSpriteDirectionalNormalsId, 0f);
                propertyBlock.SetFloat(SpriteNormalStrengthId, 0f);
                propertyBlock.SetFloat(SpriteDirectionalLightStrengthId, 0f);
            }

            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        private bool UsesNiroRuntimeVisibilityRecovery()
        {
            return spriteRenderer != null &&
                   string.Equals(spriteRenderer.gameObject.name, "Niro_Sprite64x96", System.StringComparison.Ordinal);
        }

        private void EnsureNiroRuntimeVisibilityRecoveryApplied()
        {
            if (!Application.isPlaying ||
                !UsesNiroRuntimeVisibilityRecovery() ||
                spriteRenderer.sharedMaterial == null ||
                spriteRenderer.sharedMaterial.name.EndsWith("_RuntimeVisible", System.StringComparison.Ordinal))
            {
                return;
            }

            ApplyMaterial();
        }

        private Material ResolveRuntimeMaterial(Material material)
        {
            if (!Application.isPlaying || !UsesNiroRuntimeVisibilityRecovery() || material == null)
            {
                return material;
            }

            if (!niroRuntimeVisibilityMaterials.TryGetValue(material, out var runtimeMaterial) || runtimeMaterial == null)
            {
                runtimeMaterial = new Material(material)
                {
                    name = material.name + "_RuntimeVisible"
                };
                ApplyNiroRuntimeVisibilityMaterialOverrides(runtimeMaterial);
                niroRuntimeVisibilityMaterials[material] = runtimeMaterial;
            }

            spriteRenderer.forceRenderingOff = false;
            spriteRenderer.shadowCastingMode = ShadowCastingMode.Off;
            spriteRenderer.receiveShadows = true;
            return runtimeMaterial;
        }

        private static void ApplyNiroRuntimeVisibilityMaterialOverrides(Material material)
        {
            material.enableInstancing = false;
            material.renderQueue = (int)RenderQueue.Transparent;
            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);
            material.SetShaderPassEnabled("ShadowCaster", false);
            if (material.HasProperty(ZWriteId))
            {
                material.SetFloat(ZWriteId, 0f);
            }

            if (material.HasProperty(ZTestId))
            {
                material.SetFloat(ZTestId, (float)CompareFunction.Always);
            }

            if (material.HasProperty(CharacterBillboardShadowFixId))
            {
                material.SetFloat(CharacterBillboardShadowFixId, 0f);
            }

            if (material.HasProperty(ReceiveWorldShadowsId))
            {
                material.SetFloat(ReceiveWorldShadowsId, 1f);
            }

            if (material.HasProperty(UseSpriteDirectionalNormalsId))
            {
                material.SetFloat(UseSpriteDirectionalNormalsId, 0f);
            }

            if (material.HasProperty(SpriteNormalStrengthId))
            {
                material.SetFloat(SpriteNormalStrengthId, 0f);
            }

            if (material.HasProperty(SpriteDirectionalLightStrengthId))
            {
                material.SetFloat(SpriteDirectionalLightStrengthId, 0f);
            }
        }

        private void OnDestroy()
        {
            foreach (var material in niroRuntimeVisibilityMaterials.Values)
            {
                if (material != null)
                {
                    Destroy(material);
                }
            }
        }

        private Material ResolveMaterial(FastVsCharacterDirection direction, bool otherTime, bool isMoving)
        {
            var walkMaterial = ResolveWalkMaterial(direction, otherTime);
            if (isMoving && walkMaterial != null)
            {
                return walkMaterial;
            }

            switch (direction)
            {
                case FastVsCharacterDirection.Back:
                    return otherTime ? otherBackMaterial : currentBackMaterial;
                case FastVsCharacterDirection.Left:
                    return otherTime ? otherLeftMaterial : currentLeftMaterial;
                case FastVsCharacterDirection.Right:
                    return otherTime ? otherRightMaterial : currentRightMaterial;
                default:
                    return otherTime ? otherFrontMaterial : currentFrontMaterial;
            }
        }

        private Material ResolveWalkMaterial(FastVsCharacterDirection direction, bool otherTime)
        {
            switch (direction)
            {
                case FastVsCharacterDirection.Back:
                    return otherTime ? otherWalkBackMaterial : currentWalkBackMaterial;
                case FastVsCharacterDirection.Left:
                    return otherTime ? otherWalkLeftMaterial : currentWalkLeftMaterial;
                case FastVsCharacterDirection.Right:
                    return otherTime ? otherWalkRightMaterial : currentWalkRightMaterial;
                default:
                    return otherTime ? otherWalkFrontMaterial : currentWalkFrontMaterial;
            }
        }

        private int ResolveFrameCount(Material material)
        {
            var texture = ResolveTexture(material);
            if (texture == null || framePixelWidth <= 0)
            {
                return Mathf.Max(1, animationFrameCount);
            }

            return Mathf.Max(1, Mathf.RoundToInt(texture.width / (float)framePixelWidth));
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

        private static FastVsCharacterDirection DirectionFromVector(Vector3 vector)
        {
            if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
            {
                return vector.x < 0f ? FastVsCharacterDirection.Left : FastVsCharacterDirection.Right;
            }

            return vector.z > 0f ? FastVsCharacterDirection.Back : FastVsCharacterDirection.Front;
        }

        private void ResolveReferences()
        {
            if (portalController == null)
            {
                portalController = FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            }

            if (movementGuide == null)
            {
                movementGuide = FindFirstObjectByType<FastVsVisualDirectionGuide>();
            }

            if (player == null)
            {
                var controller = FindFirstObjectByType<CharacterController>();
                if (controller != null)
                {
                    player = controller.transform;
                }
            }
        }

        private bool IsMovementFrozenForReview()
        {
            return movementGuide != null && movementGuide.MovementFrozenForReview;
        }

        private void SyncPreviousLocal()
        {
            previousLocal = portalController != null ? portalController.GetPlayerLocalCoordinateForReview() : ResolveFallbackLocal();
            hasPreviousLocal = true;
        }
    }
}
