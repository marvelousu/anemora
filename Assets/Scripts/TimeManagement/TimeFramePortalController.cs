using System;
using System.Collections;
using System.Reflection;
using Anemora.Audio;
using Anemora.UI;
using UnityEngine;

namespace Anemora.TimeManagement
{
    public enum PortalState
    {
        Normal,
        Selecting,
        Generating,
        Open,
        Crossing,
        Flipping
    }

    /// <summary>
    /// Orchestrates the time-frame portal state machine and atomic side flip.
    /// </summary>
    public sealed class TimeFramePortalController : MonoBehaviour
    {
        [SerializeField] private SymbolWheelController symbolWheel;
        [SerializeField] private Transform player;
        [SerializeField] private GameObject portalPrefab;
        [SerializeField] private Transform portalSpawnPoint;
        [SerializeField] private PortalCrossingDetector crossingDetector;
        [SerializeField] private SceneSidePolarity sidePolarity;
        [SerializeField] private PortalVisualSwitcher visualSwitcher;
        [SerializeField] private PortalFlashPlayer flashPlayer;
        [SerializeField] private float generationDuration = 0.05f;
        [SerializeField] private float flipCooldown = 0.1f;
        [SerializeField] private float flashDuration = 0.05f;
        [SerializeField] private float snapDistanceMultiplier = 1.5f;
        [SerializeField] private bool enableBrushInput = true;
        [SerializeField] private KeyCode brushModifierKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode alternateBrushModifierKey = KeyCode.RightShift;
        [SerializeField] private int brushMouseButton = 0;
        [SerializeField] private float minBrushDragPixels = 24f;
        [SerializeField] private bool enableQuickPlaceInput = true;
        [SerializeField] private KeyCode quickPlacePortalKey = KeyCode.F;
        [SerializeField] private float quickPlaceDistance = 2.2f;
        [SerializeField] private int cancelMouseButton = 1;
        [SerializeField] private bool enableKeyboardPortalShortcut = true;
        [SerializeField] private KeyCode openPortalKey = KeyCode.Q;
        [SerializeField] private KeyCode closePortalKey = KeyCode.Escape;
        [SerializeField] private bool useLocalDioramaWindow;
        [SerializeField] private Vector2 defaultLocalWindowSize = new Vector2(3.8f, 3.2f);
        [SerializeField] private Vector2 minLocalWindowSize = new Vector2(2.6f, 2.2f);
        [SerializeField] private Vector2 maxLocalWindowSize = new Vector2(9f, 8f);
        [SerializeField] private bool showBrushFootprintPreview = true;
        [SerializeField] private int brushPreviewLayer = 10;
        [SerializeField] private float brushPreviewLift = 0.075f;
        [SerializeField] private float brushPreviewEdgeThickness = 0.045f;
        [SerializeField] private float minimumDraggedWindowWorldSize = 0.75f;
        [SerializeField] private Color brushPreviewFillColor = new Color(0.16f, 0.66f, 1f, 0.2f);
        [SerializeField] private Color brushPreviewEdgeColor = new Color(1f, 0.9f, 0.62f, 0.86f);

        private Coroutine generationRoutine;
        private Coroutine flipRoutine;
        private GameObject portalInstance;
        private PortalState state = PortalState.Normal;
        private int portalGenerationCount;
        private bool brushDragActive;
        private Vector2 brushDragStart;
        private bool hasPendingLocalWindowBounds;
        private Vector3 pendingLocalWindowCenter;
        private Quaternion pendingLocalWindowRotation = Quaternion.identity;
        private Vector2 pendingLocalWindowSize;
        private GameObject brushPreviewRoot;
        private Transform brushPreviewFill;
        private Transform brushPreviewFrontEdge;
        private Transform brushPreviewBackEdge;
        private Transform brushPreviewLeftEdge;
        private Transform brushPreviewRightEdge;
        private Material brushPreviewFillMaterial;
        private Material brushPreviewEdgeMaterial;

        public event Action<PortalState, float> StateChanged;
        public event Action<SceneSide> CrossingCompleted;

        public PortalState State => state;
        public GameObject PortalInstance => portalInstance;
        public int PortalGenerationCount => portalGenerationCount;
        public bool UsesLocalDioramaWindow => useLocalDioramaWindow;

