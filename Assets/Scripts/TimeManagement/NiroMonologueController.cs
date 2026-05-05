using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Anemora.TimeManagement
{
    public sealed class NiroMonologueController : MonoBehaviour
    {
        [SerializeField] private TimeFramePortalController portalController;
        [SerializeField] private MonoBehaviour dialogueDisplay;
        [SerializeField] private ScriptableObject introDialogue;
        [SerializeField] private ScriptableObject pastPortalDialogue;
        [SerializeField] private bool showIntroOnStart = true;
        [SerializeField] private bool showPastPortalOnce = true;

        private static readonly string[] DialogueDisplayTypeNames =
        {
            "Anemora.Dialogue.DialogueDisplay, Anemora.Dialogue",
            "Anemora.Dialogue.DialogueDisplay, Assembly-CSharp"
        };

        private bool hasShownPastPortal;

        private void Awake()
        {
            if (portalController == null)
            {
                portalController = GetComponent<TimeFramePortalController>();
            }
        }

        private void OnEnable()
        {
            ResolvePortalController();
            if (portalController != null)
            {
                portalController.CrossingCompleted += HandleCrossingCompleted;
            }
        }

        private void Start()
        {
            if (!Application.isBatchMode && showIntroOnStart && introDialogue != null)
            {
                StartCoroutine(ShowWhenDialogueDisplayIsReady(introDialogue));
            }
        }

        private void OnDisable()
        {
            if (portalController != null)
            {
                portalController.CrossingCompleted -= HandleCrossingCompleted;
            }
        }

        private void HandleCrossingCompleted(SceneSide targetSide)
        {
            if (Application.isBatchMode)
            {
                return;
            }

            if (targetSide != SceneSide.Past || pastPortalDialogue == null)
            {
                return;
            }

            if (showPastPortalOnce && hasShownPastPortal)
            {
                return;
            }

            hasShownPastPortal = true;
            StartCoroutine(ShowWhenDialogueDisplayIsReady(pastPortalDialogue));
        }

        private IEnumerator ShowWhenDialogueDisplayIsReady(ScriptableObject dialogueAsset)
        {
            yield return null;

            var display = ResolveDialogueDisplay();
            if (display == null)
            {
                Debug.LogWarning("NiroMonologueController could not find a DialogueDisplay.", this);
                yield break;
            }

            while (IsDisplayVisible(display))
            {
                yield return null;
            }

            if (!TryShow(display, dialogueAsset))
            {
                Debug.LogWarning("NiroMonologueController could not show the configured dialogue asset.", this);
            }
        }

        private void ResolvePortalController()
        {
            if (portalController == null)
            {
                portalController = FindFirstObjectByType<TimeFramePortalController>();
            }
        }

        private MonoBehaviour ResolveDialogueDisplay()
        {
            if (dialogueDisplay != null)
            {
                return dialogueDisplay;
            }

            var displayType = ResolveDialogueDisplayType();
            if (displayType == null)
            {
                return null;
            }

            var instanceProperty = displayType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            dialogueDisplay = instanceProperty?.GetValue(null) as MonoBehaviour;
            if (dialogueDisplay != null)
            {
                return dialogueDisplay;
            }

            dialogueDisplay = FindFirstObjectByType(displayType) as MonoBehaviour;
            return dialogueDisplay;
        }

        private static Type ResolveDialogueDisplayType()
        {
            foreach (var typeName in DialogueDisplayTypeNames)
            {
                var type = Type.GetType(typeName, throwOnError: false);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        private static bool IsDisplayVisible(MonoBehaviour display)
        {
            var property = display.GetType().GetProperty(
                "IsVisible",
                BindingFlags.Public | BindingFlags.Instance);
            return property != null && property.PropertyType == typeof(bool) && (bool)property.GetValue(display);
        }

        private static bool TryShow(MonoBehaviour display, ScriptableObject dialogueAsset)
        {
            if (dialogueAsset == null)
            {
                return false;
            }

            foreach (var method in display.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.Name != "Show")
                {
                    continue;
                }

                var parameters = method.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType.IsInstanceOfType(dialogueAsset))
                {
                    method.Invoke(display, new object[] { dialogueAsset });
                    return true;
                }
            }

            return false;
        }
    }
}
