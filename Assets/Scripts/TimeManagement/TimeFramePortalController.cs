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

        private Coroutine generationRoutine;
        private Coroutine flipRoutine;
        private GameObject portalInstance;
        private PortalState state = PortalState.Normal;
        private int portalGenerationCount;
        private bool brushDragActive;
        private Vector2 brushDragStart;

        public event Action<PortalState, float> StateChanged;
        public event Action<SceneSide> CrossingCompleted;

        public PortalState State => state;
        public GameObject PortalInstance => portalInstance;
        public int PortalGenerationCount => portalGenerationCount;

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
        }

        private void Update()
        {
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

        public void ClosePortal()
        {
            if (crossingDetector != null)
            {
                crossingDetector.SetArmed(false);
            }

            if (portalInstance != null)
            {
                Destroy(portalInstance);
                portalInstance = null;
            }

            Zone1AudioController.Instance?.PlayTimeWheelClose();
            Time.timeScale = 1f;
            SetState(PortalState.Normal);
        }

        private IEnumerator GeneratePortalRoutine()
        {
            Time.timeScale = 0f;
            SetState(PortalState.Selecting);
            Zone1AudioController.Instance?.PlayTimeWheelOpen();

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
            if (portalPrefab != null)
            {
                portalInstance = Instantiate(portalPrefab, spawnTransform.position, spawnTransform.rotation);
            }
            else
            {
                portalInstance = GameObject.CreatePrimitive(PrimitiveType.Quad);
                portalInstance.name = "Portal_Frame_Runtime";
                portalInstance.transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
                var collider = portalInstance.GetComponent<Collider>();
                if (collider != null)
                {
                    Destroy(collider);
                }
            }

            portalGenerationCount++;
        }

        private void ConfigureOpenPortal()
        {
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
                brushDragActive = false;
                return;
            }

            var modifierHeld = Input.GetKey(brushModifierKey) || Input.GetKey(alternateBrushModifierKey);
            if (state != PortalState.Normal)
            {
                brushDragActive = false;
                return;
            }

            if (Input.GetMouseButtonDown(brushMouseButton) && modifierHeld)
            {
                brushDragStart = Input.mousePosition;
                brushDragActive = true;
                return;
            }

            if (!brushDragActive)
            {
                return;
            }

            if (!modifierHeld)
            {
                brushDragActive = false;
                return;
            }

            if (Input.GetMouseButtonUp(brushMouseButton))
            {
                TryCompleteBrushStroke(brushDragStart, Input.mousePosition, true);
                brushDragActive = false;
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
                return false;
            }

            if (symbolWheel != null)
            {
                symbolWheel.SelectFocusedSymbol();
            }
            else
            {
                HandleSymbolSelected(SymbolType.Red);
            }

            return generationRoutine != null || state != PortalState.Normal;
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
