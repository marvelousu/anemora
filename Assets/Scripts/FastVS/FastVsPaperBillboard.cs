using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsPaperBillboard : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private bool lockY = true;
        [SerializeField] private bool useCameraForward = true;

        public bool LockYForReview => lockY;
        public bool UseCameraForwardForReview => useCameraForward;

        private void LateUpdate()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            if (targetCamera == null)
            {
                return;
            }

            var direction = useCameraForward
                ? -targetCamera.transform.forward
                : targetCamera.transform.position - transform.position;
            if (lockY)
            {
                direction.y = 0f;
            }

            if (direction.sqrMagnitude < 0.0001f)
            {
                return;
            }

            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }
    }
}
