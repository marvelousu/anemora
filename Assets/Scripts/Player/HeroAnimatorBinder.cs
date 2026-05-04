using UnityEngine;

namespace Anemora.Player
{
    public sealed class HeroAnimatorBinder : MonoBehaviour
    {
        private const string IsMovingParameter = "isMoving";
        private const string FacingParameter = "facing";

        [SerializeField] private PrototypePlayerController playerController;
        [SerializeField] private Transform observedTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] idleFrontSprites;
        [SerializeField] private Sprite[] idleSideSprites;
        [SerializeField] private Sprite[] idleBackSprites;
        [SerializeField] private Sprite[] walkFrontSprites;
        [SerializeField] private Sprite[] walkSideSprites;
        [SerializeField] private Sprite[] walkBackSprites;
        [SerializeField] private float movingEpsilon = 0.0025f;
        [SerializeField] private float framesPerSecond = 6f;
        [SerializeField] private bool lockWorldRotation = true;

        private Vector3 lastObservedPosition;
        private bool hasObservedPosition;
        private float frameClock;
        private int facing;
        private bool isMoving;

        public bool IsMoving => isMoving;
        public int Facing => facing;

        private void Awake()
        {
            ResolveReferences();
            ResetTracking();
        }

        private void LateUpdate()
        {
            Tick(Time.deltaTime);
        }

        public void TickForTests(float deltaTime)
        {
            ResolveReferences();
            Tick(deltaTime);
        }

        public void ResetTrackingForTests()
        {
            ResolveReferences();
            ResetTracking();
        }

        private void Tick(float deltaTime)
        {
            var target = ResolveObservedTransform();
            if (target == null)
            {
                return;
            }

            if (!hasObservedPosition)
            {
                lastObservedPosition = target.position;
                hasObservedPosition = true;
                ApplyAnimatorParameters(false);
                ApplySprite(false, Mathf.Max(0f, deltaTime));
                return;
            }

            var delta = target.position - lastObservedPosition;
            delta.y = 0f;
            lastObservedPosition = target.position;
            frameClock += Mathf.Max(0f, deltaTime);

            isMoving = delta.sqrMagnitude > movingEpsilon * movingEpsilon;
            if (isMoving)
            {
                UpdateFacing(delta);
            }

            ApplyAnimatorParameters(isMoving);
            ApplySprite(isMoving, deltaTime);
        }

        private void ResolveReferences()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (playerController == null)
            {
                playerController = GetComponentInParent<PrototypePlayerController>();
            }
        }

        private Transform ResolveObservedTransform()
        {
            if (observedTransform != null)
            {
                return observedTransform;
            }

            if (playerController != null)
            {
                observedTransform = playerController.transform;
            }
            else if (transform.parent != null)
            {
                observedTransform = transform.parent;
            }

            return observedTransform;
        }

        private void ResetTracking()
        {
            var target = ResolveObservedTransform();
            if (target == null)
            {
                hasObservedPosition = false;
                return;
            }

            lastObservedPosition = target.position;
            hasObservedPosition = true;
            ApplyAnimatorParameters(false);
            ApplySprite(false, 0f);
        }

        private void UpdateFacing(Vector3 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.z))
            {
                facing = 1;
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = delta.x < 0f;
                }

                return;
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = false;
            }

            facing = delta.z > 0f ? 2 : 0;
        }

        private void ApplyAnimatorParameters(bool moving)
        {
            if (animator == null)
            {
                return;
            }

            animator.SetBool(IsMovingParameter, moving);
            animator.SetInteger(FacingParameter, facing);
        }

        private void ApplySprite(bool moving, float deltaTime)
        {
            if (spriteRenderer == null)
            {
                return;
            }

            if (lockWorldRotation)
            {
                transform.rotation = Quaternion.identity;
            }

            var sprites = SelectSprites(moving);
            if (sprites == null || sprites.Length == 0)
            {
                return;
            }

            var frame = Mathf.FloorToInt(frameClock * Mathf.Max(1f, framesPerSecond)) % sprites.Length;
            spriteRenderer.sprite = sprites[frame];
        }

        private Sprite[] SelectSprites(bool moving)
        {
            if (moving)
            {
                return facing switch
                {
                    1 => NonEmptyOrFallback(walkSideSprites, idleSideSprites, idleFrontSprites),
                    2 => NonEmptyOrFallback(walkBackSprites, idleBackSprites, idleFrontSprites),
                    _ => NonEmptyOrFallback(walkFrontSprites, idleFrontSprites, null)
                };
            }

            return facing switch
            {
                1 => NonEmptyOrFallback(idleSideSprites, walkSideSprites, idleFrontSprites),
                2 => NonEmptyOrFallback(idleBackSprites, walkBackSprites, idleFrontSprites),
                _ => NonEmptyOrFallback(idleFrontSprites, walkFrontSprites, null)
            };
        }

        private static Sprite[] NonEmptyOrFallback(Sprite[] first, Sprite[] second, Sprite[] third)
        {
            if (first != null && first.Length > 0)
            {
                return first;
            }

            if (second != null && second.Length > 0)
            {
                return second;
            }

            return third;
        }
    }
}
