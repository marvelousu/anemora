using Anemora.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Anemora.UI
{
    public enum SymbolType
    {
        Red,
        White,
        Blue
    }

    public sealed class SymbolWheelController : MonoBehaviour
    {
        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private Image redSymbol;
        [SerializeField] private Image whiteSymbol;
        [SerializeField] private Image blueSymbol;
        [SerializeField] private float disabledAlpha = 0.4f;
        [SerializeField] private bool acceptKeyboardSelection;
        [SerializeField] private bool acceptMouseHover = true;
        [SerializeField] private bool hideCanvasOnAwake = true;
        [SerializeField] private UnityEvent<SymbolType> onSymbolSelected = new();

        private SymbolType focusedSymbol = SymbolType.Red;
        private bool verticalAxisHeld;

        public UnityEvent<SymbolType> OnSymbolSelected => onSymbolSelected;
        public SymbolType FocusedSymbol => focusedSymbol;

        private void Awake()
        {
            if (rootCanvas == null)
            {
                rootCanvas = GetComponentInParent<Canvas>();
            }

            if (rootCanvas != null && rootCanvas.renderMode == RenderMode.ScreenSpaceCamera && rootCanvas.worldCamera == null)
            {
                rootCanvas.worldCamera = Camera.main;
            }

            if (rootCanvas != null && hideCanvasOnAwake)
            {
                rootCanvas.enabled = false;
            }

            ApplyState();
        }

        private void Update()
        {
            if (acceptKeyboardSelection)
            {
                HandleKeyboardAndPadInput();
            }

            if (acceptMouseHover)
            {
                HandleMouseHover();
            }
        }

        public void SelectFocusedSymbol()
        {
            if (focusedSymbol != SymbolType.Red)
            {
                return;
            }

            Zone1AudioController.Instance?.PlayTimeSymbolSelectRed();
            Debug.Log("Red symbol selected");
            onSymbolSelected.Invoke(SymbolType.Red);
        }

        public void SetKeyboardSelectionEnabled(bool enabled)
        {
            acceptKeyboardSelection = enabled;
        }

        private void HandleKeyboardAndPadInput()
        {
            var movePressed =
                Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.S);

            var vertical = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(vertical) > 0.5f)
            {
                if (!verticalAxisHeld)
                {
                    movePressed = true;
                    verticalAxisHeld = true;
                }
            }
            else
            {
                verticalAxisHeld = false;
            }

            if (movePressed)
            {
                Focus(SymbolType.Red);
            }

            if (Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                SelectFocusedSymbol();
            }
        }

        private void HandleMouseHover()
        {
            if (redSymbol == null)
            {
                return;
            }

            var eventCamera = rootCanvas != null ? rootCanvas.worldCamera : null;
            if (RectTransformUtility.RectangleContainsScreenPoint(redSymbol.rectTransform, Input.mousePosition, eventCamera))
            {
                Focus(SymbolType.Red);
            }
        }

        private void Focus(SymbolType symbol)
        {
            if (focusedSymbol == symbol)
            {
                return;
            }

            focusedSymbol = symbol;
            ApplyState();
            Zone1AudioController.Instance?.PlayTimeSymbolHover();
        }

        private void ApplyState()
        {
            ApplyImageState(redSymbol, true, focusedSymbol == SymbolType.Red);
            ApplyImageState(whiteSymbol, false, focusedSymbol == SymbolType.White);
            ApplyImageState(blueSymbol, false, focusedSymbol == SymbolType.Blue);
        }

        private void ApplyImageState(Image image, bool enabled, bool focused)
        {
            if (image == null)
            {
                return;
            }

            var color = image.color;
            color.a = enabled ? 1f : disabledAlpha;
            image.color = color;
            image.raycastTarget = enabled;
            image.transform.localScale = focused ? Vector3.one * 1.12f : Vector3.one;
        }

        private void OnValidate()
        {
            ApplyState();
        }
    }
}
