using UnityEngine;

namespace Anemora.TimeManagement
{
    /// <summary>
    /// Keeps Camera_Past aligned to the main camera for portal-side rendering.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public sealed class PastCameraSync : MonoBehaviour
    {
        private const int PastVisualLayer = 11;

        [SerializeField] private Camera sourceCamera;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private bool syncProjection = true;

        public Camera SourceCamera => sourceCamera;
        public Camera TargetCamera => targetCamera;

        private void Awake()
        {
            if (targetCamera == null)
            {
                targetCamera = GetComponent<Camera>();
            }

            if (sourceCamera == null)
            {
                sourceCamera = Camera.main;
            }

            if (targetCamera != null)
            {
                targetCamera.cullingMask = 1 << PastVisualLayer;
            }
        }

        private void LateUpdate()
        {
            if (sourceCamera == null || targetCamera == null)
            {
                return;
            }

            targetCamera.transform.SetPositionAndRotation(
                sourceCamera.transform.position,
                sourceCamera.transform.rotation);

            if (!syncProjection)
            {
                return;
            }

            targetCamera.fieldOfView = sourceCamera.fieldOfView;
            targetCamera.nearClipPlane = sourceCamera.nearClipPlane;
            targetCamera.farClipPlane = sourceCamera.farClipPlane;
            targetCamera.orthographic = sourceCamera.orthographic;
            targetCamera.orthographicSize = sourceCamera.orthographicSize;
        }
    }
}
