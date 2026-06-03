using System.Collections.Generic;
using UnityEngine;

namespace Anemora.TimeManagement
{
    /// <summary>
    /// Review-only Time Window v21 controller.
    /// Creates a paired portal in two real object spaces that share the same local coordinate map.
    /// This deliberately avoids stencil-only, render-surface-only, and generated-room approaches.
    /// </summary>
    public sealed class TimeWindowPairedSpacePortalController : MonoBehaviour
    {
        [Header("Spaces")]
        [SerializeField] private Transform currentSpaceRoot;
        [SerializeField] private Transform otherTimeSpaceRoot;
        [SerializeField] private Vector2 regionSize = new Vector2(5.6f, 5.2f);
        [SerializeField] private float portalLocalZ = -1.85f;
        [SerializeField] private bool placePortalFromGroundProjection = true;

        [Header("Player")]
        [SerializeField] private CharacterController playerController;
        [SerializeField] private Transform player;
        [SerializeField] private Material currentPlayerMaterial;
        [SerializeField] private Material otherTimePlayerMaterial;

        [Header("Camera and input")]
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private bool runtimeInputEnabled = true;
        [SerializeField] private float minDragPixels = 18f;
        [SerializeField] private Vector2 minPortalSize = new Vector2(1.15f, 1.1f);
        [SerializeField] private Vector2 maxPortalSize = new Vector2(3.8f, 2.55f);
        [SerializeField] private float groundClearance = 0.02f;
        [SerializeField] private bool anchorPortalBottomToGround;
        [SerializeField] private float crossingHalfDepth = 0.075f;
        [SerializeField] private float transferExitOffset = 0.18f;

        [Header("Review collision")]
        [SerializeField] private bool enableBackSideBlocking = true;
        [SerializeField] private bool enableGeneratedOtherTimeWallVolume = true;
        [SerializeField] private float wallVolumeDepth = 8.0f;
        [SerializeField] private float wallVolumeSideMargin = 0.22f;
        [SerializeField] private float wallVolumeThickness = 0.14f;
        [SerializeField] private float wallVolumeNearGapDepth = 0.42f;
        [SerializeField] private float farBackWallDepthMultiplier = 1.12f;
        [SerializeField] private float farBackWallDepthPadding = 0.28f;
        [SerializeField] private float farBackWallMinimumDepth = 1.15f;
        [SerializeField] private float currentBackSideBlockDepth = 0.38f;

        [Header("Review materials")]
        [SerializeField] private Material currentFrameMaterial;
        [SerializeField] private Material otherTimeFrameMaterial;
        [SerializeField] private Material previewFrameMaterial;
        [SerializeField] private Material thresholdMaterial;

        [Header("Portal aperture review")]
        [SerializeField] private bool enablePortalApertureView;
        [SerializeField] private int currentSpaceRenderLayer = 27;
        [SerializeField] private int otherTimeSpaceRenderLayer = 28;
        [SerializeField] private int portalFrameRenderLayer = 26;
        [SerializeField] private int playerVisibleRenderLayer = 0;
        [SerializeField] private int apertureTextureSize = 1024;
        [SerializeField] private float aperturePlaneOffset = 0.024f;
        [SerializeField] private float apertureObjectSuppressionDepth = 0.30f;
        [SerializeField] private Material portalApertureMaterial;
        [SerializeField] private Color portalCameraBackground = new Color(0.015f, 0.018f, 0.024f, 1f);

        private readonly List<Renderer> currentFrameRenderers = new List<Renderer>();
        private readonly List<Renderer> otherTimeFrameRenderers = new List<Renderer>();
        private readonly List<Collider> otherTimeWallVolumeColliders = new List<Collider>();
        private readonly List<Renderer> apertureSuppressedRenderers = new List<Renderer>();
        private GameObject currentPortalRoot;
        private GameObject otherTimePortalRoot;
        private MeshRenderer currentThresholdRenderer;
        private MeshRenderer otherTimeThresholdRenderer;
        private MeshRenderer currentApertureRenderer;
        private MeshRenderer otherTimeApertureRenderer;
        private Camera currentToOtherPortalCamera;
        private Camera otherToCurrentPortalCamera;
        private RenderTexture currentToOtherTexture;
        private RenderTexture otherToCurrentTexture;
        private Vector2 dragStart;
        private bool pointerDown;
        private bool dragging;
        private bool committed;
        private bool playerInOtherTime;
        private bool hasPreviousCurrentLocal;
        private bool hasPreviousOtherLocal;
        private Vector3 previousCurrentLocal;
        private Vector3 previousOtherLocal;
        private Vector3 portalLocalCenter;
        private Vector2 portalSize;
        private Vector2 relativeCoordinate;
        private Vector3 lastCurrentToOtherLocal;
        private Vector3 lastOtherToCurrentLocal;
        private Rect currentApertureViewportRect;
        private Rect otherTimeApertureViewportRect;
        private bool outsideCrossingRejected;
        private bool backSideCrossingRejected;
        private bool closeRejectedBecausePlayerInOtherTime;
        private Vector3 lastOtherTimeWallVolumeLocalCenter;
        private Vector3 lastOtherTimeWallVolumeLocalSize;
        private float lastFarBackWallLocalZ;
        private string lastTransition = "No portal generated.";

