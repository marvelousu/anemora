using Anemora.Game.Dialogue;
using UnityEngine;

namespace Anemora.Dialogue
{
    public sealed class NpcInteractable : MonoBehaviour
    {
        public DialogueAsset dialogueAsset;
        public float interactionRange = 1.5f;
        public KeyCode interactKey = KeyCode.E;

        private Transform player;

        public DialogueAsset DialogueAsset => dialogueAsset;

        private void Update()
        {
            if (Input.GetKeyDown(interactKey))
            {
                TryInteract();
            }
        }

        public bool TryInteract()
        {
            if (dialogueAsset == null || DialogueDisplay.Instance == null)
            {
                return false;
            }

            if (DialogueDisplay.Instance.IsVisible)
            {
                return false;
            }

            var playerTransform = ResolvePlayer();
            if (playerTransform == null)
            {
                return false;
            }

            var delta = playerTransform.position - transform.position;
            delta.y = 0f;
            if (delta.sqrMagnitude > interactionRange * interactionRange)
            {
                return false;
            }

            DialogueDisplay.Instance.Show(dialogueAsset);
            return true;
        }

        public bool IsPlayerInRange()
        {
            var playerTransform = ResolvePlayer();
            if (playerTransform == null)
            {
                return false;
            }

            var delta = playerTransform.position - transform.position;
            delta.y = 0f;
            return delta.sqrMagnitude <= interactionRange * interactionRange;
        }

        private Transform ResolvePlayer()
        {
            if (player != null)
            {
                return player;
            }

            var playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }

            return player;
        }
    }
}