        private void Awake()
        {
            if (crossingDetector == null)
            {
                crossingDetector = GetComponent<PortalCrossingDetector>();
            }

            if (sidePolarity == null)
            {
                sidePolarity = GetComponent<SceneSidePolarity>();
            }

            if (visualSwitcher == null)
            {
                visualSwitcher = GetComponent<PortalVisualSwitcher>();
            }

            if (flashPlayer == null)
            {
                flashPlayer = GetComponent<PortalFlashPlayer>();
            }

            if (player == null)
            {
                var playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            if (crossingDetector != null)
            {
                crossingDetector.SetArmed(false);
            }
        }

        private void OnEnable()
        {
            if (symbolWheel != null)
            {
                symbolWheel.OnSymbolSelected.AddListener(HandleSymbolSelected);
            }

            if (crossingDetector != null)
            {
                crossingDetector.Crossed += HandlePortalCrossed;
            }
        }

        private void OnDisable()
        {
            if (symbolWheel != null)
            {
                symbolWheel.OnSymbolSelected.RemoveListener(HandleSymbolSelected);
            }

            if (crossingDetector != null)
            {
                crossingDetector.Crossed -= HandlePortalCrossed;
                crossingDetector.SetArmed(false);
            }

            if (state == PortalState.Selecting || state == PortalState.Generating)
            {
                Time.timeScale = 1f;
            }

            DestroyBrushPreview();
        }

        private void OnDestroy()
        {
            DestroyBrushPreview();
            DestroyPreviewMaterial(brushPreviewFillMaterial);
            DestroyPreviewMaterial(brushPreviewEdgeMaterial);
        }

        private void Update()
        {
            HandleKeyboardPortalInput();
            HandleCancelInput();
            HandleQuickPlacePortalInput();
            HandleBrushInput();
        }

        public void HandleSymbolSelected(SymbolType symbol)
        {
            if (symbol != SymbolType.Red || state != PortalState.Normal)
            {
                return;
            }

            if (generationRoutine != null)
            {
                return;
            }

            generationRoutine = StartCoroutine(GeneratePortalRoutine());
        }

        public bool TryCompleteBrushStrokeForTests(Vector2 startScreenPosition, Vector2 endScreenPosition, bool modifierHeld)
        {
            return TryCompleteBrushStroke(startScreenPosition, endScreenPosition, modifierHeld);
        }

        public bool TryOpenPortalForDemo()
        {
            return TryOpenPortalFromInput();
        }

        public bool TryPlaceQuickLocalWindowForTests()
        {
            return TryPlaceQuickLocalWindow();
        }

        public bool TryUpdateBrushPreviewForTests(Vector2 startScreenPosition, Vector2 endScreenPosition)
        {
            brushDragStart = startScreenPosition;
            return TryUpdateBrushPreview(endScreenPosition);
        }

        public void ClearBrushPreviewForTests()
        {
            DestroyBrushPreview();
        }

        public void TriggerCrossingForTests()
        {
            HandlePortalCrossed();
        }

        public void SetDurationsForTests(float generationDuration, float flipCooldown, float flashDuration)
        {
            this.generationDuration = Mathf.Max(0f, generationDuration);
            this.flipCooldown = Mathf.Max(0f, flipCooldown);
            this.flashDuration = Mathf.Max(0f, flashDuration);
        }

        public void SetLocalDioramaWindowForTests(bool enabled)
        {
            useLocalDioramaWindow = enabled;
        }

        public void ClosePortal()
        {
            if (crossingDetector != null)
            {
                crossingDetector.SetArmed(false);
            }

            if (portalInstance != null)
            {
                var localWindow = portalInstance.GetComponent<TimeWindowDiorama>();
                if (localWindow != null)
                {
                    localWindow.PlayCloseAndDestroy();
                }
                else
                {
                    Destroy(portalInstance);
                }

                portalInstance = null;
            }

            Zone1AudioController.Instance?.PlayTimeWheelClose();
            Time.timeScale = 1f;
            hasPendingLocalWindowBounds = false;
            SetState(PortalState.Normal);
        }

        private IEnumerator GeneratePortalRoutine()
        {
            Time.timeScale = 0f;
            SetState(PortalState.Selecting);
            Zone1AudioController.Instance?.PlayTimeWheelOpen();
            symbolWheel?.SetVisible(false);

            SetState(PortalState.Generating);
            if (generationDuration > 0f)
            {
                yield return new WaitForSecondsRealtime(generationDuration);
            }
            else
            {
                yield return null;
            }

            EnsurePortalInstance();
            ConfigureOpenPortal();
            Zone1AudioController.Instance?.PlayTimePortalOpen();

            Time.timeScale = 1f;
            SetState(PortalState.Open);
            generationRoutine = null;
        }

        private void HandlePortalCrossed()
        {
            if (state != PortalState.Open || flipRoutine != null)
            {
                return;
            }

            if (useLocalDioramaWindow)
            {
                return;
            }

            var currentSide = sidePolarity != null ? sidePolarity.CurrentSide : SceneSide.Current;
            flipRoutine = StartCoroutine(PerformAtomicFlipRoutine(SceneSidePolarity.OppositeOf(currentSide)));
        }

        private IEnumerator PerformAtomicFlipRoutine(SceneSide targetSide)
        {
            SetState(PortalState.Crossing);
            Zone1AudioController.Instance?.PlayTimePortalFlip();

            if (crossingDetector != null)
            {
                crossingDetector.SetArmed(false);
            }

            var playerObject = player != null ? player.gameObject : null;
            if (visualSwitcher != null)
            {
                visualSwitcher.ApplyForSide(targetSide, playerObject, portalInstance);
            }

            SnapPlayerToStableSide(targetSide);

            if (sidePolarity != null)
            {
                sidePolarity.FlipTo(targetSide);
            }

            if (flashPlayer != null)
            {
                flashPlayer.PlayOnce(flashDuration);
            }

            SetState(PortalState.Flipping);
            if (flipCooldown > 0f)
            {
                yield return new WaitForSecondsRealtime(flipCooldown);
            }
            else
            {
                yield return null;
            }

            if (crossingDetector != null)
            {
                if (player != null)
                {
                    crossingDetector.SetLastStableSignedDistance(crossingDetector.PlaneNormal, player.position);
                }

                crossingDetector.SetArmed(true);
            }

            SetState(PortalState.Open);
            flipRoutine = null;
            CrossingCompleted?.Invoke(targetSide);
        }

        private void EnsurePortalInstance()
        {
            if (portalInstance != null)
            {
                return;
            }

            var spawnTransform = portalSpawnPoint != null ? portalSpawnPoint : transform;
            var spawnPosition = useLocalDioramaWindow
                ? ResolvePendingLocalWindowCenter(spawnTransform)
                : spawnTransform.position;
            var spawnRotation = useLocalDioramaWindow
                ? ResolvePendingLocalWindowRotation()
                : spawnTransform.rotation;

            if (portalPrefab != null)
            {
                portalInstance = Instantiate(portalPrefab, spawnPosition, spawnRotation);
            }
            else
            {
                portalInstance = GameObject.CreatePrimitive(useLocalDioramaWindow ? PrimitiveType.Cube : PrimitiveType.Quad);
                portalInstance.name = useLocalDioramaWindow ? "TimeWindow_Diorama_Runtime" : "Portal_Frame_Runtime";
                portalInstance.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
                var collider = portalInstance.GetComponent<Collider>();
                if (collider != null)
                {
                    Destroy(collider);
                }
            }

            if (useLocalDioramaWindow && portalInstance != null)
            {
                var window = portalInstance.GetComponent<TimeWindowDiorama>();
                if (window != null)
                {
                    window.Configure(spawnPosition, ResolvePendingLocalWindowSize(), player, spawnRotation);
                }
            }

            portalGenerationCount++;
        }

        private void ConfigureOpenPortal()
        {
            if (useLocalDioramaWindow)
            {
                if (crossingDetector != null)
                {
                    crossingDetector.SetArmed(false);
                }

                return;
            }

            var side = sidePolarity != null ? sidePolarity.CurrentSide : SceneSide.Current;
            var playerObject = player != null ? player.gameObject : null;

            if (visualSwitcher != null)
            {
                visualSwitcher.SetPortalMaskRoot(portalInstance);
                visualSwitcher.ApplyForSide(side, playerObject, portalInstance);
            }

            if (crossingDetector != null)
            {
                crossingDetector.Configure(player, portalInstance != null ? portalInstance.transform : transform);
                crossingDetector.SetArmed(true);
            }
        }

        private void SnapPlayerToStableSide(SceneSide targetSide)
        {
            if (player == null || crossingDetector == null)
            {
                return;
            }

            var sign = targetSide == SceneSide.Current ? 1f : -1f;
            var desiredDistance = crossingDetector.HysteresisBand * snapDistanceMultiplier * sign;
            var currentDistance = crossingDetector.GetSignedDistance(player.position);
            var delta = desiredDistance - currentDistance;
            player.position += crossingDetector.PlaneNormal * delta;
        }

        private void SetState(PortalState nextState)
        {
            state = nextState;
            StateChanged?.Invoke(state, Time.timeScale);
        }

        private void HandleBrushInput()
        {
            if (!enableBrushInput || IsDialogueDisplayVisible())
            {
                CancelBrushDrag();
                return;
            }

            var modifierHeld = Input.GetKey(brushModifierKey) || Input.GetKey(alternateBrushModifierKey);
            if (state != PortalState.Normal)
            {
                CancelBrushDrag();
                return;
            }

            if (Input.GetMouseButtonDown(brushMouseButton) && modifierHeld)
            {
                brushDragStart = Input.mousePosition;
                brushDragActive = true;
                symbolWheel?.SetVisible(false);
                TryUpdateBrushPreview(Input.mousePosition);
                return;
            }

            if (!brushDragActive)
            {
                return;
            }

            if (!modifierHeld)
            {
                CancelBrushDrag();
                return;
            }

            TryUpdateBrushPreview(Input.mousePosition);

            if (Input.GetMouseButtonUp(brushMouseButton))
            {
                TryCompleteBrushStroke(brushDragStart, Input.mousePosition, true);
                CancelBrushDrag();
            }
        }

        private void HandleKeyboardPortalInput()
        {
            if (IsDialogueDisplayVisible())
            {
                return;
            }

            if (enableKeyboardPortalShortcut && Input.GetKeyDown(openPortalKey))
            {
                TryOpenPortalFromInput();
                return;
            }

            if (Input.GetKeyDown(closePortalKey) && state == PortalState.Open)
            {
                ClosePortal();
            }
        }

        private void HandleQuickPlacePortalInput()
        {
            if (!enableQuickPlaceInput ||
                !useLocalDioramaWindow ||
                IsDialogueDisplayVisible() ||
                !Input.GetKeyDown(quickPlacePortalKey))
            {
                return;
            }

            TryPlaceQuickLocalWindow();
        }

        private void HandleCancelInput()
        {
            if (IsDialogueDisplayVisible() ||
                cancelMouseButton < 0 ||
                !Input.GetMouseButtonDown(cancelMouseButton))
            {
                return;
            }

            if (brushDragActive)
            {
                hasPendingLocalWindowBounds = false;
                CancelBrushDrag();
                return;
            }

            if (state == PortalState.Open)
            {
                ClosePortal();
            }
        }

        private bool TryCompleteBrushStroke(Vector2 startScreenPosition, Vector2 endScreenPosition, bool modifierHeld)
        {
            if (!enableBrushInput ||
                !modifierHeld ||
                state != PortalState.Normal ||
                IsDialogueDisplayVisible())
            {
                return false;
            }

            var minPixels = Mathf.Max(1f, minBrushDragPixels);
            if ((endScreenPosition - startScreenPosition).sqrMagnitude < minPixels * minPixels)
            {
                CaptureLocalWindowPoint(endScreenPosition);
                symbolWheel?.SetVisible(false);
                return useLocalDioramaWindow && TryOpenPortalFromInput();
            }

            if (!CaptureLocalWindowBounds(startScreenPosition, endScreenPosition))
            {
                return false;
            }

            return TryOpenPortalFromInput();
        }

        private bool TryOpenPortalFromInput()
        {
            if (state != PortalState.Normal ||
                generationRoutine != null ||
                IsDialogueDisplayVisible())
            {
                return false;
            }

            if (useLocalDioramaWindow && !hasPendingLocalWindowBounds)
            {
                SetDefaultLocalWindowBounds();
            }

            if (symbolWheel != null)
            {
                symbolWheel.SetVisible(true);
                symbolWheel.SelectFocusedSymbol();
                symbolWheel.SetVisible(false);
            }
            else
            {
                HandleSymbolSelected(SymbolType.Red);
            }

            return generationRoutine != null || state != PortalState.Normal;
        }

        private bool TryPlaceQuickLocalWindow()
        {
            if (!useLocalDioramaWindow ||
                generationRoutine != null ||
                IsDialogueDisplayVisible())
            {
                return false;
            }

            if (state == PortalState.Open)
            {
                ClosePortal();
            }

            if (state != PortalState.Normal)
            {
                return false;
            }

            SetQuickLocalWindowBounds();
            return TryOpenPortalFromInput();
        }

        private bool CaptureLocalWindowBounds(Vector2 startScreenPosition, Vector2 endScreenPosition)
        {
            if (!useLocalDioramaWindow)
            {
                return false;
            }

            if (!TryResolveDraggedLocalWindow(startScreenPosition, endScreenPosition, out var center, out var size))
            {
                SetDefaultLocalWindowBounds();
                return false;
            }

            pendingLocalWindowCenter = center;
            pendingLocalWindowRotation = Quaternion.identity;
            pendingLocalWindowSize = size;
            hasPendingLocalWindowBounds = true;
            return true;
        }

        private void CaptureLocalWindowPoint(Vector2 screenPosition)
        {
            if (!useLocalDioramaWindow)
            {
                return;
            }

            if (!TryScreenPointToGround(screenPosition, out var point))
            {
                SetDefaultLocalWindowBounds();
                return;
            }

            var size = ClampLocalWindowSize(defaultLocalWindowSize);
            var center = point;
            center.y = 0.035f;
            pendingLocalWindowCenter = center;
            pendingLocalWindowRotation = Quaternion.identity;
            pendingLocalWindowSize = size;
            hasPendingLocalWindowBounds = true;
        }

        private void SetDefaultLocalWindowBounds()
        {
            var spawnTransform = portalSpawnPoint != null ? portalSpawnPoint : transform;
            var center = spawnTransform.position;
            var rotation = Quaternion.identity;
            if (player != null)
            {
                var forward = Vector3.ProjectOnPlane(player.forward, Vector3.up);
                if (forward.sqrMagnitude > 0.001f)
                {
                    forward.Normalize();
                    center = player.position + forward * (defaultLocalWindowSize.y * 0.55f);
                }
            }

            center.y = 0.035f;
            pendingLocalWindowCenter = center;
            pendingLocalWindowRotation = rotation;
            pendingLocalWindowSize = ClampLocalWindowSize(defaultLocalWindowSize);
            hasPendingLocalWindowBounds = true;
        }

        private void SetQuickLocalWindowBounds()
        {
            var size = ClampLocalWindowSize(defaultLocalWindowSize);
            var forward = ResolvePlayerForward();
            var origin = player != null ? player.position : transform.position;
            var distance = Mathf.Max(size.y * 0.55f, quickPlaceDistance);
            var center = origin + forward * distance;
            center.y = 0.035f;

            pendingLocalWindowCenter = center;
            pendingLocalWindowRotation = Quaternion.identity;
            pendingLocalWindowSize = size;
            hasPendingLocalWindowBounds = true;
        }

        private Vector3 ResolvePlayerForward()
        {
            if (player != null)
            {
                var forward = Vector3.ProjectOnPlane(player.forward, Vector3.up);
                if (forward.sqrMagnitude > 0.001f)
                {
                    return forward.normalized;
                }
            }

            var camera = Camera.main;
            if (camera != null)
            {
                var cameraForward = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up);
                if (cameraForward.sqrMagnitude > 0.001f)
                {
                    return cameraForward.normalized;
                }
            }

            return Vector3.forward;
        }