        public bool HasPortalPair => currentPortalRoot != null && otherTimePortalRoot != null && committed;
        public bool HasPreviewPortal => currentPortalRoot != null && otherTimePortalRoot != null && !committed;
        public bool PlayerInOtherTime => playerInOtherTime;
        public bool RuntimeInputEnabledForReview => runtimeInputEnabled;
        public bool OutsideCrossingRejected => outsideCrossingRejected;
        public bool BackSideCrossingRejected => backSideCrossingRejected;
        public bool CloseRejectedBecausePlayerInOtherTimeForReview => closeRejectedBecausePlayerInOtherTime;
        public bool UsesRenderSurfaceOnlyForReview => false;
        public bool SpawnsGeneratedInteriorVolumeForReview => false;
        public Vector2 RegionSizeForReview => regionSize;
        public Vector3 PortalLocalCenterForReview => portalLocalCenter;
        public Vector2 PortalSizeForReview => portalSize;
        public float PortalBottomLocalYForReview => portalLocalCenter.y - portalSize.y * 0.5f;
        public bool AnchorPortalBottomToGroundForReview => anchorPortalBottomToGround;
        public bool PlacePortalFromGroundProjectionForReview => placePortalFromGroundProjection;
        public Vector2 RelativeCoordinateForReview => relativeCoordinate;
        public Vector3 LastCurrentToOtherLocalForReview => lastCurrentToOtherLocal;
        public Vector3 LastOtherToCurrentLocalForReview => lastOtherToCurrentLocal;
        public string LastTransitionForReview => lastTransition;
        public Transform CurrentSpaceRootForReview => currentSpaceRoot;
        public Transform OtherTimeSpaceRootForReview => otherTimeSpaceRoot;
        public GameObject CurrentPortalRootForReview => currentPortalRoot;
        public GameObject OtherTimePortalRootForReview => otherTimePortalRoot;
        public int CurrentFrameRendererCountForReview => currentFrameRenderers.Count;
        public int OtherTimeFrameRendererCountForReview => otherTimeFrameRenderers.Count;
        public bool HasLiveApertureViewForReview => enablePortalApertureView &&
                                                     currentApertureRenderer != null &&
                                                     otherTimeApertureRenderer != null &&
                                                     currentToOtherTexture != null &&
                                                     otherToCurrentTexture != null;
        public int EnabledApertureRendererCountForReview => CountEnabledApertureRenderers();
        public bool CurrentApertureTextureAssignedForReview => HasRendererTexture(currentApertureRenderer, currentToOtherTexture);
        public bool OtherTimeApertureTextureAssignedForReview => HasRendererTexture(otherTimeApertureRenderer, otherToCurrentTexture);
        public Vector3 CurrentApertureLocalScaleForReview => currentApertureRenderer != null ? currentApertureRenderer.transform.localScale : Vector3.zero;
        public Vector3 OtherTimeApertureLocalScaleForReview => otherTimeApertureRenderer != null ? otherTimeApertureRenderer.transform.localScale : Vector3.zero;
        public Vector3 CurrentToOtherPortalCameraLocalForReview => currentToOtherPortalCamera != null && otherTimeSpaceRoot != null
            ? otherTimeSpaceRoot.InverseTransformPoint(currentToOtherPortalCamera.transform.position)
            : Vector3.zero;
        public Rect CurrentApertureViewportRectForReview => currentApertureViewportRect;
        public Rect OtherTimeApertureViewportRectForReview => otherTimeApertureViewportRect;
        public bool CurrentApertureUsesScreenRegionProjectionForReview => IsMeaningfulViewportCrop(currentApertureViewportRect);
        public int CurrentApertureMaterialRenderQueueForReview => currentApertureRenderer != null && currentApertureRenderer.sharedMaterial != null
            ? currentApertureRenderer.sharedMaterial.renderQueue
            : -1;
        public int PlayerRenderLayerForReview => player != null ? player.gameObject.layer : -1;
        public int CurrentSpaceRenderLayerForReview => currentSpaceRenderLayer;
        public int OtherTimeSpaceRenderLayerForReview => otherTimeSpaceRenderLayer;
        public int PortalFrameRenderLayerForReview => portalFrameRenderLayer;
        public int PlayerVisibleRenderLayerForReview => playerVisibleRenderLayer;
        public int CurrentToOtherPortalCameraCullingMaskForReview => currentToOtherPortalCamera != null ? currentToOtherPortalCamera.cullingMask : 0;
        public int OtherToCurrentPortalCameraCullingMaskForReview => otherToCurrentPortalCamera != null ? otherToCurrentPortalCamera.cullingMask : 0;
        public bool CurrentToOtherApertureIncludesPlayerForReview => MaskIncludesLayer(CurrentToOtherPortalCameraCullingMaskForReview, playerVisibleRenderLayer);
        public bool OtherToCurrentApertureIncludesPlayerForReview => MaskIncludesLayer(OtherToCurrentPortalCameraCullingMaskForReview, playerVisibleRenderLayer);
        public int ApertureSuppressedRendererCountForReview => apertureSuppressedRenderers.Count;
        public bool HasGeneratedOtherTimeWallVolumeForReview => enableGeneratedOtherTimeWallVolume && otherTimeWallVolumeColliders.Count >= 5;
        public int OtherTimeWallVolumeColliderCountForReview => otherTimeWallVolumeColliders.Count;
        public int EnabledOtherTimeWallVolumeColliderCountForReview => CountEnabledColliders(otherTimeWallVolumeColliders);
        public Vector3 OtherTimeWallVolumeLocalCenterForReview => lastOtherTimeWallVolumeLocalCenter;
        public Vector3 OtherTimeWallVolumeLocalSizeForReview => lastOtherTimeWallVolumeLocalSize;
        public float OtherTimeWallVolumeDepthForReview => wallVolumeDepth;
        public float OtherTimeWallVolumeSideMarginForReview => wallVolumeSideMargin;
        public float OtherTimeWallVolumeThicknessForReview => wallVolumeThickness;
        public float OtherTimeWallVolumeNearGapDepthForReview => wallVolumeNearGapDepth;
        public float OtherTimeWallVolumeFarBackWallLocalZForReview => lastFarBackWallLocalZ;
        public float OtherTimeWallVolumeFarBackWallSpaceLocalZForReview => portalLocalCenter.z + lastFarBackWallLocalZ;
        public float OtherTimeWallVolumeFarBackWallDepthMultiplierForReview => farBackWallDepthMultiplier;
        public float OtherTimeWallVolumeFarBackWallDepthPaddingForReview => farBackWallDepthPadding;
        public float OtherTimeWallVolumeFarBackWallMinimumDepthForReview => farBackWallMinimumDepth;
        public float CurrentBackSideBlockDepthForReview => currentBackSideBlockDepth;
        public string OtherTimeWallVolumeSummaryForReview =>
            HasGeneratedOtherTimeWallVolumeForReview
                ? $"segments=leftSide,rightSide,leftNearGap,rightNearGap,farBackWall, removedNearBackCap=True, colliders={otherTimeWallVolumeColliders.Count}, centerLocal=({lastOtherTimeWallVolumeLocalCenter.x:0.000},{lastOtherTimeWallVolumeLocalCenter.y:0.000},{lastOtherTimeWallVolumeLocalCenter.z:0.000}), size=({lastOtherTimeWallVolumeLocalSize.x:0.000},{lastOtherTimeWallVolumeLocalSize.y:0.000},{lastOtherTimeWallVolumeLocalSize.z:0.000}), margin={wallVolumeSideMargin:0.000}, thickness={wallVolumeThickness:0.000}, nearGapDepth={wallVolumeNearGapDepth:0.000}, farBackRootZ={lastFarBackWallLocalZ:0.000}, farBackSpaceZ={OtherTimeWallVolumeFarBackWallSpaceLocalZForReview:0.000}, farBackFormula=max(minDepth={farBackWallMinimumDepth:0.000}, portalHeight={portalSize.y:0.000}*multiplier={farBackWallDepthMultiplier:0.000}+padding={farBackWallDepthPadding:0.000}), inward=+localZ"
                : "disabled";
        public string PortalApertureCameraSyncSummaryForReview => BuildPortalApertureCameraSyncSummary();

        private void Awake()
        {
            ResolveReferences();
            InitializeReviewLayers();
        }

        private void Update()
        {
            ResolveReferences();
            HandleGenerationInput();
            EvaluateCrossing();
        }

        private void LateUpdate()
        {
            RenderPortalAperturesForReview();
        }

        private void OnDestroy()
        {
            RestoreApertureSuppressedRenderers();
            DestroyApertureRenderer(ref currentApertureRenderer);
            DestroyApertureRenderer(ref otherTimeApertureRenderer);
            ReleaseRenderTexture(ref currentToOtherTexture);
            ReleaseRenderTexture(ref otherToCurrentTexture);
            DestroyCamera(ref currentToOtherPortalCamera);
            DestroyCamera(ref otherToCurrentPortalCamera);
        }

        public void RenderPortalAperturesForReview()
        {
            if (!enablePortalApertureView || !committed || sceneCamera == null || currentSpaceRoot == null || otherTimeSpaceRoot == null)
            {
                return;
            }

            EnsurePortalCameras();
            ConfigureAndRenderPortalCamera(
                currentToOtherPortalCamera,
                currentToOtherTexture,
                currentSpaceRoot,
                otherTimeSpaceRoot,
                currentPortalRoot != null ? currentPortalRoot.transform : null,
                otherTimeSpaceRenderLayer,
                ref currentApertureViewportRect);
            ConfigureAndRenderPortalCamera(
                otherToCurrentPortalCamera,
                otherToCurrentTexture,
                otherTimeSpaceRoot,
                currentSpaceRoot,
                otherTimePortalRoot != null ? otherTimePortalRoot.transform : null,
                currentSpaceRenderLayer,
                ref otherTimeApertureViewportRect);
        }

        public void SetRuntimeInputEnabledForReview(bool enabled)
        {
            runtimeInputEnabled = enabled;
            if (!runtimeInputEnabled)
            {
                pointerDown = false;
                dragging = false;
            }
        }

        public bool TryPreviewPortalForTests(Vector2 startScreenPosition, Vector2 endScreenPosition)
        {
            return TryBuildPortalPair(startScreenPosition, endScreenPosition, false);
        }

        public bool TryOpenPortalForTests(Vector2 startScreenPosition, Vector2 endScreenPosition)
        {
            return TryBuildPortalPair(startScreenPosition, endScreenPosition, true);
        }

        public void ApplyReviewVisibilityLayersForReview()
        {
            ResolveReferences();
            InitializeReviewLayers();
        }

