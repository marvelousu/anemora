using Anemora.Game.Dialogue;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Anemora.Dialogue
{
    public sealed class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private TMP_Text speakerLabel;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private TMP_Text advanceIndicator;
        [SerializeField] private MonoBehaviour playerController;
        [SerializeField] private string speakerTableName = "Anemora_Strings";
        [SerializeField] private KeyCode primaryAdvanceKey = KeyCode.Space;
        [SerializeField] private KeyCode secondaryAdvanceKey = KeyCode.E;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip advanceClip;
        [SerializeField] private AudioClip closeClip;
        [SerializeField, Range(0f, 1f)] private float dialogueAudioVolume = 0.7f;

        private DialogueVariantSO currentVariant;
        private int currentTurnIndex;
        private int ignoreInputFrame = -1;
        private bool hasShownDialogue;

        public static DialogueDisplay Instance { get; private set; }
        public bool IsVisible => panelRoot != null && panelRoot.activeSelf;
        public int CurrentLineIndex => currentTurnIndex;
        public string CurrentSpeaker => speakerLabel != null ? speakerLabel.text : string.Empty;
        public string CurrentText => dialogueText != null ? dialogueText.text : string.Empty;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            ResolveReferences();
            Close();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Update()
        {
            if (!IsVisible || Time.frameCount == ignoreInputFrame)
            {
                return;
            }

            if (Input.GetKeyDown(primaryAdvanceKey) ||
                Input.GetKeyDown(secondaryAdvanceKey) ||
                Input.GetKeyDown(KeyCode.Return))
            {
                AdvanceLine();
            }
        }

        public void Show(DialogueAsset asset)
        {
            ResolveReferences();
            currentVariant = ResolveFirstVariantWithTurns(asset);
            currentTurnIndex = 0;
            ignoreInputFrame = Time.frameCount;

            if (panelRoot != null)
            {
                panelRoot.SetActive(true);
            }

            SetPlayerFrozen(true);
            hasShownDialogue = true;
            RenderCurrentTurn();
        }

        public void AdvanceLine()
        {
            if (!IsVisible || currentVariant == null)
            {
                return;
            }

            currentTurnIndex++;
            if (currentTurnIndex >= currentVariant.turns.Count)
            {
                Close();
                return;
            }

            PlayDialogueAudio(advanceClip);
            RenderCurrentTurn();
        }

        public void Close()
        {
            var shouldPlayCloseAudio = hasShownDialogue && IsVisible;

            if (panelRoot != null)
            {
                panelRoot.SetActive(false);
            }

            if (speakerLabel != null)
            {
                speakerLabel.text = string.Empty;
            }

            if (dialogueText != null)
            {
                dialogueText.text = string.Empty;
            }

            currentVariant = null;
            currentTurnIndex = 0;
            hasShownDialogue = false;
            SetPlayerFrozen(false);

            if (shouldPlayCloseAudio)
            {
                PlayDialogueAudio(closeClip);
            }
        }

        private void ResolveReferences()
        {
            if (panelRoot == null)
            {
                panelRoot = gameObject;
            }

            if (playerController == null)
            {
                var playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    playerController = FindPrototypePlayerController(playerObject);
                }
            }

            if (advanceIndicator != null && string.IsNullOrEmpty(advanceIndicator.text))
            {
                advanceIndicator.text = ">";
            }

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        private void RenderCurrentTurn()
        {
            if (currentVariant == null ||
                currentTurnIndex < 0 ||
                currentTurnIndex >= currentVariant.turns.Count)
            {
                Close();
                return;
            }

            var turn = currentVariant.turns[currentTurnIndex];
            if (speakerLabel != null)
            {
                speakerLabel.text = ResolveSpeaker(turn.speakerId);
            }

            if (dialogueText != null)
            {
                dialogueText.text = turn.GetLocalizedTextOrFallback(GetLocalizedStringFallback(turn.text));
            }
        }

        private string ResolveSpeaker(string speakerId)
        {
            if (string.IsNullOrEmpty(speakerId))
            {
                return string.Empty;
            }

            var localizedSpeaker = new LocalizedString(speakerTableName, speakerId);
            return ResolveLocalizedStringOrFallback(localizedSpeaker, speakerId);
        }

        private void SetPlayerFrozen(bool frozen)
        {
            if (playerController != null)
            {
                var method = playerController.GetType().GetMethod(
                    "SetMovementFrozen",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new[] { typeof(bool) },
                    null);
                method?.Invoke(playerController, new object[] { frozen });
            }
        }

        private void PlayDialogueAudio(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            ResolveReferences();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(clip, dialogueAudioVolume);
            }
        }

        private static MonoBehaviour FindPrototypePlayerController(GameObject playerObject)
        {
            var behaviours = playerObject.GetComponents<MonoBehaviour>();
            foreach (var behaviour in behaviours)
            {
                if (behaviour != null && behaviour.GetType().FullName == "Anemora.Player.PrototypePlayerController")
                {
                    return behaviour;
                }
            }

            return null;
        }

        private static DialogueVariantSO ResolveFirstVariantWithTurns(DialogueAsset asset)
        {
            if (asset == null || asset.variants == null)
            {
                return null;
            }

            foreach (var variant in asset.variants)
            {
                if (variant != null && variant.turns != null && variant.turns.Count > 0)
                {
                    return variant;
                }
            }

            return null;
        }

        private static string GetLocalizedStringFallback(LocalizedString localizedString)
        {
            if (localizedString == null || localizedString.IsEmpty)
            {
                return string.Empty;
            }

            var key = localizedString.TableEntryReference.Key;
            return string.IsNullOrEmpty(key) ? localizedString.TableEntryReference.KeyId.ToString() : key;
        }

        private static string ResolveLocalizedStringOrFallback(LocalizedString localizedString, string fallback)
        {
            if (localizedString == null || localizedString.IsEmpty)
            {
                return fallback ?? string.Empty;
            }

            if (!LocalizationSettings.HasSettings || Application.isBatchMode)
            {
                return fallback ?? string.Empty;
            }

            try
            {
                localizedString.WaitForCompletion = true;
                var resolved = localizedString.GetLocalizedString();
                return string.IsNullOrEmpty(resolved) ? fallback ?? string.Empty : resolved;
            }
            catch
            {
                return fallback ?? string.Empty;
            }
        }
    }
}
