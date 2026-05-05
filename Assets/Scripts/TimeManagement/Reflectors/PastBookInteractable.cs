using System;
using System.Reflection;
using Anemora.Data;
using Anemora.TimeManagement;
using UnityEngine;

namespace Anemora.TimeManagement.Reflectors
{
    public sealed class PastBookInteractable : MonoBehaviour
    {
        [SerializeField] private string actionId = "take_book_001";
        [SerializeField] private string targetObjectId = "Book_Family_Past_001";
        [SerializeField] private float interactionRange = 1.25f;
        [SerializeField] private Transform player;
        [SerializeField] private SceneSidePolarity sidePolarity;
        [SerializeField] private bool requirePastSide = true;

        public string ActionId => actionId;
        public string TargetObjectId => targetObjectId;

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.E) && !Input.GetKeyDown(KeyCode.Space))
            {
                return;
            }

            TryInteract(ResolvePlayer());
        }

        public bool TryInteract(Transform candidatePlayer)
        {
            if (!isActiveAndEnabled ||
                candidatePlayer == null ||
                IsDialogueDisplayVisible() ||
                !IsOnInteractableSide() ||
                !IsWithinRange(candidatePlayer))
            {
                return false;
            }

            var runtime = ActionRecordRuntime.Instance;
            if (runtime == null)
            {
                Debug.LogWarning("PastBookInteractable could not find ActionRecordRuntime.", this);
                return false;
            }

            runtime.AddEntry(new ActionRecordEntry
            {
                actionId = actionId,
                targetObjectId = targetObjectId,
                type = ActionType.Take,
                gameTimeTicks = DateTime.UtcNow.Ticks,
                reflected = false
            });

            gameObject.SetActive(false);
            return true;
        }

        private Transform ResolvePlayer()
        {
            if (player != null)
            {
                return player;
            }

            var playerObject = GameObject.FindWithTag("Player");
            player = playerObject != null ? playerObject.transform : null;
            return player;
        }

        private bool IsOnInteractableSide()
        {
            if (!requirePastSide)
            {
                return true;
            }

            if (sidePolarity == null)
            {
                sidePolarity = FindFirstObjectByType<SceneSidePolarity>();
            }

            return sidePolarity == null || sidePolarity.CurrentSide == SceneSide.Past;
        }

        private bool IsWithinRange(Transform candidatePlayer)
        {
            return Vector3.Distance(candidatePlayer.position, transform.position) <= interactionRange;
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