        private Vector3 ResolvePendingLocalWindowCenter(Transform fallback)
        {
            if (!hasPendingLocalWindowBounds)
            {
                SetDefaultLocalWindowBounds();
            }

            return hasPendingLocalWindowBounds ? pendingLocalWindowCenter : fallback.position;
        }

        private Vector2 ResolvePendingLocalWindowSize()
        {
            if (!hasPendingLocalWindowBounds)
            {
                SetDefaultLocalWindowBounds();
            }

            return pendingLocalWindowSize;
        }

        private Quaternion ResolvePendingLocalWindowRotation()
        {
            if (!hasPendingLocalWindowBounds)
            {
                SetDefaultLocalWindowBounds();
            }

            return pendingLocalWindowRotation;
        }

        private Vector2 ClampLocalWindowSize(Vector2 size)
        {
            return new Vector2(
                Mathf.Clamp(size.x, Mathf.Max(0.5f, minLocalWindowSize.x), Mathf.Max(minLocalWindowSize.x, maxLocalWindowSize.x)),
                Mathf.Clamp(size.y, Mathf.Max(0.5f, minLocalWindowSize.y), Mathf.Max(minLocalWindowSize.y, maxLocalWindowSize.y)));
        }

        private Vector2 ClampDraggedLocalWindowSize(Vector2 size)
        {
            var minimum = Mathf.Max(0.05f, minimumDraggedWindowWorldSize);
            return new Vector2(
                Mathf.Clamp(size.x, minimum, Mathf.Max(minimum, maxLocalWindowSize.x)),
                Mathf.Clamp(size.y, minimum, Mathf.Max(minimum, maxLocalWindowSize.y)));
        }

