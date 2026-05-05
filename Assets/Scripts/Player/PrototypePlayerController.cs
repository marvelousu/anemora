using Anemora.Audio;
using UnityEngine;

namespace Anemora.Player
{
    /// <summary>
    /// Minimal keyboard mover for the vertical-slice scene until the authored player controller exists.
    /// </summary>
    public sealed class PrototypePlayerController : MonoBehaviour
    {
        [SerializeField] private Transform cameraYawReference;
        [SerializeField] private float moveSpeed = 2.2f;
        [SerializeField] private bool lockYPosition = true;
        [SerializeField] private float lockedYPosition = 0.62f;
        [SerializeField] private Zone1FootstepSurface defaultFootstepSurface = Zone1FootstepSurface.Stone;
        [SerializeField] private float footstepInterval = 0.42f;

        private bool movementFrozen;
        private float footstepTimer;

        public float MoveSpeed => moveSpeed;
        public bool IsMovementFrozen => movementFrozen;

        public void SetMovementFrozen(bool frozen)
        {
            movementFrozen = frozen;
        }

        private void Awake()
        {
            if (cameraYawReference == null && Camera.main != null)
            {
                cameraYawReference = Camera.main.transform;
            }

            if (lockYPosition)
            {
                ApplyLockedYPosition();
            }
        }

        private void Update()
        {
            if (movementFrozen)
            {
                if (lockYPosition)
                {
                    ApplyLockedYPosition();
                }

                return;
            }

            var input = ReadMoveInput();
            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }

            if (input.sqrMagnitude <= Mathf.Epsilon)
            {
                footstepTimer = 0f;
                return;
            }

            var forward = Vector3.forward;
            var right = Vector3.right;
            if (cameraYawReference != null)
            {
                forward = Vector3.ProjectOnPlane(cameraYawReference.forward, Vector3.up).normalized;
                right = Vector3.ProjectOnPlane(cameraYawReference.right, Vector3.up).normalized;
            }

            var delta = (right * input.x + forward * input.z) * (moveSpeed * Time.deltaTime);
            transform.position += delta;

            if (delta.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
                TickFootsteps();
            }

            if (lockYPosition)
            {
                ApplyLockedYPosition();
            }
        }

        private void ApplyLockedYPosition()
        {
            var position = transform.position;
            position.y = lockedYPosition;
            transform.position = position;
        }

        private void TickFootsteps()
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer > 0f)
            {
                return;
            }

            Zone1AudioController.Instance?.PlayFootstep(defaultFootstepSurface);
            footstepTimer = Mathf.Max(0.05f, footstepInterval);
        }

        private static Vector3 ReadMoveInput()
        {
            var x = Input.GetAxisRaw("Horizontal");
            var z = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                x -= 1f;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                x += 1f;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                z -= 1f;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                z += 1f;
            }

            return new Vector3(Mathf.Clamp(x, -1f, 1f), 0f, Mathf.Clamp(z, -1f, 1f));
        }
    }
}
