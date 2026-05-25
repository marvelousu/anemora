using Anemora.TimeManagement;
using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsVisualDirectionGuide : MonoBehaviour
    {
        private const float CameraModeFov = 38f;
        private const float CentralPlazaVsCameraMaxAnchorZ = 14.70f;
        private const float ExteriorVsCameraMinAnchorX = 7.05f;
        private const float ExteriorVsCameraMinAnchorZ = 5.00f;
        [SerializeField] private TimeWindowPairedSpacePortalController portalController;
        [SerializeField] private CharacterController playerController;
        [SerializeField] private Transform player;
        [SerializeField] private Camera reviewCamera;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private float moveSpeed = 2.35f;
        [SerializeField] private float cameraFollowSharpness = 8.5f;
        [SerializeField] private bool showGuide = true;
        [SerializeField] private bool showDebugOverlay;
        [SerializeField] private bool movementFrozen;
        [SerializeField] private string title = "Anemora Fast VS visual direction sample";
        [SerializeField] private string currentStateLabel = "CURRENT / ruined library";
        [SerializeField] private string otherStateLabel = "PAST / restored library";
        [SerializeField] private string controlHint = "Left-drag: draw a Time Window. Walk through it to enter the paired past space; walk back to return.";
        [SerializeField] private bool isolateActiveTimeLayer = true;
        [SerializeField] private bool isolateActiveTimePhysics = true;

        private int cameraMode;
        private bool hasCameraState;
        private bool lastOtherTime;
        private FastVsHouseArea lastArea;
        private bool hasOriginalCullingMask;
        private int originalCullingMask;
        private bool hasTimeIsolationState;
        private bool lastIsolatedOtherTime;

        private void Awake()
        {
            ResolveReferences();
            ApplyActiveTimeIsolation();
        }

        private void Update()
        {
            ResolveReferences();
            ApplyActiveTimeIsolation();
            HandleMovement();
            HandleCameraInput();
            UpdateCamera();
        }

        private void OnGUI()
        {
            if (!showGuide || !showDebugOverlay || portalController == null)
            {
                return;
            }

            var local = portalController.GetPlayerLocalCoordinateForReview();
            var era = portalController.PlayerInOtherTime ? otherStateLabel : currentStateLabel;
            var text =
                title + "\n" +
                controlHint + "\n" +
                "WASD / Arrow: move. Right-click or Esc: close window only in current time. 1 follow / 2 window view / 3 overview / H hide UI.\n" +
                $"State: {era}   local=({local.x:0.00}, {local.y:0.00}, {local.z:0.00})\n" +
                $"Portal local=({portalController.PortalLocalCenterForReview.x:0.00}, {portalController.PortalLocalCenterForReview.y:0.00}, {portalController.PortalLocalCenterForReview.z:0.00}) " +
                $"size={portalController.PortalSizeForReview.x:0.00}x{portalController.PortalSizeForReview.y:0.00}\n" +
                $"Last: {portalController.LastTransitionForReview}";

            GUI.Box(new Rect(12f, 12f, 800f, 142f), text);
        }

        private void HandleMovement()
        {
            if (movementFrozen ||
                playerController == null ||
                FastVsAreaDoorTransition.AnyTransitionInProgressForReview)
            {
                return;
            }

            var input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }

            var motion = input * (moveSpeed * Time.deltaTime);
            motion += Physics.gravity * Time.deltaTime;
            playerController.Move(motion);
        }

        private void HandleCameraInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                cameraMode = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                cameraMode = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                cameraMode = 2;
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                showGuide = !showGuide;
            }
        }

        private void UpdateCamera()
        {
            if (reviewCamera == null)
            {
                return;
            }

            Vector3 targetPosition;
            Vector3 lookAt;
            var targetFieldOfView = reviewCamera.fieldOfView;

            if (cameraMode == 1 && portalController != null && ResolveActivePortalRoot() != null)
            {
                var portal = ResolveActivePortalRoot().transform;
                targetPosition = portal.TransformPoint(new Vector3(0f, 1.15f, -3.25f));
                lookAt = portal.TransformPoint(new Vector3(0f, 0.45f, 0.65f));
                targetFieldOfView = CameraModeFov;
            }
            else if (cameraMode == 2)
            {
                var root = ResolveActiveSpaceRoot();
                targetPosition = root != null ? root.TransformPoint(new Vector3(0f, 7.2f, -8.2f)) : new Vector3(0f, 7.2f, -8.2f);
                lookAt = root != null ? root.TransformPoint(new Vector3(0f, 0.15f, 0.15f)) : new Vector3(0f, 0.15f, 0.15f);
                targetFieldOfView = CameraModeFov;
            }
            else
            {
                var anchor = ResolveActiveSideCameraAnchor();
                var followProfile = GetFollowCameraProfile(areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.Interior);
                targetPosition = anchor + followProfile.PositionOffset;
                lookAt = anchor + followProfile.LookOffset;
                targetFieldOfView = followProfile.FieldOfView;
            }

            var targetRotation = Quaternion.LookRotation(lookAt - targetPosition, Vector3.up);
            if (ShouldSnapCamera())
            {
                reviewCamera.transform.SetPositionAndRotation(targetPosition, targetRotation);
                reviewCamera.fieldOfView = targetFieldOfView;
                CaptureCameraState();
                return;
            }

            reviewCamera.transform.position = Vector3.Lerp(reviewCamera.transform.position, targetPosition, Time.deltaTime * cameraFollowSharpness);
            reviewCamera.transform.rotation = Quaternion.Slerp(reviewCamera.transform.rotation, targetRotation, Time.deltaTime * cameraFollowSharpness);
            reviewCamera.fieldOfView = Mathf.Lerp(reviewCamera.fieldOfView, targetFieldOfView, Time.deltaTime * cameraFollowSharpness);
            CaptureCameraState();
        }

        public Vector3 ResolveActiveCameraAnchorForReview()
        {
            return ResolveActiveSideCameraAnchor();
        }

        public Transform ResolveActiveSpaceRootForReview()
        {
            return ResolveActiveSpaceRoot();
        }

        public bool MovementFrozenForReview => movementFrozen;

        public void SetMovementFrozen(bool frozen)
        {
            movementFrozen = frozen;
        }

        public void ApplyActiveTimeIsolationForReview()
        {
            ApplyActiveTimeIsolation();
        }

        private Vector3 ResolveActiveSideCameraAnchor()
        {
            var root = ResolveActiveSpaceRoot();
            if (root == null)
            {
                return player != null ? player.position : Vector3.zero;
            }

            var local = portalController.GetPlayerLocalCoordinateForReview();
            if (areaVisibility != null && areaVisibility.ActiveAreaForReview == FastVsHouseArea.CentralPlaza)
            {
                local.z = Mathf.Min(local.z, CentralPlazaVsCameraMaxAnchorZ);
            }
            else if (areaVisibility != null && areaVisibility.ActiveAreaForReview == FastVsHouseArea.Exterior)
            {
                local.x = Mathf.Max(local.x, ExteriorVsCameraMinAnchorX);
                local.z = Mathf.Max(local.z, ExteriorVsCameraMinAnchorZ);
            }

            return root.TransformPoint(local);
        }

        private Transform ResolveActiveSpaceRoot()
        {
            if (portalController == null)
            {
                return null;
            }

            return portalController.PlayerInOtherTime
                ? portalController.OtherTimeSpaceRootForReview
                : portalController.CurrentSpaceRootForReview;
        }

        private static FollowCameraProfile GetFollowCameraProfile(FastVsHouseArea area)
        {
            switch (area)
            {
                case FastVsHouseArea.Interior:
                    return new FollowCameraProfile(new Vector3(0f, 2.75f, -4.55f), new Vector3(0f, 0.72f, 0.45f), 38f);
                case FastVsHouseArea.Exterior:
                    return new FollowCameraProfile(new Vector3(0.70f, 2.85f, -5.25f), new Vector3(0.25f, 0.78f, 0.90f), 39f);
                case FastVsHouseArea.CentralPlaza:
                    return new FollowCameraProfile(new Vector3(0f, 3.55f, -6.50f), new Vector3(0f, 1.18f, 1.35f), 40f);
                case FastVsHouseArea.Library:
                    return new FollowCameraProfile(new Vector3(0.25f, 2.95f, -5.05f), new Vector3(0.10f, 0.84f, 0.74f), 39f);
                default:
                    return new FollowCameraProfile(new Vector3(0f, 2.75f, -4.55f), new Vector3(0f, 0.72f, 0.45f), 38f);
            }
        }

        public static (Vector3 PositionOffset, Vector3 LookOffset, float FieldOfView) GetFollowCameraProfileForReview(FastVsHouseArea area)
        {
            var profile = GetFollowCameraProfile(area);
            return (profile.PositionOffset, profile.LookOffset, profile.FieldOfView);
        }

        private GameObject ResolveActivePortalRoot()
        {
            if (portalController == null)
            {
                return null;
            }

            return portalController.PlayerInOtherTime
                ? portalController.OtherTimePortalRootForReview
                : portalController.CurrentPortalRootForReview;
        }

        private void ResolveReferences()
        {
            if (portalController == null)
            {
                portalController = FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            }

            if (playerController == null)
            {
                playerController = FindFirstObjectByType<CharacterController>();
            }

            if (player == null && playerController != null)
            {
                player = playerController.transform;
            }

            if (reviewCamera == null)
            {
                reviewCamera = Camera.main;
            }

            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }
        }

        private void ApplyActiveTimeIsolation()
        {
            if (portalController == null)
            {
                return;
            }

            portalController.ApplyReviewVisibilityLayersForReview();
            var otherTime = portalController.PlayerInOtherTime;
            ApplyCameraCulling(otherTime);
            ApplyPhysicsIsolation(otherTime);
        }

        private void ApplyCameraCulling(bool otherTime)
        {
            if (!isolateActiveTimeLayer || reviewCamera == null)
            {
                return;
            }

            if (!hasOriginalCullingMask)
            {
                originalCullingMask = reviewCamera.cullingMask;
                hasOriginalCullingMask = true;
            }

            var currentLayer = Mathf.Clamp(portalController.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherLayer = Mathf.Clamp(portalController.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var portalLayer = Mathf.Clamp(portalController.PortalFrameRenderLayerForReview, 0, 31);
            var playerLayer = Mathf.Clamp(portalController.PlayerVisibleRenderLayerForReview, 0, 31);
            var currentBit = 1 << currentLayer;
            var otherBit = 1 << otherLayer;
            var portalBit = 1 << portalLayer;
            var playerBit = 1 << playerLayer;
            var mask = originalCullingMask | currentBit | portalBit;
            mask = (mask & ~otherBit) | currentBit | portalBit;
            mask = otherTime ? mask & ~playerBit : mask | playerBit;
            reviewCamera.cullingMask = mask;
        }

        private void ApplyPhysicsIsolation(bool otherTime)
        {
            if (!isolateActiveTimePhysics || hasTimeIsolationState && otherTime == lastIsolatedOtherTime)
            {
                return;
            }

            SetRootCollidersEnabled(portalController.CurrentSpaceRootForReview, !otherTime);
            SetRootCollidersEnabled(portalController.OtherTimeSpaceRootForReview, otherTime);
            hasTimeIsolationState = true;
            lastIsolatedOtherTime = otherTime;
        }

        private static void SetRootCollidersEnabled(Transform root, bool enabled)
        {
            if (root == null)
            {
                return;
            }

            foreach (var collider in root.GetComponentsInChildren<Collider>(true))
            {
                collider.enabled = enabled;
            }
        }

        private bool ShouldSnapCamera()
        {
            if (portalController == null)
            {
                return !hasCameraState;
            }

            var area = areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.Interior;
            return !hasCameraState || area != lastArea;
        }

        private void CaptureCameraState()
        {
            hasCameraState = true;
            lastOtherTime = portalController != null && portalController.PlayerInOtherTime;
            lastArea = areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.Interior;
        }

        private readonly struct FollowCameraProfile
        {
            public readonly Vector3 PositionOffset;
            public readonly Vector3 LookOffset;
            public readonly float FieldOfView;

            public FollowCameraProfile(Vector3 positionOffset, Vector3 lookOffset, float fieldOfView)
            {
                PositionOffset = positionOffset;
                LookOffset = lookOffset;
                FieldOfView = fieldOfView;
            }
        }
    }
}