        private bool TryResolveDraggedLocalWindow(Vector2 startScreenPosition, Vector2 endScreenPosition, out Vector3 center, out Vector2 size)
        {
            center = default;
            size = default;
            if (!TryScreenPointToGround(startScreenPosition, out var startWorld) ||
                !TryScreenPointToGround(endScreenPosition, out var endWorld))
            {
                return false;
            }

            var rawSize = new Vector2(
                Mathf.Abs(endWorld.x - startWorld.x),
                Mathf.Abs(endWorld.z - startWorld.z));
            if (rawSize.x <= 0.001f && rawSize.y <= 0.001f)
            {
                return false;
            }

            size = ClampDraggedLocalWindowSize(rawSize);
            center = new Vector3(
                (startWorld.x + endWorld.x) * 0.5f,
                0.035f,
                (startWorld.z + endWorld.z) * 0.5f);
            return true;
        }

        private bool TryUpdateBrushPreview(Vector2 endScreenPosition)
        {
            if (!showBrushFootprintPreview ||
                !useLocalDioramaWindow ||
                !brushDragActive && !Application.isEditor ||
                !TryResolveDraggedLocalWindow(brushDragStart, endScreenPosition, out var center, out var size))
            {
                DestroyBrushPreview();
                return false;
            }

            EnsureBrushPreview();
            brushPreviewRoot.transform.SetPositionAndRotation(center, Quaternion.identity);
            brushPreviewFill.localPosition = new Vector3(0f, brushPreviewLift, 0f);
            brushPreviewFill.localRotation = Quaternion.identity;
            brushPreviewFill.localScale = new Vector3(size.x, 0.012f, size.y);

            var edgeThickness = Mathf.Max(0.01f, brushPreviewEdgeThickness);
            var edgeLift = brushPreviewLift + 0.03f;
            brushPreviewFrontEdge.localPosition = new Vector3(0f, edgeLift, -size.y * 0.5f);
            brushPreviewFrontEdge.localScale = new Vector3(size.x, edgeThickness, edgeThickness);
            brushPreviewBackEdge.localPosition = new Vector3(0f, edgeLift, size.y * 0.5f);
            brushPreviewBackEdge.localScale = new Vector3(size.x, edgeThickness, edgeThickness);
            brushPreviewLeftEdge.localPosition = new Vector3(-size.x * 0.5f, edgeLift, 0f);
            brushPreviewLeftEdge.localScale = new Vector3(edgeThickness, edgeThickness, size.y);
            brushPreviewRightEdge.localPosition = new Vector3(size.x * 0.5f, edgeLift, 0f);
            brushPreviewRightEdge.localScale = new Vector3(edgeThickness, edgeThickness, size.y);
            return true;
        }

