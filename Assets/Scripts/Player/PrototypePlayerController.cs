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

        private bool movementFrozen;

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

            var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }

            if (input.sqrMagnitude <= Mathf.Epsilon)
            {
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
    }
}