        public void ClosePortal()
        {
            if (playerInOtherTime)
            {
                closeRejectedBecausePlayerInOtherTime = true;
                lastTransition = "Portal close rejected: return to current time before closing the Time Window.";
                return;
            }

            RestoreApertureSuppressedRenderers();
            DestroyRoot(currentPortalRoot);
            DestroyRoot(otherTimePortalRoot);
            currentPortalRoot = null;
            otherTimePortalRoot = null;
            currentThresholdRenderer = null;
            otherTimeThresholdRenderer = null;
            currentApertureRenderer = null;
            otherTimeApertureRenderer = null;
            currentFrameRenderers.Clear();
            otherTimeFrameRenderers.Clear();
            otherTimeWallVolumeColliders.Clear();
            committed = false;
            outsideCrossingRejected = false;
            backSideCrossingRejected = false;
            closeRejectedBecausePlayerInOtherTime = false;
            lastTransition = "Portal closed.";
        }

        public bool TryClosePortalForAreaTransition()
        {
            ResolveReferences();
            pointerDown = false;
            dragging = false;

            if (!HasPortalPair && !HasPreviewPortal)
            {
                return true;
            }

            if (playerInOtherTime)
            {
                closeRejectedBecausePlayerInOtherTime = true;
                lastTransition = "Area transition rejected: return to current time before changing maps with an open Time Window.";
                return false;
            }

            ClosePortal();
            pointerDown = false;
            dragging = false;
            return !HasPortalPair && !HasPreviewPortal;
        }

        public void ForcePlayerCurrentLocalForReview(Vector3 currentLocal)
        {
            ResolveReferences();
            if (currentSpaceRoot == null || player == null)
            {
                return;
            }

            playerInOtherTime = false;
            SetPlayerWorldPosition(currentSpaceRoot.TransformPoint(currentLocal));
            ApplyPlayerMaterial(false);
            previousCurrentLocal = currentLocal;
            hasPreviousCurrentLocal = true;
            hasPreviousOtherLocal = false;
            outsideCrossingRejected = false;
            backSideCrossingRejected = false;
            closeRejectedBecausePlayerInOtherTime = false;
            lastTransition = $"Review: player placed in current local {Format(currentLocal)}.";
        }

        public void ForcePlayerOtherTimeLocalForReview(Vector3 otherLocal)
        {
            ResolveReferences();
            if (otherTimeSpaceRoot == null || player == null)
            {
                return;
            }

            playerInOtherTime = true;
            SetPlayerWorldPosition(otherTimeSpaceRoot.TransformPoint(otherLocal));
            ApplyPlayerMaterial(true);
            previousOtherLocal = otherLocal;
            hasPreviousOtherLocal = true;
            hasPreviousCurrentLocal = false;
            outsideCrossingRejected = false;
            backSideCrossingRejected = false;
            closeRejectedBecausePlayerInOtherTime = false;
            lastTransition = $"Review: player placed in other-time local {Format(otherLocal)}.";
        }

        public void TransferCurrentToOtherForReview(Vector3 currentLocal)
        {
            EnterOtherTimeAtLocal(currentLocal);
        }

        public void TransferOtherToCurrentForReview(Vector3 otherLocal)
        {
            ReturnCurrentAtLocal(otherLocal);
        }

        public void WarpPlayerToLocalForReview(Vector3 localPosition, string transitionLabel = null)
        {
            ResolveReferences();
            if (player == null)
            {
                return;
            }

            var root = playerInOtherTime ? otherTimeSpaceRoot : currentSpaceRoot;
            if (root == null)
            {
                return;
            }

            SetPlayerWorldPosition(root.TransformPoint(localPosition));
            ApplyPlayerMaterial(playerInOtherTime);

            if (playerInOtherTime)
            {
                previousOtherLocal = localPosition;
                hasPreviousOtherLocal = true;
                hasPreviousCurrentLocal = false;
            }
            else
            {
                previousCurrentLocal = localPosition;
                hasPreviousCurrentLocal = true;
                hasPreviousOtherLocal = false;
            }

            outsideCrossingRejected = false;
            backSideCrossingRejected = false;
            closeRejectedBecausePlayerInOtherTime = false;
            lastTransition = string.IsNullOrEmpty(transitionLabel)
                ? $"Warped to local coordinate {Format(localPosition)}."
                : $"{transitionLabel} {Format(localPosition)}.";
        }

        public void MovePlayerWorldForReview(Vector3 worldDelta)
        {
            ResolveReferences();
            if (playerController != null && playerController.enabled)
            {
                playerController.Move(worldDelta);
            }
            else if (player != null)
            {
                player.position += worldDelta;
            }

            EvaluateCrossing();
        }

        public Vector3 GetPlayerLocalCoordinateForReview()
        {
            if (player == null)
            {
                return Vector3.zero;
            }

            return playerInOtherTime && otherTimeSpaceRoot != null
                ? otherTimeSpaceRoot.InverseTransformPoint(player.position)
                : currentSpaceRoot != null
                    ? currentSpaceRoot.InverseTransformPoint(player.position)
                    : Vector3.zero;
        }

        private void HandleGenerationInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                if (!runtimeInputEnabled && !HasPortalPair)
                {
                    return;
                }

                ClosePortal();
                return;
            }

            if (!runtimeInputEnabled)
            {
                pointerDown = false;
                dragging = false;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                pointerDown = true;
                dragging = false;
                dragStart = Input.mousePosition;
                return;
            }

            if (!pointerDown)
            {
                return;
            }