        private void EnsureBrushPreview()
        {
            if (brushPreviewRoot != null)
            {
                return;
            }

            brushPreviewRoot = new GameObject("TimeWindow_BrushPreview_Runtime");
            SetLayerRecursively(brushPreviewRoot, brushPreviewLayer);
            brushPreviewFillMaterial = brushPreviewFillMaterial != null ? brushPreviewFillMaterial : CreatePreviewMaterial("TimeWindow_BrushPreview_Fill", brushPreviewFillColor);
            brushPreviewEdgeMaterial = brushPreviewEdgeMaterial != null ? brushPreviewEdgeMaterial : CreatePreviewMaterial("TimeWindow_BrushPreview_Edge", brushPreviewEdgeColor);
            brushPreviewFill = CreatePreviewPart("TimeWindow_BrushPreview_Fill", brushPreviewFillMaterial);
            brushPreviewFrontEdge = CreatePreviewPart("TimeWindow_BrushPreview_Edge_Front", brushPreviewEdgeMaterial);
            brushPreviewBackEdge = CreatePreviewPart("TimeWindow_BrushPreview_Edge_Back", brushPreviewEdgeMaterial);
            brushPreviewLeftEdge = CreatePreviewPart("TimeWindow_BrushPreview_Edge_Left", brushPreviewEdgeMaterial);
            brushPreviewRightEdge = CreatePreviewPart("TimeWindow_BrushPreview_Edge_Right", brushPreviewEdgeMaterial);
        }