            var current = (Vector2)Input.mousePosition;
            if (Input.GetMouseButton(0))
            {
                var threshold = Mathf.Max(1f, minDragPixels);
                if (!dragging && (current - dragStart).sqrMagnitude >= threshold * threshold)
                {
                    dragging = true;
                }

                if (dragging)
                {
                    TryBuildPortalPair(dragStart, current, false);
                }

                return;
            }

            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }

            pointerDown = false;
            if (dragging)
            {
                TryBuildPortalPair(dragStart, current, true);
            }

            dragging = false;
        }

        private bool TryBuildPortalPair(Vector2 startScreenPosition, Vector2 endScreenPosition, bool commit)
        {
            ResolveReferences();
            if (sceneCamera == null || currentSpaceRoot == null || otherTimeSpaceRoot == null)
            {
                return false;
            }

            if ((endScreenPosition - startScreenPosition).sqrMagnitude < minDragPixels * minDragPixels)
            {
                return false;
            }

            if (!TryResolvePortalBounds(startScreenPosition, endScreenPosition, out var centerLocal, out var size))
            {
                return false;
            }

            CreateOrUpdatePortalPair(centerLocal, size, commit);
            committed = commit;
            lastTransition = commit
                ? $"Portal committed at local {Format(centerLocal)}, relative {relativeCoordinate:F2}."
                : $"Portal preview at local {Format(centerLocal)}, relative {relativeCoordinate:F2}.";
            return true;
        }

        private bool TryResolvePortalBounds(Vector2 startScreenPosition, Vector2 endScreenPosition, out Vector3 centerLocal, out Vector2 size)
        {
            centerLocal = default;
            size = default;

            var minScreen = Vector2.Min(startScreenPosition, endScreenPosition);
            var maxScreen = Vector2.Max(startScreenPosition, endScreenPosition);
            var screenCorners = new[]
            {
                new Vector2(minScreen.x, minScreen.y),
                new Vector2(maxScreen.x, minScreen.y),
                new Vector2(maxScreen.x, maxScreen.y),
                new Vector2(minScreen.x, maxScreen.y)
            };

            var resolvedPortalLocalZ = portalLocalZ;
            if (placePortalFromGroundProjection)
            {
                var bottomCenterScreen = new Vector2((minScreen.x + maxScreen.x) * 0.5f, minScreen.y);
                if (!TryResolveGroundPlacementLocal(bottomCenterScreen, out var groundLocal))
                {
                    return false;
                }

                var halfRegionDepth = Mathf.Max(0.5f, regionSize.y * 0.5f);
                resolvedPortalLocalZ = Mathf.Clamp(groundLocal.z, -halfRegionDepth, halfRegionDepth);
            }

            var planePoint = currentSpaceRoot.TransformPoint(new Vector3(0f, 0f, resolvedPortalLocalZ));
            var plane = new Plane(currentSpaceRoot.forward, planePoint);
            var minX = float.PositiveInfinity;
            var maxX = float.NegativeInfinity;
            var minY = float.PositiveInfinity;
            var maxY = float.NegativeInfinity;

            for (var index = 0; index < screenCorners.Length; index++)
            {
                var ray = sceneCamera.ScreenPointToRay(screenCorners[index]);
                if (!plane.Raycast(ray, out var distance) || distance <= 0f)
                {
                    return false;
                }

                var local = currentSpaceRoot.InverseTransformPoint(ray.GetPoint(distance));
                minX = Mathf.Min(minX, local.x);
                maxX = Mathf.Max(maxX, local.x);
                minY = Mathf.Min(minY, local.y);
                maxY = Mathf.Max(maxY, local.y);
            }

            var width = Mathf.Clamp(maxX - minX, minPortalSize.x, maxPortalSize.x);
            var height = Mathf.Clamp(maxY - minY, minPortalSize.y, maxPortalSize.y);
            var halfRegionWidth = Mathf.Max(0.5f, regionSize.x * 0.5f);
            var clampedX = Mathf.Clamp((minX + maxX) * 0.5f, -halfRegionWidth + width * 0.5f, halfRegionWidth - width * 0.5f);
            var bottom = anchorPortalBottomToGround ? groundClearance : Mathf.Max(groundClearance, minY);

            centerLocal = new Vector3(clampedX, bottom + height * 0.5f, resolvedPortalLocalZ);
            size = new Vector2(width, height);
            return true;
        }

        private bool TryResolveGroundPlacementLocal(Vector2 screenPosition, out Vector3 local)
        {
            local = default;
            var groundPoint = currentSpaceRoot.TransformPoint(new Vector3(0f, groundClearance, 0f));
            var groundPlane = new Plane(currentSpaceRoot.up, groundPoint);
            var ray = sceneCamera.ScreenPointToRay(screenPosition);
            if (!groundPlane.Raycast(ray, out var distance) || distance <= 0f)
            {
                return false;
            }

            local = currentSpaceRoot.InverseTransformPoint(ray.GetPoint(distance));
            return true;
        }

        private void CreateOrUpdatePortalPair(Vector3 centerLocal, Vector2 size, bool commit)
        {
            RestoreApertureSuppressedRenderers();
            portalLocalCenter = centerLocal;
            portalSize = size;
            relativeCoordinate = new Vector2(
                Mathf.InverseLerp(-regionSize.x * 0.5f, regionSize.x * 0.5f, centerLocal.x),
                Mathf.InverseLerp(-regionSize.y * 0.5f, regionSize.y * 0.5f, centerLocal.z));
            backSideCrossingRejected = false;
            closeRejectedBecausePlayerInOtherTime = false;

            if (enablePortalApertureView)
            {
                ApplyLayerRecursive(currentSpaceRoot, currentSpaceRenderLayer);
                ApplyLayerRecursive(otherTimeSpaceRoot, otherTimeSpaceRenderLayer);
                ApplyLayerRecursive(player, playerVisibleRenderLayer);
            }

            EnsurePortalRoots();
            currentPortalRoot.transform.SetParent(currentSpaceRoot, false);
            currentPortalRoot.transform.localPosition = centerLocal;
            currentPortalRoot.transform.localRotation = Quaternion.identity;
            currentPortalRoot.layer = Mathf.Clamp(portalFrameRenderLayer, 0, 31);

            otherTimePortalRoot.transform.SetParent(otherTimeSpaceRoot, false);
            otherTimePortalRoot.transform.localPosition = centerLocal;
            otherTimePortalRoot.transform.localRotation = Quaternion.identity;
            otherTimePortalRoot.layer = Mathf.Clamp(portalFrameRenderLayer, 0, 31);

            if (enablePortalApertureView && commit)
            {
                EnsurePortalCameras();
            }

            RebuildPortalVisual(
                currentPortalRoot.transform,
                currentFrameRenderers,
                ref currentThresholdRenderer,
                ref currentApertureRenderer,
                size,
                commit ? currentFrameMaterial : previewFrameMaterial,
                enablePortalApertureView && commit ? currentToOtherTexture : null,
                "Current");
            RebuildPortalVisual(
                otherTimePortalRoot.transform,
                otherTimeFrameRenderers,
                ref otherTimeThresholdRenderer,
                ref otherTimeApertureRenderer,
                size,
                commit ? otherTimeFrameMaterial : previewFrameMaterial,
                enablePortalApertureView && commit ? otherToCurrentTexture : null,
                "OtherTime");
            ApplyPortalFrameLayer();
            HideOtherTimePortalVisualInCurrentView();
            SuppressApertureIntersectingRenderers(commit);
            RebuildOtherTimeWallVolume(commit);
            SyncOtherTimeWallVolumeColliderState();

            RenderPortalAperturesForReview();
        }

        private void EnsurePortalRoots()
        {
            if (currentPortalRoot == null)
            {
                currentPortalRoot = new GameObject("TW_V21_CurrentPortal_GeneratedThreshold");
            }

            if (otherTimePortalRoot == null)
            {
                otherTimePortalRoot = new GameObject("TW_V21_OtherTimePortal_MatchingCoordinate");
            }
        }

        private void RebuildPortalVisual(
            Transform root,
            List<Renderer> frameRenderers,
            ref MeshRenderer thresholdRenderer,
            ref MeshRenderer apertureRenderer,
            Vector2 size,
            Material frameMaterial,
            RenderTexture apertureTexture,
            string prefix)
        {
            ClearRenderers(frameRenderers);
            if (thresholdRenderer != null)
            {
                DestroyRoot(thresholdRenderer.gameObject);
                thresholdRenderer = null;
            }

            DestroyApertureRenderer(ref apertureRenderer);

            var halfWidth = size.x * 0.5f;
            var halfHeight = size.y * 0.5f;
            const float thickness = 0.055f;
            const float depth = 0.035f;

            CreateFramePart($"{prefix}_Frame_Left", root, frameRenderers, new Vector3(-halfWidth - thickness * 0.5f, 0f, 0f), new Vector3(thickness, size.y, depth), frameMaterial);
            CreateFramePart($"{prefix}_Frame_Right", root, frameRenderers, new Vector3(halfWidth + thickness * 0.5f, 0f, 0f), new Vector3(thickness, size.y, depth), frameMaterial);
            CreateFramePart($"{prefix}_Frame_Top", root, frameRenderers, new Vector3(0f, halfHeight + thickness * 0.5f, 0f), new Vector3(size.x + thickness * 2f, thickness, depth), frameMaterial);
            CreateFramePart($"{prefix}_Frame_Bottom", root, frameRenderers, new Vector3(0f, -halfHeight - thickness * 0.5f, 0f), new Vector3(size.x + thickness * 2f, thickness, depth), frameMaterial);

            var slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slab.name = $"TW_V21_{prefix}_PortalThresholdLine_NotPicture";
            slab.transform.SetParent(root, false);
            slab.transform.localPosition = new Vector3(0f, -halfHeight, -0.01f);
            slab.transform.localScale = new Vector3(size.x, 0.035f, 0.04f);
            var collider = slab.GetComponent<Collider>();
            if (collider != null)
            {
                DestroySafe(collider);
            }

            thresholdRenderer = slab.GetComponent<MeshRenderer>();
            if (thresholdMaterial != null)
            {
                thresholdRenderer.sharedMaterial = thresholdMaterial;
            }

            if (apertureTexture != null)
            {
                var aperture = GameObject.CreatePrimitive(PrimitiveType.Quad);
                aperture.name = $"TW_V25_{prefix}_LivePortalAperture_ClippedToFrame";
                aperture.transform.SetParent(root, false);
                aperture.transform.localPosition = new Vector3(0f, 0f, aperturePlaneOffset);
                aperture.transform.localRotation = Quaternion.identity;
                aperture.transform.localScale = new Vector3(size.x, size.y, 1f);
                aperture.layer = root.gameObject.layer;
                var apertureCollider = aperture.GetComponent<Collider>();
                if (apertureCollider != null)
                {
                    DestroySafe(apertureCollider);
                }

                apertureRenderer = aperture.GetComponent<MeshRenderer>();
                apertureRenderer.sharedMaterial = CreateApertureMaterial(portalApertureMaterial, apertureTexture, $"{prefix}_LivePortalApertureMaterial");
            }
        }

        private void HideOtherTimePortalVisualInCurrentView()
        {
            for (var index = 0; index < otherTimeFrameRenderers.Count; index++)
            {
                if (otherTimeFrameRenderers[index] != null)
                {
                    otherTimeFrameRenderers[index].enabled = false;
                }
            }

            if (otherTimeThresholdRenderer != null)
            {
                otherTimeThresholdRenderer.enabled = false;
            }

            if (otherTimeApertureRenderer != null)
            {
                otherTimeApertureRenderer.enabled = false;
            }
        }

        private void SuppressApertureIntersectingRenderers(bool commit)
        {
            if (!commit)
            {
                return;
            }

            SuppressApertureIntersectingRenderers(currentSpaceRoot);
            SuppressApertureIntersectingRenderers(otherTimeSpaceRoot);
        }

        private void SuppressApertureIntersectingRenderers(Transform root)
        {
            if (root == null)
            {
                return;
            }

            var renderers = root.GetComponentsInChildren<Renderer>(true);
            for (var index = 0; index < renderers.Length; index++)
            {
                var renderer = renderers[index];
                if (ShouldSuppressRendererForAperture(root, renderer))
                {
                    renderer.enabled = false;
                    apertureSuppressedRenderers.Add(renderer);
                }
            }
        }

        private bool ShouldSuppressRendererForAperture(Transform root, Renderer renderer)
        {
            if (renderer == null ||
                !renderer.enabled ||
                IsChildOf(renderer.transform, currentPortalRoot != null ? currentPortalRoot.transform : null) ||
                IsChildOf(renderer.transform, otherTimePortalRoot != null ? otherTimePortalRoot.transform : null) ||
                IsChildOf(renderer.transform, player))
            {
                return false;
            }

            var landmark = renderer.GetComponentInParent<TimeWindowPairedSpaceLandmark>();
            if (landmark != null &&
                (landmark.Kind == TimeWindowPairedSpaceLandmarkKind.PathOrFloor ||
                 landmark.Kind == TimeWindowPairedSpaceLandmarkKind.WallOrLandmark))
            {
                return false;
            }

            return RendererBoundsIntersectPortal(root, renderer.bounds, Mathf.Max(0.04f, apertureObjectSuppressionDepth));
        }

        private bool RendererBoundsIntersectPortal(Transform root, Bounds worldBounds, float depth)
        {
            var min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            var max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            var worldMin = worldBounds.min;
            var worldMax = worldBounds.max;
            for (var x = 0; x < 2; x++)
            {
                for (var y = 0; y < 2; y++)
                {
                    for (var z = 0; z < 2; z++)
                    {
                        var world = new Vector3(
                            x == 0 ? worldMin.x : worldMax.x,
                            y == 0 ? worldMin.y : worldMax.y,
                            z == 0 ? worldMin.z : worldMax.z);
                        var local = root.InverseTransformPoint(world);
                        min = Vector3.Min(min, local);
                        max = Vector3.Max(max, local);
                    }
                }
            }

            var halfWidth = portalSize.x * 0.5f;
            var halfHeight = portalSize.y * 0.5f;
            var overlapsX = max.x >= portalLocalCenter.x - halfWidth && min.x <= portalLocalCenter.x + halfWidth;
            var overlapsY = max.y >= portalLocalCenter.y - halfHeight && min.y <= portalLocalCenter.y + halfHeight;
            var overlapsZ = max.z >= portalLocalCenter.z - depth && min.z <= portalLocalCenter.z + depth;
            return overlapsX && overlapsY && overlapsZ;
        }

        private void RestoreApertureSuppressedRenderers()
        {
            for (var index = apertureSuppressedRenderers.Count - 1; index >= 0; index--)
            {
                var renderer = apertureSuppressedRenderers[index];
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }

            apertureSuppressedRenderers.Clear();
        }

        private static bool IsChildOf(Transform target, Transform parent)
        {
            return target != null && parent != null && target.IsChildOf(parent);
        }

        private void RebuildOtherTimeWallVolume(bool commit)
        {
            ClearOtherTimeWallVolume();
            lastOtherTimeWallVolumeLocalCenter = Vector3.zero;
            lastOtherTimeWallVolumeLocalSize = Vector3.zero;
            lastFarBackWallLocalZ = 0f;
            if (!commit || !enableGeneratedOtherTimeWallVolume || otherTimePortalRoot == null)
            {
                return;
            }

            var safeDepth = Mathf.Max(0.25f, wallVolumeDepth);
            var safeMargin = Mathf.Max(0f, wallVolumeSideMargin);
            var safeThickness = Mathf.Max(0.04f, wallVolumeThickness);
            var safeNearGapDepth = Mathf.Clamp(wallVolumeNearGapDepth, safeThickness, safeDepth);
            var farBackZ = Mathf.Clamp(
                CalculateFarBackWallLocalZ(portalSize.y, safeThickness),
                safeNearGapDepth + safeThickness,
                safeDepth - safeThickness * 0.5f);
            var volumeWidth = portalSize.x + safeMargin * 2f;
            var volumeHeight = portalSize.y + safeMargin * 2f;
            var halfWidth = portalSize.x * 0.5f;
            var centerZ = safeDepth * 0.5f;

            CreateInvisibleWallCollider(
                "TW_V24_OtherTime_GeneratedWallVolume_Left",
                otherTimePortalRoot.transform,
                new Vector3(-halfWidth - safeMargin - safeThickness * 0.5f, 0f, centerZ),
                new Vector3(safeThickness, volumeHeight, safeDepth));
            CreateInvisibleWallCollider(
                "TW_V24_OtherTime_GeneratedWallVolume_Right",
                otherTimePortalRoot.transform,
                new Vector3(halfWidth + safeMargin + safeThickness * 0.5f, 0f, centerZ),
                new Vector3(safeThickness, volumeHeight, safeDepth));
            CreateInvisibleWallCollider(
                "TW_V24_OtherTime_GeneratedWallVolume_LeftNearGapBlocker",
                otherTimePortalRoot.transform,
                new Vector3(-halfWidth - safeMargin * 0.5f, 0f, safeNearGapDepth * 0.5f),
                new Vector3(safeMargin, volumeHeight, safeNearGapDepth));
            CreateInvisibleWallCollider(
                "TW_V24_OtherTime_GeneratedWallVolume_RightNearGapBlocker",
                otherTimePortalRoot.transform,
                new Vector3(halfWidth + safeMargin * 0.5f, 0f, safeNearGapDepth * 0.5f),
                new Vector3(safeMargin, volumeHeight, safeNearGapDepth));
            CreateInvisibleWallCollider(
                "TW_V24_OtherTime_GeneratedWallVolume_FarBackWall",
                otherTimePortalRoot.transform,
                new Vector3(0f, 0f, farBackZ),
                new Vector3(volumeWidth + safeThickness * 2f, volumeHeight, safeThickness));

            lastOtherTimeWallVolumeLocalCenter = new Vector3(0f, 0f, centerZ);
            lastOtherTimeWallVolumeLocalSize = new Vector3(volumeWidth, volumeHeight, safeDepth);
            lastFarBackWallLocalZ = farBackZ;
        }

        private float CalculateFarBackWallLocalZ(float portalHeight, float wallThickness)
        {
            var visibleDepthEstimate = Mathf.Max(
                farBackWallMinimumDepth,
                portalHeight * Mathf.Max(0.1f, farBackWallDepthMultiplier) + Mathf.Max(0f, farBackWallDepthPadding));
            return visibleDepthEstimate + Mathf.Max(0.04f, wallThickness) * 0.5f;
        }

        private void CreateInvisibleWallCollider(string objectName, Transform root, Vector3 localPosition, Vector3 localScale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = objectName;
            wall.transform.SetParent(root, false);
            wall.transform.localPosition = localPosition;
            wall.transform.localScale = localScale;
            wall.layer = 0;

            var renderer = wall.GetComponent<Renderer>();
            if (renderer != null)
            {
                DestroySafe(renderer);
            }

            var collider = wall.GetComponent<BoxCollider>();
            if (collider != null)
            {
                otherTimeWallVolumeColliders.Add(collider);
                collider.enabled = playerInOtherTime;
            }
        }

        private void SyncOtherTimeWallVolumeColliderState()
        {
            for (var index = otherTimeWallVolumeColliders.Count - 1; index >= 0; index--)
            {
                var collider = otherTimeWallVolumeColliders[index];
                if (collider == null)
                {
                    otherTimeWallVolumeColliders.RemoveAt(index);
                    continue;
                }

                collider.enabled = playerInOtherTime;
            }
        }

        private void ClearOtherTimeWallVolume()
        {
            for (var index = otherTimeWallVolumeColliders.Count - 1; index >= 0; index--)
            {
                var collider = otherTimeWallVolumeColliders[index];
                if (collider != null)
                {
                    DestroyRoot(collider.gameObject);
                }
            }

            otherTimeWallVolumeColliders.Clear();
        }

        private void CreateFramePart(string objectName, Transform root, List<Renderer> frameRenderers, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.name = $"TW_V21_{objectName}";
            part.transform.SetParent(root, false);
            part.transform.localPosition = localPosition;
            part.transform.localScale = localScale;
            var collider = part.GetComponent<Collider>();
            if (collider != null)
            {
                DestroySafe(collider);
            }

            var renderer = part.GetComponent<Renderer>();
            if (material != null)
            {
                renderer.sharedMaterial = material;
            }

            frameRenderers.Add(renderer);
        }

        private void EnsurePortalCameras()
        {
            var size = Mathf.Clamp(apertureTextureSize, 128, 2048);
            currentToOtherTexture = EnsureRenderTexture(currentToOtherTexture, size, "TW_V25_CurrentToOther_LiveApertureTexture");
            otherToCurrentTexture = EnsureRenderTexture(otherToCurrentTexture, size, "TW_V25_OtherToCurrent_LiveApertureTexture");
            currentToOtherPortalCamera = EnsurePortalCamera(currentToOtherPortalCamera, "TW_V25_CurrentToOther_LivePortalCamera");
            otherToCurrentPortalCamera = EnsurePortalCamera(otherToCurrentPortalCamera, "TW_V25_OtherToCurrent_LivePortalCamera");
        }

        private void ConfigureAndRenderPortalCamera(
            Camera portalCamera,
            RenderTexture target,
            Transform sourceRoot,
            Transform targetRoot,
            Transform sourcePortalRoot,
            int targetLayer,
            ref Rect sourceViewportRect)
        {
            if (portalCamera == null || target == null || sceneCamera == null || sourceRoot == null || targetRoot == null)
            {
                return;
            }

            var sceneLocalPosition = sourceRoot.InverseTransformPoint(sceneCamera.transform.position);
            var sceneLocalRotation = Quaternion.Inverse(sourceRoot.rotation) * sceneCamera.transform.rotation;
            var portalRelativePosition = sceneLocalPosition - portalLocalCenter;
            var mappedLocalPosition = portalLocalCenter + portalRelativePosition;
            var mappedWorldPosition = targetRoot.TransformPoint(mappedLocalPosition);
            var mappedWorldRotation = targetRoot.rotation * sceneLocalRotation;
            portalCamera.transform.SetPositionAndRotation(mappedWorldPosition, mappedWorldRotation);
            portalCamera.orthographic = sceneCamera.orthographic;
            portalCamera.orthographicSize = sceneCamera.orthographicSize;
            portalCamera.fieldOfView = sceneCamera.fieldOfView;
            portalCamera.aspect = sceneCamera.aspect;
            portalCamera.nearClipPlane = sceneCamera.nearClipPlane;
            portalCamera.farClipPlane = sceneCamera.farClipPlane;
            portalCamera.clearFlags = CameraClearFlags.SolidColor;
            portalCamera.backgroundColor = portalCameraBackground;
            var targetMask = 1 << Mathf.Clamp(targetLayer, 0, 31);
            if (ShouldRenderPlayerInPortalTarget(targetRoot))
            {
                targetMask |= 1 << Mathf.Clamp(playerVisibleRenderLayer, 0, 31);
            }

            portalCamera.cullingMask = targetMask;
            portalCamera.targetTexture = target;
            portalCamera.ResetProjectionMatrix();
            if (TryCalculatePortalViewportRect(sourcePortalRoot, out var viewportRect))
            {
                sourceViewportRect = viewportRect;
                portalCamera.projectionMatrix = BuildCroppedProjection(sceneCamera.projectionMatrix, viewportRect);
            }
            else
            {
                sourceViewportRect = new Rect(0f, 0f, 1f, 1f);
            }

            portalCamera.Render();
            portalCamera.ResetProjectionMatrix();
        }

        private bool ShouldRenderPlayerInPortalTarget(Transform targetRoot)
        {
            if (player == null || targetRoot == null)
            {
                return false;
            }

            return playerInOtherTime
                ? targetRoot == otherTimeSpaceRoot
                : targetRoot == currentSpaceRoot;
        }

        private bool TryCalculatePortalViewportRect(Transform portalRoot, out Rect viewportRect)
        {
            viewportRect = new Rect(0f, 0f, 1f, 1f);
            if (portalRoot == null || sceneCamera == null || portalSize.x <= 0f || portalSize.y <= 0f)
            {
                return false;
            }

            var halfWidth = portalSize.x * 0.5f;
            var halfHeight = portalSize.y * 0.5f;
            var corners = new[]
            {
                new Vector3(-halfWidth, -halfHeight, aperturePlaneOffset),
                new Vector3(halfWidth, -halfHeight, aperturePlaneOffset),
                new Vector3(halfWidth, halfHeight, aperturePlaneOffset),
                new Vector3(-halfWidth, halfHeight, aperturePlaneOffset)
            };

            var min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            var max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            for (var index = 0; index < corners.Length; index++)
            {
                var viewport = sceneCamera.WorldToViewportPoint(portalRoot.TransformPoint(corners[index]));
                if (viewport.z <= sceneCamera.nearClipPlane)
                {
                    return false;
                }

                min = Vector2.Min(min, new Vector2(viewport.x, viewport.y));
                max = Vector2.Max(max, new Vector2(viewport.x, viewport.y));
            }

            min.x = Mathf.Clamp01(min.x);
            min.y = Mathf.Clamp01(min.y);
            max.x = Mathf.Clamp01(max.x);
            max.y = Mathf.Clamp01(max.y);
            var width = max.x - min.x;
            var height = max.y - min.y;
            if (width < 0.05f || height < 0.05f)
            {
                return false;
            }

            viewportRect = new Rect(min.x, min.y, width, height);
            return true;
        }

        private static Matrix4x4 BuildCroppedProjection(Matrix4x4 sourceProjection, Rect viewportRect)
        {
            var minX = viewportRect.xMin * 2f - 1f;
            var maxX = viewportRect.xMax * 2f - 1f;
            var minY = viewportRect.yMin * 2f - 1f;
            var maxY = viewportRect.yMax * 2f - 1f;
            var centerX = (minX + maxX) * 0.5f;
            var centerY = (minY + maxY) * 0.5f;
            var scaleX = 2f / Mathf.Max(0.001f, maxX - minX);
            var scaleY = 2f / Mathf.Max(0.001f, maxY - minY);
            var cropped = sourceProjection;
            for (var column = 0; column < 4; column++)
            {
                cropped[0, column] = scaleX * (sourceProjection[0, column] - centerX * sourceProjection[3, column]);
                cropped[1, column] = scaleY * (sourceProjection[1, column] - centerY * sourceProjection[3, column]);
            }

            return cropped;
        }

        private static Camera EnsurePortalCamera(Camera existing, string objectName)
        {
            if (existing != null)
            {
                existing.enabled = false;
                return existing;
            }

            var cameraObject = new GameObject(objectName);
            cameraObject.hideFlags = HideFlags.DontSave;
            var camera = cameraObject.AddComponent<Camera>();
            camera.enabled = false;
            return camera;
        }

        private static RenderTexture EnsureRenderTexture(RenderTexture existing, int size, string textureName)
        {
            if (existing != null && existing.width == size && existing.height == size)
            {
                return existing;
            }

            if (existing != null)
            {
                existing.Release();
                DestroySafe(existing);
            }

            var texture = new RenderTexture(size, size, 24, RenderTextureFormat.ARGB32)
            {
                name = textureName
            };
            texture.Create();
            return texture;
        }

        private static void ReleaseRenderTexture(ref RenderTexture texture)
        {
            if (texture == null)
            {
                return;
            }

            texture.Release();
            DestroySafe(texture);
            texture = null;
        }

        private static void DestroyCamera(ref Camera camera)
        {
            if (camera == null)
            {
                return;
            }

            DestroyRoot(camera.gameObject);
            camera = null;
        }

        private static Material CreateApertureMaterial(Material template, RenderTexture texture, string materialName)
        {
            Material material;
            if (template != null)
            {
                material = new Material(template)
                {
                    name = materialName
                };
            }
            else
            {
                var shader = Shader.Find("Anemora/Review/PortalApertureOverlay");
                if (shader == null)
                {
                    shader = Shader.Find("Universal Render Pipeline/Unlit");
                }

                if (shader == null)
                {
                    shader = Shader.Find("Unlit/Texture");
                }

                if (shader == null)
                {
                    shader = Shader.Find("Standard");
                }

                material = new Material(shader)
                {
                    name = materialName
                };
            }

            if (material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", texture);
            }

            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", texture);
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", Color.white);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", Color.white);
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", 0f);
            }

            material.renderQueue = 2010;
            return material;
        }

        private void EvaluateCrossing()
        {
            if (!committed || player == null || currentSpaceRoot == null || otherTimeSpaceRoot == null)
            {
                return;
            }

            if (!playerInOtherTime)
            {
                var currentLocal = currentSpaceRoot.InverseTransformPoint(player.position);
                if (!hasPreviousCurrentLocal)
                {
                    previousCurrentLocal = currentLocal;
                    hasPreviousCurrentLocal = true;
                    return;
                }

                if (CrossedForward(previousCurrentLocal.z, currentLocal.z))
                {
                    if (IsInsidePortalOpening(currentLocal) || IsInsidePortalOpening(previousCurrentLocal))
                    {
                        EnterOtherTimeAtLocal(currentLocal);
                        return;
                    }

                    outsideCrossingRejected = true;
                    lastTransition = $"Rejected non-portal crossing at current local {Format(currentLocal)}.";
                }

                if (ShouldBlockCurrentBackSideCrossing(previousCurrentLocal, currentLocal))
                {
                    BlockCurrentBackSideCrossing(currentLocal);
                    return;
                }

                if (ShouldRejectCurrentBackSideOccupancy(currentLocal))
                {
                    BlockCurrentBackSideCrossing(currentLocal);
                    return;
                }

                previousCurrentLocal = currentLocal;
                return;
            }

            var otherLocal = otherTimeSpaceRoot.InverseTransformPoint(player.position);
            if (!hasPreviousOtherLocal)
            {
                previousOtherLocal = otherLocal;
                hasPreviousOtherLocal = true;
                return;
            }

            if (CrossedBackward(previousOtherLocal.z, otherLocal.z))
            {
                if (IsInsidePortalOpening(otherLocal) || IsInsidePortalOpening(previousOtherLocal))
                {
                    ReturnCurrentAtLocal(otherLocal);
                    return;
                }

                outsideCrossingRejected = true;
                lastTransition = $"Rejected non-portal crossing at other-time local {Format(otherLocal)}.";
            }

            previousOtherLocal = otherLocal;
        }

        private bool CrossedForward(float previousZ, float currentZ)
        {
            return previousZ <= portalLocalCenter.z &&
                   currentZ >= portalLocalCenter.z;
        }

        private bool CrossedBackward(float previousZ, float currentZ)
        {
            return previousZ >= portalLocalCenter.z &&
                   currentZ <= portalLocalCenter.z;
        }

        private bool IsInsidePortalOpening(Vector3 local)
        {
            return Mathf.Abs(local.x - portalLocalCenter.x) <= portalSize.x * 0.5f &&
                   local.y >= -0.30f &&
                   local.y <= portalLocalCenter.y + portalSize.y * 0.5f + 1.25f;
        }

        private bool ShouldBlockCurrentBackSideCrossing(Vector3 previousLocal, Vector3 currentLocal)
        {
            if (!enableBackSideBlocking ||
                currentLocal.z >= previousLocal.z ||
                (!IsInsidePortalBackBlockZone(currentLocal) && !IsInsidePortalBackBlockZone(previousLocal)))
            {
                return false;
            }

            var blockPlane = CurrentBackSideBlockPlane();
            return previousLocal.z >= blockPlane && currentLocal.z <= blockPlane;
        }

        private bool ShouldRejectCurrentBackSideOccupancy(Vector3 currentLocal)
        {
            return enableBackSideBlocking &&
                   IsInsidePortalBackBlockZone(currentLocal) &&
                   currentLocal.z > CurrentBackSideBlockPlane() + 0.005f;
        }

        private float CurrentBackSideBlockPlane()
        {
            return portalLocalCenter.z + Mathf.Max(currentBackSideBlockDepth, transferExitOffset + crossingHalfDepth);
        }

        private bool IsInsidePortalBackBlockZone(Vector3 local)
        {
            var sideMargin = Mathf.Max(wallVolumeSideMargin + wallVolumeThickness, 0.20f);
            var verticalMargin = Mathf.Max(wallVolumeSideMargin, 0.18f);
            var halfWidth = portalSize.x * 0.5f + sideMargin;
            var halfHeight = portalSize.y * 0.5f + verticalMargin;
            return Mathf.Abs(local.x - portalLocalCenter.x) <= halfWidth &&
                   local.y >= portalLocalCenter.y - halfHeight - 0.35f &&
                   local.y <= portalLocalCenter.y + halfHeight + 1.25f;
        }

        private void BlockCurrentBackSideCrossing(Vector3 attemptedLocal)
        {
            var blocked = attemptedLocal;
            blocked.z = CurrentBackSideBlockPlane();
            SetPlayerWorldPosition(currentSpaceRoot.TransformPoint(blocked));
            previousCurrentLocal = blocked;
            hasPreviousCurrentLocal = true;
            backSideCrossingRejected = true;
            outsideCrossingRejected = true;
            lastTransition = $"Blocked current-side back entry at local {Format(attemptedLocal)}.";
        }

        private void EnterOtherTimeAtLocal(Vector3 currentLocal)
        {
            var mapped = currentLocal;
            mapped.z = portalLocalCenter.z + transferExitOffset;
            lastCurrentToOtherLocal = mapped;
            playerInOtherTime = true;
            SetPlayerWorldPosition(otherTimeSpaceRoot.TransformPoint(mapped));
            ApplyPlayerMaterial(true);
            previousOtherLocal = mapped;
            hasPreviousOtherLocal = true;
            hasPreviousCurrentLocal = false;
            outsideCrossingRejected = false;
            backSideCrossingRejected = false;
            closeRejectedBecausePlayerInOtherTime = false;
            lastTransition = $"Entered other-time space at matching local coordinate {Format(mapped)}.";
        }

        private void ReturnCurrentAtLocal(Vector3 otherLocal)
        {
            var mapped = otherLocal;
            mapped.z = portalLocalCenter.z - transferExitOffset;
            lastOtherToCurrentLocal = mapped;
            playerInOtherTime = false;
            SetPlayerWorldPosition(currentSpaceRoot.TransformPoint(mapped));
            ApplyPlayerMaterial(false);
            previousCurrentLocal = mapped;
            hasPreviousCurrentLocal = true;
            hasPreviousOtherLocal = false;
            outsideCrossingRejected = false;
            backSideCrossingRejected = false;
            closeRejectedBecausePlayerInOtherTime = false;
            lastTransition = $"Returned current space at matching local coordinate {Format(mapped)}.";
        }

        private void SetPlayerWorldPosition(Vector3 worldPosition)
        {
            if (player == null)
            {
                return;
            }

            var wasEnabled = playerController != null && playerController.enabled;
            if (wasEnabled)
            {
                playerController.enabled = false;
            }

            player.position = worldPosition;

            if (wasEnabled)
            {
                playerController.enabled = true;
            }
        }

        private void ApplyPlayerMaterial(bool otherTime)
        {
            var renderer = player != null ? player.GetComponentInChildren<Renderer>() : null;
            if (renderer == null)
            {
                return;
            }

            var material = otherTime ? otherTimePlayerMaterial : currentPlayerMaterial;
            if (material != null)
            {
                renderer.sharedMaterial = material;
            }

            if (enablePortalApertureView)
            {
                ApplyLayerRecursive(player, playerVisibleRenderLayer);
            }

            SyncOtherTimeWallVolumeColliderState();
        }

        private void InitializeReviewLayers()
        {
            if (!enablePortalApertureView)
            {
                return;
            }

            ApplyLayerRecursive(currentSpaceRoot, currentSpaceRenderLayer);
            ApplyLayerRecursive(otherTimeSpaceRoot, otherTimeSpaceRenderLayer);
            ApplyPortalFrameLayer();
            ApplyLayerRecursive(player, playerVisibleRenderLayer);
        }

        private void ApplyPortalFrameLayer()
        {
            ApplyLayerRecursive(currentPortalRoot != null ? currentPortalRoot.transform : null, portalFrameRenderLayer);
            ApplyLayerRecursive(otherTimePortalRoot != null ? otherTimePortalRoot.transform : null, portalFrameRenderLayer);
        }

        private void ResolveReferences()
        {
            if (sceneCamera == null)
            {
                sceneCamera = Camera.main;
            }

            if (player == null && playerController != null)
            {
                player = playerController.transform;
            }

            if (player == null)
            {
                var playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            if (playerController == null && player != null)
            {
                playerController = player.GetComponent<CharacterController>();
            }
        }

        private static void ClearRenderers(List<Renderer> renderers)
        {
            for (var index = renderers.Count - 1; index >= 0; index--)
            {
                var renderer = renderers[index];
                if (renderer != null)
                {
                    DestroyRoot(renderer.gameObject);
                }
            }

            renderers.Clear();
        }

        private static void DestroyApertureRenderer(ref MeshRenderer renderer)
        {
            if (renderer == null)
            {
                return;
            }

            var material = renderer.sharedMaterial;
            DestroyRoot(renderer.gameObject);
            DestroySafe(material);
            renderer = null;
        }

        private static void ApplyLayerRecursive(Transform root, int layer)
        {
            if (root == null)
            {
                return;
            }

            var clamped = Mathf.Clamp(layer, 0, 31);
            foreach (var target in root.GetComponentsInChildren<Transform>(true))
            {
                target.gameObject.layer = clamped;
            }
        }

        private int CountEnabledApertureRenderers()
        {
            var count = 0;
            if (currentApertureRenderer != null && currentApertureRenderer.enabled && currentApertureRenderer.gameObject.activeInHierarchy)
            {
                count++;
            }

            if (otherTimeApertureRenderer != null && otherTimeApertureRenderer.enabled && otherTimeApertureRenderer.gameObject.activeInHierarchy)
            {
                count++;
            }

            return count;
        }

        private static bool HasRendererTexture(Renderer renderer, Texture expected)
        {
            if (renderer == null || expected == null || renderer.sharedMaterial == null)
            {
                return false;
            }

            var material = renderer.sharedMaterial;
            if (material.HasProperty("_BaseMap") && material.GetTexture("_BaseMap") == expected)
            {
                return true;
            }

            return material.HasProperty("_MainTex") && material.GetTexture("_MainTex") == expected;
        }

        private static int CountEnabledColliders(List<Collider> colliders)
        {
            var count = 0;
            for (var index = 0; index < colliders.Count; index++)
            {
                var collider = colliders[index];
                if (collider != null && collider.enabled)
                {
                    count++;
                }
            }

            return count;
        }

        private static bool IsMeaningfulViewportCrop(Rect rect)
        {
            return rect.width > 0.05f &&
                   rect.height > 0.05f &&
                   (rect.width < 0.98f || rect.height < 0.98f);
        }

        private static bool MaskIncludesLayer(int mask, int layer)
        {
            return (mask & (1 << Mathf.Clamp(layer, 0, 31))) != 0;
        }

        private string BuildPortalApertureCameraSyncSummary()
        {
            if (sceneCamera == null ||
                currentToOtherPortalCamera == null ||
                currentSpaceRoot == null ||
                otherTimeSpaceRoot == null)
            {
                return "unavailable";
            }

            var sceneLocal = currentSpaceRoot.InverseTransformPoint(sceneCamera.transform.position);
            var renderLocal = otherTimeSpaceRoot.InverseTransformPoint(currentToOtherPortalCamera.transform.position);
            var sceneLocalRotation = Quaternion.Inverse(currentSpaceRoot.rotation) * sceneCamera.transform.rotation;
            var renderLocalRotation = Quaternion.Inverse(otherTimeSpaceRoot.rotation) * currentToOtherPortalCamera.transform.rotation;
            var sceneEuler = sceneLocalRotation.eulerAngles;
            var renderEuler = renderLocalRotation.eulerAngles;
            return
                "mapping=currentRootLocalToOtherRootLocal, " +
                "projection=axisAlignedScreenRectCrop, " +
                "obliquePortalPlaneClip=False, " +
                "exactTrapezoidWarp=False, " +
                $"sceneLocal={FormatPrecise(sceneLocal)}, " +
                $"renderLocal={FormatPrecise(renderLocal)}, " +
                $"sceneEuler={FormatPrecise(sceneEuler)}, " +
                $"renderEuler={FormatPrecise(renderEuler)}, " +
                $"sceneFov={sceneCamera.fieldOfView:0.000}, renderFov={currentToOtherPortalCamera.fieldOfView:0.000}, " +
                $"sceneAspect={sceneCamera.aspect:0.000}, renderAspect={currentToOtherPortalCamera.aspect:0.000}, " +
                $"sceneNearFar=({sceneCamera.nearClipPlane:0.000},{sceneCamera.farClipPlane:0.000}), " +
                $"renderNearFar=({currentToOtherPortalCamera.nearClipPlane:0.000},{currentToOtherPortalCamera.farClipPlane:0.000}), " +
                $"currentViewport={FormatRect(currentApertureViewportRect)}, otherViewport={FormatRect(otherTimeApertureViewportRect)}, " +
                $"portalLocal={FormatPrecise(portalLocalCenter)}, portalSize=({portalSize.x:0.000},{portalSize.y:0.000})";
        }

        private static void DestroyRoot(GameObject root)
        {
            if (root == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(root);
            }
            else
            {
                DestroyImmediate(root);
            }
        }

        private static void DestroySafe(Object target)
        {
            if (target == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(target);
            }
            else
            {
                DestroyImmediate(target);
            }
        }

        private static string Format(Vector3 value)
        {
            return $"({value.x:0.00}, {value.y:0.00}, {value.z:0.00})";
        }

        private static string FormatPrecise(Vector3 value)
        {
            return $"({value.x:0.000},{value.y:0.000},{value.z:0.000})";
        }

        private static string FormatRect(Rect rect)
        {
            return $"({rect.x:0.000},{rect.y:0.000},{rect.width:0.000},{rect.height:0.000})";
        }
    }
}