        private Transform CreatePreviewPart(string objectName, Material material)
        {
            var part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.name = objectName;
            part.transform.SetParent(brushPreviewRoot.transform, false);
            SetLayerRecursively(part, brushPreviewLayer);
            var collider = part.GetComponent<Collider>();
            if (collider != null)
            {
                Destroy(collider);
            }

            var renderer = part.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            return part.transform;
        }

        private Material CreatePreviewMaterial(string materialName, Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Color");
            var material = new Material(shader)
            {
                name = materialName,
                renderQueue = 3000
            };

            material.SetColor("_BaseColor", color);
            material.SetColor("_Color", color);
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            return material;
        }

        private void CancelBrushDrag()
        {
            brushDragActive = false;
            symbolWheel?.SetVisible(false);
            DestroyBrushPreview();
        }

        private void DestroyBrushPreview()
        {
            if (brushPreviewRoot == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(brushPreviewRoot);
            }
            else
            {
                DestroyImmediate(brushPreviewRoot);
            }

            brushPreviewRoot = null;
            brushPreviewFill = null;
            brushPreviewFrontEdge = null;
            brushPreviewBackEdge = null;
            brushPreviewLeftEdge = null;
            brushPreviewRightEdge = null;
        }

        private static void DestroyPreviewMaterial(Material material)
        {
            if (material == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(material);
            }
            else
            {
                DestroyImmediate(material);
            }
        }

        private static void SetLayerRecursively(GameObject root, int layer)
        {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        private static bool TryScreenPointToGround(Vector2 screenPosition, out Vector3 worldPosition)
        {
            var camera = Camera.main;
            if (camera == null)
            {
                worldPosition = default;
                return false;
            }

            var ray = camera.ScreenPointToRay(screenPosition);
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (!plane.Raycast(ray, out var distance))
            {
                worldPosition = default;
                return false;
            }

            worldPosition = ray.GetPoint(distance);
            return true;
        }

        private static bool IsDialogueDisplayVisible()
        {
            var displayType =
                Type.GetType("Anemora.Dialogue.DialogueDisplay, Anemora.Dialogue", throwOnError: false) ??
                Type.GetType("Anemora.Dialogue.DialogueDisplay, Assembly-CSharp", throwOnError: false);
            var instance = displayType?
                .GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?
                .GetValue(null);
            var isVisible = instance?
                .GetType()
                .GetProperty("IsVisible", BindingFlags.Public | BindingFlags.Instance)?
                .GetValue(instance);
            return isVisible is bool visible && visible;
        }
    }
}
